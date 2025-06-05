using Microsoft.AspNetCore.Mvc;
using WFDBAPI.Database.Entities;
using WFDBAPI.Database.Entities.Context;
using WFDBAPI.Models;

namespace WFDBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RewardController : ControllerBase
    {
        // Controller to interact with Reward DB table
        private readonly ILogger<RewardController> _logger;
        private readonly PrimeDBContext _dbContext;
        public RewardController(ILogger<RewardController> logger, PrimeDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        // GET: /reward
        [HttpGet]
        public RewardResponse GetRewards()
        {
            RewardResponse response = new RewardResponse();
            try
            {
                var dbTask = _dbContext.Reward.ToList();
                if (dbTask != null && dbTask.Count > 0)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.RewardList = dbTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = "Failure";
                    response.RewardList = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching rewards from database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
