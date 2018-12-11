using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class QueryResult
    {
        public string Query { get; set; }
        public string BaseUrl { get; set; }
        public DataSet DataSet { get; set; }

        public string DisplayName { get; set; }

        public void BuildDisplayName(List<Dimension> dimensionList, List<string> selectedCodes)
        {
            int i = 0;
            string display = "";
            foreach(Dimension dim in dimensionList)
            {
                if (dim.Id == "TIME_PERIOD") continue;
                display += GetCodeDescription(dim, selectedCodes[i]);
                i++;
            }
            DisplayName = display;
        }

        private string GetCodeDescription(Dimension dim, string v)
        {
            string display = "";
            switch(dim.Id)
            {
                case "FREQ":
                    display = dim.CodeList[v];
                    break;
                case "INDICATOR":
                    display = " " + dim.CodeList[v];
                    break;
                case "REF_AREA":
                    display = " in " + dim.CodeList[v];
                    break;
                case "UNIT":
                    display = " in " + dim.CodeList[v];
                    break;
            }
            return display;
        }
    }
}
