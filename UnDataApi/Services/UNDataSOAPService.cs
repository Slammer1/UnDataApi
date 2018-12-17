using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UnDataApi.Services
{
    public class UNDataSOAPService
    {


        private void SendRequest(XmlDocument request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            string tempFileName = Path.GetTempFileName();
            var sb = new StringBuilder();
            sb.AppendFormat(SoapConstants.SoapRequest, "web");
            var doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            XmlNodeList nodes = doc.GetElementsByTagName(SoapConstants.Body, SoapConstants.Soap11Ns);
            XmlElement operation = doc.CreateElement(
                "web", "QueryStructure");

            XmlElement queryParent = operation;

            if (request.DocumentElement != null)
            {
                XmlNode sdmxQueryNode = doc.ImportNode(request.DocumentElement, true);
                queryParent.AppendChild(sdmxQueryNode);
            }

            nodes[0].AppendChild(operation);

            var endpointUri = new Uri(@"http://data.un.org/ws/NSIEstatV20Service");
            var webRequest = (HttpWebRequest)WebRequest.Create(endpointUri);
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            webRequest.Headers.Add(SoapConstants.SoapAction);

            webRequest.ContentType = "@text/xml;charset=\"utf - 8\"";

            webRequest.Method = "Post";
            webRequest.Timeout = 1800 * 1000;

            using (Stream stream = webRequest.GetRequestStream())
            {
                doc.Save(stream);
            }

            try
            {
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            var settings = new XmlWriterSettings();
                            settings.Indent = true;
                            using (XmlWriter writer = XmlWriter.Create(tempFileName, settings))
                            {
                                //Code to read data into temp file.
                                //SoapUtils.ExtractSdmxMessageWithComment(new ReadableDataLocationTmp(stream), writer);
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                //NsiClientHelper.HandleSoapFault(ex);
            }
        }
    }
}
