using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using SC.DAL;
using SC.BL.Domain;

namespace SC.BL
{
  public class TicketManager : ITicketManager
  {
    private readonly ITicketRepository repo;

    public TicketManager()
    {
      //repo = new TicketRepositoryHC();
      repo = new DAL.EF.TicketRepository();
    }

    public IEnumerable<Ticket> GetTickets()
    {
      return repo.ReadTickets();
    }

    public Ticket GetTicket(int ticketNumber)
    {
      return repo.ReadTicket(ticketNumber);
    }

    public Ticket AddTicket(int accountId, string question)
    {
      Ticket t = new Ticket()
      {
        AccountId = accountId,
        Text = question,
        DateOpened = DateTime.Now,
        State = TicketState.Open,
        //Responses = new List<TicketResponse>()
      };
      return this.AddTicket(t);
    }

    public Ticket AddTicket(int accountId, string device, string problem)
    {
      Ticket t = new HardwareTicket()
      {
        AccountId = accountId,
        Text = problem,
        DateOpened = DateTime.Now,
        State = TicketState.Open,
        //Responses = new List<TicketResponse>(),
        DeviceName = device
      };
      return this.AddTicket(t);
    }

    private Ticket AddTicket(Ticket ticket)
    {
      this.Validate(ticket);
      return repo.CreateTicket(ticket);
    }

    public void ChangeTicket(Ticket ticket)
    {
      this.Validate(ticket);
      repo.UpdateTicket(ticket);
    }

    public void RemoveTicket(int ticketNumber)
    {
      repo.DeleteTicket(ticketNumber);
    }

    public IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber)
    {
      return repo.ReadTicketResponsesOfTicket(ticketNumber);
    }

    public TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse)
    {
      Ticket ticketToAddResponseTo = this.GetTicket(ticketNumber);
      if (ticketToAddResponseTo != null)
      {
        // Create response
        TicketResponse newTicketResponse = new TicketResponse();
        newTicketResponse.Date = DateTime.Now;
        newTicketResponse.Text = response;
        newTicketResponse.IsClientResponse = isClientResponse;
        newTicketResponse.Ticket = ticketToAddResponseTo;

        // Add response to ticket
        var responses = this.GetTicketResponses(ticketNumber);
        if (responses != null)
          ticketToAddResponseTo.Responses = responses.ToList();
        else
          ticketToAddResponseTo.Responses = new List<TicketResponse>();
        ticketToAddResponseTo.Responses.Add(newTicketResponse);

        // Change state of ticket
        if (isClientResponse)
          ticketToAddResponseTo.State = TicketState.ClientAnswer;
        else
          ticketToAddResponseTo.State = TicketState.Answered;

        // Validatie van ticketResponse en ticket afdwingen!!!
        this.Validate(newTicketResponse);
        this.Validate(ticketToAddResponseTo);
        
        repo.CreateTicketResponse(newTicketResponse); // if repo uses EF: not necessary because response is added to responses collection of ticket
        repo.UpdateTicket(ticketToAddResponseTo);

        return newTicketResponse;
      }
      else
        throw new ArgumentException("Ticketnumber '" + ticketNumber + "' not found!");
    }
    
    #region validation-methods
    private void Validate(Ticket ticket)
    {
      List<ValidationResult> errors = new List<ValidationResult>();

      bool valid = Validator.TryValidateObject(ticket, new ValidationContext(ticket), errors, validateAllProperties: true);

      if (!valid)
        throw new ValidationException("Ticket not valid!");
    }

    private void Validate(TicketResponse response)
    {
      Validator.ValidateObject(response, new ValidationContext(response), validateAllProperties: true);
    }
    #endregion
  }
}
