namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface IRecruiterService
    {
        int Count();

        ICollection<Recruiter> GetRecruiters<T>(T queryParameters);
    }
}
