namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface IInterviewService
    {
        int Count();

        ICollection<Interview> GetAll<T>(T queryParameters);
    }
}
