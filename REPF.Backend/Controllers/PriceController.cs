using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using REPF.Backend.Models.Input;
using REPF.Backend.Models.Output;
using REPF.Backend.Utilities;
using REPF.Grpc;

namespace REPF.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly Forecaster.ForecasterClient _client;

        public PriceController(Forecaster.ForecasterClient client)
        {
            //grpc client initialization
            _client = client;
        }

        [HttpPost]
        public async Task<IActionResult> GetForecast(ForecastRequestParameters forecastRequestParameters, CancellationToken cancellationToken)
        {
            ForecastRequest request = new ForecastRequest()
            {
                PlaceTitle = forecastRequestParameters.Location.ToString(),
                RoomCount = forecastRequestParameters.RoomCount
            };


            var result =  await _client.ForecastAsync(request);

            return Ok(result);

        }
    }
}
