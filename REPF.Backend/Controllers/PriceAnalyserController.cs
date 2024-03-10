using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using REPF.Backend.Enumerations;
using REPF.Backend.Models;
using REPF.PriceCalculator;
using REPF.PriceForecaster;
using System.Globalization;

namespace REPF.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceAnalyserController : ControllerBase
    {
        private readonly CalculateService.CalculateServiceClient _calculatorClient;
        private readonly ForecastService.ForecastServiceClient _forecasterClient;
        private readonly IMemoryCache _cache;


        public PriceAnalyserController(CalculateService.CalculateServiceClient calculatorClient, 
                                     ForecastService.ForecastServiceClient forecasterClient,
                                     IMemoryCache cache)
        {
            _calculatorClient = calculatorClient;
            _forecasterClient = forecasterClient;
            _cache = cache;
        }


    [HttpPost("calculate")]
        public async Task<IActionResult> GetCalculation(CalculationRequestParameters calculationRequestParameters, CancellationToken cancellationToken)
        {
            CalculationRequest request = new CalculationRequest()
            {
                Municipality = calculationRequestParameters.Municipality,
                Neighborhood = calculationRequestParameters.Neighborhood,
                Rooms = (float)calculationRequestParameters.RoomCount,
                HasElevator = calculationRequestParameters.HasElevator,
                HeatingType = calculationRequestParameters.HeatingType,
                SquareFootage = calculationRequestParameters.Quadrature,
                IsLastFloor = calculationRequestParameters.IsLastFloor,
                Floor = calculationRequestParameters.Floor,
                IsRegistered = calculationRequestParameters.RegisteredStatus,
            };

            if (!_cache.TryGetValue(request, out CalculationResponse? result))
            {
                result = await _calculatorClient.CalculateAsync(request, cancellationToken: cancellationToken);
              
                _cache.Set(request, result,TimeSpan.FromDays(1));
            }

            return Ok(result!.Price);

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

            if (!_cache.TryGetValue(request, out ForecastResponse result))
            {
                result = await _forecasterClient.ForecastAsync(request, cancellationToken: cancellationToken);

                _cache.Set(request, result, TimeSpan.FromDays(1));
            }

            return Ok(result);

        }
    }
}
