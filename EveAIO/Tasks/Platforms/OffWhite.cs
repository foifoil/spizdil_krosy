namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Cloudflare;
    using EveAIO.Pocos;
    using EveAIO.Properties;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class OffWhite : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        private string _data;
        [Dynamic]
        private object _dynObj;
        private string _xpid;
        private object _payToken;
        private string _payLink;
        private string _geo;
        private Dictionary<string, string> _diData;

        public OffWhite(TaskRunner runner, TaskObject task)
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
                this._request = (HttpWebRequest) WebRequest.Create($"{this._geo}orders/populate.json");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Referer = this._runner.Product.Link;
                this._request.Headers.Add("Origin", "https://www.off---white.com");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                if (!string.IsNullOrEmpty(this._xpid))
                {
                    this._request.Headers.Add("X-NewRelic-ID", this._xpid);
                }
                this._data = "utf8=%E2%9C%93&authenticity_token=&variant_id=" + this._runner.PickedSize.Value.Value + "&quantity=1";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                try
                {
                    using (WebResponse response = this._request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            this._srr = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException exception1)
                {
                    if (!exception1.Message.Contains("422"))
                    {
                        throw;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    return false;
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__16.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__16.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(OffWhite), argumentInfo));
                }
                if (<>o__16.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__16.<>p__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(OffWhite), argumentInfo));
                }
                if (<>o__16.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__16.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                }
                if (<>o__16.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__16.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                }
                if (<>o__16.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__16.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "cart", typeof(OffWhite), argumentInfo));
                }
                if (<>o__16.<>p__4.Target(<>o__16.<>p__4, <>o__16.<>p__3.Target(<>o__16.<>p__3, <>o__16.<>p__2.Target(<>o__16.<>p__2, <>o__16.<>p__1.Target(<>o__16.<>p__1, <>o__16.<>p__0.Target(<>o__16.<>p__0, this._dynObj), "item_count")), 1)))
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
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", "");
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
                goto Label_004B;
            }
        Label_001C:
            switch (((0x48d3e81a ^ 0x578aef1f) % 5))
            {
                case 0:
                    return false;

                case 1:
                    break;

                case 2:
                    return false;

                case 3:
                    goto Label_001C;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (this.SubmitBilling())
            {
            }
            if (0x5ea58ca6 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            try
            {
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.SOLVING_CLOUDFLARE, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.SOLVING_CLOUDFLARE);
                    string str = link.Replace("http://", "").Replace("https://", "");
                    str = str.Substring(str.IndexOf("/") + 1);
                    this._geo = "https://www.off---white.com/" + str.Substring(0, str.IndexOf("/") + 1);
                    str = str.Substring(str.IndexOf("/") + 1);
                    this._geo = this._geo + str.Substring(0, str.IndexOf("/") + 1);
                }
                try
                {
                    this._runner._dontRotateProxy = false;
                    this._request = (HttpWebRequest) WebRequest.Create(link);
                    if (this._runner.Proxy != null)
                    {
                        this._request.Proxy = this._runner.Proxy;
                    }
                    this._request.CookieContainer = this._runner.Cookies;
                    this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                    this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                    this._request.KeepAlive = true;
                    this._request.Headers.Add("Accept-Language", "en-US");
                    using (WebResponse response = this._request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            this._srr = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException exception)
                {
                    if (exception.Message.Contains("503"))
                    {
                        this._runner._dontRotateProxy = true;
                        this._runner.Cookies = new CookieContainer();
                        this._runner.SetProxy();
                        Uri requestUri = new Uri(link);
                        ClearanceHandler handler = new ClearanceHandler(this._runner.Proxy) {
                            MaxRetries = 20
                        };
                        HttpClient client = new HttpClient(handler);
                        try
                        {
                            CookieContainer container = handler._cookies;
                            this._srr = client.GetStringAsync(requestUri).Result;
                            this._runner.Cookies = container;
                        }
                        catch (AggregateException) when ((exception.InnerException is CloudFlareClearanceException))
                        {
                            Console.WriteLine(exception.InnerException.Message);
                        }
                    }
                    else
                    {
                        if (!exception.Message.Contains("403"))
                        {
                            throw;
                        }
                        this._runner._dontRotateProxy = true;
                        HtmlDocument document = new HtmlDocument();
                        if (exception.Response != null)
                        {
                            string html = "";
                            using (Stream stream = exception.Response.GetResponseStream())
                            {
                                using (StreamReader reader2 = new StreamReader(stream))
                                {
                                    html = reader2.ReadToEnd();
                                }
                            }
                            document.LoadHtml(html);
                            if (!document.DocumentNode.InnerHtml.Contains("data-sitekey") && (document.DocumentNode.InnerHtml.Contains("ERR_ACCESS_DENIED") || exception.Message.Contains("(403)")))
                            {
                                this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                                States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                                return false;
                            }
                            string str3 = document.DocumentNode.Descendants("script").First<HtmlNode>(x => (x.Attributes["data-sitekey"] > null)).Attributes["data-ray"].Value;
                            object solverLocker = Global.SolverLocker;
                            lock (solverLocker)
                            {
                                Global.CAPTCHA_QUEUE.Add(this._task);
                            }
                            this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                            States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                            this._task.Mre = new ManualResetEvent(false);
                            CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.OW_CAPTCHA_KEY, "https://www.off---white.com", "OW");
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
                            this._request = (HttpWebRequest) WebRequest.Create("https://www.off---white.com/cdn-cgi/l/chk_captcha?id=" + str3 + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token));
                            this._request.KeepAlive = true;
                            this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                            this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                            this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                            this._request.Referer = "https://www.off---white.com";
                            this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            this._request.CookieContainer = this._runner.Cookies;
                            this._request.Referer = link;
                            if (this._runner.Proxy != null)
                            {
                                this._request.Proxy = this._runner.Proxy;
                            }
                            using (WebResponse response2 = this._request.GetResponse())
                            {
                                using (StreamReader reader3 = new StreamReader(response2.GetResponseStream()))
                                {
                                    this._srr = reader3.ReadToEnd();
                                }
                                goto Label_064A;
                            }
                        }
                        throw exception;
                    }
                }
            Label_064A:
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", link);
                }
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                this._currentDoc.LoadHtml(this._srr);
                if (this._srr.Contains("_xpid"))
                {
                    this._xpid = this._srr.Substring(this._srr.IndexOf("xpid"));
                    this._xpid = this._xpid.Substring(this._xpid.IndexOf("\"") + 1);
                    this._xpid = this._xpid.Substring(0, this._xpid.IndexOf("\""));
                }
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str4 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "prod-title"))).InnerText.Trim();
                string str5 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).Attributes["content"].Value;
                string str6 = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"))).Attributes["content"].Value;
                this._task.ImgUrl = str6;
                Product product1 = new Product {
                    ProductTitle = str4,
                    Link = link,
                    Price = str5
                };
                this._runner.Product = product1;
                if (!this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-cart-form js-cart-form"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                HtmlNode node = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-cart-form js-cart-form"));
                foreach (HtmlNode node2 in node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "styled-radio"))).Descendants("input"))
                {
                    string id = node2.Attributes["id"].Value;
                    this._runner.Product.AvailableSizes.Add(new KeyValuePair<string, string>(node.Descendants("label").First<HtmlNode>(x => ((x.Attributes["for"] != null) && (x.Attributes["for"].Value == id))).InnerText.Trim(), node2.Attributes["value"].Value));
                }
                if (this._runner.Product.AvailableSizes.Count == 0)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
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
                    string str7 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str7 = str7 + ch.ToString();
                        }
                    }
                    double num2 = double.Parse(str7.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num2 < this._task.MinimumPrice) || (num2 > this._task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        this._runner.Product = null;
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
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
                        if (this._runner.PickedSize.HasValue)
                        {
                            break;
                        }
                        using (List<KeyValuePair<string, string>>.Enumerator enumerator2 = this._runner.Product.AvailableSizes.GetEnumerator())
                        {
                            KeyValuePair<string, string> current;
                            while (enumerator2.MoveNext())
                            {
                                int num5;
                                current = enumerator2.Current;
                                List<string> source = new List<string>();
                                if (!current.Key.Contains(":"))
                                {
                                    goto Label_0C9F;
                                }
                                char[] chArray2 = new char[] { ':' };
                                string[] strArray3 = current.Key.Split(chArray2);
                                int index = 0;
                                goto Label_0C95;
                            Label_0C7E:
                                source.Add(strArray3[index].Trim());
                                index++;
                            Label_0C95:
                                if (index >= strArray3.Length)
                                {
                                    goto Label_0CAD;
                                }
                                goto Label_0C7E;
                            Label_0C9F:
                                source.Add(current.Key);
                            Label_0CAD:
                                num5 = 0;
                                while (num5 < source.Count)
                                {
                                    source[num5] = source[num5].Trim().ToUpperInvariant();
                                    num5++;
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_0D1A;
                                }
                            }
                            continue;
                        Label_0D1A:
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
            catch (Exception exception2)
            {
                this._runner.Cookies = new CookieContainer();
                this._runner.IsError = true;
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    if (!exception2.Message.Contains("404") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("404")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception2, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                        States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                    }
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
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.SOLVING_CLOUDFLARE, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SOLVING_CLOUDFLARE);
                List<string> source = new List<string>();
                string str = this._task.Link.Replace("http://", "").Replace("https://", "");
                str = str.Substring(str.IndexOf("/") + 1);
                this._geo = "https://www.off---white.com/" + str.Substring(0, str.IndexOf("/") + 1);
                str = str.Substring(str.IndexOf("/") + 1);
                this._geo = this._geo + str.Substring(0, str.IndexOf("/") + 1);
                foreach (string str2 in this._task.Keywords)
                {
                    string requestUriString = $"{this._geo}search?utf8=%E2%9C%93&q={str2.Replace(" ", "+").ToLowerInvariant()}";
                    try
                    {
                        this._runner._dontRotateProxy = false;
                        this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        this._request.CookieContainer = this._runner.Cookies;
                        this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                        this._request.KeepAlive = true;
                        this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        using (WebResponse response = this._request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                this._srr = reader.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException exception)
                    {
                        if (exception.Message.Contains("503"))
                        {
                            this._runner._dontRotateProxy = true;
                            this._runner.Cookies = new CookieContainer();
                            this._runner.SetProxy();
                            Uri requestUri = new Uri(requestUriString);
                            ClearanceHandler handler = new ClearanceHandler(this._runner.Proxy) {
                                MaxRetries = 20
                            };
                            HttpClient client = new HttpClient(handler);
                            try
                            {
                                CookieContainer container = handler._cookies;
                                this._srr = client.GetStringAsync(requestUri).Result;
                                this._runner.Cookies = container;
                            }
                            catch (AggregateException) when ((exception.InnerException is CloudFlareClearanceException))
                            {
                                Console.WriteLine(exception.InnerException.Message);
                            }
                        }
                        else
                        {
                            if (!exception.Message.Contains("403"))
                            {
                                throw;
                            }
                            this._runner._dontRotateProxy = true;
                            HtmlDocument document = new HtmlDocument();
                            if (exception.Response != null)
                            {
                                string html = "";
                                using (Stream stream = exception.Response.GetResponseStream())
                                {
                                    using (StreamReader reader2 = new StreamReader(stream))
                                    {
                                        html = reader2.ReadToEnd();
                                    }
                                }
                                document.LoadHtml(html);
                                if (!document.DocumentNode.InnerHtml.Contains("data-sitekey") && (document.DocumentNode.InnerHtml.Contains("ERR_ACCESS_DENIED") || exception.Message.Contains("(403)")))
                                {
                                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                                    return false;
                                }
                                string str5 = document.DocumentNode.Descendants("script").First<HtmlNode>(x => (x.Attributes["data-sitekey"] > null)).Attributes["data-ray"].Value;
                                object solverLocker = Global.SolverLocker;
                                lock (solverLocker)
                                {
                                    Global.CAPTCHA_QUEUE.Add(this._task);
                                }
                                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                                States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                                this._task.Mre = new ManualResetEvent(false);
                                CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.OW_CAPTCHA_KEY, "https://www.off---white.com", "OW");
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
                                this._request = (HttpWebRequest) WebRequest.Create("https://www.off---white.com/cdn-cgi/l/chk_captcha?id=" + str5 + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token));
                                this._request.KeepAlive = true;
                                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                                this._request.Referer = "https://www.off---white.com";
                                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                                this._request.CookieContainer = this._runner.Cookies;
                                this._request.Referer = requestUriString;
                                if (this._runner.Proxy != null)
                                {
                                    this._request.Proxy = this._runner.Proxy;
                                }
                                using (WebResponse response2 = this._request.GetResponse())
                                {
                                    using (StreamReader reader3 = new StreamReader(response2.GetResponseStream()))
                                    {
                                        this._srr = reader3.ReadToEnd();
                                    }
                                    goto Label_0682;
                                }
                            }
                            throw exception;
                        }
                    }
                Label_0682:
                    this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                    HtmlDocument document1 = new HtmlDocument();
                    document1.LoadHtml(this._srr);
                    foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("a")
                        where (x.Attributes["itemProp"] != null) && (x.Attributes["itemProp"].Value == "url")
                        select x)
                    {
                        string productLink = "https://www.off---white.com" + node.Attributes["href"].Value;
                        if (!source.Any<string>(x => (x == productLink)))
                        {
                            if (this.DirectLink(productLink))
                            {
                                return true;
                            }
                            source.Add(productLink);
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
            catch (Exception exception2)
            {
                this._runner.Cookies = new CookieContainer();
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SEARCH);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception2, "", "");
                    return false;
                }
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                return false;
            }
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, false);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.off---white.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.off---white.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "order_shipments_attributes_0_id"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("li").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "shipping-method"))).Descendants("input").First<HtmlNode>().Attributes["value"].Value;
                this._request = (HttpWebRequest) WebRequest.Create($"{this._geo}checkout/update/delivery");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Referer = $"{this._geo}checkout/delivery";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("Origin", "https://www.off---white.com");
                this._request.Method = "POST";
                this._request.AllowAutoRedirect = false;
                this._data = "utf8=%E2%9C%93";
                this._data = this._data + "&_method=patch";
                this._data = this._data + "&authenticity_token=";
                this._data = this._data + "&order%5Bstate_lock_version%5D=1";
                this._data = this._data + "&order%5Bshipments_attributes%5D%5B0%5D%5Bselected_shipping_rate_id%5D=" + str2;
                this._data = this._data + "&order%5Bshipments_attributes%5D%5B0%5D%5Bid%5D=" + str;
                this._data = this._data + "&commit=Save+and+Continue";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
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
                this._currentDoc.LoadHtml(this._srr);
                this._srr = this._client.Get(this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>().Attributes["href"].Value).Text();
                this._currentDoc.LoadHtml(this._srr);
                string str3 = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ORDER_NUMBER"))).Attributes["content"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "amount"))).Attributes["value"].Value;
                string str5 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "gestpay-data"))).Attributes["data-merchant"].Value;
                this._request = (HttpWebRequest) WebRequest.Create($"{this._geo}checkout/payment/get_token.json");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Referer = $"{this._geo}checkout/payment";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("Origin", "https://www.off---white.com");
                this._request.Method = "POST";
                if (!string.IsNullOrEmpty(this._xpid))
                {
                    this._request.Headers.Add("X-NewRelic-ID", this._xpid);
                }
                this._runner.Cookies.List();
                this._data = "transaction=" + str3;
                this._data = this._data + "&amount=" + str4;
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream2 = this._request.GetRequestStream())
                {
                    stream2.Write(this._bytes, 0, this._bytes.Length);
                }
                using (WebResponse response2 = this._request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        this._srr = reader2.ReadToEnd();
                    }
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__19.<>p__2 == null)
                {
                    <>o__19.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                }
                if (<>o__19.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                }
                if (<>o__19.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                }
                this._payToken = <>o__19.<>p__2.Target(<>o__19.<>p__2, <>o__19.<>p__1.Target(<>o__19.<>p__1, <>o__19.<>p__0.Target(<>o__19.<>p__0, this._dynObj, "token")));
                this._payLink = $"https://ecomm.sella.it/Pagam/hiddenIframe.aspx?a={str5}&b={this._payToken}&MerchantUrl=" + WebUtility.UrlEncode($"{this._geo}checkout/payment");
                this._request = (HttpWebRequest) WebRequest.Create(this._payLink);
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Referer = $"{this._geo}checkout/payment";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                using (WebResponse response3 = this._request.GetResponse())
                {
                    using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                    {
                        this._srr = reader3.ReadToEnd();
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.Cookies = new CookieContainer();
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception, "", "");
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
                if (this._task.Payment == TaskObject.PaymentEnum.bankTransfer)
                {
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
                    if (Global.SERIAL != "EVE-1111111111111")
                    {
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
                        this._request = (HttpWebRequest) WebRequest.Create($"{this._geo}checkout/update/payment");
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        this._request.CookieContainer = this._runner.Cookies;
                        this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                        this._request.KeepAlive = true;
                        this._request.Referer = $"{this._geo}checkout/payment";
                        this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        this._request.Headers.Add("Origin", "https://www.off---white.com");
                        this._request.ContentType = "application/x-www-form-urlencoded";
                        this._request.Method = "POST";
                        this._request.ServicePoint.Expect100Continue = false;
                        this._request.AllowAutoRedirect = false;
                        this._data = "utf8=%E2%9C%93&_method=patch&authenticity_token=&order%5Bpayments_attributes%5D%5B%5D%5Bpayment_method_id%5D=10";
                        this._bytes = Encoding.ASCII.GetBytes(this._data);
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
                        this._currentDoc.LoadHtml(this._srr);
                        this._request = (HttpWebRequest) WebRequest.Create(this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>().Attributes["href"].Value);
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        this._request.CookieContainer = this._runner.Cookies;
                        this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                        this._request.KeepAlive = true;
                        this._request.Referer = $"{this._geo}checkout/payment";
                        this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        using (WebResponse response2 = this._request.GetResponse())
                        {
                            using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                this._srr = reader2.ReadToEnd();
                            }
                        }
                        this._currentDoc.LoadHtml(this._srr);
                        if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "thanks")))
                        {
                            return true;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "inputString"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"))).Attributes["value"].Value;
                this._request = (HttpWebRequest) WebRequest.Create(this._payLink);
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Referer = this._payLink;
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Headers.Add("Origin", "https://ecomm.sella.it");
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Method = "POST";
                this._data = "__VIEWSTATE=" + WebUtility.UrlEncode(str2);
                this._data = this._data + "&__VIEWSTATEGENERATOR=" + WebUtility.UrlEncode(str3);
                this._data = this._data + "&__EVENTVALIDATION=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&cardnumber=" + this._runner.Profile.CCNumber;
                this._data = this._data + "&cardExpiryMonth=" + this._runner.Profile.ExpiryMonth;
                this._data = this._data + "&cardExpiryYear=" + this._runner.Profile.ExpiryYear.Substring(2);
                this._data = this._data + "&cvv=" + this._runner.Profile.Cvv;
                this._data = this._data + "&buyerName=undefined";
                this._data = this._data + "&buyerEMail=undefined";
                this._data = this._data + "&inputString=" + WebUtility.UrlEncode(str);
                this._data = this._data + "&pares=";
                this._data = this._data + "&logPostData=";
                this._data = this._data + "&shopLogin=";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
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
                if (Global.SERIAL != "EVE-1111111111111")
                {
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
                    using (Stream stream2 = this._request.GetRequestStream())
                    {
                        stream2.Write(this._bytes, 0, this._bytes.Length);
                    }
                    using (WebResponse response3 = this._request.GetResponse())
                    {
                        using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                        {
                            this._srr = reader3.ReadToEnd();
                        }
                    }
                    this._currentDoc.LoadHtml(this._srr);
                    string str5 = this._srr.Substring(this._srr.IndexOf("delayedSendResult"));
                    str5 = str5.Substring(0, str5.IndexOf("//]]>"));
                    str5 = str5.Substring(str5.LastIndexOf(",") + 2);
                    str5 = str5.Substring(0, str5.IndexOf("'"));
                    this._request = (HttpWebRequest) WebRequest.Create("https://www.off---white.com/checkout/payment/process_token.json");
                    if (this._runner.Proxy != null)
                    {
                        this._request.Proxy = this._runner.Proxy;
                    }
                    this._request.CookieContainer = this._runner.Cookies;
                    this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                    this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    this._request.Accept = "application/json, text/javascript, */*; q=0.01";
                    this._request.KeepAlive = true;
                    this._request.Referer = $"{this._geo}checkout/payment";
                    this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    this._request.Headers.Add("Origin", "https://www.off---white.com");
                    this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    this._request.Method = "POST";
                    this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    if (!string.IsNullOrEmpty(this._xpid))
                    {
                        this._request.Headers.Add("X-NewRelic-ID", this._xpid);
                    }
                    this._data = "token=" + str5;
                    this._bytes = Encoding.ASCII.GetBytes(this._data);
                    this._request.ContentLength = this._bytes.Length;
                    using (Stream stream3 = this._request.GetRequestStream())
                    {
                        stream3.Write(this._bytes, 0, this._bytes.Length);
                    }
                    try
                    {
                        using (WebResponse response4 = this._request.GetResponse())
                        {
                            using (StreamReader reader4 = new StreamReader(response4.GetResponseStream()))
                            {
                                this._srr = reader4.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException)
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        return false;
                    }
                    try
                    {
                        EveAIO.Helpers.AddDbValue("OffWhite|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    }
                    catch
                    {
                    }
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                flag = false;
            }
            catch (ThreadAbortException)
            {
                flag = false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                flag = false;
            }
            finally
            {
                this._runner.Cookies = new CookieContainer();
            }
            return flag;
        }

        private bool SubmitShipping()
        {
            try
            {
                IEnumerator enumerator;
                object obj6;
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create($"{this._geo}checkout/registration");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Referer = $"{this._geo}checkout/registration";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("Origin", "https://www.off---white.com");
                this._request.Method = "POST";
                this._request.AllowAutoRedirect = false;
                this._data = "utf8=%E2%9C%93&_method=put&authenticity_token=&order%5Bemail%5D=" + WebUtility.UrlEncode(this._runner.Profile.Email) + "&commit=Continue";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
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
                if (this._srr.Contains("An item in your cart has become unavailable"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                this._currentDoc.LoadHtml(this._srr);
                string billCountryId = WebsitesInfo.OFFWHITE_COUNTRIES.First<KeyValuePair<string, string>>(x => (x.Value.ToLowerInvariant() == this._runner.Profile.Country.ToLowerInvariant())).Key;
                string shipCountryId = billCountryId;
                if (this._runner.Profile.CountryId != this._runner.Profile.CountryIdShipping)
                {
                    shipCountryId = WebsitesInfo.OFFWHITE_COUNTRIES.First<KeyValuePair<string, string>>(x => (x.Value.ToLowerInvariant() == this._runner.Profile.CountryShipping.ToLowerInvariant())).Key;
                }
                List<KeyValuePair<string, string>> source = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(Resources.ow);
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(source.First<KeyValuePair<string, string>>(x => (x.Key == billCountryId)).Value);
                object obj3 = Newtonsoft.Json.JsonConvert.DeserializeObject(source.First<KeyValuePair<string, string>>(x => (x.Key == shipCountryId)).Value);
                string str2 = "None";
                string str = "None";
                if (<>o__20.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__3.Target(<>o__20.<>p__3, <>o__20.<>p__2.Target(<>o__20.<>p__2, <>o__20.<>p__1.Target(<>o__20.<>p__1, <>o__20.<>p__0.Target(<>o__20.<>p__0, obj2, "states_required")), true)))
                {
                    if ((this._runner.Profile.CountryId != "US") && (this._runner.Profile.CountryId != "CA"))
                    {
                        if (<>o__20.<>p__23 == null)
                        {
                            <>o__20.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                        }
                        if (<>o__20.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__20.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "states", typeof(OffWhite), argumentInfo));
                        }
                        <>o__20.<>p__23.Target(<>o__20.<>p__23, <>o__20.<>p__22.Target(<>o__20.<>p__22, <>o__20.<>p__21.Target(<>o__20.<>p__21, <>o__20.<>p__20.Target(<>o__20.<>p__20, <>o__20.<>p__19.Target(<>o__20.<>p__19, <>o__20.<>p__18.Target(<>o__20.<>p__18, obj2)), "id"))));
                    }
                    else
                    {
                        if (<>o__20.<>p__17 == null)
                        {
                            <>o__20.<>p__17 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(OffWhite)));
                        }
                        if (<>o__20.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "states", typeof(OffWhite), argumentInfo));
                        }
                        using (enumerator = <>o__20.<>p__17.Target(<>o__20.<>p__17, <>o__20.<>p__4.Target(<>o__20.<>p__4, obj2)).GetEnumerator())
                        {
                            object obj4;
                            goto Label_0871;
                        Label_071C:
                            obj4 = enumerator.Current;
                            if (<>o__20.<>p__8 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__8.Target(<>o__20.<>p__8, <>o__20.<>p__7.Target(<>o__20.<>p__7, <>o__20.<>p__6.Target(<>o__20.<>p__6, <>o__20.<>p__5.Target(<>o__20.<>p__5, obj4, "abbr")), this._runner.Profile.StateId)))
                            {
                                goto Label_0882;
                            }
                        Label_0871:
                            if (!enumerator.MoveNext())
                            {
                                goto Label_0AF6;
                            }
                            goto Label_071C;
                        Label_0882:
                            if (<>o__20.<>p__12 == null)
                            {
                                <>o__20.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                            }
                            if (<>o__20.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__10 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__9 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                            }
                            <>o__20.<>p__12.Target(<>o__20.<>p__12, <>o__20.<>p__11.Target(<>o__20.<>p__11, <>o__20.<>p__10.Target(<>o__20.<>p__10, <>o__20.<>p__9.Target(<>o__20.<>p__9, obj4, "id"))));
                            if (<>o__20.<>p__16 == null)
                            {
                                <>o__20.<>p__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                            }
                            if (<>o__20.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__13 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                            }
                            str2 = <>o__20.<>p__16.Target(<>o__20.<>p__16, <>o__20.<>p__15.Target(<>o__20.<>p__15, <>o__20.<>p__14.Target(<>o__20.<>p__14, <>o__20.<>p__13.Target(<>o__20.<>p__13, obj4, "name"))));
                        }
                    }
                }
            Label_0AF6:
                if (<>o__20.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__27 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__26 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__26 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__25 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__24 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__27.Target(<>o__20.<>p__27, <>o__20.<>p__26.Target(<>o__20.<>p__26, <>o__20.<>p__25.Target(<>o__20.<>p__25, <>o__20.<>p__24.Target(<>o__20.<>p__24, obj3, "states_required")), true)))
                {
                    if ((this._runner.Profile.CountryIdShipping != "US") && (this._runner.Profile.CountryIdShipping != "CA"))
                    {
                        if (<>o__20.<>p__47 == null)
                        {
                            <>o__20.<>p__47 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                        }
                        if (<>o__20.<>p__46 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__45 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__44 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__20.<>p__44 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__43 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(OffWhite), argumentInfo));
                        }
                        if (<>o__20.<>p__42 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "states", typeof(OffWhite), argumentInfo));
                        }
                        <>o__20.<>p__47.Target(<>o__20.<>p__47, <>o__20.<>p__46.Target(<>o__20.<>p__46, <>o__20.<>p__45.Target(<>o__20.<>p__45, <>o__20.<>p__44.Target(<>o__20.<>p__44, <>o__20.<>p__43.Target(<>o__20.<>p__43, <>o__20.<>p__42.Target(<>o__20.<>p__42, obj3)), "id"))));
                    }
                    else
                    {
                        if (<>o__20.<>p__41 == null)
                        {
                            <>o__20.<>p__41 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(OffWhite)));
                        }
                        if (<>o__20.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "states", typeof(OffWhite), argumentInfo));
                        }
                        using (enumerator = <>o__20.<>p__41.Target(<>o__20.<>p__41, <>o__20.<>p__28.Target(<>o__20.<>p__28, obj3)).GetEnumerator())
                        {
                            object obj5;
                            goto Label_102E;
                        Label_0ED9:
                            obj5 = enumerator.Current;
                            if (<>o__20.<>p__32 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__32 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__31 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__30 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__29 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__32.Target(<>o__20.<>p__32, <>o__20.<>p__31.Target(<>o__20.<>p__31, <>o__20.<>p__30.Target(<>o__20.<>p__30, <>o__20.<>p__29.Target(<>o__20.<>p__29, obj5, "abbr")), this._runner.Profile.StateIdShipping)))
                            {
                                goto Label_103F;
                            }
                        Label_102E:
                            if (!enumerator.MoveNext())
                            {
                                goto Label_12B3;
                            }
                            goto Label_0ED9;
                        Label_103F:
                            if (<>o__20.<>p__36 == null)
                            {
                                <>o__20.<>p__36 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                            }
                            if (<>o__20.<>p__35 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__34 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__33 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                            }
                            <>o__20.<>p__36.Target(<>o__20.<>p__36, <>o__20.<>p__35.Target(<>o__20.<>p__35, <>o__20.<>p__34.Target(<>o__20.<>p__34, <>o__20.<>p__33.Target(<>o__20.<>p__33, obj5, "id"))));
                            if (<>o__20.<>p__40 == null)
                            {
                                <>o__20.<>p__40 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                            }
                            if (<>o__20.<>p__39 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__38 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(OffWhite), argumentInfo));
                            }
                            if (<>o__20.<>p__37 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OffWhite), argumentInfo));
                            }
                            str = <>o__20.<>p__40.Target(<>o__20.<>p__40, <>o__20.<>p__39.Target(<>o__20.<>p__39, <>o__20.<>p__38.Target(<>o__20.<>p__38, <>o__20.<>p__37.Target(<>o__20.<>p__37, obj5, "name"))));
                        }
                    }
                }
            Label_12B3:
                obj6 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__20.<>p__48 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__48 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__48.Target(<>o__20.<>p__48, obj6, new Newtonsoft.Json.Linq.JObject());
                if (<>o__20.<>p__50 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__50 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__49 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__50.Target(<>o__20.<>p__50, <>o__20.<>p__49.Target(<>o__20.<>p__49, obj6), new Newtonsoft.Json.Linq.JObject());
                if (<>o__20.<>p__53 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__53 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstname", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__52 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__51 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__53.Target(<>o__20.<>p__53, <>o__20.<>p__52.Target(<>o__20.<>p__52, <>o__20.<>p__51.Target(<>o__20.<>p__51, obj6)), this._runner.Profile.FirstName);
                if (<>o__20.<>p__56 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__56 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastname", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__55 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__54 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__56.Target(<>o__20.<>p__56, <>o__20.<>p__55.Target(<>o__20.<>p__55, <>o__20.<>p__54.Target(<>o__20.<>p__54, obj6)), this._runner.Profile.LastName);
                if (<>o__20.<>p__59 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__59 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address1", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__58 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__58 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__57 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__59.Target(<>o__20.<>p__59, <>o__20.<>p__58.Target(<>o__20.<>p__58, <>o__20.<>p__57.Target(<>o__20.<>p__57, obj6)), this._runner.Profile.Address1);
                if (<>o__20.<>p__62 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__62 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address2", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__61 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__61 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__60 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__62.Target(<>o__20.<>p__62, <>o__20.<>p__61.Target(<>o__20.<>p__61, <>o__20.<>p__60.Target(<>o__20.<>p__60, obj6)), this._runner.Profile.Address2);
                if (<>o__20.<>p__65 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__65 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__64 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__64 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__63 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__63 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__65.Target(<>o__20.<>p__65, <>o__20.<>p__64.Target(<>o__20.<>p__64, <>o__20.<>p__63.Target(<>o__20.<>p__63, obj6)), this._runner.Profile.City);
                if (<>o__20.<>p__68 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__68 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country_id", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__67 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__67 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__66 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__68.Target(<>o__20.<>p__68, <>o__20.<>p__67.Target(<>o__20.<>p__67, <>o__20.<>p__66.Target(<>o__20.<>p__66, obj6)), billCountryId);
                if (<>o__20.<>p__71 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__71 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "state_name", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__70 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__69 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__69 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__71.Target(<>o__20.<>p__71, <>o__20.<>p__70.Target(<>o__20.<>p__70, <>o__20.<>p__69.Target(<>o__20.<>p__69, obj6)), str2);
                if (<>o__20.<>p__74 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__74 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "zipcode", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__73 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__73 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__72 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__74.Target(<>o__20.<>p__74, <>o__20.<>p__73.Target(<>o__20.<>p__73, <>o__20.<>p__72.Target(<>o__20.<>p__72, obj6)), this._runner.Profile.Zip);
                if (<>o__20.<>p__77 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__77 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__76 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__76 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__75 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__77.Target(<>o__20.<>p__77, <>o__20.<>p__76.Target(<>o__20.<>p__76, <>o__20.<>p__75.Target(<>o__20.<>p__75, obj6)), this._runner.Profile.Phone);
                if (<>o__20.<>p__80 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__80 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "hs_fiscal_code", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__79 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__79 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "bill_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__78 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__78 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__80.Target(<>o__20.<>p__80, <>o__20.<>p__79.Target(<>o__20.<>p__79, <>o__20.<>p__78.Target(<>o__20.<>p__78, obj6)), this._runner.Profile.VariousBilling);
                if (<>o__20.<>p__82 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__82 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__81 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__81 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__82.Target(<>o__20.<>p__82, <>o__20.<>p__81.Target(<>o__20.<>p__81, obj6), new Newtonsoft.Json.Linq.JObject());
                if (<>o__20.<>p__85 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__85 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstname", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__84 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__84 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__83 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__83 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__85.Target(<>o__20.<>p__85, <>o__20.<>p__84.Target(<>o__20.<>p__84, <>o__20.<>p__83.Target(<>o__20.<>p__83, obj6)), this._runner.Profile.FirstNameShipping);
                if (<>o__20.<>p__88 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__88 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastname", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__87 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__87 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__86 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__86 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__88.Target(<>o__20.<>p__88, <>o__20.<>p__87.Target(<>o__20.<>p__87, <>o__20.<>p__86.Target(<>o__20.<>p__86, obj6)), this._runner.Profile.LastNameShipping);
                if (<>o__20.<>p__91 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__91 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address1", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__90 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__90 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__89 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__89 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__91.Target(<>o__20.<>p__91, <>o__20.<>p__90.Target(<>o__20.<>p__90, <>o__20.<>p__89.Target(<>o__20.<>p__89, obj6)), this._runner.Profile.Address1Shipping);
                if (<>o__20.<>p__94 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__94 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address2", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__93 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__93 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__92 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__92 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__94.Target(<>o__20.<>p__94, <>o__20.<>p__93.Target(<>o__20.<>p__93, <>o__20.<>p__92.Target(<>o__20.<>p__92, obj6)), this._runner.Profile.Address2Shipping);
                if (<>o__20.<>p__97 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__97 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__96 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__96 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__95 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__95 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__97.Target(<>o__20.<>p__97, <>o__20.<>p__96.Target(<>o__20.<>p__96, <>o__20.<>p__95.Target(<>o__20.<>p__95, obj6)), this._runner.Profile.CityShipping);
                if (<>o__20.<>p__100 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__100 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country_id", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__99 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__99 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__98 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__98 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__100.Target(<>o__20.<>p__100, <>o__20.<>p__99.Target(<>o__20.<>p__99, <>o__20.<>p__98.Target(<>o__20.<>p__98, obj6)), shipCountryId);
                if (<>o__20.<>p__103 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__103 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "state_name", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__102 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__102 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__101 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__101 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__103.Target(<>o__20.<>p__103, <>o__20.<>p__102.Target(<>o__20.<>p__102, <>o__20.<>p__101.Target(<>o__20.<>p__101, obj6)), str);
                if (<>o__20.<>p__106 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__106 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "zipcode", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__105 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__105 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__104 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__104 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__106.Target(<>o__20.<>p__106, <>o__20.<>p__105.Target(<>o__20.<>p__105, <>o__20.<>p__104.Target(<>o__20.<>p__104, obj6)), this._runner.Profile.ZipShipping);
                if (<>o__20.<>p__109 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__109 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__108 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__108 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__107 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__107 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__109.Target(<>o__20.<>p__109, <>o__20.<>p__108.Target(<>o__20.<>p__108, <>o__20.<>p__107.Target(<>o__20.<>p__107, obj6)), this._runner.Profile.PhoneShipping);
                if (<>o__20.<>p__112 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__112 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shipping", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__111 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__111 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ship_address_attributes", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__110 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__110 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__112.Target(<>o__20.<>p__112, <>o__20.<>p__111.Target(<>o__20.<>p__111, <>o__20.<>p__110.Target(<>o__20.<>p__110, obj6)), "true");
                if (<>o__20.<>p__114 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__114 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "email", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__113 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__113 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__114.Target(<>o__20.<>p__114, <>o__20.<>p__113.Target(<>o__20.<>p__113, obj6), this._runner.Profile.Email);
                if (<>o__20.<>p__116 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__116 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "state_lock_version", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__115 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__115 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__116.Target(<>o__20.<>p__116, <>o__20.<>p__115.Target(<>o__20.<>p__115, obj6), "0");
                if (<>o__20.<>p__118 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__118 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "terms_and_conditions", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__117 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__117 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__118.Target(<>o__20.<>p__118, <>o__20.<>p__117.Target(<>o__20.<>p__117, obj6), "yes");
                if (<>o__20.<>p__120 == null)
                {
                    <>o__20.<>p__120 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                }
                if (<>o__20.<>p__119 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__20.<>p__119 = CallSite<Func<CallSite, object, Newtonsoft.Json.Formatting, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                }
                this._data = <>o__20.<>p__120.Target(<>o__20.<>p__120, <>o__20.<>p__119.Target(<>o__20.<>p__119, obj6, Newtonsoft.Json.Formatting.None));
                foreach (Cookie cookie in this._runner.Cookies.List())
                {
                    Cookie cookie1 = new Cookie {
                        Name = cookie.Name,
                        Value = cookie.Value,
                        Domain = cookie.Domain
                    };
                    this._client.Cookies.Add(cookie1);
                }
                string url = "";
                this._client.Session.DefaultRequestHeaders.ExpectContinue = true;
                if (<>o__20.<>p__122 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__122 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PatchJson", null, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__121 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__121 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(OffWhite), argumentInfo));
                }
                object obj7 = <>o__20.<>p__122.Target(<>o__20.<>p__122, this._client, $"{this._geo}checkout/update/address", <>o__20.<>p__121.Target(<>o__20.<>p__121, typeof(Newtonsoft.Json.JsonConvert), obj6));
                if (<>o__20.<>p__123 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__123 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(OffWhite), argumentInfo));
                }
                <>o__20.<>p__123.Target(<>o__20.<>p__123, typeof(EveAIO.Extensions), obj7);
                if (<>o__20.<>p__128 == null)
                {
                    <>o__20.<>p__128 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                }
                if (<>o__20.<>p__127 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__127 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__126 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__126 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__125 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__125 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__124 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__124 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(OffWhite), argumentInfo));
                }
                this._srr = <>o__20.<>p__128.Target(<>o__20.<>p__128, <>o__20.<>p__127.Target(<>o__20.<>p__127, <>o__20.<>p__126.Target(<>o__20.<>p__126, <>o__20.<>p__125.Target(<>o__20.<>p__125, <>o__20.<>p__124.Target(<>o__20.<>p__124, obj7)))));
                if (<>o__20.<>p__132 == null)
                {
                    <>o__20.<>p__132 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(OffWhite)));
                }
                if (<>o__20.<>p__131 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__131 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__130 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__130 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "RequestUri", typeof(OffWhite), argumentInfo));
                }
                if (<>o__20.<>p__129 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__20.<>p__129 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "RequestMessage", typeof(OffWhite), argumentInfo));
                }
                url = <>o__20.<>p__132.Target(<>o__20.<>p__132, <>o__20.<>p__131.Target(<>o__20.<>p__131, <>o__20.<>p__130.Target(<>o__20.<>p__130, <>o__20.<>p__129.Target(<>o__20.<>p__129, obj7))));
                if (!this._srr.Contains("An item in your cart has become unavailable"))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    url = this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                    this._srr = this._client.Get(url).Text();
                    this._currentDoc.LoadHtml(this._srr);
                    string str4 = url.Replace("http://", "").Replace("https://", "");
                    str4 = str4.Substring(str4.IndexOf("/") + 1);
                    this._geo = "https://www.off---white.com/" + str4.Substring(0, str4.IndexOf("/") + 1);
                    str4 = str4.Substring(str4.IndexOf("/") + 1);
                    this._geo = this._geo + str4.Substring(0, str4.IndexOf("/") + 1);
                    return url.Contains("checkout/delivery");
                }
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.Cookies = new CookieContainer();
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception, "", "");
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
            public static readonly OffWhite.<>c <>9;
            public static Func<HtmlNode, bool> <>9__18_1;
            public static Func<HtmlNode, bool> <>9__18_2;
            public static Func<HtmlNode, bool> <>9__18_3;
            public static Func<HtmlNode, bool> <>9__18_4;
            public static Func<HtmlNode, bool> <>9__18_5;
            public static Func<HtmlNode, bool> <>9__19_0;
            public static Func<HtmlNode, bool> <>9__19_1;
            public static Func<HtmlNode, bool> <>9__19_2;
            public static Func<HtmlNode, bool> <>9__19_3;
            public static Func<HtmlNode, bool> <>9__19_4;
            public static Func<HtmlNode, bool> <>9__21_5;
            public static Func<HtmlNode, bool> <>9__21_0;
            public static Func<HtmlNode, bool> <>9__21_1;
            public static Func<HtmlNode, bool> <>9__21_2;
            public static Func<HtmlNode, bool> <>9__21_3;
            public static Func<HtmlNode, bool> <>9__21_4;
            public static Func<HtmlNode, bool> <>9__21_8;
            public static Func<HtmlNode, bool> <>9__23_0;
            public static Func<HtmlNode, bool> <>9__23_3;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new OffWhite.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__21_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "prod-title"));

            internal bool <DirectLink>b__21_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__21_2(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"));

            internal bool <DirectLink>b__21_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-cart-form js-cart-form"));

            internal bool <DirectLink>b__21_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-cart-form js-cart-form"));

            internal bool <DirectLink>b__21_5(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <DirectLink>b__21_8(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "styled-radio"));

            internal bool <Search>b__23_0(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <Search>b__23_3(HtmlNode x) => 
                ((x.Attributes["itemProp"] != null) && (x.Attributes["itemProp"].Value == "url"));

            internal bool <SubmitBilling>b__19_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "order_shipments_attributes_0_id"));

            internal bool <SubmitBilling>b__19_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "shipping-method"));

            internal bool <SubmitBilling>b__19_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ORDER_NUMBER"));

            internal bool <SubmitBilling>b__19_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "amount"));

            internal bool <SubmitBilling>b__19_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "gestpay-data"));

            internal bool <SubmitOrder>b__18_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "thanks"));

            internal bool <SubmitOrder>b__18_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "inputString"));

            internal bool <SubmitOrder>b__18_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"));

            internal bool <SubmitOrder>b__18_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"));

            internal bool <SubmitOrder>b__18_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"));
        }

        [CompilerGenerated]
        private static class <>o__16
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, int, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
        }

        [CompilerGenerated]
        private static class <>o__19
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
        }

        [CompilerGenerated]
        private static class <>o__20
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string>> <>p__16;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__26;
            public static CallSite<Func<CallSite, object, bool>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, bool>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, string>> <>p__40;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, string, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, string>> <>p__47;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, object>> <>p__52;
            public static CallSite<Func<CallSite, object, string, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, string, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, object>> <>p__58;
            public static CallSite<Func<CallSite, object, string, object>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, object>> <>p__61;
            public static CallSite<Func<CallSite, object, string, object>> <>p__62;
            public static CallSite<Func<CallSite, object, object>> <>p__63;
            public static CallSite<Func<CallSite, object, object>> <>p__64;
            public static CallSite<Func<CallSite, object, string, object>> <>p__65;
            public static CallSite<Func<CallSite, object, object>> <>p__66;
            public static CallSite<Func<CallSite, object, object>> <>p__67;
            public static CallSite<Func<CallSite, object, string, object>> <>p__68;
            public static CallSite<Func<CallSite, object, object>> <>p__69;
            public static CallSite<Func<CallSite, object, object>> <>p__70;
            public static CallSite<Func<CallSite, object, string, object>> <>p__71;
            public static CallSite<Func<CallSite, object, object>> <>p__72;
            public static CallSite<Func<CallSite, object, object>> <>p__73;
            public static CallSite<Func<CallSite, object, string, object>> <>p__74;
            public static CallSite<Func<CallSite, object, object>> <>p__75;
            public static CallSite<Func<CallSite, object, object>> <>p__76;
            public static CallSite<Func<CallSite, object, string, object>> <>p__77;
            public static CallSite<Func<CallSite, object, object>> <>p__78;
            public static CallSite<Func<CallSite, object, object>> <>p__79;
            public static CallSite<Func<CallSite, object, string, object>> <>p__80;
            public static CallSite<Func<CallSite, object, object>> <>p__81;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__82;
            public static CallSite<Func<CallSite, object, object>> <>p__83;
            public static CallSite<Func<CallSite, object, object>> <>p__84;
            public static CallSite<Func<CallSite, object, string, object>> <>p__85;
            public static CallSite<Func<CallSite, object, object>> <>p__86;
            public static CallSite<Func<CallSite, object, object>> <>p__87;
            public static CallSite<Func<CallSite, object, string, object>> <>p__88;
            public static CallSite<Func<CallSite, object, object>> <>p__89;
            public static CallSite<Func<CallSite, object, object>> <>p__90;
            public static CallSite<Func<CallSite, object, string, object>> <>p__91;
            public static CallSite<Func<CallSite, object, object>> <>p__92;
            public static CallSite<Func<CallSite, object, object>> <>p__93;
            public static CallSite<Func<CallSite, object, string, object>> <>p__94;
            public static CallSite<Func<CallSite, object, object>> <>p__95;
            public static CallSite<Func<CallSite, object, object>> <>p__96;
            public static CallSite<Func<CallSite, object, string, object>> <>p__97;
            public static CallSite<Func<CallSite, object, object>> <>p__98;
            public static CallSite<Func<CallSite, object, object>> <>p__99;
            public static CallSite<Func<CallSite, object, string, object>> <>p__100;
            public static CallSite<Func<CallSite, object, object>> <>p__101;
            public static CallSite<Func<CallSite, object, object>> <>p__102;
            public static CallSite<Func<CallSite, object, string, object>> <>p__103;
            public static CallSite<Func<CallSite, object, object>> <>p__104;
            public static CallSite<Func<CallSite, object, object>> <>p__105;
            public static CallSite<Func<CallSite, object, string, object>> <>p__106;
            public static CallSite<Func<CallSite, object, object>> <>p__107;
            public static CallSite<Func<CallSite, object, object>> <>p__108;
            public static CallSite<Func<CallSite, object, string, object>> <>p__109;
            public static CallSite<Func<CallSite, object, object>> <>p__110;
            public static CallSite<Func<CallSite, object, object>> <>p__111;
            public static CallSite<Func<CallSite, object, string, object>> <>p__112;
            public static CallSite<Func<CallSite, object, object>> <>p__113;
            public static CallSite<Func<CallSite, object, string, object>> <>p__114;
            public static CallSite<Func<CallSite, object, object>> <>p__115;
            public static CallSite<Func<CallSite, object, string, object>> <>p__116;
            public static CallSite<Func<CallSite, object, object>> <>p__117;
            public static CallSite<Func<CallSite, object, string, object>> <>p__118;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Formatting, object>> <>p__119;
            public static CallSite<Func<CallSite, object, string>> <>p__120;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__121;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__122;
            public static CallSite<Action<CallSite, Type, object>> <>p__123;
            public static CallSite<Func<CallSite, object, object>> <>p__124;
            public static CallSite<Func<CallSite, object, object>> <>p__125;
            public static CallSite<Func<CallSite, object, object>> <>p__126;
            public static CallSite<Func<CallSite, object, object>> <>p__127;
            public static CallSite<Func<CallSite, object, string>> <>p__128;
            public static CallSite<Func<CallSite, object, object>> <>p__129;
            public static CallSite<Func<CallSite, object, object>> <>p__130;
            public static CallSite<Func<CallSite, object, object>> <>p__131;
            public static CallSite<Func<CallSite, object, string>> <>p__132;
        }
    }
}

