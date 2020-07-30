using AutoMapper;
using BLL.Interfaces;
using BLL.MappingProfiles;
using BLL.Services;
using DAL.Interfaces;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProjectProfile>();
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<TeamProfile>();
                cfg.AddProfile<TaskProfile>();
            },
            Assembly.GetExecutingAssembly());
        }

        public static void ConfigureCustomValidationErrors(this IServiceCollection services)
        {
            // override modelstate
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
                    var result = new
                    {
                        Message = "Validation errors",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(result);
                };
            });
        }
    }
}
