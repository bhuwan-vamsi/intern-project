using System.Text.Json;

namespace APIPractice.ExcpetionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate request;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate request, ILogger<ExceptionHandlingMiddleware> logger) 
        {
            this.request = request;
            this.logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later."
                };
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
