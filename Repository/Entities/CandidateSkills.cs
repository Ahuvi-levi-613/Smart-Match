using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class CandidateSkills
    {
        [Key]
        public int CandidateSkillsId { get; set; }

        [ForeignKey("Skills")]
        public int IdSkills { get; set; }
        public Skills Skills { get; set; }

        [ForeignKey("Candidate")]
        public int IdCandidate { get; set; }
        public Candidate Candidate { get; set; }

        public string Name { get; set; }
        public int SuccessRate { get; set; }
    }
}
