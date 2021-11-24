using System;
using System.Reflection;
using System.Text.Json;
using AttendanceStudent.Class.Interfaces;
using AttendanceStudent.Class.Repositories.Implements;
using AttendanceStudent.Class.Repositories.Interfaces;
using AttendanceStudent.Class.Services;
using AttendanceStudent.Commons.Filters;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Database;
using AttendanceStudent.RollCall.Interfaces;
using AttendanceStudent.RollCall.Repositories.Implements;
using AttendanceStudent.RollCall.Repositories.Interfaces;
using AttendanceStudent.RollCall.Services;
using AttendanceStudent.Subject.Interfaces;
using AttendanceStudent.Subject.Repositories.Implements;
using AttendanceStudent.Subject.Repositories.Interfaces;
using AttendanceStudent.Subject.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AttendanceStudent
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
            services.AddControllers(
                    options =>
                    {
                        // Generate ViewModel errors
                        options.Filters.Add<ViewModelValidationFilter>();
                    }
                )
                .AddFluentValidation(mvcConfiguration =>
                    {
                        // mvcConfiguration.RegisterValidatorsFromAssembly(typeof(AttendanceStudent.Startup)
                        //     .GetTypeInfo().Assembly);
                        mvcConfiguration.RegisterValidatorsFromAssemblyContaining(typeof(Startup));
                    }
                );
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "AttendanceStudent", Version = "v1"}); });
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string connectionString = Configuration.GetConnectionString("MySqlServerConnection");
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                options.UseMySql(connectionString, serverVersion)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IRollCallRepository, RollCallRepository>();
            services.AddScoped<IRollCallService, RollCallService>();
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddSingleton<IStringLocalizationService, StringLocalizationService>();
            services.AddLocalization(option => { option.ResourcesPath = "Resources"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AttendanceStudent v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}