namespace RealEstate.Areas.AdminArea289.ViewModels
{
    public class CurrencyVM
    {
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "من فضلك أدخل اسم العملة")]
        public string CurrencyName { get; set; }

        public bool CurrentState { get; set; }

        public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
    }
}
