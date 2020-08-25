using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using TradingPlatform.Database;
using TradingPlatform.Middleware;
using TradingPlatform.Models;

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