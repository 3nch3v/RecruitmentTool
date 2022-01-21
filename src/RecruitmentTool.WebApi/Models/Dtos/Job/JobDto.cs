namespace RecruitmentTool.WebApi.Models.Dtos.Job
{
    using RecruitmentTool.WebApi.Models.Dtos.Skills;

    public class JobDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Salary { get; set; }

        public int InterviewsCount { get; set; }

        public virtual ICollection<SkillDto> Skills { get; set; }
    }
}
