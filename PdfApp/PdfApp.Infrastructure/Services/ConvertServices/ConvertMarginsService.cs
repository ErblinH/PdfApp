using AutoMapper;
using Microsoft.Extensions.Logging;
using PdfApp.Application.Exceptions;
using PdfApp.Application.Interfaces;
using PdfApp.Application.Models.Convert;
using PdfApp.Data.Repositories;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Enums;
using PdfApp.Domain.PagedList;
using System.Net;

namespace PdfApp.Infrastructure.Services.ConvertServices
{
    public class ConvertMarginsService : IConvertMarginsService
    {
        private readonly IRepository<ConverterMargins> _convertMarginsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConverterMargins> _logger;

        public ConvertMarginsService
        (
            IRepository<ConverterMargins> convertMarginsRepository,
            IMapper mapper,
            ILogger<ConverterMargins> logger
        )
        {
            _convertMarginsRepository = convertMarginsRepository;
            _mapper = mapper;
            logger = _logger;
        }

        #region Create
        public async Task<ConverterMargins> CreateAsync(ConvertMarginsCreateModel createMarginsModel)
        {
            try
            {
                var convertMargin = _mapper.Map<ConverterMargins>(createMarginsModel);

                if (convertMargin is null)
                    throw new ConvertMarginsException($"Failed to get the convert margins!", ExceptionType.ConvertMarginNotFound,
                        HttpStatusCode.NotFound);

                await _convertMarginsRepository.InsertAsync(convertMargin);

                return convertMargin;
            }
            catch (Exception ex)
            {
                throw new ConvertJobException($"Failed to create the convert margin ex={ex}!", ExceptionType.ConvertMarginNotAdded,
                     HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteAsync(int marginId)
        {
            try
            {
                var convertMargin = await _convertMarginsRepository.GetAsync(query => query
                    .Where(x => x.Id == marginId));

                if (convertMargin is null)
                    throw new ConvertMarginsException($"Failed to get the convert margins!", ExceptionType.ConvertMarginNotFound,
                        HttpStatusCode.NotFound);

                convertMargin.Deleted = true;
                _convertMarginsRepository.Update(convertMargin);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Convert margin failed to be deleted marginId={id} - ex={ex}",
                    marginId, ex);

                return false;
            }
        }
        #endregion

        #region List
        public async Task<IPagedList<ConverterMargins>> GetAllAsync(int pageIndex, int pageSize, int status)
        {
            try
            {
                return await _convertMarginsRepository.GetAllPagedAsync(query => query.Where(x => !x.Deleted));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get the list of convert margin ex={ex}", ex);

                throw new ConvertJobException($"Failed to get the list of convert margins ex={ex}!", ExceptionType.ConvertMarginNotListed,
                                   HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Get by id
        public async Task<ConverterMargins> GetByNameAsync(int marginId)
        {
            try
            {
                var convertMargin = await _convertMarginsRepository.GetAsync(query => query
                    .Where(x => x.Id == marginId && !x.Deleted));

                if (convertMargin == null)
                    throw new ConvertJobException($"Failed to get the convert margin!", ExceptionType.ConvertMarginNotFound,
                        HttpStatusCode.NotFound);

                return convertMargin;

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get the convert margin marginId={marginId} - ex={ex}", marginId, ex);

                throw new ConvertJobException($"Failed to get the convert job marginId={marginId} ex={ex}!", ExceptionType.ConvertMarginNotFoundById,
                                   HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
