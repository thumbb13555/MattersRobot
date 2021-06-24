using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly.CoinCollect
{
    class Like2BTC
    {
        public Data data { get; set; }
        public class Data
        {
            public int id { get; set; }
            public string symbol { get; set; }
            public string name { get; set; }
            public int amount { get; set; }
            public DateTime last_updated { get; set; }
            public Quote quote { get; set; }
        }

        public class Quote
        {
            public BTC Btc { get; set; }
        }

        public class BTC
        {
            public double price { get; set; }
            public DateTime last_updated { get; set; }
        }

    }
}
