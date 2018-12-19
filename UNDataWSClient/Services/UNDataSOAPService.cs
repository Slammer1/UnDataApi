using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UNDataWSClient.Services
{
    public class UNDataSOAPService
    {

        public void SendQueryStructurRequest()
        {
            string queryStructureXML = "";
            try
            {
                queryStructureXML = File.ReadAllText(@"C:\Users\JWNolen\source\repos\UnDataApp\UNDataWSClient\Files\BasicQueryStructure.xml");
            }

            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("The TestXml.txt file did not get copied to the " +
                    "output directory correctly. Please click on the file and, in the properties," +
                    "adjust the build action and it should be copied correctly because the properties" +
                    "have been changed. That or do a clean build.");
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(queryStructureXML);
            string tempFileName = Path.GetTempFileName();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(SoapConstants.SoapRequest, "web", SoapConstants.TargetNs);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(sb.ToString());

        }




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
