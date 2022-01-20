namespace RecruitmentTool.Data.Dtos
{
    public class QueryParams
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; }

        public string Query { get; set; }

        public string OrderBy { get; set; }

        public bool HasQuery => !string.IsNullOrEmpty(Query);

        public string PropertyName => !string.IsNullOrEmpty(Query) ? Query.Split('_').First() : null;

        public string PropertyValue => !string.IsNullOrEmpty(Query) ? Query.Split('_').Last() : null;

        public string OrderByProp => !string.IsNullOrEmpty(OrderBy) ? OrderBy.Split('_').First() : null;

        public bool IsDescending => !string.IsNullOrEmpty(OrderBy) && OrderBy.Split('_').Last().ToLowerInvariant().StartsWith("desc");
    }
}
