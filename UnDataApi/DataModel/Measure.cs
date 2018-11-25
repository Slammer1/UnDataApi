using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class Measure
    {

        public ConceptIdentity ConceptId { get; set; }

        public LocalRepresentation LocalRep { get; set; }

        public string Id { get; set; }
    }
}
