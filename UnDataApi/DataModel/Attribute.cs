using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class Attribute
    {

        public string Id { get; set; }

        public string AssignmentStatus { get; set; }

        public ConceptIdentity ConceptId { get; set; }

        public LocalRepresentation LocalRep { get; set; }

        public string AttributeRelationship { get; set; }
    }
}
