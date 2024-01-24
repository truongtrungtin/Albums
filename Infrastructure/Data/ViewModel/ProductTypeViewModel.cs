using System;
using System.Collections.Generic;

namespace Infrastructure.Data.ViewModel
{
	public class ProductTypeViewModel
	{
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<ProductTypeAttributeGroupViewModel> AttributeGroups { get; set; }
    }
}

