using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CutomApiLib.Middlewares;
using System.IO;

namespace apigateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(config =>
       {
           config.Filters.Add(new CustomResponseResult());
       });
            var authenticationProviderKey = "TestKey";
            services.AddAuthentication(o =>
                    {
                        o.DefaultScheme =
                            JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(authenticationProviderKey, x =>
                {
                    x.Authority = "test";
                    x.Audience = "test";
                    x.RequireHttpsMetadata = false;
                });
            services.AddAuthorization();


            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionMiddleware();
            // app.UseMvc();
            // 啟用路由
            app.UseRouting();
            // 啟用驗證
            app.UseAuthentication();
            // 啟用授權
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var configuration = new OcelotPipelineConfiguration
            {
                PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    await next.Invoke();
                    System.Console.WriteLine(ctx.Response.StatusCode.ToString());             
                }
            };

            app.UseOcelot(configuration);

        }
    }
}
