using System;
using System.Collections.Generic;

namespace Infrastructure.Data.ViewModel
{
    public class ProductAttributeViewModel
	{
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool? Featured { get; set; }
        public List<ProductAttributeValueViewModel> Values { get; set; }
    }

    public class ProductAttributeValueViewModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }

}

