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

            return Ok(new
            {
                value = skillsDto,
                links = links
            });
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
            {
                page = queryParameters.Page,
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy,
            }), "self", "GET"));

            links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
            {
                page = 1,
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
            {
                page = queryParameters.GetTotalPages(totalCount),
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
                {
                    page = queryParameters.Page + 1,
                    pagesize = queryParameters.PageSize,
                    filtter = queryParameters.Query,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(urlHelper.Link(nameof(GetActiveSkills), new
                {
                    page = queryParameters.Page - 1,
                    pagesize = queryParameters.PageSize,
                    filtter = queryParameters.Query,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            return links;
        }
    }
}
