using API.Validator;
using Application.Interfaces;
using Application.Models.Request;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/api-log.txt", rollingInterval: RollingInterval.Day);
});

builder.Services.AddScoped<INumberSequenceService, NumberSequenceService>();
builder.Services.AddScoped<INumberSequenceAnalyzer, NumberSequenceAnalyzer>();
builder.Services.AddScoped<IValidator<NumberSequenceRequest>, NumberSequenceRequestValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Number Sequence Analyzer API",
        Version = "v1",
        Description = "API desenvolvida para teste técnico ForHolding",
        Contact = new OpenApiContact
        {
            Name = "Leonardo Sousa C.\"",
            Email = "leonardodont.dz@live.com"
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Number Sequence Analyzer API v1");
        c.DocumentTitle = "Documentação da API - Teste Técnico";
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
