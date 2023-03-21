using FluentValidation;
using PdfApp.Application.Models;
using PdfApp.Domain.Entities;

namespace PdfApp.API.Filters
{
    public class PdfInputValidator : AbstractValidator<PdfInput>
    {
        public PdfInputValidator() 
        {

            RuleFor(x => x.HtmlString)
                .NotEmpty()
                    .WithMessage("HTML base64 encode in empty");

            RuleFor(x => x.Options)
                .NotNull()
                .WithMessage("Please specify the converting options!");

            RuleFor(x => x.Options.PageColorMode)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Plase enter the page color mode");

            RuleFor(x => x.Options.PagePaperSize)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Plase enter the page papaer size");

            RuleFor(x => x.Options.PageOrientation)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Plase enter the page orientation");


        }
    }
}
