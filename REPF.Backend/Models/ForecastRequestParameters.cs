using REPF.Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace REPF.Backend.Models
{
    public class ForecastRequestParameters
    {
        [Required]
        public Location Location { get; set; }

        [Required]
        public double RoomCount { get; set; }

        //dodati: redactedFloor (sprat), lastFloor (true/false), registered, furnished
    }
}
