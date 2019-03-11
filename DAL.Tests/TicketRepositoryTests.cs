using System.Collections.Generic;
using System.Linq;
using SC.BL.Domain;
using SC.DAL.EF;
using Xunit;

namespace DAL.Tests
{
	public class TicketRepositoryTests
	{
		[Fact]
		public void ReadTickets_ReturnsListOfTickets()
		{
			// arrange
			using (var factory = new SupportCenterDbContextFactory())
			{
				IEnumerable<Ticket> result = null;
				// get ctx
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new TicketRepository(ctx);
					result = repo.ReadTickets().ToList();
				}
				
				// assert
				Assert.NotNull(result);
				Assert.Equal(TestTicketsProvider.GetTestTickets().Count, result.Count());
			}
		}
	}
}