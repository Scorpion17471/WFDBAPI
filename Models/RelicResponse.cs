using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class RelicResponse
    {
        // REST response model for relic data
        public List<Relic> RelicList { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
