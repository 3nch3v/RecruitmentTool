namespace RecruitmentTool.WebApi.Controllers.v1
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using RecruitmentTool.Services.Contracts;
    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Models.Dtos.Skills;

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

            var links = CreateLinksForCollection(queryParameters, skillsCount, version);

            var toReturn = skillsDto.Select(x => ExpandSingleSkill(x, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        private dynamic ExpandSingleSkill(SkillDto skill, ApiVersion version)
        {
            var links = GetLinks(skill.Id, version);
            var resourceToReturn = skill.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        private IEnumerable<LinkDto> GetLinks(int id, ApiVersion version)
        {
            var links = new List<LinkDto>();
            var getLink = urlHelper.Link(nameof(GetSkill), new { version = version.ToString(), id = id });
            links.Add(new LinkDto(getLink, "self", "GET"));

            return links;
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
            {
                pagesize = queryParameters.PageSize,
                page = queryParameters.Page,
                orderby = queryParameters.OrderBy
            }), "self", "GET"));

            links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
            {
                pagesize = queryParameters.PageSize,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
            {
                pagesize = queryParameters.PageSize,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
                {
                    pagesize = queryParameters.PageSize,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
                {
                    pagesize = queryParameters.PageSize,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            return links;
        }
    }
}
