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
            Services.AddScoped<IService<ManagerDto>, ManagerService>();
            Services.AddScoped<IService<CandidateDto>, CandidateService>();
            Services.AddScoped<IService<JobDto>, JobService>();
            Services.AddScoped<IService<RequirementsDto>, RequirementsService>();
            Services.AddScoped<IService<SkillsDto>, SkillsService>();
            Services.AddAutoMapper(typeof(MyMapper));
            return services;
        }
    }
}
