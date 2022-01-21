namespace RecruitmentTool.WebApi.Models.Dtos.Skills
{
    using System.ComponentModel.DataAnnotations;

    using static RecruitmentTool.Common.GlobalConstants;

    public class CreateSkillDto
    {
        [Required]
        [StringLength(SkillNameMaxLength, MinimumLength = SkillNameMinLength)]
        public string Name { get; set; }
    }
}
