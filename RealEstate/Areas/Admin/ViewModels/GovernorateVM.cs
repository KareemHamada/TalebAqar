using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Areas.Admin.ViewModels
{
    public class GovernorateVM
    {
        public int GovernorateId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "من فضلك أدخل اسم المحافظة")]
        public string GovernorateName { get; set; } = null!;

        public bool CurrentState { get; set; }

        public virtual ICollection<TbCity> TbCities { get; set; } = new List<TbCity>();

        public virtual ICollection<TbProperty> TbProperties { get; set; } = new List<TbProperty>();
    }
}
