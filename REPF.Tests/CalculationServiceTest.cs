using FluentAssertions;
using Grpc.Core;
using REPF.Grpc;
using REPF.Grpc.Models;
using REPF.Grpc.Services;

namespace REPF.Tests
{
    public class CalculationServiceTest
    {
       
        RealEstate request = new RealEstate()
        {
            Location = "Kanarevo Brdo",
            Quadrature = 78,
            RoomCount = 3,
            RedactedFloor = 4,
            IsLastFloor = false,
            RegisteredStatus = "yes",
            FurnishedStatus = "no",
            HeatingType = "district",
            Elevator = 0,
            Price = 0
        };

        CalculationService sut = new CalculationService();


        [Fact]
        public void EvaluateModel_ShouldHaveAcceptableMetrics()
        {
            var model = sut.Train();
            var metrics = sut.Evaluate(model);

            metrics.RSquared.Should().BeGreaterThanOrEqualTo(0.85);
        }

        [Fact]
        public void PredictSinglePrediction_ShouldReturnPredictedPrice()
        {
            var model = sut.Train();

            var singlePrediction = sut.TestSinglePrediction(model, request);

            singlePrediction.Price.Should().BeGreaterThan(0);
        }

        [Fact] 
        public void TrainModel_ShouldReturnTrainedModel()
        {
            var model = sut.Train();
            model.Should().NotBeNull();
        }

    }
}