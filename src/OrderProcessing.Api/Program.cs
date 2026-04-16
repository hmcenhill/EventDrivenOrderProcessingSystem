using OrderProcessing.Api.Configuraion;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<RabbitMqConfiguration>(
    builder.Configuration.GetSection(nameof(RabbitMqConfiguration)));
builder.Services.Configure<RedisConfiguration>(
    builder.Configuration.GetSection(nameof(RedisConfiguration)));


// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();
