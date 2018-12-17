
using System.Collections.Generic;
using System.Linq;
using UnDataApi.Services;

namespace UnDataApi.DataModel
{
    public class DataStructure
    {

        public string Id { get; set; }

        public string AgencyId { get; set; }

        public bool IsFinal { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public List<Dimension> DimensionList { get; set; }

        public List<Measure> MeasuresList { get; set; }

        public List<Attribute> AttributeList { get; set; }

        public void GetCodeLists()
        {
            foreach(Dimension dim in DimensionList)
            {
                Dictionary<string, string> codes = dim.GetCodeList();
            }
        }

        public Dictionary<string, HashSet<string>> GetSeriesKeys(string dataFlowId)
        {
            Services.UnDataService service = new Services.UnDataService();
            Dictionary<string, HashSet<string>> seriesKeysRaw = service.GetSeriesKeysForDatFlow(dataFlowId).Result;
            SeriesKeys = seriesKeysRaw;
            return seriesKeysRaw;
           
        }

        public void InitializeCodesForDimensions(string id)
        {
            this.GetCodeLists();
            this.FilterCodeLists(id);
        }

        private void FilterCodeLists(string id)
        {
            foreach (Dimension dim in DimensionList.Where(dim => dim.Id !=  "TIME_PERIOD" && dim.Id != "AGE" && 
            dim.Id != "SERIES" && dim.Id != "REF_AREA" && dim.Id != "INDICATOR" && dim.Id != "OCCUPATION" && dim.Id != "ACTIVITY"))
            {
                HashSet<string> validCodes = XMLService.GetValidCodes(id, dim.Id);
                dim.Codes = dim.Codes.Where(code => validCodes.Contains(code.Key)).ToList(); 
            }
        }

       

        public Dictionary<string, HashSet<string>> SeriesKeys { get; set; }
    }
}
