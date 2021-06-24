using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MattersRobot.Utils;
using System;
using System.Threading.Tasks;

namespace MattersAutoClap._Module.Entity
{
    public class Login
    {

        public static async Task<String> login(GraphQLRequest req)
        {

            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<LoginInfo>(request);
            string token;
            if (response.Data != null)
            {
                token = response.Data.userLogin.token;
            }
            else
            {
                token = "";
            }
            return token;
        }
        public class LoginInfo
        {
            public UserLogin userLogin { get; set; }
            public class UserLogin
            {
                public string token { get; set; }
            }
        }
    }
}
