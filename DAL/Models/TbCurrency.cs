namespace DAL.Models
{
    public partial class TbCurrency : IHasCurrentState
    {
        [Key]
        public int CurrencyId { get; set; }

        [StringLength(50)]
        public string CurrencyName { get; set; }


        public bool CurrentState { get; set; }


        [InverseProperty("Currency")]
        public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
    }
}
