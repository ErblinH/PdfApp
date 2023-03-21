using PdfApp.Application.Models.Convert;
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
    public interface IConvertJobService
    {
        Task<ConvertJob> CreateAsync(ConvertJobCreateModel liveJobModel);
        Task<ConvertJob> GetByNameAsync(Guid name);
        Task<IPagedList<ConvertJob>> GetAllAsync(int pageIndex, int pageSize, int status);
        Task<bool> DeleteAsync(Guid name);
    }
}
