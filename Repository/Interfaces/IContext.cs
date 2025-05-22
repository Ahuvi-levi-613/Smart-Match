using Repository.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
public interface IContext
{

    DbSet<Candidate> Candidates { get; set; }
    DbSet<CandidateRequirement> CandidateRequirements { get; set; }
    DbSet<CandidateSkills> CandidateSkills { get; set; }
    DbSet<Job> Jobs { get; set; }
    DbSet<JobRequirement> JobRequirements { get; set; }
    DbSet<JobSkill> JobSkills { get; set; }
    DbSet<Manager> Managers { get; set; }
    DbSet<Requirements> Requirements { get; set; }
    DbSet<Skills> Skills { get; set; }

    void Save();
}
