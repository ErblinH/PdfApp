using PdfApp.API.Models;
using PdfApp.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace PdfApp.API.Middlewares
{
    public class WrappingResponse
    {
        private readonly RequestDelegate _next;

        public WrappingResponse(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new ApiResponse<dynamic>
            {
                Success = false,
                Errors = new List<string> { exception.Message }
            };

            if (exception is BaseException baseException)
            {
                response = new ApiResponse<dynamic>
                {
                    Success = false,
                    Errors = new List<string> { baseException.Message }
                };
                context.Response.StatusCode = (int)baseException.Code;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            var result = System.Text.Json.JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }
}
