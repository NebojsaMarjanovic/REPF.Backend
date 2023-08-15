using REPF.Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace REPF.Backend.Models
{
    public class CalculationRequestParameters
    {
        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Neighborhood { get; set; }

        [Required]
        public int Quadrature { get; set; }

        [Required]
        public double RoomCount { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required]
        public bool IsLastFloor { get; set; }

        [Required]
        public string HeatingType { get; set; }

        [Required]
        public bool HasElevator { get; set; }

        [Required]
        public bool RegisteredStatus { get; set; }

    }
}
