namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Footlockereu : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;
        private string _synchronizer;
        private string _website;
        private string _intershop;
        private string _resUri;
        private object _payLink;

        public Footlockereu(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._diData = new Dictionary<string, string>();
            this._runner = runner;
            this._task = task;
            this.SetClient();
        }

        public bool Atc()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        string url = (((($"{this._website}addtocart" + "?SynchronizerToken=" + this._synchronizer) + "&Ajax=true" + "&Relay42_Category=Product%20Pages") + "&acctab-tabgroup-" + this._runner.PickedSize.Value.Value + "=null") + "&Quantity_" + this._runner.PickedSize.Value.Value + "=1") + "&SKU=" + this._runner.PickedSize.Value.Value;
                        this._srr = this._client.Post(url, this._diData).Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (!exception.Message.Contains("504") && !exception.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._srr.Contains("Quantity: 1"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception2, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        public bool Checkout()
        {
            // This item is obfuscated and can not be translated.
            if (this.SubmitShipping())
            {
                goto Label_0079;
            }
        Label_0042:
            switch (((((this._task.Payment != TaskObject.PaymentEnum.creditcard) ? -2057212890 : -38280924) ^ -1535635571) % 7))
            {
                case 1:
                    return false;

                case 2:
                case 4:
                    goto Label_0042;

                case 3:
                    break;

                case 5:
                    return false;

                case 6:
                    return true;

                default:
                    return this.SubmitOrder();
            }
        Label_0079:
            if (!this.SubmitBilling())
            {
            }
            if (!-1875935497 && !true)
            {
            }
            goto Label_0042;
        }

        public bool DirectLink(string link)
        {
            if ((this._task.Payment == TaskObject.PaymentEnum.paypal) && (this._task.Driver == null))
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                return false;
            }
            try
            {
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get(link).Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (!exception.Message.Contains("504") && !exception.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                this._synchronizer = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "SynchronizerToken"))).Attributes["value"].Value;
                if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection")))
                {
                    string url = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection"))).Attributes["data-ajaxcontent-url"].Value;
                    this._intershop = url.Substring(0, url.IndexOf("ViewProduct"));
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                            this._dynObj = this._client.Get(url).Json();
                            continue;
                        }
                        catch (WebException exception2)
                        {
                            if (!exception2.Message.Contains("504") && !exception2.Message.Contains("503"))
                            {
                                throw;
                            }
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                            continue;
                        }
                    }
                    string str2 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                    string str3 = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).Attributes["content"].Value;
                    this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"))).Attributes["content"].Value;
                    Product product1 = new Product {
                        ProductTitle = str2,
                        Link = link,
                        Price = str3
                    };
                    this._runner.Product = product1;
                    if (<>o__19.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__2 = CallSite<Action<CallSite, HtmlDocument, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "LoadHtml", null, typeof(Footlockereu), argumentInfo));
                    }
                    if (<>o__19.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footlockereu), argumentInfo));
                    }
                    if (<>o__19.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__19.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footlockereu), argumentInfo));
                    }
                    <>o__19.<>p__2.Target(<>o__19.<>p__2, this._currentDoc, <>o__19.<>p__1.Target(<>o__19.<>p__1, <>o__19.<>p__0.Target(<>o__19.<>p__0, this._dynObj, "content")));
                    if (this._currentDoc.DocumentNode.Descendants("label").Any<HtmlNode>(x => x.InnerText.Trim() == "US"))
                    {
                        foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("label").First<HtmlNode>(x => (x.InnerText.Trim() == "US")).NextSibling.NextSibling.Descendants("button")
                            where (x.Attributes["data-form-field-target"] != null) && (x.Attributes["data-form-field-target"].Value == "SKU")
                            select x)
                        {
                            this._runner.Product.AvailableSizes.Add(new KeyValuePair<string, string>(node.InnerText.Trim(), node.Attributes["data-form-field-value"].Value));
                        }
                    }
                    else if (this._currentDoc.DocumentNode.Descendants("label").Any<HtmlNode>(x => x.InnerText.Trim() == "Other"))
                    {
                        foreach (HtmlNode node2 in from x in this._currentDoc.DocumentNode.Descendants("label").First<HtmlNode>(x => (x.InnerText.Trim() == "Other")).NextSibling.NextSibling.Descendants("button")
                            where (x.Attributes["data-form-field-target"] != null) && (x.Attributes["data-form-field-target"].Value == "SKU")
                            select x)
                        {
                            this._runner.Product.AvailableSizes.Add(new KeyValuePair<string, string>(node2.InnerText.Trim(), node2.Attributes["data-form-field-value"].Value));
                        }
                    }
                    if (this._runner.Product.AvailableSizes.Count == 0)
                    {
                        if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        }
                        return false;
                    }
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
                    this._currentDoc.LoadHtml(this._srr);
                    this._website = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "add-to-cart-form"))).Attributes["action"].Value;
                    this._website = this._website.Replace("addtocart", "");
                    if (this._task.PriceCheck)
                    {
                        string str4 = "";
                        foreach (char ch in this._runner.Product.Price)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str4 = str4 + ch.ToString();
                            }
                        }
                        double num4 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num4 < this._task.MinimumPrice) || (num4 > this._task.MaximumPrice))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                            return false;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                    }
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
                                KeyValuePair<string, string> pair;
                                int num6;
                                goto Label_0AAB;
                            Label_09DB:
                                pair = enumerator2.Current;
                                List<string> source = new List<string>();
                                if (!pair.Key.Contains(":"))
                                {
                                    goto Label_0A3E;
                                }
                                char[] chArray2 = new char[] { ':' };
                                string[] strArray3 = pair.Key.Split(chArray2);
                                int index = 0;
                                goto Label_0A34;
                            Label_0A1D:
                                source.Add(strArray3[index].Trim());
                                index++;
                            Label_0A34:
                                if (index >= strArray3.Length)
                                {
                                    goto Label_0A4C;
                                }
                                goto Label_0A1D;
                            Label_0A3E:
                                source.Add(pair.Key);
                            Label_0A4C:
                                num6 = 0;
                                while (num6 < source.Count)
                                {
                                    source[num6] = source[num6].Trim().ToUpperInvariant();
                                    num6++;
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_0AB9;
                                }
                            Label_0AAB:
                                if (!enumerator2.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_09DB;
                            Label_0AB9:
                                this._runner.PickedSize = new KeyValuePair<string, string>?(pair);
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
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception3)
            {
                this._runner.IsError = true;
                if (!exception3.Message.Contains("430") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception3, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        public bool Login()
        {
            throw new NotImplementedException();
        }

        public bool Search()
        {
            if ((this._task.Payment == TaskObject.PaymentEnum.paypal) && (this._task.Driver == null))
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                return false;
            }
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                string various = this._task.Various;
                string url = "";
                uint num2 = <PrivateImplementationDetails>.ComputeStringHash(various);
                if (num2 <= 0x40e68a27)
                {
                    if (num2 > 0x25cc1c29)
                    {
                        switch (num2)
                        {
                            case 0x3bce7d62:
                                if (various == "DE")
                                {
                                    url = "https://www.footlocker.de/en/search?q=";
                                }
                                goto Label_0236;

                            case 0x3dce8088:
                                if (various == "DK")
                                {
                                    url = "https://www.footlocker.dk/en/search?q=";
                                }
                                goto Label_0236;
                        }
                        if ((num2 == 0x40e68a27) && (various == "NL"))
                        {
                            url = "https://www.footlocker.nl/en/search?q=";
                        }
                    }
                    else if (num2 == 0x17dd4f40)
                    {
                        if (various == "BE")
                        {
                            url = "https://www.footlocker.be/en/search?q=";
                        }
                    }
                    else if ((num2 == 0x25cc1c29) && (various == "ES"))
                    {
                        url = "https://www.footlocker.es/en/search?q=";
                    }
                }
                else if (num2 <= 0x5cd0efec)
                {
                    switch (num2)
                    {
                        case 0x41e68bba:
                            if (various == "NO")
                            {
                                url = "https://www.footlocker.no/en/search?q=";
                            }
                            goto Label_0236;

                        case 0x5c028b25:
                            if (various == "SE")
                            {
                                url = "https://www.footlocker.se/en/search?q=";
                            }
                            goto Label_0236;
                    }
                    if ((num2 == 0x5cd0efec) && (various == "GB"))
                    {
                        url = "https://www.footlocker.co.uk/en/search?q=";
                    }
                }
                else if (num2 == 0x66e90490)
                {
                    if (various == "IT")
                    {
                        url = "https://www.footlocker.it/en/search?q=";
                    }
                }
                else if (num2 != 0x76d35771)
                {
                    if ((num2 == 0x7be269da) && (various == "LU"))
                    {
                        url = "https://www.footlocker.lu/en/search?q=";
                    }
                }
                else if (various == "FR")
                {
                    url = "https://www.footlocker.fr/en/search?q=";
                }
            Label_0236:
                foreach (string str3 in this._task.Keywords)
                {
                    url = url + str3.Replace(" ", "+");
                    KeyValuePair<string, string> pair = this._client.Get(url).TextResponseUri();
                    this._srr = pair.Key;
                    HtmlDocument document1 = new HtmlDocument();
                    document1.LoadHtml(this._srr);
                    foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("div")
                        where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "fl-category--productlist--item")
                        select x)
                    {
                        if (this.DirectLink(node.Descendants("a").First<HtmlNode>().Attributes["href"].Value))
                        {
                            return true;
                        }
                    }
                }
                this._runner.Product = null;
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SEARCH);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SEARCH);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, true);
            this._client.SetDesktopAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", this._task.HomeUrl);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", this._task.HomeUrl);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                if (this._task.Payment == TaskObject.PaymentEnum.paypal)
                {
                    this._task.Driver.Navigate().GoToUrl(this._resUri);
                    this._task.Driver.FindElement(By.Name("brandName")).Click();
                    this._task.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2.0);
                    IWebElement element1 = this._task.Driver.FindElement(By.Id("card.cardNumber"));
                    element1.Clear();
                    element1.SendKeys(profile.CCNumber);
                    IWebElement element2 = this._task.Driver.FindElement(By.Id("card.cardHolderName"));
                    element2.Clear();
                    element2.SendKeys(profile.NameOnCard);
                    IWebElement element3 = this._task.Driver.FindElement(By.Id("card.cvcCode"));
                    element3.Clear();
                    element3.SendKeys(profile.Cvv);
                    new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='card.expiryMonth']"))).SelectByValue(profile.ExpiryMonth);
                    new SelectElement(this._task.Driver.FindElement(By.XPath("//select[@id='card.expiryYear']"))).SelectByValue(profile.ExpiryYear);
                    return true;
                }
                this._synchronizer = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "SynchronizerToken"))).Attributes["value"].Value;
                string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.params"))).Attributes["value"].Value;
                string str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperReference"))).Attributes["value"].Value;
                string str31 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperEmail"))).Attributes["value"].Value;
                string str28 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthDayOfMonth"))).Attributes["value"].Value;
                string str29 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthMonth"))).Attributes["value"].Value;
                string str30 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthYear"))).Attributes["value"].Value;
                string str17 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperLocale"))).Attributes["value"].Value;
                string str16 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.firstName"))).Attributes["value"].Value;
                string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.LastName"))).Attributes["value"].Value;
                string str37 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.telephoneNumber"))).Attributes["value"].Value;
                string str38 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionValidity"))).Attributes["value"].Value;
                string str35 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperType"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.gender"))).Attributes["value"].Value;
                string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressType"))).Attributes["value"].Value;
                string str22 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.street"))).Attributes["value"].Value;
                string str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.city"))).Attributes["value"].Value;
                string str18 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.houseNumberOrName"))).Attributes["value"].Value;
                string str19 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.stateOrProvince"))).Attributes["value"].Value;
                string str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.country"))).Attributes["value"].Value;
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.postalCode"))).Attributes["value"].Value;
                string str39 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressType"))).Attributes["value"].Value;
                string str40 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.street"))).Attributes["value"].Value;
                string str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.city"))).Attributes["value"].Value;
                string str24 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.houseNumberOrName"))).Attributes["value"].Value;
                string str25 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.stateOrProvince"))).Attributes["value"].Value;
                string str26 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.country"))).Attributes["value"].Value;
                string str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.postalCode"))).Attributes["value"].Value;
                string str23 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReturnData"))).Attributes["value"].Value;
                string str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipBeforeDate"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentAmount"))).Attributes["value"].Value;
                string str36 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReference"))).Attributes["value"].Value;
                string str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "skinCode"))).Attributes["value"].Value;
                string str32 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "countryCode"))).Attributes["value"].Value;
                string str33 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currencyCode"))).Attributes["value"].Value;
                string str34 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "resURL"))).Attributes["value"].Value;
                string str14 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantAccount"))).Attributes["value"].Value;
                string str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "recurringContract"))).Attributes["value"].Value;
                string str41 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "blockedMethods"))).Attributes["value"].Value;
                string str42 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantSig"))).Attributes["value"].Value;
                string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.sig"))).Attributes["value"].Value;
                string str21 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.data"))).Attributes["value"].Value;
                string str43 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperSig"))).Attributes["value"].Value;
                string str44 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressSig"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressSig"))).Attributes["value"].Value;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("SynchronizerToken", this._synchronizer);
                        this._diData.Add("encrypted.params", str9);
                        this._diData.Add("shopperReference", str10);
                        this._diData.Add("shopperEmail", str31);
                        this._diData.Add("shopper.dateOfBirthDayOfMonth", str28);
                        this._diData.Add("shopper.dateOfBirthMonth", str29);
                        this._diData.Add("shopper.dateOfBirthYear", str30);
                        this._diData.Add("shopperLocale", str17);
                        this._diData.Add("shopper.firstName", str16);
                        this._diData.Add("shopper.LastName", str5);
                        this._diData.Add("shopper.telephoneNumber", str37);
                        this._diData.Add("sessionValidity", str38);
                        this._diData.Add("shopperType", str35);
                        this._diData.Add("shopper.gender", str4);
                        this._diData.Add("billingAddressType", str8);
                        this._diData.Add("billingAddress.street", str22);
                        this._diData.Add("billingAddress.city", str15);
                        this._diData.Add("billingAddress.houseNumberOrName", str18);
                        this._diData.Add("billingAddress.stateOrProvince", str19);
                        this._diData.Add("billingAddress.country", str20);
                        this._diData.Add("billingAddress.postalCode", str);
                        this._diData.Add("deliveryAddressType", str39);
                        this._diData.Add("deliveryAddress.street", str40);
                        this._diData.Add("deliveryAddress.city", str6);
                        this._diData.Add("deliveryAddress.houseNumberOrName", str24);
                        this._diData.Add("deliveryAddress.stateOrProvince", str25);
                        this._diData.Add("deliveryAddress.country", str26);
                        this._diData.Add("deliveryAddress.postalCode", str27);
                        this._diData.Add("merchantReturnData", str23);
                        this._diData.Add("shipBeforeDate", str11);
                        this._diData.Add("paymentAmount", str2);
                        this._diData.Add("merchantReference", str36);
                        this._diData.Add("skinCode", str13);
                        this._diData.Add("countryCode", str32);
                        this._diData.Add("currencyCode", str33);
                        this._diData.Add("resURL", str34);
                        this._diData.Add("merchantAccount", str14);
                        this._diData.Add("recurringContract", str12);
                        this._diData.Add("blockedMethods", str41);
                        this._diData.Add("merchantSig", str42);
                        this._diData.Add("encrypted.sig", str7);
                        this._diData.Add("encrypted.data", str21);
                        this._diData.Add("shopperSig", str43);
                        this._diData.Add("deliveryAddressSig", str44);
                        this._diData.Add("billingAddressSig", str3);
                        this._srr = this._client.Post("https://live.barclaycardsmartpay.com/hpp/pay.shtml", this._diData).Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (!exception.Message.Contains("504") && !exception.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                string text1 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "displayGroup"))).Attributes["value"].Value;
                string text2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card.cardNumber"))).Attributes["value"].Value;
                string text3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card.cardHolderName"))).Attributes["value"].Value;
                string text4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card.cvcCode"))).Attributes["value"].Value;
                string str45 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paypal.storeOcDetails"))).Attributes["value"].Value;
                string str53 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sig"))).Attributes["value"].Value;
                str36 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReference"))).Attributes["value"].Value;
                string str46 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "brandCode"))).Attributes["value"].Value;
                str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentAmount"))).Attributes["value"].Value;
                str33 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currencyCode"))).Attributes["value"].Value;
                str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipBeforeDate"))).Attributes["value"].Value;
                str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "skinCode"))).Attributes["value"].Value;
                str14 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantAccount"))).Attributes["value"].Value;
                str17 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperLocale"))).Attributes["value"].Value;
                string str55 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stage"))).Attributes["value"].Value;
                string str54 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionId"))).Attributes["value"].Value;
                str38 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionValidity"))).Attributes["value"].Value;
                str32 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "countryCode"))).Attributes["value"].Value;
                str31 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperEmail"))).Attributes["value"].Value;
                str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperReference"))).Attributes["value"].Value;
                str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "recurringContract"))).Attributes["value"].Value;
                str34 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "resURL"))).Attributes["value"].Value;
                str41 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "blockedMethods"))).Attributes["value"].Value;
                str23 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReturnData"))).Attributes["value"].Value;
                string str48 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "originalSession"))).Attributes["value"].Value;
                str22 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.street"))).Attributes["value"].Value;
                str18 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.houseNumberOrName"))).Attributes["value"].Value;
                str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.city"))).Attributes["value"].Value;
                str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.postalCode"))).Attributes["value"].Value;
                str19 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.stateOrProvince"))).Attributes["value"].Value;
                str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.country"))).Attributes["value"].Value;
                str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressType"))).Attributes["value"].Value;
                str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressSig"))).Attributes["value"].Value;
                str40 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.street"))).Attributes["value"].Value;
                str24 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.houseNumberOrName"))).Attributes["value"].Value;
                str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.city"))).Attributes["value"].Value;
                str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.postalCode"))).Attributes["value"].Value;
                str25 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.stateOrProvince"))).Attributes["value"].Value;
                str26 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.country"))).Attributes["value"].Value;
                str39 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressType"))).Attributes["value"].Value;
                str44 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressSig"))).Attributes["value"].Value;
                str16 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.firstName"))).Attributes["value"].Value;
                str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.lastName"))).Attributes["value"].Value;
                str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.gender"))).Attributes["value"].Value;
                str28 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthDayOfMonth"))).Attributes["value"].Value;
                str29 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthMonth"))).Attributes["value"].Value;
                str30 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthYear"))).Attributes["value"].Value;
                str37 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.telephoneNumber"))).Attributes["value"].Value;
                str35 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperType"))).Attributes["value"].Value;
                str43 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperSig"))).Attributes["value"].Value;
                str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.sig"))).Attributes["value"].Value;
                str21 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.data"))).Attributes["value"].Value;
                str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.params"))).Attributes["value"].Value;
                string str52 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "referrerURL"))).Attributes["value"].Value;
                string str47 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dfValue"))).Attributes["value"].Value;
                string str50 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "usingFrame"))).Attributes["value"].Value;
                string str51 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "usingPopUp"))).Attributes["value"].Value;
                string text5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperBehaviorLog"))).Attributes["value"].Value;
                string str49 = "";
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("displayGroup", "card");
                        this._diData.Add("card.cardNumber", profile.CCNumber);
                        this._diData.Add("card.cardHolderName", profile.NameOnCard);
                        this._diData.Add("card.cvcCode", profile.Cvv);
                        this._diData.Add("paypal.storeOcDetails", str45);
                        this._diData.Add("sig", str53);
                        this._diData.Add("merchantReference", str36);
                        this._diData.Add("brandCode", str46);
                        this._diData.Add("paymentAmount", str2);
                        this._diData.Add("currencyCode", str33);
                        this._diData.Add("shipBeforeDate", str11);
                        this._diData.Add("skinCode", str13);
                        this._diData.Add("merchantAccount", str14);
                        this._diData.Add("shopperLocale", str17);
                        this._diData.Add("stage", str55);
                        this._diData.Add("sessionId", str54);
                        this._diData.Add("sessionValidity", str38);
                        this._diData.Add("countryCode", str32);
                        this._diData.Add("shopperEmail", str31);
                        this._diData.Add("shopperReference", str10);
                        this._diData.Add("recurringContract", str12);
                        this._diData.Add("resURL", str34);
                        this._diData.Add("blockedMethods", str41);
                        this._diData.Add("merchantReturnData", str23);
                        this._diData.Add("originalSession", str48);
                        this._diData.Add("billingAddress.street", str22);
                        this._diData.Add("billingAddress.houseNumberOrName", str18);
                        this._diData.Add("billingAddress.city", str15);
                        this._diData.Add("billingAddress.postalCode", str);
                        this._diData.Add("billingAddress.stateOrProvince", str19);
                        this._diData.Add("billingAddress.country", str20);
                        this._diData.Add("billingAddressType", str8);
                        this._diData.Add("billingAddressSig", str3);
                        this._diData.Add("deliveryAddress.street", str40);
                        this._diData.Add("deliveryAddress.houseNumberOrName", str24);
                        this._diData.Add("deliveryAddress.city", str6);
                        this._diData.Add("deliveryAddress.postalCode", str27);
                        this._diData.Add("deliveryAddress.stateOrProvince", str25);
                        this._diData.Add("deliveryAddress.country", str26);
                        this._diData.Add("deliveryAddressType", str39);
                        this._diData.Add("deliveryAddressSig", str44);
                        this._diData.Add("shopper.firstName", str16);
                        this._diData.Add("shopper.lastName", str5);
                        this._diData.Add("shopper.gender", str4);
                        this._diData.Add("shopper.dateOfBirthDayOfMonth", str28);
                        this._diData.Add("shopper.dateOfBirthMonth", str29);
                        this._diData.Add("shopper.dateOfBirthYear", str30);
                        this._diData.Add("shopper.telephoneNumber", str37);
                        this._diData.Add("shopperType", str35);
                        this._diData.Add("shopperSig", str43);
                        this._diData.Add("encrypted.sig", str7);
                        this._diData.Add("encrypted.data", str21);
                        this._diData.Add("encrypted.params", str9);
                        this._diData.Add("referrerURL", str52);
                        this._diData.Add("dfValue", str47);
                        this._diData.Add("usingFrame", str50);
                        this._diData.Add("usingPopUp", str51);
                        this._diData.Add("shopperBehaviorLog", "{\"numberBind\":\"1\",\"holderNameBind\":\"1\",\"cvcBind\":\"1\",\"numberFieldFocusCount\":\"1\",\"numberFieldLog\":\"fo@311,cl@312,cl@313,ch@319,bl@324\",\"numberFieldClickCount\":\"2\",\"numberFieldChangeCount\":\"1\",\"numberFieldEvHa\":\"total=0\",\"holderNameFieldChangeCount\":\"1\",\"holderNameFieldLog\":\"ch@319\",\"holderNameFieldEvHa\":\"total=0\",\"numberFieldBlurCount\":\"1\",\"cvcFieldFocusCount\":\"1\",\"cvcFieldLog\":\"fo@324,cl@325,KN@330,KN@332,KN@333\",\"cvcFieldClickCount\":\"1\",\"cvcFieldKeyCount\":\"3\"}");
                        this._diData.Add("card.expiryMonth", profile.ExpiryMonth);
                        this._diData.Add("card.expiryYear", profile.ExpiryYear);
                        KeyValuePair<string, string> pair = this._client.Post("https://live.barclaycardsmartpay.com/hpp/completeCard.shtml", this._diData).TextResponseUri();
                        this._srr = pair.Key;
                        str49 = pair.Value;
                        continue;
                    }
                    catch (WebException exception2)
                    {
                        if (!exception2.Message.Contains("504") && !exception2.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                if (!str49.Contains("/hpp/3d-redirect.shtml?merchantReference="))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.INVALID_CREDIT_CARD);
                    States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_CREDIT_CARD, null, "", "");
                    return false;
                }
                this._currentDoc.LoadHtml(this._srr);
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception3)
            {
                this._runner.IsError = true;
                if (!exception3.Message.Contains("430") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception3, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        private bool SubmitOrder()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
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
                if (Global.SERIAL == "EVE-1111111111111")
                {
                    goto Label_0C3F;
                }
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
                bool flag2 = true;
                this._currentDoc.LoadHtml(this._srr);
                string str5 = "";
                if (!this._srr.Contains(".securecode.com") && !this._srr.Contains("byvisa"))
                {
                    throw new Exception("Error processing payment");
                }
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pageform"))).Attributes["action"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("PaReq", str2);
                        this._diData.Add("TermUrl", str4);
                        this._diData.Add("MD", str3);
                        this._srr = this._client.Post(url, this._diData).Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (!exception.Message.Contains("504") && !exception.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag2 = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm")))
                {
                    url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"))).Attributes["action"].Value;
                    string str14 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                    str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                    string str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"))).Attributes["value"].Value;
                    string str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"))).Attributes["value"].Value;
                    string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"))).Attributes["value"].Value;
                    string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"))).Attributes["value"].Value;
                    string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"))).Attributes["value"].Value;
                    string str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"))).Attributes["value"].Value;
                    string str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"))).Attributes["value"].Value;
                    string str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"))).Attributes["value"].Value;
                    string str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"))).Attributes["value"].Value;
                    flag2 = true;
                    do
                    {
                        if (!flag2)
                        {
                            goto Label_0B93;
                        }
                        flag2 = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("PaRes", str14);
                            this._diData.Add("MD", str3);
                            this._diData.Add("PaReq", str2);
                            this._diData.Add("ABSlog", str15);
                            this._diData.Add("deviceDNA", str6);
                            this._diData.Add("executionTime", str8);
                            this._diData.Add("dnaError", str9);
                            this._diData.Add("mesc", str7);
                            this._diData.Add("mescIterationCount", str10);
                            this._diData.Add("desc", str11);
                            this._diData.Add("isDNADone", str13);
                            this._diData.Add("arcotFlashCookie", str12);
                            KeyValuePair<string, string> pair = this._client.Post(url, this._diData).TextResponseUri();
                            str5 = pair.Value;
                            this._srr = pair.Key;
                        }
                        catch (WebException exception2)
                        {
                            if (!exception2.Message.Contains("504") && !exception2.Message.Contains("503"))
                            {
                                throw;
                            }
                            flag2 = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        this._currentDoc.LoadHtml(this._srr);
                    }
                    while (!this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => ((x.Attributes["action"] != null) && x.Attributes["action"].Value.Contains("https://live.barclaycardsmartpay.com/hpp/pay.shtml"))));
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                string str16 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("PaRes", str16);
                        this._diData.Add("MD", str3);
                        KeyValuePair<string, string> pair2 = this._client.Post(url, this._diData).TextResponseUri();
                        str5 = pair2.Value;
                        this._srr = pair2.Key;
                        continue;
                    }
                    catch (WebException exception3)
                    {
                        if (!exception3.Message.Contains("504") && !exception3.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag2 = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => (x.Attributes["action"] != null) && x.Attributes["action"].Value.Contains("https://live.barclaycardsmartpay.com/hpp/pay.shtml")))
                {
                    goto Label_0C0E;
                }
            Label_0B93:
                if (!str5.ToLowerInvariant().Contains("paymentfailed"))
                {
                    try
                    {
                        EveAIO.Helpers.AddDbValue("Footlocker EU|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    }
                    catch
                    {
                    }
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            Label_0C0E:
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            Label_0C3F:
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception4)
            {
                if (this._srr.ToLowerInvariant().Contains("paymentfailed"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                this._runner.IsError = true;
                if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception4, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        private bool SubmitShipping()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Get($"{this._intershop}ViewCart-Checkout?SynchronizerToken={this._synchronizer}").Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (!exception.Message.Contains("504") && !exception.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                flag = true;
                string uriString = "";
                while (flag)
                {
                    flag = false;
                    try
                    {
                        KeyValuePair<string, string> pair = this._client.Get($"{this._website}checkout-overview?SynchronizerToken={this._synchronizer}").TextResponseUri();
                        this._srr = pair.Key;
                        uriString = pair.Value;
                        continue;
                    }
                    catch (WebException exception2)
                    {
                        if (!exception2.Message.Contains("504") && !exception2.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipping_AddressID"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaymentServiceSelection"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ShippingMethodUUID"))).Attributes["value"].Value;
                ProfileObject profile = this._runner.Profile;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("SynchronizerToken", this._synchronizer);
                        this._diData.Add("billing_Address3", "");
                        this._diData.Add("isshippingaddress", "");
                        this._diData.Add("billing_Title", "common.account.salutation.mr.text");
                        this._diData.Add("billing_FirstName", profile.FirstName);
                        this._diData.Add("billing_LastName", profile.LastName);
                        this._diData.Add("billing_CountryCode", profile.CountryId);
                        this._diData.Add("billing_Address1", profile.Address1);
                        this._diData.Add("billing_Address2", profile.Address2);
                        this._diData.Add("billing_City", profile.City);
                        this._diData.Add("billing_PostalCode", profile.Zip);
                        this._diData.Add("billing_PhoneHome", profile.Phone);
                        this._diData.Add("billing_BirthdayRequired", "true");
                        this._diData.Add("billing_Birthday_Day", profile.BirthdayDay);
                        this._diData.Add("billing_Birthday_Month", profile.BirthdayMonth);
                        this._diData.Add("billing_Birthday_Year", profile.BirthdayYear);
                        this._diData.Add("email_Email", profile.Email);
                        this._diData.Add("billing_ShippingAddressSameAsBilling", "");
                        this._diData.Add("shipping_Address3", "");
                        this._diData.Add("shipping_Title", "common.account.salutation.mr.text");
                        this._diData.Add("shipping_FirstName", profile.FirstNameShipping);
                        this._diData.Add("shipping_LastName", profile.LastNameShipping);
                        this._diData.Add("shipping_CountryCode", profile.CountryIdShipping);
                        this._diData.Add("shipping_Address1", profile.Address1Shipping);
                        this._diData.Add("shipping_Address2", profile.Address2Shipping);
                        this._diData.Add("shipping_City", profile.CityShipping);
                        this._diData.Add("shipping_PostalCode", profile.ZipShipping);
                        this._diData.Add("shipping_PhoneHome", profile.PhoneShipping);
                        this._diData.Add("shipping_AddressID", str3);
                        this._diData.Add("CheckoutRegisterForm_Password", "");
                        this._diData.Add("promotionCode", "");
                        this._diData.Add("PaymentServiceSelection", str4);
                        this._diData.Add("UserDeviceTypeForPaymentRedirect", "Desktop");
                        this._diData.Add("UserDeviceFingerprintForPaymentRedirect", "0400CIfeAe15Cx8Nf94lis1ztg3zcGo6H4Avnrp/XmcfWoVVgr+Rt2dAZDYKGojTbbj7Ay36XoqtPmZFXM8jbbY2/zc5J3xnltE7Kq6IhM8HltvDlnXzDbRRZMFlj8uo417+TgsSA5y2mIsp2U5yrYp+5igWriMua6v6A2LCHjpy4O3VHwYa6IiyjWwqOuJ1+ku8kgIq2hH9E7t3V4Oui+XIGB+ruaEwNh0mMLpE7w3gzTDXnfra5uHnD42fRE9PMD+Q3FCX9pS6pa9J27cNnYPSQUyAZ5slTMaJhGc3lDN94AZR9s6zoawT00N13n2vmAeykQgdlVeDidwjyx6XR+7WMQo5VNMvaDXfrI3MaSYrgzshIqKrjK+gdyAFUwN05hXzcycRhS8EwApMRV0YmXEHAJzxpxtwqoC5Z/uCi7ME8loQwLLSzeFEuVfe4edP+YyrpLGfoBXHPGknZaujYKIuYECE079VHZItljAlWwXHcT+hgN6tJA4p1nA+k9XRlIy/3HEKKSWQ2ArTou5QrL4AFM/u32NRwIY/uVKo6+FV89elMbzS7PhkqbMXWhlwl+NOxA9x3qRKbjOBF2jB4DXWGlEh3GX8eDv8Y51itE+9afEBREq5MBL5WCFTg9H7wizhfeStEaMUb1UyFgIzr08goEJI4/wX97IAaVmZqTD7tyd0W8+tYgTFPgD3AdfuKnvO6jB+ZmMavO7St0oGtrebhsMMd1YO9wPB/heW3XJz1ucaugt8Tmgw78e8mnFiPj0ig9Yv/IIsBbdtTKyPOriYw05uVw6QWyb2osTzKoxVC6fSPFFgCaMpvU/h6EFXXmXEHwfKKAi+qA626EwABzaufUcOjS9hgMLfawE4LsFveTttrNHzt95ePk0rzXnn5J+c/XQXiHK5P5lKj+qHrqGCPLI+vS3zO1UWKyNRXh0mw7Kb1x4cMpEKIrjEjVFMhgLyi4PvWoIpyZmpr5zlQx90s1eovP7W+WpQCv10TaNx+XTCCuhAF+nK4gMnQeNbSx1QeEuOAQEdtVV4VShbY+EVBDdML6ZhV5yEfobiOhcONJPNnBg89k56rNOHjs7HO5CL//li8nYbsqKV1/B3exlsY+b8dIdPEb9km9ceHDKRCiLCgbgXPmizjHlWmCRBdNDNsuaxAwAEbRdjjZSkBTBViGEmNS0zRTCiGYWc8AmavPCV2Zx3g6oWx7XTzn3r9+jd254EHQioBL0dHI457rW8syv+Vfp9JAGUjFik2mFmSi1NC8cV7JobGkQv3q3wNr9fWFJ0B7j87M9ycPtZzoVX+j+EDXm9ivw4izrrGfL02gRGw6iYQqP8SM5sToI40yMyDKThVP1hAyDsYZoE5G7Lyh0R9vRNj5evLzdF6XhbUcf3I3W4B8RJEZoJEatULOPLG1/UAvJzkw9J7W+hXCchDhDlc5ibQ6oppMw6vkl0df+6I9H+mLSXNFUlSIs/QYMq5o+EWyPKIPe0I4QET6GM9514NW1yHFeOu/gWlDy/bp5GtUdesQ0J/2S79XLLVdvOxA2gGNZcjvHH9PO4FFm2UVWCv5G3Z0BkYdGaKdph2MJSXAtXl66i3uh7tJjhpncGJXRNTb943ZXstuIaDI7pK9/lpBvgmvtOs3zOGbKg3eQbMIiy37Dr3RDRCyf05jW7zp5/V3dyW8UOan7BMtUZTwZOuP44LgB5qQwqN6vwaIxL8u/NNwYQPpa4jvGjGo75O93aahKOBWOVT31SgfbGSSK3BLIkq2YwA/L9k/nRYE7ky3s73sG5qpa8lp15d4bZITEpUa6nbYc045DcMpoSQg==");
                        this._diData.Add("ShippingMethodUUID", str2);
                        this._diData.Add("termsAndConditions", "on");
                        this._diData.Add("GDPRDataComplianceRequired", "true");
                        this._diData.Add("sendOrder", "");
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                        this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "max-age=0");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                        KeyValuePair<string, string> pair2 = this._client.Post($"{this._intershop}ViewCheckoutOverview-Dispatch", this._diData).TextResponseUri();
                        this._srr = pair2.Key;
                        this._resUri = pair2.Value;
                        continue;
                    }
                    catch (WebException exception3)
                    {
                        if (!exception3.Message.Contains("504") && !exception3.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                if (!this._srr.Contains("https://live.barclaycardsmartpay.com"))
                {
                    throw new Exception("Error processing shipping info");
                }
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception4)
            {
                this._runner.IsError = true;
                if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception4, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Footlockereu.<>c <>9;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__16_2;
            public static Func<HtmlNode, bool> <>9__16_3;
            public static Func<HtmlNode, bool> <>9__16_4;
            public static Func<HtmlNode, bool> <>9__16_5;
            public static Func<HtmlNode, bool> <>9__16_6;
            public static Func<HtmlNode, bool> <>9__16_7;
            public static Func<HtmlNode, bool> <>9__16_8;
            public static Func<HtmlNode, bool> <>9__16_9;
            public static Func<HtmlNode, bool> <>9__16_10;
            public static Func<HtmlNode, bool> <>9__16_11;
            public static Func<HtmlNode, bool> <>9__16_12;
            public static Func<HtmlNode, bool> <>9__16_13;
            public static Func<HtmlNode, bool> <>9__16_14;
            public static Func<HtmlNode, bool> <>9__16_15;
            public static Func<HtmlNode, bool> <>9__16_16;
            public static Func<HtmlNode, bool> <>9__16_17;
            public static Func<HtmlNode, bool> <>9__16_18;
            public static Func<HtmlNode, bool> <>9__16_19;
            public static Func<HtmlNode, bool> <>9__16_20;
            public static Func<HtmlNode, bool> <>9__16_21;
            public static Func<HtmlNode, bool> <>9__16_22;
            public static Func<HtmlNode, bool> <>9__17_0;
            public static Func<HtmlNode, bool> <>9__17_1;
            public static Func<HtmlNode, bool> <>9__17_2;
            public static Func<HtmlNode, bool> <>9__17_3;
            public static Func<HtmlNode, bool> <>9__17_4;
            public static Func<HtmlNode, bool> <>9__17_5;
            public static Func<HtmlNode, bool> <>9__17_6;
            public static Func<HtmlNode, bool> <>9__17_7;
            public static Func<HtmlNode, bool> <>9__17_8;
            public static Func<HtmlNode, bool> <>9__17_9;
            public static Func<HtmlNode, bool> <>9__17_10;
            public static Func<HtmlNode, bool> <>9__17_11;
            public static Func<HtmlNode, bool> <>9__17_12;
            public static Func<HtmlNode, bool> <>9__17_13;
            public static Func<HtmlNode, bool> <>9__17_14;
            public static Func<HtmlNode, bool> <>9__17_15;
            public static Func<HtmlNode, bool> <>9__17_16;
            public static Func<HtmlNode, bool> <>9__17_17;
            public static Func<HtmlNode, bool> <>9__17_18;
            public static Func<HtmlNode, bool> <>9__17_19;
            public static Func<HtmlNode, bool> <>9__17_20;
            public static Func<HtmlNode, bool> <>9__17_21;
            public static Func<HtmlNode, bool> <>9__17_22;
            public static Func<HtmlNode, bool> <>9__17_23;
            public static Func<HtmlNode, bool> <>9__17_24;
            public static Func<HtmlNode, bool> <>9__17_25;
            public static Func<HtmlNode, bool> <>9__17_26;
            public static Func<HtmlNode, bool> <>9__17_27;
            public static Func<HtmlNode, bool> <>9__17_28;
            public static Func<HtmlNode, bool> <>9__17_29;
            public static Func<HtmlNode, bool> <>9__17_30;
            public static Func<HtmlNode, bool> <>9__17_31;
            public static Func<HtmlNode, bool> <>9__17_32;
            public static Func<HtmlNode, bool> <>9__17_33;
            public static Func<HtmlNode, bool> <>9__17_34;
            public static Func<HtmlNode, bool> <>9__17_35;
            public static Func<HtmlNode, bool> <>9__17_36;
            public static Func<HtmlNode, bool> <>9__17_37;
            public static Func<HtmlNode, bool> <>9__17_38;
            public static Func<HtmlNode, bool> <>9__17_39;
            public static Func<HtmlNode, bool> <>9__17_40;
            public static Func<HtmlNode, bool> <>9__17_41;
            public static Func<HtmlNode, bool> <>9__17_42;
            public static Func<HtmlNode, bool> <>9__17_43;
            public static Func<HtmlNode, bool> <>9__17_44;
            public static Func<HtmlNode, bool> <>9__17_45;
            public static Func<HtmlNode, bool> <>9__17_46;
            public static Func<HtmlNode, bool> <>9__17_47;
            public static Func<HtmlNode, bool> <>9__17_48;
            public static Func<HtmlNode, bool> <>9__17_49;
            public static Func<HtmlNode, bool> <>9__17_50;
            public static Func<HtmlNode, bool> <>9__17_51;
            public static Func<HtmlNode, bool> <>9__17_52;
            public static Func<HtmlNode, bool> <>9__17_53;
            public static Func<HtmlNode, bool> <>9__17_54;
            public static Func<HtmlNode, bool> <>9__17_55;
            public static Func<HtmlNode, bool> <>9__17_56;
            public static Func<HtmlNode, bool> <>9__17_57;
            public static Func<HtmlNode, bool> <>9__17_58;
            public static Func<HtmlNode, bool> <>9__17_59;
            public static Func<HtmlNode, bool> <>9__17_60;
            public static Func<HtmlNode, bool> <>9__17_61;
            public static Func<HtmlNode, bool> <>9__17_62;
            public static Func<HtmlNode, bool> <>9__17_63;
            public static Func<HtmlNode, bool> <>9__17_64;
            public static Func<HtmlNode, bool> <>9__17_65;
            public static Func<HtmlNode, bool> <>9__17_66;
            public static Func<HtmlNode, bool> <>9__17_67;
            public static Func<HtmlNode, bool> <>9__17_68;
            public static Func<HtmlNode, bool> <>9__17_69;
            public static Func<HtmlNode, bool> <>9__17_70;
            public static Func<HtmlNode, bool> <>9__17_71;
            public static Func<HtmlNode, bool> <>9__17_72;
            public static Func<HtmlNode, bool> <>9__17_73;
            public static Func<HtmlNode, bool> <>9__17_74;
            public static Func<HtmlNode, bool> <>9__17_75;
            public static Func<HtmlNode, bool> <>9__17_76;
            public static Func<HtmlNode, bool> <>9__17_77;
            public static Func<HtmlNode, bool> <>9__17_78;
            public static Func<HtmlNode, bool> <>9__17_79;
            public static Func<HtmlNode, bool> <>9__17_80;
            public static Func<HtmlNode, bool> <>9__17_81;
            public static Func<HtmlNode, bool> <>9__17_82;
            public static Func<HtmlNode, bool> <>9__17_83;
            public static Func<HtmlNode, bool> <>9__17_84;
            public static Func<HtmlNode, bool> <>9__17_85;
            public static Func<HtmlNode, bool> <>9__17_86;
            public static Func<HtmlNode, bool> <>9__17_87;
            public static Func<HtmlNode, bool> <>9__17_88;
            public static Func<HtmlNode, bool> <>9__17_89;
            public static Func<HtmlNode, bool> <>9__17_90;
            public static Func<HtmlNode, bool> <>9__17_91;
            public static Func<HtmlNode, bool> <>9__17_92;
            public static Func<HtmlNode, bool> <>9__17_93;
            public static Func<HtmlNode, bool> <>9__17_94;
            public static Func<HtmlNode, bool> <>9__17_95;
            public static Func<HtmlNode, bool> <>9__17_96;
            public static Func<HtmlNode, bool> <>9__17_97;
            public static Func<HtmlNode, bool> <>9__17_98;
            public static Func<HtmlNode, bool> <>9__17_99;
            public static Func<HtmlNode, bool> <>9__17_100;
            public static Func<HtmlNode, bool> <>9__17_101;
            public static Func<HtmlNode, bool> <>9__17_102;
            public static Func<HtmlNode, bool> <>9__18_0;
            public static Func<HtmlNode, bool> <>9__18_1;
            public static Func<HtmlNode, bool> <>9__18_2;
            public static Func<HtmlNode, bool> <>9__19_0;
            public static Func<HtmlNode, bool> <>9__19_1;
            public static Func<HtmlNode, bool> <>9__19_2;
            public static Func<HtmlNode, bool> <>9__19_3;
            public static Func<HtmlNode, bool> <>9__19_4;
            public static Func<HtmlNode, bool> <>9__19_5;
            public static Func<HtmlNode, bool> <>9__19_6;
            public static Func<HtmlNode, bool> <>9__19_9;
            public static Func<HtmlNode, bool> <>9__19_10;
            public static Func<HtmlNode, bool> <>9__19_7;
            public static Func<HtmlNode, bool> <>9__19_11;
            public static Func<HtmlNode, bool> <>9__19_12;
            public static Func<HtmlNode, bool> <>9__19_8;
            public static Func<HtmlNode, bool> <>9__21_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Footlockereu.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__19_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "SynchronizerToken"));

            internal bool <DirectLink>b__19_1(HtmlNode x) => 
                ((x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection"));

            internal bool <DirectLink>b__19_10(HtmlNode x) => 
                ((x.Attributes["data-form-field-target"] != null) && (x.Attributes["data-form-field-target"].Value == "SKU"));

            internal bool <DirectLink>b__19_11(HtmlNode x) => 
                (x.InnerText.Trim() == "Other");

            internal bool <DirectLink>b__19_12(HtmlNode x) => 
                ((x.Attributes["data-form-field-target"] != null) && (x.Attributes["data-form-field-target"].Value == "SKU"));

            internal bool <DirectLink>b__19_2(HtmlNode x) => 
                ((x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection"));

            internal bool <DirectLink>b__19_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLink>b__19_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__19_5(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"));

            internal bool <DirectLink>b__19_6(HtmlNode x) => 
                (x.InnerText.Trim() == "US");

            internal bool <DirectLink>b__19_7(HtmlNode x) => 
                (x.InnerText.Trim() == "Other");

            internal bool <DirectLink>b__19_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "add-to-cart-form"));

            internal bool <DirectLink>b__19_9(HtmlNode x) => 
                (x.InnerText.Trim() == "US");

            internal bool <Search>b__21_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "fl-category--productlist--item"));

            internal bool <SubmitBilling>b__17_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "SynchronizerToken"));

            internal bool <SubmitBilling>b__17_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.params"));

            internal bool <SubmitBilling>b__17_10(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.telephoneNumber"));

            internal bool <SubmitBilling>b__17_100(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "usingFrame"));

            internal bool <SubmitBilling>b__17_101(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "usingPopUp"));

            internal bool <SubmitBilling>b__17_102(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperBehaviorLog"));

            internal bool <SubmitBilling>b__17_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionValidity"));

            internal bool <SubmitBilling>b__17_12(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperType"));

            internal bool <SubmitBilling>b__17_13(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.gender"));

            internal bool <SubmitBilling>b__17_14(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressType"));

            internal bool <SubmitBilling>b__17_15(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.street"));

            internal bool <SubmitBilling>b__17_16(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.city"));

            internal bool <SubmitBilling>b__17_17(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.houseNumberOrName"));

            internal bool <SubmitBilling>b__17_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.stateOrProvince"));

            internal bool <SubmitBilling>b__17_19(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.country"));

            internal bool <SubmitBilling>b__17_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperReference"));

            internal bool <SubmitBilling>b__17_20(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.postalCode"));

            internal bool <SubmitBilling>b__17_21(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressType"));

            internal bool <SubmitBilling>b__17_22(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.street"));

            internal bool <SubmitBilling>b__17_23(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.city"));

            internal bool <SubmitBilling>b__17_24(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.houseNumberOrName"));

            internal bool <SubmitBilling>b__17_25(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.stateOrProvince"));

            internal bool <SubmitBilling>b__17_26(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.country"));

            internal bool <SubmitBilling>b__17_27(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.postalCode"));

            internal bool <SubmitBilling>b__17_28(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReturnData"));

            internal bool <SubmitBilling>b__17_29(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipBeforeDate"));

            internal bool <SubmitBilling>b__17_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperEmail"));

            internal bool <SubmitBilling>b__17_30(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentAmount"));

            internal bool <SubmitBilling>b__17_31(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReference"));

            internal bool <SubmitBilling>b__17_32(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "skinCode"));

            internal bool <SubmitBilling>b__17_33(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "countryCode"));

            internal bool <SubmitBilling>b__17_34(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currencyCode"));

            internal bool <SubmitBilling>b__17_35(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "resURL"));

            internal bool <SubmitBilling>b__17_36(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantAccount"));

            internal bool <SubmitBilling>b__17_37(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "recurringContract"));

            internal bool <SubmitBilling>b__17_38(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "blockedMethods"));

            internal bool <SubmitBilling>b__17_39(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantSig"));

            internal bool <SubmitBilling>b__17_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthDayOfMonth"));

            internal bool <SubmitBilling>b__17_40(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.sig"));

            internal bool <SubmitBilling>b__17_41(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.data"));

            internal bool <SubmitBilling>b__17_42(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperSig"));

            internal bool <SubmitBilling>b__17_43(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressSig"));

            internal bool <SubmitBilling>b__17_44(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressSig"));

            internal bool <SubmitBilling>b__17_45(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "displayGroup"));

            internal bool <SubmitBilling>b__17_46(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card.cardNumber"));

            internal bool <SubmitBilling>b__17_47(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card.cardHolderName"));

            internal bool <SubmitBilling>b__17_48(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card.cvcCode"));

            internal bool <SubmitBilling>b__17_49(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paypal.storeOcDetails"));

            internal bool <SubmitBilling>b__17_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthMonth"));

            internal bool <SubmitBilling>b__17_50(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sig"));

            internal bool <SubmitBilling>b__17_51(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReference"));

            internal bool <SubmitBilling>b__17_52(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "brandCode"));

            internal bool <SubmitBilling>b__17_53(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentAmount"));

            internal bool <SubmitBilling>b__17_54(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currencyCode"));

            internal bool <SubmitBilling>b__17_55(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipBeforeDate"));

            internal bool <SubmitBilling>b__17_56(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "skinCode"));

            internal bool <SubmitBilling>b__17_57(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantAccount"));

            internal bool <SubmitBilling>b__17_58(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperLocale"));

            internal bool <SubmitBilling>b__17_59(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stage"));

            internal bool <SubmitBilling>b__17_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthYear"));

            internal bool <SubmitBilling>b__17_60(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionId"));

            internal bool <SubmitBilling>b__17_61(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionValidity"));

            internal bool <SubmitBilling>b__17_62(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "countryCode"));

            internal bool <SubmitBilling>b__17_63(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperEmail"));

            internal bool <SubmitBilling>b__17_64(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperReference"));

            internal bool <SubmitBilling>b__17_65(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "recurringContract"));

            internal bool <SubmitBilling>b__17_66(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "resURL"));

            internal bool <SubmitBilling>b__17_67(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "blockedMethods"));

            internal bool <SubmitBilling>b__17_68(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReturnData"));

            internal bool <SubmitBilling>b__17_69(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "originalSession"));

            internal bool <SubmitBilling>b__17_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperLocale"));

            internal bool <SubmitBilling>b__17_70(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.street"));

            internal bool <SubmitBilling>b__17_71(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.houseNumberOrName"));

            internal bool <SubmitBilling>b__17_72(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.city"));

            internal bool <SubmitBilling>b__17_73(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.postalCode"));

            internal bool <SubmitBilling>b__17_74(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.stateOrProvince"));

            internal bool <SubmitBilling>b__17_75(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.country"));

            internal bool <SubmitBilling>b__17_76(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressType"));

            internal bool <SubmitBilling>b__17_77(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressSig"));

            internal bool <SubmitBilling>b__17_78(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.street"));

            internal bool <SubmitBilling>b__17_79(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.houseNumberOrName"));

            internal bool <SubmitBilling>b__17_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.firstName"));

            internal bool <SubmitBilling>b__17_80(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.city"));

            internal bool <SubmitBilling>b__17_81(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.postalCode"));

            internal bool <SubmitBilling>b__17_82(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.stateOrProvince"));

            internal bool <SubmitBilling>b__17_83(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.country"));

            internal bool <SubmitBilling>b__17_84(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressType"));

            internal bool <SubmitBilling>b__17_85(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddressSig"));

            internal bool <SubmitBilling>b__17_86(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.firstName"));

            internal bool <SubmitBilling>b__17_87(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.lastName"));

            internal bool <SubmitBilling>b__17_88(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.gender"));

            internal bool <SubmitBilling>b__17_89(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthDayOfMonth"));

            internal bool <SubmitBilling>b__17_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.LastName"));

            internal bool <SubmitBilling>b__17_90(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthMonth"));

            internal bool <SubmitBilling>b__17_91(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.dateOfBirthYear"));

            internal bool <SubmitBilling>b__17_92(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.telephoneNumber"));

            internal bool <SubmitBilling>b__17_93(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperType"));

            internal bool <SubmitBilling>b__17_94(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperSig"));

            internal bool <SubmitBilling>b__17_95(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.sig"));

            internal bool <SubmitBilling>b__17_96(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.data"));

            internal bool <SubmitBilling>b__17_97(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "encrypted.params"));

            internal bool <SubmitBilling>b__17_98(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "referrerURL"));

            internal bool <SubmitBilling>b__17_99(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dfValue"));

            internal bool <SubmitOrder>b__16_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pageform"));

            internal bool <SubmitOrder>b__16_10(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"));

            internal bool <SubmitOrder>b__16_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"));

            internal bool <SubmitOrder>b__16_12(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"));

            internal bool <SubmitOrder>b__16_13(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"));

            internal bool <SubmitOrder>b__16_14(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"));

            internal bool <SubmitOrder>b__16_15(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"));

            internal bool <SubmitOrder>b__16_16(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"));

            internal bool <SubmitOrder>b__16_17(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"));

            internal bool <SubmitOrder>b__16_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"));

            internal bool <SubmitOrder>b__16_19(HtmlNode x) => 
                ((x.Attributes["action"] != null) && x.Attributes["action"].Value.Contains("https://live.barclaycardsmartpay.com/hpp/pay.shtml"));

            internal bool <SubmitOrder>b__16_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitOrder>b__16_20(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__16_21(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__16_22(HtmlNode x) => 
                ((x.Attributes["action"] != null) && x.Attributes["action"].Value.Contains("https://live.barclaycardsmartpay.com/hpp/pay.shtml"));

            internal bool <SubmitOrder>b__16_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"));

            internal bool <SubmitOrder>b__16_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__16_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <SubmitOrder>b__16_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <SubmitOrder>b__16_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__16_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__16_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitShipping>b__18_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipping_AddressID"));

            internal bool <SubmitShipping>b__18_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaymentServiceSelection"));

            internal bool <SubmitShipping>b__18_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ShippingMethodUUID"));
        }

        [CompilerGenerated]
        private static class <>o__19
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Action<CallSite, HtmlDocument, object>> <>p__2;
        }
    }
}

