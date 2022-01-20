namespace RecruitmentTool.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static RecruitmentTool.Common.GlobalConstants;

    public class Skill
    {
        public Skill()
        {
            Candidates = new HashSet<Candidate>();
            Jobs = new HashSet<Job>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(SkillNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
