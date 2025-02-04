using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using YssWebstoreApi.Formatters;
using YssWebstoreApi.Helpers;
using YssWebstoreApi.Installers;
using YssWebstoreApi.Services.Files;
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
                            builder.Configuration.GetSection("Security:JwtKey").Value!)),
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
            builder.Services.AddSingleton<IFilesystemService>(new PhysicalFileSystem(
                PathHelper.GetAbsolutePathRelativeToAssembly("static")));

            var app = builder.Build();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    PathHelper.GetAbsolutePathRelativeToAssembly("static")),
                RequestPath = "/static",
                EnableDirectoryBrowsing = app.Environment.IsDevelopment()
            });

            //app.UseHttpsRedirection();
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

            if (app.Configuration.GetValue<bool>("Swagger:Enabled"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
