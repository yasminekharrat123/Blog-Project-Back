namespace Blog.ResponseExceptions
{
    public enum StatusCodes
    {
        BAD_REQUEST = 400,
        UNAUTHORIZED = 401,
        FORBIDDEN = 403,
        NOT_FOUND = 404,
        INTERNAL_SERVER_ERROR = 500,
    }
    public class BaseResponseException : Exception
    {
        public StatusCodes StatusCode { get; set; }
        public BaseResponseException(string message, StatusCodes statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
