using MattersRobot._Module.Entitly.CoinCollect;
using MattersRobot._Module.Entitly;
using MattersRobot.Utils;
using Nancy.Helpers;
using Newtonsoft.Json;
using System;

using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MattersRobot._Module.WriteArticle
{
    class WriteMonthlyReport : APIs
    {
        string token;
        MonthlyReportRespond respond;
        public WriteMonthlyReport(string token, MonthlyReportRespond respond)
        {
            this.token = token;
            this.respond = respond;
            //write();
            int[] array = { 6, 9, 2, 1, 5, 1, 8, 23, 42, 65, 1, 4, 7, 2, 5 };
            RankEntity.Sort(array);
        }
        private async void write()
        {
            int monthTotalappreciationsReceived = 0;
            var hashList = await GetAllHash();
            List<RankEntity> rankList = new List<RankEntity>();
            List<string> str = new List<string>();
            for (int i = 0; i < hashList.Count; i++)
            {
                var info = await MonthlyInfo.GetArticleInfo(getArticleInfo(hashList[i].mediaHash, ""), token);
                WriteToFile($"解析文章: {info.Data.article.title}, 收到拍手: {info.Data.article.appreciationsReceivedTotal}");
                string cursor = "";
                
                while (true)
                {
                    var give = await MonthlyInfo.GetArticleInfo(getArticleInfo(hashList[i].mediaHash, cursor), token);
                    var receiveItem = give.Data.article.appreciationsReceived;
                    var edges =  receiveItem.edges;
                    for(int x = 0;x< edges.Count; x++)
                    {
                        string id =  receiveItem.edges[x].node.sender.id;
                        string name =  receiveItem.edges[x].node.sender.displayName;
                        int amount = receiveItem.edges[x].node.amount;
                        if (!str.Contains(id))
                        {
                            str.Add(id);
                            rankList.Add(new RankEntity(id,name,amount));
                        }
                        else
                        {
                            rankList[RankEntity.findIndex(rankList, id)].amount += amount;
                        }
                    }
                    cursor = receiveItem.pageInfo.endCursor;
                    if (!give.Data.article.appreciationsReceived.pageInfo.hasNextPage)
                    {
                        break;
                    }
                }
                monthTotalappreciationsReceived += info.Data.article.appreciationsReceivedTotal;
            }
            Console.WriteLine($"本月共收到了: {monthTotalappreciationsReceived}個拍手");
            Console.WriteLine($"拍手人數共{rankList.Count}人");
            Console.WriteLine("---未排序---");
            foreach (RankEntity rank in rankList)
            {
                Console.WriteLine(rank.displayname + "拍了" + rank.amount + "下");
            }
            Console.WriteLine("---已排序---");
            /*rankList = RankEntity.sortRank(rankList);
            foreach (RankEntity rank in rankList)
            {
                Console.WriteLine(rank.displayname + "拍了" + rank.amount + "下");
            }*/
           
        }

        private async Task<List<MonthlyInfo.AllArticle.Node>> GetAllHash()
        {
            List<MonthlyInfo.AllArticle.Node> list = new List<MonthlyInfo.AllArticle.Node>();
            string cursor = "";
            DateTime now = DateTime.Now;
            while (true)
            {
                var info = await MonthlyInfo.GetAllArticleHash(getAllArticleHash(UserName, cursor), token);
                var item = info.Data.user.articles;
                cursor = item.pageInfo.endCursor;
                for (int i = 0; i < item.edges.Count; i++)
                {
                    DateTime createTime = info.Data.user.articles.edges[i].node.createdAt.AddHours(8);
                    //bool isOnMonth = (((now.Year - createTime.Year) * 12) + now.Month - createTime.Month) == 0;
                    bool isOnMonth =  (now - createTime).TotalDays <= 2;

                    if (item.edges[i].node.state == "active" && isOnMonth)
                    {
                        list.Add(info.Data.user.articles.edges[i].node);
                    }

                }
                if (!info.Data.user.articles.pageInfo.hasNextPage)
                {
                    break;
                }
            }

            return list;
        }

    }

    public interface MonthlyReportRespond
    {
        void monthlyReportRespond(string coverID, string title, string summary, StringBuilder content, string[] tags, string token);
    }
}
