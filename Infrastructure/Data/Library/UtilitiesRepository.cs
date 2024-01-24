using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.Data.Library
{
    public class UtilitiesRepository
    {

        // public List<SqlDataRecord> ConvertTableFromListString(List<string> data)
        // {
        //     var tableSchema = new List<SqlMetaData>(1)
        //     {
        //         new SqlMetaData("Code", SqlDbType.NVarChar, 100)
        //     }.ToArray();
        //     //And a table as a list of those records
        //     var tableData = new List<SqlDataRecord>();
        //     if (data != null && data.Count > 0)
        //     {
        //         foreach (var p in data)
        //         {
        //             var tableRow = new SqlDataRecord(tableSchema);
        //             tableRow.SetString(0, p);
        //             tableData.Add(tableRow);
        //         }
        //     }
        //     else
        //     {
        //         var tableRow = new SqlDataRecord(tableSchema);
        //         tableData.Add(tableRow);
        //     }
        //     return tableData;
        // }
        // public List<SqlDataRecord> ConvertTableFromListGuid(List<Guid> data)
        // {
        //     var tableSchema = new List<SqlMetaData>(1)
        //     {
        //         new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
        //     }.ToArray();
        //     //And a table as a list of those records
        //     var tableData = new List<SqlDataRecord>();
        //     if (data != null && data.Count > 0)
        //     {
        //         foreach (var p in data)
        //         {
        //             var tableRow = new SqlDataRecord(tableSchema);
        //             tableRow.SetGuid(0, p);
        //             tableData.Add(tableRow);
        //         }
        //     }
        //     else
        //     {
        //         var tableRow = new SqlDataRecord(tableSchema);
        //         tableData.Add(tableRow);
        //     }
        //     return tableData;
        // }

        //Convert yyyyMMdd to DateTime
        public DateTime? ConvertDateTime(string datetime_YYYYMMDD)
        {
            DateTime? datetime = null;
            if (datetime_YYYYMMDD.ToString() != "00000000" && datetime_YYYYMMDD.Length == 8)
            {
                int year = Convert.ToInt32(datetime_YYYYMMDD.Substring(0, 4));
                int month = Convert.ToInt32(datetime_YYYYMMDD.Substring(4, 2));
                int day = Convert.ToInt32(datetime_YYYYMMDD.Substring(6, 2));

                datetime = new DateTime(year, month, day);
            }
            return datetime;
        }

        public DateTime? ConvertDateTimeFromddMMyyyy(string datetime_ddMMyyyy)
        {
            DateTime? datetime = null;
            if (!string.IsNullOrEmpty(datetime_ddMMyyyy))
            {
                //dd/MM/yyyy
                var a = datetime_ddMMyyyy.Substring(0, 2);
                int day = Convert.ToInt32(datetime_ddMMyyyy.Substring(0, 2));
                int month = Convert.ToInt32(datetime_ddMMyyyy.Substring(3, 2));
                int year = Convert.ToInt32(datetime_ddMMyyyy.Substring(6, 4));

                datetime = new DateTime(year, month, day);
            }
            return datetime;
        }

        // Tiến: Đổi tên cho dễ xài đọc vô hiểu luôn
        public DateTime? ConvertStringyyyyMMddToDateTime(string datetime_YYYYMMDD)
        {
            return ConvertDateTime(datetime_YYYYMMDD);
        }

        //Convert DateTime to dd/MM/yyyy
        public string ConvertStrDateTime(string datetime_YYYYMMDD)
        {
            if (!string.IsNullOrEmpty(datetime_YYYYMMDD))
            {
                return string.Format("{0:dd/MM/yyyy}", ConvertDateTime(datetime_YYYYMMDD));
            }
            return null;
        }

        // Tiến: Đổi tên cho dễ xài đọc vô hiểu luôn
        public string ConvertDateTimeToStringddMMyyyy(string datetime_YYYYMMDD)
        {
            return ConvertStrDateTime(datetime_YYYYMMDD);
        }




        /// <summary>
        /// Get file extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string FileExtension(string fileName)
        {
            int index = fileName.LastIndexOf('.');
            var result = fileName.Substring(index);
            return result;
        }

        /// <summary>
        /// Get file type base on extension file
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string GetFileTypeByExtension(string extension)
        {
            extension = extension.ToLower();
            string FileType = string.Empty;
            if (extension.Contains(".xls") || extension.Contains(".xlsx"))
            {
                FileType = "Excel";
            }
            else if (extension.Contains(".doc") || extension.Contains(".docx"))
            {
                FileType = "Word";
            }
            else if (extension.Contains(".pdf"))
            {
                FileType = "Pdf";
            }
            else if (extension.Contains(".ppt") || extension.Contains(".pptx"))
            {
                FileType = "PowerPoint";
            }
            else if (extension.Contains(".jpg") || extension.Contains(".png") || extension.Contains(".jpeg") || extension.Contains(".gif"))
            {
                FileType = "Image";
            }
            else
            {
                FileType = "Other";
            }
            return FileType;
        }

        public int ConvertDateTimeToInt(DateTime? dateTime)
        {
            int dateKey;
            try
            {
                dateKey = Convert.ToInt32(string.Format("{0:yyyyMMdd}", dateTime));
                return dateKey;
            }
            catch
            {
                return 0;
            }
        }


        private readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        //protected string GetConfigByKey(string key)
        //{
        //    var ret = "";

        //    var config = _context.ApplicationConfig.SingleOrDefault(a => a.ConfigKey == key);

        //    if (config != null)
        //    {
        //        ret = config.ConfigValue;
        //    }

        //    return ret;
        //}

        public string ConvertToDateTime(string strExcelDate)
        {
            double excelDate;
            try
            {
                excelDate = Convert.ToDouble(strExcelDate);
            }
            catch
            {
                return strExcelDate;
            }
            if (excelDate < 1)
            {
                throw new ArgumentException("Excel dates cannot be smaller than 0.");
            }
            DateTime dateOfReference = new DateTime(1900, 1, 1);
            if (excelDate > 60d)
            {
                excelDate = excelDate - 2;
            }
            else
            {
                excelDate = excelDate - 1;
            }
            return dateOfReference.AddDays(excelDate).ToShortDateString();
        }

        public bool ParseBool(string input)
        {
            switch (input.ToLower())
            {
                case "true":
                    return true;
                default:
                    return false;
            }
        }
    }
}
