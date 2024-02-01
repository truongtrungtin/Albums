using Core.Entities;
using Microsoft.VisualBasic.FileIO;

namespace Infrastructure.Data.Specifications
{
    public class FileAttachmentCountSpecification : BaseSpecification<FileAttachmentModel>
    {
        public FileAttachmentCountSpecification(Guid createBy, string? fileExtention, string? fileType, string? profile)
        {
            // Apply filter criteria similar to ProductWithTypesAndBrandSpecification
                ApplyCriteria(x =>
                    (string.IsNullOrWhiteSpace(fileExtention) || x.FileExtention.ToLower().Contains(fileExtention.ToLower()))
                    && (string.IsNullOrWhiteSpace(fileType) || x.FileType.ToLower().Contains(fileType.ToLower()))
                    && (string.IsNullOrWhiteSpace(profile) || x.ObjectId == Guid.Parse(profile))
                    && x.CreateBy == createBy
                );
            }
      
    }
}