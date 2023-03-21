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
    public class ConvertJobService : IConvertJobService
    {
        private readonly IRepository<ConvertJob> _convertJobRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConvertJob> _logger;

        public ConvertJobService
        (
            IRepository<ConvertJob> convertJobRepository,
            IMapper mapper,
            ILogger<ConvertJob> logger
        )
        {
            _convertJobRepository = convertJobRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Create
        public async Task<ConvertJob> CreateAsync(ConvertJobCreateModel convertJobModel)
        {
            try
            {
                var convertJob = _mapper.Map<ConvertJob>(convertJobModel);

                if (convertJob is null)
                    throw new ConvertJobException($"Failed to get the convert job!", ExceptionType.ConvertJobNotFound,
                        HttpStatusCode.NotFound);

                convertJob.Name = Guid.NewGuid();

                await _convertJobRepository.InsertAsync(convertJob);

                return convertJob;

            }
            catch (Exception ex)
            {
                throw new ConvertJobException($"Failed to create the convert job ex={ex}!", ExceptionType.ConvertJobNotAdded,
                   HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteAsync(Guid name)
        {
            try
            {
                var convertJob = await _convertJobRepository.GetAsync(query => query
                    .Where(x => x.Name.Equals(name)));

                if(convertJob == null)
                    throw new ConvertJobException($"Failed to get the convert job!", ExceptionType.ConvertJobNotFound,
                        HttpStatusCode.NotFound);

                convertJob.Deleted = true;
                _convertJobRepository.Update(convertJob);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Convert job failed to be deleted jobName={name} - ex={ex}",
                    name, ex);

                return false;
            }
        }
        #endregion

        #region List
        public async Task<IPagedList<ConvertJob>> GetAllAsync(int pageIndex, int pageSize, int status)
        {
            try
            {
                return await _convertJobRepository.GetAllPagedAsync(query => query.Where(x => !x.Deleted));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get the list of convert job ex={ex}",ex);

                throw new ConvertJobException($"Failed to get the list of convert job ex={ex}!", ExceptionType.ConvertJobsNotListed,
                                   HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Get
        public async Task<ConvertJob> GetByNameAsync(Guid name)
        {
            try
            {
                var convertJob = await _convertJobRepository.GetAsync(query => query
                    .Where(x => x.Name.Equals(name) && !x.Deleted));

                if (convertJob == null)
                    throw new ConvertJobException($"Failed to get the convert job!", ExceptionType.ConvertJobNotFound,
                        HttpStatusCode.NotFound);

                return convertJob;

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get the convert job name={name} - ex={ex}",name, ex);

                throw new ConvertJobException($"Failed to get the convert job name={name} ex={ex}!", ExceptionType.ConvertJobNotFound,
                                   HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
