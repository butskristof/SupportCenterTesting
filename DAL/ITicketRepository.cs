using System.Collections.Generic;

using SC.BL.Domain;

namespace SC.DAL
{
	public interface ITicketRepository
	{
		#region Ticket
		IEnumerable<Ticket> ReadTickets();
		// CRUD Ticket
		Ticket CreateTicket(Ticket ticket);
		Ticket ReadTicket(int ticketNumber);
		void UpdateTicket(Ticket ticket);
		void DeleteTicket(int ticketNumber);
		#endregion

		#region TicketResponse
		IEnumerable<TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber);
		//// CRUD TicketResponse
		TicketResponse CreateTicketResponse(TicketResponse response);
		#endregion
	}
}
