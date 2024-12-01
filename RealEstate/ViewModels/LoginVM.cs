namespace RealEstate.ViewModels
{
	public class LoginVM
	{
        [EmailAddress(ErrorMessage = "ادخل الاميل بشكل صحيح")]
        public string Email { get; set; }
        [DataType(DataType.Password, ErrorMessage = "ادخل كلمة السر صحيحة")]
        public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
