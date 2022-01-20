namespace RecruitmentTool.Services.Contracts
{
    using RecruitmentTool.Data.Models;

    public interface ICandidateService
    {
        int Count();

        bool Delete(string id);

        bool Update<T>(string id, T candidateDto);

        Candidate GetById(string id);

        Task<Candidate> CreateAsync<T>(T candidateDto);

        ICollection<Candidate> GetAll<T>(T queryParameters);
    }
}
