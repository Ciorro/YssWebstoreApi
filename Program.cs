using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Text;
using YssWebstoreApi.Api.Formatters;
using YssWebstoreApi.Persistance;
using YssWebstoreApi.Setup;

namespace YssWebstoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

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

            builder.Services.AddTransient<IDatabaseInitializer>(
                _ => new DatabaseInitializer(config.GetConnectionString("DefaultConnection")!));
            builder.Services.AddScoped<IDbConnection>(
                _ => new NpgsqlConnection(config.GetConnectionString("DefaultConnection")!));

            builder.Services.AddCors();
            builder.Services.AddHttpClient();
            builder.Services.AddControllers((config) =>
            {
                config.InputFormatters.Add(new PlainTextFormatter());
            });
            builder.Services.AddRepositories();
            builder.Services.AddServices();

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

            builder.Services.AddLiteBus(x =>
            {
                x.AddCommandModule(builder =>
                {
                    builder.RegisterFromAssembly(typeof(Program).Assembly);
                });

                x.AddQueryModule(builder =>
                {
                    builder.RegisterFromAssembly(typeof(Program).Assembly);
                });
            });

            var app = builder.Build();

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

            if (app.Configuration.GetValue<bool>("Swagger:Enabled"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.EnableTryItOutByDefault();
                });
            }

            app.InitDatabase();
            app.Run();
        }
    }
}
