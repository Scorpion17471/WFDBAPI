using WFDBAPI.Database.Entities;

namespace WFDBAPI.Models
{
    public class WarframeResponse : BaseResponse
    {
        // REST response model for warframe data
        public List<Warframe> WarframeList { get; set; }
    }
}
