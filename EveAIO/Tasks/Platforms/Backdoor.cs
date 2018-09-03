namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    internal class Backdoor : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        [Dynamic]
        private object _dynObj;
        private string _data;
        private string _productId;
        private string _addToCartId;
        private string _security;
        private string _billingUrl;
        private string _wpNonce;

        public Backdoor(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._productId = "";
            this._addToCartId = "";
            this._security = "";
            this._billingUrl = "";
            this._wpNonce = "";
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                this._request = (HttpWebRequest) WebRequest.Create(this._runner.Product.Link);
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
                this._request.Headers.Add("Origin", "https://www.back-door.it");
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Method = "POST";
                this._data = "attribute_pa_size=" + this._runner.PickedSize.Value.Key.Replace(".", "-");
                this._data = this._data + "&quantity=1";
                this._data = this._data + "&add-to-cart=" + this._addToCartId;
                this._data = this._data + "&product_id=" + this._productId;
                this._data = this._data + "&variation_id=" + this._runner.PickedSize.Value.Value;
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                string str = "";
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                    if (response.Headers["X-Cache"] != null)
                    {
                        str = response.Headers["X-Cache"];
                    }
                }
                if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                    return false;
                }
                this._currentDoc.LoadHtml(this._srr);
                if (!this._srr.ToUpperInvariant().Contains("HAS BEEN ADDED TO YOUR CART"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                return true;
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
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
            switch (((-813772179 ^ -1606186923) % 5))
            {
                case 0:
                    break;

                case 1:
                    return false;

                case 2:
                    goto Label_001C;

                case 3:
                    return false;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (this.SubmitBilling())
            {
            }
            if (-2109343868 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link) => 
            this.DirectLink(link, false);

        private bool DirectLink(string link, bool isSearch = false)
        {
            try
            {
                if (!isSearch)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
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
                string str = "";
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                    if (response.Headers["X-Cache"] != null)
                    {
                        str = response.Headers["X-Cache"];
                    }
                }
                if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                    return false;
                }
                if (this._srr.Contains("document.cookie = \"edge-bd="))
                {
                    string str2 = this._srr.Substring(this._srr.IndexOf("edge-bd"));
                    str2 = str2.Substring(str2.IndexOf("=") + 1);
                    str2 = str2.Substring(0, str2.IndexOf(";"));
                    Cookie cookie = new Cookie {
                        Value = str2,
                        Name = "edge-bd",
                        Domain = "back-door.it"
                    };
                    this._runner.Cookies.Add(cookie);
                    this._request = (HttpWebRequest) WebRequest.Create(link);
                    this._request.KeepAlive = true;
                    this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                    this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    this._request.CookieContainer = this._runner.Cookies;
                    this._request.Referer = link;
                    if (this._runner.Proxy != null)
                    {
                        this._request.Proxy = this._runner.Proxy;
                    }
                    str = "";
                    using (WebResponse response2 = this._request.GetResponse())
                    {
                        using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                        {
                            this._srr = reader2.ReadToEnd();
                        }
                        if (response2.Headers["X-Cache"] != null)
                        {
                            str = response2.Headers["X-Cache"];
                        }
                    }
                    if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                        return false;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str5 = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim());
                string str4 = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("p").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"))).InnerText.Trim());
                if (str4.Contains(" "))
                {
                    str4 = str4.Substring(str4.IndexOf(" ") + 1);
                }
                string str3 = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"))).Attributes["content"].Value;
                this._addToCartId = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "add-to-cart"))).Attributes["value"].Value;
                this._productId = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "product_id"))).Attributes["value"].Value;
                this._task.ImgUrl = str3;
                Product product1 = new Product {
                    ProductTitle = str5,
                    Link = link,
                    Price = str4
                };
                this._runner.Product = product1;
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ivpa-content"))).Attributes["data-variations"].Value));
                if (<>o__21.<>p__14 == null)
                {
                    <>o__21.<>p__14 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Backdoor)));
                }
                foreach (object obj3 in <>o__21.<>p__14.Target(<>o__21.<>p__14, obj2))
                {
                    if (<>o__21.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Backdoor), argumentInfo));
                    }
                    if (<>o__21.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Backdoor), argumentInfo));
                    }
                    if (<>o__21.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Backdoor), argumentInfo));
                    }
                    if (<>o__21.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                    }
                    if (!<>o__21.<>p__3.Target(<>o__21.<>p__3, <>o__21.<>p__2.Target(<>o__21.<>p__2, <>o__21.<>p__1.Target(<>o__21.<>p__1, <>o__21.<>p__0.Target(<>o__21.<>p__0, obj3, "is_in_stock")), false)))
                    {
                        if (<>o__21.<>p__9 == null)
                        {
                            <>o__21.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Backdoor)));
                        }
                        if (<>o__21.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__8 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(Backdoor), argumentInfo));
                        }
                        if (<>o__21.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Backdoor), argumentInfo));
                        }
                        if (<>o__21.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Backdoor), argumentInfo));
                        }
                        if (<>o__21.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                        }
                        if (<>o__21.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "attributes", typeof(Backdoor), argumentInfo));
                        }
                        string str6 = <>o__21.<>p__9.Target(<>o__21.<>p__9, <>o__21.<>p__8.Target(<>o__21.<>p__8, <>o__21.<>p__7.Target(<>o__21.<>p__7, <>o__21.<>p__6.Target(<>o__21.<>p__6, <>o__21.<>p__5.Target(<>o__21.<>p__5, <>o__21.<>p__4.Target(<>o__21.<>p__4, obj3), "attribute_pa_size"))), "-", "."));
                        if (<>o__21.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__13 = CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                        }
                        if (<>o__21.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Backdoor), argumentInfo));
                        }
                        if (<>o__21.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Backdoor), argumentInfo));
                        }
                        if (<>o__21.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                        }
                        this._runner.Product.AvailableSizes.Add(<>o__21.<>p__13.Target(<>o__21.<>p__13, typeof(KeyValuePair<string, string>), str6, <>o__21.<>p__12.Target(<>o__21.<>p__12, <>o__21.<>p__11.Target(<>o__21.<>p__11, <>o__21.<>p__10.Target(<>o__21.<>p__10, obj3, "variation_id")))));
                    }
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
                    string str8 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str8 = str8 + ch.ToString();
                        }
                    }
                    double num4 = double.Parse(str8.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                            KeyValuePair<string, string> pair;
                            goto Label_0E50;
                        Label_0D80:
                            pair = enumerator2.Current;
                            List<string> source = new List<string>();
                            if (!pair.Key.Contains(":"))
                            {
                                source.Add(pair.Key);
                            }
                            else
                            {
                                char[] chArray2 = new char[] { ':' };
                                string[] strArray3 = pair.Key.Split(chArray2);
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
                                goto Label_0E5E;
                            }
                        Label_0E50:
                            if (!enumerator2.MoveNext())
                            {
                                continue;
                            }
                            goto Label_0D80;
                        Label_0E5E:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(pair);
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
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
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
            throw new NotImplementedException();
        }

        public bool Search()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                using (IEnumerator<string> enumerator = this._task.Keywords.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string current = enumerator.Current;
                        string requestUriString = $"https://www.back-door.it/?s={current.Replace(" ", "+").ToLowerInvariant()}&post_type=product";
                        bool flag = true;
                        while (flag)
                        {
                            flag = false;
                            this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
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
                            string str = "";
                            string link = "";
                            using (WebResponse response = this._request.GetResponse())
                            {
                                link = response.ResponseUri.ToString();
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    this._srr = reader.ReadToEnd();
                                }
                                if (response.Headers["X-Cache"] != null)
                                {
                                    str = response.Headers["X-Cache"];
                                }
                            }
                            if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str))
                            {
                                goto Label_054D;
                            }
                            if (this._srr.Contains("document.cookie = \"edge-bd="))
                            {
                                string str5 = this._srr.Substring(this._srr.IndexOf("edge-bd"));
                                str5 = str5.Substring(str5.IndexOf("=") + 1);
                                str5 = str5.Substring(0, str5.IndexOf(";"));
                                Cookie cookie = new Cookie {
                                    Value = str5,
                                    Name = "edge-bd",
                                    Domain = "back-door.it"
                                };
                                this._runner.Cookies.Add(cookie);
                                this._request = (HttpWebRequest) WebRequest.Create(requestUriString);
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
                                str = "";
                                link = "";
                                using (WebResponse response2 = this._request.GetResponse())
                                {
                                    link = response2.ResponseUri.ToString();
                                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                                    {
                                        this._srr = reader2.ReadToEnd();
                                    }
                                    if (response2.Headers["X-Cache"] != null)
                                    {
                                        str = response2.Headers["X-Cache"];
                                    }
                                }
                            }
                            if (link.Contains("/product/"))
                            {
                                return this.DirectLink(link);
                            }
                            HtmlDocument document = new HtmlDocument();
                            document.LoadHtml(this._srr);
                            if (document.DocumentNode.Descendants("ul").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products clearfix")))
                            {
                                using (IEnumerator<HtmlNode> enumerator2 = document.DocumentNode.Descendants("ul").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products clearfix"))).Descendants("li").GetEnumerator())
                                {
                                    while (enumerator2.MoveNext())
                                    {
                                        string str6 = enumerator2.Current.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                                        if (this.DirectLink(str6, true))
                                        {
                                            return true;
                                        }
                                    }
                                }
                                if (document.DocumentNode.Descendants("a").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "next page-numbers")))
                                {
                                    requestUriString = document.DocumentNode.Descendants("a").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "next page-numbers"))).Attributes["href"].Value;
                                    flag = true;
                                }
                            }
                        }
                    }
                    goto Label_0598;
                Label_054D:
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                    return false;
                }
            Label_0598:
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
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create(this._billingUrl);
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
                this._request.Referer = "https://www.back-door.it/checkout/";
                using (WebResponse response = this._request.GetResponse())
                {
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
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
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
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                ProfileObject profile = this._runner.Profile;
                string str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_csrf"))).Attributes["value"].Value;
                string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf"))).Attributes["value"].Value;
                string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "formKey"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "internalSessionId"))).Attributes["value"].Value;
                string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "COOKIES_DISABLED_ID"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card_country_code"))).Attributes["value"].Value;
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sparta_hosted_button"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_eventId_pay"))).Attributes["value"].Value;
                this._request = (HttpWebRequest) WebRequest.Create("https://www.paypal.com/hostedpaymentnodeweb/payWithCC");
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
                this._request.Referer = "https://securepayments.paypal.com/cgi-bin/acquiringweb?";
                this._request.Headers.Add("Origin", "https://securepayments.paypal.com");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                string str5 = "";
                switch (this._runner.Profile.CardTypeId)
                {
                    case "0":
                        throw new Exception("Unsupported CC brand");

                    case "1":
                        str5 = "V";
                        break;

                    case "2":
                        str5 = "M";
                        break;

                    case "3":
                        throw new Exception("Unsupported CC brand");
                }
                this._data = "_csrf=" + WebUtility.HtmlDecode(str10);
                this._data = this._data + "&csrf=" + WebUtility.HtmlDecode(str8);
                this._data = this._data + "&formKey=" + WebUtility.HtmlDecode(str9);
                this._data = this._data + "&internalSessionId=" + WebUtility.HtmlDecode(str2);
                this._data = this._data + "&COOKIES_DISABLED_ID=" + WebUtility.UrlEncode(str7);
                this._data = this._data + "&card_country_code=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&creditCardType=" + str5;
                this._data = this._data + "&creditCardNumber=" + WebUtility.UrlEncode(this._runner.Profile.CCNumber);
                this._data = this._data + "&expiryMonth=" + this._runner.Profile.ExpiryMonth;
                this._data = this._data + "&expiryYear=" + this._runner.Profile.ExpiryYear.Substring(2);
                this._data = this._data + "&cvv2=" + this._runner.Profile.Cvv;
                this._data = this._data + "&startDateMonth=";
                this._data = this._data + "&startDateYear=";
                this._data = this._data + "&issueNumber=";
                this._data = this._data + "&birthDateDay=";
                this._data = this._data + "&birthDateMonth=";
                this._data = this._data + "&birthDateYear=";
                this._data = this._data + "&sparta_hosted_button=" + WebUtility.UrlEncode(str);
                this._data = this._data + "&_eventId_pay=" + WebUtility.UrlEncode(str3);
                this._data = this._data + "&showCustomerName=";
                this._data = this._data + "&showBillingAddress=";
                this._data = this._data + "&showBillingPhone=";
                this._data = this._data + "&showBillingEmail=";
                this._data = this._data + "&showShippingAddress=true";
                this._data = this._data + "&billingShippingRelationship=BILLING_SHIPPING_NOT_TOGETHER";
                this._data = this._data + "&shippingFirstName=" + WebUtility.UrlEncode(profile.FirstNameShipping);
                this._data = this._data + "&shippingLastName=" + WebUtility.UrlEncode(profile.LastNameShipping);
                this._data = this._data + "&shippingAddress1=" + WebUtility.UrlEncode(profile.Address1Shipping);
                this._data = this._data + "&shippingAddress2=" + WebUtility.UrlEncode(profile.Address2Shipping);
                this._data = this._data + "&shippingCity=" + WebUtility.UrlEncode(profile.CityShipping);
                this._data = this._data + "&shippingState=" + WebUtility.UrlEncode(profile.StateIdShipping);
                this._data = this._data + "&shippingZip=" + WebUtility.UrlEncode(profile.ZipShipping);
                this._data = this._data + "&shippingCountry=" + WebUtility.UrlEncode(profile.CountryIdShipping);
                this._data = this._data + "&shippingPhone=";
                this._data = this._data + "&shippingEmail=";
                this._data = this._data + "&shippingStateList=" + WebUtility.UrlEncode("[\"undefined\"]");
                this._data = this._data + "&first_name=" + WebUtility.UrlEncode(profile.FirstNameShipping);
                this._data = this._data + "&last_name=" + WebUtility.UrlEncode(profile.LastNameShipping);
                this._data = this._data + "&country=" + WebUtility.UrlEncode(profile.CountryIdShipping);
                this._data = this._data + "&sparta_hosted_button=selectcountry";
                this._data = this._data + "&address1=" + WebUtility.UrlEncode(profile.Address1Shipping);
                this._data = this._data + "&address2=" + WebUtility.UrlEncode(profile.Address2Shipping);
                this._data = this._data + "&city=" + WebUtility.UrlEncode(profile.CityShipping);
                this._data = this._data + "&state=" + (string.IsNullOrEmpty(profile.StateIdShipping) ? WebUtility.UrlEncode("n/a") : WebUtility.UrlEncode(profile.StateShipping));
                this._data = this._data + "&zip=" + WebUtility.UrlEncode(profile.ZipShipping);
                this._data = this._data + "&shippingPhoneNumber=";
                this._data = this._data + "&shippingEmailAddress=";
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
                    EveAIO.Helpers.AddDbValue("Backdoor|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "messageBox")))
                {
                    HtmlNode node = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "messageBox"));
                    if (node.Descendants("p").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "show"))) && !string.IsNullOrEmpty(node.Descendants("p").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "show"))).InnerHtml.Trim()))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        return false;
                    }
                }
                if (!this._srr.ToLowerInvariant().Contains("error occured"))
                {
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("400") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("400")))
                {
                    this._runner.IsError = true;
                    if (exception is AggregateException)
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                    }
                    else if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
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
                return false;
            }
        }

        private bool SubmitShipping()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                string str = obj2.Email.Trim();
                string str2 = obj2.FirstName.Trim();
                string str3 = obj2.LastName.Trim();
                string str4 = obj2.Address1.Trim();
                string str5 = obj2.Address2.Trim();
                string str6 = obj2.City.Trim();
                string str7 = obj2.Zip.Trim();
                string str8 = obj2.CountryId.ToUpperInvariant();
                string state = "";
                if (((str8 != "US") && (str8 != "CA")) && (str8 != "IT"))
                {
                    state = obj2.State;
                }
                else
                {
                    state = obj2.StateId;
                }
                string str10 = obj2.Phone.Trim();
                string country = obj2.Country;
                obj2.EmailShipping.Trim();
                string str11 = obj2.FirstNameShipping.Trim();
                string str12 = obj2.LastNameShipping.Trim();
                string str13 = obj2.Address1Shipping.Trim();
                string str14 = obj2.Address2Shipping.Trim();
                string str15 = obj2.CityShipping.Trim();
                string str16 = obj2.ZipShipping.Trim();
                string str17 = obj2.CountryIdShipping.ToUpperInvariant();
                string stateShipping = "";
                if (((str17 != "US") && (str17 != "CA")) && (str17 != "IT"))
                {
                    stateShipping = obj2.StateShipping;
                }
                else
                {
                    stateShipping = obj2.StateIdShipping;
                }
                obj2.PhoneShipping.Trim();
                string countryShipping = obj2.CountryShipping;
                this._request = (HttpWebRequest) WebRequest.Create("https://www.back-door.it/checkout/");
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
                this._request.Referer = "https://www.back-door.it/cart/";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                string str19 = "";
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                    if (response.Headers["X-Cache"] != null)
                    {
                        str19 = response.Headers["X-Cache"];
                    }
                }
                if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str19))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                    return false;
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._srr.Contains("Sorry, we do not have enough"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    return false;
                }
                this._security = this._srr.Substring(this._srr.IndexOf("update_order_review_nonce"));
                this._security = this._security.Substring(this._security.IndexOf(":\"") + 2);
                this._security = this._security.Substring(0, this._security.IndexOf("\""));
                this._wpNonce = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "_wpnonce"))).Attributes["value"].Value;
                this._request = (HttpWebRequest) WebRequest.Create("https://www.back-door.it/checkout/?wc-ajax=update_order_review");
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
                this._request.Referer = "https://www.back-door.it/checkout/";
                this._request.Headers.Add("Origin", "https://www.back-door.it");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._data = "security=" + this._security;
                this._data = this._data + "&payment_method=wc-gateway-paypal-pro-italia";
                this._data = this._data + "&country=" + str8;
                this._data = this._data + "&state=" + WebUtility.UrlEncode(state);
                this._data = this._data + "&postcode=" + WebUtility.UrlEncode(str7);
                this._data = this._data + "&city=" + WebUtility.UrlEncode(str6);
                this._data = this._data + "&address=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&address_2=" + WebUtility.UrlEncode(str5);
                this._data = this._data + "&s_country=" + str17;
                this._data = this._data + "&s_state=";
                this._data = this._data + "&s_postcode=";
                this._data = this._data + "&s_city=";
                this._data = this._data + "&s_address=";
                this._data = this._data + "&s_address_2=";
                this._data = this._data + "&post_data=billing_first_name%3D" + WebUtility.UrlEncode(str2);
                this._data = this._data + "%26billing_last_name%3D" + WebUtility.UrlEncode(str3);
                this._data = this._data + "%26billing_company%3D%26billing_email%3D" + WebUtility.UrlEncode(str);
                this._data = this._data + "%26billing_phone%3D" + WebUtility.UrlEncode(str10);
                this._data = this._data + "%26billing_country%3D" + WebUtility.UrlEncode(str8);
                this._data = this._data + "%26billing_address_1%3D" + WebUtility.UrlEncode(str4);
                this._data = this._data + "%26billing_address_2%3D" + WebUtility.UrlEncode(str5);
                this._data = this._data + "%26billing_postcode%3D" + WebUtility.UrlEncode(str7);
                this._data = this._data + "%26billing_city%3D" + WebUtility.UrlEncode(str6);
                this._data = this._data + "%26billing_state%3D" + WebUtility.UrlEncode(state);
                this._data = this._data + "%26account_username%3D";
                this._data = this._data + "%26account_password%3D";
                this._data = this._data + "%26ship_to_different_address%3D1";
                this._data = this._data + "%26shipping_first_name%3D" + WebUtility.UrlEncode(str11);
                this._data = this._data + "%26shipping_last_name%3D" + WebUtility.UrlEncode(str12);
                this._data = this._data + "%26shipping_company%3D%26shipping_country%3D" + str17;
                this._data = this._data + "%26shipping_address_1%3D";
                this._data = this._data + "%26shipping_address_2%3D";
                this._data = this._data + "%26shipping_city%3D";
                this._data = this._data + "%26shipping_postcode%3D";
                this._data = this._data + "%26order_comments%3D";
                this._data = this._data + "%26shipping_method%255B0%255D%3Dflat_rate%253A1";
                this._data = this._data + "%26payment_method%3Dwc-gateway-paypal-pro-italia";
                this._data = this._data + "%26terms-field%3D1%26_wpnonce%3D" + this._wpNonce;
                this._data = this._data + "%26_wp_http_referer%3D%252Fcheckout%252F%253Fwc-ajax%253Dupdate_order_review";
                this._data = this._data + "&shipping_method%5B0%5D=flat_rate%3A1";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                str19 = "";
                using (WebResponse response2 = this._request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        this._srr = reader2.ReadToEnd();
                    }
                    if (response2.Headers["X-Cache"] != null)
                    {
                        str19 = response2.Headers["X-Cache"];
                    }
                }
                if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str19))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                    return false;
                }
                string str20 = "";
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__19.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Backdoor), argumentInfo));
                }
                if (<>o__19.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                }
                if (<>o__19.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "fragments", typeof(Backdoor), argumentInfo));
                }
                object obj3 = <>o__19.<>p__2.Target(<>o__19.<>p__2, <>o__19.<>p__1.Target(<>o__19.<>p__1, <>o__19.<>p__0.Target(<>o__19.<>p__0, this._dynObj), ".woocommerce-checkout-review-order-table"));
                if (<>o__19.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__3 = CallSite<Action<CallSite, HtmlDocument, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "LoadHtml", null, typeof(Backdoor), argumentInfo));
                }
                <>o__19.<>p__3.Target(<>o__19.<>p__3, this._currentDoc, obj3);
                if (this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => (((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("shipping_method")) && (x.Attributes["value"] != null)) && x.Attributes["value"].Value.Contains("free")))
                {
                    str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("shipping_method")) && (x.Attributes["value"] != null)) && x.Attributes["value"].Value.Contains("free"))).Attributes["value"].Value;
                }
                else
                {
                    str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("shipping_method"))).Attributes["value"].Value;
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.back-door.it/checkout/?wc-ajax=update_order_review");
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
                this._request.Referer = "https://www.back-door.it/checkout/";
                this._request.Headers.Add("Origin", "https://www.back-door.it");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._data = "security=" + this._security;
                this._data = this._data + "&payment_method=wc-gateway-paypal-pro-italia";
                this._data = this._data + "&country=" + str8;
                this._data = this._data + "&state=" + WebUtility.UrlEncode(state);
                this._data = this._data + "&postcode=" + WebUtility.UrlEncode(str7);
                this._data = this._data + "&city=" + WebUtility.UrlEncode(str6);
                this._data = this._data + "&address=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&address_2=" + WebUtility.UrlEncode(str5);
                this._data = this._data + "&s_country=" + str17;
                this._data = this._data + "&s_state=" + WebUtility.UrlEncode(stateShipping);
                this._data = this._data + "&s_postcode=" + WebUtility.UrlEncode(str16);
                this._data = this._data + "&s_city=" + WebUtility.UrlEncode(str15);
                this._data = this._data + "&s_address=" + WebUtility.UrlEncode(str13);
                this._data = this._data + "&s_address_2=" + WebUtility.UrlEncode(str14);
                this._data = this._data + "&post_data=billing_first_name%3D" + WebUtility.UrlEncode(str2);
                this._data = this._data + "%26billing_last_name%3D" + WebUtility.UrlEncode(str3);
                this._data = this._data + "%26billing_company%3D%26billing_email%3D" + WebUtility.UrlEncode(str);
                this._data = this._data + "%26billing_phone%3D" + WebUtility.UrlEncode(str10);
                this._data = this._data + "%26billing_country%3D" + WebUtility.UrlEncode(str8);
                this._data = this._data + "%26billing_address_1%3D" + WebUtility.UrlEncode(str4);
                this._data = this._data + "%26billing_address_2%3D" + WebUtility.UrlEncode(str5);
                this._data = this._data + "%26billing_postcode%3D" + WebUtility.UrlEncode(str7);
                this._data = this._data + "%26billing_city%3D" + WebUtility.UrlEncode(str6);
                this._data = this._data + "%26billing_state%3D" + WebUtility.UrlEncode(state);
                this._data = this._data + "%26account_username%3D";
                this._data = this._data + "%26account_password%3D";
                this._data = this._data + "%26ship_to_different_address%3D1";
                this._data = this._data + "%26shipping_first_name%3D" + WebUtility.UrlEncode(str11);
                this._data = this._data + "%26shipping_last_name%3D" + WebUtility.UrlEncode(str12);
                this._data = this._data + "%26shipping_company%3D";
                this._data = this._data + "%26shipping_country%3D" + WebUtility.UrlEncode(str17);
                this._data = this._data + "%26shipping_address_1%3D" + WebUtility.UrlEncode(str13);
                this._data = this._data + "%26shipping_address_2%3D" + WebUtility.UrlEncode(str14);
                this._data = this._data + "%26shipping_city%3D" + WebUtility.UrlEncode(str15);
                this._data = this._data + "%26shipping_state%3D" + WebUtility.UrlEncode(stateShipping);
                this._data = this._data + "%26shipping_postcode%3D" + WebUtility.UrlEncode(str16);
                this._data = this._data + "%26order_comments%3D%26shipping_method%255B0%255D%3D" + WebUtility.UrlEncode(str20);
                this._data = this._data + "%26payment_method%3Dwc-gateway-paypal-pro-italia";
                this._data = this._data + "%26terms-field%3D1%26_wpnonce%3D" + this._wpNonce;
                this._data = this._data + "%26_wp_http_referer%3D%252Fcheckout%252F%253Fwc-ajax%253Dupdate_order_review";
                this._data = this._data + "&shipping_method%5B0%5D=" + WebUtility.UrlEncode(str20);
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream2 = this._request.GetRequestStream())
                {
                    stream2.Write(this._bytes, 0, this._bytes.Length);
                }
                str19 = "";
                using (WebResponse response3 = this._request.GetResponse())
                {
                    using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                    {
                        this._srr = reader3.ReadToEnd();
                    }
                    if (response3.Headers["X-Cache"] != null)
                    {
                        str19 = response3.Headers["X-Cache"];
                    }
                }
                if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str19))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                    return false;
                }
                this._wpNonce = this._srr.Substring(this._srr.IndexOf("_wpnonce"));
                this._wpNonce = this._wpNonce.Substring(this._wpNonce.IndexOf("value"));
                this._wpNonce = this._wpNonce.Substring(this._wpNonce.IndexOf("\"") + 1);
                this._wpNonce = this._wpNonce.Substring(0, this._wpNonce.IndexOf(@"\"));
                this._request = (HttpWebRequest) WebRequest.Create("https://www.back-door.it/checkout/?wc-ajax=checkout");
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
                this._request.Referer = "https://www.back-door.it/checkout/";
                this._request.Headers.Add("Origin", "https://www.back-door.it");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._data = "billing_first_name=" + WebUtility.UrlEncode(str2);
                this._data = this._data + "&billing_last_name=" + WebUtility.UrlEncode(str3);
                this._data = this._data + "&billing_company=";
                this._data = this._data + "&billing_email=" + WebUtility.UrlEncode(str);
                this._data = this._data + "&billing_phone=" + WebUtility.UrlEncode(str10);
                this._data = this._data + "&billing_country=" + WebUtility.UrlEncode(str8);
                this._data = this._data + "&billing_address_1=" + WebUtility.UrlEncode(str4);
                this._data = this._data + "&billing_address_2=" + WebUtility.UrlEncode(str5);
                this._data = this._data + "&billing_postcode=" + WebUtility.UrlEncode(str7);
                this._data = this._data + "&billing_city=" + WebUtility.UrlEncode(str6);
                this._data = this._data + "&billing_state=" + WebUtility.UrlEncode(state);
                this._data = this._data + "&account_username=";
                this._data = this._data + "&account_password=";
                this._data = this._data + "&ship_to_different_address=1";
                this._data = this._data + "&shipping_first_name=" + WebUtility.UrlEncode(str11);
                this._data = this._data + "&shipping_last_name=" + WebUtility.UrlEncode(str12);
                this._data = this._data + "&shipping_company=";
                this._data = this._data + "&shipping_country=" + WebUtility.UrlEncode(str17);
                this._data = this._data + "&shipping_address_1=" + WebUtility.UrlEncode(str13);
                this._data = this._data + "&shipping_address_2=" + WebUtility.UrlEncode(str14);
                this._data = this._data + "&shipping_city=" + WebUtility.UrlEncode(str15);
                this._data = this._data + "&shipping_state=" + WebUtility.UrlEncode(stateShipping);
                this._data = this._data + "&shipping_postcode=" + WebUtility.UrlEncode(str16);
                this._data = this._data + "&order_comments=";
                this._data = this._data + "&shipping_method%5B0%5D=" + WebUtility.UrlEncode(str20);
                this._data = this._data + "&payment_method=wc-gateway-paypal-pro-italia";
                this._data = this._data + "&terms=on";
                this._data = this._data + "&terms-field=1";
                this._data = this._data + "&_wpnonce=" + this._wpNonce;
                this._data = this._data + "&_wp_http_referer=%2Fcheckout%2F%3Fwc-ajax%3Dupdate_order_review";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream3 = this._request.GetRequestStream())
                {
                    stream3.Write(this._bytes, 0, this._bytes.Length);
                }
                str19 = "";
                using (WebResponse response4 = this._request.GetResponse())
                {
                    using (StreamReader reader4 = new StreamReader(response4.GetResponseStream()))
                    {
                        this._srr = reader4.ReadToEnd();
                    }
                    if (response4.Headers["X-Cache"] != null)
                    {
                        str19 = response4.Headers["X-Cache"];
                    }
                }
                if (string.IsNullOrEmpty(this._srr) && !string.IsNullOrEmpty(str19))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLOUDFRONT_ERROR, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CLOUDFRONT_ERROR);
                    return false;
                }
                this._srr = this._srr.Substring(this._srr.IndexOf("{"));
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__19.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Backdoor), argumentInfo));
                }
                if (<>o__19.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Backdoor), argumentInfo));
                }
                if (<>o__19.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Backdoor), argumentInfo));
                }
                if (<>o__19.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                }
                if (<>o__19.<>p__7.Target(<>o__19.<>p__7, <>o__19.<>p__6.Target(<>o__19.<>p__6, <>o__19.<>p__5.Target(<>o__19.<>p__5, <>o__19.<>p__4.Target(<>o__19.<>p__4, this._dynObj, "result")), "success")))
                {
                    if (<>o__19.<>p__10 == null)
                    {
                        <>o__19.<>p__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Backdoor)));
                    }
                    if (<>o__19.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Backdoor), argumentInfo));
                    }
                    if (<>o__19.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__19.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                    }
                }
                else
                {
                    if (<>o__19.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__12 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Backdoor), argumentInfo));
                    }
                    if (<>o__19.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Backdoor), argumentInfo));
                    }
                    throw <>o__19.<>p__12.Target(<>o__19.<>p__12, typeof(Exception), <>o__19.<>p__11.Target(<>o__19.<>p__11, this._dynObj));
                }
                this._billingUrl = <>o__19.<>p__10.Target(<>o__19.<>p__10, <>o__19.<>p__9.Target(<>o__19.<>p__9, <>o__19.<>p__8.Target(<>o__19.<>p__8, this._dynObj, "redirect")));
                return true;
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
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
            public static readonly Backdoor.<>c <>9;
            public static Func<HtmlNode, bool> <>9__17_0;
            public static Func<HtmlNode, bool> <>9__17_1;
            public static Func<HtmlNode, bool> <>9__17_2;
            public static Func<HtmlNode, bool> <>9__17_3;
            public static Func<HtmlNode, bool> <>9__17_4;
            public static Func<HtmlNode, bool> <>9__17_5;
            public static Func<HtmlNode, bool> <>9__17_6;
            public static Func<HtmlNode, bool> <>9__17_7;
            public static Func<HtmlNode, bool> <>9__17_9;
            public static Func<HtmlNode, bool> <>9__17_10;
            public static Func<HtmlNode, bool> <>9__17_11;
            public static Func<HtmlNode, bool> <>9__17_12;
            public static Func<HtmlNode, bool> <>9__19_1;
            public static Func<HtmlNode, bool> <>9__19_2;
            public static Func<HtmlNode, bool> <>9__19_3;
            public static Func<HtmlNode, bool> <>9__19_4;
            public static Func<HtmlNode, bool> <>9__21_0;
            public static Func<HtmlNode, bool> <>9__21_1;
            public static Func<HtmlNode, bool> <>9__21_2;
            public static Func<HtmlNode, bool> <>9__21_3;
            public static Func<HtmlNode, bool> <>9__21_4;
            public static Func<HtmlNode, bool> <>9__21_5;
            public static Func<HtmlNode, bool> <>9__23_0;
            public static Func<HtmlNode, bool> <>9__23_1;
            public static Func<HtmlNode, bool> <>9__23_2;
            public static Func<HtmlNode, bool> <>9__23_3;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Backdoor.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__21_0(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLink>b__21_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"));

            internal bool <DirectLink>b__21_2(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"));

            internal bool <DirectLink>b__21_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "add-to-cart"));

            internal bool <DirectLink>b__21_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "product_id"));

            internal bool <DirectLink>b__21_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ivpa-content"));

            internal bool <Search>b__23_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products clearfix"));

            internal bool <Search>b__23_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products clearfix"));

            internal bool <Search>b__23_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "next page-numbers"));

            internal bool <Search>b__23_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "next page-numbers"));

            internal bool <SubmitOrder>b__17_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_csrf"));

            internal bool <SubmitOrder>b__17_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf"));

            internal bool <SubmitOrder>b__17_10(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "messageBox"));

            internal bool <SubmitOrder>b__17_11(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "show"));

            internal bool <SubmitOrder>b__17_12(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "show"));

            internal bool <SubmitOrder>b__17_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "formKey"));

            internal bool <SubmitOrder>b__17_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "internalSessionId"));

            internal bool <SubmitOrder>b__17_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "COOKIES_DISABLED_ID"));

            internal bool <SubmitOrder>b__17_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "card_country_code"));

            internal bool <SubmitOrder>b__17_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sparta_hosted_button"));

            internal bool <SubmitOrder>b__17_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_eventId_pay"));

            internal bool <SubmitOrder>b__17_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "messageBox"));

            internal bool <SubmitShipping>b__19_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "_wpnonce"));

            internal bool <SubmitShipping>b__19_2(HtmlNode x) => 
                ((((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("shipping_method")) && (x.Attributes["value"] != null)) && x.Attributes["value"].Value.Contains("free"));

            internal bool <SubmitShipping>b__19_3(HtmlNode x) => 
                ((((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("shipping_method")) && (x.Attributes["value"] != null)) && x.Attributes["value"].Value.Contains("free"));

            internal bool <SubmitShipping>b__19_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("shipping_method"));
        }

        [CompilerGenerated]
        private static class <>o__19
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Action<CallSite, HtmlDocument, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__12;
        }

        [CompilerGenerated]
        private static class <>o__21
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>> <>p__13;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__14;
        }
    }
}

