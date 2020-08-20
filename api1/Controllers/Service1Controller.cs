using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Service1Controller : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<Service1Controller> _logger;

        public Service1Controller(ILogger<Service1Controller> logger)
        {
            _logger = logger;
        }

       [HttpGet]
        public async Task<IActionResult> Get()
        {
           return Ok("from service2");
        }
    }
}
