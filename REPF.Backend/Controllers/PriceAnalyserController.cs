using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using REPF.Backend.Enumerations;
using REPF.Backend.Models;
using REPF.Grpc;
using REPF.Grpc.Protos;
using System.Globalization;

namespace REPF.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceAnalyserController : ControllerBase
    {
        private readonly CalculateService.CalculateServiceClient _calculatorClient;
        private readonly ForecastService.ForecastServiceClient _forecasterClient;


        public PriceAnalyserController(CalculateService.CalculateServiceClient calculatorClient, 
                                     ForecastService.ForecastServiceClient forecasterClient)
        {
            _calculatorClient = calculatorClient;
            _forecasterClient = forecasterClient;
        }


    [HttpPost("calculate")]
        public async Task<IActionResult> GetCalculation(CalculationRequestParameters calculationRequestParameters, CancellationToken cancellationToken)
        {
            CalculationRequest request = new CalculationRequest()
            {
                Municipality = calculationRequestParameters.Municipality,
                Neighborhood = calculationRequestParameters.Neighborhood,
                Rooms = (float)calculationRequestParameters.RoomCount,
                HasElevator=calculationRequestParameters.HasElevator,
                HeatingType = calculationRequestParameters.HeatingType,
                SquareFootage=calculationRequestParameters.Quadrature,
                IsLastFloor = calculationRequestParameters.IsLastFloor,
                Floor =calculationRequestParameters.Floor,
                IsRegistered = calculationRequestParameters.RegisteredStatus,
            };


            var result = await _calculatorClient.CalculateAsync(request, cancellationToken: cancellationToken);

            return Ok(result.Price);

        }

        [HttpPost("forecast")]
        public async Task<IActionResult> GetForecast(ForecastRequestParameters forecastRequestParameters, CancellationToken cancellationToken)
        {
            if (!LocationMap.locations[forecastRequestParameters.Location]
                .Item2
                .Contains(forecastRequestParameters.RoomCount.ToString(CultureInfo.InvariantCulture)))
            {
                return NotFound("Ne postoje podaci o nekretninama sa parametrima koje ste uneli.");
            }

            ForecastRequest request = new ForecastRequest()
            {
                Location = LocationMap.locations[forecastRequestParameters.Location].Item1,
                RoomCount = forecastRequestParameters.RoomCount
            };


            var result = await _forecasterClient.ForecastAsync(request, cancellationToken: cancellationToken);

            return Ok(result);

        }
    }
}
