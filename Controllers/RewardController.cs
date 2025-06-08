using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: /reward/{relicName}+{warframeName}
        [HttpGet("{relicName}+{warframeName}", Name = "GetRewardByRelicAndFrame")]
        public async Task<SingleRewardResponse> GetRewardByRelicAndFrameAsync(string relicName, string warframeName)
        {
            SingleRewardResponse response = new SingleRewardResponse();
            try
            {
                // Attempt to find corresponding relic and warframe in DB first
                var namedWF = await _dbContext.Warframe.FirstOrDefaultAsync(x => x.Name == warframeName.ToUpper());
                var namedRelic = await _dbContext.Relic.FirstOrDefaultAsync(x => x.Name == relicName.ToUpper());
                // 404 if either relic or warframe is not found
                if (namedWF == null || namedRelic == null)
                {
                    response.Status = 404;
                    response.Message = "Failed to find relic or warframe in database";
                    return response;
                }
                var nameTask = await _dbContext.Reward.FirstOrDefaultAsync
                    (
                        x => (x.RelicID == namedRelic.ID && x.WarframeID == namedWF.ID)
                    );
                if (nameTask != null)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.Reward = nameTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = $"Failed to find a {warframeName} reward in {relicName}";
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

        // GET: /reward/relic/{relicName}
        [HttpGet("relic/{relicName}", Name = "GetRewardByRelicAsync")]
        public async Task<RewardResponse> GetRewardByRelicAsync(string relicName)
        {
            RewardResponse response = new RewardResponse();
            try
            {
                // Attempt to find corresponding relic in DB first
                var namedRelic = await _dbContext.Relic.FirstOrDefaultAsync(x => x.Name == relicName.ToUpper());
                // 404 if relic is not found
                if (namedRelic == null)
                {
                    response.Status = 404;
                    response.Message = "Failed to find relic in database";
                    return response;
                }
                // Get all rewards associated with the relic and return if possible
                var nameTask = await _dbContext.Reward.Where
                    (
                        x => (x.RelicID == namedRelic.ID)
                    ).ToListAsync();
                if (nameTask != null)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.RewardList = nameTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = $"Failed to find any rewards from {relicName}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Rewards from database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // GET: /reward/warframe/{warframeName}
        [HttpGet("warframe/{warframeName}", Name = "GetRewardByWarframeAsync")]
        public async Task<RewardResponse> GetRewardByWarframeAsync(string warframeName)
        {
            RewardResponse response = new RewardResponse();
            try
            {
                // Attempt to find corresponding Relic and Warframe in DB first
                var namedWF = await _dbContext.Warframe.FirstOrDefaultAsync(x => x.Name == warframeName.ToUpper());
                // 404 if either Relic or Warframe is not found
                if (namedWF == null)
                {
                    response.Status = 404;
                    response.Message = "Failed to find warframe in database";
                    return response;
                }
                // Get all rewards associated with the warframe and return if possible
                var nameTask = await _dbContext.Reward.Where
                    (
                        x => (x.WarframeID == namedWF.ID)
                    ).ToListAsync();
                if (nameTask != null)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.RewardList = nameTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = $"Failed to find a any {warframeName} rewards";
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

        // POST: /reward/create/{warframeName}+{relicName} (Create)
        [HttpPost("create/{warframeName}+{relicName}", Name = "PostRewardAsync")]
        public async Task<BaseResponse> PostRewardAsync(string warframeName, string relicName, [FromBody] Reward newReward)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                // DB Table autosets ID to increment with each entry
                newReward.ID = 0;
                // Attempt to find corresponding Relic and Warframe in DB first
                var namedWF = await _dbContext.Warframe.FirstOrDefaultAsync(x => x.Name == warframeName.ToUpper());
                var namedRelic = await _dbContext.Relic.FirstOrDefaultAsync(x => x.Name == relicName.ToUpper());
                if (namedWF == null || namedRelic == null)
                {
                    response.Status = 404;
                    response.Message = "Failed to find matching relic or warframe in database";
                    return response;
                }
                newReward.PartType = newReward.PartType.ToUpper();
                newReward.Rarity = newReward.Rarity.ToUpper();
                newReward.WarframeID = namedWF.ID;
                newReward.RelicID = namedRelic.ID;
                // Track new Reward for CRUD
                _dbContext.Reward.Add(newReward);
                // Save new Reward to DB
                var dbTask = await _dbContext.SaveChangesAsync();
                if (dbTask != 0)
                {
                    // Item saved to DB
                    response.Status = 201;
                    response.Message = "Successfully created and stored new reward";
                }
                else
                {
                    // Item failed to be saved to DB
                    response.Status = 400;
                    response.Message = "Failed to create and store new reward";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reward to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // DELETE: /reward/delete/{warframeName}+{relicName}+{partType} (Delete)
        [HttpDelete("delete/{warframeName}+{relicName}+{partType}", Name = "DeleteRewardAsync")]
        public async Task<BaseResponse> DeleteRewardAsync(string warframeName, string relicName, string partType)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var namedWF = await _dbContext.Warframe.FirstOrDefaultAsync(x => x.Name == warframeName.ToUpper());
                var namedRelic = await _dbContext.Relic.FirstOrDefaultAsync(x => x.Name == relicName.ToUpper());
                if (namedWF == null || namedRelic == null)
                {
                    response.Status = 404;
                    response.Message = "Failed to find matching relic or warframe in database";
                    return response;
                }
                // Attempt to find Reward in DB
                var dbTask = await _dbContext.Reward.FirstOrDefaultAsync
                    (
                        x => x.WarframeID == namedWF.ID && x.RelicID == namedRelic.ID
                        && x.PartType.ToUpper() == partType.ToUpper()
                    );
                if (dbTask != null)
                {
                    // Remove Reward from DB
                    _dbContext.Reward.Remove(dbTask);
                    var deleted = await _dbContext.SaveChangesAsync();

                    if (deleted != 0)
                    {
                        // Successfully updated DB record
                        response.Status = 200;
                        response.Message = "Successfully deleted reward";
                    }
                    else
                    {
                        // Failed to update DB record
                        response.Status = 400;
                        response.Message = "Failed to delete reward";
                    }
                }
                else
                {
                    // Failed to find Reward in DB
                    response.Status = 404;
                    response.Message = "Failed to find reward in database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reward to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
