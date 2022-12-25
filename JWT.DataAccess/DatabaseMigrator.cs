using Microsoft.EntityFrameworkCore;

namespace JWT.DataAccess
{
    internal class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly ApplicationDbContext _dbContext;
        public DatabaseMigrator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Migrate()
        {
            _dbContext.Database.Migrate();
        }
    }
}
