using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly.CoinCollect
{
    class Currency
    {
        public bool success { get; set; }
        public int timestamp { get; set; }
        public string @base { get; set; }
        public string date { get; set; }
        public Rates rates { get; set; }

        public class Rates
        {
            public double USD { get; set; }
            public double TWD { get; set; }
            public double JPY { get; set; }
            public double HKD { get; set; }
            public double CNY { get; set; }
            public double MYR { get; set; }
            public double SGD { get; set; }
            public double INR { get; set; }
        }
    }
}
