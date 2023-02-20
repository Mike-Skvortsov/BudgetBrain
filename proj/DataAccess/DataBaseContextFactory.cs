using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess
{
    public class DataBaseContextFactory : IDesignTimeDbContextFactory<DataBaseContext>
    {
        public DataBaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>();
            optionsBuilder.UseSqlServer("Server=tcp:budgetbrain.database.windows.net,1433;Initial Catalog=budgetBrainSQLDatabase;Persist Security Info=False;User ID=MikeSKV;Password=#qwertyS;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;");
			return new DataBaseContext(optionsBuilder.Options);
		}
    }
}
