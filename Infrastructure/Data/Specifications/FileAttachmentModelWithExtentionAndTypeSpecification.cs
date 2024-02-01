using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Data.Specifications;

public class FileAttachmentModelWithExtentionAndTypeSpecification : BaseSpecification<FileAttachmentModel>
{

    public FileAttachmentModelWithExtentionAndTypeSpecification()
    {
        //AddInclude(x => x.Produc);
        //AddInclude(x => x.FileExtention);
    }
    public FileAttachmentModelWithExtentionAndTypeSpecification(Guid fileAttachmentId)
    {
        //AddInclude(x => x.ProductType);
        //AddInclude(x => x.ProductBrand);
        // ApplyCriteria(x => x.Price > 100);
        ApplyCriteria(x => x.FileAttachmentId == fileAttachmentId);

    }
    public FileAttachmentModelWithExtentionAndTypeSpecification(Guid createBy, string? sort, string? fileExtention, string? fileType,string? profile, int? skip, int? take)
    {

        ApplyCriteria(x =>
                    (string.IsNullOrWhiteSpace(fileExtention) || x.FileExtention.ToLower().Contains(fileExtention.ToLower()))
                    && (string.IsNullOrWhiteSpace(fileType) || x.FileType.ToLower().Contains(fileType.ToLower()))
                    && (string.IsNullOrWhiteSpace(profile) || x.ObjectId == Guid.Parse(profile))
                    && x.CreateBy == createBy
                ); ;

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort.ToLower())
            {
                case "timedesc":
                    ApplyOrderBy(x => x.CreateTime, Core.Enums.OrderBy.Descending);
                    break;
                default:
                    ApplyOrderBy(x => x.CreateTime, Core.Enums.OrderBy.Ascending);
                    break;
            }
        }

        if (skip >= 0 && take >= 0)
        {
            ApplyPaging(skip, take);
        }

    }
}

