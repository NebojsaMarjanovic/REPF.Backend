using Microsoft.ML.Data;

namespace REPF.PriceCalculatorService.Models
{
    public class CalculationResult
    {
        [ColumnName("Score")]
        public float Price { get; set; }

    }
}
