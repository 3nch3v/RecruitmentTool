namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface IRecruiterService
    {
        int Count();

        Recruiter GetById(string id);

        ICollection<Recruiter> GetAllWithCandidates<T>(T queryParameters);
    }
}
