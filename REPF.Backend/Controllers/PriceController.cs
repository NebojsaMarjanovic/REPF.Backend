using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using REPF.Backend.Enumerations;
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

        //Request: 
        /*{
            "location": "Petlovo Brdo",
            "quadrature": 32,
            "roomCount": 1.5,
            "heatingType": "central",
            "elevator": 0
        }*/

        //Real price:
        //32|central|0|55000|04/12/2023 00:25:34|1.5|-1|Petlovo Brdo

//        {
//  "location": "Kanarevo Brdo",
//  "quadrature": 78,
//  "roomCount": 3,
//  "floor": 4,
//  "isLastFloor": false,
//  "registeredStatus": "yes",
//  "furnishedStatus": "no",
//  "heatingType": "Centralno",
//  "elevator": 0
//}




    [HttpPost]
        public async Task<IActionResult> GetForecast(ForecastRequestParameters forecastRequestParameters, CancellationToken cancellationToken)
        {
            ForecastRequest request = new ForecastRequest()
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


            var result =  await _client.ForecastAsync(request);

            return Ok(result);

        }
    }
}
