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
    [Route("simApi/[controller]")]
    [ApiController]
    public class SimulatorHashController : ControllerBase
    {
        private readonly IHashFile _hashFile;
        private readonly ILogger<SimulatorHashController> _logger;


        public SimulatorHashController(IHashFile hashFile, ILogger<SimulatorHashController> logger)
        {
            _hashFile = hashFile;
            _logger = logger;
        }


        [HttpGet]
        public async Task<string> Get(string fileUri)
        {
            _logger.LogInformation($"Get fileUri:{fileUri}");

            try {
                var hashFilebase64String = _hashFile.SimulatorHashFileBase64Async(fileUri);
                return await hashFilebase64String;
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }
    }
}