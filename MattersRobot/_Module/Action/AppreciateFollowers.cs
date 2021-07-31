using MattersRobot._Module.Entitly;
using MattersRobot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MattersRobot._Module.Action
{
    public class AppreciateFollowers : APIs
    {
        string token;
        string username;
        public AppreciateFollowers(string username, string token)
        {
            this.username = username;
            this.token = token;
            init();
        }

        private async void init()
        {
            List<string> nameList = await getFollwers();
            WriteToFile($"追蹤我的人共: {nameList.Count}位");
            List<string> articleId = await getArticleId(nameList);
            bool isFinish = await appreciateFollowers(articleId);
            if (isFinish)
            {
                string finishInfo = "拍手程序於"+DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss")+"已完成\n  ";
                WriteToFile(finishInfo);
            }

        }
        /**Catch all of followers*/
        private async Task<List<string>> getFollwers()
        {
            List<string> follwersName = new List<string>();
            string cursor = "";
            while (true)
            {
                var info = await FollowersInfo.getFollowers(getMyFollwers(username, cursor), token);
                cursor = info.Data.user.followers.pageInfo.endCursor;
                bool hasNext = info.Data.user.followers.pageInfo.hasNextPage;
                List<FollowersInfo.Edge> name = info.Data.user.followers.edges;
                for (int i = 0; i < name.Count; i++)
                {
                    Console.WriteLine(name[i].node.userName);
                    follwersName.Add(name[i].node.userName);
                }
                if (!hasNext) break;
                Thread.Sleep(500);
            }
            return follwersName;
        }
        private async Task<List<string>> getArticleId(List<string> nameList)
        {
            List<string> articleId = new List<string>();


            for (int i = 0; i < nameList.Count; i++)
            {
                string cursor = "";
                Console.WriteLine("搜尋: " + nameList[i]);
                while (true)
                {
                    var info = await UserArticle.getArticleId(getNeededAppreciateArticleId(nameList[i], cursor), token);
                    bool hasNext = info.Data.user.articles.pageInfo.hasNextPage;
                    cursor = info.Data.user.articles.pageInfo.endCursor;
                    var edges = info.Data.user.articles.edges;
                    if (edges.Count == 0)
                    {
                        Console.WriteLine("此人沒有寫任何文章...");
                        break;
                    }

                    for (int j = 0; j < edges.Count; j++)
                    {
                        bool isAppreciate = edges[j].node.hasAppreciate;
                        if (!isAppreciate)
                        {
                            articleId.Add(edges[j].node.id);
                            Console.WriteLine($"即將拍手: {nameList[i]}的 " + edges[j].node.title);
                            break;
                        }
                    }
                    if (!hasNext) break;
                }
            }
            return articleId;
        }

        private async Task<bool> appreciateFollowers(List<string> articleId)
        {
            for (int i = 0; i < articleId.Count; i++)
            {
                string item = articleId[i];
                var info = await AppreciateInfo.appreciateAerticle(appreciateArticle(item), token);
                if (info.Data == null)
                {
                    Console.WriteLine("出現無效拍手");
                }
                else
                {
                    Console.WriteLine("已拍手: " + info.Data.appreciateArticle.title);
                }
                Thread.Sleep(2000);
            }
            return true;
        }
    }
}
