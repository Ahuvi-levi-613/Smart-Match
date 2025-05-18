using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class JobSkill
    {
        [Key]
        public int JobSkillsId { get; set; }

        [ForeignKey("Job")]
        public int IdJob { get; set; }
        public Job Job { get; set; }

        [ForeignKey("Skills")]
        public int IdSkills { get; set; }
        public Skills Skills { get; set; }
        public string Name { get; set; }
        public int Mark { get; set; }
    }


}
