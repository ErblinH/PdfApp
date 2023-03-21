using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Models.Convert
{
    public class ConvertOptionsCreateModel
    {
        public string PageColorMode { get; set; }
        public string PageOrientation { get; set; }
        public string PagePaperSize { get; set; }
        public int ConverterMarginsId { get; set; }
    }
}
