using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HungarianAlgorithm;
using Service.Interfaces;
using AutoMapper;
using Common.Dto;

namespace Service.services
{
    public class CandidateMatcherService : ICandidateMatcherService
    {
        private readonly IService<JobDto> _jobService;
        private readonly IService<CandidateDto> _candidateService;
        private readonly IMapper _mapper;

        public CandidateMatcherService(IService<JobDto> jobService, IService<CandidateDto> candidateService, IMapper mapper)
        {
            _jobService = jobService;
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public async Task<List<JobMatchesDto>> MatchCandidatesToJobsAsync()
        {
            // קבלת רשימות משרות ומועמדים מהשירותים
            var jobDtos = await _jobService.GetAll();
            var candidateDtos = await _candidateService.GetAll();

            // המרה לאובייקטים מהרפו (entities)
            var jobs = _mapper.Map<List<Job>>(jobDtos);
            var candidates = _mapper.Map<List<Candidate>>(candidateDtos);

            // שכפול משרות לפי כמות המועמדים הנדרשת
            var jobInstances = DuplicateJobsByNumCandidate(jobs);

            int rows = jobInstances.Count;
            int cols = candidates.Count;
             double[,] costMatrix = new double[rows, cols];

            // חישוב מטריצת אי-התאמה
            for (int i = 0; i < rows; i++)
            {
                var jobInst = jobInstances[i];
                for (int j = 0; j < cols; j++)
                {
                    var candidate = candidates[j];
                    costMatrix[i, j] = CalculateMatchPercentage(jobInst.OriginalJob, candidate);
                }
            }
            Console.WriteLine("=== Cost Matrix (Mismatch Score) ===");
            Console.Write("      ");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"C{j:D2}   ");
            }
            Console.WriteLine();

            for (int i = 0; i < rows; i++)
            {
                Console.Write($"J{i:D2} | ");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{costMatrix[i, j],-5} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("====================================");


            // הפעלת אלגוריתם הונגרי למציאת ההקצאה הטובה ביותר
            int[] assignment = HungarianAlgorithm.HungarianAlgorithm.FindAssignments(costMatrix);

            var jobMatches = new Dictionary<int, List<CandidateMatchDto>>();
            var assignedCandidates = new HashSet<int>();

            // עיבוד התוצאות
            for (int i = 0; i < assignment.Length; i++)
            {
                int candidateIndex = assignment[i];
                if (candidateIndex < 0 || candidateIndex >= cols)
                    continue;

                if (assignedCandidates.Contains(candidateIndex))
                    continue;

                assignedCandidates.Add(candidateIndex);

                int jobId = jobInstances[i].OriginalJob.JobId;
                var candidate = candidates[candidateIndex];
                double score = costMatrix[i, candidateIndex];

                if (!jobMatches.ContainsKey(jobId))
                    jobMatches[jobId] = new List<CandidateMatchDto>();

                jobMatches[jobId].Add(new CandidateMatchDto
                {
                    Candidate = candidate,
                    Score = score
                });
            }

            var result = new List<JobMatchesDto>();

            foreach (var job in jobs)
            {
                jobMatches.TryGetValue(job.JobId, out var matchedCandidates);
                matchedCandidates ??= new List<CandidateMatchDto>();

                matchedCandidates = matchedCandidates.OrderBy(c => c.Score).ToList();

                result.Add(new JobMatchesDto
                {
                    Job = job,
                    MatchedCandidates = matchedCandidates
                });
            }

            return result;
        }

        private List<JobInstance> DuplicateJobsByNumCandidate(List<Job> jobs)
        {
            var list = new List<JobInstance>();
            foreach (var job in jobs)
            {
                int count = Math.Max(1, job.NumCandidate);
                for (int i = 1; i <= count; i++)
                {
                    list.Add(new JobInstance { OriginalJob = job, InstanceNumber = i });
                }
            }
            return list;
        }

        //private int CalculateMismatchScore(Job job, Candidate candidate)
        //{
        //    //int score = 0;

            //var mustRequirementIds = job.ListRequirement
            //    .Where(r => r.AdvantageOrMust == eAdvanOrMust.Must)
            //    .Select(r => r.RequirementId)
            //    .ToHashSet();

            //var candidateRequirementIds = candidate.ListRequirement.Select(r => r.RequirementId).ToHashSet();

            //foreach (var reqId in mustRequirementIds)
            //{
            //    if (!candidateRequirementIds.Contains(reqId))
            //        score += 1000;
            //}

            //var advantageRequirementIds = job.ListRequirement
            //    .Where(r => r.AdvantageOrMust == eAdvanOrMust.Advantage)
            //    .Select(r => r.RequirementId)
            //    .ToHashSet();

            //foreach (var reqId in advantageRequirementIds)
            //{
            //    if (candidateRequirementIds.Contains(reqId))
            //        score -= 50;
            //}

            //var jobSkills = job.ListSkills.ToDictionary(s => s.Name, s => s.Mark);
            //var candidateSkills = candidate.ListSkills.ToDictionary(s => s.Name, s => s.Mark);

            //foreach (var jobSkill in jobSkills)
            //{
            //    int candidateMark = candidateSkills.ContainsKey(jobSkill.Key) ? candidateSkills[jobSkill.Key] : 0;
            //    int diff = jobSkill.Value - candidateMark;
            //    if (diff > 0)
            //        score += diff * 10;
            //}

            //int englishDiff = (int)job.EnglishLevel - (int)candidate.EnglishLevel;
            //if (englishDiff > 0)
            //    score += englishDiff * 20;

            //int requiredExperience = 0; // אם יש שדה רלוונטי תעדכן כאן
            //int experienceDiff = requiredExperience - candidate.ExperienceYears;
            //if (experienceDiff > 0)
            //    score += experienceDiff * 15;

            //if (score > job.PassingScore)
            //    score += 10000;

            //return score;


        //}
        private double CalculateMatchPercentage(Job job, Candidate candidate)
        {
            double score = 0;

            // דרישות חובה – 20%
            var mustReqs = job.ListRequirement.Where(r => r.AdvantageOrMust == eAdvanOrMust.Must).ToList();
            if (mustReqs.Count > 0)
            {
                var candidateReqIds = candidate.ListRequirement.Select(r => r.RequirementId).ToHashSet();
                int matched = mustReqs.Count(r => candidateReqIds.Contains(r.RequirementId));
                score += (matched / (double)mustReqs.Count) * 20.0;
            }

            // דרישות יתרון – 10%
            var advantageReqs = job.ListRequirement.Where(r => r.AdvantageOrMust == eAdvanOrMust.Advantage).ToList();
            if (advantageReqs.Count > 0)
            {
                var candidateReqIds = candidate.ListRequirement.Select(r => r.RequirementId).ToHashSet();
                int matched = advantageReqs.Count(r => candidateReqIds.Contains(r.RequirementId));
                score += (matched / (double)advantageReqs.Count) * 10.0;
            }

            // כישורים – 30%
            var jobSkills = job.ListSkills;
            var candidateSkillsDict = candidate.ListSkills.ToDictionary(s => s.Name, s => s.Mark);

            double skillsScore = 0;
            foreach (var js in jobSkills)
            {
                candidateSkillsDict.TryGetValue(js.Name, out int candMark);
                double ratio = Math.Min(1.0, candMark / (double)js.Mark); // max 100%
                skillsScore += ratio;
            }

            if (jobSkills.Count > 0)
                score += (skillsScore / jobSkills.Count) * 30.0;

            // אנגלית – 15%
            if (candidate.EnglishLevel >= job.EnglishLevel)
                score += 15.0;
            else
                score += (int)candidate.EnglishLevel / (double)(int)job.EnglishLevel * 15.0;

            // ניסיון – 15%
            int requiredYears = 0;
            int experience = candidate.ExperienceYears;
            if (experience >= requiredYears)
                score += 15.0;
            else
                score += (experience / (double)requiredYears) * 15.0;

            // אזור – 10%
            if (string.Equals(job.Area?.Trim(), candidate.Area?.Trim(), StringComparison.OrdinalIgnoreCase))
                score += 10.0;

            return score; // אחוז שלם
        }

    }

    public class JobInstance
    {
        public Job OriginalJob { get; set; }
        public int InstanceNumber { get; set; }
    }

    public class JobMatchesDto
    {
        public Job Job { get; set; }
        public List<CandidateMatchDto> MatchedCandidates { get; set; }
    }

    public class CandidateMatchDto
    {
        public Candidate Candidate { get; set; }
        public double Score { get; set; }
    }
}

    // הערות:
    // מחלקה CandidateMatcherService אחראית על התאמת מועמדים למשרות באמצעות אלגוריתם הונגרי.
    // היא מקבלת רשימות של משרות ומועמדים, משכפלת משרות לפי כמות המועמדים הנדרשת,
    // בונה מטריצת אי־התאמה, מפעילה את אלגוריתם הונגרי ומחזירה רשימת התאמות מאורגנת.
    // מחלקות נוספות:
    //  JobInstance – כדי לנהל משרות משוכפלות מול מועמדים.

    //JobMatchesDto – כדי להחזיר תוצאה מאורגנת של התאמה בין משרות למועמדים.

    //CandidateMatchDto – כדי להחזיק מידע על מועמד ספציפי וציון ההתאמה שלו.

