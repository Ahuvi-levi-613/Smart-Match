using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Repository.Entities;
using Service.Interfaces;

namespace CandidateMatcher
{
   
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var jobs = new List<Job>
            {
                new Job
                {
                    JobId = 1,
                    Title = "Full Stack Developer",
                    NumCandidate = 2,
                    EnglishLevel = EnglishLevel.Medium,
                    PassingScore = 300,
                    ListRequirement = new List<Requirement>
                    {
                        new Requirement { RequirementId = 1, AdvantageOrMust = eAdvanOrMust.Must },
                        new Requirement { RequirementId = 2, AdvantageOrMust = eAdvanOrMust.Advantage }
                    },
                    ListSkills = new List<Skill>
                    {
                        new Skill { Name = "C#", Mark = 9 },
                        new Skill { Name = "React", Mark = 8 }
                    }
                }
            };

            var candidates = new List<Candidate>
            {
                new Candidate
                {
                    CandidateId = 1,
                    Name = "Alice",
                    EnglishLevel = EnglishLevel.High,
                    ExperienceYears = 2,
                    ListRequirement = new List<Requirement>
                    {
                        new Requirement { RequirementId = 1 },
                        new Requirement { RequirementId = 2 }
                    },
                    ListSkills = new List<Skill>
                    {
                        new Skill { Name = "C#", Mark = 8 },
                        new Skill { Name = "React", Mark = 6 }
                    }
                },
                new Candidate
                {
                    CandidateId = 2,
                    Name = "Bob",
                    EnglishLevel = EnglishLevel.Low,
                    ExperienceYears = 1,
                    ListRequirement = new List<Requirement>
                    {
                        new Requirement { RequirementId = 2 }
                    },
                    ListSkills = new List<Skill>
                    {
                        new Skill { Name = "C#", Mark = 5 },
                        new Skill { Name = "React", Mark = 4 }
                    }
                }
            };

            var matcher = new CandidateMatcherService(jobs, candidates);
            var results = await matcher.MatchCandidatesToJobsAsync();

            foreach (var match in results)
            {
                Console.WriteLine($"משרה: {match.Job.Title}");
                foreach (var candidateMatch in match.MatchedCandidates)
                {
                    Console.WriteLine($"  → מועמד: {candidateMatch.Candidate.Name}, ניקוד: {candidateMatch.Score}");
                }
            }

            Console.ReadLine();
        }
    }
}
