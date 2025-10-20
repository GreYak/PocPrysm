using Driver.Api.Extensions;



// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterStack();
builder.Services.AddControllers();
builder.Services.AddSwagger();

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseAppSwagger();
app.MapControllers();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }