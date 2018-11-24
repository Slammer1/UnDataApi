using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class UnData : IUnData
    {
        public virtual string Name { get; set; }

        public virtual string RawValue { get; set; }

        public virtual string UrlLocation { get; set; }
    }
}
