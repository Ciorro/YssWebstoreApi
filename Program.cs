using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;
using YssWebstoreApi.Database;
using YssWebstoreApi.Formatters;
using YssWebstoreApi.Installers;
using YssWebstoreApi.Services.Jwt;

namespace YssWebstoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            builder.Services.AddScoped<IDbConnection>((services) =>
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var connectionStr = configuration.GetConnectionString("DefaultConnection")!;
                var connection = new MySqlConnection(connectionStr);

                connection.Open();
                return connection;
            });

            builder.Services.AddCors();
            builder.Services.AddHttpClient();
            builder.Services.AddControllers((config) =>
            {
                config.InputFormatters.Add(new PlainTextFormatter());
            });
            builder.Services.AddRepositories();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            builder.Configuration.GetSection("Jwt:Key").Value!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
            builder.Services.AddSingleton<ITokenService, TokenService>();

            var app = builder.Build();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Static")),
                RequestPath = "/static"
            });

            app.UseHttpsRedirection();
            app.UseCors(
                options => options
                .WithOrigins("http://localhost:3000")
                .WithOrigins("https://localhost:3000")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build()
            );
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseVerification();

            app.MapControllers();
            app.AddTagHandlers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
