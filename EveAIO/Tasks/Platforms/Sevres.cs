namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
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
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Sevres : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;
        private string _payLink;
        private string _bearerToken;
        private string _orderId;
        private string _rsaKey;
        private static string _adyen;
        private string _shippingMethod;
        private string _packageSku;
        private string _purchaseId;

        public Sevres(TaskRunner runner, TaskObject task)
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
                    this._diData.Clear();
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("babylone-auth", "Bearer");
                        this._dynObj = this._client.Post("https://api.prod.babylone.io/users/login ", this._diData).Json();
                        if (<>o__17.<>p__2 == null)
                        {
                            <>o__17.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sevres)));
                        }
                        if (<>o__17.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__17.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__17.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        this._bearerToken = <>o__17.<>p__2.Target(<>o__17.<>p__2, <>o__17.<>p__1.Target(<>o__17.<>p__1, <>o__17.<>p__0.Target(<>o__17.<>p__0, this._dynObj, "token")));
                        this._client.Session.DefaultRequestHeaders.Remove("babylone-auth");
                        Cookie cookie = new Cookie {
                            Value = this._bearerToken,
                            Domain = ".24sevres.com",
                            Name = "Babylone_Auth"
                        };
                        this._client.Handler.CookieContainer.Add(cookie);
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
                    object obj2 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__17.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__17.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Sevres), argumentInfo));
                    }
                    <>o__17.<>p__3.Target(<>o__17.<>p__3, obj2, this._runner.Profile.CountryIdShipping);
                    Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray {
                        this._runner.PickedSize.Value.Value
                    };
                    if (<>o__17.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__17.<>p__4 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "skus", typeof(Sevres), argumentInfo));
                    }
                    <>o__17.<>p__4.Target(<>o__17.<>p__4, obj2, array);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("babylone-auth", "Bearer " + this._bearerToken);
                        if (<>o__17.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__6 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__17.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__5 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Sevres), argumentInfo));
                        }
                        object obj3 = <>o__17.<>p__6.Target(<>o__17.<>p__6, this._client, "https://api.prod.babylone.io/orders/add-to-cart-v2", <>o__17.<>p__5.Target(<>o__17.<>p__5, typeof(Newtonsoft.Json.JsonConvert), obj2));
                        if (<>o__17.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__7 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Sevres), argumentInfo));
                        }
                        <>o__17.<>p__7.Target(<>o__17.<>p__7, typeof(EveAIO.Extensions), obj3);
                        if (<>o__17.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__17.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__17.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__17.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__17.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Sevres), argumentInfo));
                        }
                        this._dynObj = <>o__17.<>p__11.Target(<>o__17.<>p__11, <>o__17.<>p__10.Target(<>o__17.<>p__10, <>o__17.<>p__9.Target(<>o__17.<>p__9, <>o__17.<>p__8.Target(<>o__17.<>p__8, obj3))));
                        this._client.Session.DefaultRequestHeaders.Remove("babylone-auth");
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
                if (<>o__17.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__17.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sevres), argumentInfo));
                }
                if (<>o__17.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__17.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Sevres), argumentInfo));
                }
                if (<>o__17.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__17.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                }
                if (!<>o__17.<>p__14.Target(<>o__17.<>p__14, <>o__17.<>p__13.Target(<>o__17.<>p__13, <>o__17.<>p__12.Target(<>o__17.<>p__12, this._dynObj), "has just been add")))
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
            catch (Exception exception3)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception3 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception3.Message.Contains("404") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("404")))
                {
                    if (!exception3.Message.Contains("430") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception3, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
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
            switch (((0x32f1b253 ^ 0x6c5ff081) % 5))
            {
                case 0:
                    goto Label_001C;

                case 2:
                    return false;

                case 3:
                    break;

                case 4:
                    return false;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (this.SubmitBilling())
            {
            }
            if (0x280d7fb0 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            try
            {
                Product product1;
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", link);
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
                string str = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-tools"))).Attributes["data-variants-tree"].Value.Replace("&quot;", "\"");
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                if (<>o__22.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sevres), argumentInfo));
                }
                if (<>o__22.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__22.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Sevres), argumentInfo));
                }
                if (<>o__22.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                }
                if (<>o__22.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__22.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                }
                if (<>o__22.<>p__3.Target(<>o__22.<>p__3, <>o__22.<>p__2.Target(<>o__22.<>p__2, <>o__22.<>p__1.Target(<>o__22.<>p__1, <>o__22.<>p__0.Target(<>o__22.<>p__0, this._dynObj, "available")), true)))
                {
                    goto Label_10A3;
                }
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str2 = EveAIO.Helpers.RemoveHtmlTags(WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "brand"))).InnerText + " " + this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).Attributes["content"].Value));
                string str3 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).Attributes["data-price"].Value;
                str3 = str3.Insert(str3.Length - 2, ".");
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("section").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "main-img"))).Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                if ((this._task.TaskType == TaskObject.TaskTypeEnum.keywords) && (this._task.NegativeKeywords.Count > 0))
                {
                    using (IEnumerator<string> enumerator = this._task.NegativeKeywords.GetEnumerator())
                    {
                        char[] chArray1;
                        goto Label_04C8;
                    Label_0480:
                        chArray1 = new char[] { ' ' };
                        foreach (string str4 in enumerator.Current.ToUpperInvariant().Split(chArray1))
                        {
                            if (str2.ToUpperInvariant().Contains(str4))
                            {
                                goto Label_04D3;
                            }
                        }
                    Label_04C8:
                        if (!enumerator.MoveNext())
                        {
                            goto Label_0501;
                        }
                        goto Label_0480;
                    Label_04D3:
                        States.WriteLogger(this._task, States.LOGGER_STATES.NEGATIVE_KWS_MATCH, null, "", "");
                        return false;
                    }
                }
            Label_0501:
                product1 = new Product();
                product1.ProductTitle = str2;
                product1.Link = link;
                product1.Price = str3;
                this._runner.Product = product1;
                if (<>o__22.<>p__26 == null)
                {
                    <>o__22.<>p__26 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Sevres)));
                }
                if (<>o__22.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "_children", typeof(Sevres), argumentInfo));
                }
                foreach (object obj2 in <>o__22.<>p__26.Target(<>o__22.<>p__26, <>o__22.<>p__4.Target(<>o__22.<>p__4, this._dynObj)))
                {
                    if (<>o__22.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sevres), argumentInfo));
                    }
                    if (<>o__22.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__22.<>p__6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Sevres), argumentInfo));
                    }
                    if (<>o__22.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "_childer", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__22.<>p__7.Target(<>o__22.<>p__7, <>o__22.<>p__6.Target(<>o__22.<>p__6, <>o__22.<>p__5.Target(<>o__22.<>p__5, obj2), null)))
                    {
                        if (<>o__22.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__15 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__22.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__11 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__22.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__22.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__22.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__22.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__22.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__22.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        this._runner.Product.AvailableSizes.Add(<>o__22.<>p__15.Target(<>o__22.<>p__15, typeof(KeyValuePair<string, string>), <>o__22.<>p__11.Target(<>o__22.<>p__11, <>o__22.<>p__10.Target(<>o__22.<>p__10, <>o__22.<>p__9.Target(<>o__22.<>p__9, <>o__22.<>p__8.Target(<>o__22.<>p__8, obj2, "label"))), @"\/", "/"), <>o__22.<>p__14.Target(<>o__22.<>p__14, <>o__22.<>p__13.Target(<>o__22.<>p__13, <>o__22.<>p__12.Target(<>o__22.<>p__12, obj2, "sku")))));
                    }
                    else
                    {
                        if (<>o__22.<>p__25 == null)
                        {
                            <>o__22.<>p__25 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Sevres)));
                        }
                        if (<>o__22.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "_children", typeof(Sevres), argumentInfo));
                        }
                        foreach (object obj3 in <>o__22.<>p__25.Target(<>o__22.<>p__25, <>o__22.<>p__16.Target(<>o__22.<>p__16, obj2)))
                        {
                            if (<>o__22.<>p__24 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__24 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__22.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__20 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__22.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__22.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                            }
                            if (<>o__22.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__22.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__22.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                            }
                            if (<>o__22.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                            }
                            this._runner.Product.AvailableSizes.Add(<>o__22.<>p__24.Target(<>o__22.<>p__24, typeof(KeyValuePair<string, string>), <>o__22.<>p__20.Target(<>o__22.<>p__20, <>o__22.<>p__19.Target(<>o__22.<>p__19, <>o__22.<>p__18.Target(<>o__22.<>p__18, <>o__22.<>p__17.Target(<>o__22.<>p__17, obj3, "label"))), @"\/", "/"), <>o__22.<>p__23.Target(<>o__22.<>p__23, <>o__22.<>p__22.Target(<>o__22.<>p__22, <>o__22.<>p__21.Target(<>o__22.<>p__21, obj3, "sku")))));
                        }
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
                    double num3 = double.Parse(str5.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                        using (List<KeyValuePair<string, string>>.Enumerator enumerator4 = this._runner.Product.AvailableSizes.GetEnumerator())
                        {
                            KeyValuePair<string, string> current;
                            while (enumerator4.MoveNext())
                            {
                                current = enumerator4.Current;
                                List<string> source = new List<string>();
                                if (current.Key.Contains(":"))
                                {
                                    char[] chArray3 = new char[] { ':' };
                                    string[] strArray3 = current.Key.Split(chArray3);
                                    for (int k = 0; k < strArray3.Length; k++)
                                    {
                                        source.Add(strArray3[k].Trim());
                                    }
                                }
                                else
                                {
                                    source.Add(current.Key);
                                }
                                for (int j = 0; j < source.Count; j++)
                                {
                                    source[j] = source[j].Trim().ToUpperInvariant();
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_0FEE;
                                }
                            }
                            continue;
                        Label_0FEE:
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
            Label_10A3:
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                }
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception2, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("404") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("404")))
                {
                    if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception2, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        public bool Login() => 
            true;

        public bool Search()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                foreach (string str in this._task.Keywords)
                {
                    string url = $"https://www.boxlunch.com/search?q={str.Replace(" ", "+").ToLowerInvariant()}";
                    KeyValuePair<string, string> pair = this._client.Get(url).TextResponseUri();
                    this._srr = pair.Key;
                    if (pair.Value.Contains("/product/"))
                    {
                        return this.DirectLink(pair.Value);
                    }
                    HtmlDocument document1 = new HtmlDocument();
                    document1.LoadHtml(this._srr);
                    foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("div")
                        where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-tile")
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.24sevres.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.24sevres.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                string cCNumber = profile.CCNumber;
                string cc = "";
                while (cCNumber.Length > 4)
                {
                    cc = cc + cCNumber.Substring(0, 4);
                    cCNumber = cCNumber.Remove(0, 4);
                    cc = cc + " ";
                }
                cc = cc + cCNumber;
                this._rsaKey = "10001|A2892D8452D5CD8EACEA4486528722AAFBCCA953970A374759906AC2A2F298D0B5E1277387C7382254266A85BFBE91505C32CFA3E60FC090EB69029EF2DA9B399D10C9A41B63047281B5B73E2833D29E26E9093E777D40B88F4EEF9AAD83C481275EF4E305817BCCD229918A2DE98C86FBB0D8C4956F2A6ED008379B7D941E6206D48288E70CD97A0431E7CA52280D13505A828BDBDED116302FDA256E88C6C50038845E3F5D477A77587EE52A72EB554DDD3FC4E63B9DF50844B9865E204E2508AEC779250508DBE25581BFD1E153D5A9D6A0A4FF4111A2FFE82942E6058CE4970BD446C115AE3A8CBAA16F1DC9AB7A47095C6DAAAAFE63003AF17F94683941";
                string str3 = Global.SENSOR.EncryptSivas(this._rsaKey, _adyen, cc, this._runner.Profile.Cvv, this._runner.Profile.NameOnCard, this._runner.Profile.ExpiryMonth, this._runner.Profile.ExpiryYear);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str3);
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    object obj2 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__20.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "orderId", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__0.Target(<>o__20.<>p__0, obj2, "/orders/" + this._orderId);
                    if (<>o__20.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "email", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__1.Target(<>o__20.<>p__1, obj2, profile.EmailShipping);
                    if (<>o__20.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__2.Target(<>o__20.<>p__2, obj2, "credit_card");
                    if (<>o__20.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__3 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "saveCreditCard", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__3.Target(<>o__20.<>p__3, obj2, false);
                    if (<>o__20.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "willBeLoyalty", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__4.Target(<>o__20.<>p__4, obj2, false);
                    if (<>o__20.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "adyenEncryptedData", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__7.Target(<>o__20.<>p__7, obj2, <>o__20.<>p__6.Target(<>o__20.<>p__6, <>o__20.<>p__5.Target(<>o__20.<>p__5, this._dynObj, "adyen-encrypted-data")));
                    if (<>o__20.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "locale", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__8.Target(<>o__20.<>p__8, obj2, "en");
                    if (<>o__20.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__9 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "giftCardsIds", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__9.Target(<>o__20.<>p__9, obj2, new Newtonsoft.Json.Linq.JArray());
                    if (<>o__20.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__10 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "selectedDeliveryDaySlot", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__10.Target(<>o__20.<>p__10, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__20.<>p__12 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "date", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "selectedDeliveryDaySlot", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__12.Target(<>o__20.<>p__12, <>o__20.<>p__11.Target(<>o__20.<>p__11, obj2), null);
                    if (<>o__20.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__20.<>p__14 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "slot", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "selectedDeliveryDaySlot", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__14.Target(<>o__20.<>p__14, <>o__20.<>p__13.Target(<>o__20.<>p__13, obj2), null);
                    if (<>o__20.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__15 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__15.Target(<>o__20.<>p__15, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__17.Target(<>o__20.<>p__17, <>o__20.<>p__16.Target(<>o__20.<>p__16, obj2), profile.LastName);
                    if (<>o__20.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__19.Target(<>o__20.<>p__19, <>o__20.<>p__18.Target(<>o__20.<>p__18, obj2), profile.City);
                    if (<>o__20.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "zipcode", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__21.Target(<>o__20.<>p__21, <>o__20.<>p__20.Target(<>o__20.<>p__20, obj2), profile.Zip);
                    if (<>o__20.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "state", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__23.Target(<>o__20.<>p__23, <>o__20.<>p__22.Target(<>o__20.<>p__22, obj2), profile.StateId);
                    if (<>o__20.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__25.Target(<>o__20.<>p__25, <>o__20.<>p__24.Target(<>o__20.<>p__24, obj2), (profile.Address1 + " " + profile.Address2).Trim());
                    if (<>o__20.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__27.Target(<>o__20.<>p__27, <>o__20.<>p__26.Target(<>o__20.<>p__26, obj2), profile.CountryId);
                    if (<>o__20.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phoneNumber", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__29.Target(<>o__20.<>p__29, <>o__20.<>p__28.Target(<>o__20.<>p__28, obj2), profile.Phone);
                    if (<>o__20.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__31.Target(<>o__20.<>p__31, <>o__20.<>p__30.Target(<>o__20.<>p__30, obj2), profile.FirstName);
                    if (<>o__20.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "civility", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__33.Target(<>o__20.<>p__33, <>o__20.<>p__32.Target(<>o__20.<>p__32, obj2), "man");
                    if (<>o__20.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__35 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__35.Target(<>o__20.<>p__35, <>o__20.<>p__34.Target(<>o__20.<>p__34, obj2), new Newtonsoft.Json.Linq.JProperty("@id", "temporary-address-id-for-guest"));
                    object obj3 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__20.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__36 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "vendor", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__36.Target(<>o__20.<>p__36, obj3, "24_sevres");
                    if (<>o__20.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "orderId", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__37.Target(<>o__20.<>p__37, obj3, "/orders/" + this._purchaseId);
                    if (<>o__20.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__38 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__38.Target(<>o__20.<>p__38, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__40.Target(<>o__20.<>p__40, <>o__20.<>p__39.Target(<>o__20.<>p__39, obj3), profile.LastNameShipping);
                    if (<>o__20.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__42 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__42.Target(<>o__20.<>p__42, <>o__20.<>p__41.Target(<>o__20.<>p__41, obj3), profile.CityShipping);
                    if (<>o__20.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__44 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "zipcode", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__44.Target(<>o__20.<>p__44, <>o__20.<>p__43.Target(<>o__20.<>p__43, obj3), profile.ZipShipping);
                    if (<>o__20.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__46 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "state", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__46.Target(<>o__20.<>p__46, <>o__20.<>p__45.Target(<>o__20.<>p__45, obj3), profile.StateIdShipping);
                    if (<>o__20.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__48.Target(<>o__20.<>p__48, <>o__20.<>p__47.Target(<>o__20.<>p__47, obj3), (profile.Address1Shipping + " " + profile.Address2Shipping).Trim());
                    if (<>o__20.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__50 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__50.Target(<>o__20.<>p__50, <>o__20.<>p__49.Target(<>o__20.<>p__49, obj3), profile.CountryIdShipping);
                    if (<>o__20.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__52 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phoneNumber", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__52.Target(<>o__20.<>p__52, <>o__20.<>p__51.Target(<>o__20.<>p__51, obj3), profile.PhoneShipping);
                    if (<>o__20.<>p__54 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__54 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__53 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__53 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__54.Target(<>o__20.<>p__54, <>o__20.<>p__53.Target(<>o__20.<>p__53, obj3), profile.FirstNameShipping);
                    if (<>o__20.<>p__56 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__56 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "civility", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__55 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__56.Target(<>o__20.<>p__56, <>o__20.<>p__55.Target(<>o__20.<>p__55, obj3), "man");
                    if (<>o__20.<>p__58 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__58 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__57 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__58.Target(<>o__20.<>p__58, <>o__20.<>p__57.Target(<>o__20.<>p__57, obj3), new Newtonsoft.Json.Linq.JProperty("@id", "temporary-address-id-for-guest"));
                    if (<>o__20.<>p__59 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__59 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingMethod", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__59.Target(<>o__20.<>p__59, obj3, this._shippingMethod);
                    if (<>o__20.<>p__60 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__60 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isClickAndCollect", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__60.Target(<>o__20.<>p__60, obj3, false);
                    if (<>o__20.<>p__61 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__61 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "package", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__61.Target(<>o__20.<>p__61, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__63 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__63 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "sku", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__62 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "package", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__63.Target(<>o__20.<>p__63, <>o__20.<>p__62.Target(<>o__20.<>p__62, obj3), this._packageSku);
                    if (<>o__20.<>p__64 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__64 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "giftMessage", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__64.Target(<>o__20.<>p__64, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__66 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__66 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "sku", typeof(Sevres), argumentInfo));
                    }
                    if (<>o__20.<>p__65 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__65 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "giftMessage", typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__66.Target(<>o__20.<>p__66, <>o__20.<>p__65.Target(<>o__20.<>p__65, obj3), "ZHWMANEUZZZZZ");
                    if (<>o__20.<>p__68 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__68 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "vendors", typeof(Sevres), argumentInfo));
                    }
                    Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray();
                    if (<>o__20.<>p__67 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__67 = CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Sevres), argumentInfo));
                    }
                    <>o__20.<>p__67.Target(<>o__20.<>p__67, array, obj3);
                    <>o__20.<>p__68.Target(<>o__20.<>p__68, obj2, array);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Remove("Upgrade-Insecure-Requests");
                        this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("babylone-locale", "en");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.24sevres.com/en-sk/order/checkout");
                        if (<>o__20.<>p__70 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__70 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__20.<>p__69 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__69 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Sevres), argumentInfo));
                        }
                        object obj5 = <>o__20.<>p__70.Target(<>o__20.<>p__70, this._client, "https://api.prod.babylone.io/orders/checkout-validation-v2", <>o__20.<>p__69.Target(<>o__20.<>p__69, typeof(Newtonsoft.Json.JsonConvert), obj2));
                        if (<>o__20.<>p__71 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__71 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Sevres), argumentInfo));
                        }
                        <>o__20.<>p__71.Target(<>o__20.<>p__71, typeof(EveAIO.Extensions), obj5);
                        if (<>o__20.<>p__76 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__76 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__20.<>p__75 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__20.<>p__74 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__20.<>p__73 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__73 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__20.<>p__72 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Sevres), argumentInfo));
                        }
                        this._dynObj = <>o__20.<>p__76.Target(<>o__20.<>p__76, typeof(Newtonsoft.Json.JsonConvert), <>o__20.<>p__75.Target(<>o__20.<>p__75, <>o__20.<>p__74.Target(<>o__20.<>p__74, <>o__20.<>p__73.Target(<>o__20.<>p__73, <>o__20.<>p__72.Target(<>o__20.<>p__72, obj5)))));
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
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("404") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("404")))
                {
                    if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception2, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        private bool SubmitOrder()
        {
            bool flag2;
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("cardBin", this._runner.Profile.CCNumber.Substring(0, 6));
                    this._diData.Add("addToEmailList", "false");
                    this._diData.Add("submit", "Place Order");
                    if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                    {
                        goto Label_02F0;
                    }
                    if (this._task.CheckoutDelay > 0)
                    {
                        Thread.Sleep(this._task.CheckoutDelay);
                    }
                    if (Global.SERIAL == "EVE-1111111111111")
                    {
                        goto Label_0321;
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
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._payLink);
                        this._srr = this._client.Post("https://www.boxlunch.com/orderconfirmation", this._diData).Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (!exception.Message.Contains("504") && !exception.Message.Contains("503"))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            return false;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._srr.Contains("There is currently a Box Lunch account for this email address, please login or reset your password"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_REQUIRED, null, "", "There is currently a Box Lunch account for this email address, please login or reset your password");
                    this._task.Status = States.GetTaskState(States.TaskState.LOGIN_REQUIRED);
                    return false;
                }
                if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form")) && !string.IsNullOrEmpty(x.InnerText.Trim())))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                try
                {
                    EveAIO.Helpers.AddDbValue("Boxlunch|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                return true;
            Label_02F0:
                this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                return false;
            Label_0321:
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                flag2 = false;
            }
            catch (ThreadAbortException)
            {
                flag2 = false;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("404") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("404")))
                {
                    if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception2, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                flag2 = false;
            }
            finally
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
            }
            return flag2;
        }

        private bool SubmitShipping()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Get("https://www.24sevres.com/en-us/order/identification").Text();
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
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "guest_signin__token"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.24sevres.com/en-us/order/identification");
                        this._diData.Clear();
                        this._diData.Add("guest_signin[email]", profile.Email);
                        this._diData.Add("guest_signin[_token]", str);
                        this._diData.Add("guest-signin", "");
                        this._srr = this._client.Post("https://www.24sevres.com/en-us/order/identification", this._diData).Text();
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
                    object obj3 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__21.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Sevres), argumentInfo));
                    }
                    <>o__21.<>p__0.Target(<>o__21.<>p__0, obj3, profile.CountryIdShipping);
                    if (<>o__21.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "currency", typeof(Sevres), argumentInfo));
                    }
                    <>o__21.<>p__1.Target(<>o__21.<>p__1, obj3, "USD");
                    if (<>o__21.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "guest", typeof(Sevres), argumentInfo));
                    }
                    <>o__21.<>p__2.Target(<>o__21.<>p__2, obj3, true);
                    if (<>o__21.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__3 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "estimatedDeliveryDateRequested", typeof(Sevres), argumentInfo));
                    }
                    <>o__21.<>p__3.Target(<>o__21.<>p__3, obj3, false);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("babylone-auth", "Bearer " + this._bearerToken);
                        if (<>o__21.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__5 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Sevres), argumentInfo));
                        }
                        object obj4 = <>o__21.<>p__5.Target(<>o__21.<>p__5, this._client, "https://api.prod.babylone.io/orders/proceed_to_checkout_v2", <>o__21.<>p__4.Target(<>o__21.<>p__4, typeof(Newtonsoft.Json.JsonConvert), obj3));
                        if (<>o__21.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__6 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Sevres), argumentInfo));
                        }
                        <>o__21.<>p__6.Target(<>o__21.<>p__6, typeof(EveAIO.Extensions), obj4);
                        if (<>o__21.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__11 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Sevres), argumentInfo));
                        }
                        this._dynObj = <>o__21.<>p__11.Target(<>o__21.<>p__11, typeof(Newtonsoft.Json.JsonConvert), <>o__21.<>p__10.Target(<>o__21.<>p__10, <>o__21.<>p__9.Target(<>o__21.<>p__9, <>o__21.<>p__8.Target(<>o__21.<>p__8, <>o__21.<>p__7.Target(<>o__21.<>p__7, obj4)))));
                        if (<>o__21.<>p__17 == null)
                        {
                            <>o__21.<>p__17 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sevres)));
                        }
                        if (<>o__21.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "purchase", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "vendors", typeof(Sevres), argumentInfo));
                        }
                        this._purchaseId = <>o__21.<>p__17.Target(<>o__21.<>p__17, <>o__21.<>p__16.Target(<>o__21.<>p__16, <>o__21.<>p__15.Target(<>o__21.<>p__15, <>o__21.<>p__14.Target(<>o__21.<>p__14, <>o__21.<>p__13.Target(<>o__21.<>p__13, <>o__21.<>p__12.Target(<>o__21.<>p__12, this._dynObj))), "@id")));
                        this._purchaseId = this._purchaseId.Substring(this._purchaseId.LastIndexOf("/") + 1);
                        if (<>o__21.<>p__24 == null)
                        {
                            <>o__21.<>p__24 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sevres)));
                        }
                        if (<>o__21.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "packagingOptions", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "vendors", typeof(Sevres), argumentInfo));
                        }
                        this._packageSku = <>o__21.<>p__24.Target(<>o__21.<>p__24, <>o__21.<>p__23.Target(<>o__21.<>p__23, <>o__21.<>p__22.Target(<>o__21.<>p__22, <>o__21.<>p__21.Target(<>o__21.<>p__21, <>o__21.<>p__20.Target(<>o__21.<>p__20, <>o__21.<>p__19.Target(<>o__21.<>p__19, <>o__21.<>p__18.Target(<>o__21.<>p__18, this._dynObj)))), "sku")));
                        if (<>o__21.<>p__28 == null)
                        {
                            <>o__21.<>p__28 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sevres)));
                        }
                        if (<>o__21.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "purchase", typeof(Sevres), argumentInfo));
                        }
                        this._orderId = <>o__21.<>p__28.Target(<>o__21.<>p__28, <>o__21.<>p__27.Target(<>o__21.<>p__27, <>o__21.<>p__26.Target(<>o__21.<>p__26, <>o__21.<>p__25.Target(<>o__21.<>p__25, this._dynObj), "@id")));
                        this._orderId = this._orderId.Substring(this._orderId.LastIndexOf("/") + 1);
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
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        string url = "https://api.prod.babylone.io/orders/delivery-options?";
                        url = (((((url + "itemSkus=" + this._runner.PickedSize.Value.Value) + "&userZipcode=" + WebUtility.UrlEncode(profile.ZipShipping)) + "&vendor=24_sevres") + "&userCity=" + WebUtility.UrlEncode(profile.CityShipping)) + "&currency=USD") + "&country=" + profile.CountryIdShipping;
                        this._dynObj = this._client.Get(url).Json();
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
                double num2 = 0.0;
                string str3 = "";
                if (<>o__21.<>p__65 == null)
                {
                    <>o__21.<>p__65 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Sevres)));
                }
                foreach (object obj5 in <>o__21.<>p__65.Target(<>o__21.<>p__65, this._dynObj))
                {
                    if (!string.IsNullOrEmpty(str3))
                    {
                        if (<>o__21.<>p__45 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__45 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__44 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__21.<>p__44 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__43 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__42 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__42 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        if (!<>o__21.<>p__45.Target(<>o__21.<>p__45, <>o__21.<>p__44.Target(<>o__21.<>p__44, <>o__21.<>p__43.Target(<>o__21.<>p__43, <>o__21.<>p__42.Target(<>o__21.<>p__42, obj5, "price")), null)))
                        {
                            if (<>o__21.<>p__49 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__49 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__48 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__47 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                            }
                        }
                        object obj6 = (<>o__21.<>p__46 != null) ? 0 : <>o__21.<>p__49.Target(<>o__21.<>p__49, typeof(double), <>o__21.<>p__48.Target(<>o__21.<>p__48, <>o__21.<>p__47.Target(<>o__21.<>p__47, <>o__21.<>p__46.Target(<>o__21.<>p__46, obj5, "price"))), CultureInfo.InvariantCulture);
                        if (<>o__21.<>p__51 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__51 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__50 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__50 = CallSite<Func<CallSite, object, double, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__51.Target(<>o__21.<>p__51, <>o__21.<>p__50.Target(<>o__21.<>p__50, obj6, num2)))
                        {
                            if (<>o__21.<>p__55 == null)
                            {
                                <>o__21.<>p__55 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sevres)));
                            }
                            if (<>o__21.<>p__54 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__53 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__53 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__52 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__52 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                            }
                            str3 = <>o__21.<>p__55.Target(<>o__21.<>p__55, <>o__21.<>p__54.Target(<>o__21.<>p__54, <>o__21.<>p__53.Target(<>o__21.<>p__53, <>o__21.<>p__52.Target(<>o__21.<>p__52, obj5, "id"))));
                            if (<>o__21.<>p__64 == null)
                            {
                                <>o__21.<>p__64 = CallSite<Func<CallSite, object, double>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Sevres)));
                            }
                            if (<>o__21.<>p__59 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__59 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__58 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                <>o__21.<>p__58 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__57 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__56 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__56 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                            }
                            if (!<>o__21.<>p__59.Target(<>o__21.<>p__59, <>o__21.<>p__58.Target(<>o__21.<>p__58, <>o__21.<>p__57.Target(<>o__21.<>p__57, <>o__21.<>p__56.Target(<>o__21.<>p__56, obj5, "price")), null)))
                            {
                                if (<>o__21.<>p__63 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__21.<>p__63 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Sevres), argumentInfo));
                                }
                                if (<>o__21.<>p__62 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__21.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                                }
                                if (<>o__21.<>p__61 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__21.<>p__61 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                                }
                            }
                            num2 = <>o__21.<>p__64.Target(<>o__21.<>p__64, (<>o__21.<>p__60 != null) ? 0 : <>o__21.<>p__63.Target(<>o__21.<>p__63, typeof(double), <>o__21.<>p__62.Target(<>o__21.<>p__62, <>o__21.<>p__61.Target(<>o__21.<>p__61, <>o__21.<>p__60.Target(<>o__21.<>p__60, obj5, "price"))), CultureInfo.InvariantCulture));
                        }
                    }
                    else
                    {
                        if (<>o__21.<>p__32 == null)
                        {
                            <>o__21.<>p__32 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sevres)));
                        }
                        if (<>o__21.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        str3 = <>o__21.<>p__32.Target(<>o__21.<>p__32, <>o__21.<>p__31.Target(<>o__21.<>p__31, <>o__21.<>p__30.Target(<>o__21.<>p__30, <>o__21.<>p__29.Target(<>o__21.<>p__29, obj5, "id"))));
                        if (<>o__21.<>p__41 == null)
                        {
                            <>o__21.<>p__41 = CallSite<Func<CallSite, object, double>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Sevres)));
                        }
                        if (<>o__21.<>p__36 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__36 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__35 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__21.<>p__35 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__34 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                        }
                        if (<>o__21.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Sevres), argumentInfo));
                        }
                        if (!<>o__21.<>p__36.Target(<>o__21.<>p__36, <>o__21.<>p__35.Target(<>o__21.<>p__35, <>o__21.<>p__34.Target(<>o__21.<>p__34, <>o__21.<>p__33.Target(<>o__21.<>p__33, obj5, "price")), null)))
                        {
                            if (<>o__21.<>p__40 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__40 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__39 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__38 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Sevres), argumentInfo));
                            }
                        }
                        num2 = <>o__21.<>p__41.Target(<>o__21.<>p__41, (<>o__21.<>p__37 != null) ? 0 : <>o__21.<>p__40.Target(<>o__21.<>p__40, typeof(double), <>o__21.<>p__39.Target(<>o__21.<>p__39, <>o__21.<>p__38.Target(<>o__21.<>p__38, <>o__21.<>p__37.Target(<>o__21.<>p__37, obj5, "price"))), CultureInfo.InvariantCulture));
                    }
                }
                if (!string.IsNullOrEmpty(str3))
                {
                    this._shippingMethod = str3;
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        object obj7 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__21.<>p__66 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__66 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Sevres), argumentInfo));
                        }
                        <>o__21.<>p__66.Target(<>o__21.<>p__66, obj7, profile.CountryIdShipping);
                        if (<>o__21.<>p__67 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__67 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "currency", typeof(Sevres), argumentInfo));
                        }
                        <>o__21.<>p__67.Target(<>o__21.<>p__67, obj7, "USD");
                        if (<>o__21.<>p__68 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__68 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingMethod", typeof(Sevres), argumentInfo));
                        }
                        <>o__21.<>p__68.Target(<>o__21.<>p__68, obj7, str3);
                        if (<>o__21.<>p__69 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__69 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "willBeLoyalty", typeof(Sevres), argumentInfo));
                        }
                        <>o__21.<>p__69.Target(<>o__21.<>p__69, obj7, false);
                        if (<>o__21.<>p__70 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__70 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "state", typeof(Sevres), argumentInfo));
                        }
                        <>o__21.<>p__70.Target(<>o__21.<>p__70, obj7, ((profile.CountryIdShipping == "US") || (profile.CountryIdShipping == "CA")) ? profile.StateIdShipping : null);
                        try
                        {
                            if (<>o__21.<>p__72 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__72 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PutJson", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__71 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__71 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Sevres), argumentInfo));
                            }
                            object obj8 = <>o__21.<>p__72.Target(<>o__21.<>p__72, this._client, $"https://api.prod.babylone.io/orders/orders/{this._purchaseId}/costV2", <>o__21.<>p__71.Target(<>o__21.<>p__71, typeof(Newtonsoft.Json.JsonConvert), obj7));
                            if (<>o__21.<>p__73 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__73 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Sevres), argumentInfo));
                            }
                            <>o__21.<>p__73.Target(<>o__21.<>p__73, typeof(EveAIO.Extensions), obj8);
                            if (<>o__21.<>p__78 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__78 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__77 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__77 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__76 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__76 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__75 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Sevres), argumentInfo));
                            }
                            if (<>o__21.<>p__74 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Sevres), argumentInfo));
                            }
                            this._dynObj = <>o__21.<>p__78.Target(<>o__21.<>p__78, typeof(Newtonsoft.Json.JsonConvert), <>o__21.<>p__77.Target(<>o__21.<>p__77, <>o__21.<>p__76.Target(<>o__21.<>p__76, <>o__21.<>p__75.Target(<>o__21.<>p__75, <>o__21.<>p__74.Target(<>o__21.<>p__74, obj8)))));
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
                States.WriteLogger(this._task, States.LOGGER_STATES.COUNTRY_NOT_SUPPORTED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception6)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception6 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, null, "", "Web request timed out");
                }
                else if (!exception6.Message.Contains("404") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("404")))
                {
                    if (!exception6.Message.Contains("430") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception6, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Sevres.<>c <>9;
            public static Func<HtmlNode, bool> <>9__19_1;
            public static Func<HtmlNode, bool> <>9__21_0;
            public static Func<HtmlNode, bool> <>9__22_1;
            public static Func<HtmlNode, bool> <>9__22_2;
            public static Func<HtmlNode, bool> <>9__22_3;
            public static Func<HtmlNode, bool> <>9__22_4;
            public static Func<HtmlNode, bool> <>9__22_5;
            public static Func<HtmlNode, bool> <>9__24_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Sevres.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__22_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product-tools"));

            internal bool <DirectLink>b__22_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "brand"));

            internal bool <DirectLink>b__22_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLink>b__22_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__22_5(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "main-img"));

            internal bool <Search>b__24_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-tile"));

            internal bool <SubmitOrder>b__19_1(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form")) && !string.IsNullOrEmpty(x.InnerText.Trim()));

            internal bool <SubmitShipping>b__21_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "guest_signin__token"));
        }

        [CompilerGenerated]
        private static class <>o__17
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__4;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__5;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__6;
            public static CallSite<Action<CallSite, Type, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
        }

        [CompilerGenerated]
        private static class <>o__20
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__9;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__35;
            public static CallSite<Func<CallSite, object, string, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, string, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, string, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, string, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, string, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, string, object>> <>p__52;
            public static CallSite<Func<CallSite, object, object>> <>p__53;
            public static CallSite<Func<CallSite, object, string, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, string, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__58;
            public static CallSite<Func<CallSite, object, string, object>> <>p__59;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__60;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, string, object>> <>p__63;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__64;
            public static CallSite<Func<CallSite, object, object>> <>p__65;
            public static CallSite<Func<CallSite, object, string, object>> <>p__66;
            public static CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>> <>p__67;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__68;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__69;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__70;
            public static CallSite<Action<CallSite, Type, object>> <>p__71;
            public static CallSite<Func<CallSite, object, object>> <>p__72;
            public static CallSite<Func<CallSite, object, object>> <>p__73;
            public static CallSite<Func<CallSite, object, object>> <>p__74;
            public static CallSite<Func<CallSite, object, object>> <>p__75;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__76;
        }

        [CompilerGenerated]
        private static class <>o__21
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__3;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__4;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__5;
            public static CallSite<Action<CallSite, Type, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, string, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, string>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, bool>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__40;
            public static CallSite<Func<CallSite, object, double>> <>p__41;
            public static CallSite<Func<CallSite, object, string, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, bool>> <>p__45;
            public static CallSite<Func<CallSite, object, string, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object>> <>p__48;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__49;
            public static CallSite<Func<CallSite, object, double, object>> <>p__50;
            public static CallSite<Func<CallSite, object, bool>> <>p__51;
            public static CallSite<Func<CallSite, object, string, object>> <>p__52;
            public static CallSite<Func<CallSite, object, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, string>> <>p__55;
            public static CallSite<Func<CallSite, object, string, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, object, object>> <>p__58;
            public static CallSite<Func<CallSite, object, bool>> <>p__59;
            public static CallSite<Func<CallSite, object, string, object>> <>p__60;
            public static CallSite<Func<CallSite, object, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__63;
            public static CallSite<Func<CallSite, object, double>> <>p__64;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__65;
            public static CallSite<Func<CallSite, object, string, object>> <>p__66;
            public static CallSite<Func<CallSite, object, string, object>> <>p__67;
            public static CallSite<Func<CallSite, object, string, object>> <>p__68;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__69;
            public static CallSite<Func<CallSite, object, string, object>> <>p__70;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__71;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__72;
            public static CallSite<Action<CallSite, Type, object>> <>p__73;
            public static CallSite<Func<CallSite, object, object>> <>p__74;
            public static CallSite<Func<CallSite, object, object>> <>p__75;
            public static CallSite<Func<CallSite, object, object>> <>p__76;
            public static CallSite<Func<CallSite, object, object>> <>p__77;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__78;
        }

        [CompilerGenerated]
        private static class <>o__22
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__24;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__25;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__26;
        }
    }
}

