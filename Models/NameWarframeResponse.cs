using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class NameWarframeResponse : BaseResponse
    {
        // REST response model for single warframe data
        public Warframe? Warframe { get; set; }
    }
}
