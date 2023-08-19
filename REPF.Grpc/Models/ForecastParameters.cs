using Microsoft.ML.Data;

namespace REPF.Grpc.Models
{
    public class ForecastParameters
    {
        public int Id { get; set; }


        public string Location { get; set; }


        public float RoomCount { get; set; }

        public float Month { get; set; }

        public string? Date { get; set; }

        public float AveragePricePerSquareMeter { get; set; }
    }
}
