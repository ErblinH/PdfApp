using PdfApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Exceptions
{
    public abstract class BaseException : Exception
    {
        public ExceptionType Type { get; set; } = ExceptionType.ServerError;
        public HttpStatusCode Code { get; set; } = HttpStatusCode.InternalServerError;

        protected BaseException(string message, ExceptionType type, HttpStatusCode code) : base(message)
        {
            this.Code = code;
            this.Type = type;
        }
    }
}
