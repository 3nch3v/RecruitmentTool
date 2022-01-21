namespace RecruitmentTool.WebApi.Models.Dtos.Job
{
    using System.ComponentModel.DataAnnotations;

    using RecruitmentTool.WebApi.Infrastructure.ValidationAttributes;
    using RecruitmentTool.WebApi.Models.Dtos.Skills;

    using static RecruitmentTool.Common.GlobalConstants;

    public class CreateJobDto
    {
        [Required]
        [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(JobDescriptionMaxLength, MinimumLength = JobDescriptionMinLength)]
        public string Description { get; set; }

        [Range(0, Double.MaxValue)]
        public decimal Salary { get; set; }

        [SkillsCount]
        public virtual ICollection<CreateSkillDto> Skills { get; set; }
    }
}
