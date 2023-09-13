using TavelEase_WebService.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

