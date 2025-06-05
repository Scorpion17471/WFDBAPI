namespace WFDBAPI.Models
{
    public class BaseResponse
    {
        // HTTP Status and Message for REST API responses
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
