using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Data.Specifications;

public class ProductWithTypesAndBrandSpecification : BaseSpecification<ProductModel>
{
    
    public ProductWithTypesAndBrandSpecification()
    {
        //AddInclude(x => x.Produc);
        //AddInclude(x => x.ProductBrand);
    }
    
    public ProductWithTypesAndBrandSpecification(Guid productId) 
    {
        //AddInclude(x => x.ProductType);
        //AddInclude(x => x.ProductBrand);
        // ApplyCriteria(x => x.Price > 100);
        ApplyCriteria(x=>x.ProductId==productId);

    }

    public ProductWithTypesAndBrandSpecification(string? sort, Guid? productTypeId, Guid? productBrandId, int? skip , int? take , string search)
    {
        //AddInclude(x => x.ProductType);
        //AddInclude(x => x.ProductBrand);
        
        if (productTypeId.HasValue || productBrandId.HasValue || !string.IsNullOrWhiteSpace(search))
        {
            ApplyCriteria(x => 
                (!productTypeId.HasValue || x.CategoryId == productTypeId.Value) &&
                //(!productBrandId.HasValue || x.ProductBrandId == productBrandId.Value) &&
                (string.IsNullOrWhiteSpace(search) || x.ProductName.ToLower().Contains(search.ToLower()))
            );
        }
        
        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort.ToLower())
            {
                case "priceasc":
                    ApplyOrderBy(x => x.Price, Core.Enums.OrderBy.Ascending);
                    break;
                case "pricedesc":
                    ApplyOrderBy(x => x.Price, Core.Enums.OrderBy.Descending);
                    break;
                case "namedesc":
                    ApplyOrderBy(x => x.ProductName, Core.Enums.OrderBy.Descending);
                    break;
                default:
                    ApplyOrderBy(x => x.ProductName, Core.Enums.OrderBy.Ascending);
                    break;
            }
        }
        
        if (skip >= 0 && take >= 0)
        {
            ApplyPaging(skip, take);
        }
        
    }
    public ProductWithTypesAndBrandSpecification(string? sort, Guid? productTypeId, int? skip, int? take)
    {
        AddInclude(x => x.Category);
        //AddInclude(x => x.Brand);

        if (productTypeId.HasValue)
        {
            ApplyCriteria(x =>
                (!productTypeId.HasValue || x.CategoryId == productTypeId.Value)
            );
        }

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort.ToLower())
            {
                case "createbyasc":
                    ApplyOrderBy(x => x.CreateBy, Core.Enums.OrderBy.Ascending);
                    break;
                case "createbydesc":
                    ApplyOrderBy(x => x.CreateBy, Core.Enums.OrderBy.Descending);
                    break;
                case "createtimeasc":
                    ApplyOrderBy(x => x.CreateTime, Core.Enums.OrderBy.Descending);
                    break;
                case "createtimedesc":
                    ApplyOrderBy(x => x.CreateTime, Core.Enums.OrderBy.Descending);
                    break;
                case "typeasc":
                    ApplyOrderBy(x => x.Category.CategoryName, Core.Enums.OrderBy.Descending);
                    break;
                case "typedesc":
                    ApplyOrderBy(x => x.Category.CategoryName, Core.Enums.OrderBy.Descending);
                    break;
                default:
                    ApplyOrderBy(x => x.CreateTime, Core.Enums.OrderBy.Descending);
                    break;
            }
        }

        if (skip >= 0 && take >= 0)
        {
            ApplyPaging(skip, take);
        }

    }
}

