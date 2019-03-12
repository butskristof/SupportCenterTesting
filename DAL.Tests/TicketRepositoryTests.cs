using System;
using System.Collections.Generic;
using System.Linq;
using SC.BL.Domain;
using SC.DAL.EF;
using Xunit;

namespace SC.DAL.Tests
{
	public class TicketRepositoryTests
	{
		[Fact]
		public void ReadTickets_WithTicketsInDb_ReturnsListOfTickets()
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

		[Fact]
		public void ReadTickets_WithoutTicketsInDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new SupportCenterDbContextFactory())
			{
				IEnumerable<Ticket> result = null;
				// get ctx
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new TicketRepository(ctx);
					result = repo.ReadTickets().ToList();
				}
				
				// assert
				Assert.NotNull(result);
				Assert.Empty(result);
			}
		}

		[Fact]
		public void CreateTicket_WithTicket_AddsTicketNumber()
		{
			// arrange
			using (var factory = new SupportCenterDbContextFactory())
			{
				Ticket toAdd = new Ticket()
				{
					AccountId = 1, DateOpened = DateTime.Now, Responses = new List<TicketResponse>(),
					State = TicketState.Open, Text = "Test Ticket"
				};
				Ticket result = null;
				// get ctx
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new TicketRepository(ctx);
					result = repo.CreateTicket(toAdd);
				}
				
				// assert
				Assert.NotNull(result);
//				Assert.Null(result.TicketNumber);
				Assert.Equal(TestTicketsProvider.GetTestTickets().Count + 1, result.TicketNumber);
			}
		}
		
		[Fact]
		public void ReadTicket_WithValidId_ReturnsTicket()
		{
			// arrange
			using (var factory = new SupportCenterDbContextFactory())
			{
				Ticket result = null;
				// get ctx
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new TicketRepository(ctx);
					result = repo.ReadTicket(1);
				}
				
				// assert
				Assert.NotNull(result);
				Assert.Equal(1, result.TicketNumber);
			}
		}

		[Fact]
		public void ReadTicket_WithInvalidId_ReturnsNull()
		{
			// arrange
			using (var factory = new SupportCenterDbContextFactory())
			{
				Ticket result = null;
				// get ctx
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new TicketRepository(ctx);
					result = repo.ReadTicket(1);
				}
				
				// assert
				Assert.Null(result);
			}
		}
		
		[Fact]
		public void DeleteTicket_WithValidId_RemovesTicketFromDatabase()
		{
			// arrange
			using (var factory = new SupportCenterDbContextFactory())
			{
				// get ctx
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new TicketRepository(ctx);
					repo.DeleteTicket(1);
				}
				
				// assert
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new TicketRepository(ctx);
					Ticket result = repo.ReadTicket(1);
					
					Assert.Null(result);
				}
			}
		}
		
		[Fact]
		public void DeleteTicket_WithInvalidId_ThrowsNullException()
		{
			// arrange
			using (var factory = new SupportCenterDbContextFactory())
			{
				// get ctx
				Action result = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new TicketRepository(ctx);
					result = () => repo.DeleteTicket(1);
					
					// assert
					Assert.Throws<ArgumentNullException>(result);
				}
			}
		}
	}
}