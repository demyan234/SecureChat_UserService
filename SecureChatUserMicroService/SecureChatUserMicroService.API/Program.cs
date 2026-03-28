using Microsoft.OpenApi.Models;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.GrpcServices;
using SecureChatUserMicroService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ChatMicroservice User API",
        Version = "v1",
        Description = "API для микросервиса чатов",
        Contact = new OpenApiContact
        {
            Name = "Demyan",
            Email = "demyan@mail.ru"
        }
    });
});

var app = builder.Build();

app.UseCors("AllowBlazorClient");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatMicroservice User API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
    app.UseHttpsRedirection();
}
else
{
    app.UseResponseCaching();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

#region MAP_GRPC

app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
app.MapGrpcService<UserProfileGrpcService>().EnableGrpcWeb();
app.MapGrpcService<BlockUserGrpcService>().EnableGrpcWeb();

#endregion

app.Run();