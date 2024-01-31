using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Data.Specifications;

public class CatalogModelSpecification : BaseSpecification<CatalogModel>
{

    public CatalogModelSpecification()
    {
        //AddInclude(x => x.Produc);
        //AddInclude(x => x.CatalogTypeCode);
    }
    public CatalogModelSpecification(string CatalogTypeCode)
    {
        //AddInclude(x => x.ProductType);
        //AddInclude(x => x.ProductBrand);
        // ApplyCriteria(x => x.Price > 100);
        ApplyCriteria(x => x.CatalogTypeCode == CatalogTypeCode);
        ApplyOrderBy(x => x.OrderIndex, Core.Enums.OrderBy.Ascending);
    }
}

