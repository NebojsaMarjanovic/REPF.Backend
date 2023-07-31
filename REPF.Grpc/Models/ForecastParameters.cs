using Microsoft.ML.Data;

namespace REPF.Grpc.Models
{
    public class ForecastParameters
    {
        [LoadColumn(0)]
        public string Location { get; set; }

        [LoadColumn(1)]

        public float RoomCount { get; set; }

        [LoadColumn(2)]
        public float Month { get; set; }

        [LoadColumn(3)]
        public string? Date { get; set; }

        [LoadColumn(4)]
        public float AveragePrice { get; set; }
    }
}
