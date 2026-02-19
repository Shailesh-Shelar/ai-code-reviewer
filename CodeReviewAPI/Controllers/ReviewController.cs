using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeReviewAPI.Data;
using CodeReviewAPI.Models;
using CodeReviewAPI.Services;

namespace CodeReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly OpenAIService _openAIService;

        public ReviewController(ApplicationDbContext context, OpenAIService openAIService)
        {
            _context = context;
            _openAIService = openAIService;
        }

        [HttpPost]
        public async Task<IActionResult> ReviewCode([FromBody] CodeReviewRequest request)
        {
            try
            {
                // Get AI review
                var aiReview = await _openAIService.ReviewCodeAsync(request.Code, request.Language);

                // Save to database
                var review = new ReviewHistory
                {
                    Id = Guid.NewGuid(),
                    CodeSnippet = request.Code,
                    Language = request.Language,
                    ReviewResult = aiReview,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ReviewHistories.Add(review);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    id = review.Id,
                    code = review.CodeSnippet,
                    language = review.Language,
                    review = review.ReviewResult,
                    createdAt = review.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to review code: {ex.Message}" });
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _context.ReviewHistories
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .ToListAsync();

            return Ok(history);
        }
    }

    public class CodeReviewRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}