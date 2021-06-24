using MattersAutoClap._Module.Entity;
using MattersRobot._Module.Action;
using MattersRobot._Module.Entitly;
using MattersRobot._Module.WriteArticle;
using MattersRobot.Utils;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MattersRobot
{
    public partial class MainService : APIs, CurrencyRespond, COVIDRespond
    {
        
        Timer timer = new Timer();
        public MainService()
        {
            InitializeComponent();
        }
        private int reportCount = 0;

        protected override async void OnStart(string[] args)
        {
            WriteToFile("開機時間: " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 1000;  
            timer.Enabled = true;
            
        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private async void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            int now = Int32.Parse(DateTime.Now.ToString("HHmmss"));
            if(now == morning)
            {
                WriteToFile("Publish covid-19 info article");
                string token = await login();
                new WriteCovidInfo(token, this);
            }
            else if(now == noon)
            {
                WriteToFile("Appreciate article");
                string token = await login();
                new AppreciateFollowers(UserName, token);
            }
            else if(now == evening)
            {
                WriteToFile("Publish currency article");
                string token = await login();
                new WriteCurrency(token, this);
            }
            else if(now == night)
            {
                WriteToFile("Appreciate article");
                string token = await login();
                new AppreciateFollowers(UserName, token);
            }

            reportCount++;
            if(reportCount == 3600)
            {
                reportCount = 0;
                Console.WriteLine("定時回報: 於" + DateTime.Now + "服務器運作正常");
                WriteToFile("定時回報: 於" + DateTime.Now+"服務器運作正常");
            }
        }

        /**Login*/
        private async Task<String> login()
        {
            /**Login*/
            WriteToFile("Send API: " + baseAPI);
            string token = await Login.login(loginGraphQL(Account, Password));
            Console.WriteLine("Login success.");
            Console.WriteLine("");
            WriteToFile("Login success.");
            WriteToFile("Token: " + token);
            if (token.Length == 0)
            {
                WriteToFile("Login error: ");
                return "";
            }
            return token;
        }        
        /**Currency Respond*/
        public void currencyRes(string title, string summary, StringBuilder content, string[] tags, string token)
        {
            pushArticle(title,summary,content,tags,token);
            string finishInfo = "匯率發文於" + DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss") + "已完成";
            Console.WriteLine(finishInfo);
            WriteToFile(finishInfo);
        }

        public void resCOVIDInfo(string title, string summary, StringBuilder content, string[] tags, string token)
        {
            pushArticle(title, summary, content,tags, token);
            string finishInfo = "Covid發文於" + DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss") + "已完成";
            Console.WriteLine(finishInfo);
            WriteToFile(finishInfo);
        }
        private async void pushArticle(string title, string summary, StringBuilder content,string[] tags, string token)
        {
            /**Push draft*/
            string draftId = await PushDraft.getDraftId(putDraftGraphQL(title, summary, content.ToString(),tags), token);
            Console.WriteLine("Draft id: " + draftId);
            WriteToFile("Draft ID: " + draftId);

            /**Publish Article*/
            /*string articleId = await PublishArticle.getArticleId(publishArticle(draftId),token);
            Console.WriteLine("Publish article success. The article id : " + articleId);
            WriteToFile("Publish article success. The article id : " + articleId);*/
        }
    }
}
