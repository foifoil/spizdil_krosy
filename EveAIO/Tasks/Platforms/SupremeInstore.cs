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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class SupremeInstore : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;

        public SupremeInstore(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._diData = new Dictionary<string, string>();
            this._runner = runner;
            this._task = task;
            this.SetClient();
        }

        public bool Atc() => 
            true;

        public bool Checkout()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.REGISTERING);
                States.WriteLogger(this._task, States.LOGGER_STATES.REGISTERING, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get((this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU) ? "https://london.supremenewyork.com" : "https://register.supremenewyork.com").Text();
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
                if (this._srr.Contains("Registration has closed for this release"))
                {
                    goto Label_1AC4;
                }
                this._currentDoc.LoadHtml(this._srr);
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf-token"))).Attributes["content"].Value;
                ProfileObject profile = this._runner.Profile;
                object solverLocker = Global.SolverLocker;
                lock (solverLocker)
                {
                    Global.CAPTCHA_QUEUE.Add(this._task);
                }
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                this._task.Mre = new ManualResetEvent(false);
                CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.SUPREME_CAPTCHA_KEY, "https://www.supremenewyork.com", "Supreme");
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
                string str3 = "";
                string cCNumber = profile.CCNumber;
                while (cCNumber.Length > 4)
                {
                    str3 = str3 + cCNumber.Substring(0, 4);
                    cCNumber = cCNumber.Remove(0, 4);
                    str3 = str3 + " ";
                }
                str3 = str3 + cCNumber;
                this._diData.Clear();
                this._diData.Add("utf8", "✓");
                this._diData.Add("authenticity_token", str);
                this._diData.Add("customer[name]", profile.FirstName + " " + profile.LastName);
                this._diData.Add("customer[email]", profile.Email);
                this._diData.Add("customer[tel]", profile.Phone);
                if (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.USA)
                {
                    this._diData.Add("customer[location_preference]", this._task.VariousStringData);
                }
                this._diData.Add("customer[street]", profile.Address1);
                this._diData.Add("customer[street_2]", profile.Address2);
                this._diData.Add("customer[zip]", profile.Zip);
                this._diData.Add("customer[city]", profile.City);
                if (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.USA)
                {
                    this._diData.Add("customer[state]", profile.StateId);
                }
                this._diData.Add("credit_card[cn]", str3);
                this._diData.Add("credit_card[month]", int.Parse(profile.ExpiryMonth).ToString());
                this._diData.Add("credit_card[year]", profile.ExpiryYear);
                this._diData.Add("credit_card[verification_value]", profile.Cvv);
                this._diData.Add("g-recaptcha-response", waiter.Token);
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
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_REGISTRATION);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_REGISTRATION, null, "", "");
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
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-CSRF-Token", str2);
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri((this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU) ? "https://london.supremenewyork.com/signup/2" : "https://register.supremenewyork.com/signup/2");
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU) ? "https://london.supremenewyork.com" : "https://register.supremenewyork.com");
                        this._srr = this._client.Post((this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU) ? "https://london.supremenewyork.com/customers" : "https://register.supremenewyork.com/customers", this._diData).Text();
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
                if (this._srr.Contains("Registration has closed for this release"))
                {
                    goto Label_1A92;
                }
                Global.Logger.Info("SupremeInstore: " + this._srr);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                string str5 = "";
                if (<>o__10.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__3.Target(<>o__10.<>p__3, <>o__10.<>p__2.Target(<>o__10.<>p__2, <>o__10.<>p__1.Target(<>o__10.<>p__1, <>o__10.<>p__0.Target(<>o__10.<>p__0, this._dynObj, "status")), "failed")))
                {
                    goto Label_1A60;
                }
                if (<>o__10.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__7.Target(<>o__10.<>p__7, <>o__10.<>p__6.Target(<>o__10.<>p__6, <>o__10.<>p__5.Target(<>o__10.<>p__5, <>o__10.<>p__4.Target(<>o__10.<>p__4, this._dynObj, "status")), "duplicate")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ALREADY_REGISTERED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ALREADY_REGISTERED, null, "", "");
                    return false;
                }
                if (<>o__10.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__11.Target(<>o__10.<>p__11, <>o__10.<>p__10.Target(<>o__10.<>p__10, <>o__10.<>p__9.Target(<>o__10.<>p__9, <>o__10.<>p__8.Target(<>o__10.<>p__8, this._dynObj, "status")), "unavailable")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_CLOSED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_CLOSED, null, "", "");
                    return false;
                }
                if (<>o__10.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__15 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (!<>o__10.<>p__15.Target(<>o__10.<>p__15, <>o__10.<>p__14.Target(<>o__10.<>p__14, <>o__10.<>p__13.Target(<>o__10.<>p__13, <>o__10.<>p__12.Target(<>o__10.<>p__12, this._dynObj, "status")), "queued")))
                {
                    goto Label_18B1;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_REGISTRATION, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_REGISTRATION);
            Label_0CAA:
                if (<>o__10.<>p__19 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__19 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (!<>o__10.<>p__19.Target(<>o__10.<>p__19, <>o__10.<>p__18.Target(<>o__10.<>p__18, <>o__10.<>p__17.Target(<>o__10.<>p__17, <>o__10.<>p__16.Target(<>o__10.<>p__16, this._dynObj, "status")), "queued")))
                {
                    goto Label_19F7;
                }
                if (string.IsNullOrEmpty(str5))
                {
                    if (<>o__10.<>p__22 == null)
                    {
                        <>o__10.<>p__22 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(SupremeInstore)));
                    }
                    if (<>o__10.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__10.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__10.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                    }
                    str5 = <>o__10.<>p__22.Target(<>o__10.<>p__22, <>o__10.<>p__21.Target(<>o__10.<>p__21, <>o__10.<>p__20.Target(<>o__10.<>p__20, this._dynObj, "queue_slug")));
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Post((this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU) ? ("https://london.supremenewyork.com/customers/status.json?id=" + str5) : ("https://register.supremenewyork.com/customers/status.json?id=" + str5), this._diData).Text();
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
                    continue;
                Label_0F90:
                    if (this._srr.Contains("Registration has closed for this release"))
                    {
                        goto Label_187F;
                    }
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                    if (<>o__10.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__10.<>p__26 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__10.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__10.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__10.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__26.Target(<>o__10.<>p__26, <>o__10.<>p__25.Target(<>o__10.<>p__25, <>o__10.<>p__24.Target(<>o__10.<>p__24, <>o__10.<>p__23.Target(<>o__10.<>p__23, this._dynObj, "status")), "failed")))
                    {
                        goto Label_184D;
                    }
                    if (<>o__10.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__10.<>p__30 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__10.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__10.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                    }
                    if (<>o__10.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__10.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                    }
                    if (!<>o__10.<>p__30.Target(<>o__10.<>p__30, <>o__10.<>p__29.Target(<>o__10.<>p__29, <>o__10.<>p__28.Target(<>o__10.<>p__28, <>o__10.<>p__27.Target(<>o__10.<>p__27, this._dynObj, "status")), "queued")))
                    {
                        goto Label_1260;
                    }
                    Thread.Sleep(600);
                    goto Label_0CAA;
                }
                goto Label_0F90;
            Label_1260:
                if (<>o__10.<>p__34 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__34 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__33 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__32 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__31 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__34.Target(<>o__10.<>p__34, <>o__10.<>p__33.Target(<>o__10.<>p__33, <>o__10.<>p__32.Target(<>o__10.<>p__32, <>o__10.<>p__31.Target(<>o__10.<>p__31, this._dynObj, "status")), "duplicate")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ALREADY_REGISTERED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ALREADY_REGISTERED, null, "", "");
                    return false;
                }
                if (<>o__10.<>p__38 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__38 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__37 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__36 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__35 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__35 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__38.Target(<>o__10.<>p__38, <>o__10.<>p__37.Target(<>o__10.<>p__37, <>o__10.<>p__36.Target(<>o__10.<>p__36, <>o__10.<>p__35.Target(<>o__10.<>p__35, this._dynObj, "status")), "unavailable")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_CLOSED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_CLOSED, null, "", "");
                    return false;
                }
                if (<>o__10.<>p__42 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__42 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__41 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__41 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__40 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__39 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__39 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__42.Target(<>o__10.<>p__42, <>o__10.<>p__41.Target(<>o__10.<>p__41, <>o__10.<>p__40.Target(<>o__10.<>p__40, <>o__10.<>p__39.Target(<>o__10.<>p__39, this._dynObj, "status")), "outOfStock")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_DECLINED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_DECLINED, null, "", "");
                    return false;
                }
                if (<>o__10.<>p__46 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__46 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__45 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__45 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__44 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__43 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (!<>o__10.<>p__46.Target(<>o__10.<>p__46, <>o__10.<>p__45.Target(<>o__10.<>p__45, <>o__10.<>p__44.Target(<>o__10.<>p__44, <>o__10.<>p__43.Target(<>o__10.<>p__43, this._dynObj, "status")), "paid")))
                {
                    try
                    {
                        EveAIO.Helpers.AddDbValue("SupremeInstore|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    }
                    catch
                    {
                    }
                    return true;
                }
                return true;
            Label_184D:
                this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_DECLINED);
                States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_DECLINED, null, "", "");
                return false;
            Label_187F:
                this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_CLOSED);
                States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_CLOSED, null, "", "");
                return false;
            Label_18B1:
                if (<>o__10.<>p__50 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__50 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__49 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__49 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__48 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__47 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__47 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(SupremeInstore), argumentInfo));
                }
                if (<>o__10.<>p__50.Target(<>o__10.<>p__50, <>o__10.<>p__49.Target(<>o__10.<>p__49, <>o__10.<>p__48.Target(<>o__10.<>p__48, <>o__10.<>p__47.Target(<>o__10.<>p__47, this._dynObj, "status")), "outOfStock")))
                {
                    goto Label_1A2E;
                }
            Label_19F7:;
                try
                {
                    EveAIO.Helpers.AddDbValue("SupremeInstore|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                return true;
            Label_1A2E:
                this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_DECLINED);
                States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_DECLINED, null, "", "");
                return false;
            Label_1A60:
                this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_DECLINED);
                States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_DECLINED, null, "", "");
                return false;
            Label_1A92:
                this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_CLOSED);
                States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_CLOSED, null, "", "");
                return false;
            Label_1AC4:
                this._task.Status = States.GetTaskState(States.TaskState.REGISTRATION_CLOSED);
                States.WriteLogger(this._task, States.LOGGER_STATES.REGISTRATION_CLOSED, null, "", "");
                return false;
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_REGISTRATION);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception4.Message.Contains("404") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("404")))
                {
                    if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_REGISTRATION);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_REGISTRATION, exception4, "", "");
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

        public bool DirectLink(string link)
        {
            Product product1 = new Product {
                ProductTitle = "Supreme Instore Signup",
                Link = (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU) ? "https://london.supremenewyork.com" : "https://register.supremenewyork.com",
                Price = "-"
            };
            this._runner.Product = product1;
            this._task.ImgUrl = "http://eve-robotics.com/dummy_product.png";
            this._runner.PickedSize = new KeyValuePair<string, string>("-", "-");
            return true;
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
            this._client.SetMobileAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SupremeInstore.<>c <>9;
            public static Func<HtmlNode, bool> <>9__10_0;
            public static Func<HtmlNode, bool> <>9__10_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new SupremeInstore.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Checkout>b__10_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "authenticity_token"));

            internal bool <Checkout>b__10_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf-token"));
        }

        [CompilerGenerated]
        private static class <>o__10
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, bool>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, bool>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, bool>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, bool>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, bool>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, bool>> <>p__34;
            public static CallSite<Func<CallSite, object, string, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, bool>> <>p__38;
            public static CallSite<Func<CallSite, object, string, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, string, object>> <>p__41;
            public static CallSite<Func<CallSite, object, bool>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string, object>> <>p__45;
            public static CallSite<Func<CallSite, object, bool>> <>p__46;
            public static CallSite<Func<CallSite, object, string, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object>> <>p__48;
            public static CallSite<Func<CallSite, object, string, object>> <>p__49;
            public static CallSite<Func<CallSite, object, bool>> <>p__50;
        }
    }
}

