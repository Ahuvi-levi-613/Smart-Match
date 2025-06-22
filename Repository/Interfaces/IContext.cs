using Repository.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
public interface IContext
{

    DbSet<Candidate> Candidates { get; set; }
    DbSet<Job> Jobs { get; set; }
    DbSet<Manager> Managers { get; set; }
    DbSet<Requirements> Requirements { get; set; }
    DbSet<Skills> Skills { get; set; }
    DbSet<User> Users { get; set; }
    Task Save();
}
