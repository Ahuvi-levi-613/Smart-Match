﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{

    public class CandidateDto
    {
        public int CandidateId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ExperienceYears { get; set; }
        public string Role { get; set; }
        public string Area { get; set; }
        public string EnglishLevel { get; set; }

        public List<RequirementsDto> ListRequirement { get; set; }
        public List<SkillsDto> ListSkills { get; set; }
    }

}


