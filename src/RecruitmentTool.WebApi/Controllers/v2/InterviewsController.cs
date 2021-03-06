namespace RecruitmentTool.WebApi.Controllers.v2
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InterviewsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Version 2.0 in progress.");
        }
    }
}
