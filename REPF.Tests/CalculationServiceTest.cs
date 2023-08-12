using FluentAssertions;
using Grpc.Core;
using Microsoft.ML;
using Microsoft.ML.Data;
using REPF.Grpc;
using REPF.Grpc.Models;
using REPF.Grpc.Services;
using System.Data.SqlClient;

namespace REPF.Tests
{
    public class CalculationServiceTest
    {
        private readonly string dataPath = "C:\\Users\\nebojsa.marjanovic\\source\\repos\\REPF.Backend\\REPF.Grpc\\Files\\fetch_from_03.05.2023.csv";

        CalculationRequest request = new CalculationRequest()
        {
            Municipality = "Rakovica",
            Neighborhood = "Kanarevo Brdo",
            SquareFootage = 78,
            Rooms = 3,
            Floor = 4,
            IsLastFloor = false,
            IsRegistered = true,
            HeatingType = "district",
            HasElevator = true,
            Price = 0
        };

        CalculationService sut = new CalculationService();



        [Fact]
        public void EvaluateModel_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<CalculationParameters>();

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=REPF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlCommand = "SELECT Id, Municipality, Neighborhood, Price, SquareFootage, Rooms, Floor, IsLastFloor, HeatingType, HasElevator, IsRegistered FROM RealEstates";

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);

            var dataView = loader.Load(dbSource);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void PredictSinglePrediction_ShouldReturnCalculatedPrice()
        {
            var mlContext = new MLContext(seed: 0);

            var realEstates = sut.LoadData(mlContext,request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;


            var model = sut.Train(mlContext, trainData);

            var singlePrediction = sut.TestSinglePrediction(model,mlContext, request);

            singlePrediction.Price.Should().BeGreaterThan(0);
        }

        [Fact] 
        public void TrainModel_ShouldReturnTrainedModel()
        {
            var mlContext = new MLContext(seed: 0);

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);
            model.Should().NotBeNull();
        }

        [Fact]
        public void LoadData_ShouldReturnData()
        {
            MLContext mlContext = new MLContext(seed:0);
            var data = sut.LoadData(mlContext, request);


            data.Count().Should().BeGreaterThan(0);
            data.Select(x => x.Municipality).Distinct().Count().Should().Be(1);
            data.Select(x => x.Municipality).Distinct().First().Should().Be("Rakovica");
        }

        [Fact]
        public void EvaluateModelForBarajevo_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Barajevo" };


            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForCukarica_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Čukarica" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForGrocka_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForLazarevac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Lazarevac" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForMladenovac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Mladenovac" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForNoviBeograd_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Novi Beograd" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForObrenovac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Obrenovac" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForPalilula_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Palilula" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForRakovica_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Rakovica" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForSavskiVenac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Savski venac" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForSopot_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Sopot" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForStariGrad_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Stari Grad" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForSurcin_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Surčin" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForVozdovac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Voždovac" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForVracar_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Vračar" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForZemun_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Zemun" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void EvaluateModelForZvezdara_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext(seed: 0);

            request = new CalculationRequest() { Municipality = "Zvezdara" };

            var realEstates = sut.LoadData(mlContext, request);
            IDataView dataView = mlContext.Data.LoadFromEnumerable<CalculationParameters>(realEstates);
            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.75);
        }

    }
}