using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public static class ExtentionRepository
    {
        //מנהל תלויות : * יוצר מופע למחלקה
        // מזריק אותה למקום המתאים *
        //פונקציה להגדרת התלויות
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {

            //יוצר מופע אחד עבור כל גולש ומזריק אותו בכל פעם
            Services.AddScoped<IRepository<Manager>, ManagerRepository>();
            Services.AddScoped<IRepository<Candidate>, CandidateRepository>();
            Services.AddScoped<IRepository<Job>, JobRepository>();
            Services.AddScoped<IRepository<Requirements>, RequirementsRepository>();
            Services.AddScoped<IRepository<Skills>, SkillsRepository>();


            return services;
        }
    }
}
