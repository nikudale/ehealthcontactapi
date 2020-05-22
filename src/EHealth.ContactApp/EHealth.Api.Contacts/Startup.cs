using System;
using AutoMapper;
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
using EHealth.Api.Contacts.Services;
using EHealth.Api.Contacts.Infrastructure.Repositories;
using EHealth.Api.Contacts.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using EHealth.Api.Contacts.Mappings;
using EHealth.Api.Contacts.Filters;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace EvolentHealth.Api.Contacts
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
            services.AddControllers(config =>
            {
                //config.Filters.Add(new ValidateDomainModelStateFilter());
            });

            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultDB"));
            });


            services.AddScoped<DbContext, AppDBContext>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactRepository, ContactRepository>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ContactMapping());
            });

            services.AddSingleton(mappingConfig.CreateMapper());


            //services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //handling unhandled exception, this will move to middleware with custom response
            app.UseExceptionHandler(builder => builder.Run(async context =>
            {
                var excFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = excFeature.Error;

                var result = JsonConvert.SerializeObject(new { Message = exception.Message, IsSuccess = false });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
