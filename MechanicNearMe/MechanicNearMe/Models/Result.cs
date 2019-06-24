using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicNearMe.Models
{
    class Result
    {
        public string name { get; set; }
        public string vicinity { get; set; }
        public string place_id { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public Geometry geometry { get; set; }
    }
}
