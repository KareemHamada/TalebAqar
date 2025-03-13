namespace DAL.Models;

public partial class TbProperty : IHasCurrentState
{
    [Key]
	public int PropertyId { get; set; }

    public int Area { get; set; }

    public int? Bedrooms { get; set; }

    public int? Bathrooms { get; set; }

    public int Price { get; set; }

    public int CostPrice { get; set; }

    public string? PropertyContractImage { get; set; }

    public bool Negotiable { get; set; }

	// Change these to string
	[ForeignKey(nameof(CreatedByUser))]
	public string CreatedBy { get; set; } // Changed to string


	public DateTime CreatedDate { get; set; }

	[ForeignKey(nameof(UpdatedByUser))]
	public string? UpdatedBy { get; set; } // Changed to string

	public DateTime? UpdatedDate { get; set; }


    public int PostDays { get; set; }


    public bool IsSoldOrRenteled { get; set; }
	public DateTime? SoldOrRenteledDate { get; set; }

    public string? Description { get; set; }
    public string? StyledDescription { get; set; }

    public bool CurrentState { get; set; }

    public int? StatusId { get; set; }

    public int? TypeId { get; set; }

    public int? AddressId { get; set; }

    public int? CityId { get; set; }

    public int? OwnerId { get; set; }

    public int? GovernorateId { get; set; }

    public int NumOfViews { get; set; }
    public int? CurrencyId { get; set; }

    public int NumOfAdvertisement { get; set; }

    [Column(TypeName = "decimal(18, 15)")]
    public decimal? Longitude { get; set; }//

    [Column(TypeName = "decimal(18, 15)")]
    public decimal? Latitude { get; set; }//

    public int? FloorNum { get; set; }

    public bool Furnished { get; set; }

    public int? Insurance { get; set; }

    [StringLength(100)]
    public string? Notes { get; set; }



    [ForeignKey("AddressId")]
    [InverseProperty("TbProperties")]
    public virtual TbAddress? Address { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("TbProperties")]
    public virtual TbCity? City { get; set; }

    [ForeignKey("GovernorateId")]
    [InverseProperty("TbProperties")]
    public virtual TbGovernorate? Governorate { get; set; }

    [ForeignKey("OwnerId")]
    [InverseProperty("TbProperties")]
    public virtual TbOwner? Owner { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("TbProperties")]
    public virtual TbStatus? Status { get; set; }

    [InverseProperty("Property")]
    public virtual ICollection<TbPropertyImage> PropertyImages { get; set; } = new List<TbPropertyImage>();

    [ForeignKey("TypeId")]
    [InverseProperty("TbProperties")]
    public virtual TbType? Type { get; set; }


    [ForeignKey("CurrencyId")]
    [InverseProperty("TbProperties")]
    public virtual TbCurrency? Currency { get; set; }

    // Navigation properties to ApplicationUser
    public virtual ApplicationUser? CreatedByUser { get; set; }
	public virtual ApplicationUser? UpdatedByUser { get; set; }


}