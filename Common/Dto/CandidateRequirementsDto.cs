using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    using System;

    namespace Repository.Dto
    {
        public class CandidateRequirementDto
        {
            public int CandidateReqId { get; set; }

            public int IdCandidate { get; set; }
            public string CandidateName { get; set; } 

            public int IdRequirement { get; set; }
            public string RequirementName { get; set; } // שם הדרישה

            public string AdvantageOrMust { get; set; } // 'Must' או 'Advantage'
        }
    }

}
