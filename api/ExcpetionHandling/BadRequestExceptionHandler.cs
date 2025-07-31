using APIPractice.Exceptions;
using System.Text.Json;

namespace APIPractice.ExcpetionHandling
{
    public class BadRequestExceptionHandler
    {
        private readonly RequestDelegate request;
        private readonly ILogger logger;
        public BadRequestExceptionHandler(RequestDelegate request, ILogger<BadRequestExceptionHandler> logger)
        {
            this.request= request;
            this.logger= logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await request(context);
            }catch(Exception ex)
            {
                logger.LogError(ex, "An exception occurred");

                var (statusCode, message) = ex switch
                {
                    BadRequestException => (StatusCodes.Status400BadRequest, ex.Message),
                    NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
                    ConflictException => (StatusCodes.Status409Conflict, ex.Message),
                    _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = statusCode,
                    Message = message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
