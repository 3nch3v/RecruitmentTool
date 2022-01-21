namespace RecruitmentTool.WebApi.Infrastructure.ControllerHelpers
{
    using Microsoft.AspNetCore.Mvc;

    using RecruitmentTool.WebApi.Models;

    public static class LinksForSigle
    {
        public static IEnumerable<LinkDto> Create(
            IUrlHelper urlHelper, 
            ApiVersion version,
            string id,
            string typeOfentity,
            string actionGet = null,
            string actionPost = null,
            string actionPut = null,
            string actionPatch = null,
            string actionDelete = null)
        {
            var links = new List<LinkDto>();

            if (actionGet != null)
            {
                var getLink = urlHelper.Link(actionGet, new { version = version.ToString(), id = id });
                links.Add(new LinkDto(getLink, "self", "GET"));
            }
            if (actionPost != null)
            {
                var createLink = urlHelper.Link(actionPost, new { version = version.ToString() });
                links.Add( new LinkDto(createLink, $"create_{typeOfentity}", "POST"));
            }
            if (actionPut != null)
            {
                var updateLink = urlHelper.Link(actionPut, new { version = version.ToString(), id = id });
                links.Add(new LinkDto(updateLink, $"update_{typeOfentity}", "PUT"));
            }
            if (actionPatch != null)
            {
                var patchLink = urlHelper.Link(actionPatch, new { version = version.ToString(), id = id });
                links.Add(new LinkDto(patchLink, $"patch_{typeOfentity}", "PATCH"));
            }
            if (actionDelete != null)
            {
                var deleteLink = urlHelper.Link(actionDelete, new { version = version.ToString(), id = id });
                links.Add(new LinkDto(deleteLink, $"delete_{typeOfentity}", "DELETE"));
            }

            return links;
        }
    }
}
