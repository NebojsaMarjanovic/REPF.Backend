namespace REPF.PriceForecasterService.Models
{
    public class ForecastResult
    {
        public float[] Forecast { get; set; }
        public float[] LowerBoundForecast { get; set; }
        public float[] UpperBoundForecast { get; set; }
    }
}
