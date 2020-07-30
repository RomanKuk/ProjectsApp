using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class ExceptionFilterExtensions
    {
        public static (HttpStatusCode statusCode, ErrorCode errorCode) ParseException(this Exception exception)
        {
            return exception switch
            {
                NotFoundException _ => (HttpStatusCode.NotFound, ErrorCode.NotFound),
                ArgumentException _ => (HttpStatusCode.BadRequest, ErrorCode.Argument),
                _ => (HttpStatusCode.InternalServerError, ErrorCode.General),
            };
        }
    }
    public enum ErrorCode
    {
        General = 1,
        NotFound,
        Argument
    }
}
