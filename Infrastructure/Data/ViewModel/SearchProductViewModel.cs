using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.ViewModel
{
    public class SearchProductViewModel
    {
        public IEnumerable<Guid?> ProductTypeId { get; set; }
        public List<ProductTypeViewModel> ProductType { get; set; }
        public string searchString { get; set; }
    }
}
