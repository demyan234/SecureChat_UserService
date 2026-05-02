using Microsoft.AspNetCore.Server.Kestrel.Core;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.GrpcServices;
using SecureChatUserMicroService.Infrastructure.Extensions;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
})
.AddJsonTranscoding();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(
        IPAddress.Any,
        5575,
        listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });

    // HTTP endpoint — порт 4126 (для Swagger, Health, REST)
    options.Listen(
        IPAddress.Any,
        4126,
        listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        });
});
builder.Services.AddGrpcClient<ChatService.Proto.ChatGrpcService.ChatGrpcServiceClient>(options =>
    {
        var chatServiceUrl = builder.Environment.IsDevelopment()
            ? "http://localhost:5576"
            : builder.Configuration.GetValue<string>("Services:ChatService:Url");
        options.Address = new(chatServiceUrl);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        return handler;
    });

builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "ChatMicroservice Chat API",
        Version = "v1",
        Description = "gRPC-сервис чата",
        Contact = new()
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

app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    service = "ChatUserService"
}));

// Регистрация gRPC-сервисов
app.MapGrpcService<UsersGrpcService>().EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client...");

app.Run();