using Grpc.Core;
using Microsoft.ML;

namespace REPF.Grpc.Services
{
    public class ForecastService:Forecaster.ForecasterBase
    {
        private readonly PredictionEngine<ForecastRequest, ForecastResponse> _predictionEngine;

        public ForecastService()
        {
            var context = new MLContext();

            var model = context.Model.Load("./MLModel/nazivfajla.zip", out DataViewSchema inputSchema);

            _predictionEngine = context.Model.CreatePredictionEngine<ForecastRequest, ForecastResponse>(model, inputSchema: inputSchema);
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
