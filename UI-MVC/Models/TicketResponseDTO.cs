using System;

namespace SC.UI.Web.MVC.Models
{
    public class TicketResponseDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsClientResponse { get; set; }
        public int TicketNumberOfTicket { get; set; }
    }
}