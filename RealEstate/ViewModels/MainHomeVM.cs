namespace RealEstate.ViewModels
{
    public class MainHomeVM
    {
        public IEnumerable<PropertyVM> FeaturedProperties { get; set; }
        public IEnumerable<PropertyVM> LatestProperties { get; set; }
        public IEnumerable<PropertyTypeCount> PropertyTypes { get; set; }



        public TbSetting Setting { get; set; }

        public int NumOfRealySold { get; set; }
        public int NumOfForSale { get; set; }
        public int NumOfRealyRentaled { get; set; }
        public int NumOfForRental { get; set; }


        public decimal MinAvaliableRentPrice { get; set; }
        public decimal MinAvaliableSalePrice { get; set; }


    }
}
