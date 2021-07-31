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

        public const string countries = "US,TW,JP,HK,CN,MY,SG,IN";
        public const string currencyCode = "USD,TWD,JPY,HKD,CNY,MYR,SGD,INR";
        public string[] countriesLable = new string[] { "美國", "台灣",  "日本", "香港", "中國", "馬來西亞", "新加坡", "印度" };
        public string[] currencyCountriesLable = new string[] { "美元", "台幣",  "日元", "港幣", "人民幣", "馬來西亞林吉特", "新加坡幣", "印度盧比" };
        public string[] currencyCodeArray = currencyCode.Split(',');
        private const string currencyAccessKey = "403e25f74871138acc22876a32d5903a";
        public const string CoinKey = "53bc319d-7249-49da-b89f-672730481632";
        public const string GetCurrency = "https://tw.rter.info/capi.php";
        public string currencyBase = $"https://data.fixer.io/api/latest?access_key={currencyAccessKey}" +
            "&base={0}" +
            $"&symbols={currencyCode}";
        
        public const string GetPriceConversion = "https://pro-api.coinmarketcap.com/v1/tools/price-conversion";//價格換算
        public const string GetQuotes = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";//價格換算
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
        public const string CurrencyCover = "http://103.246.218.136/MyImage/cryptocurrency.jpg";
        public const string CovidCover = "http://103.246.218.136/MyImage/covid19.jpg";
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
            string s = @"mutation Login{userLogin(input:{email:" + '"' + email + '"' + ",password:" + '"' + password + '"' + "}){token}}";
            GraphQLRequest request = new GraphQLRequest
            {
                Query = s,
                OperationName = "Login",
            };
            return request;
        }

        public GraphQLRequest putDraftGraphQL(string coverID,string title, string summary, string content, string[] tags)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                mutation PutDraft($cover:ID!,$title:String!, $summary: String!, $content:String!,$tags:[String]){
                    putDraft(input:{cover:$cover ,title:$title,summary:$summary,content:$content,tags:$tags}){
                        id
                    }
                }",
                OperationName = "PutDraft",
                Variables = new
                {
                    cover = coverID,
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
                 mutation AppreciateArticle($id:ID!){
                   appreciateArticle(input:{id: $id, amount:5}){
                     id
                     title
                   }
                 }",
                OperationName = "AppreciateArticle",
                Variables = new
                {
                    id = id
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
                mutation GetMatterImageId{
                  singleFileUpload(input:{type:embed, entityType:user,url:"+'"'+url+'"'+"}){" +
                  "id " +
                  "path}}",
                OperationName = "GetMatterImageId",
                /*Variables = new
                {
                    url = url
                }*/
            };
            return request;
        }//
        public GraphQLRequest getPayToLink(string amount, string recipientId, string targetId)
        {
            //recipientId 作者ID
            //targetId 文章ID
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                mutation Pay($amount:PositiveFloat!,$recipientId:ID!,$targetId:ID!){
                  payTo (input:{amount:$amount,currency:LIKE, purpose:donation,recipientId:$recipientId,targetId:$targetId}){
                    redirectUrl
                  }
                }",
                OperationName = "Pay",
                Variables = new
                {
                    amount = amount,
                    recipientId = recipientId,
                    targetId = targetId
                }
            };
            return request;
        }//
        public GraphQLRequest getAllArticleHash(string account, string artCursor) { 
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                query AllArticle($account: String!,$artCursor:String) {
                  user(input: { userName: $account }) {
                    articles(input: { after: $artCursor }) {
                      pageInfo {
                        startCursor
                        hasNextPage
                        endCursor
                        hasPreviousPage
                      }
                      edges {
                        node {
                          title
                          id
                          mediaHash
                          createdAt
                          state
                        }
                      }
                    }
                  }
                }
                ",
                OperationName = "AllArticle",
                Variables = new
                {
                    account = account,
                    artCursor = artCursor
                }
            };
            return request;
        }//
        public GraphQLRequest getArticleInfo(string mediaHash, string appCursor)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                 query ArticleInfo($mediaHash: String!, $appCursor: String!) {
                  article(input: { mediaHash: $mediaHash }) {
                    title
                    id
                    createdAt
                    #拍手數
                    appreciationsReceivedTotal
                    #給我拍手的人
                    appreciationsReceived(input: { after: $appCursor }) {
                      pageInfo {
                        startCursor
                        hasNextPage
                        endCursor
                        hasPreviousPage
                      }
                      edges {
                        node {
                          sender {
                            id
                            displayName
                            userName
                          }
                          amount
                        }
                      }
                    }
                  }
                }
                ",
                OperationName = "ArticleInfo",
                Variables = new
                {
                   mediaHash = mediaHash,
                   appCursor = appCursor
                }
            };
            return request;
        }//
    }
}
