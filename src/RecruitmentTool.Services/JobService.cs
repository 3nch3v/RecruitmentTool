namespace RecruitmentTool.Services
{
    using System.Linq;
    using System.Linq.Dynamic.Core;

    using AutoMapper;

    using RecruitmentTool.Data;
    using RecruitmentTool.Data.Dtos;
    using RecruitmentTool.Data.Models;
    using RecruitmentTool.Services.Contracts;

    public class JobService : IJobService
    {
        private int count = 0;

        private readonly IMapper mapper;
        private readonly RecruitmentToolDbContext dbContext;

        public JobService(IMapper mapper, RecruitmentToolDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public Job GetById(int id)
        {
            return dbContext.Jobs.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Job> GetAll<T>(T queryParameters)
        {
            var queryParams = mapper.Map<QueryParams>(queryParameters);

            var jobsQuery = dbContext.Jobs.AsQueryable();

            if (queryParams.HasQuery)
            {
                jobsQuery = jobsQuery
                    .Where($"{queryParams.PropertyName} = @0", $"{queryParams.PropertyValue}")
                    .AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(queryParams.OrderBy))
            {
                jobsQuery = jobsQuery
                    .OrderBy(queryParams.OrderByProp, queryParams.IsDescending)
                    .AsQueryable();
            }

            count = jobsQuery.Count();

            var toReturn = jobsQuery
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();

            return toReturn;
        }

        public int Count() => count;

        public async Task<Job> Create<T>(T jobDto)
        {
            var jobInput = mapper.Map<Job>(jobDto);

            var newJob = new Job
            {
                Title = jobInput.Title,
                Description = jobInput.Description,
                Salary = jobInput.Salary,
            };

            foreach (var currSkill in jobInput.Skills)
            {
                var skill = dbContext.Skills
                    .FirstOrDefault(x => x.Name.ToLower() == currSkill.Name.ToLower());

                if (skill == null)
                {
                    var newSkill = new Skill
                    {
                        Name = currSkill.Name,
                    };
                }

                newJob.Skills.Add(skill);
            }

            await dbContext.Jobs.AddAsync(newJob);
            await dbContext.SaveChangesAsync();

            CreateInterview(newJob.Id, newJob.Skills);

            return newJob;
        }

        public bool Delete(int id)
        {
            var job = GetById(id);

            var jobIntervies = dbContext.Interviews
                .Where(x => x.JobId == job.Id)
                .ToList();

            dbContext.Interviews.RemoveRange(jobIntervies);
            dbContext.Jobs.Remove(job);

            var isSuccessful = dbContext.SaveChanges() > 0;
            return isSuccessful;
        }

        private void CreateInterview(int id, ICollection<Skill> jobSkills)
        {
            var jobSkillsIds = jobSkills.Select(x => x.Id).ToList();

            var candidates = dbContext.Candidates
                .Where(candidate => candidate.Skills.Any(skill => jobSkillsIds.Contains(skill.Id)))
                .ToList();

            var dateTimeNow = DateTime.Now;
            var date = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day + 7, dateTimeNow.Hour, dateTimeNow.Minute, 0);

            foreach (var candidate in candidates)
            {
                if (candidate.Recruiter.Interviews.Count() < 5)
                {
                    date.AddMinutes(15);

                    var interview = new Interview
                    {
                        JobId = id,
                        CandidateId = candidate.Id,
                        RecruiterId = candidate.RecruiterId,
                        Date = date,
                    };

                    candidate.Recruiter.ExperienceLevel++;

                    dbContext.Interviews.Add(interview);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
