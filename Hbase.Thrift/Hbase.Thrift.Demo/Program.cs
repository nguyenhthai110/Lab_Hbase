using Hbase.Thrift.Demo.DB;
using Hbase.Thrift.Demo.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Hbase.Thrift.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   IConfiguration configuration = hostContext.Configuration;
                   var dbOption = new DbOptions();
                   var dbCommonOption = new DbCommonOptions();
                   var hbaseOption = new HbaseOptions();
                   configuration.Bind("ConnectionStrings:SqlServer", dbOption);
                   configuration.Bind("ConnectionStrings:SqlCommon", dbCommonOption);
                   configuration.Bind("Hbase", hbaseOption);
                   services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
                   services.AddSingleton<IDatabase, Database>();
                   services.AddSingleton(dbOption);
                   services.AddSingleton(dbCommonOption);
                   services.AddSingleton(hbaseOption);
                   services.AddHostedService<Worker>();
               });
    }
}
