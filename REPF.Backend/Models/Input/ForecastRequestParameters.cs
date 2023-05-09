using REPF.Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace REPF.Backend.Models.Input
{
    public class ForecastRequestParameters
    {
        [Required]
        public string Location { get; set; }

        [Required]
        public int Quadrature { get; set; }

        [Required]
        public double RoomCount { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required]
        public bool IsLastFloor { get; set; }

        [Required]
        public string RegisteredStatus { get; set; }

        [Required]
        public string FurnishedStatus { get; set; }

        [Required]
        public string HeatingType { get; set; }

        [Required]
        public int Elevator { get; set; }
    }
}
