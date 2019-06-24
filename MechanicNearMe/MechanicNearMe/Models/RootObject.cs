using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicNearMe.Models
{
    class RootObject
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
        public string next_page_token { get; set; }
    }
}
