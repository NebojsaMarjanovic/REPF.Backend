using REPF.Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace REPF.Backend.Models
{
    public class ForecastRequestParameters
    {
        [Required]
        public string Location { get; set; }

        [Required]
        public double RoomCount { get; set; }

    }
}
