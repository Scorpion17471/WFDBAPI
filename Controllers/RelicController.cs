using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: /relic (Read/Select)
        [HttpGet]
        public RelicResponse GetRelics()
        {
            // Created HTTP Response
            RelicResponse response = new RelicResponse();
            try
            {
                // Attempt to retrieve list of all relics from database
                var dbTask = _dbContext.Relic.ToList();
                if (dbTask != null && dbTask.Count > 0)
                {
                    // List has data
                    response.Status = 200;
                    response.Message = "Successfully retrieved list of relics";
                    response.RelicList = dbTask;
                }
                else
                {
                    //List does not have data
                    response.Status = 400;
                    response.Message = "Failed to retrieve list of relics";
                    response.RelicList = null;
                }
            }
            catch (Exception ex)
            {
                // Errors in CRUD operations
                _logger.LogError(ex, "Error fetching relics from database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // GET: /relic/{relicName} (Read/Select by Name)
        [HttpGet("{name}", Name = "GetRelicByName")]
        public NameRelicResponse GetRelicByName(string name)
        {
            NameRelicResponse response = new NameRelicResponse();
            try
            {
                var nameTask = _dbContext.Relic.FirstOrDefault(x => x.Name == name);
                if (nameTask != null)
                {
                    response.Status = 200;
                    response.Message = "Success";
                    response.Relic = nameTask;
                }
                else
                {
                    response.Status = 400;
                    response.Message = $"Failed to find {name}";
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

        // POST: /relic (Create)
        [HttpPost]
        public async Task<BaseResponse> PostRelicAsync([FromBody] Relic newRelic)
        {
            // DB Table autosets ID to increment with each entry
            newRelic.ID = 0;
            newRelic.Name = newRelic.Name.ToUpper();
            // Track new relic for CRUD
            _dbContext.Relic.Add(newRelic);

            BaseResponse response = new BaseResponse();
            try
            {
                // Save new relic to DB
                var dbTask = await _dbContext.SaveChangesAsync();
                if (dbTask != 0)
                {
                    // Item saved to DB
                    response.Status = 201;
                    response.Message = "Successfully created and stored new relic";
                }
                else
                {
                    // Item failed to be saved to DB
                    response.Status = 400;
                    response.Message = "Failed to create and store new relic";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding relic to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // PUT: /relic (Update)
        [HttpPut("{relicName}", Name = "PutRelicAsync")]
        public async Task<BaseResponse> PutRelicAsync(string relicName, [FromBody] Relic newRelic)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                // Attempt to find relic in DB
                var dbTask = await _dbContext.Relic.FirstOrDefaultAsync<Relic>(x => x.Name == relicName.ToUpper());
                if (dbTask != null)
                {
                    // Update Name/Vaulted status and attempt to save to DB
                    dbTask.Name = newRelic.Name.ToUpper();
                    dbTask.Vaulted = newRelic.Vaulted;

                    var changed = await _dbContext.SaveChangesAsync();

                    if (changed != 0)
                    {
                        // Successfully updated DB record
                        response.Status = 200;
                        response.Message = "Successfully updated relic";
                    }
                    else
                    {
                        // Failed to update DB record
                        response.Status = 400;
                        response.Message = "Failed to update relic";
                    }
                }
                else
                {
                    // Failed to find relic in DB
                    response.Status = 404;
                    response.Message = "Failed to find relic in database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding relic to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // PATCH: /relic (Update Partial (Vaulted Status))
        [HttpPatch("{relicName}", Name = "PatchRelicAsync")]
        public async Task<BaseResponse> PatchRelicAsync(string relicName, [FromBody] bool newVaulted)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                // Attempt to find relic in DB
                var dbTask = await _dbContext.Relic.FirstOrDefaultAsync<Relic>(x => x.Name == relicName.ToUpper());
                if (dbTask != null)
                {
                    // Update Name/Vaulted status and attempt to save to DB
                    dbTask.Vaulted = newVaulted;

                    var changed = await _dbContext.SaveChangesAsync();

                    if (changed != 0)
                    {
                        // Successfully updated DB record
                        response.Status = 200;
                        response.Message = "Successfully updated relic";
                    }
                    else
                    {
                        // Failed to update DB record
                        response.Status = 400;
                        response.Message = "Failed to update relic";
                    }
                }
                else
                {
                    // Failed to find relic in DB
                    response.Status = 404;
                    response.Message = "Failed to find relic in database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding relic to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // DELETE: /relic (Delete)
        [HttpDelete]
        public async Task<BaseResponse> DeleteRelicAsync([FromBody] string relicName)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                // Attempt to find relic in DB
                var dbTask = await _dbContext.Relic.FirstOrDefaultAsync<Relic>(x => x.Name == relicName.ToUpper());
                if (dbTask != null)
                {
                    // Remove relic from DB
                    _dbContext.Relic.Remove(dbTask);
                    var deleted = await _dbContext.SaveChangesAsync();

                    if (deleted != 0)
                    {
                        // Successfully updated DB record
                        response.Status = 200;
                        response.Message = "Successfully deleted relic";
                    }
                    else
                    {
                        // Failed to update DB record
                        response.Status = 400;
                        response.Message = "Failed to delete relic";
                    }
                }
                else
                {
                    // Failed to find relic in DB
                    response.Status = 404;
                    response.Message = "Failed to find relic in database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding relic to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
