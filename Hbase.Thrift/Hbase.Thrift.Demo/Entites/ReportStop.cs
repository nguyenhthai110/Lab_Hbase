using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hbase.Thrift.Demo.Entites
{
    public class ReportStop
    {
        public int FK_VehicleID { get; set; }
        public DateTime StartTime { get; set; }
        public int TotalTimeStop { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int MinutesOfManchineOn { get; set; }
        public int MinutesOfAirConditioningOn { get; set; }
        public DateTime EndTime { get; set; }
        public string DriverName { get; set; }
        public string DriverLicense { get; set; }
        public string GroupName { get; set; }
        public string VehiclePlate { get; set; }
        public string PrivateCode { get; set; }
    }
}
