using REPF.Backend.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace REPF.Backend.Models.Input
{
    public class CalculationRequestParameters
    {
        [Required]
        public Location Location { get; set; }

        [Required]
        public double RoomCount { get; set; }

        //dodati: redactedFloor (sprat), lastFloor (true/false), registered, furnished
    }
}
