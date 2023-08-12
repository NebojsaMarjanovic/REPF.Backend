using FluentAssertions;
using Microsoft.ML;
using REPF.Grpc.Protos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace REPF.Tests
{
    public class ForecastServiceTest
    {
        ForecastRequest request = new ForecastRequest()
        {
            Location = "Novi Beograd",
            RoomCount = 2
        };

        REPF.Grpc.Services.ForecastService sut = new REPF.Grpc.Services.ForecastService();

        [Fact]
        public void LoadData_ShouldReturnData()
        {
           var data = sut.LoadData(request.Location, request.RoomCount);


            data.Count().Should().BeGreaterThan(0);
            data.Select(x => x.Location).Distinct().Count().Should().Be(1);
            data.Select(x => x.Location).Distinct().First().Should().Be("Novi Beograd");
            data.Select(x => x.RoomCount).Distinct().First().Should().Be(2);
        }

        [Fact]
        public void LoadData_ShouldNotReturnData()
        {
            var data = sut.LoadData("Smederevo", 2);

            data.Count().Should().Be(0);
        }


        [Fact]
        public void EvaluateModel_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();
            var realEstates = sut.LoadData(request.Location, request.RoomCount);
            var data = mlContext.Data.LoadFromEnumerable(realEstates);

            var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
            var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


            var model = sut.Train(mlContext, trainData);
            var metrics = sut.Evaluate(model, mlContext, testData);


            metrics.Item1.Should().BeLessThanOrEqualTo(5000);
        }


        [Fact]
        public void TrainModel_ShouldReturnTrainedModel()
        {
            var mlContext = new MLContext();
            var realEstates = sut.LoadData(request.Location, request.RoomCount);
            var data = mlContext.Data.LoadFromEnumerable(realEstates);

            var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
            var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


            var model = sut.Train(mlContext, trainData);
            model.Should().NotBeNull();
        }

    }
}
