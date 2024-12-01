namespace DAL.Models;

public partial class TbCity : IHasCurrentState
{
    [Key]
	public int CityId { get; set; }

	[StringLength(50)]

	public string CityName { get; set; } = null!;

	public int? GovernorateId { get; set; }

	public bool CurrentState { get; set; }

    [ForeignKey("GovernorateId")]
    [InverseProperty("TbCities")]
    public virtual TbGovernorate? Governorate { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<TbAddress> TbAddresses { get; set; } = new List<TbAddress>();

    [InverseProperty("City")]
    public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
}