using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment Env;
        private readonly ILogger<ExceptionMiddleware> Logger;
        private readonly RequestDelegate Next;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.Next = next;
            this.Logger = logger;
            this.Env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);   
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = Env.IsDevelopment() ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
                new ApiResponse((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}