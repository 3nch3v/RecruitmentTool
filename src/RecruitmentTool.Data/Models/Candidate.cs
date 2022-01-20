namespace RecruitmentTool.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static RecruitmentTool.Common.GlobalConstants;

    public class Candidate
    {
        public Candidate()
        {
            Id = Guid.NewGuid().ToString();
            Skills = new HashSet<Skill>();
            Interviews = new HashSet<Interview>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(BioMaxLength)]
        public string Bio { get; set; }

        public DateTime Birthday { get; set; }

        [Required]
        public string RecruiterId { get; set; }

        public virtual Recruiter Recruiter { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; }
    }
}
