using Grpc.Core;
using Microsoft.ML;
using Microsoft.ML.Data;
using REPF.Grpc.Models;

namespace REPF.Grpc.Services
{
    public class ForecastService:Forecaster.ForecasterBase
    {
        private readonly PredictionEngine<ForecastRequest, ForecastResponse> _predictionEngine;

        public ForecastService()
        {

            var context = new MLContext();

            var dataView = context.Data.LoadFromTextFile<RealEstate>("C:\\Users\\nebojsa.marjanovic\\source\\repos\\REPF.Backend\\REPF.Grpc\\MLModel\\rakovica-fetch_from_24.04.2023.csv",
                separatorChar: '|',hasHeader:true);

            var preview = dataView.Preview();


            //_predictionEngine = context.Model.CreatePredictionEngine<ForecastRequest, ForecastResponse>(model, inputSchema: inputSchema);
        }

        public override async Task<ForecastResponse> Forecast(ForecastRequest request, ServerCallContext context)
        {
            var response = new ForecastResponse();

            var prediction = _predictionEngine.Predict(request);

            response.Price = prediction.Price;

            return await Task.FromResult(response);
        }

    }
}
