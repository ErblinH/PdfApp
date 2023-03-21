using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Entities
{
    public class ConverterOptions : BaseEntity
    {
        public string PageColorMode { get; set; }
        public string PageOrientation { get; set; }
        public string PagePaperSize { get; set; }
        public int ConverterMarginsId { get; set; }
        public ConverterMargins ConverterMargins { get; set; }
        public ICollection<ConvertJob> ConvertJob { get; set; }
    }
}
