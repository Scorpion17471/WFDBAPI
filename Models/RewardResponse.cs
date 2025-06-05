using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class RewardResponse : BaseResponse
    {
        // REST response model for reward data
        public List<Reward> RewardList { get; set; }
    }
}
