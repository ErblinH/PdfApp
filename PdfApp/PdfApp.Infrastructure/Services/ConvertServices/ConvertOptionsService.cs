using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PdfApp.Application.Exceptions;
using PdfApp.Application.Interfaces;
using PdfApp.Application.Models.Convert;
using PdfApp.Data.Repositories;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Enums;
using PdfApp.Domain.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Infrastructure.Services.ConvertServices
{
    public class ConvertOptionsService : IConvertOptionsService
    {
        private readonly IRepository<ConverterOptions> _convertOptionsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConverterOptions> _logger;

        public ConvertOptionsService
        (
            IRepository<ConverterOptions> convertOutputsRepository,
            IMapper mapper,
            ILogger<ConverterOptions> logger
        )
        {
            _convertOptionsRepository = convertOutputsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Create
        public async Task<ConverterOptions> CreateAsync(ConvertOptionsCreateModel convertOptionsModel)
        {
            try
            {
                var convertOptions = _mapper.Map<ConverterOptions>(convertOptionsModel);

                if (convertOptions is null)
                    throw new ConvertOptionsException($"Failed to get the convert job!", ExceptionType.ConvertOptionNotFound,
                        HttpStatusCode.NotFound);

                await _convertOptionsRepository.InsertAsync(convertOptions);

                return convertOptions;

            }
            catch (Exception ex)
            {
                throw new ConvertOptionsException($"Failed to create the convert job ex={ex}!", ExceptionType.ConvertOptionsNotAdded,
                   HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteAsync(int optionsId)
        {
            try
            {
                var convertOptions = await _convertOptionsRepository.GetAsync(query => query
                    .Where(x => x.Id == optionsId));

                if (convertOptions == null)
                    throw new ConvertOptionsException($"Failed to get the convert job!", ExceptionType.ConvertOptionNotFound,
                        HttpStatusCode.NotFound);

                convertOptions.Deleted = true;
                _convertOptionsRepository.Update(convertOptions);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Convert job failed to be deleted optionsId={optionsId} - ex={ex}",
                    optionsId, ex);

                return false;
            }
        }
        #endregion

        #region List
        public async Task<IPagedList<ConverterOptions>> GetAllAsync(int pageIndex, int pageSize, int status)
        {
            try
            {
                return await _convertOptionsRepository.GetAllPagedAsync(query => query.Where(x => !x.Deleted));

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get the list of convert options ex={ex}", ex);

                throw new ConvertOptionsException($"Failed to get the list of convert options ex={ex}!", ExceptionType.ConvertOptionNotListed,
                      HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Get
        public async Task<ConverterOptions> GetByNameAsync(int optionsId)
        {
            try
            {
                var convertOptions = await _convertOptionsRepository.GetAsync(query => query
                    .Include(x => x.ConverterMargins)
                    .Where(x => x.Id == optionsId && !x.Deleted));

                if (convertOptions == null)
                    throw new ConvertOptionsException($"Failed to get the convert option!", ExceptionType.ConvertMarginNotFound,
                        HttpStatusCode.NotFound);

                return convertOptions;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get the convert option optionsId={optionsId} - ex={ex}", optionsId, ex);

                throw new ConvertOptionsException($"Failed to get the convert job optionsId={optionsId} ex={ex}!", ExceptionType.ConvertOptionsNotFoundById,
                                   HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
