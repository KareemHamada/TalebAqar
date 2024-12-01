namespace RealEstate.ViewModels
{
	public class RegisterVM
	{
		[Required(ErrorMessage = "ادخل الاسم الاول")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "ادخل الاسم الاخير")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "ادخل الاسم بالكامل بدون مسافات")]
		public string UserName { get; set; }
		[EmailAddress(ErrorMessage = "ادخل الاميل بشكل صحيح")]
		public string Email { get; set; }
		[DataType(DataType.Password,ErrorMessage ="ادخل كلمة السر صحيحة")]
		public string Password { get; set; }
        [DataType(DataType.Password, ErrorMessage = "تاكيد كلمة السر")]
        [Compare(nameof(Password), ErrorMessage = "كلمة السر وتاكيدها ليس متساويان")]
		public string ConfirmPassword { get; set; }




		//public bool IAgree { get; set; }

	}
}
