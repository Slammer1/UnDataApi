using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class DataSeries
    {
        public List<DataPoint> DataPoints { get; set; }

        public Dictionary<string, string> Dimensions { get; set; }

        public DataSeries()
        {
            DataPoints = new List<DataPoint>();
            Dimensions = new Dictionary<string, string>();
        }
    }
}
