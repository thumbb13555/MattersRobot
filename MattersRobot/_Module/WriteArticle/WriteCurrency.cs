using MattersRobot._Module.Entitly.CoinCollect;
using MattersRobot._Module.Entitly;
using MattersRobot.Utils;
using Nancy.Helpers;
using Newtonsoft.Json;
using System;

using System.Text;


namespace MattersRobot._Module.WriteArticle
{
    class WriteCurrency : APIs
    {
        string token;
        CurrencyRespond respond;
        public WriteCurrency(string token, CurrencyRespond respond)
        {
            this.token = token;
            this.respond = respond;
            writeCurrencyArticle();
        }
        /**CurrencyArticle*/
        private async void writeCurrencyArticle()
        {
            StringBuilder exportString = new StringBuilder();

            /**Http query setting*/
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            
            queryString["id"] = "2909";
            exportString.Append("<h1>今日Likecoin-法幣匯率</h1>");

            /**GetCoinPrice(Like2USD2~~)*/
            /**Get all currency info*/
            queryString["convert"] = "USD";
            String usdAPI = String.Format(currencyBase, "USD");
            string jsonUSD = await HttpConnect.HttpConnect.sendGet(usdAPI);
            Currency usdCurrency = JsonConvert.DeserializeObject<Currency>(jsonUSD);

            string jsonLike2Usd = HttpConnect.HttpConnect.sendGet(GetQuotes, queryString);
            Like2USD priceLike2USD = JsonConvert.DeserializeObject<Like2USD>(jsonLike2Usd);
            double like2Usd = priceLike2USD.data.coin.quote.USD.price;
            WriteToFile($"取得報價: 1Like = {like2Usd}/USD");
            exportString.Append($"<p><strong>1Like = US${like2Usd.ToString("f4")} ({currencyCountriesLable[0]})</strong><br>" +
                $"≈ NT$ {(like2Usd * usdCurrency.rates.TWD).ToString("f3")} ({currencyCountriesLable[1]})<br class=smart>" +
                $"≈ ¥ {(like2Usd * usdCurrency.rates.JPY).ToString("f3")} ({currencyCountriesLable[2]})<br  class=smart>" +
                $"≈ HK$ {(like2Usd * usdCurrency.rates.HKD).ToString("f3")} ({currencyCountriesLable[3]})<br class=smart>" +
                $"≈ RMB￥ {(like2Usd * usdCurrency.rates.CNY).ToString("f3")} ({currencyCountriesLable[4]})<br class=smart>" +
                $"≈ RM {(like2Usd * usdCurrency.rates.MYR).ToString("f3")} ({currencyCountriesLable[5]})<br class=smart>" +
                $"≈ S$ {(like2Usd * usdCurrency.rates.SGD).ToString("f3")} ({currencyCountriesLable[6]})<br class=smart>" +
                $"≈ ₹ {(like2Usd * usdCurrency.rates.INR).ToString("f3")} ({currencyCountriesLable[7]})<br>" +
                $"</p>");
            exportString.Append("<hr/>");

            exportString.Append("<h1>今日Likecoin-其他虛擬幣匯率</h1>");
            /**GetCoinPrice(Like2BTC)*/
            exportString.Append(GetLike2BTC(queryString,"BTC"));
            /**GetCoinPrice(Like2ETH)*/
            exportString.Append(GetLike2ETH(queryString,"ETH"));
            /**GetCoinPrice(Like2STEEM)*/
            exportString.Append(GetLike2STEEM(queryString,"STEEM"));
            /**GetCoinPrice(Like2USDT)*/
            exportString.Append(GetLike2USDT(queryString,"USDT"));
            exportString.Append("<hr/>");

            exportString.Append("<h1>今日法幣匯率</h1>");
            for (int i = 0; i < currencyCodeArray.Length; i++)
            {
                exportString.Append($"<h1>{countriesLable[i]}</h1>");
                String sendAPI = String.Format(currencyBase, currencyCodeArray[i]);
                WriteToFile("SendAPI: "+sendAPI);
                string jsonCurrency = await HttpConnect.HttpConnect.sendGet(sendAPI);
                Currency currency = JsonConvert.DeserializeObject<Currency>(jsonCurrency);
                exportString.Append($"<p><strong>1{currencyCountriesLable[i]}</strong><br class=smart>" +
                $"≈ US$ {(currency.rates.USD).ToString("f5")} ({currencyCountriesLable[0]})<br class=smart>" +
                $"≈ NT$ {(currency.rates.TWD).ToString("f5")} ({currencyCountriesLable[1]})<br class=smart>" +
                $"≈ ¥ {(currency.rates.JPY).ToString("f5")} ({currencyCountriesLable[2]})<br  class=smart>" +
                $"≈ HK$ {(currency.rates.HKD).ToString("f5")} ({currencyCountriesLable[3]})<br class=smart>" +
                $"≈ RMB￥ {(currency.rates.CNY).ToString("f5")} ({currencyCountriesLable[4]})<br class=smart>" +
                $"≈ RM {(currency.rates.MYR).ToString("f5")} ({currencyCountriesLable[5]})<br class=smart>" +
                $"≈ S$ {(currency.rates.SGD).ToString("f5")} ({currencyCountriesLable[6]})<br class=smart>" +
                $"≈ ₹ {(currency.rates.INR).ToString("f5")} ({currencyCountriesLable[7]})<br>" +
                $"</p>");
                WriteToFile($"已取得{countriesLable[i]}報價");
            }
            WriteToFile("Today's currency success.");
            exportString.Append("<hr/>");
            exportString.Append("<H1>資訊來源: </H1>");
            exportString.Append("<p>換幣資訊: <a href = " + '"' + "https://coinmarketcap.com/api/documentation/v1/" + '"' + ">" +
                "CoinMarketCap</a><br class=smart>" +
                "匯率資訊: <a href = " + '"' + "https://fixer.io/" + '"' + ">" + "Fixer</a></p>");
            exportString.Append("<p><strong>小農碎碎念: <br class=smart>本日報由小農日報獨立製作，" +
                "資料來源如上僅供參考；若各位還有想看的資訊或者國家貨幣以及各種建議事項，都歡迎底下留言讓我知道喔OwO</strong></p>");
            
            string title = DateTime.Now.ToString("yyyy-MM-dd") + " 今日虛擬幣與法幣匯率日報";
            string[] tags = { "LikeCoin", "匯率", "虛擬幣/法幣匯率日報", "日報" };
            var coverPath = await GetMattersImage.getArticleId(getImageURL(CurrencyCover), token);
            WriteToFile($"封面ID: {coverPath.id}");
            WriteToFile($"封面連結: {coverPath.path}");
            respond.currencyRes(coverPath.id,title, "本文為小農每日統整各虛擬幣/各國匯率之報表數據", exportString,tags, token);
        }//
        /**GetCoinPrice(Like2BTC)*/
        private string GetLike2BTC(System.Collections.Specialized.NameValueCollection queryString,string coin)
        {
            StringBuilder exportString = new StringBuilder();
            queryString["convert"] = coin;
            string json = HttpConnect.HttpConnect.sendGet(GetQuotes, queryString);
            Like2BTC price = JsonConvert.DeserializeObject<Like2BTC>(json);
            Like2BTC.BTC info = price.data.coin.quote.BTC;
            WriteToFile($"取得報價: 1Like = {parseToDec(info.price)}/{coin}");
            exportString.Append($"<h1>Likecoin - 比特幣({coin})</h1>");
            exportString.Append("<p>");
            exportString.Append($"<strong>1Like = {parseToDec(info.price)} /{coin}</strong><br class=smart>");
            exportString.Append($"24小時價格走勢{percentagePoint(info.percent_change_24h)}<br class=smart>");
            exportString.Append($"7天價格走勢{percentagePoint(info.percent_change_7d)}<br class=smart>");
            exportString.Append($"1個月價格走勢{percentagePoint(info.percent_change_30d)}<br class=smart>");
            exportString.Append($"2個月價格走勢{percentagePoint(info.percent_change_60d)}<br class=smart>");
            exportString.Append($"3個月價格走勢{percentagePoint(info.percent_change_90d)}<br class=smart>");
            exportString.Append("</p>");
            return exportString.ToString();
        }
        private string GetLike2ETH(System.Collections.Specialized.NameValueCollection queryString, string coin)
        {
            StringBuilder exportString = new StringBuilder();
            queryString["convert"] = coin;
            string json = HttpConnect.HttpConnect.sendGet(GetQuotes, queryString);
            Like2ETH price = JsonConvert.DeserializeObject<Like2ETH>(json);
            Like2ETH.ETH info = price.data.coin.quote.ETH;
            WriteToFile($"取得報價: 1Like = {parseToDec(info.price)}/{coin}");
            exportString.Append($"<h1>Likecoin - 乙太幣({coin})</h1>");
            exportString.Append("<p>");
            exportString.Append($"<strong>1Like = {parseToDec(info.price)} /{coin}</strong><br class=smart>");
            exportString.Append($"24小時價格走勢{percentagePoint(info.percent_change_24h)}<br class=smart>");
            exportString.Append($"7天價格走勢{percentagePoint(info.percent_change_7d)}<br class=smart>");
            exportString.Append($"1個月價格走勢{percentagePoint(info.percent_change_30d)}<br class=smart>");
            exportString.Append($"2個月價格走勢{percentagePoint(info.percent_change_60d)}<br class=smart>");
            exportString.Append($"3個月價格走勢{percentagePoint(info.percent_change_90d)}<br class=smart>");
            exportString.Append("</p>");
            return exportString.ToString();
        }
        private string GetLike2STEEM(System.Collections.Specialized.NameValueCollection queryString, string coin)
        {
            StringBuilder exportString = new StringBuilder();
            queryString["convert"] = coin;
            string json = HttpConnect.HttpConnect.sendGet(GetQuotes, queryString);
            Like2STEEM price = JsonConvert.DeserializeObject<Like2STEEM>(json);
            Like2STEEM.STEEM info = price.data.coin.quote.STEEM;
            WriteToFile($"取得報價: 1Like = {parseToDec(info.price)}/{coin}");
            exportString.Append($"<h1>Likecoin - {coin}</h1>");
            exportString.Append("<p>");
            exportString.Append($"<strong>1Like = {parseToDec(info.price)} /{coin}</strong><br class=smart>");
            exportString.Append($"24小時價格走勢{percentagePoint(info.percent_change_24h)}<br class=smart>");
            exportString.Append($"7天價格走勢{percentagePoint(info.percent_change_7d)}<br class=smart>");
            exportString.Append($"1個月價格走勢{percentagePoint(info.percent_change_30d)}<br class=smart>");
            exportString.Append($"2個月價格走勢{percentagePoint(info.percent_change_60d)}<br class=smart>");
            exportString.Append($"3個月價格走勢{percentagePoint(info.percent_change_90d)}<br class=smart>");
            exportString.Append("</p>");
            return exportString.ToString();
        }
        private string GetLike2USDT(System.Collections.Specialized.NameValueCollection queryString, string coin)
        {
            StringBuilder exportString = new StringBuilder();
            queryString["convert"] = coin;
            string json = HttpConnect.HttpConnect.sendGet(GetQuotes, queryString);
            Like2USDT price = JsonConvert.DeserializeObject<Like2USDT>(json);
            Like2USDT.USDT info = price.data.coin.quote.USDT;
            WriteToFile($"取得報價: 1Like = {parseToDec(info.price)}/{coin}");
            exportString.Append($"<h1>Likecoin - 泰達幣({coin})</h1>");
            exportString.Append("<p>");
            exportString.Append($"<strong>1Like = {parseToDec(info.price)} /{coin}</strong><br class=smart>");
            exportString.Append($"24小時價格走勢{percentagePoint(info.percent_change_24h)}<br class=smart>");
            exportString.Append($"7天價格走勢{percentagePoint(info.percent_change_7d)}<br class=smart>");
            exportString.Append($"1個月價格走勢{percentagePoint(info.percent_change_30d)}<br class=smart>");
            exportString.Append($"2個月價格走勢{percentagePoint(info.percent_change_60d)}<br class=smart>");
            exportString.Append($"3個月價格走勢{percentagePoint(info.percent_change_90d)}<br class=smart>");
            exportString.Append("</p>");
            return exportString.ToString();
        }

        private string percentagePoint(double d)
        {
            if (d>0)
            {
                return " 上漲▲ " + d.ToString("f2") + "%";
            }
            else if(d<0){
                return " 下跌▼ " + d.ToString("f2") + "%";
            }
            else
            {
                return d.ToString("f2") + "%";
            }
        }
    }

    public interface CurrencyRespond
    {
        void currencyRes(string coverID, string title, string summary, StringBuilder content, string[] tags, string token);
    }
}
