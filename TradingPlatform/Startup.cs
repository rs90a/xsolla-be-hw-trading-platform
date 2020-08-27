using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using TradingPlatform.Database;
using TradingPlatform.Interfaces;
using TradingPlatform.Middleware;
using TradingPlatform.Models;
using TradingPlatform.Services;

namespace TradingPlatform
{
    public class Startup
    {
        private const string ApiVersion = "v1";
        private const string SystemName = "Trading Platform API";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSection = Configuration.GetSection("JWT")
                             ?? throw new ArgumentException("JWT-константы не определены");
            var jwtConfig = new Jwt();
            jwtSection.Bind(jwtConfig);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, new OpenApiInfo
                {
                    Version = ApiVersion,
                    Title = SystemName,
                    Description = @"Реализация API системы ""Торговая площадка продажи игровых ключей"""
                });
            });

            //Внедрение зависимостей - сервисы
            services.AddSingleton(jwtConfig);
            services.AddSingleton<IAuth>(new JwtService(jwtConfig));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IKeystoreService, KeystoreService>();

            //Конфигурация БД
            ConfigureDatabase(services);
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<TradingPlatformDbContext>();

            //Обработка ошибки валидации модели
            services.Configure<ApiBehaviorOptions>(options =>
                options.InvalidModelStateResponseFactory = actionContext =>
                    new BadRequestObjectResult(new
                    {
                        error = new Error
                        {
                            Message = string.Join("; ", actionContext.ModelState.Values
                                .SelectMany(x => x.Errors)
                                .Select(x => x.ErrorMessage))
                        }
                    })
            );

            //Конфигурация аутентификации
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var jwtService = scope.ServiceProvider.GetRequiredService<IAuth>();
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidIssuer = jwtConfig.Issuer,
                            ValidAudience = jwtConfig.Audience,
                            ValidateLifetime = true,
                            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                                expires != null && DateTime.UtcNow < expires,
                            IssuerSigningKey = jwtService.GetSecurityKey(jwtConfig.Key)
                        };
                    }
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomExceptionHandler();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", SystemName);
                c.RoutePrefix = string.Empty;
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var dbSection = Configuration.GetSection("DBMain:Postgres")
                            ?? throw new ArgumentException("Не определена конфигурация БД");

            var dbConfig = new DbConfig();
            dbSection.Bind(dbConfig);

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = dbConfig.Host,
                Port = dbConfig.Port,
                Database = dbConfig.Database,
                Username = dbConfig.Username,
                Password = dbConfig.Password,
                SearchPath = "Public"
            };
            services.AddDbContext<TradingPlatformDbContext>(options =>
                options.UseNpgsql(builder.ConnectionString));
        }
    }
}