namespace RecruitmentTool.WebApi.Controllers.v1
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using RecruitmentTool.Services.Contracts;
    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Models.Dtos.Skills;
    using RecruitmentTool.WebApi.Infrastructure.ControllerHelpers;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly IUrlHelper urlHelper;
        private readonly ISkillService skillService;
        private readonly IMapper mapper;

        public SkillsController(
            IUrlHelper urlHelper,
            ISkillService skillService,
            IMapper mapper)
        {
            this.urlHelper = urlHelper;
            this.skillService = skillService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSkill))]
        public ActionResult GetSkill(ApiVersion version, int id)
        {
            var skill = mapper.Map<SkillDto>(skillService.GetById(id));

            if (skill == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleSkill(skill, version));
        }

        [HttpGet]
        [Route("active", Name = nameof(GetActiveSkills))]
        public ActionResult GetActiveSkills(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            var skills = skillService.GetAllActive(queryParameters);
            var skillsDto = mapper.Map<ICollection<SkillDto>>(skills);

            var skillsCount = skillService.ActiveCount();

            var paginationMetadata = new
            {
                totalCount = skillsCount,
                pageSize = queryParameters.PageSize,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(skillsCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = LinksForCollection.Create(urlHelper, queryParameters, version, skillsCount, nameof(GetActiveSkills));

            var toReturn = skillsDto.Select(skill => ExpandSingleSkill(skill, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        private dynamic ExpandSingleSkill(SkillDto skill, ApiVersion version)
        {
            var links = LinksForSigle.Create(
                urlHelper,
                version,
                skill.Id.ToString(), 
                "skill",
                nameof(GetSkill));

            var resourceToReturn = skill.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }
    }
}
