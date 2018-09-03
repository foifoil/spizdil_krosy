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
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Mesh : IPlatform
    {
        private Client _client;
        private Dictionary<string, string> _diData;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        [Dynamic]
        private object _dynObj;
        private string _data;
        private string _apiKey;
        private object _store;
        private object _cartId;
        private string _cartLink;
        private string _customerLink;
        private string _billingLink;
        private string _shippingLink;
        private string _deliveryMethodId;
        private string _paymentSession;
        private string _paymentLink;
        private object _homeLink;
        private string _localeShipping;
        private string _localeBilling;
        private HtmlNode _regions;

        public Mesh(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._diData = new Dictionary<string, string>();
            this._currentDoc = new HtmlDocument();
            this._runner = runner;
            this._task = task;
            switch (this._task.VariousStringData)
            {
                case "size":
                    this._apiKey = "3565AE9C56464BB0AD8020F735D1479E";
                    this._store = "size";
                    this._homeLink = "https://www.size.co.uk";
                    break;

                case "hipstore":
                    this._apiKey = "117860D26D504A5FB26B2FB64CE35FB8";
                    this._store = "thehipstore";
                    this._homeLink = "https://www.thehipstore.co.uk";
                    break;

                case "jd":
                    this._apiKey = "60743806B14F4AF389F582E83A141733";
                    this._store = "jdsports";
                    this._homeLink = "https://www.jdsports.co.uk";
                    break;
            }
            this.SetClient();
        }

        public bool Atc()
        {
            if (this._task.AtcMethod == TaskObject.AtcMethodEnum.backend)
            {
                return this.AtcBackend();
            }
            return this.AtcFrondend();
        }

        internal bool AtcBackend()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                this._request = (HttpWebRequest) WebRequest.Create($"https://commerce.mesh.mx/stores/{this._store}/carts");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("x-api-key", this._apiKey);
                this._request.Method = "POST";
                this._request.ContentType = "application/json; charset=utf-8";
                this._request.ServicePoint.Expect100Continue = true;
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                this._data = "{ \"contents\": [{ \"SKU\": \"" + this._runner.PickedSize.Value.Value + "\", \"quantity\": 1, \"type\": \"cartproduct\" }]}";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse) this._request.GetResponse())
                {
                    this._srr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__39.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__39.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Mesh), argumentInfo));
                }
                if (<>o__39.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__39.<>p__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Mesh), argumentInfo));
                }
                if (<>o__39.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__39.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__39.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__39.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                if (<>o__39.<>p__3.Target(<>o__39.<>p__3, <>o__39.<>p__2.Target(<>o__39.<>p__2, <>o__39.<>p__1.Target(<>o__39.<>p__1, <>o__39.<>p__0.Target(<>o__39.<>p__0, this._dynObj, "count")), 1)))
                {
                    if (<>o__39.<>p__7 == null)
                    {
                        <>o__39.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                    }
                    if (<>o__39.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__39.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__39.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__39.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                    }
                    if (<>o__39.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__39.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                    }
                    this._cartId = <>o__39.<>p__7.Target(<>o__39.<>p__7, <>o__39.<>p__6.Target(<>o__39.<>p__6, <>o__39.<>p__5.Target(<>o__39.<>p__5, <>o__39.<>p__4.Target(<>o__39.<>p__4, this._dynObj, "ID"))));
                    if (<>o__39.<>p__12 == null)
                    {
                        <>o__39.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                    }
                    if (<>o__39.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__39.<>p__11 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__39.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__39.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__39.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__39.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                    }
                    if (<>o__39.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__39.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                    }
                    this._cartLink = <>o__39.<>p__12.Target(<>o__39.<>p__12, <>o__39.<>p__11.Target(<>o__39.<>p__11, <>o__39.<>p__10.Target(<>o__39.<>p__10, <>o__39.<>p__9.Target(<>o__39.<>p__9, <>o__39.<>p__8.Target(<>o__39.<>p__8, this._dynObj, "id"))), "https://prod.jdgroupmesh.cloud", "https://commerce.mesh.mx"));
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
                this._runner.IsError = true;
                return false;
            }
        }

        internal bool AtcFrondend()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__40.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "customisations", typeof(Mesh), argumentInfo));
                }
                <>o__40.<>p__0.Target(<>o__40.<>p__0, obj2, false);
                if (<>o__40.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__40.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cartPosition", typeof(Mesh), argumentInfo));
                }
                <>o__40.<>p__1.Target(<>o__40.<>p__1, obj2, null);
                if (<>o__40.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantityToAdd", typeof(Mesh), argumentInfo));
                }
                <>o__40.<>p__2.Target(<>o__40.<>p__2, obj2, 1);
                if (<>o__40.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__4 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Mesh), argumentInfo));
                }
                object obj3 = <>o__40.<>p__4.Target(<>o__40.<>p__4, this._client, $"{this._homeLink}/cart/{this._runner.PickedSize.Value.Value}", <>o__40.<>p__3.Target(<>o__40.<>p__3, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__40.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__5 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Mesh), argumentInfo));
                }
                <>o__40.<>p__5.Target(<>o__40.<>p__5, typeof(EveAIO.Extensions), obj3);
                if (<>o__40.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__10 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Mesh), argumentInfo));
                }
                this._dynObj = <>o__40.<>p__10.Target(<>o__40.<>p__10, typeof(Newtonsoft.Json.JsonConvert), <>o__40.<>p__9.Target(<>o__40.<>p__9, <>o__40.<>p__8.Target(<>o__40.<>p__8, <>o__40.<>p__7.Target(<>o__40.<>p__7, <>o__40.<>p__6.Target(<>o__40.<>p__6, obj3)))));
                if (<>o__40.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                if (!<>o__40.<>p__14.Target(<>o__40.<>p__14, <>o__40.<>p__13.Target(<>o__40.<>p__13, <>o__40.<>p__12.Target(<>o__40.<>p__12, <>o__40.<>p__11.Target(<>o__40.<>p__11, this._dynObj, "count")), 1)))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
                }
                if (<>o__40.<>p__18 == null)
                {
                    <>o__40.<>p__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__40.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                this._cartId = <>o__40.<>p__18.Target(<>o__40.<>p__18, <>o__40.<>p__17.Target(<>o__40.<>p__17, <>o__40.<>p__16.Target(<>o__40.<>p__16, <>o__40.<>p__15.Target(<>o__40.<>p__15, this._dynObj, "ID"))));
                if (<>o__40.<>p__23 == null)
                {
                    <>o__40.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__40.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__22 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__40.<>p__19 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                this._cartLink = <>o__40.<>p__23.Target(<>o__40.<>p__23, <>o__40.<>p__22.Target(<>o__40.<>p__22, <>o__40.<>p__21.Target(<>o__40.<>p__21, <>o__40.<>p__20.Target(<>o__40.<>p__20, <>o__40.<>p__19.Target(<>o__40.<>p__19, this._dynObj, "id"))), "https://prod.jdgroupmesh.cloud", "https://commerce.mesh.mx"));
                States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
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
                this._runner.IsError = true;
                return false;
            }
        }

        public bool Checkout()
        {
            if (this._task.AtcMethod == TaskObject.AtcMethodEnum.backend)
            {
                return ((this.SubmitShippingBackend() && this.SubmitBillingBackend()) && this.SubmitOrderBackend());
            }
            if (!this.SubmitShippingFrondend())
            {
                return false;
            }
            if (!this.SubmitBillingFrontend())
            {
                return false;
            }
            return this.SubmitOrderFrontend();
        }

        public bool DirectLink(string link)
        {
            if (this._task.AtcMethod == TaskObject.AtcMethodEnum.backend)
            {
                return this.DirectLinkBackend(link);
            }
            return this.DirectLinkFrondend(link);
        }

        internal bool DirectLinkBackend(string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                string str = this._task.Link.Substring(this._task.Link.IndexOf("/") + 1).Trim();
                this._request = (HttpWebRequest) WebRequest.Create($"https://commerce.mesh.mx/stores/{this._store}/products/{str}");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("Upgrade-Insecure-Requests", "1");
                this._request.Headers.Add("x-api-key", this._apiKey);
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                using (HttpWebResponse response = (HttpWebResponse) this._request.GetResponse())
                {
                    this._srr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__28.<>p__2 == null)
                {
                    <>o__28.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__28.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__28.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__28.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__28.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                this._task.ImgUrl = <>o__28.<>p__2.Target(<>o__28.<>p__2, <>o__28.<>p__1.Target(<>o__28.<>p__1, <>o__28.<>p__0.Target(<>o__28.<>p__0, this._dynObj, "mainImage")));
                Product product = new Product();
                if (<>o__28.<>p__5 == null)
                {
                    <>o__28.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__28.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__28.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__28.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__28.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                product.ProductTitle = <>o__28.<>p__5.Target(<>o__28.<>p__5, <>o__28.<>p__4.Target(<>o__28.<>p__4, <>o__28.<>p__3.Target(<>o__28.<>p__3, this._dynObj, "name")));
                if (<>o__28.<>p__8 == null)
                {
                    <>o__28.<>p__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__28.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__28.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__28.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__28.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                product.Link = <>o__28.<>p__8.Target(<>o__28.<>p__8, <>o__28.<>p__7.Target(<>o__28.<>p__7, <>o__28.<>p__6.Target(<>o__28.<>p__6, this._dynObj, "URL")));
                if (<>o__28.<>p__12 == null)
                {
                    <>o__28.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__28.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__28.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__28.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__28.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                if (<>o__28.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__28.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "price", typeof(Mesh), argumentInfo));
                }
                product.Price = <>o__28.<>p__12.Target(<>o__28.<>p__12, <>o__28.<>p__11.Target(<>o__28.<>p__11, <>o__28.<>p__10.Target(<>o__28.<>p__10, <>o__28.<>p__9.Target(<>o__28.<>p__9, this._dynObj), "amount")));
                this._runner.Product = product;
                if (<>o__28.<>p__26 == null)
                {
                    <>o__28.<>p__26 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Mesh)));
                }
                if (<>o__28.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__28.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "options", typeof(Mesh), argumentInfo));
                }
                foreach (object obj2 in <>o__28.<>p__26.Target(<>o__28.<>p__26, <>o__28.<>p__13.Target(<>o__28.<>p__13, this._dynObj)))
                {
                    if (<>o__28.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__28.<>p__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__28.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__28.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__28.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__28.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                    }
                    if (<>o__28.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__28.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__28.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__28.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Mesh), argumentInfo));
                    }
                    if (!<>o__28.<>p__18.Target(<>o__28.<>p__18, <>o__28.<>p__17.Target(<>o__28.<>p__17, <>o__28.<>p__16.Target(<>o__28.<>p__16, <>o__28.<>p__15.Target(<>o__28.<>p__15, <>o__28.<>p__14.Target(<>o__28.<>p__14, obj2), "stockStatus")), "IN STOCK")))
                    {
                        if (<>o__28.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__28.<>p__25 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                        }
                        if (<>o__28.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__28.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                        }
                        if (<>o__28.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__28.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                        }
                        if (<>o__28.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__28.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Mesh), argumentInfo));
                        }
                        if (<>o__28.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__28.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                        }
                        if (<>o__28.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__28.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                        }
                        if (<>o__28.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__28.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Mesh), argumentInfo));
                        }
                        this._runner.Product.AvailableSizes.Add(<>o__28.<>p__25.Target(<>o__28.<>p__25, typeof(KeyValuePair<string, string>), <>o__28.<>p__21.Target(<>o__28.<>p__21, <>o__28.<>p__20.Target(<>o__28.<>p__20, <>o__28.<>p__19.Target(<>o__28.<>p__19, obj2), "size")), <>o__28.<>p__24.Target(<>o__28.<>p__24, <>o__28.<>p__23.Target(<>o__28.<>p__23, <>o__28.<>p__22.Target(<>o__28.<>p__22, obj2), "SKU"))));
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
                    string str2 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str2 = str2 + ch.ToString();
                        }
                    }
                    double num2 = double.Parse(str2.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num2 < this._task.MinimumPrice) || (num2 > this._task.MaximumPrice))
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
                            KeyValuePair<string, string> current;
                            while (enumerator2.MoveNext())
                            {
                                current = enumerator2.Current;
                                char[] chArray2 = new char[] { ' ' };
                                string[] source = current.Key.Split(chArray2);
                                for (int j = 0; j < source.Length; j++)
                                {
                                    source[j] = source[j].Trim().ToUpperInvariant();
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_0D37;
                                }
                            }
                            continue;
                        Label_0D37:
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
            catch (Exception exception)
            {
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                this._runner.IsError = true;
                return false;
            }
        }

        internal bool DirectLinkFrondend(string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                this._task.Link.Substring(this._task.Link.IndexOf("/") + 1).Trim();
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._srr = this._client.Get(link).Text();
                    if (this._srr.Contains("currently in a queue"))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_IN_QUEUE, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                        Thread.Sleep(0x9c4);
                        flag = true;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"))).Attributes["content"].Value;
                Product product1 = new Product {
                    ProductTitle = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim()),
                    Link = link,
                    Price = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["data-e2e"] != null) && (x.Attributes["data-e2e"].Value == "product-price"))).InnerText.Trim())
                };
                this._runner.Product = product1;
                if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "options")))
                {
                    foreach (HtmlNode node in this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "options"))).Descendants("button"))
                    {
                        if (node.Attributes["title"].Value.ToLowerInvariant() != "Out Of Stock".ToLowerInvariant())
                        {
                            this._runner.Product.AvailableSizes.Add(new KeyValuePair<string, string>(node.InnerText.Trim(), node.Attributes["data-sku"].Value));
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
                        string str = "";
                        foreach (char ch in this._runner.Product.Price)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str = str + ch.ToString();
                            }
                        }
                        double num2 = double.Parse(str.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num2 < this._task.MinimumPrice) || (num2 > this._task.MaximumPrice))
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
                                goto Label_05B2;
                            Label_053C:
                                pair = enumerator2.Current;
                                char[] chArray2 = new char[] { ' ' };
                                string[] source = pair.Key.Split(chArray2);
                                for (int j = 0; j < source.Length; j++)
                                {
                                    source[j] = source[j].Trim().ToUpperInvariant();
                                }
                                if (source.Any<string>(x => x == sz))
                                {
                                    goto Label_05C0;
                                }
                            Label_05B2:
                                if (!enumerator2.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_053C;
                            Label_05C0:
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
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                this._runner.IsError = true;
                return false;
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
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, false);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        }

        private bool SubmitBillingBackend()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create(this._cartLink + "/hostedPayment");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("x-api-key", this._apiKey);
                this._request.Method = "POST";
                this._request.ContentType = "application/json; charset=utf-8";
                this._request.ServicePoint.Expect100Continue = true;
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                this._data = "{";
                this._data = this._data + "\"type\": \"CARD\",";
                this._data = this._data + "\"terminals\": {";
                this._data = this._data + "\"successURL\": \"http://payment_ok\",";
                this._data = this._data + "\"failureURL\": \"http://payment_failed\",";
                this._data = this._data + "\"timeoutURL\": \"http://payment_timeout\"";
                this._data = this._data + "}";
                this._data = this._data + "}";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse) this._request.GetResponse())
                    {
                        this._srr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    }
                }
                catch (WebException exception)
                {
                    if (!exception.Message.Contains("409"))
                    {
                        throw;
                    }
                    string str2 = "";
                    if (exception.Response != null)
                    {
                        using (Stream stream2 = exception.Response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream2))
                            {
                                str2 = reader.ReadToEnd();
                            }
                        }
                        if (!str2.Contains("products is unavailable"))
                        {
                            throw;
                        }
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__34.<>p__3 == null)
                {
                    <>o__34.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__34.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__34.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__34.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__34.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                if (<>o__34.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__34.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "terminalEndPoints", typeof(Mesh), argumentInfo));
                }
                string str = <>o__34.<>p__3.Target(<>o__34.<>p__3, <>o__34.<>p__2.Target(<>o__34.<>p__2, <>o__34.<>p__1.Target(<>o__34.<>p__1, <>o__34.<>p__0.Target(<>o__34.<>p__0, this._dynObj), "cardEntryURL")));
                this._paymentSession = str.Substring(str.IndexOf("=") + 1);
                if (<>o__34.<>p__8 == null)
                {
                    <>o__34.<>p__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__34.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__34.<>p__7 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__34.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__34.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__34.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__34.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__34.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__34.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                this._paymentLink = <>o__34.<>p__8.Target(<>o__34.<>p__8, <>o__34.<>p__7.Target(<>o__34.<>p__7, <>o__34.<>p__6.Target(<>o__34.<>p__6, <>o__34.<>p__5.Target(<>o__34.<>p__5, <>o__34.<>p__4.Target(<>o__34.<>p__4, this._dynObj, "href"))), "https://prod.jdgroupmesh.cloud", "https://commerce.mesh.mx"));
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

        private bool SubmitBillingFrontend()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__37.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__0.Target(<>o__37.<>p__0, obj2, this._localeBilling);
                if (<>o__37.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "locale", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__1.Target(<>o__37.<>p__1, obj2, "");
                if (<>o__37.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__2.Target(<>o__37.<>p__2, obj2, this._runner.Profile.FirstName);
                if (<>o__37.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__3.Target(<>o__37.<>p__3, obj2, this._runner.Profile.LastName);
                if (<>o__37.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__4.Target(<>o__37.<>p__4, obj2, this._runner.Profile.Phone);
                if (<>o__37.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address1", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__5.Target(<>o__37.<>p__5, obj2, this._runner.Profile.Address1);
                if (<>o__37.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address2", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__6.Target(<>o__37.<>p__6, obj2, this._runner.Profile.Address2);
                if (<>o__37.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "town", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__7.Target(<>o__37.<>p__7, obj2, this._runner.Profile.City);
                if (<>o__37.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "county", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__8.Target(<>o__37.<>p__8, obj2, this._runner.Profile.State);
                if (<>o__37.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postcode", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__9.Target(<>o__37.<>p__9, obj2, this._runner.Profile.Zip);
                if (<>o__37.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "addressPredict", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__10.Target(<>o__37.<>p__10, obj2, "");
                if (<>o__37.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "setOnCart", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__11.Target(<>o__37.<>p__11, obj2, "billingAddressID");
                if (<>o__37.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__13 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__12 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Mesh), argumentInfo));
                }
                object obj3 = <>o__37.<>p__13.Target(<>o__37.<>p__13, this._client, $"{this._homeLink}/myaccount/addressbook/add/", <>o__37.<>p__12.Target(<>o__37.<>p__12, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__37.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__14 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__14.Target(<>o__37.<>p__14, typeof(EveAIO.Extensions), obj3);
                if (<>o__37.<>p__19 == null)
                {
                    <>o__37.<>p__19 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__37.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Mesh), argumentInfo));
                }
                this._srr = <>o__37.<>p__19.Target(<>o__37.<>p__19, <>o__37.<>p__18.Target(<>o__37.<>p__18, <>o__37.<>p__17.Target(<>o__37.<>p__17, <>o__37.<>p__16.Target(<>o__37.<>p__16, <>o__37.<>p__15.Target(<>o__37.<>p__15, obj3)))));
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__37.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                object obj4 = <>o__37.<>p__21.Target(<>o__37.<>p__21, <>o__37.<>p__20.Target(<>o__37.<>p__20, this._dynObj, "ID"));
                obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__37.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__22 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "editAddressID", typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__22.Target(<>o__37.<>p__22, obj2, obj4);
                if (<>o__37.<>p__24 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__24 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__23 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__23 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Mesh), argumentInfo));
                }
                obj3 = <>o__37.<>p__24.Target(<>o__37.<>p__24, this._client, $"{this._homeLink}/checkout/updateBillingAddress/ajax/", <>o__37.<>p__23.Target(<>o__37.<>p__23, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__37.<>p__25 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__25 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Mesh), argumentInfo));
                }
                <>o__37.<>p__25.Target(<>o__37.<>p__25, typeof(EveAIO.Extensions), obj3);
                if (<>o__37.<>p__30 == null)
                {
                    <>o__37.<>p__30 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__37.<>p__29 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__28 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__37.<>p__26 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Mesh), argumentInfo));
                }
                this._srr = <>o__37.<>p__30.Target(<>o__37.<>p__30, <>o__37.<>p__29.Target(<>o__37.<>p__29, <>o__37.<>p__28.Target(<>o__37.<>p__28, <>o__37.<>p__27.Target(<>o__37.<>p__27, <>o__37.<>p__26.Target(<>o__37.<>p__26, obj3)))));
                this._diData.Clear();
                this._diData.Add("paySelect", "card");
                this._srr = this._client.Post($"{this._homeLink}/checkout/paymentV3/", this._diData).Text();
                this._currentDoc.LoadHtml(this._srr);
                this._paymentLink = this._currentDoc.DocumentNode.Descendants("iframe").First<HtmlNode>().Attributes["src"].Value;
                this._paymentSession = this._paymentLink.Substring(this._paymentLink.IndexOf("=") + 1);
                this._srr = this._client.Get(this._paymentLink).Text();
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

        private bool SubmitOrderBackend()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create("https://hps.datacash.com/hps/?");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("x-api-key", this._apiKey);
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded";
                this._request.ServicePoint.Expect100Continue = true;
                this._request.AllowAutoRedirect = false;
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                this._data = "card_number=" + this._runner.Profile.CCNumber;
                this._data = this._data + "&exp_month=" + this._runner.Profile.ExpiryMonth;
                this._data = this._data + "&exp_year=" + this._runner.Profile.ExpiryYear;
                this._data = this._data + "&cv2_number=" + this._runner.Profile.Cvv;
                this._data = this._data + "&HPS_SessionID=" + this._paymentSession;
                this._data = this._data + "&action=confirm";
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
                string str = "";
                using (HttpWebResponse response = (HttpWebResponse) this._request.GetResponse())
                {
                    if (response.Headers["Location"] != null)
                    {
                        str = response.Headers["Location"];
                    }
                    this._srr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                if (string.IsNullOrEmpty(str))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    if (!this._currentDoc.DocumentNode.Descendants("p").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pan"))))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                        return false;
                    }
                    HtmlNode node = this._currentDoc.DocumentNode.Descendants("p").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pan"));
                    if (!string.IsNullOrEmpty(node.InnerText.Trim()))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.MSG, null, "", node.InnerText.Trim());
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        return false;
                    }
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_ORDER, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                this._request = (HttpWebRequest) WebRequest.Create(this._paymentLink + "/hostedpaymentresult");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("x-api-key", this._apiKey);
                this._request.Method = "POST";
                this._request.ContentType = "application/json; charset=utf-8";
                this._request.ServicePoint.Expect100Continue = true;
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                this._data = "{";
                this._data = this._data + "\"HostedPaymentPageResult\": \"" + str + "\"";
                this._data = this._data + "}";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream2 = this._request.GetRequestStream())
                {
                    stream2.Write(this._bytes, 0, this._bytes.Length);
                }
                using (HttpWebResponse response2 = (HttpWebResponse) this._request.GetResponse())
                {
                    this._srr = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__33.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Mesh), argumentInfo));
                }
                if (<>o__33.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Mesh), argumentInfo));
                }
                if (<>o__33.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__33.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                if (<>o__33.<>p__3.Target(<>o__33.<>p__3, <>o__33.<>p__2.Target(<>o__33.<>p__2, <>o__33.<>p__1.Target(<>o__33.<>p__1, <>o__33.<>p__0.Target(<>o__33.<>p__0, this._dynObj, "status")), "DECLINED")))
                {
                    if (<>o__33.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.NamedArgument, "msg") };
                        <>o__33.<>p__7 = CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteLogger", null, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__33.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                    }
                    if (<>o__33.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__33.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "response", typeof(Mesh), argumentInfo));
                    }
                    <>o__33.<>p__7.Target(<>o__33.<>p__7, typeof(States), this._task, States.LOGGER_STATES.MSG, <>o__33.<>p__6.Target(<>o__33.<>p__6, <>o__33.<>p__5.Target(<>o__33.<>p__5, <>o__33.<>p__4.Target(<>o__33.<>p__4, this._dynObj), "message")));
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
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

        private bool SubmitOrderFrontend()
        {
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
                string str = "";
                this._diData.Clear();
                this._diData.Add("card_number", this._runner.Profile.CCNumber);
                this._diData.Add("exp_month", this._runner.Profile.ExpiryMonth);
                this._diData.Add("exp_year", this._runner.Profile.ExpiryYear);
                this._diData.Add("cv2_number", this._runner.Profile.Cvv);
                this._diData.Add("HPS_SessionID", this._paymentSession);
                this._diData.Add("action", "confirm");
                HttpResponseMessage response = this._client.Post("https://hps.datacash.com/hps/?", this._diData);
                KeyValuePair<string, string> pair = this._client.Get(response.Headers.GetValues("Location").First<string>()).TextResponseUri();
                this._srr = pair.Key;
                str = pair.Value;
                if (!this._srr.Contains("Please enter a valid card number") && !str.Contains("declined"))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_ORDER, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                    if (!this._srr.Contains("byvisa") && !this._srr.Contains("securecode"))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                        return false;
                    }
                    string innerText = this._currentDoc.DocumentNode.Descendants("textarea").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).InnerText;
                    string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"))).Attributes["value"].Value;
                    string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "acs3DAuthForm"))).Attributes["action"].Value;
                    this._diData.Clear();
                    this._diData.Add("PaReq", innerText);
                    this._diData.Add("MD", str3);
                    this._diData.Add("TermUrl", str8);
                    this._srr = this._client.Post(url, this._diData).Text();
                    this._currentDoc.LoadHtml(this._srr);
                    string str4 = "";
                    if (!this._srr.Contains("byvisa"))
                    {
                        string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                        str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                        string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"))).Attributes["value"].Value;
                        url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"))).Attributes["action"].Value;
                        this._diData.Clear();
                        this._diData.Add("PaRes", innerText);
                        this._diData.Add("MD", str3);
                        this._diData.Add("PaReq", str2);
                        this._diData.Add("ABSlog", str7);
                        this._diData.Add("deviceDNA", "");
                        this._diData.Add("executionTime", "");
                        this._diData.Add("dnaError", "");
                        this._diData.Add("mesc", "");
                        this._diData.Add("mescIterationCount", "0");
                        this._diData.Add("desc", "");
                        this._diData.Add("isDNADone", "false");
                        this._diData.Add("arcotFlashCookie", "");
                        response = this._client.Post(url, this._diData);
                        EveAIO.Extensions.CheckError(response);
                        str4 = response.Headers.Any<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "Location")) ? response.Headers.GetValues("Location").First<string>() : "declined";
                    }
                    else
                    {
                        innerText = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                        str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                        url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                        this._diData.Clear();
                        this._diData.Add("PaRes", innerText);
                        this._diData.Add("MD", str3);
                        this._diData.Add("TermUrl", str8);
                        response = this._client.Post(url, this._diData);
                        EveAIO.Extensions.CheckError(response);
                        str4 = response.Headers.Any<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "Location")) ? response.Headers.GetValues("Location").First<string>() : "declined";
                    }
                    if (!str4.ToLowerInvariant().Contains("declined") && !str4.ToLowerInvariant().Contains("failure"))
                    {
                        try
                        {
                            EveAIO.Helpers.AddDbValue("Mesh|" + str4 + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                        }
                        catch
                        {
                        }
                        return true;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
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

        private bool SubmitShippingBackend()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                this._request = (HttpWebRequest) WebRequest.Create($"https://commerce.mesh.mx/stores/{this._store}/customers");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("x-api-key", this._apiKey);
                this._request.Method = "POST";
                this._request.ContentType = "application/json; charset=utf-8";
                this._request.ServicePoint.Expect100Continue = true;
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                MeshCustomer customer = new MeshCustomer {
                    firstName = this._runner.Profile.FirstName,
                    lastName = this._runner.Profile.LastName,
                    email = this._runner.Profile.Email,
                    phone = this._runner.Profile.Phone,
                    enrolledForEmailMarketing = false,
                    isGuest = true
                };
                MeshAddress item = new MeshAddress {
                    firstName = this._runner.Profile.FirstNameShipping,
                    lastName = this._runner.Profile.LastNameShipping,
                    address1 = this._runner.Profile.Address1Shipping,
                    address2 = this._runner.Profile.Address2Shipping,
                    country = this._runner.Profile.CountryShipping,
                    county = this._runner.Profile.StateShipping,
                    isPrimaryAddress = true,
                    isPrimaryBillingAddress = false,
                    locale = this._runner.Profile.CountryIdShipping,
                    postcode = this._runner.Profile.ZipShipping,
                    town = this._runner.Profile.CityShipping
                };
                customer.addresses.Add(item);
                MeshAddress address2 = new MeshAddress {
                    firstName = this._runner.Profile.FirstName,
                    lastName = this._runner.Profile.LastName,
                    address1 = this._runner.Profile.Address1,
                    address2 = this._runner.Profile.Address2,
                    country = this._runner.Profile.Country,
                    county = this._runner.Profile.State,
                    isPrimaryAddress = false,
                    isPrimaryBillingAddress = true,
                    locale = this._runner.Profile.CountryId,
                    postcode = this._runner.Profile.Zip,
                    town = this._runner.Profile.City
                };
                customer.addresses.Add(address2);
                this._data = Newtonsoft.Json.JsonConvert.SerializeObject(customer);
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                this._srr = "";
                using (HttpWebResponse response = (HttpWebResponse) this._request.GetResponse())
                {
                    this._srr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__35.<>p__4 == null)
                {
                    <>o__35.<>p__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__35.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__35.<>p__3 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__35.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__35.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__35.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__35.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__35.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__35.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                this._customerLink = <>o__35.<>p__4.Target(<>o__35.<>p__4, <>o__35.<>p__3.Target(<>o__35.<>p__3, <>o__35.<>p__2.Target(<>o__35.<>p__2, <>o__35.<>p__1.Target(<>o__35.<>p__1, <>o__35.<>p__0.Target(<>o__35.<>p__0, this._dynObj, "id"))), "https://prod.jdgroupmesh.cloud", "https://commerce.mesh.mx"));
                if (<>o__35.<>p__16 == null)
                {
                    <>o__35.<>p__16 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Mesh)));
                }
                if (<>o__35.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__35.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addresses", typeof(Mesh), argumentInfo));
                }
                foreach (object obj2 in <>o__35.<>p__16.Target(<>o__35.<>p__16, <>o__35.<>p__5.Target(<>o__35.<>p__5, this._dynObj)))
                {
                    if (<>o__35.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__35.<>p__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__35.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__35.<>p__8 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Mesh), argumentInfo));
                    }
                    if (<>o__35.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__35.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                    }
                    if (<>o__35.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__35.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                    }
                    if (!<>o__35.<>p__9.Target(<>o__35.<>p__9, <>o__35.<>p__8.Target(<>o__35.<>p__8, <>o__35.<>p__7.Target(<>o__35.<>p__7, <>o__35.<>p__6.Target(<>o__35.<>p__6, obj2, "isPrimaryBillingAddress")), true)))
                    {
                        if (<>o__35.<>p__15 == null)
                        {
                            <>o__35.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                        }
                        if (<>o__35.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__35.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                        }
                        if (<>o__35.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__35.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                        }
                        this._shippingLink = <>o__35.<>p__15.Target(<>o__35.<>p__15, <>o__35.<>p__14.Target(<>o__35.<>p__14, <>o__35.<>p__13.Target(<>o__35.<>p__13, obj2, "id")));
                    }
                    else
                    {
                        if (<>o__35.<>p__12 == null)
                        {
                            <>o__35.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                        }
                        if (<>o__35.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__35.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                        }
                        if (<>o__35.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__35.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                        }
                        this._billingLink = <>o__35.<>p__12.Target(<>o__35.<>p__12, <>o__35.<>p__11.Target(<>o__35.<>p__11, <>o__35.<>p__10.Target(<>o__35.<>p__10, obj2, "id")));
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create(this._cartLink);
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("x-api-key", this._apiKey);
                this._request.Method = "PUT";
                this._request.ContentType = "application/json; charset=utf-8";
                this._request.ServicePoint.Expect100Continue = true;
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                this._data = "{";
                this._data = this._data + "\"id\": \"" + this._cartLink + "\",";
                this._data = this._data + "\"customer\": {";
                this._data = this._data + "\"id\": \"" + this._customerLink + "\"";
                this._data = this._data + "},";
                this._data = this._data + "\"billingAddress\": {";
                this._data = this._data + "\"id\": \"" + this._billingLink + "\"";
                this._data = this._data + "},";
                this._data = this._data + "\"deliveryAddress\": {";
                this._data = this._data + "\"id\": \"" + this._shippingLink + "\"";
                this._data = this._data + "}";
                this._data = this._data + "}";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream2 = this._request.GetRequestStream())
                {
                    stream2.Write(this._bytes, 0, this._bytes.Length);
                }
                this._srr = "";
                using (HttpWebResponse response2 = (HttpWebResponse) this._request.GetResponse())
                {
                    this._srr = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__35.<>p__21 == null)
                {
                    <>o__35.<>p__21 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__35.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__35.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__35.<>p__19 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__35.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__35.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__35.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                if (<>o__35.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__35.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "delivery", typeof(Mesh), argumentInfo));
                }
                this._deliveryMethodId = <>o__35.<>p__21.Target(<>o__35.<>p__21, <>o__35.<>p__20.Target(<>o__35.<>p__20, <>o__35.<>p__19.Target(<>o__35.<>p__19, <>o__35.<>p__18.Target(<>o__35.<>p__18, <>o__35.<>p__17.Target(<>o__35.<>p__17, this._dynObj), "deliveryMethodID"))));
                this._request = (HttpWebRequest) WebRequest.Create(this._cartLink);
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.KeepAlive = true;
                this._request.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                this._request.Headers.Add("x-api-key", this._apiKey);
                this._request.Method = "PUT";
                this._request.ContentType = "application/json; charset=utf-8";
                this._request.ServicePoint.Expect100Continue = true;
                if (this._task.VariousStringData == "hipstore")
                {
                    this._request.Headers.Add("X-Request-Auth", "Hawk id=7c480586f6, mac=ktrF10ZQ+WGACqni0fXXv/+gOokzMf3rDRpZ00ovmRw=, ts=1521459862, nonce=8b3661, ext=");
                }
                this._data = "{";
                this._data = this._data + "\"delivery\": {";
                this._data = this._data + "\"deliveryMethodID\": \"" + this._deliveryMethodId + "\"";
                this._data = this._data + "}";
                this._data = this._data + "}";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream3 = this._request.GetRequestStream())
                {
                    stream3.Write(this._bytes, 0, this._bytes.Length);
                }
                this._srr = "";
                using (HttpWebResponse response3 = (HttpWebResponse) this._request.GetResponse())
                {
                    this._srr = new StreamReader(response3.GetResponseStream()).ReadToEnd();
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
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

        private bool SubmitShippingFrondend()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__38.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "email", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__0.Target(<>o__38.<>p__0, obj2, this._runner.Profile.Email);
                if (<>o__38.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__2 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Mesh), argumentInfo));
                }
                object obj3 = <>o__38.<>p__2.Target(<>o__38.<>p__2, this._client, $"{this._homeLink}/checkout/guest/", <>o__38.<>p__1.Target(<>o__38.<>p__1, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__38.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__3 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__3.Target(<>o__38.<>p__3, typeof(EveAIO.Extensions), obj3);
                if (<>o__38.<>p__8 == null)
                {
                    <>o__38.<>p__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__38.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Mesh), argumentInfo));
                }
                this._srr = <>o__38.<>p__8.Target(<>o__38.<>p__8, <>o__38.<>p__7.Target(<>o__38.<>p__7, <>o__38.<>p__6.Target(<>o__38.<>p__6, <>o__38.<>p__5.Target(<>o__38.<>p__5, <>o__38.<>p__4.Target(<>o__38.<>p__4, obj3)))));
                this._srr = this._client.Get($"{this._homeLink}/checkout/delivery/").Text();
                this._currentDoc.LoadHtml(this._srr);
                this._regions = this._currentDoc.DocumentNode.Descendants("select").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "region"));
                this._localeShipping = this._regions.Descendants("option").First<HtmlNode>(x => (x.InnerText.ToLowerInvariant() == this._runner.Profile.CountryShipping.ToLowerInvariant())).Attributes["value"].Value;
                this._localeBilling = this._regions.Descendants("option").First<HtmlNode>(x => (x.InnerText.ToLowerInvariant() == this._runner.Profile.Country.ToLowerInvariant())).Attributes["value"].Value;
                this._srr = this._client.Get($"{this._homeLink}/checkout/getDeliveryMethodsByLocale/{WebUtility.UrlEncode(this._localeShipping)}/").Text();
                this._currentDoc.LoadHtml(this._srr);
                double num2 = 10000.0;
                foreach (HtmlNode node in this._currentDoc.DocumentNode.Descendants("li"))
                {
                    string str = node.Descendants("span").First<HtmlNode>(x => (x.Attributes["data-oi-price"] > null)).InnerText.Trim();
                    if (str.ToLowerInvariant().Contains("free"))
                    {
                        this._deliveryMethodId = node.Attributes["data-deliveryMethodId"].Value;
                    }
                    else
                    {
                        string str3 = "";
                        foreach (char ch in str)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str3 = str3 + ch.ToString();
                            }
                        }
                        double num3 = double.Parse(str3.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if (num3 < num2)
                        {
                            this._deliveryMethodId = node.Attributes["data-deliveryMethodId"].Value;
                            num2 = num3;
                        }
                    }
                }
                obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__38.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__9.Target(<>o__38.<>p__9, obj2, this._localeShipping);
                if (<>o__38.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "locale", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__10.Target(<>o__38.<>p__10, obj2, "");
                if (<>o__38.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstName", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__11.Target(<>o__38.<>p__11, obj2, this._runner.Profile.FirstNameShipping);
                if (<>o__38.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastName", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__12.Target(<>o__38.<>p__12, obj2, this._runner.Profile.LastNameShipping);
                if (<>o__38.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "phone", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__13.Target(<>o__38.<>p__13, obj2, this._runner.Profile.PhoneShipping);
                if (<>o__38.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address1", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__14.Target(<>o__38.<>p__14, obj2, this._runner.Profile.Address1Shipping);
                if (<>o__38.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address2", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__15.Target(<>o__38.<>p__15, obj2, this._runner.Profile.Address2Shipping);
                if (<>o__38.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "town", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__16.Target(<>o__38.<>p__16, obj2, this._runner.Profile.CityShipping);
                if (<>o__38.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "county", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__17.Target(<>o__38.<>p__17, obj2, this._runner.Profile.StateShipping);
                if (<>o__38.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postcode", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__18.Target(<>o__38.<>p__18, obj2, this._runner.Profile.ZipShipping);
                if (<>o__38.<>p__19 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "addressPredict", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__19.Target(<>o__38.<>p__19, obj2, "");
                if (<>o__38.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "setOnCart", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__20.Target(<>o__38.<>p__20, obj2, "deliveryAddressID");
                if (<>o__38.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__22 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__21 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Mesh), argumentInfo));
                }
                obj3 = <>o__38.<>p__22.Target(<>o__38.<>p__22, this._client, $"{this._homeLink}/myaccount/addressbook/add/", <>o__38.<>p__21.Target(<>o__38.<>p__21, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__38.<>p__23 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__23 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__23.Target(<>o__38.<>p__23, typeof(EveAIO.Extensions), obj3);
                if (<>o__38.<>p__28 == null)
                {
                    <>o__38.<>p__28 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__38.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__26 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__25 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__24 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Mesh), argumentInfo));
                }
                this._srr = <>o__38.<>p__28.Target(<>o__38.<>p__28, <>o__38.<>p__27.Target(<>o__38.<>p__27, <>o__38.<>p__26.Target(<>o__38.<>p__26, <>o__38.<>p__25.Target(<>o__38.<>p__25, <>o__38.<>p__24.Target(<>o__38.<>p__24, obj3)))));
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__38.<>p__30 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__29 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mesh), argumentInfo));
                }
                object obj4 = <>o__38.<>p__30.Target(<>o__38.<>p__30, <>o__38.<>p__29.Target(<>o__38.<>p__29, this._dynObj, "ID"));
                if (<>o__38.<>p__35 == null)
                {
                    <>o__38.<>p__35 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__38.<>p__34 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__33 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__32 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__32 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__31 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__31 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Mesh), argumentInfo));
                }
                this._data = <>o__38.<>p__35.Target(<>o__38.<>p__35, <>o__38.<>p__34.Target(<>o__38.<>p__34, <>o__38.<>p__33.Target(<>o__38.<>p__33, <>o__38.<>p__32.Target(<>o__38.<>p__32, <>o__38.<>p__31.Target(<>o__38.<>p__31, "{\"addressId\":\"", obj4), "\",\"methodId\":\""), this._deliveryMethodId), "\",\"deliverySlot\":{}}"));
                obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__38.<>p__36 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__36 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "addressId", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__36.Target(<>o__38.<>p__36, obj2, obj4);
                if (<>o__38.<>p__37 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "methodId", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__37.Target(<>o__38.<>p__37, obj2, this._deliveryMethodId);
                if (<>o__38.<>p__38 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__38.<>p__38 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "deliverySlot", typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__38.Target(<>o__38.<>p__38, obj2, new Newtonsoft.Json.Linq.JObject());
                if (<>o__38.<>p__40 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__40 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__39 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__39 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Mesh), argumentInfo));
                }
                obj3 = <>o__38.<>p__40.Target(<>o__38.<>p__40, this._client, $"{this._homeLink}/checkout/updateDeliveryAddressAndMethod/ajax/", <>o__38.<>p__39.Target(<>o__38.<>p__39, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__38.<>p__41 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__41 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Mesh), argumentInfo));
                }
                <>o__38.<>p__41.Target(<>o__38.<>p__41, typeof(EveAIO.Extensions), obj3);
                if (<>o__38.<>p__46 == null)
                {
                    <>o__38.<>p__46 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mesh)));
                }
                if (<>o__38.<>p__45 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__44 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__43 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Mesh), argumentInfo));
                }
                if (<>o__38.<>p__42 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Mesh), argumentInfo));
                }
                this._srr = <>o__38.<>p__46.Target(<>o__38.<>p__46, <>o__38.<>p__45.Target(<>o__38.<>p__45, <>o__38.<>p__44.Target(<>o__38.<>p__44, <>o__38.<>p__43.Target(<>o__38.<>p__43, <>o__38.<>p__42.Target(<>o__38.<>p__42, obj3)))));
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
            public static readonly Mesh.<>c <>9;
            public static Func<HtmlNode, bool> <>9__29_0;
            public static Func<HtmlNode, bool> <>9__29_3;
            public static Func<HtmlNode, bool> <>9__29_4;
            public static Func<HtmlNode, bool> <>9__29_1;
            public static Func<HtmlNode, bool> <>9__29_2;
            public static Func<HtmlNode, bool> <>9__33_1;
            public static Func<HtmlNode, bool> <>9__33_2;
            public static Func<HtmlNode, bool> <>9__36_1;
            public static Func<HtmlNode, bool> <>9__36_2;
            public static Func<HtmlNode, bool> <>9__36_3;
            public static Func<HtmlNode, bool> <>9__36_4;
            public static Func<HtmlNode, bool> <>9__36_5;
            public static Func<HtmlNode, bool> <>9__36_6;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__36_7;
            public static Func<HtmlNode, bool> <>9__36_8;
            public static Func<HtmlNode, bool> <>9__36_9;
            public static Func<HtmlNode, bool> <>9__36_10;
            public static Func<HtmlNode, bool> <>9__36_11;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__36_12;
            public static Func<HtmlNode, bool> <>9__38_0;
            public static Func<HtmlNode, bool> <>9__38_3;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Mesh.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLinkFrondend>b__29_0(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"));

            internal bool <DirectLinkFrondend>b__29_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "options"));

            internal bool <DirectLinkFrondend>b__29_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "options"));

            internal bool <DirectLinkFrondend>b__29_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLinkFrondend>b__29_4(HtmlNode x) => 
                ((x.Attributes["data-e2e"] != null) && (x.Attributes["data-e2e"].Value == "product-price"));

            internal bool <SubmitOrderBackend>b__33_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pan"));

            internal bool <SubmitOrderBackend>b__33_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pan"));

            internal bool <SubmitOrderFrontend>b__36_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitOrderFrontend>b__36_10(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"));

            internal bool <SubmitOrderFrontend>b__36_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <SubmitOrderFrontend>b__36_12(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <SubmitOrderFrontend>b__36_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrderFrontend>b__36_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"));

            internal bool <SubmitOrderFrontend>b__36_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "acs3DAuthForm"));

            internal bool <SubmitOrderFrontend>b__36_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrderFrontend>b__36_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrderFrontend>b__36_7(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <SubmitOrderFrontend>b__36_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrderFrontend>b__36_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitShippingFrondend>b__38_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "region"));

            internal bool <SubmitShippingFrondend>b__38_3(HtmlNode x) => 
                (x.Attributes["data-oi-price"] > null);
        }

        [CompilerGenerated]
        private static class <>o__28
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, bool>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__25;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__26;
        }

        [CompilerGenerated]
        private static class <>o__33
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>> <>p__7;
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
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string>> <>p__8;
        }

        [CompilerGenerated]
        private static class <>o__35
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__8;
            public static CallSite<Func<CallSite, object, bool>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string>> <>p__21;
        }

        [CompilerGenerated]
        private static class <>o__37
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__12;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__13;
            public static CallSite<Action<CallSite, Type, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object, object>> <>p__22;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__23;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__24;
            public static CallSite<Action<CallSite, Type, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, string>> <>p__30;
        }

        [CompilerGenerated]
        private static class <>o__38
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__1;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__2;
            public static CallSite<Action<CallSite, Type, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__21;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__22;
            public static CallSite<Action<CallSite, Type, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, string, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, string, object>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, string>> <>p__35;
            public static CallSite<Func<CallSite, object, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__38;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__39;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__40;
            public static CallSite<Action<CallSite, Type, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, string>> <>p__46;
        }

        [CompilerGenerated]
        private static class <>o__39
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, int, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
        }

        [CompilerGenerated]
        private static class <>o__40
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, int, object>> <>p__2;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__3;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__4;
            public static CallSite<Action<CallSite, Type, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, int, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string>> <>p__23;
        }
    }
}

