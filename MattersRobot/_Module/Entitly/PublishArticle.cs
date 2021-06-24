using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MattersRobot.Utils;
using System;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly
{
    class PublishArticle
    {
        public static async Task<String> getArticleId(GraphQLRequest req, string token)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<Publish>(request);
            return response.Data.publishArticle.id;
        }
        public class Publish
        {
            public Push publishArticle { get; set; }
            public class Push
            {
                public string id { get; set; }
                public string publishState { get; set; }
                public string __typename { get; set; }
            }
        }
    }
}
