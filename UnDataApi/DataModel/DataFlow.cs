using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnDataApi.DataModel
{
    [XmlRootAttribute("Dataflow", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    public class DataFlow : IDataFlow
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("isFinal")]
        public bool IsFinal { get; set; }
        [XmlAttribute("version")]
        public string Version { get; set; }
        [XmlAttribute("agencyId")]
        public string AgencyId { get; set; }
    }
}
