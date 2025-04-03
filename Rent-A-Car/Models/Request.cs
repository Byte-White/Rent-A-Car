using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.Models
{
	public class Request
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int CarId { get; set; }
		[Required]
		public Car Car { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
		[Required]
		public string UserId { get; set; }
		[Required]
		public User User { get; set; }
	}
}
