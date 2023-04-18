using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class States
	{[Key]
		public int StateID { get; set; }
		public string State { get; set; }
		public string Abbreviation { get; set; }
		
	}
}
