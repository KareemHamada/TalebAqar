namespace RealEstate.Areas.AdminArea289.ViewModels
{
	public class StatusVM
	{
		public int StatusId { get; set; }

		[StringLength(50)]
		[Required(ErrorMessage = "أدخل حالة العقار")]
		public string? StatusName { get; set; }

		public bool CurrentState { get; set; }

	}
}
