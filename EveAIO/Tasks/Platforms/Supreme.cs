namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    internal class Supreme : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;
        private string _id;
        private string _style;
        private List<KeyValuePair<string, Product>> _childProducts;
        private bool _isFallbackSearch;
        private Product _fallbackProduct;
        private KeyValuePair<string, string>? _fallbackPickedSize;
        [Dynamic]
        private object _stockJson;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        [Dynamic]
        private object _dynObj;
        private string _data;
        private List<SuccessObject> CHILD_SUCCESSES;

        public Supreme(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this.CHILD_SUCCESSES = new List<SuccessObject>();
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            if (this._task.SupremeAutomation == TaskObject.SuprimeAutomationEnum.browserless)
            {
                return this.AtcBrowserless();
            }
            return this.AtcBrowser();
        }

        public bool AtcBrowser()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                if (!this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"))))
                {
                    if (this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                    {
                        new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='s']"))).SelectByValue(this._runner.PickedSize.Value.Value);
                    }
                }
                else
                {
                    new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='size']"))).SelectByValue(this._runner.PickedSize.Value.Value);
                }
                this._task.Driver.FindElementByName("commit").Click();
                new WebDriverWait(this._task.Driver, TimeSpan.FromSeconds(5.0)).Until<IWebElement>(ExpectedConditions.ElementExists(By.ClassName("has-cart")));
                if (this._task.Driver.PageSource.Contains("class=\"has-cart\""))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", "");
                return false;
            }
        }

        public bool AtcBrowserless()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                this._request = (HttpWebRequest) WebRequest.Create($"http://www.supremenewyork.com/shop/{this._id}/add.json");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Mobile Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Accept = "application/json";
                this._request.KeepAlive = true;
                this._request.Headers.Add("Origin", "http://www.supremenewyork.com");
                this._request.Method = "POST";
                this._request.Referer = "http://www.supremenewyork.com/mobile";
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                string s = (("size=" + this._runner.PickedSize.Value.Value) + "&style=" + this._style) + "&qty=1";
                this._bytes = Encoding.ASCII.GetBytes(s);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                }
                if (this._srr.Contains("in_stock\":true"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", "");
                return false;
            }
        }

        public bool AtcMulti(TaskObject task, Product product)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, product.SuprimetMultiPickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                string str = "";
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create($"http://www.supremenewyork.com/shop/{product.SuprimeMultiId}/add.json");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Accept = "application/json";
                request.KeepAlive = true;
                request.Method = "POST";
                request.Headers.Add("Origin", "http://www.supremenewyork.com");
                request.Referer = "http://www.supremenewyork.com/mobile";
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.ContentType = "application/x-www-form-urlencoded";
                string s = (("size=" + product.SuprimetMultiPickedSize.Value.Value) + "&style=" + product.SuprimetMultiStyle) + "&qty=1";
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                str = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                if (str.Contains("in_stock\":true"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", "");
                return false;
            }
        }

        public bool Checkout()
        {
            if (this._task.SupremeAutomation == TaskObject.SuprimeAutomationEnum.browserless)
            {
                return this.CheckoutBrowserless();
            }
            return this.CheckoutBrowser();
        }

        public bool CheckoutBrowser()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_OUT, null, "", "");
                this._task.Driver.Navigate().GoToUrl("https://www.supremenewyork.com/checkout");
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                MethodInfo method = type.GetMethod("GetCountries");
                object obj2 = Activator.CreateInstance(type);
                List<KeyValuePair<string, string>> list1 = (List<KeyValuePair<string, string>>) method.Invoke(obj2, null);
                ProfileObject profile = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                string str = "";
                string cCNumber = profile.CCNumber;
                while (cCNumber.Length > 4)
                {
                    str = str + cCNumber.Substring(0, 4);
                    cCNumber = cCNumber.Remove(0, 4);
                    str = str + " ";
                }
                str = str + cCNumber;
                string str3 = "";
                switch (profile.CardTypeId)
                {
                    case "0":
                        str3 = "american_express";
                        break;

                    case "1":
                        str3 = "visa";
                        break;

                    case "2":
                        str3 = "master";
                        break;
                }
                if ((profile.CountryId == "JP") && (this._task.Payment == TaskObject.PaymentEnum.bankTransfer))
                {
                    str3 = "cod";
                }
                IJavaScriptExecutor driver = this._task.Driver;
                if (this._task.SupremeRegion != TaskObject.SupremeRegionEnum.EU)
                {
                    if (this._task.SupremeRegion != TaskObject.SupremeRegionEnum.USA)
                    {
                        HtmlDocument document4 = new HtmlDocument();
                        document4.LoadHtml(this._task.Driver.PageSource);
                        string nameToFind = "";
                        if (document4.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number")))
                        {
                            nameToFind = document4.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"))).Attributes["name"].Value;
                        }
                        else
                        {
                            string ccId = document4.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"))).Attributes["for"].Value;
                            nameToFind = document4.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == ccId))).Attributes["name"].Value;
                        }
                        string str11 = "";
                        if (!document4.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"))))
                        {
                            string cvvId = document4.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"))).Attributes["for"].Value;
                            str11 = document4.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == cvvId))).Attributes["name"].Value;
                        }
                        else
                        {
                            str11 = document4.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"))).Attributes["name"].Value;
                        }
                        IWebElement element19 = this._task.Driver.FindElement(By.Name("credit_card[first_name]"));
                        object[] args = new object[] { element19 };
                        driver.ExecuteScript("arguments[0].value='" + profile.FirstName + "';", args);
                        IWebElement element20 = this._task.Driver.FindElement(By.Name("credit_card[last_name]"));
                        object[] objArray2 = new object[] { element20 };
                        driver.ExecuteScript("arguments[0].value='" + profile.LastName + "';", objArray2);
                        IWebElement element21 = this._task.Driver.FindElement(By.Name("order[email]"));
                        object[] objArray3 = new object[] { element21 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Email + "';", objArray3);
                        IWebElement element22 = this._task.Driver.FindElement(By.Name("order[tel]"));
                        object[] objArray4 = new object[] { element22 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Phone + "';", objArray4);
                        IWebElement element23 = this._task.Driver.FindElement(By.Name("order[billing_address]"));
                        object[] objArray5 = new object[] { element23 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Address1 + "';", objArray5);
                        IWebElement element24 = this._task.Driver.FindElement(By.Name("order[billing_city]"));
                        object[] objArray6 = new object[] { element24 };
                        driver.ExecuteScript("arguments[0].value='" + profile.City + "';", objArray6);
                        IWebElement element25 = this._task.Driver.FindElement(By.Name("order[billing_zip]"));
                        object[] objArray7 = new object[] { element25 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Zip + "';", objArray7);
                        new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='order_billing_state']"))).SelectByValue(GetJapanStates().First<KeyValuePair<string, string>>(x => (x.Value == profile.StateId)).Key);
                        if (this._task.Payment == TaskObject.PaymentEnum.creditcard)
                        {
                            IWebElement element1 = this._task.Driver.FindElement(By.Name(nameToFind));
                            element1.Clear();
                            element1.Click();
                            Clipboard.SetText(profile.CCNumber);
                            element1.SendKeys(Keys.Control + "v");
                            new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_month']"))).SelectByValue(profile.ExpiryMonth);
                            new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_year']"))).SelectByValue(profile.ExpiryYear);
                            IWebElement element27 = this._task.Driver.FindElement(By.Name(str11));
                            element27.Clear();
                            element27.SendKeys(profile.Cvv);
                        }
                        new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_type']"))).SelectByValue(str3);
                        IWebElement element26 = this._task.Driver.FindElement(By.XPath("//input[@id='order_terms']"));
                        object[] objArray8 = new object[] { element26 };
                        driver.ExecuteScript("arguments[0].click()", objArray8);
                    }
                    else
                    {
                        HtmlDocument document3 = new HtmlDocument();
                        document3.LoadHtml(this._task.Driver.PageSource);
                        string nameToFind = "";
                        if (document3.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number")))
                        {
                            nameToFind = document3.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"))).Attributes["name"].Value;
                        }
                        else
                        {
                            string text1 = document3.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"))).Attributes["for"].Value;
                            nameToFind = document3.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == text1))).Attributes["name"].Value;
                        }
                        string str8 = "";
                        if (document3.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv")))
                        {
                            str8 = document3.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"))).Attributes["name"].Value;
                        }
                        else
                        {
                            string text2 = document3.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"))).Attributes["for"].Value;
                            str8 = document3.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == text2))).Attributes["name"].Value;
                        }
                        IWebElement element28 = this._task.Driver.FindElement(By.Name(nameToFind));
                        element28.Clear();
                        element28.Click();
                        Clipboard.SetText(profile.CCNumber);
                        element28.SendKeys(Keys.Control + "v");
                        IWebElement element10 = this._task.Driver.FindElement(By.Name("order[billing_name]"));
                        string[] textArray1 = new string[] { "arguments[0].value='", profile.FirstName, " ", profile.LastName, "';" };
                        object[] args = new object[] { element10 };
                        driver.ExecuteScript(string.Concat(textArray1), args);
                        IWebElement element11 = this._task.Driver.FindElement(By.Name("order[email]"));
                        object[] objArray10 = new object[] { element11 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Email + "';", objArray10);
                        IWebElement element12 = this._task.Driver.FindElement(By.Name("order[tel]"));
                        object[] objArray11 = new object[] { element12 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Phone + "';", objArray11);
                        IWebElement element13 = this._task.Driver.FindElement(By.Name("order[billing_address]"));
                        object[] objArray12 = new object[] { element13 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Address1 + "';", objArray12);
                        IWebElement element14 = this._task.Driver.FindElement(By.Name("order[billing_address_2]"));
                        object[] objArray13 = new object[] { element14 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Address2 + "';", objArray13);
                        IWebElement element15 = this._task.Driver.FindElement(By.Name("order[billing_city]"));
                        object[] objArray14 = new object[] { element15 };
                        driver.ExecuteScript("arguments[0].value='" + profile.City + "';", objArray14);
                        IWebElement element16 = this._task.Driver.FindElement(By.Name("order[billing_zip]"));
                        object[] objArray15 = new object[] { element16 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Zip + "';", objArray15);
                        string str9 = "";
                        if (profile.CountryId != "US")
                        {
                            str9 = "CANADA";
                        }
                        else
                        {
                            str9 = "USA";
                        }
                        new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='order_billing_country']"))).SelectByValue(str9);
                        new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_month']"))).SelectByValue(profile.ExpiryMonth);
                        new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_year']"))).SelectByValue(profile.ExpiryYear);
                        new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='order_billing_state']"))).SelectByValue(profile.StateId);
                        IWebElement element17 = this._task.Driver.FindElement(By.Name(str8));
                        object[] objArray16 = new object[] { element17 };
                        driver.ExecuteScript("arguments[0].value='" + profile.Cvv + "';", objArray16);
                        IWebElement toElement = this._task.Driver.FindElement(By.XPath("//input[@id='order_terms']"));
                        new Actions(this._task.Driver).MoveToElement(toElement).Click().Build().Perform();
                    }
                }
                else
                {
                    HtmlDocument document2 = new HtmlDocument();
                    document2.LoadHtml(this._task.Driver.PageSource);
                    string nameToFind = "";
                    if (document2.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number")))
                    {
                        nameToFind = document2.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"))).Attributes["name"].Value;
                    }
                    else
                    {
                        string text3 = document2.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"))).Attributes["for"].Value;
                        nameToFind = document2.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == text3))).Attributes["name"].Value;
                    }
                    string str6 = "";
                    if (this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv")))
                    {
                        str6 = document2.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"))).Attributes["name"].Value;
                    }
                    else
                    {
                        string text4 = document2.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"))).Attributes["for"].Value;
                        str6 = document2.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == text4))).Attributes["name"].Value;
                    }
                    IWebElement element = this._task.Driver.FindElement(By.Name("order[billing_name]"));
                    string[] textArray2 = new string[] { "arguments[0].value='", profile.FirstName, " ", profile.LastName, "';" };
                    object[] args = new object[] { element };
                    driver.ExecuteScript(string.Concat(textArray2), args);
                    IWebElement element2 = this._task.Driver.FindElement(By.Name("order[email]"));
                    object[] objArray18 = new object[] { element2 };
                    driver.ExecuteScript("arguments[0].value='" + profile.Email + "';", objArray18);
                    IWebElement element3 = this._task.Driver.FindElement(By.Name("order[tel]"));
                    object[] objArray19 = new object[] { element3 };
                    driver.ExecuteScript("arguments[0].value='" + profile.Phone + "';", objArray19);
                    IWebElement element4 = this._task.Driver.FindElement(By.Name("order[billing_address]"));
                    object[] objArray20 = new object[] { element4 };
                    driver.ExecuteScript("arguments[0].value='" + profile.Address1 + "';", objArray20);
                    IWebElement element5 = this._task.Driver.FindElement(By.Name("order[billing_address_2]"));
                    object[] objArray21 = new object[] { element5 };
                    driver.ExecuteScript("arguments[0].value='" + profile.Address2 + "';", objArray21);
                    IWebElement element6 = this._task.Driver.FindElement(By.Name("order[billing_address_3]"));
                    object[] objArray22 = new object[] { element6 };
                    driver.ExecuteScript("arguments[0].value='" + profile.State + "';", objArray22);
                    IWebElement element7 = this._task.Driver.FindElement(By.Name("order[billing_city]"));
                    object[] objArray23 = new object[] { element7 };
                    driver.ExecuteScript("arguments[0].value='" + profile.City + "';", objArray23);
                    IWebElement element8 = this._task.Driver.FindElement(By.Name("order[billing_zip]"));
                    object[] objArray24 = new object[] { element8 };
                    driver.ExecuteScript("arguments[0].value='" + profile.Zip + "';", objArray24);
                    new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='order_billing_country']"))).SelectByValue(profile.CountryId);
                    IWebElement element29 = this._task.Driver.FindElement(By.Name(nameToFind));
                    element29.Clear();
                    element29.Click();
                    Clipboard.SetText(profile.CCNumber);
                    element29.SendKeys(Keys.Control + "v");
                    new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_month']"))).SelectByValue(profile.ExpiryMonth);
                    new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_year']"))).SelectByValue(profile.ExpiryYear);
                    new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='credit_card_type']"))).SelectByValue(str3);
                    IWebElement element30 = this._task.Driver.FindElement(By.Name(str6));
                    element30.Clear();
                    element30.SendKeys(profile.Cvv);
                    IWebElement toElement = this._task.Driver.FindElement(By.XPath("//input[@id='order_terms']"));
                    object[] objArray25 = new object[] { toElement };
                    driver.ExecuteScript("arguments[0].click()", objArray25);
                    new Actions(this._task.Driver).MoveToElement(toElement).Click().Build().Perform();
                }
                if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                    return false;
                }
                if (this._task.CheckoutDelay > 0)
                {
                    Thread.Sleep(this._task.CheckoutDelay);
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                this._task.Driver.FindElementByName("commit").Click();
                Thread.Sleep(600);
                if (!this._task.Driver.PageSource.Contains("recaptcha challenge") && !this._task.Driver.PageSource.Contains("recaptcha "))
                {
                    Thread.Sleep(0xbb8);
                }
                else
                {
                    Thread.Sleep(0x4e20);
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(this._task.Driver.PageSource);
                if (!document.DocumentNode.Descendants("p").Any<HtmlNode>(x => ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cart-count")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "error"))))
                {
                    if (!document.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "confirmation")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "failed"))))
                    {
                        if (document.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "errors")) && !string.IsNullOrEmpty(x.InnerText.Trim())))
                        {
                            string str14 = document.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "errors")) && !string.IsNullOrEmpty(x.InnerText.Trim()))).InnerText.Trim();
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", str14);
                            return false;
                        }
                        if (document.DocumentNode.InnerText.ToUpperInvariant().Contains("thank you"))
                        {
                            return true;
                        }
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                        return false;
                    }
                    string str13 = document.DocumentNode.Descendants("div").First<HtmlNode>(x => ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "confirmation")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "failed"))).Descendants("p").First<HtmlNode>().InnerText.Trim();
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", str13);
                    return false;
                }
                string msg = document.DocumentNode.Descendants("p").First<HtmlNode>(x => ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cart-count")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "error"))).InnerText.Trim();
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", msg);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
                return false;
            }
        }

        public bool CheckoutBrowserless()
        {
            try
            {
                CSharpArgumentInfo[] infoArray38;
                string str15;
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_OUT, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._request = (HttpWebRequest) WebRequest.Create("https://www.supremenewyork.com/checkout");
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        this._request.CookieContainer = this._runner.Cookies;
                        this._request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Mobile Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                        this._request.Accept = "*";
                        using (WebResponse response = this._request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                this._srr = reader.ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch
                    {
                        flag = true;
                        Thread.Sleep(500);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"))).Attributes["value"].Value;
                bool flag3 = false;
                string str10 = "";
                if (this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "asec")))
                {
                    flag3 = true;
                    if (this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "asec"))).Attributes["value"] != null)
                    {
                        str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "asec"))).Attributes["value"].Value;
                    }
                }
                bool flag2 = false;
                string str8 = "";
                if (this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "hpcvv")))
                {
                    flag2 = true;
                    if (this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "hpcvv"))).Attributes["value"] != null)
                    {
                        str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "hpcvv"))).Attributes["value"].Value;
                    }
                }
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                MethodInfo method = type.GetMethod("GetCountries");
                object obj3 = Activator.CreateInstance(type);
                List<KeyValuePair<string, string>> list1 = (List<KeyValuePair<string, string>>) method.Invoke(obj3, null);
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                string str13 = Uri.EscapeDataString(obj2.Email.Trim());
                string str2 = obj2.FirstName.Trim().Replace(" ", "+");
                string str3 = obj2.LastName.Trim().Replace(" ", "+");
                string str21 = WebUtility.UrlEncode(obj2.Address1.Trim()).Replace(" ", "+");
                string str18 = WebUtility.UrlEncode(obj2.Address2.Trim()).Replace(" ", "+");
                string str19 = obj2.City.Trim().Replace(" ", "+");
                string str12 = WebUtility.UrlEncode(obj2.Zip.Trim()).Replace(" ", "+");
                string state = obj2.StateId;
                string str4 = WebUtility.UrlEncode(obj2.State.Trim()).Replace(" ", "+");
                string str9 = obj2.Phone.Trim().Replace(" ", "+");
                string countryId = obj2.CountryId;
                if (obj2.CountryId == "JP")
                {
                    state = WebUtility.UrlEncode(GetJapanStates().First<KeyValuePair<string, string>>(x => (x.Value == state)).Key);
                }
                if (countryId == "US")
                {
                    countryId = "USA";
                }
                else if (countryId == "CA")
                {
                    countryId = "CANADA";
                }
                string str16 = obj2.CCNumber.Trim().Replace(" ", "");
                obj2.NameOnCard.Trim();
                string expiryMonth = obj2.ExpiryMonth;
                if (expiryMonth.Length == 1)
                {
                    expiryMonth = "0" + expiryMonth;
                }
                string expiryYear = obj2.ExpiryYear;
                string str17 = obj2.Cvv.Trim();
                string cardTypeId = obj2.CardTypeId;
                string str = "";
                string text1 = this._runner.Cookies.List().First<System.Net.Cookie>(x => (x.Name == "pure_cart")).Value;
                if (cardTypeId == "0")
                {
                    str = "american_express";
                }
                else if (cardTypeId == "1")
                {
                    str = "visa";
                }
                else if (cardTypeId == "2")
                {
                    str = "master";
                }
                else if (cardTypeId == "4")
                {
                    str = "jcb";
                }
                if ((obj2.CountryId == "JP") && (this._task.Payment == TaskObject.PaymentEnum.bankTransfer))
                {
                    str = "cod";
                }
                if ((countryId != "USA") && (countryId != "CANADA"))
                {
                    goto Label_06F9;
                }
                if (str9.Length > 10)
                {
                    str9 = str9.Substring(0, 10);
                }
                if (str9.Length < 10)
                {
                    goto Label_06CE;
                }
                goto Label_06D9;
            Label_06BF:
                str9 = str9 + "1";
            Label_06CE:
                if (str9.Length < 10)
                {
                    goto Label_06BF;
                }
            Label_06D9:
                str9 = str9.Insert(3, "-").Insert(7, "-");
            Label_06F9:
                str15 = "";
                if (!this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"))))
                {
                    string ccId = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"))).Attributes["for"].Value;
                    str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == ccId))).Attributes["name"].Value;
                }
                else
                {
                    str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"))).Attributes["name"].Value;
                }
                string str6 = "";
                if (!this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"))))
                {
                    string cvvId = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"))).Attributes["for"].Value;
                    str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == cvvId))).Attributes["name"].Value;
                }
                else
                {
                    str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"))).Attributes["name"].Value;
                }
                this._data = "utf8=%26%23x2713%3B";
                this._data = this._data + "&authenticity_token=" + WebUtility.UrlEncode(str7);
                if (obj2.CountryId != "JP")
                {
                    this._data = this._data + "&order%5Bbilling_name%5D=" + (str2 + " " + str3).Replace(" ", "+");
                }
                else
                {
                    this._data = this._data + "&credit_card%5Blast_name%5D=" + str3;
                    this._data = this._data + "&credit_card%5Bfirst_name%5D=" + str2;
                }
                this._data = this._data + "&order%5Bemail%5D=" + str13;
                this._data = this._data + "&order%5Btel%5D=" + str9;
                this._data = this._data + "&order%5Bbilling_address%5D=" + str21;
                if (obj2.CountryId != "JP")
                {
                    this._data = this._data + "&order%5Bbilling_address_2%5D=" + str18;
                }
                if (((countryId != "USA") && (countryId != "CANADA")) && (obj2.CountryId != "JP"))
                {
                    this._data = this._data + "&order%5Bbilling_address_3%5D=" + str4;
                }
                this._data = this._data + "&order%5Bbilling_city%5D=" + str19;
                if (flag3)
                {
                    this._data = this._data + "&asec=" + WebUtility.UrlEncode(str10);
                }
                if (flag2)
                {
                    this._data = this._data + "&hpcvv=" + WebUtility.UrlEncode(str8);
                }
                switch (countryId)
                {
                    case "USA":
                    case "CANADA":
                    case "JP":
                        this._data = this._data + "&order%5Bbilling_state%5D=" + state;
                        break;
                }
                this._data = this._data + "&order%5Bbilling_zip%5D=" + str12;
                this._data = this._data + "&same_as_billing_address=1";
                this._data = this._data + "&store_credit_id=";
                this._data = this._data + "&store_address=1";
                if ((obj2.CountryId == "JP") && (this._task.Payment == TaskObject.PaymentEnum.bankTransfer))
                {
                    this._data = this._data + "&" + WebUtility.UrlEncode(str15) + "=";
                    this._data = this._data + "&" + WebUtility.UrlEncode(str6) + "=";
                }
                else
                {
                    string[] textArray1 = new string[] { this._data, "&", WebUtility.UrlEncode(str15), "=", str16 };
                    this._data = string.Concat(textArray1);
                    string[] textArray2 = new string[] { this._data, "&", WebUtility.UrlEncode(str6), "=", str17 };
                    this._data = string.Concat(textArray2);
                }
                this._data = this._data + "&order%5Bterms%5D=1";
                this._data = this._data + "&commit=process+payment";
                if (obj2.CountryId != "JP")
                {
                    this._data = this._data + "&order%5Bbilling_country%5D=" + countryId;
                }
                if ((countryId != "USA") && (countryId != "CANADA"))
                {
                    this._data = this._data + "&credit_card%5Btype%5D=" + str;
                }
                if ((obj2.CountryId == "JP") && (this._task.Payment == TaskObject.PaymentEnum.bankTransfer))
                {
                    this._data = this._data + "&credit_card%5Bmonth%5D=01";
                    this._data = this._data + "&credit_card%5Byear%5D=2018";
                }
                else
                {
                    this._data = this._data + "&credit_card%5Bmonth%5D=" + expiryMonth;
                    this._data = this._data + "&credit_card%5Byear%5D=" + expiryYear;
                }
                if (this._task.SolveCaptcha)
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.SUPREME_CAPTCHA_KEY, this._runner.Product.Link.Contains("http") ? this._runner.Product.Link : "https://www.supremenewyork.com", "Supreme");
                    waiter.Start();
                    this._task.Mre.WaitOne();
                    if (Global.CAPTCHA_QUEUE.Any<TaskObject>(x => x.Id == this._task.Id))
                    {
                        solverLocker = Global.SolverLocker;
                        lock (solverLocker)
                        {
                            Global.CAPTCHA_QUEUE.Remove(Global.CAPTCHA_QUEUE.First<TaskObject>(x => x.Id == this._task.Id));
                            this._task.ManualSolved = false;
                        }
                    }
                    this._data = this._data + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token);
                }
                string requestUriString = (string) Global.ASM.GetType("SvcHost.SvcHost").GetMethod("GetSupremeCheckoutLink").Invoke(obj3, null);
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        this._request.CookieContainer = this._runner.Cookies;
                        this._request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Mobile Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        this._request.Accept = "application/json";
                        this._request.KeepAlive = true;
                        this._request.Headers.Add("Origin", "http://www.supremenewyork.com");
                        this._request.ServicePoint.Expect100Continue = true;
                        this._request.Method = "POST";
                        this._request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                        if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                        {
                            this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                            States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                            return false;
                        }
                        if (this._task.CheckoutDelay > 0)
                        {
                            Thread.Sleep(this._task.CheckoutDelay);
                        }
                        this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                        States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                        try
                        {
                            if ((this._runner._watch != null) && this._runner._watch.IsRunning)
                            {
                                this._runner._watch.Stop();
                            }
                        }
                        catch
                        {
                        }
                        this._bytes = Encoding.UTF8.GetBytes(this._data);
                        this._request.ContentLength = this._bytes.Length;
                        using (Stream stream = this._request.GetRequestStream())
                        {
                            stream.Write(this._bytes, 0, this._bytes.Length);
                        }
                        using (WebResponse response2 = this._request.GetResponse())
                        {
                            using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                this._srr = reader2.ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch
                    {
                        Thread.Sleep(500);
                        flag = true;
                        continue;
                    }
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                string str23 = "";
                if (<>o__32.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__3.Target(<>o__32.<>p__3, <>o__32.<>p__2.Target(<>o__32.<>p__2, <>o__32.<>p__1.Target(<>o__32.<>p__1, <>o__32.<>p__0.Target(<>o__32.<>p__0, this._dynObj, "status")), "failed")))
                {
                    goto Label_25A0;
                }
                if (<>o__32.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__7.Target(<>o__32.<>p__7, <>o__32.<>p__6.Target(<>o__32.<>p__6, <>o__32.<>p__5.Target(<>o__32.<>p__5, <>o__32.<>p__4.Target(<>o__32.<>p__4, this._dynObj, "status")), "dup")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.DUPLICATE_ORDER);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                    return false;
                }
                if (<>o__32.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__11 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (!<>o__32.<>p__11.Target(<>o__32.<>p__11, <>o__32.<>p__10.Target(<>o__32.<>p__10, <>o__32.<>p__9.Target(<>o__32.<>p__9, <>o__32.<>p__8.Target(<>o__32.<>p__8, this._dynObj, "status")), "queued")))
                {
                    goto Label_22AA;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_ORDER, null, "", "");
                goto Label_2096;
            Label_163B:
                if (<>o__32.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (!<>o__32.<>p__15.Target(<>o__32.<>p__15, <>o__32.<>p__14.Target(<>o__32.<>p__14, <>o__32.<>p__13.Target(<>o__32.<>p__13, <>o__32.<>p__12.Target(<>o__32.<>p__12, this._dynObj, "status")), "queued")))
                {
                    goto Label_2567;
                }
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                if (string.IsNullOrEmpty(str23))
                {
                    if (<>o__32.<>p__18 == null)
                    {
                        <>o__32.<>p__18 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                    }
                    if (<>o__32.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                    }
                    str23 = <>o__32.<>p__18.Target(<>o__32.<>p__18, <>o__32.<>p__17.Target(<>o__32.<>p__17, <>o__32.<>p__16.Target(<>o__32.<>p__16, this._dynObj, "slug")));
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._request = (HttpWebRequest) WebRequest.Create($"https://www.supremenewyork.com/checkout/{str23}/status.json");
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        this._request.CookieContainer = this._runner.Cookies;
                        this._request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Mobile Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        this._request.Accept = "*";
                        this._request.KeepAlive = true;
                        this._request.Headers.Add("Origin", "http://www.supremenewyork.com");
                        this._request.Referer = "http://www.supremenewyork.com/mobile";
                        this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        using (WebResponse response3 = this._request.GetResponse())
                        {
                            using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                            {
                                this._srr = reader3.ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch
                    {
                        Thread.Sleep(500);
                        flag = true;
                        continue;
                    }
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__32.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__22 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__19 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__22.Target(<>o__32.<>p__22, <>o__32.<>p__21.Target(<>o__32.<>p__21, <>o__32.<>p__20.Target(<>o__32.<>p__20, <>o__32.<>p__19.Target(<>o__32.<>p__19, this._dynObj, "status")), "failed")))
                {
                    goto Label_2141;
                }
                if (<>o__32.<>p__30 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__30 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__29 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__28 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (!<>o__32.<>p__30.Target(<>o__32.<>p__30, <>o__32.<>p__29.Target(<>o__32.<>p__29, <>o__32.<>p__28.Target(<>o__32.<>p__28, <>o__32.<>p__27.Target(<>o__32.<>p__27, this._dynObj, "status")), "queued")))
                {
                    if (<>o__32.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__34 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__34.Target(<>o__32.<>p__34, <>o__32.<>p__33.Target(<>o__32.<>p__33, <>o__32.<>p__32.Target(<>o__32.<>p__32, <>o__32.<>p__31.Target(<>o__32.<>p__31, this._dynObj, "status")), "dup")))
                    {
                        goto Label_210F;
                    }
                    if (<>o__32.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__38 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__35 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__38.Target(<>o__32.<>p__38, <>o__32.<>p__37.Target(<>o__32.<>p__37, <>o__32.<>p__36.Target(<>o__32.<>p__36, <>o__32.<>p__35.Target(<>o__32.<>p__35, this._dynObj, "status")), "outOfStock")))
                    {
                        goto Label_20DE;
                    }
                    if (<>o__32.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__42 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__41 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__39 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__42.Target(<>o__32.<>p__42, <>o__32.<>p__41.Target(<>o__32.<>p__41, <>o__32.<>p__40.Target(<>o__32.<>p__40, <>o__32.<>p__39.Target(<>o__32.<>p__39, this._dynObj, "status")), "paid")))
                    {
                        goto Label_20A2;
                    }
                    try
                    {
                        EveAIO.Helpers.AddDbValue("Supreme|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                        goto Label_2096;
                    }
                    catch
                    {
                        goto Label_2096;
                    }
                }
                Thread.Sleep(600);
                goto Label_2096;
            Label_2065:
                infoArray38 = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__32.<>p__15 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), infoArray38));
                goto Label_163B;
            Label_2096:
                if (<>o__32.<>p__15 != null)
                {
                    goto Label_163B;
                }
                goto Label_2065;
            Label_20A2:
                if (this.CHILD_SUCCESSES.Count > 0)
                {
                    Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                        using (List<SuccessObject>.Enumerator enumerator = this.CHILD_SUCCESSES.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                            Label_0019:
                                if (-174951405 || true)
                                {
                                    goto Label_003A;
                                }
                            Label_001C:
                                enumerator.Current.CheckoutHidden = 0;
                            Label_003A:
                                switch ((-174951405 ^ -910628214) % 4)
                                {
                                    case 0:
                                        goto Label_003A;

                                    case 1:
                                        goto Label_001C;

                                    case 3:
                                    {
                                        continue;
                                    }
                                }
                                return;
                            }
                            goto Label_0019;
                        }
                    }, Array.Empty<object>());
                }
                return true;
            Label_20DE:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            Label_210F:
                this._task.Status = States.GetTaskState(States.TaskState.DUPLICATE_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                return false;
            Label_2141:
                try
                {
                    if (<>o__32.<>p__26 == null)
                    {
                        <>o__32.<>p__26 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                    }
                    if (<>o__32.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                    }
                    if (<>o__32.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.ResultIndexed, "mpa", typeof(Supreme), argumentInfo));
                    }
                    <>o__32.<>p__26.Target(<>o__32.<>p__26, <>o__32.<>p__25.Target(<>o__32.<>p__25, <>o__32.<>p__24.Target(<>o__32.<>p__24, <>o__32.<>p__23.Target(<>o__32.<>p__23, this._dynObj), "Failure Reason")));
                }
                catch
                {
                }
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                return false;
            Label_22AA:
                if (<>o__32.<>p__46 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__46 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__45 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__45 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__44 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__43 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__46.Target(<>o__32.<>p__46, <>o__32.<>p__45.Target(<>o__32.<>p__45, <>o__32.<>p__44.Target(<>o__32.<>p__44, <>o__32.<>p__43.Target(<>o__32.<>p__43, this._dynObj, "status")), "outOfStock")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                if (<>o__32.<>p__50 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__50 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__49 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__49 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__48 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__32.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__47 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__32.<>p__47 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                }
                if (<>o__32.<>p__50.Target(<>o__32.<>p__50, <>o__32.<>p__49.Target(<>o__32.<>p__49, <>o__32.<>p__48.Target(<>o__32.<>p__48, <>o__32.<>p__47.Target(<>o__32.<>p__47, this._dynObj, "status")), "paid")))
                {
                    return true;
                }
            Label_2567:;
                try
                {
                    EveAIO.Helpers.AddDbValue("Supreme|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                return false;
            Label_25A0:
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
                return false;
            }
        }

        private void CreateChildProdSuccess(TaskObject task, Product product)
        {
            SuccessObject success = new SuccessObject {
                Id = Guid.NewGuid().ToString(),
                TaskId = task.Id,
                TaskName = task.Name,
                Time = new DateTime?(DateTime.Now),
                Parent = this._task.Name,
                AtcHidden = 0,
                IsMultiCart = true,
                Store = EveAIO.Helpers.GetStoreUrl(this._task)
            };
            this.CHILD_SUCCESSES.Add(success);
            success.ProductName = product.ProductTitle;
            success.Size = product.SuprimetMultiPickedSize.Value.Key;
            if (string.IsNullOrEmpty(success.Size))
            {
                success.Size = "-";
            }
            success.Price = product.Price;
            success.Link = product.Link;
            success.ProductImage = task.ImgUrl;
            success.Proxy = "-";
            if (!string.IsNullOrEmpty(this._task.Proxy))
            {
                success.Proxy = "-";
            }
            else
            {
                success.Proxy = this._task.Proxy;
            }
            Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                if (Global.SUCCESS.Any<SuccessObject>(x => x.TaskId == task.Id))
                {
                    SuccessObject obj2 = Global.SUCCESS.First<SuccessObject>(x => x.TaskId == task.Id);
                    success.Repetitions = obj2.Repetitions + 1;
                    int index = Global.SUCCESS.IndexOf(Global.SUCCESS.First<SuccessObject>(x => x.TaskId == task.Id));
                    Global.SUCCESS[index] = success;
                }
                else
                {
                    success.Repetitions = 1;
                    Global.SUCCESS.Add(success);
                }
            }, Array.Empty<object>());
        }

        public bool DirectLink(string link)
        {
            if (this._task.SupremeAutomation == TaskObject.SuprimeAutomationEnum.browserless)
            {
                return this.DirectLinkBrowserless(link);
            }
            return this.DirectLinkBrowser(link);
        }

        public bool DirectLinkBrowser(string link)
        {
            try
            {
                if (this._task.Driver == null)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                    return false;
                }
                if (this._task.TaskType != TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", link);
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                this._task.Driver.Navigate().GoToUrl(link);
                string pageSource = this._task.Driver.PageSource;
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(pageSource);
                if (!this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                string str2 = this._runner.ProductPageHtml.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                string str4 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim();
                string str3 = "http:" + this._runner.ProductPageHtml.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value.Trim();
                if (this._runner.ProductPageHtml.DocumentNode.Descendants("p").Any<HtmlNode>(x => (x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model")))
                {
                    str2 = str2 + " / " + this._runner.ProductPageHtml.DocumentNode.Descendants("p").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"))).InnerText.Trim();
                }
                this._task.ImgUrl = str3;
                Product product1 = new Product {
                    ProductTitle = str2,
                    Link = link,
                    Price = str4
                };
                this._runner.Product = product1;
                if (this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size")))
                {
                    foreach (HtmlNode node in this._runner.ProductPageHtml.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"))).Descendants("option"))
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(node.InnerText.Trim(), node.Attributes["value"].Value);
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                }
                else if (this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                {
                    foreach (HtmlNode node2 in this._runner.ProductPageHtml.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"))).Descendants("option"))
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(node2.InnerText.Trim(), node2.Attributes["value"].Value);
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                }
                else if (!this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"))))
                {
                    if (this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                    {
                        HtmlNode node3 = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(node3.InnerText.Trim(), node3.Attributes["value"].Value);
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                }
                else
                {
                    HtmlNode node4 = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(node4.InnerText.Trim(), node4.Attributes["value"].Value);
                    this._runner.Product.AvailableSizes.Add(item);
                }
                if (this._runner.Product.AvailableSizes.Count != 0)
                {
                    if (this._task.IsNegativeSizing)
                    {
                        this._runner.Product.AvailableSizes = EveAIO.Helpers.ProcessNegativeSizing(this._runner.Product.AvailableSizes, this._task);
                        if (this._runner.Product.AvailableSizes.Count == 0)
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                            return false;
                        }
                    }
                    if (this._task.PriceCheck)
                    {
                        string str5 = "";
                        foreach (char ch in this._runner.Product.Price)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str5 = str5 + ch.ToString();
                            }
                        }
                        int num2 = int.Parse(str5.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num2 < this._task.MinimumPrice) || (num2 > this._task.MaximumPrice))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                            return false;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                    }
                    if (!this._task.RandomSize)
                    {
                        char[] separator = new char[] { '|' };
                        string[] strArray2 = this._task.Size.Split(separator);
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            strArray2[i] = strArray2[i].Trim().ToUpperInvariant();
                        }
                        foreach (string sz in strArray2)
                        {
                            if (this._runner.PickedSize.HasValue)
                            {
                                break;
                            }
                            using (List<KeyValuePair<string, string>>.Enumerator enumerator2 = this._runner.Product.AvailableSizes.GetEnumerator())
                            {
                                KeyValuePair<string, string> pair5;
                                goto Label_091C;
                            Label_08A6:
                                pair5 = enumerator2.Current;
                                char[] chArray2 = new char[] { ' ' };
                                string[] source = pair5.Key.Split(chArray2);
                                for (int j = 0; j < source.Length; j++)
                                {
                                    source[j] = source[j].Trim().ToUpperInvariant();
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_092A;
                                }
                            Label_091C:
                                if (!enumerator2.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_08A6;
                            Label_092A:
                                this._runner.PickedSize = new KeyValuePair<string, string>?(pair5);
                            }
                        }
                        if (!this._runner.PickedSize.HasValue)
                        {
                            if (!this._task.SizePickRandom)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    this._runner.PickedSize = new KeyValuePair<string, string>?(this._runner.Product.AvailableSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizes.Count)]);
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "");
                return false;
            }
        }

        public bool DirectLinkBrowserless(string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                string requestUriString = link;
                this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
                this._request.KeepAlive = false;
                this._request.Accept = "*/*";
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (HttpWebResponse response = (HttpWebResponse) this._request.GetResponse())
                {
                    this._srr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                if (!this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                string str3 = this._runner.ProductPageHtml.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                string str4 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim();
                string str2 = "http:" + this._runner.ProductPageHtml.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value.Trim();
                if (this._runner.ProductPageHtml.DocumentNode.Descendants("p").Any<HtmlNode>(x => (x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model")))
                {
                    str3 = str3 + " / " + this._runner.ProductPageHtml.DocumentNode.Descendants("p").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"))).InnerText.Trim();
                }
                this._task.ImgUrl = str2;
                Product product1 = new Product {
                    ProductTitle = str3,
                    Link = link,
                    Price = str4
                };
                this._runner.Product = product1;
                if (this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size")))
                {
                    foreach (HtmlNode node in this._runner.ProductPageHtml.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"))).Descendants("option"))
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(node.InnerText.Trim(), node.Attributes["value"].Value);
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                }
                else if (this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                {
                    foreach (HtmlNode node2 in this._runner.ProductPageHtml.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"))).Descendants("option"))
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(node2.InnerText.Trim(), node2.Attributes["value"].Value);
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                }
                else if (this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size")))
                {
                    HtmlNode node4 = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(node4.InnerText.Trim(), node4.Attributes["value"].Value);
                    this._runner.Product.AvailableSizes.Add(item);
                }
                else if (this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                {
                    HtmlNode node5 = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(node5.InnerText.Trim(), node5.Attributes["value"].Value);
                    this._runner.Product.AvailableSizes.Add(item);
                }
                if (this._runner.Product.AvailableSizes.Count != 0)
                {
                    if (this._task.IsNegativeSizing)
                    {
                        this._runner.Product.AvailableSizes = EveAIO.Helpers.ProcessNegativeSizing(this._runner.Product.AvailableSizes, this._task);
                        if (this._runner.Product.AvailableSizes.Count == 0)
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                            return false;
                        }
                    }
                    if (this._task.PriceCheck)
                    {
                        string str6 = "";
                        foreach (char ch in this._runner.Product.Price)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str6 = str6 + ch.ToString();
                            }
                        }
                        int num3 = int.Parse(str6.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num3 < this._task.MinimumPrice) || (num3 > this._task.MaximumPrice))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                            return false;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                    }
                    HtmlNode node3 = this._runner.ProductPageHtml.DocumentNode.Descendants("form").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "add"));
                    if (!node3.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"))))
                    {
                        if (node3.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st")))
                        {
                            this._style = node3.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"))).Attributes["value"].Value;
                        }
                    }
                    else
                    {
                        this._style = node3.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"))).Attributes["value"].Value;
                    }
                    this._id = node3.Attributes["action"].Value;
                    this._id = this._id.Replace("/shop/", "").Replace("/add", "");
                    if (!this._task.RandomSize)
                    {
                        char[] separator = new char[] { '#' };
                        string[] strArray2 = this._task.Size.Split(separator);
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            strArray2[i] = strArray2[i].Trim().ToUpperInvariant();
                        }
                        foreach (string sz in strArray2)
                        {
                            if (this._runner.PickedSize.HasValue)
                            {
                                break;
                            }
                            using (List<KeyValuePair<string, string>>.Enumerator enumerator2 = this._runner.Product.AvailableSizes.GetEnumerator())
                            {
                                KeyValuePair<string, string> current;
                                while (enumerator2.MoveNext())
                                {
                                    current = enumerator2.Current;
                                    char[] chArray2 = new char[] { ' ' };
                                    string[] source = current.Key.Split(chArray2);
                                    for (int j = 0; j < source.Length; j++)
                                    {
                                        source[j] = source[j].Trim().ToUpperInvariant();
                                    }
                                    if (source.Any<string>(x => x == sz))
                                    {
                                        goto Label_0AC1;
                                    }
                                }
                                continue;
                            Label_0AC1:
                                this._runner.PickedSize = new KeyValuePair<string, string>?(current);
                            }
                        }
                        if (this._runner.PickedSize.HasValue)
                        {
                            return true;
                        }
                        if (!this._task.SizePickRandom)
                        {
                            return false;
                        }
                    }
                    this._runner.PickedSize = new KeyValuePair<string, string>?(this._runner.Product.AvailableSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizes.Count)]);
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "");
                return false;
            }
        }

        public bool DirectLinkFallback(string link)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Accept = "*/*";
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                string html = "";
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                if (!html.Contains("Connection timed out"))
                {
                    this._runner.ProductPageHtml = new HtmlDocument();
                    this._runner.ProductPageHtml.LoadHtml(html);
                    if (this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit")))
                    {
                        string str2 = this._runner.ProductPageHtml.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                        string str4 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim();
                        string str3 = "http:" + this._runner.ProductPageHtml.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value.Trim();
                        if (this._runner.ProductPageHtml.DocumentNode.Descendants("p").Any<HtmlNode>(x => (x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model")))
                        {
                            str2 = str2 + " / " + this._runner.ProductPageHtml.DocumentNode.Descendants("p").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"))).InnerText.Trim();
                        }
                        this._task.ImgUrl = str3;
                        Product product1 = new Product {
                            ProductTitle = str2,
                            Link = link,
                            Price = str4
                        };
                        this._fallbackProduct = product1;
                        if (this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size")))
                        {
                            foreach (HtmlNode node in this._runner.ProductPageHtml.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"))).Descendants("option"))
                            {
                                KeyValuePair<string, string> item = new KeyValuePair<string, string>(node.InnerText.Trim(), node.Attributes["value"].Value);
                                this._fallbackProduct.AvailableSizes.Add(item);
                            }
                        }
                        else if (this._runner.ProductPageHtml.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                        {
                            foreach (HtmlNode node2 in this._runner.ProductPageHtml.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"))).Descendants("option"))
                            {
                                KeyValuePair<string, string> item = new KeyValuePair<string, string>(node2.InnerText.Trim(), node2.Attributes["value"].Value);
                                this._fallbackProduct.AvailableSizes.Add(item);
                            }
                        }
                        else if (this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size")))
                        {
                            HtmlNode node4 = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));
                            KeyValuePair<string, string> item = new KeyValuePair<string, string>(node4.InnerText.Trim(), node4.Attributes["value"].Value);
                            this._fallbackProduct.AvailableSizes.Add(item);
                        }
                        else if (this._runner.ProductPageHtml.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                        {
                            HtmlNode node5 = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));
                            KeyValuePair<string, string> item = new KeyValuePair<string, string>(node5.InnerText.Trim(), node5.Attributes["value"].Value);
                            this._fallbackProduct.AvailableSizes.Add(item);
                        }
                        if (this._fallbackProduct.AvailableSizes.Count == 0)
                        {
                            return false;
                        }
                        if (this._task.PriceCheck)
                        {
                            string str6 = "";
                            foreach (char ch in this._fallbackProduct.Price)
                            {
                                if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                                {
                                    str6 = str6 + ch.ToString();
                                }
                            }
                            int num4 = int.Parse(str6.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                            if ((num4 < this._task.MinimumPrice) || (num4 > this._task.MaximumPrice))
                            {
                                return false;
                            }
                        }
                        HtmlNode node3 = this._runner.ProductPageHtml.DocumentNode.Descendants("form").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "add"));
                        if (node3.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style")))
                        {
                            this._style = node3.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"))).Attributes["value"].Value;
                        }
                        else if (node3.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st")))
                        {
                            this._style = node3.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"))).Attributes["value"].Value;
                        }
                        this._id = node3.Attributes["action"].Value;
                        this._id = this._id.Replace("/shop/", "").Replace("/add", "");
                        if (!this._task.RandomSize)
                        {
                            char[] separator = new char[] { '#' };
                            string[] strArray = this._task.Size.Split(separator);
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                strArray[i] = strArray[i].Trim().ToUpperInvariant();
                            }
                            foreach (string sz in strArray)
                            {
                                if (this._fallbackPickedSize.HasValue)
                                {
                                    break;
                                }
                                using (List<KeyValuePair<string, string>>.Enumerator enumerator2 = this._fallbackProduct.AvailableSizes.GetEnumerator())
                                {
                                    KeyValuePair<string, string> current;
                                    while (enumerator2.MoveNext())
                                    {
                                        current = enumerator2.Current;
                                        char[] chArray2 = new char[] { ' ' };
                                        string[] source = current.Key.Split(chArray2);
                                        for (int j = 0; j < source.Length; j++)
                                        {
                                            source[j] = source[j].Trim().ToUpperInvariant();
                                        }
                                        if (source.Any<string>(x => x == sz))
                                        {
                                            goto Label_09AF;
                                        }
                                    }
                                    continue;
                                Label_09AF:
                                    this._fallbackPickedSize = new KeyValuePair<string, string>?(current);
                                }
                            }
                            if (!this._fallbackPickedSize.HasValue)
                            {
                                if (!this._task.SizePickRandom)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                        this._fallbackPickedSize = new KeyValuePair<string, string>?(this._fallbackProduct.AvailableSizes[this._runner.Rnd.Next(0, this._fallbackProduct.AvailableSizes.Count)]);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Product DirectLinkMulti(TaskObject task, string link)
        {
            try
            {
                string str2;
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                string requestUriString = link;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://www.supremenewyork.com/shop");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Accept = "*/*";
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        reader.ReadToEnd();
                    }
                }
                request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.KeepAlive = false;
                request.Accept = "*/*";
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                using (HttpWebResponse response2 = (HttpWebResponse) request.GetResponse())
                {
                    str2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(str2);
                if (!document.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return null;
                }
                string str3 = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                string str4 = document.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim();
                string str5 = "http:" + document.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value.Trim();
                if (document.DocumentNode.Descendants("p").Any<HtmlNode>(x => (x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model")))
                {
                    str3 = str3 + " / " + document.DocumentNode.Descendants("p").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"))).InnerText.Trim();
                }
                task.ImgUrl = str5;
                Product product = new Product {
                    ProductTitle = str3,
                    Link = link,
                    Price = str4
                };
                if (document.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size")))
                {
                    foreach (HtmlNode node in document.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"))).Descendants("option"))
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(node.InnerText.Trim(), node.Attributes["value"].Value);
                        product.AvailableSizes.Add(item);
                    }
                }
                else if (document.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                {
                    foreach (HtmlNode node2 in document.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"))).Descendants("option"))
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(node2.InnerText.Trim(), node2.Attributes["value"].Value);
                        product.AvailableSizes.Add(item);
                    }
                }
                else if (document.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size")))
                {
                    HtmlNode node4 = document.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(node4.InnerText.Trim(), node4.Attributes["value"].Value);
                    product.AvailableSizes.Add(item);
                }
                else if (document.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s")))
                {
                    HtmlNode node5 = document.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(node5.InnerText.Trim(), node5.Attributes["value"].Value);
                    product.AvailableSizes.Add(item);
                }
                if (task.PriceCheck)
                {
                    string str7 = "";
                    foreach (char ch in product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str7 = str7 + ch.ToString();
                        }
                    }
                    double num4 = double.Parse(str7.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num4 < task.MinimumPrice) || (num4 > task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        return null;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
                HtmlNode node3 = document.DocumentNode.Descendants("form").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "add"));
                if (node3.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style")))
                {
                    product.SuprimetMultiStyle = node3.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"))).Attributes["value"].Value;
                }
                else if (node3.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st")))
                {
                    product.SuprimetMultiStyle = node3.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"))).Attributes["value"].Value;
                }
                product.SuprimeMultiId = node3.Attributes["action"].Value;
                product.SuprimeMultiId = product.SuprimeMultiId.Replace("/shop/", "").Replace("/add", "");
                KeyValuePair<string, string>? nullable = null;
                if (product.AvailableSizes.Count > 0)
                {
                    if (!task.RandomSize)
                    {
                        char[] separator = new char[] { '#' };
                        string[] strArray = task.Size.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (string sz in strArray)
                        {
                            if (nullable.HasValue)
                            {
                                break;
                            }
                            using (List<KeyValuePair<string, string>>.Enumerator enumerator2 = product.AvailableSizes.GetEnumerator())
                            {
                                KeyValuePair<string, string> pair5;
                                goto Label_099A;
                            Label_0924:
                                pair5 = enumerator2.Current;
                                char[] chArray2 = new char[] { ' ' };
                                string[] source = pair5.Key.Split(chArray2);
                                for (int j = 0; j < source.Length; j++)
                                {
                                    source[j] = source[j].Trim().ToUpperInvariant();
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_09A8;
                                }
                            Label_099A:
                                if (!enumerator2.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_0924;
                            Label_09A8:
                                nullable = new KeyValuePair<string, string>?(pair5);
                            }
                        }
                        if (!nullable.HasValue)
                        {
                            if (!task.SizePickRandom)
                            {
                                return null;
                            }
                        }
                        else
                        {
                            product.SuprimetMultiPickedSize = nullable;
                            return product;
                        }
                    }
                    nullable = new KeyValuePair<string, string>?(product.AvailableSizes[this._runner.Rnd.Next(0, product.AvailableSizes.Count)]);
                    product.SuprimetMultiPickedSize = nullable;
                    return product;
                }
                return null;
            }
            catch (ThreadAbortException)
            {
                return null;
            }
            catch (Exception exception)
            {
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "");
                return null;
            }
        }

        internal static List<KeyValuePair<string, string>> GetJapanStates() => 
            new List<KeyValuePair<string, string>> { 
                new KeyValuePair<string, string>(" 北海道", "Hokkaido"),
                new KeyValuePair<string, string>(" 青森県", "Aomori Prefecture"),
                new KeyValuePair<string, string>(" 岩手県", "Iwate Prefecture"),
                new KeyValuePair<string, string>(" 宮城県", "Miyagi Prefecture"),
                new KeyValuePair<string, string>(" 秋田県", "Akita"),
                new KeyValuePair<string, string>(" 山形県", "Yamagata Prefecture"),
                new KeyValuePair<string, string>(" 福島県", "Fukushima Prefecture"),
                new KeyValuePair<string, string>(" 茨城県", "Ibaraki Prefecture"),
                new KeyValuePair<string, string>(" 栃木県", "Tochigi Prefecture"),
                new KeyValuePair<string, string>(" 群馬県", "Gunma Prefecture"),
                new KeyValuePair<string, string>(" 埼玉県", "Saitama"),
                new KeyValuePair<string, string>(" 千葉県", "Chiba ken"),
                new KeyValuePair<string, string>(" 東京都", "Tokyo"),
                new KeyValuePair<string, string>(" 神奈川県", "Kanagawa Prefecture"),
                new KeyValuePair<string, string>(" 新潟県", "Niigata Prefecture"),
                new KeyValuePair<string, string>(" 富山県", "Toyama Prefecture"),
                new KeyValuePair<string, string>(" 石川県", "Ishikawa Prefecture"),
                new KeyValuePair<string, string>(" 福井県", "Fukui Prefecture"),
                new KeyValuePair<string, string>(" 山梨県", "Yamanashi Prefecture"),
                new KeyValuePair<string, string>(" 長野県", "Nagano Prefecture"),
                new KeyValuePair<string, string>(" 岐阜県", "Gifu Prefecture"),
                new KeyValuePair<string, string>(" 静岡県", "Shizuoka Prefecture"),
                new KeyValuePair<string, string>(" 愛知県", "Aichi-ken"),
                new KeyValuePair<string, string>(" 三重県", "Mie Prefecture"),
                new KeyValuePair<string, string>(" 滋賀県", "Shiga Prefecture"),
                new KeyValuePair<string, string>(" 京都府", "Kyoto"),
                new KeyValuePair<string, string>(" 大阪府", "Osaka prefecture"),
                new KeyValuePair<string, string>(" 兵庫県", "Hyōgo Prefecture"),
                new KeyValuePair<string, string>(" 奈良県", "Nara Prefecture"),
                new KeyValuePair<string, string>(" 和歌山県", "Wakayama Prefecture"),
                new KeyValuePair<string, string>(" 鳥取県", "Tottori prefecture"),
                new KeyValuePair<string, string>(" 島根県", "Shimane Prefecture"),
                new KeyValuePair<string, string>(" 岡山県", "Okayama Prefecture"),
                new KeyValuePair<string, string>(" 広島県", "Hiroshima"),
                new KeyValuePair<string, string>(" 山口県", "Yamaguchi Prefecture"),
                new KeyValuePair<string, string>(" 徳島県", "Tokushima"),
                new KeyValuePair<string, string>(" 香川県", "Kagawa Prefecture"),
                new KeyValuePair<string, string>(" 愛媛県", "Ehime Prefecture"),
                new KeyValuePair<string, string>(" 高知県", "Kochi Prefecture"),
                new KeyValuePair<string, string>(" 福岡県", "Fukuoka Prefecture"),
                new KeyValuePair<string, string>(" 佐賀県", "Saga Prefecture"),
                new KeyValuePair<string, string>(" 長崎県", "Nagasaki Prefecture"),
                new KeyValuePair<string, string>(" 熊本県", "Kumamoto Prefecture"),
                new KeyValuePair<string, string>(" 大分県", "Oita Prefecture"),
                new KeyValuePair<string, string>(" 宮崎県", "Miyazaki"),
                new KeyValuePair<string, string>(" 鹿児島県", "Kagoshima prefecture"),
                new KeyValuePair<string, string>(" 沖縄県", "Okinawa Prefecture")
            };

        public bool Login() => 
            true;

        public void MultiCart(List<TaskObject> childTasks)
        {
            this._childProducts = new List<KeyValuePair<string, Product>>();
            foreach (TaskObject obj2 in childTasks)
            {
                try
                {
                    if (obj2.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        Product product = this.DirectLinkMulti(obj2, obj2.Link);
                        if ((product != null) && this.AtcMulti(obj2, product))
                        {
                            this._childProducts.Add(new KeyValuePair<string, Product>(obj2.Id, product));
                            this.CreateChildProdSuccess(obj2, product);
                        }
                    }
                    else
                    {
                        Product product = this.SearchMulti(obj2);
                        if ((product != null) && this.AtcMulti(obj2, product))
                        {
                            this._childProducts.Add(new KeyValuePair<string, Product>(obj2.Id, product));
                            this.CreateChildProdSuccess(obj2, product);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public bool Search()
        {
            if (this._task.SupremeAutomation == TaskObject.SuprimeAutomationEnum.browserless)
            {
                return this.SearchBrowserless();
            }
            return this.SearchBrowser();
        }

        public bool SearchBrowser()
        {
            try
            {
                if (this._task.Driver == null)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                type.GetMembers();
                List<KeyValuePair<string, string>> source = (List<KeyValuePair<string, string>>) type.GetField("SUPREME_LINKS", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                this._task.Driver.Navigate().GoToUrl(source.First<KeyValuePair<string, string>>(x => (x.Key == this._task.Group)).Value);
                HtmlDocument document1 = new HtmlDocument();
                document1.LoadHtml(this._task.Driver.PageSource);
                List<Tuple<string, string, string>> list2 = new List<Tuple<string, string, string>>();
                foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("div")
                    where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "inner-article")
                    select x)
                {
                    string str3 = node.Descendants("h1").First<HtmlNode>().InnerText.Replace("\x00d0\x00b0", "a").Replace("\x00d0\x00b5", "e").Replace("\x00d0…", "S").Replace("\x00ce’", "B").Replace("Β", "B").Replace("а", "a").Replace("е", "e").Replace("Ѕ", "S").Replace("/", " ").RemoveSpecialCharacters();
                    string innerText = node.Descendants("p").First<HtmlNode>().InnerText;
                    string str2 = "http://www.supremenewyork.com" + node.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                    list2.Add(new Tuple<string, string, string>(str3, innerText, str2));
                }
                List<Tuple<string, string, string>> list3 = new List<Tuple<string, string, string>>();
                List<string> list4 = new List<string>();
                foreach (string str4 in this._task.Keywords)
                {
                    list4.Add(str4);
                }
                foreach (string str5 in this._task.Keywords)
                {
                    list4.Add(str5.Replace("a", "").Replace("A", "").Replace("u", "").Replace("U", "").Replace("o", "").Replace("O", "").Replace("e", "").Replace("E", "").Replace("i", "").Replace("I", ""));
                }
                using (List<string>.Enumerator enumerator3 = list4.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = enumerator3.Current.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (Tuple<string, string, string> product in list2)
                        {
                            bool flag2 = true;
                            foreach (string kw in strArray)
                            {
                                if (kw[0] == '+')
                                {
                                    char[] chArray2 = new char[] { ' ' };
                                    string[] strArray3 = product.Item1.ToUpperInvariant().Split(chArray2);
                                    for (int j = 0; j < strArray3.Length; j++)
                                    {
                                        strArray3[j] = strArray3[j].Trim();
                                    }
                                    if (!strArray3.Any<string>(x => x.StartsWith(kw.Substring(1))))
                                    {
                                        goto Label_04B8;
                                    }
                                }
                                else if (flag2 && !product.Item1.ToUpperInvariant().Contains(kw))
                                {
                                    goto Label_04BD;
                                }
                            }
                            goto Label_04C0;
                        Label_04B8:
                            flag2 = false;
                            goto Label_04C0;
                        Label_04BD:
                            flag2 = false;
                        Label_04C0:
                            if (flag2 && !list3.Any<Tuple<string, string, string>>(x => (x.Item3 == product.Item3)))
                            {
                                list3.Add(product);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this._task.Color))
                {
                    List<Tuple<string, string, string>> list5 = new List<Tuple<string, string, string>>();
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                    {
                        foreach (Tuple<string, string, string> tuple in list3)
                        {
                            if (tuple.Item2.ToUpperInvariant() == this._task.Color.ToUpperInvariant())
                            {
                                list5.Add(tuple);
                            }
                        }
                    }
                    else
                    {
                        foreach (Tuple<string, string, string> tuple2 in list3)
                        {
                            if (tuple2.Item2.ToUpperInvariant().Contains(this._task.Color.ToUpperInvariant()))
                            {
                                list5.Add(tuple2);
                            }
                        }
                    }
                    if ((list5.Count > 0) || !this._task.ColorPickRandom)
                    {
                        list3 = list5;
                    }
                }
                if (list3.Count != 0)
                {
                    List<Tuple<string, string, string>> list6 = new List<Tuple<string, string, string>>();
                    if (this._task.NegativeKeywords.Any<string>())
                    {
                        foreach (Tuple<string, string, string> tuple3 in list3)
                        {
                            bool flag3 = true;
                            Tuple<string, string, string> tmpProd = tuple3;
                            using (IEnumerator<string> enumerator2 = this._task.NegativeKeywords.GetEnumerator())
                            {
                                while (enumerator2.MoveNext())
                                {
                                    char[] separator = new char[] { ' ' };
                                    string[] strArray4 = enumerator2.Current.Split(separator);
                                    for (int i = 0; i < strArray4.Length; i++)
                                    {
                                        strArray4[i] = strArray4[i].Trim().ToUpperInvariant();
                                    }
                                    foreach (string str6 in strArray4)
                                    {
                                        if (tuple3.Item1.ToUpperInvariant().Contains(str6))
                                        {
                                            goto Label_06F9;
                                        }
                                    }
                                    continue;
                                Label_06F9:
                                    flag3 = false;
                                }
                            }
                            if (flag3 && !list6.Any<Tuple<string, string, string>>(x => (x.Item3 == tmpProd.Item3)))
                            {
                                list6.Add(tmpProd);
                            }
                        }
                        if (list6.Count == 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        list6 = list3;
                    }
                    foreach (Tuple<string, string, string> tuple4 in list6)
                    {
                        if (this.DirectLinkBrowser(tuple4.Item3))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", "");
                return false;
            }
        }

        public bool SearchBrowserless()
        {
            try
            {
                List<SupremeProduct> list3;
                this._fallbackProduct = null;
                if (this._task.Group != "all")
                {
                    Task.Factory.StartNew(delegate {
                        this.SearchFallback();
                    });
                }
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://www.supremenewyork.com/shop");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Accept = "*";
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                string str = "";
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                request = (HttpWebRequest) WebRequest.Create("http://www.supremenewyork.com/mobile_stock.json");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.KeepAlive = true;
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Accept = "*/*";
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.ServicePoint.Expect100Continue = false;
                str = "";
                using (WebResponse response2 = request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        str = reader2.ReadToEnd();
                    }
                }
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                this._stockJson = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                if (<>o__34.<>p__1 == null)
                {
                    <>o__34.<>p__1 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Newtonsoft.Json.Linq.JObject), typeof(Supreme)));
                }
                if (<>o__34.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__34.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "products_and_categories", typeof(Supreme), argumentInfo));
                }
                List<SupremeProduct> list = new List<SupremeProduct>();
                foreach (Newtonsoft.Json.Linq.JToken token in <>o__34.<>p__1.Target(<>o__34.<>p__1, <>o__34.<>p__0.Target(<>o__34.<>p__0, obj2)).Children())
                {
                    foreach (Newtonsoft.Json.Linq.JToken token2 in ((Newtonsoft.Json.Linq.JArray) token.First).Children())
                    {
                        string str3 = token.ToString();
                        str3 = str3.Substring(str3.IndexOf("\"") + 1);
                        str3 = str3.Substring(0, str3.IndexOf("\""));
                        string str2 = token2["image_url"].ToString();
                        str2 = str2.Substring(str2.LastIndexOf("/") + 1);
                        str2 = str2.Substring(0, str2.LastIndexOf("."));
                        SupremeProduct item = new SupremeProduct {
                            SpecialName = str2,
                            GroupName = str3,
                            RealName = token2["name"].ToString(),
                            Id = token2["id"].ToString(),
                            Name = token2["name"].ToString().Replace("™", "").Replace("\x00d0\x00b0", "a").Replace("\x00d0\x00b5", "e").Replace("\x00d0…", "S").Replace("\x00ce’", "B").Replace("Β", "B").Replace("а", "a").Replace("е", "e").Replace("Ѕ", "S").Replace("/", " "),
                            ImageUrl = token2["image_url"].ToString(),
                            ImageUrlHi = token2["image_url_hi"].ToString(),
                            Price = token2["price"].ToString(),
                            SalePrice = token2["sale_price"].ToString(),
                            NewItem = token2["new_item"].ToString(),
                            Position = token2["position"].ToString(),
                            CategoryName = token2["category_name"].ToString()
                        };
                        list.Add(item);
                    }
                }
                foreach (SupremeProduct local1 in list)
                {
                    local1.Name = local1.Name.RemoveSpecialCharacters();
                }
                List<SupremeProduct> source = new List<SupremeProduct>();
                string group = this._task.Group;
                uint num2 = <PrivateImplementationDetails>.ComputeStringHash(group);
                if (num2 <= 0x6e2ce0f8)
                {
                    if (num2 > 0x29dcae48)
                    {
                        if (num2 > 0x3baaf4e6)
                        {
                            if (num2 == 0x4ef8af50)
                            {
                                if (group == "shirts")
                                {
                                    source = (from x9 in list
                                        where x9.GroupName == "Shirts"
                                        select x9).ToList<SupremeProduct>();
                                }
                            }
                            else if ((num2 == 0x6e2ce0f8) && (group == "jackets"))
                            {
                                source = (from x8 in list
                                    where x8.GroupName == "Jackets"
                                    select x8).ToList<SupremeProduct>();
                            }
                        }
                        else if (num2 != 0x3a0ffee1)
                        {
                            if ((num2 == 0x3baaf4e6) && (group == "bags"))
                            {
                                source = (from x3 in list
                                    where x3.GroupName == "Bags"
                                    select x3).ToList<SupremeProduct>();
                            }
                        }
                        else if (group == "skate")
                        {
                            source = (from x5 in list
                                where x5.GroupName == "Skate"
                                select x5).ToList<SupremeProduct>();
                        }
                    }
                    else if (num2 == 0x13254bc4)
                    {
                        if (group == "all")
                        {
                            foreach (SupremeProduct prod in list)
                            {
                                if (!source.Any<SupremeProduct>(x12 => (x12.Id == prod.Id)))
                                {
                                    source.Add(prod);
                                }
                            }
                        }
                    }
                    else if (num2 == 0x28999611)
                    {
                        if (group == "new")
                        {
                            source = (from x1 in list
                                where x1.GroupName == "new"
                                select x1).ToList<SupremeProduct>();
                        }
                    }
                    else if ((num2 == 0x29dcae48) && (group == "topsweaters"))
                    {
                        source = (from x6 in list
                            where x6.GroupName == "Tops/Sweaters"
                            select x6).ToList<SupremeProduct>();
                    }
                }
                else if (num2 > 0xaa2f7652)
                {
                    if (num2 > 0xb3068a53)
                    {
                        if (num2 == 0xd3d24ba3)
                        {
                            if (group == "hats")
                            {
                                source = (from x7 in list
                                    where x7.GroupName == "Hats"
                                    select x7).ToList<SupremeProduct>();
                            }
                        }
                        else if ((num2 == 0xde2337e1) && (group == "pants"))
                        {
                            source = (from x4 in list
                                where x4.GroupName == "Pants"
                                select x4).ToList<SupremeProduct>();
                        }
                    }
                    else if (num2 == 0xb17228e2)
                    {
                        if (group == "tshirts")
                        {
                            source = (from x11 in list
                                where x11.GroupName == "T-Shirts"
                                select x11).ToList<SupremeProduct>();
                        }
                    }
                    else if ((num2 == 0xb3068a53) && (group == "shoes"))
                    {
                        source = (from x4 in list
                            where x4.GroupName == "Shoes"
                            select x4).ToList<SupremeProduct>();
                    }
                }
                else
                {
                    switch (num2)
                    {
                        case 0x78b505da:
                            if (group == "sweatshirts")
                            {
                                source = (from x10 in list
                                    where x10.GroupName == "Sweatshirts"
                                    select x10).ToList<SupremeProduct>();
                            }
                            goto Label_0A06;

                        case 0xa891cc55:
                            if (group == "accessories")
                            {
                                source = (from x2 in list
                                    where x2.GroupName == "Accessories"
                                    select x2).ToList<SupremeProduct>();
                            }
                            goto Label_0A06;
                    }
                    if ((num2 == 0xaa2f7652) && (group == "shorts"))
                    {
                        source = (from x9 in list
                            where x9.GroupName == "Shorts"
                            select x9).ToList<SupremeProduct>();
                    }
                }
            Label_0A06:
                list3 = new List<SupremeProduct>();
                List<string> list4 = new List<string>();
                foreach (string str5 in this._task.Keywords)
                {
                    if (!str5.Contains("=="))
                    {
                        list4.Add(str5);
                    }
                    else
                    {
                        list4.Add(EncryptorAes2.Decrypt(str5));
                    }
                }
                using (List<string>.Enumerator enumerator5 = list4.GetEnumerator())
                {
                    while (enumerator5.MoveNext())
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = enumerator5.Current.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (SupremeProduct product in source)
                        {
                            <>c__DisplayClass34_2 class_2;
                            bool flag = true;
                            string str6 = product.Name + " " + product.SpecialName;
                            string[] strArray2 = strArray;
                            int index = 0;
                            goto Label_0BAE;
                        Label_0B18:
                            class_2 = new <>c__DisplayClass34_2();
                            class_2.kw = strArray2[index];
                            if (class_2.kw[0] == '+')
                            {
                                char[] chArray2 = new char[] { ' ' };
                                string[] strArray3 = str6.ToUpperInvariant().Split(chArray2);
                                for (int j = 0; j < strArray3.Length; j++)
                                {
                                    strArray3[j] = strArray3[j].Trim();
                                }
                                if (strArray3.Any<string>(new Func<string, bool>(class_2.<SearchBrowserless>b__16)))
                                {
                                    goto Label_0BA8;
                                }
                                goto Label_0BBB;
                            }
                            if (flag && !str6.ToUpperInvariant().Contains(class_2.kw))
                            {
                                goto Label_0BC0;
                            }
                        Label_0BA8:
                            index++;
                        Label_0BAE:
                            if (index >= strArray2.Length)
                            {
                                goto Label_0BC3;
                            }
                            goto Label_0B18;
                        Label_0BBB:
                            flag = false;
                            goto Label_0BC3;
                        Label_0BC0:
                            flag = false;
                        Label_0BC3:
                            if (flag && !list3.Any<SupremeProduct>(x => (x.Id == product.Id)))
                            {
                                list3.Add(product);
                            }
                        }
                        foreach (SupremeProduct product1 in source)
                        {
                            bool flag2 = true;
                            string str8 = product1.Name.ToUpperInvariant().Replace("A", "").Replace("O", "").Replace("I", "").Replace("E", "").Replace("U", "");
                            string str9 = product1.Name + " " + product1.SpecialName;
                            foreach (string str7 in strArray)
                            {
                                if (str7[0] != '+')
                                {
                                    if (!str8.ToUpperInvariant().Contains(str7))
                                    {
                                        goto Label_0D0A;
                                    }
                                }
                                else if (!str9.ToUpperInvariant().StartsWith(str7.Substring(1)))
                                {
                                    goto Label_0D0F;
                                }
                            }
                            goto Label_0D12;
                        Label_0D0A:
                            flag2 = false;
                            goto Label_0D12;
                        Label_0D0F:
                            flag2 = false;
                        Label_0D12:
                            if (flag2 && !list3.Any<SupremeProduct>(x => (x.Id == product1.Id)))
                            {
                                list3.Add(product1);
                            }
                        }
                    }
                }
                if (list3.Count == 0)
                {
                    return false;
                }
                List<SupremeProduct> list5 = new List<SupremeProduct>();
                if (this._task.NegativeKeywords.Any<string>())
                {
                    foreach (SupremeProduct product in list3)
                    {
                        bool flag4 = true;
                        SupremeProduct tmpProd = product;
                        using (IEnumerator<string> enumerator4 = this._task.NegativeKeywords.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                char[] separator = new char[] { ' ' };
                                string[] strArray4 = enumerator4.Current.Split(separator);
                                for (int i = 0; i < strArray4.Length; i++)
                                {
                                    strArray4[i] = strArray4[i].Trim().ToUpperInvariant();
                                }
                                foreach (string str10 in strArray4)
                                {
                                    if (product.Name.ToUpperInvariant().Contains(str10))
                                    {
                                        goto Label_0E54;
                                    }
                                }
                                continue;
                            Label_0E54:
                                flag4 = false;
                            }
                        }
                        if (flag4 && !list5.Any<SupremeProduct>(x => (x.Id == tmpProd.Id)))
                        {
                            list5.Add(tmpProd);
                        }
                    }
                    if (list5.Count == 0)
                    {
                        return false;
                    }
                }
                else
                {
                    list5 = list3;
                }
                using (List<SupremeProduct>.Enumerator enumerator3 = list5.GetEnumerator())
                {
                    char[] chArray6;
                    List<Tuple<string, string, string, string, string>>.Enumerator enumerator10;
                    Tuple<string, string, string, string, string> tuple2;
                    Tuple<string, string, string, string, string> tuple4;
                Label_0ED4:
                    if (!enumerator3.MoveNext())
                    {
                        goto Label_2267;
                    }
                    SupremeProduct current = enumerator3.Current;
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", current.RealName);
                    request = (HttpWebRequest) WebRequest.Create($"http://www.supremenewyork.com/shop/{current.Id}.json");
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.KeepAlive = true;
                    request.CookieContainer = this._runner.Cookies;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    request.Accept = "*/*";
                    request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    request.ServicePoint.Expect100Continue = false;
                    str = "";
                    using (WebResponse response3 = request.GetResponse())
                    {
                        using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                        {
                            str = reader3.ReadToEnd();
                        }
                    }
                    obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                    List<SupremeStyle> list6 = new List<SupremeStyle>();
                    if (<>o__34.<>p__25 == null)
                    {
                        <>o__34.<>p__25 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Supreme)));
                    }
                    if (<>o__34.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__34.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "styles", typeof(Supreme), argumentInfo));
                    }
                    foreach (object obj3 in <>o__34.<>p__25.Target(<>o__34.<>p__25, <>o__34.<>p__2.Target(<>o__34.<>p__2, obj2)))
                    {
                        SupremeStyle style = new SupremeStyle();
                        if (<>o__34.<>p__6 == null)
                        {
                            <>o__34.<>p__6 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                        }
                        if (<>o__34.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Supreme), argumentInfo));
                        }
                        if (<>o__34.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                        }
                        if (<>o__34.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__34.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                        }
                        style.Id = <>o__34.<>p__6.Target(<>o__34.<>p__6, <>o__34.<>p__5.Target(<>o__34.<>p__5, <>o__34.<>p__4.Target(<>o__34.<>p__4, <>o__34.<>p__3.Target(<>o__34.<>p__3, obj3, "id"))));
                        if (<>o__34.<>p__9 == null)
                        {
                            <>o__34.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                        }
                        if (<>o__34.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                        }
                        if (<>o__34.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__34.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                        }
                        style.Name = <>o__34.<>p__9.Target(<>o__34.<>p__9, <>o__34.<>p__8.Target(<>o__34.<>p__8, <>o__34.<>p__7.Target(<>o__34.<>p__7, obj3, "name")));
                        if (<>o__34.<>p__12 == null)
                        {
                            <>o__34.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                        }
                        if (<>o__34.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                        }
                        if (<>o__34.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__34.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                        }
                        style.ImagUrl = <>o__34.<>p__12.Target(<>o__34.<>p__12, <>o__34.<>p__11.Target(<>o__34.<>p__11, <>o__34.<>p__10.Target(<>o__34.<>p__10, obj3, "image_url")));
                        SupremeStyle item = style;
                        item.Name = item.Name.RemoveSpecialCharacters();
                        if (<>o__34.<>p__24 == null)
                        {
                            <>o__34.<>p__24 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Supreme)));
                        }
                        if (<>o__34.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "sizes", typeof(Supreme), argumentInfo));
                        }
                        foreach (object obj4 in <>o__34.<>p__24.Target(<>o__34.<>p__24, <>o__34.<>p__13.Target(<>o__34.<>p__13, obj3)))
                        {
                            if (<>o__34.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__34.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                            }
                            if (<>o__34.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__34.<>p__16 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Supreme), argumentInfo));
                            }
                            if (<>o__34.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__34.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                            }
                            if (<>o__34.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__34.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                            }
                            if (<>o__34.<>p__17.Target(<>o__34.<>p__17, <>o__34.<>p__16.Target(<>o__34.<>p__16, <>o__34.<>p__15.Target(<>o__34.<>p__15, <>o__34.<>p__14.Target(<>o__34.<>p__14, obj4, "stock_level")), 0)))
                            {
                                if (<>o__34.<>p__23 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__23 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                                }
                                if (<>o__34.<>p__19 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                                }
                                if (<>o__34.<>p__18 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__34.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                                }
                                if (<>o__34.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Supreme), argumentInfo));
                                }
                                if (<>o__34.<>p__21 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                                }
                                if (<>o__34.<>p__20 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__34.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                                }
                                item.Sizes.Add(<>o__34.<>p__23.Target(<>o__34.<>p__23, typeof(KeyValuePair<string, string>), <>o__34.<>p__19.Target(<>o__34.<>p__19, <>o__34.<>p__18.Target(<>o__34.<>p__18, obj4, "name")), <>o__34.<>p__22.Target(<>o__34.<>p__22, <>o__34.<>p__21.Target(<>o__34.<>p__21, <>o__34.<>p__20.Target(<>o__34.<>p__20, obj4, "id")))));
                            }
                        }
                        if (item.Sizes.Count > 0)
                        {
                            list6.Add(item);
                        }
                    }
                    if (list6.Count == 0)
                    {
                        goto Label_0ED4;
                    }
                    if (this._task.PriceCheck)
                    {
                        string price = current.Price;
                        double num7 = double.Parse(price.Substring(0, price.Length - 2), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num7 < this._task.MinimumPrice) || (num7 > this._task.MaximumPrice))
                        {
                            goto Label_0ED4;
                        }
                    }
                    this._id = current.Id;
                    if (string.IsNullOrEmpty(this._task.Color))
                    {
                        Product product3 = new Product {
                            ProductTitle = current.Name,
                            Link = current.Name,
                            Price = current.Price.Substring(0, current.Price.Length - 2)
                        };
                        this._runner.Product = product3;
                        foreach (SupremeStyle style3 in list6)
                        {
                            foreach (KeyValuePair<string, string> pair in style3.Sizes)
                            {
                                this._runner.Product.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair.Key, pair.Value, style3.Id, style3.ImagUrl, style3.Name));
                            }
                        }
                        if (!this._task.RandomSize)
                        {
                            char[] separator = new char[] { '#' };
                            string[] strArray5 = this._task.Size.Split(separator);
                            for (int j = 0; j < strArray5.Length; j++)
                            {
                                strArray5[j] = strArray5[j].Trim().ToUpperInvariant();
                            }
                            foreach (string sz in strArray5)
                            {
                                if (this._runner.PickedSize.HasValue)
                                {
                                    break;
                                }
                                using (enumerator10 = this._runner.Product.AvailableSizesSupreme.GetEnumerator())
                                {
                                    Tuple<string, string, string, string, string> tuple;
                                    goto Label_1A93;
                                Label_1A1D:
                                    tuple = enumerator10.Current;
                                    char[] chArray5 = new char[] { ' ' };
                                    string[] strArray6 = tuple.Item1.Split(chArray5);
                                    for (int k = 0; k < strArray6.Length; k++)
                                    {
                                        strArray6[k] = strArray6[k].Trim().ToUpperInvariant();
                                    }
                                    if (strArray6.Any<string>(f => f == sz))
                                    {
                                        goto Label_1AA4;
                                    }
                                Label_1A93:
                                    if (!enumerator10.MoveNext())
                                    {
                                        continue;
                                    }
                                    goto Label_1A1D;
                                Label_1AA4:
                                    this._runner.PickedSize = new KeyValuePair<string, string>(tuple.Item1, tuple.Item2);
                                    this._style = tuple.Item3;
                                    this._task.ImgUrl = "https:" + tuple.Item4;
                                    this._runner.Product.ProductTitle = this._runner.Product.ProductTitle + " / " + tuple.Item5;
                                }
                            }
                            if (this._runner.PickedSize.HasValue)
                            {
                                return true;
                            }
                            if (!this._task.SizePickRandom)
                            {
                                goto Label_0ED4;
                            }
                        }
                        goto Label_20C7;
                    }
                    Product product4 = new Product {
                        ProductTitle = current.Name,
                        Link = current.Name,
                        Price = current.Price.Substring(0, current.Price.Length - 2)
                    };
                    this._runner.Product = product4;
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                    {
                        if (!list6.Any<SupremeStyle>(u => (u.Name.ToUpperInvariant() == this._task.Color.ToUpperInvariant())))
                        {
                            if (!this._task.ColorPickRandom)
                            {
                                goto Label_0ED4;
                            }
                            foreach (SupremeStyle style4 in list6)
                            {
                                foreach (KeyValuePair<string, string> pair2 in style4.Sizes)
                                {
                                    this._runner.Product.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair2.Key, pair2.Value, style4.Id, style4.ImagUrl, style4.Name));
                                }
                            }
                        }
                        else
                        {
                            foreach (SupremeStyle style5 in from u in list6
                                where u.Name.ToUpperInvariant() == this._task.Color.ToUpperInvariant()
                                select u)
                            {
                                foreach (KeyValuePair<string, string> pair3 in style5.Sizes)
                                {
                                    this._runner.Product.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair3.Key, pair3.Value, style5.Id, style5.ImagUrl, style5.Name));
                                }
                            }
                        }
                    }
                    else if (!list6.Any<SupremeStyle>(u => u.Name.ToUpperInvariant().Contains(this._task.Color.ToUpperInvariant())))
                    {
                        if (!this._task.ColorPickRandom)
                        {
                            goto Label_0ED4;
                        }
                        foreach (SupremeStyle style6 in list6)
                        {
                            foreach (KeyValuePair<string, string> pair4 in style6.Sizes)
                            {
                                this._runner.Product.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair4.Key, pair4.Value, style6.Id, style6.ImagUrl, style6.Name));
                            }
                        }
                    }
                    else
                    {
                        foreach (SupremeStyle style7 in from u in list6
                            where u.Name.ToUpperInvariant().Contains(this._task.Color.ToUpperInvariant())
                            select u)
                        {
                            foreach (KeyValuePair<string, string> pair5 in style7.Sizes)
                            {
                                this._runner.Product.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair5.Key, pair5.Value, style7.Id, style7.ImagUrl, style7.Name));
                            }
                        }
                    }
                    goto Label_20AA;
                Label_1EC7:
                    chArray6 = new char[] { '#' };
                    string[] strArray7 = this._task.Size.Split(chArray6);
                    for (int i = 0; i < strArray7.Length; i++)
                    {
                        strArray7[i] = strArray7[i].Trim().ToUpperInvariant();
                    }
                    foreach (string text1 in strArray7)
                    {
                        if (this._runner.PickedSize.HasValue)
                        {
                            break;
                        }
                        using (enumerator10 = this._runner.Product.AvailableSizesSupreme.GetEnumerator())
                        {
                            Tuple<string, string, string, string, string> tuple3;
                            goto Label_1FCF;
                        Label_1F59:
                            tuple3 = enumerator10.Current;
                            char[] separator = new char[] { ' ' };
                            string[] strArray8 = tuple3.Item1.Split(separator);
                            for (int j = 0; j < strArray8.Length; j++)
                            {
                                strArray8[j] = strArray8[j].Trim().ToUpperInvariant();
                            }
                            if (strArray8.Any<string>(f => f == text1))
                            {
                                goto Label_1FE0;
                            }
                        Label_1FCF:
                            if (!enumerator10.MoveNext())
                            {
                                continue;
                            }
                            goto Label_1F59;
                        Label_1FE0:
                            this._runner.PickedSize = new KeyValuePair<string, string>(tuple3.Item1, tuple3.Item2);
                            this._task.ImgUrl = "https:" + tuple3.Item4;
                            this._style = tuple3.Item3;
                            this._runner.Product.ProductTitle = this._runner.Product.ProductTitle + " / " + tuple3.Item5;
                        }
                    }
                    if (this._runner.PickedSize.HasValue)
                    {
                        return true;
                    }
                    if (!this._task.SizePickRandom)
                    {
                        goto Label_0ED4;
                    }
                    goto Label_2193;
                Label_20AA:
                    if (this._task.RandomSize)
                    {
                        goto Label_2193;
                    }
                    goto Label_1EC7;
                Label_20C7:
                    tuple2 = this._runner.Product.AvailableSizesSupreme[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizesSupreme.Count)];
                    this._runner.PickedSize = new KeyValuePair<string, string>(tuple2.Item1, tuple2.Item2);
                    this._style = tuple2.Item3;
                    this._task.ImgUrl = "https:" + tuple2.Item4;
                    this._runner.Product.ProductTitle = this._runner.Product.ProductTitle + " / " + tuple2.Item5;
                    return true;
                Label_2193:
                    tuple4 = this._runner.Product.AvailableSizesSupreme[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizesSupreme.Count)];
                    this._runner.PickedSize = new KeyValuePair<string, string>(tuple4.Item1, tuple4.Item2);
                    this._style = tuple4.Item3;
                    this._task.ImgUrl = "https:" + tuple4.Item4;
                    this._runner.Product.ProductTitle = this._runner.Product.ProductTitle + " / " + tuple4.Item5;
                    return true;
                }
            Label_2267:
                while (this._isFallbackSearch)
                {
                }
                if (this._fallbackProduct != null)
                {
                    this._runner.Product = this._fallbackProduct;
                    this._runner.PickedSize = this._fallbackPickedSize;
                    return true;
                }
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", "");
                return false;
            }
        }

        public void SearchFallback()
        {
            try
            {
                int num3;
                string[] strArray2;
                this._isFallbackSearch = true;
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                type.GetMembers();
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(((List<KeyValuePair<string, string>>) type.GetField("SUPREME_LINKS", BindingFlags.Public | BindingFlags.Static).GetValue(null)).First<KeyValuePair<string, string>>(x => (x.Key == this._task.Group)).Value);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Accept = "*/*";
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                string html = "";
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                HtmlDocument document1 = new HtmlDocument();
                document1.LoadHtml(html);
                List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>();
                foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("div")
                    where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "inner-article")
                    select x)
                {
                    string str4 = node.Descendants("h1").First<HtmlNode>().InnerText.RemoveSpecialCharacters();
                    string innerText = node.Descendants("p").First<HtmlNode>().InnerText;
                    string str3 = "https://www.supremenewyork.com" + node.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                    list.Add(new Tuple<string, string, string>(str4, innerText, str3));
                }
                List<Tuple<string, string, string>> source = new List<Tuple<string, string, string>>();
                List<string> list3 = new List<string>();
                foreach (string str5 in this._task.Keywords)
                {
                    list3.Add(str5);
                }
                using (List<string>.Enumerator enumerator3 = list3.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = enumerator3.Current.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (Tuple<string, string, string> product in list)
                        {
                            string str6;
                            bool flag = true;
                            strArray2 = strArray;
                            num3 = 0;
                            goto Label_02BD;
                        Label_0296:
                            str6 = strArray2[num3];
                            if (!product.Item1.ToUpperInvariant().Contains(str6))
                            {
                                goto Label_02C7;
                            }
                            num3++;
                        Label_02BD:
                            if (num3 >= strArray2.Length)
                            {
                                goto Label_02CA;
                            }
                            goto Label_0296;
                        Label_02C7:
                            flag = false;
                        Label_02CA:
                            if (flag && !source.Any<Tuple<string, string, string>>(x => (x.Item3 == product.Item3)))
                            {
                                source.Add(product);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this._task.Color))
                {
                    List<Tuple<string, string, string>> list4 = new List<Tuple<string, string, string>>();
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                    {
                        foreach (Tuple<string, string, string> tuple in source)
                        {
                            if (tuple.Item2.ToUpperInvariant() == this._task.Color.ToUpperInvariant())
                            {
                                list4.Add(tuple);
                            }
                        }
                    }
                    else
                    {
                        foreach (Tuple<string, string, string> tuple2 in source)
                        {
                            if (tuple2.Item2.ToUpperInvariant().Contains(this._task.Color.ToUpperInvariant()))
                            {
                                list4.Add(tuple2);
                            }
                        }
                    }
                    if ((list4.Count > 0) || !this._task.ColorPickRandom)
                    {
                        source = list4;
                    }
                }
                if (source.Count == 0)
                {
                    this._isFallbackSearch = false;
                    return;
                }
                List<Tuple<string, string, string>> list5 = new List<Tuple<string, string, string>>();
                if (this._task.NegativeKeywords.Any<string>())
                {
                    foreach (Tuple<string, string, string> tuple3 in source)
                    {
                        bool flag2 = true;
                        Tuple<string, string, string> tmpProd = tuple3;
                        using (IEnumerator<string> enumerator2 = this._task.NegativeKeywords.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                string str7;
                                char[] separator = new char[] { ' ' };
                                string[] strArray3 = enumerator2.Current.Split(separator);
                                for (int i = 0; i < strArray3.Length; i++)
                                {
                                    strArray3[i] = strArray3[i].Trim().ToUpperInvariant();
                                }
                                strArray2 = strArray3;
                                num3 = 0;
                                goto Label_04FE;
                            Label_04DC:
                                str7 = strArray2[num3];
                                if (tuple3.Item1.ToUpperInvariant().Contains(str7))
                                {
                                    goto Label_0508;
                                }
                                num3++;
                            Label_04FE:
                                if (num3 >= strArray2.Length)
                                {
                                    continue;
                                }
                                goto Label_04DC;
                            Label_0508:
                                flag2 = false;
                            }
                        }
                        if (flag2 && !list5.Any<Tuple<string, string, string>>(x => (x.Item3 == tmpProd.Item3)))
                        {
                            list5.Add(tmpProd);
                        }
                    }
                    if (list5.Count == 0)
                    {
                        this._isFallbackSearch = false;
                        return;
                    }
                }
                else
                {
                    list5 = source;
                }
                using (List<Tuple<string, string, string>>.Enumerator enumerator4 = list5.GetEnumerator())
                {
                    while (enumerator4.MoveNext())
                    {
                        Tuple<string, string, string> current = enumerator4.Current;
                        if (this.DirectLinkFallback(current.Item3))
                        {
                            goto Label_05AE;
                        }
                    }
                    goto Label_05C5;
                Label_05AE:
                    this._isFallbackSearch = false;
                    return;
                }
            Label_05C5:
                this._isFallbackSearch = false;
            }
            catch (Exception)
            {
                this._isFallbackSearch = false;
            }
        }

        public Product SearchMulti(TaskObject task)
        {
            Product product;
            try
            {
                List<SupremeProduct> list3;
                int num4;
                string[] strArray2;
                object obj2 = this._stockJson;
                if (<>o__38.<>p__1 == null)
                {
                    <>o__38.<>p__1 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Newtonsoft.Json.Linq.JObject), typeof(Supreme)));
                }
                if (<>o__38.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "products_and_categories", typeof(Supreme), argumentInfo));
                }
                List<SupremeProduct> list = new List<SupremeProduct>();
                foreach (Newtonsoft.Json.Linq.JToken token in <>o__38.<>p__1.Target(<>o__38.<>p__1, <>o__38.<>p__0.Target(<>o__38.<>p__0, obj2)).Children())
                {
                    foreach (Newtonsoft.Json.Linq.JToken token2 in ((Newtonsoft.Json.Linq.JArray) token.First).Children())
                    {
                        string str = token.ToString();
                        str = str.Substring(str.IndexOf("\"") + 1);
                        str = str.Substring(0, str.IndexOf("\""));
                        string str2 = token2["image_url"].ToString();
                        str2 = str2.Substring(str2.LastIndexOf("/") + 1);
                        str2 = str2.Substring(0, str2.LastIndexOf("."));
                        SupremeProduct item = new SupremeProduct {
                            GroupName = str,
                            RealName = token2["name"].ToString(),
                            SpecialName = str2,
                            Id = token2["id"].ToString(),
                            Name = token2["name"].ToString().Replace("™", "").Replace("\x00d0\x00b0", "a").Replace("\x00d0\x00b5", "e").Replace("\x00d0…", "S").Replace("\x00ce’", "B").Replace("Β", "B").Replace("а", "a").Replace("е", "e").Replace("Ѕ", "S").Replace("/", " "),
                            ImageUrl = token2["image_url"].ToString(),
                            ImageUrlHi = token2["image_url_hi"].ToString(),
                            Price = token2["price"].ToString(),
                            SalePrice = token2["sale_price"].ToString(),
                            NewItem = token2["new_item"].ToString(),
                            Position = token2["position"].ToString(),
                            CategoryName = token2["category_name"].ToString()
                        };
                        list.Add(item);
                    }
                }
                foreach (SupremeProduct local1 in list)
                {
                    local1.Name = local1.Name.RemoveSpecialCharacters();
                }
                List<SupremeProduct> source = new List<SupremeProduct>();
                string group = task.Group;
                uint num2 = <PrivateImplementationDetails>.ComputeStringHash(group);
                if (num2 > 0x4ef8af50)
                {
                    if (num2 > 0xa891cc55)
                    {
                        if (num2 > 0xb3068a53)
                        {
                            if (num2 == 0xd3d24ba3)
                            {
                                if (group == "hats")
                                {
                                    source = (from x7 in list
                                        where x7.GroupName == "Hats"
                                        select x7).ToList<SupremeProduct>();
                                }
                            }
                            else if ((num2 == 0xde2337e1) && (group == "pants"))
                            {
                                source = (from x4 in list
                                    where x4.GroupName == "Pants"
                                    select x4).ToList<SupremeProduct>();
                            }
                        }
                        else if (num2 == 0xb17228e2)
                        {
                            if (group == "tshirts")
                            {
                                source = (from x11 in list
                                    where x11.GroupName == "T-Shirts"
                                    select x11).ToList<SupremeProduct>();
                            }
                        }
                        else if ((num2 == 0xb3068a53) && (group == "shoes"))
                        {
                            source = (from x4 in list
                                where x4.GroupName == "Shoes"
                                select x4).ToList<SupremeProduct>();
                        }
                    }
                    else
                    {
                        switch (num2)
                        {
                            case 0x6e2ce0f8:
                                if (group == "jackets")
                                {
                                    source = (from x8 in list
                                        where x8.GroupName == "Jackets"
                                        select x8).ToList<SupremeProduct>();
                                }
                                goto Label_07F7;

                            case 0x78b505da:
                                if (group == "sweatshirts")
                                {
                                    source = (from x10 in list
                                        where x10.GroupName == "Sweatshirts"
                                        select x10).ToList<SupremeProduct>();
                                }
                                goto Label_07F7;
                        }
                        if ((num2 == 0xa891cc55) && (group == "accessories"))
                        {
                            source = (from x2 in list
                                where x2.GroupName == "Accessories"
                                select x2).ToList<SupremeProduct>();
                        }
                    }
                }
                else if (num2 > 0x29dcae48)
                {
                    switch (num2)
                    {
                        case 0x3a0ffee1:
                            if (group == "skate")
                            {
                                source = (from x5 in list
                                    where x5.GroupName == "Skate"
                                    select x5).ToList<SupremeProduct>();
                            }
                            goto Label_07F7;

                        case 0x3baaf4e6:
                            if (group == "bags")
                            {
                                source = (from x3 in list
                                    where x3.GroupName == "Bags"
                                    select x3).ToList<SupremeProduct>();
                            }
                            goto Label_07F7;
                    }
                    if ((num2 == 0x4ef8af50) && (group == "shirts"))
                    {
                        source = (from x9 in list
                            where x9.GroupName == "Shirts"
                            select x9).ToList<SupremeProduct>();
                    }
                }
                else
                {
                    switch (num2)
                    {
                        case 0x13254bc4:
                            if (group == "all")
                            {
                                foreach (SupremeProduct prod in list)
                                {
                                    if (!source.Any<SupremeProduct>(x12 => (x12.Id == prod.Id)))
                                    {
                                        source.Add(prod);
                                    }
                                }
                            }
                            goto Label_07F7;

                        case 0x28999611:
                            if (group == "new")
                            {
                                source = (from x1 in list
                                    where x1.GroupName == "new"
                                    select x1).ToList<SupremeProduct>();
                            }
                            goto Label_07F7;
                    }
                    if ((num2 == 0x29dcae48) && (group == "topsweaters"))
                    {
                        source = (from x6 in list
                            where x6.GroupName == "Tops/Sweaters"
                            select x6).ToList<SupremeProduct>();
                    }
                }
            Label_07F7:
                list3 = new List<SupremeProduct>();
                List<string> list4 = new List<string>();
                foreach (string str4 in task.Keywords)
                {
                    if (!str4.Contains("=="))
                    {
                        list4.Add(str4);
                    }
                    else
                    {
                        list4.Add(EncryptorAes2.Decrypt(str4));
                    }
                }
                foreach (string str5 in task.Keywords)
                {
                    list4.Add(str5.Replace("a", "").Replace("A", "").Replace("u", "").Replace("U", "").Replace("o", "").Replace("O", "").Replace("e", "").Replace("E", "").Replace("i", "").Replace("I", ""));
                }
                using (List<string>.Enumerator enumerator5 = list4.GetEnumerator())
                {
                    while (enumerator5.MoveNext())
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = enumerator5.Current.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (SupremeProduct product in source)
                        {
                            bool flag = true;
                            string str7 = product.Name + " " + product.SpecialName;
                            strArray2 = strArray;
                            num4 = 0;
                            while (num4 < strArray2.Length)
                            {
                                string str6 = strArray2[num4];
                                if (!str7.ToUpperInvariant().Contains(str6))
                                {
                                    goto Label_0A11;
                                }
                                num4++;
                            }
                            goto Label_0A14;
                        Label_0A11:
                            flag = false;
                        Label_0A14:
                            if (flag && !list3.Any<SupremeProduct>(x => (x.Id == product.Id)))
                            {
                                list3.Add(product);
                            }
                        }
                    }
                }
                if (list3.Count == 0)
                {
                    return null;
                }
                List<SupremeProduct> list5 = new List<SupremeProduct>();
                if (task.NegativeKeywords.Any<string>())
                {
                    foreach (SupremeProduct product2 in list3)
                    {
                        bool flag2 = true;
                        SupremeProduct tmpProd = product2;
                        using (IEnumerator<string> enumerator4 = task.NegativeKeywords.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                string str8;
                                char[] separator = new char[] { ' ' };
                                string[] strArray3 = enumerator4.Current.Split(separator);
                                for (int i = 0; i < strArray3.Length; i++)
                                {
                                    strArray3[i] = strArray3[i].Trim().ToUpperInvariant();
                                }
                                strArray2 = strArray3;
                                num4 = 0;
                                goto Label_0B4C;
                            Label_0B2A:
                                str8 = strArray2[num4];
                                if (product2.Name.ToUpperInvariant().Contains(str8))
                                {
                                    goto Label_0B56;
                                }
                                num4++;
                            Label_0B4C:
                                if (num4 >= strArray2.Length)
                                {
                                    continue;
                                }
                                goto Label_0B2A;
                            Label_0B56:
                                flag2 = false;
                            }
                        }
                        if (flag2 && !list5.Any<SupremeProduct>(x => (x.Id == tmpProd.Id)))
                        {
                            list5.Add(tmpProd);
                        }
                    }
                    if (list5.Count == 0)
                    {
                        return null;
                    }
                }
                else
                {
                    list5 = list3;
                }
                using (List<SupremeProduct>.Enumerator enumerator3 = list5.GetEnumerator())
                {
                    char[] chArray5;
                    Product product4;
                    List<Tuple<string, string, string, string, string>>.Enumerator enumerator10;
                    Tuple<string, string, string, string, string> tuple2;
                    Tuple<string, string, string, string, string> tuple4;
                Label_0BD6:
                    if (!enumerator3.MoveNext())
                    {
                        goto Label_1EEA;
                    }
                    SupremeProduct current = enumerator3.Current;
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create($"http://www.supremenewyork.com/shop/{current.Id}.json");
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.KeepAlive = true;
                    request.CookieContainer = this._runner.Cookies;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.Accept = "*/*";
                    request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    request.Referer = "http://www.supremenewyork.com/mobile";
                    request.ServicePoint.Expect100Continue = false;
                    string str9 = "";
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            str9 = reader.ReadToEnd();
                        }
                    }
                    obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str9);
                    List<SupremeStyle> list6 = new List<SupremeStyle>();
                    if (<>o__38.<>p__25 == null)
                    {
                        <>o__38.<>p__25 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Supreme)));
                    }
                    if (<>o__38.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "styles", typeof(Supreme), argumentInfo));
                    }
                    foreach (object obj3 in <>o__38.<>p__25.Target(<>o__38.<>p__25, <>o__38.<>p__2.Target(<>o__38.<>p__2, obj2)))
                    {
                        SupremeStyle style = new SupremeStyle();
                        if (<>o__38.<>p__6 == null)
                        {
                            <>o__38.<>p__6 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                        }
                        if (<>o__38.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Supreme), argumentInfo));
                        }
                        if (<>o__38.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                        }
                        if (<>o__38.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                        }
                        style.Id = <>o__38.<>p__6.Target(<>o__38.<>p__6, <>o__38.<>p__5.Target(<>o__38.<>p__5, <>o__38.<>p__4.Target(<>o__38.<>p__4, <>o__38.<>p__3.Target(<>o__38.<>p__3, obj3, "id"))));
                        if (<>o__38.<>p__9 == null)
                        {
                            <>o__38.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                        }
                        if (<>o__38.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                        }
                        if (<>o__38.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                        }
                        style.Name = <>o__38.<>p__9.Target(<>o__38.<>p__9, <>o__38.<>p__8.Target(<>o__38.<>p__8, <>o__38.<>p__7.Target(<>o__38.<>p__7, obj3, "name")));
                        if (<>o__38.<>p__12 == null)
                        {
                            <>o__38.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Supreme)));
                        }
                        if (<>o__38.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                        }
                        if (<>o__38.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                        }
                        style.ImagUrl = <>o__38.<>p__12.Target(<>o__38.<>p__12, <>o__38.<>p__11.Target(<>o__38.<>p__11, <>o__38.<>p__10.Target(<>o__38.<>p__10, obj3, "image_url")));
                        SupremeStyle item = style;
                        item.Name = item.Name.RemoveSpecialCharacters();
                        if (<>o__38.<>p__24 == null)
                        {
                            <>o__38.<>p__24 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Supreme)));
                        }
                        if (<>o__38.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "sizes", typeof(Supreme), argumentInfo));
                        }
                        foreach (object obj4 in <>o__38.<>p__24.Target(<>o__38.<>p__24, <>o__38.<>p__13.Target(<>o__38.<>p__13, obj3)))
                        {
                            if (<>o__38.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__38.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Supreme), argumentInfo));
                            }
                            if (<>o__38.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__38.<>p__16 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Supreme), argumentInfo));
                            }
                            if (<>o__38.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__38.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                            }
                            if (<>o__38.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__38.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                            }
                            if (<>o__38.<>p__17.Target(<>o__38.<>p__17, <>o__38.<>p__16.Target(<>o__38.<>p__16, <>o__38.<>p__15.Target(<>o__38.<>p__15, <>o__38.<>p__14.Target(<>o__38.<>p__14, obj4, "stock_level")), 0)))
                            {
                                if (<>o__38.<>p__23 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__23 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                                }
                                if (<>o__38.<>p__19 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                                }
                                if (<>o__38.<>p__18 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                                }
                                if (<>o__38.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Supreme), argumentInfo));
                                }
                                if (<>o__38.<>p__21 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Supreme), argumentInfo));
                                }
                                if (<>o__38.<>p__20 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Supreme), argumentInfo));
                                }
                                item.Sizes.Add(<>o__38.<>p__23.Target(<>o__38.<>p__23, typeof(KeyValuePair<string, string>), <>o__38.<>p__19.Target(<>o__38.<>p__19, <>o__38.<>p__18.Target(<>o__38.<>p__18, obj4, "name")), <>o__38.<>p__22.Target(<>o__38.<>p__22, <>o__38.<>p__21.Target(<>o__38.<>p__21, <>o__38.<>p__20.Target(<>o__38.<>p__20, obj4, "id")))));
                            }
                        }
                        if (item.Sizes.Count > 0)
                        {
                            list6.Add(item);
                        }
                    }
                    if (list6.Count == 0)
                    {
                        goto Label_0BD6;
                    }
                    if (task.PriceCheck)
                    {
                        string price = current.Price;
                        int num6 = int.Parse(price.Substring(0, price.Length - 2), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num6 < task.MinimumPrice) || (num6 > task.MaximumPrice))
                        {
                            goto Label_0BD6;
                        }
                    }
                    KeyValuePair<string, string>? nullable = null;
                    if (string.IsNullOrEmpty(task.Color))
                    {
                        product4 = new Product {
                            ProductTitle = current.Name,
                            Link = current.Name,
                            Price = current.Price.Substring(0, current.Price.Length - 2),
                            SuprimeMultiId = current.Id
                        };
                        foreach (SupremeStyle style3 in list6)
                        {
                            foreach (KeyValuePair<string, string> pair in style3.Sizes)
                            {
                                product4.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair.Key, pair.Value, style3.Id, style3.ImagUrl, style3.Name));
                            }
                        }
                        if (!task.RandomSize)
                        {
                            char[] separator = new char[] { '#' };
                            string[] strArray4 = task.Size.Split(separator);
                            for (int j = 0; j < strArray4.Length; j++)
                            {
                                strArray4[j] = strArray4[j].Trim().ToUpperInvariant();
                            }
                            foreach (string sz in strArray4)
                            {
                                if (nullable.HasValue)
                                {
                                    break;
                                }
                                using (enumerator10 = product4.AvailableSizesSupreme.GetEnumerator())
                                {
                                    Tuple<string, string, string, string, string> tuple;
                                    while (enumerator10.MoveNext())
                                    {
                                        tuple = enumerator10.Current;
                                        char[] chArray4 = new char[] { ' ' };
                                        string[] strArray5 = tuple.Item1.Split(chArray4);
                                        for (int k = 0; k < strArray5.Length; k++)
                                        {
                                            strArray5[k] = strArray5[k].Trim().ToUpperInvariant();
                                        }
                                        if (strArray5.Any<string>(f => f == sz))
                                        {
                                            goto Label_1796;
                                        }
                                    }
                                    continue;
                                Label_1796:
                                    nullable = new KeyValuePair<string, string>(tuple.Item1, tuple.Item2);
                                    product4.SuprimetMultiStyle = tuple.Item3;
                                    task.ImgUrl = "https:" + tuple.Item4;
                                    product4.ProductTitle = product4.ProductTitle + " / " + tuple.Item5;
                                }
                            }
                            if (nullable.HasValue)
                            {
                                return product4;
                            }
                            if (!task.SizePickRandom)
                            {
                                goto Label_0BD6;
                            }
                        }
                        goto Label_1D90;
                    }
                    Product product5 = new Product {
                        ProductTitle = current.Name,
                        Link = current.Name,
                        Price = current.Price.Substring(0, current.Price.Length - 2),
                        SuprimeMultiId = current.Id
                    };
                    if (task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                    {
                        if (!list6.Any<SupremeStyle>(u => (u.Name.ToUpperInvariant() == task.Color.ToUpperInvariant())))
                        {
                            if (!task.ColorPickRandom)
                            {
                                goto Label_0BD6;
                            }
                            foreach (SupremeStyle style4 in list6)
                            {
                                foreach (KeyValuePair<string, string> pair2 in style4.Sizes)
                                {
                                    product5.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair2.Key, pair2.Value, style4.Id, style4.ImagUrl, style4.Name));
                                }
                            }
                        }
                        else
                        {
                            foreach (SupremeStyle style5 in from u in list6
                                where u.Name.ToUpperInvariant() == task.Color.ToUpperInvariant()
                                select u)
                            {
                                foreach (KeyValuePair<string, string> pair3 in style5.Sizes)
                                {
                                    product5.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair3.Key, pair3.Value, style5.Id, style5.ImagUrl, style5.Name));
                                }
                            }
                        }
                    }
                    else if (!list6.Any<SupremeStyle>(u => u.Name.ToUpperInvariant().Contains(task.Color.ToUpperInvariant())))
                    {
                        if (!task.ColorPickRandom)
                        {
                            goto Label_0BD6;
                        }
                        foreach (SupremeStyle style6 in list6)
                        {
                            foreach (KeyValuePair<string, string> pair4 in style6.Sizes)
                            {
                                product5.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair4.Key, pair4.Value, style6.Id, style6.ImagUrl, style6.Name));
                            }
                        }
                    }
                    else
                    {
                        foreach (SupremeStyle style7 in from u in list6
                            where u.Name.ToUpperInvariant().Contains(task.Color.ToUpperInvariant())
                            select u)
                        {
                            foreach (KeyValuePair<string, string> pair5 in style7.Sizes)
                            {
                                product5.AvailableSizesSupreme.Add(new Tuple<string, string, string, string, string>(pair5.Key, pair5.Value, style7.Id, style7.ImagUrl, style7.Name));
                            }
                        }
                    }
                    goto Label_1D72;
                Label_1BBE:
                    chArray5 = new char[] { '#' };
                    string[] strArray6 = task.Size.Split(chArray5);
                    for (int i = 0; i < strArray6.Length; i++)
                    {
                        strArray6[i] = strArray6[i].Trim().ToUpperInvariant();
                    }
                    foreach (string text1 in strArray6)
                    {
                        if (nullable.HasValue)
                        {
                            break;
                        }
                        using (enumerator10 = product5.AvailableSizesSupreme.GetEnumerator())
                        {
                            Tuple<string, string, string, string, string> tuple3;
                            goto Label_1CB4;
                        Label_1C3E:
                            tuple3 = enumerator10.Current;
                            char[] separator = new char[] { ' ' };
                            string[] strArray7 = tuple3.Item1.Split(separator);
                            for (int j = 0; j < strArray7.Length; j++)
                            {
                                strArray7[j] = strArray7[j].Trim().ToUpperInvariant();
                            }
                            if (strArray7.Any<string>(f => f == text1))
                            {
                                goto Label_1CC2;
                            }
                        Label_1CB4:
                            if (!enumerator10.MoveNext())
                            {
                                continue;
                            }
                            goto Label_1C3E;
                        Label_1CC2:
                            nullable = new KeyValuePair<string, string>(tuple3.Item1, tuple3.Item2);
                            task.ImgUrl = "https:" + tuple3.Item4;
                            product5.SuprimetMultiStyle = tuple3.Item3;
                            product5.SuprimetMultiPickedSize = nullable;
                            product5.ProductTitle = product5.ProductTitle + " / " + tuple3.Item5;
                        }
                    }
                    if (nullable.HasValue)
                    {
                        return product5;
                    }
                    if (task.SizePickRandom)
                    {
                        goto Label_1E3B;
                    }
                    goto Label_0BD6;
                Label_1D72:
                    if (task.RandomSize)
                    {
                        goto Label_1E3B;
                    }
                    goto Label_1BBE;
                Label_1D90:
                    tuple2 = product4.AvailableSizesSupreme[this._runner.Rnd.Next(0, product4.AvailableSizesSupreme.Count)];
                    nullable = new KeyValuePair<string, string>(tuple2.Item1, tuple2.Item2);
                    product4.SuprimetMultiStyle = tuple2.Item3;
                    task.ImgUrl = "https:" + tuple2.Item4;
                    product4.ProductTitle = product4.ProductTitle + " / " + tuple2.Item5;
                    product4.SuprimetMultiPickedSize = nullable;
                    return product4;
                Label_1E3B:
                    tuple4 = product5.AvailableSizesSupreme[this._runner.Rnd.Next(0, product5.AvailableSizesSupreme.Count)];
                    nullable = new KeyValuePair<string, string>(tuple4.Item1, tuple4.Item2);
                    product5.SuprimetMultiStyle = tuple4.Item3;
                    task.ImgUrl = "https:" + tuple4.Item4;
                    product5.ProductTitle = product5.ProductTitle + " / " + tuple4.Item5;
                    product5.SuprimetMultiPickedSize = nullable;
                    return product5;
                }
            Label_1EEA:
                product = null;
            }
            catch (ThreadAbortException)
            {
                product = null;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", "");
                product = null;
            }
            return product;
        }

        public void SetClient()
        {
        }

        public List<KeyValuePair<string, Product>> ChildProducts =>
            this._childProducts;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Supreme.<>c <>9;
            public static Func<HtmlNode, bool> <>9__25_0;
            public static Func<HtmlNode, bool> <>9__25_1;
            public static Func<HtmlNode, bool> <>9__25_2;
            public static Func<HtmlNode, bool> <>9__25_3;
            public static Func<HtmlNode, bool> <>9__25_4;
            public static Func<HtmlNode, bool> <>9__25_5;
            public static Func<HtmlNode, bool> <>9__25_6;
            public static Func<HtmlNode, bool> <>9__25_15;
            public static Func<HtmlNode, bool> <>9__25_7;
            public static Func<HtmlNode, bool> <>9__25_16;
            public static Func<HtmlNode, bool> <>9__25_8;
            public static Func<HtmlNode, bool> <>9__25_17;
            public static Func<HtmlNode, bool> <>9__25_9;
            public static Func<HtmlNode, bool> <>9__25_18;
            public static Func<HtmlNode, bool> <>9__25_10;
            public static Func<HtmlNode, bool> <>9__25_11;
            public static Func<HtmlNode, bool> <>9__25_12;
            public static Func<HtmlNode, bool> <>9__25_13;
            public static Func<HtmlNode, bool> <>9__25_14;
            public static Func<HtmlNode, bool> <>9__26_0;
            public static Func<HtmlNode, bool> <>9__26_1;
            public static Func<HtmlNode, bool> <>9__26_2;
            public static Func<HtmlNode, bool> <>9__26_3;
            public static Func<HtmlNode, bool> <>9__26_4;
            public static Func<HtmlNode, bool> <>9__26_5;
            public static Func<HtmlNode, bool> <>9__26_6;
            public static Func<HtmlNode, bool> <>9__26_10;
            public static Func<HtmlNode, bool> <>9__26_7;
            public static Func<HtmlNode, bool> <>9__26_11;
            public static Func<HtmlNode, bool> <>9__26_8;
            public static Func<HtmlNode, bool> <>9__26_12;
            public static Func<HtmlNode, bool> <>9__26_9;
            public static Func<HtmlNode, bool> <>9__26_13;
            public static Func<HtmlNode, bool> <>9__27_0;
            public static Func<HtmlNode, bool> <>9__27_1;
            public static Func<HtmlNode, bool> <>9__27_2;
            public static Func<HtmlNode, bool> <>9__27_3;
            public static Func<HtmlNode, bool> <>9__27_4;
            public static Func<HtmlNode, bool> <>9__27_5;
            public static Func<HtmlNode, bool> <>9__27_6;
            public static Func<HtmlNode, bool> <>9__27_15;
            public static Func<HtmlNode, bool> <>9__27_7;
            public static Func<HtmlNode, bool> <>9__27_16;
            public static Func<HtmlNode, bool> <>9__27_8;
            public static Func<HtmlNode, bool> <>9__27_17;
            public static Func<HtmlNode, bool> <>9__27_9;
            public static Func<HtmlNode, bool> <>9__27_18;
            public static Func<HtmlNode, bool> <>9__27_10;
            public static Func<HtmlNode, bool> <>9__27_11;
            public static Func<HtmlNode, bool> <>9__27_12;
            public static Func<HtmlNode, bool> <>9__27_13;
            public static Func<HtmlNode, bool> <>9__27_14;
            public static Func<HtmlNode, bool> <>9__30_0;
            public static Func<HtmlNode, bool> <>9__30_1;
            public static Func<HtmlNode, bool> <>9__32_0;
            public static Func<HtmlNode, bool> <>9__32_1;
            public static Func<HtmlNode, bool> <>9__32_2;
            public static Func<HtmlNode, bool> <>9__32_3;
            public static Func<HtmlNode, bool> <>9__32_4;
            public static Func<HtmlNode, bool> <>9__32_5;
            public static Func<HtmlNode, bool> <>9__32_6;
            public static Func<System.Net.Cookie, bool> <>9__32_9;
            public static Func<HtmlNode, bool> <>9__32_10;
            public static Func<HtmlNode, bool> <>9__32_11;
            public static Func<HtmlNode, bool> <>9__32_16;
            public static Func<HtmlNode, bool> <>9__32_12;
            public static Func<HtmlNode, bool> <>9__32_13;
            public static Func<HtmlNode, bool> <>9__32_18;
            public static Func<HtmlNode, bool> <>9__33_5;
            public static Func<HtmlNode, bool> <>9__33_6;
            public static Func<HtmlNode, bool> <>9__33_9;
            public static Func<HtmlNode, bool> <>9__33_7;
            public static Func<HtmlNode, bool> <>9__33_8;
            public static Func<HtmlNode, bool> <>9__33_11;
            public static Func<HtmlNode, bool> <>9__33_13;
            public static Func<HtmlNode, bool> <>9__33_14;
            public static Func<HtmlNode, bool> <>9__33_17;
            public static Func<HtmlNode, bool> <>9__33_15;
            public static Func<HtmlNode, bool> <>9__33_16;
            public static Func<HtmlNode, bool> <>9__33_19;
            public static Func<HtmlNode, bool> <>9__33_21;
            public static Func<HtmlNode, bool> <>9__33_22;
            public static Func<HtmlNode, bool> <>9__33_26;
            public static Func<HtmlNode, bool> <>9__33_23;
            public static Func<HtmlNode, bool> <>9__33_24;
            public static Func<HtmlNode, bool> <>9__33_28;
            public static Func<HtmlNode, bool> <>9__33_2;
            public static Func<HtmlNode, bool> <>9__33_30;
            public static Func<HtmlNode, bool> <>9__33_3;
            public static Func<HtmlNode, bool> <>9__33_31;
            public static Func<HtmlNode, bool> <>9__33_4;
            public static Func<HtmlNode, bool> <>9__33_32;
            public static Func<SupremeProduct, bool> <>9__34_1;
            public static Func<SupremeProduct, bool> <>9__34_2;
            public static Func<SupremeProduct, bool> <>9__34_3;
            public static Func<SupremeProduct, bool> <>9__34_4;
            public static Func<SupremeProduct, bool> <>9__34_5;
            public static Func<SupremeProduct, bool> <>9__34_6;
            public static Func<SupremeProduct, bool> <>9__34_7;
            public static Func<SupremeProduct, bool> <>9__34_8;
            public static Func<SupremeProduct, bool> <>9__34_9;
            public static Func<SupremeProduct, bool> <>9__34_10;
            public static Func<SupremeProduct, bool> <>9__34_11;
            public static Func<SupremeProduct, bool> <>9__34_12;
            public static Func<SupremeProduct, bool> <>9__34_13;
            public static Func<HtmlNode, bool> <>9__35_1;
            public static Func<HtmlNode, bool> <>9__36_0;
            public static Func<HtmlNode, bool> <>9__36_1;
            public static Func<HtmlNode, bool> <>9__36_2;
            public static Func<HtmlNode, bool> <>9__36_3;
            public static Func<HtmlNode, bool> <>9__36_4;
            public static Func<HtmlNode, bool> <>9__36_5;
            public static Func<HtmlNode, bool> <>9__36_6;
            public static Func<HtmlNode, bool> <>9__36_15;
            public static Func<HtmlNode, bool> <>9__36_7;
            public static Func<HtmlNode, bool> <>9__36_16;
            public static Func<HtmlNode, bool> <>9__36_8;
            public static Func<HtmlNode, bool> <>9__36_17;
            public static Func<HtmlNode, bool> <>9__36_9;
            public static Func<HtmlNode, bool> <>9__36_18;
            public static Func<HtmlNode, bool> <>9__36_10;
            public static Func<HtmlNode, bool> <>9__36_11;
            public static Func<HtmlNode, bool> <>9__36_12;
            public static Func<HtmlNode, bool> <>9__36_13;
            public static Func<HtmlNode, bool> <>9__36_14;
            public static Func<HtmlNode, bool> <>9__37_1;
            public static Func<SupremeProduct, bool> <>9__38_0;
            public static Func<SupremeProduct, bool> <>9__38_1;
            public static Func<SupremeProduct, bool> <>9__38_2;
            public static Func<SupremeProduct, bool> <>9__38_3;
            public static Func<SupremeProduct, bool> <>9__38_4;
            public static Func<SupremeProduct, bool> <>9__38_5;
            public static Func<SupremeProduct, bool> <>9__38_6;
            public static Func<SupremeProduct, bool> <>9__38_7;
            public static Func<SupremeProduct, bool> <>9__38_8;
            public static Func<SupremeProduct, bool> <>9__38_9;
            public static Func<SupremeProduct, bool> <>9__38_10;
            public static Func<SupremeProduct, bool> <>9__38_11;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Supreme.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <AtcBrowser>b__30_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <AtcBrowser>b__30_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <CheckoutBrowser>b__33_11(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_13(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowser>b__33_14(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowser>b__33_15(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_16(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_17(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"));

            internal bool <CheckoutBrowser>b__33_19(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_2(HtmlNode x) => 
                ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cart-count")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "error"));

            internal bool <CheckoutBrowser>b__33_21(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowser>b__33_22(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowser>b__33_23(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_24(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_26(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"));

            internal bool <CheckoutBrowser>b__33_28(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_3(HtmlNode x) => 
                ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "confirmation")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "failed"));

            internal bool <CheckoutBrowser>b__33_30(HtmlNode x) => 
                ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cart-count")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "error"));

            internal bool <CheckoutBrowser>b__33_31(HtmlNode x) => 
                ((((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "confirmation")) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "failed"));

            internal bool <CheckoutBrowser>b__33_32(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "errors")) && !string.IsNullOrEmpty(x.InnerText.Trim()));

            internal bool <CheckoutBrowser>b__33_4(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "errors")) && !string.IsNullOrEmpty(x.InnerText.Trim()));

            internal bool <CheckoutBrowser>b__33_5(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowser>b__33_6(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowser>b__33_7(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_8(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowser>b__33_9(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"));

            internal bool <CheckoutBrowserless>b__32_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"));

            internal bool <CheckoutBrowserless>b__32_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "asec"));

            internal bool <CheckoutBrowserless>b__32_10(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowserless>b__32_11(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("number"));

            internal bool <CheckoutBrowserless>b__32_12(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowserless>b__32_13(HtmlNode x) => 
                ((x.Attributes["placeholder"] != null) && x.Attributes["placeholder"].Value.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowserless>b__32_16(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("カード番号"));

            internal bool <CheckoutBrowserless>b__32_18(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("label")) && x.InnerText.ToLowerInvariant().Contains("cvv"));

            internal bool <CheckoutBrowserless>b__32_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "asec"));

            internal bool <CheckoutBrowserless>b__32_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "asec"));

            internal bool <CheckoutBrowserless>b__32_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "hpcvv"));

            internal bool <CheckoutBrowserless>b__32_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "hpcvv"));

            internal bool <CheckoutBrowserless>b__32_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "hpcvv"));

            internal bool <CheckoutBrowserless>b__32_9(System.Net.Cookie x) => 
                (x.Name == "pure_cart");

            internal bool <DirectLinkBrowser>b__26_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit"));

            internal bool <DirectLinkBrowser>b__26_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLinkBrowser>b__26_10(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowser>b__26_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkBrowser>b__26_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowser>b__26_13(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkBrowser>b__26_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLinkBrowser>b__26_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLinkBrowser>b__26_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkBrowser>b__26_5(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkBrowser>b__26_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowser>b__26_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkBrowser>b__26_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowser>b__26_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkBrowserless>b__25_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit"));

            internal bool <DirectLinkBrowserless>b__25_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLinkBrowserless>b__25_10(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "add"));

            internal bool <DirectLinkBrowserless>b__25_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"));

            internal bool <DirectLinkBrowserless>b__25_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"));

            internal bool <DirectLinkBrowserless>b__25_13(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"));

            internal bool <DirectLinkBrowserless>b__25_14(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"));

            internal bool <DirectLinkBrowserless>b__25_15(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowserless>b__25_16(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkBrowserless>b__25_17(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowserless>b__25_18(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkBrowserless>b__25_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLinkBrowserless>b__25_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLinkBrowserless>b__25_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkBrowserless>b__25_5(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkBrowserless>b__25_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowserless>b__25_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkBrowserless>b__25_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkBrowserless>b__25_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkFallback>b__36_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit"));

            internal bool <DirectLinkFallback>b__36_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLinkFallback>b__36_10(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "add"));

            internal bool <DirectLinkFallback>b__36_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"));

            internal bool <DirectLinkFallback>b__36_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"));

            internal bool <DirectLinkFallback>b__36_13(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"));

            internal bool <DirectLinkFallback>b__36_14(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"));

            internal bool <DirectLinkFallback>b__36_15(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkFallback>b__36_16(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkFallback>b__36_17(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkFallback>b__36_18(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkFallback>b__36_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLinkFallback>b__36_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLinkFallback>b__36_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkFallback>b__36_5(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkFallback>b__36_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkFallback>b__36_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkFallback>b__36_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkFallback>b__36_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkMulti>b__27_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "commit"));

            internal bool <DirectLinkMulti>b__27_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLinkMulti>b__27_10(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "add"));

            internal bool <DirectLinkMulti>b__27_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"));

            internal bool <DirectLinkMulti>b__27_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "style"));

            internal bool <DirectLinkMulti>b__27_13(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"));

            internal bool <DirectLinkMulti>b__27_14(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "st"));

            internal bool <DirectLinkMulti>b__27_15(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkMulti>b__27_16(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkMulti>b__27_17(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkMulti>b__27_18(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkMulti>b__27_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLinkMulti>b__27_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLinkMulti>b__27_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkMulti>b__27_5(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLinkMulti>b__27_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkMulti>b__27_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <DirectLinkMulti>b__27_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "size"));

            internal bool <DirectLinkMulti>b__27_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "s"));

            internal bool <SearchBrowser>b__37_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "inner-article"));

            internal bool <SearchBrowserless>b__34_1(SupremeProduct x1) => 
                (x1.GroupName == "new");

            internal bool <SearchBrowserless>b__34_10(SupremeProduct x9) => 
                (x9.GroupName == "Shirts");

            internal bool <SearchBrowserless>b__34_11(SupremeProduct x9) => 
                (x9.GroupName == "Shorts");

            internal bool <SearchBrowserless>b__34_12(SupremeProduct x10) => 
                (x10.GroupName == "Sweatshirts");

            internal bool <SearchBrowserless>b__34_13(SupremeProduct x11) => 
                (x11.GroupName == "T-Shirts");

            internal bool <SearchBrowserless>b__34_2(SupremeProduct x2) => 
                (x2.GroupName == "Accessories");

            internal bool <SearchBrowserless>b__34_3(SupremeProduct x3) => 
                (x3.GroupName == "Bags");

            internal bool <SearchBrowserless>b__34_4(SupremeProduct x4) => 
                (x4.GroupName == "Pants");

            internal bool <SearchBrowserless>b__34_5(SupremeProduct x4) => 
                (x4.GroupName == "Shoes");

            internal bool <SearchBrowserless>b__34_6(SupremeProduct x5) => 
                (x5.GroupName == "Skate");

            internal bool <SearchBrowserless>b__34_7(SupremeProduct x6) => 
                (x6.GroupName == "Tops/Sweaters");

            internal bool <SearchBrowserless>b__34_8(SupremeProduct x7) => 
                (x7.GroupName == "Hats");

            internal bool <SearchBrowserless>b__34_9(SupremeProduct x8) => 
                (x8.GroupName == "Jackets");

            internal bool <SearchFallback>b__35_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "inner-article"));

            internal bool <SearchMulti>b__38_0(SupremeProduct x1) => 
                (x1.GroupName == "new");

            internal bool <SearchMulti>b__38_1(SupremeProduct x2) => 
                (x2.GroupName == "Accessories");

            internal bool <SearchMulti>b__38_10(SupremeProduct x10) => 
                (x10.GroupName == "Sweatshirts");

            internal bool <SearchMulti>b__38_11(SupremeProduct x11) => 
                (x11.GroupName == "T-Shirts");

            internal bool <SearchMulti>b__38_2(SupremeProduct x3) => 
                (x3.GroupName == "Bags");

            internal bool <SearchMulti>b__38_3(SupremeProduct x4) => 
                (x4.GroupName == "Pants");

            internal bool <SearchMulti>b__38_4(SupremeProduct x4) => 
                (x4.GroupName == "Shoes");

            internal bool <SearchMulti>b__38_5(SupremeProduct x5) => 
                (x5.GroupName == "Skate");

            internal bool <SearchMulti>b__38_6(SupremeProduct x6) => 
                (x6.GroupName == "Tops/Sweaters");

            internal bool <SearchMulti>b__38_7(SupremeProduct x7) => 
                (x7.GroupName == "Hats");

            internal bool <SearchMulti>b__38_8(SupremeProduct x8) => 
                (x8.GroupName == "Jackets");

            internal bool <SearchMulti>b__38_9(SupremeProduct x9) => 
                (x9.GroupName == "Shirts");
        }

        [CompilerGenerated]
        private static class <>o__32
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, bool>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, bool>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, bool>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, string>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, bool>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, bool>> <>p__34;
            public static CallSite<Func<CallSite, object, string, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, bool>> <>p__38;
            public static CallSite<Func<CallSite, object, string, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, string, object>> <>p__41;
            public static CallSite<Func<CallSite, object, bool>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string, object>> <>p__45;
            public static CallSite<Func<CallSite, object, bool>> <>p__46;
            public static CallSite<Func<CallSite, object, string, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object>> <>p__48;
            public static CallSite<Func<CallSite, object, string, object>> <>p__49;
            public static CallSite<Func<CallSite, object, bool>> <>p__50;
        }

        [CompilerGenerated]
        private static class <>o__34
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, int, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__23;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__24;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__25;
        }

        [CompilerGenerated]
        private static class <>o__38
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, int, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__23;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__24;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__25;
        }
    }
}

