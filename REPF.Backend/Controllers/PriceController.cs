using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using REPF.Backend.Enumerations;
using REPF.Backend.Models;
using REPF.Grpc;

namespace REPF.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly Calculator.CalculatorClient _client;

        public PriceController(Calculator.CalculatorClient client)
        {
            _client = client;
        }


    [HttpPost]
        public async Task<IActionResult> GetCalculation(CalculationRequestParameters forecastRequestParameters, CancellationToken cancellationToken)
        {
            CalculationRequest request = new CalculationRequest()
            {
                PlaceTitle = forecastRequestParameters.Location.ToString(),
                RoomCount = forecastRequestParameters.RoomCount,
                Elevator=forecastRequestParameters.Elevator,
                HeatingType = HeatingType.HeatingTypeMap[forecastRequestParameters.HeatingType],
                M2=forecastRequestParameters.Quadrature,
                FurnishedStatus=forecastRequestParameters.FurnishedStatus,
                IsLastFloor=forecastRequestParameters.IsLastFloor,
                RedactedFloor=forecastRequestParameters.Floor,
                RegisteredStatus= forecastRequestParameters.RegisteredStatus
            };


            var result =  await _client.CalculateAsync(request, cancellationToken: cancellationToken);

            return Ok(result);

        }
    }
}
