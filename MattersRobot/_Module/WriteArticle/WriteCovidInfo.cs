using System;
using System.Collections.Generic;
using System.Text;

using MattersRobot._Module.Entitly;
using MattersRobot.Utils;
using Nancy.Helpers;
using Newtonsoft.Json;

namespace MattersRobot._Module.WriteArticle
{
    class WriteCovidInfo : APIs
    {
        string token;
        COVIDRespond respond;

        public WriteCovidInfo(string token, COVIDRespond respond)
        {
            this.token = token;
            this.respond = respond;
            writeCovidArticle();
        }

        private void writeCovidArticle()
        {
            StringBuilder exportString = new StringBuilder();
            exportString.Append("<h1>最新COVID-19確診數</h1>");
            var covidQueryString = HttpUtility.ParseQueryString(string.Empty);
            covidQueryString["yesterday"] = "true";
            string jsonCovid = HttpConnect.HttpConnect.sendGet(CovidData, covidQueryString);
            List<CovidInfo> covidInfo = JsonConvert.DeserializeObject<List<CovidInfo>>(jsonCovid);
            for(int i = 0; i < covidInfo.Count; i++)
            {
                var item = covidInfo[i];
                exportString.Append($"<H2><strong>{countriesLable[i]}</strong></H2>");
                exportString.Append($"<p>最新確診數: {item.todayCases.ToString("N0")}, 累計總確診數: {item.cases.ToString("N0")}</p>");
                exportString.Append($"<p>最新死亡數: {item.todayDeaths.ToString("N0")}, 累計總死亡數: {item.deaths.ToString("N0")}</p>");
                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var time = start.AddMilliseconds(item.updated).ToLocalTime().ToString("yyyy-MM-dd, HH:mm:ss");
                exportString.Append($"<p>API資訊更新時間: {time}</p>");
                WriteToFile(countries[i] + "昨日確診: " + item.todayCases.ToString("N0") + ", 昨日死亡: " + item.todayDeaths.ToString("N0") + ",  更新時間: "+ item.updated.ToString("yyyy-MM-dd, HH:mm:ss"));

            }
            string title = DateTime.Now.ToString("yyyy-MM-dd") + " Covid-19各國確診數日報";

            exportString.Append("<hr/>");
            exportString.Append("<H1>API來源: </H1>");
            exportString.Append("<p>NovelCOVID API: <a href = " + '"' + "https://github.com/disease-sh/API" + '"' + ">" +
                "https://documenter.getpostman.com/view/11144369/Szf6Z9B3?version=latest#a9a60f59-fde4-4e94-b1f1-a3cb92bd1046 </a></p>");
            string[] tags = { "COVID-19 各國疫情日報", "COVID-19", "COVID", "covid", "WebMonetizationMatters" };
            respond.resCOVIDInfo(title, "本文為伺服器每日統整各國Covid-19資訊之報表數據。", exportString,tags, token);
        }

    }
    public interface COVIDRespond
    {
        void resCOVIDInfo(string title, string summary, StringBuilder content,string[] tags, string token);
    }
}
