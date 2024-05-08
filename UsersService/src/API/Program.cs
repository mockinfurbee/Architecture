using API.MySwagger;
using Application.Extensions;
using Asp.Versioning;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Extensions;
using System.Text;

namespace UsersService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourIssuer",
                    ValidAudience = "yourAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey")),
                    ClockSkew = TimeSpan.Zero // Optional - adjust as needed
                };
            }); ////////////////

            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer();
            builder.Services.AddPersistenceLayer(builder.Configuration);

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.OperationFilter<SwaggerDefaults>();

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() // just for.
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT-token in format: \"Bearer myToken\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                //swagger.IncludeXmlComments(filename);
            });

            builder.Services.AddApiVersioning(api =>
            {
                api.DefaultApiVersion = new ApiVersion(1.0);
                api.ReportApiVersions = true;
            })
            .AddApiExplorer(api =>
            {
                api.GroupNameFormat = "'v'VVV";
                api.AssumeDefaultVersionWhenUnspecified = true;
                api.SubstituteApiVersionInUrl = true;
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
            {
                var specifications = app.DescribeApiVersions();
                foreach (var spec in specifications)
                {
                    var subpath = $"/swagger/{spec.GroupName}/swagger.json";
                    var title = spec.GroupName.ToUpperInvariant();
                    swagger.SwaggerEndpoint(subpath, title);
                }
            });

            app.Run();
        }
    }
}