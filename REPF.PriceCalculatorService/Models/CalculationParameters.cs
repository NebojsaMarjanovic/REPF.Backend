namespace REPF.PriceCalculatorService.Models
{
    public class CalculationParameters
    {
        public int Id { get; set; }
        public string Municipality { get; set; }
        public string Neighborhood { get; set; }
        public float Price { get; set; }
        public float SquareFootage { get; set; }
        public float Rooms { get; set; }
        public float Floor { get; set; }
        public bool IsLastFloor { get; set; }
        public string HeatingType { get; set; }
        public bool HasElevator { get; set; }
        public bool IsRegistered { get; set; }
    }
}
