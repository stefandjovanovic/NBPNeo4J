using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NBPNeo4J.Repositories;
using NBPNeo4J.Services;
using Neo4j.Driver;


namespace NBPNeo4J
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
            //services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IHubRepository, HubRepository>();
            services.AddScoped<IHubService, HubService>();
            services.AddScoped<IServiceStationRepository, ServiceStationRepository>();
            services.AddScoped<IServiceStationService, ServiceStationService>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleService, VehicleService>();

            services.AddSingleton(GraphDatabase.Driver(
                Environment.GetEnvironmentVariable("NEO4J_URI"),
                AuthTokens.Basic(
                    Environment.GetEnvironmentVariable("NEO4J_USER"),
                    Environment.GetEnvironmentVariable("NEO4J_PASSWORD")
                )
            ));

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost4200",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowLocalhost4200");

            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}
