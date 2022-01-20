namespace RecruitmentTool.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static RecruitmentTool.Common.GlobalConstants;

    public class Recruiter
    {
        public Recruiter()
        {
            Id = Guid.NewGuid().ToString();
            Interviews = new HashSet<Interview>(MaxInterviewsCount);
            Candidates = new HashSet<Candidate>();
            ExperienceLevel = 1;
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(CountryMaxLength)]
        public string Country { get; set; }

        public int ExperienceLevel { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
