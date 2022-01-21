namespace RecruitmentTool.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static RecruitmentTool.Common.GlobalConstants;

    public class Job
    {
        public Job()
        {
            Skills = new HashSet<Skill>();
            Interviews = new HashSet<Interview>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(JobTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(JobDescriptionMaxLength)]
        public string Description { get; set; }

        public decimal Salary { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; }
    }
}
