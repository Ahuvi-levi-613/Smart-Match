using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Repository.Entities
{
    public enum eEnglishLevel
    {
        Basic,
        Intermediate,
        Fluent,
        Native
    }

    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
        public virtual Manager Manager { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Area {  get; set; }
        public eEnglishLevel EnglishLevel { get; set; }

        public virtual List<JobRequirement> ListRequirement { get; set; }
        public virtual List<JobSkill> ListSkills { get; set; }
        public int PassingScore { get; set; }
        public int NumCandidate { get; set; }
    }
}
