using FluentAssertions;
using Microsoft.ML;
using REPF.PriceForecaster;
using System.Globalization;

namespace REPF.Tests
{
    public class ForecastServiceTest
    {
        ForecastRequest request = new ForecastRequest()
        {
            Location = "Novi Beograd",
            RoomCount = 2
        };

        Dictionary<string, string[]> requestParams = new Dictionary<string, string[]>()
        {
            {"Barajevo",new string[]{"1","1.5","2","2.5","3","3.5","4"} },
            {"Čukarica",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } },   //0.5
            {"Grocka",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4"} },                //0.5
            {"Lazarevac",new string[]{"2.5","3","4"} },
            {"Mladenovac",new string[]{"1","1.5","2","2.5","3","4"} },
            {"Novi Beograd",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } }, //0.5,  1,   3.5,    5
            {"Obrenovac",new string[]{"2","2.5","3","3.5"} },
            {"Palilula",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5", "5" } },  //0.5,   1,  1.5,  5
            {"Rakovica",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } },  //0.5,   1
            {"Savski venac",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } },   //0.5,  1,    1.5,    2,  2.5,  3,  3.5
            {"Sopot",new string[]{ } },
            {"Stari grad",new string[]{"0.5","1","2","2.5","3","3.5","4","4.5","5" } },     //0.5,  1,  1.5,  2,  2.5,  3,  4.5(221)
            {"Surčin",new string[] {"1.5","2","2.5","3" } },
            {"Voždovac",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } },   //"0.5",    1
            {"Vračar",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } },   //0.5,    1,  1.5(151), 2,  4(201)
            {"Zemun",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } }, //0.5,   1(121), 2.5(151)
            {"Zvezdara",new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5" } }, //0.5,    1(120.5)
        };

        PriceForecasterService.Services.ForecastService sut = new PriceForecasterService.Services.ForecastService();

        [Fact]
        public void EvaluateModelForEveryRequestBarajevo_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Barajevo";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);
            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestCukarica_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Čukarica";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestGrocka_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Grocka";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestLazarevac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Lazarevac";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestMladenovac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Mladenovac";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestNoviBeograd_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Novi Beograd";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestObrenovac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Obrenovac";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }

        [Fact]
        public void EvaluateModelForEveryRequestPalilula_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Palilula";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestRakovica_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Rakovica";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestSavskiVenac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Savski venac";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(450);
                metrics.Item2.Should().BeLessThanOrEqualTo(300);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestSopot_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Sopot";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

metrics.Item1.Should().BeLessThanOrEqualTo(250);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestStariGrad_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Stari grad";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

metrics.Item1.Should().BeLessThanOrEqualTo(250);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestSurcin_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Surčin";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

metrics.Item1.Should().BeLessThanOrEqualTo(250);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestVozdovac_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Voždovac";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

metrics.Item1.Should().BeLessThanOrEqualTo(250);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestVracar_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Vračar";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

metrics.Item1.Should().BeLessThanOrEqualTo(250);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void EvaluateModelForEveryRequestZemun_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Zemun";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }

        [Fact]
        public void EvaluateModelForEveryRequestZvezdara_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();

            var location = "Zvezdara";
            var roomCountCollection = requestParams[location];


            foreach (var roomCount in roomCountCollection)
            {
                var realEstates = sut.LoadData(mlContext, location, double.Parse(roomCount, CultureInfo.InvariantCulture)).ToList();
                var data = mlContext.Data.LoadFromEnumerable(realEstates);

                var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
                var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


                var model = sut.Train(mlContext, trainData);

                var metrics = sut.Evaluate(model, mlContext, testData);

                metrics.Item1.Should().BeLessThanOrEqualTo(350);
                metrics.Item2.Should().BeLessThanOrEqualTo(200);

            }
        }


        [Fact]
        public void LoadData_ShouldReturnData()
        {
            MLContext mlContext = new MLContext(seed : 0);
           var data = sut.LoadData(mlContext, request.Location, request.RoomCount);


            data.Count().Should().BeGreaterThan(0);
            data.Select(x => x.Location).Distinct().Count().Should().Be(1);
            data.Select(x => x.Location).Distinct().First().Should().Be("Novi Beograd");
            data.Select(x => x.RoomCount).Distinct().First().Should().Be(2);
        }

        [Fact]
        public void LoadData_ShouldNotReturnData()
        {
            MLContext mlContext = new MLContext(seed: 0);

            var data = sut.LoadData(mlContext, "Smederevo", 2);

            data.Count().Should().Be(0);
        }


        [Fact]
        public void EvaluateModel_ShouldHaveAcceptableMetrics()
        {
            var mlContext = new MLContext();
            var realEstates = sut.LoadData(mlContext, request.Location, request.RoomCount);
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
            var realEstates = sut.LoadData(mlContext, request.Location, request.RoomCount);
            var data = mlContext.Data.LoadFromEnumerable(realEstates);

            var trainData = mlContext.Data.FilterRowsByColumn(data, "Month", upperBound: (DateTime.Now.Year - 2018) * 100);
            var testData = mlContext.Data.FilterRowsByColumn(data, "Month", lowerBound: (DateTime.Now.Year - 2018) * 100);


            var model = sut.Train(mlContext, trainData);
            model.Should().NotBeNull();
        }

    }
}
