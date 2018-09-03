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
    using System.Net.Cache;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Sivasdescalzo : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _sizegroup;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        [Dynamic]
        private object _dynObj;
        private string _data;
        private bool _isLoggedIn;
        private string _formKey;
        private string _shippingMethod;
        private string _radioShipping;
        private string _rsaKey;
        private static string _adyen;
        private Dictionary<string, string> _diData;

        public Sivasdescalzo(TaskRunner runner, TaskObject task)
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
                string str2 = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "form_key"))).Attributes["value"].Value;
                HtmlNode node = this._runner.ProductPageHtml.DocumentNode.Descendants("form").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_addtocart_form"));
                string str = node.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "product"))).Attributes["value"].Value;
                this._request = (HttpWebRequest) WebRequest.Create(node.Attributes["action"].Value);
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "application/json, text/javascript, */*; q=0.01";
                this._request.KeepAlive = true;
                this._request.Referer = this._runner.Product.Link;
                this._request.Headers.Add("Origin", "https://www.sivasdescalzo.com");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._data = "form_key=" + str2;
                this._data = this._data + "&product=" + str;
                this._data = this._data + "&related_product=";
                this._data = this._data + "&size_group=" + this._sizegroup;
                this._data = this._data + "&super_attribute%5B138%5D=" + this._runner.PickedSize.Value.Value;
                if (this._runner.ProductPageHtml.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "recaptcha_html_element")))
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.SIVAS_CAPTCHA_KEY, "https://www.sivasdescalzo.com", "Sivas");
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
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__19.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__19.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__19.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__19.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__19.<>p__3.Target(<>o__19.<>p__3, <>o__19.<>p__2.Target(<>o__19.<>p__2, <>o__19.<>p__1.Target(<>o__19.<>p__1, <>o__19.<>p__0.Target(<>o__19.<>p__0, this._dynObj, "error")), false)))
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
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
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
            switch (((-1856766390 ^ -665604996) % 5))
            {
                case 0:
                    goto Label_001C;

                case 1:
                    return false;

                case 3:
                    break;

                case 4:
                    return false;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (!this.SubmitBilling())
            {
            }
            if (-2075409600 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            if (!this._isLoggedIn && !this.Login())
            {
                return false;
            }
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                this._request = (HttpWebRequest) WebRequest.Create(link);
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                this._request.Headers.Add("Accept-Language", "sk-SK,sk;q=0.8,cs;q=0.6,en-US;q=0.4,en;q=0.2");
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
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                if (!this._runner.ProductPageHtml.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value.ToLowerInvariant() == "content size-options size_us-options"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                this._sizegroup = "size_us";
                string str = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "brand"))).InnerText.Trim() + " " + this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "model-1"))).InnerText.Trim();
                string str2 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-price"))).InnerText.Trim();
                string str3 = this._runner.ProductPageHtml.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-carousel"))).Descendants("img").First<HtmlNode>().Attributes["src"].Value.Trim();
                this._task.ImgUrl = str3;
                Product product1 = new Product {
                    ProductTitle = str,
                    Link = link,
                    Price = str2
                };
                this._runner.Product = product1;
                foreach (HtmlNode node in from x in this._runner.ProductPageHtml.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value.ToLowerInvariant() == "content size-options size_us-options"))).Descendants("a")
                    where (x.Attributes["class"] != null) && !x.Attributes["class"].Value.Contains("unavailable")
                    select x)
                {
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(node.InnerText.Trim(), node.Attributes["data-optionIndex"].Value);
                    this._runner.Product.AvailableSizes.Add(item);
                }
                if (this._runner.Product.AvailableSizes.Count == 0)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
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
                    double num2 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                                    goto Label_06F8;
                                }
                            }
                            continue;
                        Label_06F8:
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
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "");
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
            if (string.IsNullOrEmpty(_adyen))
            {
                Task.Factory.StartNew(delegate {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://www.sivasdescalzo.com/js/adyen/payment/adyen.encrypt_0_1_18.js");
                    request.KeepAlive = true;
                    request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.162 Mobile Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                    request.Accept = "*/*";
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                    {
                        _adyen = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    }
                });
            }
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGGING_IN, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sivasdescalzo.com/req.php?lang=en");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "application/json, text/javascript, */*; q=0.01";
                this._request.KeepAlive = true;
                this._request.Referer = this._task.Link;
                this._request.Headers.Add("Origin", "https://www.sivasdescalzo.com");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Reload);
                this._data = "v%5B%5D=form_key&v%5B%5D=referer_url~" + WebUtility.UrlEncode(this._task.Link);
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
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__25.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__25.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sivasdescalzo), argumentInfo));
                }
                object obj2 = <>o__25.<>p__1.Target(<>o__25.<>p__1, <>o__25.<>p__0.Target(<>o__25.<>p__0, this._dynObj, "form_key"));
                if (<>o__25.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__2 = CallSite<Func<CallSite, Type, string, object, Cookie>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Sivasdescalzo), argumentInfo));
                }
                Cookie cookie = <>o__25.<>p__2.Target(<>o__25.<>p__2, typeof(Cookie), "PAGECACHE_FORMKEY", obj2);
                cookie.Domain = new Uri("https://www.sivasdescalzo.com").Host;
                this._runner.Cookies.Add(cookie);
                Cookie cookie1 = new Cookie("traffic_src", WebUtility.UrlEncode("{\"ga_gclid\":\"\",\"ga_source\":\"(direct)\",\"ga_medium\":\"(none)\",\"ga_campaign\":\"\",\"ga_content\":\"\",\"ga_keyword\":\"\",\"ga_landing_page\":\"https://www.sivasdescalzo.com/en/lifestyle\",\"ga_client_id\":\"1318267545.1525881450\"}")) {
                    Domain = new Uri("https://www.sivasdescalzo.com").Host
                };
                this._runner.Cookies.Add(cookie1);
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sivasdescalzo.com/en/customer/account/loginPost/");
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
                this._request.Referer = this._task.Link;
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.ServicePoint.Expect100Continue = false;
                this._request.Headers.Add("Cache-Control", "max-age=0");
                this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Headers.Add("Origin", "https://www.sivasdescalzo.com");
                if (<>o__25.<>p__4 == null)
                {
                    <>o__25.<>p__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sivasdescalzo)));
                }
                if (<>o__25.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__3 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Sivasdescalzo), argumentInfo));
                }
                this._data = <>o__25.<>p__4.Target(<>o__25.<>p__4, <>o__25.<>p__3.Target(<>o__25.<>p__3, "form_key=", obj2));
                this._data = this._data + "&login%5Busername%5D=" + WebUtility.UrlEncode(this._task.Username);
                this._data = this._data + "&login%5Bpassword%5D=" + this._task.Password;
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
                if (!this._srr.ToLowerInvariant().Contains("Invalid login or password".ToLowerInvariant()))
                {
                    this._isLoggedIn = true;
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_SUCCESSFUL, null, "", "");
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_LOGIN);
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
            throw new NotImplementedException();
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, true);
            this._client.SetDesktopAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.sivasdescalzo.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.sivasdescalzo.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                obj2.Email.Trim();
                string str = obj2.FirstName.Trim();
                string str2 = obj2.LastName.Trim();
                string str3 = obj2.Address1.Trim();
                string str4 = obj2.Address2.Trim();
                string str5 = obj2.City.Trim();
                string str6 = obj2.Zip.Trim();
                string str7 = obj2.CountryId.ToUpperInvariant();
                string state = "";
                if ((str7 != "US") && (str7 != "CA"))
                {
                    state = obj2.State;
                }
                else
                {
                    state = obj2.StateId;
                }
                string str9 = obj2.Phone.Trim();
                ProfileObject obj3 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                obj3.EmailShipping.Trim();
                string str10 = obj3.FirstNameShipping.Trim();
                string str11 = obj3.LastNameShipping.Trim();
                string str12 = obj3.Address1Shipping.Trim();
                string str13 = obj3.Address2Shipping.Trim();
                string str14 = obj3.CityShipping.Trim();
                string str15 = obj3.ZipShipping.Trim();
                string str16 = obj3.CountryIdShipping.ToUpperInvariant();
                string stateShipping = "";
                if ((str16 != "us") && (str16 != "ca"))
                {
                    stateShipping = obj3.StateShipping;
                }
                else
                {
                    stateShipping = obj3.StateIdShipping;
                }
                string str18 = obj3.PhoneShipping.Trim();
                this._request = (HttpWebRequest) WebRequest.Create($"https://www.sivasdescalzo.com/en/checkout/onepage/updatePayment/form_key/{this._formKey}/");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "*/*";
                this._request.KeepAlive = true;
                this._request.Referer = "https://www.sivasdescalzo.com/en/checkout/onepage/";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Method = "POST";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._data = "shipping%5Baddress_title%5D=New+Address";
                this._data = this._data + "&shipping%5Bfirstname%5D=" + WebUtility.UrlEncode(str10);
                this._data = this._data + "&shipping%5Blastname%5D=" + WebUtility.UrlEncode(str11);
                this._data = this._data + "&shipping%5Bstreet%5D%5B0%5D=" + WebUtility.UrlEncode(str12);
                this._data = this._data + "&shipping%5Bstreet%5D%5B1%5D=" + WebUtility.UrlEncode(str13);
                this._data = this._data + "&shipping%5Bpostcode%5D=" + WebUtility.UrlEncode(str15);
                this._data = this._data + "&shipping%5Bcountry_id%5D=" + WebUtility.UrlEncode(str16);
                this._data = this._data + "&shipping%5Bregion_id%5D=";
                this._data = this._data + "&shipping%5Bregion%5D=" + (string.IsNullOrEmpty(stateShipping) ? "n%2Fa" : stateShipping);
                this._data = this._data + "&shipping%5Bcity%5D=" + WebUtility.UrlEncode(str14);
                this._data = this._data + "&shipping%5Btaxvat%5D=";
                this._data = this._data + "&shipping%5Btelephone%5D=" + WebUtility.UrlEncode(str18);
                this._data = this._data + "&billing%5Baddress_title%5D=New+Address";
                this._data = this._data + "&billing%5Bfirstname%5D=" + WebUtility.UrlEncode(str);
                this._data = this._data + "&billing%5Blastname%5D=" + WebUtility.UrlEncode(str2);
                this._data = this._data + "&billing%5Bstreet%5D%5B0%5D=" + WebUtility.UrlEncode(str3);
                this._data = this._data + "&billing%5Bstreet%5D%5B1%5D=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&billing%5Bpostcode%5D=" + WebUtility.UrlEncode(str6);
                this._data = this._data + "&billing%5Bcountry_id%5D=" + WebUtility.UrlEncode(str7);
                this._data = this._data + "&billing%5Bregion_id%5D=";
                this._data = this._data + "&billing%5Bregion%5D=" + (string.IsNullOrEmpty(state) ? "n%2Fa" : state);
                this._data = this._data + "&billing%5Bcity%5D=" + WebUtility.UrlEncode(str5);
                this._data = this._data + "&billing%5Btelephone%5D=" + WebUtility.UrlEncode(str9);
                this._data = this._data + "&billing%5Btaxvat%5D=";
                this._data = this._data + "&billing%5Buser_taxvat%5D=";
                this._data = this._data + "&radio_shipping=" + this._radioShipping;
                this._data = this._data + "&shipping_method=" + this._shippingMethod;
                this._data = this._data + "&radio_payment=adyen_cc";
                this._data = this._data + "&payment%5Bmethod%5D=adyen_cc";
                this._data = this._data + "&payment%5Bcc_type%5D=";
                this._data = this._data + "&payment%5Bencrypted_data%5D=";
                this._data = this._data + "&newsletter%5B2%5D=1";
                this._data = this._data + "&agreement%5B4%5D=1";
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
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sivasdescalzo.com/req.php?lang=en");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                this._request.Headers.Add("Accept-Language", "sk-SK,sk;q=0.8,cs;q=0.6,en-US;q=0.4,en;q=0.2");
                this._request.CookieContainer = this._runner.Cookies;
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Method = "POST";
                this._data = "v%5B%5D=form_key&v%5B%5D=referer_url~https%3A%2F%2Fwww.sivasdescalzo.com%2Fen%2Fcheckout%2Fonepage%2F";
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
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
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
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                obj2.Email.Trim();
                string str10 = obj2.FirstName.Trim();
                string str16 = obj2.LastName.Trim();
                string str9 = obj2.Address1.Trim();
                string str18 = obj2.Address2.Trim();
                string str19 = obj2.City.Trim();
                string str20 = obj2.Zip.Trim();
                string str17 = obj2.CountryId.ToUpperInvariant();
                string state = "";
                if ((str17 != "us") && (str17 != "ca"))
                {
                    state = obj2.State;
                }
                else
                {
                    state = obj2.StateId;
                }
                string str4 = obj2.Phone.Trim();
                ProfileObject obj3 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                obj3.EmailShipping.Trim();
                string str21 = obj3.FirstNameShipping.Trim();
                string str12 = obj3.LastNameShipping.Trim();
                string str22 = obj3.Address1Shipping.Trim();
                string str5 = obj3.Address2Shipping.Trim();
                string str14 = obj3.CityShipping.Trim();
                string str8 = obj3.ZipShipping.Trim();
                string str3 = obj3.CountryIdShipping.ToUpperInvariant();
                string stateShipping = "";
                if ((str3 != "us") && (str3 != "ca"))
                {
                    stateShipping = obj3.StateShipping;
                }
                else
                {
                    stateShipping = obj3.StateIdShipping;
                }
                string str15 = obj3.PhoneShipping.Trim();
                this._request = (HttpWebRequest) WebRequest.Create($"https://www.sivasdescalzo.com/en/checkout/onepage/saveOrder/form_key/{this._formKey}/");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Accept = "*/*";
                this._request.KeepAlive = true;
                this._request.Headers.Add("Origin", "https://www.sivasdescalzo.com");
                this._request.Referer = "https://www.sivasdescalzo.com/en/checkout/onepage/";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Method = "POST";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.Headers.Add("X-Prototype-Version", "1.7");
                string cCNumber = obj2.CCNumber;
                string cc = "";
                while (cCNumber.Length > 4)
                {
                    cc = cc + cCNumber.Substring(0, 4);
                    cCNumber = cCNumber.Remove(0, 4);
                    cc = cc + " ";
                }
                cc = cc + cCNumber;
                string str7 = Global.SENSOR.EncryptSivas(this._rsaKey, _adyen, cc, this._runner.Profile.Cvv, this._runner.Profile.NameOnCard, this._runner.Profile.ExpiryMonth, this._runner.Profile.ExpiryYear);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str7);
                string str23 = "";
                switch (this._runner.Profile.CardTypeId)
                {
                    case "0":
                        throw new Exception("Unsupported CC brand");

                    case "1":
                        str23 = "VI";
                        break;

                    case "2":
                        str23 = "MC";
                        break;

                    case "3":
                        throw new Exception("Unsupported CC brand");
                }
                this._data = "shipping%5Baddress_title%5D=New+Address";
                this._data = this._data + "&shipping%5Bfirstname%5D=" + WebUtility.UrlEncode(str21);
                this._data = this._data + "&shipping%5Blastname%5D=" + WebUtility.UrlEncode(str12);
                this._data = this._data + "&shipping%5Bstreet%5D%5B0%5D=" + WebUtility.UrlEncode(str22);
                this._data = this._data + "&shipping%5Bstreet%5D%5B1%5D=" + WebUtility.UrlEncode(str5);
                this._data = this._data + "&shipping%5Bpostcode%5D=" + WebUtility.UrlEncode(str8);
                this._data = this._data + "&shipping%5Bcountry_id%5D=" + WebUtility.UrlEncode(str3);
                this._data = this._data + "&shipping%5Bregion_id%5D=";
                this._data = this._data + "&shipping%5Bregion%5D=" + (string.IsNullOrEmpty(stateShipping) ? "n%2Fa" : stateShipping);
                this._data = this._data + "&shipping%5Bcity%5D=" + WebUtility.UrlEncode(str14);
                this._data = this._data + "&shipping%5Btaxvat%5D=";
                this._data = this._data + "&shipping%5Btelephone%5D=" + WebUtility.UrlEncode(str15);
                this._data = this._data + "&billing%5Baddress_title%5D=New+Address";
                this._data = this._data + "&billing%5Bfirstname%5D=" + WebUtility.UrlEncode(str10);
                this._data = this._data + "&billing%5Blastname%5D=" + WebUtility.UrlEncode(str16);
                this._data = this._data + "&billing%5Bstreet%5D%5B0%5D=" + WebUtility.UrlEncode(str9);
                this._data = this._data + "&billing%5Bstreet%5D%5B1%5D=" + WebUtility.UrlEncode(str18);
                this._data = this._data + "&billing%5Bpostcode%5D=" + WebUtility.UrlEncode(str20);
                this._data = this._data + "&billing%5Bcountry_id%5D=" + WebUtility.UrlEncode(str17);
                this._data = this._data + "&billing%5Bregion_id%5D=";
                this._data = this._data + "&billing%5Bregion%5D=" + (string.IsNullOrEmpty(state) ? "n%2Fa" : state);
                this._data = this._data + "&billing%5Bcity%5D=" + WebUtility.UrlEncode(str19);
                this._data = this._data + "&billing%5Btelephone%5D=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&billing%5Btaxvat%5D=";
                this._data = this._data + "&billing%5Buser_taxvat%5D=";
                this._data = this._data + "&radio_shipping=" + this._radioShipping;
                this._data = this._data + "&shipping_method=" + this._shippingMethod;
                this._data = this._data + "&radio_payment=adyen_cc";
                this._data = this._data + "&payment%5Bmethod%5D=adyen_cc";
                this._data = this._data + "&payment%5Bcc_type%5D=" + str23;
                if (<>o__21.<>p__5 == null)
                {
                    <>o__21.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Sivasdescalzo)));
                }
                if (<>o__21.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__21.<>p__4 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__21.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__21.<>p__3 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__21.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__21.<>p__2 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "UrlEncode", null, typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__21.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__21.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sivasdescalzo), argumentInfo));
                }
                if (<>o__21.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__21.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sivasdescalzo), argumentInfo));
                }
                this._data = <>o__21.<>p__5.Target(<>o__21.<>p__5, <>o__21.<>p__4.Target(<>o__21.<>p__4, this._data, <>o__21.<>p__3.Target(<>o__21.<>p__3, "&payment%5Bencrypted_data%5D=", <>o__21.<>p__2.Target(<>o__21.<>p__2, typeof(WebUtility), <>o__21.<>p__1.Target(<>o__21.<>p__1, <>o__21.<>p__0.Target(<>o__21.<>p__0, this._dynObj, "adyen-encrypted-data"))))));
                this._data = this._data + "&newsletter%5B2%5D=";
                this._data = this._data + "&agreement%5B4%5D=1";
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
                    try
                    {
                        EveAIO.Helpers.AddDbValue("Sivas|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    }
                    catch
                    {
                    }
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                    if (<>o__21.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sivasdescalzo), argumentInfo));
                    }
                    if (<>o__21.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__8 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Sivasdescalzo), argumentInfo));
                    }
                    if (<>o__21.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sivasdescalzo), argumentInfo));
                    }
                    if (<>o__21.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sivasdescalzo), argumentInfo));
                    }
                    if (<>o__21.<>p__9.Target(<>o__21.<>p__9, <>o__21.<>p__8.Target(<>o__21.<>p__8, <>o__21.<>p__7.Target(<>o__21.<>p__7, <>o__21.<>p__6.Target(<>o__21.<>p__6, this._dynObj, "success")), false)))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        return false;
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
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
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
                this.SetClient();
                this._isLoggedIn = false;
            }
            return flag;
        }

        private bool SubmitShipping()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                obj2.EmailShipping.Trim();
                string str = obj2.FirstNameShipping.Trim();
                string str2 = obj2.LastNameShipping.Trim();
                string str3 = obj2.Address1Shipping.Trim();
                string str4 = obj2.Address2Shipping.Trim();
                string str5 = obj2.CityShipping.Trim();
                string str6 = obj2.ZipShipping.Trim();
                string str7 = obj2.CountryIdShipping.ToUpperInvariant();
                string stateShipping = "";
                if ((str7 != "US") && (str7 != "CA"))
                {
                    stateShipping = obj2.StateShipping;
                }
                else
                {
                    stateShipping = obj2.StateIdShipping;
                }
                string str9 = obj2.PhoneShipping.Trim();
                obj2.Email.Trim();
                string str10 = obj2.FirstName.Trim();
                string str11 = obj2.LastName.Trim();
                string str12 = obj2.Address1.Trim();
                string str13 = obj2.Address2.Trim();
                string str14 = obj2.City.Trim();
                string str15 = obj2.Zip.Trim();
                string str16 = obj2.CountryId.ToUpperInvariant();
                string state = "";
                if ((str16 != "US") && (str16 != "CA"))
                {
                    state = obj2.State;
                }
                else
                {
                    state = obj2.StateId;
                }
                string str18 = obj2.Phone.Trim();
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sivasdescalzo.com/en/checkout/cart/estimateShipping/");
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
                this._request.Referer = this._task.Link;
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Method = "POST";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._data = "countryId=" + str7;
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
                this._request = (HttpWebRequest) WebRequest.Create("https://www.sivasdescalzo.com/en/checkout/onepage/");
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
                this._request.Referer = this._task.Link;
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                using (WebResponse response2 = this._request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        this._srr = reader2.ReadToEnd();
                    }
                }
                string str19 = this._srr.Substring(this._srr.IndexOf("PAGECACHE_ENV"));
                str19 = str19.Substring(str19.IndexOf(","));
                str19 = str19.Substring(str19.IndexOf("'") + 1);
                str19 = str19.Substring(0, str19.IndexOf("'"));
                Cookie cookie = new Cookie("PAGECACHE_ENV", str19) {
                    Domain = new Uri("https://www.sivasdescalzo.com").Host
                };
                this._runner.Cookies.Add(cookie);
                this._formKey = this._srr.Substring(this._srr.IndexOf("updatePaymentUrl"));
                this._formKey = this._formKey.Substring(this._formKey.IndexOf("form_key"));
                this._formKey = this._formKey.Substring(this._formKey.IndexOf("/") + 1);
                this._formKey = this._formKey.Substring(0, this._formKey.IndexOf("/"));
                this._currentDoc.LoadHtml(this._srr);
                this._radioShipping = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "radio_shipping"))).Attributes["value"].Value;
                this._shippingMethod = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipping_method"))).Attributes["value"].Value;
                this._rsaKey = this._srr.Substring(this._srr.IndexOf("cse_key"));
                this._rsaKey = this._rsaKey.Substring(this._rsaKey.IndexOf("\"") + 1);
                this._rsaKey = this._rsaKey.Substring(0, this._rsaKey.IndexOf("\""));
                this._request = (HttpWebRequest) WebRequest.Create($"https://www.sivasdescalzo.com/en/checkout/onepage/updateShipping/form_key/{this._formKey}/");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.CookieContainer = this._runner.Cookies;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                this._request.Accept = "*/*";
                this._request.KeepAlive = true;
                this._request.Referer = "https://www.sivasdescalzo.com/en/checkout/onepage/";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Method = "POST";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._data = "shipping%5Baddress_title%5D=New+Address";
                this._data = this._data + "&shipping%5Bfirstname%5D=" + WebUtility.UrlEncode(str);
                this._data = this._data + "&shipping%5Blastname%5D=" + WebUtility.UrlEncode(str2);
                this._data = this._data + "&shipping%5Bstreet%5D%5B0%5D=" + WebUtility.UrlEncode(str3);
                this._data = this._data + "&shipping%5Bstreet%5D%5B1%5D=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&shipping%5Bpostcode%5D=" + WebUtility.UrlEncode(str6);
                this._data = this._data + "&shipping%5Bcountry_id%5D=" + WebUtility.UrlEncode(str7);
                this._data = this._data + "&shipping%5Bregion_id%5D=";
                this._data = this._data + "&shipping%5Bregion%5D=" + (string.IsNullOrEmpty(stateShipping) ? "n%2Fa" : stateShipping);
                this._data = this._data + "&shipping%5Bcity%5D=" + WebUtility.UrlEncode(str5);
                this._data = this._data + "&shipping%5Btaxvat%5D=";
                this._data = this._data + "&shipping%5Btelephone%5D=" + WebUtility.UrlEncode(str9);
                this._data = this._data + "&billing%5Baddress_title%5D=New+Address";
                this._data = this._data + "&billing%5Bfirstname%5D=" + WebUtility.UrlEncode(str10);
                this._data = this._data + "&billing%5Blastname%5D=" + WebUtility.UrlEncode(str11);
                this._data = this._data + "&billing%5Bstreet%5D%5B0%5D=" + WebUtility.UrlEncode(str12);
                this._data = this._data + "&billing%5Bstreet%5D%5B1%5D=" + WebUtility.UrlEncode(str13);
                this._data = this._data + "&billing%5Bpostcode%5D=" + WebUtility.UrlEncode(str15);
                this._data = this._data + "&billing%5Bcountry_id%5D=" + WebUtility.UrlEncode(str16);
                this._data = this._data + "&billing%5Bregion_id%5D=";
                this._data = this._data + "&billing%5Bregion%5D=" + (string.IsNullOrEmpty(state) ? "n%2Fa" : state);
                this._data = this._data + "&billing%5Bcity%5D=" + WebUtility.UrlEncode(str14);
                this._data = this._data + "&billing%5Btelephone%5D=" + WebUtility.UrlEncode(str18);
                this._data = this._data + "&billing%5Btaxvat%5D=";
                this._data = this._data + "&billing%5Buser_taxvat%5D=";
                this._data = this._data + "&radio_shipping=" + this._radioShipping;
                this._data = this._data + "&shipping_method=" + this._shippingMethod;
                this._data = this._data + "&radio_payment=adyen_cc";
                this._data = this._data + "&payment%5Bmethod%5D=adyen_cc";
                this._data = this._data + "&payment%5Bcc_type%5D=";
                this._data = this._data + "&payment%5Bencrypted_data%5D=";
                this._data = this._data + "&newsletter%5B2%5D=1";
                this._data = this._data + "&agreement%5B4%5D=1";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
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
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
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
            public static readonly Sivasdescalzo.<>c <>9;
            public static Func<HtmlNode, bool> <>9__19_0;
            public static Func<HtmlNode, bool> <>9__19_1;
            public static Func<HtmlNode, bool> <>9__19_2;
            public static Func<HtmlNode, bool> <>9__19_3;
            public static Func<HtmlNode, bool> <>9__23_1;
            public static Func<HtmlNode, bool> <>9__23_2;
            public static Func<HtmlNode, bool> <>9__24_0;
            public static Func<HtmlNode, bool> <>9__24_1;
            public static Func<HtmlNode, bool> <>9__24_2;
            public static Func<HtmlNode, bool> <>9__24_3;
            public static Func<HtmlNode, bool> <>9__24_4;
            public static Func<HtmlNode, bool> <>9__24_5;
            public static Func<HtmlNode, bool> <>9__24_6;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Sivasdescalzo.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__19_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "form_key"));

            internal bool <Atc>b__19_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_addtocart_form"));

            internal bool <Atc>b__19_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "product"));

            internal bool <Atc>b__19_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "recaptcha_html_element"));

            internal bool <DirectLink>b__24_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value.ToLowerInvariant() == "content size-options size_us-options"));

            internal bool <DirectLink>b__24_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value.ToLowerInvariant() == "content size-options size_us-options"));

            internal bool <DirectLink>b__24_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "brand"));

            internal bool <DirectLink>b__24_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "model-1"));

            internal bool <DirectLink>b__24_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-price"));

            internal bool <DirectLink>b__24_5(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-carousel"));

            internal bool <DirectLink>b__24_6(HtmlNode x) => 
                ((x.Attributes["class"] != null) && !x.Attributes["class"].Value.Contains("unavailable"));

            internal bool <SubmitShipping>b__23_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "radio_shipping"));

            internal bool <SubmitShipping>b__23_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipping_method"));
        }

        [CompilerGenerated]
        private static class <>o__19
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__21
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__2;
            public static CallSite<Func<CallSite, string, object, object>> <>p__3;
            public static CallSite<Func<CallSite, string, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__8;
            public static CallSite<Func<CallSite, object, bool>> <>p__9;
        }

        [CompilerGenerated]
        private static class <>o__25
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, Type, string, object, Cookie>> <>p__2;
            public static CallSite<Func<CallSite, string, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string>> <>p__4;
        }
    }
}

