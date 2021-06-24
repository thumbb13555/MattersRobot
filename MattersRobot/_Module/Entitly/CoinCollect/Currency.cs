using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly.CoinCollect
{
    class Currency
    {

        public TWD USDTWD { get; set; }
        public JPY USDJPY { get; set; }
        public HKD USDHKD { get; set; }
        public CNY USDCNY { get; set; }
        public MYR USDMYR { get; set; }
        public SGD USDSGD { get; set; }

        public class TWD
        {
            public double Exrate { get; set; }
            public string UTC { get; set; }
        }

        public class JPY
        {
            public double Exrate { get; set; }
            public string UTC { get; set; }
        }
        public class HKD
        {
            public double Exrate { get; set; }
            public string UTC { get; set; }
        }

        public class CNY
        {
            public double Exrate { get; set; }
            public string UTC { get; set; }
        }

        public class MYR
        {
            public double Exrate { get; set; }
            public string UTC { get; set; }
        }

        public class SGD
        {
            public double Exrate { get; set; }
            public string UTC { get; set; }
        }
    }
}
