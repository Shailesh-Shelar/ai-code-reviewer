using Microsoft.EntityFrameworkCore;
using CodeReviewAPI.Models;

namespace CodeReviewAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReviewHistory> ReviewHistories { get; set; }
    }
}