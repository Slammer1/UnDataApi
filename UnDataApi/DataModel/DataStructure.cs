using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
