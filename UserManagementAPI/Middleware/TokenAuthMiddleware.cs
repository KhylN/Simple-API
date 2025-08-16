namespace UserManagementAPI.Middleware;

public class TokenAuthMiddleware(RequestDelegate next, ILogger<TokenAuthMiddleware> logger, IConfiguration config)
{
    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";
        var method = context.Request.Method.ToUpperInvariant();

        // Allow anonymous for Swagger & health
        if (path.StartsWith("/swagger") || path.Equals("/health", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        // For write ops, require token
        var requiresToken = method is "POST" or "PUT" or "DELETE";
        if (!requiresToken)
        {
            await next(context);
            return;
        }

        var header = context.Request.Headers.Authorization.ToString();
        var token = header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? header["Bearer ".Length..]
            : null;

        var validToken = config["API_TOKEN"] ?? "dev-token"; // fallback for local testing

        if (string.IsNullOrWhiteSpace(token) || token != validToken)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
            return;
        }

        await next(context);
    }
}
