using AutoMapper;
using PdfApp.Application.Models.Convert;
using PdfApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.AutoMapper
{
    public class PdfMapper : Profile
    {
        public PdfMapper() 
        {
            ConvertJobMapper();
            ConvertMarginsMapper();
            ConverOptionsMapper();
        }

        public void ConvertJobMapper() 
        {
            CreateMap<ConvertJobCreateModel, ConvertJob>().ReverseMap();
        }

        public void ConvertMarginsMapper()
        {
            CreateMap<ConvertMarginsCreateModel, ConverterMargins>().ReverseMap();
        }

        public void ConverOptionsMapper()
        {
            CreateMap<ConvertOptionsCreateModel, ConverterOptions>().ReverseMap();
        }
    }
}
