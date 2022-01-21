namespace RecruitmentTool.Services
{
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Collections.Generic;

    using AutoMapper;

    using RecruitmentTool.Data;
    using RecruitmentTool.Data.Models;
    using RecruitmentTool.Data.Dtos;
    using RecruitmentTool.Services.Contracts;

    public class SkillService : ISkillService
    {
        private readonly IMapper mapper;
        private readonly RecruitmentToolDbContext dbContext;

        public SkillService(IMapper mapper, RecruitmentToolDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public int ActiveCount()
        {
            return dbContext.Skills
                .Where(x => x.Candidates.Count > 0)
                .Count();
        }

        public Skill GetById(int id)
        {
            return dbContext.Skills.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Skill> GetAllActive<T>(T queryParameters)
        {
            var queryParams = mapper.Map<QueryParams>(queryParameters);

            var activeSkillsQuery = dbContext.Skills
                .Where(x => x.Candidates.Count > 0)
                .AsQueryable();

            if (queryParams.HasQuery)
            {
                activeSkillsQuery = activeSkillsQuery
                    .Where($"Name = @0", $"{queryParams.PropertyValue}")
                    .AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(queryParams.OrderBy))
            {
                activeSkillsQuery = activeSkillsQuery
                    .OrderBy(queryParams.OrderByProp, queryParams.IsDescending)
                    .AsQueryable();
            }
                
            var toReturn = activeSkillsQuery
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();

            return toReturn;
        }
    }
}
