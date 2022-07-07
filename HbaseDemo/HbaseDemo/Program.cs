using Microsoft.HBase.Client;
using org.apache.hadoop.hbase.rest.protobuf.generated;
using System;
using System.Threading.Tasks;

namespace HbaseDemo
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            // Connection
            var credentials = new ClusterCredentials(new Uri("https://CLUSTERNAME.azurehdinsight.net"), "USERNAME", "PASSWORD");
            var client = new HBaseClient(credentials);

            if (!client.ListTablesAsync().Result.name.Contains("RestSDKTable"))
            {
                // Create the table
                var newTableSchema = new TableSchema { name = "RestSDKTable" };
                newTableSchema.columns.Add(new ColumnSchema() { name = "t1" });
                newTableSchema.columns.Add(new ColumnSchema() { name = "t2" });

                await client.CreateTableAsync(newTableSchema);
            }


        }
    }
}
