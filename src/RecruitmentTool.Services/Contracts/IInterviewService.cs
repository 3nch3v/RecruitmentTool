namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface IInterviewService
    {
        int Count();

        Interview GetById(int id);

        ICollection<Interview> GetAll<T>(T queryParameters);
    }
}
