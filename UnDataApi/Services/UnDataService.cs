using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnDataApi.DataModel;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.IO;
using System.Xml.Serialization;

namespace UnDataApi.Services
{
    public class UnDataService
    {
        public List<DataFlow> DataFlows { get; set; }


        public HttpClient UnApiClient = new HttpClient();

        public UnDataService()
        {

            DataFlows = GetDataFlows().Result;

        }

        public async Task<List<DataFlow>> GetDataFlows()
        {
            HttpResponseMessage response = await UnApiClient.GetAsync("http://data.un.org/ws/rest/dataflow/");
            List<DataFlow> localFlows = new List<DataFlow>();
            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStreamAsync().Result;
                localFlows = ProcessXml(stream, localFlows);
            }

            if (localFlows == null)
            {
                throw new Exception("Error getting DataFlows");
            }
            return localFlows;

        }

        public List<DataFlow> ProcessXml(Stream stream, List<DataFlow> localFlows)
        {
                XmlReader reader = XmlReader.Create(stream);
                reader.ReadToDescendant("Dataflow");
                DataFlow flow = new DataFlow();
                do
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "Dataflow":
                                    if (flow != null && flow.Id != string.Empty && flow.Id != null) localFlows.Add(flow);
                                    flow = new DataFlow();
                                    while (reader.MoveToNextAttribute())
                                    {
                                        switch (reader.Name)
                                        {
                                            case "id":
                                                flow.Id = reader.Value;
                                                break;

                                            case "version":
                                                flow.Version = reader.Value;
                                                break;

                                            case "agencyID":
                                                flow.AgencyId = reader.Value;
                                                break;

                                            case "isFinal":
                                                bool isfinal;
                                                bool isParsed = bool.TryParse(reader.Value, out isfinal);
                                                flow.IsFinal = isfinal;
                                                break;
                                        }

                                    }
                                    break;
                            }

                            break;
                        case XmlNodeType.Text:
                            flow.Name = reader.Value;
                            break;
                        case XmlNodeType.EndElement:
                            Console.Write("</{0}>", reader.Name);
                            break;
                    }
                } while (reader.Read());
                if (flow != null && flow.Id != string.Empty && flow.Id != null) localFlows.Add(flow);
                reader.Close();
            return localFlows;
            }
        }
    }
