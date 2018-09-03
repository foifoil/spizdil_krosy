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
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class MrPorter : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private object _link;
        private string _region;
        private Dictionary<string, string> _diData;
        private bool _isLoggedIn;
        private string _channel;
        private string _local;

        public MrPorter(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._diData = new Dictionary<string, string>();
            this._runner = runner;
            this._task = task;
            this.SetClient();
            if (this._task.SupremeRegion != TaskObject.SupremeRegionEnum.USA)
            {
                this._region = "intl";
            }
            else
            {
                this._region = "am";
            }
        }

        public bool Atc()
        {
            try
            {
                if (!this._task.Link.ToLowerInvariant().Contains("http") && this._task.Link.ToLowerInvariant().Contains("-"))
                {
                    this._runner.PickedSize = new KeyValuePair<string, string>("?", this._task.Link);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ATC_PID, null, this._runner.PickedSize.Value.Value, "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                    if (this._task.SupremeRegion != TaskObject.SupremeRegionEnum.USA)
                    {
                        this._region = "intl";
                    }
                    else
                    {
                        this._region = "am";
                    }
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                }
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._dynObj = this._client.Get($"https://www.mrporter.com/{this._region}/api/basket/addsku/{this._runner.PickedSize.Value.Value}.json").Json();
                        if (<>o__14.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__14.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__2.Target(<>o__14.<>p__2, <>o__14.<>p__1.Target(<>o__14.<>p__1, <>o__14.<>p__0.Target(<>o__14.<>p__0, this._dynObj), "This size is sold out")))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.SIZE_OOS, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.SIZE_OOS);
                            return false;
                        }
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
                if (<>o__14.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__14.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(MrPorter), argumentInfo));
                }
                if (<>o__14.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__14.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(MrPorter), argumentInfo));
                }
                if (<>o__14.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__14.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                }
                if (<>o__14.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__14.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                }
                if (<>o__14.<>p__6.Target(<>o__14.<>p__6, <>o__14.<>p__5.Target(<>o__14.<>p__5, <>o__14.<>p__4.Target(<>o__14.<>p__4, <>o__14.<>p__3.Target(<>o__14.<>p__3, this._dynObj, "message")), "Product added to basket")))
                {
                    Cookie cookie = new Cookie {
                        Name = "recentlyViewedProducts",
                        Value = "\"[" + this._runner.PickedSize.Value.Value.Substring(0, this._runner.PickedSize.Value.Value.IndexOf("-")) + "]\"",
                        Domain = "mrporter.com"
                    };
                    this._runner.Cookies.Add(cookie);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    if (this._runner.Product == null)
                    {
                        flag = true;
                        while (flag)
                        {
                            flag = false;
                            try
                            {
                                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                                this._dynObj = this._client.Get($"https://www.mrporter.com/{this._region}/api/basket/view.json").Json();
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
                        Product product = new Product();
                        if (<>o__14.<>p__11 == null)
                        {
                            <>o__14.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__14.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__14.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "basketItems", typeof(MrPorter), argumentInfo));
                        }
                        product.ProductTitle = <>o__14.<>p__11.Target(<>o__14.<>p__11, <>o__14.<>p__10.Target(<>o__14.<>p__10, <>o__14.<>p__9.Target(<>o__14.<>p__9, <>o__14.<>p__8.Target(<>o__14.<>p__8, <>o__14.<>p__7.Target(<>o__14.<>p__7, this._dynObj)), "title")));
                        if (<>o__14.<>p__16 == null)
                        {
                            <>o__14.<>p__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__14.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__14.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "basketItems", typeof(MrPorter), argumentInfo));
                        }
                        product.Link = <>o__14.<>p__16.Target(<>o__14.<>p__16, <>o__14.<>p__15.Target(<>o__14.<>p__15, <>o__14.<>p__14.Target(<>o__14.<>p__14, <>o__14.<>p__13.Target(<>o__14.<>p__13, <>o__14.<>p__12.Target(<>o__14.<>p__12, this._dynObj)), "sku")));
                        if (<>o__14.<>p__22 == null)
                        {
                            <>o__14.<>p__22 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__14.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__21 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "HtmlDecode", null, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__14.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "basketItems", typeof(MrPorter), argumentInfo));
                        }
                        product.Price = <>o__14.<>p__22.Target(<>o__14.<>p__22, <>o__14.<>p__21.Target(<>o__14.<>p__21, typeof(WebUtility), <>o__14.<>p__20.Target(<>o__14.<>p__20, <>o__14.<>p__19.Target(<>o__14.<>p__19, <>o__14.<>p__18.Target(<>o__14.<>p__18, <>o__14.<>p__17.Target(<>o__14.<>p__17, this._dynObj)), "originalPrice"))));
                        this._runner.Product = product;
                        this._task.ImgUrl = "http://eve-robotics.com/dummy_product.png";
                        if (<>o__14.<>p__27 == null)
                        {
                            <>o__14.<>p__27 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__14.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__14.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__14.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__14.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "basketItems", typeof(MrPorter), argumentInfo));
                        }
                        this._task.Size = <>o__14.<>p__27.Target(<>o__14.<>p__27, <>o__14.<>p__26.Target(<>o__14.<>p__26, <>o__14.<>p__25.Target(<>o__14.<>p__25, <>o__14.<>p__24.Target(<>o__14.<>p__24, <>o__14.<>p__23.Target(<>o__14.<>p__23, this._dynObj)), "size")));
                        this._task.Size = this._task.Size.Replace("US", "").Replace("UK", "").Replace("EU", "").Trim();
                        this._runner.PickedSize = new KeyValuePair<string, string>(this._task.Size, this._task.Link);
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
                    return true;
                }
                if (!this._srr.ToLowerInvariant().Contains("out"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
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
                if (exception3 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception3.Message.Contains("430") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception3, "", "");
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
            if (!this.SubmitShipping())
            {
                return false;
            }
            if (!this._isLoggedIn && !this.SubmitBilling())
            {
                return false;
            }
            return this.SubmitOrder();
        }

        public bool DirectLink(string link)
        {
            try
            {
                object obj5;
                List<KeyValuePair<string, string>>.Enumerator enumerator3;
                string str10;
                bool flag = true;
                string str = "";
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                if (!link.Contains("https"))
                {
                    goto Label_133B;
                }
                while (flag)
                {
                    flag = false;
                    try
                    {
                        KeyValuePair<string, string> pair = this._client.Get(link).TextResponseUri();
                        this._srr = pair.Key;
                        str = pair.Value;
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
                using (List<Cookie>.Enumerator enumerator = this._client.Cookies.List().GetEnumerator())
                {
                    Cookie current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.Name == "channel")
                        {
                            goto Label_010E;
                        }
                    }
                    goto Label_0130;
                Label_010E:
                    this._channel = current.Value.ToLowerInvariant();
                }
            Label_0130:
                this._currentDoc.LoadHtml(this._srr);
                this._local = this._currentDoc.DocumentNode.Descendants().First<HtmlNode>(x => (x.Attributes["data-locale-id"] > null)).Attributes["data-locale-id"].Value;
                if (this._task.Login && !this._isLoggedIn)
                {
                    this.Login();
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                    this._currentDoc.LoadHtml(this._srr);
                }
                if (!str.Contains("soldOutId="))
                {
                    this._region = this._currentDoc.DocumentNode.Descendants("body").First<HtmlNode>().Attributes["data-region"].Value;
                    string innerText = this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productData"))).InnerText;
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(innerText);
                    this._runner.ProductPageHtml = new HtmlDocument();
                    this._runner.ProductPageHtml.LoadHtml(this._srr);
                    if (<>o__19.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__19.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                    }
                    object obj2 = <>o__19.<>p__1.Target(<>o__19.<>p__1, <>o__19.<>p__0.Target(<>o__19.<>p__0, this._dynObj, "name"));
                    if (<>o__19.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__19.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "price", typeof(MrPorter), argumentInfo));
                    }
                    object obj3 = <>o__19.<>p__4.Target(<>o__19.<>p__4, <>o__19.<>p__3.Target(<>o__19.<>p__3, <>o__19.<>p__2.Target(<>o__19.<>p__2, this._dynObj), "formattedPrice"));
                    string str3 = "http://eve-robotics.com/dummy_product.png";
                    try
                    {
                        if (<>o__19.<>p__8 == null)
                        {
                            <>o__19.<>p__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__19.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__19.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "images", typeof(MrPorter), argumentInfo));
                        }
                        string str4 = <>o__19.<>p__8.Target(<>o__19.<>p__8, <>o__19.<>p__7.Target(<>o__19.<>p__7, <>o__19.<>p__6.Target(<>o__19.<>p__6, <>o__19.<>p__5.Target(<>o__19.<>p__5, this._dynObj), "urlTemplate"))).Replace("{{scheme}}", "https:");
                        if (<>o__19.<>p__13 == null)
                        {
                            <>o__19.<>p__13 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__19.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shots", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "images", typeof(MrPorter), argumentInfo));
                        }
                        string newValue = <>o__19.<>p__13.Target(<>o__19.<>p__13, <>o__19.<>p__12.Target(<>o__19.<>p__12, <>o__19.<>p__11.Target(<>o__19.<>p__11, <>o__19.<>p__10.Target(<>o__19.<>p__10, <>o__19.<>p__9.Target(<>o__19.<>p__9, this._dynObj)))));
                        if (<>o__19.<>p__18 == null)
                        {
                            <>o__19.<>p__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__19.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sizes", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "images", typeof(MrPorter), argumentInfo));
                        }
                        string str6 = <>o__19.<>p__18.Target(<>o__19.<>p__18, <>o__19.<>p__17.Target(<>o__19.<>p__17, <>o__19.<>p__16.Target(<>o__19.<>p__16, <>o__19.<>p__15.Target(<>o__19.<>p__15, <>o__19.<>p__14.Target(<>o__19.<>p__14, this._dynObj)))));
                        str3 = str4.Replace("{{shot}}", newValue).Replace("{{size}}", str6);
                    }
                    catch
                    {
                    }
                    this._task.ImgUrl = str3;
                    Product product1 = new Product();
                    if (<>o__19.<>p__19 == null)
                    {
                        <>o__19.<>p__19 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                    }
                    product1.ProductTitle = <>o__19.<>p__19.Target(<>o__19.<>p__19, obj2);
                    product1.Link = link;
                    if (<>o__19.<>p__20 == null)
                    {
                        <>o__19.<>p__20 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                    }
                    product1.Price = <>o__19.<>p__20.Target(<>o__19.<>p__20, obj3);
                    this._runner.Product = product1;
                    if (<>o__19.<>p__37 == null)
                    {
                        <>o__19.<>p__37 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(MrPorter)));
                    }
                    if (<>o__19.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "skus", typeof(MrPorter), argumentInfo));
                    }
                    foreach (object obj4 in <>o__19.<>p__37.Target(<>o__19.<>p__37, <>o__19.<>p__21.Target(<>o__19.<>p__21, this._dynObj)))
                    {
                        if (<>o__19.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__30 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__19.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__19.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        obj5 = <>o__19.<>p__24.Target(<>o__19.<>p__24, <>o__19.<>p__23.Target(<>o__19.<>p__23, <>o__19.<>p__22.Target(<>o__19.<>p__22, obj4, "stockLevel")), "In_Stock");
                        if (<>o__19.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__29 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(MrPorter), argumentInfo));
                        }
                        if (!<>o__19.<>p__29.Target(<>o__19.<>p__29, obj5))
                        {
                            if (<>o__19.<>p__28 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__19.<>p__28 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(MrPorter), argumentInfo));
                            }
                            if (<>o__19.<>p__27 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__19.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(MrPorter), argumentInfo));
                            }
                            if (<>o__19.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__19.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                            }
                        }
                        if (!<>o__19.<>p__30.Target(<>o__19.<>p__30, (<>o__19.<>p__25 != null) ? obj5 : <>o__19.<>p__28.Target(<>o__19.<>p__28, obj5, <>o__19.<>p__27.Target(<>o__19.<>p__27, <>o__19.<>p__26.Target(<>o__19.<>p__26, <>o__19.<>p__25.Target(<>o__19.<>p__25, obj4, "stockLevel")), "Low_Stock"))))
                        {
                            if (<>o__19.<>p__33 == null)
                            {
                                <>o__19.<>p__33 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                            }
                            if (<>o__19.<>p__32 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__19.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                            }
                            if (<>o__19.<>p__31 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__19.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                            }
                            string str7 = <>o__19.<>p__33.Target(<>o__19.<>p__33, <>o__19.<>p__32.Target(<>o__19.<>p__32, <>o__19.<>p__31.Target(<>o__19.<>p__31, obj4, "displaySize"))).Replace("UK", "").Replace("US", "").Replace("EU", "").Trim();
                            if (<>o__19.<>p__36 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__19.<>p__36 = CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                            }
                            if (<>o__19.<>p__35 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__19.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                            }
                            if (<>o__19.<>p__34 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__19.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                            }
                            KeyValuePair<string, string> item = <>o__19.<>p__36.Target(<>o__19.<>p__36, typeof(KeyValuePair<string, string>), str7, <>o__19.<>p__35.Target(<>o__19.<>p__35, <>o__19.<>p__34.Target(<>o__19.<>p__34, obj4, "id")));
                            this._runner.Product.AvailableSizes.Add(item);
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
                            using (enumerator3 = this._runner.Product.AvailableSizes.GetEnumerator())
                            {
                                KeyValuePair<string, string> pair3;
                                int num6;
                                goto Label_1259;
                            Label_1189:
                                pair3 = enumerator3.Current;
                                List<string> source = new List<string>();
                                if (!pair3.Key.Contains(":"))
                                {
                                    goto Label_11EC;
                                }
                                char[] chArray2 = new char[] { ':' };
                                string[] strArray3 = pair3.Key.Split(chArray2);
                                int index = 0;
                                goto Label_11E2;
                            Label_11CB:
                                source.Add(strArray3[index].Trim());
                                index++;
                            Label_11E2:
                                if (index >= strArray3.Length)
                                {
                                    goto Label_11FA;
                                }
                                goto Label_11CB;
                            Label_11EC:
                                source.Add(pair3.Key);
                            Label_11FA:
                                num6 = 0;
                                while (num6 < source.Count)
                                {
                                    source[num6] = source[num6].Trim().ToUpperInvariant();
                                    num6++;
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_1267;
                                }
                            Label_1259:
                                if (!enumerator3.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_1189;
                            Label_1267:
                                this._runner.PickedSize = new KeyValuePair<string, string>?(pair3);
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
            Label_133B:
                str10 = (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.USA) ? "US" : "GB";
                this._region = (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.USA) ? "am" : "intl";
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._dynObj = this._client.Get($"https://api.net-a-porter.com/MRP/{str10}/en/detail/{this._task.Link}").Json();
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
                if (<>o__19.<>p__39 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                }
                if (<>o__19.<>p__38 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__38 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                }
                object obj6 = <>o__19.<>p__39.Target(<>o__19.<>p__39, <>o__19.<>p__38.Target(<>o__19.<>p__38, this._dynObj, "name"));
                if (<>o__19.<>p__44 == null)
                {
                    <>o__19.<>p__44 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                }
                if (<>o__19.<>p__43 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(MrPorter), argumentInfo));
                }
                if (<>o__19.<>p__42 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                }
                if (<>o__19.<>p__41 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__19.<>p__41 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                }
                if (<>o__19.<>p__40 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "price", typeof(MrPorter), argumentInfo));
                }
                string str11 = <>o__19.<>p__44.Target(<>o__19.<>p__44, <>o__19.<>p__43.Target(<>o__19.<>p__43, <>o__19.<>p__42.Target(<>o__19.<>p__42, <>o__19.<>p__41.Target(<>o__19.<>p__41, <>o__19.<>p__40.Target(<>o__19.<>p__40, this._dynObj), "amount"))));
                str11 = str11.Insert(str11.Length - 2, ".");
                string str12 = "http://eve-robotics.com/dummy_product.png";
                try
                {
                    if (<>o__19.<>p__48 == null)
                    {
                        <>o__19.<>p__48 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                    }
                    if (<>o__19.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__19.<>p__46 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "images", typeof(MrPorter), argumentInfo));
                    }
                    string str13 = <>o__19.<>p__48.Target(<>o__19.<>p__48, <>o__19.<>p__47.Target(<>o__19.<>p__47, <>o__19.<>p__46.Target(<>o__19.<>p__46, <>o__19.<>p__45.Target(<>o__19.<>p__45, this._dynObj), "urlTemplate"))).Replace("{{scheme}}", "https:");
                    if (<>o__19.<>p__53 == null)
                    {
                        <>o__19.<>p__53 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                    }
                    if (<>o__19.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shots", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "images", typeof(MrPorter), argumentInfo));
                    }
                    string newValue = <>o__19.<>p__53.Target(<>o__19.<>p__53, <>o__19.<>p__52.Target(<>o__19.<>p__52, <>o__19.<>p__51.Target(<>o__19.<>p__51, <>o__19.<>p__50.Target(<>o__19.<>p__50, <>o__19.<>p__49.Target(<>o__19.<>p__49, this._dynObj)))));
                    if (<>o__19.<>p__58 == null)
                    {
                        <>o__19.<>p__58 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                    }
                    if (<>o__19.<>p__57 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__56 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__55 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sizes", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__54 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "images", typeof(MrPorter), argumentInfo));
                    }
                    string str15 = <>o__19.<>p__58.Target(<>o__19.<>p__58, <>o__19.<>p__57.Target(<>o__19.<>p__57, <>o__19.<>p__56.Target(<>o__19.<>p__56, <>o__19.<>p__55.Target(<>o__19.<>p__55, <>o__19.<>p__54.Target(<>o__19.<>p__54, this._dynObj)))));
                    str12 = str13.Replace("{{shot}}", newValue).Replace("{{size}}", str15);
                }
                catch
                {
                }
                this._task.ImgUrl = str12;
                Product product2 = new Product();
                if (<>o__19.<>p__59 == null)
                {
                    <>o__19.<>p__59 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                }
                product2.ProductTitle = <>o__19.<>p__59.Target(<>o__19.<>p__59, obj6);
                product2.Link = link;
                product2.Price = str11;
                this._runner.Product = product2;
                if (<>o__19.<>p__76 == null)
                {
                    <>o__19.<>p__76 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(MrPorter)));
                }
                if (<>o__19.<>p__60 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__19.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "skus", typeof(MrPorter), argumentInfo));
                }
                foreach (object obj7 in <>o__19.<>p__76.Target(<>o__19.<>p__76, <>o__19.<>p__60.Target(<>o__19.<>p__60, this._dynObj)))
                {
                    if (<>o__19.<>p__69 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__69 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__63 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__19.<>p__63 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__62 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                    }
                    if (<>o__19.<>p__61 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__19.<>p__61 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                    }
                    obj5 = <>o__19.<>p__63.Target(<>o__19.<>p__63, <>o__19.<>p__62.Target(<>o__19.<>p__62, <>o__19.<>p__61.Target(<>o__19.<>p__61, obj7, "stockLevel")), "In_Stock");
                    if (<>o__19.<>p__68 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__19.<>p__68 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(MrPorter), argumentInfo));
                    }
                    if (!<>o__19.<>p__68.Target(<>o__19.<>p__68, obj5))
                    {
                        if (<>o__19.<>p__67 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__67 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__66 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__19.<>p__66 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__65 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__65 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                    }
                    if (!<>o__19.<>p__69.Target(<>o__19.<>p__69, (<>o__19.<>p__64 != null) ? obj5 : <>o__19.<>p__67.Target(<>o__19.<>p__67, obj5, <>o__19.<>p__66.Target(<>o__19.<>p__66, <>o__19.<>p__65.Target(<>o__19.<>p__65, <>o__19.<>p__64.Target(<>o__19.<>p__64, obj7, "stockLevel")), "Low_Stock"))))
                    {
                        if (<>o__19.<>p__72 == null)
                        {
                            <>o__19.<>p__72 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(MrPorter)));
                        }
                        if (<>o__19.<>p__71 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__71 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__70 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__19.<>p__70 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        string str16 = <>o__19.<>p__72.Target(<>o__19.<>p__72, <>o__19.<>p__71.Target(<>o__19.<>p__71, <>o__19.<>p__70.Target(<>o__19.<>p__70, obj7, "displaySize"))).Replace("UK", "").Replace("US", "").Replace("EU", "").Trim();
                        if (<>o__19.<>p__75 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__75 = CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__74 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__19.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(MrPorter), argumentInfo));
                        }
                        if (<>o__19.<>p__73 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__19.<>p__73 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(MrPorter), argumentInfo));
                        }
                        KeyValuePair<string, string> item = <>o__19.<>p__75.Target(<>o__19.<>p__75, typeof(KeyValuePair<string, string>), str16, <>o__19.<>p__74.Target(<>o__19.<>p__74, <>o__19.<>p__73.Target(<>o__19.<>p__73, obj7, "id")));
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                }
                if (this._runner.Product.AvailableSizes.Count == 0)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                if (this._task.PriceCheck)
                {
                    string str17 = "";
                    foreach (char ch2 in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch2) || (ch2 == '.')) || (ch2 == ','))
                        {
                            str17 = str17 + ch2.ToString();
                        }
                    }
                    double num7 = double.Parse(str17.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num7 < this._task.MinimumPrice) || (num7 > this._task.MaximumPrice))
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
                    string[] strArray4 = this._task.Size.Split(separator);
                    for (int i = 0; i < strArray4.Length; i++)
                    {
                        strArray4[i] = strArray4[i].Trim().ToUpperInvariant();
                    }
                    foreach (string text1 in strArray4)
                    {
                        if (this._runner.PickedSize.HasValue)
                        {
                            break;
                        }
                        using (enumerator3 = this._runner.Product.AvailableSizes.GetEnumerator())
                        {
                            KeyValuePair<string, string> current;
                            while (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                List<string> source = new List<string>();
                                if (current.Key.Contains(":"))
                                {
                                    char[] chArray4 = new char[] { ':' };
                                    string[] strArray5 = current.Key.Split(chArray4);
                                    for (int k = 0; k < strArray5.Length; k++)
                                    {
                                        source.Add(strArray5[k].Trim());
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
                                if (source.Any<string>(x => x == text1))
                                {
                                    goto Label_23B1;
                                }
                            }
                            continue;
                        Label_23B1:
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
            catch (Exception exception3)
            {
                this._isLoggedIn = false;
                this._runner.Cookies = new CookieContainer();
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
            if (this._isLoggedIn)
            {
                return true;
            }
            try
            {
                string html = "";
                this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGGING_IN, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        html = this._client.Get($"https://www.mrporter.com/signin.mrp?redirect=/{this._channel}/account&httpsRedirect=true").Text();
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
                string token = "";
                if (html.Contains("data-sitekey"))
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.MR_PORTER_CAPTCHA_KEY, "https://www.mrporter.com", "MrPorter");
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
                    token = waiter.Token;
                    this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGGING_IN, null, "", "");
                }
                flag = true;
                string str3 = "";
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("redirect", $"/{this._local}/account");
                    this._diData.Add("httpsRedirect", "true");
                    this._diData.Add("j_username", this._task.Username);
                    this._diData.Add("j_password", this._task.Password);
                    this._diData.Add("didProvideAPassword", "yes");
                    this._diData.Add("actionFrom", "");
                    if (!string.IsNullOrEmpty(token))
                    {
                        this._diData.Add("g-recaptcha-response", token);
                    }
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri($"https://www.mrporter.com/signin.mrp?redirect=/{this._local}/account&httpsRedirect=true");
                        KeyValuePair<string, string> pair = this._client.Post($"https://www.mrporter.com/{this._region}/j_spring_security_check", this._diData).TextResponseUri();
                        html = pair.Key;
                        str3 = pair.Value;
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
                if (!str3.Contains("/account") || str3.Contains("error"))
                {
                    goto Label_05F7;
                }
                this._isLoggedIn = true;
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_SUCCESSFUL, null, "", "");
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        html = this._client.Get($"https://www.mrporter.com/{this._local}/shoppingbag.mrp").Text();
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
                bool flag4 = html.ToLowerInvariant().Contains("update=remove");
                bool flag3 = false;
            Label_048E:
                if (!flag4)
                {
                    return true;
                }
                this._currentDoc.LoadHtml(html);
                if (!flag3)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CLEANING_CART);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CLEARING_CART, null, "", "");
                    flag3 = true;
                }
                string url = "https://www.mrporter.com" + WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>(x => ((x.Attributes["href"] != null) && x.Attributes["href"].Value.Contains("Update=Remove"))).Attributes["href"].Value);
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri($"https://www.mrporter.com/{this._local}/shoppingbag.mrp");
                        html = this._client.Get(url).Text();
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
                    }
                    continue;
                Label_05CF:
                    flag4 = html.ToLowerInvariant().Contains("update=remove");
                    goto Label_048E;
                }
                goto Label_05CF;
            Label_05F7:
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_UNSUCCESSFUL, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.LOGIN_UNSUCCESSFUL);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception5)
            {
                if (exception5 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_LOGIN);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception5.Message.Contains("430") && ((exception5.InnerException == null) || !exception5.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_LOGIN);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_LOGGING_IN, exception5, "", "");
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.mrporter.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.mrporter.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
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
                string str9 = obj2.FirstName.Trim();
                string str7 = obj2.LastName.Trim();
                string str8 = obj2.Address1.Trim();
                string str3 = obj2.Address2.Trim();
                string str4 = obj2.City.Trim();
                string str5 = obj2.Zip.Trim();
                string str2 = obj2.CountryId.ToLowerInvariant();
                string state = "";
                if ((str2 != "us") && (str2 != "ca"))
                {
                    state = obj2.State;
                }
                else
                {
                    state = obj2.StateId;
                }
                string str11 = obj2.Phone.Trim();
                string url = "https://www.mrporter.com" + this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "addressCommand"))).Attributes["action"].Value;
                string str6 = url.Substring(url.IndexOf("execution="));
                str6 = str6.Substring(str6.IndexOf("=") + 1);
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("overlay", "");
                        this._diData.Add("addressType", "_BILLING");
                        this._diData.Add("address.title", "Mr");
                        this._diData.Add("address.firstName", str9);
                        this._diData.Add("address.lastName", str7);
                        this._diData.Add("address.countryLookup", str2);
                        this._diData.Add("address.address1", str8);
                        this._diData.Add("address.address2", str3);
                        this._diData.Add("address.towncity", str4);
                        this._diData.Add("address.state", state);
                        this._diData.Add("address.postcode", str5);
                        this._diData.Add("address.work", str11);
                        this._diData.Add("address.mobile", "");
                        this._diData.Add("_eventId_proceedToPurchase", "Proceed to purchase");
                        this._diData.Add("_flowExecutionKey", str6);
                        this._diData.Add("eventId", "eventIdNotSet");
                        KeyValuePair<string, string> pair = this._client.Post(url, this._diData).TextResponseUri();
                        this._srr = pair.Key;
                        this._link = pair.Value;
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
                if (!this._task.Link.ToLowerInvariant().Contains("http"))
                {
                    string newImg = "https:" + this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "order_summary_section"))).Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                    Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                        Global.SUCCESS.First<SuccessObject>(new Func<SuccessObject, bool>(this.<SubmitBilling>b__17_4)).ProductImage = newImg;
                        this._task.ImgUrl = newImg;
                    }, Array.Empty<object>());
                }
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                this._isLoggedIn = false;
                this._runner.Cookies = new CookieContainer();
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception2, "", "");
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
                if (this._srr.Contains("Some items can not be shipped to your chosen destination. Please either remove them or select an alternative country for delivery"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CANT_BE_SHIPPED, null, "", "");
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentSessionId"))).Attributes["value"].Value;
                string text1 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "submittedToken"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "redirectUrl"))).Attributes["value"].Value;
                string str = "";
                switch (this._runner.Profile.CardTypeId)
                {
                    case "0":
                        str = "AMEX";
                        break;

                    case "1":
                        str = "VISA";
                        break;

                    case "2":
                        str = "MASTERCARD";
                        break;

                    case "3":
                        str = "JCB";
                        break;
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
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("redirectUrl", str3);
                        this._diData.Add("adminId", "0");
                        this._diData.Add("cardType", str);
                        this._diData.Add("savedCard", "false");
                        this._diData.Add("cardNumber", this._runner.Profile.CCNumber);
                        this._diData.Add("cardHoldersName", this._runner.Profile.NameOnCard);
                        this._diData.Add("cVSNumber", this._runner.Profile.Cvv);
                        this._diData.Add("expiryMonth", this._runner.Profile.ExpiryMonth);
                        this._diData.Add("expiryYear", this._runner.Profile.ExpiryYear.Substring(2));
                        this._diData.Add("issueNumber", "");
                        this._diData.Add("keepCard", "false");
                        this._diData.Add("paymentSessionId", str4);
                        this._diData.Add("customerId", "0");
                        this._diData.Add("site", "mrp_" + this._region);
                        this._srr = this._client.Post("https://secure.net-a-porter.com/psp/payment", this._diData).Text();
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
                try
                {
                    EveAIO.Helpers.AddDbValue("MrPorterCheckout|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                if (this._currentDoc.DocumentNode.Descendants("li").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error"))) && !string.IsNullOrEmpty(this._currentDoc.DocumentNode.Descendants("li").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error"))).InnerText.Trim()))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                flag = true;
            }
            catch (ThreadAbortException)
            {
                flag = false;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception2, "", "");
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
                this._isLoggedIn = false;
                this._runner.Cookies = new CookieContainer();
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
                        this._client.Get($"https://www.mrporter.com/{this._region}/api/basket/view.json").Text();
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
                        this._srr = this._client.Get("https://www.mrporter.com/purchasepath.mrp?link=bagdropdown&startpage=true").Text();
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
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                string str5 = obj2.EmailShipping.Trim();
                string str6 = obj2.FirstNameShipping.Trim();
                string str4 = obj2.LastNameShipping.Trim();
                string str11 = obj2.Address1Shipping.Trim();
                string str3 = obj2.Address2Shipping.Trim();
                string str7 = obj2.CityShipping.Trim();
                string str8 = obj2.ZipShipping.Trim();
                string str9 = obj2.CountryIdShipping.ToLowerInvariant();
                string stateShipping = "";
                if ((str9 != "us") && (str9 != "ca"))
                {
                    stateShipping = obj2.StateShipping;
                }
                else
                {
                    stateShipping = obj2.StateIdShipping;
                }
                string str2 = obj2.PhoneShipping.Trim();
                if (this._isLoggedIn)
                {
                    return true;
                }
                string token = "";
                if (this._srr.Contains("data-sitekey"))
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.MR_PORTER_CAPTCHA_KEY, "https://www.mrporter.com", "MrPorter");
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
                    token = waiter.Token;
                    this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                }
                string str12 = "";
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("j_username", str5);
                        this._diData.Add("didProvideAPassword", "no");
                        if (!string.IsNullOrEmpty(token))
                        {
                            this._diData.Add("g-recaptcha-response", token);
                        }
                        KeyValuePair<string, string> pair = this._client.Post($"https://www.mrporter.com/{this._region}/j_spring_security_check", this._diData).TextResponseUri();
                        this._srr = pair.Key;
                        str12 = pair.Value;
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
                if (!str12.Contains("/signin.mrp"))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    if (!this._srr.Contains("Some items can not be shipped to your chosen destination. Please either remove them or select an alternative country for delivery"))
                    {
                        string url = "https://www.mrporter.com" + this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "addressCommand"))).Attributes["action"].Value;
                        string str14 = url.Substring(url.IndexOf("execution="));
                        str14 = str14.Substring(str14.IndexOf("=") + 1);
                        flag = true;
                        while (flag)
                        {
                            flag = false;
                            try
                            {
                                this._diData.Clear();
                                this._diData.Add("overlay", "");
                                this._diData.Add("addressType", "_SHIPPING");
                                this._diData.Add("address.title", "Mr");
                                this._diData.Add("address.firstName", str6);
                                this._diData.Add("address.lastName", str4);
                                this._diData.Add("address.countryLookup", str9);
                                this._diData.Add("address.address1", str11);
                                this._diData.Add("address.address2", str3);
                                this._diData.Add("address.towncity", str7);
                                this._diData.Add("address.state", stateShipping);
                                this._diData.Add("address.postcode", str8);
                                this._diData.Add("address.work", str2);
                                this._diData.Add("address.mobile", "");
                                this._diData.Add("deliveryAndBillingSame", "false");
                                this._diData.Add("_eventId_proceedToPurchase", "Proceed to purchase");
                                this._diData.Add("_flowExecutionKey", str14);
                                this._diData.Add("eventId", "eventIdNotSet");
                                this._srr = this._client.Post(url, this._diData).Text();
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
                        this._currentDoc.LoadHtml(this._srr);
                        url = "https://www.mrporter.com" + this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shippingOptionsCommand"))).Attributes["action"].Value;
                        str14 = url.Substring(url.IndexOf("execution="));
                        str14 = str14.Substring(str14.IndexOf("=") + 1);
                        string str15 = "";
                        double num2 = 10000.0;
                        foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipping_method"))).Descendants("div")
                            where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "field_row")
                            select x)
                        {
                            string str18 = node.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"))).InnerText.Trim();
                            if (str18 != "FREE")
                            {
                                string str17 = "";
                                foreach (char ch in str18)
                                {
                                    if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                                    {
                                        str17 = str17 + ch.ToString();
                                    }
                                }
                                double num4 = double.Parse(str17.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                                if (num4 < num2)
                                {
                                    str15 = node.Descendants("input").First<HtmlNode>().Attributes["value"].Value;
                                    num2 = num4;
                                }
                            }
                            else
                            {
                                num2 = 0.0;
                                str15 = node.Descendants("input").First<HtmlNode>().Attributes["value"].Value;
                            }
                        }
                        flag = true;
                        while (flag)
                        {
                            flag = false;
                            try
                            {
                                this._diData.Clear();
                                this._diData.Add("selectedShippingMethodId", str15);
                                this._diData.Add("signatureRequired", "false");
                                this._diData.Add("selectedPackagingOptionId", "900006-001");
                                this._diData.Add("stickerDetails.stickerOption", "FOR_ME");
                                this._diData.Add("stickerDetails.stickerSurname", "");
                                this._diData.Add("stickerDetails.stickerTitle", "");
                                this._diData.Add("_giftMessageEnabled", "");
                                this._diData.Add("giftMessage", "");
                                this._diData.Add("_eventId_proceedToPurchase", "Proceed to purchase");
                                this._diData.Add("_flowExecutionKey", "e1s2");
                                this._diData.Add("eventId", "eventIdNotSet");
                                this._srr = this._client.Post(url, this._diData).Text();
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
                        this._currentDoc.LoadHtml(this._srr);
                        return true;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CANT_BE_SHIPPED, null, "", "");
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.LOGIN_REQUIRED);
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_REQUIRED, null, "", "");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception6)
            {
                this._isLoggedIn = false;
                this._runner.Cookies = new CookieContainer();
                this._runner.IsError = true;
                if (exception6 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception6.Message.Contains("430") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception6, "", "");
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
            public static readonly MrPorter.<>c <>9;
            public static Func<HtmlNode, bool> <>9__16_0;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__16_2;
            public static Func<HtmlNode, bool> <>9__16_4;
            public static Func<HtmlNode, bool> <>9__16_5;
            public static Func<HtmlNode, bool> <>9__17_1;
            public static Func<HtmlNode, bool> <>9__17_2;
            public static Func<HtmlNode, bool> <>9__18_1;
            public static Func<HtmlNode, bool> <>9__18_2;
            public static Func<HtmlNode, bool> <>9__18_5;
            public static Func<HtmlNode, bool> <>9__18_6;
            public static Func<HtmlNode, bool> <>9__18_7;
            public static Func<HtmlNode, bool> <>9__19_0;
            public static Func<HtmlNode, bool> <>9__19_1;
            public static Func<HtmlNode, bool> <>9__20_2;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new MrPorter.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__19_0(HtmlNode x) => 
                (x.Attributes["data-locale-id"] > null);

            internal bool <DirectLink>b__19_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productData"));

            internal bool <Login>b__20_2(HtmlNode x) => 
                ((x.Attributes["href"] != null) && x.Attributes["href"].Value.Contains("Update=Remove"));

            internal bool <SubmitBilling>b__17_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "addressCommand"));

            internal bool <SubmitBilling>b__17_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "order_summary_section"));

            internal bool <SubmitOrder>b__16_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "paymentSessionId"));

            internal bool <SubmitOrder>b__16_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "submittedToken"));

            internal bool <SubmitOrder>b__16_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "redirectUrl"));

            internal bool <SubmitOrder>b__16_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error"));

            internal bool <SubmitOrder>b__16_5(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error"));

            internal bool <SubmitShipping>b__18_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "addressCommand"));

            internal bool <SubmitShipping>b__18_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shippingOptionsCommand"));

            internal bool <SubmitShipping>b__18_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipping_method"));

            internal bool <SubmitShipping>b__18_6(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "field_row"));

            internal bool <SubmitShipping>b__18_7(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"));
        }

        [CompilerGenerated]
        private static class <>o__14
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string>> <>p__27;
        }

        [CompilerGenerated]
        private static class <>o__19
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string>> <>p__18;
            public static CallSite<Func<CallSite, object, string>> <>p__19;
            public static CallSite<Func<CallSite, object, string>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, bool>> <>p__29;
            public static CallSite<Func<CallSite, object, bool>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, string>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>> <>p__36;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__37;
            public static CallSite<Func<CallSite, object, string, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, string, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, string>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, string, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, object>> <>p__52;
            public static CallSite<Func<CallSite, object, string>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, string>> <>p__58;
            public static CallSite<Func<CallSite, object, string>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, string, object>> <>p__63;
            public static CallSite<Func<CallSite, object, string, object>> <>p__64;
            public static CallSite<Func<CallSite, object, object>> <>p__65;
            public static CallSite<Func<CallSite, object, string, object>> <>p__66;
            public static CallSite<Func<CallSite, object, object, object>> <>p__67;
            public static CallSite<Func<CallSite, object, bool>> <>p__68;
            public static CallSite<Func<CallSite, object, bool>> <>p__69;
            public static CallSite<Func<CallSite, object, string, object>> <>p__70;
            public static CallSite<Func<CallSite, object, object>> <>p__71;
            public static CallSite<Func<CallSite, object, string>> <>p__72;
            public static CallSite<Func<CallSite, object, string, object>> <>p__73;
            public static CallSite<Func<CallSite, object, object>> <>p__74;
            public static CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>> <>p__75;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__76;
        }
    }
}

