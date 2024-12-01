namespace DAL.Models;

public partial class TbType : IHasCurrentState
{
    [Key]
    public int TypeId { get; set; }

    [StringLength(50)]
    public string TypeName { get; set; }
    public string? TypeImage { get; set; }

    public bool CurrentState { get; set; }

    [InverseProperty("Type")]
    public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
}