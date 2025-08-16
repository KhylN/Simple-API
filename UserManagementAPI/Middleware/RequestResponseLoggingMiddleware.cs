namespace UserManagementAPI.Middleware;

public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;
        var method = context.Request.Method;
        var path = context.Request.Path;

        await next(context);

        var elapsed = (DateTime.UtcNow - start).TotalMilliseconds;
        var status = context.Response?.StatusCode;
        logger.LogInformation("{Method} {Path} -> {Status} ({Elapsed} ms)", method, path, status, elapsed);
    }
}
