using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnDataApi.DataModel
{
    public class DataSet
    {
        public List<DataSeries> DataSeries { get; set; }

        public DataSet()
        {
            DataSeries = new List<DataSeries>();
        } 
    }
}
