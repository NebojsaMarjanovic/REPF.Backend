using Grpc.Core;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Transforms;
using REPF.Grpc.Models;
using System.Data.SqlClient;

namespace REPF.Grpc.Services
{
    public class CalculationService:CalculateService.CalculateServiceBase
    {
        private readonly string dataPath = "C:\\Users\\nebojsa.marjanovic\\source\\repos\\REPF.Backend\\REPF.Grpc\\Files\\fetch_from_03.05.2023.csv";

        public override Task<CalculationResponse> Calculate(CalculationRequest request, ServerCallContext context)
        {

            var mlContext = new MLContext(seed: 0);
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<CalculationParameters>();

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=REPF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlCommand = "SELECT Id, Municipality, Neighborhood, Price, SquareFootage, Rooms, Floor, IsLastFloor, HeatingType, HasElevator, IsRegistered FROM RealEstates";

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);


            //var dataView = mlContext.Data.LoadFromTextFile<CalculationParameters>(dataPath, separatorChar: '|', hasHeader: true);

            var dataView = loader.Load(dbSource);

            var realEstates = mlContext.Data.CreateEnumerable<CalculationParameters>(dataView, false).Where(x=>x.Municipality==request.Municipality).ToList();

            dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);

            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);
            //dataView = mlContext.Data.FilterRowsByMissingValues(dataView, "SquareFootage","Rooms");


            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;



            var model = Train(mlContext, trainData);
            var metrics = Evaluate(model, mlContext, testData);
            var singlePrediction = TestSinglePrediction(model,mlContext, request);
            singlePrediction.Price = Math.Round(singlePrediction.Price/100d,0)*100;


            return Task.FromResult(singlePrediction);
        }


        public ITransformer? Train(MLContext mlContext, IDataView trainData)
        {
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName:"Label", inputColumnName:"Price") 
                                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(new[] 
                                    { new InputOutputColumnPair(@"Municipality", @"Municipality"), 
                                        new InputOutputColumnPair(@"IsLastFloor", @"IsLastFloor"), 
                                        new InputOutputColumnPair(@"HeatingType", @"HeatingType"), 
                                        new InputOutputColumnPair(@"HasElevator", @"HasElevator"), 
                                        new InputOutputColumnPair(@"IsRegistered", @"IsRegistered") }, 
                                        outputKind: OneHotEncodingEstimator.OutputKind.Indicator))
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new[] { new InputOutputColumnPair(@"SquareFootage", @"SquareFootage"), new InputOutputColumnPair(@"Rooms", @"Rooms"), new InputOutputColumnPair(@"Floor", @"Floor") }))
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: @"Neighborhood", outputColumnName: @"Neighborhood"))
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"Municipality", @"IsLastFloor", @"HeatingType", @"HasElevator", @"IsRegistered", @"SquareFootage", @"Rooms", @"Floor", @"Neighborhood" }))
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))
                                        //.Append(mlContext.Regression.Trainers.FastTreeTweedie(new FastTreeTweedieTrainer.Options() { NumberOfLeaves = 10, MinimumExampleCountPerLeaf = 2, NumberOfTrees = 705, MaximumBinCountPerFeature = 1022, FeatureFraction = 0.99999999, LearningRate = 0.30498982295545 }));
                                     .Append(mlContext.Regression.Trainers.FastTreeTweedie(new FastTreeTweedieTrainer.Options() { NumberOfLeaves = 11, MinimumExampleCountPerLeaf = 4, NumberOfTrees = 79, MaximumBinCountPerFeature = 300, FeatureFraction = 0.313806953660382, LearningRate = 0.423485295561442, LabelColumnName = @"Price", FeatureColumnName = @"Features" }));

           

            Console.WriteLine("=============== Create and Train the Model ===============");

            var trainedModel = pipeline.Fit(trainData);

            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();

            return trainedModel;
        }

        public RegressionMetrics Evaluate(ITransformer model, MLContext mlContext, IDataView testData)
        {
           

            var predictions = model.Transform(testData);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
            
            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");

            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");

            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");


            var mean = testData.GetColumn<float>("Price").Average();

            Console.WriteLine($"*       Root Mean Squared Error (%):      {metrics.RootMeanSquaredError / mean * 100:#.##}%");


            Console.WriteLine($"*       Mean Absolute Error:      {metrics.MeanAbsoluteError:#.##}");

            Console.WriteLine($"*       Mean Squared Error:      {metrics.MeanSquaredError:#.##}");


            Console.WriteLine($"*************************************************");

            return metrics;

        }



        public CalculationResponse TestSinglePrediction(ITransformer model, MLContext mlContext, CalculationRequest realEstate)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<CalculationRequest, CalculationResult>(model);

            var prediction = predictionFunction.Predict(realEstate);

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted price: {prediction.Price:0.####}, actual price: 180000");
            Console.WriteLine($"**********************************************************************");

            return new CalculationResponse()
            {
                Price = prediction.Price
            };
        }

    }
}
