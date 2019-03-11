using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SC.BL;
using SC.BL.Domain;
using SC.UI.Web.MVC.Models;

namespace SC.UI.Web.MVC.Controllers.Api
{
	[ApiController]
	[Route("api/[controller]")]
	public class TicketResponsesController : ControllerBase
	{
//		private ITicketManager mgr = new TicketManager();
		private ITicketManager mgr;

		public TicketResponsesController(ITicketManager mgr)
		{
			this.mgr = mgr;
		}

		// GET: api/TicketResponses?ticketNumber=5
		[HttpGet()]
		public IActionResult Get(int ticketNumber)
		{
			var responses = mgr.GetTicketResponses(ticketNumber);

			if (responses == null || !responses.Any())
				return NoContent(); //of: StatusCode(StatusCodes.Status204NoContent);

			return Ok(responses);
		}
		
		// POST: api/TicketResponse
		[HttpPost]
		public IActionResult Post(NewTicketResponseDTO response)
		{
			TicketResponse createdResponse = mgr.AddTicketResponse(response.TicketNumber
				, response.ResponseText, response.IsClientResponse);

			if (createdResponse == null)
				return BadRequest("Er is iets misgelopen bij het registreren van het antwoord!");

			//return CreatedAtAction("Get", new { id = createdResponse.Id }, null);
			//return CreatedAtAction("Get", new { id = createdResponse.Id }, createdResponse); // -> serializing: circulaire referentie!!
			
			/* Oplossing 'DTO' */
			TicketResponseDTO responseData = new TicketResponseDTO()
			{
				Id = createdResponse.Id,
				Text = createdResponse.Text,
				Date = createdResponse.Date,
				IsClientResponse = createdResponse.IsClientResponse,
				TicketNumberOfTicket = createdResponse.Ticket.TicketNumber
			};
			return CreatedAtAction("Get", new { id = responseData.Id }, responseData);
		}
	}
}