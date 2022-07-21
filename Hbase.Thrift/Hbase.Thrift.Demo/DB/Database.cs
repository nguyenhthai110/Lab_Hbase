using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Logging;
using Hbase.Thrift.Demo.Settings;
using System.Data.SqlClient;
using Hbase.Thrift.Demo.Entites;

namespace Hbase.Thrift.Demo.DB
{
    public class Database : IDatabase
    {
        private readonly ILogger<Database> _logger;
        private readonly DbOptions _db;
        private readonly DbCommonOptions _dbCommon;
        public Database(ILogger<Database> logger, DbOptions db, DbCommonOptions dbCommon)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbCommon = dbCommon ?? throw new ArgumentNullException(nameof(dbCommon));

        }

        public List<string> LayDanhSachXN(List<string> DanhSachDaCo, int year, int xnCodeMin, int xnCodeMax, string systemWeb)
        {
            List<string> listReturn = new List<string>();
            DateTime date = new DateTime(year, 1, 1);

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(_dbCommon.conn);
            try
            {
                string store = _dbCommon.store;

                //store = String.Format("Company_GetDownload{0}", Config.IndexListXN);

                cmd = new SqlCommand(store, conn);
                cmd.Parameters.Add(new SqlParameter("@StartLogfileTime", date));
                cmd.Parameters.Add(new SqlParameter("@XNCodeMin", xnCodeMin));
                cmd.Parameters.Add(new SqlParameter("@XNCodeMax", xnCodeMax));

                SqlParameter param = new SqlParameter("@SystemWeb", systemWeb);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                cmd.Parameters.Add(param);

                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
                string TmpXN = string.Empty;
                int SoXN = 0;
                foreach (DataRow row in dt.Rows)
                {
                    TmpXN = row[0].ToString();
                    if (string.IsNullOrEmpty(TmpXN) == false && TmpXN.Contains(";") == false)
                    {
                        if (int.TryParse(TmpXN, out SoXN))
                        {
                            if (DanhSachDaCo.Contains(SoXN.ToString()) == false)
                                listReturn.Add(SoXN.ToString());
                        }
                    }
                }
            }
            catch
            {
                listReturn = new List<string>();
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return listReturn;

        }

        public List<ReportStop> Report_Stops_SelectAll(int companyID, DateTime startDate, DateTime endDate, string vehicleID, int totalTime, int minutesMechineOn, int minuteConditionOn, string sortField, int rowStart, int rowEnd)
        {
            List<ReportStop> lst = new List<ReportStop>();

            DataTable dtb = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection conn = new SqlConnection(_db.conn);
            try
            {
                string store = _db.store;

                cmd = new SqlCommand(store, conn);
                cmd.Parameters.Add(new SqlParameter("@CompanyID", companyID));
                cmd.Parameters.Add(new SqlParameter("@DateStart", startDate));
                cmd.Parameters.Add(new SqlParameter("@DateEnd", endDate));
                cmd.Parameters.Add(new SqlParameter("@VehicleID", vehicleID));
                cmd.Parameters.Add(new SqlParameter("@TotalTime", totalTime));
                cmd.Parameters.Add(new SqlParameter("@MinutesMechineOn", minutesMechineOn));
                cmd.Parameters.Add(new SqlParameter("@MinuteConditionOn", minuteConditionOn));
                cmd.Parameters.Add(new SqlParameter("@SortField", sortField));
                cmd.Parameters.Add(new SqlParameter("@RowStart", rowStart));
                cmd.Parameters.Add(new SqlParameter("@RowEnd", rowEnd));

                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dtb);

                if (dtb.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtb.Rows)
                    {
                        lst.Add(new ReportStop
                        {
                            FK_VehicleID = int.Parse(dr["FK_VehicleID"].ToString()),
                            StartTime = DateTime.Parse(dr["StartTime"].ToString()),
                            TotalTimeStop = int.Parse(dr["TotalTimeStop"].ToString()),
                            Longitude = double.Parse(dr["Longitude"].ToString()),
                            Latitude = double.Parse(dr["Latitude"].ToString()),
                            MinutesOfManchineOn = int.Parse(dr["MinutesOfManchineOn"].ToString()),
                            MinutesOfAirConditioningOn = int.Parse(dr["MinutesOfAirConditioningOn"].ToString()),
                            EndTime = DateTime.Parse(dr["EndTime"].ToString()),
                            DriverName = dr["DriverName"].ToString(),
                            DriverLicense = dr["DriverLicense"].ToString(),
                            PrivateCode = dr["PrivateCode"].ToString(),
                            VehiclePlate = dr["VehiclePlate"].ToString(),
                            GroupName = dr["GroupName"].ToString(),
                        });
                    }
                }
            }
            catch
            {
                lst = new List<ReportStop>();
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return lst;
        }
    }
}
