using Grpc.Core;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using REPF.Grpc.Models;
using REPF.Grpc.Protos;
using System.Data.SqlClient;
using System.Globalization;

namespace REPF.Grpc.Services
{
    public class ForecastService:REPF.Grpc.Protos.ForecastService.ForecastServiceBase
    {
        private MLContext mlContext;
        private IDataView data;
        private IDataView trainData;
        private IDataView testData;

        public override async Task<ForecastResponse> Forecast(ForecastRequest request, ServerCallContext context)
        {
            mlContext = new MLContext();


            var realEstates = LoadData(mlContext, request.Location, request.RoomCount);

            data = mlContext.Data.LoadFromEnumerable(realEstates);

            trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year-2018)*100);
            testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year-2018)*100);


            var forecaster = Train(mlContext,trainData);

            var evaluationResult = Evaluate(forecaster, mlContext, testData);

            var forecastResult = MakeForecast(forecaster, mlContext, testData);

            var response = new ForecastResponse();

            foreach (var realEstate in realEstates)
            {
                var date = DateOnly.ParseExact(realEstate.Date, "MM/dd/yyyy");
                response.HistoricalData.Add(date.ToString("MM/yyyy"), realEstate.AveragePricePerSquareMeter);
            }

            var lastDate = DateOnly.ParseExact(realEstates.Last().Date, "MM/dd/yyyy");

            for(int i=6; i<forecastResult.Forecast.Length; i++)
            {
                lastDate = lastDate.AddMonths(1);
                
                response.LowerBoundForecast.Add(lastDate.ToString("MM/yyyy"), forecastResult.LowerBoundForecast[i]);
                response.Forecast.Add(lastDate.ToString("MM/yyyy"), forecastResult.Forecast[i]);
                response.UpperBoundForecast.Add(lastDate.ToString("MM/yyyy"), forecastResult.UpperBoundForecast[i]);
            }

            return await Task.FromResult(response);
        }

        public IEnumerable<ForecastParameters> LoadData(MLContext mlContext, string location, double roomCount)
        {
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<ForecastParameters>();

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=REPF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlCommand = $"select ID, Location, RoomCount, Month, Date, AveragePricePerSquareMeter from HistoricalData";

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);

            var dataView = loader.Load(dbSource);

            var realEstates = mlContext.Data.CreateEnumerable<ForecastParameters>(dataView, false)
                .Where(x=>x.Location==location && x.RoomCount==roomCount).ToList();

            return realEstates.ToList();

        }


        public Tuple<float,double> Evaluate(ITransformer model, MLContext mlContext, IDataView testData)
        {
            IDataView predictions = model.Transform(testData);

            IEnumerable<float> actual = mlContext.Data.CreateEnumerable<ForecastParameters>(testData, true).Select(observed => observed.AveragePricePerSquareMeter);
            IEnumerable<float> forecast = mlContext.Data.CreateEnumerable<ForecastResult>(predictions, true).Select(observed => observed.Forecast[0]);

            var mean = actual.Average();
            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Err 
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); //Root Mean Squared Err

            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");

            return Tuple.Create(MAE, RMSE);
        }

        public ForecastResult MakeForecast(ITransformer model, MLContext mlContext, IDataView testData)
        { 
            var forecastEngine = model.CreateTimeSeriesEngine<ForecastParameters, ForecastResult>(mlContext);

            var forecast = forecastEngine.Predict();

                IEnumerable<string> forecastOutput =
                    mlContext.Data.CreateEnumerable<ForecastParameters>(testData, reuseRowObject: false)
                        .Take(12-DateTime.Now.Month+12)
                        .Select((ForecastParameters forecastedValue, int index) =>
                        {
                            string rentalDate = forecastedValue.Date;
                            float actualRentals = forecastedValue.AveragePricePerSquareMeter;
                            float lowerEstimate = Math.Max(0, forecast.LowerBoundForecast[index]);
                            float estimate = forecast.Forecast[index];
                            float upperEstimate = forecast.UpperBoundForecast[index];
                            return $"Date: {rentalDate}\n" +
                            $"Actual Rentals: {actualRentals}\n" +
                            $"Lower Estimate: {lowerEstimate}\n" +
                            $"Forecast: {estimate}\n" +
                            $"Upper Estimate: {upperEstimate}\n";
                        });

            //Output predictions
            Console.WriteLine("Rental Forecast");
            Console.WriteLine("---------------------");
            foreach (var prediction in forecastOutput)
            {
                Console.WriteLine(prediction);
            }

            //Console.WriteLine();
            //Console.WriteLine();

            //var date = new DateOnly(2023, 1, 1);
            //foreach (var forecasted in forecast.Forecast)
            //{
            //    Console.WriteLine($"{date}: {forecasted}");
            //    date = date.AddMonths(1);
            //}
            return forecast;

        }
   
        public ITransformer? Train(MLContext mlContext, IDataView trainData)
        {
            var pipelinePrediction = mlContext.Forecasting.ForecastBySsa(nameof(ForecastResult.Forecast), nameof(ForecastParameters.AveragePricePerSquareMeter),
               windowSize: 3, seriesLength: 6, trainSize: 48, horizon: 18,
               confidenceLowerBoundColumn: "LowerBoundForecast",
               confidenceUpperBoundColumn: "UpperBoundForecast");

            var forecaster = pipelinePrediction.Fit(trainData);

            return forecaster;
        }
    
    }
}
