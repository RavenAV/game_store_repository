using GameStore.Api.Authorization;
using GameStore.Api.Cors;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.ErrorHandling;
using GameStore.Api.Middleware;
using GameStore.Api.OpenAPI;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddAuthentication()
                .AddJwtBearer()
                .AddJwtBearer("Auth0");

builder.Services.AddGameStoreAuthorization();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
})
.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

builder.Services.AddGameStoreCors(builder.Configuration);

builder.Services.AddSwaggerGen()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddEndpointsApiExplorer();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler());
app.UseMiddleware<RequestTimingMiddleware>();

await app.Services.InitializeDbAsync();

app.UseHttpLogging();
app.MapGamesEndpoints();
app.UseCors();

app.UseGameStoreSwagger();

app.Run();
