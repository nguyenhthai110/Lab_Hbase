﻿using Hbase.Thrift.Demo.DB;
using Hbase.Thrift.Demo.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        static byte[] table_name = Encoding.UTF8.GetBytes("emp");
        static readonly byte[] ID = Encoding.UTF8.GetBytes("personal data");
        static readonly byte[] NAME = Encoding.UTF8.GetBytes("professinal data");

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

                List<string> lstXN = new List<string>();
                lstXN = _database.LayDanhSachXN(new List<string>(), 2022, 0, 10000, "GPS6");
                //var names = await _hbase.getTableNamesAsync(CancellationToken.None);
                //names.ForEach(msg => Console.WriteLine(Encoding.UTF8.GetString(msg)));

                //await CreateTable();
                await Insert();
                //await Get();

                


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

        private static async Task Get()
        {
            var scanner = await _hbase.scannerOpenAsync(table_name, Guid.Empty.ToByteArray(),
                                             new List<byte[]>() { ID, NAME }, new Dictionary<byte[], byte[]>(), CancellationToken.None);
            for (var entry = (await _hbase.scannerGetAsync(scanner, CancellationToken.None)); entry.Count > 0; entry = await _hbase.scannerGetAsync(scanner, CancellationToken.None))
            {
                foreach (var rowResult in entry)
                {
                    if (rowResult.Columns.Count > 0)
                    {
                        foreach (var item in rowResult.Columns)
                        {
                            Console.WriteLine($"{Encoding.UTF8.GetString(item.Key)}:{Encoding.UTF8.GetString(item.Value.Value)}");
                        }
                    }
                }
            }

        }

        private static async Task Insert()
        {
            try
            {

               


                await _hbase.mutateRowsAsync(table_name, new List<BatchMutation>()
                {
                    new BatchMutation()
                    {
                        Row = Encoding.UTF8.GetBytes("8"),
                        Mutations = new List<Mutation> {
                            new Mutation{Column = Encoding.UTF8.GetBytes("personal data:Id"), IsDelete = false, Value = Encoding.UTF8.GetBytes("2023") },
                            new Mutation{Column = Encoding.UTF8.GetBytes("personal data:Name"), IsDelete = false, Value = Encoding.UTF8.GetBytes("Nguyen Thi Dao") },
                            new Mutation{Column = Encoding.UTF8.GetBytes("professinal data:Designation"), IsDelete = false, Value = Encoding.UTF8.GetBytes("IT") },
                            new Mutation{Column = Encoding.UTF8.GetBytes("professinal data:Salary"), IsDelete = false, Value = Encoding.UTF8.GetBytes("40000") },
                        }
                    }

                }, new Dictionary<byte[], byte[]>(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        private static async Task CreateTable()
        {
            //await _hbase.disableTableAsync(table_name, CancellationToken.None);
            //await _hbase.deleteTableAsync(table_name, CancellationToken.None);

            await _hbase.createTableAsync(
                table_name,
                new List<ColumnDescriptor>()
                    {
                        new ColumnDescriptor {Name = ID, InMemory = true},
                        new ColumnDescriptor {Name = NAME, InMemory = true}
                    },
                CancellationToken.None
                );
        }
    }
}