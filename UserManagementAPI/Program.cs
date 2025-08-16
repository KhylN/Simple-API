using UserManagementAPI.Middleware;
using UserManagementAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers().AddJsonOptions(o =>
{
    // Keep default; customize if needed
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory repo
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

var app = builder.Build();

// Swagger (dev-friendly)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// MIDDLEWARE ORDER (as required):
// 1) Error handling
app.UseMiddleware<ErrorHandlingMiddleware>();
// 2) Authentication
app.UseMiddleware<TokenAuthMiddleware>();
// 3) Logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.MapControllers();
app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();
