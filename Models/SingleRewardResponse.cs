using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class SingleRewardResponse : BaseResponse
    {
        // REST response model for single relic data
        public Reward? Reward { get; set; }
    }
}
