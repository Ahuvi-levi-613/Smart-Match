using Service.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.services.CandidateMatcherService;

namespace Service.Interfaces
{
    public interface ICandidateMatcherService
    {
        Task<List<JobMatchesDto>> MatchCandidatesToJobsAsync();
    }
}
