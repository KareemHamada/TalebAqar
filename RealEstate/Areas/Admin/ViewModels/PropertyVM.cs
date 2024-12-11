
namespace RealEstate.Areas.Admin.ViewModels
{
	public class PropertyVM
	{
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "أدخل المساحة")]
        public int Area { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        [Required(ErrorMessage = "أدخل السعر")]
        public int Price { get; set; }

        [Required(ErrorMessage = "أدخل تكلفة الاعلان")]
        public int CostPrice { get; set; }

        public bool Negotiable { get; set; }


		public string CreatedBy { get; set; } // Changed to string

		public DateTime CreatedDate { get; set; }

		public string? UpdatedBy { get; set; } // Changed to string

		public DateTime? UpdatedDate { get; set; }

		public bool IsSoldOrRenteled { get; set; }
		public DateTime? SoldOrRenteledDate { get; set; }

        public string? Description { get; set; }

        public bool CurrentState { get; set; }

        public int? StatusId { get; set; }

        public int? TypeId { get; set; }

        public int? AddressId { get; set; }

        public int? CityId { get; set; }

        public int? OwnerId { get; set; }

        public int? GovernorateId { get; set; }

        public int? NumOfViews { get; set; }

        public int NumOfAdvertisement { get; set; }

        [Column(TypeName = "decimal(18, 15)")]
        public decimal? Longitude { get; set; }

        [Column(TypeName = "decimal(18, 15)")]
        public decimal? Latitude { get; set; }

        public int? FloorNum { get; set; }

        public bool Furnished { get; set; }

        public int? Insurance { get; set; }

        [StringLength(100)]
        public string? Notes { get; set; }
        public virtual TbAddress? Address { get; set; }

        public virtual TbCity? City { get; set; }

        public virtual TbGovernorate? Governorate { get; set; }

        public virtual TbOwner? Owner { get; set; }

        public virtual TbStatus? Status { get; set; }

        public virtual ICollection<TbPropertyImage> PropertyImages { get; set; } = new List<TbPropertyImage>();

        public virtual TbType? Type { get; set; }



		// Navigation properties to ApplicationUser
		public virtual ApplicationUser? CreatedByUser { get; set; }
		public virtual ApplicationUser? UpdatedByUser { get; set; }




        // Computed property for truncated description
        public string TruncatedDescription =>
            Description?.Length > 40
                ? Description.Substring(0, 40) + "..."
                : Description ?? string.Empty;
    }
}
