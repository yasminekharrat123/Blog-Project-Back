using Blog.ResponseExceptions;

namespace Blog.ReponseExceptions
{
    public class NotFoundException : BaseResponseException
    {
        public NotFoundException(string message): base(message, ResponseExceptions.StatusCodes.NOT_FOUND) { }
    }
}
