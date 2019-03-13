using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SC.BL;
using SC.BL.Domain;
using SC.DAL.Tests;
using SC.UI.Web.MVC.Controllers;
using Xunit;

namespace SC.UI.Web.MVC.Tests
{
	public class TicketControllerTests
	{
		[Fact]
		public void Index_WithTicketsAvailable_ReturnsViewResultWithListOfTickets()
		{
			// arrange
			var mgr = new Mock<ITicketManager>();
			mgr
				.Setup(x => x.GetTickets())
				.Returns(TestTicketsProvider.GetTestTickets);
			var controller = new TicketController(mgr.Object);

			// act
			var result = controller.Index();

			// assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<Ticket>>(viewResult.ViewData.Model);
			Assert.Equal(3, model.Count());
		}

		[Fact]
		public void Details_WithValidId_ReturnsViewResultWithTicket()
		{
			// arrange
			int ticketId = 1;
			var mgr = new Mock<ITicketManager>();
			mgr
				.Setup(x => x.GetTicket(ticketId))
				.Returns(TestTicketsProvider.GetTestTickets().FirstOrDefault(ticket => ticket.TicketNumber == ticketId));
			var controller = new TicketController(mgr.Object);
			var ticketThatShouldBeReturned = TestTicketsProvider.GetTestTickets().First(t => t.TicketNumber == ticketId);
			
			// act
			var result = controller.Details(ticketId);
			
			// assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<Ticket>(viewResult.Model);
			Assert.Equal(ticketThatShouldBeReturned.TicketNumber, model.TicketNumber);
			Assert.Equal(ticketThatShouldBeReturned.AccountId, model.AccountId);
			Assert.Equal(ticketThatShouldBeReturned.Text, model.Text);
			Assert.Equal(ticketThatShouldBeReturned.DateOpened, model.DateOpened);
			Assert.Equal(ticketThatShouldBeReturned.State, model.State);
//			Assert.Equal(ticketThatShouldBeReturned.Responses, model.Responses);
//			bool responesEqual = ticketThatShouldBeReturned.Responses.SequenceEqual(model.Responses);
//			Assert.True(responesEqual);
		}

//		[Fact]
//		public void Details_WithInvalidId_ThrowsNullReferenceException()
//		{
//			// arrange
//			int ticketId = 1;
//			var mgr = new Mock<ITicketManager>();
//			mgr
//				.Setup(x => x.GetTicket(ticketId))
//				.Returns((Ticket) null);
//			var controller = new TicketController(mgr.Object);
//			
//			// act
//			Action result = () => controller.Details(ticketId);
//			
//			// assert
//			Assert.Throws<NullReferenceException>(result);
//		}

		[Fact]
		public void Details_WithInvalidId_ReturnsNullAsViewResult()
		{
			// arrange
			int ticketId = 1;
			var mgr = new Mock<ITicketManager>();
			mgr
				.Setup(x => x.GetTicket(ticketId))
				.Returns((Ticket) null);
			var controller = new TicketController(mgr.Object);
			
			// act
			var result = controller.Details(ticketId);
			
			// assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Null(viewResult.Model);
		}
		
		[Fact]
		public void Edit_WithValidId_ReturnsViewResultWithTicket()
		{
			// arrange
			int ticketId = 1;
			var mgr = new Mock<ITicketManager>();
			mgr
				.Setup(x => x.GetTicket(ticketId))
				.Returns(TestTicketsProvider.GetTestTickets().FirstOrDefault(ticket => ticket.TicketNumber == ticketId));
			var controller = new TicketController(mgr.Object);
			var ticketThatShouldBeReturned = TestTicketsProvider.GetTestTickets().First(t => t.TicketNumber == ticketId);
			
			// act
			var result = controller.Edit(ticketId);
			
			// assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<Ticket>(viewResult.Model);
			Assert.Equal(ticketThatShouldBeReturned.TicketNumber, model.TicketNumber);
			Assert.Equal(ticketThatShouldBeReturned.AccountId, model.AccountId);
			Assert.Equal(ticketThatShouldBeReturned.Text, model.Text);
			Assert.Equal(ticketThatShouldBeReturned.DateOpened, model.DateOpened);
			Assert.Equal(ticketThatShouldBeReturned.State, model.State);
//			Assert.Equal(ticketThatShouldBeReturned.Responses, model.Responses);
//			bool responesEqual = ticketThatShouldBeReturned.Responses.SequenceEqual(model.Responses);
//			Assert.True(responesEqual);
		}
		
		[Fact]
		public void Edit_WithInvalidId_ReturnsNullAsViewResult()
		{
			// arrange
			int ticketId = 1;
			var mgr = new Mock<ITicketManager>();
			mgr
				.Setup(x => x.GetTicket(ticketId))
				.Returns((Ticket) null);
			var controller = new TicketController(mgr.Object);
			
			// act
			var result = controller.Edit(ticketId);
			
			// assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Null(viewResult.Model);
		}
		
	}
}