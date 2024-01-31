using Microsoft.EntityFrameworkCore;
using SoftDeleteMultilingualBaseDbContext.Localization;
using SoftDeleteMultilingualBaseDbContext.Repository.Models.B;

namespace SoftDeleteMultilingualBaseDbContext.Repository.Contexts
{
    public class BDbContext : BaseDbContext
    {
        public BDbContext(DbContextOptions<BDbContext> options, ISharedResource sharedResource) : base(options, sharedResource)
        {

        }
        public DbSet<BModel> BModels { get; set; }
    }
}
