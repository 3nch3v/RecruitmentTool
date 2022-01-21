namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface ISkillService
    {
        int ActiveCount();

        ICollection<Skill> GetAllActive<T>(T queryParameters);
    }
}
