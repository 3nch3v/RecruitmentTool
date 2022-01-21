namespace RecruitmentTool.WebApi.Infrastructure.ControllerHelpers
{
    using Microsoft.AspNetCore.Mvc;

    using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;
    using RecruitmentTool.WebApi.Models;

    public static class LinksForCollection
    {
        public static List<LinkDto> Create(
            IUrlHelper urlHelper,
            QueryParameters queryParameters,
            ApiVersion version,
            int totalCount, 
            string actionGet, 
            string actionPost = null, 
            string typeOfEntity = null)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(urlHelper.Link(actionGet, new
            {
                page = queryParameters.Page,
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy,
            }), "self", "GET"));

            links.Add(new LinkDto(urlHelper.Link(actionGet, new
            {
                page = 1,
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy,
            }), "first", "GET"));

            links.Add(new LinkDto(urlHelper.Link(actionGet, new
            {
                page = queryParameters.GetTotalPages(totalCount),
                pagesize = queryParameters.PageSize,
                filtter = queryParameters.Query,
                orderby = queryParameters.OrderBy,
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(urlHelper.Link(actionGet, new
                {
                    page = queryParameters.Page + 1,
                    pagesize = queryParameters.PageSize,
                    filtter = queryParameters.Query,
                    orderby = queryParameters.OrderBy,
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(urlHelper.Link(actionGet, new
                {
                    page = queryParameters.Page - 1,
                    pagesize = queryParameters.PageSize,
                    filtter = queryParameters.Query,
                    orderby = queryParameters.OrderBy,
                }), "previous", "GET"));
            }

            if (actionPost != null)
            {
                var posturl = urlHelper.Link(
                    actionPost, 
                    new { version = version.ToString() });

                links.Add(new LinkDto(posturl, $"create_{typeOfEntity}", "POST"));

            }

            return links;
        }
    }
}
