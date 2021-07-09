using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MattersRobot.Utils;
using System;
using System.Threading.Tasks;
using static MattersRobot._Module.Entitly.GetMattersImage.GetImage;

namespace MattersRobot._Module.Entitly
{
    class GetMattersImage
    {
        public static async Task<SingleFileUpload> getArticleId(GraphQLRequest req, string token)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<GetImage>(request);
            return response.Data.singleFileUpload;
        }


        public class GetImage
        {
            public SingleFileUpload singleFileUpload { get; set; }

            public class SingleFileUpload
            {
                public string id { get; set; }
                public string path { get; set; }
            }
        }
       

        
    }
}
