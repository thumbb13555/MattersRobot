using MattersRobot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MattersRobot._Module.Entitly;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using System.Threading;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;

namespace MattersRobot._Module.Action
{
    class Donate : APIs
    {
        private string token;
        private string amount;
        private string recipientId;//接收者
        private string targetId;//接收文章

        public Donate(string token)
        {
            this.token = token;
            this.amount = "1";
            this.recipientId = "VXNlcjo1NzI3NA";
            this.targetId = "QXJ0aWNsZToxNTQ1Nzc";
            WriteToFile("測試中");
            getDonateLink();
        }
        public Donate(string token, string displayName, string amount,string recipientId, string targetId)
        {
            this.token = token;
            this.amount = amount;
            this.recipientId = recipientId;
            this.targetId = targetId;
            WriteToFile("斗內程序: 斗內給"+displayName);
            getDonateLink();
        }
        private async void getDonateLink()
        {
            var link = await DonateLink.getDonateLink(getPayToLink(amount,recipientId,targetId), token);
            runWeb(link.Data.payTo.redirectUrl);
        }

        private void runWeb(string link)
        {
            WriteToFile("取得斗內連結: " + link);
            string driverPosition = Application.StartupPath+ "\\driver";
            Thread.Sleep(2000);
            EdgeOptions edgeOptions = new EdgeOptions();
            //edgeOptions.addArguments("user-data-dir=C:\\Users\\edge2automation\\AppData\\Local\\Microsoft\\Edge\\User Data");
            IWebDriver driver = new EdgeDriver(driverPosition,edgeOptions);

            driver.Navigate().GoToUrl(link);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30000);
            IWebElement button = driver.FindElement(By.ClassName("likepay-block-button"));
            button.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30000);
            //遍歷所有iframe
            List<IWebElement> frames = new List<IWebElement>(driver.FindElements(By.TagName("iframe")));
            Console.WriteLine("Number of Frames: " + frames.Count);
            for (int i = 0; i < frames.Count; i++)
            {
                Console.WriteLine("frame[" + i + "]: " + frames[i].GetAttribute("id").ToString());
            }
            //視角切換到iframe
            driver.SwitchTo().Frame(0);
            //遍歷iframe內所有物件
            List<IWebElement> divClass = new List<IWebElement>(driver.FindElements(By.TagName("div")));
            Console.WriteLine("Number of iFrame Class: " + divClass.Count);
            for (int i = 0; i < divClass.Count; i++)
            {
                Console.WriteLine("iFrame[" + i + "]: " + divClass[i].GetAttribute("class").ToString());
            }

            var input = driver.FindElement(By.XPath("//*[@class='bsq-form-field']/div/input"));
            Console.WriteLine(input.GetAttribute("class"));
            input.SendKeys("102402137@stu.ctu.edu.tw");

            var loginButton = driver.FindElement(By.XPath("//*[@class='row mb-4']/div/button"));
            Console.WriteLine(loginButton.GetAttribute("class"));
            loginButton.Click();

            Thread.Sleep(3000);
            List<IWebElement> div2Class = new List<IWebElement>(driver.FindElements(By.TagName("div")));
            Console.WriteLine("Number of iFrame2 Class: " + div2Class.Count);
            for (int i = 0; i < div2Class.Count; i++)
            {
                Console.WriteLine("iFrame2[" + i + "]: " + div2Class[i].GetAttribute("class").ToString());
            }

            var input2 = driver.FindElement(By.XPath("//*[@class='col']/div/div/input"));
            Console.WriteLine(input2.GetAttribute("class"));
            input.SendKeys("tuba21031");




            Console.WriteLine("結束執行");
            //driver.Quit();
        }
        private void findMattersLoginButton(IWebDriver driver)
        {
            var socialElement = driver.FindElement(By.Id("social-row"));
            Console.WriteLine("定位: class=" + socialElement.GetAttribute("class"));
            var w = socialElement.FindElement(By.TagName("ul"));
            Console.WriteLine("定位: w class=" + w.GetAttribute("class"));
            var x = w.FindElements(By.TagName("li"));
            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("li[" + i + "]: " + x[i].GetAttribute("class").ToString());
            }
            var a = x[4].FindElements(By.TagName("a"));
            for (int i = 0; i < a.Count; i++)
            {
                Console.WriteLine("a[" + i + "]: " + a[i].GetAttribute("class").ToString());
            }
            var ii = a[0].FindElements(By.TagName("i"));
            for (int i = 0; i < ii.Count; i++)
            {
                Console.WriteLine("i[" + i + "]: " + ii[i].GetAttribute("class").ToString());
            }


            var matters = ii[0];

            matters.Click();
        }

    }
}
