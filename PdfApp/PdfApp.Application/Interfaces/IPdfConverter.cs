using PdfApp.Application.Models;
using PdfApp.Domain;
using PdfApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PdfApp.Application.Interfaces
{
    public interface IPdfConverter
    {
        Task<PdfOutput> ProcessAsync(PdfInput pdfInput);
    }
}
