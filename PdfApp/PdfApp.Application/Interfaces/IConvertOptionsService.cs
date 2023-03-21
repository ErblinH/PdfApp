using PdfApp.Application.Models.Convert;
using PdfApp.Domain.Entities;
using PdfApp.Domain.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Interfaces
{
    public interface IConvertOptionsService
    {
        Task<ConverterOptions> CreateAsync(ConvertOptionsCreateModel liveJobModel);
        Task<ConverterOptions> GetByNameAsync(int optionsId);
        Task<IPagedList<ConverterOptions>> GetAllAsync(int pageIndex, int pageSize, int status);
        Task<bool> DeleteAsync(int optionsId);
    }
}
