using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using Hbase.Thrift.Demo.Entites;

namespace Hbase.Thrift.Demo.DB
{
    public interface IDatabase
    {
        List<string> LayDanhSachXN(List<string> DanhSachDaCo, int year, int xnCodeMin, int xnCodeMax, string systemWeb);
        List<ReportStop> Report_Stops_SelectAll(int companyID, DateTime startDate, DateTime endDate, string vehicleID, int totalTime, int minutesMechineOn, int minuteConditionOn, string sortField, int rowStart, int rowEnd);
    }
}
