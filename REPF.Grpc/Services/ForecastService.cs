using Grpc.Core;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using REPF.Grpc.Models;
using REPF.Grpc.Protos;
using System.Globalization;

namespace REPF.Grpc.Services
{
    public class ForecastService:REPF.Grpc.Protos.ForecastService.ForecastServiceBase
    {
        public override async Task<ForecastResponse> Forecast(ForecastRequest request, ServerCallContext context)
        {
            string dataPath = "C:\\Users\\nebojsa.marjanovic\\source\\repos\\REPF.Backend\\REPF.Grpc\\Files\\historical_data.csv";

            var realEstates = File.ReadAllLines(dataPath)
                                .Skip(1)
                                .Select(x => x.Split('|'))
                                .Select(x => new ForecastParameters()
                                {
                                    Location = x[0],
                                    RoomCount = float.Parse(x[1], CultureInfo.InvariantCulture),
                                    Month = float.Parse(x[2], CultureInfo.InvariantCulture),
                                    Date = x[3],
                                    AveragePrice = float.Parse(x[4], CultureInfo.InvariantCulture),

                                });

            realEstates = realEstates.Where(x => x.Location == request.Location && x.RoomCount == request.RoomCount).ToList();

            var mlContext = new MLContext();

            var data = mlContext.Data.LoadFromEnumerable(realEstates);

            IDataView trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: 500);
            IDataView testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: 500);

            var pipelinePrediction = mlContext.Forecasting.ForecastBySsa(nameof(ForecastResult.Forecast), nameof(ForecastParameters.AveragePrice),
                windowSize: 3, seriesLength: 6, trainSize: 48, horizon: 18,
                confidenceLowerBoundColumn: "LowerBoundForecast",
                confidenceUpperBoundColumn: "UpperBoundForecast");
            var model = pipelinePrediction.Fit(trainData);

            var forecaster = pipelinePrediction.Fit(trainData);
            var forecastEngine = forecaster.CreateTimeSeriesEngine<ForecastParameters, ForecastResult>(mlContext);

            var evaluationResult = Evaluate(testData, forecaster, mlContext);

            var forecastResult = Forecast(testData, 18, forecastEngine, mlContext);

            var response = new ForecastResponse();

            foreach (var realEstate in realEstates)
            {
                response.HistoricalData.Add(realEstate.AveragePrice);
            }

            for(int i=6; i<forecastResult.Forecast.Length; i++)
            {
                response.LowerBoundForecast.Add(forecastResult.LowerBoundForecast[i]);
                response.Forecast.Add(forecastResult.Forecast[i]);
                response.UpperBoundForecast.Add(forecastResult.UpperBoundForecast[i]);
            }

            return await Task.FromResult(response);
        }


        private Tuple<float,double> Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            IDataView predictions = model.Transform(testData);

            IEnumerable<float> actual = mlContext.Data.CreateEnumerable<ForecastParameters>(testData, true).Select(observed => observed.AveragePrice);
            IEnumerable<float> forecast = mlContext.Data.CreateEnumerable<ForecastResult>(predictions, true).Select(observed => observed.Forecast[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Err 
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); //Root Mean Squared Err

            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");

            return Tuple.Create(MAE, RMSE);
        }

        private ForecastResult Forecast(IDataView testData, int horizon, TimeSeriesPredictionEngine<ForecastParameters, ForecastResult> forecaster, MLContext mlContext)
            {

                var forecast = forecaster.Predict();

                IEnumerable<string> forecastOutput =
                    mlContext.Data.CreateEnumerable<ForecastParameters>(testData, reuseRowObject: false)
                        .Take(horizon)
                        .Select((ForecastParameters forecastedValue, int index) =>
                        {
                            string rentalDate = forecastedValue.Date;
                            float actualRentals = forecastedValue.AveragePrice;
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
    }
}
