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
    public interface IConvertMarginsService
    {
        Task<ConverterMargins> CreateAsync(ConvertMarginsCreateModel liveJobModel);
        Task<ConverterMargins> GetByNameAsync(int marginId);
        Task<IPagedList<ConverterMargins>> GetAllAsync(int pageIndex, int pageSize, int status);
        Task<bool> DeleteAsync(int marginId);
    }
}
