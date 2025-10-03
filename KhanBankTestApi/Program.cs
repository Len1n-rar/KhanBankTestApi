using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KhanBankTestApi",
        Version = "v1"
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// IIS дээр ажиллахад Swagger UI-д шаардлагатай
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Swagger тохиргоо
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // JSON endpoint
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KhanBankTestApi v1");

    // Swagger UI хаанаас нээгдэхийг заана
    // 👉 http://localhost:8083/swagger/
    c.RoutePrefix = "swagger";

    // Хэрэв шууд root дээрээс нээмээр байвал:
    // c.RoutePrefix = string.Empty;
});

app.Run();
