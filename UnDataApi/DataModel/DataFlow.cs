using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnDataApi.DataModel
{
    public class DataFlow : IDataFlow
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public bool IsFinal { get; set; }

        public string Version { get; set; }

        public string AgencyId { get; set; }

        public string DisplayName
        {
            get
            {
                return GetDisplayName();
            }
        }

        public string Description
        {
            get
            {
                return GetDescription();
            }
        }
        public Structure Structure { get; set; }

        public DataStructure DataStructure {
            get
            {
                return GetDataStructure();
            }
        }

        public DataStructure GetDataStructure()
        {
            DataStructure structure = new DataStructure();
            Services.UnDataService service = new Services.UnDataService();
            structure = service.GetDataStructure(Structure.AgencyId, Structure.Id);
            return structure;

        }

        public string GetDescription()
        {
            return DescriptionList[Id];
        }

        public string GetDisplayName()
        {
            return DisplayNames[Id];
        }

        private Dictionary<string, string> DescriptionList = new Dictionary<string, string>();
        private Dictionary<string, string> DisplayNames = new Dictionary<string, string>();
        

        /// <summary>
        /// Empty constructor for a DataFlow object.
        /// </summary>
        public DataFlow()
        {
            SetDescriptions();
            SetDisplayNames();

        }
        /// <summary>
        /// Sets the display names for the dataflow.
        /// </summary>
        private void SetDisplayNames()
        {
            DisplayNames.Add("DF_UNData_UNFCC", "Environmental Data");
            DisplayNames.Add("DF_UNData_UIS", "Education Data");
            DisplayNames.Add("DF_UNDATA_MDG", "Development Data (Population, poverty, unemployment, etc.)");
            DisplayNames.Add("DF_UNDATA_COUNTRYDATA", "Development Data (Population, poverty, unemployment, etc.)");
            DisplayNames.Add("DF_UNDATA_WDI", "Development Data (Financial growth, military capability, imports and exports, etc.)");
            DisplayNames.Add("DF_UNDATA_SDG_PILOT", "Development Data - Sustainable development goals");
            DisplayNames.Add("DF_UNDATA_WPP", "World Population Prospects");
            DisplayNames.Add("NA_MAIN", "National Accounts Aggregate Data");
            DisplayNames.Add("NASEC_IDCNFSA_A", "Annual Non-Financial Accounts");
            DisplayNames.Add("NASEC_IDCFINA_Q", "Quarterly Financial Accounts");
            DisplayNames.Add("NASEC_IDCFINA_A", "Annual Financial Accounts");
            DisplayNames.Add("NASEC_IDCNFSA_Q", "Quarterly Non-Financial Accounts");
        }

        private void SetDescriptions()
        {
            DescriptionList.Add("DF_UNData_UIS", "This data set provides information on education at various levels.");
            DescriptionList.Add("DF_UNData_UNFCC", "This data set provides information on Greenhouse gas emissions.");
            DescriptionList.Add("DF_UNDATA_MDG", "This data set provides information on country development such as " +
                "Population growth, poverty rates, unemployment rates, etc.");
            DescriptionList.Add("DF_UNDATA_COUNTRYDATA", "This data set provides information on country development such as " +
                "Population growth, poverty rates, unemployment rates, etc.");
            DescriptionList.Add("DF_UNDATA_WDI", "This data set provides information on country development such as " +
                "Financial growth indicators, military capability, imports and exports, etc.");
            DescriptionList.Add("DF_UNDATA_SDG_PILOT", "Development Data - Sustainable development goals.");
            DescriptionList.Add("NA_MAIN", "This data set provides global financial information.");
            DescriptionList.Add("DF_UNDATA_WPP", "This data set provides information on population, population growth, etc.");
            DescriptionList.Add("NASEC_IDCNFSA_A", "This data set provides financial information.");
            DescriptionList.Add("NASEC_IDCFINA_Q", "This data set provides financial information.");
            DescriptionList.Add("NASEC_IDCFINA_A", "This data set provides financial information.");
            DescriptionList.Add("NASEC_IDCNFSA_Q", "This data set provides financial information.");
        }
    }
}
