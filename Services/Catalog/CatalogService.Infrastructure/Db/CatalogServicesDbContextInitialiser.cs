using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NStore.Shared.Constants;

namespace CatalogService.Infrastructure.Db
{
    public class CatalogServicesDbContextInitialiser
    {
        private readonly ILogger<CatalogServiceDbContext> _logger;
        private readonly CatalogServiceDbContext _context;

        public CatalogServicesDbContextInitialiser(ILogger<CatalogServiceDbContext> logger, CatalogServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessageConstants.DatabaseInitializationErrorMessage);
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessageConstants.DatabaseSeedingErrorMessage);
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if (!_context.Categories.Any())
            {
                var parent = new Category("Clothing and apparel", null, null, null);

                var categories = new List<Category>
                {
                    new Category("Women's clothing", null, null, parent),
                    new Category("Athletic wear", null, null, parent),
                    new Category("Electronics", null, null, null),
                    new Category("Home and garden", null, null, null),
                    new Category("Sports and outdoor", null, null, null),
                    new Category("Health and beauty", null, null, null),
                };

                _context.Categories.AddRange(categories);

                await _context.SaveChangesAsync();
            }
        }
    }

}
