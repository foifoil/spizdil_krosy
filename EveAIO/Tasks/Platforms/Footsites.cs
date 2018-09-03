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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Footsites : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _requestKey;
        private string _hbg;
        private string _sku;
        private string _websiteLink;
        private string _mobileLink;
        private int _maxRepetitions;
        private string SELECTEDMETHODCODE;
        private string SHIPHASH;
        private string SELECTEDMETHODNAME;
        private string LGR;
        private string _deviceId;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;
        private bool _isNewFootAction;
        private string _billingAddressId;
        private string _cartId;
        private static string _asyncEastbay;
        private static string _asyncChamps;
        private static string _asyncFootaction;
        private static string _asyncFootlockerUS;
        private static string _asyncFootlockerCA;

        public Footsites(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._maxRepetitions = 0x186a0;
            this.SELECTEDMETHODCODE = "";
            this.SHIPHASH = "";
            this.SELECTEDMETHODNAME = "";
            this.LGR = "";
            this._deviceId = "";
            this._currentDoc = new HtmlDocument();
            this._diData = new Dictionary<string, string>();
            this._runner = runner;
            this._task = task;
            this.SetClient();
        }

        public bool Atc()
        {
            this.GetSensorData();
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                if (this._isNewFootAction)
                {
                    return this.AtcFootAction();
                }
                this._diData.Clear();
                this._diData.Add("requestKey", this._requestKey);
                this._diData.Add("sku", this._sku);
                this._diData.Add("qty", "1");
                this._diData.Add("size", this._runner.PickedSize.Value.Key);
                this._diData.Add("quantity", "1");
                this._diData.Add("the_model_nbr", "");
                this._diData.Add("storeNumber", "0");
                this._diData.Add("fulfillmentType", "SHIP_TO_HOME");
                this._diData.Add("hasXYPromo", "false");
                this._diData.Add("sameDayDeliveryConfig", "1");
                this._diData.Add("model_name", "");
                this._diData.Add("skipISA", "no");
                this._diData.Add("selectedPrice", "false");
                this._diData.Add("BV_TrackingTag_Review_Display_Sort", "");
                this._dynObj = this._client.Post(this._websiteLink + "/index.cfm?uri=add2cart&fragment=true", this._diData).Json();
                if (<>o__45.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__45.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                }
                if (<>o__45.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__45.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footsites), argumentInfo));
                }
                if (<>o__45.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__45.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                }
                if (<>o__45.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__45.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                }
                if (<>o__45.<>p__3.Target(<>o__45.<>p__3, <>o__45.<>p__2.Target(<>o__45.<>p__2, <>o__45.<>p__1.Target(<>o__45.<>p__1, <>o__45.<>p__0.Target(<>o__45.<>p__0, this._dynObj, "success")), true)))
                {
                    if (<>o__45.<>p__6 == null)
                    {
                        <>o__45.<>p__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                    }
                    if (<>o__45.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__45.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__45.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__45.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                    }
                    this._requestKey = <>o__45.<>p__6.Target(<>o__45.<>p__6, <>o__45.<>p__5.Target(<>o__45.<>p__5, <>o__45.<>p__4.Target(<>o__45.<>p__4, this._dynObj, "nextRequestKey")));
                    if (!string.IsNullOrEmpty(this._task.VariousStringData))
                    {
                        if (<>o__45.<>p__12 == null)
                        {
                            <>o__45.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                        }
                        if (<>o__45.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__45.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__45.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__45.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__45.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__45.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__45.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__45.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "last", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__45.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__45.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "cart", typeof(Footsites), argumentInfo));
                        }
                        string str = <>o__45.<>p__12.Target(<>o__45.<>p__12, <>o__45.<>p__11.Target(<>o__45.<>p__11, <>o__45.<>p__10.Target(<>o__45.<>p__10, <>o__45.<>p__9.Target(<>o__45.<>p__9, <>o__45.<>p__8.Target(<>o__45.<>p__8, <>o__45.<>p__7.Target(<>o__45.<>p__7, this._dynObj)), "SKU")))).ToLowerInvariant();
                        if (!this._task.VariousStringData.ToLowerInvariant().Contains(str))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.CART_CHECK_NOT_PASSED, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                            return false;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CART_CHECK_PASSED, null, "", "");
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                if (!this._srr.Contains("Item Out Of Stock"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
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
                this._runner.IsError = true;
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
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
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        private bool AtcFootAction()
        {
            this._dynObj = this._client.Get("https://www.footaction.com/api/session?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds()).Json();
            object obj2 = new Newtonsoft.Json.Linq.JObject();
            if (<>o__44.<>p__0 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__44.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "productId", typeof(Footsites), argumentInfo));
            }
            <>o__44.<>p__0.Target(<>o__44.<>p__0, obj2, this._runner.PickedSize.Value.Value);
            if (<>o__44.<>p__1 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__44.<>p__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "productQuantity", typeof(Footsites), argumentInfo));
            }
            <>o__44.<>p__1.Target(<>o__44.<>p__1, obj2, 1);
            if (<>o__44.<>p__5 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__5 = CallSite<Action<CallSite, HttpRequestHeaders, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryAddWithoutValidation", null, typeof(Footsites), argumentInfo));
            }
            if (<>o__44.<>p__4 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
            }
            if (<>o__44.<>p__3 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__44.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
            }
            if (<>o__44.<>p__2 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "data", typeof(Footsites), argumentInfo));
            }
            <>o__44.<>p__5.Target(<>o__44.<>p__5, this._client.Session.DefaultRequestHeaders, "x-csrf-token", <>o__44.<>p__4.Target(<>o__44.<>p__4, <>o__44.<>p__3.Target(<>o__44.<>p__3, <>o__44.<>p__2.Target(<>o__44.<>p__2, this._dynObj), "csrfToken")));
            if (<>o__44.<>p__7 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__7 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footsites), argumentInfo));
            }
            if (<>o__44.<>p__6 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__6 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footsites), argumentInfo));
            }
            object obj3 = <>o__44.<>p__7.Target(<>o__44.<>p__7, this._client, "https://www.footaction.com/api/users/carts/current/entries?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds(), <>o__44.<>p__6.Target(<>o__44.<>p__6, typeof(Newtonsoft.Json.JsonConvert), obj2));
            if (<>o__44.<>p__12 == null)
            {
                <>o__44.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
            }
            if (<>o__44.<>p__11 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
            }
            if (<>o__44.<>p__10 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footsites), argumentInfo));
            }
            if (<>o__44.<>p__9 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footsites), argumentInfo));
            }
            if (<>o__44.<>p__8 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footsites), argumentInfo));
            }
            this._srr = <>o__44.<>p__12.Target(<>o__44.<>p__12, <>o__44.<>p__11.Target(<>o__44.<>p__11, <>o__44.<>p__10.Target(<>o__44.<>p__10, <>o__44.<>p__9.Target(<>o__44.<>p__9, <>o__44.<>p__8.Target(<>o__44.<>p__8, obj3)))));
            if (<>o__44.<>p__13 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__44.<>p__13 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footsites), argumentInfo));
            }
            <>o__44.<>p__13.Target(<>o__44.<>p__13, typeof(EveAIO.Extensions), obj3);
            if (this._srr.Contains(this._runner.PickedSize.Value.Value))
            {
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__44.<>p__16 == null)
                {
                    <>o__44.<>p__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                }
                if (<>o__44.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__44.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                }
                if (<>o__44.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__44.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                }
                this._cartId = <>o__44.<>p__16.Target(<>o__44.<>p__16, <>o__44.<>p__15.Target(<>o__44.<>p__15, <>o__44.<>p__14.Target(<>o__44.<>p__14, this._dynObj, "guid")));
                States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                return true;
            }
            States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
            this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
            return false;
        }

        private bool CheckInventory()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_INVENTORY);
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_INVENTORY, null, "", "");
                if (this._isNewFootAction)
                {
                    return this.CheckInventoryFootAction();
                }
                int num2 = 0;
                bool flag = true;
                while (flag)
                {
                    if (num2 >= this._maxRepetitions)
                    {
                        break;
                    }
                    num2++;
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get($"{this._websiteLink}/pdp/gateway?requestKey={this._requestKey}&action=checkout").Text();
                        continue;
                    }
                    catch (Exception exception)
                    {
                        if (((num2 >= this._maxRepetitions) || !(exception is WebException)) || exception.Message.Contains("403"))
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
                num2 = 0;
                while (flag)
                {
                    if (num2 >= this._maxRepetitions)
                    {
                        break;
                    }
                    num2++;
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get(this._websiteLink + "/checkout/inventoryCheck.cfm?_=" + DateTimeOffset.Now.ToUnixTimeSeconds()).Text();
                        this._currentDoc.LoadHtml(this._srr);
                        if (this._srr.Contains("SESSION_EXPIRED"))
                        {
                            Thread.Sleep(500);
                            this._srr = this._client.Get($"{this._websiteLink}/session/").Text();
                            flag = true;
                        }
                        continue;
                    }
                    catch (WebException exception3)
                    {
                        if (exception3.Message.Contains("403"))
                        {
                            this.GetSensorData();
                            this._task.Status = States.GetTaskState(States.TaskState.CHECKING_INVENTORY);
                            States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_INVENTORY, null, "", "");
                        }
                        else
                        {
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        flag = true;
                        continue;
                    }
                }
                flag = true;
                num2 = 0;
                while (flag)
                {
                    if (num2 >= this._maxRepetitions)
                    {
                        break;
                    }
                    num2++;
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get(this._websiteLink + "/checkout/?uri=checkout/").Text();
                        this._currentDoc.LoadHtml(this._srr);
                        if (!this._srr.Contains("SESSION_EXPIRED"))
                        {
                            HtmlNode node = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "spcoForm"));
                            this._requestKey = node.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "requestKey"))).Attributes["value"].Value;
                            this._hbg = node.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "hbg"))).Attributes["value"].Value;
                        }
                        else
                        {
                            Thread.Sleep(500);
                            this._srr = this._client.Get($"{this._websiteLink}/session/").Text();
                            flag = true;
                        }
                        continue;
                    }
                    catch (WebException exception4)
                    {
                        if (exception4.Message.Contains("403"))
                        {
                            this.GetSensorData();
                            this._task.Status = States.GetTaskState(States.TaskState.CHECKING_INVENTORY);
                            States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_INVENTORY, null, "", "");
                        }
                        else
                        {
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        flag = true;
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
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("404") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("404")))
                {
                    if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_INVENTORY, exception2, "", "");
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

        private bool CheckInventoryFootAction()
        {
            this._client.Get("https://www.footaction.com/api/users/carts/current?checkInventory=true&checkPriceVariation=true&timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds()).Json();
            this._client.Get("https://www.footaction.com/api/payment/token?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds()).Json();
            return true;
        }

        public bool Checkout()
        {
            // This item is obfuscated and can not be translated.
            if (this.CheckInventory())
            {
                goto Label_00A4;
            }
            goto Label_0061;
        Label_001A:
            if (-1207989141 || true)
            {
                goto Label_0061;
            }
        Label_003B:
            if (-1142785420 || true)
            {
            }
        Label_0061:
            switch (((-523909148 ^ -2070021521) % 9))
            {
                case 0:
                    goto Label_0061;

                case 1:
                    return false;

                case 2:
                    break;

                case 3:
                    return false;

                case 4:
                    return false;

                case 5:
                    return this.SubmitOrder();

                case 7:
                    goto Label_003B;

                case 8:
                    goto Label_001A;

                default:
                    return this.SubmitOrderFootAction();
            }
        Label_00A4:
            if (!this.SubmitShipping())
            {
            }
            if (-1279108986 || true)
            {
                goto Label_0061;
            }
            goto Label_001A;
        }

        public bool DirectLink(string link)
        {
            this._deviceId = EveAIO.Helpers.RandomString(0x10);
            switch (this._task.FootSite)
            {
                case TaskObject.FootSitesEnum.footlocker:
                    this._websiteLink = "https://www.footlocker.com";
                    this._mobileLink = "https://m.footlocker.com";
                    break;

                case TaskObject.FootSitesEnum.footlockerCa:
                    this._websiteLink = "https://www.footlocker.ca";
                    this._mobileLink = "https://m.footlocker.ca";
                    break;

                case TaskObject.FootSitesEnum.footaction:
                    this._websiteLink = "https://www.footaction.com";
                    this._mobileLink = "https://m.footaction.com";
                    break;

                case TaskObject.FootSitesEnum.eastbay:
                    this._websiteLink = "https://www.eastbay.com";
                    this._mobileLink = "https://m.eastbay.com";
                    break;

                case TaskObject.FootSitesEnum.champssports:
                    this._websiteLink = "https://www.champssports.com";
                    this._mobileLink = "https://m.champssports.com";
                    break;
            }
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", this._websiteLink);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", this._websiteLink);
            return this.DirectLinkInternal(link);
        }

        private bool DirectLinkFootAction(string link)
        {
            this._srr = this._client.Get(link).Text();
            this._currentDoc.LoadHtml(this._srr);
            this._runner.ProductPageHtml = new HtmlDocument();
            this._runner.ProductPageHtml.LoadHtml(this._srr);
            string str2 = this._srr.Substring(this._srr.IndexOf(".pdpData"));
            str2 = str2.Substring(str2.IndexOf("{"));
            str2 = str2.Substring(0, str2.IndexOf("};") + 1);
            this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str2);
            if (<>o__33.<>p__1 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__33.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
            }
            if (<>o__33.<>p__0 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__33.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
            }
            object obj2 = <>o__33.<>p__1.Target(<>o__33.<>p__1, <>o__33.<>p__0.Target(<>o__33.<>p__0, this._dynObj, "name"));
            if (<>o__33.<>p__7 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__33.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
            }
            if (<>o__33.<>p__6 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__33.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
            }
            if (<>o__33.<>p__5 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__33.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
            }
            if (<>o__33.<>p__4 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__33.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "price", typeof(Footsites), argumentInfo));
            }
            if (<>o__33.<>p__3 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__33.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Footsites), argumentInfo));
            }
            if (<>o__33.<>p__2 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__33.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "variantAttributes", typeof(Footsites), argumentInfo));
            }
            object obj5 = <>o__33.<>p__7.Target(<>o__33.<>p__7, <>o__33.<>p__6.Target(<>o__33.<>p__6, <>o__33.<>p__5.Target(<>o__33.<>p__5, <>o__33.<>p__4.Target(<>o__33.<>p__4, <>o__33.<>p__3.Target(<>o__33.<>p__3, <>o__33.<>p__2.Target(<>o__33.<>p__2, this._dynObj))), "value")));
            this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "c-image product"))).Descendants("img").First<HtmlNode>().Attributes["src"].Value;
            Product product1 = new Product();
            if (<>o__33.<>p__8 == null)
            {
                <>o__33.<>p__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
            }
            product1.ProductTitle = <>o__33.<>p__8.Target(<>o__33.<>p__8, obj2);
            product1.Link = link;
            if (<>o__33.<>p__9 == null)
            {
                <>o__33.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
            }
            product1.Price = <>o__33.<>p__9.Target(<>o__33.<>p__9, obj5);
            this._runner.Product = product1;
            if (<>o__33.<>p__26 == null)
            {
                <>o__33.<>p__26 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footsites)));
            }
            if (<>o__33.<>p__10 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__33.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sellableUnits", typeof(Footsites), argumentInfo));
            }
            foreach (object obj3 in <>o__33.<>p__26.Target(<>o__33.<>p__26, <>o__33.<>p__10.Target(<>o__33.<>p__10, this._dynObj)))
            {
                if (<>o__33.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                }
                if (<>o__33.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                }
                if (<>o__33.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                }
                if (<>o__33.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                }
                if (!<>o__33.<>p__14.Target(<>o__33.<>p__14, <>o__33.<>p__13.Target(<>o__33.<>p__13, <>o__33.<>p__12.Target(<>o__33.<>p__12, <>o__33.<>p__11.Target(<>o__33.<>p__11, obj3, "stockLevelStatus")), "inStock")))
                {
                    if (<>o__33.<>p__25 == null)
                    {
                        <>o__33.<>p__25 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footsites)));
                    }
                    if (<>o__33.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "attributes", typeof(Footsites), argumentInfo));
                    }
                    foreach (object obj4 in <>o__33.<>p__25.Target(<>o__33.<>p__25, <>o__33.<>p__15.Target(<>o__33.<>p__15, obj3)))
                    {
                        if (<>o__33.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__19 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__33.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__33.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__33.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__33.<>p__19.Target(<>o__33.<>p__19, <>o__33.<>p__18.Target(<>o__33.<>p__18, <>o__33.<>p__17.Target(<>o__33.<>p__17, <>o__33.<>p__16.Target(<>o__33.<>p__16, obj4, "type")), "size")))
                        {
                            if (<>o__33.<>p__24 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__24 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__33.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                            }
                            if (<>o__33.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__33.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                            }
                            if (<>o__33.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            this._runner.Product.AvailableSizes.Add(<>o__33.<>p__24.Target(<>o__33.<>p__24, typeof(KeyValuePair<string, string>), <>o__33.<>p__21.Target(<>o__33.<>p__21, <>o__33.<>p__20.Target(<>o__33.<>p__20, obj4, "value")), <>o__33.<>p__23.Target(<>o__33.<>p__23, <>o__33.<>p__22.Target(<>o__33.<>p__22, obj4, "id"))));
                        }
                    }
                }
            }
            if (this._runner.Product.AvailableSizes.Count == 0)
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
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
                double num5 = double.Parse(str3.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                if ((num5 < this._task.MinimumPrice) || (num5 > this._task.MaximumPrice))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                    return false;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
            }
            if (this._task.RandomSize)
            {
                goto Label_0CCB;
            }
            char[] separator = new char[] { '#' };
            string[] strArray = this._task.Size.Split(separator);
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = strArray[i].Trim().ToUpperInvariant();
            }
            for (int j = 0; j < strArray.Length; j++)
            {
                if (!strArray[j].Contains("."))
                {
                    strArray[j] = strArray[j] + ".0";
                }
                if (strArray[j].IndexOf(".") != 2)
                {
                    strArray[j] = "0" + strArray[j];
                }
            }
            string[] strArray2 = strArray;
            int index = 0;
        Label_0BB1:
            if (index >= strArray2.Length)
            {
                goto Label_0CA8;
            }
            string sz = strArray2[index];
            if (this._runner.PickedSize.HasValue)
            {
                goto Label_0CA8;
            }
            using (List<KeyValuePair<string, string>>.Enumerator enumerator = this._runner.Product.AvailableSizes.GetEnumerator())
            {
                KeyValuePair<string, string> current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    char[] chArray2 = new char[] { ' ' };
                    string[] source = current.Key.Split(chArray2);
                    for (int k = 0; k < source.Length; k++)
                    {
                        source[k] = source[k].Trim().ToUpperInvariant();
                    }
                    if (source.Any<string>(x => x == sz))
                    {
                        goto Label_0C7D;
                    }
                }
                goto Label_0C9F;
            Label_0C7D:
                this._runner.PickedSize = new KeyValuePair<string, string>?(current);
            }
        Label_0C9F:
            index++;
            goto Label_0BB1;
        Label_0CA8:
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
        Label_0CCB:
            this._runner.PickedSize = new KeyValuePair<string, string>?(this._runner.Product.AvailableSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizes.Count)]);
            return true;
        }

        private bool DirectLinkInternal(string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                string tmpAsync = "";
                switch (this._task.FootSite)
                {
                    case TaskObject.FootSitesEnum.footlocker:
                        tmpAsync = _asyncFootlockerUS;
                        break;

                    case TaskObject.FootSitesEnum.footlockerCa:
                        tmpAsync = _asyncFootlockerCA;
                        break;

                    case TaskObject.FootSitesEnum.footaction:
                        tmpAsync = _asyncFootaction;
                        break;

                    case TaskObject.FootSitesEnum.eastbay:
                        tmpAsync = _asyncEastbay;
                        break;

                    case TaskObject.FootSitesEnum.champssports:
                        tmpAsync = _asyncChamps;
                        break;
                }
                if (string.IsNullOrEmpty(tmpAsync))
                {
                    Task.Factory.StartNew(delegate {
                        tmpAsync = this._client.Get(this._websiteLink + "/_bm/async.js").Text();
                        switch (this._task.FootSite)
                        {
                            case TaskObject.FootSitesEnum.footlocker:
                                _asyncFootlockerUS = tmpAsync;
                                return;

                            case TaskObject.FootSitesEnum.footlockerCa:
                                _asyncFootlockerCA = tmpAsync;
                                return;

                            case TaskObject.FootSitesEnum.footaction:
                                _asyncFootaction = tmpAsync;
                                return;

                            case TaskObject.FootSitesEnum.eastbay:
                                _asyncEastbay = tmpAsync;
                                return;

                            case TaskObject.FootSitesEnum.champssports:
                                _asyncChamps = tmpAsync;
                                return;
                        }
                    });
                }
                KeyValuePair<string, string> pair = this._client.Get(link).TextResponseUri();
                this._srr = pair.Key;
                string str2 = pair.Value;
                if (str2.Contains("footlocker.eu"))
                {
                    goto Label_1529;
                }
                if ((((str2 == "https://www.footaction.com/") || (str2 == "https://www.eastbay.com/")) || ((str2 == "https://www.champssports.com/") || (str2 == "https://www.footlocker.com/"))) || (str2 == "https://www.footlocker.ca/"))
                {
                    goto Label_14F8;
                }
                this._client.Get($"{this._websiteLink}/login/login_form.cfm?secured=false&bv_RR_enabled=false&bv_AA_enabled=false&bv_JS_enabled=true&dontRunBV=true&ignorebv=false").Text();
                this._client.Get($"{this._websiteLink}/session").Text();
                this._currentDoc.LoadHtml(this._srr);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str3 = "";
                if ((this._task.FootSite != TaskObject.FootSitesEnum.footaction) && (this._task.FootSite != TaskObject.FootSitesEnum.eastbay))
                {
                    if (this._task.FootSite != TaskObject.FootSitesEnum.footlockerCa)
                    {
                        str3 = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product_title"))).InnerText.Trim();
                    }
                    else
                    {
                        str3 = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>().InnerText.Trim();
                    }
                }
                else
                {
                    str3 = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                }
                string str = "";
                if (this._task.FootSite == TaskObject.FootSitesEnum.footaction)
                {
                    str = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim();
                }
                else if (this._task.FootSite != TaskObject.FootSitesEnum.eastbay)
                {
                    if (this._task.FootSite != TaskObject.FootSitesEnum.champssports)
                    {
                        str = this._srr.Substring(this._srr.IndexOf("PR_LIST\":"));
                        str = str.Substring(str.IndexOf(":") + 2);
                        str = str.Substring(0, str.IndexOf("\""));
                    }
                    else
                    {
                        string str4 = this._srr.Substring(this._srr.IndexOf("var sizeObj"));
                        str4 = str4.Substring(str4.IndexOf("["));
                        str4 = str4.Substring(0, str4.IndexOf("];") + 1);
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str4);
                        if (<>o__34.<>p__3 == null)
                        {
                            <>o__34.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                        }
                        if (<>o__34.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__34.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__34.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__34.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                        }
                        str = <>o__34.<>p__3.Target(<>o__34.<>p__3, <>o__34.<>p__2.Target(<>o__34.<>p__2, <>o__34.<>p__1.Target(<>o__34.<>p__1, <>o__34.<>p__0.Target(<>o__34.<>p__0, this._dynObj), "pr_list")));
                    }
                }
                else
                {
                    str = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_price"))).InnerText.Trim();
                }
                string str5 = "http://eve-robotics.com/dummy_product.png";
                if (this._task.FootSite != TaskObject.FootSitesEnum.champssports)
                {
                    this._sku = this._srr.Substring(this._srr.IndexOf("dtm_sku"));
                    this._sku = this._sku.Substring(this._sku.IndexOf("\"") + 1);
                    this._sku = this._sku.Substring(this._sku.IndexOf("dtm_sku"));
                    this._sku = this._sku.Substring(this._sku.IndexOf("\"") + 1);
                    this._sku = this._sku.Substring(0, this._sku.IndexOf("\""));
                }
                else
                {
                    this._sku = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["data-info"] != null) && (x.Attributes["data-info"].Value == "product_sku"))).InnerText.Trim();
                }
                this._task.ImgUrl = str5;
                Product product1 = new Product {
                    ProductTitle = str3,
                    Link = link,
                    Price = str
                };
                this._runner.Product = product1;
                this._requestKey = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "requestKey"))).Attributes["value"].Value;
                string str6 = this._srr.Substring(this._srr.IndexOf("styles ="));
                str6 = str6.Substring(str6.IndexOf("{"));
                str6 = str6.Substring(0, str6.IndexOf("]};") + 2);
                if (this._srr.Contains("shoelaunch_skus"))
                {
                    string str7 = this._srr.Substring(this._srr.IndexOf("shoelaunch_skus"));
                    str7 = str7.Substring(str7.IndexOf("["));
                    str7 = str7.Substring(0, str7.IndexOf("]") + 1);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str7);
                    if (<>o__34.<>p__13 == null)
                    {
                        <>o__34.<>p__13 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footsites)));
                    }
                    using (IEnumerator enumerator = <>o__34.<>p__13.Target(<>o__34.<>p__13, this._dynObj).GetEnumerator())
                    {
                        object obj2;
                        DateTime time;
                        goto Label_0B2B;
                    Label_0815:
                        obj2 = enumerator.Current;
                        if (<>o__34.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__34.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__34.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__34.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__34.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__34.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__34.<>p__7.Target(<>o__34.<>p__7, <>o__34.<>p__6.Target(<>o__34.<>p__6, <>o__34.<>p__5.Target(<>o__34.<>p__5, <>o__34.<>p__4.Target(<>o__34.<>p__4, obj2, "sku")), this._task.VariousStringData.Trim())))
                        {
                            if (<>o__34.<>p__12 == null)
                            {
                                <>o__34.<>p__12 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(DateTime), typeof(Footsites)));
                            }
                            if (<>o__34.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__34.<>p__11 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__34.<>p__10 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__34.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__34.<>p__9 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__34.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                            }
                            if (<>o__34.<>p__8 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__34.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            time = <>o__34.<>p__12.Target(<>o__34.<>p__12, <>o__34.<>p__11.Target(<>o__34.<>p__11, typeof(DateTime), <>o__34.<>p__10.Target(<>o__34.<>p__10, <>o__34.<>p__9.Target(<>o__34.<>p__9, <>o__34.<>p__8.Target(<>o__34.<>p__8, obj2, "startTime")))));
                            if (time.ToLocalTime().AddSeconds(-20.0) > DateTime.Now.ToLocalTime())
                            {
                                goto Label_0B39;
                            }
                        }
                    Label_0B2B:
                        if (!enumerator.MoveNext())
                        {
                            goto Label_0BC5;
                        }
                        goto Label_0815;
                    Label_0B39:
                        this._task.State = TaskObject.StateEnum.scheduled;
                        this._task.IsStartScheduled = true;
                        this._task.ScheduleStart = time.ToLocalTime().AddSeconds(-20.0);
                        States.WriteLogger(this._task, States.LOGGER_STATES.SCHEDULLING, null, "", this._task.ScheduleStart.ToString("MM-dd-yyyy HH:mm:ss tt", CultureInfo.InvariantCulture));
                        return false;
                    }
                }
            Label_0BC5:
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str6);
                if (<>o__34.<>p__28 == null)
                {
                    <>o__34.<>p__28 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footsites)));
                }
                foreach (object obj3 in <>o__34.<>p__28.Target(<>o__34.<>p__28, this._dynObj))
                {
                    if (<>o__34.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__34.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__34.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__34.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__34.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__34.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(Footsites), argumentInfo));
                    }
                    if (!<>o__34.<>p__16.Target(<>o__34.<>p__16, <>o__34.<>p__15.Target(<>o__34.<>p__15, <>o__34.<>p__14.Target(<>o__34.<>p__14, obj3), this._sku)))
                    {
                        if (<>o__34.<>p__27 == null)
                        {
                            <>o__34.<>p__27 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footsites)));
                        }
                        if (<>o__34.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__34.<>p__18 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__34.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__34.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Children", null, typeof(Footsites), argumentInfo));
                        }
                        foreach (object obj4 in <>o__34.<>p__27.Target(<>o__34.<>p__27, <>o__34.<>p__18.Target(<>o__34.<>p__18, <>o__34.<>p__17.Target(<>o__34.<>p__17, obj3), 7)))
                        {
                            if (<>o__34.<>p__26 == null)
                            {
                                <>o__34.<>p__26 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Footsites)));
                            }
                            if (<>o__34.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__34.<>p__20 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__34.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__34.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Children", null, typeof(Footsites), argumentInfo));
                            }
                            foreach (object obj5 in <>o__34.<>p__26.Target(<>o__34.<>p__26, <>o__34.<>p__20.Target(<>o__34.<>p__20, <>o__34.<>p__19.Target(<>o__34.<>p__19, obj4), 0)))
                            {
                                if (<>o__34.<>p__23 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__34.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__34.<>p__21 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                object obj6 = <>o__34.<>p__23.Target(<>o__34.<>p__23, <>o__34.<>p__22.Target(<>o__34.<>p__22, <>o__34.<>p__21.Target(<>o__34.<>p__21, obj5)));
                                if (<>o__34.<>p__25 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__25 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__34.<>p__24 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__34.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(Footsites), argumentInfo));
                                }
                                this._runner.Product.AvailableSizes.Add(<>o__34.<>p__25.Target(<>o__34.<>p__25, typeof(KeyValuePair<string, string>), obj6, <>o__34.<>p__24.Target(<>o__34.<>p__24, obj3)));
                            }
                        }
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
                    string str9 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str9 = str9 + ch.ToString();
                        }
                    }
                    double num5 = double.Parse(str9.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray[i] = strArray[i].Trim().ToUpperInvariant();
                    }
                    for (int j = 0; j < strArray.Length; j++)
                    {
                        if (!strArray[j].Contains("."))
                        {
                            strArray[j] = strArray[j] + ".0";
                        }
                        if (strArray[j].IndexOf(".") != 2)
                        {
                            strArray[j] = "0" + strArray[j];
                        }
                    }
                    foreach (string sz in strArray)
                    {
                        if (this._runner.PickedSize.HasValue)
                        {
                            break;
                        }
                        using (List<KeyValuePair<string, string>>.Enumerator enumerator4 = this._runner.Product.AvailableSizes.GetEnumerator())
                        {
                            KeyValuePair<string, string> pair2;
                            goto Label_1438;
                        Label_13C2:
                            pair2 = enumerator4.Current;
                            char[] chArray2 = new char[] { ' ' };
                            string[] source = pair2.Key.Split(chArray2);
                            for (int k = 0; k < source.Length; k++)
                            {
                                source[k] = source[k].Trim().ToUpperInvariant();
                            }
                            if (source.Any<string>(x => x == sz))
                            {
                                goto Label_1446;
                            }
                        Label_1438:
                            if (!enumerator4.MoveNext())
                            {
                                continue;
                            }
                            goto Label_13C2;
                        Label_1446:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(pair2);
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
            Label_14F8:
                this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                return false;
            Label_1529:
                this._task.Status = States.GetTaskState(States.TaskState.US_IP_NEEDED);
                States.WriteLogger(this._task, States.LOGGER_STATES.US_IP_NEEDED, null, "", "");
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
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
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        internal void GetSensorData()
        {
        }

        public bool Login()
        {
            throw new NotFiniteNumberException();
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive, keep-alive,Keep-Alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        public void SetMobileClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, true);
            this._client.SetMobileAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive,Keep-Alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                if (this._isNewFootAction)
                {
                    return this.SubmitBillingFootAction();
                }
                int num2 = 0;
                bool flag = true;
                while (flag)
                {
                    if (num2 >= this._maxRepetitions)
                    {
                        break;
                    }
                    num2++;
                    flag = false;
                    try
                    {
                        string[] textArray1 = new string[] { 
                            "{\"maxVisitedPane\":\"promoCodePane\",\"updateBillingForBML\":false,\"billMyAddressBookIndex\":\"-1\",\"addressNeedsVerification\":false,\"billFirstName\":\"", this._runner.Profile.FirstName, "\",\"billLastName\":\"", this._runner.Profile.LastName, "\",\"billAddress1\":\"", this._runner.Profile.Address1, "\",\"billAddress2\":\"", this._runner.Profile.Address2, "\",\"billCity\":\"", this._runner.Profile.City, "\",\"billState\":\"", (this._runner.Profile.CountryId == "US") ? this._runner.Profile.StateId : "None", "\",\"billProvince\":\"", (this._runner.Profile.CountryId == "CA") ? this._runner.Profile.StateId : "None", "\",\"billPostalCode\":\"", this._runner.Profile.Zip,
                            "\",\"billHomePhone\":\"", this._runner.Profile.Phone, "\",\"billMobilePhone\":\"\",\"billCountry\":\"", this._runner.Profile.CountryId, "\",\"billEmailAddress\":\"", this._runner.Profile.Email, "\",\"billConfirmEmail\":\"", this._runner.Profile.Email, "\",\"billAddrIsPhysical\":true,\"billSubscribePhone\":false,\"billAbbreviatedAddress\":false,\"shipUpdateDefaultAddress\":false,\"VIPNumber\":\"\",\"accountBillAddress\":{\"billMyAddressBookIndex\":-1},\"selectedBillAddress\":{},\"billMyAddressBook\":[],\"shipMyAddressBookIndex\":-2,\"shipContactID\":\"\",\"shipFirstName\":\"", this._runner.Profile.FirstNameShipping, "\",\"shipLastName\":\"", this._runner.Profile.LastNameShipping, "\",\"shipAddress1\":\"", this._runner.Profile.Address1Shipping, "\",\"shipAddress2\":\"", this._runner.Profile.Address2Shipping,
                            "\",\"shipCity\":\"", this._runner.Profile.CityShipping, "\",\"shipState\":\"", (this._runner.Profile.CountryIdShipping == "US") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipProvince\":\"", (this._runner.Profile.CountryIdShipping == "CA") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipPostalCode\":\"", this._runner.Profile.ZipShipping, "\",\"shipHomePhone\":\"", this._runner.Profile.PhoneShipping, "\",\"shipMobilePhone\":\"\",\"shipCountry\":\"", this._runner.Profile.CountryIdShipping, "\",\"shipToStore\":false,\"shipHash\":\"", this.SHIPHASH, "\",\"shipMultiple\":false,\"isShipToStoreEligibleCheckout\":true,\"isMultipleAddressEligible\":false,\"shipAbbreviatedAddress\":false,\"selectedStore\":{},\"accountShipAddress\":{\"shipMyAddressBookIndex\":-1},\"selectedShipAddress\":{},\"shipMyAddressBook\":[],\"shipMethodCode\":\"", this.SELECTEDMETHODCODE,
                            "\",\"shipMethodName\":\"", this.SELECTEDMETHODNAME, "\",\"shipMethodPrice\":\"$0.00\",\"shipDeliveryEstimate\":\"\",\"shipMethodCodeSDD\":\"\",\"shipMethodNameSDD\":\"\",\"shipMethodPriceSDD\":\"$0.00\",\"shipDeliveryEstimateSDD\":\"\",\"shipMethodCodeS2S\":\"\",\"shipMethodNameS2S\":\"\",\"shipMethodPriceS2S\":\"$0.00\",\"shipDeliveryEstimateS2S\":\"\",\"shipMethodCodeSFS\":\"\",\"shipMethodNameSFS\":\"\",\"shipMethodPriceSFS\":\"$0.00\",\"shipDeliveryEstimateSFS\":\"\",\"homeDeliveryPrice\":\"$0.00\",\"overallHomeDeliveryPrice\":\"$0.00\",\"aggregatedDeliveryPrice\":\"FREE\",\"aggregatedDeliveryLabel\":\"\",\"showGiftBoxOption\":true,\"addGiftBox\":false,\"giftBoxPrice\":\"$3.99\",\"useGiftReceipt\":false,\"showGiftOptions\":true,\"loyaltyMessageText\":false,\"showLoyaltyRenewalMessage\":false,\"pickupPersonFirstName\":\"\",\"pickupPersonLastName\":\"\",\"preferredLanguage\":\"\",\"advanceToConfirm\":false,\"payType\":\"NO_PAYMENT_METHOD\",\"payPalToken\":\"\",\"payPalInContext\":true,\"payPalMerchantId\":\"\",\"payPalStage\":\"\",\"payPalPaymentAllowed\":true,\"payMethodPaneExpireMonth\":\"\",\"payMethodPaneExpireYear\":\"\",\"payMethodPaneCardNumber\":\"\",\"payMethodPaneCardType\":\"\",\"payMethodPaneLastFour\":\"\",\"payMethodPanePurchaseOrder\":\"\",\"payMethodPanePurchaseOrderNewCustomer\":\"\",\"payMethodPaneCVV\":\"\",\"creditcardPaymentAllowed\":true,\"billMeLaterStage\":\"\",\"BMLPaymentAllowed\":true,\"displayBMLPromotion\":false,\"POPaymentAllowed\":false,\"promoType\":\"\",\"promoCode\":\"\",\"sourceCode\":\"INETSRC\",\"sourceCodeDescription\":\"\",\"sourceCodeCartDisplayText\":\"\",\"GIFTCARDCODE1\":\"\",\"GIFTCARDPIN1\":\"\",\"GIFTCARDUSED1\":\"\",\"GIFTCARDCODE2\":\"\",\"GIFTCARDPIN2\":\"\",\"GIFTCARDUSED2\":\"\",\"GIFTCARDCODE3\":\"\",\"GIFTCARDPIN3\":\"\",\"GIFTCARDUSED3\":\"\",\"GIFTCARDCODE4\":\"\",\"GIFTCARDPIN4\":\"\",\"GIFTCARDUSED4\":\"\",\"GIFTCARDCODE5\":\"\",\"GIFTCARDPIN5\":\"\",\"GIFTCARDUSED5\":\"\",\"rewardBarCode\":\"\",\"giftCardsEmpty\":true,\"sourceCodesEmpty\":true,\"ContingencyQueue\":\"\",\"lgr\":\"", this.LGR, "\",\"displayEmailOptIn\":false,\"billSubscribeEmail\":false,\"billReceiveNewsletter\":false,\"billFavoriteTeams\":false,\"paypalEmailAddress\":\"\",\"displaySheerIdIframe\":true,\"displayCmsEntry\":\"\",\"payMethodPaneUserGotStoredCC\":false,\"payMethodPaneHasStoredCC\":false,\"payMethodPaneUsedStoredCC\":false,\"payMethodPaneSavedNewCC\":false,\"selectedCreditCard\":{\"payMethodPaneHasCVV\":true},\"payMethodPaneHasCVV\":true}"
                        };
                        string str = string.Concat(textArray1);
                        string str2 = "";
                        switch (this._runner.Profile.CardTypeId)
                        {
                            case "0":
                                str2 = "amex";
                                break;

                            case "1":
                                str2 = "visa";
                                break;

                            case "2":
                                str2 = "mc";
                                break;
                        }
                        this._diData.Clear();
                        this._diData.Add("verifiedCheckoutData", str);
                        this._diData.Add("requestKey", this._requestKey);
                        this._diData.Add("hbg", this._hbg);
                        this._diData.Add("bb_device_id", this._deviceId);
                        this._diData.Add("addressBookEnabled", "true");
                        this._diData.Add("loginHeaderEmailAddress", "");
                        this._diData.Add("loginHeaderPassword", "");
                        this._diData.Add("loginPaneNewEmailAddress", "");
                        this._diData.Add("loginPaneConfirmNewEmailAddress", "");
                        this._diData.Add("loginPaneEmailAddress", "");
                        this._diData.Add("loginPanePassword", "");
                        this._diData.Add("billAddressType", "new");
                        this._diData.Add("billAddressInputType", "single");
                        this._diData.Add("billAPOFPOCountry", this._runner.Profile.CountryId);
                        this._diData.Add("billCountry", this._runner.Profile.CountryId);
                        this._diData.Add("billMyAddressBookIndex", "-1");
                        this._diData.Add("billFirstName", this._runner.Profile.FirstName);
                        this._diData.Add("billLastName", this._runner.Profile.LastName);
                        this._diData.Add("billAddress1", this._runner.Profile.Address1);
                        this._diData.Add("billAddress2", this._runner.Profile.Address2);
                        this._diData.Add("billPostalCode", this._runner.Profile.Zip);
                        this._diData.Add("billCity", this._runner.Profile.City);
                        this._diData.Add("billAPOFPORegion", "");
                        if (this._runner.Profile.CountryId == "US")
                        {
                            this._diData.Add("billState", this._runner.Profile.StateId);
                        }
                        else
                        {
                            this._diData.Add("billState", "None");
                        }
                        if (this._runner.Profile.CountryId == "CA")
                        {
                            this._diData.Add("billProvince", this._runner.Profile.StateId);
                        }
                        else
                        {
                            this._diData.Add("billProvince", "None");
                        }
                        this._diData.Add("billAPOFPOState", "");
                        this._diData.Add("billAPOFPOPostalCode", "");
                        this._diData.Add("billHomePhone", this._runner.Profile.Phone);
                        this._diData.Add("billEmailAddress", this._runner.Profile.Email);
                        this._diData.Add("billConfirmEmail", this._runner.Profile.Email);
                        this._diData.Add("shipAddressType", "new");
                        this._diData.Add("shipAddressInputType", "single");
                        this._diData.Add("shipAPOFPOCountry", this._runner.Profile.CountryIdShipping);
                        this._diData.Add("shipCountry", this._runner.Profile.CountryIdShipping);
                        this._diData.Add("shipMyAddressBookIndex", "-1");
                        this._diData.Add("shipToStore", "false");
                        this._diData.Add("shipFirstName", this._runner.Profile.FirstNameShipping);
                        this._diData.Add("shipLastName", this._runner.Profile.LastNameShipping);
                        this._diData.Add("shipAddress1", this._runner.Profile.Address1Shipping);
                        this._diData.Add("shipAddress2", this._runner.Profile.Address2Shipping);
                        this._diData.Add("shipPostalCode", this._runner.Profile.ZipShipping);
                        this._diData.Add("shipCity", this._runner.Profile.CityShipping);
                        this._diData.Add("shipAPOFPORegion", "");
                        if (this._runner.Profile.CountryIdShipping == "US")
                        {
                            this._diData.Add("shipState", this._runner.Profile.StateIdShipping);
                        }
                        else
                        {
                            this._diData.Add("shipState", "None");
                        }
                        if (this._runner.Profile.CountryIdShipping != "CA")
                        {
                            this._diData.Add("shipProvince", "None");
                        }
                        else
                        {
                            this._diData.Add("shipProvince", this._runner.Profile.StateIdShipping);
                        }
                        this._diData.Add("shipAPOFPOState", "");
                        this._diData.Add("shipAPOFPOPostalCode", "");
                        this._diData.Add("shipHomePhone", this._runner.Profile.PhoneShipping);
                        this._diData.Add("shipMethodCodeS2S", "");
                        this._diData.Add("shipMethodCode", this.SELECTEDMETHODCODE);
                        this._diData.Add("storePickupInputPostalCode", "");
                        this._diData.Add("promoType", "");
                        this._diData.Add("CPCOrSourceCode", "");
                        this._diData.Add("payMethodPanePayType", "CC");
                        this._diData.Add("payMethodPanestoredCCCardNumber", "CC");
                        this._diData.Add("CardNumber", this._runner.Profile.CCNumber.Replace(" ", "").Trim());
                        this._diData.Add("CardExpireDateMM", (this._runner.Profile.ExpiryMonth.Length == 2) ? this._runner.Profile.ExpiryMonth : ("0" + this._runner.Profile.ExpiryMonth));
                        this._diData.Add("CardExpireDateYY", this._runner.Profile.ExpiryYear.Substring(2));
                        this._diData.Add("CardCCV", this._runner.Profile.Cvv);
                        this._diData.Add("payMethodPaneStoredType", "");
                        this._diData.Add("payMethodPaneConfirmCardNumber", "");
                        this._diData.Add("payMethodPaneStoredCCExpireMonth", "");
                        this._diData.Add("payMethodPaneStoredCCExpireYear", "");
                        this._diData.Add("payMethodPaneCardType", str2);
                        this._diData.Add("payMethodPaneCardNumber", this._runner.Profile.CCNumber.Replace(" ", "").Trim());
                        this._diData.Add("payMethodPaneExpireMonth", (this._runner.Profile.ExpiryMonth.Length == 2) ? this._runner.Profile.ExpiryMonth : ("0" + this._runner.Profile.ExpiryMonth));
                        this._diData.Add("payMethodPaneExpireYear", this._runner.Profile.ExpiryYear.Substring(2));
                        this._diData.Add("payMethodPaneCVV", this._runner.Profile.Cvv);
                        this._diData.Add("payMethodPaneStoredCCCVV", "");
                        foreach (Cookie cookie in this._client.Cookies.List())
                        {
                            if (cookie.Domain == this._websiteLink.Replace("https://", ""))
                            {
                                Cookie cookie1 = new Cookie {
                                    Domain = this._mobileLink.Replace("https://", ""),
                                    Value = cookie.Value,
                                    Name = cookie.Name
                                };
                                this._client.Cookies.Add(cookie1);
                            }
                        }
                        this._srr = this._client.Post(this._mobileLink + "/checkout/eventGateway?method=validatePaymentMethodPane", this._diData).Text();
                        if (this._srr.Contains("SESSION_EXPIRED"))
                        {
                            Thread.Sleep(500);
                            this._srr = this._client.Get($"{this._websiteLink}/session/").Text();
                            flag = true;
                        }
                        else
                        {
                            this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr.Substring(2));
                            if (<>o__39.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__39.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                <>o__39.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__39.<>p__0 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__39.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            object obj3 = <>o__39.<>p__1.Target(<>o__39.<>p__1, <>o__39.<>p__0.Target(<>o__39.<>p__0, this._dynObj, "ERRORS"), null);
                            if (<>o__39.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                            }
                            if (!<>o__39.<>p__6.Target(<>o__39.<>p__6, obj3))
                            {
                                if (<>o__39.<>p__5 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__39.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__39.<>p__4 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__39.<>p__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__39.<>p__3 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__39.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footsites), argumentInfo));
                                }
                            }
                            if (<>o__39.<>p__7.Target(<>o__39.<>p__7, (<>o__39.<>p__2 != null) ? obj3 : <>o__39.<>p__5.Target(<>o__39.<>p__5, obj3, <>o__39.<>p__4.Target(<>o__39.<>p__4, <>o__39.<>p__3.Target(<>o__39.<>p__3, <>o__39.<>p__2.Target(<>o__39.<>p__2, this._dynObj, "ERRORS")), 0))))
                            {
                                if (<>o__39.<>p__11 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__39.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__39.<>p__10 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__39.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__39.<>p__9 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__39.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__39.<>p__8 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__39.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                object obj2 = <>o__39.<>p__11.Target(<>o__39.<>p__11, <>o__39.<>p__10.Target(<>o__39.<>p__10, <>o__39.<>p__9.Target(<>o__39.<>p__9, <>o__39.<>p__8.Target(<>o__39.<>p__8, this._dynObj, "ERRORS")), "RESPONSEMESSAGE"));
                                if (<>o__39.<>p__12 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__39.<>p__12 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                throw <>o__39.<>p__12.Target(<>o__39.<>p__12, typeof(Exception), obj2);
                            }
                            if (<>o__39.<>p__15 == null)
                            {
                                <>o__39.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                            }
                            if (<>o__39.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                            }
                            if (<>o__39.<>p__13 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__39.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            this._requestKey = <>o__39.<>p__15.Target(<>o__39.<>p__15, <>o__39.<>p__14.Target(<>o__39.<>p__14, <>o__39.<>p__13.Target(<>o__39.<>p__13, this._dynObj, "REQUESTKEY")));
                            if (<>o__39.<>p__18 == null)
                            {
                                <>o__39.<>p__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                            }
                            if (<>o__39.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__39.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                            }
                            if (<>o__39.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__39.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                            }
                            this._hbg = <>o__39.<>p__18.Target(<>o__39.<>p__18, <>o__39.<>p__17.Target(<>o__39.<>p__17, <>o__39.<>p__16.Target(<>o__39.<>p__16, this._dynObj, "hbg")));
                        }
                        continue;
                    }
                    catch (WebException exception1)
                    {
                        if (exception1.Message.Contains("403"))
                        {
                            this.GetSensorData();
                            this._srr = this._client.Post(this._mobileLink + "/checkout/eventGateway?method=validatePaymentMethodPane", this._diData).Text();
                            this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                            States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                        }
                        else
                        {
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        flag = true;
                        continue;
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
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
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
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        private bool SubmitBillingFootAction()
        {
            ProfileObject profile = this._runner.Profile;
            bool flag = true;
            while (flag)
            {
                flag = false;
                try
                {
                    object obj5 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__38.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__0 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__0.Target(<>o__38.<>p__0, obj5, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__38.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__2.Target(<>o__38.<>p__2, <>o__38.<>p__1.Target(<>o__38.<>p__1, obj5), "US");
                    if (<>o__38.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "name", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__4.Target(<>o__38.<>p__4, <>o__38.<>p__3.Target(<>o__38.<>p__3, obj5), "United States");
                    if (<>o__38.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "type", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__5.Target(<>o__38.<>p__5, obj5, "Home/Business Address");
                    if (<>o__38.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__6.Target(<>o__38.<>p__6, obj5, profile.FirstNameShipping);
                    if (<>o__38.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__7.Target(<>o__38.<>p__7, obj5, profile.LastNameShipping);
                    if (<>o__38.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line1", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__8.Target(<>o__38.<>p__8, obj5, profile.Address1Shipping);
                    if (<>o__38.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line2", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__9.Target(<>o__38.<>p__9, obj5, profile.Address2Shipping);
                    if (<>o__38.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postalCode", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__10.Target(<>o__38.<>p__10, obj5, profile.ZipShipping);
                    if (<>o__38.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__11.Target(<>o__38.<>p__11, obj5, profile.PhoneShipping);
                    if (<>o__38.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "town", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__12.Target(<>o__38.<>p__12, obj5, profile.CityShipping);
                    if (<>o__38.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__38.<>p__13 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionFPO", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__13.Target(<>o__38.<>p__13, obj5, null);
                    if (<>o__38.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__14 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isFPO", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__14.Target(<>o__38.<>p__14, obj5, false);
                    if (<>o__38.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__15 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__15.Target(<>o__38.<>p__15, obj5, true);
                    if (<>o__38.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "recordType", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__16.Target(<>o__38.<>p__16, obj5, " ");
                    if (<>o__38.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__17 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "visibleInAddressBook", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__17.Target(<>o__38.<>p__17, obj5, true);
                    if (<>o__38.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__18 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "setAsBilling", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__18.Target(<>o__38.<>p__18, obj5, false);
                    if (<>o__38.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__19 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__19.Target(<>o__38.<>p__19, obj5, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__38.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "countryIso", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__21.Target(<>o__38.<>p__21, <>o__38.<>p__20.Target(<>o__38.<>p__20, obj5), "US");
                    if (<>o__38.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__23.Target(<>o__38.<>p__23, <>o__38.<>p__22.Target(<>o__38.<>p__22, obj5), "US-AL");
                    if (<>o__38.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocodeShort", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__25.Target(<>o__38.<>p__25, <>o__38.<>p__24.Target(<>o__38.<>p__24, obj5), "AL");
                    if (<>o__38.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "name", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__27.Target(<>o__38.<>p__27, <>o__38.<>p__26.Target(<>o__38.<>p__26, obj5), "Alabama");
                    if (<>o__38.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__29 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__28 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footsites), argumentInfo));
                    }
                    object obj7 = <>o__38.<>p__29.Target(<>o__38.<>p__29, this._client, "https://www.footaction.com/api/users/carts/current/set-billing?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds(), <>o__38.<>p__28.Target(<>o__38.<>p__28, typeof(Newtonsoft.Json.JsonConvert), obj5));
                    if (<>o__38.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__30 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__30.Target(<>o__38.<>p__30, typeof(EveAIO.Extensions), obj7);
                    if (<>o__38.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__35 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footsites), argumentInfo));
                    }
                    this._dynObj = <>o__38.<>p__35.Target(<>o__38.<>p__35, typeof(Newtonsoft.Json.JsonConvert), <>o__38.<>p__34.Target(<>o__38.<>p__34, <>o__38.<>p__33.Target(<>o__38.<>p__33, <>o__38.<>p__32.Target(<>o__38.<>p__32, <>o__38.<>p__31.Target(<>o__38.<>p__31, obj7)))));
                    if (<>o__38.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__43 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__38.<>p__37 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__36 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                    }
                    object obj6 = <>o__38.<>p__37.Target(<>o__38.<>p__37, <>o__38.<>p__36.Target(<>o__38.<>p__36, this._dynObj, "errors"), null);
                    if (<>o__38.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__42 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                    }
                    if (!<>o__38.<>p__42.Target(<>o__38.<>p__42, obj6))
                    {
                        if (<>o__38.<>p__41 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__41 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__40 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__40 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__39 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footsites), argumentInfo));
                        }
                    }
                    if (<>o__38.<>p__43.Target(<>o__38.<>p__43, (<>o__38.<>p__38 != null) ? obj6 : <>o__38.<>p__41.Target(<>o__38.<>p__41, obj6, <>o__38.<>p__40.Target(<>o__38.<>p__40, <>o__38.<>p__39.Target(<>o__38.<>p__39, <>o__38.<>p__38.Target(<>o__38.<>p__38, this._dynObj, "errors")), 0))))
                    {
                        if (<>o__38.<>p__54 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__54 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__53 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__53 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__48 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__47 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__46 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__46 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__45 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__44 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__44 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__52 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__51 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__51 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__50 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__38.<>p__49 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__49 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        throw <>o__38.<>p__54.Target(<>o__38.<>p__54, typeof(Exception), <>o__38.<>p__53.Target(<>o__38.<>p__53, <>o__38.<>p__48.Target(<>o__38.<>p__48, <>o__38.<>p__47.Target(<>o__38.<>p__47, <>o__38.<>p__46.Target(<>o__38.<>p__46, <>o__38.<>p__45.Target(<>o__38.<>p__45, <>o__38.<>p__44.Target(<>o__38.<>p__44, this._dynObj, "errors")), "subject")), " "), <>o__38.<>p__52.Target(<>o__38.<>p__52, <>o__38.<>p__51.Target(<>o__38.<>p__51, <>o__38.<>p__50.Target(<>o__38.<>p__50, <>o__38.<>p__49.Target(<>o__38.<>p__49, this._dynObj, "errors")), "message"))));
                    }
                    continue;
                }
                catch (WebException exception6)
                {
                    if (!exception6.Message.Contains("504") && !exception6.Message.Contains("503"))
                    {
                        throw;
                    }
                    flag = true;
                    States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                    Thread.Sleep(0x3e8);
                    continue;
                }
            }
            this._dynObj = this._client.Get("https://www.footaction.com/api/users/carts/current?checkInventory=true&checkPriceVariation=true&timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds()).Json();
            if (<>o__38.<>p__58 == null)
            {
                <>o__38.<>p__58 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
            }
            if (<>o__38.<>p__57 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__38.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
            }
            if (<>o__38.<>p__56 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__38.<>p__56 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
            }
            if (<>o__38.<>p__55 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__38.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "paymentAddress", typeof(Footsites), argumentInfo));
            }
            this._billingAddressId = <>o__38.<>p__58.Target(<>o__38.<>p__58, <>o__38.<>p__57.Target(<>o__38.<>p__57, <>o__38.<>p__56.Target(<>o__38.<>p__56, <>o__38.<>p__55.Target(<>o__38.<>p__55, this._dynObj), "id")));
            flag = true;
            while (flag)
            {
                flag = false;
                try
                {
                    this._srr = this._client.Get("https://www.footaction.com/paygate/ccn").Text();
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
                    this._diData.Clear();
                    this._diData.Add("action", "authorize");
                    this._diData.Add("companyNumber", "1");
                    this._diData.Add("customerNumber", "1");
                    this._diData.Add("cardNumber", "4580917308873");
                    this._diData.Add("storageType", "");
                    this._srr = this._client.Post("https://www.footaction.com/paygate/ccn", this._diData).Text();
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
                try
                {
                    object obj3 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__38.<>p__59 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__59 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "expiryMonth", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__59.Target(<>o__38.<>p__59, obj3, profile.ExpiryMonth);
                    if (<>o__38.<>p__60 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__60 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "expiryYear", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__60.Target(<>o__38.<>p__60, obj3, profile.ExpiryYear);
                    if (<>o__38.<>p__61 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__61 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "flApiCCNumber", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__61.Target(<>o__38.<>p__61, obj3, profile.CCNumber);
                    if (<>o__38.<>p__62 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__62 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "billingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__62.Target(<>o__38.<>p__62, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__38.<>p__64 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__64 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "id", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__63 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__63 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__64.Target(<>o__38.<>p__64, <>o__38.<>p__63.Target(<>o__38.<>p__63, obj3), this._billingAddressId);
                    if (<>o__38.<>p__65 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__65 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cardType", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__65.Target(<>o__38.<>p__65, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__38.<>p__67 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__67 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "code", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__66 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "cardType", typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__67.Target(<>o__38.<>p__67, <>o__38.<>p__66.Target(<>o__38.<>p__66, obj3), "visa");
                    if (<>o__38.<>p__69 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__69 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__68 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__68 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footsites), argumentInfo));
                    }
                    object obj4 = <>o__38.<>p__69.Target(<>o__38.<>p__69, this._client, "https://www.footaction.com/api/users/carts/current/payment-detail?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds(), <>o__38.<>p__68.Target(<>o__38.<>p__68, typeof(Newtonsoft.Json.JsonConvert), obj3));
                    if (<>o__38.<>p__74 == null)
                    {
                        <>o__38.<>p__74 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                    }
                    if (<>o__38.<>p__73 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__73 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__72 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__71 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__71 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__38.<>p__70 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footsites), argumentInfo));
                    }
                    this._srr = <>o__38.<>p__74.Target(<>o__38.<>p__74, <>o__38.<>p__73.Target(<>o__38.<>p__73, <>o__38.<>p__72.Target(<>o__38.<>p__72, <>o__38.<>p__71.Target(<>o__38.<>p__71, <>o__38.<>p__70.Target(<>o__38.<>p__70, obj4)))));
                    if (<>o__38.<>p__75 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__75 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footsites), argumentInfo));
                    }
                    <>o__38.<>p__75.Target(<>o__38.<>p__75, typeof(EveAIO.Extensions), obj4);
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
            flag = true;
            while (flag)
            {
                flag = false;
                try
                {
                    this._dynObj = this._client.Get("https://www.footaction.com/api/users/carts/current?checkInventory=true&checkPriceVariation=true&timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds()).Json();
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
            while (true)
            {
                if (!flag)
                {
                    break;
                }
                flag = false;
                try
                {
                    this._srr = this._client.Get("https://www.footaction.com/paygate/ccn").Text();
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
                }
            }
            return true;
        }

        private bool SubmitOrder()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                int num2 = 0;
                bool flag = true;
            Label_0034:
                if (flag && (num2 < this._maxRepetitions))
                {
                    num2++;
                    flag = false;
                    try
                    {
                        string str = "";
                        string str2 = "";
                        switch (this._runner.Profile.CardTypeId)
                        {
                            case "0":
                                str = "amex";
                                str2 = "American Express";
                                break;

                            case "1":
                                str = "visa";
                                str2 = "Visa";
                                break;

                            case "2":
                                str = "mc";
                                str2 = "MasterCard";
                                break;
                        }
                        string[] textArray1 = new string[] { 
                            "{\"maxVisitedPane\":\"orderReviewPane\",\"updateBillingForBML\":false,\"billMyAddressBookIndex\":\"-1\",\"addressNeedsVerification\":false,\"billFirstName\":\"", this._runner.Profile.FirstName, "\",\"billLastName\":\"", this._runner.Profile.LastName, "\",\"billAddress1\":\"", this._runner.Profile.Address1, "\",\"billAddress2\":\"", this._runner.Profile.Address2, "\",\"billCity\":\"", this._runner.Profile.City, "\",\"billState\":\"", (this._runner.Profile.CountryId == "US") ? this._runner.Profile.StateId : "None", "\",\"billProvince\":\"", (this._runner.Profile.CountryId == "CA") ? this._runner.Profile.StateId : "None", "\",\"billPostalCode\":\"", this._runner.Profile.Zip,
                            "\",\"billHomePhone\":\"", this._runner.Profile.Phone, "\",\"billMobilePhone\":\"\",\"billCountry\":\"", this._runner.Profile.CountryId, "\",\"billEmailAddress\":\"", this._runner.Profile.Email, "\",\"billConfirmEmail\":\"", this._runner.Profile.Email, "\",\"billAddrIsPhysical\":true,\"billSubscribePhone\":false,\"billAbbreviatedAddress\":false,\"shipUpdateDefaultAddress\":false,\"VIPNumber\":\"\",\"accountBillAddress\":{\"billMyAddressBookIndex\":-1},\"selectedBillAddress\":{},\"billMyAddressBook\":[],\"shipMyAddressBookIndex\":-2,\"shipContactID\":\"\",\"shipFirstName\":\"", this._runner.Profile.FirstNameShipping, "\",\"shipLastName\":\"", this._runner.Profile.LastNameShipping, "\",\"shipAddress1\":\"", this._runner.Profile.Address1Shipping, "\",\"shipAddress2\":\"", this._runner.Profile.Address2Shipping,
                            "\",\"shipCity\":\"", this._runner.Profile.CityShipping, "\",\"shipState\":\"", (this._runner.Profile.CountryIdShipping == "US") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipProvince\":\"", (this._runner.Profile.CountryIdShipping == "CA") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipPostalCode\":\"", this._runner.Profile.ZipShipping, "\",\"shipHomePhone\":\"", this._runner.Profile.PhoneShipping, "\",\"shipMobilePhone\":\"\",\"shipCountry\":\"", this._runner.Profile.CountryIdShipping, "\",\"shipToStore\":false,\"shipHash\":\"", this.SHIPHASH, "\",\"shipMultiple\":false,\"isShipToStoreEligibleCheckout\":true,\"isMultipleAddressEligible\":false,\"shipAbbreviatedAddress\":false,\"selectedStore\":{ },\"accountShipAddress\":{ \"shipMyAddressBookIndex\":-1},\"selectedShipAddress\":{ },\"shipMyAddressBook\":[],\"shipMethodCode\":\"", this.SELECTEDMETHODCODE,
                            "\",\"shipMethodName\":\"", this.SELECTEDMETHODNAME, "\",\"shipMethodPrice\":\"$0.00\",\"shipDeliveryEstimate\":\"\",\"shipMethodCodeSDD\":\"\",\"shipMethodNameSDD\":\"\",\"shipMethodPriceSDD\":\"$0.00\",\"shipDeliveryEstimateSDD\":\"\",\"shipMethodCodeS2S\":\"\",\"shipMethodNameS2S\":\"\",\"shipMethodPriceS2S\":\"$0.00\",\"shipDeliveryEstimateS2S\":\"\",\"shipMethodCodeSFS\":\"\",\"shipMethodNameSFS\":\"\",\"shipMethodPriceSFS\":\"$0.00\",\"shipDeliveryEstimateSFS\":\"\",\"homeDeliveryPrice\":\"$0.00\",\"overallHomeDeliveryPrice\":\"$0.00\",\"aggregatedDeliveryPrice\":\"FREE\",\"aggregatedDeliveryLabel\":\"\",\"showGiftBoxOption\":true,\"addGiftBox\":false,\"giftBoxPrice\":\"$3.99\",\"useGiftReceipt\":false,\"showGiftOptions\":true,\"loyaltyMessageText\":false,\"showLoyaltyRenewalMessage\":false,\"pickupPersonFirstName\":\"\",\"pickupPersonLastName\":\"\",\"preferredLanguage\":\"\",\"advanceToConfirm\":false,\"payType\":\"CREDIT_CARD\",\"payPalToken\":\"\",\"payPalInContext\":true,\"payPalMerchantId\":\"\",\"payPalStage\":\"\",\"payPalPaymentAllowed\":true,\"payMethodPaneExpireMonth\":\"", this._runner.Profile.ExpiryMonth, "\",\"payMethodPaneExpireYear\":\"", this._runner.Profile.ExpiryYear.Substring(2), "\",\"payMethodPaneCardNumber\":\"", this._runner.Profile.CCNumber.Replace(" ", "").Trim(), "\",\"payMethodPaneCardType\":\"", str, "\",\"payMethodPaneLastFour\":\"", this._runner.Profile.CCNumber.Substring(this._runner.Profile.CCNumber.Length - 4).Trim(), "\",\"payMethodPanePayMethodName\":\"", str2, "\",\"payMethodPanePurchaseOrder\":\"\",\"payMethodPanePurchaseOrderNewCustomer\":\"\",\"payMethodPaneCVV\":\"", this._runner.Profile.Cvv,
                            "\",\"creditcardPaymentAllowed\":true,\"billMeLaterStage\":\"\",\"BMLPaymentAllowed\":true,\"displayBMLPromotion\":false,\"POPaymentAllowed\":false,\"promoType\":\"\",\"promoCode\":\"\",\"sourceCode\":\"INETSRC\",\"sourceCodeDescription\":\"\",\"sourceCodeCartDisplayText\":\"\",\"GIFTCARDCODE1\":\"\",\"GIFTCARDPIN1\":\"\",\"GIFTCARDUSED1\":\"\",\"GIFTCARDCODE2\":\"\",\"GIFTCARDPIN2\":\"\",\"GIFTCARDUSED2\":\"\",\"GIFTCARDCODE3\":\"\",\"GIFTCARDPIN3\":\"\",\"GIFTCARDUSED3\":\"\",\"GIFTCARDCODE4\":\"\",\"GIFTCARDPIN4\":\"\",\"GIFTCARDUSED4\":\"\",\"GIFTCARDCODE5\":\"\",\"GIFTCARDPIN5\":\"\",\"GIFTCARDUSED5\":\"\",\"rewardBarCode\":\"\",\"giftCardsEmpty\":true,\"sourceCodesEmpty\":true,\"emptyCart\":false,\"ContingencyQueue\":\"\",\"lgr\":\"", this.LGR, "\",\"displayEmailOptIn\":false,\"billSubscribeEmail\":false,\"billReceiveNewsletter\":false,\"billFavoriteTeams\":false,\"paypalEmailAddress\":\"\",\"displaySheerIdIframe\":true,\"displayCmsEntry\":\"\",\"payMethodPaneUserGotStoredCC\":false,\"payMethodPaneHasStoredCC\":false,\"payMethodPaneUsedStoredCC\":false,\"payMethodPaneSavedNewCC\":false,\"selectedCreditCard\":{},\"payMethodPaneHasCVV\":true,\"payMethodPaneCVVAF\":\"0\"}"
                        };
                        string str4 = string.Concat(textArray1);
                        this._diData.Clear();
                        this._diData.Add("verifiedCheckoutData", str4);
                        this._diData.Add("requestKey", this._requestKey);
                        this._diData.Add("hbg", this._hbg);
                        this._diData.Add("bb_device_id", this._deviceId);
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
                        this._srr = this._client.Post(this._mobileLink + "/checkout/eventGateway?method=validateReviewPane", this._diData).Text();
                        if (this._srr.Contains("SESSION_EXPIRED"))
                        {
                            Thread.Sleep(500);
                            this._srr = this._client.Get($"{this._websiteLink}/session/").Text();
                            this._srr = this._client.Get(this._websiteLink + "/checkout/?uri=checkout/").Text();
                            flag = true;
                            goto Label_0034;
                        }
                        try
                        {
                            EveAIO.Helpers.AddDbValue("Footsites|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                        }
                        catch
                        {
                        }
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr.Substring(2));
                        if (<>o__37.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__37.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__37.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__37.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        object obj2 = <>o__37.<>p__1.Target(<>o__37.<>p__1, <>o__37.<>p__0.Target(<>o__37.<>p__0, this._dynObj, "SESSION_EXPIRED"), null);
                        if (<>o__37.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                        }
                        if (!<>o__37.<>p__6.Target(<>o__37.<>p__6, obj2))
                        {
                            if (<>o__37.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__37.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__37.<>p__4 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__37.<>p__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__37.<>p__3 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__37.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                            }
                        }
                        if (!<>o__37.<>p__7.Target(<>o__37.<>p__7, (<>o__37.<>p__2 != null) ? obj2 : <>o__37.<>p__5.Target(<>o__37.<>p__5, obj2, <>o__37.<>p__4.Target(<>o__37.<>p__4, <>o__37.<>p__3.Target(<>o__37.<>p__3, <>o__37.<>p__2.Target(<>o__37.<>p__2, this._dynObj, "SESSION_EXPIRED")), true))))
                        {
                            if (<>o__37.<>p__9 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                <>o__37.<>p__9 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__37.<>p__8 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__37.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ERRORS", typeof(Footsites), argumentInfo));
                            }
                            obj2 = <>o__37.<>p__9.Target(<>o__37.<>p__9, <>o__37.<>p__8.Target(<>o__37.<>p__8, this._dynObj), null);
                            if (<>o__37.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__37.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                            }
                            if (!<>o__37.<>p__17.Target(<>o__37.<>p__17, obj2))
                            {
                                if (<>o__37.<>p__16 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__37.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__37.<>p__15 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__37.<>p__15 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__37.<>p__12 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__37.<>p__12 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__37.<>p__11 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__37.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__37.<>p__10 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__37.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ERRORS", typeof(Footsites), argumentInfo));
                                }
                                object obj3 = <>o__37.<>p__12.Target(<>o__37.<>p__12, <>o__37.<>p__11.Target(<>o__37.<>p__11, <>o__37.<>p__10.Target(<>o__37.<>p__10, this._dynObj)), 1);
                                if (<>o__37.<>p__14 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__37.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                                }
                                if (!<>o__37.<>p__14.Target(<>o__37.<>p__14, obj3))
                                {
                                }
                                if (!<>o__37.<>p__16.Target(<>o__37.<>p__16, <>o__37.<>p__15.Target(<>o__37.<>p__15, obj2, (<>o__37.<>p__13 != null) ? obj3 : <>o__37.<>p__13.Target(<>o__37.<>p__13, obj3, this._srr.Contains("\"ORDERSUBMITTED\":true")))))
                                {
                                    if (<>o__37.<>p__21 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__37.<>p__21 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__37.<>p__20 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__37.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__37.<>p__19 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__37.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__37.<>p__18 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__37.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ERRORS", typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__37.<>p__21.Target(<>o__37.<>p__21, <>o__37.<>p__20.Target(<>o__37.<>p__20, <>o__37.<>p__19.Target(<>o__37.<>p__19, <>o__37.<>p__18.Target(<>o__37.<>p__18, this._dynObj)), "unable to process your credit card")))
                                    {
                                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                                    }
                                    else
                                    {
                                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                                    }
                                    return false;
                                }
                            }
                            return true;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.SESSION_EXPIRED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.SESSION_EXPIRED);
                        return false;
                    }
                    catch (WebException exception1)
                    {
                        if (exception1.Message.Contains("403"))
                        {
                            this.GetSensorData();
                            this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                            States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                        }
                        else
                        {
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        flag = true;
                        goto Label_0034;
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
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
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
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        private bool SubmitOrderFootAction()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                int num2 = 0;
                object obj3 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__36.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__36.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cartId", typeof(Footsites), argumentInfo));
                }
                <>o__36.<>p__0.Target(<>o__36.<>p__0, obj3, this._cartId);
                if (<>o__36.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__36.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "securityCode", typeof(Footsites), argumentInfo));
                }
                <>o__36.<>p__1.Target(<>o__36.<>p__1, obj3, this._runner.Profile.Cvv);
                if (<>o__36.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__36.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "deviceId", typeof(Footsites), argumentInfo));
                }
                <>o__36.<>p__2.Target(<>o__36.<>p__2, obj3, "0400JapG4txqVP4Nf94lis1ztrjQCvk297SBnrp/XmcfWoWZcxqq3O2NEIo7BJIRIWDNtjiuvPP9Vk+xH1ZPRIwM6njw/ujAyYdbGKZt5JLThTvosS1xgSAgNfLEMokGoGJxgu1y04HLnOzU8XBpd1sDgWA73rYZtIsVyoA80pXCaXawrwf88vayC9C1UJgA1BQZQ4JudHOlf1KSnrbJSNyIsWB9R+WFa6fdxifyrThRovESnjwNVGXSgGQ8InPsuf6/kpMgG84gzO5PMQF00uJew9XqxzJ4y+q4kBq8NCD1TYtQjQ2NN27n5FC+59ZFSvq8ljAlWwXHcT+hgN6tJA4p1nA+k9XRlIy/LSQOVlpp6LrPMaM61Uewda1ugt8NgthyPMiH45XmO3J8Gkeky+MwZsWyp9+aWHAQxA9x3qRKbjOBF2jB4DXWGlEh3GX8eDv8Y51itE+9afEBREq5MBL5WCFTg9H7wizhfeStEaMUb1UyFgIzr08goEJI4/wX97IAaVmZqTD7tyd0W8+tYgTFPk5uMbchLUfL6jB+ZmMavO7St0oGtrebhhYMxcsNW12s/Cq0klbLOK/SrIFDhOsmDce8mnFiPj0ifd8Wb2EoeYX6uUg3H2+vCWXix/MfChYmosTzKoxVC6fSPFFgCaMpvU/h6EFXXmXE+w9Hmlm6TlEWpZVNIfofXuPp/Vp8gaYPTuOKDQED3XyyPr0t8ztVFjo8eOcfWZcDCXZjVj+Jpy+I6nUOK19VzsgH1DGeSyhahZfNQgu+lmSl2pKAmu1wluCISazs9PD83/seKcOlvXrGxhKauwqEWLxUN0sYF8r08f8uCcC0xODcb+CqMKE6vRMIPz669mi8FvrX9FpdDZVMyPpjzYuuV+VhT2JQxvTdC24eZzo97tw16kQoiglK7BJDLfM/X8TvaSyxlUFCiGEdsE8OdwolivehTqT3rw1rR9l0dEJHzndNg+95/NceoRDAstLN4US59PE4fRZTRwzCQai4/k8/Kw8+9yi8eph5uwLLNTE9qpy8yLSNDwHyTWg0DVh4KskWaAa7FpIh9vKei8IwzGqIQfm558wslBym5DVVI9Dx1mGsVwmN86toNQ3dzNyzQZ74A+RDDzayHhgFCE1XRfN+HZ21ET4rhaHYN4YbjEiJxk0sBAAQcH4loNwTidneZ5Wedvp3EOKiSx6OC0ceqoTbyG/m3Zqxt3w1Bn6LxoWOnZu64rr07LwRGXZpyc7oQIV63xadNE2jqb6o3IAEdbB+xCVNS65m83pQ+qMTRH6GRVfg7HAcS5fnSw==");
                if (<>o__36.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__4 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footsites), argumentInfo));
                }
                if (<>o__36.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footsites), argumentInfo));
                }
                object obj2 = <>o__36.<>p__4.Target(<>o__36.<>p__4, this._client, "https://www.footaction.com/api/users/orders?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds(), <>o__36.<>p__3.Target(<>o__36.<>p__3, typeof(Newtonsoft.Json.JsonConvert), obj3));
                if (<>o__36.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Footsites), argumentInfo));
                }
                if (<>o__36.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                }
                if (<>o__36.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footsites), argumentInfo));
                }
                if (<>o__36.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footsites), argumentInfo));
                }
                if (<>o__36.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footsites), argumentInfo));
                }
                this._dynObj = <>o__36.<>p__9.Target(<>o__36.<>p__9, typeof(Newtonsoft.Json.JsonConvert), <>o__36.<>p__8.Target(<>o__36.<>p__8, <>o__36.<>p__7.Target(<>o__36.<>p__7, <>o__36.<>p__6.Target(<>o__36.<>p__6, <>o__36.<>p__5.Target(<>o__36.<>p__5, obj2)))));
                if (<>o__36.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__10 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footsites), argumentInfo));
                }
                <>o__36.<>p__10.Target(<>o__36.<>p__10, typeof(EveAIO.Extensions), obj2);
                bool flag = true;
                while (flag)
                {
                    if (num2 >= this._maxRepetitions)
                    {
                        break;
                    }
                    num2++;
                    flag = false;
                    try
                    {
                        string str = "";
                        string str2 = "";
                        switch (this._runner.Profile.CardTypeId)
                        {
                            case "0":
                                str = "amex";
                                str2 = "American Express";
                                break;

                            case "1":
                                str = "visa";
                                str2 = "Visa";
                                break;

                            case "2":
                                str = "mc";
                                str2 = "MasterCard";
                                break;
                        }
                        string[] textArray1 = new string[] { 
                            "{\"maxVisitedPane\":\"orderReviewPane\",\"updateBillingForBML\":false,\"billMyAddressBookIndex\":\"-1\",\"addressNeedsVerification\":false,\"billFirstName\":\"", this._runner.Profile.FirstName, "\",\"billLastName\":\"", this._runner.Profile.LastName, "\",\"billAddress1\":\"", this._runner.Profile.Address1, "\",\"billAddress2\":\"", this._runner.Profile.Address2, "\",\"billCity\":\"", this._runner.Profile.City, "\",\"billState\":\"", (this._runner.Profile.CountryId == "US") ? this._runner.Profile.StateId : "None", "\",\"billProvince\":\"", (this._runner.Profile.CountryId == "CA") ? this._runner.Profile.StateId : "None", "\",\"billPostalCode\":\"", this._runner.Profile.Zip,
                            "\",\"billHomePhone\":\"", this._runner.Profile.Phone, "\",\"billMobilePhone\":\"\",\"billCountry\":\"", this._runner.Profile.CountryId, "\",\"billEmailAddress\":\"", this._runner.Profile.Email, "\",\"billConfirmEmail\":\"", this._runner.Profile.Email, "\",\"billAddrIsPhysical\":true,\"billSubscribePhone\":false,\"billAbbreviatedAddress\":false,\"shipUpdateDefaultAddress\":false,\"VIPNumber\":\"\",\"accountBillAddress\":{\"billMyAddressBookIndex\":-1},\"selectedBillAddress\":{},\"billMyAddressBook\":[],\"shipMyAddressBookIndex\":-2,\"shipContactID\":\"\",\"shipFirstName\":\"", this._runner.Profile.FirstNameShipping, "\",\"shipLastName\":\"", this._runner.Profile.LastNameShipping, "\",\"shipAddress1\":\"", this._runner.Profile.Address1Shipping, "\",\"shipAddress2\":\"", this._runner.Profile.Address2Shipping,
                            "\",\"shipCity\":\"", this._runner.Profile.CityShipping, "\",\"shipState\":\"", (this._runner.Profile.CountryIdShipping == "US") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipProvince\":\"", (this._runner.Profile.CountryIdShipping == "CA") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipPostalCode\":\"", this._runner.Profile.ZipShipping, "\",\"shipHomePhone\":\"", this._runner.Profile.PhoneShipping, "\",\"shipMobilePhone\":\"\",\"shipCountry\":\"", this._runner.Profile.CountryIdShipping, "\",\"shipToStore\":false,\"shipHash\":\"", this.SHIPHASH, "\",\"shipMultiple\":false,\"isShipToStoreEligibleCheckout\":true,\"isMultipleAddressEligible\":false,\"shipAbbreviatedAddress\":false,\"selectedStore\":{ },\"accountShipAddress\":{ \"shipMyAddressBookIndex\":-1},\"selectedShipAddress\":{ },\"shipMyAddressBook\":[],\"shipMethodCode\":\"", this.SELECTEDMETHODCODE,
                            "\",\"shipMethodName\":\"", this.SELECTEDMETHODNAME, "\",\"shipMethodPrice\":\"$0.00\",\"shipDeliveryEstimate\":\"\",\"shipMethodCodeSDD\":\"\",\"shipMethodNameSDD\":\"\",\"shipMethodPriceSDD\":\"$0.00\",\"shipDeliveryEstimateSDD\":\"\",\"shipMethodCodeS2S\":\"\",\"shipMethodNameS2S\":\"\",\"shipMethodPriceS2S\":\"$0.00\",\"shipDeliveryEstimateS2S\":\"\",\"shipMethodCodeSFS\":\"\",\"shipMethodNameSFS\":\"\",\"shipMethodPriceSFS\":\"$0.00\",\"shipDeliveryEstimateSFS\":\"\",\"homeDeliveryPrice\":\"$0.00\",\"overallHomeDeliveryPrice\":\"$0.00\",\"aggregatedDeliveryPrice\":\"FREE\",\"aggregatedDeliveryLabel\":\"\",\"showGiftBoxOption\":true,\"addGiftBox\":false,\"giftBoxPrice\":\"$3.99\",\"useGiftReceipt\":false,\"showGiftOptions\":true,\"loyaltyMessageText\":false,\"showLoyaltyRenewalMessage\":false,\"pickupPersonFirstName\":\"\",\"pickupPersonLastName\":\"\",\"preferredLanguage\":\"\",\"advanceToConfirm\":false,\"payType\":\"CREDIT_CARD\",\"payPalToken\":\"\",\"payPalInContext\":true,\"payPalMerchantId\":\"\",\"payPalStage\":\"\",\"payPalPaymentAllowed\":true,\"payMethodPaneExpireMonth\":\"", this._runner.Profile.ExpiryMonth, "\",\"payMethodPaneExpireYear\":\"", this._runner.Profile.ExpiryYear.Substring(2), "\",\"payMethodPaneCardNumber\":\"", this._runner.Profile.CCNumber.Replace(" ", "").Trim(), "\",\"payMethodPaneCardType\":\"", str, "\",\"payMethodPaneLastFour\":\"", this._runner.Profile.CCNumber.Substring(this._runner.Profile.CCNumber.Length - 4).Trim(), "\",\"payMethodPanePayMethodName\":\"", str2, "\",\"payMethodPanePurchaseOrder\":\"\",\"payMethodPanePurchaseOrderNewCustomer\":\"\",\"payMethodPaneCVV\":\"", this._runner.Profile.Cvv,
                            "\",\"creditcardPaymentAllowed\":true,\"billMeLaterStage\":\"\",\"BMLPaymentAllowed\":true,\"displayBMLPromotion\":false,\"POPaymentAllowed\":false,\"promoType\":\"\",\"promoCode\":\"\",\"sourceCode\":\"INETSRC\",\"sourceCodeDescription\":\"\",\"sourceCodeCartDisplayText\":\"\",\"GIFTCARDCODE1\":\"\",\"GIFTCARDPIN1\":\"\",\"GIFTCARDUSED1\":\"\",\"GIFTCARDCODE2\":\"\",\"GIFTCARDPIN2\":\"\",\"GIFTCARDUSED2\":\"\",\"GIFTCARDCODE3\":\"\",\"GIFTCARDPIN3\":\"\",\"GIFTCARDUSED3\":\"\",\"GIFTCARDCODE4\":\"\",\"GIFTCARDPIN4\":\"\",\"GIFTCARDUSED4\":\"\",\"GIFTCARDCODE5\":\"\",\"GIFTCARDPIN5\":\"\",\"GIFTCARDUSED5\":\"\",\"rewardBarCode\":\"\",\"giftCardsEmpty\":true,\"sourceCodesEmpty\":true,\"emptyCart\":false,\"ContingencyQueue\":\"\",\"lgr\":\"", this.LGR, "\",\"displayEmailOptIn\":false,\"billSubscribeEmail\":false,\"billReceiveNewsletter\":false,\"billFavoriteTeams\":false,\"paypalEmailAddress\":\"\",\"displaySheerIdIframe\":true,\"displayCmsEntry\":\"\",\"payMethodPaneUserGotStoredCC\":false,\"payMethodPaneHasStoredCC\":false,\"payMethodPaneUsedStoredCC\":false,\"payMethodPaneSavedNewCC\":false,\"selectedCreditCard\":{},\"payMethodPaneHasCVV\":true,\"payMethodPaneCVVAF\":\"0\"}"
                        };
                        string str4 = string.Concat(textArray1);
                        this._diData.Clear();
                        this._diData.Add("verifiedCheckoutData", str4);
                        this._diData.Add("requestKey", this._requestKey);
                        this._diData.Add("hbg", this._hbg);
                        this._diData.Add("bb_device_id", this._deviceId);
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
                        this._srr = this._client.Post(this._mobileLink + "/checkout/eventGateway?method=validateReviewPane", this._diData).Text();
                        if (this._srr.Contains("SESSION_EXPIRED"))
                        {
                            Thread.Sleep(500);
                            this._srr = this._client.Get($"{this._websiteLink}/session/").Text();
                            this._srr = this._client.Get(this._websiteLink + "/checkout/?uri=checkout/").Text();
                            flag = true;
                            continue;
                        }
                        try
                        {
                            EveAIO.Helpers.AddDbValue("Footsites|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                        }
                        catch
                        {
                        }
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr.Substring(2));
                        if (<>o__36.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__36.<>p__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__36.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__36.<>p__12 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__36.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__36.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        object obj4 = <>o__36.<>p__12.Target(<>o__36.<>p__12, <>o__36.<>p__11.Target(<>o__36.<>p__11, this._dynObj, "SESSION_EXPIRED"), null);
                        if (<>o__36.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__36.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                        }
                        if (!<>o__36.<>p__17.Target(<>o__36.<>p__17, obj4))
                        {
                            if (<>o__36.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__16 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__36.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__36.<>p__15 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__36.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                            }
                        }
                        if (<>o__36.<>p__18.Target(<>o__36.<>p__18, (<>o__36.<>p__13 != null) ? obj4 : <>o__36.<>p__16.Target(<>o__36.<>p__16, obj4, <>o__36.<>p__15.Target(<>o__36.<>p__15, <>o__36.<>p__14.Target(<>o__36.<>p__14, <>o__36.<>p__13.Target(<>o__36.<>p__13, this._dynObj, "SESSION_EXPIRED")), true))))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.SESSION_EXPIRED, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.SESSION_EXPIRED);
                            return false;
                        }
                        if (<>o__36.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__36.<>p__20 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__36.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__36.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ERRORS", typeof(Footsites), argumentInfo));
                        }
                        obj4 = <>o__36.<>p__20.Target(<>o__36.<>p__20, <>o__36.<>p__19.Target(<>o__36.<>p__19, this._dynObj), null);
                        if (<>o__36.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__36.<>p__28 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                        }
                        if (!<>o__36.<>p__28.Target(<>o__36.<>p__28, obj4))
                        {
                            if (<>o__36.<>p__27 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__27 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__36.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__26 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__36.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__36.<>p__23 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Footsites), argumentInfo));
                            }
                            if (<>o__36.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footsites), argumentInfo));
                            }
                            if (<>o__36.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ERRORS", typeof(Footsites), argumentInfo));
                            }
                            object obj5 = <>o__36.<>p__23.Target(<>o__36.<>p__23, <>o__36.<>p__22.Target(<>o__36.<>p__22, <>o__36.<>p__21.Target(<>o__36.<>p__21, this._dynObj)), 1);
                            if (<>o__36.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__25 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                            }
                            if (!<>o__36.<>p__25.Target(<>o__36.<>p__25, obj5))
                            {
                            }
                            if (!<>o__36.<>p__27.Target(<>o__36.<>p__27, <>o__36.<>p__26.Target(<>o__36.<>p__26, obj4, (<>o__36.<>p__24 != null) ? obj5 : <>o__36.<>p__24.Target(<>o__36.<>p__24, obj5, this._srr.Contains("\"ORDERSUBMITTED\":true")))))
                            {
                                if (<>o__36.<>p__32 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__32 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__36.<>p__31 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__36.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__36.<>p__30 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__36.<>p__29 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ERRORS", typeof(Footsites), argumentInfo));
                                }
                                if (!<>o__36.<>p__32.Target(<>o__36.<>p__32, <>o__36.<>p__31.Target(<>o__36.<>p__31, <>o__36.<>p__30.Target(<>o__36.<>p__30, <>o__36.<>p__29.Target(<>o__36.<>p__29, this._dynObj)), "unable to process your credit card")))
                                {
                                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                                    this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                                }
                                else
                                {
                                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                                }
                                return false;
                            }
                        }
                        return true;
                    }
                    catch (WebException exception1)
                    {
                        if (exception1.Message.Contains("403"))
                        {
                            this.GetSensorData();
                            this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                            States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                        }
                        else
                        {
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        flag = true;
                        continue;
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
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
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
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
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
                if (!this._isNewFootAction)
                {
                    object obj2;
                    int num2 = 0;
                    bool flag = true;
                    while (flag)
                    {
                        if (num2 >= this._maxRepetitions)
                        {
                            break;
                        }
                        num2++;
                        flag = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("verifiedCheckoutData", "{\"maxVisitedPane\":\"billAddressPane\",\"billMyAddressBookIndex\":\"-1\",\"addressNeedsVerification\":true,\"billFirstName\":\"\",\"billLastName\":\"\",\"billAddress1\":\"\",\"billAddress2\":\"\",\"billCity\":\"\",\"billState\":\"\",\"billProvince\":\"\",\"billPostalCode\":\"\",\"billHomePhone\":\"\",\"billMobilePhone\":\"\",\"billCountry\":\"US\",\"billEmailAddress\":\"\",\"billConfirmEmail\":\"\",\"billAddrIsPhysical\":true,\"billSubscribePhone\":false,\"billAbbreviatedAddress\":false,\"shipUpdateDefaultAddress\":false,\"VIPNumber\":\"\",\"accountBillAddress\":{\"billMyAddressBookIndex\":-1},\"selectedBillAddress\":{},\"billMyAddressBook\":[],\"updateBillingForBML\":false}");
                            this._diData.Add("requestKey", this._requestKey);
                            this._diData.Add("hbg", this._hbg);
                            this._diData.Add("addressBookEnabled", "true");
                            this._diData.Add("billAddressType", "new");
                            this._diData.Add("billAddressInputType", "single");
                            this._diData.Add("billCountry", this._runner.Profile.CountryId);
                            this._diData.Add("billMyAddressBookIndex", "-1");
                            this._diData.Add("billFirstName", this._runner.Profile.FirstName);
                            this._diData.Add("billLastName", this._runner.Profile.LastName);
                            this._diData.Add("billAddress1", this._runner.Profile.Address1);
                            this._diData.Add("billAddress2", this._runner.Profile.Address2);
                            this._diData.Add("billPostalCode", this._runner.Profile.Zip);
                            this._diData.Add("billCity", this._runner.Profile.City);
                            if ((this._runner.Profile.CountryId != "US") && (this._runner.Profile.CountryId != "CA"))
                            {
                                this._diData.Add("billState", "None");
                            }
                            else
                            {
                                this._diData.Add("billState", this._runner.Profile.StateId);
                            }
                            if ((this._runner.Profile.CountryId != "US") && (this._runner.Profile.CountryId != "CA"))
                            {
                                this._diData.Add("billProvince", "None");
                            }
                            else
                            {
                                this._diData.Add("billProvince", this._runner.Profile.StateId);
                            }
                            this._diData.Add("billHomePhone", this._runner.Profile.Phone);
                            this._diData.Add("billEmailAddress", this._runner.Profile.Email);
                            this._diData.Add("billConfirmEmail", this._runner.Profile.Email);
                            this._diData.Add("shipAddressType", "different");
                            this._diData.Add("shipAddressInputType", "single");
                            this._diData.Add("shipCountry", this._runner.Profile.CountryIdShipping);
                            this._diData.Add("shipMyAddressBookIndex", "-2");
                            this._diData.Add("shipToStore", "false");
                            this._diData.Add("shipFirstName", this._runner.Profile.FirstNameShipping);
                            this._diData.Add("shipLastName", this._runner.Profile.LastNameShipping);
                            this._diData.Add("shipAddress1", this._runner.Profile.Address1Shipping);
                            this._diData.Add("shipAddress2", this._runner.Profile.Address2Shipping);
                            this._diData.Add("shipPostalCode", this._runner.Profile.ZipShipping);
                            this._diData.Add("shipCity", this._runner.Profile.CityShipping);
                            if (this._runner.Profile.CountryId == "US")
                            {
                                this._diData.Add("shipState", this._runner.Profile.StateIdShipping);
                            }
                            else
                            {
                                this._diData.Add("shipState", "None");
                            }
                            if (this._runner.Profile.CountryId == "CA")
                            {
                                this._diData.Add("shipProvince", this._runner.Profile.StateIdShipping);
                            }
                            else
                            {
                                this._diData.Add("shipProvince", "None");
                            }
                            this._diData.Add("shipHomePhone", this._runner.Profile.PhoneShipping);
                            this._srr = this._client.Post(this._websiteLink + "/checkout/eventGateway?method=validateShipPane", this._diData).Text();
                            if (this._srr.Contains("SESSION_EXPIRED"))
                            {
                                Thread.Sleep(500);
                                this._srr = this._client.Get($"{this._websiteLink}/session/").Text();
                                this._srr = this._client.Get(this._websiteLink + "/checkout/?uri=checkout/").Text();
                                flag = true;
                            }
                            else
                            {
                                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr.Substring(2));
                                if (<>o__41.<>p__7 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__1 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                    <>o__41.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__0 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                obj2 = <>o__41.<>p__1.Target(<>o__41.<>p__1, <>o__41.<>p__0.Target(<>o__41.<>p__0, this._dynObj, "ERRORS"), null);
                                if (<>o__41.<>p__6 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                                }
                                if (!<>o__41.<>p__6.Target(<>o__41.<>p__6, obj2))
                                {
                                    if (<>o__41.<>p__5 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__4 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__41.<>p__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__3 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footsites), argumentInfo));
                                    }
                                }
                                if (<>o__41.<>p__7.Target(<>o__41.<>p__7, (<>o__41.<>p__2 != null) ? obj2 : <>o__41.<>p__5.Target(<>o__41.<>p__5, obj2, <>o__41.<>p__4.Target(<>o__41.<>p__4, <>o__41.<>p__3.Target(<>o__41.<>p__3, <>o__41.<>p__2.Target(<>o__41.<>p__2, this._dynObj, "ERRORS")), 0))))
                                {
                                    if (<>o__41.<>p__11 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__10 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__41.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__9 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__8 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__41.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                    }
                                    object obj3 = <>o__41.<>p__11.Target(<>o__41.<>p__11, <>o__41.<>p__10.Target(<>o__41.<>p__10, <>o__41.<>p__9.Target(<>o__41.<>p__9, <>o__41.<>p__8.Target(<>o__41.<>p__8, this._dynObj, "ERRORS")), "RESPONSEMESSAGE"));
                                    if (<>o__41.<>p__12 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__12 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                    }
                                    throw <>o__41.<>p__12.Target(<>o__41.<>p__12, typeof(Exception), obj3);
                                }
                                if (<>o__41.<>p__15 == null)
                                {
                                    <>o__41.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__14 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__13 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                this._requestKey = <>o__41.<>p__15.Target(<>o__41.<>p__15, <>o__41.<>p__14.Target(<>o__41.<>p__14, <>o__41.<>p__13.Target(<>o__41.<>p__13, this._dynObj, "REQUESTKEY")));
                                if (<>o__41.<>p__18 == null)
                                {
                                    <>o__41.<>p__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__17 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__16 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                this._hbg = <>o__41.<>p__18.Target(<>o__41.<>p__18, <>o__41.<>p__17.Target(<>o__41.<>p__17, <>o__41.<>p__16.Target(<>o__41.<>p__16, this._dynObj, "hbg")));
                                if (<>o__41.<>p__23 == null)
                                {
                                    <>o__41.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__21 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__20 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__19 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "SHIPMETHODPANE", typeof(Footsites), argumentInfo));
                                }
                                this.SELECTEDMETHODCODE = <>o__41.<>p__23.Target(<>o__41.<>p__23, <>o__41.<>p__22.Target(<>o__41.<>p__22, <>o__41.<>p__21.Target(<>o__41.<>p__21, <>o__41.<>p__20.Target(<>o__41.<>p__20, <>o__41.<>p__19.Target(<>o__41.<>p__19, this._dynObj), "SELECTEDMETHODCODE"))));
                                if (<>o__41.<>p__27 == null)
                                {
                                    <>o__41.<>p__27 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__26 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__25 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__24 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "SHIPPANE", typeof(Footsites), argumentInfo));
                                }
                                this.SHIPHASH = <>o__41.<>p__27.Target(<>o__41.<>p__27, <>o__41.<>p__26.Target(<>o__41.<>p__26, <>o__41.<>p__25.Target(<>o__41.<>p__25, <>o__41.<>p__24.Target(<>o__41.<>p__24, this._dynObj), "SHIPHASH")));
                                if (<>o__41.<>p__31 == null)
                                {
                                    <>o__41.<>p__31 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__30 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__29 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__28 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "SHIPMETHODPANE", typeof(Footsites), argumentInfo));
                                }
                                this.SELECTEDMETHODNAME = <>o__41.<>p__31.Target(<>o__41.<>p__31, <>o__41.<>p__30.Target(<>o__41.<>p__30, <>o__41.<>p__29.Target(<>o__41.<>p__29, <>o__41.<>p__28.Target(<>o__41.<>p__28, this._dynObj), "SELECTEDMETHODNAME")));
                            }
                            continue;
                        }
                        catch (WebException exception1)
                        {
                            if (exception1.Message.Contains("403"))
                            {
                                this.GetSensorData();
                                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                            }
                            else
                            {
                                flag = true;
                                States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                                Thread.Sleep(0x3e8);
                            }
                            flag = true;
                            continue;
                        }
                    }
                    flag = true;
                    num2 = 0;
                    while (flag)
                    {
                        if (num2 >= this._maxRepetitions)
                        {
                            break;
                        }
                        num2++;
                        flag = false;
                        try
                        {
                            string[] textArray1 = new string[] { 
                                "{\"maxVisitedPane\":\"shipMethodPane\",\"billMyAddressBookIndex\":\"-1\",\"addressNeedsVerification\":false,\"billFirstName\":\"", this._runner.Profile.FirstName, "\",\"billLastName\":\"", this._runner.Profile.LastName, "\",\"billAddress1\":\"", this._runner.Profile.Address1, "\",\"billAddress2\":\"", this._runner.Profile.Address2, "\",\"billCity\":\"", this._runner.Profile.City, "\",\"billState\":\"", (this._runner.Profile.CountryId == "US") ? this._runner.Profile.StateId : "None", "\",\"billProvince\":\"", (this._runner.Profile.CountryId == "CA") ? this._runner.Profile.StateId : "None", "\",\"billPostalCode\":\"", this._runner.Profile.Zip,
                                "\",\"billHomePhone\":\"", this._runner.Profile.Phone, "\",\"billMobilePhone\":\"\",\"billCountry\":\"", this._runner.Profile.CountryId, "\",\"billEmailAddress\":\"", this._runner.Profile.Email, "\",\"billConfirmEmail\":\"", this._runner.Profile.Email, "\",\"billAddrIsPhysical\":true,\"billSubscribePhone\":false,\"billAbbreviatedAddress\":false,\"shipUpdateDefaultAddress\":false,\"VIPNumber\":\"\",\"accountBillAddress\":{\"billMyAddressBookIndex\":-1},\"selectedBillAddress\":{},\"billMyAddressBook\":[],\"updateBillingForBML\":false,\"shipMyAddressBookIndex\":-1,\"useBillingAsShipping\":false,\"shipFirstName\":\"", this._runner.Profile.FirstNameShipping, "\",\"shipLastName\":\"", this._runner.Profile.LastNameShipping, "\",\"shipAddress1\":\"", this._runner.Profile.Address1Shipping, "\",\"shipAddress2\":\"", this._runner.Profile.Address2Shipping,
                                "\",\"shipCity\":\"", this._runner.Profile.CityShipping, "\",\"shipState\":\"", (this._runner.Profile.CountryIdShipping == "US") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipProvince\":\"", (this._runner.Profile.CountryIdShipping == "CA") ? this._runner.Profile.StateIdShipping : "None", "\",\"shipPostalCode\":\"", this._runner.Profile.ZipShipping, "\",\"shipHomePhone\":\"", this._runner.Profile.PhoneShipping, "\",\"shipMobilePhone\":\"\",\"shipCountry\":\"", this._runner.Profile.CountryIdShipping, "\",\"shipHash\":\"", this.SHIPHASH, "\",\"shipMultiple\":false,\"isShipToStoreEligibleCheckout\":false,\"shipToStore\":false,\"isMultipleAddressEligible\":false,\"shipAbbreviatedAddress\":false,\"selectedStore\":{},\"accountShipAddress\":{\"shipMyAddressBookIndex\":-1},\"selectedShipAddress\":{},\"shipMyAddressBook\":[],\"shipMethodCode\":\"", this.SELECTEDMETHODCODE,
                                "\",\"shipMethodName\":\"", this.SELECTEDMETHODNAME, "\",\"shipMethodPrice\":\"$42.00\",\"shipDeliveryEstimate\":\"\",\"shipMethodCodeSDD\":\"\",\"shipMethodNameSDD\":\"\",\"shipMethodPriceSDD\":\"$0.00\",\"shipDeliveryEstimateSDD\":\"\",\"shipMethodCodeS2S\":\"\",\"shipMethodNameS2S\":\"\",\"shipMethodPriceS2S\":\"$0.00\",\"shipDeliveryEstimateS2S\":\"\",\"shipMethodCodeSFS\":\"\",\"shipMethodNameSFS\":\"\",\"shipMethodPriceSFS\":\"$0.00\",\"shipDeliveryEstimateSFS\":\"\",\"homeDeliveryPrice\":\"$42.00\",\"overallHomeDeliveryPrice\":\"$42.00\",\"aggregatedDeliveryPrice\":\"$42.00\",\"aggregatedDeliveryLabel\":\"\",\"showGiftBoxOption\":false,\"addGiftBox\":false,\"giftBoxPrice\":\"$3.99\",\"useGiftReceipt\":false,\"showGiftOptions\":false,\"loyaltyMessageText\":false,\"showLoyaltyRenewalMessage\":false,\"pickupPersonFirstName\":\"\",\"pickupPersonLastName\":\"\"}"
                            };
                            string str = string.Concat(textArray1);
                            this._diData.Clear();
                            this._diData.Add("verifiedCheckoutData", str);
                            this._diData.Add("requestKey", this._requestKey);
                            this._diData.Add("hbg", this._hbg);
                            this._diData.Add("addressBookEnabled", "true");
                            this._diData.Add("billAddressType", "new");
                            this._diData.Add("billAddressInputType", "single");
                            this._diData.Add("billCountry", this._runner.Profile.CountryId);
                            this._diData.Add("billMyAddressBookIndex", "-1");
                            this._diData.Add("billFirstName", this._runner.Profile.FirstName);
                            this._diData.Add("billLastName", this._runner.Profile.LastName);
                            this._diData.Add("billAddress1", this._runner.Profile.Address1);
                            this._diData.Add("billAddress2", this._runner.Profile.Address2);
                            this._diData.Add("billPostalCode", this._runner.Profile.Zip);
                            this._diData.Add("billCity", this._runner.Profile.City);
                            if (this._runner.Profile.CountryId == "US")
                            {
                                this._diData.Add("billState", this._runner.Profile.StateId);
                            }
                            else
                            {
                                this._diData.Add("billState", "None");
                            }
                            if (this._runner.Profile.CountryId == "CA")
                            {
                                this._diData.Add("billProvince", this._runner.Profile.StateId);
                            }
                            else
                            {
                                this._diData.Add("billProvince", "None");
                            }
                            this._diData.Add("billHomePhone", this._runner.Profile.Phone);
                            this._diData.Add("billEmailAddress", this._runner.Profile.Email);
                            this._diData.Add("billConfirmEmail", this._runner.Profile.Email);
                            this._diData.Add("shipAddressType", "different");
                            this._diData.Add("shipAddressInputType", "single");
                            this._diData.Add("shipCountry", this._runner.Profile.CountryIdShipping);
                            this._diData.Add("shipMyAddressBookIndex", "-2");
                            this._diData.Add("shipToStore", "false");
                            this._diData.Add("shipFirstName", this._runner.Profile.FirstNameShipping);
                            this._diData.Add("shipLastName", this._runner.Profile.LastNameShipping);
                            this._diData.Add("shipAddress1", this._runner.Profile.Address1Shipping);
                            this._diData.Add("shipAddress2", this._runner.Profile.Address2Shipping);
                            this._diData.Add("shipPostalCode", this._runner.Profile.ZipShipping);
                            this._diData.Add("shipCity", this._runner.Profile.CityShipping);
                            if (this._runner.Profile.CountryIdShipping == "US")
                            {
                                this._diData.Add("shipState", this._runner.Profile.StateIdShipping);
                            }
                            else
                            {
                                this._diData.Add("shipState", "None");
                            }
                            if (this._runner.Profile.CountryIdShipping == "US")
                            {
                                this._diData.Add("shipProvince", this._runner.Profile.StateIdShipping);
                            }
                            else
                            {
                                this._diData.Add("shipProvince", "None");
                            }
                            this._diData.Add("shipHomePhone", this._runner.Profile.PhoneShipping);
                            this._diData.Add("shipMethodCode", this.SELECTEDMETHODCODE);
                            this._srr = this._client.Post(this._websiteLink + "/checkout/eventGateway?method=validateShipMethodPane", this._diData).Text();
                            if (!this._srr.Contains("SESSION_EXPIRED"))
                            {
                                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr.Substring(2));
                                if (<>o__41.<>p__39 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__39 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__33 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                    <>o__41.<>p__33 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__32 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__32 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                obj2 = <>o__41.<>p__33.Target(<>o__41.<>p__33, <>o__41.<>p__32.Target(<>o__41.<>p__32, this._dynObj, "ERRORS"), null);
                                if (<>o__41.<>p__38 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__38 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                                }
                                if (!<>o__41.<>p__38.Target(<>o__41.<>p__38, obj2))
                                {
                                    if (<>o__41.<>p__37 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__37 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__36 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__41.<>p__36 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__35 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footsites), argumentInfo));
                                    }
                                }
                                if (<>o__41.<>p__39.Target(<>o__41.<>p__39, (<>o__41.<>p__34 != null) ? obj2 : <>o__41.<>p__37.Target(<>o__41.<>p__37, obj2, <>o__41.<>p__36.Target(<>o__41.<>p__36, <>o__41.<>p__35.Target(<>o__41.<>p__35, <>o__41.<>p__34.Target(<>o__41.<>p__34, this._dynObj, "ERRORS")), 0))))
                                {
                                    if (<>o__41.<>p__43 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__42 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__41.<>p__42 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__41 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                                    }
                                    if (<>o__41.<>p__40 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__41.<>p__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                    }
                                    object obj4 = <>o__41.<>p__43.Target(<>o__41.<>p__43, <>o__41.<>p__42.Target(<>o__41.<>p__42, <>o__41.<>p__41.Target(<>o__41.<>p__41, <>o__41.<>p__40.Target(<>o__41.<>p__40, this._dynObj, "ERRORS")), "RESPONSEMESSAGE"));
                                    if (<>o__41.<>p__44 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__41.<>p__44 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                    }
                                    throw <>o__41.<>p__44.Target(<>o__41.<>p__44, typeof(Exception), obj4);
                                }
                                if (<>o__41.<>p__47 == null)
                                {
                                    <>o__41.<>p__47 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__46 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__45 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__45 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                this._requestKey = <>o__41.<>p__47.Target(<>o__41.<>p__47, <>o__41.<>p__46.Target(<>o__41.<>p__46, <>o__41.<>p__45.Target(<>o__41.<>p__45, this._dynObj, "REQUESTKEY")));
                                if (<>o__41.<>p__50 == null)
                                {
                                    <>o__41.<>p__50 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__49 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__48 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                this._hbg = <>o__41.<>p__50.Target(<>o__41.<>p__50, <>o__41.<>p__49.Target(<>o__41.<>p__49, <>o__41.<>p__48.Target(<>o__41.<>p__48, this._dynObj, "hbg")));
                                if (<>o__41.<>p__54 == null)
                                {
                                    <>o__41.<>p__54 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Footsites)));
                                }
                                if (<>o__41.<>p__53 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__53 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__52 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__52 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                                }
                                if (<>o__41.<>p__51 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "PAYMENTMETHODPANE", typeof(Footsites), argumentInfo));
                                }
                                this.LGR = <>o__41.<>p__54.Target(<>o__41.<>p__54, <>o__41.<>p__53.Target(<>o__41.<>p__53, <>o__41.<>p__52.Target(<>o__41.<>p__52, <>o__41.<>p__51.Target(<>o__41.<>p__51, this._dynObj), "LGR")));
                            }
                            else
                            {
                                Thread.Sleep(500);
                                this._srr = this._client.Get($"{this._websiteLink}/session/").Text();
                                this._srr = this._client.Get(this._websiteLink + "/checkout/?uri=checkout/").Text();
                                flag = true;
                            }
                            continue;
                        }
                        catch (WebException exception2)
                        {
                            if (exception2.Message.Contains("403"))
                            {
                                this.GetSensorData();
                                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                            }
                            else
                            {
                                flag = true;
                                States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                                Thread.Sleep(0x3e8);
                            }
                            flag = true;
                            continue;
                        }
                    }
                    return true;
                }
                return this.SubmitShippingFootAction();
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
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
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
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        private bool SubmitShippingFootAction()
        {
            ProfileObject profile = this._runner.Profile;
            bool flag = true;
            while (flag)
            {
                flag = false;
                try
                {
                    this._diData.Clear();
                    this._srr = this._client.Put($"https://www.footaction.com/api/users/carts/current/email/{profile.Email}?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds(), this._diData).Text();
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
        Label_00C6:
            if (flag)
            {
                flag = false;
                try
                {
                    object obj3 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__40.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__0 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__0.Target(<>o__40.<>p__0, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__40.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__2 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__2.Target(<>o__40.<>p__2, <>o__40.<>p__1.Target(<>o__40.<>p__1, obj3), new Newtonsoft.Json.Linq.JObject());
                    if (<>o__40.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__5.Target(<>o__40.<>p__5, <>o__40.<>p__4.Target(<>o__40.<>p__4, <>o__40.<>p__3.Target(<>o__40.<>p__3, obj3)), "US");
                    if (<>o__40.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "name", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "country", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__8.Target(<>o__40.<>p__8, <>o__40.<>p__7.Target(<>o__40.<>p__7, <>o__40.<>p__6.Target(<>o__40.<>p__6, obj3)), "United States");
                    if (<>o__40.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "type", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__10.Target(<>o__40.<>p__10, <>o__40.<>p__9.Target(<>o__40.<>p__9, obj3), "Home/Business Address");
                    if (<>o__40.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__12 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "setAsBilling", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__12.Target(<>o__40.<>p__12, <>o__40.<>p__11.Target(<>o__40.<>p__11, obj3), false);
                    if (<>o__40.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__14.Target(<>o__40.<>p__14, <>o__40.<>p__13.Target(<>o__40.<>p__13, obj3), profile.FirstNameShipping);
                    if (<>o__40.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__16.Target(<>o__40.<>p__16, <>o__40.<>p__15.Target(<>o__40.<>p__15, obj3), profile.LastNameShipping);
                    if (<>o__40.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line1", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__18.Target(<>o__40.<>p__18, <>o__40.<>p__17.Target(<>o__40.<>p__17, obj3), profile.Address1Shipping);
                    if (<>o__40.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line2", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__20.Target(<>o__40.<>p__20, <>o__40.<>p__19.Target(<>o__40.<>p__19, obj3), profile.Address2Shipping);
                    if (<>o__40.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postalCode", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__22.Target(<>o__40.<>p__22, <>o__40.<>p__21.Target(<>o__40.<>p__21, obj3), profile.ZipShipping);
                    if (<>o__40.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__24.Target(<>o__40.<>p__24, <>o__40.<>p__23.Target(<>o__40.<>p__23, obj3), profile.PhoneShipping);
                    if (<>o__40.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "town", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__26.Target(<>o__40.<>p__26, <>o__40.<>p__25.Target(<>o__40.<>p__25, obj3), profile.CityShipping);
                    if (<>o__40.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__40.<>p__28 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionFPO", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__28.Target(<>o__40.<>p__28, <>o__40.<>p__27.Target(<>o__40.<>p__27, obj3), null);
                    if (<>o__40.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__30 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isFPO", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__30.Target(<>o__40.<>p__30, <>o__40.<>p__29.Target(<>o__40.<>p__29, obj3), false);
                    if (<>o__40.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__32 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__32.Target(<>o__40.<>p__32, <>o__40.<>p__31.Target(<>o__40.<>p__31, obj3), true);
                    if (<>o__40.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "recordType", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__34.Target(<>o__40.<>p__34, <>o__40.<>p__33.Target(<>o__40.<>p__33, obj3), " ");
                    if (<>o__40.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__36 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "email", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__36.Target(<>o__40.<>p__36, <>o__40.<>p__35.Target(<>o__40.<>p__35, obj3), false);
                    if (<>o__40.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__38 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__38.Target(<>o__40.<>p__38, <>o__40.<>p__37.Target(<>o__40.<>p__37, obj3), new Newtonsoft.Json.Linq.JObject());
                    if (<>o__40.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__41 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "countryIso", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__41.Target(<>o__40.<>p__41, <>o__40.<>p__40.Target(<>o__40.<>p__40, <>o__40.<>p__39.Target(<>o__40.<>p__39, obj3)), "US");
                    if (<>o__40.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__44 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocode", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__44.Target(<>o__40.<>p__44, <>o__40.<>p__43.Target(<>o__40.<>p__43, <>o__40.<>p__42.Target(<>o__40.<>p__42, obj3)), "US-AL");
                    if (<>o__40.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__47 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isocodeShort", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__47.Target(<>o__40.<>p__47, <>o__40.<>p__46.Target(<>o__40.<>p__46, <>o__40.<>p__45.Target(<>o__40.<>p__45, obj3)), "AL");
                    if (<>o__40.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__50 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "name", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shippingAddress", typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__50.Target(<>o__40.<>p__50, <>o__40.<>p__49.Target(<>o__40.<>p__49, <>o__40.<>p__48.Target(<>o__40.<>p__48, obj3)), "Alabama");
                    if (<>o__40.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__52 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__51 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Footsites), argumentInfo));
                    }
                    object obj4 = <>o__40.<>p__52.Target(<>o__40.<>p__52, this._client, "https://www.footaction.com/api/users/carts/current/addresses/shipping?timestamp=" + DateTimeOffset.Now.ToUnixTimeSeconds(), <>o__40.<>p__51.Target(<>o__40.<>p__51, typeof(Newtonsoft.Json.JsonConvert), obj3));
                    if (<>o__40.<>p__53 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__53 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Footsites), argumentInfo));
                    }
                    <>o__40.<>p__53.Target(<>o__40.<>p__53, typeof(EveAIO.Extensions), obj4);
                    if (<>o__40.<>p__58 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__58 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__57 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__56 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__55 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__54 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Footsites), argumentInfo));
                    }
                    this._dynObj = <>o__40.<>p__58.Target(<>o__40.<>p__58, typeof(Newtonsoft.Json.JsonConvert), <>o__40.<>p__57.Target(<>o__40.<>p__57, <>o__40.<>p__56.Target(<>o__40.<>p__56, <>o__40.<>p__55.Target(<>o__40.<>p__55, <>o__40.<>p__54.Target(<>o__40.<>p__54, obj4)))));
                    if (<>o__40.<>p__66 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__66 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__60 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__40.<>p__60 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Footsites), argumentInfo));
                    }
                    if (<>o__40.<>p__59 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__59 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                    }
                    object obj5 = <>o__40.<>p__60.Target(<>o__40.<>p__60, <>o__40.<>p__59.Target(<>o__40.<>p__59, this._dynObj, "errors"), null);
                    if (<>o__40.<>p__65 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__65 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Footsites), argumentInfo));
                    }
                    if (!<>o__40.<>p__65.Target(<>o__40.<>p__65, obj5))
                    {
                        if (<>o__40.<>p__64 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__64 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__63 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__40.<>p__63 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__62 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Footsites), argumentInfo));
                        }
                    }
                    if (<>o__40.<>p__66.Target(<>o__40.<>p__66, (<>o__40.<>p__61 != null) ? obj5 : <>o__40.<>p__64.Target(<>o__40.<>p__64, obj5, <>o__40.<>p__63.Target(<>o__40.<>p__63, <>o__40.<>p__62.Target(<>o__40.<>p__62, <>o__40.<>p__61.Target(<>o__40.<>p__61, this._dynObj, "errors")), 0))))
                    {
                        if (<>o__40.<>p__77 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__77 = CallSite<Func<CallSite, Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__76 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__76 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__71 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__40.<>p__71 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__70 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__69 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__40.<>p__69 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__68 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__67 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__40.<>p__67 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__75 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__74 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__40.<>p__74 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__73 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__73 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Footsites), argumentInfo));
                        }
                        if (<>o__40.<>p__72 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__40.<>p__72 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Footsites), argumentInfo));
                        }
                        throw <>o__40.<>p__77.Target(<>o__40.<>p__77, typeof(Exception), <>o__40.<>p__76.Target(<>o__40.<>p__76, <>o__40.<>p__71.Target(<>o__40.<>p__71, <>o__40.<>p__70.Target(<>o__40.<>p__70, <>o__40.<>p__69.Target(<>o__40.<>p__69, <>o__40.<>p__68.Target(<>o__40.<>p__68, <>o__40.<>p__67.Target(<>o__40.<>p__67, this._dynObj, "errors")), "subject")), " "), <>o__40.<>p__75.Target(<>o__40.<>p__75, <>o__40.<>p__74.Target(<>o__40.<>p__74, <>o__40.<>p__73.Target(<>o__40.<>p__73, <>o__40.<>p__72.Target(<>o__40.<>p__72, this._dynObj, "errors")), "message"))));
                    }
                    goto Label_00C6;
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
                    goto Label_00C6;
                }
            }
            return true;
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Footsites.<>c <>9;
            public static Func<HtmlNode, bool> <>9__33_0;
            public static Func<HtmlNode, bool> <>9__34_1;
            public static Func<HtmlNode, bool> <>9__34_2;
            public static Func<HtmlNode, bool> <>9__34_3;
            public static Func<HtmlNode, bool> <>9__34_4;
            public static Func<HtmlNode, bool> <>9__34_5;
            public static Func<HtmlNode, bool> <>9__34_6;
            public static Func<HtmlNode, bool> <>9__43_0;
            public static Func<HtmlNode, bool> <>9__43_1;
            public static Func<HtmlNode, bool> <>9__43_2;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Footsites.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <CheckInventory>b__43_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "spcoForm"));

            internal bool <CheckInventory>b__43_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "requestKey"));

            internal bool <CheckInventory>b__43_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "hbg"));

            internal bool <DirectLinkFootAction>b__33_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "c-image product"));

            internal bool <DirectLinkInternal>b__34_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLinkInternal>b__34_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product_title"));

            internal bool <DirectLinkInternal>b__34_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLinkInternal>b__34_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_price"));

            internal bool <DirectLinkInternal>b__34_5(HtmlNode x) => 
                ((x.Attributes["data-info"] != null) && (x.Attributes["data-info"].Value == "product_sku"));

            internal bool <DirectLinkInternal>b__34_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "requestKey"));
        }

        [CompilerGenerated]
        private static class <>o__32
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
        }

        [CompilerGenerated]
        private static class <>o__33
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string>> <>p__8;
            public static CallSite<Func<CallSite, object, string>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, bool>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__24;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__25;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__26;
        }

        [CompilerGenerated]
        private static class <>o__34
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, DateTime>> <>p__12;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, int, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, int, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__25;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__26;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__27;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__28;
        }

        [CompilerGenerated]
        private static class <>o__36
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
            public static CallSite<Func<CallSite, object, bool, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, bool>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, int, object>> <>p__23;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__24;
            public static CallSite<Func<CallSite, object, bool>> <>p__25;
            public static CallSite<Func<CallSite, object, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, bool>> <>p__27;
            public static CallSite<Func<CallSite, object, bool>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, bool>> <>p__32;
        }

        [CompilerGenerated]
        private static class <>o__37
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, int, object>> <>p__12;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, bool>> <>p__21;
        }

        [CompilerGenerated]
        private static class <>o__38
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
            public static CallSite<Func<CallSite, object, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__14;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__17;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__18;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__28;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__29;
            public static CallSite<Action<CallSite, Type, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, string, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, int, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, bool>> <>p__42;
            public static CallSite<Func<CallSite, object, bool>> <>p__43;
            public static CallSite<Func<CallSite, object, string, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, string, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, string, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, string, object>> <>p__51;
            public static CallSite<Func<CallSite, object, object>> <>p__52;
            public static CallSite<Func<CallSite, object, object, object>> <>p__53;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, string, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, string>> <>p__58;
            public static CallSite<Func<CallSite, object, string, object>> <>p__59;
            public static CallSite<Func<CallSite, object, string, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string, object>> <>p__61;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__62;
            public static CallSite<Func<CallSite, object, object>> <>p__63;
            public static CallSite<Func<CallSite, object, string, object>> <>p__64;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__65;
            public static CallSite<Func<CallSite, object, object>> <>p__66;
            public static CallSite<Func<CallSite, object, string, object>> <>p__67;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__68;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__69;
            public static CallSite<Func<CallSite, object, object>> <>p__70;
            public static CallSite<Func<CallSite, object, object>> <>p__71;
            public static CallSite<Func<CallSite, object, object>> <>p__72;
            public static CallSite<Func<CallSite, object, object>> <>p__73;
            public static CallSite<Func<CallSite, object, string>> <>p__74;
            public static CallSite<Action<CallSite, Type, object>> <>p__75;
        }

        [CompilerGenerated]
        private static class <>o__39
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, int, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string>> <>p__18;
        }

        [CompilerGenerated]
        private static class <>o__40
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
            public static CallSite<Func<CallSite, object, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, string, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, string, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, string, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string, object>> <>p__50;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__51;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__52;
            public static CallSite<Action<CallSite, Type, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__58;
            public static CallSite<Func<CallSite, object, string, object>> <>p__59;
            public static CallSite<Func<CallSite, object, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, int, object>> <>p__63;
            public static CallSite<Func<CallSite, object, object, object>> <>p__64;
            public static CallSite<Func<CallSite, object, bool>> <>p__65;
            public static CallSite<Func<CallSite, object, bool>> <>p__66;
            public static CallSite<Func<CallSite, object, string, object>> <>p__67;
            public static CallSite<Func<CallSite, object, object>> <>p__68;
            public static CallSite<Func<CallSite, object, string, object>> <>p__69;
            public static CallSite<Func<CallSite, object, object>> <>p__70;
            public static CallSite<Func<CallSite, object, string, object>> <>p__71;
            public static CallSite<Func<CallSite, object, string, object>> <>p__72;
            public static CallSite<Func<CallSite, object, object>> <>p__73;
            public static CallSite<Func<CallSite, object, string, object>> <>p__74;
            public static CallSite<Func<CallSite, object, object>> <>p__75;
            public static CallSite<Func<CallSite, object, object, object>> <>p__76;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__77;
        }

        [CompilerGenerated]
        private static class <>o__41
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, int, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string>> <>p__31;
            public static CallSite<Func<CallSite, object, string, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, int, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, bool>> <>p__38;
            public static CallSite<Func<CallSite, object, bool>> <>p__39;
            public static CallSite<Func<CallSite, object, string, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, string, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, Type, object, Exception>> <>p__44;
            public static CallSite<Func<CallSite, object, string, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, string>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, string, object>> <>p__52;
            public static CallSite<Func<CallSite, object, object>> <>p__53;
            public static CallSite<Func<CallSite, object, string>> <>p__54;
        }

        [CompilerGenerated]
        private static class <>o__44
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, int, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Action<CallSite, HttpRequestHeaders, string, object>> <>p__5;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__6;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Action<CallSite, Type, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string>> <>p__16;
        }

        [CompilerGenerated]
        private static class <>o__45
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
        }
    }
}

