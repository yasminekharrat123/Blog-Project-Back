﻿namespace Blog.ResponseExceptions
{
    public class BadRequestException : BaseResponseException
    {
        public BadRequestException(string message): base(message, StatusCodes.BAD_REQUEST) { }
    }
}
