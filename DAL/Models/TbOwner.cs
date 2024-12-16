namespace DAL.Models;

public partial class TbOwner : IHasCurrentState
{
    [Key]
    [Column("OwnerID")]
    public int OwnerId { get; set; }
	public string FullName { get; set; } = null!;

    public string? Email { get; set; }
	[StringLength(maximumLength: 11, MinimumLength = 11, ErrorMessage = "رقم التليفون يجب الا يقل او يزيد عن 11 رقم")]
	public string PhoneNumber { get; set; } = null!;

    public bool HasWhatsApp { get; set; }


    [StringLength(255)]

	public string? Address { get; set; }

    public bool CurrentState { get; set; }

    [InverseProperty("Owner")]
    public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
}