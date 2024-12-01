namespace RealEstate.ViewModels
{
	public class ForgetPasswordVM
	{
        [EmailAddress(ErrorMessage = "ادخل الاميل بشكل صحيح")]
        public string Email { get; set; }
	}
}
