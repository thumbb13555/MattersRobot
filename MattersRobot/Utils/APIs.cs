using GraphQL;
using System;
using System.Globalization;
using System.IO;
using System.ServiceProcess;

namespace MattersRobot.Utils
{
    public abstract class APIs : ServiceBase
    {
        /**BTC id = 1
          LikeCoin id = 2909
          USDT id = 825
          ETH id = 1027
          STEEM id = 1230
         */
        /**貨幣代號
            USD(美元)
            TWD(台幣)
            JPY(日圓)
            HKD(港元)
            CNY(人民幣)
            MYR(馬來西亞林吉特)
            SGD(新加坡)

        國家代號
        TW
        HK
        CN
        US
        JP
        MY
        SG
            */

        public const string countries = "TW,US,JP,HK,CN,MY,SG,IN";
        public string[] countriesLable = new string[] { "台灣", "美國", "日本", "香港", "中國", "馬來西亞", "新加坡", "印度" };
        public const string GetCurrency = "https://tw.rter.info/capi.php";
        public const string CoinKey = "53bc319d-7249-49da-b89f-672730481632";
        public const string GetPriceConversion = "https://pro-api.coinmarketcap.com/v1/tools/price-conversion";//價格換算
        public const int tryLimit = 30;
        public string CovidData = $"https://corona.lmao.ninja/v2/countries/{countries}";    
        public const string baseAPI = "https://server.matters.news/graphql";
        public int earlyMorning = Int32.Parse(DateTime.Today.AddHours(7).AddMinutes(00).ToString("HHmmss"));//早上7點00分
        public int morning = Int32.Parse(DateTime.Today.AddHours(10).AddMinutes(00).ToString("HHmmss"));//早上10點
        public int noon = Int32.Parse(DateTime.Today.AddHours(12).AddMinutes(00).ToString("HHmmss"));//中午12點
        public int afternoon = Int32.Parse(DateTime.Today.AddHours(15).AddMinutes(00).ToString("HHmmss"));//下午3點
        public int evening = Int32.Parse(DateTime.Today.AddHours(17).ToString("HHmmss"));//下午5點
        public int night = Int32.Parse(DateTime.Today.AddHours(20).AddMinutes(30).ToString("HHmmss"));//晚上3點30分
        public const string UserName = "";
        public string Account = $"{UserName}@gmail.com";
        public const string Password = "";
        public static string token = "";
        public string parseToDec(double d)
        {
            return Decimal.Parse(d.ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint).ToString();
        }
        /**Writeing a diary log */
        public static void WriteToFile(string Message)
        {
            Console.WriteLine(Message);
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        public GraphQLRequest loginGraphQL(string email, string password)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                mutation Login($email:EmailAddress!, $password:String!){
                  userLogin(input:{email:$email,password:$password}){
                    token
                  }
                }",
                OperationName = "Login",
                Variables = new
                {
                    email = email,
                    password = password
                }
            };
            return request;
        }

        public GraphQLRequest putDraftGraphQL(string title, string summary, string content, string[] tags)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                mutation PutDraft($title:String!, $summary: String!, $content:String!,$tags:[String]){
                    putDraft(input:{title:$title,summary:$summary,content:$content,tags:$tags}){
                        id
                    }
                }",
                OperationName = "PutDraft",
                Variables = new
                {
                    title = title,
                    summary = summary,
                    content = content,
                    tags = tags
                }
            };
            return request;
        }//

        public GraphQLRequest getMyFollwers(string username, string after)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
               query GetMyFollowers($username:String!,$after:String!){
                  user(input : {userName : $username}){
  	                followers(input :{after :$after}){
                      pageInfo{
                        hasNextPage
                        endCursor
                      }
                      edges{
                        node{
                          id
                          displayName
                          userName
                        }
                      }
                    }
                  }
                }",
                OperationName = "GetMyFollowers",
                Variables = new
                {
                    username = username,
                    after = after
                }
            };
            return request;
        }
        public GraphQLRequest getNeededAppreciateArticleId(string username, string after)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                query GetUserAericle($username:String!, $cursor:String!){
                  user(input:{userName : $username}){
                    articles(input:{after : $cursor}){
                      pageInfo{
                        endCursor
                        hasNextPage
                      }
                      edges{
                        node{
                          id
                          title
                          hasAppreciate#是否已拍手
          
                        }
                      }
                    }
	              }
                }",
                OperationName = "GetUserAericle",
                Variables = new
                {
                    username = username,
                    cursor = after
                }
            };
            return request;
        }
        public GraphQLRequest appreciateArticle(string id)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                 mutation AppreciateArticle($id:ID!, $amount:PositiveInt!){
                   appreciateArticle(input:{id: $id, amount:$amount}){
                     id
                     title
                   }
                 }",
                OperationName = "AppreciateArticle",
                Variables = new
                {
                    id = id,
                    amount = 5
                }
            };
            return request;
        }//

        public GraphQLRequest publishArticle(string id)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                mutation PublishAreticle($id:ID!){
                  publishArticle(input:{id:$id}){
                    id
                    publishState
                    __typename
                  }
                }",
                OperationName = "PublishAreticle",
                Variables = new
                {
                    id = id
                }
            };
            return request;
        }//

        public GraphQLRequest getImageURL(string url)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                mutation GetMatterImageId($url:URL){
                  singleFileUpload(input:{type:embed, entityType:user,url:$url}){
                    id
                    path
                  }
                }",
                OperationName = "GetMatterImageId",
                Variables = new
                {
                    url = url
                }
            };
            return request;
        }//
    }
}
