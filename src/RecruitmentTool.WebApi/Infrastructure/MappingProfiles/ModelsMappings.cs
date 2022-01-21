namespace RecruitmentTool.WebApi.Infrastructure.MappingProfiles
{
    using AutoMapper;

    using RecruitmentTool.Data.Dtos;
    using RecruitmentTool.Data.Models;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Models.Dtos.Candidate;
    using RecruitmentTool.WebApi.Models.Dtos.Recruiter;
    using RecruitmentTool.WebApi.Models.Dtos.Skills;

    public class ModelsMappings : Profile
    {
        public ModelsMappings()
        {
            //CreateMap<TSource, TDestination>().ReverseMap();
            CreateMap<QueryParameters, QueryParams>().ReverseMap();

            //Skills mappings

            CreateMap<Skill, SkillDto>().ReverseMap();
            CreateMap<CreateSkillDto, Skill>().ReverseMap();

            //Candidate mappings
            CreateMap<CreateCandidateDto, Candidate>().ReverseMap();
            CreateMap<Candidate, CandidateDto>().ReverseMap();
            CreateMap<PartiallyUpdateCandidateDto, Candidate>().ReverseMap();
            CreateMap<UpdateCandidateDto, Candidate>().ReverseMap();

            //Recruiter mappings
            CreateMap<CreateRecruiterDto, Recruiter>().ReverseMap();
            CreateMap<Recruiter, RecruiterDto>().ReverseMap();
        }
    }
}
