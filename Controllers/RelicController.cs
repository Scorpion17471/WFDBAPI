using Microsoft.AspNetCore.Mvc;
using WFDBAPI.Database.Entities;
using WFDBAPI.Database.Entities.Context;
using WFDBAPI.Models;

namespace WFDBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelicController : ControllerBase
    {
        // Controller to interact with Relic DB table
        private readonly ILogger<RelicController> _logger;
        private readonly PrimeDBContext _dbContext;
        public RelicController(ILogger<RelicController> logger, PrimeDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        // GET: /relic
        [HttpGet]
        public RelicResponse GetRelics()
        {
            RelicResponse response = new RelicResponse();
            try
            {
                var dbTask = _dbContext.Relic.ToList();
                if (dbTask != null && dbTask.Count > 0)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.RelicList = dbTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = "Failure";
                    response.RelicList = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching relics from database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
