namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface IJobService
    {
        int Count();

        bool Delete(int id);

        Job GetById(int id);

        Job Create<T>(T jobDto);

        ICollection<Job> GetAll<T>(T queryParameters);
    }
}
