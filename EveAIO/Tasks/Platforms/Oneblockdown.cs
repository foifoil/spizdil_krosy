namespace EveAIO.Tasks.Platforms
{
    using EveAIO.Cloudflare;
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
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Oneblockdown : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        private string _data;
        [Dynamic]
        private object _dynObj;

        public Oneblockdown(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                this._request = (HttpWebRequest) WebRequest.Create("http://www.oneblockdown.it/index.php");
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
                this._request.Headers.Add("Origin", "http://www.oneblockdown.it");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._data = "controller=orders";
                this._data = this._data + "&action=addStockItemToBasket";
                this._data = this._data + "&stockItemId=" + this._runner.PickedSize.Value.Value;
                this._data = this._data + "&quantity=1";
                this._data = this._data + "&site=site";
                this._data = this._data + "&skin=obd";
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
                if (<>o__10.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Oneblockdown), argumentInfo));
                }
                if (<>o__10.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Oneblockdown), argumentInfo));
                }
                if (<>o__10.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Oneblockdown), argumentInfo));
                }
                if (<>o__10.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Oneblockdown), argumentInfo));
                }
                if (<>o__10.<>p__3.Target(<>o__10.<>p__3, <>o__10.<>p__2.Target(<>o__10.<>p__2, <>o__10.<>p__1.Target(<>o__10.<>p__1, <>o__10.<>p__0.Target(<>o__10.<>p__0, this._dynObj, "success")), true)))
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

        public bool Checkout() => 
            false;

        public bool DirectLink(string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.SOLVING_CLOUDFLARE, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SOLVING_CLOUDFLARE);
                try
                {
                    this._request = (HttpWebRequest) WebRequest.Create(link);
                    if (this._runner.Proxy != null)
                    {
                        this._request.Proxy = this._runner.Proxy;
                    }
                    this._request.CookieContainer = this._runner.Cookies;
                    this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
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
                    if (!exception.Message.Contains("503"))
                    {
                        throw;
                    }
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
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                this._currentDoc.LoadHtml(this._srr);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                HtmlNode node = this._currentDoc.DocumentNode.Descendants("hgroup").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-title"));
                string str2 = node.Descendants("h3").First<HtmlNode>().InnerText.Trim() + " " + node.Descendants("h1").First<HtmlNode>().InnerText.Trim();
                string str3 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "item-sell-price"))).InnerText.Trim();
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-carousel "))).Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                Product product1 = new Product {
                    ProductTitle = str2,
                    Link = link,
                    Price = str3
                };
                this._runner.Product = product1;
                if (!this._srr.Contains("var preloadedStock"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                string str = this._srr.Substring(this._srr.IndexOf("var preloadedStock"));
                str = str.Substring(str.IndexOf("["));
                str = str.Substring(0, str.IndexOf("];") + 1);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                if (<>o__12.<>p__14 == null)
                {
                    <>o__12.<>p__14 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Oneblockdown)));
                }
                foreach (object obj2 in <>o__12.<>p__14.Target(<>o__12.<>p__14, this._dynObj))
                {
                    if (<>o__12.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__12.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Oneblockdown), argumentInfo));
                    }
                    if (<>o__12.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__12.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Oneblockdown), argumentInfo));
                    }
                    if (<>o__12.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__12.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Oneblockdown), argumentInfo));
                    }
                    if (<>o__12.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__12.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Oneblockdown), argumentInfo));
                    }
                    if (!<>o__12.<>p__3.Target(<>o__12.<>p__3, <>o__12.<>p__2.Target(<>o__12.<>p__2, <>o__12.<>p__1.Target(<>o__12.<>p__1, <>o__12.<>p__0.Target(<>o__12.<>p__0, obj2, "displaystatus")), "IN_STOCK")))
                    {
                        if (<>o__12.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__12.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Oneblockdown), argumentInfo));
                        }
                        if (<>o__12.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__12.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Oneblockdown), argumentInfo));
                        }
                        object obj4 = <>o__12.<>p__5.Target(<>o__12.<>p__5, <>o__12.<>p__4.Target(<>o__12.<>p__4, obj2, "id"));
                        if (<>o__12.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__12.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Oneblockdown), argumentInfo));
                        }
                        if (<>o__12.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__12.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "attributes", typeof(Oneblockdown), argumentInfo));
                        }
                        object obj3 = <>o__12.<>p__7.Target(<>o__12.<>p__7, <>o__12.<>p__6.Target(<>o__12.<>p__6, obj2), "7");
                        if (<>o__12.<>p__12 == null)
                        {
                            <>o__12.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Oneblockdown)));
                        }
                        if (<>o__12.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__12.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Oneblockdown), argumentInfo));
                        }
                        if (<>o__12.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__12.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Oneblockdown), argumentInfo));
                        }
                        if (<>o__12.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__12.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Oneblockdown), argumentInfo));
                        }
                        if (<>o__12.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__12.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "value", typeof(Oneblockdown), argumentInfo));
                        }
                        string str4 = <>o__12.<>p__12.Target(<>o__12.<>p__12, <>o__12.<>p__11.Target(<>o__12.<>p__11, <>o__12.<>p__10.Target(<>o__12.<>p__10, <>o__12.<>p__9.Target(<>o__12.<>p__9, <>o__12.<>p__8.Target(<>o__12.<>p__8, obj3), "title")))).Replace("US", "");
                        if (<>o__12.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__12.<>p__13 = CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Oneblockdown), argumentInfo));
                        }
                        this._runner.Product.AvailableSizes.Add(<>o__12.<>p__13.Target(<>o__12.<>p__13, typeof(KeyValuePair<string, string>), str4, obj4));
                    }
                }
                if (this._runner.Product.AvailableSizes.Count == 0)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
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
                    double num2 = double.Parse(str5.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                                    goto Label_0BDC;
                                }
                            }
                            continue;
                        Label_0BDC:
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

        public bool Login() => 
            true;

        public bool Search()
        {
            throw new NotSupportedException();
        }

        public void SetClient()
        {
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Oneblockdown.<>c <>9;
            public static Func<HtmlNode, bool> <>9__12_0;
            public static Func<HtmlNode, bool> <>9__12_1;
            public static Func<HtmlNode, bool> <>9__12_2;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Oneblockdown.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__12_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-title"));

            internal bool <DirectLink>b__12_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "item-sell-price"));

            internal bool <DirectLink>b__12_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-carousel "));
        }

        [CompilerGenerated]
        private static class <>o__10
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__12
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>> <>p__13;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__14;
        }
    }
}

