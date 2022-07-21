using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hbase.Thrift.Demo.Settings
{
    public class DbOptions
    {
        public string conn { get; set; }
        public string store { get; set; }
    }

    public class DbCommonOptions
    {
        public string conn { get; set; }
        public string store { get; set; }
    }
}
