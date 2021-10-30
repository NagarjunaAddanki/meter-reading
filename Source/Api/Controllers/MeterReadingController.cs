using Meter.Reading.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Meter.Reading.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly ILogger<MeterReadingController> _logger;

        public MeterReadingController(ILogger<MeterReadingController> logger)
        {
            _logger = logger;
        }

        [HttpPost("meter-reading-uploads")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostReadings(
            [FromServices] IMeterReadingImporter meterReadingImporter,
            IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string encodedString = Convert.ToBase64String(fileBytes);
                    byte[] data = Convert.FromBase64String(encodedString);
                    string decodedString = Encoding.UTF8.GetString(data);
                    return Ok(await meterReadingImporter.ImportMeterData(decodedString));
                }
            }

            return BadRequest();
        }
    }
}
