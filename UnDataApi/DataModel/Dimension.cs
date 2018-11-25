using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class Dimension
    {
        public int Position { get; set; }

        public string Id { get; set; }

        public ConceptIdentity ConceptId { get; set; }

        public LocalRepresentation Representation { get; set; }
    }
}
