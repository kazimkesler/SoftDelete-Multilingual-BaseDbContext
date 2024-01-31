using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using SoftDeleteMultilingualBaseDbContext.Localization;
using SoftDeleteMultilingualBaseDbContext.Repository.Models;
using System.Linq.Expressions;

namespace SoftDeleteMultilingualBaseDbContext.Repository.Contexts
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly ISharedResource _sharedResource;
        private string _defaultLanguage;

        protected BaseDbContext(DbContextOptions options, ISharedResource sharedResource) : base(options)
        {
            _sharedResource = sharedResource;
            _defaultLanguage = _sharedResource["_language"].ToLower();
        }

        public override int SaveChanges()
        {
            UpdateChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entity)
                {
                    if (entity.Language == null)
                        entity.Language = _defaultLanguage;

                    switch (item.State)
                    {
                        case EntityState.Added:
                            entity.Id = Guid.NewGuid();
                            entity.CreationTime = DateTime.UtcNow;
                            entity.ModificationTime = null;
                            entity.DeletionTime = null;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.Id).IsModified = false;
                            Entry(entity).Property(x => x.CreationTime).IsModified = false;
                            entity.ModificationTime = DateTime.UtcNow;
                            Entry(entity).Property(x => x.DeletionTime).IsModified = false;
                            break;
                        case EntityState.Deleted:
                            item.State = EntityState.Modified;
                            Entry(entity).Property(x => x.Id).IsModified = false;
                            Entry(entity).Property(x => x.CreationTime).IsModified = false;
                            Entry(entity).Property(x => x.ModificationTime).IsModified = false;
                            entity.DeletionTime = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConstraintToAllModels(modelBuilder);
            ApplyFilterToAllModels(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveMaxLength(1024);
            base.ConfigureConventions(configurationBuilder);
        }

        public void ApplyConstraintToAllModels(ModelBuilder modelBuilder)
        {
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(BaseEntity)))
                {
                    mutableEntityType.FindProperty("Language").SetMaxLength(8);
                }
            }
        }

        private void ApplyFilterToAllModels(ModelBuilder modelBuilder)
        {
            Expression<Func<BaseEntity, bool>> filterExpr = bm => bm.DeletionTime == null && bm.Language == _defaultLanguage;
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(BaseEntity)))
                {
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);

                    mutableEntityType.SetQueryFilter(lambdaExpression);
                }
            }
        }
    }
}
