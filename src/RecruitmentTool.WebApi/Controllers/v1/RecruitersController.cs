namespace RecruitmentTool.WebApi.Controllers.v1
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using RecruitmentTool.Services.Contracts;
    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Models.Dtos.Recruiter;
    using RecruitmentTool.WebApi.Infrastructure.ControllerHelpers;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RecruitersController : ControllerBase
    {
        private readonly IUrlHelper urlHelper;
        private readonly IRecruiterService recruiterService;
        private readonly IMapper mapper;

        public RecruitersController(
            IUrlHelper urlHelper,
            IRecruiterService recruiterService,
            IMapper mapper)
        {
            this.urlHelper = urlHelper;
            this.recruiterService = recruiterService;
            this.mapper = mapper;
        }

        [HttpGet(Name = nameof(GetRecuiters))]
        public ActionResult GetRecuiters(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            var recruiters = recruiterService.GetRecruiters(queryParameters);
            var recruitersDto = mapper.Map<ICollection<RecruiterDto>>(recruiters);

            var recruitersCount = recruiterService.Count();

            var paginationMetadata = new
            {
                totalCount = recruitersCount,
                pageSize = queryParameters.PageSize,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(recruitersCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = LinksForCollection.Create(urlHelper, queryParameters, version, recruitersCount, nameof(GetRecuiters));

            return Ok(new
            {
                value = recruitersDto,
                links = links,
            });
        }
    }
}
