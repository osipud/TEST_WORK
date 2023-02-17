using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsPing
{
    public class DataClass
    {
        public string? NmeaMessage { get; set; }
        public int CurrentSpeed { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
