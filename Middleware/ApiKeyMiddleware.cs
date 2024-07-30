namespace FileStorageMicroService.Middleware
{
	public class ApiKeyMiddleware
	{
        private readonly RequestDelegate _next;
        private const string APIKEY_NAME = "ApiKey";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY_NAME, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided.");
                return;
            }

            var apiKey = configuration.GetValue<string>(APIKEY_NAME) ?? throw new Exception("Missing Auth Api Key config value");

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }

    }
}

