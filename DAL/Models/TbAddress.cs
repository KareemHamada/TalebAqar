

namespace DAL.Models;
public partial class TbAddress : IHasCurrentState
{
    [Key]
    public int AddressId { get; set; }
	[StringLength(100)]

	public string AddressName { get; set; } = null!;

    public int? CityId { get; set; }

    public bool CurrentState { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("TbAddresses")]
    public virtual TbCity? City { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
}