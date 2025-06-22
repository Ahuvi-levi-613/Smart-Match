using Repository.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public enum eEnglishLevel
{
    Basic,
    Intermediate,
    Fluent,
    Native
}

public class Job
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobId { get; set; } // שימי לב לשם הזה

    [ForeignKey("Manager")]
    public int ManagerId { get; set; }
    public virtual Manager Manager { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Area { get; set; }
    public eEnglishLevel EnglishLevel { get; set; }

    public virtual List<Requirements> ListRequirement { get; set; }
    public virtual List<Skills> ListSkills { get; set; }
    public int PassingScore { get; set; }
    public int NumCandidate { get; set; }
}
