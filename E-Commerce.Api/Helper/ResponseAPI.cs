namespace E_Commerce.Api.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string? message = null, object? data = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(StatusCode);
            Data = data;
        }

        private string GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Resource Not Found",
                500 => "Server Error",
                _ => "Unknown Error",
            };
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; } 
    }

}
