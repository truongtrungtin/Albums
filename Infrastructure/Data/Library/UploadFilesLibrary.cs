
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using Core.Entities;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.ViewModel;

namespace Infrastructure.Data.Library
{
    public interface IUploadFilesLibrary
    {
        Task<string> UploadFile(IFormFile file, string folder, int minWidth = 300, int maxWidth = 1600, int minHeight = 300, int maxHeight = 1600, string FileType = null);
        (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory);
    }
    public class UploadFilesLibrary : IUploadFilesLibrary
    {
        private EntityDataContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private UnitOfWork _unitOfWork;
        public UploadFilesLibrary(IHostingEnvironment hostingEnvironment, JwtSettings appSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = new EntityDataContext();
            _unitOfWork = new UnitOfWork(_context, appSettings);
        }


        /// <summary>
        /// Save file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="minWidth"></param>
        /// <param name="maxWidth"></param>
        /// <param name="minHeight"></param>
        /// <param name="maxHeight"></param>
        /// <param name="FileType"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(IFormFile file, string folder, int minWidth = 300, int maxWidth = 1600, int minHeight = 300, int maxHeight = 1600, string FileType = null)
        {
            string ret = "";
            string parth = "";
            try
            {
                if (file != null && file.Length > 0)
                {
                    // nếu có chọn file
                    int indexDot = file.FileName.LastIndexOf('.');
                    string filename = file.FileName.Substring(0, indexDot);

                    //Convert file name
                    filename = _unitOfWork.RepositoryLibrary.ConvertToNoMarkString(filename);
                    //get file type
                    string fileType = file.FileName.Substring(indexDot);
                    //Đổi tên lại thành chuỗi phân biệt tránh trùng
                    filename = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + filename;
                    //folder path
                    var existPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/Upload/" + folder);
                    Directory.CreateDirectory(existPath);

                    //file path
                    filename = filename + fileType;
                    parth = Path.Combine(existPath, filename);

                    //gán giá trị trả về
                    ret = filename;

                    //Save File
                    using (var stream = new FileStream(parth, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            catch
            {
                ret = null;
            }
            return ret;
        }


        public (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory)
        {
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            var files = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, "Upload/" + subDirectory)).ToList();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    files.ForEach(file =>
                    {
                        var theFile = archive.CreateEntry(file);
                        using (var streamWriter = new StreamWriter(theFile.Open()))
                        {
                            streamWriter.Write(File.ReadAllText(file));
                        }

                    });
                }
                return ("application/zip", memoryStream.ToArray(), zipName);
            }

        }
    }
}
