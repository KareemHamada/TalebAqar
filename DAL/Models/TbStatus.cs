public partial class TbStatus : IHasCurrentState
{
    [Key]
    public int StatusId { get; set; }
	[StringLength(50)]

	public string StatusName { get; set; }

    public bool CurrentState { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
}