using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class WarframeResponse
    {
        // REST response model for relic data
        public List<Warframe> WarframeList { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
