using Services.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Starting up");

//var contextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();

app.MapGet("/", (ILogger<Program> logger, IHttpContextAccessor contextAccessor) =>
{
    var context = contextAccessor.HttpContext;
    var msg = $"Welcome to the API running on {context!.Request.Host}";
    logger.LogInformation(msg);
    return Results.Ok(msg);
});

app.MapGet("/info", (ILogger<Program> logger, IHttpContextAccessor contextAccessor) =>
{
    var context = contextAccessor.HttpContext;
    var request = context!.Request;
    var baseUrl = $"{request.Scheme}://{request.Host}";
    var ocelotReqId = context.Request.Headers["OcRequestId"].FirstOrDefault() ?? "N/A";
    var result = $"Url: {baseUrl}, Method: {request.Method}, Path: {request.Path}, OcelotRequestId: {ocelotReqId}";
    logger.LogInformation(result);
    return Results.Ok(result);
});

app.MapGet("/health", (ILogger<Program> logger, IHttpContextAccessor contextAccessor) =>
{
    var context = contextAccessor.HttpContext;
    var msg = $"{context!.Request.Host} is healthy";
    logger.LogInformation(msg);
    return Results.Ok(msg);
});

app.MapGet("/status", (ILogger<Program> logger, IHttpContextAccessor contextAccessor) =>
{
    var context = contextAccessor.HttpContext;
    var msg = $"Running on {context!.Request.Host}";
    logger.LogInformation(msg);
    return Results.Ok(msg);
});

await app.RunAsync();