using Microsoft.ML.Data;

namespace REPF.Grpc.Models
{
    public class RealEstatePrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}
