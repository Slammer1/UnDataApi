using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnDataApi.DataModel;
using System.Net.Http;
using System.IO;

namespace UnDataApi.Services
{
    /// <summary>
    /// This class provides access to the Undata Api through HTTP calls to the Api which returns
    /// XML which is processed by the XMLService class.
    /// </summary>
    public class UnDataService
    {
        public List<DataFlow> AllDataFlows
        {
            get
            {
                return GetAllDataFlows().Result;
            }
        }

        public List<DataStructure> AllDataStructures { get
            {
                return GetAllDataStructures().Result;
            }
        }

        internal void GetCodes(string agencyId, string id)
        {
            throw new NotImplementedException();
        }

        public HttpClient UnApiClient = new HttpClient();

        public UnDataService()
        {



        }
        public DataStructure GetDataStructure(string agency, string structureId)
        {
            DataStructure structure = GetDataStructureFromUnApi(agency, structureId).Result;

            return structure;
        }

    public async Task<DataStructure> GetDataStructureFromUnApi(string agency, string structure)
    {
        HttpResponseMessage response = await UnApiClient.GetAsync("http://data.un.org/ws/rest/datastructure/" + agency + @"/" + structure);
        List<DataStructure> localStrucs = new List<DataStructure>();
        if (response.IsSuccessStatusCode)
        {
            Stream stream = response.Content.ReadAsStreamAsync().Result;
            localStrucs = ProcessStrucXml(stream, localStrucs);
        }

        if (localStrucs == null)
        {
            throw new Exception("Error getting DataFlows");
        }
        return localStrucs[0];
    }
    public async Task<List<DataStructure>> GetAllDataStructures()
        {
            HttpResponseMessage response = await UnApiClient.GetAsync("http://data.un.org/ws/rest/datastructure/");
            List<DataStructure> localStrucs = new List<DataStructure>();
            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStreamAsync().Result;
                localStrucs = ProcessStrucXml(stream, localStrucs);
            }

            if (localStrucs == null)
            {
                throw new Exception("Error getting DataFlows");
            }
            return localStrucs;
        }

        public List<DataStructure> ProcessStrucXml(Stream stream, List<DataStructure> localStrucs)
        {
            localStrucs = XMLService.InitializeDataStrucsFromXml(stream);
            return localStrucs;
        }



        public async Task<List<DataFlow>> GetAllDataFlows()
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
            return XMLService.InitializeFlowsFromXml(stream);
        }
    }
}
