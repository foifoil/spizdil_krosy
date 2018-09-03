namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Properties;
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

    internal class Funko : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private bool _isSpConfig;
        private string _productId;
        private List<KeyValuePair<string, string>> _config;
        private string _formKey;
        private string _regionIdShipping;
        private string _regionShipping;
        private string _regionIdBilling;
        private string _regionBilling;
        private string _cartId;
        private Dictionary<string, string> _diData;

        public Funko(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._config = new List<KeyValuePair<string, string>>();
            this._regionIdShipping = "";
            this._regionShipping = "";
            this._regionIdBilling = "";
            this._regionBilling = "";
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
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_addtocart_form"))).Attributes["action"].Value;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    if (!this._isSpConfig)
                    {
                        this._diData.Add("product", this._productId);
                        this._diData.Add("selected_configurable_option", "");
                        this._diData.Add("related_product", "");
                        this._diData.Add("form_key", this._formKey);
                        this._diData.Add("qty", "1");
                    }
                    else
                    {
                        this._diData.Add("product", this._productId);
                        this._diData.Add("selected_configurable_option", this._config.First<KeyValuePair<string, string>>(x => (x.Key == this._runner.PickedSize.Value.Value)).Value);
                        this._diData.Add("related_product", "");
                        this._diData.Add("form_key", this._formKey);
                        this._diData.Add("super_attribute[184]", this._runner.PickedSize.Value.Value);
                        this._diData.Add("qty", "1");
                    }
                    try
                    {
                        this._srr = this._client.Post(url, this._diData).Text();
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
                        this._srr = this._client.Get("https://shop.funko.com/sso/Customer/Cart").Text();
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
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__18.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                }
                if (<>o__18.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Funko), argumentInfo));
                }
                if (<>o__18.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                }
                if (<>o__18.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                }
                object obj2 = <>o__18.<>p__2.Target(<>o__18.<>p__2, <>o__18.<>p__1.Target(<>o__18.<>p__1, <>o__18.<>p__0.Target(<>o__18.<>p__0, this._dynObj, "success")), true);
                if (<>o__18.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Funko), argumentInfo));
                }
                if (!<>o__18.<>p__8.Target(<>o__18.<>p__8, obj2))
                {
                    if (<>o__18.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__18.<>p__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Funko), argumentInfo));
                    }
                    if (<>o__18.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__18.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Funko), argumentInfo));
                    }
                    if (<>o__18.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__18.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Funko), argumentInfo));
                    }
                    if (<>o__18.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__18.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                    }
                }
                if (<>o__18.<>p__9.Target(<>o__18.<>p__9, (<>o__18.<>p__3 != null) ? obj2 : <>o__18.<>p__7.Target(<>o__18.<>p__7, obj2, <>o__18.<>p__6.Target(<>o__18.<>p__6, <>o__18.<>p__5.Target(<>o__18.<>p__5, <>o__18.<>p__4.Target(<>o__18.<>p__4, <>o__18.<>p__3.Target(<>o__18.<>p__3, this._dynObj, "cart_qty"))), "0"))))
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
            // This item is obfuscated and can not be translated.
            if (this.SubmitShipping())
            {
                goto Label_004B;
            }
        Label_001C:
            switch (((-1344825306 ^ -1136973909) % 5))
            {
                case 1:
                    break;

                case 2:
                    return false;

                case 3:
                    goto Label_001C;

                case 4:
                    return false;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (this.SubmitBilling())
            {
            }
            if (-1610314225 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            try
            {
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", link);
                }
                bool flag = true;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get("https://shop.funko.com/banner/ajax/load/?sections=&_=1524458013112").Text();
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
                        this._srr = this._client.Get(link).Text();
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
                this._currentDoc = new HtmlDocument();
                this._currentDoc.LoadHtml(this._srr);
                this._currentDoc.LoadHtml(this._srr);
                if (!this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_addtocart_form"))))
                {
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    }
                    return false;
                }
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str2 = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-info-title"))).InnerText.Trim();
                string str3 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"))).InnerText.Trim();
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("meta").First<HtmlNode>(x => ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"))).Attributes["content"].Value;
                Product product1 = new Product {
                    ProductTitle = str2,
                    Link = link,
                    Price = str3
                };
                this._runner.Product = product1;
                string str = "";
                if (this._srr.Contains("spConfig"))
                {
                    this._isSpConfig = true;
                    str = this._srr.Substring(this._srr.IndexOf("spConfig"));
                    str = str.Substring(str.IndexOf("{"));
                    str = str.Substring(0, str.IndexOf("}}},") + 3);
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                    new List<Tuple<string, string, string>>();
                    if (<>o__23.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__23.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                    }
                    if (<>o__23.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__23.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "attributes", typeof(Funko), argumentInfo));
                    }
                    object obj2 = <>o__23.<>p__1.Target(<>o__23.<>p__1, <>o__23.<>p__0.Target(<>o__23.<>p__0, this._dynObj), "184");
                    if (<>o__23.<>p__11 == null)
                    {
                        <>o__23.<>p__11 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Funko)));
                    }
                    if (<>o__23.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__23.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "options", typeof(Funko), argumentInfo));
                    }
                    foreach (object obj3 in <>o__23.<>p__11.Target(<>o__23.<>p__11, <>o__23.<>p__2.Target(<>o__23.<>p__2, obj2)))
                    {
                        if (<>o__23.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__23.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__23.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        object obj4 = <>o__23.<>p__4.Target(<>o__23.<>p__4, <>o__23.<>p__3.Target(<>o__23.<>p__3, obj3, "label"));
                        if (<>o__23.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__23.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__23.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        object obj5 = <>o__23.<>p__6.Target(<>o__23.<>p__6, <>o__23.<>p__5.Target(<>o__23.<>p__5, obj3, "id"));
                        if (<>o__23.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__23.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Firts", typeof(Funko), argumentInfo));
                        }
                        object obj6 = <>o__23.<>p__8.Target(<>o__23.<>p__8, <>o__23.<>p__7.Target(<>o__23.<>p__7, obj3));
                        if (<>o__23.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__9 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        this._runner.Product.AvailableSizes.Add(<>o__23.<>p__9.Target(<>o__23.<>p__9, typeof(KeyValuePair<string, string>), obj4, obj5));
                        if (<>o__23.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__23.<>p__10 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        this._config.Add(<>o__23.<>p__10.Target(<>o__23.<>p__10, typeof(KeyValuePair<string, string>), obj5, obj6));
                    }
                }
                else
                {
                    this._runner.Product.AvailableSizes.Add(new KeyValuePair<string, string>("-", ""));
                }
                this._productId = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "product"))).Attributes["value"].Value;
                this._formKey = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "form_key"))).Attributes["value"].Value;
                Cookie cookie = new Cookie {
                    Value = this._formKey,
                    Name = "form_key",
                    Domain = "shop.funko.com"
                };
                this._runner.Cookies.Add(cookie);
                if (this._runner.Product.AvailableSizes.Count != 0)
                {
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
                        string str4 = "";
                        foreach (char ch in this._runner.Product.Price)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str4 = str4 + ch.ToString();
                            }
                        }
                        double num4 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num4 < this._task.MinimumPrice) || (num4 > this._task.MaximumPrice))
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
                                goto Label_0BE3;
                            Label_0B13:
                                pair = enumerator2.Current;
                                List<string> source = new List<string>();
                                if (!pair.Key.Contains(":"))
                                {
                                    source.Add(pair.Key);
                                }
                                else
                                {
                                    char[] chArray2 = new char[] { ':' };
                                    string[] strArray3 = pair.Key.Split(chArray2);
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
                                    goto Label_0BF1;
                                }
                            Label_0BE3:
                                if (!enumerator2.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_0B13;
                            Label_0BF1:
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
            catch (Exception exception3)
            {
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

        public bool Login() => 
            true;

        public bool Search()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                foreach (string str2 in this._task.Keywords)
                {
                    string url = $"https://shop.funko.com/catalogsearch/result/?q={str2.Replace(" ", "+").ToLowerInvariant()}";
                    this._srr = this._client.Get(url).Text();
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(this._srr);
                    if (document.DocumentNode.Descendants("ol").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products list items product-items")))
                    {
                        foreach (HtmlNode node in from x in document.DocumentNode.Descendants("ol").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products list items product-items"))).Descendants("li")
                            where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item product product-item")
                            select x)
                        {
                            if (this.DirectLink(node.Descendants("a").First<HtmlNode>().Attributes["href"].Value))
                            {
                                return true;
                            }
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
                this._runner.Cookies = new CookieContainer();
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://shop.funko.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://shop.funko.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
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
                string str10 = obj2.LastName.Trim();
                string str11 = obj2.Address1.Trim();
                string str12 = obj2.Address2.Trim();
                string str2 = obj2.City.Trim();
                string str3 = obj2.Zip.Trim();
                string state = obj2.State;
                string str = obj2.Phone.Trim();
                obj2.EmailShipping.Trim();
                string str4 = obj2.FirstNameShipping.Trim();
                string str5 = obj2.LastNameShipping.Trim();
                string str16 = obj2.Address1Shipping.Trim();
                string str13 = obj2.Address2Shipping.Trim();
                string str14 = obj2.CityShipping.Trim();
                string str15 = obj2.ZipShipping.Trim();
                string stateShipping = obj2.StateShipping;
                string str8 = obj2.PhoneShipping.Trim();
                string str6 = "";
                string str7 = "";
                double num2 = 10000.0;
                if (<>o__21.<>p__28 == null)
                {
                    <>o__21.<>p__28 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Funko)));
                }
                foreach (object obj3 in <>o__21.<>p__28.Target(<>o__21.<>p__28, this._dynObj))
                {
                    if (<>o__21.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                    }
                    object obj4 = <>o__21.<>p__2.Target(<>o__21.<>p__2, <>o__21.<>p__1.Target(<>o__21.<>p__1, <>o__21.<>p__0.Target(<>o__21.<>p__0, obj3, "base_amount")));
                    if (<>o__21.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Funko), argumentInfo));
                    }
                    if (!<>o__21.<>p__4.Target(<>o__21.<>p__4, <>o__21.<>p__3.Target(<>o__21.<>p__3, obj4, "FREE")))
                    {
                        string str17 = "";
                        if (<>o__21.<>p__21 == null)
                        {
                            <>o__21.<>p__21 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Funko)));
                        }
                        foreach (object obj5 in <>o__21.<>p__21.Target(<>o__21.<>p__21, obj4))
                        {
                            if (<>o__21.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__11 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "IsDigit", null, typeof(Funko), argumentInfo));
                            }
                            object obj6 = <>o__21.<>p__11.Target(<>o__21.<>p__11, typeof(char), obj5);
                            if (<>o__21.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                            }
                            if (!<>o__21.<>p__14.Target(<>o__21.<>p__14, obj6) && (<>o__21.<>p__13 == null))
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__13 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Funko), argumentInfo));
                            }
                            object obj7 = (<>o__21.<>p__12 != null) ? obj6 : <>o__21.<>p__13.Target(<>o__21.<>p__13, obj6, <>o__21.<>p__12.Target(<>o__21.<>p__12, obj5, '.'));
                            if (<>o__21.<>p__18 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                            }
                            if (!<>o__21.<>p__18.Target(<>o__21.<>p__18, obj7))
                            {
                                if (<>o__21.<>p__17 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__21.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                                }
                                if (<>o__21.<>p__16 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__21.<>p__16 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Funko), argumentInfo));
                                }
                                if (<>o__21.<>p__15 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__21.<>p__15 = CallSite<Func<CallSite, object, char, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Funko), argumentInfo));
                                }
                                if (!<>o__21.<>p__17.Target(<>o__21.<>p__17, <>o__21.<>p__16.Target(<>o__21.<>p__16, obj7, <>o__21.<>p__15.Target(<>o__21.<>p__15, obj5, ','))))
                                {
                                    continue;
                                }
                            }
                            if (<>o__21.<>p__20 == null)
                            {
                                <>o__21.<>p__20 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Funko)));
                            }
                            if (<>o__21.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__19 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Funko), argumentInfo));
                            }
                            str17 = <>o__21.<>p__20.Target(<>o__21.<>p__20, <>o__21.<>p__19.Target(<>o__21.<>p__19, str17, obj5));
                        }
                        double num3 = double.Parse(str17.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if (num3 < num2)
                        {
                            if (<>o__21.<>p__24 == null)
                            {
                                <>o__21.<>p__24 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                            }
                            if (<>o__21.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                            }
                            if (<>o__21.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                            }
                            str6 = <>o__21.<>p__24.Target(<>o__21.<>p__24, <>o__21.<>p__23.Target(<>o__21.<>p__23, <>o__21.<>p__22.Target(<>o__21.<>p__22, obj3, "carrier_code")));
                            if (<>o__21.<>p__27 == null)
                            {
                                <>o__21.<>p__27 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                            }
                            if (<>o__21.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__21.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                            }
                            if (<>o__21.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__21.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                            }
                            str7 = <>o__21.<>p__27.Target(<>o__21.<>p__27, <>o__21.<>p__26.Target(<>o__21.<>p__26, <>o__21.<>p__25.Target(<>o__21.<>p__25, obj3, "method_code")));
                            num2 = num3;
                        }
                    }
                    else
                    {
                        num2 = 0.0;
                        if (<>o__21.<>p__7 == null)
                        {
                            <>o__21.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                        }
                        if (<>o__21.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__21.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        str6 = <>o__21.<>p__7.Target(<>o__21.<>p__7, <>o__21.<>p__6.Target(<>o__21.<>p__6, <>o__21.<>p__5.Target(<>o__21.<>p__5, obj3, "carrier_code")));
                        if (<>o__21.<>p__10 == null)
                        {
                            <>o__21.<>p__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                        }
                        if (<>o__21.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__21.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__21.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        str7 = <>o__21.<>p__10.Target(<>o__21.<>p__10, <>o__21.<>p__9.Target(<>o__21.<>p__9, <>o__21.<>p__8.Target(<>o__21.<>p__8, obj3, "method_code")));
                    }
                }
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    object obj8 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__21.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__29 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__29.Target(<>o__21.<>p__29, obj8, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__21.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__31 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__31.Target(<>o__21.<>p__31, <>o__21.<>p__30.Target(<>o__21.<>p__30, obj8), new Newtonsoft.Json.Linq.JObject());
                    if (<>o__21.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "countryId", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__34.Target(<>o__21.<>p__34, <>o__21.<>p__33.Target(<>o__21.<>p__33, <>o__21.<>p__32.Target(<>o__21.<>p__32, obj8)), "US");
                    if (<>o__21.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionId", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__37.Target(<>o__21.<>p__37, <>o__21.<>p__36.Target(<>o__21.<>p__36, <>o__21.<>p__35.Target(<>o__21.<>p__35, obj8)), this._regionIdShipping);
                    if (<>o__21.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionCode", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__40.Target(<>o__21.<>p__40, <>o__21.<>p__39.Target(<>o__21.<>p__39, <>o__21.<>p__38.Target(<>o__21.<>p__38, obj8)), obj2.StateIdShipping);
                    if (<>o__21.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__43.Target(<>o__21.<>p__43, <>o__21.<>p__42.Target(<>o__21.<>p__42, <>o__21.<>p__41.Target(<>o__21.<>p__41, obj8)), this._regionShipping);
                    Newtonsoft.Json.Linq.JArray array2 = new Newtonsoft.Json.Linq.JArray {
                        str16,
                        str13,
                        str15
                    };
                    if (<>o__21.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__46 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "street", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__46.Target(<>o__21.<>p__46, <>o__21.<>p__45.Target(<>o__21.<>p__45, <>o__21.<>p__44.Target(<>o__21.<>p__44, obj8)), array2);
                    if (<>o__21.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__49 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "company", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__49.Target(<>o__21.<>p__49, <>o__21.<>p__48.Target(<>o__21.<>p__48, <>o__21.<>p__47.Target(<>o__21.<>p__47, obj8)), "Mr");
                    if (<>o__21.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__52 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "telephone", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__52.Target(<>o__21.<>p__52, <>o__21.<>p__51.Target(<>o__21.<>p__51, <>o__21.<>p__50.Target(<>o__21.<>p__50, obj8)), str8);
                    if (<>o__21.<>p__55 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__55 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postcode", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__54 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__53 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__53 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__55.Target(<>o__21.<>p__55, <>o__21.<>p__54.Target(<>o__21.<>p__54, <>o__21.<>p__53.Target(<>o__21.<>p__53, obj8)), str15);
                    if (<>o__21.<>p__58 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__58 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__57 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__56 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__58.Target(<>o__21.<>p__58, <>o__21.<>p__57.Target(<>o__21.<>p__57, <>o__21.<>p__56.Target(<>o__21.<>p__56, obj8)), str14);
                    if (<>o__21.<>p__61 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__61 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__60 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__59 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__59 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__61.Target(<>o__21.<>p__61, <>o__21.<>p__60.Target(<>o__21.<>p__60, <>o__21.<>p__59.Target(<>o__21.<>p__59, obj8)), str4);
                    if (<>o__21.<>p__64 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__64 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__63 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__63 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__62 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__64.Target(<>o__21.<>p__64, <>o__21.<>p__63.Target(<>o__21.<>p__63, <>o__21.<>p__62.Target(<>o__21.<>p__62, obj8)), str5);
                    if (<>o__21.<>p__67 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__21.<>p__67 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "save_in_address_book", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__66 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__65 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__65 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__67.Target(<>o__21.<>p__67, <>o__21.<>p__66.Target(<>o__21.<>p__66, <>o__21.<>p__65.Target(<>o__21.<>p__65, obj8)), null);
                    if (<>o__21.<>p__69 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__69 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__68 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__69.Target(<>o__21.<>p__69, <>o__21.<>p__68.Target(<>o__21.<>p__68, obj8), new Newtonsoft.Json.Linq.JObject());
                    if (<>o__21.<>p__72 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__72 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "countryId", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__71 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__71 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__70 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__72.Target(<>o__21.<>p__72, <>o__21.<>p__71.Target(<>o__21.<>p__71, <>o__21.<>p__70.Target(<>o__21.<>p__70, obj8)), "US");
                    if (<>o__21.<>p__75 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__75 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionId", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__74 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__73 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__73 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__75.Target(<>o__21.<>p__75, <>o__21.<>p__74.Target(<>o__21.<>p__74, <>o__21.<>p__73.Target(<>o__21.<>p__73, obj8)), this._regionIdBilling);
                    if (<>o__21.<>p__78 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__78 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionCode", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__77 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__77 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__76 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__76 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__78.Target(<>o__21.<>p__78, <>o__21.<>p__77.Target(<>o__21.<>p__77, <>o__21.<>p__76.Target(<>o__21.<>p__76, obj8)), obj2.StateId);
                    if (<>o__21.<>p__81 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__81 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__80 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__80 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__79 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__79 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__81.Target(<>o__21.<>p__81, <>o__21.<>p__80.Target(<>o__21.<>p__80, <>o__21.<>p__79.Target(<>o__21.<>p__79, obj8)), this._regionBilling);
                    Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray {
                        str11,
                        str12,
                        str3
                    };
                    if (<>o__21.<>p__84 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__84 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "street", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__83 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__83 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__82 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__82 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__84.Target(<>o__21.<>p__84, <>o__21.<>p__83.Target(<>o__21.<>p__83, <>o__21.<>p__82.Target(<>o__21.<>p__82, obj8)), array);
                    if (<>o__21.<>p__87 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__87 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "company", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__86 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__86 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__85 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__85 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__87.Target(<>o__21.<>p__87, <>o__21.<>p__86.Target(<>o__21.<>p__86, <>o__21.<>p__85.Target(<>o__21.<>p__85, obj8)), "Mr");
                    if (<>o__21.<>p__90 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__90 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "telephone", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__89 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__89 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__88 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__88 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__90.Target(<>o__21.<>p__90, <>o__21.<>p__89.Target(<>o__21.<>p__89, <>o__21.<>p__88.Target(<>o__21.<>p__88, obj8)), str);
                    if (<>o__21.<>p__93 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__93 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postcode", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__92 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__92 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__91 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__91 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__93.Target(<>o__21.<>p__93, <>o__21.<>p__92.Target(<>o__21.<>p__92, <>o__21.<>p__91.Target(<>o__21.<>p__91, obj8)), str3);
                    if (<>o__21.<>p__96 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__96 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__95 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__95 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__94 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__94 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__96.Target(<>o__21.<>p__96, <>o__21.<>p__95.Target(<>o__21.<>p__95, <>o__21.<>p__94.Target(<>o__21.<>p__94, obj8)), str2);
                    if (<>o__21.<>p__99 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__99 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__98 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__98 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__97 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__97 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__99.Target(<>o__21.<>p__99, <>o__21.<>p__98.Target(<>o__21.<>p__98, <>o__21.<>p__97.Target(<>o__21.<>p__97, obj8)), str9);
                    if (<>o__21.<>p__102 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__102 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__101 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__101 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__100 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__100 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__102.Target(<>o__21.<>p__102, <>o__21.<>p__101.Target(<>o__21.<>p__101, <>o__21.<>p__100.Target(<>o__21.<>p__100, obj8)), str10);
                    if (<>o__21.<>p__105 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__105 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "save_in_address_book", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__104 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__104 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__103 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__103 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__105.Target(<>o__21.<>p__105, <>o__21.<>p__104.Target(<>o__21.<>p__104, <>o__21.<>p__103.Target(<>o__21.<>p__103, obj8)), 0);
                    if (<>o__21.<>p__108 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__21.<>p__108 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "saveInAddressBook", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__107 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__107 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billing_address", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__106 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__106 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__108.Target(<>o__21.<>p__108, <>o__21.<>p__107.Target(<>o__21.<>p__107, <>o__21.<>p__106.Target(<>o__21.<>p__106, obj8)), null);
                    if (<>o__21.<>p__110 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__110 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shipping_method_code", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__109 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__109 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__110.Target(<>o__21.<>p__110, <>o__21.<>p__109.Target(<>o__21.<>p__109, obj8), str7);
                    if (<>o__21.<>p__112 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__21.<>p__112 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "shipping_carrier_code", typeof(Funko), argumentInfo));
                    }
                    if (<>o__21.<>p__111 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__21.<>p__111 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "addressInformation", typeof(Funko), argumentInfo));
                    }
                    <>o__21.<>p__112.Target(<>o__21.<>p__112, <>o__21.<>p__111.Target(<>o__21.<>p__111, obj8), str6);
                    try
                    {
                        if (<>o__21.<>p__114 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__114 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Funko), argumentInfo));
                        }
                        if (<>o__21.<>p__113 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__113 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Funko), argumentInfo));
                        }
                        object obj9 = <>o__21.<>p__114.Target(<>o__21.<>p__114, this._client, $"https://shop.funko.com/rest/default/V1/guest-carts/{this._cartId}/shipping-information", <>o__21.<>p__113.Target(<>o__21.<>p__113, typeof(Newtonsoft.Json.JsonConvert), obj8));
                        if (<>o__21.<>p__115 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__21.<>p__115 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Funko), argumentInfo));
                        }
                        <>o__21.<>p__115.Target(<>o__21.<>p__115, typeof(EveAIO.Extensions), obj9);
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
                        this._client.Get("https://shop.funko.com/customer/section/load/?sections=messages&update_section_id=true").Text();
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
                return false;
            }
        }

        private bool SubmitOrder()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                obj2.Email.Trim();
                string str = obj2.FirstName.Trim();
                string str8 = obj2.LastName.Trim();
                string str3 = obj2.Address1.Trim();
                string str4 = obj2.Address2.Trim();
                string str7 = obj2.City.Trim();
                string str6 = obj2.Zip.Trim();
                string state = obj2.State;
                string str2 = obj2.Phone.Trim();
                bool flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    string str5 = "";
                    if (obj2.CardTypeId != "1")
                    {
                        if (obj2.CardTypeId != "2")
                        {
                            if (obj2.CardTypeId != "3")
                            {
                                goto Label_1519;
                            }
                            str5 = "DI";
                        }
                        else
                        {
                            str5 = "MC";
                        }
                    }
                    else
                    {
                        str5 = "VI";
                    }
                    object obj3 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__20.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cartId", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__0.Target(<>o__20.<>p__0, obj3, this._cartId);
                    if (<>o__20.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "email", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__1.Target(<>o__20.<>p__1, obj3, obj2.Email);
                    if (<>o__20.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__2 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__2.Target(<>o__20.<>p__2, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "countryId", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__4.Target(<>o__20.<>p__4, <>o__20.<>p__3.Target(<>o__20.<>p__3, obj3), "US");
                    if (<>o__20.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionId", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__6.Target(<>o__20.<>p__6, <>o__20.<>p__5.Target(<>o__20.<>p__5, obj3), this._regionIdBilling);
                    if (<>o__20.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "regionCode", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__8.Target(<>o__20.<>p__8, <>o__20.<>p__7.Target(<>o__20.<>p__7, obj3), obj2.StateId);
                    if (<>o__20.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__10.Target(<>o__20.<>p__10, <>o__20.<>p__9.Target(<>o__20.<>p__9, obj3), this._regionBilling);
                    Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray {
                        str3,
                        str4,
                        this._regionBilling
                    };
                    if (<>o__20.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__12 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "street", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__12.Target(<>o__20.<>p__12, <>o__20.<>p__11.Target(<>o__20.<>p__11, obj3), array);
                    if (<>o__20.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "company", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__14.Target(<>o__20.<>p__14, <>o__20.<>p__13.Target(<>o__20.<>p__13, obj3), "Mr");
                    if (<>o__20.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "telephone", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__16.Target(<>o__20.<>p__16, <>o__20.<>p__15.Target(<>o__20.<>p__15, obj3), str2);
                    if (<>o__20.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postcode", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__17 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__18.Target(<>o__20.<>p__18, <>o__20.<>p__17.Target(<>o__20.<>p__17, obj3), str6);
                    if (<>o__20.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__20.Target(<>o__20.<>p__20, <>o__20.<>p__19.Target(<>o__20.<>p__19, obj3), str7);
                    if (<>o__20.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__22.Target(<>o__20.<>p__22, <>o__20.<>p__21.Target(<>o__20.<>p__21, obj3), str);
                    if (<>o__20.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__24.Target(<>o__20.<>p__24, <>o__20.<>p__23.Target(<>o__20.<>p__23, obj3), str8);
                    if (<>o__20.<>p__26 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__20.<>p__26 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "saveInAddressBook", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__25 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "billingAddress", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__26.Target(<>o__20.<>p__26, <>o__20.<>p__25.Target(<>o__20.<>p__25, obj3), null);
                    if (<>o__20.<>p__27 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__27 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__27.Target(<>o__20.<>p__27, obj3, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__29 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__29 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "method", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__28 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__29.Target(<>o__20.<>p__29, <>o__20.<>p__28.Target(<>o__20.<>p__28, obj3), "magedelight_cybersource");
                    if (<>o__20.<>p__31 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__31 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__30 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__31.Target(<>o__20.<>p__31, <>o__20.<>p__30.Target(<>o__20.<>p__30, obj3), new Newtonsoft.Json.Linq.JObject());
                    if (<>o__20.<>p__34 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "subscription_id", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__33 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__32 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__34.Target(<>o__20.<>p__34, <>o__20.<>p__33.Target(<>o__20.<>p__33, <>o__20.<>p__32.Target(<>o__20.<>p__32, obj3)), "new");
                    if (<>o__20.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cc_type", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__36 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__37.Target(<>o__20.<>p__37, <>o__20.<>p__36.Target(<>o__20.<>p__36, <>o__20.<>p__35.Target(<>o__20.<>p__35, obj3)), str5);
                    if (<>o__20.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cc_number", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__40.Target(<>o__20.<>p__40, <>o__20.<>p__39.Target(<>o__20.<>p__39, <>o__20.<>p__38.Target(<>o__20.<>p__38, obj3)), obj2.CCNumber);
                    if (<>o__20.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__43 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "expiration", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__43.Target(<>o__20.<>p__43, <>o__20.<>p__42.Target(<>o__20.<>p__42, <>o__20.<>p__41.Target(<>o__20.<>p__41, obj3)), int.Parse(obj2.ExpiryMonth));
                    if (<>o__20.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__46 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "expiration_yr", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__46.Target(<>o__20.<>p__46, <>o__20.<>p__45.Target(<>o__20.<>p__45, <>o__20.<>p__44.Target(<>o__20.<>p__44, obj3)), obj2.ExpiryYear);
                    if (<>o__20.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__49 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cc_cid", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__49.Target(<>o__20.<>p__49, <>o__20.<>p__48.Target(<>o__20.<>p__48, <>o__20.<>p__47.Target(<>o__20.<>p__47, obj3)), obj2.Cvv);
                    if (<>o__20.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__20.<>p__52 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "save_card", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "additional_data", typeof(Funko), argumentInfo));
                    }
                    if (<>o__20.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__20.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethod", typeof(Funko), argumentInfo));
                    }
                    <>o__20.<>p__52.Target(<>o__20.<>p__52, <>o__20.<>p__51.Target(<>o__20.<>p__51, <>o__20.<>p__50.Target(<>o__20.<>p__50, obj3)), false);
                    if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                    {
                        goto Label_154B;
                    }
                    if (this._task.CheckoutDelay > 0)
                    {
                        Thread.Sleep(this._task.CheckoutDelay);
                    }
                    if (Global.SERIAL == "EVE-1111111111111")
                    {
                        goto Label_157D;
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
                        if (<>o__20.<>p__54 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__54 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Funko), argumentInfo));
                        }
                        if (<>o__20.<>p__53 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__53 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Funko), argumentInfo));
                        }
                        object obj4 = <>o__20.<>p__54.Target(<>o__20.<>p__54, this._client, $"https://shop.funko.com/rest/default/V1/guest-carts/{this._cartId}/payment-information", <>o__20.<>p__53.Target(<>o__20.<>p__53, typeof(Newtonsoft.Json.JsonConvert), obj3));
                        if (<>o__20.<>p__55 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__20.<>p__55 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Funko), argumentInfo));
                        }
                        <>o__20.<>p__55.Target(<>o__20.<>p__55, typeof(EveAIO.Extensions), obj4);
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
                        flag2 = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                try
                {
                    EveAIO.Helpers.AddDbValue("Funko|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                return true;
            Label_1519:
                this._task.Status = States.GetTaskState(States.TaskState.ERROR_PROCESSING_CC);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_PROCESSING_CC, null, "", "");
                return false;
            Label_154B:
                this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                return false;
            Label_157D:
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
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
                return false;
            }
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
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://shop.funko.com/checkout/cart/");
                        this._srr = this._client.Get("https://shop.funko.com/checkout/").Text();
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
                string str = this._srr.Substring(this._srr.IndexOf("window.checkoutConfig"));
                str = str.Substring(str.IndexOf("{"));
                str = str.Substring(0, str.IndexOf("}}};") + 3);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                if (<>o__22.<>p__3 == null)
                {
                    <>o__22.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                }
                if (<>o__22.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                }
                if (<>o__22.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__22.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                }
                if (<>o__22.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "quoteData", typeof(Funko), argumentInfo));
                }
                this._cartId = <>o__22.<>p__3.Target(<>o__22.<>p__3, <>o__22.<>p__2.Target(<>o__22.<>p__2, <>o__22.<>p__1.Target(<>o__22.<>p__1, <>o__22.<>p__0.Target(<>o__22.<>p__0, this._dynObj), "entity_id")));
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                string str10 = obj2.EmailShipping.Trim();
                string str7 = obj2.FirstNameShipping.Trim();
                string str8 = obj2.LastNameShipping.Trim();
                string str9 = obj2.Address1Shipping.Trim();
                string str2 = obj2.Address2Shipping.Trim();
                string str3 = obj2.CityShipping.Trim();
                string str4 = obj2.ZipShipping.Trim();
                string stateShipping = obj2.StateShipping;
                string str6 = obj2.PhoneShipping.Trim();
                flag = true;
                while (flag)
                {
                    flag = false;
                    object obj3 = new Newtonsoft.Json.Linq.JObject();
                    if (<>o__22.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "customerEmail", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__4.Target(<>o__22.<>p__4, obj3, str10);
                    try
                    {
                        if (<>o__22.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__6 = CallSite<Action<CallSite, Client, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "PostJson", null, typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__5 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Funko), argumentInfo));
                        }
                        <>o__22.<>p__6.Target(<>o__22.<>p__6, this._client, "https://shop.funko.com/rest/default/V1/customers/isEmailAvailable", <>o__22.<>p__5.Target(<>o__22.<>p__5, typeof(Newtonsoft.Json.JsonConvert), obj3));
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
                    IEnumerator enumerator;
                    string str11;
                    object obj6;
                    flag = false;
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Resources.funko);
                    if (<>o__22.<>p__21 == null)
                    {
                        <>o__22.<>p__21 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Funko)));
                    }
                    if (<>o__22.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region_id", typeof(Funko), argumentInfo));
                    }
                    using (enumerator = <>o__22.<>p__21.Target(<>o__22.<>p__21, <>o__22.<>p__7.Target(<>o__22.<>p__7, this._dynObj)).GetEnumerator())
                    {
                        object obj4;
                        goto Label_078D;
                    Label_0551:
                        obj4 = enumerator.Current;
                        if (<>o__22.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__10 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                            <>o__22.<>p__9 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        if (!<>o__22.<>p__10.Target(<>o__22.<>p__10, <>o__22.<>p__9.Target(<>o__22.<>p__9, <>o__22.<>p__8.Target(<>o__22.<>p__8, obj4, "country_id"), null)))
                        {
                            if (<>o__22.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                            }
                            if (<>o__22.<>p__13 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Funko), argumentInfo));
                            }
                            if (<>o__22.<>p__12 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                            }
                            if (<>o__22.<>p__11 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                            }
                            if (<>o__22.<>p__14.Target(<>o__22.<>p__14, <>o__22.<>p__13.Target(<>o__22.<>p__13, <>o__22.<>p__12.Target(<>o__22.<>p__12, <>o__22.<>p__11.Target(<>o__22.<>p__11, obj4, "title")), stateShipping)))
                            {
                                goto Label_079E;
                            }
                        }
                    Label_078D:
                        if (!enumerator.MoveNext())
                        {
                            goto Label_0983;
                        }
                        goto Label_0551;
                    Label_079E:
                        if (<>o__22.<>p__17 == null)
                        {
                            <>o__22.<>p__17 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                        }
                        if (<>o__22.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        this._regionIdShipping = <>o__22.<>p__17.Target(<>o__22.<>p__17, <>o__22.<>p__16.Target(<>o__22.<>p__16, <>o__22.<>p__15.Target(<>o__22.<>p__15, obj4, "value")));
                        if (<>o__22.<>p__20 == null)
                        {
                            <>o__22.<>p__20 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                        }
                        if (<>o__22.<>p__19 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        this._regionShipping = <>o__22.<>p__20.Target(<>o__22.<>p__20, <>o__22.<>p__19.Target(<>o__22.<>p__19, <>o__22.<>p__18.Target(<>o__22.<>p__18, obj4, "title")));
                    }
                Label_0983:
                    str11 = obj2.State;
                    if (<>o__22.<>p__36 == null)
                    {
                        <>o__22.<>p__36 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Funko)));
                    }
                    if (<>o__22.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "region_id", typeof(Funko), argumentInfo));
                    }
                    using (enumerator = <>o__22.<>p__36.Target(<>o__22.<>p__36, <>o__22.<>p__22.Target(<>o__22.<>p__22, this._dynObj)).GetEnumerator())
                    {
                        object current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (<>o__22.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__22.<>p__25 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                            }
                            if (<>o__22.<>p__24 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                                <>o__22.<>p__24 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Funko), argumentInfo));
                            }
                            if (<>o__22.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__22.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                            }
                            if (!<>o__22.<>p__25.Target(<>o__22.<>p__25, <>o__22.<>p__24.Target(<>o__22.<>p__24, <>o__22.<>p__23.Target(<>o__22.<>p__23, current, "country_id"), null)))
                            {
                                if (<>o__22.<>p__29 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__22.<>p__29 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                                }
                                if (<>o__22.<>p__28 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__22.<>p__28 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Funko), argumentInfo));
                                }
                                if (<>o__22.<>p__27 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__22.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                                }
                                if (<>o__22.<>p__26 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__22.<>p__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                                }
                                if (<>o__22.<>p__29.Target(<>o__22.<>p__29, <>o__22.<>p__28.Target(<>o__22.<>p__28, <>o__22.<>p__27.Target(<>o__22.<>p__27, <>o__22.<>p__26.Target(<>o__22.<>p__26, current, "title")), str11)))
                                {
                                    goto Label_0C76;
                                }
                            }
                        }
                        goto Label_0E5B;
                    Label_0C76:
                        if (<>o__22.<>p__32 == null)
                        {
                            <>o__22.<>p__32 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                        }
                        if (<>o__22.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__30 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        this._regionIdBilling = <>o__22.<>p__32.Target(<>o__22.<>p__32, <>o__22.<>p__31.Target(<>o__22.<>p__31, <>o__22.<>p__30.Target(<>o__22.<>p__30, current, "value")));
                        if (<>o__22.<>p__35 == null)
                        {
                            <>o__22.<>p__35 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Funko)));
                        }
                        if (<>o__22.<>p__34 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__22.<>p__33 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Funko), argumentInfo));
                        }
                        this._regionBilling = <>o__22.<>p__35.Target(<>o__22.<>p__35, <>o__22.<>p__34.Target(<>o__22.<>p__34, <>o__22.<>p__33.Target(<>o__22.<>p__33, current, "title")));
                    }
                Label_0E5B:
                    obj6 = new Newtonsoft.Json.Linq.JObject();
                    Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray {
                        str9,
                        str2,
                        str4
                    };
                    if (<>o__22.<>p__37 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__37 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__37.Target(<>o__22.<>p__37, obj6, new Newtonsoft.Json.Linq.JObject());
                    if (<>o__22.<>p__39 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__39 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "street", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__38 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__39.Target(<>o__22.<>p__39, <>o__22.<>p__38.Target(<>o__22.<>p__38, obj6), array);
                    if (<>o__22.<>p__41 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__41 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "city", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__40 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__41.Target(<>o__22.<>p__41, <>o__22.<>p__40.Target(<>o__22.<>p__40, obj6), str3);
                    if (<>o__22.<>p__43 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region_id", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__42 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__43.Target(<>o__22.<>p__43, <>o__22.<>p__42.Target(<>o__22.<>p__42, obj6), this._regionIdShipping);
                    if (<>o__22.<>p__45 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__45 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "region", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__44 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__45.Target(<>o__22.<>p__45, <>o__22.<>p__44.Target(<>o__22.<>p__44, obj6), this._regionShipping);
                    if (<>o__22.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__47 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "country_id", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__46 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__47.Target(<>o__22.<>p__47, <>o__22.<>p__46.Target(<>o__22.<>p__46, obj6), "US");
                    if (<>o__22.<>p__49 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__49 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "postcode", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__49.Target(<>o__22.<>p__49, <>o__22.<>p__48.Target(<>o__22.<>p__48, obj6), str4);
                    if (<>o__22.<>p__51 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__51 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "firstname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__50 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__51.Target(<>o__22.<>p__51, <>o__22.<>p__50.Target(<>o__22.<>p__50, obj6), str7);
                    if (<>o__22.<>p__53 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__53 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "lastname", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__52 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__53.Target(<>o__22.<>p__53, <>o__22.<>p__52.Target(<>o__22.<>p__52, obj6), str8);
                    if (<>o__22.<>p__55 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__55 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "company", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__54 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__55.Target(<>o__22.<>p__55, <>o__22.<>p__54.Target(<>o__22.<>p__54, obj6), "Mr");
                    if (<>o__22.<>p__57 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__57 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "telephone", typeof(Funko), argumentInfo));
                    }
                    if (<>o__22.<>p__56 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "address", typeof(Funko), argumentInfo));
                    }
                    <>o__22.<>p__57.Target(<>o__22.<>p__57, <>o__22.<>p__56.Target(<>o__22.<>p__56, obj6), str6);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        if (<>o__22.<>p__59 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__59 = CallSite<Func<CallSite, Client, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PostJson", null, typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__58 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__58 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Funko), argumentInfo));
                        }
                        object obj7 = <>o__22.<>p__59.Target(<>o__22.<>p__59, this._client, $"https://shop.funko.com/rest/default/V1/guest-carts/{this._cartId}/estimate-shipping-methods", <>o__22.<>p__58.Target(<>o__22.<>p__58, typeof(Newtonsoft.Json.JsonConvert), obj6));
                        if (<>o__22.<>p__60 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__60 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CheckError", null, typeof(Funko), argumentInfo));
                        }
                        <>o__22.<>p__60.Target(<>o__22.<>p__60, typeof(EveAIO.Extensions), obj7);
                        if (<>o__22.<>p__65 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__65 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__64 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__64 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__63 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__63 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__62 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadAsStringAsync", null, typeof(Funko), argumentInfo));
                        }
                        if (<>o__22.<>p__61 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__22.<>p__61 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Content", typeof(Funko), argumentInfo));
                        }
                        this._dynObj = <>o__22.<>p__65.Target(<>o__22.<>p__65, typeof(Newtonsoft.Json.JsonConvert), <>o__22.<>p__64.Target(<>o__22.<>p__64, <>o__22.<>p__63.Target(<>o__22.<>p__63, <>o__22.<>p__62.Target(<>o__22.<>p__62, <>o__22.<>p__61.Target(<>o__22.<>p__61, obj7)))));
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
                if (<>o__22.<>p__68 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__68 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Funko), argumentInfo));
                }
                if (<>o__22.<>p__67 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__22.<>p__67 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(Funko), argumentInfo));
                }
                if (<>o__22.<>p__66 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Funko), argumentInfo));
                }
                if (!<>o__22.<>p__68.Target(<>o__22.<>p__68, <>o__22.<>p__67.Target(<>o__22.<>p__67, <>o__22.<>p__66.Target(<>o__22.<>p__66, this._dynObj), 1)))
                {
                    return true;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CALCULATING_SHIPPING, null, "", "");
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
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception4, "", "");
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
            public static readonly Funko.<>c <>9;
            public static Func<HtmlNode, bool> <>9__18_0;
            public static Func<HtmlNode, bool> <>9__23_0;
            public static Func<HtmlNode, bool> <>9__23_1;
            public static Func<HtmlNode, bool> <>9__23_2;
            public static Func<HtmlNode, bool> <>9__23_3;
            public static Func<HtmlNode, bool> <>9__23_4;
            public static Func<HtmlNode, bool> <>9__23_5;
            public static Func<HtmlNode, bool> <>9__25_0;
            public static Func<HtmlNode, bool> <>9__25_1;
            public static Func<HtmlNode, bool> <>9__25_2;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Funko.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__18_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_addtocart_form"));

            internal bool <DirectLink>b__23_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "product_addtocart_form"));

            internal bool <DirectLink>b__23_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-info-title"));

            internal bool <DirectLink>b__23_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"));

            internal bool <DirectLink>b__23_3(HtmlNode x) => 
                ((x.Attributes["property"] != null) && (x.Attributes["property"].Value == "og:image"));

            internal bool <DirectLink>b__23_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "product"));

            internal bool <DirectLink>b__23_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "form_key"));

            internal bool <Search>b__25_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products list items product-items"));

            internal bool <Search>b__25_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "products list items product-items"));

            internal bool <Search>b__25_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item product product-item"));
        }

        [CompilerGenerated]
        private static class <>o__18
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, bool>> <>p__8;
            public static CallSite<Func<CallSite, object, bool>> <>p__9;
        }

        [CompilerGenerated]
        private static class <>o__20
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__12;
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
            public static CallSite<Func<CallSite, object, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, string, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, int, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, string, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object>> <>p__48;
            public static CallSite<Func<CallSite, object, string, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__52;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__53;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__54;
            public static CallSite<Action<CallSite, Type, object>> <>p__55;
        }

        [CompilerGenerated]
        private static class <>o__21
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string>> <>p__10;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, char, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, char, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, bool>> <>p__18;
            public static CallSite<Func<CallSite, string, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string>> <>p__20;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, object, string>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string>> <>p__27;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__28;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, object>> <>p__39;
            public static CallSite<Func<CallSite, object, string, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, object>> <>p__45;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object>> <>p__48;
            public static CallSite<Func<CallSite, object, string, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, object>> <>p__51;
            public static CallSite<Func<CallSite, object, string, object>> <>p__52;
            public static CallSite<Func<CallSite, object, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, string, object>> <>p__55;
            public static CallSite<Func<CallSite, object, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, string, object>> <>p__58;
            public static CallSite<Func<CallSite, object, object>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, string, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, object>> <>p__63;
            public static CallSite<Func<CallSite, object, string, object>> <>p__64;
            public static CallSite<Func<CallSite, object, object>> <>p__65;
            public static CallSite<Func<CallSite, object, object>> <>p__66;
            public static CallSite<Func<CallSite, object, object, object>> <>p__67;
            public static CallSite<Func<CallSite, object, object>> <>p__68;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__69;
            public static CallSite<Func<CallSite, object, object>> <>p__70;
            public static CallSite<Func<CallSite, object, object>> <>p__71;
            public static CallSite<Func<CallSite, object, string, object>> <>p__72;
            public static CallSite<Func<CallSite, object, object>> <>p__73;
            public static CallSite<Func<CallSite, object, object>> <>p__74;
            public static CallSite<Func<CallSite, object, string, object>> <>p__75;
            public static CallSite<Func<CallSite, object, object>> <>p__76;
            public static CallSite<Func<CallSite, object, object>> <>p__77;
            public static CallSite<Func<CallSite, object, string, object>> <>p__78;
            public static CallSite<Func<CallSite, object, object>> <>p__79;
            public static CallSite<Func<CallSite, object, object>> <>p__80;
            public static CallSite<Func<CallSite, object, string, object>> <>p__81;
            public static CallSite<Func<CallSite, object, object>> <>p__82;
            public static CallSite<Func<CallSite, object, object>> <>p__83;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__84;
            public static CallSite<Func<CallSite, object, object>> <>p__85;
            public static CallSite<Func<CallSite, object, object>> <>p__86;
            public static CallSite<Func<CallSite, object, string, object>> <>p__87;
            public static CallSite<Func<CallSite, object, object>> <>p__88;
            public static CallSite<Func<CallSite, object, object>> <>p__89;
            public static CallSite<Func<CallSite, object, string, object>> <>p__90;
            public static CallSite<Func<CallSite, object, object>> <>p__91;
            public static CallSite<Func<CallSite, object, object>> <>p__92;
            public static CallSite<Func<CallSite, object, string, object>> <>p__93;
            public static CallSite<Func<CallSite, object, object>> <>p__94;
            public static CallSite<Func<CallSite, object, object>> <>p__95;
            public static CallSite<Func<CallSite, object, string, object>> <>p__96;
            public static CallSite<Func<CallSite, object, object>> <>p__97;
            public static CallSite<Func<CallSite, object, object>> <>p__98;
            public static CallSite<Func<CallSite, object, string, object>> <>p__99;
            public static CallSite<Func<CallSite, object, object>> <>p__100;
            public static CallSite<Func<CallSite, object, object>> <>p__101;
            public static CallSite<Func<CallSite, object, string, object>> <>p__102;
            public static CallSite<Func<CallSite, object, object>> <>p__103;
            public static CallSite<Func<CallSite, object, object>> <>p__104;
            public static CallSite<Func<CallSite, object, int, object>> <>p__105;
            public static CallSite<Func<CallSite, object, object>> <>p__106;
            public static CallSite<Func<CallSite, object, object>> <>p__107;
            public static CallSite<Func<CallSite, object, object, object>> <>p__108;
            public static CallSite<Func<CallSite, object, object>> <>p__109;
            public static CallSite<Func<CallSite, object, string, object>> <>p__110;
            public static CallSite<Func<CallSite, object, object>> <>p__111;
            public static CallSite<Func<CallSite, object, string, object>> <>p__112;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__113;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__114;
            public static CallSite<Action<CallSite, Type, object>> <>p__115;
        }

        [CompilerGenerated]
        private static class <>o__22
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__5;
            public static CallSite<Action<CallSite, Client, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, bool>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string>> <>p__17;
            public static CallSite<Func<CallSite, object, string, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, string>> <>p__20;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, bool>> <>p__25;
            public static CallSite<Func<CallSite, object, string, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, bool>> <>p__29;
            public static CallSite<Func<CallSite, object, string, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, string>> <>p__32;
            public static CallSite<Func<CallSite, object, string, object>> <>p__33;
            public static CallSite<Func<CallSite, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, string>> <>p__35;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__36;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JArray, object>> <>p__39;
            public static CallSite<Func<CallSite, object, object>> <>p__40;
            public static CallSite<Func<CallSite, object, string, object>> <>p__41;
            public static CallSite<Func<CallSite, object, object>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string, object>> <>p__45;
            public static CallSite<Func<CallSite, object, object>> <>p__46;
            public static CallSite<Func<CallSite, object, string, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object>> <>p__48;
            public static CallSite<Func<CallSite, object, string, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, string, object>> <>p__51;
            public static CallSite<Func<CallSite, object, object>> <>p__52;
            public static CallSite<Func<CallSite, object, string, object>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, string, object>> <>p__55;
            public static CallSite<Func<CallSite, object, object>> <>p__56;
            public static CallSite<Func<CallSite, object, string, object>> <>p__57;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__58;
            public static CallSite<Func<CallSite, Client, string, object, object>> <>p__59;
            public static CallSite<Action<CallSite, Type, object>> <>p__60;
            public static CallSite<Func<CallSite, object, object>> <>p__61;
            public static CallSite<Func<CallSite, object, object>> <>p__62;
            public static CallSite<Func<CallSite, object, object>> <>p__63;
            public static CallSite<Func<CallSite, object, object>> <>p__64;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__65;
            public static CallSite<Func<CallSite, object, object>> <>p__66;
            public static CallSite<Func<CallSite, object, int, object>> <>p__67;
            public static CallSite<Func<CallSite, object, bool>> <>p__68;
        }

        [CompilerGenerated]
        private static class <>o__23
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__9;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__10;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__11;
        }
    }
}

