namespace DAL.Models;

public partial class TbGovernorate : IHasCurrentState
{
    [Key]
	public int GovernorateId { get; set; }


    [StringLength(50)]
	[Required(ErrorMessage = "من فضلك أدخل اسم المحافظة")]

	public string GovernorateName { get; set; } = null!;

	public bool CurrentState { get; set; }

    [InverseProperty("Governorate")]
    public virtual ICollection<TbCity> TbCities { get; set; } = new List<TbCity>();

    [InverseProperty("Governorate")]
    public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
}