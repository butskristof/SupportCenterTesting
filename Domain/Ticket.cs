using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SC.BL.Domain
{
  public class Ticket
  {
    [Key]
    public int TicketNumber { get; set; }
    public int AccountId { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Er zijn maximaal 100 tekens toegestaan")]
    public string Text { get; set; }
    public DateTime DateOpened { get; set; }
    public TicketState State { get; set; }

    public virtual ICollection<TicketResponse> Responses { get; set; }
  }
}
