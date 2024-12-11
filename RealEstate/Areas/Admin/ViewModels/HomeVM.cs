namespace RealEstate.Areas.Admin.ViewModels
{
    public class HomeVM
    {
        public int NumOfTotalSales { get; set; }
        public int NumOfRealySold { get; set; }
        public int NumOfForSale { get; set; }
        public int NumOfDeletedSales { get; set; }

        public int NumOfTotalRentals { get; set; }
        public int NumOfRealyRentaled { get; set; }
        public int NumOfForRental { get; set; }
        public int NumOfDeletedRentals { get; set; }


        public decimal TotalPriceSales { get; set; }
        public decimal HighestPriceSold { get; set; }
        public decimal LowestPriceSold { get; set; }
        public decimal HighestPriceAvaliableForSale { get; set; }
        public decimal LowestPriceAvaliableForSale { get; set; }


        public decimal TotalPriceRentals { get; set; }
        public decimal HighestPriceRentaled { get; set; }
        public decimal LowestPriceRentaled { get; set; }
        public decimal HighestPriceAvaliableForRent { get; set; }
        public decimal LowestPriceAvaliableForRent { get; set; }


        public int TotalGovernorates { get; set; }
		public string BiggestSellingGovernorate { get; set; }
		public int NumOfPropertiesForBiggestSellingGovernorate { get; set; }
		public string BiggestRentalGovernorate { get; set; }
		public int NumOfPropertiesForBiggestRentalGovernorate { get; set; }



		public int TotalCities { get; set; }
		public string BiggestSellingCity { get; set; }
        public int NumOfPropertiesForBiggestSellingCity { get; set; }
        public string BiggestRentalCity { get; set; }
        public int NumOfPropertiesForBiggestRentalCity { get; set; }


        public int NumOfOwners { get; set; }
        public int NumOfPostsViews { get; set; }


        public decimal TotalSalesProfit { get; set; }
		public decimal MaximumSalesProfit { get; set; }
		public decimal MinimumSalesProfit { get; set; }


		public decimal TotalRentalsProfit { get; set; }
		public decimal MaximumRentalsProfit { get; set; }
		public decimal MinimumRentalsProfit { get; set; }



	}
}
