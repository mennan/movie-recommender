using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MovieRecommender.Api.Helpers;
using MovieRecommender.Job;
using MovieRecommenderService.Extensions;

namespace MovieRecommender.Api
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
            services.ConfigureContext(Configuration);
            services.ConfigureRepository(Configuration);

            services.ConfigureAuthentication(Configuration);

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddScoped<ValidateModelAttribute>();
            
            services.AddHostedService<MovieSyncService>();

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Authorization"
                            },
                            Scheme = "apiKey",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(options => { options.RouteTemplate = "help/{documentName}/swagger.json"; });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/help/v1/swagger.json", "Movie Recommender API");
                options.RoutePrefix = "help";
            });

            app.UseHttpsRedirection();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Recommender API"); });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}