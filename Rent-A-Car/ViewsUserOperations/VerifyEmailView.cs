using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.ViewsUserOperations
{
	public class VerifyEmailView
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }
	}
}
