using Microsoft.EntityFrameworkCore;
using SoftDeleteMultilingualBaseDbContext.Localization;
using SoftDeleteMultilingualBaseDbContext.Repository.Models.A;

namespace SoftDeleteMultilingualBaseDbContext.Repository.Contexts
{
    public class ADbContext : BaseDbContext
    {
        public ADbContext(DbContextOptions<ADbContext> options, ISharedResource sharedResource) : base(options, sharedResource)
        {

        }
        public DbSet<AModel> AModels { get; set; }
    }
}
