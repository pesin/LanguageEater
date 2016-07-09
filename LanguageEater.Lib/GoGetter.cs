using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace LanguageEater.Lib
{
    class GoGetter
    {
        private static string workDir = System.Configuration.ConfigurationManager.AppSettings["workDir"];
        private static string linksFile = System.Configuration.ConfigurationManager.AppSettings["linksFile"];

        public static void GoGetThem()
        {
            XDocument doc = XDocument.Load(linksFile);

            MongoMongo mongo = new MongoMongo();

            foreach (var el in doc.Elements("a"))
            {
                string url = el.Attribute("href").Value;
                string filename = url.Substring(url.LastIndexOf("/") + 1);

                bool flag = DownloadFile(url, filename);
                if (flag)
                {
                    //unzip
                   string newFileName = Decompress(new FileInfo(filename));
                    
                    if (!string.IsNullOrEmpty(newFileName))
                    {
                        //load to db
                       // mongo.insert5Gram()
                    }
                }
            }
        }

        /// <summary>
        /// Split each like per format
        /// ngram TAB year TAB match_count TAB volume_count NEWLINE
        /// </summary>
        /// <param name="fileName"></param>
        private static void readAndLoad(string fileName)
        {
            var stream = File.OpenText(fileName);
            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine();
                string[] tokens = line.Split('\t');
                if (tokens.Length != 4)
                {
                    Console.WriteLine("error line: " + line);
                    continue;
                }

                string ngram = tokens[0];
                int year=0;
                int.TryParse(tokens[1], out year);
                long count = 0;
                long.TryParse(tokens[2], out count);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileToDecompress"></param>
        /// <returns></returns>
        private static string Decompress(FileInfo fileToDecompress)
        {
            string newFileName = null;
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                 newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
            return newFileName;
        }

        private static bool DownloadFile(string url, string filename)
        {
            bool retval = true;
            var request = WebRequest.CreateHttp(url);
            request.AllowReadStreamBuffering = true;
            FileStream f=null;
            Stream responseStream = null;
            try
            {
                var response = request.GetResponse();
                 responseStream = response.GetResponseStream();
                 f = File.Create(string.Format("{0}\\{1}", workDir, filename));

                while (responseStream.CanRead)
                {
                    f.WriteByte((byte)responseStream.ReadByte());
                }
                f.Flush();
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
                retval = false;
            }
            finally
            {
                if (f != null)
                {
                    f.Close();
                    f.Dispose();
                }

                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream.Dispose();
                }
            }
            return retval;
        }
    }
}
