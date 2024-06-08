using API.HealthCheck;
using API.Middlewares;
using API.MySwagger;
using Application.Extensions;
using Asp.Versioning;
using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Persistence.Contexts;
using Persistence.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace UsersService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                    ValidIssuer = builder.Configuration.GetValue<string>("TokenValidationParams:ValidIssuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("TokenValidationParams:ValidAudience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenValidationParams:SecretKey"))),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer();
            builder.Services.AddPersistenceLayer(builder.Configuration);

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.OperationFilter<SwaggerDefaults>();
                swagger.SupportNonNullableReferenceTypes();
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT-token in format: \"Bearer myTokenFullValue\"",
                });
                swagger.UseAllOfToExtendReferenceSchemas();
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

                var myAppAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName().Name).ToList();
                var xmlFilesPath = myAppAssemblies.Select(x => $"{x}.xml").ToList();
                foreach (var path in xmlFilesPath)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, path);
                    if (File.Exists(xmlPath))
                    {
                        swagger.IncludeXmlComments(xmlPath);
                    }
                }
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

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, MySwaggerOptions>();

            builder.Services.AddHealthChecks()
                            .AddDbContextCheck<DataContext>()
                            .AddCheck<ExampleHealthCheck>(name: "IsKaboomHC");

            var app = builder.Build();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandler>();

            using (var scope = app.Services.CreateScope()) // Init db w/ roles and admin if db clean.
            {
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await DbSeeder.InitializeAsync(userManager, rolesManager, builder.Configuration);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

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

            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description

                        }),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });

            app.Run();
        }
    }
}