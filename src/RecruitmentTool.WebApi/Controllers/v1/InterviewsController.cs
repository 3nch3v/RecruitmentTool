namespace RecruitmentTool.WebApi.Controllers.v1
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using RecruitmentTool.Services.Contracts;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Models.Dtos.Interview;
    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;
    using RecruitmentTool.WebApi.Infrastructure.ControllerHelpers;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InterviewsController : ControllerBase
    {
        private readonly IUrlHelper urlHelper;
        private readonly IInterviewService interviewService;
        private readonly IMapper mapper;

        public InterviewsController(
            IUrlHelper urlHelper,
            IInterviewService interviewService,
            IMapper mapper)
        {
            this.urlHelper = urlHelper;
            this.interviewService = interviewService;
            this.mapper = mapper;
        }

        [HttpGet(Name = nameof(GetInterviews))]
        public ActionResult GetInterviews(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            var interviews = interviewService.GetAll(queryParameters);
            var interviewsDto = mapper.Map<ICollection<InterviewDto>>(interviews);

            var interviewsCount = interviewService.Count();

            var paginationMetadata = new
            {
                totalCount = interviewsCount,
                pageSize = queryParameters.PageSize,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(interviewsCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = LinksForCollection.Create(
                urlHelper,
                queryParameters,
                version,
                interviewsCount,
                nameof(GetInterviews));

            return Ok(new
            {
                value = interviewsDto,
                links = links
            });
        }
    }
}
