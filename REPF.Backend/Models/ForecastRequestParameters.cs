using REPF.Backend.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace REPF.Backend.Models
{
    public class ForecastRequestParameters
    {
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Use JsonStringEnumConverter
        public Location Location { get; set; }

        [Required]
        public double RoomCount { get; set; }

    }
}
