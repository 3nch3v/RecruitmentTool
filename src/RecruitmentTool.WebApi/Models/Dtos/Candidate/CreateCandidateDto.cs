namespace RecruitmentTool.WebApi.Models.Dtos.Candidate
{
    using System.ComponentModel.DataAnnotations;

    using RecruitmentTool.WebApi.Models.Dtos.Recruiter;
    using RecruitmentTool.WebApi.Models.Dtos.Skills;

    using static RecruitmentTool.Common.GlobalConstants;

    public class CreateCandidateDto
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; }

        [Required]
        [StringLength(BioMaxLength, MinimumLength = BioMinLength)]
        public string Bio { get; set; }

        public DateTime Birthday { get; set; }

        [Required]
        public CreateRecruiterDto Recruiter { get; set; }

        public ICollection<CreateSkillDto> Skills { get; set; }
    }
}
