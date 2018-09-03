namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpenQA.Selenium;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Footlockerau : IPlatform
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

        public Footlockerau(TaskRunner runner, TaskObject task)
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
                        string url = "https://www.footlocker.com.au/en/addtocart";
                        url = ((((url + "?SynchronizerToken=" + this._synchronizer) + "&Ajax=true" + "&Relay42_Category=Product%20Pages") + "&acctab-tabgroup-" + this._runner.PickedSize.Value.Value + "=null") + "&Quantity_" + this._runner.PickedSize.Value.Value + "=1") + "&SKU=" + this._runner.PickedSize.Value.Value;
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
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
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
            goto Label_0042;
        Label_0017:
            if (-1136271139 || true)
            {
            }
        Label_0042:
            switch (((-902877297 ^ -956278179) % 7))
            {
                case 0:
                    goto Label_0042;

                case 1:
                    return false;

                case 2:
                    break;

                case 3:
                    goto Label_0017;

                case 4:
                    return false;

                case 6:
                    return true;

                default:
                    return this.SubmitOrder();
            }
        Label_0079:
            if (this.SubmitBilling())
            {
            }
            if (-1211133 || true)
            {
                goto Label_0042;
            }
            goto Label_0017;
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
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
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
                if (!this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    return false;
                }
                string url = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection"))).Attributes["data-ajaxcontent-url"].Value;
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
                if (<>o__15.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__2 = CallSite<Action<CallSite, HtmlDocument, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "LoadHtml", null, typeof(Footlockerau), argumentInfo));
                }
                if (<>o__15.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footlockerau), argumentInfo));
                }
                if (<>o__15.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__15.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footlockerau), argumentInfo));
                }
                <>o__15.<>p__2.Target(<>o__15.<>p__2, this._currentDoc, <>o__15.<>p__1.Target(<>o__15.<>p__1, <>o__15.<>p__0.Target(<>o__15.<>p__0, this._dynObj, "content")));
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
                    double num3 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num3 < this._task.MinimumPrice) || (num3 > this._task.MaximumPrice))
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
                            KeyValuePair<string, string> current;
                            while (enumerator2.MoveNext())
                            {
                                current = enumerator2.Current;
                                List<string> source = new List<string>();
                                if (!current.Key.Contains(":"))
                                {
                                    source.Add(current.Key);
                                }
                                else
                                {
                                    char[] chArray2 = new char[] { ':' };
                                    string[] strArray3 = current.Key.Split(chArray2);
                                    for (int k = 0; k < strArray3.Length; k++)
                                    {
                                        source.Add(strArray3[k].Trim());
                                    }
                                }
                                for (int j = 0; j < source.Count; j++)
                                {
                                    source[j] = source[j].Trim().ToUpperInvariant();
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_0A3E;
                                }
                            }
                            continue;
                        Label_0A3E:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(current);
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
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception3)
            {
                this._runner.IsError = true;
                if (exception3 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception3.Message.Contains("430") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("430")))
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
            throw new NotImplementedException();
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, true);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.footlocker.com.au");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.footlocker.com.au");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.79 Safari/537.36");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                this._synchronizer = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "SynchronizerToken"))).Attributes["value"].Value;
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "in_pay_token"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "IsFixed"))).Attributes["value"].Value;
                if (this._task.Payment != TaskObject.PaymentEnum.paypal)
                {
                    bool flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            KeyValuePair<string, HttpResponseHeaders> pair = this._client.Get($"https://www.bpoint.com.au/payments/FOOTLOCKERAUSTRALIA?SynchronizerToken={this._synchronizer}&in_pay_token={str}&IsFixed={str2}").TextHeaders();
                            this._srr = pair.Key;
                            HttpResponseHeaders local1 = pair.Value;
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
                    string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "BillerCode"))).Attributes["value"].Value;
                    string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "CRN1"))).Attributes["value"].Value;
                    string str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "Amount"))).Attributes["value"].Value;
                    string str7 = "";
                    string str8 = "0400bpNfiPCR/AUNf94lis1ztp16088t6Mybnrp/XmcfWoVVgr+Rt2dAZHTHKKElGtacqWYHG919cPZAs3w4eTjAAemnINQu20lpE6D+DwUuc/sxAoWvl5bTSDY887kVsVV4Cd8WIxXHiB6n7s4lwMbUt4S+QGiegQbHW1G32q0vK172li+P5kNt/kxHIeZ050CioloT9rQfawSRgc0X8XMqAmMwfa+Y3/Tg8wof1nuCyfWWsn54eY67j7itCU2gw4IF15362ubh5w+Nn0RPTzA/kNxQl/aUuqWvSdu3DZ2D0kFMgGebJUzGiYRnN5QzfeAGMQSqs60pLstDdd59r5gHspEIHZVXg4ncShzemood3yoKOVTTL2g136yNzGkmK4M7ISKiq4yvoHcgBVMDdOYV83MnEYUvBMAKTEVdGJlxBwCc8acbcKqAuWf7gouzBPJaEMCy0s3hRLlX3uHnT/mMq6Sxn6AVxzxpgEfoQEt47QbkEwIoyXSE46Dl1FsZY8jwYa/hFBBVErqVT31SgfbGSQShQTehJEjtTOqpViCS7gZzTuTNNrR5hMIMrE2Nq1tIurFPMVowBmZWHWP2Z/ZNwV+rdsbATajANTqk0g7LR2HqGsxRE/gfTqOYi6pM6fTkb+FAzdLUY1zx4Gvqf4J14ogJBmPFITryUlTPLhoSRXjCPw/wpHHNoaNiYutQpOa3Yh7EVKwOkEtjQrg+B67vw7kuoE2TiR1KKULPxBs5mS+OqFQy7cgzM+Li8f61EJy8jIUnAGJR8eX+mvvWgPnrWuwTRhqhcaO/5FtYrCru6XTFKVGW7+QWo4QonmwROuiCBJVw9IRDLb0JdqJazYFDT8y3oh8XSPwcnjEsfLF0w492Aku1JAevyMyZh0oClnCJyAfUMZ5LKFoA4BbW41P4v+i7kYvuVLUfVkXwXd4127YB84+POrI4MlDvgCJ6XflMpM5YbymrVY7rLMnUY2Yy4xZkFqaUZegb+KaePAdj0dC1DOkZ9ybRxHxfYV3WeA0UYsMZmVY5fSNRdyw/dDDuGtE5rAP9cxALu6828R9GQm6kHLdnyFIQwcv2RovzmXNT1g9RJPeBNicb7yAKVlYU34/VcdsVtZ270iAXyzfkdDO/TDp0UzLYS+KjA5OTNopRQtncHmePC+1SwejOX5dhKGYrsu13rc4RCbryu9G8AaS2CCqXaO3671cTfDvI9EEy8/hKQSkMTtRUJsjRf0K+pRX/djcjGvvNfDZoXdsLdaOvNZW5LfYHX1k/zztGe52GvzNsubdcjv4Ge9pgWJjBl+5mav9qB7ewlcTtesm7F7fm3+uU8rF9f6LUMZSo//9qfqYAndlMe7bdZoUxykO48vWDDY6js4EVtBghuIcxUUkUUA3pDXHzRR0J0Z3KrDBt6QO53m5a4VI05lyt1veYpHNuv7sghWOEeioIZRnmC0W+ochQziz7ftZJCaKwymxS1wfmZzpno80A2VIT3+Ga4St+ovP0jNDimRWJbwHrzJgsl2RE758GOiqoMhh9mcPyLg81FSfhWxslLNWDPKUtbyQHfzuN4m6EiADt8M16DcJXub41RnJ/CgiXjad65osFSDG+zNqC2yc9rUH4ur27WLd4LjvTDOiKr3Psbd+0neYDQK1N4MtGzsYjNmX7PeQmnL28UjHOmZVf9RHlbCpTN5xEVv/Y0624QH1XIfKOoIprY6b4WQIsDNLTblltSMAwdYH97UXVJ7hRTuID9xOeZCtLBc1Xj+QE3c0saNaIeurO1x5vl4HfsdWTbKnDlXP5LVSIao6gq7cn1x9VUidiGk7VOnzh7v+32n4AQVLjN1AQ8KnLCTB89CyjRviQ3Qg7Q/22EFCpSrC/gUB2sh89yihH/hr6dfTxMcatCFEVI6FwPpPV0ZSMv0MgsLOdHokniOMiA+K9WYcDJ3aXuRT/8OdkKwTX4qCHuOMnLQ0ReYCxN0JKSnrcoQ==";
                    switch (this._runner.Profile.CardTypeId)
                    {
                        case "1":
                            str7 = "visa";
                            break;

                        case "2":
                            str7 = "mastercard";
                            break;
                    }
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            object obj3 = new Newtonsoft.Json.Linq.JObject();
                            if (<>o__13.<>p__0 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BillerCode", typeof(Footlockerau), argumentInfo));
                            }
                            <>o__13.<>p__0.Target(<>o__13.<>p__0, obj3, str4);
                            if (<>o__13.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CRN1", typeof(Footlockerau), argumentInfo));
                            }
                            <>o__13.<>p__1.Target(<>o__13.<>p__1, obj3, str5);
                            if (<>o__13.<>p__2 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Amount", typeof(Footlockerau), argumentInfo));
                            }
                            <>o__13.<>p__2.Target(<>o__13.<>p__2, obj3, str6);
                            if (<>o__13.<>p__3 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CardType", typeof(Footlockerau), argumentInfo));
                            }
                            <>o__13.<>p__3.Target(<>o__13.<>p__3, obj3, str7);
                            if (<>o__13.<>p__4 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DeviceFingerprint", typeof(Footlockerau), argumentInfo));
                            }
                            <>o__13.<>p__4.Target(<>o__13.<>p__4, obj3, str8);
                            this._diData.Clear();
                            if (<>o__13.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__6 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Footlockerau), argumentInfo));
                            }
                            if (<>o__13.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__5 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footlockerau), argumentInfo));
                            }
                            <>o__13.<>p__6.Target(<>o__13.<>p__6, this._diData, "form", <>o__13.<>p__5.Target(<>o__13.<>p__5, typeof(Newtonsoft.Json.JsonConvert), obj3));
                            this._dynObj = this._client.Post("https://www.bpoint.com.au/payments/FOOTLOCKERAUSTRALIA", this._diData).Json();
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
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            if (<>o__13.<>p__8 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footlockerau), argumentInfo));
                            }
                            if (<>o__13.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footlockerau), argumentInfo));
                            }
                            this._srr = this._client.Get($"https://www.bpoint.com.au{<>o__13.<>p__8.Target(<>o__13.<>p__8, <>o__13.<>p__7.Target(<>o__13.<>p__7, this._dynObj, "Data"))}").Text();
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
                    str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "DeviceFingerprint"))).Attributes["value"].Value;
                    string cCNumber = profile.CCNumber;
                    string str10 = "";
                    while (cCNumber.Length > 4)
                    {
                        str10 = str10 + cCNumber.Substring(0, 4);
                        cCNumber = cCNumber.Remove(0, 4);
                        str10 = str10 + " ";
                    }
                    str10 = str10 + cCNumber;
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        this._diData.Clear();
                        this._diData.Add("cardBin", str10.Substring(0, 6));
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.Remove("Origin");
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.bpoint.com.au");
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.bpoint.com.au/payments/FOOTLOCKERAUSTRALIA/Payment/Confirm");
                            this._srr = this._client.Post("https://www.bpoint.com.au/payments/FOOTLOCKERAUSTRALIA/Payment/GetSuchargeDetails", this._diData).Text();
                            continue;
                        }
                        catch (WebException exception4)
                        {
                            if (!exception4.Message.Contains("504") && !exception4.Message.Contains("503"))
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
                    while (flag)
                    {
                        flag = false;
                        this._diData.Clear();
                        this._diData.Add("CardNumber", str10);
                        this._diData.Add("ExpiryMonth", profile.ExpiryMonth);
                        this._diData.Add("ExpiryYear", profile.ExpiryYear.Substring(2));
                        this._diData.Add("CVC", profile.Cvv);
                        this._diData.Add("DeviceFingerprint", str8);
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                            this._client.Session.DefaultRequestHeaders.Remove("Origin");
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.bpoint.com.au");
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.bpoint.com.au/payments/FOOTLOCKERAUSTRALIA/Payment/Confirm");
                            this._srr = this._client.Post("https://www.bpoint.com.au/payments/FOOTLOCKERAUSTRALIA/Payment/Confirm", this._diData).Text();
                            continue;
                        }
                        catch (WebException exception5)
                        {
                            if (!exception5.Message.Contains("504") && !exception5.Message.Contains("503"))
                            {
                                throw;
                            }
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                            continue;
                        }
                    }
                    return true;
                }
                this._task.Driver.Navigate().GoToUrl($"https://www.bpoint.com.au/payments/FOOTLOCKERAUSTRALIA?SynchronizerToken={this._synchronizer}&in_pay_token={str}&IsFixed={str2}");
                if (profile.CardTypeId == "2")
                {
                    this._task.Driver.FindElement(By.ClassName("mastercard")).Click();
                }
                else
                {
                    this._task.Driver.FindElement(By.ClassName("visa")).Click();
                }
                this._task.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5.0);
                IWebElement element1 = this._task.Driver.FindElement(By.Name("CardNumber"));
                element1.Clear();
                element1.SendKeys(profile.CCNumber);
                IWebElement element2 = this._task.Driver.FindElement(By.Name("CVC"));
                element2.Clear();
                element2.SendKeys(profile.Cvv);
                IWebElement element3 = this._task.Driver.FindElement(By.Name("ExpiryMonth"));
                element3.Clear();
                element3.SendKeys(profile.ExpiryMonth);
                IWebElement element4 = this._task.Driver.FindElement(By.Name("ExpiryYear"));
                element4.Clear();
                element4.SendKeys(profile.ExpiryYear.Substring(2));
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception6)
            {
                this._runner.IsError = true;
                if (exception6 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception6.Message.Contains("430") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception6, "", "");
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
            bool flag;
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
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
                string str18 = "";
                if (!this._srr.Contains("mastercard.com"))
                {
                    goto Label_14D3;
                }
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_AccessCode"))).Attributes["value"].Value;
                string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Amount"))).Attributes["value"].Value;
                string str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_CardExp"))).Attributes["value"].Value;
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_CardNum"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_CardSecurityCode"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Command"))).Attributes["value"].Value;
                string str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Locale"))).Attributes["value"].Value;
                string str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_MerchTxnRef"))).Attributes["value"].Value;
                string str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Merchant"))).Attributes["value"].Value;
                string str14 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_OrderInfo"))).Attributes["value"].Value;
                string str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_ReturnURL"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Version"))).Attributes["value"].Value;
                string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_card"))).Attributes["value"].Value;
                string str17 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_gateway"))).Attributes["value"].Value;
                string str16 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_SecureHash"))).Attributes["value"].Value;
                string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_SecureHashType"))).Attributes["value"].Value;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("vpc_AccessCode", str7);
                        this._diData.Add("vpc_Amount", str8);
                        this._diData.Add("vpc_CardExp", str11);
                        this._diData.Add("vpc_CardNum", str);
                        this._diData.Add("vpc_CardSecurityCode", str2);
                        this._diData.Add("vpc_Command", str3);
                        this._diData.Add("vpc_Locale", str10);
                        this._diData.Add("vpc_MerchTxnRef", str12);
                        this._diData.Add("vpc_Merchant", str13);
                        this._diData.Add("vpc_OrderInfo", str14);
                        this._diData.Add("vpc_ReturnURL", str15);
                        this._diData.Add("vpc_Version", str4);
                        this._diData.Add("vpc_card", str5);
                        this._diData.Add("vpc_gateway", str17);
                        this._diData.Add("vpc_SecureHash", str16);
                        this._diData.Add("vpc_SecureHashType", str9);
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
                url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PAReq"))).Attributes["action"].Value;
                string str19 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                string str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"))).Attributes["value"].Value;
                string str21 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("PaReq", str19);
                        this._diData.Add("TermUrl", str20);
                        this._diData.Add("MD", str21);
                        this._srr = this._client.Post(url, this._diData).Text();
                        continue;
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
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm")))
                {
                    url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"))).Attributes["action"].Value;
                    string str22 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                    str21 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    str19 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                    string str23 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"))).Attributes["value"].Value;
                    string str24 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"))).Attributes["value"].Value;
                    string str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"))).Attributes["value"].Value;
                    string str28 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"))).Attributes["value"].Value;
                    string str29 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"))).Attributes["value"].Value;
                    string str25 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"))).Attributes["value"].Value;
                    string str31 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"))).Attributes["value"].Value;
                    string str30 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"))).Attributes["value"].Value;
                    string str26 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"))).Attributes["value"].Value;
                    flag2 = true;
                    while (flag2)
                    {
                        flag2 = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("PaRes", str22);
                            this._diData.Add("MD", str21);
                            this._diData.Add("PaReq", str19);
                            this._diData.Add("ABSlog", str23);
                            this._diData.Add("deviceDNA", str24);
                            this._diData.Add("executionTime", str27);
                            this._diData.Add("dnaError", str28);
                            this._diData.Add("mesc", str29);
                            this._diData.Add("mescIterationCount", str25);
                            this._diData.Add("desc", str31);
                            this._diData.Add("isDNADone", str30);
                            this._diData.Add("arcotFlashCookie", str26);
                            KeyValuePair<string, string> pair = this._client.Post(url, this._diData).TextResponseUri();
                            str18 = pair.Value;
                            this._srr = pair.Key;
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
                    goto Label_145F;
                }
                if (this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes")))
                {
                    goto Label_12C6;
                }
                url = this._currentDoc.DocumentNode.Descendants("iframe").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "tmxFrame"))).Attributes["src"].Value;
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._srr = this._client.Get(url).Text();
                        continue;
                    }
                    catch (WebException exception4)
                    {
                        if (!exception4.Message.Contains("504") && !exception4.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag2 = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "submitForm"))).Attributes["action"].Value;
                bool flag3 = true;
            Label_11B1:
                if (flag3)
                {
                    flag3 = false;
                    flag2 = true;
                    while (flag2)
                    {
                        flag2 = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("processing", "true");
                            this._srr = this._client.Post(url, this._diData).Text();
                        }
                        catch (WebException exception5)
                        {
                            if (!exception5.Message.Contains("504") && !exception5.Message.Contains("503"))
                            {
                                throw;
                            }
                            flag2 = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        continue;
                    Label_125C:
                        this._currentDoc.LoadHtml(this._srr);
                        if (flag3 = this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "processing")))
                        {
                            Thread.Sleep(200);
                        }
                        goto Label_11B1;
                    }
                    goto Label_125C;
                }
            Label_12C6:
                url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                string str32 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                str21 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("PaRes", str32);
                        this._diData.Add("MD", str21);
                        KeyValuePair<string, string> pair2 = this._client.Post(url, this._diData).TextResponseUri();
                        str18 = pair2.Value;
                        this._srr = pair2.Key;
                        continue;
                    }
                    catch (WebException exception6)
                    {
                        if (!exception6.Message.Contains("504") && !exception6.Message.Contains("503"))
                        {
                            throw;
                        }
                        flag2 = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
            Label_145F:
                if (str18.ToLowerInvariant().Contains("paymentfailed"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                try
                {
                    EveAIO.Helpers.AddDbValue("Footlocker AU|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    goto Label_14DF;
                }
                catch
                {
                    goto Label_14DF;
                }
            Label_14D3:
                throw new Exception("Error processing payment");
            Label_14DF:
                flag = true;
            }
            catch (ThreadAbortException)
            {
                flag = false;
            }
            catch (Exception exception7)
            {
                if (!this._srr.ToLowerInvariant().Contains("paymentfailed"))
                {
                    this._runner.IsError = true;
                    if (exception7 is AggregateException)
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                    }
                    else if (!exception7.Message.Contains("430") && ((exception7.InnerException == null) || !exception7.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception7, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                    return false;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                flag = false;
            }
            return flag;
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
                        this._srr = this._client.Get($"https://www.footlocker.com.au/INTERSHOP/web/WFS/FootlockerAustraliaPacific-Footlocker_AU-Site/en_AU/-/AUD/ViewCart-Checkout?SynchronizerToken={this._synchronizer}").Text();
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
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get($"https://www.footlocker.com.au/en/checkout-overview?SynchronizerToken={this._synchronizer}").Text();
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
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipping_AddressID"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaymentServiceSelection"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ShippingMethodUUID"))).Attributes["value"].Value;
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
                        this._diData.Add("billing_CompanyName", "");
                        this._diData.Add("billing_CountryCode", "AU");
                        this._diData.Add("billing_Address1", profile.Address1);
                        this._diData.Add("billing_Address2", profile.Address2);
                        this._diData.Add("billing_City", profile.City);
                        this._diData.Add("billing_PostalCode", profile.Zip);
                        this._diData.Add("billing_State", profile.StateId);
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
                        this._diData.Add("shipping_CompanyName", "");
                        this._diData.Add("shipping_CountryCode", "AU");
                        this._diData.Add("shipping_Address1", profile.Address1Shipping);
                        this._diData.Add("shipping_Address2", profile.Address2Shipping);
                        this._diData.Add("shipping_City", profile.CityShipping);
                        this._diData.Add("shipping_PostalCode", profile.ZipShipping);
                        this._diData.Add("shipping_State", profile.StateIdShipping);
                        this._diData.Add("shipping_PhoneHome", profile.PhoneShipping);
                        this._diData.Add("shipping_AddressID", str);
                        this._diData.Add("CheckoutRegisterForm_Password", "");
                        this._diData.Add("promotionCode", "");
                        this._diData.Add("PaymentServiceSelection", str2);
                        this._diData.Add("UserDeviceTypeForPaymentRedirect", "Desktop");
                        this._diData.Add("UserDeviceFingerprintForPaymentRedirect", "0400bpNfiPCR/AUNf94lis1ztp16088t6Mybnrp/XmcfWoVVgr+Rt2dAZHTHKKElGtacqWYHG919cPZAs3w4eTjAAemnINQu20lpE6D+DwUuc/sxAoWvl5bTSDY887kVsVV4Cd8WIxXHiB6n7s4lwMbUt4S+QGiegQbHW1G32q0vK172li+P5kNt/kxHIeZ050CioloT9rQfawSRgc0X8XMqAmMwfa+Y3/Tg8wof1nuCyfWWsn54eY67j7itCU2gw4IF15362ubh5w+Nn0RPTzA/kNxQl/aUuqWvSdu3DZ2D0kFMgGebJUzGiYRnN5QzfeAGMQSqs60pLstDdd59r5gHspEIHZVXg4ncShzemood3yoKOVTTL2g136yNzGkmK4M7ISKiq4yvoHcgBVMDdOYV83MnEYUvBMAKTEVdGJlxBwCc8acbcKqAuWf7gouzBPJaEMCy0s3hRLlX3uHnT/mMq6Sxn6AVxzxpgEfoQEt47QbkEwIoyXSE46Dl1FsZY8jwYa/hFBBVErqVT31SgfbGSQShQTehJEjtTOqpViCS7gZzTuTNNrR5hMIMrE2Nq1tIurFPMVowBmZWHWP2Z/ZNwV+rdsbATajANTqk0g7LR2HqGsxRE/gfTqOYi6pM6fTkb+FAzdLUY1zx4Gvqf4J14ogJBmPFITryUlTPLhoSRXjCPw/wpHHNoaNiYutQpOa3Yh7EVKwOkEtjQrg+B67vw7kuoE2TiR1KKULPxBs5mS+OqFQy7cgzM+Li8f61EJy8jIUnAGJR8eX+mvvWgPnrWuwTRhqhcaO/5FtYrCru6XTFKVGW7+QWo4QonmwROuiCBJVw9IRDLb0JdqJazYFDT8y3oh8XSPwcnjEsfLF0w492Aku1JAevyMyZh0oClnCJyAfUMZ5LKFoA4BbW41P4v+i7kYvuVLUfVkXwXd4127YB84+POrI4MlDvgCJ6XflMpM5YbymrVY7rLMnUY2Yy4xZkFqaUZegb+KaePAdj0dC1DOkZ9ybRxHxfYV3WeA0UYsMZmVY5fSNRdyw/dDDuGtE5rAP9cxALu6828R9GQm6kHLdnyFIQwcv2RovzmXNT1g9RJPeBNicb7yAKVlYU34/VcdsVtZ270iAXyzfkdDO/TDp0UzLYS+KjA5OTNopRQtncHmePC+1SwejOX5dhKGYrsu13rc4RCbryu9G8AaS2CCqXaO3671cTfDvI9EEy8/hKQSkMTtRUJsjRf0K+pRX/djcjGvvNfDZoXdsLdaOvNZW5LfYHX1k/zztGe52GvzNsubdcjv4Ge9pgWJjBl+5mav9qB7ewlcTtesm7F7fm3+uU8rF9f6LUMZSo//9qfqYAndlMe7bdZoUxykO48vWDDY6js4EVtBghuIcxUUkUUA3pDXHzRR0J0Z3KrDBt6QO53m5a4VI05lyt1veYpHNuv7sghWOEeioIZRnmC0W+ochQziz7ftZJCaKwymxS1wfmZzpno80A2VIT3+Ga4St+ovP0jNDimRWJbwHrzJgsl2RE758GOiqoMhh9mcPyLg81FSfhWxslLNWDPKUtbyQHfzuN4m6EiADt8M16DcJXub41RnJ/CgiXjad65osFSDG+zNqC2yc9rUH4ur27WLd4LjvTDOiKr3Psbd+0neYDQK1N4MtGzsYjNmX7PeQmnL28UjHOmZVf9RHlbCpTN5xEVv/Y0624QH1XIfKOoIprY6b4WQIsDNLTblltSMAwdYH97UXVJ7hRTuID9xOeZCtLBc1Xj+QE3c0saNaIeurO1x5vl4HfsdWTbKnDlXP5LVSIao6gq7cn1x9VUidiGk7VOnzh7v+32n4AQVLjN1AQ8KnLCTB89CyjRviQ3Qg7Q/22EFCpSrC/gUB2sh89yihH/hr6dfTxMcatCFEVI6FwPpPV0ZSMv0MgsLOdHokniOMiA+K9WYcDJ3aXuRT/8OdkKwTX4qCHuOMnLQ0ReYCxN0JKSnrcoQ==");
                        this._diData.Add("ShippingMethodUUID", str3);
                        this._diData.Add("termsAndConditions", "on");
                        this._diData.Add("email_Newsletter", "true");
                        this._diData.Add("sendOrder", "");
                        this._srr = this._client.Post("https://www.footlocker.com.au/INTERSHOP/web/WFS/FootlockerAustraliaPacific-Footlocker_AU-Site/en_AU/-/AUD/ViewCheckoutOverview-Dispatch", this._diData).Text();
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
                if (!this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "BPointPaymentRedirectForm"))))
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
                if (exception4 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
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
            public static readonly Footlockerau.<>c <>9;
            public static Func<HtmlNode, bool> <>9__12_1;
            public static Func<HtmlNode, bool> <>9__12_2;
            public static Func<HtmlNode, bool> <>9__12_3;
            public static Func<HtmlNode, bool> <>9__12_4;
            public static Func<HtmlNode, bool> <>9__12_5;
            public static Func<HtmlNode, bool> <>9__12_6;
            public static Func<HtmlNode, bool> <>9__12_7;
            public static Func<HtmlNode, bool> <>9__12_8;
            public static Func<HtmlNode, bool> <>9__12_9;
            public static Func<HtmlNode, bool> <>9__12_10;
            public static Func<HtmlNode, bool> <>9__12_11;
            public static Func<HtmlNode, bool> <>9__12_12;
            public static Func<HtmlNode, bool> <>9__12_13;
            public static Func<HtmlNode, bool> <>9__12_14;
            public static Func<HtmlNode, bool> <>9__12_15;
            public static Func<HtmlNode, bool> <>9__12_16;
            public static Func<HtmlNode, bool> <>9__12_17;
            public static Func<HtmlNode, bool> <>9__12_18;
            public static Func<HtmlNode, bool> <>9__12_19;
            public static Func<HtmlNode, bool> <>9__12_20;
            public static Func<HtmlNode, bool> <>9__12_21;
            public static Func<HtmlNode, bool> <>9__12_22;
            public static Func<HtmlNode, bool> <>9__12_23;
            public static Func<HtmlNode, bool> <>9__12_24;
            public static Func<HtmlNode, bool> <>9__12_25;
            public static Func<HtmlNode, bool> <>9__12_26;
            public static Func<HtmlNode, bool> <>9__12_27;
            public static Func<HtmlNode, bool> <>9__12_28;
            public static Func<HtmlNode, bool> <>9__12_29;
            public static Func<HtmlNode, bool> <>9__12_30;
            public static Func<HtmlNode, bool> <>9__12_31;
            public static Func<HtmlNode, bool> <>9__12_32;
            public static Func<HtmlNode, bool> <>9__12_33;
            public static Func<HtmlNode, bool> <>9__12_34;
            public static Func<HtmlNode, bool> <>9__12_35;
            public static Func<HtmlNode, bool> <>9__12_38;
            public static Func<HtmlNode, bool> <>9__12_39;
            public static Func<HtmlNode, bool> <>9__12_40;
            public static Func<HtmlNode, bool> <>9__12_36;
            public static Func<HtmlNode, bool> <>9__12_37;
            public static Func<HtmlNode, bool> <>9__13_0;
            public static Func<HtmlNode, bool> <>9__13_1;
            public static Func<HtmlNode, bool> <>9__13_2;
            public static Func<HtmlNode, bool> <>9__13_3;
            public static Func<HtmlNode, bool> <>9__13_4;
            public static Func<HtmlNode, bool> <>9__13_5;
            public static Func<HtmlNode, bool> <>9__13_6;
            public static Func<HtmlNode, bool> <>9__14_0;
            public static Func<HtmlNode, bool> <>9__14_1;
            public static Func<HtmlNode, bool> <>9__14_2;
            public static Func<HtmlNode, bool> <>9__14_3;
            public static Func<HtmlNode, bool> <>9__15_0;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__15_2;
            public static Func<HtmlNode, bool> <>9__15_3;
            public static Func<HtmlNode, bool> <>9__15_4;
            public static Func<HtmlNode, bool> <>9__15_5;
            public static Func<HtmlNode, bool> <>9__15_6;
            public static Func<HtmlNode, bool> <>9__15_8;
            public static Func<HtmlNode, bool> <>9__15_9;
            public static Func<HtmlNode, bool> <>9__15_7;
            public static Func<HtmlNode, bool> <>9__15_10;
            public static Func<HtmlNode, bool> <>9__15_11;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Footlockerau.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__15_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "SynchronizerToken"));

            internal bool <DirectLink>b__15_1(HtmlNode x) => 
                ((x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection"));

            internal bool <DirectLink>b__15_10(HtmlNode x) => 
                (x.InnerText.Trim() == "Other");

            internal bool <DirectLink>b__15_11(HtmlNode x) => 
                ((x.Attributes["data-form-field-target"] != null) && (x.Attributes["data-form-field-target"].Value == "SKU"));

            internal bool <DirectLink>b__15_2(HtmlNode x) => 
                ((x.Attributes["data-ajaxcontent"] != null) && (x.Attributes["data-ajaxcontent"].Value == "fl-productDetailsSizeSelection"));

            internal bool <DirectLink>b__15_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLink>b__15_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__15_5(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"));

            internal bool <DirectLink>b__15_6(HtmlNode x) => 
                (x.InnerText.Trim() == "US");

            internal bool <DirectLink>b__15_7(HtmlNode x) => 
                (x.InnerText.Trim() == "Other");

            internal bool <DirectLink>b__15_8(HtmlNode x) => 
                (x.InnerText.Trim() == "US");

            internal bool <DirectLink>b__15_9(HtmlNode x) => 
                ((x.Attributes["data-form-field-target"] != null) && (x.Attributes["data-form-field-target"].Value == "SKU"));

            internal bool <SubmitBilling>b__13_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "SynchronizerToken"));

            internal bool <SubmitBilling>b__13_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "in_pay_token"));

            internal bool <SubmitBilling>b__13_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "IsFixed"));

            internal bool <SubmitBilling>b__13_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "BillerCode"));

            internal bool <SubmitBilling>b__13_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "CRN1"));

            internal bool <SubmitBilling>b__13_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "Amount"));

            internal bool <SubmitBilling>b__13_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "DeviceFingerprint"));

            internal bool <SubmitOrder>b__12_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_AccessCode"));

            internal bool <SubmitOrder>b__12_10(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_OrderInfo"));

            internal bool <SubmitOrder>b__12_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_ReturnURL"));

            internal bool <SubmitOrder>b__12_12(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Version"));

            internal bool <SubmitOrder>b__12_13(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_card"));

            internal bool <SubmitOrder>b__12_14(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_gateway"));

            internal bool <SubmitOrder>b__12_15(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_SecureHash"));

            internal bool <SubmitOrder>b__12_16(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_SecureHashType"));

            internal bool <SubmitOrder>b__12_17(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PAReq"));

            internal bool <SubmitOrder>b__12_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitOrder>b__12_19(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"));

            internal bool <SubmitOrder>b__12_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Amount"));

            internal bool <SubmitOrder>b__12_20(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__12_21(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <SubmitOrder>b__12_22(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <SubmitOrder>b__12_23(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__12_24(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__12_25(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitOrder>b__12_26(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"));

            internal bool <SubmitOrder>b__12_27(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"));

            internal bool <SubmitOrder>b__12_28(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"));

            internal bool <SubmitOrder>b__12_29(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"));

            internal bool <SubmitOrder>b__12_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_CardExp"));

            internal bool <SubmitOrder>b__12_30(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"));

            internal bool <SubmitOrder>b__12_31(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"));

            internal bool <SubmitOrder>b__12_32(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"));

            internal bool <SubmitOrder>b__12_33(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"));

            internal bool <SubmitOrder>b__12_34(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"));

            internal bool <SubmitOrder>b__12_35(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__12_36(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__12_37(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__12_38(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "tmxFrame"));

            internal bool <SubmitOrder>b__12_39(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "submitForm"));

            internal bool <SubmitOrder>b__12_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_CardNum"));

            internal bool <SubmitOrder>b__12_40(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "processing"));

            internal bool <SubmitOrder>b__12_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_CardSecurityCode"));

            internal bool <SubmitOrder>b__12_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Command"));

            internal bool <SubmitOrder>b__12_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Locale"));

            internal bool <SubmitOrder>b__12_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_MerchTxnRef"));

            internal bool <SubmitOrder>b__12_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "vpc_Merchant"));

            internal bool <SubmitShipping>b__14_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipping_AddressID"));

            internal bool <SubmitShipping>b__14_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaymentServiceSelection"));

            internal bool <SubmitShipping>b__14_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ShippingMethodUUID"));

            internal bool <SubmitShipping>b__14_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "BPointPaymentRedirectForm"));
        }

        [CompilerGenerated]
        private static class <>o__13
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__5;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
        }

        [CompilerGenerated]
        private static class <>o__15
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Action<CallSite, HtmlDocument, object>> <>p__2;
        }
    }
}

