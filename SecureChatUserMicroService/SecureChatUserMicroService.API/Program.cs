using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.GrpcServices;
using SecureChatUserMicroService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    })
    .AddJsonTranscoding();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5555, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    options.ListenLocalhost(5143, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ChatMicroservice Chat API",
        Version = "v1",
        Description = "gRPC-сервис чата",
        Contact = new OpenApiContact
        {
            Name = "Demyan",
            Email = "demyan@mail.ru"
        }
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, "SecureChatUserMicroService.xml");
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
        options.IncludeGrpcXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

// Регистрация gRPC-сервисов
app.MapGrpcService<UsersGrpcService>().EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client...");

app.Run();