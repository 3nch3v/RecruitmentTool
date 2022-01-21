namespace RecruitmentTool.Services
{
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Collections.Generic;

    using AutoMapper;

    using RecruitmentTool.Data;
    using RecruitmentTool.Data.Dtos;
    using RecruitmentTool.Data.Models;
    using RecruitmentTool.Services.Contracts;

    public class RecruiterService : IRecruiterService
    {
        private readonly IMapper mapper;
        private readonly RecruitmentToolDbContext dbContext;

        private int count = 0;

        public RecruiterService(IMapper mapper, RecruitmentToolDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public int Count() => count;

        public ICollection<Recruiter> GetRecruiters<T>(T queryParameters)
        {
            var queryParams = mapper.Map<QueryParams>(queryParameters);

            var recruiters = dbContext.Recruiters
                .AsQueryable();         

            if (queryParams.HasQuery)
            {
                recruiters = recruiters
                    .Where($"ExperienceLevel = @0", $"{queryParams.PropertyValue}")
                    .AsQueryable();
            }
            else
            {
                recruiters = recruiters
                    .Where(x => x.Candidates.Count > 0)
                    .AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(queryParams.OrderBy))
            {
                recruiters = recruiters
                    .OrderBy(queryParams.OrderByProp, queryParams.IsDescending)
                    .AsQueryable();
            }

            count = recruiters.Count();

            var toReturn = recruiters
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();

            return toReturn;
        }
    }
}
