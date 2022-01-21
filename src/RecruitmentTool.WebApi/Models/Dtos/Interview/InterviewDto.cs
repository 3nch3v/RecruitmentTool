namespace RecruitmentTool.WebApi.Models.Dtos.Interview
{
    using RecruitmentTool.WebApi.Models.Dtos.Candidate;
    using RecruitmentTool.WebApi.Models.Dtos.Job;

    public class InterviewDto
    {
        public CandidateDto Candidate { get; set; }

        public JobDto Job { get; set; }

        public DateTime Date { get; set; }
    }
}
