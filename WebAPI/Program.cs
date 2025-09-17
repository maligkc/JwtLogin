using Business.Abstract;
using Business.Auth;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DBCONTEXT AYARLARI VE VER�TABANINA BA�LANMA (appsettings.json ile)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// repository ve servisler
builder.Services.AddScoped<IAuthService, UserManager>();
builder.Services.AddScoped<IEfUserRepository, EfUserRepositoryDal>();
builder.Services.AddSingleton(new JwtHelper(builder.Configuration["Jwt:Key"]));


// JWT Ayarlar�

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // TOKEN� K�M VERD� KONTORL ET
        ValidateAudience = true, // TOKEN K�ME VER�LD� KONTORL ET
        ValidateLifetime = true, // TOKEN S�RES� DOLMU� MU KONTORL ET
        ValidateIssuerSigningKey = true, // �MZA DO�RU MU KONTROL ET

        ValidIssuer = "JwtAuthProject", // TOKEN� YAYINLAYAN
        ValidAudience = "JwtAuthProject", // TOKEN� KULLANAN
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});




builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Swagger art�k root (/)'ta
    });
}
app.UseCors("AllowAll");
app.UseStaticFiles();


app.UseHttpsRedirection();



app.UseAuthentication(); // �NCE TOKEN DO�RULAMASI YAPILIR (Gelen request i�inde Bearer token var m� diye bakar.)
app.UseAuthorization(); // SONRA YETK� KONTROL� YAPILIR ([Authorize] attribute varsa token��n rol�n�/kimli�ini kontrol eder.)




app.MapControllers();

app.Run();
