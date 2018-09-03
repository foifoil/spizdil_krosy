namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Notifications;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpenQA.Selenium;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    internal class Shopify : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private List<string> _yeezyVisited;
        private bool _isLoggedIn;
        private string _smartCheckoutShipping;
        private string _cardId;
        private string _checkoutLink;
        private bool _isInitError;
        private bool _isQueue;
        private bool _isCaptcha;
        private string _captchaKey;
        private string _pickedShippingName;
        private string _pickedShippingPrice;
        private string _pickedShippingSource;
        private string _captchaToken;
        private string _paymentGateway;
        private Dictionary<string, string> _diData;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private HttpHeaders _responseHeaders;
        private static DateTime? _eflashUkGetter;

        public Shopify(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._yeezyVisited = new List<string>();
            this._diData = new Dictionary<string, string>();
            this._currentDoc = new HtmlDocument();
            this._runner = runner;
            this._task = task;
            this.SetClient();
        }

        private bool AddToCardCheckoutLink()
        {
            this._task.Status = States.GetTaskState(States.TaskState.CHECKING_CHECKOUT_LINK);
            try
            {
                HttpWebRequest request;
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"checking shopify checkout link"}");
                }
                string html = "";
                if (!this._task.IsShopifyCheckoutLink)
                {
                    request = (HttpWebRequest) WebRequest.Create(this._task.Link);
                }
                else
                {
                    request = (HttpWebRequest) WebRequest.Create(this._task.ShopifyCheckoutLink);
                }
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                }
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Cache-Control", "max-age=0");
                html = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                string str2 = html.Substring(html.IndexOf("url=") + 4);
                request = (HttpWebRequest) WebRequest.Create(str2.Substring(0, str2.IndexOf("\"")));
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                }
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Cache-Control", "max-age=0");
                html = "";
                using (WebResponse response2 = request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        html = reader2.ReadToEnd();
                    }
                }
                str2 = html.Substring(html.IndexOf("url=") + 4);
                request = (HttpWebRequest) WebRequest.Create(str2.Substring(0, str2.IndexOf("\"")));
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                }
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Cache-Control", "max-age=0");
                html = "";
                bool flag = false;
                bool flag2 = true;
                using (WebResponse response3 = request.GetResponse())
                {
                    flag = response3.ResponseUri.ToString().ToLowerInvariant().Contains("stock_problems");
                    flag2 = response3.ResponseUri.ToString().ToLowerInvariant().Contains("/checkouts/");
                    this._runner.CartResponseUri = response3.ResponseUri.ToString();
                    this._checkoutLink = response3.ResponseUri.ToString();
                    using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                    {
                        html = reader3.ReadToEnd();
                    }
                }
                if (!flag2)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.INVALID_CHECKOUT_LINK);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"invalid checkout link, try later"}");
                    return false;
                }
                if (!flag)
                {
                    HtmlNode.ElementsFlags.Remove("form");
                    HtmlNode.ElementsFlags.Remove("option");
                    HtmlDocument document1 = new HtmlDocument();
                    document1.LoadHtml(html);
                    HtmlNode node = document1.DocumentNode.Descendants("div").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "order-summary__section__content"));
                    Product product1 = new Product {
                        ProductTitle = node.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__name order-summary__emphasis"))).InnerText.Trim(),
                        Link = node.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__name order-summary__emphasis"))).InnerText.Trim(),
                        Price = node.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "order-summary__emphasis"))).InnerText.Trim()
                    };
                    this._runner.Product = product1;
                    this._task.ImgUrl = "https:" + node.Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                    string size = node.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__variant order-summary__small-text"))).InnerText.Trim();
                    string str4 = node.Descendants("tr").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product "))).Attributes["data-variant-id"].Value;
                    if (this._task.ShopifyWebsite != "other")
                    {
                        size = ShopifyCommon.UpdateSizeString(this._task, size);
                    }
                    if (string.IsNullOrEmpty(size))
                    {
                        size = "-";
                    }
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(size, str4);
                    this._runner.Product.AvailableSizes.Add(item);
                    this._runner.PickedSize = new KeyValuePair<string, string>?(item);
                    this._runner.CartPageHtml = new HtmlDocument();
                    this._runner.CartPageHtml.LoadHtml(html);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"product available"}");
                    return true;
                }
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_RESTOCK);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"product out of stock"}");
                return false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    this._runner.IsError = true;
                    string str5 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str5 = " - " + exception.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error while checking shopify checkout link{str5}");
                    Global.Logger.Error($"Error while checking shopify checkout link of task '{this._task.Name} - {this._task.Guid}'", exception);
                    return false;
                }
                return false;
            }
        }

        private bool AddToCart()
        {
            this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
            try
            {
                HttpWebRequest request;
                string variant = "";
                if (this._task.Link.ToLowerInvariant().Contains("cart") && this._task.Link.ToLowerInvariant().Contains(":1"))
                {
                    variant = this._task.Link.Substring(this._task.Link.IndexOf("cart") + 5);
                    variant = variant.Substring(0, variant.IndexOf(":1"));
                }
                else if (this._task.TaskType != TaskObject.TaskTypeEnum.variant)
                {
                    variant = this._runner.PickedSize.Value.Value;
                }
                else
                {
                    variant = this._task.Variant;
                }
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"adding product to cart"}");
                }
                string str2 = "";
                string s = "";
                byte[] bytes = null;
                if (!this._task.HomeUrl.Contains("eflash.") && !this._task.HomeUrl.Contains("eflash-us"))
                {
                    request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/cart/add.js");
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.CookieContainer = this._runner.Cookies;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                    {
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    }
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.KeepAlive = true;
                    request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    if (this._task.Quantity < 1)
                    {
                        this._task.Quantity = 1;
                    }
                    s = "";
                    if (this._task.HomeUrl.ToLowerInvariant().Contains("kith") && !string.IsNullOrEmpty(WebsitesInfo.KITH_PROPERTIES.Key))
                    {
                        object[] objArray1 = new object[] { "{\"quantity\": ", this._task.Quantity, ", \"id\": \"", variant, "\", \"properties\": { \"", WebsitesInfo.KITH_PROPERTIES.Key, "\": \"", WebsitesInfo.KITH_PROPERTIES.Value, "\" }}" };
                        s = string.Concat(objArray1);
                    }
                    else if (this._task.HomeUrl.ToLowerInvariant().Contains("palace"))
                    {
                        object[] objArray2 = new object[] { "{\"quantity\": ", this._task.Quantity, ", \"id\": \"", variant, "\", \"properties\": { \"m92rFZL623mmjNyt\": \"X28tunc8HA25375X\" }}" };
                        s = string.Concat(objArray2);
                    }
                    else if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash-us") && !string.IsNullOrEmpty(WebsitesInfo.EFLESH_US_PROPERTIES.Key))
                    {
                        object[] objArray3 = new object[] { "{\"quantity\": ", this._task.Quantity, ", \"id\": \"", variant, "\", \"properties\": { \"", WebsitesInfo.EFLESH_US_PROPERTIES.Key, "\": \"", WebsitesInfo.EFLESH_US_PROPERTIES.Value, "\" }}" };
                        s = string.Concat(objArray3);
                    }
                    else if ((this._task.HomeUrl.ToLowerInvariant().Contains("eflash.") && WebsitesInfo.EFLESH_UK_PROPERTIES.HasValue) && !string.IsNullOrEmpty(WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Key))
                    {
                        object[] objArray4 = new object[] { "{\"quantity\": ", this._task.Quantity, ", \"id\": \"", variant, "\", \"properties\": { \"", WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Key, "\": \"", WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Value, "\" }}" };
                        s = string.Concat(objArray4);
                    }
                    else
                    {
                        object[] objArray5 = new object[] { "{\"quantity\": ", this._task.Quantity, ", \"id\": \"", variant, "\"}" };
                        s = string.Concat(objArray5);
                    }
                    bytes = Encoding.ASCII.GetBytes(s);
                    request.ContentLength = bytes.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    str2 = "";
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            str2 = reader.ReadToEnd();
                        }
                    }
                    if (str2.ToLowerInvariant().Contains("inventory issues"))
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        return false;
                    }
                    object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str2);
                    if (<>o__39.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__39.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__39.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__39.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__39.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__39.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__39.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__39.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__39.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__39.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__39.<>p__4.Target(<>o__39.<>p__4, <>o__39.<>p__3.Target(<>o__39.<>p__3, <>o__39.<>p__2.Target(<>o__39.<>p__2, <>o__39.<>p__1.Target(<>o__39.<>p__1, <>o__39.<>p__0.Target(<>o__39.<>p__0, obj2, "id"))), variant)))
                    {
                        request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/cart.js");
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.CookieContainer = this._runner.Cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                        {
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        }
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        request.KeepAlive = true;
                        request.Headers.Add("Upgrade-Insecure-Requests", "1");
                        str2 = "";
                        using (WebResponse response2 = request.GetResponse())
                        {
                            using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                str2 = reader2.ReadToEnd();
                            }
                        }
                        obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str2);
                        if (<>o__39.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__39.<>p__8 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__39.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__39.<>p__7 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__39.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__39.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__39.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__39.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__39.<>p__8.Target(<>o__39.<>p__8, <>o__39.<>p__7.Target(<>o__39.<>p__7, <>o__39.<>p__6.Target(<>o__39.<>p__6, <>o__39.<>p__5.Target(<>o__39.<>p__5, obj2, "items")), 0)))
                        {
                            this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"failed adding product to cart"}");
                            return false;
                        }
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"product successfully added to cart"}");
                        if (this._runner.Product == null)
                        {
                            Product product = new Product();
                            if (<>o__39.<>p__11 == null)
                            {
                                <>o__39.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__39.<>p__10 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__39.<>p__9 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__39.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            product.ProductTitle = <>o__39.<>p__11.Target(<>o__39.<>p__11, <>o__39.<>p__10.Target(<>o__39.<>p__10, <>o__39.<>p__9.Target(<>o__39.<>p__9, obj2, "title")));
                            if (<>o__39.<>p__15 == null)
                            {
                                <>o__39.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__39.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__14 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__39.<>p__13 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__39.<>p__12 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__39.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            product.Link = <>o__39.<>p__15.Target(<>o__39.<>p__15, <>o__39.<>p__14.Target(<>o__39.<>p__14, this._task.HomeUrl, <>o__39.<>p__13.Target(<>o__39.<>p__13, <>o__39.<>p__12.Target(<>o__39.<>p__12, obj2, "url"))));
                            product.Price = "-";
                            this._runner.Product = product;
                            if (<>o__39.<>p__18 == null)
                            {
                                <>o__39.<>p__18 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__39.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__39.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__39.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            this._task.ImgUrl = <>o__39.<>p__18.Target(<>o__39.<>p__18, <>o__39.<>p__17.Target(<>o__39.<>p__17, <>o__39.<>p__16.Target(<>o__39.<>p__16, obj2, "image")));
                            if (<>o__39.<>p__21 == null)
                            {
                                <>o__39.<>p__21 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__39.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__39.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__39.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            this._task.Size = <>o__39.<>p__21.Target(<>o__39.<>p__21, <>o__39.<>p__20.Target(<>o__39.<>p__20, <>o__39.<>p__19.Target(<>o__39.<>p__19, obj2, "variant_title")));
                            this._runner.PickedSize = new KeyValuePair<string, string>(this._task.Size, variant);
                            this._runner.Success.Size = this._runner.PickedSize.Value.Key;
                            this._runner.Success.Price = this._runner.Product.Price;
                            this._runner.Success.Link = this._runner.Product.Link;
                            this._runner.Success.ProductName = this._runner.Product.ProductTitle;
                            this._runner.Success.Store = EveAIO.Helpers.GetStoreUrl(this._task);
                            if (this._runner.Proxy != null)
                            {
                                this._runner.Success.Proxy = this._task.Proxy;
                            }
                            else
                            {
                                this._runner.Success.Proxy = "-";
                            }
                            this._runner.Success.ProductImage = this._task.ImgUrl;
                            Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                                if (!Global.SUCCESS.Any<SuccessObject>(x => (x.TaskId == this._task.Id)))
                                {
                                    this._runner.Success.Repetitions = 1;
                                    Global.SUCCESS.Add(this._runner.Success);
                                }
                                else
                                {
                                    this._runner.Success.Repetitions++;
                                    int index = Global.SUCCESS.IndexOf(Global.SUCCESS.First<SuccessObject>(x => x.TaskId == this._task.Id));
                                    Global.SUCCESS[index] = this._runner.Success;
                                }
                            }, Array.Empty<object>());
                        }
                        if (this._isQueue)
                        {
                            request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/cart");
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.CookieContainer = this._runner.Cookies;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                            {
                                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            }
                            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                            request.KeepAlive = true;
                            request.Headers.Add("Upgrade-Insecure-Requests", "1");
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            s = "updates%5B%5D=1&checkout=Check+Out";
                            bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream2 = request.GetRequestStream())
                            {
                                stream2.Write(bytes, 0, bytes.Length);
                            }
                            string str4 = "";
                            string str5 = "";
                            str2 = "";
                            using (WebResponse response3 = request.GetResponse())
                            {
                                using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                                {
                                    str2 = reader3.ReadToEnd();
                                }
                                str4 = response3.ResponseUri.ToString();
                                if (response3.Headers["Location"] != null)
                                {
                                    str5 = response3.Headers["Location"];
                                }
                            }
                            this._checkoutLink = string.IsNullOrEmpty(str5) ? str4 : str5;
                        }
                        return true;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"failed adding product to cart"}");
                    return false;
                }
                request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/cart/add");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                }
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (this._task.Quantity < 1)
                {
                    this._task.Quantity = 1;
                }
                s = "id=" + variant + "&add=";
                if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash-us") && !string.IsNullOrEmpty(WebsitesInfo.EFLESH_US_PROPERTIES.Key))
                {
                    string[] textArray1 = new string[] { s, "&", WebUtility.UrlEncode("properties[" + WebsitesInfo.EFLESH_US_PROPERTIES.Key + "]"), "=", WebUtility.UrlEncode(WebsitesInfo.EFLESH_US_PROPERTIES.Value) };
                    s = string.Concat(textArray1);
                }
                else if ((this._task.HomeUrl.ToLowerInvariant().Contains("eflash.") && WebsitesInfo.EFLESH_UK_PROPERTIES.HasValue) && !string.IsNullOrEmpty(WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Key))
                {
                    string[] textArray2 = new string[] { s, "&", WebUtility.UrlEncode("properties[" + WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Key + "]"), "=", WebUtility.UrlEncode(WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Value) };
                    s = string.Concat(textArray2);
                }
                bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream3 = request.GetRequestStream())
                {
                    stream3.Write(bytes, 0, bytes.Length);
                }
                str2 = "";
                using (WebResponse response4 = request.GetResponse())
                {
                    using (StreamReader reader4 = new StreamReader(response4.GetResponseStream()))
                    {
                        str2 = reader4.ReadToEnd();
                    }
                }
                if (str2.ToLowerInvariant().Contains("inventory issues"))
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    return false;
                }
                if (str2.Contains(variant))
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"product successfully added to cart"}");
                    if (this._isQueue)
                    {
                        request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/cart");
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.CookieContainer = this._runner.Cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                        {
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        }
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        request.KeepAlive = true;
                        request.Headers.Add("Upgrade-Insecure-Requests", "1");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        s = "updates%5B%5D=1&checkout=Check+Out";
                        bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream4 = request.GetRequestStream())
                        {
                            stream4.Write(bytes, 0, bytes.Length);
                        }
                        string str6 = "";
                        string str7 = "";
                        str2 = "";
                        using (WebResponse response5 = request.GetResponse())
                        {
                            using (StreamReader reader5 = new StreamReader(response5.GetResponseStream()))
                            {
                                str2 = reader5.ReadToEnd();
                            }
                            str6 = response5.ResponseUri.ToString();
                            if (response5.Headers["Location"] != null)
                            {
                                str7 = response5.Headers["Location"];
                            }
                        }
                        this._checkoutLink = string.IsNullOrEmpty(str7) ? str6 : str7;
                    }
                    return true;
                }
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"failed adding product to cart"}");
                return false;
            }
            catch (Exception exception)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
                this._cardId = "";
                this._runner._tokenTimestamp = null;
                this._checkoutLink = "";
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    this._runner.IsError = true;
                    string str8 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str8 = " - " + exception.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error while adding product to cart {str8}");
                    Global.Logger.Error($"Error while adding product to cart of task '{this._task.Name} - {this._task.Guid}'", exception);
                    return false;
                }
                return false;
            }
        }

        public bool Atc()
        {
            if (this._task.Link.ToLowerInvariant().Contains("/checkouts/") || this._task.IsShopifyCheckoutLink)
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUTLINK_RECOGNIZED, null, "", "");
                return this.AddToCardCheckoutLink();
            }
            if ((this._task.TaskType == TaskObject.TaskTypeEnum.variant) || (this._task.Link.ToLowerInvariant().Contains("/cart/") && this._task.Link.ToLowerInvariant().Contains(":1")))
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATCLINK_RECOGNIZED, null, "", "");
                this.Init(true);
            }
            while (!string.IsNullOrEmpty(this._cardId))
            {
                if (!string.IsNullOrEmpty(this._checkoutLink))
                {
                    return this.AddToCart();
                }
            Label_00A8:
                if (this._isInitError)
                {
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error during task init");
                    return false;
                }
                Thread.Sleep(200);
            }
            goto Label_00A8;
        }

        public bool Checkout()
        {
            this.SetClient();
            if (!this.SubmitShipping())
            {
                return false;
            }
            if (!this.GetShippingRates())
            {
                return false;
            }
            if (!this.SubmitBilling())
            {
                return false;
            }
            if (this._runner.ForcedPaypal)
            {
                return true;
            }
            if (this._isCaptcha && !this.WaitingForCaptcha())
            {
                return false;
            }
            if (this._task.Payment == TaskObject.PaymentEnum.paypal)
            {
                return this.SubmitPaypal(false);
            }
            if (this._task.HomeUrl.Contains("shop-jp.palaceskateboards.com"))
            {
                return this.SubmitOrderPalaceJP();
            }
            return this.SubmitOrder();
        }

        private bool CheckoutInternal(bool smartCheckoutPrep = false)
        {
            int num = 0;
            try
            {
                string str9;
                string str11;
                bool flag7;
                Global.ProxyCheck();
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checkout started");
                HtmlNode.ElementsFlags.Remove("form");
                HtmlDocument document = new HtmlDocument();
                HttpWebRequest request = null;
                string html = "";
                byte[] bytes = null;
                if (this._checkoutLink.Contains("throttle"))
                {
                    string requestUriString = this._checkoutLink;
                    string str3 = "";
                    bool flag = true;
                    string str4 = requestUriString;
                    while (requestUriString.Contains("throttle"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"waiting in the checkout queue"}");
                        if (!flag)
                        {
                            Thread.Sleep(0x1388);
                        }
                        request = (HttpWebRequest) WebRequest.Create(requestUriString);
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.AllowAutoRedirect = false;
                        request.CookieContainer = this._runner.Cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                        {
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        }
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        request.KeepAlive = true;
                        request.Headers.Add("Upgrade-Insecure-Requests", "1");
                        request.Headers.Add("Cache-Control", "max-age=0");
                        request.Referer = flag ? (this._task.HomeUrl + "/cart") : requestUriString;
                        if (!string.IsNullOrEmpty(str3))
                        {
                            request.Headers.Add("If-None-Match", str3);
                        }
                        using (WebResponse response = request.GetResponse())
                        {
                            response.ResponseUri.ToString();
                            if (response.Headers["ETag"] != null)
                            {
                                str3 = response.Headers["ETag"];
                            }
                            if (response.Headers["Location"] != null)
                            {
                                requestUriString = response.Headers["Location"];
                            }
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                html = reader.ReadToEnd();
                            }
                        }
                        flag = false;
                        document.LoadHtml(html);
                        if (!requestUriString.Contains("throttle"))
                        {
                            request = (HttpWebRequest) WebRequest.Create(requestUriString);
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.AllowAutoRedirect = true;
                            request.CookieContainer = this._runner.Cookies;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                            {
                                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            }
                            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                            request.KeepAlive = true;
                            request.Headers.Add("Upgrade-Insecure-Requests", "1");
                            request.Headers.Add("Cache-Control", "max-age=0");
                            request.Referer = str4;
                            using (WebResponse response2 = request.GetResponse())
                            {
                                using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                                {
                                    html = reader2.ReadToEnd();
                                }
                            }
                            this._checkoutLink = requestUriString;
                            this._runner.CartResponseUri = requestUriString;
                            document.LoadHtml(html);
                        }
                        if (html.ToLowerInvariant().Contains("inventory issues") || html.ToLowerInvariant().Contains("stock_problems"))
                        {
                            goto Label_043A;
                        }
                    }
                    this._runner.CartPageHtml = new HtmlDocument();
                    this._runner.CartPageHtml.LoadHtml(html);
                    request.AllowAutoRedirect = true;
                }
                goto Label_04AF;
            Label_043A:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                Global.Logger.Debug($"Task '{this._task.Name} - {this._task.Guid}': Product OOS from queue");
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                return false;
            Label_049A:
                if (this._isInitError)
                {
                    goto Label_3920;
                }
                Thread.Sleep(200);
            Label_04AF:
                if (string.IsNullOrEmpty(this._checkoutLink))
                {
                    goto Label_049A;
                }
                if (this._task.Payment != TaskObject.PaymentEnum.paypal)
                {
                    goto Label_0955;
                }
                request = (HttpWebRequest) WebRequest.Create(this._task.HomeUrl + "/cart");
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string str5 = $"updates%5B{this._runner.PickedSize.Value.Value}%5D=1&checkout=CHECKOUT&note=";
                bytes = Encoding.ASCII.GetBytes(str5);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response3 = (HttpWebResponse) request.GetResponse())
                {
                    new StreamReader(response3.GetResponseStream()).ReadToEnd();
                }
                request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/cart");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                }
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string str6 = "updates%5B%5D=1&goto_pp=paypal_express";
                bytes = Encoding.ASCII.GetBytes(str6);
                request.ContentLength = bytes.Length;
                using (Stream stream2 = request.GetRequestStream())
                {
                    stream2.Write(bytes, 0, bytes.Length);
                }
                string arguments = "";
                using (HttpWebResponse response4 = (HttpWebResponse) request.GetResponse())
                {
                    arguments = response4.ResponseUri.ToString();
                    using (StreamReader reader3 = new StreamReader(response4.GetResponseStream()))
                    {
                        html = reader3.ReadToEnd();
                    }
                    goto Label_08C7;
                }
            Label_07B7:
                request = (HttpWebRequest) WebRequest.Create(this._checkoutLink + "/express/redirecting?refresh_count=1");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                }
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                using (HttpWebResponse response5 = (HttpWebResponse) request.GetResponse())
                {
                    arguments = response5.ResponseUri.ToString();
                    using (StreamReader reader4 = new StreamReader(response5.GetResponseStream()))
                    {
                        html = reader4.ReadToEnd();
                    }
                }
            Label_08C7:
                if (!arguments.Contains("paypal.com"))
                {
                    goto Label_07B7;
                }
                if (Global.SETTINGS.PayPalBeep)
                {
                    EveAIO.Helpers.PlayBell();
                }
                this._task.Status = States.GetTaskState(States.TaskState.OPENING_PAYPAL);
                States.WriteLogger(this._task, States.LOGGER_STATES.OPENING_PAYPAL, null, "", "");
                if (this._runner._notificator != null)
                {
                    this._runner._notificator.Notify(Notificator.NotificationType.Paypal);
                }
                this._task.PaypalLink = arguments;
                Process.Start("chrome.exe", arguments);
                return true;
            Label_0955:
                num++;
                request = (HttpWebRequest) WebRequest.Create(this._task.HomeUrl + "/cart");
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string str8 = $"updates%5B{this._runner.PickedSize.Value.Value}%5D=1&checkout=CHECKOUT&note=";
                bytes = Encoding.ASCII.GetBytes(str8);
                request.ContentLength = bytes.Length;
                using (Stream stream3 = request.GetRequestStream())
                {
                    stream3.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response6 = (HttpWebResponse) request.GetResponse())
                {
                    str9 = new StreamReader(response6.GetResponseStream()).ReadToEnd();
                }
                this._currentDoc.LoadHtml(str9);
                string str10 = WebUtility.UrlEncode(this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"))).Attributes["value"].Value);
                request = (HttpWebRequest) WebRequest.Create(this._checkoutLink + $"?utf8=%E2%9C%93&_method=patch&authenticity_token={str10}&previous_step=contact_information&checkout%5Bemail%5D=&checkout%5Bbuyer_accepts_marketing%5D=0&checkout%5Bbuyer_accepts_marketing%5D=1&checkout%5Bshipping_address%5D%5Bfirst_name%5D=&checkout%5Bshipping_address%5D%5Blast_name%5D=&checkout%5Bshipping_address%5D%5Bcompany%5D=&checkout%5Bshipping_address%5D%5Baddress1%5D=&checkout%5Bshipping_address%5D%5Baddress2%5D=&checkout%5Bshipping_address%5D%5Bcity%5D=&checkout%5Bshipping_address%5D%5Bcountry%5D=&checkout%5Bshipping_address%5D%5Bprovince%5D=&checkout%5Bshipping_address%5D%5Bzip%5D=&checkout%5Bshipping_address%5D%5Bphone%5D=&checkout%5Bshipping_address%5D%5Bfirst_name%5D=&checkout%5Bshipping_address%5D%5Blast_name%5D=&checkout%5Bshipping_address%5D%5Bcompany%5D=&checkout%5Bshipping_address%5D%5Baddress1%5D=&checkout%5Bshipping_address%5D%5Baddress2%5D=&checkout%5Bshipping_address%5D%5Bcity%5D=&checkout%5Bshipping_address%5D%5Bcountry%5D=Slovakia&checkout%5Bshipping_address%5D%5Bprovince%5D=&checkout%5Bshipping_address%5D%5Bzip%5D=&checkout%5Bshipping_address%5D%5Bphone%5D=&step=contact_information");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                request.AllowAutoRedirect = true;
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                html = "";
                using (HttpWebResponse response7 = (HttpWebResponse) request.GetResponse())
                {
                    using (StreamReader reader5 = new StreamReader(response7.GetResponseStream()))
                    {
                        html = reader5.ReadToEnd();
                    }
                }
                this._runner.CartPageHtml = new HtmlDocument();
                this._runner.CartPageHtml.LoadHtml(str9);
                num++;
                if (this._task.Discount && !string.IsNullOrEmpty(this._task.DiscountCode))
                {
                    HtmlNode local1 = this._runner.CartPageHtml.DocumentNode.Descendants("form").First<HtmlNode>(node => (((node.Attributes["class"] != null) && (node.Attributes["class"].Value == "edit_checkout")) && (node.Attributes["novalidate"] != null)) && (node.Attributes["novalidate"].Value == "novalidate"));
                    string text1 = local1.Attributes["action"].Value;
                    str10 = local1.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"))).Attributes["value"].Value;
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"applying discount code"}");
                    str11 = ("utf8=%E2%9C%93&_method=patch&authenticity_token=" + Uri.EscapeDataString(str10) + "&step=contact_information") + "&checkout%5Breduction_code%5D=" + this._task.DiscountCode;
                    request = (HttpWebRequest) WebRequest.Create(this._checkoutLink);
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.Method = "POST";
                    request.CookieContainer = this._runner.Cookies;
                    request.KeepAlive = true;
                    request.ContentLength = str11.Length;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                    {
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    }
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    request.Headers.Add("Cache-Control", "max-age=0");
                    using (Stream stream4 = request.GetRequestStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream4))
                        {
                            writer.Write(str11, str11.Length);
                        }
                    }
                    html = "";
                    using (WebResponse response8 = request.GetResponse())
                    {
                        using (StreamReader reader6 = new StreamReader(response8.GetResponseStream()))
                        {
                            html = reader6.ReadToEnd();
                        }
                    }
                    if (html.ToLowerInvariant().Contains("inventory issues"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                        return false;
                    }
                    document.LoadHtml(html);
                    string innerText = document.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "total-recap__final-price"))).InnerText;
                    if (!Global.SETTINGS.PowerMode)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"discount code applied - new price: " + innerText}");
                    }
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                num++;
                if (this._runner.CartPageHtml.DocumentNode.InnerHtml.Contains("/stock_problem"))
                {
                    goto Label_38EB;
                }
                num++;
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"submitting shipping information"}");
                }
                string str20 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Zip.Trim().Replace(" ", "+");
                string str22 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).State.Trim().Replace(" ", "+");
                string str24 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Country.Trim().Replace(" ", "+");
                bool flag3 = false;
                bool flag4 = false;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                bool flag5 = true;
                goto Label_1CCF;
            Label_10C3:
                string text2 = this._runner.CartPageHtml.DocumentNode.Descendants("form").First<HtmlNode>(node => ((((node.Attributes["class"] != null) && (node.Attributes["class"].Value == "edit_checkout")) && (node.Attributes["novalidate"] != null)) && (node.Attributes["novalidate"].Value == "novalidate"))).Attributes["action"].Value;
                str10 = this._runner.CartPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"))).Attributes["value"].Value;
                flag3 = true;
                if (this._task.ShopifyWebsite != "other")
                {
                    EveAIO.Helpers.AddCheckoutLink(this._checkoutLink, this._task.HomeUrl, this._runner.Product.ProductTitle, this._runner.PickedSize.Value.Key);
                }
                request = (HttpWebRequest) WebRequest.Create(this._checkoutLink);
                this._runner.CheckoutUrl = this._checkoutLink;
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                string str16 = Uri.EscapeDataString(Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Email.Trim());
                string str23 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).FirstName.Trim().Replace(" ", "+");
                string str17 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).LastName.Trim().Replace(" ", "+");
                string str18 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address1.Trim().Replace(" ", "+");
                string str19 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address2.Trim().Replace(" ", "+");
                string str14 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).City.Trim().Replace(" ", "+");
                string str15 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Phone.Trim().Replace(" ", "+");
                if (!Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).SameBillingShipping)
                {
                    Uri.EscapeDataString(Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).EmailShipping.Trim());
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).FirstNameShipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).LastNameShipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address1Shipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address2Shipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CityShipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).ZipShipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).StateShipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).PhoneShipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CountryShipping.Trim().Replace(" ", "+");
                }
                str11 = ((((((((((((((("utf8=%E2%9C%93&_method=patch&authenticity_token=" + Uri.EscapeDataString(str10)) + "&previous_step=contact_information" + "&step=shipping_method") + "&checkout%5bbuyer_accepts_marketing%5d=0" + "&checkout%5bremember_me%5d=0") + "&checkout%5bemail%5d=" + str16) + "&checkout%5bshipping_address%5d%5bfirst_name%5d=" + str23) + "&checkout%5bshipping_address%5d%5blast_name%5d=" + str17) + "&checkout%5bshipping_address%5d%5bcompany%5d=") + "&checkout%5bshipping_address%5d%5baddress1%5d=" + str18) + "&checkout%5bshipping_address%5d%5baddress2%5d=" + str19) + "&checkout%5bshipping_address%5d%5bcity%5d=" + str14) + "&checkout%5bshipping_address%5d%5bcountry%5d=" + str24) + "&checkout%5bshipping_address%5d%5bprovince%5d=" + str22) + "&checkout%5bshipping_address%5d%5bzip%5d=" + str20) + "&checkout%5bshipping_address%5d%5bphone%5d=" + str15) + "&button=" + "&checkout%5bclient_details%5d%5bbrowser_width%5d=2016") + "&checkout%5bclient_details%5d%5bbrowser_height%5d=481" + "&salesFinal=1";
                if (this._runner.CartPageHtml.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "g-recaptcha")))
                {
                    flag4 = true;
                    string str21 = "";
                    if (this._runner.CartPageHtml.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "g-recaptcha")) && (x.Attributes["data-sitekey"] > null)))
                    {
                        str21 = this._runner.CartPageHtml.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "g-recaptcha"))).Attributes["data-sitekey"].Value;
                    }
                    else
                    {
                        string str12 = "";
                        if (!this._runner.CartPageHtml.DocumentNode.InnerHtml.Contains("'sitekey'"))
                        {
                            str12 = this._runner.CartPageHtml.DocumentNode.InnerHtml.Substring(this._runner.CartPageHtml.DocumentNode.InnerHtml.IndexOf("sitekey:"));
                        }
                        else
                        {
                            str12 = this._runner.CartPageHtml.DocumentNode.InnerHtml.Substring(this._runner.CartPageHtml.DocumentNode.InnerHtml.IndexOf("'sitekey'"));
                        }
                        str12 = str12.Substring(str12.IndexOf("\"") + 1);
                        str21 = str12.Substring(0, str12.IndexOf("\""));
                    }
                    if (string.IsNullOrEmpty(str21))
                    {
                        goto Label_1CD8;
                    }
                    if (flag5 && Global.SETTINGS.DetailedLog)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"captcha required - captcha key found: " + str21}");
                    }
                    this._captchaKey = str21;
                    if (flag5)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"waiting for valid captcha token"}");
                    }
                    flag5 = false;
                    this._isCaptcha = true;
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    this._task.Mre = new ManualResetEvent(false);
                    string link = "";
                    if (!this._task.Link.ToLowerInvariant().Contains("/checkouts/") && !this._task.IsShopifyCheckoutLink)
                    {
                        if ((this._task.TaskType == TaskObject.TaskTypeEnum.variant) || (this._task.Link.ToLowerInvariant().Contains("/cart/") && this._task.Link.ToLowerInvariant().Contains(":1")))
                        {
                            link = this._task.Link;
                        }
                        else
                        {
                            link = this._runner.Product.Link;
                        }
                    }
                    else
                    {
                        link = this._runner.HomeUrl;
                    }
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), str21, link, "Shopify");
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
                    str11 = str11 + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token);
                }
                else if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"no captcha required"}");
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                request.Method = "POST";
                request.CookieContainer = this._runner.Cookies;
                request.KeepAlive = true;
                request.ContentLength = str11.Length;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                }
                request.Accept = "*/*";
                request.ServicePoint.Expect100Continue = false;
                using (Stream stream5 = request.GetRequestStream())
                {
                    using (StreamWriter writer2 = new StreamWriter(stream5))
                    {
                        writer2.Write(str11, str11.Length);
                    }
                }
                html = "";
                using (WebResponse response9 = request.GetResponse())
                {
                    using (StreamReader reader7 = new StreamReader(response9.GetResponseStream()))
                    {
                        html = reader7.ReadToEnd();
                    }
                }
                if (html.ToLowerInvariant().Contains("inventory issues"))
                {
                    goto Label_1D01;
                }
                document.LoadHtml(html);
                if (html.Contains("Captcha validation failed"))
                {
                    flag3 = false;
                    this._runner.CartPageHtml = new HtmlDocument();
                    this._runner.CartPageHtml.LoadHtml(html);
                }
            Label_1CCF:
                if (flag3)
                {
                    goto Label_1D36;
                }
                goto Label_10C3;
            Label_1CD8:
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"no captcha key found"}");
                return false;
            Label_1D01:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                return false;
            Label_1D36:
                num++;
                stopwatch.Stop();
                if (flag4 && Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"waited for valid token - " + ((stopwatch.Elapsed.Minutes * 60) + stopwatch.Elapsed.Seconds) + " seconds"}");
                }
                document.LoadHtml(html);
                this._smartCheckoutShipping = html;
                double num3 = 0.0;
                string str27 = "";
                string str26 = "";
                string str28 = "";
                if (!this._task.IsShopifyCheckoutLink && !this._task.Link.Contains("checkout"))
                {
                    string[] textArray1 = new string[] { this._runner.HomeUrl, "/cart/shipping_rates.json?shipping_address%5Bzip%5D=", str20, "&shipping_address%5Bcountry%5D=", str24, "&shipping_address%5Bprovince%5D=", str22 };
                    request = (HttpWebRequest) WebRequest.Create(string.Concat(textArray1));
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.CookieContainer = this._runner.Cookies;
                    request.KeepAlive = true;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    request.Accept = "*/*";
                    request.Referer = this._runner.CheckoutUrl;
                    html = "";
                    try
                    {
                        using (WebResponse response10 = request.GetResponse())
                        {
                            using (StreamReader reader8 = new StreamReader(response10.GetResponseStream()))
                            {
                                html = reader8.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException exception)
                    {
                        string str29 = "";
                        if (exception.Response != null)
                        {
                            using (Stream stream6 = exception.Response.GetResponseStream())
                            {
                                using (StreamReader reader9 = new StreamReader(stream6))
                                {
                                    str29 = reader9.ReadToEnd();
                                }
                            }
                            if (str29.Contains("is not supported"))
                            {
                                goto Label_1F7D;
                            }
                        }
                        throw new Exception("Error getting shipping rates");
                    Label_1F7D:
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"your shipping country not supported"}");
                        this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                        return false;
                    }
                    object obj3 = Newtonsoft.Json.JsonConvert.DeserializeObject(html);
                    if (<>o__44.<>p__41 == null)
                    {
                        <>o__44.<>p__41 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                    }
                    if (<>o__44.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__44.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "shipping_rates", typeof(Shopify), argumentInfo));
                    }
                    foreach (object obj4 in <>o__44.<>p__41.Target(<>o__44.<>p__41, <>o__44.<>p__0.Target(<>o__44.<>p__0, obj3)))
                    {
                        if (!string.IsNullOrEmpty(str27))
                        {
                            if (<>o__44.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__21 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            object obj5 = <>o__44.<>p__21.Target(<>o__44.<>p__21, typeof(double), <>o__44.<>p__20.Target(<>o__44.<>p__20, <>o__44.<>p__19.Target(<>o__44.<>p__19, <>o__44.<>p__18.Target(<>o__44.<>p__18, obj4, "price"))), CultureInfo.InvariantCulture);
                            if (<>o__44.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__23 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__22 = CallSite<Func<CallSite, object, double, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__23.Target(<>o__44.<>p__23, <>o__44.<>p__22.Target(<>o__44.<>p__22, obj5, num3)))
                            {
                                if (<>o__44.<>p__27 == null)
                                {
                                    <>o__44.<>p__27 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                                }
                                if (<>o__44.<>p__26 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__25 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__24 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__44.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                str27 = <>o__44.<>p__27.Target(<>o__44.<>p__27, <>o__44.<>p__26.Target(<>o__44.<>p__26, <>o__44.<>p__25.Target(<>o__44.<>p__25, <>o__44.<>p__24.Target(<>o__44.<>p__24, obj4, "code"))));
                                if (<>o__44.<>p__32 == null)
                                {
                                    <>o__44.<>p__32 = CallSite<Func<CallSite, object, double>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify)));
                                }
                                if (<>o__44.<>p__31 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__44.<>p__31 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__30 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__29 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__28 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__44.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                num3 = <>o__44.<>p__32.Target(<>o__44.<>p__32, <>o__44.<>p__31.Target(<>o__44.<>p__31, typeof(double), <>o__44.<>p__30.Target(<>o__44.<>p__30, <>o__44.<>p__29.Target(<>o__44.<>p__29, <>o__44.<>p__28.Target(<>o__44.<>p__28, obj4, "price"))), CultureInfo.InvariantCulture));
                                if (<>o__44.<>p__36 == null)
                                {
                                    <>o__44.<>p__36 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                                }
                                if (<>o__44.<>p__35 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__34 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__33 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__44.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                str26 = <>o__44.<>p__36.Target(<>o__44.<>p__36, <>o__44.<>p__35.Target(<>o__44.<>p__35, <>o__44.<>p__34.Target(<>o__44.<>p__34, <>o__44.<>p__33.Target(<>o__44.<>p__33, obj4, "price"))));
                                if (<>o__44.<>p__40 == null)
                                {
                                    <>o__44.<>p__40 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                                }
                                if (<>o__44.<>p__39 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__38 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__44.<>p__37 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__44.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                str28 = <>o__44.<>p__40.Target(<>o__44.<>p__40, <>o__44.<>p__39.Target(<>o__44.<>p__39, <>o__44.<>p__38.Target(<>o__44.<>p__38, <>o__44.<>p__37.Target(<>o__44.<>p__37, obj4, "source"))));
                            }
                        }
                        else
                        {
                            if (<>o__44.<>p__4 == null)
                            {
                                <>o__44.<>p__4 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__44.<>p__3 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__2 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            str27 = <>o__44.<>p__4.Target(<>o__44.<>p__4, <>o__44.<>p__3.Target(<>o__44.<>p__3, <>o__44.<>p__2.Target(<>o__44.<>p__2, <>o__44.<>p__1.Target(<>o__44.<>p__1, obj4, "code"))));
                            if (<>o__44.<>p__9 == null)
                            {
                                <>o__44.<>p__9 = CallSite<Func<CallSite, object, double>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify)));
                            }
                            if (<>o__44.<>p__8 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__8 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            num3 = <>o__44.<>p__9.Target(<>o__44.<>p__9, <>o__44.<>p__8.Target(<>o__44.<>p__8, typeof(double), <>o__44.<>p__7.Target(<>o__44.<>p__7, <>o__44.<>p__6.Target(<>o__44.<>p__6, <>o__44.<>p__5.Target(<>o__44.<>p__5, obj4, "price"))), CultureInfo.InvariantCulture));
                            if (<>o__44.<>p__13 == null)
                            {
                                <>o__44.<>p__13 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__44.<>p__12 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__10 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            str26 = <>o__44.<>p__13.Target(<>o__44.<>p__13, <>o__44.<>p__12.Target(<>o__44.<>p__12, <>o__44.<>p__11.Target(<>o__44.<>p__11, <>o__44.<>p__10.Target(<>o__44.<>p__10, obj4, "price"))));
                            if (<>o__44.<>p__17 == null)
                            {
                                <>o__44.<>p__17 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__44.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__44.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__44.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            str28 = <>o__44.<>p__17.Target(<>o__44.<>p__17, <>o__44.<>p__16.Target(<>o__44.<>p__16, <>o__44.<>p__15.Target(<>o__44.<>p__15, <>o__44.<>p__14.Target(<>o__44.<>p__14, obj4, "source"))));
                        }
                    }
                    goto Label_33E0;
                }
                List<Tuple<string, string, string, double?>> source = new List<Tuple<string, string, string, double?>>();
                if (document.DocumentNode.Descendants("div").Any<HtmlNode>(node => (node.Attributes["class"] != null) && (node.Attributes["class"].Value == "section section--shipping-method")))
                {
                    HtmlNode node = document.DocumentNode.Descendants("div").First<HtmlNode>(node => (node.Attributes["class"] != null) && (node.Attributes["class"].Value == "section section--shipping-method"));
                    foreach (HtmlNode node2 in from x in node.Descendants("input")
                        where (x.Attributes["type"] != null) && (x.Attributes["type"].Value == "radio")
                        select x)
                    {
                        double? nullable;
                        StringBuilder builder = new StringBuilder();
                        string str32 = node2.Attributes["data-checkout-shipping-rate"].Value;
                        string str31 = node2.Attributes["value"].Value;
                        if (this._task.Link.ToLowerInvariant().Contains("worldofhombre"))
                        {
                            str31 = str31.Replace("%20", "%2520").Replace("&amp;", "%26");
                        }
                        string id = node2.Attributes["id"].Value;
                        foreach (char ch in str32)
                        {
                            if (((ch == '.') || (ch == ',')) || char.IsDigit(ch))
                            {
                                builder.Append(ch);
                            }
                        }
                        if (!double.TryParse(builder.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out double num5))
                        {
                            nullable = null;
                        }
                        else
                        {
                            nullable = new double?(num5);
                        }
                        string str30 = node.Descendants("label").First<HtmlNode>(x => ((x.Attributes["for"] != null) && (x.Attributes["for"].Value == id))).Descendants("span").First<HtmlNode>().InnerHtml.Trim();
                        source.Add(new Tuple<string, string, string, double?>(str30, str31, str32, nullable));
                    }
                }
                if (source.Count != 0)
                {
                    goto Label_330B;
                }
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': {"waiting for shipping methods"}");
                }
                int num6 = 0;
                goto Label_32C8;
            Label_2F6D:
                if (num6 > 10)
                {
                    goto Label_330B;
                }
                num6++;
                request = (HttpWebRequest) WebRequest.Create(this._runner.CheckoutUrl + "/shipping_rates?step=shipping_method ");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                }
                request.Accept = "*/*";
                request.Referer = this._runner.CheckoutUrl;
                html = "";
                using (WebResponse response11 = request.GetResponse())
                {
                    using (StreamReader reader10 = new StreamReader(response11.GetResponseStream()))
                    {
                        html = reader10.ReadToEnd();
                    }
                }
                if (html.ToLowerInvariant().Contains("inventory issues"))
                {
                    goto Label_32D6;
                }
                document.LoadHtml(html);
                if (html.Contains("section section--shipping-method"))
                {
                    HtmlNode node3 = document.DocumentNode.Descendants("div").First<HtmlNode>(node => (node.Attributes["class"] != null) && (node.Attributes["class"].Value == "section section--shipping-method"));
                    source = new List<Tuple<string, string, string, double?>>();
                    foreach (HtmlNode node4 in from x in node3.Descendants("input")
                        where (x.Attributes["type"] != null) && (x.Attributes["type"].Value == "radio")
                        select x)
                    {
                        double? nullable2;
                        StringBuilder builder2 = new StringBuilder();
                        string str36 = node4.Attributes["data-checkout-shipping-rate"].Value;
                        string str34 = node4.Attributes["value"].Value;
                        if (this._task.Link.ToLowerInvariant().Contains("worldofhombre"))
                        {
                            str34 = str34.Replace("%20", "%2520").Replace("&amp;", "%26");
                        }
                        string text1 = node4.Attributes["id"].Value;
                        string str35 = node3.Descendants("label").First<HtmlNode>(x => ((x.Attributes["for"] != null) && (x.Attributes["for"].Value == text1))).Descendants("span").First<HtmlNode>().InnerHtml.Trim();
                        foreach (char ch2 in str36)
                        {
                            if (((ch2 == '.') || (ch2 == ',')) || char.IsDigit(ch2))
                            {
                                builder2.Append(ch2);
                            }
                        }
                        if (double.TryParse(builder2.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out double num7))
                        {
                            nullable2 = new double?(num7);
                        }
                        else
                        {
                            nullable2 = null;
                        }
                        source.Add(new Tuple<string, string, string, double?>(str35, str34, str36, nullable2));
                    }
                }
            Label_32C8:
                if (source.Count != 0)
                {
                    goto Label_330B;
                }
                goto Label_2F6D;
            Label_32D6:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                return false;
            Label_330B:
                if (source.Count == 0)
                {
                    goto Label_38C2;
                }
                Tuple<string, string, string, double?> tuple = source[0];
                if (source.Count > 1)
                {
                    if (!source.Any<Tuple<string, string, string, double?>>(x => ((x.Item3.ToUpper().Contains("FREE") && !x.Item1.ToUpper().Contains("PICK")) && !x.Item1.ToUpper().Contains("COLLECT"))))
                    {
                        tuple = (from s in source
                            where s.Item4.HasValue
                            orderby s.Item4
                            select s).Take<Tuple<string, string, string, double?>>(1).First<Tuple<string, string, string, double?>>();
                    }
                    else
                    {
                        tuple = source.First<Tuple<string, string, string, double?>>(x => (x.Item3.ToUpper().Contains("FREE") && !x.Item1.ToUpper().Contains("PICK")) && !x.Item1.ToUpper().Contains("COLLECT"));
                    }
                }
                str28 = tuple.Item1;
            Label_33E0:
                flag7 = true;
                while (flag7)
                {
                    flag7 = false;
                    HtmlNode local2 = document.DocumentNode.Descendants("div").First<HtmlNode>(node => ((node.Attributes["data-step"] != null) && (node.Attributes["data-step"].Value == "shipping_method"))).Descendants("form").First<HtmlNode>();
                    string text3 = local2.Attributes["action"].Value;
                    str10 = local2.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"))).Attributes["value"].Value;
                    request = (HttpWebRequest) WebRequest.Create(this._checkoutLink);
                    string str37 = "";
                    if (!this._task.IsShopifyCheckoutLink && !this._task.Link.Contains("checkout"))
                    {
                        string[] textArray2 = new string[] { str28, "-", str27.Replace(" ", "%2520").Replace("&amp;", "%26"), "-", str26 };
                        str37 = string.Concat(textArray2);
                    }
                    else
                    {
                        str37 = str28;
                    }
                    str11 = (("utf8=%E2%9C%93&_method=patch&authenticity_token=" + Uri.EscapeDataString(str10) + "&previous_step=shipping_method&step=payment_method") + "&checkout%5Bshipping_rate%5D%5Bid%5D=" + str37 + "&button=") + "&checkout%5bclient_details%5d%5bbrowser_width%5d=2016&checkout%5bclient_details%5d%5bbrowser_height%5d=481";
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.Method = "POST";
                    request.CookieContainer = this._runner.Cookies;
                    request.KeepAlive = true;
                    request.ContentLength = str11.Length;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                    {
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    }
                    request.Accept = "*/*";
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.ServicePoint.Expect100Continue = true;
                    request.AllowAutoRedirect = true;
                    using (Stream stream7 = request.GetRequestStream())
                    {
                        using (StreamWriter writer3 = new StreamWriter(stream7))
                        {
                            writer3.Write(str11, str11.Length);
                        }
                    }
                    html = "";
                    using (HttpWebResponse response12 = (HttpWebResponse) request.GetResponse())
                    {
                        using (StreamReader reader11 = new StreamReader(response12.GetResponseStream()))
                        {
                            html = reader11.ReadToEnd();
                        }
                        response12.ResponseUri.ToString();
                    }
                    num++;
                    if (html.ToLowerInvariant().Contains("inventory issues"))
                    {
                        goto Label_388D;
                    }
                    if (html.ToLowerInvariant().Contains("calculating taxes"))
                    {
                        flag7 = true;
                    }
                }
                document.LoadHtml(html);
                if (Global.Machine_name == "Phillip")
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment didn't go through - correct your payment information");
                }
                num++;
                if (!html.ToUpper().Contains("CREDIT CARD") && !html.ToUpper().Contains("SHOPIFY PAYMENTS"))
                {
                    if (!html.ToUpper().Contains("PayPal".ToUpper()))
                    {
                        EveAIO.Helpers.WriteLog($"task '{this._task.Name}': unknown error occured while entering to payment gateway page");
                        return false;
                    }
                    if (this._task.Payment != TaskObject.PaymentEnum.creditcard)
                    {
                        return this.PaymentMethod(html, smartCheckoutPrep, false, false);
                    }
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': credit card payment not available. Paypal is possible. Manual checkout needed");
                    return this.PaymentMethod(html, smartCheckoutPrep, true, false);
                }
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': entering payment gateway page");
                }
                if (!html.Contains("payment-gateway"))
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': unknown shopify payment method");
                    return false;
                }
                return this.PaymentMethod(html, smartCheckoutPrep, false, false);
            Label_388D:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                return false;
            Label_38C2:
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': {"no shipping methods available!"}");
                return false;
            Label_38EB:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                return false;
            Label_3920:
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error during task init");
                return false;
            }
            catch (Exception exception2)
            {
                if (!exception2.Message.Contains("Thread was being aborted") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("Thread was being aborted")))
                {
                    this._runner.IsError = true;
                    string str38 = "";
                    if (exception2.GetType() == typeof(WebException))
                    {
                        str38 = " - " + exception2.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error while product checkout{str38}");
                    Global.Logger.Error($"Error while product checkout of task '{this._task.Name} - {this._task.Guid}', errCode: {num}", exception2);
                    return false;
                }
                return false;
            }
        }

        public bool DirectLink(string link)
        {
            if ((this._task.Payment == TaskObject.PaymentEnum.paypal) && (this._task.Driver == null))
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                return false;
            }
            this.Init(!link.ToLowerInvariant().Contains("/checkouts/") ? !this._task.IsShopifyCheckoutLink : false);
            if (this._task.Login && !this._isLoggedIn)
            {
                this.Login();
            }
            if (link.ToLowerInvariant().Contains("/checkouts/") || this._task.IsShopifyCheckoutLink)
            {
                return true;
            }
            if ((this._task.ShopifyWebsite != "yeezy") && !this._task.Link.Contains("yeezysupply"))
            {
                return this.DirectLinkGeneral(this._task.Link);
            }
            return this.DirectLinkYeezy(this._task.Link, false);
        }

        private bool DirectLinkGeneral(string link)
        {
            try
            {
                string innerText;
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                if (this._task.SmartCheckout && !this._runner.IsSmartCheckoutReady)
                {
                    link = this._task.DummyProduct;
                }
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checking stock availability - {link}");
                string url = link.Replace("#product", "");
                if (url.Contains("?"))
                {
                    url = url.Substring(0, url.IndexOf("?"));
                }
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                MethodInfo method = type.GetMethod("GetShopifyXml");
                object obj2 = Activator.CreateInstance(type);
                string text1 = (string) method.Invoke(obj2, null);
                string requestUriString = "";
                if (!this._task.Link.Contains("eflash") && !this._task.Link.Contains("store.travisscott.com"))
                {
                    requestUriString = url + ".js";
                }
                else
                {
                    requestUriString = url;
                }
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    innerText = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                string str4 = innerText;
                string str5 = innerText;
                if (this._task.Link.Contains("eflash"))
                {
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(innerText);
                    if (!document.DocumentNode.Descendants("script").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"))))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        return false;
                    }
                    innerText = document.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"))).InnerText;
                }
                else if (this._task.Link.Contains("store.travisscott.com"))
                {
                    this._currentDoc.LoadHtml(innerText);
                    innerText = innerText.Substring(innerText.IndexOf("var meta = {"));
                    innerText = innerText.Substring(innerText.IndexOf("{"));
                    innerText = innerText.Substring(0, innerText.IndexOf("}};") + 2);
                }
                object obj3 = Newtonsoft.Json.JsonConvert.DeserializeObject(innerText);
                if (this._task.Link.Contains("store.travisscott.com"))
                {
                    if (<>o__41.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "product", typeof(Shopify), argumentInfo));
                    }
                    obj3 = <>o__41.<>p__0.Target(<>o__41.<>p__0, obj3);
                    Product product1 = new Product {
                        ProductTitle = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:title"))).Attributes["content"].Value,
                        Link = link
                    };
                    this._runner.Product = product1;
                }
                else
                {
                    Product product = new Product();
                    if (<>o__41.<>p__3 == null)
                    {
                        <>o__41.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                    }
                    if (<>o__41.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__41.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    product.ProductTitle = <>o__41.<>p__3.Target(<>o__41.<>p__3, <>o__41.<>p__2.Target(<>o__41.<>p__2, <>o__41.<>p__1.Target(<>o__41.<>p__1, obj3, "title")));
                    product.Link = link;
                    this._runner.Product = product;
                }
                if (this._task.HomeUrl.ToLowerInvariant().Contains("bowsandarrowsberkeley"))
                {
                    if (<>o__41.<>p__6 == null)
                    {
                        <>o__41.<>p__6 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                    }
                    if (<>o__41.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__41.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__6.Target(<>o__41.<>p__6, <>o__41.<>p__5.Target(<>o__41.<>p__5, <>o__41.<>p__4.Target(<>o__41.<>p__4, obj3, "options"))).ToLowerInvariant().Contains("IN-STORE ONLY".ToLowerInvariant()))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.IN_STORE_ONLY, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.IN_STORE_ONLY);
                        return false;
                    }
                }
                if (<>o__41.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__41.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                }
                if (<>o__41.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__41.<>p__8 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                }
                if (<>o__41.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__41.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                }
                object obj4 = <>o__41.<>p__8.Target(<>o__41.<>p__8, <>o__41.<>p__7.Target(<>o__41.<>p__7, obj3, "available"), null);
                if (<>o__41.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__41.<>p__13 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Shopify), argumentInfo));
                }
                if (!<>o__41.<>p__13.Target(<>o__41.<>p__13, obj4))
                {
                    if (<>o__41.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__12 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__41.<>p__11 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                }
                if (<>o__41.<>p__14.Target(<>o__41.<>p__14, (<>o__41.<>p__9 != null) ? obj4 : <>o__41.<>p__12.Target(<>o__41.<>p__12, obj4, <>o__41.<>p__11.Target(<>o__41.<>p__11, <>o__41.<>p__10.Target(<>o__41.<>p__10, <>o__41.<>p__9.Target(<>o__41.<>p__9, obj3, "available")), true))))
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': product out of stock");
                    return false;
                }
                if (this._task.Link.Contains("store.travisscott.com"))
                {
                    goto Label_1137;
                }
                if (<>o__41.<>p__32 == null)
                {
                    <>o__41.<>p__32 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                }
                if (<>o__41.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__41.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify), argumentInfo));
                }
                foreach (object obj5 in <>o__41.<>p__32.Target(<>o__41.<>p__32, <>o__41.<>p__15.Target(<>o__41.<>p__15, obj3)))
                {
                    if (<>o__41.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__19 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__41.<>p__18 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__41.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    if (!<>o__41.<>p__19.Target(<>o__41.<>p__19, <>o__41.<>p__18.Target(<>o__41.<>p__18, <>o__41.<>p__17.Target(<>o__41.<>p__17, <>o__41.<>p__16.Target(<>o__41.<>p__16, obj5, "available")), true)))
                    {
                        if (<>o__41.<>p__23 == null)
                        {
                            <>o__41.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                        }
                        if (<>o__41.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__41.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        this._runner.Product.Price = <>o__41.<>p__23.Target(<>o__41.<>p__23, <>o__41.<>p__22.Target(<>o__41.<>p__22, <>o__41.<>p__21.Target(<>o__41.<>p__21, <>o__41.<>p__20.Target(<>o__41.<>p__20, obj5, "price"))));
                        this._runner.Product.Price = this._runner.Product.Price.Insert(this._runner.Product.Price.Length - 2, ".");
                        if (<>o__41.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__31 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__41.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__41.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__41.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        KeyValuePair<string, string> item = <>o__41.<>p__31.Target(<>o__41.<>p__31, typeof(KeyValuePair<string, string>), <>o__41.<>p__27.Target(<>o__41.<>p__27, <>o__41.<>p__26.Target(<>o__41.<>p__26, <>o__41.<>p__25.Target(<>o__41.<>p__25, <>o__41.<>p__24.Target(<>o__41.<>p__24, obj5, "title")))), <>o__41.<>p__30.Target(<>o__41.<>p__30, <>o__41.<>p__29.Target(<>o__41.<>p__29, <>o__41.<>p__28.Target(<>o__41.<>p__28, obj5, "id"))));
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                }
                goto Label_1149;
            Label_0F45:
                str4 = str4.Substring(str4.IndexOf("p.variants.push({"));
                str4 = str4.Substring(str4.IndexOf("id"));
                str4 = str4.Substring(str4.IndexOf(":") + 1);
                string str7 = str4.Substring(0, str4.IndexOf(",")).Trim();
                str4 = str4.Substring(str4.IndexOf("available"));
                str4 = str4.Substring(str4.IndexOf(":") + 1);
                if (str4.Substring(0, str4.IndexOf(",")).Trim() != "false")
                {
                    str4 = str4.Substring(str4.IndexOf("price"));
                    str4 = str4.Substring(str4.IndexOf(":") + 1);
                    string str6 = str4.Substring(0, str4.IndexOf(",")).Trim();
                    str4 = str4.Substring(str4.IndexOf("options"));
                    str4 = str4.Substring(str4.IndexOf(":") + 1);
                    string key = str4.Substring(0, str4.IndexOf(",")).Trim().Replace("[", "").Replace("]", "").Replace("\"", "");
                    this._runner.Product.Price = str6;
                    this._runner.Product.Price = this._runner.Product.Price.Insert(this._runner.Product.Price.Length - 2, ".");
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(key, str7);
                    this._runner.Product.AvailableSizes.Add(item);
                }
            Label_1137:
                if (str4.Contains("p.variants.push({"))
                {
                    goto Label_0F45;
                }
            Label_1149:
                if (this._runner.Product.AvailableSizes.Count == 0)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': product out of stock");
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
                if (this._task.ShopifyWebsite != "other")
                {
                    ShopifyCommon.UpdatePredefinedSizing((Product) this._runner, this._task);
                }
                if (!this._task.Link.Contains("store.travisscott.com"))
                {
                    if (<>o__41.<>p__37 == null)
                    {
                        <>o__41.<>p__37 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                    }
                    if (<>o__41.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__36 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__41.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__41.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    this._task.ImgUrl = <>o__41.<>p__37.Target(<>o__41.<>p__37, <>o__41.<>p__36.Target(<>o__41.<>p__36, "http:", <>o__41.<>p__35.Target(<>o__41.<>p__35, <>o__41.<>p__34.Target(<>o__41.<>p__34, <>o__41.<>p__33.Target(<>o__41.<>p__33, obj3, "featured_image")))));
                }
                else
                {
                    this._task.ImgUrl = str5.Substring(str5.IndexOf("p.images.push({"));
                    this._task.ImgUrl = this._task.ImgUrl.Substring(this._task.ImgUrl.IndexOf("\"") + 1);
                    this._task.ImgUrl = this._task.ImgUrl.Substring(0, this._task.ImgUrl.IndexOf("\"")).Trim();
                    this._task.ImgUrl = "https:" + this._task.ImgUrl.Replace(@"\/", "/");
                }
                if (this._task.PriceCheck && (!this._task.SmartCheckout || this._runner.IsSmartCheckoutReady))
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
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': price check didn't PASS (product price: '{this._runner.Product.Price}', minimum: '{this._task.MinimumPrice.ToString()}', maximum: '{this._task.MaximumPrice.ToString()}')");
                        this._runner.Product = null;
                        return false;
                    }
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': price check PASSED (product price: '{this._runner.Product.Price}', minimum: '{this._task.MinimumPrice.ToString()}', maximum: '{this._task.MaximumPrice.ToString()}')");
                }
                if (this._task.HomeUrl.ToLowerInvariant().Contains("kith"))
                {
                    try
                    {
                        this._srr = this._client.Get(url).Text();
                        string key = this._srr.Substring(this._srr.IndexOf("htikb"));
                        key = key.Substring(key.IndexOf("'") + 1);
                        key = key.Substring(0, key.IndexOf("'"));
                        string str12 = this._srr.Substring(this._srr.IndexOf("htikc"));
                        str12 = str12.Substring(str12.IndexOf("'") + 1);
                        str12 = str12.Substring(0, str12.IndexOf("'"));
                        WebsitesInfo.KITH_PROPERTIES = new KeyValuePair<string, string>(key, str12);
                    }
                    catch
                    {
                    }
                }
                if (this._task.HomeUrl.ToLowerInvariant().Contains("funko-shop"))
                {
                    try
                    {
                        this._srr = this._client.Get(url).Text();
                        HtmlDocument document1 = new HtmlDocument();
                        document1.LoadHtml(this._srr);
                        HtmlNode node = document1.DocumentNode.Descendants("input").First<HtmlNode>(x => (x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("properties["));
                        WebsitesInfo.FUNKO_PROPERTIES = new KeyValuePair<string, string>(node.Attributes["name"].Value.Replace("]", "").Replace("properties[", ""), node.Attributes["value"].Value);
                    }
                    catch
                    {
                    }
                }
                if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash-us"))
                {
                    try
                    {
                        this._srr = this._client.Get(url).Text();
                        string str15 = this._srr.Substring(this._srr.IndexOf("atob") + 2);
                        str15 = str15.Substring(str15.IndexOf("atob") + 2);
                        str15 = str15.Substring(str15.IndexOf("'") + 1);
                        string s = str15.Substring(0, str15.IndexOf("'"));
                        str15 = str15.Substring(str15.IndexOf("atob") + 2);
                        str15 = str15.Substring(str15.IndexOf("'") + 1);
                        string str14 = str15.Substring(0, str15.IndexOf("'"));
                        byte[] bytes = Convert.FromBase64String(s);
                        s = Encoding.UTF8.GetString(bytes);
                        s = s.Substring(s.IndexOf("[") + 1);
                        s = s.Substring(0, s.IndexOf("]"));
                        bytes = Convert.FromBase64String(str14);
                        str14 = Encoding.UTF8.GetString(bytes);
                        WebsitesInfo.EFLESH_US_PROPERTIES = new KeyValuePair<string, string>(s, str14);
                    }
                    catch
                    {
                    }
                }
                if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash.") && (!_eflashUkGetter.HasValue || (_eflashUkGetter.Value.AddHours(1.0) < DateTime.Now)))
                {
                    try
                    {
                        this._srr = this._client.Get(url).Text();
                        HtmlDocument document2 = new HtmlDocument();
                        document2.LoadHtml(this._srr);
                        string str18 = "https:" + document2.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["src"] != null) && x.Attributes["src"].Value.Contains("custom.js"))).Attributes["src"].Value;
                        this._srr = this._client.Get(str18).Text();
                        string str17 = this._srr.Substring(this._srr.IndexOf("form.product-form"));
                        str17 = str17.Substring(str17.IndexOf("value"));
                        str17 = str17.Substring(str17.IndexOf("\"") + 1);
                        str17 = str17.Substring(0, str17.IndexOf("\""));
                        string str16 = this._srr.Substring(this._srr.IndexOf("properties"));
                        str16 = str16.Substring(str16.IndexOf("[") + 1);
                        WebsitesInfo.EFLESH_UK_PROPERTIES = new KeyValuePair<string, string>(str16.Substring(0, str16.IndexOf("]")), str17);
                        _eflashUkGetter = new DateTime?(DateTime.Now);
                    }
                    catch
                    {
                    }
                }
                if (!this._task.RandomSize && (!this._task.SmartCheckout || (this._task.SmartCheckout && this._runner.IsSmartCheckoutReady)))
                {
                    char[] separator = new char[] { '#' };
                    string[] strArray = this._task.Size.Split(separator);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray[i] = strArray[i].Trim().ToUpperInvariant();
                    }
                    foreach (string str19 in strArray)
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
                                if (current.Key.Trim().ToUpperInvariant() == str19.ToUpperInvariant())
                                {
                                    goto Label_1C1E;
                                }
                            }
                            continue;
                        Label_1C1E:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(current);
                        }
                    }
                    if (this._runner.PickedSize.HasValue)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': picked size '{this._runner.PickedSize.Value.Key}' (sku: {this._runner.PickedSize.Value.Value})");
                        return true;
                    }
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': defined size not available");
                    if (!this._task.SizePickRandom)
                    {
                        return false;
                    }
                }
                this._runner.PickedSize = new KeyValuePair<string, string>?(this._runner.Product.AvailableSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizes.Count)]);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': picked random size '{this._runner.PickedSize.Value.Key}' (sku: {this._runner.PickedSize.Value.Value})");
                return true;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    string str20 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str20 = " - " + exception.Message;
                    }
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error checking products availability{str20}");
                    Global.Logger.Error($"Error checking products availability of task '{this._task.Name} - {this._task.Guid}'", exception);
                    return false;
                }
                return false;
            }
        }

        private bool DirectLinkYeezy(string link, bool isSource = false)
        {
            try
            {
                string str;
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checking stock availability - {link}");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Accept-Language", "sk-SK,sk;q=0.8,cs;q=0.6,en-US;q=0.4,en;q=0.2");
                request.CookieContainer = this._runner.Cookies;
                request.ServicePoint.Expect100Continue = true;
                request.AllowAutoRedirect = true;
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    str = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(str);
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(document.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-product-json"))).InnerText);
                Product product = new Product();
                if (<>o__42.<>p__2 == null)
                {
                    <>o__42.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                }
                if (<>o__42.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__42.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                }
                if (<>o__42.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__42.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                }
                product.ProductTitle = <>o__42.<>p__2.Target(<>o__42.<>p__2, <>o__42.<>p__1.Target(<>o__42.<>p__1, <>o__42.<>p__0.Target(<>o__42.<>p__0, obj2, "title")));
                product.Link = link;
                this._runner.Product = product;
                if (<>o__42.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__42.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                }
                if (<>o__42.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__42.<>p__5 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                }
                if (<>o__42.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__42.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                }
                if (<>o__42.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__42.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                }
                if (<>o__42.<>p__6.Target(<>o__42.<>p__6, <>o__42.<>p__5.Target(<>o__42.<>p__5, <>o__42.<>p__4.Target(<>o__42.<>p__4, <>o__42.<>p__3.Target(<>o__42.<>p__3, obj2, "available")), true)))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                if (<>o__42.<>p__29 == null)
                {
                    <>o__42.<>p__29 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                }
                if (<>o__42.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__42.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify), argumentInfo));
                }
                foreach (object obj3 in <>o__42.<>p__29.Target(<>o__42.<>p__29, <>o__42.<>p__7.Target(<>o__42.<>p__7, obj2)))
                {
                    if (<>o__42.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__42.<>p__11 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__42.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__42.<>p__10 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__42.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__42.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__42.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__42.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    if (!<>o__42.<>p__11.Target(<>o__42.<>p__11, <>o__42.<>p__10.Target(<>o__42.<>p__10, <>o__42.<>p__9.Target(<>o__42.<>p__9, <>o__42.<>p__8.Target(<>o__42.<>p__8, obj3, "available")), true)))
                    {
                        bool flag2;
                        if (<>o__42.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__42.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                        }
                        if (flag2 = this._task.Link.Contains("shophny.com") || (this._task.ShopifyWebsite == "shophny.com"))
                        {
                            if (<>o__42.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__15 = CallSite<Func<CallSite, bool, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                <>o__42.<>p__14 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__13 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                        }
                        if (!<>o__42.<>p__16.Target(<>o__42.<>p__16, (<>o__42.<>p__12 != null) ? flag2 : <>o__42.<>p__15.Target(<>o__42.<>p__15, flag2, <>o__42.<>p__14.Target(<>o__42.<>p__14, <>o__42.<>p__13.Target(<>o__42.<>p__13, <>o__42.<>p__12.Target(<>o__42.<>p__12, obj3, "inventory_management")), null))))
                        {
                            if (<>o__42.<>p__20 == null)
                            {
                                <>o__42.<>p__20 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__42.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__42.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            this._runner.Product.Price = <>o__42.<>p__20.Target(<>o__42.<>p__20, <>o__42.<>p__19.Target(<>o__42.<>p__19, <>o__42.<>p__18.Target(<>o__42.<>p__18, <>o__42.<>p__17.Target(<>o__42.<>p__17, obj3, "price"))));
                            this._runner.Product.Price = this._runner.Product.Price.Insert(this._runner.Product.Price.Length - 2, ".");
                            if (<>o__42.<>p__28 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__28 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__24 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__42.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__27 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__42.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__42.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__42.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            KeyValuePair<string, string> item = <>o__42.<>p__28.Target(<>o__42.<>p__28, typeof(KeyValuePair<string, string>), <>o__42.<>p__24.Target(<>o__42.<>p__24, <>o__42.<>p__23.Target(<>o__42.<>p__23, <>o__42.<>p__22.Target(<>o__42.<>p__22, <>o__42.<>p__21.Target(<>o__42.<>p__21, obj3, "option1")))), <>o__42.<>p__27.Target(<>o__42.<>p__27, <>o__42.<>p__26.Target(<>o__42.<>p__26, <>o__42.<>p__25.Target(<>o__42.<>p__25, obj3, "id"))));
                            this._runner.Product.AvailableSizes.Add(item);
                        }
                    }
                }
                if (this._runner.Product.AvailableSizes.Count == 0)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': product out of stock");
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
                if (document.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "P__img_bg")))
                {
                    string str2 = "https:" + document.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "P__img_bg"))).Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                    this._task.ImgUrl = str2;
                }
                if (this._task.PriceCheck)
                {
                    string str3 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str3 = str3 + ch.ToString();
                        }
                    }
                    int num2 = int.Parse(str3.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num2 < this._task.MinimumPrice) || (num2 > this._task.MaximumPrice))
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': price check didn't PASS (product price: '{this._runner.Product.Price}', minimum: '{this._task.MinimumPrice.ToString()}', maximum: '{this._task.MaximumPrice.ToString()}')");
                        return false;
                    }
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': price check PASSED (product price: '{this._runner.Product.Price}', minimum: '{this._task.MinimumPrice.ToString()}', maximum: '{this._task.MaximumPrice.ToString()}')");
                }
                if (!this._task.RandomSize)
                {
                    char[] separator = new char[] { ',' };
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
                            goto Label_0FCA;
                        Label_0F54:
                            pair2 = enumerator2.Current;
                            char[] chArray2 = new char[] { ' ' };
                            string[] source = pair2.Key.Split(chArray2);
                            for (int j = 0; j < source.Length; j++)
                            {
                                source[j] = source[j].Trim().ToUpperInvariant();
                            }
                            if (source.Any<string>(x => x == sz))
                            {
                                goto Label_0FD8;
                            }
                        Label_0FCA:
                            if (!enumerator2.MoveNext())
                            {
                                continue;
                            }
                            goto Label_0F54;
                        Label_0FD8:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(pair2);
                        }
                    }
                    if (this._runner.PickedSize.HasValue)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': picked size '{this._runner.PickedSize.Value.Key}' (sku: {this._runner.PickedSize.Value.Value})");
                        return true;
                    }
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': defined size not available");
                    if (!this._task.SizePickRandom)
                    {
                        return false;
                    }
                }
                this._runner.PickedSize = new KeyValuePair<string, string>?(this._runner.Product.AvailableSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizes.Count)]);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': picked random size '{this._runner.PickedSize.Value.Key}' (sku: {this._runner.PickedSize.Value.Value})");
                return true;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    this._runner.IsError = true;
                    string str5 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str5 = " - " + exception.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error checking products availability{str5}");
                    Global.Logger.Error($"Error checking products availability of task '{this._task.Name} - {this._task.Guid}'", exception);
                    return false;
                }
                return false;
            }
        }

        private bool GetShippingRates()
        {
            this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
            States.WriteLogger(this._task, States.LOGGER_STATES.CALCULATING_SHIPPING, null, "", "");
            string str3 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Zip.Trim().Replace(" ", "+");
            string str4 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).State.Trim().Replace(" ", "+");
            string str5 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Country.Trim().Replace(" ", "+");
            string[] textArray1 = new string[] { this._runner.HomeUrl, "/cart/shipping_rates.json?shipping_address%5Bzip%5D=", str3, "&shipping_address%5Bcountry%5D=", str5, "&shipping_address%5Bprovince%5D=", str4 };
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Concat(textArray1));
            if (this._runner.Proxy != null)
            {
                request.Proxy = this._runner.Proxy;
            }
            request.CookieContainer = this._runner.Cookies;
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Accept = "*/*";
            request.Referer = this._runner.CheckoutUrl;
            string str = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        str = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException exception)
            {
                string str2 = "";
                if (exception.Response != null)
                {
                    using (Stream stream = exception.Response.GetResponseStream())
                    {
                        using (StreamReader reader2 = new StreamReader(stream))
                        {
                            str2 = reader2.ReadToEnd();
                        }
                    }
                    if (str2.Contains("is not supported"))
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"your shipping country not supported"}");
                        this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                        return false;
                    }
                }
            }
            object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
            double num2 = 0.0;
            this._pickedShippingName = "";
            this._pickedShippingPrice = "";
            this._pickedShippingSource = "";
            if (<>o__37.<>p__41 == null)
            {
                <>o__37.<>p__41 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
            }
            if (<>o__37.<>p__0 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__37.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "shipping_rates", typeof(Shopify), argumentInfo));
            }
            foreach (object obj3 in <>o__37.<>p__41.Target(<>o__37.<>p__41, <>o__37.<>p__0.Target(<>o__37.<>p__0, obj2)))
            {
                if (string.IsNullOrEmpty(this._pickedShippingName))
                {
                    if (<>o__37.<>p__4 == null)
                    {
                        <>o__37.<>p__4 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                    }
                    if (<>o__37.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    this._pickedShippingName = <>o__37.<>p__4.Target(<>o__37.<>p__4, <>o__37.<>p__3.Target(<>o__37.<>p__3, <>o__37.<>p__2.Target(<>o__37.<>p__2, <>o__37.<>p__1.Target(<>o__37.<>p__1, obj3, "name"))));
                    if (<>o__37.<>p__9 == null)
                    {
                        <>o__37.<>p__9 = CallSite<Func<CallSite, object, double>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify)));
                    }
                    if (<>o__37.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__8 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    num2 = <>o__37.<>p__9.Target(<>o__37.<>p__9, <>o__37.<>p__8.Target(<>o__37.<>p__8, typeof(double), <>o__37.<>p__7.Target(<>o__37.<>p__7, <>o__37.<>p__6.Target(<>o__37.<>p__6, <>o__37.<>p__5.Target(<>o__37.<>p__5, obj3, "price"))), CultureInfo.InvariantCulture));
                    if (<>o__37.<>p__13 == null)
                    {
                        <>o__37.<>p__13 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                    }
                    if (<>o__37.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    this._pickedShippingPrice = <>o__37.<>p__13.Target(<>o__37.<>p__13, <>o__37.<>p__12.Target(<>o__37.<>p__12, <>o__37.<>p__11.Target(<>o__37.<>p__11, <>o__37.<>p__10.Target(<>o__37.<>p__10, obj3, "price"))));
                    if (<>o__37.<>p__17 == null)
                    {
                        <>o__37.<>p__17 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                    }
                    if (<>o__37.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    this._pickedShippingSource = <>o__37.<>p__17.Target(<>o__37.<>p__17, <>o__37.<>p__16.Target(<>o__37.<>p__16, <>o__37.<>p__15.Target(<>o__37.<>p__15, <>o__37.<>p__14.Target(<>o__37.<>p__14, obj3, "source"))));
                }
                else
                {
                    if (<>o__37.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__21 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    object obj4 = <>o__37.<>p__21.Target(<>o__37.<>p__21, typeof(double), <>o__37.<>p__20.Target(<>o__37.<>p__20, <>o__37.<>p__19.Target(<>o__37.<>p__19, <>o__37.<>p__18.Target(<>o__37.<>p__18, obj3, "price"))), CultureInfo.InvariantCulture);
                    if (<>o__37.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__23 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__22 = CallSite<Func<CallSite, object, double, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Shopify), argumentInfo));
                    }
                    if (<>o__37.<>p__23.Target(<>o__37.<>p__23, <>o__37.<>p__22.Target(<>o__37.<>p__22, obj4, num2)))
                    {
                        if (<>o__37.<>p__27 == null)
                        {
                            <>o__37.<>p__27 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                        }
                        if (<>o__37.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        this._pickedShippingName = <>o__37.<>p__27.Target(<>o__37.<>p__27, <>o__37.<>p__26.Target(<>o__37.<>p__26, <>o__37.<>p__25.Target(<>o__37.<>p__25, <>o__37.<>p__24.Target(<>o__37.<>p__24, obj3, "name"))));
                        if (<>o__37.<>p__32 == null)
                        {
                            <>o__37.<>p__32 = CallSite<Func<CallSite, object, double>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify)));
                        }
                        if (<>o__37.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__31 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        num2 = <>o__37.<>p__32.Target(<>o__37.<>p__32, <>o__37.<>p__31.Target(<>o__37.<>p__31, typeof(double), <>o__37.<>p__30.Target(<>o__37.<>p__30, <>o__37.<>p__29.Target(<>o__37.<>p__29, <>o__37.<>p__28.Target(<>o__37.<>p__28, obj3, "price"))), CultureInfo.InvariantCulture));
                        if (<>o__37.<>p__36 == null)
                        {
                            <>o__37.<>p__36 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                        }
                        if (<>o__37.<>p__35 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__34 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        this._pickedShippingPrice = <>o__37.<>p__36.Target(<>o__37.<>p__36, <>o__37.<>p__35.Target(<>o__37.<>p__35, <>o__37.<>p__34.Target(<>o__37.<>p__34, <>o__37.<>p__33.Target(<>o__37.<>p__33, obj3, "price"))));
                        if (<>o__37.<>p__40 == null)
                        {
                            <>o__37.<>p__40 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                        }
                        if (<>o__37.<>p__39 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__38 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__37.<>p__37 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        this._pickedShippingSource = <>o__37.<>p__40.Target(<>o__37.<>p__40, <>o__37.<>p__39.Target(<>o__37.<>p__39, <>o__37.<>p__38.Target(<>o__37.<>p__38, <>o__37.<>p__37.Target(<>o__37.<>p__37, obj3, "source"))));
                    }
                }
            }
            return true;
        }

        public void Init(bool all = true)
        {
            this._isInitError = false;
            Task.Factory.StartNew(delegate {
                try
                {
                    this._isQueue = false;
                    if ((all && !this._task.IsShopifyCheckoutLink) && (!this._runner._tokenTimestamp.HasValue || (this._runner._tokenTimestamp.Value.AddHours(3.0) < DateTime.Now)))
                    {
                        this._isCaptcha = false;
                        this._runner._tokenTimestamp = null;
                        this._runner.Cookies = new CookieContainer();
                        this.SetClient();
                        this._checkoutLink = "";
                        Dictionary<string, string> data = new Dictionary<string, string> {
                            { 
                                "checkout",
                                "Checkout"
                            },
                            { 
                                "note",
                                ""
                            },
                            { 
                                "1381190565893",
                                "1"
                            }
                        };
                        this._client.Handler.AllowAutoRedirect = false;
                        HttpResponseMessage response = this._client.Post(this._task.HomeUrl + "/checkout.json", data);
                        EveAIO.Extensions.CheckError(response);
                        string html = response.Content.ReadAsStringAsync().Result.ToString();
                        string uriString = "";
                        if (response.Headers.Any<KeyValuePair<string, IEnumerable<string>>>(x => x.Key == "Location"))
                        {
                            uriString = response.Headers.First<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "Location")).Value.First<string>();
                        }
                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(html);
                        this._checkoutLink = document.DocumentNode.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                        if (uriString.Contains("throttle"))
                        {
                            html = "{}";
                            this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                            Global.Logger.Info($"['{this._task.Name} - {this._task.Guid}']: {"Waiting in queue"}");
                            while (html.Contains("{}"))
                            {
                                this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                                response = this._client.Get(uriString);
                                EveAIO.Extensions.CheckError(response);
                                html = response.Content.ReadAsStringAsync().Result.ToString();
                                if (response.Headers.Any<KeyValuePair<string, IEnumerable<string>>>(x => x.Key == "Location"))
                                {
                                    uriString = response.Headers.First<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "Location")).Value.First<string>();
                                }
                                if (html.ToLowerInvariant().Contains("checkout queue"))
                                {
                                    Thread.Sleep(0x7d0);
                                }
                            }
                        }
                        this._runner._tokenTimestamp = new DateTime?(DateTime.Now);
                        this._checkoutLink = uriString;
                    }
                    if (this._task.IsShopifyCheckoutLink)
                    {
                        this._checkoutLink = this._task.ShopifyCheckoutLink;
                    }
                    if (string.IsNullOrEmpty(this._cardId))
                    {
                        if (this._task.Payment == TaskObject.PaymentEnum.paypal)
                        {
                            this._cardId = "-";
                        }
                        else
                        {
                            string str3 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CCNumber.Trim().Replace(" ", "");
                            string str4 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).NameOnCard.Trim();
                            string expiryMonth = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).ExpiryMonth;
                            if (expiryMonth[0] == '0')
                            {
                                expiryMonth = expiryMonth.Substring(1);
                            }
                            string expiryYear = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).ExpiryYear;
                            string str7 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Cvv.Trim();
                            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://elb.deposit.shopifycs.com/sessions");
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.Method = "POST";
                            request.CookieContainer = this._runner.Cookies;
                            request.KeepAlive = true;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                            request.ContentType = "application/json";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                            request.Accept = "application/json";
                            request.Headers.Add("Origin", "https://checkout.shopify.com");
                            request.Referer = "https://checkout.shopifycs.com/number";
                            request.AllowAutoRedirect = true;
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            string[] textArray1 = new string[] { "{\"credit_card\":{\"number\":\"", str3, "\",\"name\":\"", str4, "\",\"month\":", expiryMonth, ",\"year\":", expiryYear, ",\"verification_value\":\"", str7, "\"}}" };
                            string s = string.Concat(textArray1);
                            byte[] bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream = request.GetRequestStream())
                            {
                                stream.Write(bytes, 0, bytes.Length);
                            }
                            string str9 = "";
                            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                            {
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    str9 = reader.ReadToEnd();
                                }
                            }
                            object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str9);
                            if (<>o__25.<>p__3 == null)
                            {
                                <>o__25.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__25.<>p__2 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__25.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__25.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__25.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__25.<>p__0 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__25.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            this._cardId = <>o__25.<>p__3.Target(<>o__25.<>p__3, <>o__25.<>p__2.Target(<>o__25.<>p__2, <>o__25.<>p__1.Target(<>o__25.<>p__1, <>o__25.<>p__0.Target(<>o__25.<>p__0, obj2, "id"))));
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                    {
                        this._isInitError = true;
                        Global.Logger.Error($"Error during task init '{this._task.Name} - {this._task.Guid}'", exception);
                    }
                }
            });
        }

        public bool Login()
        {
            if ((this._task.Link.Contains("/cart/") && this._task.Link.Contains(":1")) || (this._task.TaskType == TaskObject.TaskTypeEnum.variant))
            {
                this.Init(true);
            }
            while (!string.IsNullOrEmpty(this._cardId))
            {
                if (!string.IsNullOrEmpty(this._checkoutLink))
                {
                    goto Label_00B0;
                }
            Label_0054:
                if (this._isInitError)
                {
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error during task init");
                    return false;
                }
                Thread.Sleep(200);
            }
            goto Label_0054;
        Label_00B0:
            if (this._isLoggedIn)
            {
                return true;
            }
            try
            {
                this.SetClient();
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': logging in with '{this._task.Username}'");
                }
                this._diData.Clear();
                this._diData.Add("form_type", "customer_login");
                this._diData.Add("utf8", "✓");
                this._diData.Add("customer[email]", this._task.Username);
                this._diData.Add("customer[password]", this._task.Password);
                this._srr = this._client.Post(this._runner.HomeUrl + "/account/login", this._diData).Text();
                HtmlNode.ElementsFlags.Remove("form");
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(this._srr);
                if ((!document.DocumentNode.Descendants("a").Any<HtmlNode>(delegate (HtmlNode x) {
                    if (!x.InnerHtml.ToUpperInvariant().Contains("Logged in as".ToUpperInvariant()) && !x.InnerHtml.ToUpperInvariant().Contains("Logout".ToUpperInvariant()))
                    {
                        return x.InnerHtml.ToUpperInvariant().Contains("Log out".ToUpperInvariant());
                    }
                    return true;
                }) && !document.DocumentNode.Descendants("h5").Any<HtmlNode>(x => (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "name")) && !string.IsNullOrEmpty(x.InnerText)))) && !this._srr.ToLowerInvariant().Contains("my account"))
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': login unsuccessful");
                    return false;
                }
                this._isLoggedIn = true;
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': login successful");
                return true;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    string str = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str = " - " + exception.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    Global.Logger.Error($"Error while loging for task '{this._task.Name} - {this._task.Guid}'", exception);
                    this._runner.IsError = true;
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error login in{str}");
                    return false;
                }
                return false;
            }
        }

        private void ParseProductPage(string website, Product product)
        {
            try
            {
                string str;
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(website);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Accept-Language", "sk-SK,sk;q=0.8,cs;q=0.6,en-US;q=0.4,en;q=0.2");
                request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    str = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                HtmlNode.ElementsFlags.Remove("form");
                HtmlNode.ElementsFlags.Remove("option");
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(str);
                this._runner.ProductPageHtml = document;
                if (document.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productSelect")))
                {
                    foreach (HtmlNode local1 in from x in document.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productSelect"))).Descendants("option")
                        where x.Attributes["value"] > null
                        select x)
                    {
                        string innerText = local1.InnerText;
                        if (website.ToLowerInvariant().Contains("trophyroomstore"))
                        {
                            innerText = innerText.Substring(0, innerText.IndexOf(" "));
                        }
                        string str3 = local1.Attributes["value"].Value;
                        list.Add(new KeyValuePair<string, string>(innerText, str3));
                    }
                }
                else if (document.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-select")))
                {
                    if (str.Contains("OptionSelectors"))
                    {
                        string str4 = str.Substring(str.IndexOf("Shopify.OptionSelectors"));
                        str4 = str4.Substring(str4.IndexOf("product:"));
                        str4 = str4.Substring(str4.IndexOf("{"));
                        str4 = str4.Substring(0, str4.IndexOf("onVariantSelected") + 1);
                        object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str4.Substring(0, str4.LastIndexOf("},") + 1));
                        if (<>o__43.<>p__14 == null)
                        {
                            <>o__43.<>p__14 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                        }
                        if (<>o__43.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify), argumentInfo));
                        }
                        foreach (object obj3 in <>o__43.<>p__14.Target(<>o__43.<>p__14, <>o__43.<>p__0.Target(<>o__43.<>p__0, obj2)))
                        {
                            if (<>o__43.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__5 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__4 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__3 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__2 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__6.Target(<>o__43.<>p__6, <>o__43.<>p__5.Target(<>o__43.<>p__5, <>o__43.<>p__4.Target(<>o__43.<>p__4, typeof(int), <>o__43.<>p__3.Target(<>o__43.<>p__3, <>o__43.<>p__2.Target(<>o__43.<>p__2, <>o__43.<>p__1.Target(<>o__43.<>p__1, obj3, "inventory_quantity")))), 0)))
                            {
                                if (<>o__43.<>p__13 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__13 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__9 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__8 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__7 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__12 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__11 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__10 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                list.Add(<>o__43.<>p__13.Target(<>o__43.<>p__13, typeof(KeyValuePair<string, string>), <>o__43.<>p__9.Target(<>o__43.<>p__9, <>o__43.<>p__8.Target(<>o__43.<>p__8, <>o__43.<>p__7.Target(<>o__43.<>p__7, obj3, "option1"))), <>o__43.<>p__12.Target(<>o__43.<>p__12, <>o__43.<>p__11.Target(<>o__43.<>p__11, <>o__43.<>p__10.Target(<>o__43.<>p__10, obj3, "id")))));
                            }
                        }
                    }
                    else
                    {
                        foreach (HtmlNode local2 in from x in document.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-select"))).Descendants("option")
                            where x.Attributes["value"] > null
                            select x)
                        {
                            string innerText = local2.InnerText;
                            string str6 = local2.Attributes["value"].Value;
                            list.Add(new KeyValuePair<string, string>(innerText, str6));
                        }
                    }
                }
                else
                {
                    IEnumerator<HtmlNode> enumerator;
                    if (document.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("swatch"))) && !this._task.HomeUrl.Contains("undefeated"))
                    {
                        string str7 = str.Substring(str.IndexOf("var meta = ") + 11);
                        object obj4 = Newtonsoft.Json.JsonConvert.DeserializeObject(Regex.Replace(str7.Substring(0, str7.IndexOf(";")), "(\\d+\\/\\d+)\"", "$1\\\""));
                        using (enumerator = (from x in document.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("swatch"))).Descendants("div")
                            where (x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("available")
                            select x).GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                string str8 = enumerator.Current.Attributes["data-value"].Value.ToString();
                                if (<>o__43.<>p__25 == null)
                                {
                                    <>o__43.<>p__25 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                                }
                                if (<>o__43.<>p__16 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__15 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "product", typeof(Shopify), argumentInfo));
                                }
                                using (IEnumerator enumerator2 = <>o__43.<>p__25.Target(<>o__43.<>p__25, <>o__43.<>p__16.Target(<>o__43.<>p__16, <>o__43.<>p__15.Target(<>o__43.<>p__15, obj4))).GetEnumerator())
                                {
                                    object obj5;
                                    goto Label_0C20;
                                Label_0AD9:
                                    obj5 = enumerator2.Current;
                                    if (<>o__43.<>p__20 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__20 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__19 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__43.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__18 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__17 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__43.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__20.Target(<>o__43.<>p__20, <>o__43.<>p__19.Target(<>o__43.<>p__19, <>o__43.<>p__18.Target(<>o__43.<>p__18, <>o__43.<>p__17.Target(<>o__43.<>p__17, obj5, "public_title")), str8)))
                                    {
                                        goto Label_0C31;
                                    }
                                Label_0C20:
                                    if (!enumerator2.MoveNext())
                                    {
                                        continue;
                                    }
                                    goto Label_0AD9;
                                Label_0C31:
                                    if (<>o__43.<>p__24 == null)
                                    {
                                        <>o__43.<>p__24 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                                    }
                                    if (<>o__43.<>p__23 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__22 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__21 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__43.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    string str9 = <>o__43.<>p__24.Target(<>o__43.<>p__24, <>o__43.<>p__23.Target(<>o__43.<>p__23, <>o__43.<>p__22.Target(<>o__43.<>p__22, <>o__43.<>p__21.Target(<>o__43.<>p__21, obj5, "id"))));
                                    list.Add(new KeyValuePair<string, string>(str8, str9));
                                    continue;
                                }
                            }
                            goto Label_257F;
                        }
                    }
                    if (this._task.HomeUrl.Contains("xhibition"))
                    {
                        string str10 = str.Substring(str.IndexOf("var amastyXnotifConfig = {"));
                        str10 = str10.Substring(str10.IndexOf("{"));
                        object obj6 = Newtonsoft.Json.JsonConvert.DeserializeObject(str10.Substring(0, str10.IndexOf("};") + 1).Replace("product:", "\"product\":").Replace("customer_id:", "\"customer_id\":").Replace(@"\/", "v"));
                        if (<>o__43.<>p__38 == null)
                        {
                            <>o__43.<>p__38 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                        }
                        if (<>o__43.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__43.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "product", typeof(Shopify), argumentInfo));
                        }
                        foreach (object obj7 in <>o__43.<>p__38.Target(<>o__43.<>p__38, <>o__43.<>p__27.Target(<>o__43.<>p__27, <>o__43.<>p__26.Target(<>o__43.<>p__26, obj6))))
                        {
                            if (<>o__43.<>p__31 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__31 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__30 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__30 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__29 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__28 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__31.Target(<>o__43.<>p__31, <>o__43.<>p__30.Target(<>o__43.<>p__30, <>o__43.<>p__29.Target(<>o__43.<>p__29, <>o__43.<>p__28.Target(<>o__43.<>p__28, obj7, "inventory_quantity")), 0)))
                            {
                                if (<>o__43.<>p__37 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__37 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__33 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__32 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__32 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__36 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__35 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__34 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                list.Add(<>o__43.<>p__37.Target(<>o__43.<>p__37, typeof(KeyValuePair<string, string>), <>o__43.<>p__33.Target(<>o__43.<>p__33, <>o__43.<>p__32.Target(<>o__43.<>p__32, obj7, "title")), <>o__43.<>p__36.Target(<>o__43.<>p__36, <>o__43.<>p__35.Target(<>o__43.<>p__35, <>o__43.<>p__34.Target(<>o__43.<>p__34, obj7, "id")))));
                            }
                        }
                    }
                    else if (!str.Contains("KANYE.p.variants.push("))
                    {
                        if (document.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select-")))
                        {
                            foreach (HtmlNode node in from x in document.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select-"))).Descendants("option")
                                where x.Attributes["value"] > null
                                select x)
                            {
                                string innerText = node.InnerText;
                                if (!innerText.ToUpper().Contains("sold out".ToUpper()))
                                {
                                    string str12 = node.Attributes["value"].Value;
                                    list.Add(new KeyValuePair<string, string>(innerText, str12));
                                }
                            }
                        }
                        else if (str.Contains("window.products[\""))
                        {
                            string str14 = str.Substring(str.IndexOf("window.products[\""));
                            str14 = str14.Substring(str14.IndexOf("{"));
                            object obj9 = Newtonsoft.Json.JsonConvert.DeserializeObject(str14.Substring(0, str14.IndexOf("};") + 1));
                            if (<>o__43.<>p__59 == null)
                            {
                                <>o__43.<>p__59 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                            }
                            if (<>o__43.<>p__49 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify), argumentInfo));
                            }
                            foreach (object obj10 in <>o__43.<>p__59.Target(<>o__43.<>p__59, <>o__43.<>p__49.Target(<>o__43.<>p__49, obj9)))
                            {
                                if (<>o__43.<>p__52 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__52 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__51 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__50 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__50 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__52.Target(<>o__43.<>p__52, <>o__43.<>p__51.Target(<>o__43.<>p__51, <>o__43.<>p__50.Target(<>o__43.<>p__50, obj10, "available"))))
                                {
                                    if (<>o__43.<>p__58 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__58 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__54 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__53 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__43.<>p__53 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__57 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__56 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__55 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__43.<>p__55 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    list.Add(<>o__43.<>p__58.Target(<>o__43.<>p__58, typeof(KeyValuePair<string, string>), <>o__43.<>p__54.Target(<>o__43.<>p__54, <>o__43.<>p__53.Target(<>o__43.<>p__53, obj10, "option2")), <>o__43.<>p__57.Target(<>o__43.<>p__57, <>o__43.<>p__56.Target(<>o__43.<>p__56, <>o__43.<>p__55.Target(<>o__43.<>p__55, obj10, "id")))));
                                }
                            }
                        }
                        else if (document.DocumentNode.Descendants("script").Any<HtmlNode>(x => (x.Attributes["Id"] != null) && (x.Attributes["Id"].Value == "ProductJson-product-template")))
                        {
                            object obj11 = Newtonsoft.Json.JsonConvert.DeserializeObject(document.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["Id"] != null) && (x.Attributes["Id"].Value == "ProductJson-product-template"))).InnerText.Trim());
                            if (<>o__43.<>p__74 == null)
                            {
                                <>o__43.<>p__74 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                            }
                            if (<>o__43.<>p__60 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify), argumentInfo));
                            }
                            foreach (object obj12 in <>o__43.<>p__74.Target(<>o__43.<>p__74, <>o__43.<>p__60.Target(<>o__43.<>p__60, obj11)))
                            {
                                if (<>o__43.<>p__66 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__66 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__65 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__65 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__64 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__64 = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__63 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__63 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__62 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__61 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__61 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__66.Target(<>o__43.<>p__66, <>o__43.<>p__65.Target(<>o__43.<>p__65, <>o__43.<>p__64.Target(<>o__43.<>p__64, typeof(int), <>o__43.<>p__63.Target(<>o__43.<>p__63, <>o__43.<>p__62.Target(<>o__43.<>p__62, <>o__43.<>p__61.Target(<>o__43.<>p__61, obj12, "inventory_quantity")))), 0)))
                                {
                                    if (<>o__43.<>p__73 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__73 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__69 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__69 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__68 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__68 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__67 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__43.<>p__67 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__72 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__72 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__71 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__43.<>p__71 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__43.<>p__70 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__43.<>p__70 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                    }
                                    list.Add(<>o__43.<>p__73.Target(<>o__43.<>p__73, typeof(KeyValuePair<string, string>), <>o__43.<>p__69.Target(<>o__43.<>p__69, <>o__43.<>p__68.Target(<>o__43.<>p__68, <>o__43.<>p__67.Target(<>o__43.<>p__67, obj12, "option1"))), <>o__43.<>p__72.Target(<>o__43.<>p__72, <>o__43.<>p__71.Target(<>o__43.<>p__71, <>o__43.<>p__70.Target(<>o__43.<>p__70, obj12, "id")))));
                                }
                            }
                        }
                        else if (document.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "id")))
                        {
                            foreach (HtmlNode node2 in document.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "id"))).Descendants("option"))
                            {
                                string key = node2.InnerText.Trim();
                                if (this._task.HomeUrl.ToLower().Contains("bimtoy.com") && key.Contains("-"))
                                {
                                    key = key.Substring(0, key.IndexOf("-")).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, node2.Attributes["value"].Value));
                            }
                        }
                        else if (!document.DocumentNode.Descendants("form").Any<HtmlNode>(x => ((x.Attributes["data-product-id"] != null) && !string.IsNullOrEmpty(x.Attributes["data-product-id"].Value))))
                        {
                            if (document.DocumentNode.Descendants("a").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "selectedSKU")))
                            {
                                using (enumerator = (from x in document.DocumentNode.Descendants("a")
                                    where (x.Attributes["onClick"] != null) && x.Attributes["onClick"].Value.Contains("assignSKU")
                                    select x).GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        string text1 = enumerator.Current.Attributes["onClick"].Value;
                                        string str17 = text1.Substring(text1.IndexOf("(") + 1);
                                        str17 = str17.Substring(0, str17.IndexOf(","));
                                        string key = text1.Substring(text1.IndexOf("'") + 1);
                                        key = key.Substring(0, key.IndexOf(":")).Trim();
                                        list.Add(new KeyValuePair<string, string>(key, str17));
                                    }
                                    goto Label_257F;
                                }
                            }
                            if (!document.DocumentNode.Descendants("select").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select"))))
                            {
                                if (document.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("selector")))
                                {
                                    foreach (HtmlNode node4 in from x in document.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("selector"))).Descendants("input")
                                        where (x.Attributes["disabled"] == null) && (x.Attributes["disabled/"] == null)
                                        select x)
                                    {
                                        list.Add(new KeyValuePair<string, string>(node4.NextSibling.NextSibling.InnerText.Trim(), node4.Attributes["value"].Value));
                                    }
                                }
                            }
                            else
                            {
                                document.DocumentNode.Descendants("select").First<HtmlNode>(x => (x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select"));
                            }
                        }
                        else
                        {
                            HtmlNode node3 = document.DocumentNode.Descendants("form").First<HtmlNode>(x => (x.Attributes["data-product-id"] != null) && !string.IsNullOrEmpty(x.Attributes["data-product-id"].Value));
                            list.Add(new KeyValuePair<string, string>("-", node3.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "id"))).Attributes["value"].Value));
                        }
                    }
                    else
                    {
                        string str11 = str;
                        while (str11.Contains("KANYE.p.variants.push("))
                        {
                            str11 = str11.Substring(str11.IndexOf("KANYE.p.variants.push(") + 5);
                            str11 = str11.Substring(str11.IndexOf("(") + 1);
                            object obj8 = Newtonsoft.Json.JsonConvert.DeserializeObject(str11.Substring(0, str11.IndexOf(")")));
                            if (<>o__43.<>p__42 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__42 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__41 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__41 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__40 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__39 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__39 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__43.<>p__42.Target(<>o__43.<>p__42, <>o__43.<>p__41.Target(<>o__43.<>p__41, <>o__43.<>p__40.Target(<>o__43.<>p__40, <>o__43.<>p__39.Target(<>o__43.<>p__39, obj8, "available")), true)))
                            {
                                if (<>o__43.<>p__48 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__48 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__44 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__43 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__47 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__46 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__43.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                }
                                if (<>o__43.<>p__45 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__43.<>p__45 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                                }
                                list.Add(<>o__43.<>p__48.Target(<>o__43.<>p__48, typeof(KeyValuePair<string, string>), <>o__43.<>p__44.Target(<>o__43.<>p__44, <>o__43.<>p__43.Target(<>o__43.<>p__43, obj8, "option1")), <>o__43.<>p__47.Target(<>o__43.<>p__47, <>o__43.<>p__46.Target(<>o__43.<>p__46, <>o__43.<>p__45.Target(<>o__43.<>p__45, obj8, "id")))));
                            }
                        }
                    }
                }
            Label_257F:
                product.AvailableSizes = list;
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("Thread was being aborted") || ((exception.InnerException != null) && exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    throw;
                }
                this._runner.IsError = true;
                string str18 = "";
                if (exception.GetType() == typeof(WebException))
                {
                    str18 = " - " + exception.Message;
                }
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error parsing product page{str18}");
                Global.Logger.Error($"Error parsing product page for task '{this._task.Name} - {this._task.Guid}'", exception);
                throw;
            }
        }

        private bool PaymentMethod(string responseString, bool smartCheckoutPrep = false, bool forcePayPal = false, bool finishSmartCheckout = false)
        {
            bool flag;
            if (smartCheckoutPrep)
            {
                return true;
            }
            HttpWebRequest request = null;
            string format = "";
            HtmlDocument document = new HtmlDocument();
            string html = "";
            if (!finishSmartCheckout)
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
            }
            else
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
            }
            try
            {
                if (finishSmartCheckout)
                {
                    request = (HttpWebRequest) WebRequest.Create(this._checkoutLink + "?previous_step=shipping_method&step=payment_method ");
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.AllowAutoRedirect = false;
                    request.CookieContainer = this._runner.Cookies;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                    {
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    }
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.KeepAlive = true;
                    request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    request.Headers.Add("Cache-Control", "max-age=0");
                    responseString = "";
                    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = reader.ReadToEnd();
                        }
                    }
                }
                if (string.IsNullOrEmpty(responseString))
                {
                    responseString = this._runner.CheckoutHtml.DocumentNode.InnerHtml;
                }
                this._runner.ForcedPaypal = forcePayPal;
                Uri.EscapeDataString(Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Email.Trim());
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).FirstName.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).LastName.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address1.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address2.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).City.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Zip.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).State.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Phone.Trim().Replace(" ", "+");
                Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Country.Trim().Replace(" ", "+");
                document.LoadHtml(responseString);
                if (responseString.Contains("All major debit/credit cards"))
                {
                    goto Label_1814;
                }
                HtmlNode node = (from x in document.DocumentNode.Descendants("form")
                    where x.Attributes["data-payment-form"] > null
                    select x).First<HtmlNode>();
                string text1 = node.Attributes["action"].Value;
                string stringToEscape = node.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"))).Attributes["value"].Value;
                string str3 = "";
                if (node.Descendants("input").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "checkout_total_price")))
                {
                    str3 = node.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "checkout_total_price"))).Attributes["value"].Value;
                }
                string str10 = "";
                if (!node.Descendants("div").Any<HtmlNode>(x => (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "direct"))) && !node.Descendants("div").Any<HtmlNode>(x => (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "express"))))
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': payment gateway not found");
                    return false;
                }
                if ((this._task.Payment == TaskObject.PaymentEnum.creditcard) && !forcePayPal)
                {
                    if (!node.Descendants("div").Any<HtmlNode>(x => (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "direct"))))
                    {
                        str10 = node.Descendants("div").First<HtmlNode>(x => (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "express"))).Attributes["data-select-gateway"].Value;
                        forcePayPal = true;
                        this._runner.ForcedPaypal = true;
                        EveAIO.Helpers.WriteLog($"task '{this._task.Name}': credit card payment not available. Paypal is possible. Manual checkout needed");
                    }
                    else
                    {
                        str10 = node.Descendants("div").First<HtmlNode>(x => (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "direct"))).Attributes["data-select-gateway"].Value;
                    }
                }
                else
                {
                    str10 = node.Descendants("div").First<HtmlNode>(x => (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "express"))).Attributes["data-select-gateway"].Value;
                }
                request = (HttpWebRequest) WebRequest.Create(this._checkoutLink);
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                string str4 = "";
                string str5 = "";
                string str8 = "";
                string str7 = "";
                string str6 = "";
                string str11 = "";
                string str12 = "";
                string str9 = "";
                if (!Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).SameBillingShipping)
                {
                    Uri.EscapeDataString(Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).EmailShipping.Trim());
                    str4 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).FirstNameShipping.Trim().Replace(" ", "+");
                    str5 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).LastNameShipping.Trim().Replace(" ", "+");
                    str8 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address1Shipping.Trim().Replace(" ", "+");
                    str7 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address2Shipping.Trim().Replace(" ", "+");
                    str6 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CityShipping.Trim().Replace(" ", "+");
                    str11 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).ZipShipping.Trim().Replace(" ", "+");
                    str12 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).StateShipping.Trim().Replace(" ", "+");
                    Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).PhoneShipping.Trim().Replace(" ", "+");
                    str9 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CountryShipping.Trim().Replace(" ", "+");
                }
                format = "utf8=%e2%9c%93";
                format = ((((format + "&_method=patch") + "&authenticity_token=" + Uri.EscapeDataString(stringToEscape)) + "&previous_step=payment_method") + "&checkout%5bpayment_gateway%5d=" + str10) + "&checkout%5bcredit_card%5d%5bvault%5d=false";
                while (string.IsNullOrEmpty(this._cardId))
                {
                    if (this._isInitError)
                    {
                        goto Label_17D3;
                    }
                    Thread.Sleep(200);
                }
                if (Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).SameBillingShipping)
                {
                    format = format + "&checkout%5bdifferent_billing_address%5d=false";
                }
                else
                {
                    format = ((((((((((((((((format + "&checkout%5Bdifferent_billing_address%5D=true" + "&checkout%5Bbilling_address%5D%5Bfirst_name%5D=") + "&checkout%5Bbilling_address%5D%5Bfirst_name%5D=" + str4) + "&checkout%5Bbilling_address%5D%5Blast_name%5D=") + "&checkout%5Bbilling_address%5D%5Blast_name%5D=" + str5) + "&checkout%5Bbilling_address%5D%5Bcompany%5D=") + "&checkout%5Bbilling_address%5D%5Bcompany%5D=" + "&checkout%5Bbilling_address%5D%5Baddress1%5D=") + "&checkout%5Bbilling_address%5D%5Baddress1%5D=" + str8) + "&checkout%5Bbilling_address%5D%5Baddress2%5D=") + "&checkout%5Bbilling_address%5D%5Baddress2%5D=" + str7) + "&checkout%5Bbilling_address%5D%5Bcity%5D=") + "&checkout%5Bbilling_address%5D%5Bcity%5D=" + str6) + "&checkout%5Bbilling_address%5D%5Bcountry%5D=") + "&checkout%5Bbilling_address%5D%5Bcountry%5D=" + str9) + "&checkout%5Bbilling_address%5D%5Bprovince%5D=") + "&checkout%5Bbilling_address%5D%5Bprovince%5D=" + str12) + "&checkout%5Bbilling_address%5D%5Bzip%5D=") + "&checkout%5Bbilling_address%5D%5Bzip%5D=" + str11;
                }
                format = ((((format + "&checkout%5btotal_price%5d=" + str3) + "&complete=1") + "&checkout%5bbuyer_accepts_marketing%5d=0" + "&checkout%5bclient_details%5d%5bbrowser_width%5d=2016") + "&checkout%5bclient_details%5d%5bbrowser_height%5d=481" + "&checkout%5bclient_details%5d%5bjavascript_enabled%5d=1") + "&s=" + this._cardId;
                request.Method = "POST";
                request.CookieContainer = this._runner.Cookies;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                }
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                if ((this._task.Payment == TaskObject.PaymentEnum.paypal) | forcePayPal)
                {
                    request.ServicePoint.Expect100Continue = false;
                }
                if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checkout profile already used for this website");
                    return false;
                }
                if (this._isCaptcha && this._task.SmartCheckout)
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    this._task.Mre = new ManualResetEvent(false);
                    string link = "";
                    if (!this._task.Link.ToLowerInvariant().Contains("/checkouts/") && !this._task.IsShopifyCheckoutLink)
                    {
                        if ((this._task.TaskType == TaskObject.TaskTypeEnum.variant) || (this._task.Link.ToLowerInvariant().Contains("/cart/") && this._task.Link.ToLowerInvariant().Contains(":1")))
                        {
                            link = this._task.Link;
                        }
                        else
                        {
                            link = this._runner.Product.Link;
                        }
                    }
                    else
                    {
                        link = this._runner.HomeUrl;
                    }
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), this._captchaKey, link, "Shopify");
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
                    format = format + "&g-recaptcha-response=" + WebUtility.UrlEncode(waiter.Token);
                }
                request.ContentLength = format.Length;
                if (this._task.CheckoutDelay > 0)
                {
                    if (Global.SETTINGS.DetailedLog)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"checkout delay turned on, waiting " + this._task.CheckoutDelay + " ms"}");
                    }
                    Thread.Sleep(this._task.CheckoutDelay);
                }
                using (Stream stream = request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(format, format.Length);
                    }
                }
                html = "";
                string arguments = "";
                if ((this._task.Payment == TaskObject.PaymentEnum.creditcard) && !forcePayPal)
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': submiting order");
                }
                if (Global.SERIAL == "EVE-1111111111111")
                {
                    goto Label_17B0;
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
                using (HttpWebResponse response2 = (HttpWebResponse) request.GetResponse())
                {
                    arguments = response2.ResponseUri.ToString();
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        html = reader2.ReadToEnd();
                    }
                }
                if (html.ToLowerInvariant().Contains("inventory issues"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                if ((this._task.Payment == TaskObject.PaymentEnum.paypal) | forcePayPal)
                {
                    if (Global.SETTINGS.PayPalBeep)
                    {
                        EveAIO.Helpers.PlayBell();
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.OPENING_PAYPAL);
                    States.WriteLogger(this._task, States.LOGGER_STATES.OPENING_PAYPAL, null, "", "");
                    if (this._runner._notificator != null)
                    {
                        this._runner._notificator.Notify(Notificator.NotificationType.Paypal);
                    }
                    this._task.PaypalLink = arguments;
                    Process.Start("chrome.exe", arguments);
                    return true;
                }
                document.LoadHtml(html);
                if (html.Contains("The total of your order has changed. "))
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': The total of your order has changed");
                    return false;
                }
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': waiting for order");
                goto Label_12B7;
            Label_117C:
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                Thread.Sleep(0x3e8);
                request = (HttpWebRequest) WebRequest.Create(this._runner.CheckoutUrl + "/processing");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                }
                request.Accept = "*/*";
                html = "";
                using (HttpWebResponse response3 = (HttpWebResponse) request.GetResponse())
                {
                    using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                    {
                        html = reader3.ReadToEnd();
                    }
                }
                if (html.ToLowerInvariant().Contains("inventory issues"))
                {
                    goto Label_12D4;
                }
            Label_12B7:
                if (!html.ToUpper().Contains("being processed".ToUpper()))
                {
                    goto Label_1309;
                }
                goto Label_117C;
            Label_12D4:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': Some products became unavailable and your cart has been updated. We're sorry for the inconvenience.");
                return false;
            Label_1309:
                document.LoadHtml(html);
                string str16 = "";
                string innerText = "";
                if (document.DocumentNode.Descendants("tr").Any<HtmlNode>(x => ((x.Attributes["data-product-id"] != null) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "product")))
                {
                    HtmlNode local1 = document.DocumentNode.Descendants("tr").First<HtmlNode>(x => ((x.Attributes["data-product-id"] != null) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "product"));
                    str16 = local1.Attributes["data-variant-id"].Value;
                    innerText = local1.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("product__description__name"))).InnerText;
                }
                else if (!document.DocumentNode.Descendants("span").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-title"))))
                {
                    if (document.DocumentNode.Descendants("td").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description")))
                    {
                        innerText = document.DocumentNode.Descendants("td").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description"))).Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__name order-summary__emphasis"))).InnerText.Trim();
                        str16 = this._runner.PickedSize.Value.Value;
                    }
                }
                else
                {
                    innerText = document.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-title"))).InnerText.Trim();
                    str16 = this._runner.PickedSize.Value.Value;
                }
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': product in cart '{innerText}' ({str16})");
                if (!document.DocumentNode.Descendants("span").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number"))))
                {
                    if (!html.ToUpper().Contains("Correct your payment information".ToUpper()))
                    {
                        if (html.ToUpper().Contains("Card was declined".ToUpper()))
                        {
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment didn't go through - card was declined");
                        }
                        else if (html.ToUpper().Contains("Your billing information does not match your credit card. Please check with your bank.".ToUpper()))
                        {
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment didn't go through - Your billing information does not match your credit card. Please check with your bank.");
                        }
                        else if (!html.ToUpper().Contains("There was an error processing your payment".ToUpper()))
                        {
                            if (html.ToUpper().Contains("An error occurred while processing the payment information. Please try again later".ToUpper()))
                            {
                                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment didn't go through - an error occurred while processing the payment information. Please try again later");
                            }
                            else
                            {
                                this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment didn't go through - unknown error");
                            }
                        }
                        else
                        {
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment didn't go through - there was an error processing your payment");
                        }
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment didn't go through - correct your payment information");
                    }
                    return false;
                }
                string str18 = document.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number"))).InnerHtml.Trim();
                this._task.OrderNo = str18;
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': the payment was successful. Congratulations! ({str18})");
                return true;
            Label_17B0:
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': checkout unsuccessful");
                return false;
            Label_17D3:
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error during task init");
                return false;
            Label_1814:
                flag = false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    this._runner.IsError = true;
                    string str19 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str19 = " - " + exception.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error while processing payment{str19}");
                    Global.Logger.Error($"Error while processing payment of task '{this._task.Name} - {this._task.Guid}'", exception);
                    return false;
                }
                flag = false;
            }
            return flag;
        }

        private void ProcessDeepSearchLink(Product product)
        {
            object obj2;
            Product product2;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(product.Link);
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Accept-Language", "sk-SK,sk;q=0.8,cs;q=0.6,en-US;q=0.4,en;q=0.2");
            request.CookieContainer = this._runner.Cookies;
            if (this._runner.Proxy != null)
            {
                request.Proxy = this._runner.Proxy;
            }
            string html = "";
            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {
                html = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            string str6 = "";
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            string shopifyWebsite = this._task.ShopifyWebsite;
            uint num = <PrivateImplementationDetails>.ComputeStringHash(shopifyWebsite);
            if (num <= 0x81836124)
            {
                if (num <= 0x4e1453cf)
                {
                    if (num <= 0x28553330)
                    {
                        if (num <= 0x494f792)
                        {
                            if (num != 0x2850ca0)
                            {
                                if ((num != 0x494f792) || (shopifyWebsite != "saintalfred"))
                                {
                                    goto Label_18B4;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                            }
                            else
                            {
                                if (shopifyWebsite != "undefeated")
                                {
                                    goto Label_18B4;
                                }
                                product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                                if (html.Contains("\"tags\":"))
                                {
                                    string str10 = html.Substring(html.IndexOf("\"tags\":"));
                                    str10 = str10.Substring(str10.IndexOf("[") + 1);
                                    char[] separator = new char[] { ',' };
                                    foreach (string str15 in str10.Substring(0, str10.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                                    {
                                        product.ExtendedTitle = product.ExtendedTitle + " " + str15;
                                    }
                                }
                            }
                        }
                        else if (num == 0x2132f0cd)
                        {
                            if (shopifyWebsite != "hanon")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                            if (html.Contains("\"tags\":"))
                            {
                                string str12 = html.Substring(html.IndexOf("\"tags\":"));
                                str12 = str12.Substring(str12.IndexOf("[") + 1);
                                char[] separator = new char[] { ',' };
                                foreach (string str8 in str12.Substring(0, str12.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                                {
                                    product.ExtendedTitle = product.ExtendedTitle + " " + str8;
                                }
                            }
                        }
                        else if (num == 0x2315585e)
                        {
                            if (shopifyWebsite != "offthehook")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                        else
                        {
                            if ((num != 0x28553330) || (shopifyWebsite != "nrml"))
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                    }
                    else if (num <= 0x38b6e100)
                    {
                        if (num == 0x339ec4fc)
                        {
                            if (shopifyWebsite != "renarts")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                        else
                        {
                            if ((num != 0x38b6e100) || (shopifyWebsite != "funko"))
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"))).InnerText.Trim();
                        }
                    }
                    else if (num == 0x396a78ce)
                    {
                        if (shopifyWebsite != "lapstoneandhammer")
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                    else if (num != 0x4b0d5ebb)
                    {
                        if ((num != 0x4e1453cf) || (shopifyWebsite != "proper"))
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                    else
                    {
                        if (shopifyWebsite != "palaceeu")
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"))).InnerText.Trim();
                    }
                }
                else if (num <= 0x6d6601da)
                {
                    if (num <= 0x5134779d)
                    {
                        if (num == 0x4e70bbdf)
                        {
                            if (shopifyWebsite != "premierestore")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                        else
                        {
                            if ((num != 0x5134779d) || (shopifyWebsite != "palaceus"))
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"))).InnerText.Trim();
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 0x63c27943:
                                if (shopifyWebsite != "havenshop")
                                {
                                    goto Label_18B4;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_18CE;

                            case 0x63f2cf1c:
                                if (shopifyWebsite != "socialstatus")
                                {
                                    goto Label_18B4;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_18CE;
                        }
                        if ((num != 0x6d6601da) || (shopifyWebsite != "shophny"))
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                }
                else if (num > 0x73761b67)
                {
                    switch (num)
                    {
                        case 0x756a879f:
                            if (shopifyWebsite != "abpstore")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_18CE;

                        case 0x7a949f40:
                            if (shopifyWebsite != "eflashjp")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_18CE;
                    }
                    if ((num != 0x81836124) || (shopifyWebsite != "eflashsg"))
                    {
                        goto Label_18B4;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                else
                {
                    switch (num)
                    {
                        case 0x6d970277:
                            if (shopifyWebsite != "notre")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_18CE;

                        case 0x6f7f4ed7:
                            if (shopifyWebsite != "packer")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>().InnerText.Trim();
                            if (html.Contains("\"tags\":"))
                            {
                                string str5 = html.Substring(html.IndexOf("\"tags\":"));
                                str5 = str5.Substring(str5.IndexOf("[") + 1);
                                char[] separator = new char[] { ',' };
                                foreach (string str14 in str5.Substring(0, str5.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                                {
                                    product.ExtendedTitle = product.ExtendedTitle + " " + str14;
                                }
                            }
                            goto Label_18CE;
                    }
                    if ((num != 0x73761b67) || (shopifyWebsite != "a-ma-maniere"))
                    {
                        goto Label_18B4;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                goto Label_18CE;
            }
            if (num <= 0xb4e9bd0e)
            {
                if (num <= 0x9553d8f4)
                {
                    if (num <= 0x8b55eb57)
                    {
                        if (num != 0x85732999)
                        {
                            if ((num != 0x8b55eb57) || (shopifyWebsite != "sneakerpolitics"))
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                        else
                        {
                            if (shopifyWebsite != "trophyroomstore")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 0x8fb99128:
                                if (shopifyWebsite != "eflasheu")
                                {
                                    goto Label_18B4;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_18CE;

                            case 0x94481229:
                                if (shopifyWebsite != "capsuletoronto")
                                {
                                    goto Label_18B4;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_18CE;
                        }
                        if ((num != 0x9553d8f4) || (shopifyWebsite != "leaders1354"))
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                }
                else if (num <= 0x98f09984)
                {
                    if (num == 0x95928b2a)
                    {
                        if (shopifyWebsite != "eflashus")
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                    else
                    {
                        if ((num != 0x98f09984) || (shopifyWebsite != "oipolloi"))
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name"))).InnerText.Trim();
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0x9e145f8b:
                            if (shopifyWebsite != "deadstock")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = document.DocumentNode.Descendants("title").First<HtmlNode>().InnerText.Trim();
                            if (html.Contains("data-product="))
                            {
                                string str11 = html.Substring(html.IndexOf("data-product="));
                                str11 = str11.Substring(str11.IndexOf("{"));
                                obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str11.Substring(0, str11.IndexOf("}\"") + 1).Replace("&quot;", "\""));
                                if (<>o__47.<>p__10 == null)
                                {
                                    <>o__47.<>p__10 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                                }
                                if (<>o__47.<>p__5 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__47.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "tags", typeof(Shopify), argumentInfo));
                                }
                                foreach (object obj4 in <>o__47.<>p__10.Target(<>o__47.<>p__10, <>o__47.<>p__5.Target(<>o__47.<>p__5, obj2)))
                                {
                                    product2 = product;
                                    if (<>o__47.<>p__9 == null)
                                    {
                                        <>o__47.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Shopify)));
                                    }
                                    if (<>o__47.<>p__8 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__47.<>p__8 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__47.<>p__7 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__47.<>p__7 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                                    }
                                    if (<>o__47.<>p__6 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__47.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                                    }
                                    product2.ExtendedTitle = <>o__47.<>p__9.Target(<>o__47.<>p__9, <>o__47.<>p__8.Target(<>o__47.<>p__8, product2.ExtendedTitle, <>o__47.<>p__7.Target(<>o__47.<>p__7, " ", <>o__47.<>p__6.Target(<>o__47.<>p__6, obj4))));
                                }
                            }
                            if (html.Contains("Product Code:"))
                            {
                                string str13 = html.Substring(html.IndexOf("Product Code:"));
                                str13 = str13.Substring(str13.IndexOf(":") + 1);
                                str13 = str13.Substring(0, str13.IndexOf("\"")).Trim();
                                product.ExtendedTitle = product.ExtendedTitle + " " + str13;
                            }
                            goto Label_18CE;

                        case 0x9e4864ef:
                            if (shopifyWebsite != "addictmiami")
                            {
                                goto Label_18B4;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_18CE;
                    }
                    if ((num != 0xb4e9bd0e) || (shopifyWebsite != "obey"))
                    {
                        goto Label_18B4;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                goto Label_18CE;
            }
            if (num <= 0xda304ded)
            {
                if (num > 0xc0c6237f)
                {
                    if (num == 0xc5246ded)
                    {
                        if (shopifyWebsite != "wishatl")
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                    else if (num == 0xd39d8c83)
                    {
                        if (shopifyWebsite != "xhibition")
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = document.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:title"))).Attributes["content"].Value.Trim();
                        string str7 = html.Substring(html.IndexOf("var amastyXnotifConfig = {"));
                        str7 = str7.Substring(str7.IndexOf("{"));
                        obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str7.Substring(0, str7.IndexOf("};") + 1).Replace("product:", "\"product\":").Replace("customer_id:", "\"customer_id\":").Replace(@"\/", "v"));
                        if (<>o__47.<>p__23 == null)
                        {
                            <>o__47.<>p__23 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                        }
                        if (<>o__47.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__47.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "tags", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__47.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__47.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "product", typeof(Shopify), argumentInfo));
                        }
                        foreach (object obj3 in <>o__47.<>p__23.Target(<>o__47.<>p__23, <>o__47.<>p__18.Target(<>o__47.<>p__18, <>o__47.<>p__17.Target(<>o__47.<>p__17, obj2))))
                        {
                            product2 = product;
                            if (<>o__47.<>p__22 == null)
                            {
                                <>o__47.<>p__22 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__47.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__47.<>p__21 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__47.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__47.<>p__20 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__47.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__47.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            product2.ExtendedTitle = <>o__47.<>p__22.Target(<>o__47.<>p__22, <>o__47.<>p__21.Target(<>o__47.<>p__21, product2.ExtendedTitle, <>o__47.<>p__20.Target(<>o__47.<>p__20, " ", <>o__47.<>p__19.Target(<>o__47.<>p__19, obj3))));
                        }
                    }
                    else
                    {
                        if ((num != 0xda304ded) || (shopifyWebsite != "blendsus"))
                        {
                            goto Label_18B4;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                }
                else if (num == 0xb77b9be7)
                {
                    if (shopifyWebsite != "shopnicekicks")
                    {
                        goto Label_18B4;
                    }
                    product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                    if (html.Contains("STYLE#:"))
                    {
                        string str3 = html.Substring(html.IndexOf("STYLE#:"));
                        str3 = str3.Substring(str3.IndexOf(":") + 1);
                        str3 = str3.Substring(0, str3.IndexOf("<")).Trim();
                        product.ExtendedTitle = product.ExtendedTitle + " " + str3;
                    }
                    if (html.Contains("\"tags\":["))
                    {
                        string str2 = html.Substring(html.IndexOf("\"tags\":["));
                        str2 = str2.Substring(str2.IndexOf("[") + 1);
                        char[] separator = new char[] { ',' };
                        foreach (string str9 in str2.Substring(0, str2.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                        {
                            product.ExtendedTitle = product.ExtendedTitle + " " + str9;
                        }
                    }
                }
                else
                {
                    if ((num != 0xc0c6237f) || (shopifyWebsite != "creme321"))
                    {
                        goto Label_18B4;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                goto Label_18CE;
            }
            if (num > 0xe5c648dd)
            {
                if (num == 0xe72487ea)
                {
                    if (shopifyWebsite != "unknwn")
                    {
                        goto Label_18B4;
                    }
                    product.ExtendedTitle = document.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                    obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(document.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "productInfo"))).InnerText);
                    if (<>o__47.<>p__16 == null)
                    {
                        <>o__47.<>p__16 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                    }
                    if (<>o__47.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__47.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "tags", typeof(Shopify), argumentInfo));
                    }
                    foreach (object obj5 in <>o__47.<>p__16.Target(<>o__47.<>p__16, <>o__47.<>p__11.Target(<>o__47.<>p__11, obj2)))
                    {
                        product2 = product;
                        if (<>o__47.<>p__15 == null)
                        {
                            <>o__47.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Shopify)));
                        }
                        if (<>o__47.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__47.<>p__14 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__47.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__47.<>p__13 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__47.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__47.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        product2.ExtendedTitle = <>o__47.<>p__15.Target(<>o__47.<>p__15, <>o__47.<>p__14.Target(<>o__47.<>p__14, product2.ExtendedTitle, <>o__47.<>p__13.Target(<>o__47.<>p__13, " ", <>o__47.<>p__12.Target(<>o__47.<>p__12, obj5))));
                    }
                }
                else if (num != 0xe8e8c154)
                {
                    if ((num != 0xe8fbf68d) || (shopifyWebsite != "kith"))
                    {
                        goto Label_18B4;
                    }
                    str6 = html.Substring(html.IndexOf("_BISConfig.product ="));
                    str6 = str6.Substring(str6.IndexOf("{"));
                    obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str6.Substring(0, str6.IndexOf(";")));
                    if (<>o__47.<>p__2 == null)
                    {
                        <>o__47.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                    }
                    if (<>o__47.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__47.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                    }
                    if (<>o__47.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__47.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    product.ExtendedTitle = <>o__47.<>p__2.Target(<>o__47.<>p__2, <>o__47.<>p__1.Target(<>o__47.<>p__1, <>o__47.<>p__0.Target(<>o__47.<>p__0, obj2, "title")));
                    if (<>o__47.<>p__4 == null)
                    {
                        <>o__47.<>p__4 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Newtonsoft.Json.Linq.JArray), typeof(Shopify)));
                    }
                    if (<>o__47.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__47.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                    }
                    foreach (Newtonsoft.Json.Linq.JToken token in <>o__47.<>p__4.Target(<>o__47.<>p__4, <>o__47.<>p__3.Target(<>o__47.<>p__3, obj2, "tags")))
                    {
                        product.ExtendedTitle = product.ExtendedTitle + " " + token;
                    }
                }
                else
                {
                    if (shopifyWebsite != "aboveclouds")
                    {
                        goto Label_18B4;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                goto Label_18CE;
            }
            switch (num)
            {
                case 0xdfd25109:
                    if (shopifyWebsite != "18montrose")
                    {
                        break;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                    goto Label_18CE;

                case 0xe1c6240d:
                    if (shopifyWebsite != "commonwealth")
                    {
                        break;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                    goto Label_18CE;

                default:
                    if ((num == 0xe5c648dd) && (shopifyWebsite == "solefly"))
                    {
                        product.ExtendedTitle = product.ProductTitle;
                        goto Label_18CE;
                    }
                    break;
            }
        Label_18B4:
            product.ExtendedTitle = product.ProductTitle;
        Label_18CE:
            product.ExtendedTitle = product.ExtendedTitle.RemoveSpecialCharacters();
        }

        public bool Search()
        {
            if ((this._task.Payment == TaskObject.PaymentEnum.paypal) && (this._task.Driver == null))
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                return false;
            }
            this.Init(true);
            if (this._task.Login && !this._isLoggedIn)
            {
                this.Login();
            }
            this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
            States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
            return this.SearchInternal();
        }

        private bool SearchInternal()
        {
            try
            {
                IEnumerator<string> enumerator5;
                List<string> list = new List<string> { 
                    "https://yeezysupply.com/collections/new-arrivals",
                    "https://yeezysupply.com/collections/men",
                    "https://yeezysupply.com/collections/women"
                };
                string requestUriString = "";
                if (this._task.Link.Contains("eflash"))
                {
                    requestUriString = this._task.HomeUrl;
                }
                else if (!this._task.Link.Contains("yeezysupply"))
                {
                    if (!this._task.Link.Contains("store.travisscott.com"))
                    {
                        if (this._task.ShopifyWebsite == "other")
                        {
                            requestUriString = this._task.Link + "/sitemap_products_1.xml";
                        }
                        else
                        {
                            requestUriString = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.Website == this._task.ShopifyWebsite)).SearchLink;
                        }
                    }
                    else
                    {
                        requestUriString = "https://store.travisscott.com";
                    }
                }
                else
                {
                    requestUriString = list[0];
                }
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Cache-Control", "max-age=0");
                string text = "";
                string str3 = "";
                using (WebResponse response = request.GetResponse())
                {
                    str3 = response.ResponseUri.ToString();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        text = reader.ReadToEnd();
                    }
                }
                List<Product> source = new List<Product>();
                if ((this._task.Link.Contains("eflash") || this._task.Link.Contains("yeezysupply")) || this._task.HomeUrl.Contains("store.travisscott.com"))
                {
                    goto Label_0611;
                }
                XDocument document1 = XDocument.Parse(text);
                if (document1 == null)
                {
                    throw new Exception("sitemap_products_1.xml not available");
                }
                XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
                XNamespace namespace2 = "http://www.google.com/schemas/sitemap-image/1.1";
                List<XElement>.Enumerator enumerator = (from x in document1.Descendants(ns + "url")
                    where x.Descendants(ns + "lastmod").ToList<XElement>().Count > 0
                    select x).ToList<XElement>().GetEnumerator();
            Label_02AC:
                try
                {
                Label_02AC:
                    if (!enumerator.MoveNext())
                    {
                        goto Label_03AE;
                    }
                    XElement current = enumerator.Current;
                    if (!current.Descendants(ns + "loc").Any<XElement>())
                    {
                        goto Label_02AC;
                    }
                    Product item = new Product {
                        Link = current.Descendants(ns + "loc").FirstOrDefault<XElement>().Value
                    };
                    try
                    {
                        item.LastMod = new DateTime?(DateTime.Parse(current.Descendants(ns + "lastmod").FirstOrDefault<XElement>().Value));
                    }
                    catch
                    {
                    }
                    goto Label_037E;
                Label_034B:
                    item.ProductTitle = current.Descendants(namespace2 + "title").FirstOrDefault<XElement>().Value;
                    source.Add(item);
                    goto Label_02AC;
                Label_037E:
                    if (current.Descendants(namespace2 + "title").FirstOrDefault<XElement>() == null)
                    {
                        goto Label_02AC;
                    }
                    goto Label_034B;
                }
                finally
                {
                    enumerator.Dispose();
                }
            Label_03AE:;
                try
                {
                    request = (HttpWebRequest) WebRequest.Create(requestUriString.Replace("1.xml", "2.xml"));
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.KeepAlive = true;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    request.Headers.Add("Cache-Control", "max-age=0");
                    text = "";
                    using (WebResponse response2 = request.GetResponse())
                    {
                        using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                        {
                            text = reader2.ReadToEnd();
                        }
                    }
                    XDocument document2 = XDocument.Parse(text);
                    if (document2 == null)
                    {
                        throw new Exception("sitemap_products_1.xml not available");
                    }
                    ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
                    namespace2 = "http://www.google.com/schemas/sitemap-image/1.1";
                    using (enumerator = (from x in document2.Descendants(ns + "url")
                        where x.Descendants(ns + "lastmod").ToList<XElement>().Count > 0
                        select x).ToList<XElement>().GetEnumerator())
                    {
                    Label_0504:
                        if (!enumerator.MoveNext())
                        {
                            goto Label_1BC8;
                        }
                        XElement current = enumerator.Current;
                        if (!current.Descendants(ns + "loc").Any<XElement>())
                        {
                            goto Label_0504;
                        }
                        Product item = new Product {
                            Link = current.Descendants(ns + "loc").FirstOrDefault<XElement>().Value
                        };
                        try
                        {
                            item.LastMod = new DateTime?(DateTime.Parse(current.Descendants(ns + "lastmod").FirstOrDefault<XElement>().Value));
                        }
                        catch
                        {
                        }
                        goto Label_05D6;
                    Label_05A3:
                        item.ProductTitle = current.Descendants(namespace2 + "title").FirstOrDefault<XElement>().Value;
                        source.Add(item);
                        goto Label_0504;
                    Label_05D6:
                        if (current.Descendants(namespace2 + "title").FirstOrDefault<XElement>() == null)
                        {
                            goto Label_0504;
                        }
                        goto Label_05A3;
                    }
                }
                catch
                {
                    goto Label_1BC8;
                }
            Label_0611:
                if (this._task.Link.Contains("yeezysupply"))
                {
                    this._currentDoc.LoadHtml(text);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-new-arrivals-json"))).InnerText);
                    DateTime now = DateTime.Now;
                    if (<>o__46.<>p__12 == null)
                    {
                        <>o__46.<>p__12 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                    }
                    if (<>o__46.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__46.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "products", typeof(Shopify), argumentInfo));
                    }
                    foreach (object obj2 in <>o__46.<>p__12.Target(<>o__46.<>p__12, <>o__46.<>p__0.Target(<>o__46.<>p__0, this._dynObj)))
                    {
                        if (<>o__46.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__46.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__46.<>p__3 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__46.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__46.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        if (!<>o__46.<>p__4.Target(<>o__46.<>p__4, <>o__46.<>p__3.Target(<>o__46.<>p__3, <>o__46.<>p__2.Target(<>o__46.<>p__2, <>o__46.<>p__1.Target(<>o__46.<>p__1, obj2, "available")), true)))
                        {
                            Product prod = new Product {
                                LastMod = new DateTime?(now)
                            };
                            if (<>o__46.<>p__8 == null)
                            {
                                <>o__46.<>p__8 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__46.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__7 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            prod.Link = <>o__46.<>p__8.Target(<>o__46.<>p__8, <>o__46.<>p__7.Target(<>o__46.<>p__7, "https://yeezysupply.com", <>o__46.<>p__6.Target(<>o__46.<>p__6, <>o__46.<>p__5.Target(<>o__46.<>p__5, obj2, "url"))));
                            if (<>o__46.<>p__11 == null)
                            {
                                <>o__46.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__46.<>p__10 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__9 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            prod.ProductTitle = <>o__46.<>p__11.Target(<>o__46.<>p__11, <>o__46.<>p__10.Target(<>o__46.<>p__10, <>o__46.<>p__9.Target(<>o__46.<>p__9, obj2, "handle")));
                            prod.ProductTitle = prod.ProductTitle.Replace("-", " ");
                            if (!source.Any<Product>(x => (x.ProductTitle == prod.ProductTitle)))
                            {
                                source.Add(prod);
                            }
                        }
                    }
                    string str4 = this._client.Get("https://yeezysupply.com").Text();
                    while (str4.Contains("var p = {"))
                    {
                        str4 = str4.Substring(str4.IndexOf("var p = {"));
                        str4 = str4.Substring(str4.IndexOf("title"));
                        str4 = str4.Substring(str4.IndexOf("\"") + 1);
                        string str6 = str4.Substring(0, str4.IndexOf("\""));
                        str4 = str4.Substring(str4.IndexOf("handle"));
                        str4 = str4.Substring(str4.IndexOf("\"") + 1);
                        string str5 = str4.Substring(0, str4.IndexOf("\""));
                        str4 = str4.Substring(str4.IndexOf("featured_image"));
                        str4 = str4.Substring(str4.IndexOf("\"") + 1);
                        string text1 = "https:" + str4.Substring(0, str4.IndexOf("\"")).Replace(@"\/", "/");
                        Product product1 = new Product {
                            LastMod = new DateTime?(now),
                            Link = "https://yeezysupply.com/products/" + str5,
                            ProductTitle = str6
                        };
                        if (!source.Any<Product>(x => (x.ProductTitle == product1.ProductTitle)))
                        {
                            source.Add(product1);
                        }
                    }
                    this._srr = this._client.Get(list[1]).Text();
                    this._currentDoc.LoadHtml(this._srr);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-collection-json"))).InnerText);
                    if (<>o__46.<>p__29 == null)
                    {
                        <>o__46.<>p__29 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                    }
                    if (<>o__46.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__46.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "products", typeof(Shopify), argumentInfo));
                    }
                    foreach (object obj3 in <>o__46.<>p__29.Target(<>o__46.<>p__29, <>o__46.<>p__13.Target(<>o__46.<>p__13, this._dynObj)))
                    {
                        if (<>o__46.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__46.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__46.<>p__16 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__46.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__46.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        if (!<>o__46.<>p__17.Target(<>o__46.<>p__17, <>o__46.<>p__16.Target(<>o__46.<>p__16, <>o__46.<>p__15.Target(<>o__46.<>p__15, <>o__46.<>p__14.Target(<>o__46.<>p__14, obj3, "available")), true)))
                        {
                            Product product2 = new Product {
                                LastMod = new DateTime?(now)
                            };
                            if (<>o__46.<>p__21 == null)
                            {
                                <>o__46.<>p__21 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__46.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__20 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            product2.Link = <>o__46.<>p__21.Target(<>o__46.<>p__21, <>o__46.<>p__20.Target(<>o__46.<>p__20, "https://yeezysupply.com", <>o__46.<>p__19.Target(<>o__46.<>p__19, <>o__46.<>p__18.Target(<>o__46.<>p__18, obj3, "url"))));
                            if (<>o__46.<>p__24 == null)
                            {
                                <>o__46.<>p__24 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__46.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            product2.ProductTitle = <>o__46.<>p__24.Target(<>o__46.<>p__24, <>o__46.<>p__23.Target(<>o__46.<>p__23, <>o__46.<>p__22.Target(<>o__46.<>p__22, obj3, "handle")));
                            if (<>o__46.<>p__28 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__28 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__27 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            <>o__46.<>p__28.Target(<>o__46.<>p__28, <>o__46.<>p__27.Target(<>o__46.<>p__27, <>o__46.<>p__26.Target(<>o__46.<>p__26, <>o__46.<>p__25.Target(<>o__46.<>p__25, obj3, "handle")), "mens-military-boot-in-washed-canvas-splinter-camo"));
                            product2.ProductTitle = product2.ProductTitle.Replace("-", " ");
                            if (!source.Any<Product>(x => (x.ProductTitle == product2.ProductTitle)))
                            {
                                source.Add(product2);
                            }
                        }
                    }
                    this._srr = this._client.Get(list[2]).Text();
                    this._currentDoc.LoadHtml(this._srr);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-collection-json"))).InnerText);
                    if (<>o__46.<>p__46 == null)
                    {
                        <>o__46.<>p__46 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify)));
                    }
                    if (<>o__46.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__46.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "products", typeof(Shopify), argumentInfo));
                    }
                    foreach (object obj4 in <>o__46.<>p__46.Target(<>o__46.<>p__46, <>o__46.<>p__30.Target(<>o__46.<>p__30, this._dynObj)))
                    {
                        if (<>o__46.<>p__34 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__46.<>p__34 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__46.<>p__33 = CallSite<Func<CallSite, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__32 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__46.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                        }
                        if (<>o__46.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__46.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                        }
                        if (!<>o__46.<>p__34.Target(<>o__46.<>p__34, <>o__46.<>p__33.Target(<>o__46.<>p__33, <>o__46.<>p__32.Target(<>o__46.<>p__32, <>o__46.<>p__31.Target(<>o__46.<>p__31, obj4, "available")), true)))
                        {
                            Product product3 = new Product {
                                LastMod = new DateTime?(now)
                            };
                            if (<>o__46.<>p__38 == null)
                            {
                                <>o__46.<>p__38 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__46.<>p__37 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__37 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__36 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__35 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__35 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            product3.Link = <>o__46.<>p__38.Target(<>o__46.<>p__38, <>o__46.<>p__37.Target(<>o__46.<>p__37, "https://yeezysupply.com", <>o__46.<>p__36.Target(<>o__46.<>p__36, <>o__46.<>p__35.Target(<>o__46.<>p__35, obj4, "url"))));
                            if (<>o__46.<>p__41 == null)
                            {
                                <>o__46.<>p__41 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify)));
                            }
                            if (<>o__46.<>p__40 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__39 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__39 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            product3.ProductTitle = <>o__46.<>p__41.Target(<>o__46.<>p__41, <>o__46.<>p__40.Target(<>o__46.<>p__40, <>o__46.<>p__39.Target(<>o__46.<>p__39, obj4, "handle")));
                            if (<>o__46.<>p__45 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__45 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__44 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__44 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__43 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__46.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify), argumentInfo));
                            }
                            if (<>o__46.<>p__42 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__46.<>p__42 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify), argumentInfo));
                            }
                            <>o__46.<>p__45.Target(<>o__46.<>p__45, <>o__46.<>p__44.Target(<>o__46.<>p__44, <>o__46.<>p__43.Target(<>o__46.<>p__43, <>o__46.<>p__42.Target(<>o__46.<>p__42, obj4, "handle")), "mens-military-boot-in-washed-canvas-splinter-camo"));
                            product3.ProductTitle = product3.ProductTitle.Replace("-", " ");
                            if (!source.Any<Product>(x => (x.ProductTitle == product3.ProductTitle)))
                            {
                                source.Add(product3);
                            }
                        }
                    }
                }
                else if (this._task.HomeUrl.Contains("store.travisscott.com"))
                {
                    this._currentDoc.LoadHtml(text);
                    DateTime now = DateTime.Now;
                    foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("a")
                        where (x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("ProductGridItem")
                        select x)
                    {
                        Product item = new Product {
                            LastMod = new DateTime?(now),
                            Link = "https://store.travisscott.com" + node.Attributes["href"].Value,
                            ProductTitle = node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("PGI__title"))).InnerText.Trim()
                        };
                        source.Add(item);
                    }
                }
                else
                {
                    this._currentDoc.LoadHtml(text);
                    DateTime now = DateTime.Now;
                    foreach (HtmlNode node2 in from x in this._currentDoc.DocumentNode.Descendants("a")
                        where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "grid-view-item__link")
                        select x)
                    {
                        Product item = new Product {
                            LastMod = new DateTime?(now),
                            Link = str3 + node2.Attributes["href"].Value,
                            ProductTitle = node2.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("h4"))).InnerText.Trim()
                        };
                        source.Add(item);
                    }
                }
            Label_1BC8:
                if ((this._task.ShopifyWebsite != "other") && this._task.DeepSearch)
                {
                    source = (from x in source
                        orderby x.LastMod descending
                        select x).Take<Product>(this._task.DeepSearchLinks).ToList<Product>();
                    foreach (Product product5 in source)
                    {
                        this.ProcessDeepSearchLink(product5);
                        if (source.Count > 1)
                        {
                            this._runner.SetProxy();
                        }
                    }
                }
                if (this._task.Last25Products)
                {
                    source = (from x in (from x in source
                        where x.LastMod.HasValue
                        select x).ToList<Product>()
                        orderby x.LastMod descending
                        select x).Take<Product>(0x19).ToList<Product>();
                }
                foreach (Product local1 in source)
                {
                    local1.ProductTitle = local1.ProductTitle.Replace("/", " ").Replace("  ", " ").Replace("\"", "").Replace("'", "");
                }
                foreach (Product local2 in source)
                {
                    local2.ProductTitle = local2.ProductTitle.RemoveSpecialCharacters();
                }
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': products available {source.Count}");
                }
                List<Product> list3 = new List<Product>();
                using (enumerator5 = this._task.Keywords.GetEnumerator())
                {
                    while (enumerator5.MoveNext())
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = enumerator5.Current.Split(separator);
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            strArray[j] = strArray[j].Trim().ToUpperInvariant();
                        }
                        foreach (Product product in source)
                        {
                            string str7;
                            bool flag = true;
                            string[] strArray2 = strArray;
                            int index = 0;
                            goto Label_1EC7;
                        Label_1E60:
                            str7 = strArray2[index];
                            string extendedTitle = "";
                            if ((this._task.ShopifyWebsite != "other") && this._task.DeepSearch)
                            {
                                extendedTitle = product.ExtendedTitle;
                            }
                            else
                            {
                                extendedTitle = product.ProductTitle;
                            }
                            if (!extendedTitle.ToUpperInvariant().Contains(str7))
                            {
                                goto Label_1ED1;
                            }
                            index++;
                        Label_1EC7:
                            if (index >= strArray2.Length)
                            {
                                goto Label_1ED4;
                            }
                            goto Label_1E60;
                        Label_1ED1:
                            flag = false;
                        Label_1ED4:
                            if (flag && !list3.Any<Product>(x => (x.Link == product.Link)))
                            {
                                list3.Add(product);
                            }
                        }
                    }
                }
                if (list3.Count == 0)
                {
                    if (Global.SETTINGS.DetailedLog)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': no positive keywords matches found");
                    }
                    return false;
                }
                string str9 = "";
                for (int i = 0; i < list3.Count; i++)
                {
                    if (i != (list3.Count - 1))
                    {
                        string[] textArray1 = new string[] { str9, "\t", list3[i].ProductTitle, " (", list3[i].Link, ")\n" };
                        str9 = string.Concat(textArray1);
                    }
                    else
                    {
                        string[] textArray2 = new string[] { str9, "\t", list3[i].ProductTitle, " (", list3[i].Link, ")" };
                        str9 = string.Concat(textArray2);
                    }
                }
                if (Global.SETTINGS.DetailedLog)
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': positive keywords matched products ({list3.Count}): 
{str9}");
                }
                List<Product> list4 = new List<Product>();
                if (this._task.NegativeKeywords.Any<string>())
                {
                    foreach (Product product6 in list3)
                    {
                        bool flag3 = true;
                        Product tmpProd = null;
                        tmpProd = product6;
                        using (enumerator5 = this._task.NegativeKeywords.GetEnumerator())
                        {
                            while (enumerator5.MoveNext())
                            {
                                char[] separator = new char[] { ' ' };
                                string[] strArray3 = enumerator5.Current.Split(separator);
                                for (int k = 0; k < strArray3.Length; k++)
                                {
                                    strArray3[k] = strArray3[k].Trim().ToUpperInvariant();
                                }
                                foreach (string str11 in strArray3)
                                {
                                    string extendedTitle = "";
                                    if ((this._task.ShopifyWebsite != "other") && this._task.DeepSearch)
                                    {
                                        extendedTitle = product6.ExtendedTitle;
                                    }
                                    else
                                    {
                                        extendedTitle = product6.ProductTitle;
                                    }
                                    if (extendedTitle.ToUpperInvariant().Contains(str11))
                                    {
                                        goto Label_2186;
                                    }
                                }
                                continue;
                            Label_2186:
                                flag3 = false;
                            }
                        }
                        if (flag3 && !list4.Any<Product>(x => (x.Link == tmpProd.Link)))
                        {
                            list4.Add(tmpProd);
                        }
                    }
                    if (list4.Count == 0)
                    {
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': no negative keywords matches found");
                        }
                        return false;
                    }
                    str9 = "";
                    for (int j = 0; j < list4.Count; j++)
                    {
                        if (j == (list4.Count - 1))
                        {
                            string[] textArray3 = new string[] { str9, "\t", list4[j].ProductTitle, " (", list4[j].Link, ")" };
                            str9 = string.Concat(textArray3);
                        }
                        else
                        {
                            string[] textArray4 = new string[] { str9, "\t", list4[j].ProductTitle, " (", list4[j].Link, ")\n" };
                            str9 = string.Concat(textArray4);
                        }
                    }
                    if (Global.SETTINGS.DetailedLog)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': negative keywords matched products ({list4.Count}): 
{str9}");
                    }
                }
                else
                {
                    list4 = list3;
                }
                foreach (Product product7 in list4)
                {
                    if (!this._task.Link.ToLowerInvariant().Contains("yeezysupply.com"))
                    {
                        this.DirectLinkGeneral(product7.Link);
                    }
                    else
                    {
                        this.DirectLinkYeezy(product7.Link, false);
                    }
                    if ((this._runner.Product != null) && (this._runner.Product.AvailableSizes.Count > 0))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    this._runner.IsError = true;
                    string str12 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str12 = " - " + exception.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error searching for products{str12}");
                    Global.Logger.Error($"Error searching for products for task '{this._task.Name} - {this._task.Guid}'", exception);
                    return false;
                }
                return false;
            }
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, true);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        }

        public void SmartCheckoutInit()
        {
            this._task.Status = States.GetTaskState(States.TaskState.PREPARING_SMART_CHECKOUT);
            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': smart checkout initiated");
            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': processing dummy product started");
            bool flag = this.DirectLink(this._task.DummyProduct);
            bool flag2 = false;
            bool flag3 = false;
            if (flag)
            {
                bool flag4 = true;
                while (!string.IsNullOrEmpty(this._cardId))
                {
                    if (!string.IsNullOrEmpty(this._checkoutLink))
                    {
                        if (this._task.Login)
                        {
                            flag4 = this.Login();
                        }
                        if (flag4)
                        {
                            this._isLoggedIn = true;
                            if (flag3 = this.AddToCart())
                            {
                                flag2 = this.CheckoutInternal(true);
                            }
                        }
                        goto Label_010D;
                    }
                Label_0075:
                    if (this._isInitError)
                    {
                        this._runner.IsError = true;
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error during task init");
                        throw new Exception("Init exception");
                    }
                    Thread.Sleep(200);
                }
                goto Label_0075;
            }
        Label_010D:
            if ((flag & flag3) & flag2)
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/cart/clear.js");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                if (!this._runner.HomeUrl.ToLowerInvariant().Contains("rsvpgallery.com") && !this._runner.HomeUrl.ToLowerInvariant().Contains("clot.com"))
                {
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                }
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*;q=0.8";
                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                string s = "";
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        reader.ReadToEnd();
                    }
                }
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': processing dummy product finished successfuly");
                this._runner.IsSmartCheckoutReady = true;
                if (this._task.ShopifySmartCheckoutStop)
                {
                    this._task.State = TaskObject.StateEnum.smartWaiting;
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_NEXT_STEP);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': smart checkout waiting for next step");
                    while (this._task.State == TaskObject.StateEnum.smartWaiting)
                    {
                        Thread.Sleep(100);
                    }
                }
                this._runner.PickedSize = null;
            }
            else
            {
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': processing dummy product finished unsuccessfuly");
            }
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                this._srr = this._client.Get(this._checkoutLink + "?step=payment_method").Text();
                this._currentDoc.LoadHtml(this._srr);
                this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => x.Attributes["data-select-gateway"] > null);
                this._paymentGateway = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => (x.Attributes["data-select-gateway"] > null)).Attributes["data-select-gateway"].Value;
                string[] textArray1 = new string[] { this._pickedShippingSource, "-", this._pickedShippingName.Replace(" ", "%20").Replace("&amp;", "%26"), "-", this._pickedShippingPrice };
                string str2 = string.Concat(textArray1);
                this._diData.Clear();
                this._diData.Add("utf8", "✓");
                this._diData.Add("_method", "patch");
                this._diData.Add("authenticity_token", "");
                this._diData.Add("previous_step", "shipping_method");
                this._diData.Add("step", "payment_method");
                this._diData.Add("checkout[shipping_rate][id]", str2);
                this._diData.Add("complete", "1");
                this._diData.Add("checkout[client_details][browser_width]", "1710");
                this._diData.Add("checkout[client_details][browser_height]", "1289");
                this._diData.Add("checkout[client_details][javascript_enabled]", "1");
                this._diData.Add("button", "");
                for (bool flag = true; flag; flag = this._srr.ToLowerInvariant().Contains("calculating taxes"))
                {
                    flag = false;
                    this._srr = this._client.Post(this._checkoutLink, this._diData).Text();
                }
                if (this._task.Payment == TaskObject.PaymentEnum.paypal)
                {
                    DateTime? nullable;
                    this._runner.ForcedPaypal = true;
                    this._diData.Clear();
                    this._diData.Add($"updates[{this._runner.PickedSize.Value.Value}]", "1");
                    this._diData.Add("goto_pp", "paypal_express");
                    HttpResponseMessage message = this._client.Get($"{this._checkoutLink}/express/redirect?complete=1 ");
                    string url = message.RequestMessage.RequestUri.ToString();
                    this._srr = message.Content.ReadAsStringAsync().Result.ToString();
                    while (!url.Contains("paypal.com"))
                    {
                        KeyValuePair<string, string> pair2 = this._client.Get(this._checkoutLink + "/express/redirecting?refresh_count=1").TextResponseUri();
                        url = pair2.Value;
                        this._srr = pair2.Key;
                    }
                    if (Global.SETTINGS.PayPalBeep)
                    {
                        EveAIO.Helpers.PlayBell();
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.OPENING_PAYPAL);
                    States.WriteLogger(this._task, States.LOGGER_STATES.OPENING_PAYPAL, null, "", "");
                    if (this._runner._notificator != null)
                    {
                        this._runner._notificator.Notify(Notificator.NotificationType.Paypal);
                    }
                    this._task.PaypalLink = url;
                    string domain = "";
                    foreach (System.Net.Cookie cookie in this._client.Cookies.List())
                    {
                        nullable = null;
                        this._task.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path, nullable));
                        domain = cookie.Domain;
                    }
                    foreach (string str4 in this._responseHeaders.GetValues("Set-Cookie"))
                    {
                        string str5 = str4.Substring(0, str4.IndexOf(";"));
                        string str6 = str5.Substring(str5.IndexOf("=") + 1);
                        if (str6.Contains(","))
                        {
                            str6 = WebUtility.UrlEncode(str6);
                        }
                        nullable = null;
                        OpenQA.Selenium.Cookie cookie2 = new OpenQA.Selenium.Cookie(str5.Substring(0, str5.IndexOf("=")), str6, domain, "", nullable);
                        this._task.Driver.Manage().Cookies.AddCookie(cookie2);
                    }
                    this._task.Driver.Navigate().GoToUrl(url);
                    return true;
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
                this._cardId = "";
                this._runner._tokenTimestamp = null;
                this._checkoutLink = "";
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
                ProfileObject profile = this._runner.Profile;
                string[] textArray1 = new string[] { this._pickedShippingSource, "-", this._pickedShippingName.Replace(" ", "%20").Replace("&amp;", "%26"), "-", this._pickedShippingPrice };
                string str = string.Concat(textArray1);
                this.SetClient();
                this._client.Handler.AllowAutoRedirect = false;
                this._diData.Clear();
                this._diData.Add("utf8", "✓");
                this._diData.Add("_method", "patch");
                this._diData.Add("authenticity_token", "");
                this._diData.Add("previous_step", "payment_method");
                this._diData.Add("step", "");
                this._diData.Add("s", this._cardId);
                this._diData.Add("checkout[payment_gateway]", this._paymentGateway);
                this._diData.Add("checkout[credit_card][vault]", "false");
                this._diData.Add("checkout[different_billing_address]", "true");
                this._diData.Add("checkout[billing_address][first_name]", profile.FirstName);
                this._diData.Add("checkout[billing_address][last_name]", profile.LastName);
                this._diData.Add("checkout[billing_address][address1]", profile.Address1);
                this._diData.Add("checkout[billing_address][address2]", profile.Address2);
                this._diData.Add("checkout[billing_address][city]", profile.City);
                this._diData.Add("checkout[billing_address][country]", profile.Country);
                this._diData.Add("checkout[billing_address][state]", string.IsNullOrEmpty(profile.State) ? "None" : profile.State);
                this._diData.Add("checkout[billing_address][province]", string.IsNullOrEmpty(profile.State) ? "None" : profile.State);
                this._diData.Add("checkout[billing_address][zip]", profile.Zip);
                this._diData.Add("checkout[billing_address][phone]", profile.Phone);
                this._diData.Add("checkout[shipping_rate][id]", str);
                this._diData.Add("complete", "1");
                this._diData.Add("checkout[client_details][browser_width]", "1710");
                this._diData.Add("checkout[client_details][browser_height]", "1289");
                this._diData.Add("checkout[client_details][javascript_enabled]", "1");
                this._diData.Add("button", "");
                if (this._isCaptcha)
                {
                    this._diData.Add("g-recaptcha-response", this._captchaToken);
                }
                this._srr = this._client.Post(this._checkoutLink, this._diData).Text();
                if (this._srr.ToLowerInvariant().Contains("inventory issues"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                this._currentDoc.LoadHtml(this._srr);
                bool flag2 = true;
                goto Label_053F;
            Label_04A8:
                if (flag2)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_ORDER, null, "", "");
                    flag2 = false;
                }
                Thread.Sleep(0x3e8);
                this._srr = this._client.Get(this._checkoutLink + "/processing").Text();
                if (this._srr.ToLowerInvariant().Contains("inventory issues"))
                {
                    goto Label_07B3;
                }
                goto Label_053F;
            Label_0527:
                if (this._srr.Contains("processing\">redirected"))
                {
                    goto Label_04A8;
                }
                goto Label_0561;
            Label_053F:
                if (this._srr.ToUpper().Contains("being processed".ToUpper()))
                {
                    goto Label_04A8;
                }
                goto Label_0527;
            Label_0561:
                this._currentDoc.LoadHtml(this._srr);
                this._srr = this._client.Get(this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>().Attributes["href"].Value).Text();
                this._currentDoc.LoadHtml(this._srr);
                if (this._currentDoc.DocumentNode.Descendants("span").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number")))
                {
                    return true;
                }
                if (this._srr.ToUpper().Contains("Correct your payment information".ToUpper()))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                }
                else if (this._srr.ToUpper().Contains("Card was declined".ToUpper()))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                }
                else if (this._srr.ToUpper().Contains("Your billing information does not match your credit card. Please check with your bank.".ToUpper()))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                }
                else if (!this._srr.ToUpper().Contains("There was an error processing your payment".ToUpper()))
                {
                    if (!this._srr.ToUpper().Contains("An error occurred while processing the payment information. Please try again later".ToUpper()))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                }
                return false;
            Label_07B3:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
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
                    string str2 = "";
                    if (exception is WebException)
                    {
                        str2 = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    if (string.IsNullOrEmpty(str2))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", str2);
                    }
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                flag = false;
            }
            finally
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
                this._cardId = "";
                this._runner._tokenTimestamp = null;
                this._checkoutLink = "";
            }
            return flag;
        }

        private bool SubmitOrderPalaceJP()
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
                    goto Label_07E4;
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
                ProfileObject profile = this._runner.Profile;
                string[] textArray1 = new string[] { this._pickedShippingSource, "-", this._pickedShippingName.Replace(" ", "%20").Replace("&amp;", "%26"), "-", this._pickedShippingPrice };
                string str = string.Concat(textArray1);
                this.SetClient();
                this._client.Handler.AllowAutoRedirect = false;
                this._diData.Clear();
                this._diData.Add("utf8", "✓");
                this._diData.Add("_method", "patch");
                this._diData.Add("authenticity_token", "");
                this._diData.Add("previous_step", "payment_method");
                this._diData.Add("step", "");
                this._diData.Add("s", this._cardId);
                this._diData.Add("checkout[payment_gateway]", this._paymentGateway);
                this._diData.Add("checkout[credit_card][vault]", "false");
                this._diData.Add("checkout[different_billing_address]", "true");
                this._diData.Add("checkout[billing_address][first_name]", profile.FirstName);
                this._diData.Add("checkout[billing_address][last_name]", profile.LastName);
                this._diData.Add("checkout[billing_address][address1]", profile.Address1);
                this._diData.Add("checkout[billing_address][address2]", profile.Address2);
                this._diData.Add("checkout[billing_address][city]", profile.City);
                this._diData.Add("checkout[billing_address][country]", profile.Country);
                this._diData.Add("checkout[billing_address][state]", string.IsNullOrEmpty(profile.State) ? "None" : profile.State);
                this._diData.Add("checkout[billing_address][province]", string.IsNullOrEmpty(profile.State) ? "None" : profile.State);
                this._diData.Add("checkout[billing_address][zip]", profile.Zip);
                this._diData.Add("checkout[billing_address][phone]", profile.Phone);
                this._diData.Add("checkout[shipping_rate][id]", str);
                this._diData.Add("complete", "1");
                this._diData.Add("checkout[client_details][browser_width]", "1710");
                this._diData.Add("checkout[client_details][browser_height]", "1289");
                this._diData.Add("checkout[client_details][javascript_enabled]", "1");
                this._diData.Add("button", "");
                if (this._isCaptcha)
                {
                    this._diData.Add("g-recaptcha-response", this._captchaToken);
                }
                this._srr = this._client.Post(this._checkoutLink, this._diData).Text();
                if (this._srr.ToLowerInvariant().Contains("inventory issues"))
                {
                    goto Label_07B4;
                }
                this._currentDoc.LoadHtml(this._srr);
                bool flag2 = true;
                goto Label_04E3;
            Label_044E:
                if (flag2)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_ORDER, null, "", "");
                    flag2 = false;
                }
                Thread.Sleep(0x3e8);
                this._srr = this._client.Get(this._checkoutLink + "/processing").Text();
                if (!this._srr.ToLowerInvariant().Contains("inventory issues"))
                {
                    goto Label_04E3;
                }
                goto Label_0784;
            Label_04CB:
                if (this._srr.Contains("processing\">redirected"))
                {
                    goto Label_044E;
                }
                goto Label_0505;
            Label_04E3:
                if (this._srr.ToUpper().Contains("being processed".ToUpper()))
                {
                    goto Label_044E;
                }
                goto Label_04CB;
            Label_0505:
                this._srr = this._client.Get(this._checkoutLink + "/processing?").Text();
                this._currentDoc.LoadHtml(this._srr);
                this._srr = this._client.Get(this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>().Attributes["href"].Value).Text();
                this._currentDoc.LoadHtml(this._srr);
                if (this._currentDoc.DocumentNode.Descendants("span").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number")))
                {
                    return true;
                }
                if (!this._srr.ToUpper().Contains("Correct your payment information".ToUpper()))
                {
                    if (this._srr.ToUpper().Contains("Card was declined".ToUpper()))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    }
                    else if (this._srr.ToUpper().Contains("Your billing information does not match your credit card. Please check with your bank.".ToUpper()))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    }
                    else if (this._srr.ToUpper().Contains("There was an error processing your payment".ToUpper()))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    }
                    else if (!this._srr.ToUpper().Contains("An error occurred while processing the payment information. Please try again later".ToUpper()))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                }
                return false;
            Label_0784:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            Label_07B4:
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            Label_07E4:
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
                    string str2 = "";
                    if (exception is WebException)
                    {
                        str2 = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    if (string.IsNullOrEmpty(str2))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", str2);
                    }
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                flag = false;
            }
            finally
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
                this._cardId = "";
                this._runner._tokenTimestamp = null;
                this._checkoutLink = "";
            }
            return flag;
        }

        private bool SubmitPaypal(bool auto = false)
        {
            bool flag;
            try
            {
                DateTime? nullable;
                this._diData.Clear();
                this._diData.Add($"updates[{this._runner.PickedSize.Value.Value}]", "1");
                this._diData.Add("goto_pp", "paypal_express");
                HttpResponseMessage message = this._client.Post(this._task.HomeUrl + "/cart", this._diData);
                string url = message.RequestMessage.RequestUri.ToString();
                this._srr = message.Content.ReadAsStringAsync().Result.ToString();
                while (!url.Contains("paypal.com"))
                {
                    KeyValuePair<string, string> pair = this._client.Get(this._checkoutLink + "/express/redirecting?refresh_count=1").TextResponseUri();
                    url = pair.Value;
                    this._srr = pair.Key;
                }
                if (Global.SETTINGS.PayPalBeep)
                {
                    EveAIO.Helpers.PlayBell();
                }
                this._task.Status = States.GetTaskState(States.TaskState.OPENING_PAYPAL);
                States.WriteLogger(this._task, States.LOGGER_STATES.OPENING_PAYPAL, null, "", "");
                if (this._runner._notificator != null)
                {
                    this._runner._notificator.Notify(Notificator.NotificationType.Paypal);
                }
                this._task.PaypalLink = url;
                foreach (string str2 in this._responseHeaders.GetValues("Set-Cookie"))
                {
                    string str3 = str2.Substring(0, str2.IndexOf(";"));
                    string str4 = str3.Substring(str3.IndexOf("=") + 1);
                    if (str4.Contains(","))
                    {
                        str4 = WebUtility.UrlEncode(str4);
                    }
                    nullable = null;
                    OpenQA.Selenium.Cookie cookie = new OpenQA.Selenium.Cookie(str3.Substring(0, str3.IndexOf("=")), str4, ".kith.com", "", nullable);
                    this._task.Driver.Manage().Cookies.AddCookie(cookie);
                }
                foreach (System.Net.Cookie cookie2 in this._client.Cookies.List())
                {
                    nullable = null;
                    this._task.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookie2.Name, cookie2.Value, cookie2.Domain, cookie2.Path, nullable));
                }
                this._task.Driver.Navigate().GoToUrl(url);
                flag = true;
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
                    string str5 = "";
                    if (exception is WebException)
                    {
                        str5 = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    if (string.IsNullOrEmpty(str5))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", str5);
                    }
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                flag = false;
            }
            finally
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._isLoggedIn = false;
                this._cardId = "";
                this._runner._tokenTimestamp = null;
                this._checkoutLink = "";
            }
            return flag;
        }

        private bool SubmitShipping()
        {
            try
            {
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", this._task.HomeUrl);
                try
                {
                    this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                }
                catch
                {
                    this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.HomeUrl);
                }
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                if (this._task.Payment != TaskObject.PaymentEnum.paypal)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                }
                if (!this._task.Link.ToLowerInvariant().Contains("/checkouts/") && !this._task.IsShopifyCheckoutLink)
                {
                    this._diData.Clear();
                    this._diData.Add("updates[" + this._runner.PickedSize.Value.Value + "]", "1");
                    this._diData.Add("checkout", "Checkout");
                    this._diData.Add("note", "");
                    KeyValuePair<string, HttpResponseHeaders> pair = this._client.Post(this._task.HomeUrl + "/cart/", this._diData).TextHeaders();
                    this._srr = pair.Key;
                    this._responseHeaders = pair.Value;
                }
                this._isCaptcha = this._srr.Contains("6LeoeSkTAAAAAA9rkZs5oS82l69OEYjKRZAiKdaF");
                ProfileObject profile = this._runner.Profile;
                this._diData.Clear();
                this._diData.Add("utf8", "✓");
                this._diData.Add("_method", "patch");
                this._diData.Add("authenticity_token", "");
                this._diData.Add("previous_step", "contact_information");
                this._diData.Add("step", "shipping_method");
                this._diData.Add("checkout[email]", profile.EmailShipping);
                this._diData.Add("checkout[buyer_accepts_marketing]", "0");
                this._diData.Add("checkout[shipping_address][first_name]", profile.FirstNameShipping);
                this._diData.Add("checkout[shipping_address][last_name]", profile.LastNameShipping);
                this._diData.Add("checkout[shipping_address][company]", "");
                this._diData.Add("checkout[shipping_address][address1]", profile.Address1Shipping);
                this._diData.Add("checkout[shipping_address][address2]", profile.Address2Shipping);
                this._diData.Add("checkout[shipping_address][city]", profile.CityShipping);
                this._diData.Add("checkout[shipping_address][country]", profile.CountryShipping);
                this._diData.Add("checkout[shipping_address][state]", string.IsNullOrEmpty(profile.StateShipping) ? "None" : profile.StateShipping);
                this._diData.Add("checkout[shipping_address][province]", string.IsNullOrEmpty(profile.StateShipping) ? "None" : profile.StateShipping);
                this._diData.Add("checkout[shipping_address][zip]", profile.ZipShipping);
                this._diData.Add("checkout[shipping_address][phone]", profile.PhoneShipping);
                this._diData.Add("checkout[remember_me]", "0");
                this._diData.Add("checkout[client_details][browser_width]", "1710");
                this._diData.Add("checkout[client_details][browser_height]", "1289");
                this._diData.Add("checkout[client_details][javascript_enabled]", "1");
                this._diData.Add("button", "");
                this._srr = this._client.Post(this._checkoutLink, this._diData).Text();
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
                this._cardId = "";
                this._runner._tokenTimestamp = null;
                this._checkoutLink = "";
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

        private bool WaitingForCaptcha()
        {
            this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
            States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
            object solverLocker = Global.SolverLocker;
            lock (solverLocker)
            {
                Global.CAPTCHA_QUEUE.Add(this._task);
            }
            this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
            this._task.Mre = new ManualResetEvent(false);
            string link = "";
            if (!this._task.Link.ToLowerInvariant().Contains("/checkouts/") && !this._task.IsShopifyCheckoutLink)
            {
                if ((this._task.TaskType == TaskObject.TaskTypeEnum.variant) || (this._task.Link.ToLowerInvariant().Contains("/cart/") && this._task.Link.ToLowerInvariant().Contains(":1")))
                {
                    link = this._task.Link;
                }
                else
                {
                    link = this._runner.Product.Link;
                }
            }
            else
            {
                link = this._runner.HomeUrl;
            }
            CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.SHOPIFY_CAPTCHA_KEY, link, "Shopify");
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
            this._captchaToken = waiter.Token;
            return true;
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Shopify.<>c <>9;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__25_1;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__25_2;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__25_3;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__25_4;
            public static Func<HtmlNode, bool> <>9__32_1;
            public static Func<HtmlNode, bool> <>9__33_1;
            public static Func<HtmlNode, bool> <>9__35_0;
            public static Func<HtmlNode, bool> <>9__35_1;
            public static Func<HtmlNode, bool> <>9__38_0;
            public static Func<HtmlNode, bool> <>9__38_3;
            public static Func<HtmlNode, bool> <>9__38_4;
            public static Func<HtmlNode, bool> <>9__38_5;
            public static Func<HtmlNode, bool> <>9__38_1;
            public static Func<HtmlNode, bool> <>9__38_2;
            public static Func<HtmlNode, bool> <>9__40_0;
            public static Func<HtmlNode, bool> <>9__40_1;
            public static Func<HtmlNode, bool> <>9__41_0;
            public static Func<HtmlNode, bool> <>9__41_1;
            public static Func<HtmlNode, bool> <>9__41_2;
            public static Func<HtmlNode, bool> <>9__41_3;
            public static Func<HtmlNode, bool> <>9__41_4;
            public static Func<HtmlNode, bool> <>9__42_0;
            public static Func<HtmlNode, bool> <>9__42_1;
            public static Func<HtmlNode, bool> <>9__42_2;
            public static Func<HtmlNode, bool> <>9__43_0;
            public static Func<HtmlNode, bool> <>9__43_10;
            public static Func<HtmlNode, bool> <>9__43_11;
            public static Func<HtmlNode, bool> <>9__43_1;
            public static Func<HtmlNode, bool> <>9__43_12;
            public static Func<HtmlNode, bool> <>9__43_13;
            public static Func<HtmlNode, bool> <>9__43_2;
            public static Func<HtmlNode, bool> <>9__43_14;
            public static Func<HtmlNode, bool> <>9__43_15;
            public static Func<HtmlNode, bool> <>9__43_3;
            public static Func<HtmlNode, bool> <>9__43_16;
            public static Func<HtmlNode, bool> <>9__43_17;
            public static Func<HtmlNode, bool> <>9__43_4;
            public static Func<HtmlNode, bool> <>9__43_18;
            public static Func<HtmlNode, bool> <>9__43_5;
            public static Func<HtmlNode, bool> <>9__43_19;
            public static Func<HtmlNode, bool> <>9__43_6;
            public static Func<HtmlNode, bool> <>9__43_20;
            public static Func<HtmlNode, bool> <>9__43_21;
            public static Func<HtmlNode, bool> <>9__43_7;
            public static Func<HtmlNode, bool> <>9__43_22;
            public static Func<HtmlNode, bool> <>9__43_8;
            public static Func<HtmlNode, bool> <>9__43_23;
            public static Func<HtmlNode, bool> <>9__43_9;
            public static Func<HtmlNode, bool> <>9__43_24;
            public static Func<HtmlNode, bool> <>9__43_25;
            public static Func<HtmlNode, bool> <>9__44_0;
            public static Func<HtmlNode, bool> <>9__44_4;
            public static Func<HtmlNode, bool> <>9__44_5;
            public static Func<HtmlNode, bool> <>9__44_6;
            public static Func<HtmlNode, bool> <>9__44_7;
            public static Func<HtmlNode, bool> <>9__44_8;
            public static Func<HtmlNode, bool> <>9__44_27;
            public static Func<HtmlNode, bool> <>9__44_28;
            public static Func<HtmlNode, bool> <>9__44_29;
            public static Func<HtmlNode, bool> <>9__44_32;
            public static Func<HtmlNode, bool> <>9__44_37;
            public static Func<HtmlNode, bool> <>9__44_38;
            public static Func<HtmlNode, bool> <>9__44_40;
            public static Func<HtmlNode, bool> <>9__44_41;
            public static Func<Tuple<string, string, string, double?>, bool> <>9__44_33;
            public static Func<Tuple<string, string, string, double?>, bool> <>9__44_34;
            public static Func<Tuple<string, string, string, double?>, bool> <>9__44_35;
            public static Func<Tuple<string, string, string, double?>, double?> <>9__44_36;
            public static Func<HtmlNode, bool> <>9__44_43;
            public static Func<HtmlNode, bool> <>9__44_44;
            public static Func<HtmlNode, bool> <>9__45_10;
            public static Func<HtmlNode, bool> <>9__45_11;
            public static Func<HtmlNode, bool> <>9__45_12;
            public static Func<HtmlNode, bool> <>9__45_13;
            public static Func<HtmlNode, bool> <>9__45_14;
            public static Func<HtmlNode, bool> <>9__45_15;
            public static Func<HtmlNode, bool> <>9__45_16;
            public static Func<HtmlNode, bool> <>9__45_17;
            public static Func<HtmlNode, bool> <>9__45_18;
            public static Func<HtmlNode, bool> <>9__45_19;
            public static Func<HtmlNode, bool> <>9__45_33;
            public static Func<HtmlNode, bool> <>9__45_42;
            public static Func<HtmlNode, bool> <>9__45_43;
            public static Func<HtmlNode, bool> <>9__45_34;
            public static Func<HtmlNode, bool> <>9__45_35;
            public static Func<HtmlNode, bool> <>9__45_36;
            public static Func<HtmlNode, bool> <>9__45_37;
            public static Func<HtmlNode, bool> <>9__45_38;
            public static Func<HtmlNode, bool> <>9__45_39;
            public static Func<HtmlNode, bool> <>9__45_44;
            public static Func<HtmlNode, bool> <>9__46_3;
            public static Func<HtmlNode, bool> <>9__46_4;
            public static Func<HtmlNode, bool> <>9__46_5;
            public static Func<HtmlNode, bool> <>9__46_10;
            public static Func<HtmlNode, bool> <>9__46_11;
            public static Func<HtmlNode, bool> <>9__46_12;
            public static Func<HtmlNode, bool> <>9__46_13;
            public static Func<Product, DateTime?> <>9__46_14;
            public static Func<Product, bool> <>9__46_15;
            public static Func<Product, DateTime?> <>9__46_16;
            public static Func<HtmlNode, bool> <>9__47_0;
            public static Func<HtmlNode, bool> <>9__47_1;
            public static Func<HtmlNode, bool> <>9__47_2;
            public static Func<HtmlNode, bool> <>9__47_3;
            public static Func<HtmlNode, bool> <>9__47_4;
            public static Func<HtmlNode, bool> <>9__47_5;
            public static Func<HtmlNode, bool> <>9__47_6;
            public static Func<HtmlNode, bool> <>9__47_7;
            public static Func<HtmlNode, bool> <>9__47_8;
            public static Func<HtmlNode, bool> <>9__47_9;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Shopify.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <AddToCardCheckoutLink>b__38_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "order-summary__section__content"));

            internal bool <AddToCardCheckoutLink>b__38_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__variant order-summary__small-text"));

            internal bool <AddToCardCheckoutLink>b__38_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product "));

            internal bool <AddToCardCheckoutLink>b__38_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__name order-summary__emphasis"));

            internal bool <AddToCardCheckoutLink>b__38_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__name order-summary__emphasis"));

            internal bool <AddToCardCheckoutLink>b__38_5(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "order-summary__emphasis"));

            internal bool <CheckoutInternal>b__44_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"));

            internal bool <CheckoutInternal>b__44_27(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "g-recaptcha"));

            internal bool <CheckoutInternal>b__44_28(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "g-recaptcha")) && (x.Attributes["data-sitekey"] > null));

            internal bool <CheckoutInternal>b__44_29(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "g-recaptcha"));

            internal bool <CheckoutInternal>b__44_32(HtmlNode node) => 
                ((node.Attributes["class"] != null) && (node.Attributes["class"].Value == "section section--shipping-method"));

            internal bool <CheckoutInternal>b__44_33(Tuple<string, string, string, double?> x) => 
                ((x.Item3.ToUpper().Contains("FREE") && !x.Item1.ToUpper().Contains("PICK")) && !x.Item1.ToUpper().Contains("COLLECT"));

            internal bool <CheckoutInternal>b__44_34(Tuple<string, string, string, double?> x) => 
                ((x.Item3.ToUpper().Contains("FREE") && !x.Item1.ToUpper().Contains("PICK")) && !x.Item1.ToUpper().Contains("COLLECT"));

            internal bool <CheckoutInternal>b__44_35(Tuple<string, string, string, double?> x) => 
                x.Item4.HasValue;

            internal double? <CheckoutInternal>b__44_36(Tuple<string, string, string, double?> s) => 
                s.Item4;

            internal bool <CheckoutInternal>b__44_37(HtmlNode node) => 
                ((node.Attributes["class"] != null) && (node.Attributes["class"].Value == "section section--shipping-method"));

            internal bool <CheckoutInternal>b__44_38(HtmlNode x) => 
                ((x.Attributes["type"] != null) && (x.Attributes["type"].Value == "radio"));

            internal bool <CheckoutInternal>b__44_4(HtmlNode node) => 
                ((((node.Attributes["class"] != null) && (node.Attributes["class"].Value == "edit_checkout")) && (node.Attributes["novalidate"] != null)) && (node.Attributes["novalidate"].Value == "novalidate"));

            internal bool <CheckoutInternal>b__44_40(HtmlNode node) => 
                ((node.Attributes["class"] != null) && (node.Attributes["class"].Value == "section section--shipping-method"));

            internal bool <CheckoutInternal>b__44_41(HtmlNode x) => 
                ((x.Attributes["type"] != null) && (x.Attributes["type"].Value == "radio"));

            internal bool <CheckoutInternal>b__44_43(HtmlNode node) => 
                ((node.Attributes["data-step"] != null) && (node.Attributes["data-step"].Value == "shipping_method"));

            internal bool <CheckoutInternal>b__44_44(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"));

            internal bool <CheckoutInternal>b__44_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"));

            internal bool <CheckoutInternal>b__44_6(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "total-recap__final-price"));

            internal bool <CheckoutInternal>b__44_7(HtmlNode node) => 
                ((((node.Attributes["class"] != null) && (node.Attributes["class"].Value == "edit_checkout")) && (node.Attributes["novalidate"] != null)) && (node.Attributes["novalidate"].Value == "novalidate"));

            internal bool <CheckoutInternal>b__44_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"));

            internal bool <DirectLinkGeneral>b__41_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"));

            internal bool <DirectLinkGeneral>b__41_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"));

            internal bool <DirectLinkGeneral>b__41_2(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:title"));

            internal bool <DirectLinkGeneral>b__41_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("properties["));

            internal bool <DirectLinkGeneral>b__41_4(HtmlNode x) => 
                ((x.Attributes["src"] != null) && x.Attributes["src"].Value.Contains("custom.js"));

            internal bool <DirectLinkYeezy>b__42_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-product-json"));

            internal bool <DirectLinkYeezy>b__42_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "P__img_bg"));

            internal bool <DirectLinkYeezy>b__42_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "P__img_bg"));

            internal bool <Init>b__25_1(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <Init>b__25_2(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <Init>b__25_3(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <Init>b__25_4(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <Login>b__40_0(HtmlNode x)
            {
                if (!x.InnerHtml.ToUpperInvariant().Contains("Logged in as".ToUpperInvariant()) && !x.InnerHtml.ToUpperInvariant().Contains("Logout".ToUpperInvariant()))
                {
                    return x.InnerHtml.ToUpperInvariant().Contains("Log out".ToUpperInvariant());
                }
                return true;
            }

            internal bool <Login>b__40_1(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "name")) && !string.IsNullOrEmpty(x.InnerText));

            internal bool <ParseProductPage>b__43_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productSelect"));

            internal bool <ParseProductPage>b__43_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-select"));

            internal bool <ParseProductPage>b__43_10(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productSelect"));

            internal bool <ParseProductPage>b__43_11(HtmlNode x) => 
                (x.Attributes["value"] > null);

            internal bool <ParseProductPage>b__43_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-select"));

            internal bool <ParseProductPage>b__43_13(HtmlNode x) => 
                (x.Attributes["value"] > null);

            internal bool <ParseProductPage>b__43_14(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("swatch"));

            internal bool <ParseProductPage>b__43_15(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("available"));

            internal bool <ParseProductPage>b__43_16(HtmlNode x) => 
                ((x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select-"));

            internal bool <ParseProductPage>b__43_17(HtmlNode x) => 
                (x.Attributes["value"] > null);

            internal bool <ParseProductPage>b__43_18(HtmlNode x) => 
                ((x.Attributes["Id"] != null) && (x.Attributes["Id"].Value == "ProductJson-product-template"));

            internal bool <ParseProductPage>b__43_19(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "id"));

            internal bool <ParseProductPage>b__43_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("swatch"));

            internal bool <ParseProductPage>b__43_20(HtmlNode x) => 
                ((x.Attributes["data-product-id"] != null) && !string.IsNullOrEmpty(x.Attributes["data-product-id"].Value));

            internal bool <ParseProductPage>b__43_21(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "id"));

            internal bool <ParseProductPage>b__43_22(HtmlNode x) => 
                ((x.Attributes["onClick"] != null) && x.Attributes["onClick"].Value.Contains("assignSKU"));

            internal bool <ParseProductPage>b__43_23(HtmlNode x) => 
                ((x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select"));

            internal bool <ParseProductPage>b__43_24(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("selector"));

            internal bool <ParseProductPage>b__43_25(HtmlNode x) => 
                ((x.Attributes["disabled"] == null) && (x.Attributes["disabled/"] == null));

            internal bool <ParseProductPage>b__43_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select-"));

            internal bool <ParseProductPage>b__43_4(HtmlNode x) => 
                ((x.Attributes["Id"] != null) && (x.Attributes["Id"].Value == "ProductJson-product-template"));

            internal bool <ParseProductPage>b__43_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "id"));

            internal bool <ParseProductPage>b__43_6(HtmlNode x) => 
                ((x.Attributes["data-product-id"] != null) && !string.IsNullOrEmpty(x.Attributes["data-product-id"].Value));

            internal bool <ParseProductPage>b__43_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "selectedSKU"));

            internal bool <ParseProductPage>b__43_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && x.Attributes["id"].Value.Contains("product-select"));

            internal bool <ParseProductPage>b__43_9(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("selector"));

            internal bool <PaymentMethod>b__45_10(HtmlNode x) => 
                (x.Attributes["data-payment-form"] > null);

            internal bool <PaymentMethod>b__45_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"));

            internal bool <PaymentMethod>b__45_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "checkout_total_price"));

            internal bool <PaymentMethod>b__45_13(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "checkout_total_price"));

            internal bool <PaymentMethod>b__45_14(HtmlNode x) => 
                (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "direct"));

            internal bool <PaymentMethod>b__45_15(HtmlNode x) => 
                (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "express"));

            internal bool <PaymentMethod>b__45_16(HtmlNode x) => 
                (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "direct"));

            internal bool <PaymentMethod>b__45_17(HtmlNode x) => 
                (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "direct"));

            internal bool <PaymentMethod>b__45_18(HtmlNode x) => 
                (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "express"));

            internal bool <PaymentMethod>b__45_19(HtmlNode x) => 
                (((x.Attributes["data-select-gateway"] != null) && (x.Attributes["data-gateway-group"] != null)) && (x.Attributes["data-gateway-group"].Value == "express"));

            internal bool <PaymentMethod>b__45_33(HtmlNode x) => 
                (((x.Attributes["data-product-id"] != null) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "product"));

            internal bool <PaymentMethod>b__45_34(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-title"));

            internal bool <PaymentMethod>b__45_35(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-title"));

            internal bool <PaymentMethod>b__45_36(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description"));

            internal bool <PaymentMethod>b__45_37(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description"));

            internal bool <PaymentMethod>b__45_38(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product__description__name order-summary__emphasis"));

            internal bool <PaymentMethod>b__45_39(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number"));

            internal bool <PaymentMethod>b__45_42(HtmlNode x) => 
                (((x.Attributes["data-product-id"] != null) && (x.Attributes["class"] != null)) && (x.Attributes["class"].Value == "product"));

            internal bool <PaymentMethod>b__45_43(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("product__description__name"));

            internal bool <PaymentMethod>b__45_44(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number"));

            internal bool <ProcessDeepSearchLink>b__47_0(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <ProcessDeepSearchLink>b__47_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <ProcessDeepSearchLink>b__47_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "productInfo"));

            internal bool <ProcessDeepSearchLink>b__47_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <ProcessDeepSearchLink>b__47_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"));

            internal bool <ProcessDeepSearchLink>b__47_5(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:title"));

            internal bool <ProcessDeepSearchLink>b__47_6(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name"));

            internal bool <ProcessDeepSearchLink>b__47_7(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"));

            internal bool <ProcessDeepSearchLink>b__47_8(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"));

            internal bool <ProcessDeepSearchLink>b__47_9(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <SearchInternal>b__46_10(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("ProductGridItem"));

            internal bool <SearchInternal>b__46_11(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("PGI__title"));

            internal bool <SearchInternal>b__46_12(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "grid-view-item__link"));

            internal bool <SearchInternal>b__46_13(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("h4"));

            internal DateTime? <SearchInternal>b__46_14(Product x) => 
                x.LastMod;

            internal bool <SearchInternal>b__46_15(Product x) => 
                x.LastMod.HasValue;

            internal DateTime? <SearchInternal>b__46_16(Product x) => 
                x.LastMod;

            internal bool <SearchInternal>b__46_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-new-arrivals-json"));

            internal bool <SearchInternal>b__46_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-collection-json"));

            internal bool <SearchInternal>b__46_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-collection-json"));

            internal bool <SubmitBilling>b__35_0(HtmlNode x) => 
                (x.Attributes["data-select-gateway"] > null);

            internal bool <SubmitBilling>b__35_1(HtmlNode x) => 
                (x.Attributes["data-select-gateway"] > null);

            internal bool <SubmitOrder>b__32_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number"));

            internal bool <SubmitOrderPalaceJP>b__33_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "os-order-number"));
        }

        [CompilerGenerated]
        private static class <>o__25
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__37
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__8;
            public static CallSite<Func<CallSite, object, double>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__21;
            public static CallSite<Func<CallSite, object, double, object>> <>p__22;
            public static CallSite<Func<CallSite, object, bool>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__31;
            public static CallSite<Func<CallSite, object, double>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, string>> <>p__40;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__41;
        }

        [CompilerGenerated]
        private static class <>o__39
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, int, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, string, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string>> <>p__21;
        }

        [CompilerGenerated]
        private static class <>o__41
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, bool>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__18;
            public static CallSite<Func<CallSite, object, bool>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__31;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, string, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string>> <>p__37;
        }

        [CompilerGenerated]
        private static class <>o__42
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__10;
            public static CallSite<Func<CallSite, object, bool>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object, object>> <>p__14;
            public static CallSite<Func<CallSite, bool, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__28;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__29;
        }

        [CompilerGenerated]
        private static class <>o__43
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, int, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__13;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, bool>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string>> <>p__24;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, int, object>> <>p__30;
            public static CallSite<Func<CallSite, object, bool>> <>p__31;
            public static CallSite<Func<CallSite, object, string, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__37;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__38;
            public static CallSite<Func<CallSite, object, string, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__41;
            public static CallSite<Func<CallSite, object, bool>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, bool>> <>p__52;
            public static CallSite<Func<CallSite, object, string, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, string, object>> <>p__55;
            public static CallSite<Func<CallSite, object, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__58;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, object>> <>p__63;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__64;
            public static CallSite<Func<CallSite, object, int, object>> <>p__65;
            public static CallSite<Func<CallSite, object, bool>> <>p__66;
            public static CallSite<Func<CallSite, object, string, object>> <>p__67;
            public static CallSite<Func<CallSite, object, object>> <>p__68;
            public static CallSite<Func<CallSite, object, object>> <>p__69;
            public static CallSite<Func<CallSite, object, string, object>> <>p__70;
            public static CallSite<Func<CallSite, object, object>> <>p__71;
            public static CallSite<Func<CallSite, object, object>> <>p__72;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__73;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__74;
        }

        [CompilerGenerated]
        private static class <>o__44
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__8;
            public static CallSite<Func<CallSite, object, double>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__21;
            public static CallSite<Func<CallSite, object, double, object>> <>p__22;
            public static CallSite<Func<CallSite, object, bool>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__31;
            public static CallSite<Func<CallSite, object, double>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, string>> <>p__40;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__41;
        }

        [CompilerGenerated]
        private static class <>o__46
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, string, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string>> <>p__11;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, string, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, object, bool>> <>p__28;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__33;
            public static CallSite<Func<CallSite, object, bool>> <>p__34;
            public static CallSite<Func<CallSite, object, string, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, string, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, string>> <>p__38;
            public static CallSite<Func<CallSite, object, string, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, string>> <>p__41;
            public static CallSite<Func<CallSite, object, string, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, string, object>> <>p__44;
            public static CallSite<Func<CallSite, object, bool>> <>p__45;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__46;
        }

        [CompilerGenerated]
        private static class <>o__47
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, string, object, object>> <>p__7;
            public static CallSite<Func<CallSite, string, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string>> <>p__9;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, string, object, object>> <>p__13;
            public static CallSite<Func<CallSite, string, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, string, object, object>> <>p__20;
            public static CallSite<Func<CallSite, string, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string>> <>p__22;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__23;
        }
    }
}

