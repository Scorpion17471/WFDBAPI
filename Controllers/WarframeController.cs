using Microsoft.AspNetCore.Mvc;
using WFDBAPI.Database.Entities;
using WFDBAPI.Database.Entities.Context;
using WFDBAPI.Models;

namespace WFDBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarframeController : ControllerBase
    {
        // Controller to interact with Warframe DB table
        private readonly ILogger<WarframeController> _logger;
        private readonly PrimeDBContext _dbContext;
        public WarframeController(ILogger<WarframeController> logger, PrimeDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        // GET: /warframe
        [HttpGet]
        public WarframeResponse GetWarframes()
        {
            WarframeResponse response = new WarframeResponse();
            try
            {
                var dbTask = _dbContext.Warframe.ToList();
                if (dbTask != null && dbTask.Count > 0)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.WarframeList = dbTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = "Failure";
                    response.WarframeList = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching warframes from database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet("{name}", Name="GetWarframeByName")]
        public NameWarframeResponse GetWarframeByName(string name)
        {
            NameWarframeResponse response = new NameWarframeResponse();
            try
            {
                var nameTask = _dbContext.Warframe.FirstOrDefault(x => x.Name == name);
                if (nameTask != null)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.Warframe = nameTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = $"Failed to find {name}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching warframes from database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
