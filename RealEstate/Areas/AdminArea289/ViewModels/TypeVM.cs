namespace RealEstate.Areas.AdminArea289.ViewModels
{
	public class TypeVM
	{
		public int TypeId { get; set; }

		[StringLength(50)]
		[Required(ErrorMessage = "أدخل نوع العقار")]
		public string TypeName { get; set; }
        public string? TypeImage { get; set; }

        public bool ShowHomeRoomNumber { get; set; }

        public bool CurrentState { get; set; }

		public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
	}
}
