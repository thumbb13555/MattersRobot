using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly
{
    class PriceConvert
    {
        public class Convert
        {   
            public Data data { get; set; }
        }
        public class Status
        {
            public DateTime timestamp { get; set; }
            public int error_code { get; set; }
            public object error_message { get; set; }
            public int elapsed { get; set; }
            public int credit_count { get; set; }
            public object notice { get; set; }
        }

        public class USD
        {
            public double price { get; set; }
            public DateTime last_updated { get; set; }
        }

        public class Quote
        {
            public USD USD { get; set; }
        }

        public class Data
        {
            public int id { get; set; }
            public string symbol { get; set; }
            public string name { get; set; }
            public int amount { get; set; }
            public DateTime last_updated { get; set; }
            public Quote quote { get; set; }
        }
    }
}
