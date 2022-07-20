using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Logging;
using Hbase.Thrift.Demo.Settings;
using System.Data.SqlClient;

namespace Hbase.Thrift.Demo.DB
{
    public class Database : IDatabase
    {
        private readonly ILogger<Database> _logger;
        private readonly DbOptions _db;

        public Database(ILogger<Database> logger, DbOptions db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        /// <summary>
        /// Lấy danh sách XN mới
        /// </summary>
        public List<string> LayDanhSachXN(List<string> DanhSachDaCo, int year, int xnCodeMin, int xnCodeMax, string systemWeb)
        {
            List<string> listReturn = new List<string>();
            DateTime date = new DateTime(year, 1, 1);

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(_db.conn);
            try
            {
                string store = _db.store;

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


        public Dictionary<string, string> LayDanhSachMappingXNG7()
        {
            Dictionary<string, string> dicRet = new Dictionary<string, string>();

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(_db.conn);
            try
            {
                cmd = new SqlCommand("Company_GetDMapping_G7", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
               
                foreach (DataRow row in dt.Rows)
                {
                    string oblXNCode = string.Empty;
                    string g7XNCode = string.Empty;
                    oblXNCode = row[0].ToString();
                    g7XNCode = row[1].ToString();

                    if (!dicRet.ContainsKey(oblXNCode))
                        dicRet.Add(oblXNCode, g7XNCode);
                }
            }
            catch
            {
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return dicRet;

        }
    }
}
