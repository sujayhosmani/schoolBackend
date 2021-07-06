using jay.school.bussiness.Bussiness;
using jay.school.bussiness.Repository;
using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using jay.school.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace jay.school
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddCors(c =>  
            {  
                c.AddPolicy("AllowOrigin", options => {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });  
            }); 
            
            services.AddControllers();

            services.Configure <AppSettings>( Configuration.GetSection ("Appsettings")); 

            services.Configure<SchoolAppsettings>(Configuration);

            services.AddTransient<ISchoolService, SchoolBussiness>();

            services.AddTransient<ITeachersService, TeacherBussiness>();

            services.AddTransient<IFileDocService, FileDocBussiness>();

            services.AddTransient<ITimeTableService, TimeTableBusiness>();

            services.AddTransient<IAnnouncementService, AnnouncementBusiness>();

            services.AddTransient<IAssignmentService, AssignmentBusiness>();

            services.AddScoped<IMDBContext, SchoolMDBContext>(); 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    //Globar error handler
                    errorApp.Run(async context =>
                    {
                        var logger = context.RequestServices.GetService<ILogger<Startup>>();

                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                        context.Response.StatusCode = 500;

                        context.Response.ContentType = "application/json";

                        logger.LogError(exceptionHandlerPathFeature.Error, exceptionHandlerPathFeature.Error.Message, exceptionHandlerPathFeature.Path);

                        CustomResponse<object> response = new CustomResponse<object>(0,null,string.Concat(exceptionHandlerPathFeature.Error, 
                            exceptionHandlerPathFeature.Error.Message, exceptionHandlerPathFeature.Path));

                        var result = JsonSerializer.Serialize(response);

                        await context.Response.WriteAsync(result);
                    });
                });
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseCors("AllowOrigin");  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
