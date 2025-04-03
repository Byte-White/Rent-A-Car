using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.ViewsUserOperations
{
	public class RegisterView
	{

		[Required(ErrorMessage = "Username is required")]
		[StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
		[Display(Name = "Username")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[Compare("ConfirmPassword", ErrorMessage = "Passwords do not match")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm password is required")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "First name is required")]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "EGN is required")]
		public string EGN { get; set; }

		[Required(ErrorMessage = "Phone number is required")]
		[Display(Name = "Phone number")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }
	}
}
