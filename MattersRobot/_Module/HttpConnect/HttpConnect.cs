using MattersRobot.Utils;
using Nancy.Helpers;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MattersRobot._Module.HttpConnect
{
    public class HttpConnect
    {
        public static string sendGet(string http, NameValueCollection queryString)
        {
            try
            {
                var URL = new UriBuilder(http);
                URL.Query = queryString.ToString();
                APIs.WriteToFile("Send API: " + URL.ToString());
                
                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", APIs.CoinKey);
                client.Headers.Add("Accepts", "application/json");
                return client.DownloadString(URL.ToString());
            }
            catch(Exception e)
            {
                return "0";
            }
            
        }//
        public static async Task<string> sendGet(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpResponseMessage response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    return "";
                }
            }
        }//
        public static string sendCovidGet(string http, NameValueCollection queryString)
        {
            var URL = new UriBuilder(http);
            URL.Query = queryString.ToString();
            Console.WriteLine(URL.ToString());
            var client = new WebClient();
            return client.DownloadString(URL.ToString());
        }//
    }
}
