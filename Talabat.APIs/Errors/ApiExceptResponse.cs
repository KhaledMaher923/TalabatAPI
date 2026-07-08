namespace Talabat.APIs.Errors
{
    public class ApiExceptResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            Details = details;
        }
    }
}
