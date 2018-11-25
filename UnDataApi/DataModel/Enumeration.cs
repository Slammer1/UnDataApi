﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class Enumeration
    {
        public string Class { get; set; }

        public string Id { get; set; }

        public string Version { get; set; }

        public string AgencyId { get; set; }

        public string Package { get; set; }

        public EnumerationFormat Format { get; set; }
    }
}
