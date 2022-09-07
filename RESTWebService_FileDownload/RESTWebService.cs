using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace RESTWebService_FileDownload
{
    public class RESTWebService
    {
        public static string DownloadFile(string domain, string apiUrl, string cookie, string headers, string outputFolder)
        {
            string[] cookies = cookie.Split(';');
            string fileName = apiUrl.Split('/')[apiUrl.Split('/').Length - 1];
            CookieContainer cookieContainer = new CookieContainer();
            if (cookie.Length != 0)
            {

                Uri target = new Uri(domain);
                foreach (string cookie_ in cookies)
                {
                    if (cookie_.Length != 0)
                    {
                        cookieContainer.Add(new Cookie(cookie_.Split('=')[0], cookie_.Split('=')[1]) { Domain = target.Host });
                    }
                }
            }
            string remoteUrl = string.Format(apiUrl);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(remoteUrl);
            httpRequest.CookieContainer = cookieContainer;

            if (headers.Length != 0)
            {
                string[] headers_ = headers.Split(';');
                foreach (string header in headers_)
                {
                    if (header.Length != 0)
                    {
                        if (header.Split('=')[0] == "Accept")
                        {
                            httpRequest.Accept = header.Split('=')[1] + ";";
                        }
                        else
                        {
                            httpRequest.Headers.Add(header.Split('=')[0], header.Split('=')[1]);
                        }

                    }
                }
            }
            try
            {
                WebResponse response = httpRequest.GetResponse();
                using (Stream output = File.OpenWrite("" + outputFolder + "/"+fileName))
                using (Stream input = response.GetResponseStream())
                {
                    input.CopyTo(output);
                }

                return "File saved in  : " + "" + outputFolder + "/" + fileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
