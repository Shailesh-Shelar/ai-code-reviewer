using CodeReviewAPI.Data;
using CodeReviewAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> ReviewCode([FromBody] CodeReviewRequest request)

        {
            var review = new ReviewHistory
            {
                Id = Guid.NewGuid(),
                CodeSnippet = request.Code,
                Language = request.Language,
                ReviewResult = "AI Review Will be here",
                CreatedAt = DateTime.UtcNow,

            };
            _context.ReviewHistories.Add(review);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                id = review.Id,
                code=review.CodeSnippet,
                language = review.Language,
                review = review.ReviewResult,
                createdAt = review.CreatedAt
            });
            
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
}
     


public class CodeReviewRequest
{
    public string Code { get; set; }
    public string Language { get; set; }
}
