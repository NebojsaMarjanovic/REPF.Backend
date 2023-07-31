﻿using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using REPF.Backend.Enumerations;
using REPF.Backend.Models;
using REPF.Grpc;
using REPF.Grpc.Protos;

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
                PlaceTitle = calculationRequestParameters.Location.ToString(),
                RoomCount = calculationRequestParameters.RoomCount,
                Elevator=calculationRequestParameters.Elevator,
                HeatingType = HeatingType.HeatingTypeMap[calculationRequestParameters.HeatingType],
                M2=calculationRequestParameters.Quadrature,
                //FurnishedStatus=forecastRequestParameters.FurnishedStatus,
                //IsLastFloor=forecastRequestParameters.IsLastFloor,
                RedactedFloor=calculationRequestParameters.Floor,
                //RegisteredStatus= forecastRequestParameters.RegisteredStatus
            };


            var result = await _calculatorClient.CalculateAsync(request, cancellationToken: cancellationToken);

            return Ok(result.Price);

        }

        [HttpPost("forecast")]
        public async Task<IActionResult> GetForecast(ForecastRequestParameters forecastRequestParameters, CancellationToken cancellationToken)
        {
            ForecastRequest request = new ForecastRequest()
            {
                Location = forecastRequestParameters.Location,
                RoomCount = forecastRequestParameters.RoomCount
            };


            var result = await _forecasterClient.ForecastAsync(request, cancellationToken: cancellationToken);

            return Ok(result);

        }
    }
}