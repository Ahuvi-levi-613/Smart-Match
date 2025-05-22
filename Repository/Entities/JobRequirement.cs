using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public enum eAdvanOrMust
    {
        Advantege,Must
    }
    public class JobRequirement
    {
        [Key]
        public int JobRequirementId { get; set; }

        [ForeignKey("Job")]
        public int IdJob { get; set; }
        public virtual Job Job { get; set; }

        [ForeignKey("Requirement")]
        public int IdRequirement { get; set; }
        public Requirements Requirement { get; set; }

        public eAdvanOrMust AdvantageOrMust { get; set; } 
    }
}
