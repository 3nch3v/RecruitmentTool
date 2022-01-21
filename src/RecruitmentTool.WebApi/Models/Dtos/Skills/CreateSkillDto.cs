namespace RecruitmentTool.WebApi.Models.Dtos.Skills
{
    using System.ComponentModel.DataAnnotations;

    public class CreateSkillDto
    {
        [Required]
        public string Name { get; set; }
    }
}
