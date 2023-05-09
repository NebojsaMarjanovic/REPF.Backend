using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPF.Backend.Enumerations;
using REPF.Backend.Models.Input;

namespace REPF.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapperController : ControllerBase
    {
        [HttpGet("{location}")]
        public async Task<IActionResult> GetNeighborhoods(Location location, CancellationToken cancellationToken)
        {
            return Ok(new { Location = LocationMap.locations[location], Neighborhoods = Neighborhoods.NeighborhoodsMap[location]});

        }
    }
}
