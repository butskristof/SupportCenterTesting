using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SC.DAL.EF;

namespace DAL.Tests
{
	public class SupportCenterDbContextFactory : IDisposable
	{
		private DbConnection _connection;

		private DbContextOptions<SupportCenterDbContext> CreateOptions()
		{
			return new DbContextOptionsBuilder<SupportCenterDbContext>()
				.UseSqlite(_connection)
				.Options;
		}

		public SupportCenterDbContext CreateContext()
		{
			if (_connection == null)
			{
				_connection = new SqliteConnection("DataSource=:memory:");
				_connection.Open();

				var options = CreateOptions();
				using (var ctx = new SupportCenterDbContext(options))
				{
					ctx.Database.EnsureCreated();
					ctx.AddRange(TestTicketsProvider.GetTestTickets());
					ctx.SaveChanges();
				}
			}
			
			return new SupportCenterDbContext(CreateOptions());
		}

		public void Dispose()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection = null;
			}
		}
	}
}