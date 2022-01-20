namespace RecruitmentTool.WebApi.Models.Dtos.Candidate
{
    using RecruitmentTool.WebApi.Models.Dtos.Recruiter;
    using RecruitmentTool.WebApi.Models.Dtos.Skills;

    public class CandidateDto
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public DateTime Birthday { get; set; }

        public RecruiterDto Recruiter { get; set; }

        public ICollection<SkillDto> Skills { get; set; }
    }
}
