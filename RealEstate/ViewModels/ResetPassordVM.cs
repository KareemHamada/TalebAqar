namespace RealEstate.ViewModels
{
	public class ResetPassordVM
	{
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and Confirm password does not match")]
		public string ConfirmPassword { get; set; }

		public string Email { get; set; }

		public string Token { get; set; }

	}
}
