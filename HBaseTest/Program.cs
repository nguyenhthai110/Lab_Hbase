using Hbase.Thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocols;
using Thrift.Transports;
using Thrift.Transports.Client;

namespace HBaseTest
{
    class Program
    {
        private static Hbase.Thrift.Hbase.Client _hbase;
        static byte[] table_name = Encoding.UTF8.GetBytes("emp");
        static readonly byte[] ID = Encoding.UTF8.GetBytes("personal data");
        static readonly byte[] NAME = Encoding.UTF8.GetBytes("professinal data");
        static readonly string host = "192.168.1.48";
        static readonly int port = 9090;
        static async Task Main(string[] args)
        {
            var ip = IPAddress.Parse(host);
            var transport = new TBufferedClientTransport(new TSocketClientTransport(ip, port));
            var proto = new TBinaryProtocol(transport);
            _hbase = new Hbase.Thrift.Hbase.Client(proto);

            try
            {
                await transport.OpenAsync();

                var names = await _hbase.getTableNamesAsync(CancellationToken.None);
                names.ForEach(msg => Console.WriteLine(Encoding.UTF8.GetString(msg)));

                //await CreateTable();
                await Insert();
                //await Get();

                Console.ReadLine();
                transport.Close();
            }
            catch (Exception e)
            {
                Console.Error.Write(e.ToString());
            }
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
                        Row = Encoding.UTF8.GetBytes("6"),
                        Mutations = new List<Mutation> {
                            new Mutation{Column = Encoding.UTF8.GetBytes("personal data:Id"), IsDelete = false, Value = Encoding.UTF8.GetBytes("2020") },
                            new Mutation{Column = Encoding.UTF8.GetBytes("personal data:Name"), IsDelete = false, Value = Encoding.UTF8.GetBytes("Nguyen Thi Mit") },
                            new Mutation{Column = Encoding.UTF8.GetBytes("professinal data:Designation"), IsDelete = false, Value = Encoding.UTF8.GetBytes("Testing") },
                            new Mutation{Column = Encoding.UTF8.GetBytes("professinal data:Salary"), IsDelete = false, Value = Encoding.UTF8.GetBytes("20000") },
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
