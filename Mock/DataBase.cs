using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;

namespace Mock
{
    public class SmartMatchDbContext : DbContext, IContext

    {
        // DbSets
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Requirements> Requirements { get; set; }
        public DbSet<Skills> Skills { get; set; }

        public DbSet<User> Users { get; set; }
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

            //// קשר Job 1—* JobRequirements
            //modelBuilder.Entity<Requirements>()
            //    .HasOne(jr => jr.ListJob)
            //    .WithMany(j => j.)
            //    .HasForeignKey(jr => jr.IdJob)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// קשר Requirement 1—* JobRequirements
            //modelBuilder.Entity<Requirements>()
            //    .HasOne(jr => jr.Requirement)
            //    .WithMany() // אם אין ICollection< JobRequirement > ב-Requirement
            //    .HasForeignKey(jr => jr.IdRequirement)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// קשר Job 1—* JobSkills
            //modelBuilder.Entity<Skills>()
            //    .HasOne(js => js.Job)
            //    .WithMany(j => j.ListSkills)
            //    .HasForeignKey(js => js.IdJob)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// קשר Skills 1—* JobSkills
            //modelBuilder.Entity<Skills>()
            //    .HasOne(js => js.Skills)
            //    .WithMany() // אם אין ICollection< JobSkills > ב-Skills
            //    .HasForeignKey(js => js.IdSkills)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// קשר Candidate 1—* CandidateRequirements
            //modelBuilder.Entity<Requirements>()
            //    .HasOne(cr => cr.Candidate)
            //    .WithMany(c => c.ListCandidateRequirement)
            //    .HasForeignKey(cr => cr.IdCandidate)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// קשר Requirement 1—* CandidateRequirements
            //modelBuilder.Entity<Requirements>()
            //    .HasOne(cr => cr.Requirement)
            //    .WithMany() // אם אין ICollection< CandidateRequirement > ב-Requirement
            //    .HasForeignKey(cr => cr.IdRequirement)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// קשר Candidate 1—* CandidateSkills
            //modelBuilder.Entity<Skills>()
            //    .HasOne(cs => cs.Candidate)
            //    .WithMany(c => c.ListSkills)
            //    .HasForeignKey(cs => cs.IdCandidate)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// קשר Skills 1—* CandidateSkills
            //modelBuilder.Entity<Skills>()
            //    .HasOne(cs => cs.Skills)
            //    .WithMany() // אם אין ICollection< CandidateSkills > ב-Skills
            //    .HasForeignKey(cs => cs.IdSkills)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
        public async Task Save()
        {
            await this.SaveChangesAsync();
        }

    }
}
