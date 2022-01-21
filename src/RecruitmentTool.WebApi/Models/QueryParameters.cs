namespace RecruitmentTool.WebApi.Models
{
    using static RecruitmentTool.Common.GlobalConstants;

    public class QueryParameters
    {
        private int pageSize = MaxPageSize;

        public int Page { get; set; } = 1;

        public string Query { get; set; }

        public string OrderBy { get; set; }

        public int PageSize
        {
            get 
            { 
                return pageSize; 
            }
            set 
            { 
                pageSize = value > MaxPageSize ? MaxPageSize : value; 
            }
        }
    }
}