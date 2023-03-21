using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PdfApp.API.Filters;
using PdfApp.API.Middlewares;
using PdfApp.API.Models;
using PdfApp.Application.AutoMapper;
using PdfApp.Application.Interfaces;
using PdfApp.Application.Models;
using PdfApp.Application.Models.Convert;
using PdfApp.Application.Models.Kubernetes;
using PdfApp.Data;
using PdfApp.Data.Repositories;
using PdfApp.Domain.Caching;
using PdfApp.Domain.Configuration;
using PdfApp.Domain.Entities;
using PdfApp.Domain.PagedList;
using PdfApp.Infrastructure.Services;
using PdfApp.Infrastructure.Services.ConvertServices;
using System.Net;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

var _configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(_configuration);
Singleton<AppSettings>.Instance =
    builder.Services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value;

builder.Services.AddScoped<IPdfConverter, PdfConverter>();
builder.Services.AddScoped<IConvertJobService, ConvertJobService>();
builder.Services.AddScoped<IConvertMarginsService, ConvertMarginsService>();
builder.Services.AddScoped<IConvertOptionsService, ConvertOptionsService>();
builder.Services.AddScoped<IKubernetesDeployerService, KubernetesDeployerService>();

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddTransient<IValidator<PdfInput>, PdfInputValidator>();

var oko = _configuration["ConnectionStrings:EncoderDbContext"];

builder.Services.AddDbContext<PdfAppDbContext>(options =>
                options.UseSqlServer(_configuration["ConnectionStrings:EncoderDbContext"]));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
builder.Services.AddAutoMapper(typeof(PdfMapper));

var ok = builder.Services.BuildServiceProvider().GetRequiredService<PdfAppDbContext>();
var dataInitializer = new DataInitializer(ok);
dataInitializer.EnsureMigrated();

var app = builder.Build();

app.UseRouting();

///// <summary>
///// Generate a base64 pdf representation of a html page
///// </summary>
app.MapPost("/convert",
    async (HttpRequest request, IPdfConverter pdf, IValidator<PdfInput> validator, PdfInput? input) =>
    {
        request.Headers.TryGetValue("X-API-KEY", out var apiKey);

        if (apiKey != "7a8a7cd837b042b58b56617114f4d3d7")
            return Results.Unauthorized();

        return Results.Ok(new ApiResponse<PdfOutput>(await pdf.ProcessAsync(input)));
    });

/// <summary>
/// Create a converter job
/// </summary>
app.MapPost("/convert/job",
    async (IConvertJobService jobService, ConvertJobCreateModel jobCreateModel) =>
    {
        return Results.Ok(new ApiResponse<ConvertJob>(await jobService.CreateAsync(jobCreateModel)));
    });

///// <summary>
///// Get a converter job based on the name
///// </summary>
app.MapGet("/convert/job/{name}",
    async (IConvertJobService jobService, Guid name) =>
    {
        return Results.Ok(new ApiResponse<ConvertJob>(await jobService.GetByNameAsync(name)));
    });

///// <summary>
///// Get a paginated list of converter jobs
///// </summary>
app.MapGet("/convert/job",
    async (IConvertJobService jobService, [FromQuery] int? pageIndex, [FromQuery] int? pageSize, [FromQuery] int? status) =>
    {
        return Results.Ok(new ApiResponse<IPagedList<ConvertJob>>(await jobService.GetAllAsync(pageIndex ?? 0, pageSize ?? 10, status ?? 0)));
    });

/// <summary>
/// Delete a converter job
/// </summary>
app.MapDelete("/convert/job/{name}",
    async (IConvertJobService jobService, Guid name) =>
    {
        return Results.Ok(new ApiResponse<bool>(await jobService.DeleteAsync(name)));
    });

/// <summary>
/// Create a converter option
/// </summary>
app.MapPost("/convert/options",
    async (IConvertOptionsService optionsService, ConvertOptionsCreateModel optionsCreateModel) =>
    {
        return Results.Ok(new ApiResponse<ConverterOptions>(await optionsService.CreateAsync(optionsCreateModel)));
    });

/// <summary>
/// Get a converter option based on the id
/// </summary>
app.MapGet("/convert/options/{id}",
    async (IConvertOptionsService optionsService, int id) =>
    {
        return Results.Ok(new ApiResponse<ConverterOptions>(await optionsService.GetByNameAsync(id)));
    });

/// <summary>
/// Get a paginated list of converter option
/// </summary>
app.MapGet("/convert/options",
    async (IConvertOptionsService optionsService, [FromQuery] int? pageIndex, [FromQuery] int? pageSize, [FromQuery] int? status) =>
    {
        return Results.Ok(new ApiResponse<IPagedList<ConverterOptions>>(await optionsService.GetAllAsync(pageIndex ?? 0, pageSize ?? 10, status ?? 0)));
    });

/// <summary>
/// Delete a converter option
/// </summary>
app.MapDelete("/convert/options/{id}",
    async (IConvertOptionsService optionsService, int id) =>
    {
        return Results.Ok(new ApiResponse<bool>(await optionsService.DeleteAsync(id)));
    });

/// <summary>
/// Create a converter margin
/// </summary>
app.MapPost("/convert/margins",
    async (IConvertMarginsService marginsService, ConvertMarginsCreateModel marginsCreateModel) =>
    {
        return Results.Ok(new ApiResponse<ConverterMargins>(await marginsService.CreateAsync(marginsCreateModel)));
    });

/// <summary>
/// Get a converter margin based on the id
/// </summary>
app.MapGet("/convert/margins/{id}",
    async (IConvertMarginsService marginsService, int id) =>
    {
        return Results.Ok(new ApiResponse<ConverterMargins>(await marginsService.GetByNameAsync(id)));
    });

/// <summary>
/// Get a paginated list of converter margin
/// </summary>
app.MapGet("/convert/margins",
    async (IConvertMarginsService marginsService, [FromQuery] int? pageIndex, [FromQuery] int? pageSize, [FromQuery] int? status) =>
    {
        return Results.Ok(new ApiResponse<IPagedList<ConverterMargins>>(await marginsService.GetAllAsync(pageIndex ?? 0, pageSize ?? 10, status ?? 0)));
    });

/// <summary>
/// Delete a converter margin
/// </summary>
app.MapDelete("/convert/margins/{id}",
    async (IConvertMarginsService marginsService, int id) =>
    {
        return Results.Ok(new ApiResponse<bool>(await marginsService.DeleteAsync(id)));
    });

/// <summary>
/// Create and deploy a kubernetes deployment
/// </summary>
app.MapPost("/deploy",
    async (IKubernetesDeployerService kubernetesServices, PodModel podModel) =>
    {
        return Results.Ok(new ApiResponse<PodResponseModel>(await kubernetesServices.LaunchDeploymentAsync(podModel)));
    });

/// <summary>
/// Delete a kubernetes deployment
/// </summary>
app.MapDelete("/deploy/{name}",
    async (IKubernetesDeployerService kubernetesServices, string name) =>
    {
        return Results.Ok(new ApiResponse<PodResponseModel>(await kubernetesServices.DeleteDeploymentAsync(name)));
    });

/// <summary>
/// Scale a kubernetes deployment
/// </summary>
app.MapPut("/deploy/scale",
    async (IKubernetesDeployerService kubernetesServices, ScaleDeployment scale) =>
    {
        return Results.Ok(new ApiResponse<PodResponseModel>(await kubernetesServices.ScaleDeploymentAsync(scale)));
    });

/// <summary>
/// Watch a kubernetes pod
/// </summary>
app.MapGet("/deploy/{name}",
    async (IKubernetesDeployerService kubernetesServices, string name) =>
    {
        return Results.Ok(new ApiResponse<PodResponseModel>(await kubernetesServices.PodWatcherAsync(name)));
    });

app.UseHttpsRedirection();
app.UseMiddleware<WrappingResponse>();

app.Run();
