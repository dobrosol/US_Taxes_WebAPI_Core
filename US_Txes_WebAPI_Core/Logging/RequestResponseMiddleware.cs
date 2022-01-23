using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace US_Txes_WebAPI_Core.Logging
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);

            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var originalRequestBody = context.Request.Body;

            originalRequestBody.Seek(0, SeekOrigin.Begin);
            string requestBody = await new StreamReader(originalRequestBody).ReadToEndAsync();
            originalRequestBody.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"{Environment.NewLine} UTC DateTime:{DateTime.UtcNow} {Environment.NewLine}" + 
                                   $"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} {Environment.NewLine}" +
                                   $"Host: {context.Request.Host} {Environment.NewLine}" +
                                   $"Method:{context.Request.Method} {Environment.NewLine}" +
                                   $"Path: {context.Request.Path} {Environment.NewLine}" +
                                   $"QueryString: {context.Request.QueryString} {Environment.NewLine}" +
                                   $"Request Body: {requestBody} {Environment.NewLine}");
        }

        private async Task LogResponse(HttpContext context)
        {
            var responseBody = string.Empty;

            using (var stream = new MemoryStream())
            {
                var originalResponseBody = context.Response.Body;
                context.Response.Body = stream;

                await _next(context);

                stream.Seek(0, SeekOrigin.Begin);
                responseBody = await new StreamReader(stream).ReadToEndAsync();
                stream.Seek(0, SeekOrigin.Begin);

                await stream.CopyToAsync(originalResponseBody);
                context.Response.Body = originalResponseBody;
            }
            
            _logger.LogInformation($"{Environment.NewLine} UTC DateTime:{DateTime.UtcNow} {Environment.NewLine}" +
                                   $"Http Response Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} {Environment.NewLine}" +
                                   $"Host: {context.Request.Host} {Environment.NewLine}" +
                                   $"Path: {context.Request.Path} {Environment.NewLine}" +
                                   $"QueryString: {context.Request.QueryString} {Environment.NewLine}" +
                                   $"Status Code: {context.Response.StatusCode} {Environment.NewLine}" +
                                   $"Response Body: {responseBody}{Environment.NewLine}");
        }
    }
}
