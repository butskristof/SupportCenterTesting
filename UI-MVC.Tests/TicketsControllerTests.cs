using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SC.BL;
using SC.BL.Domain;
using SC.UI.Web.MVC.Controllers;
using SC.UI.Web.MVC.Controllers.Api;
using Xunit;

namespace SC.UI.Web.MVC.Tests
{
	public class TicketsControllerTests
	{
		[Fact]
		public void PutTicketStateToClosed_WithInvalidId_ReturnsNotFound()
		{
			// arrange
			int ticketId = 1;
			var mgr = new Mock<ITicketManager>();
			mgr
				.Setup(x => x.GetTicket(ticketId))
				.Returns((Ticket) null);
			var controller = new TicketsController(mgr.Object);
			
			// act
			var result = controller.PutTicketStateToClosed(ticketId);
			
			// assert
			var notFoundResult = Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void PutTicketStateToClosed_WithValidId_ReturnsNoContentAndUpdatesTicket()
		{
			// arrange
			int ticketId = 1;
			var mgr = new Mock<ITicketManager>();
			mgr
				.Setup(x => x.GetTicket(ticketId))
				.Returns(new Ticket()
				{
					TicketNumber = ticketId,
					State = TicketState.Open
				});
			var controller = new TicketsController(mgr.Object);
			
			// act
			var result = controller.PutTicketStateToClosed(ticketId);
			
			// assert
//			var notFoundResult = Assert.IsType<NotFoundResult>(result);
			Assert.IsType<NoContentResult>(result);
			var ctrl2 = new TicketController(mgr.Object);
			Assert.Equal(TicketState.Closed, ((Ticket)((ViewResult)ctrl2.Details(ticketId)).Model).State);
		}
		
	}
}