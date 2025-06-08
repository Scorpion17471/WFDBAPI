using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class NameRelicResponse : BaseResponse
    {
        // REST response model for single relic data
        public Relic? Relic { get; set; }
    }
}
