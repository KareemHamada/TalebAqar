namespace DAL.Models;

public partial class TbPropertyImage : IHasCurrentState
{
    [Key]
    public int ImageId { get; set; }

    [Column("ImageURL")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

	public bool CurrentState { get; set; }


	public int? PropertyId { get; set; }


    [ForeignKey("PropertyId")]
    [InverseProperty("PropertyImages")]
    public virtual TbProperty? Property { get; set; }
}