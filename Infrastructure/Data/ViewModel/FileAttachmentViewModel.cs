namespace Infrastructure.Data.ViewModel
{
    public class FileAttachmentViewModel
    {
        public Guid FileAttachmentId { get; set; }
        public string FileAttachmentCode { get; set; }
        public string FileAttachmentName { get; set; }
        public string FileExtention { get; set; }
        public string FileType { get; set; }
        public decimal? Size { get; set; }
        public string FileUrl { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid CreateBy { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
