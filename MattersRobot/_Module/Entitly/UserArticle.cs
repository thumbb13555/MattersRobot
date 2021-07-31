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
    class UserArticle
    {
        public static async Task<GraphQLResponse<UserArticle>> getArticleId(GraphQLRequest req, string token)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<UserArticle>(request);
            return response;
        }

        public User user { get; set; }

        public class PageInfo
        {
            public string endCursor { get; set; }
            public bool hasNextPage { get; set; }
        }

        public class Node
        {
            public string id { get; set; }
            public string title { get; set; }
            public bool hasAppreciate { get; set; }
            public int appreciateLeft { get; set; }
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
            public Articles articles { get; set; }
        }
    }
}
