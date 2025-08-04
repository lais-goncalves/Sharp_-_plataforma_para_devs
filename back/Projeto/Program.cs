using Projeto.Config;
using Projeto.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddCookie();
builder.Services.AddDistributedMemoryCache();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
});

var politicaCors = "politicaCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(politicaCors, builder => {
        builder.WithOrigins("https://github.com").AllowAnyMethod().AllowAnyHeader();
        builder.WithOrigins("https://api.github.com").AllowAnyMethod().AllowAnyHeader();
    });
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();
app.UseCors(politicaCors);

app.Run();
