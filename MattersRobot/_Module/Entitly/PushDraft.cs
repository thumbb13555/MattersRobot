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

    public class PushDraft
    {
        public static async Task<String> getDraftId(GraphQLRequest req,string token)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("x-access-token",token);
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<Draft>(request);
            
            return response.Data.putDraft.id;
        }
        public class Draft
        {
            public PutDraft putDraft { get; set; }
            public class PutDraft
            {
                public string id { get; set; }
            }
        }
    }
}
