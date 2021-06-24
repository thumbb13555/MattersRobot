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
    class AppreciateInfo
    {
        public static async Task<GraphQLResponse<AppreciateInfo>> appreciateAerticle (GraphQLRequest req, string token)
        {
            try
            {
                GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
                client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
                GraphQLRequest request = req;
                var response = await client.SendQueryAsync<AppreciateInfo>(request);
                return response;
            }
            catch(Exception e)
            {
                return null;
            }
           
        }

        public AppreciateArticle appreciateArticle { get; set; }

        public class AppreciateArticle
        {
            public string id { get; set; }
            public string title { get; set; }
            public int appreciateLeft { get; set; }
            public int appreciationsReceivedTotal { get; set; }
            public bool hasAppreciate { get; set; }
        }

    }
}
