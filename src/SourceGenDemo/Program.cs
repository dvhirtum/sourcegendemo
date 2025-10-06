var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCqrs();

var app = builder.Build();

app.MapGet("/", () => Hello.World);

app.Run();
