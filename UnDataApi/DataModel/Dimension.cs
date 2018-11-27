using System;
using System.Collections.Generic;
using System.Linq;
using UnDataApi.Services;

namespace UnDataApi.DataModel
{
    public class Dimension
    {
        public int Position { get; set; }

        public string Id { get; set; }

        public ConceptIdentity ConceptId { get; set; }

        public LocalRepresentation Representation { get; set; }

        public Dictionary<string, string> CodeList { get; set; }

        public string CodeListName { get; set; }

        public Dictionary<string, string> GetCodeList()
        {
            if(Representation == null || Representation.Enumeration == null)
            {
                return null;
            }
            Dictionary<string, string> codeList = new Dictionary<string, string>();
            UnDataService service = new UnDataService();
            codeList = service.GetCodes(Representation.Enumeration.AgencyId, Representation.Enumeration.Id);
            this.CodeList = codeList;
            return codeList;
        }
    }
}
