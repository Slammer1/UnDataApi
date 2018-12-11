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

        public Dictionary<string, string> GetCodes(string agencyId, string id)
        {
            Dictionary<string, string> codes = GetCodeListFromUnApi(agencyId, id).Result;

            return codes;
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

        public async Task<QueryResult> GetDataFromDataFlowAndDimensions(string dataflowId, List<string> codes)
        {
            QueryResult queryResult = new QueryResult();
            queryResult.BaseUrl = "http://data.un.org/ws/rest/data/";
            string codesString = "";
            foreach(string code in codes)
            {
                codesString += code + ".";
            }
            queryResult.Query = dataflowId + "/" + codesString;
            HttpResponseMessage response = await UnApiClient.GetAsync(queryResult.BaseUrl + queryResult.Query);
            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStreamAsync().Result;
                queryResult.DataSet = ProcessQueryXml(stream);
            }

            if (queryResult.DataSet == null)
            {
                throw new Exception("Error getting Data");
            }
            return queryResult;
        }

        private DataSet ProcessQueryXml(Stream stream)
        {
            DataSet data = XMLService.ProcessQuery(stream);
            return data;
        }

        public async Task<Dictionary<string, string>> GetCodeListFromUnApi(string agency, string id)
        {
            HttpResponseMessage response = await UnApiClient.GetAsync("http://data.un.org/ws/rest/codelist/" + agency + @"/" + id);
            Dictionary<string, string> codes = new Dictionary<string, string>();
            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStreamAsync().Result;
                codes = ProcessCodeXml(stream, codes);
            }

            if (codes == null)
            {
                throw new Exception("Error getting DataFlows");
            }
            return codes;
        }

        private Dictionary<string, string> ProcessCodeXml(Stream stream, Dictionary<string, string> codes)
        {
            codes = XMLService.InitializeCodesFromXml(stream);
            return codes;
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

        public async Task<List<DataFlow>> GetAllDataFromDataFlow(string dataflowId)
        {
            HttpResponseMessage response = await UnApiClient.GetAsync("http://data.un.org/ws/rest/data/" + dataflowId);
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
