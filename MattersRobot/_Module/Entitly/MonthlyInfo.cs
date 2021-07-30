using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MattersRobot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly
{
    class MonthlyInfo
    {
        public static async Task<GraphQLResponse<AllArticle>> GetAllArticleHash(GraphQLRequest req, string token)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<AllArticle>(request);
            return response;
        }

        public static async Task<GraphQLResponse<ArticleInfo>> GetArticleInfo(GraphQLRequest req, string token)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<ArticleInfo>(request);
            return response;
        }
        
        public class ArticleInfo
        {
            public Article article { get; set; }

            public class PageInfo
            {
                public string startCursor { get; set; }
                public bool hasNextPage { get; set; }
                public string endCursor { get; set; }
                public bool hasPreviousPage { get; set; }
            }

            public class Sender
            {
                public string id { get; set; }
                public string displayName { get; set; }
                public string userName { get; set; }
            }

            public class Node
            {
                public Sender sender { get; set; }
                public int amount { get; set; }
            }

            public class Edge
            {
                public Node node { get; set; }
            }

            public class AppreciationsReceived
            {
                public PageInfo pageInfo { get; set; }
                public List<Edge> edges { get; set; }
            }

            public class Article
            {
                public string title { get; set; }
                public string id { get; set; }
                public DateTime createdAt { get; set; }
                public int appreciationsReceivedTotal { get; set; }
                public AppreciationsReceived appreciationsReceived { get; set; }
            }
        }



        public class AllArticle
        {
            public User user { get; set; }
            public class Liker
            {
                public string likerId { get; set; }
                public bool civicLiker { get; set; }
                public double total { get; set; }
            }

            public class PageInfo
            {
                public string startCursor { get; set; }
                public bool hasNextPage { get; set; }
                public string endCursor { get; set; }
                public bool hasPreviousPage { get; set; }
            }

            public class Node
            {
                public string title { get; set; }
                public string id { get; set; }
                public string mediaHash { get; set; }
                public DateTime createdAt { get; set; }
                public string state { get; set; }
            }

            public class Edge
            {
                public Node node { get; set; }
            }

            public class Articles
            {
                public PageInfo pageInfo { get; set; }
                public List<Edge> edges { get; set; }
            }

            public class User
            {
                public string id { get; set; }
                public string displayName { get; set; }
                public string likerId { get; set; }
                public Liker liker { get; set; }
                public Articles articles { get; set; }
            }
        }


    }
}
