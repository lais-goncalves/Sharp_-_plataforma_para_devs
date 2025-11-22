var builder = WebApplication.CreateBuilder(args);

// Add services to the conexaoiner.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddCookie();
builder.Services.AddDistributedMemoryCache();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var politicaCors = "politicaCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(politicaCors, builder => {
        builder.WithOrigins("https://github.com").AllowAnyMethod().AllowAnyHeader();
        builder.WithOrigins("https://api.github.com").AllowAnyMethod().AllowAnyHeader();
        builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        builder.WithOrigins("https://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();
app.UseCors(politicaCors);
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
