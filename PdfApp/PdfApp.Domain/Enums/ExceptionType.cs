using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Enums
{
    public enum ExceptionType
    {
        ServerError,
        ConvertJobNotFound,
        ConvertJobNotAdded,
        ConvertJobsNotListed,
        ConvertMarginNotFound,
        ConvertMarginNotAdded,
        ConvertMarginNotListed,
        ConvertMarginNotFoundById,
        ConvertOptionNotFound,
        ConvertOptionNotListed,
        ConvertOptionsNotAdded,
        ConvertOptionsNotFoundById
    }
}
