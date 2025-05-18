using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Repository.Entities;


namespace Common.Dto
{
    public class JobDto
    {
        public int JobId { get; set; }
        public int ManagerId { get; set; }
        //public string ManagerName { get; set; } // שם המנהל
        public string Title { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string EnglishLevel { get; set; } // רמת האנגלית

        public int PassingScore { get; set; }
        public int NumCandidate { get; set; }
      //  public List<JobRequirementDto> ListRequirement { get; set; }
       // public List<JobSkillDto> ListSkills { get; set; }
 
    }
}
