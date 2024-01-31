using Core.Entities;
using Microsoft.VisualBasic.FileIO;

namespace Infrastructure.Data.Specifications
{
    public class FileAttachmentCountSpecification : BaseSpecification<FileAttachmentModel>
    {
        public FileAttachmentCountSpecification(string fileExtention, string fileType)
        {
            // Apply filter criteria similar to ProductWithTypesAndBrandSpecification
            if (!string.IsNullOrWhiteSpace(fileExtention) ||  !string.IsNullOrWhiteSpace(fileType))
            {
                ApplyCriteria(x =>
                    (string.IsNullOrWhiteSpace(fileExtention) || x.FileExtention.ToLower().Contains(fileExtention.ToLower()))
                    && (string.IsNullOrWhiteSpace(fileType) || x.FileType.ToLower().Contains(fileType.ToLower()))

                );
            }
        }
      
    }
}