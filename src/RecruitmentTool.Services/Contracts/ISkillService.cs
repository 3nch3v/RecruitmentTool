namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface ISkillService
    {
        int ActiveCount();

        Skill GetById(int id);

        ICollection<Skill> GetAllActive<T>(T queryParameters);
    }
}
