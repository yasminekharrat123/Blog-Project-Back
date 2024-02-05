namespace Blog.ResponseExceptions
{
    public class UnauthorizedException : BaseResponseException
    {
        public UnauthorizedException(string message) : base(message, ResponseExceptions.StatusCodes.UNAUTHORIZED) { }
    }
}
