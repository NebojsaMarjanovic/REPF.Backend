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
            //Municipality = "Rakovica",
            //Neighborhood = "Kanarevo Brdo",
            //SquareFootage = 78,
            //RoomCount = 3,
            //Floor = 4,
            ////IsLastFloor = false,
            //Registered = true,
            //Furnished = false,
            //HeatingType = "district",
            //Elevator = true,
            //Price = 0
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


            //var dataView = mlContext.Data.LoadFromTextFile<CalculationParameters>(dataPath, separatorChar: '|', hasHeader: true);

            var dataView = loader.Load(dbSource);

            var realEstates = mlContext.Data.CreateEnumerable<CalculationParameters>(dataView, false).Where(x => x.Municipality == request.Municipality).ToList();

            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);

            var metrics = sut.Evaluate(model, mlContext, testData);

            metrics.RSquared.Should().BeGreaterThanOrEqualTo(0.75);
        }

        [Fact]
        public void PredictSinglePrediction_ShouldReturnPredictedPrice()
        {
            var mlContext = new MLContext(seed: 0);
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<CalculationParameters>();

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=REPF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlCommand = "SELECT Id, Municipality, Neighborhood, Price, SquareFootage, Rooms, Floor, IsLastFloor, HeatingType, HasElevator, IsRegistered FROM RealEstates";

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);


            //var dataView = mlContext.Data.LoadFromTextFile<CalculationParameters>(dataPath, separatorChar: '|', hasHeader: true);

            var dataView = loader.Load(dbSource);

            var realEstates = mlContext.Data.CreateEnumerable<CalculationParameters>(dataView, false).Where(x => x.Municipality == request.Municipality).ToList();

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
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<CalculationParameters>();

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=REPF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlCommand = "SELECT Id, Municipality, Neighborhood, Price, SquareFootage, Rooms, Floor, IsLastFloor, HeatingType, HasElevator, IsRegistered FROM RealEstates";

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);


            //var dataView = mlContext.Data.LoadFromTextFile<CalculationParameters>(dataPath, separatorChar: '|', hasHeader: true);

            var dataView = loader.Load(dbSource);

            var realEstates = mlContext.Data.CreateEnumerable<CalculationParameters>(dataView, false).Where(x => x.Municipality == request.Municipality).ToList();

            dataView = mlContext.Data.FilterRowsByColumn(dataView, "Price", lowerBound: 10000);


            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var model = sut.Train(mlContext, trainData);
            model.Should().NotBeNull();
        }

    }
}