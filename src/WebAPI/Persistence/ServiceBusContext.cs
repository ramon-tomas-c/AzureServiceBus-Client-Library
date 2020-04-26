namespace SB.WebAPI.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using SB.WebAPI.Persistence.Entities;

    /// <summary>
    /// Context
    /// </summary>
    public partial class ServiceBusContext : DbContext
    {
        /// <summary>
        /// Constructor used to specify options
        /// </summary>
        /// <param name="options">Context options</param>
        public ServiceBusContext(DbContextOptions<ServiceBusContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Actions on Topics
        /// </summary>
        public virtual DbSet<TopicLog> TopicLog { get; set; }
 
        /// <summary>
        /// Executed during model creation
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServiceBusContext).Assembly);
        }
    }

    /// <summary>
    /// Used by VS to enable design-time services for context types 
    /// that do not have a public default constructor
    /// </summary>
    public class EFToolsDesignFactory
    : IDesignTimeDbContextFactory<ServiceBusContext>
    {
        /// <summary>
        /// Creates a SB Context
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ServiceBusContext CreateDbContext(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString(nameof(ServiceBusContext));

            var optionsBuilder = new DbContextOptionsBuilder<ServiceBusContext>();
            optionsBuilder.UseSqlServer(connectionString,
                options =>
                {
                    options.EnableRetryOnFailure();
                    options.CommandTimeout(600);
                });

            return new ServiceBusContext(optionsBuilder.Options);
        }
    }
}
