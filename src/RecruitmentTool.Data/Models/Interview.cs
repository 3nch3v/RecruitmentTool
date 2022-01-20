namespace RecruitmentTool.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Interview
    {
        [Required]
        public string RecruiterId { get; set; }

        public virtual Recruiter Recruiter { get; set; }

        [Required]
        public string CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public DateTime Date { get; set; }
    }
}
