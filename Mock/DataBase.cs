using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;

namespace Mock
{ 
    public class SmartMatchDbContext :DbContext,IContext

    {
        // DbSets
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Requirements> Requirements { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<JobRequirement> JobRequirements { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<CandidateRequirement> CandidateRequirements { get; set; }
        public DbSet<CandidateSkills> CandidateSkills { get; set; }

        // הגדרת חיבור למסד הנתונים
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=Smart-MatchDB;trusted_connection=true");
        }

        // קונפיגורציה של קשרים בין טבלאות
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // קשר Manager 1—* Jobs
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Manager)
                .WithMany(m => m.Jobs)
                .HasForeignKey(j => j.ManagerId)
                .OnDelete(DeleteBehavior.Cascade);

            // קשר Job 1—* JobRequirements
            modelBuilder.Entity<JobRequirement>()
                .HasOne(jr => jr.Job)
                .WithMany(j => j.ListRequirement)
                .HasForeignKey(jr => jr.IdJob)
                .OnDelete(DeleteBehavior.Cascade);

            // קשר Requirement 1—* JobRequirements
            modelBuilder.Entity<JobRequirement>()
                .HasOne(jr => jr.Requirement)
                .WithMany() // אם אין ICollection< JobRequirement > ב-Requirement
                .HasForeignKey(jr => jr.IdRequirement)
                .OnDelete(DeleteBehavior.Restrict);

            // קשר Job 1—* JobSkills
            modelBuilder.Entity<JobSkill>()
                .HasOne(js => js.Job)
                .WithMany(j => j.ListSkills)
                .HasForeignKey(js => js.IdJob)
                .OnDelete(DeleteBehavior.Cascade);

            // קשר Skills 1—* JobSkills
            modelBuilder.Entity<JobSkill>()
                .HasOne(js => js.Skills)
                .WithMany() // אם אין ICollection< JobSkills > ב-Skills
                .HasForeignKey(js => js.IdSkills)
                .OnDelete(DeleteBehavior.Restrict);

            // קשר Candidate 1—* CandidateRequirements
            modelBuilder.Entity<CandidateRequirement>()
                .HasOne(cr => cr.Candidate)
                .WithMany(c => c.ListCandidateRequirement)
                .HasForeignKey(cr => cr.IdCandidate)
                .OnDelete(DeleteBehavior.Cascade);

            // קשר Requirement 1—* CandidateRequirements
            modelBuilder.Entity<CandidateRequirement>()
                .HasOne(cr => cr.Requirement)
                .WithMany() // אם אין ICollection< CandidateRequirement > ב-Requirement
                .HasForeignKey(cr => cr.IdRequirement)
                .OnDelete(DeleteBehavior.Restrict);

            // קשר Candidate 1—* CandidateSkills
            modelBuilder.Entity<CandidateSkills>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.ListSkills)
                .HasForeignKey(cs => cs.IdCandidate)
                .OnDelete(DeleteBehavior.Cascade);

            // קשר Skills 1—* CandidateSkills
            modelBuilder.Entity<CandidateSkills>()
                .HasOne(cs => cs.Skills)
                .WithMany() // אם אין ICollection< CandidateSkills > ב-Skills
                .HasForeignKey(cs => cs.IdSkills)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public void Save()
        {
            this.SaveChanges();
        }

    }
}
