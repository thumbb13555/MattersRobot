using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly
{
    class Like2TWD
    {

        public Data data { get; set; }

        public class TWD
        {
            public double price { get; set; }
            public double volume_24h { get; set; }
            public double percent_change_1h { get; set; }
            public double percent_change_24h { get; set; }
            public double percent_change_7d { get; set; }
            public double percent_change_30d { get; set; }
            public double percent_change_60d { get; set; }
            public double percent_change_90d { get; set; }
            public double market_cap { get; set; }
            public DateTime last_updated { get; set; }
        }

        public class Quote
        {
            public TWD TWD { get; set; }
        }

        public class LikeCoin
        {
            public int id { get; set; }
            public string name { get; set; }
            public string symbol { get; set; }
            public string slug { get; set; }
            public int num_market_pairs { get; set; }
            public DateTime date_added { get; set; }
            public List<string> tags { get; set; }
            public int max_supply { get; set; }
            public double circulating_supply { get; set; }
            public double total_supply { get; set; }
            public int is_active { get; set; }
            public object platform { get; set; }
            public int cmc_rank { get; set; }
            public int is_fiat { get; set; }
            public DateTime last_updated { get; set; }
            public Quote quote { get; set; }
        }

        public class Data
        {
            [JsonProperty(PropertyName = "2909")]
            public LikeCoin Likecoin { get; set; }
        }
    }

}
