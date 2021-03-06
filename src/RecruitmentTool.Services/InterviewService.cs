namespace RecruitmentTool.Services
{
    using System.Linq;
    using System.Linq.Dynamic.Core;

    using AutoMapper;

    using RecruitmentTool.Data;
    using RecruitmentTool.Data.Dtos;
    using RecruitmentTool.Data.Models;
    using RecruitmentTool.Services.Contracts;

    public class InterviewService : IInterviewService
    {
        private int count = 0;

        private readonly IMapper mapper;
        private readonly RecruitmentToolDbContext dbContext;

        public InterviewService(IMapper mapper, RecruitmentToolDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public int Count() => count;

        public ICollection<Interview> GetAll<T>(T queryParameters)
        {
            var queryParams = mapper.Map<QueryParams>(queryParameters);

            var interviesQuery = dbContext.Interviews.AsQueryable();

            if (queryParams.HasQuery)
            {
                interviesQuery = interviesQuery
                    .Where($"Date = @0", $"{queryParams.PropertyValue}")
                    .AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(queryParams.OrderBy))
            {
                interviesQuery = interviesQuery
                    .OrderBy(queryParams.OrderByProp, queryParams.IsDescending)
                    .AsQueryable();
            }

            count = interviesQuery.Count();

            var toReturn = interviesQuery
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();

            return toReturn;
        }
    }
}
