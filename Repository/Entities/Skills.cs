using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Skills
    {
        [Key]
        public int SkillsId { get; set; }
        public string Name { get; set; }
        public int Mark { get; set; }
        public virtual List<Job> ListJob { get; set; } 
        public virtual List<Candidate> ListCandidate { get; set; }


    }
}
