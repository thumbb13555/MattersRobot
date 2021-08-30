using MattersRobot._Module.Entitly;
using MattersRobot._Module.Entitly.CoinCollect;
using MattersRobot.Utils;
using Nancy.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MattersRobot._Module.Action
{
    class UpdateCoinPrice: APIs
    {
        
        public UpdateCoinPrice()
        {
            init();
        }
        private async void init()
        {
            WriteToFile("更新幣價資訊: ");
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["id"] = "2909";
            queryString["convert"] = "USD";
            String usdAPI = String.Format(currencyBase, "USD");
            string jsonUSD = await HttpConnect.HttpConnect.sendGet(usdAPI);
            Currency usdCurrency = JsonConvert.DeserializeObject<Currency>(jsonUSD);

            string jsonLike2Usd = HttpConnect.HttpConnect.sendGet(GetQuotes, queryString);
            Like2USD priceLike2USD = JsonConvert.DeserializeObject<Like2USD>(jsonLike2Usd);
            double like2Usd = priceLike2USD.data.coin.quote.USD.price;
            WriteToFile($"取得報價: 1Like = {like2Usd}/USD");
            string[] prices = new string[] {
                (like2Usd).ToString("f4"),
                (like2Usd * usdCurrency.rates.TWD).ToString("f3"),
                (like2Usd * usdCurrency.rates.JPY).ToString("f3"),
                (like2Usd * usdCurrency.rates.HKD).ToString("f3"),
                (like2Usd * usdCurrency.rates.CNY).ToString("f3"),
                (like2Usd * usdCurrency.rates.MYR).ToString("f3"),
                (like2Usd * usdCurrency.rates.SGD).ToString("f3"),
                (like2Usd * usdCurrency.rates.INR).ToString("f3")
            };

            for(int i=0;i< currencyCodeArray.Length; i++)
            {
                string price = prices[i];
                string quote = currencyCodeArray[i];
                sendInfo(price,quote);
            }

        }
        private async void sendInfo(string price, string quote)
        {
            string url = InsertCoinInfo + $"?coin=2909&name=LikeCoin&price={price}&quote={quote}";
            string res = await HttpConnect.HttpConnect.sendGet(url);
            Console.WriteLine(res);

        }



    }
}
