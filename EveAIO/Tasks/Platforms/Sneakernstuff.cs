namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Sneakernstuff : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;
        private string _token;
        private HtmlDocument _cartView;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        [Dynamic]
        private object _dynObj;
        private string _data;
        private string _resUri;
        private string _regionCode;
        private bool _isLoggedIn;

        public Sneakernstuff(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            if ((!this._isLoggedIn && this._task.Login) && !this.Login())
            {
                return false;
            }
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                if (this._cartView != null)
                {
                    string str = this._runner.ProductPageHtml.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "add-to-cart-form"))).Attributes["data-external-reference"].Value;
                    using (IEnumerator<HtmlNode> enumerator = (from x in this._cartView.DocumentNode.Descendants("div")
                        where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-row ")
                        select x).GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.Attributes["data-ga-product"].Value.Contains(str))
                            {
                                return true;
                            }
                        }
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/en/cart/add");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.KeepAlive = true;
                this._request.Referer = this._runner.Product.Link;
                this._request.ServicePoint.Expect100Continue = false;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.Headers.Add("Cache-Control", "max-age=0");
                this._request.Headers.Add("Origin", "https://www.sneakersnstuff.com");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("X-AntiCsrfToken", this._token);
                this._data = "_AntiCsrfToken=" + this._token + "&partial=cart-summary&id=" + this._runner.PickedSize.Value.Value;
                if (this._runner.ProductPageHtml.DocumentNode.Descendants("div").Any<HtmlNode>(x => x.Attributes["data-sitekey"] > null))
                {
                    string captchaKey = this._runner.ProductPageHtml.DocumentNode.Descendants("div").First<HtmlNode>(x => (x.Attributes["data-sitekey"] > null)).Attributes["data-sitekey"].Value;
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), captchaKey, this._task.Link, "Sneakernstuff");
                    waiter.Start();
                    this._task.Mre.WaitOne();
                    if (Global.CAPTCHA_QUEUE.Any<TaskObject>(x => x.Id == this._task.Id))
                    {
                        solverLocker = Global.SolverLocker;
                        lock (solverLocker)
                        {
                            Global.CAPTCHA_QUEUE.Remove(Global.CAPTCHA_QUEUE.First<TaskObject>(x => x.Id == this._task.Id));
                            States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                        }
                    }
                    this._data = this._data + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token);
                }
                this._request.Method = "POST";
                this._request.ContentLength = this._data.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(this._data, this._data.Length);
                    }
                }
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/en/cart/view");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (WebResponse response2 = this._request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        this._srr = reader2.ReadToEnd();
                    }
                }
                if (this._srr.ToUpperInvariant().Contains(this._runner.PickedSize.Value.Value))
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
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
            switch (((0x60f654f1 ^ 0x203fbe78) % 5))
            {
                case 0:
                    goto Label_001C;

                case 1:
                    return false;

                case 2:
                    return false;

                case 3:
                    break;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (this.SubmitBilling())
            {
            }
            if (0x51cf9c4f || true)
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                bool flag = false;
                try
                {
                    CloudFlareSolver solver1 = new CloudFlareSolver(link, this._runner.Cookies, true, "", false, this._runner.Proxy);
                    solver1.Solve();
                    solver1.SendChallengeRequest();
                    HttpWebRequest request = solver1.CreateWebRequest(link);
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            this._srr = reader.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    flag = true;
                }
                if (flag)
                {
                    try
                    {
                        this._request = (HttpWebRequest) WebRequest.Create(link);
                        this._request.KeepAlive = true;
                        this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                        this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        this._request.CookieContainer = this._runner.Cookies;
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        using (WebResponse response2 = this._request.GetResponse())
                        {
                            using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                this._srr = reader2.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException exception)
                    {
                        HtmlDocument document = new HtmlDocument();
                        if (exception.Response == null)
                        {
                            throw exception;
                        }
                        string html = "";
                        using (Stream stream = exception.Response.GetResponseStream())
                        {
                            using (StreamReader reader3 = new StreamReader(stream))
                            {
                                html = reader3.ReadToEnd();
                            }
                        }
                        document.LoadHtml(html);
                        if (document.DocumentNode.Descendants("script").Any<HtmlNode>(x => x.Attributes["data-sitekey"] > null))
                        {
                            string captchaKey = document.DocumentNode.Descendants("script").First<HtmlNode>(x => (x.Attributes["data-sitekey"] > null)).Attributes["data-sitekey"].Value;
                            string str3 = document.DocumentNode.Descendants("script").First<HtmlNode>(x => (x.Attributes["data-sitekey"] > null)).Attributes["data-ray"].Value;
                            object solverLocker = Global.SolverLocker;
                            lock (solverLocker)
                            {
                                Global.CAPTCHA_QUEUE.Add(this._task);
                            }
                            States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                            this._task.Mre = new ManualResetEvent(false);
                            CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), captchaKey, this._task.Link, "Sneakernstuff");
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
                            this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/cdn-cgi/l/chk_captcha?id=" + str3 + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token));
                            this._request.KeepAlive = true;
                            this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                            this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                            this._request.Referer = "https://www.sneakersnstuff.com/";
                            this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            this._request.CookieContainer = this._runner.Cookies;
                            this._request.Referer = link;
                            if (this._runner.Proxy != null)
                            {
                                this._request.Proxy = this._runner.Proxy;
                            }
                            using (WebResponse response3 = this._request.GetResponse())
                            {
                                using (StreamReader reader4 = new StreamReader(response3.GetResponseStream()))
                                {
                                    this._srr = reader4.ReadToEnd();
                                }
                                goto Label_0567;
                            }
                        }
                        throw exception;
                    }
                }
            Label_0567:
                this._currentDoc.LoadHtml(this._srr);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                this._token = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_AntiCsrfToken"))).Attributes["value"].Value;
                if (!this._runner.ProductPageHtml.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "hasStock"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                string str4 = this._runner.ProductPageHtml.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-name"))).InnerText.Trim();
                string str5 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"))).InnerText.Trim();
                string str6 = "https://www.sneakersnstuff.com" + this._runner.ProductPageHtml.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "primary-image"))).Attributes["src"].Value.Trim();
                this._task.ImgUrl = str6;
                Product product1 = new Product {
                    ProductTitle = str4,
                    Link = link,
                    Price = str5,
                    Color = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-color"))).InnerText.Trim(),
                    Style = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-artno"))).InnerText.Trim()
                };
                this._runner.Product = product1;
                if ((this._task.TaskType == TaskObject.TaskTypeEnum.keywords) && !string.IsNullOrEmpty(this._task.Color))
                {
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                    {
                        if (this._runner.Product.Color.ToUpperInvariant() != this._task.Color.ToUpperInvariant())
                        {
                            return false;
                        }
                    }
                    else if (!this._runner.Product.Color.ToUpperInvariant().Contains(this._task.Color.ToUpperInvariant()))
                    {
                        return false;
                    }
                }
                foreach (HtmlNode local1 in this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sizes "))).Descendants("div"))
                {
                    string str7 = local1.Attributes["data-productid"].Value;
                    string key = local1.Descendants("span").First<HtmlNode>().InnerText.Replace("Wm", "").Trim();
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(key, str7);
                    this._runner.Product.AvailableSizes.Add(item);
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
                    string str10 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str10 = str10 + ch.ToString();
                        }
                    }
                    double num3 = double.Parse(str10.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                            KeyValuePair<string, string> pair2;
                            goto Label_0C52;
                        Label_0BDC:
                            pair2 = enumerator2.Current;
                            char[] chArray2 = new char[] { ' ' };
                            string[] source = pair2.Key.Split(chArray2);
                            for (int j = 0; j < source.Length; j++)
                            {
                                source[j] = source[j].Trim().ToUpperInvariant();
                            }
                            if (source.Any<string>(x => x == sz))
                            {
                                goto Label_0C60;
                            }
                        Label_0C52:
                            if (!enumerator2.MoveNext())
                            {
                                continue;
                            }
                            goto Label_0BDC;
                        Label_0C60:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(pair2);
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
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception2, "", "");
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
            if (this._isLoggedIn)
            {
                return true;
            }
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGGING_IN, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/en/authentication/login?skip_layout=1");
                this._request.Headers.Add("X-AntiCsrfToken", this._token);
                this._data = "ReturnUrl=";
                this._data = this._data + "&username=" + Uri.EscapeDataString(this._task.Username);
                this._data = this._data + "&password=" + Uri.EscapeDataString(this._task.Password);
                this._data = this._data + "&_AntiCsrfToken=" + this._token;
                this._request.Method = "POST";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.Accept = "application/json, text/javascript, */*; q=0.01";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentLength = this._data.Length;
                this._request.Referer = "https://www.sneakersnstuff.com/";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.ServicePoint.Expect100Continue = false;
                this._request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (Stream stream = this._request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(this._data, this._data.Length);
                    }
                }
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__21.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__21.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sneakernstuff), argumentInfo));
                }
                if (<>o__21.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__21.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Sneakernstuff), argumentInfo));
                }
                if (<>o__21.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__21.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sneakernstuff), argumentInfo));
                }
                object obj2 = <>o__21.<>p__1.Target(<>o__21.<>p__1, <>o__21.<>p__0.Target(<>o__21.<>p__0, this._dynObj, "Status"), null);
                if (<>o__21.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__21.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Sneakernstuff), argumentInfo));
                }
                if (!<>o__21.<>p__6.Target(<>o__21.<>p__6, obj2))
                {
                    if (<>o__21.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Sneakernstuff), argumentInfo));
                    }
                    if (<>o__21.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Sneakernstuff), argumentInfo));
                    }
                    if (<>o__21.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sneakernstuff), argumentInfo));
                    }
                }
                if (<>o__21.<>p__7.Target(<>o__21.<>p__7, (<>o__21.<>p__2 != null) ? obj2 : <>o__21.<>p__5.Target(<>o__21.<>p__5, obj2, <>o__21.<>p__4.Target(<>o__21.<>p__4, <>o__21.<>p__3.Target(<>o__21.<>p__3, <>o__21.<>p__2.Target(<>o__21.<>p__2, this._dynObj, "Status")), "OK"))))
                {
                    this._isLoggedIn = true;
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_SUCCESSFUL, null, "", "");
                    this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/en/cart/view");
                    this._request.KeepAlive = true;
                    this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                    this._request.Accept = "text/html, */*; q=0.01";
                    this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    this._request.CookieContainer = this._runner.Cookies;
                    if (this._runner.Proxy != null)
                    {
                        this._request.Proxy = this._runner.Proxy;
                    }
                    using (WebResponse response2 = this._request.GetResponse())
                    {
                        using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                        {
                            this._srr = reader2.ReadToEnd();
                        }
                    }
                    this._cartView = new HtmlDocument();
                    this._cartView.LoadHtml(this._srr);
                    if (!this._srr.Contains("cart is empty"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CLEANING_CART);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CLEARING_CART, null, "", "");
                        foreach (HtmlNode node in from x in this._cartView.DocumentNode.Descendants("div")
                            where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-row ")
                            select x)
                        {
                            string str2 = node.Attributes["panagora:productid"].Value;
                            string requestUriString = "https://www.sneakersnstuff.com" + node.Descendants("a").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "remove"))).Attributes["href"].Value;
                            this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
                            this._request.Headers.Add("X-AntiCsrfToken", this._token);
                            this._data = "id=" + str2 + "&quantity=1&partial=cart-contents";
                            this._request.Method = "POST";
                            this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                            this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            this._request.Accept = "application/json, text/javascript, */*; q=0.01";
                            this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                            this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            this._request.ContentLength = this._data.Length;
                            this._request.Referer = "https://www.sneakersnstuff.com/en/cart/view";
                            this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            this._request.ServicePoint.Expect100Continue = false;
                            this._request.CookieContainer = this._runner.Cookies;
                            if (this._runner.Proxy != null)
                            {
                                this._request.Proxy = this._runner.Proxy;
                            }
                            using (Stream stream2 = this._request.GetRequestStream())
                            {
                                using (StreamWriter writer2 = new StreamWriter(stream2))
                                {
                                    writer2.Write(this._data, this._data.Length);
                                }
                            }
                            using (WebResponse response3 = this._request.GetResponse())
                            {
                                using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                                {
                                    this._srr = reader3.ReadToEnd();
                                }
                            }
                        }
                    }
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_UNSUCCESSFUL, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.LOGIN_UNSUCCESSFUL);
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_LOGGING_IN, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        public bool Search()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                string str = "";
                bool flag = false;
                using (IEnumerator<string> enumerator = this._task.Keywords.GetEnumerator())
                {
                Label_0044:
                    if (!enumerator.MoveNext())
                    {
                        goto Label_07B7;
                    }
                    string current = enumerator.Current;
                    str = $"https://www.sneakersnstuff.com/en/search/searchbytext?key={current.Replace(" ", "+").ToLowerInvariant()}";
                    bool flag2 = false;
                Label_007E:
                    if (string.IsNullOrEmpty(str) || flag2)
                    {
                        goto Label_0044;
                    }
                    try
                    {
                        CloudFlareSolver solver1 = new CloudFlareSolver(str, this._runner.Cookies, true, "", false, this._runner.Proxy);
                        solver1.Solve();
                        solver1.SendChallengeRequest();
                        HttpWebRequest request = solver1.CreateWebRequest(str);
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                this._srr = reader.ReadToEnd();
                            }
                        }
                    }
                    catch
                    {
                        flag = true;
                    }
                    goto Label_07A0;
                Label_012E:;
                    try
                    {
                        this._request = (HttpWebRequest) WebRequest.Create(str);
                        this._request.KeepAlive = true;
                        this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                        this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        this._request.CookieContainer = this._runner.Cookies;
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        using (WebResponse response2 = this._request.GetResponse())
                        {
                            using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                this._srr = reader2.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException exception)
                    {
                        HtmlDocument document = new HtmlDocument();
                        if (exception.Response == null)
                        {
                            throw exception;
                        }
                        string html = "";
                        using (Stream stream = exception.Response.GetResponseStream())
                        {
                            using (StreamReader reader3 = new StreamReader(stream))
                            {
                                html = reader3.ReadToEnd();
                            }
                        }
                        document.LoadHtml(html);
                        if (document.DocumentNode.Descendants("script").Any<HtmlNode>(x => x.Attributes["data-sitekey"] > null))
                        {
                            string captchaKey = document.DocumentNode.Descendants("script").First<HtmlNode>(x => (x.Attributes["data-sitekey"] > null)).Attributes["data-sitekey"].Value;
                            string str4 = document.DocumentNode.Descendants("script").First<HtmlNode>(x => (x.Attributes["data-sitekey"] > null)).Attributes["data-ray"].Value;
                            object solverLocker = Global.SolverLocker;
                            lock (solverLocker)
                            {
                                Global.CAPTCHA_QUEUE.Add(this._task);
                            }
                            States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                            this._task.Mre = new ManualResetEvent(false);
                            CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), captchaKey, "https://www.sneakersnstuff.com", "Sneakernstuff");
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
                            this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/cdn-cgi/l/chk_captcha?id=" + str4 + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token));
                            this._request.KeepAlive = true;
                            this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                            this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                            this._request.Referer = str;
                            this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            this._request.CookieContainer = this._runner.Cookies;
                            if (this._runner.Proxy != null)
                            {
                                this._request.Proxy = this._runner.Proxy;
                            }
                            using (WebResponse response3 = this._request.GetResponse())
                            {
                                using (StreamReader reader4 = new StreamReader(response3.GetResponseStream()))
                                {
                                    this._srr = reader4.ReadToEnd();
                                }
                                goto Label_05AB;
                            }
                        }
                        throw exception;
                    }
                Label_05AB:
                    str = "";
                    HtmlNode.ElementsFlags.Remove("form");
                    this._currentDoc.LoadHtml(this._srr);
                    foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("li")
                        where (x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("product c-")
                        select x)
                    {
                        node.Descendants("h4").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("name"))).Descendants("a").First<HtmlNode>().InnerHtml.Trim();
                        string link = "https://www.sneakersnstuff.com" + node.Descendants("h4").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("name"))).Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                        if (flag2 = this.DirectLink(link))
                        {
                            return true;
                        }
                    }
                    if (this._currentDoc.DocumentNode.Descendants("a").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "control next")))
                    {
                        str = "https://www.sneakersnstuff.com" + this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "control next"))).Attributes["href"].Value;
                    }
                    goto Label_007E;
                Label_07A0:
                    if (!flag)
                    {
                        goto Label_05AB;
                    }
                    goto Label_012E;
                }
            Label_07B7:
                this._runner.Product = null;
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
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
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/en/cart/process");
                this._data = "kreditor_campaign=-1";
                this._data = this._data + "&country=" + this._runner.Profile.CountryId;
                if ((this._runner.Profile.CountryId == "US") || (this._runner.Profile.CountryId == "CA"))
                {
                    this._data = this._data + "&region=" + this._regionCode;
                }
                if (!this._task.Login)
                {
                    this._data = this._data + "&emailAddress=" + WebUtility.UrlEncode(this._runner.Profile.Email);
                }
                this._data = this._data + "&loginPassword=";
                this._data = this._data + "&kreditor_ssn=%24res_site.get_Item%28%24SsnInputLabel%29";
                this._data = this._data + "&firstName=" + WebUtility.UrlEncode(this._runner.Profile.FirstName);
                this._data = this._data + "&lastName=" + WebUtility.UrlEncode(this._runner.Profile.LastName);
                this._data = this._data + "&addressLine2=" + WebUtility.UrlEncode(this._runner.Profile.Address1);
                this._data = this._data + "&addressLine3=" + WebUtility.UrlEncode(this._runner.Profile.Address2);
                this._data = this._data + "&postalCode=" + WebUtility.UrlEncode(this._runner.Profile.Zip);
                this._data = this._data + "&city=" + WebUtility.UrlEncode(this._runner.Profile.City);
                this._data = this._data + "&phoneNumber=" + WebUtility.UrlEncode(this._runner.Profile.Phone);
                this._data = this._data + "&password=";
                this._data = this._data + "&repeatPassword=";
                this._data = this._data + "&termsAccepted=true";
                this._data = this._data + "&_AntiCsrfToken=" + this._token;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentLength = this._data.Length;
                this._request.Headers.Add("Origin", "https://www.sneakersnstuff.com");
                this._request.Referer = "https://www.sneakersnstuff.com/en/cart/view";
                using (Stream stream = this._request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(this._data, this._data.Length);
                    }
                }
                using (WebResponse response = this._request.GetResponse())
                {
                    this._resUri = response.ResponseUri.ToString();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
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
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
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
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create("https://live.adyen.com/hpp/completeCard.shtml");
                string str38 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sig"))).Attributes["value"].Value);
                string str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReference"))).Attributes["value"].Value;
                string str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "brandCode"))).Attributes["value"].Value;
                string str14 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentAmount"))).Attributes["value"].Value;
                string str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currencyCode"))).Attributes["value"].Value;
                string str36 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipBeforeDate"))).Attributes["value"].Value);
                string str26 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "skinCode"))).Attributes["value"].Value;
                string str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantAccount"))).Attributes["value"].Value;
                string str28 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperLocale"))).Attributes["value"].Value;
                string str16 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stage"))).Attributes["value"].Value;
                string str42 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionId"))).Attributes["value"].Value);
                string str43 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "orderData"))).Attributes["value"].Value);
                string str37 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionValidity"))).Attributes["value"].Value);
                string str39 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "countryCode"))).Attributes["value"].Value;
                string str17 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperEmail"))).Attributes["value"].Value);
                string str21 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperReference"))).Attributes["value"].Value;
                string str22 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantOrderReference"))).Attributes["value"].Value;
                string str23 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "resURL"))).Attributes["value"].Value);
                string str18 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "allowedMethods"))).Attributes["value"].Value;
                string str19 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "originalSession"))).Attributes["value"].Value);
                string str20 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.street"))).Attributes["value"].Value);
                string str = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.city"))).Attributes["value"].Value);
                string str2 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.postalCode"))).Attributes["value"].Value);
                string str3 = "";
                if ((this._runner.Profile.CountryId == "US") || (this._runner.Profile.CountryId == "CA"))
                {
                    str3 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.stateOrProvince"))).Attributes["value"].Value);
                }
                string str24 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.country"))).Attributes["value"].Value);
                string str41 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressSig"))).Attributes["value"].Value);
                string str25 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.firstName"))).Attributes["value"].Value);
                string str29 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.lastName"))).Attributes["value"].Value);
                string str11 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.gender"))).Attributes["value"].Value);
                string str30 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.telephoneNumber"))).Attributes["value"].Value);
                string str31 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "riskdata.deliveryMethod"))).Attributes["value"].Value);
                string str32 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "riskdata.sig"))).Attributes["value"].Value);
                string str33 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.street"))).Attributes["value"].Value);
                string str34 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.houseNumberOrName"))).Attributes["value"].Value);
                string str35 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.houseNumberOrName"))).Attributes["value"].Value);
                string str7 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressType"))).Attributes["value"].Value);
                string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dfValue"))).Attributes["value"].Value;
                string str40 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.city"))).Attributes["value"].Value);
                string str9 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.postalCode"))).Attributes["value"].Value);
                string str4 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.stateOrProvince"))).Attributes["value"].Value);
                string str10 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.country"))).Attributes["value"].Value);
                string str5 = "";
                string cCNumber = this._runner.Profile.CCNumber;
                while (cCNumber.Length > 4)
                {
                    str5 = str5 + cCNumber.Substring(0, 4);
                    cCNumber = cCNumber.Remove(0, 4);
                    str5 = str5 + " ";
                }
                str5 = str5 + cCNumber;
                this._data = "displayGroup=card";
                this._data = this._data + "&card.cardNumber=" + WebUtility.UrlEncode(str5);
                this._data = this._data + "&card.cardHolderName=" + WebUtility.UrlEncode(this._runner.Profile.NameOnCard);
                this._data = this._data + "&card.expiryMonth=" + WebUtility.UrlEncode(this._runner.Profile.ExpiryMonth);
                this._data = this._data + "&card.expiryYear=" + WebUtility.UrlEncode(this._runner.Profile.ExpiryYear);
                this._data = this._data + "&card.cvcCode=" + WebUtility.UrlEncode(this._runner.Profile.Cvv);
                this._data = this._data + "&sig=" + str38;
                this._data = this._data + "&merchantReference=" + str12;
                this._data = this._data + "&brandCode=" + str13;
                this._data = this._data + "&paymentAmount=" + str14;
                this._data = this._data + "&currencyCode=" + str15;
                this._data = this._data + "&shipBeforeDate=" + str36;
                this._data = this._data + "&skinCode=" + str26;
                this._data = this._data + "&merchantAccount=" + str27;
                this._data = this._data + "&shopperLocale=" + str28;
                this._data = this._data + "&stage=" + str16;
                this._data = this._data + "&sessionId=" + str42;
                this._data = this._data + "&orderData=" + str43;
                this._data = this._data + "&sessionValidity=" + str37;
                this._data = this._data + "&countryCode=" + str39;
                this._data = this._data + "&shopperEmail=" + str17;
                this._data = this._data + "&shopperReference=" + str21;
                this._data = this._data + "&merchantOrderReference=" + str22;
                this._data = this._data + "&resURL=" + str23;
                this._data = this._data + "&allowedMethods=" + str18;
                this._data = this._data + "&originalSession=" + str19;
                this._data = this._data + "&billingAddress.street=" + str20;
                this._data = this._data + "&billingAddress.city=" + str;
                this._data = this._data + "&billingAddress.postalCode=" + str2;
                if ((this._runner.Profile.CountryId == "US") || (this._runner.Profile.CountryId == "CA"))
                {
                    this._data = this._data + "&billingAddress.stateOrProvince=" + str3;
                }
                this._data = this._data + "&billingAddress.country=" + str24;
                this._data = this._data + "&shopper.firstName=" + str25;
                this._data = this._data + "&shopper.lastName=" + str29;
                this._data = this._data + "&shopper.gender=" + str11;
                this._data = this._data + "&shopper.telephoneNumber=" + str30;
                this._data = this._data + "&riskdata.deliveryMethod=" + str31;
                this._data = this._data + "&riskdata.sig=" + str32;
                this._data = this._data + "&referrerURL=https%3A%2F%2Fwww.sneakersnstuff.com%2Fen%2Fcart%2Fview";
                this._data = this._data + "&dfValue=" + str8;
                this._data = this._data + "&usingFrame=false";
                this._data = this._data + "&usingPopUp=false";
                this._data = this._data + "&shopperBehaviorLog=" + WebUtility.UrlEncode("{\"numberBind\":\"1\",\"holderNameBind\":\"1\",\"cvcBind\":\"1\",\"activate\":\"2\",\"numberFieldFocusCount\":\"1\",\"numberFieldLog\":\"fo@61,cl@62,Cd@68,KL@70,Cu@75,ch@76,bl@76\",\"numberFieldClickCount\":\"1\",\"numberFieldKeyCount\":\"2\",\"numberFieldChangeCount\":\"1\",\"numberFieldEvHa\":\"total=0\",\"numberFieldBlurCount\":\"1\",\"holderNameFieldFocusCount\":\"1\",\"holderNameFieldLog\":\"fo@76,cl@77,Sd@83,KL@84,Su@85,KL@86,KL@87,KL@88,KL@89,Ks@92,Sd@103,KL@105,Su@105,KL@106,KL@107,KL@108,KL@109,KU@111,ch@111,bl@111\",\"holderNameFieldClickCount\":\"1\",\"holderNameFieldKeyCount\":\"14\",\"holderNameUnkKeysFieldLog\":\"9@111\",\"holderNameFieldChangeCount\":\"1\",\"holderNameFieldEvHa\":\"total=0\",\"holderNameFieldBlurCount\":\"1\",\"cvcFieldFocusCount\":\"1\",\"cvcFieldLog\":\"fo@162,cl@163,KN@170,KN@170,KN@171\",\"cvcFieldClickCount\":\"1\",\"cvcFieldKeyCount\":\"3\"}");
                this._data = this._data + "&billingAddressSig=" + str41;
                this._data = this._data + "&billingAddress.houseNumberOrName=" + str35;
                this._data = this._data + "&billingAddressType=" + str7;
                this._data = this._data + "&deliveryAddress.street=" + str33;
                this._data = this._data + "&deliveryAddress.houseNumberOrName=" + str34;
                this._data = this._data + "&deliveryAddress.city=" + str40;
                this._data = this._data + "&deliveryAddress.postalCode=" + str9;
                if ((this._runner.Profile.CountryIdShipping == "US") || (this._runner.Profile.CountryIdShipping == "CA"))
                {
                    this._data = this._data + "&deliveryAddress.stateOrProvince=" + str4;
                }
                this._data = this._data + "&deliveryAddress.country=" + str10;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                this._request.KeepAlive = true;
                this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("Accept-Language", "sk-SK,sk;q=0.8,cs;q=0.6,en-US;q=0.4,en;q=0.2,nl;q=0.2");
                this._request.ContentLength = this._data.Length;
                this._request.Headers.Add("Origin", "https://www.sneakersnstuff.com");
                this._request.Referer = this._resUri;
                using (Stream stream = this._request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(this._data, this._data.Length);
                    }
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
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                    this._resUri = response.ResponseUri.ToString();
                }
                if (this._resUri.Contains("completeCard.shtml"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    return false;
                }
                this._currentDoc.LoadHtml(this._srr);
                string str48 = "";
                if (this._srr.Contains("byvisa") || this._srr.Contains("securecode"))
                {
                    bool flag2 = this._srr.Contains("byvisa");
                    string str47 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                    string str45 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    string str46 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"))).Attributes["value"].Value;
                    string requestUriString = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pageform"))).Attributes["action"].Value;
                    this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    if (this._runner.Proxy != null)
                    {
                        this._request.Proxy = this._runner.Proxy;
                    }
                    this._request.CookieContainer = this._runner.Cookies;
                    this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                    this._request.KeepAlive = true;
                    this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    this._request.ContentType = "application/x-www-form-urlencoded";
                    this._request.Method = "POST";
                    this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    this._data = "PaReq=" + WebUtility.UrlEncode(str47);
                    this._data = this._data + "&MD=" + WebUtility.UrlEncode(str45);
                    this._data = this._data + "&TermUrl=" + WebUtility.UrlEncode(str46);
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
                    this._currentDoc.LoadHtml(this._srr);
                    if (flag2)
                    {
                        str47 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                        str45 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                        requestUriString = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                        this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
                        if (this._runner.Proxy != null)
                        {
                            this._request.Proxy = this._runner.Proxy;
                        }
                        this._request.CookieContainer = this._runner.Cookies;
                        this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                        this._request.KeepAlive = true;
                        this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        this._request.ContentType = "application/x-www-form-urlencoded";
                        this._request.Method = "POST";
                        this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                        this._data = "PaRes=" + WebUtility.UrlEncode(str47);
                        this._data = this._data + "&MD=" + WebUtility.UrlEncode(str45);
                        this._bytes = Encoding.ASCII.GetBytes(this._data);
                        this._request.ContentLength = this._bytes.Length;
                        using (Stream stream3 = this._request.GetRequestStream())
                        {
                            stream3.Write(this._bytes, 0, this._bytes.Length);
                        }
                        using (WebResponse response3 = this._request.GetResponse())
                        {
                            using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                            {
                                this._srr = reader3.ReadToEnd();
                            }
                            str48 = response3.ResponseUri.ToString().ToLowerInvariant();
                            goto Label_20D1;
                        }
                    }
                    string str49 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                    str45 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    string str50 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"))).Attributes["value"].Value;
                    requestUriString = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"))).Attributes["action"].Value;
                    this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    if (this._runner.Proxy != null)
                    {
                        this._request.Proxy = this._runner.Proxy;
                    }
                    this._request.CookieContainer = this._runner.Cookies;
                    this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                    this._request.KeepAlive = true;
                    this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    this._request.ContentType = "application/x-www-form-urlencoded";
                    this._request.Method = "POST";
                    this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    this._data = "PaRes=" + WebUtility.UrlEncode(str49);
                    this._data = this._data + "&MD=" + WebUtility.UrlEncode(str45);
                    this._data = this._data + "&PaReq=" + WebUtility.UrlEncode(str49);
                    this._data = this._data + "&ABSlog=" + WebUtility.UrlEncode(str50);
                    this._data = this._data + "&deviceDNA=";
                    this._data = this._data + "&executionTime=";
                    this._data = this._data + "&dnaError=";
                    this._data = this._data + "&mesc=";
                    this._data = this._data + "&mescIterationCount=0";
                    this._data = this._data + "&desc=";
                    this._data = this._data + "&isDNADone=false";
                    this._data = this._data + "&arcotFlashCookie=";
                    this._bytes = Encoding.ASCII.GetBytes(this._data);
                    this._request.ContentLength = this._bytes.Length;
                    using (Stream stream4 = this._request.GetRequestStream())
                    {
                        stream4.Write(this._bytes, 0, this._bytes.Length);
                    }
                    using (WebResponse response4 = this._request.GetResponse())
                    {
                        using (StreamReader reader4 = new StreamReader(response4.GetResponseStream()))
                        {
                            this._srr = reader4.ReadToEnd();
                        }
                        str48 = response4.ResponseUri.ToString().ToLowerInvariant();
                    }
                }
            Label_20D1:
                if (str48 != "https://www.sneakersnstuff.com/en/cart/view")
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    return false;
                }
                if (this._srr.Contains("Authentication for 3D-Secure"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.D3_SECURE_FAILED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.D3_SECURE_FAILED, null, "", "");
                    return false;
                }
                try
                {
                    EveAIO.Helpers.AddDbValue("SNS|" + str48 + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                if (this._srr.Contains("Your order is received"))
                {
                    return true;
                }
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
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
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
                string str = "";
                if (this._runner.Profile.CountryId != "US")
                {
                    if (this._runner.Profile.CountryId == "CA")
                    {
                        str = "&regionId=" + EveAIO.Helpers.GetSNSCanadaStates().First<KeyValuePair<string, string>>(x => (x.Value == this._runner.Profile.StateId)).Key;
                        this._regionCode = EveAIO.Helpers.GetSNSCanadaStates().First<KeyValuePair<string, string>>(x => (x.Value == this._runner.Profile.StateId)).Key;
                    }
                }
                else
                {
                    str = "&regionId=" + EveAIO.Helpers.GetSNSUsaStates().First<KeyValuePair<string, string>>(x => (x.Value == this._runner.Profile.StateId)).Key;
                    this._regionCode = EveAIO.Helpers.GetSNSUsaStates().First<KeyValuePair<string, string>>(x => (x.Value == this._runner.Profile.StateId)).Key;
                }
                this._request = (HttpWebRequest) WebRequest.Create($"https://www.sneakersnstuff.com/customer/setregion?countryCode={this._runner.Profile.CountryId}{str}&redirectUrl=%2Fen%2Fcart%2Fview");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Referer = "https://www.sneakersnstuff.com/en/cart/view";
                this._request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sneakersnstuff.com/en/cart/setpaymentmethod");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.KeepAlive = true;
                this._request.Referer = "https://www.sneakersnstuff.com/en/cart/view";
                this._request.ServicePoint.Expect100Continue = false;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.Headers.Add("X-AntiCsrfToken", this._token);
                string str2 = "";
                if (this._runner.Profile.CardTypeId == "0")
                {
                    str2 = "55";
                }
                else if ((this._runner.Profile.CardTypeId == "1") || (this._runner.Profile.CardTypeId == "2"))
                {
                    str2 = "56";
                }
                this._data = "id=" + str2 + "&partial=cart-contents";
                this._request.Method = "POST";
                this._request.ContentLength = this._data.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(this._data, this._data.Length);
                    }
                }
                using (WebResponse response2 = this._request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        this._srr = reader2.ReadToEnd();
                    }
                }
                return true;
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
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
            public static readonly Sneakernstuff.<>c <>9;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__15_2;
            public static Func<HtmlNode, bool> <>9__15_0;
            public static Func<HtmlNode, bool> <>9__15_3;
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
            public static Func<HtmlNode, bool> <>9__20_5;
            public static Func<HtmlNode, bool> <>9__20_6;
            public static Func<HtmlNode, bool> <>9__20_7;
            public static Func<HtmlNode, bool> <>9__20_0;
            public static Func<HtmlNode, bool> <>9__20_1;
            public static Func<HtmlNode, bool> <>9__20_2;
            public static Func<HtmlNode, bool> <>9__20_3;
            public static Func<HtmlNode, bool> <>9__20_4;
            public static Func<HtmlNode, bool> <>9__20_10;
            public static Func<HtmlNode, bool> <>9__20_11;
            public static Func<HtmlNode, bool> <>9__20_12;
            public static Func<HtmlNode, bool> <>9__21_0;
            public static Func<HtmlNode, bool> <>9__21_1;
            public static Func<HtmlNode, bool> <>9__22_2;
            public static Func<HtmlNode, bool> <>9__22_3;
            public static Func<HtmlNode, bool> <>9__22_4;
            public static Func<HtmlNode, bool> <>9__22_7;
            public static Func<HtmlNode, bool> <>9__22_8;
            public static Func<HtmlNode, bool> <>9__22_9;
            public static Func<HtmlNode, bool> <>9__22_0;
            public static Func<HtmlNode, bool> <>9__22_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Sneakernstuff.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__15_0(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <Atc>b__15_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "add-to-cart-form"));

            internal bool <Atc>b__15_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-row "));

            internal bool <Atc>b__15_3(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <DirectLink>b__20_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_AntiCsrfToken"));

            internal bool <DirectLink>b__20_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "hasStock"));

            internal bool <DirectLink>b__20_10(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-color"));

            internal bool <DirectLink>b__20_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-artno"));

            internal bool <DirectLink>b__20_12(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sizes "));

            internal bool <DirectLink>b__20_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-name"));

            internal bool <DirectLink>b__20_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"));

            internal bool <DirectLink>b__20_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "primary-image"));

            internal bool <DirectLink>b__20_5(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <DirectLink>b__20_6(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <DirectLink>b__20_7(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <Login>b__21_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-row "));

            internal bool <Login>b__21_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "remove"));

            internal bool <Search>b__22_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "control next"));

            internal bool <Search>b__22_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "control next"));

            internal bool <Search>b__22_2(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <Search>b__22_3(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <Search>b__22_4(HtmlNode x) => 
                (x.Attributes["data-sitekey"] > null);

            internal bool <Search>b__22_7(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("product c-"));

            internal bool <Search>b__22_8(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("name"));

            internal bool <Search>b__22_9(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("name"));

            internal bool <SubmitOrder>b__17_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sig"));

            internal bool <SubmitOrder>b__17_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantReference"));

            internal bool <SubmitOrder>b__17_10(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionId"));

            internal bool <SubmitOrder>b__17_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "orderData"));

            internal bool <SubmitOrder>b__17_12(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sessionValidity"));

            internal bool <SubmitOrder>b__17_13(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "countryCode"));

            internal bool <SubmitOrder>b__17_14(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperEmail"));

            internal bool <SubmitOrder>b__17_15(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperReference"));

            internal bool <SubmitOrder>b__17_16(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantOrderReference"));

            internal bool <SubmitOrder>b__17_17(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "resURL"));

            internal bool <SubmitOrder>b__17_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "allowedMethods"));

            internal bool <SubmitOrder>b__17_19(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "originalSession"));

            internal bool <SubmitOrder>b__17_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "brandCode"));

            internal bool <SubmitOrder>b__17_20(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.street"));

            internal bool <SubmitOrder>b__17_21(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.city"));

            internal bool <SubmitOrder>b__17_22(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.postalCode"));

            internal bool <SubmitOrder>b__17_23(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.stateOrProvince"));

            internal bool <SubmitOrder>b__17_24(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.country"));

            internal bool <SubmitOrder>b__17_25(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressSig"));

            internal bool <SubmitOrder>b__17_26(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.firstName"));

            internal bool <SubmitOrder>b__17_27(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.lastName"));

            internal bool <SubmitOrder>b__17_28(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.gender"));

            internal bool <SubmitOrder>b__17_29(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopper.telephoneNumber"));

            internal bool <SubmitOrder>b__17_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentAmount"));

            internal bool <SubmitOrder>b__17_30(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "riskdata.deliveryMethod"));

            internal bool <SubmitOrder>b__17_31(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "riskdata.sig"));

            internal bool <SubmitOrder>b__17_32(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.street"));

            internal bool <SubmitOrder>b__17_33(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.houseNumberOrName"));

            internal bool <SubmitOrder>b__17_34(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddress.houseNumberOrName"));

            internal bool <SubmitOrder>b__17_35(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "billingAddressType"));

            internal bool <SubmitOrder>b__17_36(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dfValue"));

            internal bool <SubmitOrder>b__17_37(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.city"));

            internal bool <SubmitOrder>b__17_38(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.postalCode"));

            internal bool <SubmitOrder>b__17_39(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.stateOrProvince"));

            internal bool <SubmitOrder>b__17_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currencyCode"));

            internal bool <SubmitOrder>b__17_40(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deliveryAddress.country"));

            internal bool <SubmitOrder>b__17_41(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitOrder>b__17_42(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__17_43(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"));

            internal bool <SubmitOrder>b__17_44(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pageform"));

            internal bool <SubmitOrder>b__17_45(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__17_46(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__17_47(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__17_48(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__17_49(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"));

            internal bool <SubmitOrder>b__17_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipBeforeDate"));

            internal bool <SubmitOrder>b__17_50(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <SubmitOrder>b__17_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "skinCode"));

            internal bool <SubmitOrder>b__17_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchantAccount"));

            internal bool <SubmitOrder>b__17_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopperLocale"));

            internal bool <SubmitOrder>b__17_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stage"));
        }

        [CompilerGenerated]
        private static class <>o__21
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
        }
    }
}

