using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Models
{
    public class PdfOptions
    {
        public string PageColorMode { get; set; }
        public string PageOrientation { get; set; }
        public string PagePaperSize { get; set; }
        public PageMargins PageMargins { get; set; }

    }
}
