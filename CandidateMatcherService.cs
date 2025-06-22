using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Dto;
using Repository.Entities;

namespace CandidateMatcher
{
    public class CandidateMatcherService
    {
        private readonly List<Job> _jobs;
        private readonly List<Candidate> _candidates;

        public CandidateMatcherService(List<Job> jobs, List<Candidate> candidates)
        {
            _jobs = jobs;
            _candidates = candidates;
        }

        public async Task<List<JobMatch>> MatchCandidatesToJobsAsync()
        {
            var jobMatches = new List<JobMatch>();

            foreach (var job in _jobs)
            {
                var matchedCandidates = new List<CandidateMatch>();

                foreach (var candidate in _candidates)
                {
                    var score = CalculateScore(job, candidate);
                    if (score >= job.PassingScore)
                    {
                        matchedCandidates.Add(new CandidateMatch
                        {
                            Candidate = candidate,
                            Score = score
                        });
                    }
                }

                jobMatches.Add(new JobMatch
                {
                    Job = job,
                    MatchedCandidates = matchedCandidates
                });
            }

            return await Task.FromResult(jobMatches);
        }

        private int CalculateScore(Job job, Candidate candidate)
        {
            int score = 0;

            // Check English level
            if (candidate.EnglishLevel >= job.EnglishLevel)
            {
                score += 100; // Base score for English level
            }

            // Check skills
            foreach (var skill in job.ListSkills)
            {
                var candidateSkill = candidate.ListSkills.FirstOrDefault(s => s.Name == skill.Name);
                if (candidateSkill != null)
                {
                    score += candidateSkill.Mark * 10; // Score based on skill mark
                }
            }

            // Check requirements
            foreach (var requirement in job.ListRequirement)
            {
                if (candidate.ListRequirement.Any(r => r.RequirementId == requirement.RequirementId))
                {
                    score += requirement.AdvantageOrMust == eAdvanOrMust.Must ? 50 : 25; // Score based on requirement type
                }
            }

            return score;
        }
    }

    public class JobMatch
    {
        public Job Job { get; set; }
        public List<CandidateMatch> MatchedCandidates { get; set; }
    }

    public class CandidateMatch
    {
        public Candidate Candidate { get; set; }
        public int Score { get; set; }
    }
}
