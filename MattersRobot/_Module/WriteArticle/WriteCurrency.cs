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
            queryString["amount"] = "1";
            queryString["id"] = "2909";

            exportString.Append("<h1>今日Likecoin-法幣匯率</h1>");
            /**GetCoinPrice(Like2USD2~~)*/
            /**Get all currency info*/
            queryString["convert"] = "USD";

            String usdAPI = String.Format(currencyBase, "USD");
            string jsonUSD = await HttpConnect.HttpConnect.sendGet(usdAPI);
            Currency usdCurrency = JsonConvert.DeserializeObject<Currency>(jsonUSD);

            string jsonLike2Usd = HttpConnect.HttpConnect.sendGet(GetPriceConversion, queryString);
            Like2USD priceLike2USD = JsonConvert.DeserializeObject<Like2USD>(jsonLike2Usd);
            double like2Usd = priceLike2USD.data.quote.usd.price;
            WriteToFile($"取得報價: 1Like = {like2Usd}/USD");
            exportString.Append($"<p><strong>1Like = US${like2Usd.ToString("f4")} (美金)</strong><br>" +
                $"≈ NT$ {(like2Usd * usdCurrency.rates.TWD).ToString("f3")} (台幣)<br class=smart>" +
                $"≈ ¥ {(like2Usd * usdCurrency.rates.JPY).ToString("f3")} (日幣)<br  class=smart>" +
                $"≈ HK$ {(like2Usd * usdCurrency.rates.HKD).ToString("f3")} (港幣)<br class=smart>" +
                $"≈ RMB￥ {(like2Usd * usdCurrency.rates.CNY).ToString("f3")} (人民幣)<br class=smart>" +
                $"≈ RM {(like2Usd * usdCurrency.rates.MYR).ToString("f3")} (令吉)<br class=smart>" +
                $"≈ S$ {(like2Usd * usdCurrency.rates.SGD).ToString("f3")} (新加坡幣)<br class=smart>" +
                $"≈ ₹ {(like2Usd * usdCurrency.rates.INR).ToString("f3")} (印度盧比)<br>" +
                $"</p>");
            exportString.Append("<hr/>");

            exportString.Append("<h1>今日Likecoin-其他虛擬幣匯率</h1>");
            /**GetCoinPrice(Like2BTC)*/
            queryString["convert"] = "BTC";
            string jsonBtc = HttpConnect.HttpConnect.sendGet(GetPriceConversion, queryString);
            Like2BTC priceLike2BTC = JsonConvert.DeserializeObject<Like2BTC>(jsonBtc);
            string like2Btc = parseToDec(priceLike2BTC.data.quote.Btc.price);
            WriteToFile($"取得報價: 1Like = {like2Btc}/BTC");
            exportString.Append($"1Like = {like2Btc} /BTC<br>");

            /**GetCoinPrice(Like2ETH)*/
            queryString["convert"] = "ETH";
            string jsonETH = HttpConnect.HttpConnect.sendGet(GetPriceConversion, queryString);
            Like2ETH priceLike2ETH = JsonConvert.DeserializeObject<Like2ETH>(jsonETH);
            string like2ETH = parseToDec(priceLike2ETH.data.quote.Eth.price);
            WriteToFile($"取得報價: 1Like = {like2ETH}/ETH");
            exportString.Append($"1Like = {like2ETH} /ETH<br>");

            /**GetCoinPrice(Like2STEEM)*/
            queryString["convert"] = "STEEM";
            string jsonSTEEM = HttpConnect.HttpConnect.sendGet(GetPriceConversion, queryString);
            Like2STEEM priceLike2STEEM = JsonConvert.DeserializeObject<Like2STEEM>(jsonSTEEM);
            string like2STEEM = parseToDec(priceLike2STEEM.data.quote.Steem.price);
            WriteToFile($"取得報價: 1Like = {like2STEEM}/STEEM");
            exportString.Append($"1Like = {like2STEEM} /STEEM<br>");

            /**GetCoinPrice(Like2USDT)*/
            queryString["convert"] = "USDT";
            string jsonUSDT = HttpConnect.HttpConnect.sendGet(GetPriceConversion, queryString);
            Like2USDT priceLike2USDT = JsonConvert.DeserializeObject<Like2USDT>(jsonUSDT);
            string like2USDT = parseToDec(priceLike2USDT.data.quote.Usdt.price);
            WriteToFile($"取得報價: 1Like = {like2USDT}/USDT");
            exportString.Append($"1Like = {like2USDT} /USDT<br>");
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
                $"≈ NT$ {(currency.rates.TWD).ToString("f5")} (台幣)<br class=smart>" +
                $"≈ ¥ {(currency.rates.JPY).ToString("f5")} (日幣)<br  class=smart>" +
                $"≈ HK$ {(currency.rates.HKD).ToString("f5")} (港幣)<br class=smart>" +
                $"≈ RMB￥ {(currency.rates.CNY).ToString("f5")} (人民幣)<br class=smart>" +
                $"≈ RM {(currency.rates.MYR).ToString("f5")} (令吉)<br class=smart>" +
                $"≈ S$ {(currency.rates.SGD).ToString("f5")} (新加坡幣)<br class=smart>" +
                $"≈ ₹ {(currency.rates.INR).ToString("f5")} (印度盧比)<br>" +
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
            respond.currencyRes(coverPath.id,title, "本文為小農每日統整各虛擬幣/各國匯率之報表數據", exportString,tags, token);
        }
    }

    public interface CurrencyRespond
    {
        void currencyRes(string coverID, string title, string summary, StringBuilder content, string[] tags, string token);
    }
}
