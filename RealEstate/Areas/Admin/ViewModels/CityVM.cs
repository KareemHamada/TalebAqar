namespace RealEstate.Areas.Admin.ViewModels
{
    public class CityVM
    {
        public int CityId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "أدخل اسم المدينة")]

        public string CityName { get; set; } = null!;

		public bool CurrentState { get; set; }
		[Required(ErrorMessage = "أختر محافظة")]
		public int? GovernorateId { get; set; }

        public virtual TbGovernorate? Governorate { get; set; }

        public virtual ICollection<TbAddress> TbAddresses { get; set; } = new List<TbAddress>();

        public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
    }
}

