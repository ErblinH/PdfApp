using PdfApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Exceptions
{
    public class ConvertOptionsException : BaseException
    {
        public ConvertOptionsException(string message, ExceptionType type) : base(message, type, HttpStatusCode.BadRequest)
        {

        }

        public ConvertOptionsException(string message, ExceptionType type, HttpStatusCode code) : base(message, type, code)
        {

        }
    }
}
