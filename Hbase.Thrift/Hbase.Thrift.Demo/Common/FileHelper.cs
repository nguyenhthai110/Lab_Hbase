using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hbase.Thrift.Demo.Common
{
    /// <summary>Xử lý file</summary>
    /// <Modified>
    /// Name     Date         Comments
    /// thainh2  3/15/2022   created
    /// </Modified>
    public class FileHelper
    {
        private static object objWrite = true;

        /// <summary>tạo đường dẫn theo cấu trúc Năm/Tháng/Ngày ...</summary>
        /// <param name="types">The types.</param>
        /// <param name="content">The content.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// thainh2  3/15/2022   created
        /// </Modified>
        public static void GeneratorFileByDay(string types = "", string content = "")
        {
            DateTime time = DateTime.Now;
            var path = AppDomain.CurrentDomain.BaseDirectory + string.Format(@"{0}\{1}\{2}\{3}\", time.Year, time.Month, time.Day, types);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var fullPath = path + time.Day + ".txt";
            WriteFile(fullPath, content);
        }
        
        /// <summary>
        /// Ghi 1 dòng dữ liệu vào file
        /// </summary>
        /// <param name="pathWrite"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool WriteFile(string pathWrite, string content)
        {
            bool result = false;
            lock (objWrite)
            {
                var dPath = Path.GetDirectoryName(pathWrite);
                if (!Directory.Exists(dPath))
                    Directory.CreateDirectory(dPath);

                if (!File.Exists(pathWrite))
                {
                    FileStream strLocal = new FileStream(pathWrite, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    strLocal.Close();
                }

                StreamWriter streamw = new StreamWriter(pathWrite, true, Encoding.GetEncoding("utf-8"));
                try
                {
                    streamw.WriteLine(content);
                    result = true;
                }
                catch
                {
                    result = false;
                }
                finally
                {
                    streamw.Close();
                }

                return result;
            }
        }
    }
}
