using Common.Dto;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Service.Interfaces;
using Service.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.servicess
{
    public static class ExtentionService
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //IServiceCollection---ממשק בנוי 
            services.AddRepository();
            services.AddScoped<IService<ManagerDto>, ManagerService>();
            services.AddScoped<IService<CandidateDto>, CandidateService>();
            services.AddScoped<IService<JobDto>, JobService>();
            services.AddScoped<IService<RequirementsDto>, RequirementsService>();
            services.AddScoped<IService<SkillsDto>, SkillsService>();
            services.AddScoped<IService<UserDto>, UserService>();
            services.AddScoped<ICandidateMatcherService, CandidateMatcherService>();

            services.AddAutoMapper(typeof(MyMapper));
            return services;
        }
    }
}
