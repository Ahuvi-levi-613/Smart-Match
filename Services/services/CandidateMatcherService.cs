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
            int[,] costMatrix = new int[rows, cols];
            Console.WriteLine("row:" + rows + "col:" + cols);

            // חישוב מטריצת אי-התאמה
            for (int i = 0; i < rows; i++)
            {
                var jobInst = jobInstances[i];
                for (int j = 0; j < cols; j++)
                {
                    var candidate = candidates[j];
                    costMatrix[i, j] = (int)CalculateMatchPercentage(jobInst.OriginalJob, candidate);
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


//using Repository.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using HungarianAlgorithm;
//using Service.Interfaces;
//using AutoMapper;
//using Common.Dto;

//namespace Service.services
//{
//    public class CandidateMatcherService : ICandidateMatcherService
//    {
//        private readonly IService<JobDto> _jobService;
//        private readonly IService<CandidateDto> _candidateService;
//        private readonly IMapper _mapper;

//        // מילון תפקידים לדוגמה - אפשר להרחיב ולהתאים
//        private readonly Dictionary<string, List<string>> _roleMapping = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
//        {
//            { "Full Stack Developer", new List<string> { "Full Stack", "Backend", "Frontend" } },
//            { "QA Engineer", new List<string> { "QA", "Tester" } },
//            { "DevOps Engineer", new List<string> { "DevOps", "SysAdmin" } }
//            // הוסף כאן לפי הצורך
//        };

//        public CandidateMatcherService(IService<JobDto> jobService, IService<CandidateDto> candidateService, IMapper mapper)
//        {
//            _jobService = jobService;
//            _candidateService = candidateService;
//            _mapper = mapper;
//        }

//        public async Task<List<JobMatchesDto>> MatchCandidatesToJobsAsync()
//        {
//            var jobDtos = await _jobService.GetAll();
//            var candidateDtos = await _candidateService.GetAll();

//            var jobs = _mapper.Map<List<Job>>(jobDtos);
//            var candidates = _mapper.Map<List<Candidate>>(candidateDtos);

//            var jobInstances = DuplicateJobsByNumCandidate(jobs);

//            int rows = jobInstances.Count;
//            int cols = candidates.Count;
//            int[,] costMatrix = new int[rows, cols];

//            for (int i = 0; i < rows; i++)
//            {
//                var jobInst = jobInstances[i];
//                for (int j = 0; j < cols; j++)
//                {
//                    var candidate = candidates[j];

//                    double baseScore = CalculateMatchPercentage(jobInst.OriginalJob, candidate);

//                    // הורדת ניקוד אם התפקיד לא תואם (לא פסילה אלא הורדה של 30 נקודות לדוגמה)
//                    if (!IsRoleMatch(jobInst.OriginalJob.Title, candidate.Role))
//                    {
//                        baseScore -= 30;
//                        if (baseScore < 0) baseScore = 0;
//                    }

//                    // נקודת עלות = 100 - ניקוד התאמה (ככל שהעלות נמוכה יותר, ההתאמה טובה יותר)
//                    costMatrix[i, j] = 100 - (int)baseScore;
//                }
//            }

//            // הפוך למטריצה ריבועית
//            int[,] squareMatrix = MakeSquareCostMatrix(costMatrix, rows, cols);

//            // הרץ את האלגוריתם ההונגרי
//            int[] assignment = HungarianAlgorithm.HungarianAlgorithm.FindAssignments(squareMatrix);

//            var jobMatches = new Dictionary<int, List<CandidateMatchDto>>();
//            var assignedCandidates = new HashSet<int>();

//            for (int i = 0; i < assignment.Length; i++)
//            {
//                int candidateIndex = assignment[i];

//                if (i >= rows || candidateIndex < 0 || candidateIndex >= cols)
//                    continue;

//                if (assignedCandidates.Contains(candidateIndex))
//                    continue;

//                assignedCandidates.Add(candidateIndex);

//                int jobId = jobInstances[i].OriginalJob.JobId;
//                var candidate = candidates[candidateIndex];
//                double score = 100 - squareMatrix[i, candidateIndex];

//                // סינון לפי PassingScore
//                if (score < jobInstances[i].OriginalJob.PassingScore)
//                    continue;

//                if (!jobMatches.ContainsKey(jobId))
//                    jobMatches[jobId] = new List<CandidateMatchDto>();

//                jobMatches[jobId].Add(new CandidateMatchDto
//                {
//                    Candidate = candidate,
//                    Score = score
//                });
//            }

//            var result = new List<JobMatchesDto>();

//            foreach (var job in jobs)
//            {
//                jobMatches.TryGetValue(job.JobId, out var matchedCandidates);
//                matchedCandidates ??= new List<CandidateMatchDto>();

//                matchedCandidates = matchedCandidates.OrderByDescending(c => c.Score).ToList();

//                result.Add(new JobMatchesDto
//                {
//                    Job = job,
//                    MatchedCandidates = matchedCandidates
//                });
//            }

//            return result;
//        }

//        private bool IsRoleMatch(string jobTitle, string candidateRole)
//        {
//            if (string.IsNullOrWhiteSpace(jobTitle) || string.IsNullOrWhiteSpace(candidateRole))
//                return false;

//            if (_roleMapping.TryGetValue(jobTitle.Trim(), out var acceptedRoles))
//            {
//                return acceptedRoles.Any(r => string.Equals(r, candidateRole.Trim(), StringComparison.OrdinalIgnoreCase));
//            }
//            // אם אין מיפוי, בדיקה פשוטה לפי שוויון מחרוזות חלקי (אפשר לשפר)
//            return jobTitle.IndexOf(candidateRole, StringComparison.OrdinalIgnoreCase) >= 0;
//        }

//        private List<JobInstance> DuplicateJobsByNumCandidate(List<Job> jobs)
//        {
//            var list = new List<JobInstance>();
//            foreach (var job in jobs)
//            {
//                int count = Math.Max(1, job.NumCandidate);
//                for (int i = 1; i <= count; i++)
//                {
//                    list.Add(new JobInstance { OriginalJob = job, InstanceNumber = i });
//                }
//            }
//            return list;
//        }

//        private int[,] MakeSquareCostMatrix(int[,] originalMatrix, int rows, int cols, int highCost = 9999)
//        {
//            int size = Math.Max(rows, cols);
//            int[,] squareMatrix = new int[size, size];

//            for (int i = 0; i < size; i++)
//            {
//                for (int j = 0; j < size; j++)
//                {
//                    if (i < rows && j < cols)
//                        squareMatrix[i, j] = originalMatrix[i, j];
//                    else
//                        squareMatrix[i, j] = highCost;
//                }
//            }

//            return squareMatrix;
//        }

//        private double CalculateMatchPercentage(Job job, Candidate candidate)
//        {
//            double score = 0;

//            var mustReqs = job.ListRequirement.Where(r => r.AdvantageOrMust == eAdvanOrMust.Must).ToList();
//            var candidateReqIds = candidate.ListRequirement.Select(r => r.RequirementId).ToHashSet();

//            if (mustReqs.Count > 0)
//            {
//                int matched = mustReqs.Count(r => candidateReqIds.Contains(r.RequirementId));
//                if (matched == 0)
//                    return 0; // פסילה מיידית - אין דרישות חובה כלל
//                score += (matched / (double)mustReqs.Count) * 20.0;
//            }

//            var advantageReqs = job.ListRequirement.Where(r => r.AdvantageOrMust == eAdvanOrMust.Advantage).ToList();
//            if (advantageReqs.Count > 0)
//            {
//                int matched = advantageReqs.Count(r => candidateReqIds.Contains(r.RequirementId));
//                score += (matched / (double)advantageReqs.Count) * 10.0;
//            }

//            var jobSkills = job.ListSkills;
//            var candidateSkillsDict = candidate.ListSkills.ToDictionary(s => s.Name, s => s.Mark);

//            double skillsScore = 0;
//            foreach (var js in jobSkills)
//            {
//                candidateSkillsDict.TryGetValue(js.Name, out int candMark);
//                double ratio = candMark == 0 ? 0 : Math.Min(1.0, candMark / (double)js.Mark);
//                skillsScore += ratio;
//            }

//            if (jobSkills.Count > 0)
//                score += (skillsScore / jobSkills.Count) * 30.0;

//            if (candidate.EnglishLevel >= job.EnglishLevel)
//                score += 15.0;
//            else
//                score += ((int)candidate.EnglishLevel / (double)(int)job.EnglishLevel) * 15.0;

//            int requiredYears = 2;
//            int experience = candidate.ExperienceYears;
//            if (requiredYears > 0)
//            {
//                if (experience >= requiredYears)
//                    score += 15.0;
//                else
//                    score += (experience / (double)requiredYears) * 15.0;
//            }
//            else
//            {
//                score += 15.0;
//            }

//            if (!string.IsNullOrWhiteSpace(job.Area) && !string.IsNullOrWhiteSpace(candidate.Area))
//            {
//                if (string.Equals(job.Area.Trim(), candidate.Area.Trim(), StringComparison.OrdinalIgnoreCase))
//                    score += 10.0;
//            }

//            return Math.Round(score, 2);
//        }

//        public class JobInstance
//        {
//            public Job OriginalJob { get; set; }
//            public int InstanceNumber { get; set; }
//        }

//        public class JobMatchesDto
//        {
//            public Job Job { get; set; }
//            public List<CandidateMatchDto> MatchedCandidates { get; set; }
//        }

//        public class CandidateMatchDto
//        {
//            public Candidate Candidate { get; set; }
//            public double Score { get; set; }
//        }
//    }
//}
