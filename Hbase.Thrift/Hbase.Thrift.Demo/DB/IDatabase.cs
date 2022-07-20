using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Hbase.Thrift.Demo.DB
{
    public interface IDatabase
    {
        List<string> LayDanhSachXN(List<string> DanhSachDaCo, int year, int xnCodeMin, int xnCodeMax, string systemWeb);

        Dictionary<string, string> LayDanhSachMappingXNG7();
    }
}
