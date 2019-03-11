using System.ComponentModel.DataAnnotations;

namespace SC.BL.Domain
{
	public class HardwareTicket : Ticket
	{
		[RegularExpression("^(PC-)[0-9]+")]
		public string DeviceName { get; set; }
	}
}
