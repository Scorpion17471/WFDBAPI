using Microsoft.AspNetCore.Mvc;
using WFDBAPI.Database.Entities;
using WFDBAPI.Database.Entities.Context;

namespace WFDBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelicController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PrimeDBContext _dbContext;
        public RelicController(ILogger<WeatherForecastController> logger, PrimeDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        [HttpGet]
        public IEnumerable<Relic> Task()
        {
            var dbTask = _dbContext.Relic.ToList();
            if (dbTask != null && dbTask.Count > 0)
            {
                return dbTask;
            }
            throw new Exception("Unable to fetch relic info from database.");
    }
}
