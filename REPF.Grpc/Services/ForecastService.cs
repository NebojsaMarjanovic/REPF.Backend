using Grpc.Core;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using REPF.Grpc.Models;
using static Microsoft.ML.DataOperationsCatalog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace REPF.Grpc.Services
{
    public class ForecastService:Forecaster.ForecasterBase
    {
        private readonly PredictionEngine<ForecastRequest, ForecastResponse> _predictionEngine;
        private readonly string dataPath = "C:\\Users\\nebojsa.marjanovic\\source\\repos\\REPF.Backend\\REPF.Grpc\\MLModel\\rakovica-fetch_from_24.04.2023.csv";
        private MLContext mlContext;
        private IDataView dataView;
        private IDataView trainData;
        private IDataView testData;


        public ForecastService()
        {
            mlContext = new MLContext(seed: 0);
            dataView = mlContext.Data.LoadFromTextFile<RealEstate>(dataPath, separatorChar: '|', hasHeader: true);

            //=====proveriti da li se menja sa SrednjomVrednoscu ili se izbacuje!!!
            //var replacementEstimator = mlContext.Transforms.ReplaceMissingValues("Price", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            //ITransformer replacementTransformer = replacementEstimator.Fit(dataView);

            //dataView = replacementTransformer.Transform(dataView);

            //=====ovime se dobijaju bolje metrike
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 1000);
            //dataView = mlContext.Data.FilterRowsByMissingValues(dataView, "Quadrature", "Elevator", "RoomCount");

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            trainData = split.TrainSet;
            testData = split.TestSet;
            //var dataView = context.Data.LoadFromTextFile<RealEstate>("C:\\Users\\nebojsa.marjanovic\\source\\repos\\REPF.Backend\\REPF.Grpc\\MLModel\\rakovica-fetch_from_24.04.2023.csv",
            //    separatorChar: '|',hasHeader:true);

            //var preview = dataView.Preview();


            //_predictionEngine = context.Model.CreatePredictionEngine<ForecastRequest, ForecastResponse>(model, inputSchema: inputSchema);
        }

        public override Task<ForecastResponse> Forecast(ForecastRequest request, ServerCallContext context)
        {
            var model = Train(mlContext);
            Evaluate(mlContext, model);
            var singlePrediction = TestSinglePrediction(mlContext, model);


            return Task.FromResult(singlePrediction);
        }

        private ITransformer Train(MLContext mLContext)
        {

            //linearna regresija koristi samo numericke tipove
            var pipeline = mLContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Price")
                .Append(mLContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "HeatingTypeEncoded", inputColumnName: "HeatingType"))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "LocationEncoded", inputColumnName: "Location"))
                .Append(mLContext.Transforms.DropColumns("CreatedAt", "RedactedFloor")
                .Append(mLContext.Transforms.Concatenate("Features", "Quadrature", "HeatingTypeEncoded", "Elevator", "RoomCount", "LocationEncoded"))
                .Append(mLContext.Regression.Trainers.FastTree()));

            Console.WriteLine("=============== Create and Train the Model ===============");

            var model = pipeline.Fit(trainData);

            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();

            return model;
        }

        private void Evaluate(MLContext mLContext, ITransformer model)
        {
           

            var predictions = model.Transform(testData);
            var metrics = mLContext.Regression.Evaluate(predictions, "Label", "Score");

            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");

            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");

            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
            
            Console.WriteLine($"*************************************************");

        }


        private ForecastResponse TestSinglePrediction(MLContext mlContext, ITransformer model)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<RealEstate, RealEstatePrediction>(model);

            //90|district|1|180000|03/24/2023 21:27:09|5|5|Rakovica - Centar
            //38|district|2|82000|04/14/2023 20:59:47|1.5|0|Kanarevo Brdo
            //54|district|1|120000|12/29/2022 15:27:50|1.5|1|Vidikovac




            var realEstateSample = new RealEstate()
            {
                Quadrature = 54,
                CreatedAt = DateTime.Now,
                Elevator = 1,
                HeatingType = "district",
                Location = "Vidikovac",
                Price = 0,
                RedactedFloor = 1,
                RoomCount = 1.5f
            };

            var prediction = predictionFunction.Predict(realEstateSample);

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
