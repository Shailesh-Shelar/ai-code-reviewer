namespace CodeReviewAPI.Models
{
    public class ReviewHistory
    {
        public Guid Id { get; set; }
        public string CodeSnippet { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string ReviewResult { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}