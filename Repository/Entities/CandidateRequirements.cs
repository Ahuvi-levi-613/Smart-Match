using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public enum eAdvantageOrMust
    {
        Must,
        Advantage
    }
    public class CandidateRequirement
    {
        [Key]
        public int CandidateReqId { get; set; }

        [ForeignKey("Candidate")]
        public int IdCandidate { get; set; }
        public Candidate Candidate { get; set; }

        [ForeignKey("Requirement")]
        public int IdRequirement { get; set; }
        public Requirements Requirement { get; set; }

        public eAdvantageOrMust AdvantageOrMust { get; set; }
    }

}
