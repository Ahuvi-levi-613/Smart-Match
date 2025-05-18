using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
   
        public class Candidate
        {
            [Key]
            public int CandidateId { get; set; }

            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public int ExperienceYears { get; set; }
            public string Role { get; set; }
            public string Area {  get; set; }
            public eEnglishLevel EnglishLevel { get; set; }

            public virtual List<CandidateRequirement> ListCandidateRequirement { get; set; }
            public virtual List<CandidateSkills> ListSkills { get; set; }
        }
    }

