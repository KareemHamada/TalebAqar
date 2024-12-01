using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Areas.Admin.ViewModels
{
	public class OwnerVM
	{
		public int OwnerId { get; set; }

		[StringLength(100)]
		[Required(ErrorMessage = "أدخل الاسم")]

		public string FullName { get; set; } = null!;


		[EmailAddress(ErrorMessage = "Please enter email addresss")]
		public string? Email { get; set; }

		[StringLength(maximumLength: 11, MinimumLength = 11,ErrorMessage ="رقم التليفون يجب الا يقل او يزيد عن 11 رقم")]
		[Required(ErrorMessage = "أدخل رقم التليفون")]
		[Phone(ErrorMessage = "أدخل رقم تليفون صحيح")]
		public string PhoneNumber { get; set; } = null!;


		[StringLength(255)]
		[Required(ErrorMessage = "أدخل العنوان")]
		public string? Address { get; set; }

		public bool CurrentState { get; set; }

		public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
	}
}
