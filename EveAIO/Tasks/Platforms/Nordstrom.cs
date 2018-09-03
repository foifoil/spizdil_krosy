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
    using System.Threading;
    using System.Threading.Tasks;

    internal class Nordstrom : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _mobileLink;
        private string _prodName;
        private string _brandName;
        private object _styleNumber;
        private string _styleId;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private object _bytes;
        [Dynamic]
        private object _dynObj;
        private object _data;
        private string _requestVerificationToken;
        private string _paypageId;
        private string _reportGroup;
        private string _payPageMerchantTxnId;
        private string _addressId;
        private string _sctkn;
        private string _productHtml;
        private Dictionary<string, string> _diData;
        private static string _async;

        public Nordstrom(TaskRunner runner, TaskObject task)
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
            if (this._runner.Profile.CountryIdShipping != "US")
            {
                this._task.Status = States.GetTaskState(States.TaskState.US_SHIPPING_NEEDED);
                States.WriteLogger(this._task, States.LOGGER_STATES.US_SHIPPING_NEEDED, null, "", "");
                return false;
            }
            this.GegSensorData(this._mobileLink);
            try
            {
                if (this._task.TaskType == TaskObject.TaskTypeEnum.variant)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, "", "");
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedNordstromSize.SizeAltered, "");
                }
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                if (this._task.TaskType == TaskObject.TaskTypeEnum.variant)
                {
                    if (string.IsNullOrEmpty(_async))
                    {
                        Task.Factory.StartNew(delegate {
                            _async = this._client.Get("https://secure.nordstrom.com/_bm/async.js").Text();
                        });
                    }
                    this._client.Get("https://m.shop.nordstrom.com/currency/getrate/USD").Text();
                    foreach (Cookie cookie in this._client.Handler.CookieContainer.List())
                    {
                        if (cookie.Name == "internationalshippref")
                        {
                            cookie.Expired = true;
                            cookie.Expires = DateTime.Now.AddDays(-1.0);
                        }
                    }
                    Cookie cookie1 = new Cookie {
                        Name = "internationalshippref",
                        Value = "preferredcountry=US&preferredcurrency=USD&preferredcountryname=United%20States",
                        Domain = "nordstrom.com"
                    };
                    this._client.Handler.CookieContainer.Add(cookie1);
                    this._mobileLink = "https://m.secure.nordstrom.com";
                    this._client.Get("https://m.secure.nordstrom.com/shoppingbag.aspx").Text();
                }
                Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray();
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (this._task.TaskType == TaskObject.TaskTypeEnum.variant)
                {
                    if (<>o__33.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "NewQuantity", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__33.<>p__0.Target(<>o__33.<>p__0, obj2, 1);
                    if (<>o__33.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__1 = CallSite<Func<CallSite, object, long, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "SkuId", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__33.<>p__1.Target(<>o__33.<>p__1, obj2, long.Parse(this._task.SkuId));
                    if (<>o__33.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__2 = CallSite<Func<CallSite, object, long, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StyleId", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__33.<>p__2.Target(<>o__33.<>p__2, obj2, long.Parse(this._task.StyleId));
                }
                else
                {
                    if (<>o__33.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "NewQuantity", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__33.<>p__3.Target(<>o__33.<>p__3, obj2, 1);
                    if (<>o__33.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__4 = CallSite<Func<CallSite, object, long, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "SkuId", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__33.<>p__4.Target(<>o__33.<>p__4, obj2, long.Parse(this._runner.PickedNordstromSize.SkuId));
                    if (<>o__33.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__5 = CallSite<Func<CallSite, object, long, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StyleId", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__33.<>p__5.Target(<>o__33.<>p__5, obj2, long.Parse(this._styleId));
                }
                if (<>o__33.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__6 = CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Nordstrom), argumentInfo));
                }
                <>o__33.<>p__6.Target(<>o__33.<>p__6, array, obj2);
                this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._mobileLink);
                this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Expect", "100-continue");
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Pragma", "no-cache");
                HttpResponseMessage response = this._client.PostPlain("https://m.shop.nordstrom.com/shoppingbag", Newtonsoft.Json.JsonConvert.SerializeObject(array).ToString());
                EveAIO.Extensions.CheckError(response);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
                this._client.Session.DefaultRequestHeaders.Remove("Pragma");
                this._client.Session.DefaultRequestHeaders.Remove("Expect");
                if (<>o__33.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__10 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__33.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__9 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__33.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__33.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__33.<>p__10.Target(<>o__33.<>p__10, <>o__33.<>p__9.Target(<>o__33.<>p__9, <>o__33.<>p__8.Target(<>o__33.<>p__8, <>o__33.<>p__7.Target(<>o__33.<>p__7, this._dynObj, "HasItems")), true)))
                {
                    if (<>o__33.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__33.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__33.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__33.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__33.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__33.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__33.<>p__14.Target(<>o__33.<>p__14, <>o__33.<>p__13.Target(<>o__33.<>p__13, <>o__33.<>p__12.Target(<>o__33.<>p__12, <>o__33.<>p__11.Target(<>o__33.<>p__11, this._dynObj, "ShoppingBagCount")), 0)))
                    {
                        if (this._runner.Product == null)
                        {
                            foreach (Cookie cookie2 in this._client.Handler.CookieContainer.List())
                            {
                                if (cookie2.Name == "internationalshippref")
                                {
                                    cookie2.Expired = true;
                                    cookie2.Expires = DateTime.Now.AddDays(-1.0);
                                }
                            }
                            Cookie cookie = new Cookie {
                                Name = "internationalshippref",
                                Value = "preferredcountry=US&preferredcurrency=USD&preferredcountryname=United%20States",
                                Domain = "nordstrom.com"
                            };
                            this._client.Handler.CookieContainer.Add(cookie);
                            Product product = new Product();
                            if (<>o__33.<>p__19 == null)
                            {
                                <>o__33.<>p__19 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                            }
                            if (<>o__33.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__17 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__16 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Items", typeof(Nordstrom), argumentInfo));
                            }
                            product.ProductTitle = <>o__33.<>p__19.Target(<>o__33.<>p__19, <>o__33.<>p__18.Target(<>o__33.<>p__18, <>o__33.<>p__17.Target(<>o__33.<>p__17, <>o__33.<>p__16.Target(<>o__33.<>p__16, <>o__33.<>p__15.Target(<>o__33.<>p__15, this._dynObj)), "StyleName")));
                            product.Link = "https://shop.nordstrom.com";
                            if (<>o__33.<>p__24 == null)
                            {
                                <>o__33.<>p__24 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                            }
                            if (<>o__33.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__21 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Items", typeof(Nordstrom), argumentInfo));
                            }
                            product.Price = <>o__33.<>p__24.Target(<>o__33.<>p__24, <>o__33.<>p__23.Target(<>o__33.<>p__23, <>o__33.<>p__22.Target(<>o__33.<>p__22, <>o__33.<>p__21.Target(<>o__33.<>p__21, <>o__33.<>p__20.Target(<>o__33.<>p__20, this._dynObj)), "UnitPrice")));
                            this._runner.Product = product;
                            this._task.ImgUrl = "http://eve-robotics.com/dummy_product.png";
                            if (<>o__33.<>p__30 == null)
                            {
                                <>o__33.<>p__30 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                            }
                            if (<>o__33.<>p__29 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__28 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__33.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__27 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Size", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__33.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__33.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Items", typeof(Nordstrom), argumentInfo));
                            }
                            this._task.Size = <>o__33.<>p__30.Target(<>o__33.<>p__30, <>o__33.<>p__29.Target(<>o__33.<>p__29, <>o__33.<>p__28.Target(<>o__33.<>p__28, <>o__33.<>p__27.Target(<>o__33.<>p__27, <>o__33.<>p__26.Target(<>o__33.<>p__26, <>o__33.<>p__25.Target(<>o__33.<>p__25, this._dynObj))), "Message")));
                            if (string.IsNullOrEmpty(this._task.Size))
                            {
                                this._task.Size = "-";
                            }
                            if (this._task.Size.Contains("US"))
                            {
                                this._task.Size = this._task.Size.Substring(0, this._task.Size.IndexOf("US"));
                            }
                            if (this._task.Size.ToLowerInvariant().Contains("women") && this._task.Size.ToLowerInvariant().Contains("men"))
                            {
                                this._task.Size = this._task.Size.Substring(this._task.Size.IndexOf("/") + 1).Trim();
                                this._task.Size = this._task.Size.Substring(0, this._task.Size.IndexOf(" ")).Trim();
                            }
                            this._runner.PickedSize = new KeyValuePair<string, string>(this._task.Size, this._task.SkuId);
                            this._runner.Success.Size = this._runner.PickedSize.Value.Key;
                            this._runner.Success.Price = this._runner.Product.Price;
                            this._runner.Success.Link = this._runner.Product.Link;
                            this._runner.Success.ProductImage = this._task.ImgUrl;
                            this._runner.Success.ProductName = this._runner.Product.ProductTitle;
                            this._runner.Success.Store = EveAIO.Helpers.GetStoreUrl(this._task);
                            if (this._runner.Proxy == null)
                            {
                                this._runner.Success.Proxy = "-";
                            }
                            else
                            {
                                this._runner.Success.Proxy = this._task.Proxy;
                            }
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
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                        return true;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
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
            if (this._runner.Profile.CountryIdShipping != "US")
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.COUNTRY_NOT_SUPPORTED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                return false;
            }
            if (!this.SubmitShipping())
            {
                return false;
            }
            return (this.SubmitBilling() && this.SubmitOrder());
        }

        public bool DirectLink(string link)
        {
            try
            {
                List<NordStromProduct>.Enumerator enumerator3;
                if (this._runner.Profile.CountryIdShipping != "US")
                {
                    this._task.Status = States.GetTaskState(States.TaskState.US_SHIPPING_NEEDED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.US_SHIPPING_NEEDED, null, "", "");
                    return false;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                if (string.IsNullOrEmpty(_async))
                {
                    Task.Factory.StartNew(delegate {
                        _async = this._client.Get("https://secure.nordstrom.com/_bm/async.js").Text();
                    });
                }
                this._client.Get("https://m.shop.nordstrom.com/currency/getrate/USD").Text();
                KeyValuePair<string, string> pair = this._client.Get(link).TextResponseUri();
                this._mobileLink = pair.Value;
                this._srr = pair.Key;
                foreach (Cookie cookie in this._client.Handler.CookieContainer.List())
                {
                    if (cookie.Name == "internationalshippref")
                    {
                        cookie.Expired = true;
                        cookie.Expires = DateTime.Now.AddDays(-1.0);
                    }
                    if (cookie.Name == "rfx-forex-rate")
                    {
                        cookie.Expired = true;
                        cookie.Expires = DateTime.Now.AddDays(-1.0);
                    }
                }
                Cookie cookie1 = new Cookie {
                    Name = "internationalshippref",
                    Value = "preferredcountry=US&preferredcurrency=USD&preferredcountryname=United%20States",
                    Domain = "nordstrom.com"
                };
                this._client.Handler.CookieContainer.Add(cookie1);
                Cookie cookie2 = new Cookie {
                    Name = "rfx-forex-rate",
                    Value = "currencyCode=USD&exchangeRate=1",
                    Domain = "nordstrom.com"
                };
                this._client.Handler.CookieContainer.Add(cookie2);
                this._productHtml = this._srr;
                this._currentDoc.LoadHtml(this._srr);
                string str = this._srr.Substring(this._srr.IndexOf("renderer({"));
                str = str.Substring(str.IndexOf("{"));
                str = str.Substring(0, str.IndexOf("});") + 1);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                object obj2 = null;
                if (<>o__25.<>p__3 == null)
                {
                    <>o__25.<>p__3 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Nordstrom)));
                }
                if (<>o__25.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "data", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "stylesById", typeof(Nordstrom), argumentInfo));
                }
                using (IEnumerator enumerator2 = <>o__25.<>p__3.Target(<>o__25.<>p__3, <>o__25.<>p__1.Target(<>o__25.<>p__1, <>o__25.<>p__0.Target(<>o__25.<>p__0, this._dynObj))).GetEnumerator())
                {
                    if (enumerator2.MoveNext())
                    {
                        object current = enumerator2.Current;
                        if (<>o__25.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Nordstrom), argumentInfo));
                        }
                        obj2 = <>o__25.<>p__2.Target(<>o__25.<>p__2, current);
                    }
                }
                if (<>o__25.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__6 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__7.Target(<>o__25.<>p__7, <>o__25.<>p__6.Target(<>o__25.<>p__6, <>o__25.<>p__5.Target(<>o__25.<>p__5, <>o__25.<>p__4.Target(<>o__25.<>p__4, obj2, "isAvailable")), true)))
                {
                    goto Label_23D1;
                }
                if (<>o__25.<>p__11 == null)
                {
                    <>o__25.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                }
                if (<>o__25.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                this._prodName = <>o__25.<>p__11.Target(<>o__25.<>p__11, <>o__25.<>p__10.Target(<>o__25.<>p__10, <>o__25.<>p__9.Target(<>o__25.<>p__9, <>o__25.<>p__8.Target(<>o__25.<>p__8, obj2, "productName"))));
                if (<>o__25.<>p__16 == null)
                {
                    <>o__25.<>p__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                }
                if (<>o__25.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "brand", typeof(Nordstrom), argumentInfo));
                }
                this._brandName = <>o__25.<>p__16.Target(<>o__25.<>p__16, <>o__25.<>p__15.Target(<>o__25.<>p__15, <>o__25.<>p__14.Target(<>o__25.<>p__14, <>o__25.<>p__13.Target(<>o__25.<>p__13, <>o__25.<>p__12.Target(<>o__25.<>p__12, obj2), "brandName"))));
                if (<>o__25.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__19 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "style", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "price", typeof(Nordstrom), argumentInfo));
                }
                object obj4 = <>o__25.<>p__21.Target(<>o__25.<>p__21, <>o__25.<>p__20.Target(<>o__25.<>p__20, <>o__25.<>p__19.Target(<>o__25.<>p__19, <>o__25.<>p__18.Target(<>o__25.<>p__18, <>o__25.<>p__17.Target(<>o__25.<>p__17, obj2)), "priceString")));
                string str2 = this._currentDoc.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["alt"] != null) && (x.Attributes["alt"].Value == "Product Image 0"))).Attributes["src"].Value;
                if (<>o__25.<>p__25 == null)
                {
                    <>o__25.<>p__25 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                }
                if (<>o__25.<>p__24 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__23 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                this._styleNumber = <>o__25.<>p__25.Target(<>o__25.<>p__25, <>o__25.<>p__24.Target(<>o__25.<>p__24, <>o__25.<>p__23.Target(<>o__25.<>p__23, <>o__25.<>p__22.Target(<>o__25.<>p__22, obj2, "styleNumber"))));
                if (<>o__25.<>p__29 == null)
                {
                    <>o__25.<>p__29 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                }
                if (<>o__25.<>p__28 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__26 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__25.<>p__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                this._styleId = <>o__25.<>p__29.Target(<>o__25.<>p__29, <>o__25.<>p__28.Target(<>o__25.<>p__28, <>o__25.<>p__27.Target(<>o__25.<>p__27, <>o__25.<>p__26.Target(<>o__25.<>p__26, obj2, "id"))));
                this._task.ImgUrl = str2;
                Product product1 = new Product {
                    ProductTitle = this._prodName + " " + this._brandName,
                    Link = link
                };
                if (<>o__25.<>p__30 == null)
                {
                    <>o__25.<>p__30 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                }
                product1.Price = <>o__25.<>p__30.Target(<>o__25.<>p__30, obj4);
                this._runner.Product = product1;
                List<KeyValuePair<string, string>> source = new List<KeyValuePair<string, string>>();
                if (<>o__25.<>p__39 == null)
                {
                    <>o__25.<>p__39 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Nordstrom)));
                }
                if (<>o__25.<>p__32 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "color", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__31 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "selectedChoiceGroup", typeof(Nordstrom), argumentInfo));
                }
                foreach (object obj5 in <>o__25.<>p__39.Target(<>o__25.<>p__39, <>o__25.<>p__32.Target(<>o__25.<>p__32, <>o__25.<>p__31.Target(<>o__25.<>p__31, obj2))))
                {
                    if (<>o__25.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__25.<>p__38 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__25.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__25.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__25.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__25.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__25.<>p__36 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    source.Add(<>o__25.<>p__38.Target(<>o__25.<>p__38, typeof(KeyValuePair<string, string>), <>o__25.<>p__35.Target(<>o__25.<>p__35, <>o__25.<>p__34.Target(<>o__25.<>p__34, <>o__25.<>p__33.Target(<>o__25.<>p__33, obj5, "colorId"))), <>o__25.<>p__37.Target(<>o__25.<>p__37, <>o__25.<>p__36.Target(<>o__25.<>p__36, obj5, "value"))));
                }
                if (<>o__25.<>p__77 == null)
                {
                    <>o__25.<>p__77 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Nordstrom)));
                }
                if (<>o__25.<>p__41 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "byId", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__25.<>p__40 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__25.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "skus", typeof(Nordstrom), argumentInfo));
                }
                foreach (object obj6 in <>o__25.<>p__77.Target(<>o__25.<>p__77, <>o__25.<>p__41.Target(<>o__25.<>p__41, <>o__25.<>p__40.Target(<>o__25.<>p__40, obj2))))
                {
                    if (<>o__25.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__25.<>p__46 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__25.<>p__45 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__25.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__25.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__25.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__25.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                    }
                    if (!<>o__25.<>p__46.Target(<>o__25.<>p__46, <>o__25.<>p__45.Target(<>o__25.<>p__45, <>o__25.<>p__44.Target(<>o__25.<>p__44, <>o__25.<>p__43.Target(<>o__25.<>p__43, <>o__25.<>p__42.Target(<>o__25.<>p__42, obj6), "isAvailable")), true)))
                    {
                        NordStromProduct product = new NordStromProduct();
                        if (<>o__25.<>p__51 == null)
                        {
                            <>o__25.<>p__51 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__25.<>p__50 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__49 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__48 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__47 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        product.SkuId = <>o__25.<>p__51.Target(<>o__25.<>p__51, <>o__25.<>p__50.Target(<>o__25.<>p__50, <>o__25.<>p__49.Target(<>o__25.<>p__49, <>o__25.<>p__48.Target(<>o__25.<>p__48, <>o__25.<>p__47.Target(<>o__25.<>p__47, obj6), "id"))));
                        if (<>o__25.<>p__56 == null)
                        {
                            <>o__25.<>p__56 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__25.<>p__55 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__54 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__53 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__53 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__52 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        product.Color = <>o__25.<>p__56.Target(<>o__25.<>p__56, <>o__25.<>p__55.Target(<>o__25.<>p__55, <>o__25.<>p__54.Target(<>o__25.<>p__54, <>o__25.<>p__53.Target(<>o__25.<>p__53, <>o__25.<>p__52.Target(<>o__25.<>p__52, obj6), "colorId"))));
                        if (<>o__25.<>p__61 == null)
                        {
                            <>o__25.<>p__61 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__25.<>p__60 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__59 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__59 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__58 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__58 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__57 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        product.Size = <>o__25.<>p__61.Target(<>o__25.<>p__61, <>o__25.<>p__60.Target(<>o__25.<>p__60, <>o__25.<>p__59.Target(<>o__25.<>p__59, <>o__25.<>p__58.Target(<>o__25.<>p__58, <>o__25.<>p__57.Target(<>o__25.<>p__57, obj6), "sizeId"))));
                        if (<>o__25.<>p__66 == null)
                        {
                            <>o__25.<>p__66 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__25.<>p__65 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__65 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__64 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__64 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__63 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__63 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__62 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        product.Width = <>o__25.<>p__66.Target(<>o__25.<>p__66, <>o__25.<>p__65.Target(<>o__25.<>p__65, <>o__25.<>p__64.Target(<>o__25.<>p__64, <>o__25.<>p__63.Target(<>o__25.<>p__63, <>o__25.<>p__62.Target(<>o__25.<>p__62, obj6), "widthId"))));
                        if (<>o__25.<>p__71 == null)
                        {
                            <>o__25.<>p__71 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__25.<>p__70 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__69 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__69 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__68 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__68 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__67 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__67 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        product.DisplayPrice = <>o__25.<>p__71.Target(<>o__25.<>p__71, <>o__25.<>p__70.Target(<>o__25.<>p__70, <>o__25.<>p__69.Target(<>o__25.<>p__69, <>o__25.<>p__68.Target(<>o__25.<>p__68, <>o__25.<>p__67.Target(<>o__25.<>p__67, obj6), "displayPrice"))));
                        if (<>o__25.<>p__76 == null)
                        {
                            <>o__25.<>p__76 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__25.<>p__75 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__74 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__73 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__73 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__25.<>p__72 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        product.OriginalPrice = <>o__25.<>p__76.Target(<>o__25.<>p__76, <>o__25.<>p__75.Target(<>o__25.<>p__75, <>o__25.<>p__74.Target(<>o__25.<>p__74, <>o__25.<>p__73.Target(<>o__25.<>p__73, <>o__25.<>p__72.Target(<>o__25.<>p__72, obj6), "displayOriginalPrice"))));
                        NordStromProduct pr = product;
                        pr.Color = source.First<KeyValuePair<string, string>>(x => (x.Key == pr.Color)).Value;
                        if (string.IsNullOrEmpty(pr.Size))
                        {
                            pr.Size = "-";
                        }
                        if (!pr.Size.Contains("US"))
                        {
                            pr.SizeAltered = pr.Size;
                        }
                        else
                        {
                            pr.SizeAltered = pr.Size.Substring(0, pr.Size.IndexOf("US"));
                        }
                        if (pr.Size.ToLowerInvariant().Contains("women") && pr.Size.ToLowerInvariant().Contains("men"))
                        {
                            pr.SizeAltered = pr.Size.Substring(pr.Size.IndexOf("/") + 1).Trim();
                            pr.SizeAltered = pr.SizeAltered.Substring(0, pr.SizeAltered.IndexOf(" ")).Trim();
                        }
                        pr.ColorAltered = pr.Color.Replace("/", "");
                        this._runner.Product.AvailableNordstromSizes.Add(pr);
                    }
                }
                if (this._runner.Product.AvailableNordstromSizes.Count == 0)
                {
                    goto Label_23B3;
                }
                if (this._task.IsNegativeSizing)
                {
                    this._runner.Product.AvailableNordstromSizes = EveAIO.Helpers.ProcessNegativeSizing(this._runner.Product.AvailableNordstromSizes, this._task);
                    if (this._runner.Product.AvailableNordstromSizes.Count == 0)
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        return false;
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
                    double num3 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num3 < this._task.MinimumPrice) || (num3 > this._task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
                if (this._task.RandomSize)
                {
                    goto Label_21A8;
                }
                char[] separator = new char[] { '#' };
                string[] strArray = this._task.Size.Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Trim().ToUpperInvariant();
                }
                string[] strArray2 = strArray;
                int index = 0;
            Label_1F1E:
                if (index >= strArray2.Length)
                {
                    goto Label_20D0;
                }
                string str5 = strArray2[index];
                if (this._runner.PickedSize.HasValue)
                {
                    goto Label_20D0;
                }
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    using (enumerator3 = this._runner.Product.AvailableNordstromSizes.GetEnumerator())
                    {
                        NordStromProduct current;
                        while (enumerator3.MoveNext())
                        {
                            current = enumerator3.Current;
                            if (current.SizeAltered == str5)
                            {
                                goto Label_1F95;
                            }
                        }
                        goto Label_20C5;
                    Label_1F95:
                        this._runner.PickedNordstromSize = current;
                        goto Label_20C5;
                    }
                }
                if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                {
                    using (enumerator3 = this._runner.Product.AvailableNordstromSizes.GetEnumerator())
                    {
                        NordStromProduct product3;
                        goto Label_201C;
                    Label_1FE0:
                        product3 = enumerator3.Current;
                        if ((product3.SizeAltered == str5) && product3.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant()))
                        {
                            goto Label_2027;
                        }
                    Label_201C:
                        if (!enumerator3.MoveNext())
                        {
                            goto Label_20C5;
                        }
                        goto Label_1FE0;
                    Label_2027:
                        this._runner.PickedNordstromSize = product3;
                        goto Label_20C5;
                    }
                }
                using (enumerator3 = this._runner.Product.AvailableNordstromSizes.GetEnumerator())
                {
                    NordStromProduct product4;
                    goto Label_209D;
                Label_2061:
                    product4 = enumerator3.Current;
                    if ((product4.SizeAltered == str5) && (product4.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()))
                    {
                        goto Label_20A8;
                    }
                Label_209D:
                    if (!enumerator3.MoveNext())
                    {
                        goto Label_20C5;
                    }
                    goto Label_2061;
                Label_20A8:
                    this._runner.PickedNordstromSize = product4;
                }
            Label_20C5:
                index++;
                goto Label_1F1E;
            Label_20D0:
                if (this._runner.PickedNordstromSize == null)
                {
                    this._runner.PickedNordstromSize = this._runner.Product.AvailableNordstromSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableNordstromSizes.Count)];
                    this._runner.PickedSize = new KeyValuePair<string, string>(string.IsNullOrEmpty(this._runner.PickedNordstromSize.SizeAltered) ? "-" : this._runner.PickedNordstromSize.SizeAltered, this._runner.PickedNordstromSize.SkuId);
                    return true;
                }
                if (this._task.SizePickRandom || (this._runner.PickedNordstromSize != null))
                {
                    goto Label_2341;
                }
                return false;
            Label_21A8:
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    this._runner.PickedNordstromSize = this._runner.Product.AvailableNordstromSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableNordstromSizes.Count)];
                }
                else if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                {
                    if (!this._runner.Product.AvailableNordstromSizes.Any<NordStromProduct>(x => x.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant())))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                    List<NordStromProduct> list2 = (from x in this._runner.Product.AvailableNordstromSizes
                        where x.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant())
                        select x).ToList<NordStromProduct>();
                    this._runner.PickedNordstromSize = list2[this._runner.Rnd.Next(0, list2.Count)];
                }
                else
                {
                    if (!this._runner.Product.AvailableNordstromSizes.Any<NordStromProduct>(x => (x.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant())))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                    List<NordStromProduct> list3 = (from x in this._runner.Product.AvailableNordstromSizes
                        where x.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()
                        select x).ToList<NordStromProduct>();
                    this._runner.PickedNordstromSize = list3[this._runner.Rnd.Next(0, list3.Count)];
                }
            Label_2341:
                this._runner.PickedSize = new KeyValuePair<string, string>(string.IsNullOrEmpty(this._runner.PickedNordstromSize.SizeAltered) ? "-" : this._runner.PickedNordstromSize.SizeAltered, this._runner.PickedNordstromSize.SkuId);
                this._client.Get("https://m.shop.nordstrom.com/shoppingbag").Text();
                return true;
            Label_23B3:
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            Label_23D1:
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
                return false;
            }
        }

        internal void GegSensorData(string link)
        {
            try
            {
                bool flag = true;
                while (flag)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.GETTING_COOKIES, null, "", "");
                    flag = false;
                    while (string.IsNullOrEmpty(_async))
                    {
                    }
                    string str = Global.SENSOR.GetData(this._productHtml, link, _async, false);
                    try
                    {
                        SensorJson json1 = new SensorJson {
                            sensor_data = str
                        };
                        this._srr = this._client.PostJson("https://m.secure.nordstrom.com/_bm/_data", Newtonsoft.Json.JsonConvert.SerializeObject(json1)).Text();
                        if (!this._srr.Contains("Error") && !this._srr.Contains("{\"success\": false}"))
                        {
                            object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                            if (<>o__31.<>p__3 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__31.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__31.<>p__2 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__31.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__31.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__31.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__31.<>p__0 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__31.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                            }
                            if (<>o__31.<>p__3.Target(<>o__31.<>p__3, <>o__31.<>p__2.Target(<>o__31.<>p__2, <>o__31.<>p__1.Target(<>o__31.<>p__1, <>o__31.<>p__0.Target(<>o__31.<>p__0, obj2, "success")), true)))
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                        continue;
                    }
                    catch (WebException exception1)
                    {
                        if (!exception1.Message.Contains("400"))
                        {
                            throw;
                        }
                        flag = true;
                        continue;
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_GETTING_COOKIES, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                throw;
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 7_0_6 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) Version/7.0 Mobile/11B651 Safari/9537.53");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://m.shop.nordstrom.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://m.shop.nordstrom.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                this._srr = this._client.Get("https://request.eprotect.vantivcnp.com/eProtect/litle-api3.js").Text();
                string str = this._srr.Substring(this._srr.IndexOf("var LitlePayPage=function"));
                str = str.Substring(str.IndexOf("{") + 1);
                str = str.Substring(str.IndexOf("{"));
                str = str.Substring(0, str.IndexOf("}") + 1);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                if (<>o__30.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__30.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                object obj2 = <>o__30.<>p__2.Target(<>o__30.<>p__2, <>o__30.<>p__1.Target(<>o__30.<>p__1, <>o__30.<>p__0.Target(<>o__30.<>p__0, this._dynObj, "keyId")));
                if (<>o__30.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__30.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                object obj3 = <>o__30.<>p__5.Target(<>o__30.<>p__5, <>o__30.<>p__4.Target(<>o__30.<>p__4, <>o__30.<>p__3.Target(<>o__30.<>p__3, this._dynObj, "modulus")));
                if (<>o__30.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__30.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                }
                object obj4 = <>o__30.<>p__8.Target(<>o__30.<>p__8, <>o__30.<>p__7.Target(<>o__30.<>p__7, <>o__30.<>p__6.Target(<>o__30.<>p__6, this._dynObj, "exponent")));
                if (<>o__30.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__30.<>p__9 = CallSite<Func<CallSite, Sensor, object, object, object, string, string, string, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ProcessNordStrom", null, typeof(Nordstrom), argumentInfo));
                }
                object obj5 = <>o__30.<>p__9.Target(<>o__30.<>p__9, Global.SENSOR, obj3, obj4, obj2, this._payPageMerchantTxnId, this._paypageId, this._reportGroup, this._runner.Profile.CCNumber, this._runner.Profile.Cvv);
                long num = EveAIO.Helpers.ToUnixTime(DateTime.UtcNow);
                string str2 = "jQuery224007696117499153043_" + num;
                string str3 = "https://request.eprotect.vantivcnp.com/eProtect/paypage?";
                str3 = (((str3 + "paypageId=" + this._paypageId) + "&reportGroup=" + this._reportGroup) + "&id=" + this._payPageMerchantTxnId) + "&orderId=order_123";
                if (<>o__30.<>p__12 == null)
                {
                    <>o__30.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Nordstrom)));
                }
                if (<>o__30.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__11 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__10 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Nordstrom), argumentInfo));
                }
                str3 = <>o__30.<>p__12.Target(<>o__30.<>p__12, <>o__30.<>p__11.Target(<>o__30.<>p__11, str3, <>o__30.<>p__10.Target(<>o__30.<>p__10, "&encryptedAccount=", obj5)));
                if (<>o__30.<>p__15 == null)
                {
                    <>o__30.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Nordstrom)));
                }
                if (<>o__30.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__14 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Nordstrom), argumentInfo));
                }
                if (<>o__30.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__13 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Nordstrom), argumentInfo));
                }
                str3 = (<>o__30.<>p__15.Target(<>o__30.<>p__15, <>o__30.<>p__14.Target(<>o__30.<>p__14, str3, <>o__30.<>p__13.Target(<>o__30.<>p__13, "&publicKeyId=", obj2))) + "&pciNonSensitive=false") + "&targetServer=primary&jsoncallback=" + str2;
                this._srr = this._client.Get(str3).Text();
                str = this._srr.Substring(this._srr.IndexOf("{"));
                str = str.Substring(0, str.IndexOf("}") + 1);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                if (!str.Contains("Failure"))
                {
                    if (<>o__30.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    object obj6 = <>o__30.<>p__18.Target(<>o__30.<>p__18, <>o__30.<>p__17.Target(<>o__30.<>p__17, <>o__30.<>p__16.Target(<>o__30.<>p__16, this._dynObj, "paypageRegistrationId")));
                    str3 = "https://request.eprotect.vantivcnp.com/eProtect/ppstats?";
                    str3 = ((((((((str3 + "paypageId=" + this._paypageId) + "&responseTime=1229") + "&responseCode=870" + "&tzOffset=-480") + "&timeout=10000" + "&reportGroup=*merchant1500") + "&txnId=" + this._payPageMerchantTxnId) + "&orderId=order_123") + "&startTime=" + EveAIO.Helpers.ToUnixTime(DateTime.UtcNow)) + "&targetServer=primary") + "&jsoncallback=" + str2;
                    this._srr = this._client.Get(str3).Text();
                    string str4 = "";
                    switch (this._runner.Profile.CardTypeId)
                    {
                        case "0":
                            str4 = "AX";
                            break;

                        case "1":
                            str4 = "VI";
                            break;

                        case "2":
                            str4 = "MC";
                            break;
                    }
                    object obj7 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__30.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__19 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__19.Target(<>o__30.<>p__19, obj7, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__21 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Id", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__21.Target(<>o__30.<>p__21, <>o__30.<>p__20.Target(<>o__30.<>p__20, obj7), int.Parse(this._addressId));
                    if (<>o__30.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FirstName", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__23.Target(<>o__30.<>p__23, <>o__30.<>p__22.Target(<>o__30.<>p__22, obj7), this._runner.Profile.FirstName);
                    if (<>o__30.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "MiddleInitial", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__25.Target(<>o__30.<>p__25, <>o__30.<>p__24.Target(<>o__30.<>p__24, obj7), "");
                    if (<>o__30.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "LastName", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__27.Target(<>o__30.<>p__27, <>o__30.<>p__26.Target(<>o__30.<>p__26, obj7), this._runner.Profile.LastName);
                    if (<>o__30.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Line1", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__29.Target(<>o__30.<>p__29, <>o__30.<>p__28.Target(<>o__30.<>p__28, obj7), this._runner.Profile.Address1);
                    if (<>o__30.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Line2", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__31.Target(<>o__30.<>p__31, <>o__30.<>p__30.Target(<>o__30.<>p__30, obj7), this._runner.Profile.Address2);
                    if (<>o__30.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "City", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__33.Target(<>o__30.<>p__33, <>o__30.<>p__32.Target(<>o__30.<>p__32, obj7), this._runner.Profile.City);
                    if (<>o__30.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__35 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StateProvinceId", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__35.Target(<>o__30.<>p__35, <>o__30.<>p__34.Target(<>o__30.<>p__34, obj7), int.Parse(WebsitesInfo.GetNordstromgStateId(this._runner.Profile.StateId)));
                    if (<>o__30.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__37 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CountryId", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__37.Target(<>o__30.<>p__37, <>o__30.<>p__36.Target(<>o__30.<>p__36, obj7), int.Parse(WebsitesInfo.GetNordstromCountryId(this._runner.Profile.Country)));
                    if (<>o__30.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__39 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ZipCode", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BillingAddress", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__39.Target(<>o__30.<>p__39, <>o__30.<>p__38.Target(<>o__30.<>p__38, obj7), this._runner.Profile.Zip);
                    if (<>o__30.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__40 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__40.Target(<>o__30.<>p__40, obj7, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__42 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Id", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__42.Target(<>o__30.<>p__42, <>o__30.<>p__41.Target(<>o__30.<>p__41, obj7), 0);
                    if (<>o__30.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__44 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CardType", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__44.Target(<>o__30.<>p__44, <>o__30.<>p__43.Target(<>o__30.<>p__43, obj7), str4);
                    if (<>o__30.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__46 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CreditCardToken", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__46.Target(<>o__30.<>p__46, <>o__30.<>p__45.Target(<>o__30.<>p__45, obj7), obj6);
                    if (<>o__30.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ProtectionSource", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__48.Target(<>o__30.<>p__48, <>o__30.<>p__47.Target(<>o__30.<>p__47, obj7), "V");
                    if (<>o__30.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__50 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ProtectionMethod", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__50.Target(<>o__30.<>p__50, <>o__30.<>p__49.Target(<>o__30.<>p__49, obj7), "Tokenized");
                    if (<>o__30.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__30.<>p__52 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Number", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__52.Target(<>o__30.<>p__52, <>o__30.<>p__51.Target(<>o__30.<>p__51, obj7), null);
                    if (<>o__30.<>p__54 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__54 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "SecurityCode", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__53 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__53 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__54.Target(<>o__30.<>p__54, <>o__30.<>p__53.Target(<>o__30.<>p__53, obj7), this._runner.Profile.Cvv);
                    if (<>o__30.<>p__56 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__56 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ExpirationMonth", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__55 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__56.Target(<>o__30.<>p__56, <>o__30.<>p__55.Target(<>o__30.<>p__55, obj7), int.Parse(this._runner.Profile.ExpiryMonth));
                    if (<>o__30.<>p__58 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__58 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ExpirationYear", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__57 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__58.Target(<>o__30.<>p__58, <>o__30.<>p__57.Target(<>o__30.<>p__57, obj7), int.Parse(this._runner.Profile.ExpiryYear));
                    if (<>o__30.<>p__60 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__60 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "IsSaveOptionEnabled", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__59 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__59 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__60.Target(<>o__30.<>p__60, <>o__30.<>p__59.Target(<>o__30.<>p__59, obj7), false);
                    if (<>o__30.<>p__62 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__62 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "IsDefaultCard", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__61 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__61 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CreditCard", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__62.Target(<>o__30.<>p__62, <>o__30.<>p__61.Target(<>o__30.<>p__61, obj7), false);
                    if (<>o__30.<>p__63 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__63 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "EmailSubscribed", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__63.Target(<>o__30.<>p__63, obj7, false);
                    if (<>o__30.<>p__64 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__64 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ContactInfo", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__64.Target(<>o__30.<>p__64, obj7, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__30.<>p__66 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__66 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "EmailAddress", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__65 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__65 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContactInfo", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__66.Target(<>o__30.<>p__66, <>o__30.<>p__65.Target(<>o__30.<>p__65, obj7), this._runner.Profile.Email);
                    if (<>o__30.<>p__68 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__68 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "PhoneNumber", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__67 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__67 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContactInfo", typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__68.Target(<>o__30.<>p__68, <>o__30.<>p__67.Target(<>o__30.<>p__67, obj7), this._runner.Profile.Phone);
                    this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://m.secure.nordstrom.com/os");
                    this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Pragma", "no-cache");
                    this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "no-cache");
                    this._client.Session.DefaultRequestHeaders.Remove("Upgrade-Insecure-Requests");
                    if (<>o__30.<>p__70 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__70 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__69 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__69 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Nordstrom), argumentInfo));
                    }
                    object obj8 = <>o__30.<>p__70.Target(<>o__30.<>p__70, this._client, "https://m.secure.nordstrom.com/os/savepayment", <>o__30.<>p__69.Target(<>o__30.<>p__69, typeof(Newtonsoft.Json.JsonConvert), obj7));
                    if (<>o__30.<>p__71 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__71 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Nordstrom), argumentInfo));
                    }
                    <>o__30.<>p__71.Target(<>o__30.<>p__71, typeof(EveAIO.Extensions), obj8);
                    if (<>o__30.<>p__76 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__76 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__75 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__74 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__73 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__73 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__72 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Nordstrom), argumentInfo));
                    }
                    this._dynObj = <>o__30.<>p__76.Target(<>o__30.<>p__76, typeof(Newtonsoft.Json.JsonConvert), <>o__30.<>p__75.Target(<>o__30.<>p__75, <>o__30.<>p__74.Target(<>o__30.<>p__74, <>o__30.<>p__73.Target(<>o__30.<>p__73, <>o__30.<>p__72.Target(<>o__30.<>p__72, obj8)))));
                    if (<>o__30.<>p__80 == null)
                    {
                        <>o__30.<>p__80 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                    }
                    if (<>o__30.<>p__79 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__79 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__78 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__78 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__30.<>p__77 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__77 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    this._sctkn = <>o__30.<>p__80.Target(<>o__30.<>p__80, <>o__30.<>p__79.Target(<>o__30.<>p__79, <>o__30.<>p__78.Target(<>o__30.<>p__78, <>o__30.<>p__77.Target(<>o__30.<>p__77, this._dynObj, "Sctkn"))));
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_PROCESSING_CC, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
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
                    string str6 = "";
                    if ((exception is WebException) && (((WebException) exception).Response != null))
                    {
                        using (Stream stream = ((WebException) exception).Response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                str6 = reader.ReadToEnd();
                            }
                        }
                    }
                    string str7 = "";
                    if (str6.Contains("\"Errors\":[{\""))
                    {
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str6);
                        if (<>o__30.<>p__85 == null)
                        {
                            <>o__30.<>p__85 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__30.<>p__84 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__30.<>p__84 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__30.<>p__83 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__83 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__30.<>p__82 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__30.<>p__82 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__30.<>p__81 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__30.<>p__81 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        str7 = <>o__30.<>p__85.Target(<>o__30.<>p__85, <>o__30.<>p__84.Target(<>o__30.<>p__84, <>o__30.<>p__83.Target(<>o__30.<>p__83, <>o__30.<>p__82.Target(<>o__30.<>p__82, <>o__30.<>p__81.Target(<>o__30.<>p__81, this._dynObj, "Errors")), "Id")));
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    if (!string.IsNullOrEmpty(str7))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception, "", str7);
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

        private bool SubmitOrder()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                object obj2 = new Newtonsoft.Json.Linq.JObject();
                if (<>o__29.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__29.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ScTkn", typeof(Nordstrom), argumentInfo));
                }
                <>o__29.<>p__0.Target(<>o__29.<>p__0, obj2, this._sctkn);
                if (<>o__29.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__29.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "EmailSubscribed", typeof(Nordstrom), argumentInfo));
                }
                <>o__29.<>p__1.Target(<>o__29.<>p__1, obj2, null);
                if (<>o__29.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__29.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "HasUserSkippedLoyalty", typeof(Nordstrom), argumentInfo));
                }
                <>o__29.<>p__2.Target(<>o__29.<>p__2, obj2, false);
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
                if (Global.SERIAL != "EVE-1111111111111")
                {
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
                        if (<>o__29.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__4 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Nordstrom), argumentInfo));
                        }
                        object obj3 = <>o__29.<>p__4.Target(<>o__29.<>p__4, this._client, "https://m.secure.nordstrom.com/os/placeorder", <>o__29.<>p__3.Target(<>o__29.<>p__3, typeof(Newtonsoft.Json.JsonConvert), obj2));
                        if (<>o__29.<>p__9 == null)
                        {
                            <>o__29.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__29.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Nordstrom), argumentInfo));
                        }
                        this._srr = <>o__29.<>p__9.Target(<>o__29.<>p__9, <>o__29.<>p__8.Target(<>o__29.<>p__8, <>o__29.<>p__7.Target(<>o__29.<>p__7, <>o__29.<>p__6.Target(<>o__29.<>p__6, <>o__29.<>p__5.Target(<>o__29.<>p__5, obj3)))));
                        if (<>o__29.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__10 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Nordstrom), argumentInfo));
                        }
                        <>o__29.<>p__10.Target(<>o__29.<>p__10, typeof(EveAIO.Extensions), obj3);
                    }
                    catch (Exception)
                    {
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                        if (<>o__29.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__14.Target(<>o__29.<>p__14, <>o__29.<>p__13.Target(<>o__29.<>p__13, <>o__29.<>p__12.Target(<>o__29.<>p__12, <>o__29.<>p__11.Target(<>o__29.<>p__11, this._dynObj, "Errors")), 0)))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                            this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                            return false;
                        }
                    }
                    try
                    {
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                        if (<>o__29.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__18 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Text", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__17 = CallSite<Func<CallSite, Client, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Get", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        <>o__29.<>p__18.Target(<>o__29.<>p__18, <>o__29.<>p__17.Target(<>o__29.<>p__17, this._client, <>o__29.<>p__16.Target(<>o__29.<>p__16, <>o__29.<>p__15.Target(<>o__29.<>p__15, this._dynObj, "OrderConfirmationUrl"))));
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
                    if ((exception is WebException) && (((WebException) exception).Response != null))
                    {
                        using (Stream stream = ((WebException) exception).Response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                str = reader.ReadToEnd();
                            }
                        }
                    }
                    string str2 = "";
                    if (str.Contains("\"Errors\":[{\""))
                    {
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                        if (<>o__29.<>p__23 == null)
                        {
                            <>o__29.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__29.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__29.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        str2 = <>o__29.<>p__23.Target(<>o__29.<>p__23, <>o__29.<>p__22.Target(<>o__29.<>p__22, <>o__29.<>p__21.Target(<>o__29.<>p__21, <>o__29.<>p__20.Target(<>o__29.<>p__20, <>o__29.<>p__19.Target(<>o__29.<>p__19, this._dynObj, "Errors")), "Id")));
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    if (!string.IsNullOrEmpty(str2))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", str2);
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
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

        private bool SubmitShipping()
        {
            this.GegSensorData("https://m.shop.nordstrom.com/shoppingbag");
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                bool flag = true;
                int num2 = 0;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        num2++;
                        this._srr = this._client.Get("https://m.secure.nordstrom.com/ShoppingBag.aspx?bco=1 ").Text();
                        continue;
                    }
                    catch (Exception exception1)
                    {
                        if (!exception1.Message.Contains("400") || (num2 >= 4))
                        {
                            throw;
                        }
                        flag = true;
                        continue;
                    }
                }
                if (!this._srr.Contains("class=\"error\">Sold out"))
                {
                    string str = this._srr.Substring(this._srr.IndexOf("paypageRequestSettings"));
                    str = str.Substring(str.IndexOf("{"));
                    str = str.Substring(0, str.IndexOf("}") + 1);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                    if (<>o__32.<>p__3 == null)
                    {
                        <>o__32.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                    }
                    if (<>o__32.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__32.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__32.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    this._paypageId = <>o__32.<>p__3.Target(<>o__32.<>p__3, <>o__32.<>p__2.Target(<>o__32.<>p__2, <>o__32.<>p__1.Target(<>o__32.<>p__1, <>o__32.<>p__0.Target(<>o__32.<>p__0, this._dynObj, "PayPageId"))));
                    if (<>o__32.<>p__7 == null)
                    {
                        <>o__32.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                    }
                    if (<>o__32.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__32.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__32.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    this._reportGroup = <>o__32.<>p__7.Target(<>o__32.<>p__7, <>o__32.<>p__6.Target(<>o__32.<>p__6, <>o__32.<>p__5.Target(<>o__32.<>p__5, <>o__32.<>p__4.Target(<>o__32.<>p__4, this._dynObj, "PayPageReportGroup"))));
                    if (<>o__32.<>p__11 == null)
                    {
                        <>o__32.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                    }
                    if (<>o__32.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__32.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__32.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                    }
                    if (<>o__32.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__32.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                    }
                    this._payPageMerchantTxnId = <>o__32.<>p__11.Target(<>o__32.<>p__11, <>o__32.<>p__10.Target(<>o__32.<>p__10, <>o__32.<>p__9.Target(<>o__32.<>p__9, <>o__32.<>p__8.Target(<>o__32.<>p__8, this._dynObj, "PayPageMerchantTxnId"))));
                    this._requestVerificationToken = this._srr.Substring(this._srr.IndexOf("requestVerificationToken"));
                    this._requestVerificationToken = this._requestVerificationToken.Substring(this._requestVerificationToken.IndexOf("'") + 1);
                    this._requestVerificationToken = this._requestVerificationToken.Substring(0, this._requestVerificationToken.IndexOf("'"));
                    this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("RequestVerificationToken", this._requestVerificationToken);
                    this.GegSensorData("https://m.secure.nordstrom.com/os");
                    this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                    this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://m.secure.nordstrom.com/os");
                    flag = true;
                    num2 = 0;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            num2++;
                            this._srr = this._client.Get("https://m.secure.nordstrom.com/os/Initialize").Text();
                            continue;
                        }
                        catch (Exception exception2)
                        {
                            if (!exception2.Message.Contains("400") || (num2 >= 4))
                            {
                                throw;
                            }
                            flag = true;
                            continue;
                        }
                    }
                    if (!this._srr.Contains("class=\"error\">Sold out"))
                    {
                        this._client.PostJson("https://m.secure.nordstrom.com/os/guest", Newtonsoft.Json.JsonConvert.SerializeObject(new Newtonsoft.Json.Linq.JObject())).Text();
                        object obj3 = new Newtonsoft.Json.Linq.JObject();
                        if (<>o__32.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__12 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__12.Target(<>o__32.<>p__12, obj3, new Newtonsoft.Json.Linq.JObject());
                        if (<>o__32.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__14 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Id", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__14.Target(<>o__32.<>p__14, <>o__32.<>p__13.Target(<>o__32.<>p__13, obj3), 0);
                        if (<>o__32.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FirstName", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__16.Target(<>o__32.<>p__16, <>o__32.<>p__15.Target(<>o__32.<>p__15, obj3), this._runner.Profile.FirstNameShipping);
                        if (<>o__32.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "LastName", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__18.Target(<>o__32.<>p__18, <>o__32.<>p__17.Target(<>o__32.<>p__17, obj3), this._runner.Profile.LastNameShipping);
                        if (<>o__32.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Line1", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__20.Target(<>o__32.<>p__20, <>o__32.<>p__19.Target(<>o__32.<>p__19, obj3), this._runner.Profile.Address1Shipping);
                        if (<>o__32.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Line2", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__22.Target(<>o__32.<>p__22, <>o__32.<>p__21.Target(<>o__32.<>p__21, obj3), this._runner.Profile.Address2Shipping);
                        if (<>o__32.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "City", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__23 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__24.Target(<>o__32.<>p__24, <>o__32.<>p__23.Target(<>o__32.<>p__23, obj3), this._runner.Profile.CityShipping);
                        if (<>o__32.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__26 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StateProvinceId", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__26.Target(<>o__32.<>p__26, <>o__32.<>p__25.Target(<>o__32.<>p__25, obj3), int.Parse(WebsitesInfo.GetNordstromgStateId(this._runner.Profile.StateIdShipping)));
                        if (<>o__32.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ZipCode", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__28.Target(<>o__32.<>p__28, <>o__32.<>p__27.Target(<>o__32.<>p__27, obj3), this._runner.Profile.ZipShipping);
                        if (<>o__32.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__30 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "PhoneNumber", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__30.Target(<>o__32.<>p__30, <>o__32.<>p__29.Target(<>o__32.<>p__29, obj3), this._runner.Profile.PhoneShipping);
                        if (<>o__32.<>p__32 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__32 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "isAddressValidated", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingAddress", typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__32.Target(<>o__32.<>p__32, <>o__32.<>p__31.Target(<>o__32.<>p__31, obj3), false);
                        if (<>o__32.<>p__34 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__34 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__33 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Nordstrom), argumentInfo));
                        }
                        object obj2 = <>o__32.<>p__34.Target(<>o__32.<>p__34, this._client, "https://m.secure.nordstrom.com/os/SaveShippingAddress", <>o__32.<>p__33.Target(<>o__32.<>p__33, typeof(Newtonsoft.Json.JsonConvert), obj3));
                        if (<>o__32.<>p__35 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__35 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Nordstrom), argumentInfo));
                        }
                        <>o__32.<>p__35.Target(<>o__32.<>p__35, typeof(EveAIO.Extensions), obj2);
                        if (<>o__32.<>p__40 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__40 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__39 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__38 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__37 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__36 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Nordstrom), argumentInfo));
                        }
                        this._dynObj = <>o__32.<>p__40.Target(<>o__32.<>p__40, typeof(Newtonsoft.Json.JsonConvert), <>o__32.<>p__39.Target(<>o__32.<>p__39, <>o__32.<>p__38.Target(<>o__32.<>p__38, <>o__32.<>p__37.Target(<>o__32.<>p__37, <>o__32.<>p__36.Target(<>o__32.<>p__36, obj2)))));
                        if (<>o__32.<>p__47 == null)
                        {
                            <>o__32.<>p__47 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__32.<>p__46 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__45 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__44 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__44 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__43 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__43 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__42 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AddressEntries", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__41 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AddressBook", typeof(Nordstrom), argumentInfo));
                        }
                        this._addressId = <>o__32.<>p__47.Target(<>o__32.<>p__47, <>o__32.<>p__46.Target(<>o__32.<>p__46, <>o__32.<>p__45.Target(<>o__32.<>p__45, <>o__32.<>p__44.Target(<>o__32.<>p__44, <>o__32.<>p__43.Target(<>o__32.<>p__43, <>o__32.<>p__42.Target(<>o__32.<>p__42, <>o__32.<>p__41.Target(<>o__32.<>p__41, this._dynObj))), "Id"))));
                        return true;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
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
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    string str2 = "";
                    if ((exception is WebException) && (((WebException) exception).Response != null))
                    {
                        using (Stream stream = ((WebException) exception).Response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                str2 = reader.ReadToEnd();
                            }
                        }
                    }
                    string str3 = "";
                    if (str2.Contains("\"Errors\":[{\""))
                    {
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str2);
                        if (<>o__32.<>p__52 == null)
                        {
                            <>o__32.<>p__52 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Nordstrom)));
                        }
                        if (<>o__32.<>p__51 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__50 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__50 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__49 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__49 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Nordstrom), argumentInfo));
                        }
                        if (<>o__32.<>p__48 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__48 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Nordstrom), argumentInfo));
                        }
                        str3 = <>o__32.<>p__52.Target(<>o__32.<>p__52, <>o__32.<>p__51.Target(<>o__32.<>p__51, <>o__32.<>p__50.Target(<>o__32.<>p__50, <>o__32.<>p__49.Target(<>o__32.<>p__49, <>o__32.<>p__48.Target(<>o__32.<>p__48, this._dynObj, "Errors")), "Id")));
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception, "", str3);
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception, "", "");
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

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Nordstrom.<>c <>9;
            public static Func<HtmlNode, bool> <>9__25_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Nordstrom.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__25_1(HtmlNode x) => 
                ((x.Attributes["alt"] != null) && (x.Attributes["alt"].Value == "Product Image 0"));
        }

        [CompilerGenerated]
        private static class <>o__25
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string>> <>p__25;
            public static CallSite<Func<CallSite, object, string, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string>> <>p__29;
            public static CallSite<Func<CallSite, object, string>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object>> <>p__37;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__38;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__45;
            public static CallSite<Func<CallSite, object, bool>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, string>> <>p__51;
            public static CallSite<Func<CallSite, object, object>> <>p__52;
            public static CallSite<Func<CallSite, object, string, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, string>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, string, object>> <>p__58;
            public static CallSite<Func<CallSite, object, object>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, string, object>> <>p__63;
            public static CallSite<Func<CallSite, object, object>> <>p__64;
            public static CallSite<Func<CallSite, object, object>> <>p__65;
            public static CallSite<Func<CallSite, object, string>> <>p__66;
            public static CallSite<Func<CallSite, object, object>> <>p__67;
            public static CallSite<Func<CallSite, object, string, object>> <>p__68;
            public static CallSite<Func<CallSite, object, object>> <>p__69;
            public static CallSite<Func<CallSite, object, object>> <>p__70;
            public static CallSite<Func<CallSite, object, string>> <>p__71;
            public static CallSite<Func<CallSite, object, object>> <>p__72;
            public static CallSite<Func<CallSite, object, string, object>> <>p__73;
            public static CallSite<Func<CallSite, object, object>> <>p__74;
            public static CallSite<Func<CallSite, object, object>> <>p__75;
            public static CallSite<Func<CallSite, object, string>> <>p__76;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__77;
        }

        [CompilerGenerated]
        private static class <>o__29
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__3;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string>> <>p__9;
            public static CallSite<Action<CallSite, Type, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, int, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, Client, object, object>> <>p__17;
            public static CallSite<Action<CallSite, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string>> <>p__23;
        }

        [CompilerGenerated]
        private static class <>o__30
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, Sensor, object, object, object, string, string, string, string, string, object>> <>p__9;
            public static CallSite<Func<CallSite, string, object, object>> <>p__10;
            public static CallSite<Func<CallSite, string, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string>> <>p__12;
            public static CallSite<Func<CallSite, string, object, object>> <>p__13;
            public static CallSite<Func<CallSite, string, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, int, object>> <>p__21;
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
            public static CallSite<Func<CallSite, object, int, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, int, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, string, object>> <>p__39;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, int, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, string, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, object, object>> <>p__52;
            public static CallSite<Func<CallSite, object, object>> <>p__53;
            public static CallSite<Func<CallSite, object, string, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, int, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, int, object>> <>p__58;
            public static CallSite<Func<CallSite, object, object>> <>p__59;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__60;
            public static CallSite<Func<CallSite, object, object>> <>p__61;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__62;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__63;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__64;
            public static CallSite<Func<CallSite, object, object>> <>p__65;
            public static CallSite<Func<CallSite, object, string, object>> <>p__66;
            public static CallSite<Func<CallSite, object, object>> <>p__67;
            public static CallSite<Func<CallSite, object, string, object>> <>p__68;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__69;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__70;
            public static CallSite<Action<CallSite, Type, object>> <>p__71;
            public static CallSite<Func<CallSite, object, object>> <>p__72;
            public static CallSite<Func<CallSite, object, object>> <>p__73;
            public static CallSite<Func<CallSite, object, object>> <>p__74;
            public static CallSite<Func<CallSite, object, object>> <>p__75;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__76;
            public static CallSite<Func<CallSite, object, string, object>> <>p__77;
            public static CallSite<Func<CallSite, object, object>> <>p__78;
            public static CallSite<Func<CallSite, object, object>> <>p__79;
            public static CallSite<Func<CallSite, object, string>> <>p__80;
            public static CallSite<Func<CallSite, object, string, object>> <>p__81;
            public static CallSite<Func<CallSite, object, object>> <>p__82;
            public static CallSite<Func<CallSite, object, string, object>> <>p__83;
            public static CallSite<Func<CallSite, object, object>> <>p__84;
            public static CallSite<Func<CallSite, object, string>> <>p__85;
        }

        [CompilerGenerated]
        private static class <>o__31
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__32
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string>> <>p__11;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, int, object>> <>p__14;
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
            public static CallSite<Func<CallSite, object, int, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, string, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__32;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__33;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__34;
            public static CallSite<Action<CallSite, Type, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, object>> <>p__43;
            public static CallSite<Func<CallSite, object, string, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, string>> <>p__47;
            public static CallSite<Func<CallSite, object, string, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, string, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, string>> <>p__52;
        }

        [CompilerGenerated]
        private static class <>o__33
        {
            public static CallSite<Func<CallSite, object, int, object>> <>p__0;
            public static CallSite<Func<CallSite, object, long, object>> <>p__1;
            public static CallSite<Func<CallSite, object, long, object>> <>p__2;
            public static CallSite<Func<CallSite, object, int, object>> <>p__3;
            public static CallSite<Func<CallSite, object, long, object>> <>p__4;
            public static CallSite<Func<CallSite, object, long, object>> <>p__5;
            public static CallSite<Action<CallSite, Newtonsoft.Json.Linq.JArray, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__9;
            public static CallSite<Func<CallSite, object, bool>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, int, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, string>> <>p__30;
        }
    }
}

