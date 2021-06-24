using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MattersRobot.Utils;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly
{
    class FollowersInfo
    {
        public static async Task<GraphQLResponse<FollowersInfo>> getFollowers(GraphQLRequest req,string token)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(APIs.baseAPI, new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("x-access-token", token);
            GraphQLRequest request = req;
            var response = await client.SendQueryAsync<FollowersInfo>(request);
            return response;
        }
        public User user { get; set; }
        public class PageInfo
        {
            public bool hasNextPage { get; set; }
            public string endCursor { get; set; }
        }

        public class Node
        {
            public string id { get; set; }
            public string displayName { get; set; }
            public string userName { get; set; }
        }

        public class Edge
        {
            public Node node { get; set; }
        }

        public class Followers
        {
            public PageInfo pageInfo { get; set; }
            public List<Edge> edges { get; set; }
        }

        public class User
        {
            public Followers followers { get; set; }
        }
    }
}
