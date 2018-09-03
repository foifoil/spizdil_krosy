namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Captcha;
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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class Footaction : IPlatform
    {
        private Client _client;
        private Client _client2;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;
        private bool _init;
        private string _billingAddressId;
        private string _cartId;
        private bool _isCaptcha;
        private string _token;
        private bool _ipError;
        private CookieContainer _cleanCookies;
        private string _ccToken;

        public Footaction(TaskRunner runner, TaskObject task)
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
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__18.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "productId", typeof(Footaction), argumentInfo));
                }
                <>o__18.<>p__0.Target(<>o__18.<>p__0, obj2, this._runner.PickedSize.Value.Value);
                if (<>o__18.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "productQuantity", typeof(Footaction), argumentInfo));
                }
                <>o__18.<>p__1.Target(<>o__18.<>p__1, obj2, "1");
                if (this._isCaptcha)
                {
                    if (!this._task.SolveCaptcha)
                    {
                        if (<>o__18.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__18.<>p__2 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__18.<>p__2.Target(<>o__18.<>p__2, obj2, new Newtonsoft.Json.Linq.JProperty("g-recaptcha-response", "03AEMEkEnYflZbNX9sIlfP9NzbCS9NoXBIq93dPJ93gKu4TqgyGCVbPcP6-q-mgljbTOWaWoUuuUf2kXwbAH9kruFInvPxFWMdBdX1ZdPssO484XHXI9vIyu2Nfx6AYy6698U9h6WarNQsPwg0gk0sMb9sdraHYmlHq5hUlRH6vq87uylY0GoFTtwFV2K0ONCo3v3j0OtBMXdNxgJJ_I7OXG8AV6JhWK5xGixLNgtywjlMV8nH9yzDImutehagUWh7Dpvy8MfpBL563YBZA53I3yNIZ-GavzILOPfbiUIsOP_FNUEWYb5tVGD85yAPaAmX1KeTrETJ74k1mJNd-nbT0ur3wr4bVIa7jwN7YDs-2OIDDi5A356mglA"));
                    }
                    else
                    {
                        object solverLocker = Global.SolverLocker;
                        lock (solverLocker)
                        {
                            Global.CAPTCHA_QUEUE.Add(this._task);
                        }
                        this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                        States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                        this._task.Mre = new ManualResetEvent(false);
                        CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.FOOTACTION_CAPTCHA_KEY, this._task.Link, "Footaction");
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
                        if (<>o__18.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__18.<>p__3 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__18.<>p__3.Target(<>o__18.<>p__3, obj2, new Newtonsoft.Json.Linq.JProperty("g-recaptcha-response", waiter.Token));
                    }
                }
                Cookie cookie = new Cookie {
                    Name = "cart-guid",
                    Value = this._cartId,
                    Domain = ".footaction.com"
                };
                this._client2.Cookies.Add(cookie);
                bool flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        if (!this._client2.Session.DefaultRequestHeaders.Any<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "x-csrf-token")))
                        {
                            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("x-csrf-token", this._token);
                        }
                        if (<>o__18.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__5 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__18.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        object obj4 = <>o__18.<>p__5.Target(<>o__18.<>p__5, this._client2, "https://www.footaction.com/api/users/carts/current/entries", <>o__18.<>p__4.Target(<>o__18.<>p__4, typeof(Newtonsoft.Json.JsonConvert), obj2));
                        if (<>o__18.<>p__10 == null)
                        {
                            <>o__18.<>p__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                        }
                        if (<>o__18.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__18.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__18.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__18.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footaction), argumentInfo));
                        }
                        this._srr = <>o__18.<>p__10.Target(<>o__18.<>p__10, <>o__18.<>p__9.Target(<>o__18.<>p__9, <>o__18.<>p__8.Target(<>o__18.<>p__8, <>o__18.<>p__7.Target(<>o__18.<>p__7, <>o__18.<>p__6.Target(<>o__18.<>p__6, obj4)))));
                        if (<>o__18.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__11 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__18.<>p__11.Target(<>o__18.<>p__11, typeof(EveAIO.Extensions), obj4);
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
                if (!this._srr.Contains(this._runner.PickedSize.Value.Value))
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
                this._init = false;
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                return false;
            }
        }

        public bool Checkout()
        {
            if (!this.SubmitBilling())
            {
                return false;
            }
            return this.SubmitOrder();
        }

        public bool DirectLink(string link)
        {
            if (!this._init)
            {
                this.Init();
                if (this._ipError)
                {
                    return false;
                }
                if (!this.SubmitShipping())
                {
                    this._runner.Cookies = new CookieContainer();
                    this.SetClient();
                    return false;
                }
            }
            if (!this._init)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                return false;
            }
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                bool flag = true;
                this._client = new Client(this._cleanCookies, this._runner.Proxy, true);
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");
                this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._dynObj = this._client.Get($"https://www.footaction.com/api/products/pdp/{link.Trim()}").Json();
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
                string str = "";
                if (<>o__24.<>p__25 == null)
                {
                    <>o__24.<>p__25 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footaction)));
                }
                if (<>o__24.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "variantAttributes", typeof(Footaction), argumentInfo));
                }
                using (IEnumerator enumerator = <>o__24.<>p__25.Target(<>o__24.<>p__25, <>o__24.<>p__0.Target(<>o__24.<>p__0, this._dynObj)).GetEnumerator())
                {
                    object current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (<>o__24.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__24.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__24.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__4.Target(<>o__24.<>p__4, <>o__24.<>p__3.Target(<>o__24.<>p__3, <>o__24.<>p__2.Target(<>o__24.<>p__2, <>o__24.<>p__1.Target(<>o__24.<>p__1, current, "sku")), link.Trim())))
                        {
                            goto Label_03EB;
                        }
                    }
                    goto Label_0ACE;
                Label_03EB:
                    if (<>o__24.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__24.<>p__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                    }
                    if (<>o__24.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__24.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footaction), argumentInfo));
                    }
                    if (<>o__24.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__24.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                    }
                    if (<>o__24.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__24.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                    }
                    if (!<>o__24.<>p__8.Target(<>o__24.<>p__8, <>o__24.<>p__7.Target(<>o__24.<>p__7, <>o__24.<>p__6.Target(<>o__24.<>p__6, <>o__24.<>p__5.Target(<>o__24.<>p__5, current, "stockLevelStatus")), "outOfStock")))
                    {
                        if (<>o__24.<>p__11 == null)
                        {
                            <>o__24.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                        }
                        if (<>o__24.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__24.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                        }
                        str = <>o__24.<>p__11.Target(<>o__24.<>p__11, <>o__24.<>p__10.Target(<>o__24.<>p__10, <>o__24.<>p__9.Target(<>o__24.<>p__9, current, "code")));
                        if (<>o__24.<>p__16 == null)
                        {
                            <>o__24.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(Footaction)));
                        }
                        if (<>o__24.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__15 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__24.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                        }
                        this._isCaptcha = <>o__24.<>p__16.Target(<>o__24.<>p__16, <>o__24.<>p__15.Target(<>o__24.<>p__15, typeof(bool), <>o__24.<>p__14.Target(<>o__24.<>p__14, <>o__24.<>p__13.Target(<>o__24.<>p__13, <>o__24.<>p__12.Target(<>o__24.<>p__12, current, "recaptchaOn")))));
                        if (<>o__24.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__19 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__24.<>p__18 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__24.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__24.<>p__19.Target(<>o__24.<>p__19, <>o__24.<>p__18.Target(<>o__24.<>p__18, <>o__24.<>p__17.Target(<>o__24.<>p__17, current, "skuLaunchDate"), null)))
                        {
                            if (<>o__24.<>p__24 == null)
                            {
                                <>o__24.<>p__24 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(DateTime), typeof(Footaction)));
                            }
                            if (<>o__24.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__24.<>p__23 = CallSite<Func<CallSite, Type, object, string, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ParseExact", null, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__24.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__24.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__24.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__24.Target(<>o__24.<>p__24, <>o__24.<>p__23.Target(<>o__24.<>p__23, typeof(DateTime), <>o__24.<>p__22.Target(<>o__24.<>p__22, <>o__24.<>p__21.Target(<>o__24.<>p__21, <>o__24.<>p__20.Target(<>o__24.<>p__20, current, "skuLaunchDate"))), "MMM dd yyyy HH:mm:ss 'GMT'K", CultureInfo.InvariantCulture)).ToLocalTime() > DateTime.Now.ToLocalTime())
                            {
                                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_NOT_LIVE_YET, null, "", "");
                                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_NOT_LIVE_YET);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                }
            Label_0ACE:
                if (<>o__24.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__26 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__24.<>p__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                }
                object obj4 = <>o__24.<>p__27.Target(<>o__24.<>p__27, <>o__24.<>p__26.Target(<>o__24.<>p__26, this._dynObj, "name"));
                if (<>o__24.<>p__32 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__31 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__24.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__30 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "price", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__29 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__28 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "variantAttributes", typeof(Footaction), argumentInfo));
                }
                object obj3 = <>o__24.<>p__32.Target(<>o__24.<>p__32, <>o__24.<>p__31.Target(<>o__24.<>p__31, <>o__24.<>p__30.Target(<>o__24.<>p__30, <>o__24.<>p__29.Target(<>o__24.<>p__29, <>o__24.<>p__28.Target(<>o__24.<>p__28, this._dynObj))), "formattedValue"));
                if (<>o__24.<>p__39 == null)
                {
                    <>o__24.<>p__39 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                }
                if (<>o__24.<>p__38 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__37 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__24.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__36 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__35 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "variations", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__34 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Footaction), argumentInfo));
                }
                if (<>o__24.<>p__33 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "images", typeof(Footaction), argumentInfo));
                }
                this._task.ImgUrl = <>o__24.<>p__39.Target(<>o__24.<>p__39, <>o__24.<>p__38.Target(<>o__24.<>p__38, <>o__24.<>p__37.Target(<>o__24.<>p__37, <>o__24.<>p__36.Target(<>o__24.<>p__36, <>o__24.<>p__35.Target(<>o__24.<>p__35, <>o__24.<>p__34.Target(<>o__24.<>p__34, <>o__24.<>p__33.Target(<>o__24.<>p__33, this._dynObj)))), "url")));
                Product product1 = new Product();
                if (<>o__24.<>p__40 == null)
                {
                    <>o__24.<>p__40 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                }
                product1.ProductTitle = <>o__24.<>p__40.Target(<>o__24.<>p__40, obj4);
                product1.Link = link;
                if (<>o__24.<>p__41 == null)
                {
                    <>o__24.<>p__41 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                }
                product1.Price = <>o__24.<>p__41.Target(<>o__24.<>p__41, obj3);
                this._runner.Product = product1;
                if (<>o__24.<>p__69 == null)
                {
                    <>o__24.<>p__69 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footaction)));
                }
                if (<>o__24.<>p__42 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__24.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sellableUnits", typeof(Footaction), argumentInfo));
                }
                foreach (object obj5 in <>o__24.<>p__69.Target(<>o__24.<>p__69, <>o__24.<>p__42.Target(<>o__24.<>p__42, this._dynObj)))
                {
                    if (<>o__24.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__24.<>p__46 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                    }
                    if (<>o__24.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__24.<>p__45 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footaction), argumentInfo));
                    }
                    if (<>o__24.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__24.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                    }
                    if (<>o__24.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__24.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                    }
                    if (!<>o__24.<>p__46.Target(<>o__24.<>p__46, <>o__24.<>p__45.Target(<>o__24.<>p__45, <>o__24.<>p__44.Target(<>o__24.<>p__44, <>o__24.<>p__43.Target(<>o__24.<>p__43, obj5, "stockLevelStatus")), "outOfStock")))
                    {
                        string key = "";
                        string str3 = "";
                        if (<>o__24.<>p__58 == null)
                        {
                            <>o__24.<>p__58 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footaction)));
                        }
                        if (<>o__24.<>p__47 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "attributes", typeof(Footaction), argumentInfo));
                        }
                        foreach (object obj6 in <>o__24.<>p__58.Target(<>o__24.<>p__58, <>o__24.<>p__47.Target(<>o__24.<>p__47, obj5)))
                        {
                            if (<>o__24.<>p__51 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__24.<>p__51 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__50 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__24.<>p__50 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__49 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__24.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__48 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__24.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__51.Target(<>o__24.<>p__51, <>o__24.<>p__50.Target(<>o__24.<>p__50, <>o__24.<>p__49.Target(<>o__24.<>p__49, <>o__24.<>p__48.Target(<>o__24.<>p__48, obj6, "type")), "size")))
                            {
                                if (<>o__24.<>p__54 == null)
                                {
                                    <>o__24.<>p__54 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                                }
                                if (<>o__24.<>p__53 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__24.<>p__53 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                                }
                                if (<>o__24.<>p__52 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__24.<>p__52 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                                }
                                key = <>o__24.<>p__54.Target(<>o__24.<>p__54, <>o__24.<>p__53.Target(<>o__24.<>p__53, <>o__24.<>p__52.Target(<>o__24.<>p__52, obj6, "value")));
                                if (<>o__24.<>p__57 == null)
                                {
                                    <>o__24.<>p__57 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                                }
                                if (<>o__24.<>p__56 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__24.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                                }
                                if (<>o__24.<>p__55 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__24.<>p__55 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                                }
                                str3 = <>o__24.<>p__57.Target(<>o__24.<>p__57, <>o__24.<>p__56.Target(<>o__24.<>p__56, <>o__24.<>p__55.Target(<>o__24.<>p__55, obj6, "id")));
                            }
                        }
                        if (<>o__24.<>p__68 == null)
                        {
                            <>o__24.<>p__68 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footaction)));
                        }
                        if (<>o__24.<>p__59 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__24.<>p__59 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "attributes", typeof(Footaction), argumentInfo));
                        }
                        foreach (object obj7 in <>o__24.<>p__68.Target(<>o__24.<>p__68, <>o__24.<>p__59.Target(<>o__24.<>p__59, obj5)))
                        {
                            if (<>o__24.<>p__63 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__24.<>p__63 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__62 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__24.<>p__62 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__61 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__24.<>p__61 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__60 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__24.<>p__60 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__24.<>p__63.Target(<>o__24.<>p__63, <>o__24.<>p__62.Target(<>o__24.<>p__62, <>o__24.<>p__61.Target(<>o__24.<>p__61, <>o__24.<>p__60.Target(<>o__24.<>p__60, obj7, "type")), "style")))
                            {
                                if (<>o__24.<>p__67 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__24.<>p__67 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                                }
                                if (<>o__24.<>p__66 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__24.<>p__66 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footaction), argumentInfo));
                                }
                                if (<>o__24.<>p__65 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__24.<>p__65 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                                }
                                if (<>o__24.<>p__64 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__24.<>p__64 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                                }
                                if (<>o__24.<>p__67.Target(<>o__24.<>p__67, <>o__24.<>p__66.Target(<>o__24.<>p__66, str, <>o__24.<>p__65.Target(<>o__24.<>p__65, <>o__24.<>p__64.Target(<>o__24.<>p__64, obj7, "id")))))
                                {
                                    this._runner.Product.AvailableSizes.Add(new KeyValuePair<string, string>(key, str3));
                                }
                            }
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
                for (int i = 0; i < this._runner.Product.AvailableSizes.Count; i++)
                {
                    KeyValuePair<string, string> pair = this._runner.Product.AvailableSizes[i];
                    if (pair.Key[0] == '0')
                    {
                        pair = this._runner.Product.AvailableSizes[i];
                        pair = this._runner.Product.AvailableSizes[i];
                        string key = pair.Key.Substring(1);
                        this._runner.Product.AvailableSizes[i] = new KeyValuePair<string, string>(key, pair.Value);
                    }
                    pair = this._runner.Product.AvailableSizes[i];
                    pair = this._runner.Product.AvailableSizes[i];
                    if (pair.Key[pair.Key.Length - 1] == '0')
                    {
                        pair = this._runner.Product.AvailableSizes[i];
                        pair = this._runner.Product.AvailableSizes[i];
                        pair = this._runner.Product.AvailableSizes[i];
                        string key = pair.Key.Substring(0, pair.Key.IndexOf("."));
                        this._runner.Product.AvailableSizes[i] = new KeyValuePair<string, string>(key, pair.Value);
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
                    double num5 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num5 < this._task.MinimumPrice) || (num5 > this._task.MaximumPrice))
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
                    for (int j = 0; j < strArray.Length; j++)
                    {
                        strArray[j] = strArray[j].Trim().ToUpperInvariant();
                    }
                    foreach (string sz in strArray)
                    {
                        if (this._runner.PickedSize.HasValue)
                        {
                            break;
                        }
                        using (List<KeyValuePair<string, string>>.Enumerator enumerator3 = this._runner.Product.AvailableSizes.GetEnumerator())
                        {
                            KeyValuePair<string, string> current;
                            while (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                List<string> source = new List<string>();
                                if (!current.Key.Contains(":"))
                                {
                                    source.Add(current.Key);
                                }
                                else
                                {
                                    char[] chArray2 = new char[] { ':' };
                                    string[] strArray3 = current.Key.Split(chArray2);
                                    for (int m = 0; m < strArray3.Length; m++)
                                    {
                                        source.Add(strArray3[m].Trim());
                                    }
                                }
                                for (int k = 0; k < source.Count; k++)
                                {
                                    source[k] = source[k].Trim().ToUpperInvariant();
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_1DF3;
                                }
                            }
                            continue;
                        Label_1DF3:
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
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception2, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                this._init = false;
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                return false;
            }
        }

        private void Init()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.SETTING_UP, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SETTING_UP);
                this._ipError = false;
                ProfileObject profile = this._runner.Profile;
                bool flag = true;
                string str = "";
            Label_0046:
                if (flag)
                {
                    goto Label_0102;
                }
                goto Label_010D;
            Label_0051:
                flag = false;
                try
                {
                    this._client.Session.DefaultRequestHeaders.Remove("Accept");
                    this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                    this._dynObj = this._client.Get("https://www.footaction.com/api/session").Json();
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
                }
            Label_0102:
                if (flag)
                {
                    goto Label_0051;
                }
                goto Label_0046;
            Label_010D:
                if (str.Contains("footlocker.eu"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.US_IP_NEEDED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.US_IP_NEEDED, null, "", "");
                    this._ipError = true;
                    return;
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        object obj3 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__23.<>p__3 == null)
                        {
                            <>o__23.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                        }
                        if (<>o__23.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__23.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__23.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__23.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "data", typeof(Footaction), argumentInfo));
                        }
                        this._token = <>o__23.<>p__3.Target(<>o__23.<>p__3, <>o__23.<>p__2.Target(<>o__23.<>p__2, <>o__23.<>p__1.Target(<>o__23.<>p__1, <>o__23.<>p__0.Target(<>o__23.<>p__0, this._dynObj), "csrfToken")));
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("x-csrf-token", this._token);
                        if (<>o__23.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__5 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PutJson", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__23.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        object obj4 = <>o__23.<>p__5.Target(<>o__23.<>p__5, this._client, $"https://www.footaction.com/api/users/carts/current/email/{profile.Email}", <>o__23.<>p__4.Target(<>o__23.<>p__4, typeof(Newtonsoft.Json.JsonConvert), obj3));
                        if (<>o__23.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__6 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__23.<>p__6.Target(<>o__23.<>p__6, typeof(EveAIO.Extensions), obj4);
                        if (<>o__23.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__10 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ToString", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__23.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__23.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__23.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footaction), argumentInfo));
                        }
                        <>o__23.<>p__10.Target(<>o__23.<>p__10, <>o__23.<>p__9.Target(<>o__23.<>p__9, <>o__23.<>p__8.Target(<>o__23.<>p__8, <>o__23.<>p__7.Target(<>o__23.<>p__7, obj4))));
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
                foreach (Cookie cookie in this._client.Handler.CookieContainer.List())
                {
                    if (cookie.Name == "cart-guid")
                    {
                        this._cartId = cookie.Value;
                    }
                }
                this._cleanCookies = new CookieContainer();
                foreach (Cookie cookie2 in this._client.Handler.CookieContainer.List())
                {
                    if ((cookie2.Name != "cart-guid") && (cookie2.Name != "bm_sv"))
                    {
                        this._cleanCookies.Add(cookie2);
                        this._client2.Handler.CookieContainer.Add(cookie2);
                    }
                }
                flag = true;
            Label_0688:
                if (flag)
                {
                    goto Label_07C2;
                }
                goto Label_07CD;
            Label_0693:
                flag = false;
                try
                {
                    this._diData.Clear();
                    this._diData.Add("action", "authorize");
                    this._diData.Add("companyNumber", "1");
                    this._diData.Add("customerNumber", "1");
                    this._diData.Add("cardNumber", profile.CCNumber);
                    this._client2.Session.DefaultRequestHeaders.Remove("Accept");
                    this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                    KeyValuePair<string, string> pair = this._client2.Post("https://www.footaction.com/paygate/ccn", this._diData).TextResponseUri();
                    this._srr = pair.Key;
                    str = pair.Value;
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
                }
            Label_07C2:
                if (flag)
                {
                    goto Label_0693;
                }
                goto Label_0688;
            Label_07CD:
                this._ccToken = this._srr.Substring(this._srr.IndexOf("ccToken") + 6);
                this._ccToken = this._ccToken.Substring(this._ccToken.IndexOf("token"));
                this._ccToken = this._ccToken.Substring(this._ccToken.IndexOf("'") + 1);
                this._ccToken = this._ccToken.Substring(0, this._ccToken.IndexOf("'"));
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception4)
            {
                this._runner.IsError = true;
                if (exception4 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SETTING_UP);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SETTING_UP);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SETTING_UP, exception4, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");
            this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
            this._client2 = new Client(new CookieContainer(), this._runner.Proxy, true);
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                Cookie cookie = new Cookie {
                    Name = "cart-guid",
                    Value = this._cartId,
                    Domain = ".footaction.com"
                };
                this._client.Cookies.Add(cookie);
                Cookie cookie2 = new Cookie {
                    Name = "cart-guid",
                    Value = this._cartId,
                    Domain = ".footaction.com"
                };
                this._client2.Cookies.Add(cookie2);
                if (!this._client.Session.DefaultRequestHeaders.Any<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "x-csrf-token")))
                {
                    this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("x-csrf-token", this._token);
                }
                if (!this._client2.Session.DefaultRequestHeaders.Any<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "x-csrf-token")))
                {
                    this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("x-csrf-token", this._token);
                }
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        object obj3 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__21.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__0 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__0.Target(<>o__21.<>p__0, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__21.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__2.Target(<>o__21.<>p__2, <>o__21.<>p__1.Target(<>o__21.<>p__1, obj3), profile.CountryId);
                        if (<>o__21.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "name", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__4.Target(<>o__21.<>p__4, <>o__21.<>p__3.Target(<>o__21.<>p__3, obj3), profile.Country);
                        if (<>o__21.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "type", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__5.Target(<>o__21.<>p__5, obj3, "Home/Business Address");
                        if (<>o__21.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__6.Target(<>o__21.<>p__6, obj3, profile.FirstName);
                        if (<>o__21.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__7.Target(<>o__21.<>p__7, obj3, profile.LastName);
                        if (<>o__21.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line1", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__8.Target(<>o__21.<>p__8, obj3, profile.Address1);
                        if (<>o__21.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line2", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__9.Target(<>o__21.<>p__9, obj3, profile.Address2);
                        if (<>o__21.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postalCode", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__10.Target(<>o__21.<>p__10, obj3, profile.Zip);
                        if (<>o__21.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__11.Target(<>o__21.<>p__11, obj3, profile.Phone);
                        if (<>o__21.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "town", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__12.Target(<>o__21.<>p__12, obj3, profile.City);
                        if (<>o__21.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__13 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isFPO", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__13.Target(<>o__21.<>p__13, obj3, false);
                        if (<>o__21.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__14 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "setAsBilling", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__14.Target(<>o__21.<>p__14, obj3, true);
                        if (<>o__21.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__15 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__15.Target(<>o__21.<>p__15, obj3, false);
                        if (<>o__21.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "email", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__16.Target(<>o__21.<>p__16, obj3, profile.Email);
                        string stateId = "";
                        if ((profile.CountryId != "US") && (profile.CountryId != "CA"))
                        {
                            stateId = "None";
                        }
                        else
                        {
                            stateId = profile.StateId;
                        }
                        if (<>o__21.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__17 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__17.Target(<>o__21.<>p__17, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__21.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__19.Target(<>o__21.<>p__19, <>o__21.<>p__18.Target(<>o__21.<>p__18, obj3), "US-" + stateId);
                        if (<>o__21.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__21 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__20 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        object obj4 = <>o__21.<>p__21.Target(<>o__21.<>p__21, this._client, "https://www.footaction.com/api/users/carts/current/set-billing", <>o__21.<>p__20.Target(<>o__21.<>p__20, typeof(Newtonsoft.Json.JsonConvert), obj3));
                        if (<>o__21.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__22 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__22.Target(<>o__21.<>p__22, typeof(EveAIO.Extensions), obj4);
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
                string cardTypeId = profile.CardTypeId;
                string str2 = "";
                switch (cardTypeId)
                {
                    case "0":
                        str2 = "amex";
                        break;

                    case "1":
                        str2 = "visa";
                        break;

                    case "2":
                        str2 = "master";
                        break;
                }
                flag = true;
            Label_0A9C:
                if (flag)
                {
                    flag = false;
                    try
                    {
                        object obj5 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__21.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "expiryMonth", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__23.Target(<>o__21.<>p__23, obj5, profile.ExpiryMonth);
                        if (<>o__21.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "expiryYear", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__24.Target(<>o__21.<>p__24, obj5, profile.ExpiryYear);
                        if (<>o__21.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "flApiCCNumber", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__25.Target(<>o__21.<>p__25, obj5, this._ccToken);
                        if (<>o__21.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__26 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "billingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__26.Target(<>o__21.<>p__26, obj5, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__21.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "id", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__28.Target(<>o__21.<>p__28, <>o__21.<>p__27.Target(<>o__21.<>p__27, obj5), this._billingAddressId);
                        if (<>o__21.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__29 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cardType", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__29.Target(<>o__21.<>p__29, obj5, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__21.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "code", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "cardType", typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__31.Target(<>o__21.<>p__31, <>o__21.<>p__30.Target(<>o__21.<>p__30, obj5), str2);
                        if (<>o__21.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__33 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__32 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__32 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        object obj6 = <>o__21.<>p__33.Target(<>o__21.<>p__33, this._client2, "https://www.footaction.com/api/users/carts/current/payment-detail", <>o__21.<>p__32.Target(<>o__21.<>p__32, typeof(Newtonsoft.Json.JsonConvert), obj5));
                        if (<>o__21.<>p__38 == null)
                        {
                            <>o__21.<>p__38 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                        }
                        if (<>o__21.<>p__37 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__36 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__35 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__34 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footaction), argumentInfo));
                        }
                        this._srr = <>o__21.<>p__38.Target(<>o__21.<>p__38, <>o__21.<>p__37.Target(<>o__21.<>p__37, <>o__21.<>p__36.Target(<>o__21.<>p__36, <>o__21.<>p__35.Target(<>o__21.<>p__35, <>o__21.<>p__34.Target(<>o__21.<>p__34, obj6)))));
                        if (<>o__21.<>p__39 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__39 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__21.<>p__39.Target(<>o__21.<>p__39, typeof(EveAIO.Extensions), obj6);
                        if (<>o__21.<>p__47 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__47 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__41 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__21.<>p__41 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__21.<>p__40 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                        }
                        object obj7 = <>o__21.<>p__41.Target(<>o__21.<>p__41, <>o__21.<>p__40.Target(<>o__21.<>p__40, this._dynObj, "errors"), null);
                        if (<>o__21.<>p__46 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__46 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footaction), argumentInfo));
                        }
                        if (!<>o__21.<>p__46.Target(<>o__21.<>p__46, obj7))
                        {
                            if (<>o__21.<>p__45 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__45 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__44 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__44 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__43 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footaction), argumentInfo));
                            }
                        }
                        if (<>o__21.<>p__47.Target(<>o__21.<>p__47, (<>o__21.<>p__42 != null) ? obj7 : <>o__21.<>p__45.Target(<>o__21.<>p__45, obj7, <>o__21.<>p__44.Target(<>o__21.<>p__44, <>o__21.<>p__43.Target(<>o__21.<>p__43, <>o__21.<>p__42.Target(<>o__21.<>p__42, this._dynObj, "errors")), 0))))
                        {
                            if (<>o__21.<>p__58 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__58 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__57 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__57 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__52 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__52 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__51 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__50 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__50 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__49 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__48 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__56 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__55 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__55 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__54 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__21.<>p__53 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__53 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            throw <>o__21.<>p__58.Target(<>o__21.<>p__58, typeof(Exception), <>o__21.<>p__57.Target(<>o__21.<>p__57, <>o__21.<>p__52.Target(<>o__21.<>p__52, <>o__21.<>p__51.Target(<>o__21.<>p__51, <>o__21.<>p__50.Target(<>o__21.<>p__50, <>o__21.<>p__49.Target(<>o__21.<>p__49, <>o__21.<>p__48.Target(<>o__21.<>p__48, this._dynObj, "errors")), "subject")), " "), <>o__21.<>p__56.Target(<>o__21.<>p__56, <>o__21.<>p__55.Target(<>o__21.<>p__55, <>o__21.<>p__54.Target(<>o__21.<>p__54, <>o__21.<>p__53.Target(<>o__21.<>p__53, this._dynObj, "errors")), "message"))));
                        }
                        goto Label_0A9C;
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
                        goto Label_0A9C;
                    }
                }
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception3.Message.Contains("430") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception3, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                this._init = false;
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
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
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        object obj2 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__20.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__20.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cartId", typeof(Footaction), argumentInfo));
                        }
                        <>o__20.<>p__0.Target(<>o__20.<>p__0, obj2, this._cartId);
                        if (<>o__20.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__20.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "securityCode", typeof(Footaction), argumentInfo));
                        }
                        <>o__20.<>p__1.Target(<>o__20.<>p__1, obj2, this._runner.Profile.Cvv);
                        if (<>o__20.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__20.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "deviceId", typeof(Footaction), argumentInfo));
                        }
                        <>o__20.<>p__2.Target(<>o__20.<>p__2, obj2, EveAIO.Helpers.RandomString(50));
                        if (!this._client2.Session.DefaultRequestHeaders.Any<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "x-csrf-token")))
                        {
                            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("x-csrf-token", this._token);
                        }
                        if (<>o__20.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__4 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__20.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        object obj3 = <>o__20.<>p__4.Target(<>o__20.<>p__4, this._client2, "https://www.footaction.com/api/users/orders", <>o__20.<>p__3.Target(<>o__20.<>p__3, typeof(Newtonsoft.Json.JsonConvert), obj2));
                        if (<>o__20.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__20.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__20.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__20.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__20.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footaction), argumentInfo));
                        }
                        this._dynObj = <>o__20.<>p__9.Target(<>o__20.<>p__9, typeof(Newtonsoft.Json.JsonConvert), <>o__20.<>p__8.Target(<>o__20.<>p__8, <>o__20.<>p__7.Target(<>o__20.<>p__7, <>o__20.<>p__6.Target(<>o__20.<>p__6, <>o__20.<>p__5.Target(<>o__20.<>p__5, obj3)))));
                        if (<>o__20.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__10 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__20.<>p__10.Target(<>o__20.<>p__10, typeof(EveAIO.Extensions), obj3);
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (!exception.Message.Contains("504") && !exception.Message.Contains("503"))
                        {
                            if (!exception.Message.Contains("400"))
                            {
                                throw;
                            }
                            if (<>o__20.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__20.<>p__12 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                <>o__20.<>p__12 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__20.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            object obj4 = <>o__20.<>p__12.Target(<>o__20.<>p__12, <>o__20.<>p__11.Target(<>o__20.<>p__11, this._dynObj, "errors"), null);
                            if (<>o__20.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footaction), argumentInfo));
                            }
                            if (!<>o__20.<>p__17.Target(<>o__20.<>p__17, obj4))
                            {
                                if (<>o__20.<>p__16 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__20.<>p__16 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footaction), argumentInfo));
                                }
                                if (<>o__20.<>p__15 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__20.<>p__15 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footaction), argumentInfo));
                                }
                                if (<>o__20.<>p__14 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__20.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footaction), argumentInfo));
                                }
                            }
                            if (!<>o__20.<>p__18.Target(<>o__20.<>p__18, (<>o__20.<>p__13 != null) ? obj4 : <>o__20.<>p__16.Target(<>o__20.<>p__16, obj4, <>o__20.<>p__15.Target(<>o__20.<>p__15, <>o__20.<>p__14.Target(<>o__20.<>p__14, <>o__20.<>p__13.Target(<>o__20.<>p__13, this._dynObj, "errors")), 0))))
                            {
                                throw;
                            }
                            if (<>o__20.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.NamedArgument, "msg") };
                                <>o__20.<>p__23 = CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteLogger", null, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__20.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__20.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__20.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__20.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__20.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__20.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errors", typeof(Footaction), argumentInfo));
                            }
                            <>o__20.<>p__23.Target(<>o__20.<>p__23, typeof(States), this._task, States.LOGGER_STATES.CARD_DECLINED, <>o__20.<>p__22.Target(<>o__20.<>p__22, <>o__20.<>p__21.Target(<>o__20.<>p__21, <>o__20.<>p__20.Target(<>o__20.<>p__20, <>o__20.<>p__19.Target(<>o__20.<>p__19, this._dynObj)), "message")));
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            return false;
                        }
                        flag2 = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
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
                flag = false;
            }
            finally
            {
                this._init = false;
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
            }
            return flag;
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
                        object obj3 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__22.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__0 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__0.Target(<>o__22.<>p__0, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__22.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__2 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__2.Target(<>o__22.<>p__2, <>o__22.<>p__1.Target(<>o__22.<>p__1, obj3), new Newtonsoft.Json.Linq.JObject());
                        if (<>o__22.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__5.Target(<>o__22.<>p__5, <>o__22.<>p__4.Target(<>o__22.<>p__4, <>o__22.<>p__3.Target(<>o__22.<>p__3, obj3)), profile.CountryIdShipping);
                        if (<>o__22.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "name", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__8.Target(<>o__22.<>p__8, <>o__22.<>p__7.Target(<>o__22.<>p__7, <>o__22.<>p__6.Target(<>o__22.<>p__6, obj3)), profile.CountryShipping);
                        if (<>o__22.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "type", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__10.Target(<>o__22.<>p__10, <>o__22.<>p__9.Target(<>o__22.<>p__9, obj3), "Home/Business Address");
                        if (<>o__22.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__12 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "setAsBilling", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__12.Target(<>o__22.<>p__12, <>o__22.<>p__11.Target(<>o__22.<>p__11, obj3), true);
                        if (<>o__22.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__14.Target(<>o__22.<>p__14, <>o__22.<>p__13.Target(<>o__22.<>p__13, obj3), profile.FirstNameShipping);
                        if (<>o__22.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__16.Target(<>o__22.<>p__16, <>o__22.<>p__15.Target(<>o__22.<>p__15, obj3), profile.LastNameShipping);
                        if (<>o__22.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line1", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__18.Target(<>o__22.<>p__18, <>o__22.<>p__17.Target(<>o__22.<>p__17, obj3), profile.Address1Shipping);
                        if (<>o__22.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line2", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__20.Target(<>o__22.<>p__20, <>o__22.<>p__19.Target(<>o__22.<>p__19, obj3), profile.Address2Shipping);
                        if (<>o__22.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postalCode", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__22.Target(<>o__22.<>p__22, <>o__22.<>p__21.Target(<>o__22.<>p__21, obj3), profile.ZipShipping);
                        if (<>o__22.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__24.Target(<>o__22.<>p__24, <>o__22.<>p__23.Target(<>o__22.<>p__23, obj3), profile.PhoneShipping);
                        if (<>o__22.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "town", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__26.Target(<>o__22.<>p__26, <>o__22.<>p__25.Target(<>o__22.<>p__25, obj3), profile.CityShipping);
                        if (<>o__22.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__28 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isFPO", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__28.Target(<>o__22.<>p__28, <>o__22.<>p__27.Target(<>o__22.<>p__27, obj3), false);
                        if (<>o__22.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__30 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__30.Target(<>o__22.<>p__30, <>o__22.<>p__29.Target(<>o__22.<>p__29, obj3), true);
                        if (<>o__22.<>p__32 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__32 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "email", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__32.Target(<>o__22.<>p__32, <>o__22.<>p__31.Target(<>o__22.<>p__31, obj3), profile.EmailShipping);
                        string stateIdShipping = "";
                        if ((profile.CountryIdShipping != "US") && (profile.CountryIdShipping != "CA"))
                        {
                            stateIdShipping = "None";
                        }
                        else
                        {
                            stateIdShipping = profile.StateIdShipping;
                        }
                        if (<>o__22.<>p__34 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__34 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__34.Target(<>o__22.<>p__34, <>o__22.<>p__33.Target(<>o__22.<>p__33, obj3), new Newtonsoft.Json.Linq.JObject());
                        if (<>o__22.<>p__37 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__36 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__35 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__37.Target(<>o__22.<>p__37, <>o__22.<>p__36.Target(<>o__22.<>p__36, <>o__22.<>p__35.Target(<>o__22.<>p__35, obj3)), "US-" + stateIdShipping);
                        if (<>o__22.<>p__39 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__39 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__38 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__38 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        object obj4 = <>o__22.<>p__39.Target(<>o__22.<>p__39, this._client, "https://www.footaction.com/api/users/carts/current/addresses/shipping", <>o__22.<>p__38.Target(<>o__22.<>p__38, typeof(Newtonsoft.Json.JsonConvert), obj3));
                        if (<>o__22.<>p__40 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__40 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footaction), argumentInfo));
                        }
                        <>o__22.<>p__40.Target(<>o__22.<>p__40, typeof(EveAIO.Extensions), obj4);
                        if (<>o__22.<>p__45 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__45 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__44 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__43 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__42 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__41 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footaction), argumentInfo));
                        }
                        this._dynObj = <>o__22.<>p__45.Target(<>o__22.<>p__45, typeof(Newtonsoft.Json.JsonConvert), <>o__22.<>p__44.Target(<>o__22.<>p__44, <>o__22.<>p__43.Target(<>o__22.<>p__43, <>o__22.<>p__42.Target(<>o__22.<>p__42, <>o__22.<>p__41.Target(<>o__22.<>p__41, obj4)))));
                        if (<>o__22.<>p__53 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__53 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__47 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__22.<>p__47 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footaction), argumentInfo));
                        }
                        if (<>o__22.<>p__46 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__46 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                        }
                        object obj5 = <>o__22.<>p__47.Target(<>o__22.<>p__47, <>o__22.<>p__46.Target(<>o__22.<>p__46, this._dynObj, "errors"), null);
                        if (<>o__22.<>p__52 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__52 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footaction), argumentInfo));
                        }
                        if (!<>o__22.<>p__52.Target(<>o__22.<>p__52, obj5))
                        {
                            if (<>o__22.<>p__51 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__51 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__50 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__50 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__49 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footaction), argumentInfo));
                            }
                        }
                        if (!<>o__22.<>p__53.Target(<>o__22.<>p__53, (<>o__22.<>p__48 != null) ? obj5 : <>o__22.<>p__51.Target(<>o__22.<>p__51, obj5, <>o__22.<>p__50.Target(<>o__22.<>p__50, <>o__22.<>p__49.Target(<>o__22.<>p__49, <>o__22.<>p__48.Target(<>o__22.<>p__48, this._dynObj, "errors")), 0))))
                        {
                            if (<>o__22.<>p__67 == null)
                            {
                                <>o__22.<>p__67 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footaction)));
                            }
                            if (<>o__22.<>p__66 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__65 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__65 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                        }
                        else
                        {
                            if (<>o__22.<>p__64 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__64 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__63 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__63 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__58 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__58 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__57 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__56 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__56 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__55 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__54 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__54 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__62 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__61 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__61 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__60 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footaction), argumentInfo));
                            }
                            if (<>o__22.<>p__59 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__59 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footaction), argumentInfo));
                            }
                            throw <>o__22.<>p__64.Target(<>o__22.<>p__64, typeof(Exception), <>o__22.<>p__63.Target(<>o__22.<>p__63, <>o__22.<>p__58.Target(<>o__22.<>p__58, <>o__22.<>p__57.Target(<>o__22.<>p__57, <>o__22.<>p__56.Target(<>o__22.<>p__56, <>o__22.<>p__55.Target(<>o__22.<>p__55, <>o__22.<>p__54.Target(<>o__22.<>p__54, this._dynObj, "errors")), "subject")), " "), <>o__22.<>p__62.Target(<>o__22.<>p__62, <>o__22.<>p__61.Target(<>o__22.<>p__61, <>o__22.<>p__60.Target(<>o__22.<>p__60, <>o__22.<>p__59.Target(<>o__22.<>p__59, this._dynObj, "errors")), "message"))));
                        }
                        this._billingAddressId = <>o__22.<>p__67.Target(<>o__22.<>p__67, <>o__22.<>p__66.Target(<>o__22.<>p__66, <>o__22.<>p__65.Target(<>o__22.<>p__65, this._dynObj, "id")));
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
                this._init = true;
                return true;
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception2, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                this._init = false;
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                return false;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Footaction.<>c <>9;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__18_2;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__20_1;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__21_0;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__21_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Footaction.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__18_2(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "x-csrf-token");

            internal bool <SubmitBilling>b__21_0(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "x-csrf-token");

            internal bool <SubmitBilling>b__21_1(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "x-csrf-token");

            internal bool <SubmitOrder>b__20_1(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "x-csrf-token");
        }

        [CompilerGenerated]
        private static class <>o__18
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__2;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__3;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__4;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string>> <>p__10;
            public static CallSite<Action<CallSite, Type, object>> <>p__11;
        }

        [CompilerGenerated]
        private static class <>o__20
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__3;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__9;
            public static CallSite<Action<CallSite, Type, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, int, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, bool>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>> <>p__23;
        }

        [CompilerGenerated]
        private static class <>o__21
        {
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__14;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__20;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__21;
            public static CallSite<Action<CallSite, Type, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__32;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, string>> <>p__38;
            public static CallSite<Action<CallSite, Type, object>> <>p__39;
            public static CallSite<Func<CallSite, object, string, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, string, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, int, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, bool>> <>p__46;
            public static CallSite<Func<CallSite, object, bool>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, string, object>> <>p__52;
            public static CallSite<Func<CallSite, object, string, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, string, object>> <>p__55;
            public static CallSite<Func<CallSite, object, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object, object>> <>p__57;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__58;
        }

        [CompilerGenerated]
        private static class <>o__22
        {
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, string, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, string, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__38;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__39;
            public static CallSite<Action<CallSite, Type, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, string, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, int, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, bool>> <>p__52;
            public static CallSite<Func<CallSite, object, bool>> <>p__53;
            public static CallSite<Func<CallSite, object, string, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, string, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, string, object>> <>p__58;
            public static CallSite<Func<CallSite, object, string, object>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, object, object>> <>p__63;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__64;
            public static CallSite<Func<CallSite, object, string, object>> <>p__65;
            public static CallSite<Func<CallSite, object, object>> <>p__66;
            public static CallSite<Func<CallSite, object, string>> <>p__67;
        }

        [CompilerGenerated]
        private static class <>o__23
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__4;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__5;
            public static CallSite<Action<CallSite, Type, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Action<CallSite, object>> <>p__10;
        }

        [CompilerGenerated]
        private static class <>o__24
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, bool>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, Type, object, string, CultureInfo, object>> <>p__23;
            public static CallSite<Func<CallSite, object, DateTime>> <>p__24;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__25;
            public static CallSite<Func<CallSite, object, string, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, string>> <>p__39;
            public static CallSite<Func<CallSite, object, string>> <>p__40;
            public static CallSite<Func<CallSite, object, string>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string, object>> <>p__45;
            public static CallSite<Func<CallSite, object, bool>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string, object>> <>p__50;
            public static CallSite<Func<CallSite, object, bool>> <>p__51;
            public static CallSite<Func<CallSite, object, string, object>> <>p__52;
            public static CallSite<Func<CallSite, object, object>> <>p__53;
            public static CallSite<Func<CallSite, object, string>> <>p__54;
            public static CallSite<Func<CallSite, object, string, object>> <>p__55;
            public static CallSite<Func<CallSite, object, object>> <>p__56;
            public static CallSite<Func<CallSite, object, string>> <>p__57;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__58;
            public static CallSite<Func<CallSite, object, object>> <>p__59;
            public static CallSite<Func<CallSite, object, string, object>> <>p__60;
            public static CallSite<Func<CallSite, object, object>> <>p__61;
            public static CallSite<Func<CallSite, object, string, object>> <>p__62;
            public static CallSite<Func<CallSite, object, bool>> <>p__63;
            public static CallSite<Func<CallSite, object, string, object>> <>p__64;
            public static CallSite<Func<CallSite, object, object>> <>p__65;
            public static CallSite<Func<CallSite, string, object, object>> <>p__66;
            public static CallSite<Func<CallSite, object, bool>> <>p__67;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__68;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__69;
        }
    }
}

