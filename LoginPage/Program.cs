using EyeWearProduct.Data;
using Microsoft.EntityFrameworkCore;
//using Scalar.AspNetCore;
using EyeWearProduct.Interface;
using EyeWearProduct.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;


var builder = WebApplication.CreateBuilder(args);

//SET CLoudinary credentials
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
//Cloudinary cloud = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
//cloud.Api.Secure = true;

var account = new Account
    (
    Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
    Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
    Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
    );

var cloud = new Cloudinary (account);
cloud.Api.Secure = true;
builder.Services.AddSingleton(cloud);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(
       builder.Configuration.GetConnectionString("DConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IProducts, ProductService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!
                )
            ),
            ClockSkew = TimeSpan.Zero
        }; 
    });
builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

// Configure the HTTP request pipeline.dw

//app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseCors("AllowReact");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
