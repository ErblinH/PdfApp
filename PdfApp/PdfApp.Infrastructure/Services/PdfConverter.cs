using DinkToPdf;
using PdfApp.Application.Exceptions;
using PdfApp.Application.Interfaces;
using PdfApp.Application.Models;
using PdfApp.Data.Repositories;
using PdfApp.Domain;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PdfApp.Infrastructure.Services
{
    public class PdfConverter : IPdfConverter
    {
        public async Task<PdfOutput> ProcessAsync(PdfInput pdfModel)
        {
            try
            {
                var converter = new BasicConverter(new PdfTools());

                var htmlBytes = Convert.FromBase64String(pdfModel.HtmlString);
                var htmlString = Encoding.UTF8.GetString(htmlBytes);

                var colorModeValid = Enum.TryParse(pdfModel.Options.PageColorMode, true, out ColorMode colorMode);
                var orientationValid = Enum.TryParse(pdfModel.Options.PageOrientation, true, out Orientation portrait);
                var paperSizeValid =  Enum.TryParse(pdfModel.Options.PagePaperSize, true, out PaperKind paperKind);

                if (!colorModeValid || !orientationValid || !paperSizeValid)
                    return new PdfOutput("Please make sure all the input options are correct");

                var pdf = converter.Convert(new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    ColorMode = colorMode,
                    Orientation = portrait,
                    PaperSize = paperKind,
                    Margins = new MarginSettings() 
                    { 
                        Top = pdfModel.Options.PageMargins.Top,
                        Right = pdfModel.Options.PageMargins.Right,
                        Bottom = pdfModel.Options.PageMargins.Bottom,
                        Left = pdfModel.Options.PageMargins.Left
                    },
                },
                    Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = htmlString
                    },
                }
                });

                var base64Pdf = Convert.ToBase64String(pdf);
                var pdfSize = pdf.Length;

                Console.WriteLine("Base64 encoded PDF: " + base64Pdf);
                Console.WriteLine("Size of PDF: " + pdfSize + " bytes");

                return new PdfOutput(Convert.ToBase64String(pdf), pdf.Length);
            }
            catch (Exception ex)
            {
                throw new PdfConverterException($"Failed to convert html to pdf ex={ex}!", ExceptionType.ServerError,
                        HttpStatusCode.InternalServerError);
            }
        }
    }
}
