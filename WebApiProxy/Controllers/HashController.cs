using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiProxy.Utility;

namespace WebApiProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashController : ControllerBase
    {
        private readonly IHashFile _hashFile;
        private readonly ILogger<HashController> _logger;

        public HashController(IHashFile hashFile, ILogger<HashController> logger)
        {
            _hashFile = hashFile;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(string fileUri)
        {
            if (string.IsNullOrWhiteSpace(fileUri))
                return BadRequest("fileUri is requried");

            try
            {
                var hashFilebase64String = _hashFile.GetHashFileBase64(fileUri);
                return Ok($"hashBase64: {hashFilebase64String}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Internal Error");
            }
        }
    }
}