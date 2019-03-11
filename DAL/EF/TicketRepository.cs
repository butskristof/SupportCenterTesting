using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SC.BL.Domain;

namespace SC.DAL.EF
{
	public class TicketRepository : ITicketRepository
	{
		private SupportCenterDbContext ctx = null;

		public TicketRepository()
		{
			ctx = new SupportCenterDbContext();
		}
		
		public IEnumerable<Ticket> ReadTickets()
		{
			IEnumerable<Ticket> tickets = ctx.Tickets
											 .Include(t => t.Responses) // eager-loading 'Responses'
											 .AsEnumerable();
			return tickets;
		}

		public Ticket CreateTicket(Ticket ticket)
		{
			ctx.Tickets.Add(ticket);
			ctx.SaveChanges();

			return ticket;
		}

		public Ticket ReadTicket(int ticketNumber)
		{
			//Ticket ticket = ctx.Tickets.Single(t => t.TicketNumber == ticketNumber);
			Ticket ticket = ctx.Tickets.Find(ticketNumber);
			return ticket;
		}

		public void UpdateTicket(Ticket ticket)
		{
			ctx.Tickets.Update(ticket);
			ctx.SaveChanges();
		}

		public void DeleteTicket(int ticketNumber)
		{
			Ticket ticketToDelete = this.ReadTicket(ticketNumber);
			ctx.Tickets.Remove(ticketToDelete);
			ctx.SaveChanges();
		}

		public IEnumerable<TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
		{
			IEnumerable<TicketResponse> responses = ctx.TicketResponses
													   .Where(response => response.Ticket.TicketNumber == ticketNumber)
													   .AsEnumerable();

			return responses;
			
			/* Explicit-loading */
			//Ticket ticket = ctx.Tickets.Find(ticketNumber);
			//ctx.Entry(ticket).Collection(t => t.Responses).Load();
			//return ticket.Responses;
		}

		public TicketResponse CreateTicketResponse(TicketResponse response)
		{
			ctx.TicketResponses.Add(response);
			ctx.SaveChanges();

			return response;
		}
	}
}