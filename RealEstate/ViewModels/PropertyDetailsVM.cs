namespace RealEstate.ViewModels
{
    public class PropertyDetailsVM
    {
        public PropertyVM propertyVM { get; set; }
        public IEnumerable<PropertyVM> PropertiesInTheSameGovernorate { get; set; }

    }
}
