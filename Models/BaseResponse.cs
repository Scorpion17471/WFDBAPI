namespace WFDBAPI.Models
{
    public class BaseResponse
    {
        // HTTP Status and Message for REST API responses
        public int Status { get; set; }

        #pragma warning disable 8618 // Suppress non-nullable warning due to cleaner syntax in body
        public string Message { get; set; }
        #pragma warning restore 8618
    }
}
