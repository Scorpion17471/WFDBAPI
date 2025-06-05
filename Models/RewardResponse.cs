using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class RewardResponse
    {
        // REST response model for relic data
        public List<Reward> RewardList { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
