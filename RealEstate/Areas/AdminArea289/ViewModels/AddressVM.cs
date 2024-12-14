namespace RealEstate.Areas.AdminArea289.ViewModels
{
	public class AddressVM
	{
		public int AddressId { get; set; }

		[StringLength(100)]
		[Required(ErrorMessage = "أدخل العنوان")]

		public string AddressName { get; set; } = null!;
		[Required(ErrorMessage = "أختر المدينة")]
		public int? CityId { get; set; }

		public bool CurrentState { get; set; }

		public virtual TbCity? City { get; set; }

		public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
	}
}
