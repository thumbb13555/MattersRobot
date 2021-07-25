using MattersRobot._Module.Entitly.CoinCollect;
using MattersRobot._Module.Entitly;
using MattersRobot.Utils;
using Nancy.Helpers;
using Newtonsoft.Json;
using System;

using System.Text;


namespace MattersRobot._Module.WriteArticle
{
    class WriteCurrency: APIs
    {
        string token;
        CurrencyRespond respond;
        public WriteCurrency(string token,CurrencyRespond respond)
        {
            this.token = token;
            this.respond = respond;
            writeCurrencyArticle();
        }
        /**CurrencyArticle*/
        private async void writeCurrencyArticle()
        {
            StringBuilder exportString = new StringBuilder();
            
            /**Get all currency info*/
            string jsonCurrency = await HttpConnect.HttpConnect.sendGet(GetCurrency);
            Currency currency = JsonConvert.DeserializeObject<Currency>(jsonCurrency);

            /**Http query setting*/
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["amount"] = "1";
            queryString["id"] = "2909";

            exportString.Append("<h1>今日虛擬幣-法幣匯率</h1>");
            /**GetCoinPrice(Like2USD2~~)*/
            queryString["convert"] = "USD";
            string jsonUsd = HttpConnect.HttpConnect.sendGet(GetPriceConversion, queryString);
            Like2USD priceLike2USD = JsonConvert.DeserializeObject<Like2USD>(jsonUsd);
            double like2Usd = priceLike2USD.data.quote.usd.price;
            WriteToFile($"取得報價: 1Like = {like2Usd}/USD");
            exportString.Append($"<p><strong>1Like = US${like2Usd} (美金)</strong><br>" +
                $"≈ NT$ {like2Usd * currency.USDTWD.Exrate} (台幣)<br>" +
                $"≈ ¥ {like2Usd * currency.USDJPY.Exrate} (日幣)<br>" +
                $"≈ HK$ {like2Usd * currency.USDHKD.Exrate} (港幣)<br>" +
                $"≈ RMB￥ {like2Usd * currency.USDCNY.Exrate} (人民幣)<br>" +
                $"≈ RM {like2Usd * currency.USDMYR.Exrate} (令吉)<br>" +
                $"≈ S$ {like2Usd * currency.USDSGD.Exrate} (新加坡幣)<br>" +
                $"</p>");
            exportString.Append("<hr/>");

            exportString.Append("<h1>今日虛擬幣-虛擬幣匯率</h1>");
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

            exportString.Append("<h1>今日法幣匯率(美金-其他法幣)</h1>");
            exportString.Append($"<p>USD 換 <strong>TWD(台幣) </strong>= 1 : <strong>{currency.USDTWD.Exrate}</strong></p>");
            exportString.Append($"<p>USD 換 <strong>JPY(日幣) </strong>= 1 : <strong>{currency.USDJPY.Exrate}</strong></p>");
            exportString.Append($"<p>USD 換 <strong>HKD(港元) </strong>= 1 : <strong>{currency.USDHKD.Exrate}</strong></p>");
            exportString.Append($"<p>USD 換 <strong>CNY(人民幣) </strong>= 1 : <strong>{currency.USDCNY.Exrate}</strong></p>");
            exportString.Append($"<p>USD 換 <strong>MYR(馬來西亞林吉特) </strong>= 1 : <strong>{currency.USDMYR.Exrate}</strong></p>");
            exportString.Append($"<p>USD 換 <strong>SGD(新加坡幣) </strong>= 1 : <strong>{currency.USDSGD.Exrate}</strong></p>");
            WriteToFile("Today's currency success.");
            exportString.Append("<hr/>");
            exportString.Append("<H1>資訊來源: </H1>");
            exportString.Append("<p>換幣資訊: <a href = " + '"' + "https://coinmarketcap.com/api/documentation/v1/" + '"' + ">" +
                "CoinMarketCap</a></p>");
            exportString.Append("<p>匯率資訊: <a href = " + '"' + "https://tw.rter.info/" + '"' + ">" + "即匯站</a></p>");
            string title = DateTime.Now.ToString("yyyy-MM-dd") + " 今日虛擬幣與法幣匯率日報";
            string[] tags = {"LikeCoin","匯率","虛擬幣/法幣匯率日報", "日報" };
            var coverPath = await GetMattersImage.getArticleId(getImageURL(CurrencyCover), token);
            respond.currencyRes(coverPath.id,title, "本文為小農每日統整各虛擬幣/各國匯率之報表數據", exportString,tags, token);
        }
    }

    public interface CurrencyRespond
    {
        void currencyRes(string coverID, string title, string summary, StringBuilder content,string[] tags, string token);
    }
}
