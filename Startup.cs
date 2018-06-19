using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using WeatherAPI.Models;

namespace WeatherAPI
{
    #region Startup
    public class Startup
    {
        public IConfiguration AppConfiguration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            AppConfiguration = configuration;
        }
        #endregion

        #region snippet_ConfigureServices
        public void ConfigureServices(IServiceCollection services)
        {
            // Register configuration
            services.AddOptions();
            services.Configure<WeatherAPISettings>(AppConfiguration.GetSection("security"));

            services.AddMvc();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "Weather API",
                        Description = "Weather API"
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = "WeatherAPI.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.DescribeAllEnumsAsStrings();
            });
        }
        #endregion

        #region snippet_Configure
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API V1");
                c.RoutePrefix = string.Empty;
            });
        }
        #endregion
    }
}
