using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Moq;
using SC.BL.Domain;
using SC.DAL;
using Xunit;

namespace SC.BL.Tests
{
	public class TicketManagerTests
	{
		[Fact]
		public void GetTickets_WithEmptyRepo_ReturnsEmptyCollection()
		{
			// arrange
			var repo = new Mock<ITicketRepository>();
			repo
				.Setup(x => x.ReadTickets())
				.Returns(new List<Ticket>());
			var mgr = new TicketManager(repo.Object);
			
			// act
			var result = mgr.GetTickets();
			
			// assert
			Assert.Empty(result);
		}

		[Fact]
		public void GetTickets_WithTicketsAvailable_ReturnsListOfTickets()
		{
			// arrange
			var repo = new Mock<ITicketRepository>();
			repo
				.Setup(x => x.ReadTickets())
				.Returns(new List<Ticket>{ new Ticket(), new Ticket() });
			var mgr = new TicketManager(repo.Object);
			
			// act
			var result = mgr.GetTickets();
			
			// assert
			Assert.Equal(repo.Object.ReadTickets().Count(), result.Count());
		}

		[Fact]
		public void GetTicket_WithInvalidId_ReturnsNull()
		{
			// arrange
			var repo = new Mock<ITicketRepository>();
			repo
				.Setup(x => x.ReadTicket(It.IsAny<int>()))
				.Returns((Ticket) null);
			var mgr = new TicketManager(repo.Object);
			
			// act
			var result = mgr.GetTicket(1);
			
			// assert
			Assert.Null(result);
		}

		[Fact]
		public void GetTicket_WithValidId_ReturnsTicket()
		{
			// arrange
			int ticketId = 1;
			var repo = new Mock<ITicketRepository>();
			repo
				.Setup(x => x.ReadTicket(ticketId))
				.Returns(new Ticket(){TicketNumber = 1});
			var mgr = new TicketManager(repo.Object);
			
			// act
			var result = mgr.GetTicket(ticketId);
			
			// assert
			Assert.Equal(ticketId, result.TicketNumber);
		}

		[Fact]
		public void AddTicket_WithValidTicket_ReturnsAddedTicket()
		{
			// arrange
			int accountid = 1;
			string question = "Ticket text";
			int ticketId = 1; // will be added in repo
			
			var repo = new Mock<ITicketRepository>();
			repo
				.Setup(x => x.CreateTicket(It.IsAny<Ticket>()))
				.Returns((Ticket t) =>
				{
					t.TicketNumber = ticketId;
					return t;
				});
			var mgr = new TicketManager(repo.Object);
			
			// act
			var result = mgr.AddTicket(accountid, question);
			
			// assert
			Assert.Equal(ticketId, result.TicketNumber);
		}
		
		[Fact]
		public void AddTicket_WithInvalidText_ReturnsValidationError()
		{
			// arrange
			int accountid = 1;
			string question = "Ticket text should be longer than 100 characters in order to make the verification step of the AddTicket process fail.";
			
			var repo = new Mock<ITicketRepository>();
			// repo won't be called when validation fails 
			// so we don't need any setup here
			var mgr = new TicketManager(repo.Object);
			
			// act
			Action result = () => mgr.AddTicket(accountid, question);

			// assert
			Assert.Throws<ValidationException>(result);
		}
		
		[Fact]
		public void AddHardwareTicket_WithValidTicket_ReturnsAddedTicket()
		{
			// arrange
			int accountid = 1;
			string problem = "Ticket text";
			string device = "PC-1";
			int ticketId = 1; // will be added in repo
			
			var repo = new Mock<ITicketRepository>();
			repo
				.Setup(x => x.CreateTicket(It.IsAny<Ticket>()))
				.Returns((Ticket t) =>
				{
					t.TicketNumber = ticketId;
					return t;
				});
			var mgr = new TicketManager(repo.Object);
			
			// act
			var result = mgr.AddTicket(accountid, device, problem);
			
			// assert
			Assert.Equal(ticketId, result.TicketNumber);
		}
		
		[Fact]
		public void AddHardwareTicket_WithInvalidDeviceName_ReturnsValidationError()
		{
			// arrange
			int accountid = 1;
			string problem = "Ticket text";
			string device = "1";
			
			var repo = new Mock<ITicketRepository>();
			// repo won't be called when validation fails 
			// so we don't need any setup here
			var mgr = new TicketManager(repo.Object);
			
			// act
			Action result = () => mgr.AddTicket(accountid, device, problem);

			// assert
			Assert.Throws<ValidationException>(result);
		}
		
		[Fact]
		public void AddHardwareTicket_WithInvalidText_ReturnsValidationError()
		{
			// arrange
			int accountid = 1;
			string problem = "Ticket text should be longer than 100 characters in order to make the verification step of the AddTicket process fail.";
			string device = "PC-1";
			
			var repo = new Mock<ITicketRepository>();
			// repo won't be called when validation fails 
			// so we don't need any setup here
			var mgr = new TicketManager(repo.Object);
			
			// act
			Action result = () => mgr.AddTicket(accountid, device, problem);

			// assert
			Assert.Throws<ValidationException>(result);
		}
		
		[Fact]
		public void AddHardwareTicket_WithInvalidTextAndDeviceName_ReturnsValidationError()
		{
			// arrange
			int accountid = 1;
			string problem = "Ticket text should be longer than 100 characters in order to make the verification step of the AddTicket process fail.";
			string device = "1";
			
			var repo = new Mock<ITicketRepository>();
			// repo won't be called when validation fails 
			// so we don't need any setup here
			var mgr = new TicketManager(repo.Object);
			
			// act
			Action result = () => mgr.AddTicket(accountid, device, problem);

			// assert
			Assert.Throws<ValidationException>(result);
		}
		
	}
}
