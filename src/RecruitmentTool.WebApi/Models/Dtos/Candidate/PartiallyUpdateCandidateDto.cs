namespace RecruitmentTool.WebApi.Models.Dtos.Candidate
{
    using System.ComponentModel.DataAnnotations;

    using RecruitmentTool.WebApi.Infrastructure.ValidationAttributes;
    using RecruitmentTool.WebApi.Models.Dtos.Recruiter;
    using RecruitmentTool.WebApi.Models.Dtos.Skills;

    using static RecruitmentTool.Common.GlobalConstants;

    public class PartiallyUpdateCandidateDto
    {
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; }

        [StringLength(BioMaxLength, MinimumLength = BioMinLength)]
        public string Bio { get; set; }

        [BirthDate]
        public DateTime Birthday { get; set; }

        public CreateRecruiterDto Recruiter { get; set; }

        public ICollection<CreateSkillDto> Skills { get; set; }
    }
}
