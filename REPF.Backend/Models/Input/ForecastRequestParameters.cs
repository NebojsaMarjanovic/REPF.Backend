using REPF.Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace REPF.Backend.Models.Input
{
    public class ForecastRequestParameters
    {
        [Required]
        public Location Location { get; set; }

        [Required]
        public double RoomCount { get; set; }
    }
}
