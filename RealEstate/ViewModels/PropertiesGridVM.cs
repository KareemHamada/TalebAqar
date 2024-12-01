using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RealEstate.ViewModels
{
    
    public class PropertiesGridVM
    {
        public IEnumerable<TbProperty> Properties { get; set; }
        public PaginationModel Pagination { get; set; }
    }
    public class PaginationModel
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}




