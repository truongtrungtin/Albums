using System;
using System.Collections.Generic;

namespace Infrastructure.Data.ViewModel
{
    public class BaseCategoryViewModel
	{
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public decimal? Items { get; set; }
        public BaseCategoryViewModel Parent { get; set; }
        public List<BaseCategoryViewModel> Children { get; set; }
    }
}

