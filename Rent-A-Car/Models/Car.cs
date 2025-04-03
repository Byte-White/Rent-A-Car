using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.Models
{
	public class Car
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Brand { get; set; }
		[Required]
		public string Model { get; set; }
		[Required]
		public DateTime YearOfProduction { get; set; }
		[Required]
		public int Seats { get; set; }
		[Required]
		public double PricePerDay { get; set; }
		
		public string Description { get; set; }
	}
}
