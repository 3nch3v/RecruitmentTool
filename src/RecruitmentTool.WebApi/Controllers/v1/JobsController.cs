namespace RecruitmentTool.WebApi.Controllers.v1
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using RecruitmentTool.Services.Contracts;
    using RecruitmentTool.WebApi.Models.Dtos.Job;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Infrastructure.ControllerHelpers;
    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IUrlHelper urlHelper;
        private readonly IJobService jobService;
        private readonly IMapper mapper;

        public JobsController(
            IUrlHelper urlHelper,
            IJobService jobService,
            IMapper mapper)
        {
            this.urlHelper = urlHelper;
            this.jobService = jobService;
            this.mapper = mapper;
        }

        [HttpPost(Name = nameof(AddJob))]
        public async Task<ActionResult<JobDto>> AddJob(ApiVersion version, [FromBody] CreateJobDto createJobDto)
        {
            var newJob = await jobService.Create(createJobDto);

            if (newJob == null)
            {
                throw new Exception("Creating a candidate failed on save.");
            }

            var jobDto = mapper.Map<JobDto>(newJob);

            return CreatedAtRoute(
                nameof(GetJobs),
                new { version = version.ToString(), id = newJob.Id },
                jobDto);
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveJob))]
        public ActionResult RemoveJob(int id)
        {
            var job = jobService.GetById(id);

            if (job == null)
            {
                return NotFound();
            }

            var succseed = jobService.Delete(id);

            if (!succseed)
            {
                throw new Exception("Deleting a job failed on save.");
            }

            return NoContent();
        }

        [HttpGet(Name = nameof(GetJobs))]
        public ActionResult GetJobs(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            var jobs = jobService.GetAll(queryParameters);
            var jobsDto = mapper.Map<ICollection<JobDto>>(jobs);

            var jobsCount = jobService.Count();

            var paginationMetadata = new
            {
                totalCount = jobsCount,
                pageSize = queryParameters.PageSize,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(jobsCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = LinksForCollection.Create(
                urlHelper,
                queryParameters,
                version,
                jobsCount,
                nameof(GetJobs),
                nameof(AddJob),
                "job");

            var toReturn = jobsDto.Select(x => ExpandSingleJob(x, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        private dynamic ExpandSingleJob(JobDto job, ApiVersion version)
        {
            var links = LinksForSigle.Create(
                urlHelper,
                version,
                job.Id.ToString(),
                "Job",
                null,
                nameof(AddJob),
                null,
                null,
                nameof(RemoveJob));

            var resourceToReturn = job.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }
    }
}
