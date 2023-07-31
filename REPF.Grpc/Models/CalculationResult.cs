using Microsoft.ML.Data;

namespace REPF.Grpc.Models
{
    public class CalculationResult
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}
