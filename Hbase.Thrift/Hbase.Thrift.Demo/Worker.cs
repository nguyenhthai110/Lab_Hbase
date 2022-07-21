using Hbase.Thrift.Demo.DB;
using Hbase.Thrift.Demo.Entites;
using Hbase.Thrift.Demo.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocols;
using Thrift.Transports.Client;

namespace Hbase.Thrift.Demo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HbaseOptions _hbaseOption;
        private readonly IDatabase _database;
        private static Hbase.Client _hbase;
        public Worker(ILogger<Worker> logger, HbaseOptions hbaseOption, IDatabase database)
        {
            _logger = logger;
            _hbaseOption = hbaseOption;
            _database = database;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var ip = IPAddress.Parse(_hbaseOption.Host);
            var transport = new TBufferedClientTransport(new TSocketClientTransport(ip, _hbaseOption.Port));
            var proto = new TBinaryProtocol(transport);
            _hbase = new Hbase.Client(proto);

            try
            {
                await transport.OpenAsync();
                var names = await _hbase.getTableNamesAsync(CancellationToken.None);
                names.ForEach(msg => _logger.LogInformation(Encoding.UTF8.GetString(msg)));

                //await CreateTable();
                await Insert();
               // await Get();

                transport.Close();
            }
            catch (Exception e)
            {
                Console.Error.Write(e.ToString());
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>Get dữ liệu theo bảng & Column family</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// thainh2  7/21/2022   created
        /// </Modified>
        private async Task Get()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var scanner = await _hbase.scannerOpenAsync(Encoding.UTF8.GetBytes(_hbaseOption.TableName), Guid.Empty.ToByteArray(),
                                            new List<byte[]>() { Encoding.UTF8.GetBytes(_hbaseOption.ColFamily) }, new Dictionary<byte[], byte[]>(), CancellationToken.None);
                for (var entry = (await _hbase.scannerGetAsync(scanner, CancellationToken.None)); entry.Count > 0; entry = await _hbase.scannerGetAsync(scanner, CancellationToken.None))
                {
                    //foreach (var rowResult in entry)
                    //{
                    //    if (rowResult.Columns.Count > 0)
                    //    {
                    //        foreach (var item in rowResult.Columns)
                    //        {
                    //            Console.WriteLine($"{Encoding.UTF8.GetString(item.Key)}:{Encoding.UTF8.GetString(item.Value.Value)}");
                    //        }
                    //    }
                    //}
                }

                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);

            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex}");
            }
        }

        /// <summary>
        ///   <para>
        /// Put data</para>
        ///   <para>
        ///     <br />
        ///   </para>
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// thainh2  7/21/2022   created
        /// </Modified>
        private async Task Insert()
        {
            try
            {
                //var lstXN = _database.LayDanhSachXN(new List<string>(), 2022, 1, 10000, "GPS2");

                long _count = 1;
                BatchMutation batchMutation = null;
                // fake dữ liệu
                for (int i = 0; i < 300; i++)
                {
                    List<BatchMutation> lstBat = new List<BatchMutation>();
                    // lấy dữ liệu
                    var data = _database.Report_Stops_SelectAll(13578, DateTime.Parse("2017-01-01"), DateTime.Parse("2022-07-20"), "449026", 0, 0, 0, "", 1, 2147483647);

                    if (data.Count > 0)
                    {
                        foreach (ReportStop item in data)
                        {
                            batchMutation = new BatchMutation()
                            {
                                Row = Encoding.UTF8.GetBytes(_count.ToString()),
                                Mutations = new List<Mutation>
                            {
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:FK_VehicleID"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.FK_VehicleID}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:StartTime"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.StartTime}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:TotalTimeStop"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.TotalTimeStop}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:Longitude"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.Longitude}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:Latitude"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.Latitude}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:MinutesOfManchineOn"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.MinutesOfManchineOn}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:MinutesOfAirConditioningOn"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.MinutesOfAirConditioningOn}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:EndTime"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.EndTime}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:DriverName"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.DriverName}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:DriverLicense"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.DriverLicense}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:GroupName"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.GroupName}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:VehiclePlate"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.VehiclePlate}")},
                                new Mutation{Column = Encoding.UTF8.GetBytes($"{_hbaseOption.ColFamily}:PrivateCode"), IsDelete = false, Value = Encoding.UTF8.GetBytes($"{item.PrivateCode}")},
                            }
                            };

                            lstBat.Add(batchMutation);
                            _count++;
                        }
                    }
                    await _hbase.mutateRowsAsync(Encoding.UTF8.GetBytes(_hbaseOption.TableName), lstBat, new Dictionary<byte[], byte[]>(), CancellationToken.None);
                    _logger.LogInformation("PUT DONE : {0}", _count);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
            }
        }

        /// <summary>Tạo bảng</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// thainh2  7/21/2022   created
        /// </Modified>
        private async Task CreateTable()
        {
            try
            {
                //await _hbase.disableTableAsync(table_name, CancellationToken.None);
                //await _hbase.deleteTableAsync(table_name, CancellationToken.None);

                await _hbase.createTableAsync(
                    Encoding.UTF8.GetBytes(_hbaseOption.TableName),
                    new List<ColumnDescriptor>()
                        {
                        new ColumnDescriptor {Name =  Encoding.UTF8.GetBytes(_hbaseOption.ColFamily), InMemory = true},
                        },
                    CancellationToken.None
                    );
            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex}");
            }
        }
    }
}
