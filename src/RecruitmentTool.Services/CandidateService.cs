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

    public class CandidateService : ICandidateService
    {
        private int count = 0;

        private readonly IMapper mapper;
        private readonly RecruitmentToolDbContext dbContext;

        public CandidateService(IMapper mapper, RecruitmentToolDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public Candidate GetById(string id)
        {
            return dbContext.Candidates.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Candidate> CreateAsync<T>(T candidateDto)
        {
            var candidateInput = mapper.Map<Candidate>(candidateDto);
            var existingCandidate = dbContext.Candidates.FirstOrDefault(x => x.Email.ToLower() == candidateInput.Email.ToLower());

            if (existingCandidate != null)
            {
                return null;
            }

            var newCandidate = new Candidate
            {
                FirstName = candidateInput.FirstName,
                LastName = candidateInput.LastName,
                Email = candidateInput.Email,
                Bio = candidateInput.Bio,
                Birthday = candidateInput.Birthday,
            };

            SetRecruiter(candidateInput.Recruiter, newCandidate);
            SetSkills(candidateInput, newCandidate);

            await dbContext.Candidates.AddAsync(newCandidate);
            await dbContext.SaveChangesAsync();

            return newCandidate;
        }

        public bool Delete(string id)
        {
            var candidate = GetById(id);

            try
            {
                dbContext.Candidates.Remove(candidate);
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ICollection<Candidate> GetAll<T>(T queryParameters)
        {
            var queryParams = mapper.Map<QueryParams>(queryParameters);
            
            var candidates = dbContext.Candidates.AsQueryable();

            if (queryParams.HasQuery)
            {
                candidates = candidates
                    .Where($"{queryParams.PropertyName} = @0", $"{queryParams.PropertyValue}")
                    .AsQueryable();
            }

            if (!string.IsNullOrEmpty(queryParams.OrderBy))
            {
                candidates = candidates
                    .OrderBy(queryParams.OrderByProp, queryParams.IsDescending)
                    .AsQueryable();
            }

            count = candidates.Count();

            return candidates
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();
        }

        public int Count() => count;

        public bool Update<T>(string id, T candidateDto)
        {
            var candidateInput = mapper.Map<Candidate>(candidateDto);
            var candidate = GetById(id);

            candidate.FirstName = candidateInput.FirstName;
            candidate.LastName = candidateInput.LastName;
            candidate.Email = candidateInput.Email;
            candidate.Bio = candidateInput.Bio;
            candidate.Birthday = candidateInput.Birthday;

            candidate.RecruiterId = null;
            SetRecruiter(candidateInput.Recruiter, candidate);

            candidate.Skills.Clear();
            SetSkills(candidateInput, candidate);

            var isSuccseed = dbContext.SaveChanges() >= 0;

            return isSuccseed;
        }

        public bool UpdatePartially<T>(string id, T candidateDto)
        {
            var candidateInput = mapper.Map<Candidate>(candidateDto);
            var existingCandidate = GetById(id);

            if (candidateInput.FirstName != null 
                && existingCandidate.FirstName != candidateInput.FirstName)
            {
                existingCandidate.FirstName = candidateInput.FirstName;
            }

            if (candidateInput.LastName != null
                && existingCandidate.LastName != candidateInput.LastName)
            {
                existingCandidate.LastName = candidateInput.LastName;
            }

            if (candidateInput.Bio != null
                && existingCandidate.Bio != candidateInput.Bio)
            {
                existingCandidate.Bio = candidateInput.Bio;
            }

            if (candidateInput.Email != null
                && existingCandidate.Email != candidateInput.Email)
            {
                existingCandidate.Email = candidateInput.Email;
            }

            if (DateTime.Compare(candidateInput.Birthday, new DateTime()) > 0
                && DateTime.Compare(existingCandidate.Birthday, candidateInput.Birthday) != 0)
            {
                existingCandidate.Birthday = candidateInput.Birthday;
            }

            if (candidateInput.Recruiter  != null
                && (existingCandidate.Recruiter.Email != candidateInput.Recruiter.Email))
            {
                SetRecruiter(candidateInput.Recruiter, existingCandidate);
            }

            if (candidateInput.Skills.Any())
            {
                SetSkills(candidateInput, existingCandidate);
            }

            var isSuccseed = dbContext.SaveChanges() >= 0;
            return isSuccseed;
        }

        private void SetRecruiter(Recruiter recruiter, Candidate candidate)
        {
            var existingRecruiter = dbContext.Recruiters
                                .FirstOrDefault(x => x.Email.ToLower() == recruiter.Email.ToLower());

            if (existingRecruiter != null)
            {
                existingRecruiter.ExperienceLevel++;
                candidate.Recruiter = existingRecruiter;
            }
            else
            {
                var newRecruiter = new Recruiter
                {
                    LastName = recruiter.LastName,
                    Email = recruiter.Email,
                    Country = recruiter.Country,
                };

                candidate.Recruiter = newRecruiter;
            }
        }

        private void SetSkills(Candidate candidateInput, Candidate candidate)
        {
            foreach (var currSkill in candidateInput.Skills)
            {
                var skill = dbContext.Skills
                    .FirstOrDefault(x => x.Name.ToLower() == currSkill.Name.ToLower());

                if (skill == null)
                {
                    skill = new Skill
                    {
                        Name = currSkill.Name,
                    };
                }

                if (!candidate.Skills.Any(x => x.Name == currSkill.Name))
                {
                    candidate.Skills.Add(skill);
                }
            }
        }
    }
}
