using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class RelicResponse : BaseResponse
    {
        // REST response model for relic data
        public List<Relic> RelicList { get; set; }
    }
}
