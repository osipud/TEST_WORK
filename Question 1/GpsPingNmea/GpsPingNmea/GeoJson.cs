using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsPingNmea
{
    public class RootGeo
    {
        public string type { get; set; } = "FeatureCollection";
        public List<Feature> features { get; set; }
    }
    public class Feature
    {
        public string type { get; set; } = "Feature";
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }
    public class Properties
    {
        public string name { get; set; }
        public object description { get; set; } = null;
    }
    public class Geometry
    {
        public string type { get; set; }
        public List<object> coordinates { get; set; }
    }




}
