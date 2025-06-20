using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{


    public class JobDto
    {
        public int JobId { get; set; }

        [Required(ErrorMessage = "ManagerId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ManagerId must be positive")]
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [MinLength(2, ErrorMessage = "Area must be at least 2 characters long")]
        public string Area { get; set; }

        [Required(ErrorMessage = "EnglishLevel is required")]
        [EnumDataType(typeof(eEnglishLevel), ErrorMessage = "Invalid English level")]
        public eEnglishLevel EnglishLevel { get; set; }

        [Range(0, 100, ErrorMessage = "PassingScore must be between 0 and 100")]
        public int PassingScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "NumCandidate cannot be negative")]
        public int NumCandidate { get; set; }

        public List<RequirementsDto> ListRequirement { get; set; } = new List<RequirementsDto>();

        public List<SkillsDto> ListSkills { get; set; } = new List<SkillsDto>();
    }
}
