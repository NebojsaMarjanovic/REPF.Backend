using Grpc.Core;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Transforms;
using REPF.Grpc.Models;
using static Microsoft.ML.DataOperationsCatalog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace REPF.Grpc.Services
{
    public class ForecastService:Forecaster.ForecasterBase
    {
        private readonly string dataPath = "C:\\Users\\nebojsa.marjanovic\\source\\repos\\REPF.Backend\\REPF.Grpc\\MLModel\\fetch_from_03.05.2023.csv";
        private MLContext mlContext;
        private IDataView dataView;
        private IDataView trainData;
        private IDataView testData;


        public ForecastService()
        {
            mlContext = new MLContext(seed: 0);
            dataView = mlContext.Data.LoadFromTextFile<RealEstate>(dataPath, separatorChar: '|', hasHeader: true);

            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);
            dataView = mlContext.Data.FilterRowsByMissingValues(dataView, "Quadrature", "Elevator", "RoomCount");


            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            trainData = split.TrainSet;
            testData = split.TestSet;
            
        }


        public override Task<ForecastResponse> Forecast(ForecastRequest request, ServerCallContext context)
        {

            var realEstateSample = new RealEstate()
            {
                Quadrature = request.M2,
                CreatedAt = DateTime.Now,
                Elevator = request.Elevator,
                HeatingType = request.HeatingType,
                Location = request.PlaceTitle,
                Price = 0,
                RedactedFloor = request.RedactedFloor,
                RoomCount = (float)request.RoomCount,
                FurnishedStatus = request.FurnishedStatus,
                IsLastFloor = request.IsLastFloor,
                RegisteredStatus = request.RegisteredStatus
            };


            var model = Train();
            var metrics = Evaluate(model);
            var singlePrediction = TestSinglePrediction(model, realEstateSample);


            return Task.FromResult(singlePrediction);
        }


        public ITransformer? Train()
        {
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Price")
                .Append(mlContext.Transforms.DropColumns("CreatedAt"))
                .Append(mlContext.Transforms.ReplaceMissingValues(new[] { new InputOutputColumnPair("QuadratureReplaced", "Quadrature"), new InputOutputColumnPair("ElevatorReplaced", "Elevator"), new InputOutputColumnPair("RoomCountReplaced", "RoomCount") }))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "Location", outputColumnName: "LocationEncoded"))
                .Append(mlContext.Transforms.Concatenate("Features", new[] { "LocationEncoded", "QuadratureReplaced", "RoomCountReplaced", "ElevatorReplaced" }))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.FastTreeTweedie(new FastTreeTweedieTrainer.Options() { NumberOfLeaves = 4, MinimumExampleCountPerLeaf = 2, NumberOfTrees = 705, MaximumBinCountPerFeature = 1022, FeatureFraction = 0.99999999, LearningRate = 0.30498982295545 }));



            Console.WriteLine("=============== Create and Train the Model ===============");

            var trainedModel = pipeline.Fit(trainData);

            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();

            return trainedModel;
        }

        public RegressionMetrics Evaluate(ITransformer model)
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



        public ForecastResponse TestSinglePrediction(ITransformer model, RealEstate realEstate)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<RealEstate, RealEstatePrediction>(model);

            var prediction = predictionFunction.Predict(realEstate);

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted price: {prediction.Price:0.####}, actual price: 180000");
            Console.WriteLine($"**********************************************************************");

            return new ForecastResponse()
            {
                Price = prediction.Price
            };
        }

    }
}
