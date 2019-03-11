using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Proxies; // NuGet-package!
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug; // NuGet-package!
using SC.BL.Domain;

namespace SC.DAL.EF
{
	internal class SupportCenterDbContext : DbContext
	{
		public SupportCenterDbContext()
		{
			SupportCenterDbInitializer.Initialize(this, dropCreateDatabase: true);
		}
		
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<HardwareTicket> HardwareTickets { get; set; }
		public DbSet<TicketResponse> TicketResponses { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=SupportCenterDb_EFCodeFirst.db");
			
			// configure logging-information
			optionsBuilder.UseLoggerFactory(new LoggerFactory(
				new[] { new DebugLoggerProvider(
					(category, level) => category == DbLoggerCategory.Database.Command.Name
										 && level == LogLevel.Information
				)}
			));

			// configure lazy-loading: requires ALL navigation-properties to be 'virtual'!!
			//optionsBuilder.UseLazyLoadingProxies();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Ticket>().HasIndex(t => t.State);

			modelBuilder.Entity<TicketResponse>().Property<int>("TicketFK");
			modelBuilder.Entity<TicketResponse>().HasOne(tr => tr.Ticket).WithMany(t => t.Responses)
												 .HasForeignKey("TicketFK");
		}
	}
}