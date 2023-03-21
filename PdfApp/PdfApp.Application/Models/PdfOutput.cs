using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Models
{
    public class PdfOutput
    {
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
        public string? ErrorMessage { get; private set; }
        public string? PdfDocument { get; private set; }
        public int? PdfDocumentSize { get; private set; }

        public PdfOutput(string errorMessage) => ErrorMessage = errorMessage;

        public PdfOutput(string pdfDocument, int pdfDocumentSize)
        {
            PdfDocument = pdfDocument;
            PdfDocumentSize = pdfDocumentSize;
        }
    }
}
