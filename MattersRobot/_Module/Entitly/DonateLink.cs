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
    public class DonateLink
    {
        public static async Task<GraphQLResponse<DonateLink>> getDonateLink(GraphQLRequest req, string token)
        {
            try
            {
                GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
                client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
                GraphQLRequest request = req;
                var response = await client.SendQueryAsync<DonateLink>(request);
                return response;
            }
            catch (Exception e)
            {
                APIs.WriteToFile($"斗內過程出錯，原因: {e.Message}");
                return null;
            }

        }
        public PayTo payTo { get; set; }
        public class PayTo
        {
            public string redirectUrl { get; set; }
        }
    }
}
