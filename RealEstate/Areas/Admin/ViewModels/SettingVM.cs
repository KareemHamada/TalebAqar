namespace RealEstate.Areas.Admin.ViewModels
{
    public class SettingVM
    {
        public int SettingId { get; set; }
        public string WebsiteName { get; set; }
        public string? Logo { get; set; }
        public string? WebsiteDescription { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstgramLink { get; set; }
        public string? LinkedinLink { get; set; }
        public string? YoutubeLink { get; set; }
        public string? Address { get; set; }
        [StringLength(maximumLength: 11, MinimumLength = 11, ErrorMessage = "رقم التليفون يجب الا يقل او يزيد عن 11 رقم")]
        public string? ContactNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address.")]

        public string? Email { get; set; }
        public string? MainPanner { get; set; }
        public string? PropertyDetailsPanner { get; set; }
        public string? UpdatedBy { get; set; } // Changed to string
        public DateTime? UpdatedDate { get; set; }

        public virtual ApplicationUser? UpdatedByUser { get; set; }

    }
}
