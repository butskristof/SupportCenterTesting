using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SC.BL;
using SC.BL.Domain;
using SC.UI.Web.MVC.Models;

namespace SC.UI.Web.MVC.Controllers
{
	public class TicketController : Controller
	{
		private ITicketManager mgr;

//		public TicketController()
//		{
//			mgr = new TicketManager();
//		}

		public TicketController(ITicketManager mgr)
		{
			this.mgr = mgr;
		}

		// GET: /Ticket
		public IActionResult Index()
		{
			IEnumerable<Ticket> tickets = mgr.GetTickets();
			return View(tickets);
		}
		
		// GET: /Ticket/Details/<ticket_number>
		public IActionResult Details(int id)
		{
			Ticket ticket = mgr.GetTicket(id);
			
			if(ticket.Responses != null)
				ViewBag.Responses = ticket.Responses;
			else
				ViewBag.Responses = mgr.GetTicketResponses(ticket.TicketNumber);
			
			return View(ticket);
		}
		
		// GET: /Ticket/Edit/<ticket_number>
		public IActionResult Edit(int id)
		{
			Ticket ticket = mgr.GetTicket(id);
			return View(ticket);
		}
		// POST: /Ticket/Edit/<ticket_number>
		[HttpPost]
		public IActionResult Edit(int id, Ticket ticket)
		{
			if(!ModelState.IsValid)
				return View(ticket);
			
			mgr.ChangeTicket(ticket);
			return RedirectToAction("Details", new {id = ticket.TicketNumber});
		}
		
		// GET: /Ticket/Create
		public IActionResult Create()
		{
			return View();
		}
		// POST: /Ticket/Create
		[HttpPost]
		public IActionResult Create(/*int accId, string problem*/CreateTicketViewModel createTicket)
		{
			Ticket newTicket = mgr.AddTicket(/*accId*/createTicket.AccId, /*problem*/createTicket.Problem);
			return RedirectToAction("Details", new {id = newTicket.TicketNumber});
		}
		
		// GET: /Ticket/Delete/<ticket_number>
		public IActionResult Delete(int id)
		{
			Ticket ticket = mgr.GetTicket(id);
			return View(ticket);
		}
		// POST: /Ticket/Delete/<ticket_number>
		[HttpPost]
		public IActionResult DeleteConfirmed(int id)
		{
			mgr.RemoveTicket(id);
			return RedirectToAction("Index");
		}
	}
}