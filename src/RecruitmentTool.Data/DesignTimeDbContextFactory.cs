namespace RecruitmentTool.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RecruitmentToolDbContext>
    {
        public RecruitmentToolDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            var builder = new DbContextOptionsBuilder<RecruitmentToolDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new RecruitmentToolDbContext(builder.Options);
        }
    }
}
