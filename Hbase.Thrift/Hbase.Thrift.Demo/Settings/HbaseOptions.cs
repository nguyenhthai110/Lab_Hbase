using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hbase.Thrift.Demo.Settings
{
    public class HbaseOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string TableName { get; set; }
        public string ColFamily { get; set; }
        public string ColFamily1 { get; set; }
    }
}
