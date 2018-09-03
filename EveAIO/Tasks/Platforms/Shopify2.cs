namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using EveAIO.Tasks.Dto;
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
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    internal class Shopify2 : IPlatform
    {
        private Client _client;
        private Client _clientTmp;
        private TaskRunner _runner;
        private TaskObject _task;
        private List<string> _yeezyVisited;
        private string _cardId;
        private string _checkoutToken;
        private bool _isInitError;
        private bool _isInitIpBan;
        private string _shipping;
        private string _apiToken;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private bool _queueUp;
        private object _webUrl;
        private Dictionary<string, string> _diData;
        private string _note;
        private static DateTime? _eflashUkGetter;
        private List<KeyValuePair<string, Product>> _childProducts;
        private List<SuccessObject> CHILD_SUCCESSES;
        private List<Newtonsoft.Json.Linq.JObject> _childTasksAtc;
        public List<ShopifyMultiAddition> _multiAdditions;
        public List<Product> _availableProducts;
        private object _endpoint;

        public Shopify2(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._yeezyVisited = new List<string>();
            this._currentDoc = new HtmlDocument();
            this._diData = new Dictionary<string, string>();
            this.CHILD_SUCCESSES = new List<SuccessObject>();
            this._endpoint = "api";
            this._runner = runner;
            this._task = task;
            string apiToken = "";
            if (!WebsitesInfo.SHOPIFY_WEBS.Any<ShopifyWebsiteInfo>(x => (x.HomeLink == this._task.HomeUrl)))
            {
                this.ExtractApiKey();
            }
            apiToken = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.HomeLink == this._task.HomeUrl)).ApiToken;
            byte[] bytes = Encoding.UTF8.GetBytes(apiToken + ":");
            this._apiToken = Convert.ToBase64String(bytes);
            this.SetClient();
            if ((this._task.HomeUrl.ToLowerInvariant().Contains("kith") || this._task.HomeUrl.ToLowerInvariant().Contains("yeezysu")) || this._task.HomeUrl.ToLowerInvariant().Contains("eflash"))
            {
                this._endpoint = "wallets";
            }
        }

        public bool Atc()
        {
            this._shipping = "";
            if ((this._task.TaskType == TaskObject.TaskTypeEnum.variant) || (this._task.Link.ToLowerInvariant().Contains("/cart/") && this._task.Link.ToLowerInvariant().Contains(":1")))
            {
                this.Init();
                States.WriteLogger(this._task, States.LOGGER_STATES.ATCLINK_RECOGNIZED, null, "", "");
            }
            if (this._queueUp)
            {
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_IN_QUEUE, null, "", "");
            }
            while (string.IsNullOrEmpty(this._cardId) || string.IsNullOrEmpty(this._checkoutToken))
            {
                if (this._isInitError)
                {
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    return false;
                }
                if (this._isInitIpBan)
                {
                    goto Label_3D67;
                }
                Thread.Sleep(200);
            }
            this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
            try
            {
                Newtonsoft.Json.Linq.JArray array;
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
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SHOPIFY, null, variant, "");
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                object obj3 = new Newtonsoft.Json.Linq.JObject();
                if (this._task.HomeUrl.ToLowerInvariant().Contains("palace"))
                {
                    this._srr = this._client.Get($"{this._task.HomeUrl}/cart/add/{variant}").Text();
                    this._currentDoc.LoadHtml(this._srr);
                    this._srr = this._client.Get($"{this._task.HomeUrl}/cart").Text();
                    this._currentDoc.LoadHtml(this._srr);
                    this._note = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "note"))).Attributes["value"].Value;
                }
                if (Global.SETTINGS.TASKS.Any<TaskObject>(x => x.ParentId == this._task.Id))
                {
                    this._childTasksAtc = new List<Newtonsoft.Json.Linq.JObject>();
                    this._childProducts = new List<KeyValuePair<string, Product>>();
                    this._multiAdditions = new List<ShopifyMultiAddition>();
                    foreach (TaskObject obj4 in from x in Global.SETTINGS.TASKS
                        where x.ParentId == this._task.Id
                        select x)
                    {
                        try
                        {
                            if (obj4.TaskType == TaskObject.TaskTypeEnum.directlink)
                            {
                                Product product = this.DirectLinkMulti(obj4, obj4.Link);
                                if ((product != null) && this.AtcMulti(obj4, product))
                                {
                                    this._childProducts.Add(new KeyValuePair<string, Product>(obj4.Id, product));
                                    this.CreateChildProdSuccess(obj4, product);
                                }
                            }
                            else
                            {
                                Product product = this.SearchMulti(obj4);
                                if ((product != null) && this.AtcMulti(obj4, product))
                                {
                                    this._childProducts.Add(new KeyValuePair<string, Product>(obj4.Id, product));
                                    this.CreateChildProdSuccess(obj4, product);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                if (this._task.Quantity < 1)
                {
                    this._task.Quantity = 1;
                }
                if (this._task.HomeUrl.ToLowerInvariant().Contains("kith"))
                {
                    if (<>o__33.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__0 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__0.Target(<>o__33.<>p__0, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__33.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__1.Target(<>o__33.<>p__1, obj3, variant);
                    if (<>o__33.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__2.Target(<>o__33.<>p__2, obj3, this._task.Quantity);
                    if (!string.IsNullOrEmpty(WebsitesInfo.KITH_PROPERTIES.Key))
                    {
                        if (<>o__33.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__3 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__3.Target(<>o__33.<>p__3, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__33.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__5 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__5.Target(<>o__33.<>p__5, <>o__33.<>p__4.Target(<>o__33.<>p__4, obj3), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.KITH_PROPERTIES.Key, WebsitesInfo.KITH_PROPERTIES.Value));
                    }
                    array = new Newtonsoft.Json.Linq.JArray();
                    if (<>o__33.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__6 = CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__6.Target(<>o__33.<>p__6, array, obj3);
                    Newtonsoft.Json.Linq.JArray array2 = array;
                    if ((this._childTasksAtc != null) && (this._childTasksAtc.Count > 0))
                    {
                        foreach (Newtonsoft.Json.Linq.JObject obj5 in this._childTasksAtc)
                        {
                            array2.Add(obj5);
                        }
                    }
                    if (<>o__33.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__8 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__8.Target(<>o__33.<>p__8, <>o__33.<>p__7.Target(<>o__33.<>p__7, obj2), array2);
                }
                else if (this._task.HomeUrl.ToLowerInvariant().Contains("funko-shop"))
                {
                    if (<>o__33.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__9 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__9.Target(<>o__33.<>p__9, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__33.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__10.Target(<>o__33.<>p__10, obj3, variant);
                    if (<>o__33.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__11 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__11.Target(<>o__33.<>p__11, obj3, this._task.Quantity);
                    if (!string.IsNullOrEmpty(WebsitesInfo.FUNKO_PROPERTIES.Key))
                    {
                        if (<>o__33.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__12 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__12.Target(<>o__33.<>p__12, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__33.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__14 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__14.Target(<>o__33.<>p__14, <>o__33.<>p__13.Target(<>o__33.<>p__13, obj3), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.FUNKO_PROPERTIES.Key, WebsitesInfo.FUNKO_PROPERTIES.Value));
                    }
                    array = new Newtonsoft.Json.Linq.JArray();
                    if (<>o__33.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__15 = CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__15.Target(<>o__33.<>p__15, array, obj3);
                    Newtonsoft.Json.Linq.JArray array3 = array;
                    if ((this._childTasksAtc != null) && (this._childTasksAtc.Count > 0))
                    {
                        foreach (Newtonsoft.Json.Linq.JObject obj6 in this._childTasksAtc)
                        {
                            array3.Add(obj6);
                        }
                    }
                    if (<>o__33.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__17 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__17.Target(<>o__33.<>p__17, <>o__33.<>p__16.Target(<>o__33.<>p__16, obj2), array3);
                }
                else if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash-us"))
                {
                    if (<>o__33.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__18 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__18.Target(<>o__33.<>p__18, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__33.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__19.Target(<>o__33.<>p__19, obj3, variant);
                    if (<>o__33.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__20 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__20.Target(<>o__33.<>p__20, obj3, this._task.Quantity);
                    if (!string.IsNullOrEmpty(WebsitesInfo.EFLESH_US_PROPERTIES.Key))
                    {
                        if (<>o__33.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__21 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__21.Target(<>o__33.<>p__21, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__33.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__23 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__23.Target(<>o__33.<>p__23, <>o__33.<>p__22.Target(<>o__33.<>p__22, obj3), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.EFLESH_US_PROPERTIES.Key, WebsitesInfo.EFLESH_US_PROPERTIES.Value));
                    }
                    array = new Newtonsoft.Json.Linq.JArray();
                    if (<>o__33.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__24 = CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__24.Target(<>o__33.<>p__24, array, obj3);
                    Newtonsoft.Json.Linq.JArray array4 = array;
                    if ((this._childTasksAtc != null) && (this._childTasksAtc.Count > 0))
                    {
                        foreach (Newtonsoft.Json.Linq.JObject obj7 in this._childTasksAtc)
                        {
                            array4.Add(obj7);
                        }
                    }
                    if (<>o__33.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__26 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__26.Target(<>o__33.<>p__26, <>o__33.<>p__25.Target(<>o__33.<>p__25, obj2), array4);
                }
                else if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash."))
                {
                    if (<>o__33.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__27 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__27.Target(<>o__33.<>p__27, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__33.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__28.Target(<>o__33.<>p__28, obj3, variant);
                    if (<>o__33.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__29 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__29.Target(<>o__33.<>p__29, obj3, this._task.Quantity);
                    if ((WebsitesInfo.EFLESH_UK_PROPERTIES.HasValue && WebsitesInfo.EFLESH_UK_PROPERTIES.HasValue) && !string.IsNullOrEmpty(WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Key))
                    {
                        if (<>o__33.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__30 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__30.Target(<>o__33.<>p__30, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__33.<>p__32 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__32 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__33.<>p__32.Target(<>o__33.<>p__32, <>o__33.<>p__31.Target(<>o__33.<>p__31, obj3), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Key, WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Value));
                    }
                    array = new Newtonsoft.Json.Linq.JArray();
                    if (<>o__33.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__33 = CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__33.Target(<>o__33.<>p__33, array, obj3);
                    Newtonsoft.Json.Linq.JArray array5 = array;
                    if ((this._childTasksAtc != null) && (this._childTasksAtc.Count > 0))
                    {
                        foreach (Newtonsoft.Json.Linq.JObject obj8 in this._childTasksAtc)
                        {
                            array5.Add(obj8);
                        }
                    }
                    if (<>o__33.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__35 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__35.Target(<>o__33.<>p__35, <>o__33.<>p__34.Target(<>o__33.<>p__34, obj2), array5);
                }
                else
                {
                    if (<>o__33.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__36 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__36.Target(<>o__33.<>p__36, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__33.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__37.Target(<>o__33.<>p__37, obj3, variant);
                    if (<>o__33.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__38 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__38.Target(<>o__33.<>p__38, obj3, this._task.Quantity);
                    array = new Newtonsoft.Json.Linq.JArray();
                    if (<>o__33.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__39 = CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__39.Target(<>o__33.<>p__39, array, obj3);
                    Newtonsoft.Json.Linq.JArray array6 = array;
                    if ((this._childTasksAtc != null) && (this._childTasksAtc.Count > 0))
                    {
                        foreach (Newtonsoft.Json.Linq.JObject obj9 in this._childTasksAtc)
                        {
                            array6.Add(obj9);
                        }
                    }
                    if (<>o__33.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__41 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__41.Target(<>o__33.<>p__41, <>o__33.<>p__40.Target(<>o__33.<>p__40, obj2), array6);
                }
                try
                {
                    if (<>o__33.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__43 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PatchJson", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__42 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Shopify2), argumentInfo));
                    }
                    object obj10 = <>o__33.<>p__43.Target(<>o__33.<>p__43, this._client, this._runner.HomeUrl + $"/{this._endpoint}/checkouts/" + this._checkoutToken + ".json", <>o__33.<>p__42.Target(<>o__33.<>p__42, typeof(Newtonsoft.Json.JsonConvert), obj2));
                    if (<>o__33.<>p__48 == null)
                    {
                        <>o__33.<>p__48 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__33.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Shopify2), argumentInfo));
                    }
                    this._srr = <>o__33.<>p__48.Target(<>o__33.<>p__48, <>o__33.<>p__47.Target(<>o__33.<>p__47, <>o__33.<>p__46.Target(<>o__33.<>p__46, <>o__33.<>p__45.Target(<>o__33.<>p__45, <>o__33.<>p__44.Target(<>o__33.<>p__44, obj10)))));
                    if (<>o__33.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__49 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__33.<>p__49.Target(<>o__33.<>p__49, typeof(EveAIO.Extensions), obj10);
                }
                catch (WebException)
                {
                }
                if (this._srr.Contains("not_enough_in_stock"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    return false;
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__33.<>p__52 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__52 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                }
                if (<>o__33.<>p__51 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__33.<>p__51 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                }
                if (<>o__33.<>p__50 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errors", typeof(Shopify2), argumentInfo));
                }
                if (<>o__33.<>p__52.Target(<>o__33.<>p__52, <>o__33.<>p__51.Target(<>o__33.<>p__51, <>o__33.<>p__50.Target(<>o__33.<>p__50, this._dynObj), null)))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
                }
                if (<>o__33.<>p__56 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__56 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                }
                if (<>o__33.<>p__55 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__33.<>p__55 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                }
                if (<>o__33.<>p__54 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                }
                if (<>o__33.<>p__53 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__53 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                }
                if (<>o__33.<>p__56.Target(<>o__33.<>p__56, <>o__33.<>p__55.Target(<>o__33.<>p__55, <>o__33.<>p__54.Target(<>o__33.<>p__54, <>o__33.<>p__53.Target(<>o__33.<>p__53, this._dynObj)), null)))
                {
                    if (<>o__33.<>p__66 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__66 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__59 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__33.<>p__59 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__58 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__58 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "available_shipping_rates", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__57 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    object obj11 = <>o__33.<>p__59.Target(<>o__33.<>p__59, <>o__33.<>p__58.Target(<>o__33.<>p__58, <>o__33.<>p__57.Target(<>o__33.<>p__57, this._dynObj)), null);
                    if (<>o__33.<>p__65 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__65 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Shopify2), argumentInfo));
                    }
                    if (!<>o__33.<>p__65.Target(<>o__33.<>p__65, obj11))
                    {
                        if (<>o__33.<>p__64 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__64 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__63 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__63 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__62 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__61 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__61 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                    }
                    if (<>o__33.<>p__66.Target(<>o__33.<>p__66, (<>o__33.<>p__60 != null) ? obj11 : <>o__33.<>p__64.Target(<>o__33.<>p__64, obj11, <>o__33.<>p__63.Target(<>o__33.<>p__63, <>o__33.<>p__62.Target(<>o__33.<>p__62, <>o__33.<>p__61.Target(<>o__33.<>p__61, <>o__33.<>p__60.Target(<>o__33.<>p__60, this._dynObj), "available_shipping_rates")), 0))))
                    {
                        double num2 = 0.0;
                        string str2 = "";
                        if (<>o__33.<>p__114 == null)
                        {
                            <>o__33.<>p__114 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                        }
                        if (<>o__33.<>p__68 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "available_shipping_rates", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__67 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__67 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                        }
                        foreach (object obj12 in <>o__33.<>p__114.Target(<>o__33.<>p__114, <>o__33.<>p__68.Target(<>o__33.<>p__68, <>o__33.<>p__67.Target(<>o__33.<>p__67, this._dynObj))))
                        {
                            if (<>o__33.<>p__72 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__72 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__71 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__71 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__70 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__69 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__69 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            obj11 = <>o__33.<>p__72.Target(<>o__33.<>p__72, <>o__33.<>p__71.Target(<>o__33.<>p__71, <>o__33.<>p__70.Target(<>o__33.<>p__70, <>o__33.<>p__69.Target(<>o__33.<>p__69, obj12, "id"))), "In%20Shop%20Pick%20Up");
                            if (<>o__33.<>p__79 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__79 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                            }
                            if (!<>o__33.<>p__79.Target(<>o__33.<>p__79, obj11))
                            {
                                if (<>o__33.<>p__78 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__33.<>p__78 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__33.<>p__77 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__33.<>p__77 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__33.<>p__76 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__33.<>p__76 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__33.<>p__75 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__33.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__33.<>p__74 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__33.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__33.<>p__73 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__33.<>p__73 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                if (!<>o__33.<>p__78.Target(<>o__33.<>p__78, <>o__33.<>p__77.Target(<>o__33.<>p__77, obj11, <>o__33.<>p__76.Target(<>o__33.<>p__76, <>o__33.<>p__75.Target(<>o__33.<>p__75, <>o__33.<>p__74.Target(<>o__33.<>p__74, <>o__33.<>p__73.Target(<>o__33.<>p__73, obj12, "id"))), "In%20Store%20Pick"))))
                                {
                                    if (!string.IsNullOrEmpty(str2))
                                    {
                                        if (<>o__33.<>p__96 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__96 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__95 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__95 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__94 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__94 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__93 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__93 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                        }
                                        object obj13 = <>o__33.<>p__96.Target(<>o__33.<>p__96, typeof(double), <>o__33.<>p__95.Target(<>o__33.<>p__95, <>o__33.<>p__94.Target(<>o__33.<>p__94, <>o__33.<>p__93.Target(<>o__33.<>p__93, obj12, "price"))), CultureInfo.InvariantCulture);
                                        if (<>o__33.<>p__97 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__97 = CallSite<Func<CallSite, object, double, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Shopify2), argumentInfo));
                                        }
                                        obj11 = <>o__33.<>p__97.Target(<>o__33.<>p__97, obj13, num2);
                                        if (<>o__33.<>p__100 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__100 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                                        }
                                        if (!<>o__33.<>p__100.Target(<>o__33.<>p__100, obj11))
                                        {
                                            if (<>o__33.<>p__99 == null)
                                            {
                                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                                <>o__33.<>p__99 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                                            }
                                            if (<>o__33.<>p__98 == null)
                                            {
                                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                                <>o__33.<>p__98 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Shopify2), argumentInfo));
                                            }
                                            if (!<>o__33.<>p__99.Target(<>o__33.<>p__99, <>o__33.<>p__98.Target(<>o__33.<>p__98, obj11, str2.Contains("In%20Shop%20Pick%20Up"))))
                                            {
                                                continue;
                                            }
                                        }
                                        if (<>o__33.<>p__104 == null)
                                        {
                                            <>o__33.<>p__104 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                        }
                                        if (<>o__33.<>p__103 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__103 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__102 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__102 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__101 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__101 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                        }
                                        str2 = <>o__33.<>p__104.Target(<>o__33.<>p__104, <>o__33.<>p__103.Target(<>o__33.<>p__103, <>o__33.<>p__102.Target(<>o__33.<>p__102, <>o__33.<>p__101.Target(<>o__33.<>p__101, obj12, "id"))));
                                        if (<>o__33.<>p__109 == null)
                                        {
                                            <>o__33.<>p__109 = CallSite<Func<CallSite, object, double>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify2)));
                                        }
                                        if (<>o__33.<>p__108 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__108 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__107 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__107 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__106 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__106 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__105 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__105 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                        }
                                        num2 = <>o__33.<>p__109.Target(<>o__33.<>p__109, <>o__33.<>p__108.Target(<>o__33.<>p__108, typeof(double), <>o__33.<>p__107.Target(<>o__33.<>p__107, <>o__33.<>p__106.Target(<>o__33.<>p__106, <>o__33.<>p__105.Target(<>o__33.<>p__105, obj12, "price"))), CultureInfo.InvariantCulture));
                                        if (<>o__33.<>p__113 == null)
                                        {
                                            <>o__33.<>p__113 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                        }
                                        if (<>o__33.<>p__112 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__112 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__111 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__111 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__110 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__110 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                        }
                                        <>o__33.<>p__113.Target(<>o__33.<>p__113, <>o__33.<>p__112.Target(<>o__33.<>p__112, <>o__33.<>p__111.Target(<>o__33.<>p__111, <>o__33.<>p__110.Target(<>o__33.<>p__110, obj12, "price"))));
                                    }
                                    else
                                    {
                                        if (<>o__33.<>p__83 == null)
                                        {
                                            <>o__33.<>p__83 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                        }
                                        if (<>o__33.<>p__82 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__82 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__81 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__81 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__80 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__80 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                        }
                                        str2 = <>o__33.<>p__83.Target(<>o__33.<>p__83, <>o__33.<>p__82.Target(<>o__33.<>p__82, <>o__33.<>p__81.Target(<>o__33.<>p__81, <>o__33.<>p__80.Target(<>o__33.<>p__80, obj12, "id"))));
                                        if (<>o__33.<>p__88 == null)
                                        {
                                            <>o__33.<>p__88 = CallSite<Func<CallSite, object, double>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify2)));
                                        }
                                        if (<>o__33.<>p__87 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__87 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__86 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__86 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__85 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__85 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__84 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__84 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                        }
                                        num2 = <>o__33.<>p__88.Target(<>o__33.<>p__88, <>o__33.<>p__87.Target(<>o__33.<>p__87, typeof(double), <>o__33.<>p__86.Target(<>o__33.<>p__86, <>o__33.<>p__85.Target(<>o__33.<>p__85, <>o__33.<>p__84.Target(<>o__33.<>p__84, obj12, "price"))), CultureInfo.InvariantCulture));
                                        if (<>o__33.<>p__92 == null)
                                        {
                                            <>o__33.<>p__92 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                        }
                                        if (<>o__33.<>p__91 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__91 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__90 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__33.<>p__90 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                        }
                                        if (<>o__33.<>p__89 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                            <>o__33.<>p__89 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                        }
                                        <>o__33.<>p__92.Target(<>o__33.<>p__92, <>o__33.<>p__91.Target(<>o__33.<>p__91, <>o__33.<>p__90.Target(<>o__33.<>p__90, <>o__33.<>p__89.Target(<>o__33.<>p__89, obj12, "price"))));
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(str2))
                        {
                            this._shipping = str2;
                        }
                    }
                    if (<>o__33.<>p__118 == null)
                    {
                        <>o__33.<>p__118 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__33.<>p__117 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__117 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__116 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__116 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__33.<>p__115 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__115 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    string str3 = <>o__33.<>p__118.Target(<>o__33.<>p__118, <>o__33.<>p__117.Target(<>o__33.<>p__117, <>o__33.<>p__116.Target(<>o__33.<>p__116, <>o__33.<>p__115.Target(<>o__33.<>p__115, this._dynObj))));
                    if (str3.Contains(variant))
                    {
                        if ((this._multiAdditions != null) && (this._multiAdditions.Count > 0))
                        {
                            foreach (ShopifyMultiAddition add in this._multiAdditions)
                            {
                                if (str3.Contains(add.Variant))
                                {
                                    add.Added = true;
                                    KeyValuePair<string, Product> addition = this._childProducts.First<KeyValuePair<string, Product>>(x => x.Value.SuprimetMultiPickedSize.Value.Value == add.Variant);
                                    Global.SETTINGS.TASKS.First<TaskObject>(x => (x.Id == addition.Key)).PickedSize = addition.Value.SuprimetMultiPickedSize.Value.Key;
                                }
                            }
                        }
                        if (<>o__33.<>p__122 == null)
                        {
                            <>o__33.<>p__122 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                        }
                        if (<>o__33.<>p__121 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__121 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__120 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__33.<>p__120 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__33.<>p__119 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__33.<>p__119 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "checkout", typeof(Shopify2), argumentInfo));
                        }
                        this._webUrl = <>o__33.<>p__122.Target(<>o__33.<>p__122, <>o__33.<>p__121.Target(<>o__33.<>p__121, <>o__33.<>p__120.Target(<>o__33.<>p__120, <>o__33.<>p__119.Target(<>o__33.<>p__119, this._dynObj), "web_url")));
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                        if (this._runner.Product == null)
                        {
                            Product product3 = new Product();
                            if (<>o__33.<>p__128 == null)
                            {
                                <>o__33.<>p__128 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__33.<>p__127 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__127 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__126 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__126 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__125 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__125 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__124 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__124 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__123 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__123 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                            }
                            product3.ProductTitle = <>o__33.<>p__128.Target(<>o__33.<>p__128, <>o__33.<>p__127.Target(<>o__33.<>p__127, <>o__33.<>p__126.Target(<>o__33.<>p__126, <>o__33.<>p__125.Target(<>o__33.<>p__125, <>o__33.<>p__124.Target(<>o__33.<>p__124, <>o__33.<>p__123.Target(<>o__33.<>p__123, this._dynObj))), "title")));
                            if (<>o__33.<>p__134 == null)
                            {
                                <>o__33.<>p__134 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__33.<>p__133 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__133 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__132 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__132 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__131 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__131 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__130 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__130 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__129 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__129 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                            }
                            product3.Link = <>o__33.<>p__134.Target(<>o__33.<>p__134, <>o__33.<>p__133.Target(<>o__33.<>p__133, <>o__33.<>p__132.Target(<>o__33.<>p__132, <>o__33.<>p__131.Target(<>o__33.<>p__131, <>o__33.<>p__130.Target(<>o__33.<>p__130, <>o__33.<>p__129.Target(<>o__33.<>p__129, this._dynObj))), "title")));
                            if (<>o__33.<>p__140 == null)
                            {
                                <>o__33.<>p__140 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__33.<>p__139 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__139 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__138 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__138 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__137 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__137 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__136 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__136 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__135 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__135 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                            }
                            product3.Price = <>o__33.<>p__140.Target(<>o__33.<>p__140, <>o__33.<>p__139.Target(<>o__33.<>p__139, <>o__33.<>p__138.Target(<>o__33.<>p__138, <>o__33.<>p__137.Target(<>o__33.<>p__137, <>o__33.<>p__136.Target(<>o__33.<>p__136, <>o__33.<>p__135.Target(<>o__33.<>p__135, this._dynObj))), "price")));
                            this._runner.Product = product3;
                            if (<>o__33.<>p__146 == null)
                            {
                                <>o__33.<>p__146 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__33.<>p__145 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__145 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__144 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__144 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__143 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__143 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__142 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__142 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__141 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__141 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                            }
                            this._task.ImgUrl = <>o__33.<>p__146.Target(<>o__33.<>p__146, <>o__33.<>p__145.Target(<>o__33.<>p__145, <>o__33.<>p__144.Target(<>o__33.<>p__144, <>o__33.<>p__143.Target(<>o__33.<>p__143, <>o__33.<>p__142.Target(<>o__33.<>p__142, <>o__33.<>p__141.Target(<>o__33.<>p__141, this._dynObj))), "image_url")));
                            if (<>o__33.<>p__153 == null)
                            {
                                <>o__33.<>p__153 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__33.<>p__152 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__152 = CallSite<Func<CallSite, Type, TaskObject, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "UpdateSizeString", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__151 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__151 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__150 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__150 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__149 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__149 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__148 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__148 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "line_items", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__33.<>p__147 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__147 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                            }
                            this._task.Size = <>o__33.<>p__153.Target(<>o__33.<>p__153, <>o__33.<>p__152.Target(<>o__33.<>p__152, typeof(ShopifyCommon), this._task, <>o__33.<>p__151.Target(<>o__33.<>p__151, <>o__33.<>p__150.Target(<>o__33.<>p__150, <>o__33.<>p__149.Target(<>o__33.<>p__149, <>o__33.<>p__148.Target(<>o__33.<>p__148, <>o__33.<>p__147.Target(<>o__33.<>p__147, this._dynObj))), "variant_title"))));
                            this._runner.PickedSize = new KeyValuePair<string, string>(this._task.Size, variant);
                            this._runner.Success.Size = this._runner.PickedSize.Value.Key;
                            this._runner.Success.Price = this._runner.Product.Price;
                            this._runner.Success.Link = this._runner.Product.Link;
                            this._runner.Success.ProductName = this._runner.Product.ProductTitle;
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
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
                    string str4 = "";
                    if (exception is WebException)
                    {
                        str4 = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    if (string.IsNullOrEmpty(str4))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", str4);
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        Label_3D67:
            this._runner.IsError = true;
            this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
            return false;
        }

        public bool AtcMulti(TaskObject task, Product product)
        {
            this._shipping = "";
            this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
            try
            {
                string variant = "";
                if (task.Link.ToLowerInvariant().Contains("cart") && task.Link.ToLowerInvariant().Contains(":1"))
                {
                    variant = task.Link.Substring(task.Link.IndexOf("cart") + 5);
                    variant = variant.Substring(0, variant.IndexOf(":1"));
                }
                else if (this._task.TaskType == TaskObject.TaskTypeEnum.variant)
                {
                    variant = task.Variant;
                }
                else
                {
                    variant = product.SuprimetMultiPickedSize.Value.Value;
                }
                States.WriteLogger(task, States.LOGGER_STATES.ATC_SHOPIFY, null, variant, "");
                object obj3 = new Newtonsoft.Json.Linq.JObject();
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (this._task.HomeUrl.ToLowerInvariant().Contains("palace"))
                {
                    this._srr = this._client.Get($"{this._task.HomeUrl}/cart/add/{variant}").Text();
                    this._currentDoc.LoadHtml(this._srr);
                    this._srr = this._client.Get($"{this._task.HomeUrl}/cart").Text();
                    this._currentDoc.LoadHtml(this._srr);
                    this._note = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "note"))).Attributes["value"].Value;
                }
                if (this._task.HomeUrl.ToLowerInvariant().Contains("kith"))
                {
                    if (<>o__30.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__0 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__0.Target(<>o__30.<>p__0, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__1.Target(<>o__30.<>p__1, obj2, variant);
                    if (<>o__30.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__2.Target(<>o__30.<>p__2, obj2, 1);
                    if (<>o__30.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__3 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__3.Target(<>o__30.<>p__3, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__5 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__30.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__5.Target(<>o__30.<>p__5, <>o__30.<>p__4.Target(<>o__30.<>p__4, obj2), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.KITH_PROPERTIES.Key, WebsitesInfo.KITH_PROPERTIES.Value));
                    if (<>o__30.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__6 = CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__6.Target(<>o__30.<>p__6, this._childTasksAtc, obj2);
                    ShopifyMultiAddition item = new ShopifyMultiAddition {
                        TaskId = task.Id,
                        Variant = variant
                    };
                    this._multiAdditions.Add(item);
                }
                else if (this._task.HomeUrl.ToLowerInvariant().Contains("funko-shop"))
                {
                    if (<>o__30.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__7 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__7.Target(<>o__30.<>p__7, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__8.Target(<>o__30.<>p__8, obj2, variant);
                    if (<>o__30.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__9.Target(<>o__30.<>p__9, obj2, 1);
                    if (<>o__30.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__10 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__10.Target(<>o__30.<>p__10, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__12 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__30.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__12.Target(<>o__30.<>p__12, <>o__30.<>p__11.Target(<>o__30.<>p__11, obj2), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.FUNKO_PROPERTIES.Key, WebsitesInfo.FUNKO_PROPERTIES.Value));
                    if (<>o__30.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__13 = CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__13.Target(<>o__30.<>p__13, this._childTasksAtc, obj2);
                    ShopifyMultiAddition item = new ShopifyMultiAddition {
                        TaskId = task.Id,
                        Variant = variant
                    };
                    this._multiAdditions.Add(item);
                }
                else if (!this._task.HomeUrl.ToLowerInvariant().Contains("eflash-us"))
                {
                    if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash"))
                    {
                        if (<>o__30.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__21 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__21.Target(<>o__30.<>p__21, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__30.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__22.Target(<>o__30.<>p__22, obj2, variant);
                        if (<>o__30.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__23 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__23.Target(<>o__30.<>p__23, obj2, 1);
                        if (<>o__30.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__24 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__24.Target(<>o__30.<>p__24, obj2, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__30.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__26 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__30.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__30.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__26.Target(<>o__30.<>p__26, <>o__30.<>p__25.Target(<>o__30.<>p__25, obj2), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Key, WebsitesInfo.EFLESH_UK_PROPERTIES.Value.Value));
                        if (<>o__30.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__30.<>p__27 = CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__27.Target(<>o__30.<>p__27, this._childTasksAtc, obj2);
                        ShopifyMultiAddition item = new ShopifyMultiAddition {
                            TaskId = task.Id,
                            Variant = variant
                        };
                        this._multiAdditions.Add(item);
                    }
                    else
                    {
                        if (<>o__30.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__28 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__28.Target(<>o__30.<>p__28, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__30.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__29.Target(<>o__30.<>p__29, obj2, variant);
                        if (<>o__30.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__30 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__30.Target(<>o__30.<>p__30, obj2, 1);
                        if (<>o__30.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__30.<>p__31 = CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                        }
                        <>o__30.<>p__31.Target(<>o__30.<>p__31, this._childTasksAtc, obj2);
                        ShopifyMultiAddition item = new ShopifyMultiAddition {
                            TaskId = task.Id,
                            Variant = variant
                        };
                        this._multiAdditions.Add(item);
                    }
                }
                else
                {
                    if (<>o__30.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__14 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__14.Target(<>o__30.<>p__14, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "variant_id", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__15.Target(<>o__30.<>p__15, obj2, variant);
                    if (<>o__30.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__16 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "quantity", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__16.Target(<>o__30.<>p__16, obj2, 1);
                    if (<>o__30.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__17 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__17.Target(<>o__30.<>p__17, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__19 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__30.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "properties", typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__19.Target(<>o__30.<>p__19, <>o__30.<>p__18.Target(<>o__30.<>p__18, obj2), new Newtonsoft.Json.Linq.JProperty(WebsitesInfo.EFLESH_US_PROPERTIES.Key, WebsitesInfo.EFLESH_US_PROPERTIES.Value));
                    if (<>o__30.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__20 = CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__30.<>p__20.Target(<>o__30.<>p__20, this._childTasksAtc, obj2);
                    ShopifyMultiAddition item = new ShopifyMultiAddition {
                        TaskId = task.Id,
                        Variant = variant
                    };
                    this._multiAdditions.Add(item);
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
                    string str2 = "";
                    if (exception is WebException)
                    {
                        str2 = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    if (string.IsNullOrEmpty(str2))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", str2);
                    }
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
            if (string.IsNullOrEmpty(this._shipping) && !this.GetShippingRates())
            {
                return false;
            }
            if (!this.Payment())
            {
                return false;
            }
            return this.SubmitOrder();
        }

        private void CreateChildProdSuccess(TaskObject task, Product product)
        {
            SuccessObject success = new SuccessObject {
                Id = Guid.NewGuid().ToString(),
                TaskId = task.Id,
                TaskName = task.Name,
                Time = new DateTime?(DateTime.Now),
                Parent = this._task.Name,
                AtcHidden = 0,
                IsMultiCart = true,
                Store = EveAIO.Helpers.GetStoreUrl(this._task)
            };
            this.CHILD_SUCCESSES.Add(success);
            success.ProductName = product.ProductTitle;
            success.Size = product.SuprimetMultiPickedSize.Value.Key;
            if (string.IsNullOrEmpty(success.Size))
            {
                success.Size = "-";
            }
            success.Price = product.Price;
            success.Link = product.Link;
            success.ProductImage = task.ImgUrl;
            success.Proxy = "-";
            if (!string.IsNullOrEmpty(this._task.Proxy))
            {
                success.Proxy = "-";
            }
            else
            {
                success.Proxy = this._task.Proxy;
            }
            Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                // This item is obfuscated and can not be translated.
                if (!Global.SUCCESS.Any<SuccessObject>(x => (x.TaskId == task.Id)))
                {
                    goto Label_00BC;
                }
            Label_002E:;
                SuccessObject obj2 = Global.SUCCESS.First<SuccessObject>(x => x.TaskId == task.Id);
            Label_00BC:
                switch ((-1400859372 ^ -538950493) % 5)
                {
                    case 0:
                    {
                        success.Repetitions = obj2.Repetitions + 1;
                        int index = Global.SUCCESS.IndexOf(Global.SUCCESS.First<SuccessObject>(x => x.TaskId == task.Id));
                        Global.SUCCESS[index] = success;
                        goto Label_00BC;
                    }
                    case 1:
                        success.Repetitions = 1;
                        Global.SUCCESS.Add(success);
                        return;

                    case 2:
                        goto Label_00BC;

                    case 4:
                        goto Label_002E;
                }
            }, Array.Empty<object>());
        }

        public bool DirectLink(string link)
        {
            this.Init();
            States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", link);
            if (link.ToLowerInvariant().Contains("/checkouts/") || this._task.IsShopifyCheckoutLink)
            {
                return true;
            }
            if ((this._task.ShopifyWebsite != "yeezy") && !this._task.Link.Contains("yeezysupply"))
            {
                return this.DirectLinkGeneral(this._task.Link, false);
            }
            return this.DirectLinkYeezy(this._task.Link, false);
        }

        private bool DirectLinkGeneral(string link, bool isSearch = false)
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                string url = link.Replace("#product", "");
                if (url.Contains("?"))
                {
                    url = url.Substring(0, url.IndexOf("?"));
                }
                string str3 = url;
                string str2 = "";
                if (!this._task.Link.Contains("eflash"))
                {
                    str2 = url + ".js";
                }
                else
                {
                    str2 = url;
                }
                try
                {
                    this._srr = this._client.Get(str2).Text();
                }
                catch (Exception exception1)
                {
                    if (!exception1.Message.Contains("404") || !this._task.HomeUrl.Contains("kith"))
                    {
                        throw;
                    }
                    HttpResponseMessage message = this._client.Get(str3);
                    this._srr = this._client.Get("https://kith.com" + message.Headers.GetValues("Location").First<string>() + ".js").Text();
                }
                if (this._task.Link.Contains("eflash"))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    if (!this._currentDoc.DocumentNode.Descendants("script").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"))))
                    {
                        if (!isSearch)
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        }
                        return false;
                    }
                    this._srr = this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"))).InnerText;
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                Product product = new Product();
                if (<>o__40.<>p__2 == null)
                {
                    <>o__40.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                }
                if (<>o__40.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                }
                if (<>o__40.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                }
                product.ProductTitle = <>o__40.<>p__2.Target(<>o__40.<>p__2, <>o__40.<>p__1.Target(<>o__40.<>p__1, <>o__40.<>p__0.Target(<>o__40.<>p__0, this._dynObj, "title")));
                product.Link = link;
                this._runner.Product = product;
                if (this._task.HomeUrl.ToLowerInvariant().Contains("bowsandarrowsberkeley"))
                {
                    if (<>o__40.<>p__5 == null)
                    {
                        <>o__40.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__40.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__5.Target(<>o__40.<>p__5, <>o__40.<>p__4.Target(<>o__40.<>p__4, <>o__40.<>p__3.Target(<>o__40.<>p__3, this._dynObj, "options"))).ToLowerInvariant().Contains("IN-STORE ONLY".ToLowerInvariant()))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.IN_STORE_ONLY, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.IN_STORE_ONLY);
                        return false;
                    }
                }
                if (<>o__40.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                }
                if (<>o__40.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__8 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                }
                if (<>o__40.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                }
                if (<>o__40.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__40.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                }
                if (<>o__40.<>p__9.Target(<>o__40.<>p__9, <>o__40.<>p__8.Target(<>o__40.<>p__8, <>o__40.<>p__7.Target(<>o__40.<>p__7, <>o__40.<>p__6.Target(<>o__40.<>p__6, this._dynObj, "available")), true)))
                {
                    if (!isSearch)
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    }
                    return false;
                }
                if (<>o__40.<>p__32 == null)
                {
                    <>o__40.<>p__32 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                }
                if (<>o__40.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__40.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify2), argumentInfo));
                }
                foreach (object obj2 in <>o__40.<>p__32.Target(<>o__40.<>p__32, <>o__40.<>p__10.Target(<>o__40.<>p__10, this._dynObj)))
                {
                    if (<>o__40.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__13 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    if (!<>o__40.<>p__14.Target(<>o__40.<>p__14, <>o__40.<>p__13.Target(<>o__40.<>p__13, <>o__40.<>p__12.Target(<>o__40.<>p__12, <>o__40.<>p__11.Target(<>o__40.<>p__11, obj2, "available")), true)))
                    {
                        bool flag2;
                        if (<>o__40.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__40.<>p__19 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                        }
                        if (flag2 = this._task.Link.Contains("shophny.com") || (this._task.ShopifyWebsite == "shophny.com"))
                        {
                            if (<>o__40.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__18 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                <>o__40.<>p__17 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                        }
                        if (!<>o__40.<>p__19.Target(<>o__40.<>p__19, (<>o__40.<>p__15 != null) ? flag2 : <>o__40.<>p__18.Target(<>o__40.<>p__18, flag2, <>o__40.<>p__17.Target(<>o__40.<>p__17, <>o__40.<>p__16.Target(<>o__40.<>p__16, <>o__40.<>p__15.Target(<>o__40.<>p__15, obj2, "inventory_management")), null))))
                        {
                            if (<>o__40.<>p__23 == null)
                            {
                                <>o__40.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__40.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__40.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            this._runner.Product.Price = <>o__40.<>p__23.Target(<>o__40.<>p__23, <>o__40.<>p__22.Target(<>o__40.<>p__22, <>o__40.<>p__21.Target(<>o__40.<>p__21, <>o__40.<>p__20.Target(<>o__40.<>p__20, obj2, "price"))));
                            this._runner.Product.Price = this._runner.Product.Price.Insert(this._runner.Product.Price.Length - 2, ".");
                            if (<>o__40.<>p__31 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__31 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__27 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__24 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__40.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__30 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__29 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__40.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__40.<>p__28 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__40.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            KeyValuePair<string, string> item = <>o__40.<>p__31.Target(<>o__40.<>p__31, typeof(KeyValuePair<string, string>), <>o__40.<>p__27.Target(<>o__40.<>p__27, <>o__40.<>p__26.Target(<>o__40.<>p__26, <>o__40.<>p__25.Target(<>o__40.<>p__25, <>o__40.<>p__24.Target(<>o__40.<>p__24, obj2, "title")))), <>o__40.<>p__30.Target(<>o__40.<>p__30, <>o__40.<>p__29.Target(<>o__40.<>p__29, <>o__40.<>p__28.Target(<>o__40.<>p__28, obj2, "id"))));
                            this._runner.Product.AvailableSizes.Add(item);
                        }
                    }
                }
                if (this._runner.Product.AvailableSizes.Count == 0)
                {
                    if (!isSearch)
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
                        if (!isSearch)
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        }
                        return false;
                    }
                }
                if (this._task.ShopifyWebsite != "other")
                {
                    ShopifyCommon.UpdatePredefinedSizing((Product) this._runner, this._task);
                }
                if (this._runner.Product.AvailableSizes.Count != 0)
                {
                    if (<>o__40.<>p__37 == null)
                    {
                        <>o__40.<>p__37 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__40.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__36 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__40.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__40.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__40.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    this._task.ImgUrl = <>o__40.<>p__37.Target(<>o__40.<>p__37, <>o__40.<>p__36.Target(<>o__40.<>p__36, "http:", <>o__40.<>p__35.Target(<>o__40.<>p__35, <>o__40.<>p__34.Target(<>o__40.<>p__34, <>o__40.<>p__33.Target(<>o__40.<>p__33, this._dynObj, "featured_image")))));
                    if (this._task.PriceCheck && (!this._task.SmartCheckout || this._runner.IsSmartCheckoutReady))
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
                    if (this._task.HomeUrl.ToLowerInvariant().Contains("kith"))
                    {
                        try
                        {
                            this._srr = this._client.Get(url).Text();
                            string key = this._srr.Substring(this._srr.IndexOf("htikb"));
                            key = key.Substring(key.IndexOf("'") + 1);
                            key = key.Substring(0, key.IndexOf("'"));
                            string str6 = this._srr.Substring(this._srr.IndexOf("htikc"));
                            str6 = str6.Substring(str6.IndexOf("'") + 1);
                            str6 = str6.Substring(0, str6.IndexOf("'"));
                            WebsitesInfo.KITH_PROPERTIES = new KeyValuePair<string, string>(key, str6);
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
                            string str8 = this._srr.Substring(this._srr.IndexOf("atob") + 2);
                            str8 = str8.Substring(str8.IndexOf("atob") + 2);
                            str8 = str8.Substring(str8.IndexOf("'") + 1);
                            string s = str8.Substring(0, str8.IndexOf("'"));
                            str8 = str8.Substring(str8.IndexOf("atob") + 2);
                            str8 = str8.Substring(str8.IndexOf("'") + 1);
                            string str10 = str8.Substring(0, str8.IndexOf("'"));
                            byte[] bytes = Convert.FromBase64String(s);
                            s = Encoding.UTF8.GetString(bytes);
                            s = s.Substring(s.IndexOf("[") + 1);
                            s = s.Substring(0, s.IndexOf("]"));
                            int num4 = str10.Length % 4;
                            if (num4 > 0)
                            {
                                str10 = str10 + new string('=', 4 - num4);
                            }
                            bytes = Convert.FromBase64String(str10);
                            str10 = Encoding.UTF8.GetString(bytes);
                            WebsitesInfo.EFLESH_US_PROPERTIES = new KeyValuePair<string, string>(s, str10);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (this._task.HomeUrl.ToLowerInvariant().Contains("eflash.") && (!_eflashUkGetter.HasValue || (_eflashUkGetter.Value.AddHours(1.0) < DateTime.Now)))
                    {
                        try
                        {
                            this._srr = this._client.Get(url).Text();
                            HtmlDocument document = new HtmlDocument();
                            document.LoadHtml(this._srr);
                            string str11 = "https:" + document.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["src"] != null) && x.Attributes["src"].Value.Contains("custom.js"))).Attributes["src"].Value;
                            this._srr = this._client.Get(str11).Text();
                            string str12 = this._srr.Substring(this._srr.IndexOf("form.product-form"));
                            str12 = str12.Substring(str12.IndexOf("value"));
                            str12 = str12.Substring(str12.IndexOf("\"") + 1);
                            str12 = str12.Substring(0, str12.IndexOf("\""));
                            string str13 = this._srr.Substring(this._srr.IndexOf("properties"));
                            str13 = str13.Substring(str13.IndexOf("[") + 1);
                            WebsitesInfo.EFLESH_UK_PROPERTIES = new KeyValuePair<string, string>(str13.Substring(0, str13.IndexOf("]")), str12);
                            _eflashUkGetter = new DateTime?(DateTime.Now);
                        }
                        catch
                        {
                        }
                    }
                    if (!this._task.RandomSize && (!this._task.SmartCheckout || (this._task.SmartCheckout && this._runner.IsSmartCheckoutReady)))
                    {
                        char[] separator = new char[] { '#' };
                        string[] strArray2 = this._task.Size.Split(separator);
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            strArray2[i] = strArray2[i].Trim().ToUpperInvariant();
                        }
                        foreach (string str14 in strArray2)
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
                                    if (current.Key.Trim().ToUpperInvariant() == str14.ToUpperInvariant())
                                    {
                                        goto Label_1783;
                                    }
                                }
                                continue;
                            Label_1783:
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
                if (!isSearch)
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
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
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

        public Product DirectLinkMulti(TaskObject task, string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", link);
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                string str = link.Replace("#product", "");
                if (str.Contains("?"))
                {
                    str = str.Substring(0, str.IndexOf("?"));
                }
                string url = "";
                if (task.Link.Contains("eflash"))
                {
                    url = str;
                }
                else
                {
                    url = str + ".js";
                }
                this._srr = this._client.Get(url).Text();
                if (task.Link.Contains("eflash"))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    if (this._currentDoc.DocumentNode.Descendants("script").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template")))
                    {
                        this._srr = this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"))).InnerText;
                    }
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                Product product = new Product();
                if (<>o__31.<>p__2 == null)
                {
                    <>o__31.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                }
                if (<>o__31.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__31.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                }
                if (<>o__31.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__31.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                }
                product.ProductTitle = <>o__31.<>p__2.Target(<>o__31.<>p__2, <>o__31.<>p__1.Target(<>o__31.<>p__1, <>o__31.<>p__0.Target(<>o__31.<>p__0, this._dynObj, "title")));
                product.Link = link;
                Product product2 = product;
                if (this._task.HomeUrl.ToLowerInvariant().Contains("bowsandarrowsberkeley"))
                {
                    if (<>o__31.<>p__5 == null)
                    {
                        <>o__31.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__31.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__31.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__31.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__31.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__31.<>p__5.Target(<>o__31.<>p__5, <>o__31.<>p__4.Target(<>o__31.<>p__4, <>o__31.<>p__3.Target(<>o__31.<>p__3, this._dynObj, "options"))).ToLowerInvariant().Contains("IN-STORE ONLY".ToLowerInvariant()))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.IN_STORE_ONLY, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.IN_STORE_ONLY);
                        return null;
                    }
                }
                if (<>o__31.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__31.<>p__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                }
                if (<>o__31.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__31.<>p__8 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                }
                if (<>o__31.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__31.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                }
                if (<>o__31.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__31.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                }
                if (!<>o__31.<>p__9.Target(<>o__31.<>p__9, <>o__31.<>p__8.Target(<>o__31.<>p__8, <>o__31.<>p__7.Target(<>o__31.<>p__7, <>o__31.<>p__6.Target(<>o__31.<>p__6, this._dynObj, "available")), true)))
                {
                    if (<>o__31.<>p__32 == null)
                    {
                        <>o__31.<>p__32 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                    }
                    if (<>o__31.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__31.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify2), argumentInfo));
                    }
                    foreach (object obj2 in <>o__31.<>p__32.Target(<>o__31.<>p__32, <>o__31.<>p__10.Target(<>o__31.<>p__10, this._dynObj)))
                    {
                        if (<>o__31.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__31.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__31.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__31.<>p__13 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__31.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__31.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__31.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__31.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (!<>o__31.<>p__14.Target(<>o__31.<>p__14, <>o__31.<>p__13.Target(<>o__31.<>p__13, <>o__31.<>p__12.Target(<>o__31.<>p__12, <>o__31.<>p__11.Target(<>o__31.<>p__11, obj2, "available")), true)))
                        {
                            bool flag;
                            if (<>o__31.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__31.<>p__19 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                            }
                            if (flag = task.Link.Contains("shophny.com") || (task.ShopifyWebsite == "shophny.com"))
                            {
                                if (<>o__31.<>p__18 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__18 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__17 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                    <>o__31.<>p__17 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__16 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                            }
                            if (!<>o__31.<>p__19.Target(<>o__31.<>p__19, (<>o__31.<>p__15 != null) ? flag : <>o__31.<>p__18.Target(<>o__31.<>p__18, flag, <>o__31.<>p__17.Target(<>o__31.<>p__17, <>o__31.<>p__16.Target(<>o__31.<>p__16, <>o__31.<>p__15.Target(<>o__31.<>p__15, obj2, "inventory_management")), null))))
                            {
                                if (<>o__31.<>p__23 == null)
                                {
                                    <>o__31.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                }
                                if (<>o__31.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__21 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__20 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__31.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                product2.Price = <>o__31.<>p__23.Target(<>o__31.<>p__23, <>o__31.<>p__22.Target(<>o__31.<>p__22, <>o__31.<>p__21.Target(<>o__31.<>p__21, <>o__31.<>p__20.Target(<>o__31.<>p__20, obj2, "price"))));
                                product2.Price = product2.Price.Insert(product2.Price.Length - 2, ".");
                                if (<>o__31.<>p__31 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__31 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__27 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__26 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__25 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__24 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__31.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__30 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__29 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__31.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__31.<>p__28 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__31.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                KeyValuePair<string, string> item = <>o__31.<>p__31.Target(<>o__31.<>p__31, typeof(KeyValuePair<string, string>), <>o__31.<>p__27.Target(<>o__31.<>p__27, <>o__31.<>p__26.Target(<>o__31.<>p__26, <>o__31.<>p__25.Target(<>o__31.<>p__25, <>o__31.<>p__24.Target(<>o__31.<>p__24, obj2, "title")))), <>o__31.<>p__30.Target(<>o__31.<>p__30, <>o__31.<>p__29.Target(<>o__31.<>p__29, <>o__31.<>p__28.Target(<>o__31.<>p__28, obj2, "id"))));
                                product2.AvailableSizes.Add(item);
                            }
                        }
                    }
                    if (product2.AvailableSizes.Count == 0)
                    {
                        return null;
                    }
                    if (task.ShopifyWebsite != "other")
                    {
                        ShopifyCommon.UpdatePredefinedSizing((Product) this._runner, task);
                    }
                    if (product2.AvailableSizes.Count == 0)
                    {
                        return null;
                    }
                    if (<>o__31.<>p__37 == null)
                    {
                        <>o__31.<>p__37 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__31.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__31.<>p__36 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__31.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__31.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__31.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__31.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__31.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__31.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    task.ImgUrl = <>o__31.<>p__37.Target(<>o__31.<>p__37, <>o__31.<>p__36.Target(<>o__31.<>p__36, "http:", <>o__31.<>p__35.Target(<>o__31.<>p__35, <>o__31.<>p__34.Target(<>o__31.<>p__34, <>o__31.<>p__33.Target(<>o__31.<>p__33, this._dynObj, "featured_image")))));
                    if (task.PriceCheck && (!task.SmartCheckout || this._runner.IsSmartCheckoutReady))
                    {
                        string str4 = "";
                        foreach (char ch in this._runner.Product.Price)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str4 = str4 + ch.ToString();
                            }
                        }
                        double num4 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num4 < task.MinimumPrice) || (num4 > task.MaximumPrice))
                        {
                            States.WriteLogger(task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                            this._runner.Product = null;
                            return null;
                        }
                        States.WriteLogger(task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                    }
                    KeyValuePair<string, string>? nullable = null;
                    if (!task.RandomSize && (!task.SmartCheckout || (task.SmartCheckout && this._runner.IsSmartCheckoutReady)))
                    {
                        char[] separator = new char[] { '#' };
                        string[] strArray = task.Size.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (string str3 in strArray)
                        {
                            if (nullable.HasValue)
                            {
                                break;
                            }
                            using (List<KeyValuePair<string, string>>.Enumerator enumerator2 = product2.AvailableSizes.GetEnumerator())
                            {
                                KeyValuePair<string, string> pair2;
                                goto Label_103D;
                            Label_1015:
                                pair2 = enumerator2.Current;
                                if (pair2.Key.Trim().ToUpperInvariant() == str3.ToUpperInvariant())
                                {
                                    goto Label_1048;
                                }
                            Label_103D:
                                if (!enumerator2.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_1015;
                            Label_1048:
                                nullable = new KeyValuePair<string, string>?(pair2);
                            }
                        }
                        if (!nullable.HasValue)
                        {
                            if (!task.SizePickRandom)
                            {
                                return null;
                            }
                        }
                        else
                        {
                            product2.SuprimetMultiPickedSize = nullable;
                            return product2;
                        }
                    }
                    nullable = new KeyValuePair<string, string>?(product2.AvailableSizes[this._runner.Rnd.Next(0, product2.AvailableSizes.Count)]);
                    product2.SuprimetMultiPickedSize = nullable;
                    return product2;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                return null;
            }
            catch (ThreadAbortException)
            {
                return null;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
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
                return null;
            }
        }

        private bool DirectLinkYeezy(string link, bool isSource = false)
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                if (this._task.SmartCheckout && !this._runner.IsSmartCheckoutReady)
                {
                    link = this._task.DummyProduct;
                }
                this._srr = this._client.Get(link).Text();
                this._currentDoc.LoadHtml(this._srr);
                string innerText = this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-product-json"))).InnerText;
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(innerText);
                Product product = new Product();
                if (<>o__41.<>p__2 == null)
                {
                    <>o__41.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                }
                if (<>o__41.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__41.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                }
                if (<>o__41.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__41.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                }
                product.ProductTitle = <>o__41.<>p__2.Target(<>o__41.<>p__2, <>o__41.<>p__1.Target(<>o__41.<>p__1, <>o__41.<>p__0.Target(<>o__41.<>p__0, this._dynObj, "title")));
                product.Link = link;
                this._runner.Product = product;
                if (<>o__41.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__41.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                }
                if (<>o__41.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__41.<>p__5 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                }
                if (<>o__41.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__41.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                }
                if (<>o__41.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__41.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                }
                if (!<>o__41.<>p__6.Target(<>o__41.<>p__6, <>o__41.<>p__5.Target(<>o__41.<>p__5, <>o__41.<>p__4.Target(<>o__41.<>p__4, <>o__41.<>p__3.Target(<>o__41.<>p__3, this._dynObj, "available")), true)))
                {
                    if (<>o__41.<>p__29 == null)
                    {
                        <>o__41.<>p__29 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                    }
                    if (<>o__41.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__41.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "variants", typeof(Shopify2), argumentInfo));
                    }
                    foreach (object obj2 in <>o__41.<>p__29.Target(<>o__41.<>p__29, <>o__41.<>p__7.Target(<>o__41.<>p__7, this._dynObj)))
                    {
                        if (<>o__41.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__41.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__41.<>p__10 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__41.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__41.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__41.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__41.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (!<>o__41.<>p__11.Target(<>o__41.<>p__11, <>o__41.<>p__10.Target(<>o__41.<>p__10, <>o__41.<>p__9.Target(<>o__41.<>p__9, <>o__41.<>p__8.Target(<>o__41.<>p__8, obj2, "available")), true)))
                        {
                            bool flag2;
                            if (<>o__41.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__41.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                            }
                            if (flag2 = this._task.Link.Contains("shophny.com") || (this._task.ShopifyWebsite == "shophny.com"))
                            {
                                if (<>o__41.<>p__15 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__15 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__14 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                    <>o__41.<>p__14 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__13 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                            }
                            if (!<>o__41.<>p__16.Target(<>o__41.<>p__16, (<>o__41.<>p__12 != null) ? flag2 : <>o__41.<>p__15.Target(<>o__41.<>p__15, flag2, <>o__41.<>p__14.Target(<>o__41.<>p__14, <>o__41.<>p__13.Target(<>o__41.<>p__13, <>o__41.<>p__12.Target(<>o__41.<>p__12, obj2, "inventory_management")), null))))
                            {
                                if (<>o__41.<>p__20 == null)
                                {
                                    <>o__41.<>p__20 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                }
                                if (<>o__41.<>p__19 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__18 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__17 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                this._runner.Product.Price = <>o__41.<>p__20.Target(<>o__41.<>p__20, <>o__41.<>p__19.Target(<>o__41.<>p__19, <>o__41.<>p__18.Target(<>o__41.<>p__18, <>o__41.<>p__17.Target(<>o__41.<>p__17, obj2, "price"))));
                                this._runner.Product.Price = this._runner.Product.Price.Insert(this._runner.Product.Price.Length - 2, ".");
                                if (<>o__41.<>p__28 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__28 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__24 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__23 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__21 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__27 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__26 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__41.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__41.<>p__25 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__41.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                KeyValuePair<string, string> item = <>o__41.<>p__28.Target(<>o__41.<>p__28, typeof(KeyValuePair<string, string>), <>o__41.<>p__24.Target(<>o__41.<>p__24, <>o__41.<>p__23.Target(<>o__41.<>p__23, <>o__41.<>p__22.Target(<>o__41.<>p__22, <>o__41.<>p__21.Target(<>o__41.<>p__21, obj2, "option1")))), <>o__41.<>p__27.Target(<>o__41.<>p__27, <>o__41.<>p__26.Target(<>o__41.<>p__26, <>o__41.<>p__25.Target(<>o__41.<>p__25, obj2, "id"))));
                                this._runner.Product.AvailableSizes.Add(item);
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
                    this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "twitter:image"))).Attributes["content"].Value;
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
                        int num4 = int.Parse(str2.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                                        goto Label_0E20;
                                    }
                                }
                                continue;
                            Label_0E20:
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
                if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
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

        private bool ExtractApiKey()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.EXTRACTING_API_KEY);
                States.WriteLogger(this._task, States.LOGGER_STATES.EXTRACTING_API_KEY, null, "", "");
                this.SetClientTmp();
                HttpResponseMessage message = this._clientTmp.Get(this._task.HomeUrl);
                this._srr = message.Content.ReadAsStringAsync().Result.ToString();
                this._currentDoc.LoadHtml(this._srr);
                if (this._srr.Contains("You are being "))
                {
                    message = this._clientTmp.Get(this._currentDoc.DocumentNode.Descendants("a").First<HtmlNode>().Attributes["href"].Value);
                    this._srr = message.Content.ReadAsStringAsync().Result.ToString();
                    this._currentDoc.LoadHtml(this._srr);
                }
                string str = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopify-checkout-api-token"))).Attributes["content"].Value;
                ShopifyWebsiteInfo item = new ShopifyWebsiteInfo {
                    ApiToken = str,
                    HomeLink = this._task.HomeUrl,
                    SearchLink = this._task.HomeUrl + "/sitemap_products_1.xml"
                };
                WebsitesInfo.SHOPIFY_WEBS.Add(item);
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_API_EXTRACTION);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_API_EXTRACTION, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
                    if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_API_EXTRACTION);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_API_EXTRACTION, exception, "", "");
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

        private bool GetShippingRates()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.CALCULATING_SHIPPING, null, "", "");
                this._srr = "{}";
                while (this._srr == "{}")
                {
                    try
                    {
                        this._srr = this._client.Get(this._runner.HomeUrl + $"/{this._endpoint}/checkouts/" + this._checkoutToken + "/shipping_rates.json").Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (this._srr.Contains("is not supported"))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.COUNTRY_NOT_SUPPORTED, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                            return false;
                        }
                        if (!exception.Message.Contains("412"))
                        {
                            throw;
                        }
                        return true;
                    }
                }
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                double num2 = 0.0;
                string str = "";
                if (<>o__38.<>p__46 == null)
                {
                    <>o__38.<>p__46 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                }
                if (<>o__38.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__38.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_rates", typeof(Shopify2), argumentInfo));
                }
                foreach (object obj3 in <>o__38.<>p__46.Target(<>o__38.<>p__46, <>o__38.<>p__0.Target(<>o__38.<>p__0, obj2)))
                {
                    if (<>o__38.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__38.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__38.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__38.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__38.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    object obj4 = <>o__38.<>p__4.Target(<>o__38.<>p__4, <>o__38.<>p__3.Target(<>o__38.<>p__3, <>o__38.<>p__2.Target(<>o__38.<>p__2, <>o__38.<>p__1.Target(<>o__38.<>p__1, obj3, "id"))), "In%20Shop%20Pick%20Up");
                    if (<>o__38.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__38.<>p__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                    }
                    if (!<>o__38.<>p__11.Target(<>o__38.<>p__11, obj4))
                    {
                        if (<>o__38.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__10 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__38.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__9 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__38.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__38.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__38.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__38.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__38.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__38.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (!<>o__38.<>p__10.Target(<>o__38.<>p__10, <>o__38.<>p__9.Target(<>o__38.<>p__9, obj4, <>o__38.<>p__8.Target(<>o__38.<>p__8, <>o__38.<>p__7.Target(<>o__38.<>p__7, <>o__38.<>p__6.Target(<>o__38.<>p__6, <>o__38.<>p__5.Target(<>o__38.<>p__5, obj3, "id"))), "In%20Store%20Pick"))))
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (<>o__38.<>p__28 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__28 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__27 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__26 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__25 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                object obj5 = <>o__38.<>p__28.Target(<>o__38.<>p__28, typeof(double), <>o__38.<>p__27.Target(<>o__38.<>p__27, <>o__38.<>p__26.Target(<>o__38.<>p__26, <>o__38.<>p__25.Target(<>o__38.<>p__25, obj3, "price"))), CultureInfo.InvariantCulture);
                                if (<>o__38.<>p__29 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__29 = CallSite<Func<CallSite, object, double, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Shopify2), argumentInfo));
                                }
                                obj4 = <>o__38.<>p__29.Target(<>o__38.<>p__29, obj5, num2);
                                if (<>o__38.<>p__32 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__32 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                                }
                                if (!<>o__38.<>p__32.Target(<>o__38.<>p__32, obj4))
                                {
                                    if (<>o__38.<>p__31 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__38.<>p__31 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                                    }
                                    if (<>o__38.<>p__30 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                        <>o__38.<>p__30 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Shopify2), argumentInfo));
                                    }
                                    if (!<>o__38.<>p__31.Target(<>o__38.<>p__31, <>o__38.<>p__30.Target(<>o__38.<>p__30, obj4, str.Contains("In%20Shop%20Pick%20Up"))))
                                    {
                                        continue;
                                    }
                                }
                                if (<>o__38.<>p__36 == null)
                                {
                                    <>o__38.<>p__36 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                }
                                if (<>o__38.<>p__35 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__34 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__33 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                str = <>o__38.<>p__36.Target(<>o__38.<>p__36, <>o__38.<>p__35.Target(<>o__38.<>p__35, <>o__38.<>p__34.Target(<>o__38.<>p__34, <>o__38.<>p__33.Target(<>o__38.<>p__33, obj3, "id"))));
                                if (<>o__38.<>p__41 == null)
                                {
                                    <>o__38.<>p__41 = CallSite<Func<CallSite, object, double>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify2)));
                                }
                                if (<>o__38.<>p__40 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__40 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__39 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__38 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__37 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                num2 = <>o__38.<>p__41.Target(<>o__38.<>p__41, <>o__38.<>p__40.Target(<>o__38.<>p__40, typeof(double), <>o__38.<>p__39.Target(<>o__38.<>p__39, <>o__38.<>p__38.Target(<>o__38.<>p__38, <>o__38.<>p__37.Target(<>o__38.<>p__37, obj3, "price"))), CultureInfo.InvariantCulture));
                                if (<>o__38.<>p__45 == null)
                                {
                                    <>o__38.<>p__45 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                }
                                if (<>o__38.<>p__44 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__43 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__42 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__42 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                <>o__38.<>p__45.Target(<>o__38.<>p__45, <>o__38.<>p__44.Target(<>o__38.<>p__44, <>o__38.<>p__43.Target(<>o__38.<>p__43, <>o__38.<>p__42.Target(<>o__38.<>p__42, obj3, "price"))));
                            }
                            else
                            {
                                if (<>o__38.<>p__15 == null)
                                {
                                    <>o__38.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                }
                                if (<>o__38.<>p__14 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__13 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__12 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                str = <>o__38.<>p__15.Target(<>o__38.<>p__15, <>o__38.<>p__14.Target(<>o__38.<>p__14, <>o__38.<>p__13.Target(<>o__38.<>p__13, <>o__38.<>p__12.Target(<>o__38.<>p__12, obj3, "id"))));
                                if (<>o__38.<>p__20 == null)
                                {
                                    <>o__38.<>p__20 = CallSite<Func<CallSite, object, double>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Shopify2)));
                                }
                                if (<>o__38.<>p__19 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__19 = CallSite<Func<CallSite, Type, object, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__18 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__17 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__16 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                num2 = <>o__38.<>p__20.Target(<>o__38.<>p__20, <>o__38.<>p__19.Target(<>o__38.<>p__19, typeof(double), <>o__38.<>p__18.Target(<>o__38.<>p__18, <>o__38.<>p__17.Target(<>o__38.<>p__17, <>o__38.<>p__16.Target(<>o__38.<>p__16, obj3, "price"))), CultureInfo.InvariantCulture));
                                if (<>o__38.<>p__24 == null)
                                {
                                    <>o__38.<>p__24 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                                }
                                if (<>o__38.<>p__23 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__38.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__38.<>p__21 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__38.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                <>o__38.<>p__24.Target(<>o__38.<>p__24, <>o__38.<>p__23.Target(<>o__38.<>p__23, <>o__38.<>p__22.Target(<>o__38.<>p__22, <>o__38.<>p__21.Target(<>o__38.<>p__21, obj3, "price"))));
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.COUNTRY_NOT_SUPPORTED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                    return false;
                }
                this._shipping = str;
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
                    string str2 = "";
                    if (exception2 is WebException)
                    {
                        str2 = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    if (string.IsNullOrEmpty(str2))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CALCULATING_SHIPPING, exception2, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CALCULATING_SHIPPING, exception2, "", str2);
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        private void Init()
        {
            this._isInitError = false;
            Task.Factory.StartNew(delegate {
                try
                {
                    string str = "";
                    if (!this._task.IsShopifyCheckoutLink && (!this._runner._tokenTimestamp.HasValue || (this._runner._tokenTimestamp.Value.AddHours(3.0) < DateTime.Now)))
                    {
                        HttpWebRequest request;
                        byte[] bytes;
                        this._runner.Cookies = new CookieContainer();
                        this._checkoutToken = "";
                        this._runner._tokenTimestamp = null;
                        if (this._task.Login)
                        {
                            request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + "/account/login");
                            request.Method = "POST";
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.KeepAlive = true;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate");
                            request.Headers.Add("Upgrade-Insecure-Requests", "1");
                            request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                            request.Headers.Add("Authorization", "Basic " + this._apiToken);
                            request.AllowAutoRedirect = false;
                            request.CookieContainer = this._client.Cookies;
                            string str23 = "form_type=customer_login&utf8=%E2%9C%93";
                            str23 = (str23 + "&customer%5Bemail%5D=" + WebUtility.UrlEncode(this._task.Username)) + "&customer%5Bpassword%5D=" + this._task.Password;
                            bytes = Encoding.ASCII.GetBytes(str23);
                            request.ContentLength = bytes.Length;
                            using (Stream stream = request.GetRequestStream())
                            {
                                stream.Write(bytes, 0, bytes.Length);
                            }
                            str = "";
                            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                            {
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    str = reader.ReadToEnd();
                                }
                            }
                        }
                        request = (HttpWebRequest) WebRequest.Create(this._runner.HomeUrl + $"/{this._endpoint}/checkouts.json");
                        request.Method = "POST";
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.KeepAlive = true;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                        request.ContentType = "application/json; charset=utf-8";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate");
                        request.Headers.Add("Upgrade-Insecure-Requests", "1");
                        request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                        request.Headers.Add("Authorization", "Basic " + this._apiToken);
                        request.AllowAutoRedirect = false;
                        string str2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Email.Trim();
                        string str3 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).FirstName.Trim();
                        string str4 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).LastName.Trim();
                        string str5 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address1.Trim();
                        string str6 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address2.Trim();
                        string str7 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).City.Trim();
                        string str8 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Zip.Trim();
                        string str9 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).State.Trim();
                        string str10 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Phone.Trim();
                        string str11 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Country.Trim();
                        string str12 = "";
                        string str13 = "";
                        string str14 = "";
                        string str15 = "";
                        string str16 = "";
                        string str17 = "";
                        string str18 = "";
                        string str19 = "";
                        string str20 = "";
                        if (Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).SameBillingShipping)
                        {
                            str12 = str3;
                            str13 = str4;
                            str14 = str5;
                            str15 = str6;
                            str16 = str7;
                            str17 = str8;
                            str18 = str9;
                            str19 = str10;
                            str20 = str11;
                        }
                        else
                        {
                            Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).EmailShipping.Trim();
                            str12 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).FirstNameShipping.Trim();
                            str13 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).LastNameShipping.Trim();
                            str14 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address1Shipping.Trim();
                            str15 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Address2Shipping.Trim();
                            str16 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CityShipping.Trim();
                            str17 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).ZipShipping.Trim();
                            str18 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).StateShipping.Trim();
                            str19 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).PhoneShipping.Trim();
                            str20 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CountryShipping.Trim();
                        }
                        ShopifyCheckout checkout1 = new ShopifyCheckout {
                            checkout = { 
                                email = str2,
                                shipping_address = { 
                                    address1 = str14,
                                    address2 = str15,
                                    city = str16,
                                    country = str20,
                                    first_name = str12,
                                    last_name = str13,
                                    phone = str19,
                                    province = str18,
                                    state = str18,
                                    zip = str17
                                },
                                billing_address = { 
                                    address1 = str5,
                                    address2 = str6,
                                    city = str7,
                                    country = str11,
                                    first_name = str3,
                                    last_name = str4,
                                    phone = str10,
                                    province = str9,
                                    state = str9,
                                    zip = str8
                                }
                            }
                        };
                        string s = Newtonsoft.Json.JsonConvert.SerializeObject(checkout1);
                        bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream2 = request.GetRequestStream())
                        {
                            stream2.Write(bytes, 0, bytes.Length);
                        }
                        str = "";
                        string str22 = "";
                        using (HttpWebResponse response2 = (HttpWebResponse) request.GetResponse())
                        {
                            if (response2.Headers["Location"] != null)
                            {
                                str22 = response2.Headers["Location"];
                            }
                            using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                str = reader2.ReadToEnd();
                            }
                        }
                        if (!string.IsNullOrEmpty(str22))
                        {
                            str = "{}";
                            this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                            States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_IN_QUEUE, null, "", "");
                            this._queueUp = true;
                            if (((this._task.HomeUrl.ToLowerInvariant().Contains("kith") || this._task.HomeUrl.ToLowerInvariant().Contains("eflash")) || this._task.HomeUrl.ToLowerInvariant().Contains("yeezysu")) && str22.Contains("api/checkouts"))
                            {
                                str22 = str22.Replace("api", "wallets");
                            }
                            while (str == "{}")
                            {
                                if (((this._task.HomeUrl.ToLowerInvariant().Contains("kith") || this._task.HomeUrl.ToLowerInvariant().Contains("eflash")) || this._task.HomeUrl.ToLowerInvariant().Contains("yeezysu")) && str22.Contains("api/checkouts"))
                                {
                                    str22 = str22.Replace("api", "wallets");
                                }
                                request = (HttpWebRequest) WebRequest.Create(str22);
                                if (this._runner.Proxy != null)
                                {
                                    request.Proxy = this._runner.Proxy;
                                }
                                request.KeepAlive = true;
                                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                                request.Headers.Add("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
                                request.Headers.Add("Authorization", "Basic " + this._apiToken);
                                request.Referer = str22;
                                request.AllowAutoRedirect = false;
                                str = "";
                                using (HttpWebResponse response3 = (HttpWebResponse) request.GetResponse())
                                {
                                    using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                                    {
                                        str = reader3.ReadToEnd();
                                    }
                                }
                                if (str.Contains("{}"))
                                {
                                    Thread.Sleep(0x7d0);
                                }
                            }
                        }
                        object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                        this._queueUp = false;
                        if (<>o__29.<>p__3 == null)
                        {
                            <>o__29.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                        }
                        if (<>o__29.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "checkout", typeof(Shopify2), argumentInfo));
                        }
                        this._checkoutToken = <>o__29.<>p__3.Target(<>o__29.<>p__3, <>o__29.<>p__2.Target(<>o__29.<>p__2, <>o__29.<>p__1.Target(<>o__29.<>p__1, <>o__29.<>p__0.Target(<>o__29.<>p__0, obj2), "token")));
                        this._runner._tokenTimestamp = new DateTime?(DateTime.Now);
                    }
                    if (string.IsNullOrEmpty(this._cardId))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ACQUIRING_PAYMENT_TOKEN, null, "", "");
                        string str24 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).CCNumber.Trim().Replace(" ", "");
                        Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).NameOnCard.Trim();
                        string expiryMonth = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).ExpiryMonth;
                        string expiryYear = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).ExpiryYear;
                        string str27 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._task.CheckoutId)).Cvv.Trim();
                        object obj3 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__29.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__4 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                        }
                        <>o__29.<>p__4.Target(<>o__29.<>p__4, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__29.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "number", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                        }
                        <>o__29.<>p__6.Target(<>o__29.<>p__6, <>o__29.<>p__5.Target(<>o__29.<>p__5, obj3), str24);
                        if (!this._task.HomeUrl.Contains("yeezysupply"))
                        {
                            if (<>o__29.<>p__10 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__29.<>p__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "month", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__29.<>p__9 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__29.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                            }
                            <>o__29.<>p__10.Target(<>o__29.<>p__10, <>o__29.<>p__9.Target(<>o__29.<>p__9, obj3), int.Parse(expiryMonth.ToString()));
                        }
                        else
                        {
                            if (<>o__29.<>p__8 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__29.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "month", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__29.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__29.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                            }
                            <>o__29.<>p__8.Target(<>o__29.<>p__8, <>o__29.<>p__7.Target(<>o__29.<>p__7, obj3), expiryMonth.ToString());
                        }
                        if (<>o__29.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "year", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                        }
                        <>o__29.<>p__12.Target(<>o__29.<>p__12, <>o__29.<>p__11.Target(<>o__29.<>p__11, obj3), expiryYear);
                        if (<>o__29.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "verification_value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                        }
                        <>o__29.<>p__14.Target(<>o__29.<>p__14, <>o__29.<>p__13.Target(<>o__29.<>p__13, obj3), str27);
                        if (<>o__29.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "first_name", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                        }
                        <>o__29.<>p__16.Target(<>o__29.<>p__16, <>o__29.<>p__15.Target(<>o__29.<>p__15, obj3), this._runner.Profile.FirstName);
                        if (<>o__29.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "last_name", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "credit_card", typeof(Shopify2), argumentInfo));
                        }
                        <>o__29.<>p__18.Target(<>o__29.<>p__18, <>o__29.<>p__17.Target(<>o__29.<>p__17, obj3), this._runner.Profile.LastName);
                        if (<>o__29.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__20 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__19 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Shopify2), argumentInfo));
                        }
                        object obj4 = <>o__29.<>p__20.Target(<>o__29.<>p__20, this._client, "https://elb.deposit.shopifycs.com/sessions", <>o__29.<>p__19.Target(<>o__29.<>p__19, typeof(Newtonsoft.Json.JsonConvert), obj3));
                        if (<>o__29.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__21 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Shopify2), argumentInfo));
                        }
                        <>o__29.<>p__21.Target(<>o__29.<>p__21, typeof(EveAIO.Extensions), obj4);
                        if (<>o__29.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__26 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Shopify2), argumentInfo));
                        }
                        object obj5 = <>o__29.<>p__26.Target(<>o__29.<>p__26, typeof(Newtonsoft.Json.JsonConvert), <>o__29.<>p__25.Target(<>o__29.<>p__25, <>o__29.<>p__24.Target(<>o__29.<>p__24, <>o__29.<>p__23.Target(<>o__29.<>p__23, <>o__29.<>p__22.Target(<>o__29.<>p__22, obj4)))));
                        if (<>o__29.<>p__30 == null)
                        {
                            <>o__29.<>p__30 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                        }
                        if (<>o__29.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__29.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        this._cardId = <>o__29.<>p__30.Target(<>o__29.<>p__30, <>o__29.<>p__29.Target(<>o__29.<>p__29, <>o__29.<>p__28.Target(<>o__29.<>p__28, <>o__29.<>p__27.Target(<>o__29.<>p__27, obj5, "id"))));
                    }
                }
                catch (Exception exception)
                {
                    if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                    {
                        if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                        {
                            string str28 = "";
                            if ((exception is WebException) && (((WebException) exception).Response != null))
                            {
                                using (Stream stream3 = ((WebException) exception).Response.GetResponseStream())
                                {
                                    using (StreamReader reader4 = new StreamReader(stream3))
                                    {
                                        str28 = reader4.ReadToEnd();
                                    }
                                }
                            }
                            this._isInitError = true;
                            if (!string.IsNullOrEmpty(str28))
                            {
                                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_DURING_TASK_INIT, exception, "", str28);
                            }
                            else
                            {
                                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_DURING_TASK_INIT, exception, "", "");
                            }
                        }
                        else
                        {
                            this._isInitIpBan = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                        }
                    }
                }
            });
        }

        public bool Login() => 
            true;

        private bool Payment()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__37.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "complete", typeof(Shopify2), argumentInfo));
                }
                <>o__37.<>p__0.Target(<>o__37.<>p__0, obj2, "1");
                if (<>o__37.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "s", typeof(Shopify2), argumentInfo));
                }
                <>o__37.<>p__1.Target(<>o__37.<>p__1, obj2, this._cardId);
                if (!string.IsNullOrEmpty(this._shipping))
                {
                    if (<>o__37.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__2 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__37.<>p__2.Target(<>o__37.<>p__2, obj2, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__37.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__4 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shipping_rate", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__37.<>p__4.Target(<>o__37.<>p__4, <>o__37.<>p__3.Target(<>o__37.<>p__3, obj2), new Newtonsoft.Json.Linq.JObject());
                    if (<>o__37.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "id", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_rate", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__37.<>p__7.Target(<>o__37.<>p__7, <>o__37.<>p__6.Target(<>o__37.<>p__6, <>o__37.<>p__5.Target(<>o__37.<>p__5, obj2)), this._shipping);
                }
                if (!string.IsNullOrEmpty(this._note))
                {
                    if (<>o__37.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "note", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__37.<>p__9.Target(<>o__37.<>p__9, <>o__37.<>p__8.Target(<>o__37.<>p__8, obj2), this._note);
                }
                if (<>o__37.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__11 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PatchJson", null, typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__10 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Shopify2), argumentInfo));
                }
                object obj3 = <>o__37.<>p__11.Target(<>o__37.<>p__11, this._client, (string) this._webUrl, <>o__37.<>p__10.Target(<>o__37.<>p__10, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__37.<>p__16 == null)
                {
                    <>o__37.<>p__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                }
                if (<>o__37.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Shopify2), argumentInfo));
                }
                this._srr = <>o__37.<>p__16.Target(<>o__37.<>p__16, <>o__37.<>p__15.Target(<>o__37.<>p__15, <>o__37.<>p__14.Target(<>o__37.<>p__14, <>o__37.<>p__13.Target(<>o__37.<>p__13, <>o__37.<>p__12.Target(<>o__37.<>p__12, obj3)))));
                if (<>o__37.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__17 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Shopify2), argumentInfo));
                }
                <>o__37.<>p__17.Target(<>o__37.<>p__17, typeof(EveAIO.Extensions), obj3);
                if (!this._srr.Contains("The information you provided couldn't be verified. Please check your card details and try again."))
                {
                    goto Label_0A16;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "The information you provided couldn't be verified. Please check your card details and try again.");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                this._checkoutToken = "";
                this._cardId = "";
                this._runner._tokenTimestamp = null;
                return false;
            Label_06C1:
                obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__37.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__37.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "complete", typeof(Shopify2), argumentInfo));
                }
                <>o__37.<>p__18.Target(<>o__37.<>p__18, obj2, "1");
                if (<>o__37.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__20 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PatchJson", null, typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__19 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__19 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Shopify2), argumentInfo));
                }
                obj3 = <>o__37.<>p__20.Target(<>o__37.<>p__20, this._client, (string) this._webUrl, <>o__37.<>p__19.Target(<>o__37.<>p__19, typeof(Newtonsoft.Json.JsonConvert), obj2));
                if (<>o__37.<>p__25 == null)
                {
                    <>o__37.<>p__25 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                }
                if (<>o__37.<>p__24 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__23 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Shopify2), argumentInfo));
                }
                if (<>o__37.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Shopify2), argumentInfo));
                }
                this._srr = <>o__37.<>p__25.Target(<>o__37.<>p__25, <>o__37.<>p__24.Target(<>o__37.<>p__24, <>o__37.<>p__23.Target(<>o__37.<>p__23, <>o__37.<>p__22.Target(<>o__37.<>p__22, <>o__37.<>p__21.Target(<>o__37.<>p__21, obj3)))));
                if (<>o__37.<>p__26 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__37.<>p__26 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Shopify2), argumentInfo));
                }
                <>o__37.<>p__26.Target(<>o__37.<>p__26, typeof(EveAIO.Extensions), obj3);
                if (this._srr.Contains("Calculatin taxes"))
                {
                    Thread.Sleep(200);
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                }
            Label_0A16:
                if (this._srr.Contains("Calculating taxes"))
                {
                    goto Label_06C1;
                }
                if ((this._srr.Contains("You are being") && this._srr.Contains("/account/login")) && !this._srr.Contains("Captcha validation failed. Please try again"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.LOGIN_REQUIRED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_REQUIRED, null, "", "");
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                if (this._srr.Contains("Captcha validation failed. Please try again") || this._srr.Contains("recaptchaCallback"))
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
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.SHOPIFY_CAPTCHA_KEY, this._runner.HomeUrl, "Shopify");
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
                    obj2 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__37.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "complete", typeof(Shopify2), argumentInfo));
                    }
                    <>o__37.<>p__27.Target(<>o__37.<>p__27, obj2, "1");
                    if (<>o__37.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__37.<>p__28 = CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__37.<>p__28.Target(<>o__37.<>p__28, obj2, new Newtonsoft.Json.Linq.JProperty("g-recaptcha-response", waiter.Token));
                    if (!string.IsNullOrEmpty(this._note))
                    {
                        if (<>o__37.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__29 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                        }
                        <>o__37.<>p__29.Target(<>o__37.<>p__29, obj2, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__37.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__37.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "note", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__37.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__37.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                        }
                        <>o__37.<>p__31.Target(<>o__37.<>p__31, <>o__37.<>p__30.Target(<>o__37.<>p__30, obj2), this._note);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                    if (<>o__37.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__33 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PatchJson", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__32 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Shopify2), argumentInfo));
                    }
                    obj3 = <>o__37.<>p__33.Target(<>o__37.<>p__33, this._client, (string) this._webUrl, <>o__37.<>p__32.Target(<>o__37.<>p__32, typeof(Newtonsoft.Json.JsonConvert), obj2));
                    if (<>o__37.<>p__38 == null)
                    {
                        <>o__37.<>p__38 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__37.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__37.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Shopify2), argumentInfo));
                    }
                    this._srr = <>o__37.<>p__38.Target(<>o__37.<>p__38, <>o__37.<>p__37.Target(<>o__37.<>p__37, <>o__37.<>p__36.Target(<>o__37.<>p__36, <>o__37.<>p__35.Target(<>o__37.<>p__35, <>o__37.<>p__34.Target(<>o__37.<>p__34, obj3)))));
                    if (<>o__37.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__37.<>p__39 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__37.<>p__39.Target(<>o__37.<>p__39, typeof(EveAIO.Extensions), obj3);
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
                    string str = "";
                    if (exception is WebException)
                    {
                        str = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    if (!string.IsNullOrEmpty(str))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception, "", "");
                    }
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        private void ProcessDeepSearchLink(Product product)
        {
            this._srr = this._client.Get(product.Link).Text();
            string str3 = "";
            this._currentDoc.LoadHtml(this._srr);
            string shopifyWebsite = this._task.ShopifyWebsite;
            uint num2 = <PrivateImplementationDetails>.ComputeStringHash(shopifyWebsite);
            if (num2 > 0x7a949f40)
            {
                Product product2;
                if (num2 <= 0x9e4864ef)
                {
                    if (num2 > 0x94481229)
                    {
                        if (num2 > 0x95928b2a)
                        {
                            switch (num2)
                            {
                                case 0x98f09984:
                                    if (shopifyWebsite != "oipolloi")
                                    {
                                        goto Label_1866;
                                    }
                                    product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name"))).InnerText.Trim();
                                    goto Label_1880;

                                case 0x9e145f8b:
                                    if (shopifyWebsite != "deadstock")
                                    {
                                        goto Label_1866;
                                    }
                                    product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("title").First<HtmlNode>().InnerText.Trim();
                                    if (this._srr.Contains("data-product="))
                                    {
                                        string str5 = this._srr.Substring(this._srr.IndexOf("data-product="));
                                        str5 = str5.Substring(str5.IndexOf("{"));
                                        str5 = str5.Substring(0, str5.IndexOf("}\"") + 1).Replace("&quot;", "\"");
                                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str5);
                                        if (<>o__44.<>p__10 == null)
                                        {
                                            <>o__44.<>p__10 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                                        }
                                        if (<>o__44.<>p__5 == null)
                                        {
                                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                            <>o__44.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "tags", typeof(Shopify2), argumentInfo));
                                        }
                                        foreach (object obj4 in <>o__44.<>p__10.Target(<>o__44.<>p__10, <>o__44.<>p__5.Target(<>o__44.<>p__5, this._dynObj)))
                                        {
                                            product2 = product;
                                            if (<>o__44.<>p__9 == null)
                                            {
                                                <>o__44.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Shopify2)));
                                            }
                                            if (<>o__44.<>p__8 == null)
                                            {
                                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                                <>o__44.<>p__8 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Shopify2), argumentInfo));
                                            }
                                            if (<>o__44.<>p__7 == null)
                                            {
                                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                                <>o__44.<>p__7 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                                            }
                                            if (<>o__44.<>p__6 == null)
                                            {
                                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                                <>o__44.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                            }
                                            product2.ExtendedTitle = <>o__44.<>p__9.Target(<>o__44.<>p__9, <>o__44.<>p__8.Target(<>o__44.<>p__8, product2.ExtendedTitle, <>o__44.<>p__7.Target(<>o__44.<>p__7, " ", <>o__44.<>p__6.Target(<>o__44.<>p__6, obj4))));
                                        }
                                    }
                                    if (this._srr.Contains("Product Code:"))
                                    {
                                        string str = this._srr.Substring(this._srr.IndexOf("Product Code:"));
                                        str = str.Substring(str.IndexOf(":") + 1);
                                        str = str.Substring(0, str.IndexOf("\"")).Trim();
                                        product.ExtendedTitle = product.ExtendedTitle + " " + str;
                                    }
                                    goto Label_1880;
                            }
                            if ((num2 != 0x9e4864ef) || (shopifyWebsite != "addictmiami"))
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                        else if (num2 == 0x9553d8f4)
                        {
                            if (shopifyWebsite != "leaders1354")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                        else
                        {
                            if ((num2 != 0x95928b2a) || (shopifyWebsite != "eflashus"))
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                    }
                    else if (num2 > 0x85732999)
                    {
                        switch (num2)
                        {
                            case 0x8b55eb57:
                                if (shopifyWebsite != "sneakerpolitics")
                                {
                                    goto Label_1866;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_1880;

                            case 0x8fb99128:
                                if (shopifyWebsite != "eflasheu")
                                {
                                    goto Label_1866;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_1880;
                        }
                        if ((num2 != 0x94481229) || (shopifyWebsite != "capsuletoronto"))
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                    else if (num2 != 0x81836124)
                    {
                        if ((num2 != 0x85732999) || (shopifyWebsite != "trophyroomstore"))
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                    else
                    {
                        if (shopifyWebsite != "eflashsg")
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                }
                else if (num2 <= 0xd39d8c83)
                {
                    if (num2 > 0xb77b9be7)
                    {
                        switch (num2)
                        {
                            case 0xc0c6237f:
                                if (shopifyWebsite != "creme321")
                                {
                                    goto Label_1866;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_1880;

                            case 0xc5246ded:
                                if (shopifyWebsite != "wishatl")
                                {
                                    goto Label_1866;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_1880;
                        }
                        if ((num2 != 0xd39d8c83) || (shopifyWebsite != "xhibition"))
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:title"))).Attributes["content"].Value.Trim();
                        string str7 = this._srr.Substring(this._srr.IndexOf("var amastyXnotifConfig = {"));
                        str7 = str7.Substring(str7.IndexOf("{"));
                        str7 = str7.Substring(0, str7.IndexOf("};") + 1).Replace("product:", "\"product\":").Replace("customer_id:", "\"customer_id\":").Replace(@"\/", "v");
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str7);
                        if (<>o__44.<>p__23 == null)
                        {
                            <>o__44.<>p__23 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                        }
                        if (<>o__44.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__44.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "tags", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__44.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__44.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "product", typeof(Shopify2), argumentInfo));
                        }
                        foreach (object obj3 in <>o__44.<>p__23.Target(<>o__44.<>p__23, <>o__44.<>p__18.Target(<>o__44.<>p__18, <>o__44.<>p__17.Target(<>o__44.<>p__17, this._dynObj))))
                        {
                            product2 = product;
                            if (<>o__44.<>p__22 == null)
                            {
                                <>o__44.<>p__22 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__44.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__21 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__44.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__20 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__44.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            product2.ExtendedTitle = <>o__44.<>p__22.Target(<>o__44.<>p__22, <>o__44.<>p__21.Target(<>o__44.<>p__21, product2.ExtendedTitle, <>o__44.<>p__20.Target(<>o__44.<>p__20, " ", <>o__44.<>p__19.Target(<>o__44.<>p__19, obj3))));
                        }
                    }
                    else if (num2 != 0xb4e9bd0e)
                    {
                        if ((num2 != 0xb77b9be7) || (shopifyWebsite != "shopnicekicks"))
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                        if (this._srr.Contains("STYLE#:"))
                        {
                            string str6 = this._srr.Substring(this._srr.IndexOf("STYLE#:"));
                            str6 = str6.Substring(str6.IndexOf(":") + 1);
                            str6 = str6.Substring(0, str6.IndexOf("<")).Trim();
                            product.ExtendedTitle = product.ExtendedTitle + " " + str6;
                        }
                        if (this._srr.Contains("\"tags\":["))
                        {
                            string str4 = this._srr.Substring(this._srr.IndexOf("\"tags\":["));
                            str4 = str4.Substring(str4.IndexOf("[") + 1);
                            char[] separator = new char[] { ',' };
                            foreach (string str13 in str4.Substring(0, str4.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                            {
                                product.ExtendedTitle = product.ExtendedTitle + " " + str13;
                            }
                        }
                    }
                    else
                    {
                        if (shopifyWebsite != "obey")
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                }
                else if (num2 <= 0xe1c6240d)
                {
                    switch (num2)
                    {
                        case 0xda304ded:
                            if (shopifyWebsite != "blendsus")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_1880;

                        case 0xdfd25109:
                            if (shopifyWebsite != "18montrose")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_1880;
                    }
                    if ((num2 != 0xe1c6240d) || (shopifyWebsite != "commonwealth"))
                    {
                        goto Label_1866;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                else
                {
                    switch (num2)
                    {
                        case 0xe5c648dd:
                            if (shopifyWebsite != "solefly")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_1880;

                        case 0xe72487ea:
                            if (shopifyWebsite != "unknwn")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                            this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "productInfo"))).InnerText);
                            if (<>o__44.<>p__16 == null)
                            {
                                <>o__44.<>p__16 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                            }
                            if (<>o__44.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__44.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "tags", typeof(Shopify2), argumentInfo));
                            }
                            foreach (object obj2 in <>o__44.<>p__16.Target(<>o__44.<>p__16, <>o__44.<>p__11.Target(<>o__44.<>p__11, this._dynObj)))
                            {
                                product2 = product;
                                if (<>o__44.<>p__15 == null)
                                {
                                    <>o__44.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Shopify2)));
                                }
                                if (<>o__44.<>p__14 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__14 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__44.<>p__13 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__13 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__44.<>p__12 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__44.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                product2.ExtendedTitle = <>o__44.<>p__15.Target(<>o__44.<>p__15, <>o__44.<>p__14.Target(<>o__44.<>p__14, product2.ExtendedTitle, <>o__44.<>p__13.Target(<>o__44.<>p__13, " ", <>o__44.<>p__12.Target(<>o__44.<>p__12, obj2))));
                            }
                            goto Label_1880;
                    }
                    if ((num2 != 0xe8fbf68d) || (shopifyWebsite != "kith"))
                    {
                        goto Label_1866;
                    }
                    str3 = this._srr.Substring(this._srr.IndexOf("_BISConfig.product ="));
                    str3 = str3.Substring(str3.IndexOf("{"));
                    str3 = str3.Substring(0, str3.IndexOf(";"));
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str3);
                    if (<>o__44.<>p__2 == null)
                    {
                        <>o__44.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                    }
                    if (<>o__44.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__44.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__44.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__44.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    product.ExtendedTitle = <>o__44.<>p__2.Target(<>o__44.<>p__2, <>o__44.<>p__1.Target(<>o__44.<>p__1, <>o__44.<>p__0.Target(<>o__44.<>p__0, this._dynObj, "title")));
                    if (<>o__44.<>p__4 == null)
                    {
                        <>o__44.<>p__4 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Newtonsoft.Json.Linq.JArray), typeof(Shopify2)));
                    }
                    if (<>o__44.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__44.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    foreach (Newtonsoft.Json.Linq.JToken token in <>o__44.<>p__4.Target(<>o__44.<>p__4, <>o__44.<>p__3.Target(<>o__44.<>p__3, this._dynObj, "tags")))
                    {
                        product.ExtendedTitle = product.ExtendedTitle + " " + token;
                    }
                }
                goto Label_1880;
            }
            if (num2 <= 0x4e1453cf)
            {
                if (num2 > 0x28553330)
                {
                    if (num2 <= 0x38b6e100)
                    {
                        if (num2 != 0x339ec4fc)
                        {
                            if ((num2 != 0x38b6e100) || (shopifyWebsite != "funko"))
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"))).InnerText.Trim();
                        }
                        else
                        {
                            if (shopifyWebsite != "renarts")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                        }
                    }
                    else
                    {
                        switch (num2)
                        {
                            case 0x396a78ce:
                                if (shopifyWebsite != "lapstoneandhammer")
                                {
                                    goto Label_1866;
                                }
                                product.ExtendedTitle = product.ProductTitle;
                                goto Label_1880;

                            case 0x4b0d5ebb:
                                if (shopifyWebsite != "palaceeu")
                                {
                                    goto Label_1866;
                                }
                                product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"))).InnerText.Trim();
                                goto Label_1880;
                        }
                        if ((num2 != 0x4e1453cf) || (shopifyWebsite != "proper"))
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                }
                else if (num2 <= 0x494f792)
                {
                    if (num2 != 0x2850ca0)
                    {
                        if ((num2 != 0x494f792) || (shopifyWebsite != "saintalfred"))
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                    else
                    {
                        if (shopifyWebsite != "undefeated")
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                        if (this._srr.Contains("\"tags\":"))
                        {
                            string str9 = this._srr.Substring(this._srr.IndexOf("\"tags\":"));
                            str9 = str9.Substring(str9.IndexOf("[") + 1);
                            char[] separator = new char[] { ',' };
                            foreach (string str11 in str9.Substring(0, str9.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                            {
                                product.ExtendedTitle = product.ExtendedTitle + " " + str11;
                            }
                        }
                    }
                }
                else if (num2 == 0x2132f0cd)
                {
                    if (shopifyWebsite != "hanon")
                    {
                        goto Label_1866;
                    }
                    product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                    if (this._srr.Contains("\"tags\":"))
                    {
                        string str10 = this._srr.Substring(this._srr.IndexOf("\"tags\":"));
                        str10 = str10.Substring(str10.IndexOf("[") + 1);
                        char[] separator = new char[] { ',' };
                        foreach (string str14 in str10.Substring(0, str10.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                        {
                            product.ExtendedTitle = product.ExtendedTitle + " " + str14;
                        }
                    }
                }
                else if (num2 != 0x2315585e)
                {
                    if ((num2 != 0x28553330) || (shopifyWebsite != "nrml"))
                    {
                        goto Label_1866;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                else
                {
                    if (shopifyWebsite != "offthehook")
                    {
                        goto Label_1866;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                goto Label_1880;
            }
            if (num2 > 0x6d6601da)
            {
                if (num2 <= 0x6f7f4ed7)
                {
                    if (num2 != 0x6d970277)
                    {
                        if ((num2 != 0x6f7f4ed7) || (shopifyWebsite != "packer"))
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>().InnerText.Trim();
                        if (this._srr.Contains("\"tags\":"))
                        {
                            string str8 = this._srr.Substring(this._srr.IndexOf("\"tags\":"));
                            str8 = str8.Substring(str8.IndexOf("[") + 1);
                            char[] separator = new char[] { ',' };
                            foreach (string str12 in str8.Substring(0, str8.IndexOf("]")).Replace("\"", "").Split(separator).ToList<string>())
                            {
                                product.ExtendedTitle = product.ExtendedTitle + " " + str12;
                            }
                        }
                    }
                    else
                    {
                        if (shopifyWebsite != "notre")
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                    }
                }
                else
                {
                    switch (num2)
                    {
                        case 0x73761b67:
                            if (shopifyWebsite != "a-ma-maniere")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_1880;

                        case 0x756a879f:
                            if (shopifyWebsite != "abpstore")
                            {
                                goto Label_1866;
                            }
                            product.ExtendedTitle = product.ProductTitle;
                            goto Label_1880;
                    }
                    if ((num2 != 0x7a949f40) || (shopifyWebsite != "eflashjp"))
                    {
                        goto Label_1866;
                    }
                    product.ExtendedTitle = product.ProductTitle;
                }
                goto Label_1880;
            }
            if (num2 > 0x5134779d)
            {
                switch (num2)
                {
                    case 0x63c27943:
                        if (shopifyWebsite != "havenshop")
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                        goto Label_1880;

                    case 0x63f2cf1c:
                        if (shopifyWebsite != "socialstatus")
                        {
                            goto Label_1866;
                        }
                        product.ExtendedTitle = product.ProductTitle;
                        goto Label_1880;
                }
                if ((num2 != 0x6d6601da) || (shopifyWebsite != "shophny"))
                {
                    goto Label_1866;
                }
                product.ExtendedTitle = product.ProductTitle;
                goto Label_1880;
            }
            if (num2 != 0x4e70bbdf)
            {
                if ((num2 != 0x5134779d) || (shopifyWebsite != "palaceus"))
                {
                    goto Label_1866;
                }
                product.ExtendedTitle = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"))).InnerText.Trim();
                goto Label_1880;
            }
            if (shopifyWebsite == "premierestore")
            {
                product.ExtendedTitle = product.ProductTitle;
                goto Label_1880;
            }
        Label_1866:
            product.ExtendedTitle = product.ProductTitle;
        Label_1880:
            product.ExtendedTitle = product.ExtendedTitle.RemoveSpecialCharacters();
        }

        public bool Search()
        {
            this.Init();
            this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
            States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
            return this.SearchInternal();
        }

        private bool SearchInternal()
        {
            try
            {
                IEnumerator<string> enumerator5;
                int num3;
                string[] strArray2;
                List<string> list = new List<string> { 
                    "https://yeezysupply.com/collections/new-arrivals",
                    "https://yeezysupply.com/collections/men",
                    "https://yeezysupply.com/collections/women"
                };
                string url = "";
                if (!this._task.Link.Contains("eflash"))
                {
                    if (this._task.Link.Contains("yeezysupply"))
                    {
                        url = list[0];
                    }
                    else if (this._task.ShopifyWebsite == "other")
                    {
                        url = this._task.Link + "/sitemap_products_1.xml";
                    }
                    else
                    {
                        url = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.Website == this._task.ShopifyWebsite)).SearchLink;
                    }
                }
                else
                {
                    url = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.Website == this._task.ShopifyWebsite)).HomeLink;
                }
                KeyValuePair<string, string> pair = this._client.Get(url).TextResponseUri();
                string str2 = pair.Value;
                this._srr = pair.Key;
                List<Product> source = new List<Product>();
                if (url.Contains(".json"))
                {
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                    if (<>o__43.<>p__13 == null)
                    {
                        <>o__43.<>p__13 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                    }
                    if (<>o__43.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__43.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "products", typeof(Shopify2), argumentInfo));
                    }
                    foreach (object obj2 in <>o__43.<>p__13.Target(<>o__43.<>p__13, <>o__43.<>p__0.Target(<>o__43.<>p__0, this._dynObj)))
                    {
                        Product item = new Product();
                        if (<>o__43.<>p__4 == null)
                        {
                            <>o__43.<>p__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                        }
                        if (<>o__43.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__3 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        item.Link = <>o__43.<>p__4.Target(<>o__43.<>p__4, <>o__43.<>p__3.Target(<>o__43.<>p__3, $"{this._task.HomeUrl}/products/", <>o__43.<>p__2.Target(<>o__43.<>p__2, <>o__43.<>p__1.Target(<>o__43.<>p__1, obj2, "handle"))));
                        if (<>o__43.<>p__9 == null)
                        {
                            <>o__43.<>p__9 = CallSite<Func<CallSite, object, DateTime?>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(DateTime?), typeof(Shopify2)));
                        }
                        if (<>o__43.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__8 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        item.LastMod = <>o__43.<>p__9.Target(<>o__43.<>p__9, <>o__43.<>p__8.Target(<>o__43.<>p__8, typeof(DateTime), <>o__43.<>p__7.Target(<>o__43.<>p__7, <>o__43.<>p__6.Target(<>o__43.<>p__6, <>o__43.<>p__5.Target(<>o__43.<>p__5, obj2, "published_at")))));
                        if (<>o__43.<>p__12 == null)
                        {
                            <>o__43.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                        }
                        if (<>o__43.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        item.ProductTitle = <>o__43.<>p__12.Target(<>o__43.<>p__12, <>o__43.<>p__11.Target(<>o__43.<>p__11, <>o__43.<>p__10.Target(<>o__43.<>p__10, obj2, "title")));
                        source.Add(item);
                    }
                    goto Label_1DBE;
                }
                if (this._task.Link.Contains("eflash") || this._task.Link.Contains("yeezysupply"))
                {
                    goto Label_092A;
                }
                XDocument document1 = XDocument.Parse(this._srr);
                if (document1 == null)
                {
                    throw new Exception("sitemap_products_1.xml not available");
                }
                XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
                XNamespace namespace2 = "http://www.google.com/schemas/sitemap-image/1.1";
                List<XElement>.Enumerator enumerator = (from x in document1.Descendants(ns + "url")
                    where x.Descendants(ns + "lastmod").ToList<XElement>().Count > 0
                    select x).ToList<XElement>().GetEnumerator();
            Label_067E:
                try
                {
                Label_067E:
                    if (!enumerator.MoveNext())
                    {
                        goto Label_077F;
                    }
                    XElement current = enumerator.Current;
                    if (!current.Descendants(ns + "loc").Any<XElement>())
                    {
                        goto Label_067E;
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
                    goto Label_074F;
                Label_071D:
                    item.ProductTitle = current.Descendants(namespace2 + "title").FirstOrDefault<XElement>().Value;
                    source.Add(item);
                    goto Label_067E;
                Label_074F:
                    if (current.Descendants(namespace2 + "title").FirstOrDefault<XElement>() == null)
                    {
                        goto Label_067E;
                    }
                    goto Label_071D;
                }
                finally
                {
                    enumerator.Dispose();
                }
            Label_077F:;
                try
                {
                    url = url.Replace("1.xml", "2.xml");
                    this._srr = this._client.Get("searchLink").Text();
                    XDocument document2 = XDocument.Parse(this._srr);
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
                    Label_081E:
                        if (!enumerator.MoveNext())
                        {
                            goto Label_1DBE;
                        }
                        XElement current = enumerator.Current;
                        if (!current.Descendants(ns + "loc").Any<XElement>())
                        {
                            goto Label_081E;
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
                        goto Label_08EF;
                    Label_08BD:
                        item.ProductTitle = current.Descendants(namespace2 + "title").FirstOrDefault<XElement>().Value;
                        source.Add(item);
                        goto Label_081E;
                    Label_08EF:
                        if (current.Descendants(namespace2 + "title").FirstOrDefault<XElement>() == null)
                        {
                            goto Label_081E;
                        }
                        goto Label_08BD;
                    }
                }
                catch
                {
                    goto Label_1DBE;
                }
            Label_092A:
                if (this._task.Link.Contains("yeezysupply"))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-new-arrivals-json"))).InnerText);
                    DateTime now = DateTime.Now;
                    if (<>o__43.<>p__26 == null)
                    {
                        <>o__43.<>p__26 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                    }
                    if (<>o__43.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__43.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "products", typeof(Shopify2), argumentInfo));
                    }
                    foreach (object obj3 in <>o__43.<>p__26.Target(<>o__43.<>p__26, <>o__43.<>p__14.Target(<>o__43.<>p__14, this._dynObj)))
                    {
                        if (<>o__43.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__17 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (!<>o__43.<>p__18.Target(<>o__43.<>p__18, <>o__43.<>p__17.Target(<>o__43.<>p__17, <>o__43.<>p__16.Target(<>o__43.<>p__16, <>o__43.<>p__15.Target(<>o__43.<>p__15, obj3, "available")), true)))
                        {
                            Product prod = new Product {
                                LastMod = new DateTime?(now)
                            };
                            if (<>o__43.<>p__22 == null)
                            {
                                <>o__43.<>p__22 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__43.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__21 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            prod.Link = <>o__43.<>p__22.Target(<>o__43.<>p__22, <>o__43.<>p__21.Target(<>o__43.<>p__21, "https://yeezysupply.com", <>o__43.<>p__20.Target(<>o__43.<>p__20, <>o__43.<>p__19.Target(<>o__43.<>p__19, obj3, "url"))));
                            if (<>o__43.<>p__25 == null)
                            {
                                <>o__43.<>p__25 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__43.<>p__24 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            prod.ProductTitle = <>o__43.<>p__25.Target(<>o__43.<>p__25, <>o__43.<>p__24.Target(<>o__43.<>p__24, <>o__43.<>p__23.Target(<>o__43.<>p__23, obj3, "handle")));
                            prod.ProductTitle = prod.ProductTitle.Replace("-", " ");
                            if (!source.Any<Product>(x => (x.ProductTitle == prod.ProductTitle)))
                            {
                                source.Add(prod);
                            }
                        }
                    }
                    string str3 = this._client.Get("https://yeezysupply.com").Text();
                    while (str3.Contains("var p = {"))
                    {
                        str3 = str3.Substring(str3.IndexOf("var p = {"));
                        str3 = str3.Substring(str3.IndexOf("title"));
                        str3 = str3.Substring(str3.IndexOf("\"") + 1);
                        string str5 = str3.Substring(0, str3.IndexOf("\""));
                        str3 = str3.Substring(str3.IndexOf("handle"));
                        str3 = str3.Substring(str3.IndexOf("\"") + 1);
                        string str4 = str3.Substring(0, str3.IndexOf("\""));
                        str3 = str3.Substring(str3.IndexOf("featured_image"));
                        str3 = str3.Substring(str3.IndexOf("\"") + 1);
                        string text1 = "https:" + str3.Substring(0, str3.IndexOf("\"")).Replace(@"\/", "/");
                        Product product1 = new Product {
                            LastMod = new DateTime?(now),
                            Link = "https://yeezysupply.com/products/" + str4,
                            ProductTitle = str5
                        };
                        if (!source.Any<Product>(x => (x.ProductTitle == product1.ProductTitle)))
                        {
                            source.Add(product1);
                        }
                    }
                    this._srr = this._client.Get(list[1]).Text();
                    this._currentDoc.LoadHtml(this._srr);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._currentDoc.DocumentNode.Descendants("script").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-collection-json"))).InnerText);
                    if (<>o__43.<>p__43 == null)
                    {
                        <>o__43.<>p__43 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                    }
                    if (<>o__43.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__43.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "products", typeof(Shopify2), argumentInfo));
                    }
                    foreach (object obj4 in <>o__43.<>p__43.Target(<>o__43.<>p__43, <>o__43.<>p__27.Target(<>o__43.<>p__27, this._dynObj)))
                    {
                        if (<>o__43.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__31 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__30 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (!<>o__43.<>p__31.Target(<>o__43.<>p__31, <>o__43.<>p__30.Target(<>o__43.<>p__30, <>o__43.<>p__29.Target(<>o__43.<>p__29, <>o__43.<>p__28.Target(<>o__43.<>p__28, obj4, "available")), true)))
                        {
                            Product product2 = new Product {
                                LastMod = new DateTime?(now)
                            };
                            if (<>o__43.<>p__35 == null)
                            {
                                <>o__43.<>p__35 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__43.<>p__34 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__34 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__33 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__32 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__32 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            product2.Link = <>o__43.<>p__35.Target(<>o__43.<>p__35, <>o__43.<>p__34.Target(<>o__43.<>p__34, "https://yeezysupply.com", <>o__43.<>p__33.Target(<>o__43.<>p__33, <>o__43.<>p__32.Target(<>o__43.<>p__32, obj4, "url"))));
                            if (<>o__43.<>p__38 == null)
                            {
                                <>o__43.<>p__38 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__43.<>p__37 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__36 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__36 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            product2.ProductTitle = <>o__43.<>p__38.Target(<>o__43.<>p__38, <>o__43.<>p__37.Target(<>o__43.<>p__37, <>o__43.<>p__36.Target(<>o__43.<>p__36, obj4, "handle")));
                            if (<>o__43.<>p__42 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__42 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__41 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__41 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__40 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__39 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__39 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            <>o__43.<>p__42.Target(<>o__43.<>p__42, <>o__43.<>p__41.Target(<>o__43.<>p__41, <>o__43.<>p__40.Target(<>o__43.<>p__40, <>o__43.<>p__39.Target(<>o__43.<>p__39, obj4, "handle")), "mens-military-boot-in-washed-canvas-splinter-camo"));
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
                    if (<>o__43.<>p__60 == null)
                    {
                        <>o__43.<>p__60 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Shopify2)));
                    }
                    if (<>o__43.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__43.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "products", typeof(Shopify2), argumentInfo));
                    }
                    foreach (object obj5 in <>o__43.<>p__60.Target(<>o__43.<>p__60, <>o__43.<>p__44.Target(<>o__43.<>p__44, this._dynObj)))
                    {
                        if (<>o__43.<>p__48 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__48 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__47 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__47 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__46 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__43.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                        }
                        if (<>o__43.<>p__45 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__43.<>p__45 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                        }
                        if (!<>o__43.<>p__48.Target(<>o__43.<>p__48, <>o__43.<>p__47.Target(<>o__43.<>p__47, <>o__43.<>p__46.Target(<>o__43.<>p__46, <>o__43.<>p__45.Target(<>o__43.<>p__45, obj5, "available")), true)))
                        {
                            Product product3 = new Product {
                                LastMod = new DateTime?(now)
                            };
                            if (<>o__43.<>p__52 == null)
                            {
                                <>o__43.<>p__52 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__43.<>p__51 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__51 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__50 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__49 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__49 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            product3.Link = <>o__43.<>p__52.Target(<>o__43.<>p__52, <>o__43.<>p__51.Target(<>o__43.<>p__51, "https://yeezysupply.com", <>o__43.<>p__50.Target(<>o__43.<>p__50, <>o__43.<>p__49.Target(<>o__43.<>p__49, obj5, "url"))));
                            if (<>o__43.<>p__55 == null)
                            {
                                <>o__43.<>p__55 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Shopify2)));
                            }
                            if (<>o__43.<>p__54 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__53 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__53 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            product3.ProductTitle = <>o__43.<>p__55.Target(<>o__43.<>p__55, <>o__43.<>p__54.Target(<>o__43.<>p__54, <>o__43.<>p__53.Target(<>o__43.<>p__53, obj5, "handle")));
                            if (<>o__43.<>p__59 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__59 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__58 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__58 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__57 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__43.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__43.<>p__56 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__43.<>p__56 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            <>o__43.<>p__59.Target(<>o__43.<>p__59, <>o__43.<>p__58.Target(<>o__43.<>p__58, <>o__43.<>p__57.Target(<>o__43.<>p__57, <>o__43.<>p__56.Target(<>o__43.<>p__56, obj5, "handle")), "mens-military-boot-in-washed-canvas-splinter-camo"));
                            product3.ProductTitle = product3.ProductTitle.Replace("-", " ");
                            if (!source.Any<Product>(x => (x.ProductTitle == product3.ProductTitle)))
                            {
                                source.Add(product3);
                            }
                        }
                    }
                }
                else
                {
                    this._currentDoc.LoadHtml(this._srr);
                    DateTime now = DateTime.Now;
                    foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("a")
                        where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "grid-view-item__link")
                        select x)
                    {
                        Product item = new Product {
                            LastMod = new DateTime?(now),
                            Link = str2 + node.Attributes["href"].Value,
                            ProductTitle = node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("h4"))).InnerText.Trim()
                        };
                        source.Add(item);
                    }
                }
            Label_1DBE:
                this._availableProducts = source;
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
                if (this._task.Last25Products && !this._task.HomeUrl.Contains("yeezysupply"))
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
                List<Product> list3 = new List<Product>();
                using (enumerator5 = this._task.Keywords.GetEnumerator())
                {
                    while (enumerator5.MoveNext())
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = enumerator5.Current.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (Product product in source)
                        {
                            bool flag = true;
                            string extendedTitle = "";
                            if ((this._task.ShopifyWebsite != "other") && this._task.DeepSearch)
                            {
                                extendedTitle = product.ExtendedTitle;
                            }
                            else
                            {
                                extendedTitle = product.ProductTitle;
                            }
                            char[] chArray2 = new char[] { ' ' };
                            string[] strArray3 = extendedTitle.ToUpperInvariant().Split(chArray2);
                            strArray2 = strArray;
                            num3 = 0;
                            while (num3 < strArray2.Length)
                            {
                                string kw = strArray2[num3];
                                if (!strArray3.Any<string>(x => (x == kw)))
                                {
                                    goto Label_20D3;
                                }
                                num3++;
                            }
                            goto Label_20D6;
                        Label_20D3:
                            flag = false;
                        Label_20D6:
                            if (flag && !list3.Any<Product>(x => (x.Link == product.Link)))
                            {
                                list3.Add(product);
                            }
                        }
                    }
                }
                if (list3.Count != 0)
                {
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
                                    string str8;
                                    char[] separator = new char[] { ' ' };
                                    string[] strArray4 = enumerator5.Current.Split(separator);
                                    for (int i = 0; i < strArray4.Length; i++)
                                    {
                                        strArray4[i] = strArray4[i].Trim().ToUpperInvariant();
                                    }
                                    strArray2 = strArray4;
                                    num3 = 0;
                                    goto Label_2252;
                                Label_21F5:
                                    str8 = strArray2[num3];
                                    string extendedTitle = "";
                                    if ((this._task.ShopifyWebsite != "other") && this._task.DeepSearch)
                                    {
                                        extendedTitle = product6.ExtendedTitle;
                                    }
                                    else
                                    {
                                        extendedTitle = product6.ProductTitle;
                                    }
                                    if (extendedTitle.ToUpperInvariant().Contains(str8))
                                    {
                                        goto Label_225C;
                                    }
                                    num3++;
                                Label_2252:
                                    if (num3 >= strArray2.Length)
                                    {
                                        continue;
                                    }
                                    goto Label_21F5;
                                Label_225C:
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
                            return false;
                        }
                    }
                    else
                    {
                        list4 = list3;
                    }
                    foreach (Product product7 in list4)
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", product7.Link);
                        if (!this._task.Link.ToLowerInvariant().Contains("yeezysupply.com"))
                        {
                            this.DirectLinkGeneral(product7.Link, true);
                        }
                        else
                        {
                            this.DirectLinkYeezy(product7.Link, false);
                        }
                        if ((this._runner.PickedSize.HasValue && (this._runner.Product != null)) && (this._runner.Product.AvailableSizes.Count > 0))
                        {
                            return true;
                        }
                    }
                }
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
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SEARCH);
                    string str9 = "";
                    if ((exception is WebException) && (((WebException) exception).Response != null))
                    {
                        using (Stream stream = ((WebException) exception).Response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                str9 = reader.ReadToEnd();
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(str9))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", str9);
                    }
                    return false;
                }
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                return false;
            }
        }

        private Product SearchMulti(TaskObject task)
        {
            try
            {
                IEnumerator<string> enumerator2;
                int num3;
                string[] strArray3;
                List<Product> list = this._availableProducts;
                if (task.Last25Products)
                {
                    list = (from x in (from x in list
                        where x.LastMod.HasValue
                        select x).ToList<Product>()
                        orderby x.LastMod descending
                        select x).Take<Product>(0x19).ToList<Product>();
                }
                foreach (Product local1 in list)
                {
                    local1.ProductTitle = local1.ProductTitle.Replace("/", " ").Replace("  ", " ").Replace("\"", "").Replace("'", "");
                }
                foreach (Product local2 in list)
                {
                    local2.ProductTitle = local2.ProductTitle.RemoveSpecialCharacters();
                }
                List<Product> source = new List<Product>();
                using (enumerator2 = task.Keywords.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = enumerator2.Current.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            strArray[i] = strArray[i].Trim().ToUpperInvariant();
                        }
                        foreach (Product product in list)
                        {
                            <>c__DisplayClass42_1 class_2;
                            bool flag = true;
                            string extendedTitle = "";
                            if ((task.ShopifyWebsite != "other") && task.DeepSearch)
                            {
                                extendedTitle = product.ExtendedTitle;
                            }
                            else
                            {
                                extendedTitle = product.ProductTitle;
                            }
                            char[] chArray2 = new char[] { ' ' };
                            string[] strArray2 = extendedTitle.ToUpperInvariant().Split(chArray2);
                            strArray3 = strArray;
                            num3 = 0;
                            goto Label_022D;
                        Label_01FE:
                            class_2 = new <>c__DisplayClass42_1();
                            class_2.kw = strArray3[num3];
                            if (!strArray2.Any<string>(new Func<string, bool>(class_2.<SearchMulti>b__3)))
                            {
                                goto Label_0237;
                            }
                            num3++;
                        Label_022D:
                            if (num3 >= strArray3.Length)
                            {
                                goto Label_023A;
                            }
                            goto Label_01FE;
                        Label_0237:
                            flag = false;
                        Label_023A:
                            if (flag && !source.Any<Product>(x => (x.Link == product.Link)))
                            {
                                source.Add(product);
                            }
                        }
                    }
                }
                if (source.Count != 0)
                {
                    List<Product> list3 = new List<Product>();
                    if (task.NegativeKeywords.Any<string>())
                    {
                        foreach (Product product2 in source)
                        {
                            bool flag2 = true;
                            Product tmpProd = null;
                            tmpProd = product2;
                            using (enumerator2 = task.NegativeKeywords.GetEnumerator())
                            {
                                while (enumerator2.MoveNext())
                                {
                                    string str3;
                                    char[] separator = new char[] { ' ' };
                                    string[] strArray4 = enumerator2.Current.Split(separator);
                                    for (int i = 0; i < strArray4.Length; i++)
                                    {
                                        strArray4[i] = strArray4[i].Trim().ToUpperInvariant();
                                    }
                                    strArray3 = strArray4;
                                    num3 = 0;
                                    goto Label_039D;
                                Label_034A:
                                    str3 = strArray3[num3];
                                    string extendedTitle = "";
                                    if ((task.ShopifyWebsite != "other") && task.DeepSearch)
                                    {
                                        extendedTitle = product2.ExtendedTitle;
                                    }
                                    else
                                    {
                                        extendedTitle = product2.ProductTitle;
                                    }
                                    if (extendedTitle.ToUpperInvariant().Contains(str3))
                                    {
                                        goto Label_03A7;
                                    }
                                    num3++;
                                Label_039D:
                                    if (num3 >= strArray3.Length)
                                    {
                                        continue;
                                    }
                                    goto Label_034A;
                                Label_03A7:
                                    flag2 = false;
                                }
                            }
                            if (flag2 && !list3.Any<Product>(x => (x.Link == tmpProd.Link)))
                            {
                                list3.Add(tmpProd);
                            }
                        }
                        if (list3.Count == 0)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        list3 = source;
                    }
                    foreach (Product product4 in list3)
                    {
                        Product product3 = this.DirectLinkMulti(task, product4.Link);
                        if ((product3 != null) && (product3.AvailableSizes.Count > 0))
                        {
                            return product3;
                        }
                    }
                }
                return null;
            }
            catch (ThreadAbortException)
            {
                return null;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SEARCH);
                    string str4 = "";
                    if ((exception is WebException) && (((WebException) exception).Response != null))
                    {
                        using (Stream stream = ((WebException) exception).Response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                str4 = reader.ReadToEnd();
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(str4))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, exception, "", str4);
                    }
                    return null;
                }
                this._runner.IsError = true;
                this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                return null;
            }
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, false);
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Basic " + this._apiToken);
            this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
        }

        public void SetClientTmp()
        {
            this._clientTmp = new Client(null, this._runner.Proxy, false);
            this._clientTmp.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            this._clientTmp.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._clientTmp.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._clientTmp.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._clientTmp.Session.DefaultRequestHeaders.ExpectContinue = false;
        }

        private bool SubmitOrder()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                EveAIO.Helpers.RandomString(0x10);
                if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                    this._checkoutToken = "";
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
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
                    this._checkoutToken = "";
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
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
                try
                {
                    HttpResponseMessage response = this._client.Get(this._runner.HomeUrl + "/api/checkouts/" + this._checkoutToken + ".json");
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
                    EveAIO.Extensions.CheckError(response);
                }
                catch (WebException exception)
                {
                    if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                    {
                        if (exception.Message.Contains("422") || ((exception.InnerException != null) && exception.InnerException.Message.Contains("422")))
                        {
                            bool flag2;
                            this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                            if (!(flag2 = this._srr.Contains("Payment gateway not supported")))
                            {
                                if (<>o__36.<>p__7 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__36.<>p__6 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__6 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__36.<>p__5 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__36.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__36.<>p__4 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__36.<>p__3 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__36.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__36.<>p__2 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__36.<>p__1 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payment_gateway", typeof(Shopify2), argumentInfo));
                                }
                                if (<>o__36.<>p__0 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__36.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errors", typeof(Shopify2), argumentInfo));
                                }
                                if (!<>o__36.<>p__7.Target(<>o__36.<>p__7, <>o__36.<>p__6.Target(<>o__36.<>p__6, flag2, <>o__36.<>p__5.Target(<>o__36.<>p__5, <>o__36.<>p__4.Target(<>o__36.<>p__4, <>o__36.<>p__3.Target(<>o__36.<>p__3, <>o__36.<>p__2.Target(<>o__36.<>p__2, <>o__36.<>p__1.Target(<>o__36.<>p__1, <>o__36.<>p__0.Target(<>o__36.<>p__0, this._dynObj))), "code")), "invalid"))))
                                {
                                    goto Label_0812;
                                }
                            }
                            States.WriteLogger(this._task, States.LOGGER_STATES.UNSUPPORTED_GATEWAY, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.UNSUPPORTED_GATEWAY);
                            return false;
                        }
                        if (exception.Message.Contains("424") || ((exception.InnerException != null) && exception.InnerException.Message.Contains("424")))
                        {
                            if (<>o__36.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.NamedArgument, "msg") };
                                <>o__36.<>p__14 = CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteLogger", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__13 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__12 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__36.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__10 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payment_gateway", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__9 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__8 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errors", typeof(Shopify2), argumentInfo));
                            }
                            <>o__36.<>p__14.Target(<>o__36.<>p__14, typeof(States), this._task, States.LOGGER_STATES.CARD_DECLINED, <>o__36.<>p__13.Target(<>o__36.<>p__13, <>o__36.<>p__12.Target(<>o__36.<>p__12, <>o__36.<>p__11.Target(<>o__36.<>p__11, <>o__36.<>p__10.Target(<>o__36.<>p__10, <>o__36.<>p__9.Target(<>o__36.<>p__9, <>o__36.<>p__8.Target(<>o__36.<>p__8, this._dynObj)))), "message")));
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            this._checkoutToken = "";
                            this._cardId = "";
                            this._runner._tokenTimestamp = null;
                            return false;
                        }
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                }
            Label_0812:
                if (<>o__36.<>p__28 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__28 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                }
                if (<>o__36.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__36.<>p__16 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                }
                if (<>o__36.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                }
                object obj2 = <>o__36.<>p__16.Target(<>o__36.<>p__16, <>o__36.<>p__15.Target(<>o__36.<>p__15, this._dynObj), null);
                if (<>o__36.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__21 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Shopify2), argumentInfo));
                }
                if (!<>o__36.<>p__21.Target(<>o__36.<>p__21, obj2))
                {
                    if (<>o__36.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__20 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__36.<>p__19 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__36.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                }
                object obj3 = (<>o__36.<>p__17 != null) ? obj2 : <>o__36.<>p__20.Target(<>o__36.<>p__20, obj2, <>o__36.<>p__19.Target(<>o__36.<>p__19, <>o__36.<>p__18.Target(<>o__36.<>p__18, <>o__36.<>p__17.Target(<>o__36.<>p__17, this._dynObj), "payments"), null));
                if (<>o__36.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__36.<>p__27 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Shopify2), argumentInfo));
                }
                if (!<>o__36.<>p__27.Target(<>o__36.<>p__27, obj3))
                {
                    if (<>o__36.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__26 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__36.<>p__25 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__36.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                }
                if (!<>o__36.<>p__28.Target(<>o__36.<>p__28, (<>o__36.<>p__22 != null) ? obj3 : <>o__36.<>p__26.Target(<>o__36.<>p__26, obj3, <>o__36.<>p__25.Target(<>o__36.<>p__25, <>o__36.<>p__24.Target(<>o__36.<>p__24, <>o__36.<>p__23.Target(<>o__36.<>p__23, <>o__36.<>p__22.Target(<>o__36.<>p__22, this._dynObj), "payments")), 0))))
                {
                    if (<>o__36.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__35 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__36.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__36.<>p__32 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payments", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    if (!<>o__36.<>p__35.Target(<>o__36.<>p__35, <>o__36.<>p__34.Target(<>o__36.<>p__34, <>o__36.<>p__33.Target(<>o__36.<>p__33, <>o__36.<>p__32.Target(<>o__36.<>p__32, <>o__36.<>p__31.Target(<>o__36.<>p__31, <>o__36.<>p__30.Target(<>o__36.<>p__30, <>o__36.<>p__29.Target(<>o__36.<>p__29, this._dynObj))), "status")), "failure")))
                    {
                        try
                        {
                            if (<>o__36.<>p__43 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__43 = CallSite<Func<CallSite, Encoding, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetBytes", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__42 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                            }
                            object obj4 = <>o__36.<>p__43.Target(<>o__36.<>p__43, Encoding.UTF8, <>o__36.<>p__42.Target(<>o__36.<>p__42, this._dynObj));
                            if (<>o__36.<>p__44 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__44 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToBase64String", null, typeof(Shopify2), argumentInfo));
                            }
                            object obj5 = <>o__36.<>p__44.Target(<>o__36.<>p__44, typeof(Convert), obj4);
                            if (<>o__36.<>p__46 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__46 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddDbValue", null, typeof(Shopify2), argumentInfo));
                            }
                            if (<>o__36.<>p__45 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__36.<>p__45 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                            }
                            <>o__36.<>p__46.Target(<>o__36.<>p__46, typeof(EveAIO.Helpers), <>o__36.<>p__45.Target(<>o__36.<>p__45, "Shopify order success|", obj5));
                        }
                        catch
                        {
                        }
                        if (this.CHILD_SUCCESSES.Count > 0)
                        {
                            Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                                foreach (SuccessObject s in this.CHILD_SUCCESSES)
                                {
                                    if (this._multiAdditions.First<ShopifyMultiAddition>(x => (x.TaskId == s.TaskId)).Added)
                                    {
                                        s.CheckoutHidden = 0;
                                    }
                                }
                            }, Array.Empty<object>());
                        }
                        return true;
                    }
                    if (<>o__36.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.NamedArgument, "msg") };
                        <>o__36.<>p__41 = CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteLogger", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__36.<>p__39 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payments", typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "checkout", typeof(Shopify2), argumentInfo));
                    }
                    <>o__36.<>p__41.Target(<>o__36.<>p__41, typeof(States), this._task, States.LOGGER_STATES.CARD_DECLINED, <>o__36.<>p__40.Target(<>o__36.<>p__40, <>o__36.<>p__39.Target(<>o__36.<>p__39, <>o__36.<>p__38.Target(<>o__36.<>p__38, <>o__36.<>p__37.Target(<>o__36.<>p__37, <>o__36.<>p__36.Target(<>o__36.<>p__36, this._dynObj))), "message")));
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    this._checkoutToken = "";
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                    return false;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PAYMENT_ERROR, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.PAYMENT_ERROR);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (this._task.Link.Contains("eflash"))
                {
                    Global.Logger.Info("elfash src : " + this._srr);
                    if (<>o__36.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__49 = CallSite<Action<CallSite, ILog, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Info", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__48 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    <>o__36.<>p__49.Target(<>o__36.<>p__49, Global.Logger, <>o__36.<>p__48.Target(<>o__36.<>p__48, "elfash json : ", <>o__36.<>p__47.Target(<>o__36.<>p__47, this._dynObj)));
                }
                try
                {
                    if (<>o__36.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__51 = CallSite<Func<CallSite, Encoding, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetBytes", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Shopify2), argumentInfo));
                    }
                    object obj6 = <>o__36.<>p__51.Target(<>o__36.<>p__51, Encoding.UTF8, <>o__36.<>p__50.Target(<>o__36.<>p__50, this._dynObj));
                    if (<>o__36.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__52 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToBase64String", null, typeof(Shopify2), argumentInfo));
                    }
                    object obj7 = <>o__36.<>p__52.Target(<>o__36.<>p__52, typeof(Convert), obj6);
                    if (<>o__36.<>p__54 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__54 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddDbValue", null, typeof(Shopify2), argumentInfo));
                    }
                    if (<>o__36.<>p__53 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__36.<>p__53 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Shopify2), argumentInfo));
                    }
                    <>o__36.<>p__54.Target(<>o__36.<>p__54, typeof(EveAIO.Helpers), <>o__36.<>p__53.Target(<>o__36.<>p__53, "Shopify order error|", obj7));
                }
                catch
                {
                }
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    string str = "";
                    if (exception2 is WebException)
                    {
                        str = this._srr;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    if (!string.IsNullOrEmpty(str))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception2, "", str);
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception2, "", "");
                    }
                    this._checkoutToken = "";
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    this._checkoutToken = "";
                    this._cardId = "";
                    this._runner._tokenTimestamp = null;
                }
                return false;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Shopify2.<>c <>9;
            public static Func<HtmlNode, bool> <>9__28_0;
            public static Func<HtmlNode, bool> <>9__30_0;
            public static Func<HtmlNode, bool> <>9__31_0;
            public static Func<HtmlNode, bool> <>9__31_1;
            public static Func<HtmlNode, bool> <>9__33_0;
            public static Func<HtmlNode, bool> <>9__40_0;
            public static Func<HtmlNode, bool> <>9__40_1;
            public static Func<HtmlNode, bool> <>9__40_2;
            public static Func<HtmlNode, bool> <>9__40_3;
            public static Func<HtmlNode, bool> <>9__41_0;
            public static Func<HtmlNode, bool> <>9__41_1;
            public static Func<Product, bool> <>9__42_0;
            public static Func<Product, DateTime?> <>9__42_1;
            public static Func<HtmlNode, bool> <>9__43_4;
            public static Func<HtmlNode, bool> <>9__43_5;
            public static Func<HtmlNode, bool> <>9__43_6;
            public static Func<HtmlNode, bool> <>9__43_11;
            public static Func<HtmlNode, bool> <>9__43_12;
            public static Func<Product, DateTime?> <>9__43_13;
            public static Func<Product, bool> <>9__43_14;
            public static Func<Product, DateTime?> <>9__43_15;
            public static Func<HtmlNode, bool> <>9__44_0;
            public static Func<HtmlNode, bool> <>9__44_1;
            public static Func<HtmlNode, bool> <>9__44_2;
            public static Func<HtmlNode, bool> <>9__44_3;
            public static Func<HtmlNode, bool> <>9__44_4;
            public static Func<HtmlNode, bool> <>9__44_5;
            public static Func<HtmlNode, bool> <>9__44_6;
            public static Func<HtmlNode, bool> <>9__44_7;
            public static Func<HtmlNode, bool> <>9__44_8;
            public static Func<HtmlNode, bool> <>9__44_9;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Shopify2.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__33_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "note"));

            internal bool <AtcMulti>b__30_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "note"));

            internal bool <DirectLinkGeneral>b__40_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"));

            internal bool <DirectLinkGeneral>b__40_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"));

            internal bool <DirectLinkGeneral>b__40_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("properties["));

            internal bool <DirectLinkGeneral>b__40_3(HtmlNode x) => 
                ((x.Attributes["src"] != null) && x.Attributes["src"].Value.Contains("custom.js"));

            internal bool <DirectLinkMulti>b__31_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"));

            internal bool <DirectLinkMulti>b__31_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ProductJson-product-template"));

            internal bool <DirectLinkYeezy>b__41_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-product-json"));

            internal bool <DirectLinkYeezy>b__41_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "twitter:image"));

            internal bool <ExtractApiKey>b__28_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shopify-checkout-api-token"));

            internal bool <ProcessDeepSearchLink>b__44_0(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <ProcessDeepSearchLink>b__44_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <ProcessDeepSearchLink>b__44_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "productInfo"));

            internal bool <ProcessDeepSearchLink>b__44_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <ProcessDeepSearchLink>b__44_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"));

            internal bool <ProcessDeepSearchLink>b__44_5(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:title"));

            internal bool <ProcessDeepSearchLink>b__44_6(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name"));

            internal bool <ProcessDeepSearchLink>b__44_7(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"));

            internal bool <ProcessDeepSearchLink>b__44_8(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "title"));

            internal bool <ProcessDeepSearchLink>b__44_9(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <SearchInternal>b__43_11(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "grid-view-item__link"));

            internal bool <SearchInternal>b__43_12(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("h4"));

            internal DateTime? <SearchInternal>b__43_13(Product x) => 
                x.LastMod;

            internal bool <SearchInternal>b__43_14(Product x) => 
                x.LastMod.HasValue;

            internal DateTime? <SearchInternal>b__43_15(Product x) => 
                x.LastMod;

            internal bool <SearchInternal>b__43_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-new-arrivals-json"));

            internal bool <SearchInternal>b__43_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-collection-json"));

            internal bool <SearchInternal>b__43_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "js-collection-json"));

            internal bool <SearchMulti>b__42_0(Product x) => 
                x.LastMod.HasValue;

            internal DateTime? <SearchMulti>b__42_1(Product x) => 
                x.LastMod;
        }

        [CompilerGenerated]
        private static class <>o__29
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, int, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__19;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__20;
            public static CallSite<Action<CallSite, Type, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, string>> <>p__30;
        }

        [CompilerGenerated]
        private static class <>o__30
        {
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, int, object>> <>p__2;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__5;
            public static CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>> <>p__6;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, int, object>> <>p__9;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__12;
            public static CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>> <>p__13;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, int, object>> <>p__16;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__19;
            public static CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>> <>p__20;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, int, object>> <>p__23;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__26;
            public static CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>> <>p__27;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, int, object>> <>p__30;
            public static CallSite<Action<CallSite, List<Newtonsoft.Json.Linq.JObject>, object>> <>p__31;
        }

        [CompilerGenerated]
        private static class <>o__31
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__8;
            public static CallSite<Func<CallSite, object, bool>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object, object>> <>p__17;
            public static CallSite<Func<CallSite, bool, object, object>> <>p__18;
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
        private static class <>o__33
        {
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, int, object>> <>p__2;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__5;
            public static CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__8;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, int, object>> <>p__11;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__14;
            public static CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__17;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, int, object>> <>p__20;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__23;
            public static CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__26;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, int, object>> <>p__29;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__32;
            public static CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__35;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, int, object>> <>p__38;
            public static CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__41;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__42;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string>> <>p__48;
            public static CallSite<Action<CallSite, Type, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, bool>> <>p__52;
            public static CallSite<Func<CallSite, object, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, bool>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, object>> <>p__58;
            public static CallSite<Func<CallSite, object, object, object>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, int, object>> <>p__63;
            public static CallSite<Func<CallSite, object, object, object>> <>p__64;
            public static CallSite<Func<CallSite, object, bool>> <>p__65;
            public static CallSite<Func<CallSite, object, bool>> <>p__66;
            public static CallSite<Func<CallSite, object, object>> <>p__67;
            public static CallSite<Func<CallSite, object, object>> <>p__68;
            public static CallSite<Func<CallSite, object, string, object>> <>p__69;
            public static CallSite<Func<CallSite, object, object>> <>p__70;
            public static CallSite<Func<CallSite, object, object>> <>p__71;
            public static CallSite<Func<CallSite, object, string, object>> <>p__72;
            public static CallSite<Func<CallSite, object, string, object>> <>p__73;
            public static CallSite<Func<CallSite, object, object>> <>p__74;
            public static CallSite<Func<CallSite, object, object>> <>p__75;
            public static CallSite<Func<CallSite, object, string, object>> <>p__76;
            public static CallSite<Func<CallSite, object, object, object>> <>p__77;
            public static CallSite<Func<CallSite, object, bool>> <>p__78;
            public static CallSite<Func<CallSite, object, bool>> <>p__79;
            public static CallSite<Func<CallSite, object, string, object>> <>p__80;
            public static CallSite<Func<CallSite, object, object>> <>p__81;
            public static CallSite<Func<CallSite, object, object>> <>p__82;
            public static CallSite<Func<CallSite, object, string>> <>p__83;
            public static CallSite<Func<CallSite, object, string, object>> <>p__84;
            public static CallSite<Func<CallSite, object, object>> <>p__85;
            public static CallSite<Func<CallSite, object, object>> <>p__86;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__87;
            public static CallSite<Func<CallSite, object, double>> <>p__88;
            public static CallSite<Func<CallSite, object, string, object>> <>p__89;
            public static CallSite<Func<CallSite, object, object>> <>p__90;
            public static CallSite<Func<CallSite, object, object>> <>p__91;
            public static CallSite<Func<CallSite, object, string>> <>p__92;
            public static CallSite<Func<CallSite, object, string, object>> <>p__93;
            public static CallSite<Func<CallSite, object, object>> <>p__94;
            public static CallSite<Func<CallSite, object, object>> <>p__95;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__96;
            public static CallSite<Func<CallSite, object, double, object>> <>p__97;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__98;
            public static CallSite<Func<CallSite, object, bool>> <>p__99;
            public static CallSite<Func<CallSite, object, bool>> <>p__100;
            public static CallSite<Func<CallSite, object, string, object>> <>p__101;
            public static CallSite<Func<CallSite, object, object>> <>p__102;
            public static CallSite<Func<CallSite, object, object>> <>p__103;
            public static CallSite<Func<CallSite, object, string>> <>p__104;
            public static CallSite<Func<CallSite, object, string, object>> <>p__105;
            public static CallSite<Func<CallSite, object, object>> <>p__106;
            public static CallSite<Func<CallSite, object, object>> <>p__107;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__108;
            public static CallSite<Func<CallSite, object, double>> <>p__109;
            public static CallSite<Func<CallSite, object, string, object>> <>p__110;
            public static CallSite<Func<CallSite, object, object>> <>p__111;
            public static CallSite<Func<CallSite, object, object>> <>p__112;
            public static CallSite<Func<CallSite, object, string>> <>p__113;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__114;
            public static CallSite<Func<CallSite, object, object>> <>p__115;
            public static CallSite<Func<CallSite, object, object>> <>p__116;
            public static CallSite<Func<CallSite, object, object>> <>p__117;
            public static CallSite<Func<CallSite, object, string>> <>p__118;
            public static CallSite<Func<CallSite, object, object>> <>p__119;
            public static CallSite<Func<CallSite, object, string, object>> <>p__120;
            public static CallSite<Func<CallSite, object, object>> <>p__121;
            public static CallSite<Func<CallSite, object, string>> <>p__122;
            public static CallSite<Func<CallSite, object, object>> <>p__123;
            public static CallSite<Func<CallSite, object, object>> <>p__124;
            public static CallSite<Func<CallSite, object, object>> <>p__125;
            public static CallSite<Func<CallSite, object, string, object>> <>p__126;
            public static CallSite<Func<CallSite, object, object>> <>p__127;
            public static CallSite<Func<CallSite, object, string>> <>p__128;
            public static CallSite<Func<CallSite, object, object>> <>p__129;
            public static CallSite<Func<CallSite, object, object>> <>p__130;
            public static CallSite<Func<CallSite, object, object>> <>p__131;
            public static CallSite<Func<CallSite, object, string, object>> <>p__132;
            public static CallSite<Func<CallSite, object, object>> <>p__133;
            public static CallSite<Func<CallSite, object, string>> <>p__134;
            public static CallSite<Func<CallSite, object, object>> <>p__135;
            public static CallSite<Func<CallSite, object, object>> <>p__136;
            public static CallSite<Func<CallSite, object, object>> <>p__137;
            public static CallSite<Func<CallSite, object, string, object>> <>p__138;
            public static CallSite<Func<CallSite, object, object>> <>p__139;
            public static CallSite<Func<CallSite, object, string>> <>p__140;
            public static CallSite<Func<CallSite, object, object>> <>p__141;
            public static CallSite<Func<CallSite, object, object>> <>p__142;
            public static CallSite<Func<CallSite, object, object>> <>p__143;
            public static CallSite<Func<CallSite, object, string, object>> <>p__144;
            public static CallSite<Func<CallSite, object, object>> <>p__145;
            public static CallSite<Func<CallSite, object, string>> <>p__146;
            public static CallSite<Func<CallSite, object, object>> <>p__147;
            public static CallSite<Func<CallSite, object, object>> <>p__148;
            public static CallSite<Func<CallSite, object, object>> <>p__149;
            public static CallSite<Func<CallSite, object, string, object>> <>p__150;
            public static CallSite<Func<CallSite, object, object>> <>p__151;
            public static CallSite<Func<CallSite, Type, TaskObject, object, object>> <>p__152;
            public static CallSite<Func<CallSite, object, string>> <>p__153;
        }

        [CompilerGenerated]
        private static class <>o__36
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, bool, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, bool>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, int, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, bool>> <>p__27;
            public static CallSite<Func<CallSite, object, bool>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, string, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, bool>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, string, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Action<CallSite, Type, TaskObject, States.LOGGER_STATES, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, Encoding, object, object>> <>p__43;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__44;
            public static CallSite<Func<CallSite, string, object, object>> <>p__45;
            public static CallSite<Action<CallSite, Type, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, string, object, object>> <>p__48;
            public static CallSite<Action<CallSite, ILog, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, Encoding, object, object>> <>p__51;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__52;
            public static CallSite<Func<CallSite, string, object, object>> <>p__53;
            public static CallSite<Action<CallSite, Type, object>> <>p__54;
        }

        [CompilerGenerated]
        private static class <>o__37
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__10;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string>> <>p__16;
            public static CallSite<Action<CallSite, Type, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__19;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string>> <>p__25;
            public static CallSite<Action<CallSite, Type, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Action<CallSite, object, Newtonsoft.Json.Linq.JProperty>> <>p__28;
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
        }

        [CompilerGenerated]
        private static class <>o__38
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, bool>> <>p__10;
            public static CallSite<Func<CallSite, object, bool>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__19;
            public static CallSite<Func<CallSite, object, double>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__28;
            public static CallSite<Func<CallSite, object, double, object>> <>p__29;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__30;
            public static CallSite<Func<CallSite, object, bool>> <>p__31;
            public static CallSite<Func<CallSite, object, bool>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, Type, object, CultureInfo, object>> <>p__40;
            public static CallSite<Func<CallSite, object, double>> <>p__41;
            public static CallSite<Func<CallSite, object, string, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string>> <>p__45;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__46;
        }

        [CompilerGenerated]
        private static class <>o__40
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__8;
            public static CallSite<Func<CallSite, object, bool>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object, object>> <>p__17;
            public static CallSite<Func<CallSite, bool, object, object>> <>p__18;
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
        private static class <>o__41
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
            public static CallSite<Func<CallSite, string, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, DateTime?>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__17;
            public static CallSite<Func<CallSite, object, bool>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, string, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string>> <>p__25;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__30;
            public static CallSite<Func<CallSite, object, bool>> <>p__31;
            public static CallSite<Func<CallSite, object, string, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, string, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, string>> <>p__35;
            public static CallSite<Func<CallSite, object, string, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, string>> <>p__38;
            public static CallSite<Func<CallSite, object, string, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, string, object>> <>p__41;
            public static CallSite<Func<CallSite, object, bool>> <>p__42;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__47;
            public static CallSite<Func<CallSite, object, bool>> <>p__48;
            public static CallSite<Func<CallSite, object, string, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, string, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, string>> <>p__52;
            public static CallSite<Func<CallSite, object, string, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, string>> <>p__55;
            public static CallSite<Func<CallSite, object, string, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, string, object>> <>p__58;
            public static CallSite<Func<CallSite, object, bool>> <>p__59;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__60;
        }

        [CompilerGenerated]
        private static class <>o__44
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

