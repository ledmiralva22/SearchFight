namespace Infrastructure.Models
{
    public class BingResponse
    {
        public WebAnswer WebPages { get; set; }
    }

    public class WebAnswer
    {
        public long TotalEstimatedMatches { get; set; }
    }
}
