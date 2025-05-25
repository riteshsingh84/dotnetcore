var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<WebHookPOC.Services.IWebhookService>(WebhookService => new WebHookPOC.Services.WebhookService());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseAuthorization();

app.MapControllers();

app.Run();
