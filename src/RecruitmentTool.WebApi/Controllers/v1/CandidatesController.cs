namespace RecruitmentTool.WebApi.Controllers.v1
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using RecruitmentTool.Services.Contracts;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Models.Dtos.Candidate;
    using RecruitmentTool.WebApi.Infrastructure.ControllerHelpers;
    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class CandidatesController : ControllerBase
    {
        private readonly IUrlHelper urlHelper;
        private readonly ICandidateService candidateService;
        private readonly IMapper mapper;

        public CandidatesController(
            IUrlHelper urlHelper,
            ICandidateService candidateService,
            IMapper mapper)
        {
            this.urlHelper = urlHelper;
            this.candidateService = candidateService;
            this.mapper = mapper;
        }

        [HttpPost(Name = nameof(AddCandidate))]
        public async Task<ActionResult<CandidateDto>> AddCandidate(ApiVersion version, [FromBody] CreateCandidateDto candidateCreatedto)
        {
            var newCandidate = await candidateService.CreateAsync(candidateCreatedto);

            if (newCandidate == null)
            {
                throw new Exception("Creating a candidate failed.");
            }

            var candidateDto = mapper.Map<CandidateDto>(newCandidate);

            return CreatedAtRoute(
                nameof(GetCandidate),
                new { version = version.ToString(), id = newCandidate.Id },
                candidateDto);
        }

        [HttpGet]
        [Route("{id}", Name = nameof(GetCandidate))]
        public ActionResult GetCandidate(ApiVersion version, string id)
        {
            var candidateDto = mapper.Map<CandidateDto>(candidateService.GetById(id));

            if (candidateDto == null)
            {
                return NotFound();
            }

            var toReturn = ExpandSingleCandidate(candidateDto, version);

            return Ok(toReturn);
        }

        [HttpGet(Name = nameof(GetAllCandidates))]
        public ActionResult GetAllCandidates(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            var candidates = candidateService.GetAll(queryParameters);
            var candidatesDto = mapper.Map<ICollection<CandidateDto>>(candidates);

            var candidateCount = candidateService.Count();

            var paginationMetadata = new
            {
                totalCount = candidateCount,
                pageSize = queryParameters.PageSize,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(candidateCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = LinksForCollection.Create(
                urlHelper, 
                queryParameters, 
                version, 
                candidateCount, 
                nameof(GetAllCandidates), 
                nameof(AddCandidate), 
                "candidate");

            var toReturn = candidatesDto.Select(candidate => ExpandSingleCandidate(candidate, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpDelete]
        [Route("{id}", Name = nameof(RemoveCandidate))]
        public ActionResult RemoveCandidate(string id)
        {
            var candidate = candidateService.GetById(id);

            if (candidate == null)
            {
                return NotFound();
            }

            var succseed = candidateService.Delete(id);

            if (!succseed)
            {
                throw new Exception("Deleting a candidate failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id}", Name = nameof(UpdateCandidate))]
        public ActionResult<CandidateDto> UpdateCandidate(string id, [FromBody] UpdateCandidateDto candidateDto)
        {
            var existingCandidate = candidateService.GetById(id);

            if (existingCandidate == null)
            {
                return NotFound();
            }

            var succseed = candidateService.Update(id, candidateDto);

            if (!succseed)
            {
                throw new Exception("Updating a candidate failed on save.");
            }

            return Ok(mapper.Map<CandidateDto>(existingCandidate));
        }

        [HttpPatch("{id}", Name = nameof(PartiallyUpdateCandidate))]
        public ActionResult<CandidateDto> PartiallyUpdateCandidate(string id, [FromBody] PartiallyUpdateCandidateDto candidateInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingCandidate = candidateService.GetById(id);

            if (existingCandidate == null)
            {
                return NotFound();
            }

            var succseed = candidateService.UpdatePartially(id, candidateInput);

            if (!succseed)
            {
                throw new Exception("Updating a candidate failed on save.");
            }

            return Ok(mapper.Map<CandidateDto>(existingCandidate));
        }

        private dynamic ExpandSingleCandidate(CandidateDto candidate, ApiVersion version)
        {
            var links = LinksForSigle.Create(
                urlHelper,
                version,
                candidate.Id,
                "candidate",
                nameof(GetCandidate),
                nameof(AddCandidate),
                nameof(UpdateCandidate),
                nameof(PartiallyUpdateCandidate),
                nameof(RemoveCandidate));

            var resourceToReturn = candidate.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);
            return resourceToReturn;
        }
    }
}
