using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Models.Convert
{
    public class ConvertJobCreateModel
    {
        public string HtmlInput { get; set; }
        public int ConverterOptionsId { get; set; }
    }
}
