using arts_core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ISeed _seeder;
        private readonly ILogger<SeedController> _logger;
        public SeedController(ISeed seed, ILogger<SeedController> logger)
        {
            _seeder = seed;
            _logger = logger;
        }

        [HttpGet("seedProducts")]
        public IActionResult SeedProducts()
        {
            try
            {
                _seeder.SeedProductAndVariantData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something wrong in seedController");
            }
            return Ok("");
        }
        [HttpGet("seedUsers")]
        public IActionResult SeedUsers()
        {
            try
            {
                _seeder.SeedUser();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something wrong in seedController");
            }
            return Ok("");
        }

        [HttpGet("seedVariantAttributes")]
        public IActionResult SeedVariantAttributes()
        {
            try
            {
                _seeder.SeedVariantAttribute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something wrong in seedController");
            }
            return Ok("");
        }
    }
}
