using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public enum eAdvanOrMust
    {
        Advantage, Must
    }

    public class Requirements
    {
        [Key]
        public int RequirementId { get; set; }
        public string Description { get; set; }
        public eAdvanOrMust AdvantageOrMust { get; set; }
        public virtual List<Job> ListJob { get; set; } 
        public virtual List<Candidate> ListCandidate { get; set; } 

    }

}
