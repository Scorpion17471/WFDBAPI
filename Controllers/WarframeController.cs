using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: /Warframe
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

        // GET: /Warframe/{name}
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

        // POST: /Warframe/Create (Create)
        [HttpPost("Create", Name = "PostWarframeAsync")]
        public async Task<BaseResponse> PostWarframeAsync([FromBody] Warframe newWarframe)
        {
            // DB Table autosets ID to increment with each entry
            newWarframe.ID = 0;
            newWarframe.Name = newWarframe.Name.ToUpper();
            // Track new Warframe for CRUD
            _dbContext.Warframe.Add(newWarframe);

            BaseResponse response = new BaseResponse();
            try
            {
                // Save new Warframe to DB
                var dbTask = await _dbContext.SaveChangesAsync();
                if (dbTask != 0)
                {
                    // Item saved to DB
                    response.Status = 201;
                    response.Message = "Successfully created and stored new Warframe";
                }
                else
                {
                    // Item failed to be saved to DB
                    response.Status = 400;
                    response.Message = "Failed to create and store new Warframe";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Warframe to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // PUT: /Warframe/Update/{WarframeName} (Update)
        [HttpPut("{WarframeName}", Name = "PutWarframeAsync")]
        public async Task<BaseResponse> PutWarframeAsync(string WarframeName, [FromBody] Warframe newWarframe)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                // Attempt to find Warframe in DB
                var dbTask = await _dbContext.Warframe.FirstOrDefaultAsync<Warframe>(x => x.Name == WarframeName.ToUpper());
                if (dbTask != null)
                {
                    // Update Name/Vaulted status and attempt to save to DB
                    dbTask.Name = newWarframe.Name.ToUpper();

                    var changed = await _dbContext.SaveChangesAsync();

                    if (changed != 0)
                    {
                        // Successfully updated DB record
                        response.Status = 200;
                        response.Message = "Successfully updated Warframe";
                    }
                    else
                    {
                        // Failed to update DB record
                        response.Status = 400;
                        response.Message = "Failed to update Warframe";
                    }
                }
                else
                {
                    // Failed to find Warframe in DB
                    response.Status = 404;
                    response.Message = "Failed to find Warframe in database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Warframe to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        // DELETE: /Warframe/Remove (Delete)
        [HttpDelete("Remove", Name = "DeleteWarframeAsync")]
        public async Task<BaseResponse> DeleteWarframeAsync([FromBody] string WarframeName)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                // Attempt to find Warframe in DB
                var dbTask = await _dbContext.Warframe.FirstOrDefaultAsync<Warframe>(x => x.Name == WarframeName.ToUpper());
                if (dbTask != null)
                {
                    // Remove Warframe from DB
                    _dbContext.Warframe.Remove(dbTask);
                    var deleted = await _dbContext.SaveChangesAsync();

                    if (deleted != 0)
                    {
                        // Successfully updated DB record
                        response.Status = 200;
                        response.Message = "Successfully deleted Warframe";
                    }
                    else
                    {
                        // Failed to update DB record
                        response.Status = 400;
                        response.Message = "Failed to delete Warframe";
                    }
                }
                else
                {
                    // Failed to find Warframe in DB
                    response.Status = 404;
                    response.Message = "Failed to find Warframe in database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Warframe to database.");
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
