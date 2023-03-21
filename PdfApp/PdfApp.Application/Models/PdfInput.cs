using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Models
{
    public class PdfInput
    {
        public string? HtmlString { get; set; }
        public PdfOptions? Options { get; set; }

        public PdfInput(string htmlString)
        {
            HtmlString = htmlString;
        }
    }
}
