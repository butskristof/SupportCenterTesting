namespace SC.BL.Domain
{
  public enum TicketState : byte
  {
    Open = 1,
    Answered,
    ClientAnswer,
    Closed
  }
}
