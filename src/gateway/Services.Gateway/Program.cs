using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Services.AspNetCore;
using Services.Gateway;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddSwaggerGen();

// Add Ocelot and related services
builder.Services
    .AddOcelot()
    .AddConsul<ConsulServiceBuilder>()
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    })
    .AddPolly();

// Configure app configuration
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    //.AddOcelot(builder.Environment, MergeOcelotJson.ToFile, reloadOnChange: true)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.UseOcelot();

app.Run();