using System;
using System.Collections.Generic;
using System.IO;
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

            

            foreach (var el in doc.Elements("a"))
            {
                string url = el.Attribute("href").Value;
                string filename = url.Substring(url.LastIndexOf("/") + 1);

                bool flag = DownloadFile(url, filename);
                if (flag)
                {
                    //unzip
                    //load to db
                }
            }
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
