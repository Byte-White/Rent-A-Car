using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Rent_A_Car.Models
{
	public class User : IdentityUser
	{
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
        [Column(TypeName = "varchar(10)")] 
		public string EGN { get; set; }
		public string PhoneNumber { get; set; }
		public ICollection<Request> Requests { get; set; }
		public User()
		{

		}
	}
}
