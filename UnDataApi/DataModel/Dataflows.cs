using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnDataApi.DataModel
{
    [XmlRoot("Dataflows")]
    public class Dataflows
    {
        public List<DataFlow> Flows { get; set; }
    }
}
