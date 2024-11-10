using Initializer;
using Server.Services;


// Объявляем клиент перевода и кэширования.
Run.DefineCore();

// Создаем Http/2 сервер
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Добавляем сервис в контейнер
builder.Services.AddGrpc();

WebApplication app = builder.Build();

// Настраиваем пипелайн http запроса
app.MapGrpcService<TranslatorService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();