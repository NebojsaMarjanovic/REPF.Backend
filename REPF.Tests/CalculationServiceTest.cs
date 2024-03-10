using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using REPF.Grpc.Services;
using REPF.PriceCalculator;
using REPF.PriceCalculatorService.Configuration;
using REPF.PriceCalculatorService.Models;
using System.Data.SqlClient;

namespace REPF.Tests
{
    public class CalculationServiceTest
    {

        //CalculationRequest request = new CalculationRequest()
        //{
        //    Municipality = "Rakovica",
        //    Neighborhood = "Kanarevo Brdo",
        //    SquareFootage = 78,
        //    Rooms = 3,
        //    Floor = 4,
        //    IsLastFloor = false,
        //    IsRegistered = true,
        //    HeatingType = "Centralno",
        //    HasElevator = true,
        //    Price = 0
        //};

        //CalculationService sut = new CalculationService(IOptions<Database> database);



        //[Fact]
        //public void EvaluateModel_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<CalculationParameters>();

        //    string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=REPF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //    string sqlCommand = "SELECT Id, Municipality, Neighborhood, Price, SquareFootage, Rooms, Floor, IsLastFloor, HeatingType, HasElevator, IsRegistered FROM RealEstates";

        //    DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);

        //    var dataView = loader.Load(dbSource);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);

        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);


        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void PredictSinglePrediction_ShouldReturnCalculatedPrice()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    IDataView dataView = sut.LoadData(mlContext,request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;


        //    var model = sut.Train(mlContext, trainData);

        //    var singlePrediction = sut.MakeCalculation(model,mlContext, request);

        //    singlePrediction.Price.Should().BeGreaterThan(0);
        //}

        //[Fact] 
        //public void TrainModel_ShouldReturnTrainedModel()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    IDataView dataView = sut.LoadData(mlContext,request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);
        //    model.Should().NotBeNull();
        //}

        //[Fact]
        //public void LoadData_ShouldReturnData()
        //{
        //    MLContext mlContext = new MLContext(seed: 0);

        //    var data = mlContext.Data.CreateEnumerable<CalculationParameters>(sut.LoadData(mlContext, request),false);


        //    data.Count().Should().BeGreaterThan(0);
        //    data.Select(x => x.Municipality).Distinct().Count().Should().Be(1);
        //    data.Select(x => x.Municipality).Distinct().First().Should().Be("Rakovica");
        //}

        //[Fact]
        //public void EvaluateModelForBarajevo_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Barajevo";
        //    request.Neighborhood = "Barajevo - Centar";


        //    IDataView dataView = sut.LoadData(mlContext, request);

        //    var x = mlContext.Data.CreateEnumerable<CalculationParameters>(dataView, false).ToList();

        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForCukarica_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Čukarica";
        //    request.Neighborhood = "Čukarica - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForGrocka_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Grocka";
        //    request.Neighborhood = "Grocka - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForLazarevac_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Lazarevac";
        //    request.Neighborhood = "Lazarevac - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForMladenovac_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Mladenovac";
        //    request.Neighborhood = "Mladenovac - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForNoviBeograd_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Novi Beograd";
        //    request.Neighborhood = "Novi Beograd - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForObrenovac_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Obrenovac";
        //    request.Neighborhood = "Obrenovac - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForPalilula_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request = new CalculationRequest() { Municipality = "Palilula" };

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForRakovica_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Rakovica";
        //    request.Neighborhood = "Rakovica - Centar";


        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForSavskiVenac_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Savski venac";
        //    request.Neighborhood = "Savski venac - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForSopot_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Sopot";
        //    request.Neighborhood = "Sopot - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForStariGrad_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Stari Grad";
        //    request.Neighborhood = "Stari Grad - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForSurcin_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Surčin";
        //    request.Neighborhood = "Surčin - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForVozdovac_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Voždovac";
        //    request.Neighborhood = "Voždovac - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForVracar_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Vračar";
        //    request.Neighborhood = "Vračar - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForZemun_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Zemun";
        //    request.Neighborhood = "Zemun - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

        //[Fact]
        //public void EvaluateModelForZvezdara_ShouldHaveAcceptableMetrics()
        //{
        //    var mlContext = new MLContext(seed: 0);

        //    request.Municipality = "Zvezdara";
        //    request.Neighborhood = "Zvezdara - Centar";

        //    IDataView dataView = sut.LoadData(mlContext, request);
        //    dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);



        //    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //    var trainData = split.TrainSet;
        //    var testData = split.TestSet;

        //    var model = sut.Train(mlContext, trainData);

        //    var metrics = sut.Evaluate(model, mlContext, testData);

        //    Math.Round(metrics.RSquared,2).Should().BeGreaterThanOrEqualTo(0.7);
        //}

    }
}