namespace RealEstate.DTOS
{
    public class PropertyDTO
    {
        public int PropertyId { get; set; }
        public decimal Area { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public decimal Price { get; set; }
        public bool? Negotiable { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public string Image { get; set; }

    }
}
