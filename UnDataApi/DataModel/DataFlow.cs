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

        public Structure Structure { get; set; }

        public DataStructure DataStructure { get; set; }

        public DataStructure GetDataStructure()
        {
            DataStructure structure = new DataStructure();
            Services.UnDataService service = new Services.UnDataService();
            structure = service.GetDataStructure(Structure.AgencyId, Structure.Id);
            return structure;

        }
    }
}
