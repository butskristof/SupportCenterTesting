using System;
using System.Collections.Generic;
using SC.BL.Domain;

namespace SC.DAL.Tests
{
	public static class TestTicketsProvider
	{
		public static List<Ticket> GetTestTickets()
		{
			var tickets = new List<Ticket>();
			
            // Create first ticket with three responses
            Ticket t1 = new Ticket()
            {
	            TicketNumber = 1,
                AccountId = 1,
                Text = "Ik kan mij niet aanmelden op de webmail",
                DateOpened = new DateTime(2017, 9, 9, 13, 5, 59),
                State = TicketState.Open,
                Responses = new List<TicketResponse>()
            };
            tickets.Add(t1);

            TicketResponse t1r1 = new TicketResponse()
            {
                Ticket = t1,
                Text = "Account was geblokkeerd",
                Date = new DateTime(2017, 9, 9, 13, 24, 48),
                IsClientResponse = false
            };
            t1.Responses.Add(t1r1);

            TicketResponse t1r2 = new TicketResponse()
            {
                Ticket = t1,
                Text = "Account terug in orde en nieuw paswoord ingesteld",
                Date = new DateTime(2017, 9, 9, 13, 29, 11),
                IsClientResponse = false
            };
            t1.Responses.Add(t1r2);

            TicketResponse t1r3 = new TicketResponse()
            {
                Ticket = t1,
                Text = "Aanmelden gelukt en paswoord gewijzigd",
                Date = new DateTime(2017, 9, 10, 7, 22, 36),
                IsClientResponse = true
            };
            t1.Responses.Add(t1r3);
            t1.State = TicketState.Closed;

            // Create second ticket with one response
            Ticket t2 = new Ticket()
            {
	            TicketNumber = 2,
                AccountId = 1,
                Text = "Geen internetverbinding",
                DateOpened = new DateTime(2017, 11, 5, 9, 45, 13),
                State = TicketState.Open,
                Responses = new List<TicketResponse>()
            };
            tickets.Add(t2);

            TicketResponse t2r1 = new TicketResponse()
            {
                Ticket = t2,
                Text = "Controleer of de kabel goed is aangesloten",
                Date = new DateTime(2017, 11, 5, 11, 25, 42),
                IsClientResponse = false
            };
            t2.Responses.Add(t2r1);
            t2.State = TicketState.Answered;

            // Create hardware ticket without response
            HardwareTicket ht1 = new HardwareTicket()
            {
	            TicketNumber = 3,
                AccountId = 2,
                Text = "Blue screen!",
                DateOpened = new DateTime(2017, 12, 14, 19, 5, 2),
                State = TicketState.Open,
                DeviceName = "PC-123456"
            };
            tickets.Add(ht1);

			return tickets;
		}
	}
}