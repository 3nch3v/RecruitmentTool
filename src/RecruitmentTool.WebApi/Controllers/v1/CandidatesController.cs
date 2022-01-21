namespace RecruitmentTool.WebApi.Controllers.v1
{
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using RecruitmentTool.Services.Contracts;
    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;
    using RecruitmentTool.WebApi.Models;
    using RecruitmentTool.WebApi.Models.Dtos.Candidate;

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

            return CreatedAtRoute(
                nameof(GetCandidate),
                new { version = version.ToString(), id = newCandidate.Id },
                mapper.Map<CandidateDto>(newCandidate));
        }

        [HttpGet]
        [Route("{id}", Name = nameof(GetCandidate))]
        public ActionResult GetCandidate(ApiVersion version, string id)
        {
            var candidate = mapper.Map<CandidateDto>(candidateService.GetById(id));

            if (candidate == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleCandidate(candidate, version));
        }

        [HttpGet(Name = nameof(GetAllCandidates))]
        public ActionResult GetAllCandidates(
            ApiVersion version, [FromQuery] QueryParameters queryParameters)
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

            var links = CreateLinksForCollection(queryParameters, candidateCount, version);

            var toReturn = candidatesDto.Select(x => ExpandSingleCandidate(x, version));

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

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(urlHelper.Link(nameof(GetAllCandidates), new
            {
                page = queryParameters.Page,
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy
            }), "self", "GET"));

            links.Add(new LinkDto(urlHelper.Link(nameof(GetAllCandidates), new
            {
                page = 1,
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(urlHelper.Link(nameof(GetAllCandidates), new
            {
                page = queryParameters.GetTotalPages(totalCount),
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(urlHelper.Link(nameof(GetAllCandidates), new
                {
                    page = queryParameters.Page + 1,
                    pagesize = queryParameters.PageSize,
                    filtter = queryParameters.Query,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(urlHelper.Link(nameof(GetAllCandidates), new
                {
                    page = queryParameters.Page - 1,
                    pagesize = queryParameters.PageSize,
                    filtter = queryParameters.Query,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            var posturl = urlHelper.Link(nameof(AddCandidate), new { version = version.ToString() });

            links.Add(new LinkDto(posturl, "create_candidate", "POST"));

            return links;
        }

        private dynamic ExpandSingleCandidate(CandidateDto candidate, ApiVersion version)
        {
            var links = GetLinks(candidate.Id, version);
            var resourceToReturn = candidate.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        private IEnumerable<LinkDto> GetLinks(string id, ApiVersion version)
        {
            var links = new List<LinkDto>();

            var getLink = urlHelper.Link(nameof(GetCandidate), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(getLink, "self", "GET"));

            var createLink = urlHelper.Link(nameof(AddCandidate), new { version = version.ToString() });

            links.Add(
              new LinkDto(createLink,
              "create_candidate",
              "POST"));

            var updateLink = urlHelper.Link(nameof(UpdateCandidate), new { version = version.ToString(), id = id });

            links.Add(
               new LinkDto(updateLink,
               "update_candidate",
               "PUT"));

            var deleteLink = urlHelper.Link(nameof(RemoveCandidate), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(deleteLink,
              "delete_candidate",
              "DELETE"));

            return links;
        }
    }
}
