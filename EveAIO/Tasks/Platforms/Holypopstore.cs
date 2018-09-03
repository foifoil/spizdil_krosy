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

    internal class Holypopstore : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private bool _isLoggedIn;
        private Dictionary<string, string> _diData;
        private string _referer;
        private string _version;

        public Holypopstore(TaskRunner runner, TaskObject task)
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
                    this._diData.Add("controller", "orders");
                    this._diData.Add("action", "addStockItemToBasket");
                    this._diData.Add("stockItemId", this._runner.PickedSize.Value.Value);
                    this._diData.Add("quantity", "1");
                    this._diData.Add("extension", "holypop");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                if (this._srr.Contains("Sorry, you have been blacklisted"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.BLACKLISTED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.BLACKLISTED);
                    return false;
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__12.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__12.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__12.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__12.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                }
                if (!<>o__12.<>p__3.Target(<>o__12.<>p__3, <>o__12.<>p__2.Target(<>o__12.<>p__2, <>o__12.<>p__1.Target(<>o__12.<>p__1, <>o__12.<>p__0.Target(<>o__12.<>p__0, this._dynObj, "success")), true)))
                {
                    if (!this._srr.Contains("The item you are trying to purchase is not available"))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.SIZE_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.SIZE_OOS);
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
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception2, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("404") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("404")))
                {
                    if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception2, "", "");
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
            switch (((-1968962954 ^ -1883193504) % 5))
            {
                case 0:
                    goto Label_001C;

                case 1:
                    return false;

                case 2:
                    break;

                case 4:
                    return false;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (!this.SubmitBilling())
            {
            }
            if (-1735638004 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            if (!this._isLoggedIn && !this.Login())
            {
                return false;
            }
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
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
                if (!this._currentDoc.DocumentNode.Descendants("button").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "item-action-btn"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                    return false;
                }
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str2 = EveAIO.Helpers.RemoveHtmlTags(this._runner.ProductPageHtml.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-title"))).InnerText.Trim());
                string str3 = this._runner.ProductPageHtml.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "item-prices"))).InnerText.Trim();
                string str = this._runner.ProductPageHtml.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-image"))).Attributes["src"].Value;
                this._task.ImgUrl = str;
                Product product1 = new Product {
                    ProductTitle = str2,
                    Link = link,
                    Price = str3
                };
                this._runner.Product = product1;
                string str4 = this._srr.Substring(this._srr.IndexOf("preloadedStock"));
                str4 = str4.Substring(str4.IndexOf("["));
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str4.Substring(0, str4.IndexOf("]")));
                if (<>o__17.<>p__6 == null)
                {
                    <>o__17.<>p__6 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Holypopstore)));
                }
                foreach (object obj3 in <>o__17.<>p__6.Target(<>o__17.<>p__6, obj2))
                {
                    if (<>o__17.<>p__2 == null)
                    {
                        <>o__17.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                    }
                    if (<>o__17.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__17.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__17.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__17.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                    }
                    string str5 = <>o__17.<>p__2.Target(<>o__17.<>p__2, <>o__17.<>p__1.Target(<>o__17.<>p__1, <>o__17.<>p__0.Target(<>o__17.<>p__0, obj3, "variant")));
                    if (str5.Contains("US"))
                    {
                        str5 = str5.Substring(0, str5.IndexOf("US")).Trim();
                    }
                    if (<>o__17.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__17.<>p__5 = CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__17.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__17.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__17.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__17.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                    }
                    this._runner.Product.AvailableSizes.Add(<>o__17.<>p__5.Target(<>o__17.<>p__5, typeof(KeyValuePair<string, string>), str5, <>o__17.<>p__4.Target(<>o__17.<>p__4, <>o__17.<>p__3.Target(<>o__17.<>p__3, obj3, "id"))));
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
                    string str7 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str7 = str7 + ch.ToString();
                        }
                    }
                    double num4 = double.Parse(str7.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                        using (List<KeyValuePair<string, string>>.Enumerator enumerator2 = this._runner.Product.AvailableSizes.GetEnumerator())
                        {
                            KeyValuePair<string, string> pair;
                            int num6;
                            goto Label_0893;
                        Label_07C3:
                            pair = enumerator2.Current;
                            List<string> source = new List<string>();
                            if (!pair.Key.Contains(":"))
                            {
                                goto Label_0826;
                            }
                            char[] chArray2 = new char[] { ':' };
                            string[] strArray3 = pair.Key.Split(chArray2);
                            int index = 0;
                            goto Label_081C;
                        Label_0805:
                            source.Add(strArray3[index].Trim());
                            index++;
                        Label_081C:
                            if (index >= strArray3.Length)
                            {
                                goto Label_0834;
                            }
                            goto Label_0805;
                        Label_0826:
                            source.Add(pair.Key);
                        Label_0834:
                            num6 = 0;
                            while (num6 < source.Count)
                            {
                                source[num6] = source[num6].Trim().ToUpperInvariant();
                                num6++;
                            }
                            if (source.Any<string>(x => x == sz))
                            {
                                goto Label_08A1;
                            }
                        Label_0893:
                            if (!enumerator2.MoveNext())
                            {
                                continue;
                            }
                            goto Label_07C3;
                        Label_08A1:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(pair);
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
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
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

        public bool Login()
        {
            if (this._isLoggedIn)
            {
                return true;
            }
            try
            {
                object solverLocker;
                this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGGING_IN, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.holypopstore.com/en/login/signin").Text();
                        this._version = this._srr.Substring(this._srr.IndexOf("b.version"));
                        this._version = this._version.Substring(this._version.IndexOf("'") + 1);
                        this._version = this._version.Substring(0, this._version.IndexOf("'"));
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
                    this._diData.Clear();
                    this._diData.Add("controller", "auth");
                    this._diData.Add("action", "authenticate");
                    this._diData.Add("type", "standard");
                    this._diData.Add("extension", "holypop");
                    this._diData.Add("credential", this._task.Username);
                    this._diData.Add("password", this._task.Password);
                    this._diData.Add("language", "EN");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/login/signin");
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                if (<>o__18.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__18.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__18.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__18.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                }
                if (!<>o__18.<>p__3.Target(<>o__18.<>p__3, <>o__18.<>p__2.Target(<>o__18.<>p__2, <>o__18.<>p__1.Target(<>o__18.<>p__1, <>o__18.<>p__0.Target(<>o__18.<>p__0, this._dynObj, "success")), true)))
                {
                    goto Label_15A6;
                }
                this._isLoggedIn = true;
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_SUCCESSFUL, null, "", "");
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/login/signin");
                        this._srr = this._client.Get("https://www.holypopstore.com/en/login/signin").Text();
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
                if (this._srr.Contains("data-sitekey"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                    solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.HOLYPOP_CAPTCHA_KEY, this._runner.HomeUrl, "Holypopstore");
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
                    this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGGING_IN, null, "", "");
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("action", "captcha_verify");
                            this._diData.Add("captcha", waiter.Token);
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/login/signin");
                            this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
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
                            this._srr = this._client.Get("https://www.holypopstore.com/en").Text();
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
                }
                string str = this._srr.Substring(this._srr.IndexOf("preloadedBasket"));
                str = str.Substring(str.IndexOf("["));
                str = str.Substring(0, str.IndexOf("]") + 1);
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                bool flag3 = false;
                if (<>o__18.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__18.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Holypopstore), argumentInfo));
                }
                solverLocker = <>o__18.<>p__5.Target(<>o__18.<>p__5, <>o__18.<>p__4.Target(<>o__18.<>p__4, this._dynObj), 0);
                if (<>o__18.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                }
                if (!<>o__18.<>p__8.Target(<>o__18.<>p__8, solverLocker))
                {
                    if (<>o__18.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__18.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__18.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__18.<>p__6 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof(Holypopstore), argumentInfo));
                    }
                    if (!<>o__18.<>p__7.Target(<>o__18.<>p__7, <>o__18.<>p__6.Target(<>o__18.<>p__6, solverLocker, flag3)))
                    {
                        goto Label_159E;
                    }
                }
                this._task.Status = States.GetTaskState(States.TaskState.CLEANING_CART);
                States.WriteLogger(this._task, States.LOGGER_STATES.CLEARING_CART, null, "", "");
                if (<>o__18.<>p__15 == null)
                {
                    <>o__18.<>p__15 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Holypopstore)));
                }
                foreach (object obj3 in <>o__18.<>p__15.Target(<>o__18.<>p__15, this._dynObj))
                {
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        this._diData.Clear();
                        this._diData.Add("controller", "orders");
                        this._diData.Add("action", "removeStockItemFromBasket");
                        if (<>o__18.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__11 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__18.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        <>o__18.<>p__11.Target(<>o__18.<>p__11, this._diData, "stockItemId", <>o__18.<>p__10.Target(<>o__18.<>p__10, <>o__18.<>p__9.Target(<>o__18.<>p__9, obj3, "stockId")));
                        this._diData.Add("quantity", "1");
                        if (<>o__18.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__14 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__13 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__18.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        <>o__18.<>p__14.Target(<>o__18.<>p__14, this._diData, "basketItemId", <>o__18.<>p__13.Target(<>o__18.<>p__13, <>o__18.<>p__12.Target(<>o__18.<>p__12, obj3, "id")));
                        this._diData.Add("extension", "holypop");
                        this._diData.Add("language", "EN");
                        this._diData.Add("version", this._version);
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en");
                            this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                            this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                }
                if (this._srr.Contains("Sorry, you have been blacklisted"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.BLACKLISTED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.BLACKLISTED);
                    return false;
                }
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
            Label_0E30:
                if (<>o__18.<>p__23 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__23 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__18.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__18.<>p__17 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__18.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                }
                solverLocker = <>o__18.<>p__17.Target(<>o__18.<>p__17, <>o__18.<>p__16.Target(<>o__18.<>p__16, this._dynObj), null);
                if (<>o__18.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__22 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Holypopstore), argumentInfo));
                }
                if (!<>o__18.<>p__22.Target(<>o__18.<>p__22, solverLocker))
                {
                    if (<>o__18.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__18.<>p__21 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__18.<>p__20 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__18.<>p__20 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__18.<>p__19 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__18.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Holypopstore), argumentInfo));
                    }
                }
                if (<>o__18.<>p__23.Target(<>o__18.<>p__23, (<>o__18.<>p__18 != null) ? solverLocker : <>o__18.<>p__21.Target(<>o__18.<>p__21, solverLocker, <>o__18.<>p__20.Target(<>o__18.<>p__20, <>o__18.<>p__19.Target(<>o__18.<>p__19, <>o__18.<>p__18.Target(<>o__18.<>p__18, this._dynObj)), 0))))
                {
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        this._diData.Clear();
                        this._diData.Add("controller", "orders");
                        this._diData.Add("action", "removeStockItemFromBasket");
                        if (<>o__18.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__28 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__18.<>p__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__24 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                        }
                        <>o__18.<>p__28.Target(<>o__18.<>p__28, this._diData, "stockItemId", <>o__18.<>p__27.Target(<>o__18.<>p__27, <>o__18.<>p__26.Target(<>o__18.<>p__26, <>o__18.<>p__25.Target(<>o__18.<>p__25, <>o__18.<>p__24.Target(<>o__18.<>p__24, this._dynObj)), "stockId")));
                        this._diData.Add("quantity", "1");
                        if (<>o__18.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__33 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__32 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__18.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__18.<>p__29 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                        }
                        <>o__18.<>p__33.Target(<>o__18.<>p__33, this._diData, "basketItemId", <>o__18.<>p__32.Target(<>o__18.<>p__32, <>o__18.<>p__31.Target(<>o__18.<>p__31, <>o__18.<>p__30.Target(<>o__18.<>p__30, <>o__18.<>p__29.Target(<>o__18.<>p__29, this._dynObj)), "id")));
                        this._diData.Add("extension", "holypop");
                        this._diData.Add("language", "EN");
                        this._diData.Add("version", this._version);
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en");
                            this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                            this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                        }
                        catch (WebException exception7)
                        {
                            if (!exception7.Message.Contains("504") && !exception7.Message.Contains("503"))
                            {
                                throw;
                            }
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        continue;
                    Label_1580:
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                        goto Label_0E30;
                    }
                    goto Label_1580;
                }
            Label_159E:
                return true;
            Label_15A6:
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_UNSUCCESSFUL, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.LOGIN_UNSUCCESSFUL);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception8)
            {
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
                this._runner.IsError = true;
                if (exception8 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_LOGIN);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_LOGGING_IN, exception8, "", "Web request timed out");
                }
                else if (!exception8.Message.Contains("404") && ((exception8.InnerException == null) || !exception8.InnerException.Message.Contains("404")))
                {
                    if (!exception8.Message.Contains("430") && ((exception8.InnerException == null) || !exception8.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_LOGIN);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_LOGGING_IN, exception8, "", "");
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.holypopstore.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.holypopstore.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject obj4 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                if (<>o__15.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__15.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "accounts", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethods", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "review", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                }
                object obj3 = <>o__15.<>p__7.Target(<>o__15.<>p__7, <>o__15.<>p__6.Target(<>o__15.<>p__6, <>o__15.<>p__5.Target(<>o__15.<>p__5, <>o__15.<>p__4.Target(<>o__15.<>p__4, <>o__15.<>p__3.Target(<>o__15.<>p__3, <>o__15.<>p__2.Target(<>o__15.<>p__2, <>o__15.<>p__1.Target(<>o__15.<>p__1, <>o__15.<>p__0.Target(<>o__15.<>p__0, this._dynObj)))))), "id"));
                if (<>o__15.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__15.<>p__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "paymentMethods", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "review", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                }
                object obj2 = <>o__15.<>p__13.Target(<>o__15.<>p__13, <>o__15.<>p__12.Target(<>o__15.<>p__12, <>o__15.<>p__11.Target(<>o__15.<>p__11, <>o__15.<>p__10.Target(<>o__15.<>p__10, <>o__15.<>p__9.Target(<>o__15.<>p__9, <>o__15.<>p__8.Target(<>o__15.<>p__8, this._dynObj)))), "id"));
                if (<>o__15.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__15.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                }
                if (<>o__15.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "payload", typeof(Holypopstore), argumentInfo));
                }
                object obj5 = <>o__15.<>p__16.Target(<>o__15.<>p__16, <>o__15.<>p__15.Target(<>o__15.<>p__15, <>o__15.<>p__14.Target(<>o__15.<>p__14, this._dynObj), "orderId"));
                string uriString = "";
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Remove("Origin");
                        KeyValuePair<string, string> pair = this._client.Get($"https://www.holypopstore.com/en/orders/checkout/{obj5}?paymentMethodId={obj2}&paymentMethodAccountId={obj3}").TextResponseUri();
                        this._srr = pair.Key;
                        uriString = pair.Value;
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
                string url = "https://ecomm.sella.it/pagam/" + this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_SwitPayment"))).Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                        this._srr = this._client.Get(url).Text();
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
                string str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"))).Attributes["value"].Value;
                string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"))).Attributes["value"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"))).Attributes["value"].Value;
                string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"))).Attributes["value"].Value;
                string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("__EVENTTARGET", "ctl00$ContentPlaceHolder1$btnProcedi");
                    this._diData.Add("__EVENTARGUMENT", str6);
                    this._diData.Add("__VIEWSTATE", str8);
                    this._diData.Add("__VIEWSTATEGENERATOR", str4);
                    this._diData.Add("__SCROLLPOSITIONX", str3);
                    this._diData.Add("__SCROLLPOSITIONY", str7);
                    this._diData.Add("__EVENTVALIDATION", str5);
                    this._diData.Add("ctl00$ContentPlaceHolder1$textPAY1_CHNAME", obj4.NameOnCard);
                    this._diData.Add("ctl00$ContentPlaceHolder1$textPAY1_CARDNUMBER", obj4.CCNumber);
                    this._diData.Add("ctl00$ContentPlaceHolder1$txtPAY1_EXPMONTH", obj4.ExpiryMonth);
                    this._diData.Add("ctl00$ContentPlaceHolder1$txtPAY1_EXPYEAR", obj4.ExpiryYear.Substring(2));
                    this._diData.Add("ctl00$ContentPlaceHolder1$textPAY1_CVV", obj4.Cvv);
                    this._diData.Add("ctl00$ContentPlaceHolder1$textPAY1_CHEMAIL", "");
                    this._diData.Add("ctl00$ContentPlaceHolder1$hdCookieCheck", "FromPagam");
                    this._diData.Add("ctl00$ContentPlaceHolder1$hidErrorCode", "");
                    this._diData.Add("ctl00$ContentPlaceHolder1$IsDCCFeatureEnabled", "False");
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(url);
                        this._srr = this._client.Post(url, this._diData).Text();
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
                this._currentDoc.LoadHtml(this._srr);
                this._referer = url;
                if (!this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_btnProceedi"))) && !this._currentDoc.DocumentNode.Descendants("a").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_btnProceedi"))))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.INVALID_CREDIT_CARD);
                    States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_CREDIT_CARD, null, "", "");
                    return false;
                }
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception4)
            {
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
                this._runner.IsError = true;
                if (exception4 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception4, "", "Web request timed out");
                }
                else if (!exception4.Message.Contains("404") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("404")))
                {
                    if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception4, "", "");
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
            bool flag3;
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                string url = "https://ecomm.sella.it/pagam/" + WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "aspnetForm"))).Attributes["action"].Value);
                string str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"))).Attributes["value"].Value;
                string str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"))).Attributes["value"].Value;
                string str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"))).Attributes["value"].Value;
                string str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"))).Attributes["value"].Value;
                string str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"))).Attributes["value"].Value;
                string str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"))).Attributes["value"].Value;
                string str26 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__PREVIOUSPAGE"))).Attributes["value"].Value;
                this._diData.Clear();
                this._diData.Add("__EVENTTARGET", "ctl00$ContentPlaceHolder1$btnProceedi");
                this._diData.Add("__EVENTARGUMENT", str12);
                this._diData.Add("__VIEWSTATE", str6);
                this._diData.Add("__VIEWSTATEGENERATOR", str13);
                this._diData.Add("__SCROLLPOSITIONX", str10);
                this._diData.Add("__SCROLLPOSITIONY", str11);
                this._diData.Add("__PREVIOUSPAGE", str26);
                this._diData.Add("__EVENTVALIDATION", str20);
                this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._referer);
                this._srr = this._client.Post(url, this._diData).Text();
                this._currentDoc.LoadHtml(this._srr);
                if (this._srr.Contains("If the Visa authentication window"))
                {
                    bool flag;
                    this._referer = url;
                    url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "form1"))).Attributes["action"].Value;
                    str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"))).Attributes["value"].Value;
                    str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"))).Attributes["value"].Value;
                    str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"))).Attributes["value"].Value;
                    str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"))).Attributes["value"].Value;
                    str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"))).Attributes["value"].Value;
                    str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"))).Attributes["value"].Value;
                    string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "PaReq"))).Attributes["value"].Value;
                    string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "MD"))).Attributes["value"].Value;
                    string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "TermUrl"))).Attributes["value"].Value;
                    this._diData.Clear();
                    this._diData.Add("__VIEWSTATE", str6);
                    this._diData.Add("__VIEWSTATEGENERATOR", str13);
                    this._diData.Add("__SCROLLPOSITIONX", str10);
                    this._diData.Add("__SCROLLPOSITIONY", str11);
                    this._diData.Add("__EVENTTARGET", "");
                    this._diData.Add("__EVENTARGUMENT", str12);
                    this._diData.Add("__EVENTVALIDATION", str20);
                    this._diData.Add("PaReq", str3);
                    this._diData.Add("MD", str4);
                    this._diData.Add("TermUrl", str5);
                    this._diData.Add("PAY1_TRANSKEY", "");
                    for (flag = true; flag; flag = this._srr.Contains("Your payment is being processed"))
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._referer);
                        KeyValuePair<string, string> pair2 = this._client.Post(url, this._diData).TextResponseUri();
                        this._srr = pair2.Key;
                        this._referer = pair2.Value;
                    }
                    this._currentDoc.LoadHtml(this._srr);
                    string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                    str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        this._diData.Clear();
                        this._diData.Add("PaRes", str7);
                        this._diData.Add("MD", str4);
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._referer);
                        this._srr = this._client.Post("https://ecomm.sella.it/pagam/pagam3Do.aspx", this._diData).Text();
                        this._srr.Contains("Transazione in corso");
                    }
                    this._currentDoc.LoadHtml(this._srr);
                    this._srr = this._client.Get("https://ecomm.sella.it/pagam/PagamExe.aspx").Text();
                    this._currentDoc.LoadHtml(this._srr);
                }
                else
                {
                    this._referer = url;
                    url = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "form1"))).Attributes["action"].Value);
                    str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"))).Attributes["value"].Value;
                    str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"))).Attributes["value"].Value;
                    str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"))).Attributes["value"].Value;
                    str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"))).Attributes["value"].Value;
                    str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"))).Attributes["value"].Value;
                    str20 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"))).Attributes["value"].Value;
                    string str17 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "PaReq"))).Attributes["value"].Value;
                    string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "MD"))).Attributes["value"].Value;
                    string str25 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "TermUrl"))).Attributes["value"].Value;
                    this._diData.Clear();
                    this._diData.Add("__VIEWSTATE", str6);
                    this._diData.Add("__VIEWSTATEGENERATOR", str13);
                    this._diData.Add("__SCROLLPOSITIONX", str10);
                    this._diData.Add("__SCROLLPOSITIONY", str11);
                    this._diData.Add("__EVENTTARGET", "");
                    this._diData.Add("__EVENTARGUMENT", str12);
                    this._diData.Add("__EVENTVALIDATION", str20);
                    this._diData.Add("PaReq", str17);
                    this._diData.Add("MD", str2);
                    this._diData.Add("TermUrl", str25);
                    this._diData.Add("PAY1_TRANSKEY", "");
                    this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._referer);
                    KeyValuePair<string, string> pair = this._client.Post(url, this._diData).TextResponseUri();
                    this._srr = pair.Key;
                    this._referer = pair.Value;
                    this._currentDoc.LoadHtml(this._srr);
                    string str16 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                    str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                    string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"))).Attributes["value"].Value;
                    string str21 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"))).Attributes["value"].Value;
                    string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"))).Attributes["value"].Value;
                    string str24 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"))).Attributes["value"].Value;
                    string str18 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"))).Attributes["value"].Value;
                    string str19 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"))).Attributes["value"].Value;
                    string str22 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"))).Attributes["value"].Value;
                    string str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"))).Attributes["value"].Value;
                    string str23 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"))).Attributes["value"].Value;
                    bool flag2 = true;
                    while (flag2)
                    {
                        flag2 = false;
                        this._diData.Clear();
                        this._diData.Add("PaRes", str16);
                        this._diData.Add("MD", str2);
                        this._diData.Add("PaReq", str8);
                        this._diData.Add("ABSlog", str9);
                        this._diData.Add("deviceDNA", str21);
                        this._diData.Add("executionTime", str);
                        this._diData.Add("dnaError", str24);
                        this._diData.Add("mesc", str18);
                        this._diData.Add("mescIterationCount", str19);
                        this._diData.Add("desc", str22);
                        this._diData.Add("isDNADone", str15);
                        this._diData.Add("arcotFlashCookie", str23);
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._referer);
                        this._srr = this._client.Post("https://ecomm.sella.it/pagam/pagam3Do.aspx", this._diData).Text();
                        this._srr.Contains("Transazione in corso");
                    }
                    this._currentDoc.LoadHtml(this._srr);
                    this._srr = this._client.Get("https://ecomm.sella.it/pagam/PagamExe.aspx").Text();
                    this._currentDoc.LoadHtml(this._srr);
                }
                if (!this._srr.ToUpperInvariant().Contains("IL VOSTRO PAGAMENTO NON E' STATO AUTORIZZATO"))
                {
                    try
                    {
                        EveAIO.Helpers.AddDbValue("Holypop|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    }
                    catch
                    {
                    }
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                flag3 = false;
            }
            catch (ThreadAbortException)
            {
                flag3 = false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (exception is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "Web request timed out");
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
                flag3 = false;
            }
            finally
            {
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
            }
            return flag3;
        }

        private bool SubmitShipping()
        {
            try
            {
                IEnumerator enumerator;
                object obj5;
                object obj6;
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                obj2.Email.Trim();
                string str2 = obj2.FirstName.Trim();
                string str11 = obj2.LastName.Trim();
                string str12 = obj2.Address1.Trim();
                string str13 = obj2.Address2.Trim();
                string str16 = obj2.City.Trim();
                string str9 = obj2.Zip.Trim();
                string str = obj2.CountryId.ToUpperInvariant();
                string state = "";
                if ((str != "US") && (str != "CA"))
                {
                    state = obj2.State;
                }
                else
                {
                    state = obj2.StateId;
                }
                string str14 = obj2.Phone.Trim();
                string country = obj2.Country;
                string key = WebsitesInfo.HOLYPOP_COUNTRIES.First<KeyValuePair<string, string>>(x => (x.Value.ToUpperInvariant() == country.ToUpperInvariant())).Key;
                obj2.EmailShipping.Trim();
                string str4 = obj2.FirstNameShipping.Trim();
                string str7 = obj2.LastNameShipping.Trim();
                string str8 = obj2.Address1Shipping.Trim();
                string str18 = obj2.Address2Shipping.Trim();
                string str19 = obj2.CityShipping.Trim();
                string str17 = obj2.ZipShipping.Trim();
                string str10 = obj2.CountryIdShipping.ToUpperInvariant();
                string stateShipping = "";
                if ((str10 != "US") && (str10 != "CA"))
                {
                    stateShipping = obj2.StateShipping;
                }
                else
                {
                    stateShipping = obj2.StateIdShipping;
                }
                string str5 = obj2.PhoneShipping.Trim();
                string countryShipping = obj2.CountryShipping;
                string str20 = WebsitesInfo.HOLYPOP_COUNTRIES.First<KeyValuePair<string, string>>(x => (x.Value.ToUpperInvariant() == country.ToUpperInvariant())).Key;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("extension", "geo");
                    this._diData.Add("controller", "countries");
                    this._diData.Add("action", "retrieve_division");
                    this._diData.Add("countryId", key);
                    this._diData.Add("language", "EN");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/orders/review");
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                object obj3 = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                string str21 = "";
                string str22 = "";
                if ((obj2.CountryId == "CA") || (obj2.CountryId == "US"))
                {
                    if (<>o__16.<>p__11 == null)
                    {
                        <>o__16.<>p__11 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Holypopstore)));
                    }
                    if (<>o__16.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                    }
                    using (enumerator = <>o__16.<>p__11.Target(<>o__16.<>p__11, <>o__16.<>p__0.Target(<>o__16.<>p__0, obj3)).GetEnumerator())
                    {
                        object current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (<>o__16.<>p__4 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__16.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                            }
                            if (<>o__16.<>p__3 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__16.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Holypopstore), argumentInfo));
                            }
                            if (<>o__16.<>p__2 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__16.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                            }
                            if (<>o__16.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__16.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                            }
                            if (<>o__16.<>p__4.Target(<>o__16.<>p__4, <>o__16.<>p__3.Target(<>o__16.<>p__3, <>o__16.<>p__2.Target(<>o__16.<>p__2, <>o__16.<>p__1.Target(<>o__16.<>p__1, current, "display_name")), obj2.StateId)))
                            {
                                goto Label_05A7;
                            }
                        }
                        goto Label_0784;
                    Label_05A7:
                        if (<>o__16.<>p__7 == null)
                        {
                            <>o__16.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        str21 = <>o__16.<>p__7.Target(<>o__16.<>p__7, <>o__16.<>p__6.Target(<>o__16.<>p__6, <>o__16.<>p__5.Target(<>o__16.<>p__5, current, "id")));
                        if (<>o__16.<>p__10 == null)
                        {
                            <>o__16.<>p__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        str22 = <>o__16.<>p__10.Target(<>o__16.<>p__10, <>o__16.<>p__9.Target(<>o__16.<>p__9, <>o__16.<>p__8.Target(<>o__16.<>p__8, current, "display_name")));
                    }
                }
            Label_0784:
                if (obj2.CountryId != "RU")
                {
                    if (<>o__16.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__24 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__16.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__16.<>p__13 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__16.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                    }
                    obj5 = <>o__16.<>p__13.Target(<>o__16.<>p__13, <>o__16.<>p__12.Target(<>o__16.<>p__12, obj3), null);
                    if (<>o__16.<>p__18 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Holypopstore), argumentInfo));
                    }
                    if (!<>o__16.<>p__18.Target(<>o__16.<>p__18, obj5))
                    {
                        if (<>o__16.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__17 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__16 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Holypopstore), argumentInfo));
                        }
                    }
                    obj6 = (<>o__16.<>p__14 != null) ? obj5 : <>o__16.<>p__17.Target(<>o__16.<>p__17, obj5, <>o__16.<>p__16.Target(<>o__16.<>p__16, <>o__16.<>p__15.Target(<>o__16.<>p__15, <>o__16.<>p__14.Target(<>o__16.<>p__14, obj3)), "False"));
                    if (<>o__16.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__23 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Holypopstore), argumentInfo));
                    }
                    if (!<>o__16.<>p__23.Target(<>o__16.<>p__23, obj6))
                    {
                        if (<>o__16.<>p__22 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__22 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__21 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Holypopstore), argumentInfo));
                        }
                    }
                    if (<>o__16.<>p__24.Target(<>o__16.<>p__24, (<>o__16.<>p__19 != null) ? obj6 : <>o__16.<>p__22.Target(<>o__16.<>p__22, obj6, <>o__16.<>p__21.Target(<>o__16.<>p__21, <>o__16.<>p__20.Target(<>o__16.<>p__20, <>o__16.<>p__19.Target(<>o__16.<>p__19, obj3)), 0))))
                    {
                        if (<>o__16.<>p__29 == null)
                        {
                            <>o__16.<>p__29 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__28 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__27 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__26 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__25 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                        }
                        str21 = <>o__16.<>p__29.Target(<>o__16.<>p__29, <>o__16.<>p__28.Target(<>o__16.<>p__28, <>o__16.<>p__27.Target(<>o__16.<>p__27, <>o__16.<>p__26.Target(<>o__16.<>p__26, <>o__16.<>p__25.Target(<>o__16.<>p__25, obj3)), "id")));
                        if (<>o__16.<>p__34 == null)
                        {
                            <>o__16.<>p__34 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__33 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__32 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__32 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__31 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__30 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                        }
                        str22 = <>o__16.<>p__34.Target(<>o__16.<>p__34, <>o__16.<>p__33.Target(<>o__16.<>p__33, <>o__16.<>p__32.Target(<>o__16.<>p__32, <>o__16.<>p__31.Target(<>o__16.<>p__31, <>o__16.<>p__30.Target(<>o__16.<>p__30, obj3)), "display_name")));
                    }
                }
                else
                {
                    str22 = state;
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("extension", "geo");
                    this._diData.Add("controller", "countries");
                    this._diData.Add("action", "retrieve_division");
                    this._diData.Add("countryId", str20);
                    this._diData.Add("language", "EN");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/orders/review");
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                object obj7 = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                string str23 = "";
                string str24 = "";
                if ((obj2.CountryIdShipping == "CA") || (obj2.CountryIdShipping == "US"))
                {
                    if (<>o__16.<>p__46 == null)
                    {
                        <>o__16.<>p__46 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Holypopstore)));
                    }
                    if (<>o__16.<>p__35 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                    }
                    using (enumerator = <>o__16.<>p__46.Target(<>o__16.<>p__46, <>o__16.<>p__35.Target(<>o__16.<>p__35, obj7)).GetEnumerator())
                    {
                        object obj8;
                        goto Label_1246;
                    Label_10FB:
                        obj8 = enumerator.Current;
                        if (<>o__16.<>p__39 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__39 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__38 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__38 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__37 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__37 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__36 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__36 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__39.Target(<>o__16.<>p__39, <>o__16.<>p__38.Target(<>o__16.<>p__38, <>o__16.<>p__37.Target(<>o__16.<>p__37, <>o__16.<>p__36.Target(<>o__16.<>p__36, obj8, "display_name")), obj2.StateId)))
                        {
                            goto Label_1257;
                        }
                    Label_1246:
                        if (!enumerator.MoveNext())
                        {
                            goto Label_1434;
                        }
                        goto Label_10FB;
                    Label_1257:
                        if (<>o__16.<>p__42 == null)
                        {
                            <>o__16.<>p__42 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__41 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__40 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        str23 = <>o__16.<>p__42.Target(<>o__16.<>p__42, <>o__16.<>p__41.Target(<>o__16.<>p__41, <>o__16.<>p__40.Target(<>o__16.<>p__40, obj8, "id")));
                        if (<>o__16.<>p__45 == null)
                        {
                            <>o__16.<>p__45 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__44 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__43 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__43 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        str24 = <>o__16.<>p__45.Target(<>o__16.<>p__45, <>o__16.<>p__44.Target(<>o__16.<>p__44, <>o__16.<>p__43.Target(<>o__16.<>p__43, obj8, "display_name")));
                    }
                }
            Label_1434:
                if (obj2.CountryIdShipping != "RU")
                {
                    if (<>o__16.<>p__59 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__59 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__16.<>p__48 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                        <>o__16.<>p__48 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Holypopstore), argumentInfo));
                    }
                    if (<>o__16.<>p__47 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                    }
                    obj5 = <>o__16.<>p__48.Target(<>o__16.<>p__48, <>o__16.<>p__47.Target(<>o__16.<>p__47, obj7), null);
                    if (<>o__16.<>p__53 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__53 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Holypopstore), argumentInfo));
                    }
                    if (!<>o__16.<>p__53.Target(<>o__16.<>p__53, obj5))
                    {
                        if (<>o__16.<>p__52 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__52 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__51 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__51 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__50 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Holypopstore), argumentInfo));
                        }
                    }
                    obj6 = (<>o__16.<>p__49 != null) ? obj5 : <>o__16.<>p__52.Target(<>o__16.<>p__52, obj5, <>o__16.<>p__51.Target(<>o__16.<>p__51, <>o__16.<>p__50.Target(<>o__16.<>p__50, <>o__16.<>p__49.Target(<>o__16.<>p__49, obj7)), "False"));
                    if (<>o__16.<>p__58 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__58 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Holypopstore), argumentInfo));
                    }
                    if (!<>o__16.<>p__58.Target(<>o__16.<>p__58, obj6))
                    {
                        if (<>o__16.<>p__57 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__57 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__56 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__56 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__55 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(Holypopstore), argumentInfo));
                        }
                    }
                    if (<>o__16.<>p__59.Target(<>o__16.<>p__59, (<>o__16.<>p__54 != null) ? obj6 : <>o__16.<>p__57.Target(<>o__16.<>p__57, obj6, <>o__16.<>p__56.Target(<>o__16.<>p__56, <>o__16.<>p__55.Target(<>o__16.<>p__55, <>o__16.<>p__54.Target(<>o__16.<>p__54, obj7)), 0))))
                    {
                        if (<>o__16.<>p__64 == null)
                        {
                            <>o__16.<>p__64 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__63 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__63 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__62 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__62 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__61 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__61 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__60 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                        }
                        str23 = <>o__16.<>p__64.Target(<>o__16.<>p__64, <>o__16.<>p__63.Target(<>o__16.<>p__63, <>o__16.<>p__62.Target(<>o__16.<>p__62, <>o__16.<>p__61.Target(<>o__16.<>p__61, <>o__16.<>p__60.Target(<>o__16.<>p__60, obj7)), "id")));
                        if (<>o__16.<>p__69 == null)
                        {
                            <>o__16.<>p__69 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Holypopstore)));
                        }
                        if (<>o__16.<>p__68 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__67 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__67 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__66 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "First", typeof(Holypopstore), argumentInfo));
                        }
                        if (<>o__16.<>p__65 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__65 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof(Holypopstore), argumentInfo));
                        }
                        str24 = <>o__16.<>p__69.Target(<>o__16.<>p__69, <>o__16.<>p__68.Target(<>o__16.<>p__68, <>o__16.<>p__67.Target(<>o__16.<>p__67, <>o__16.<>p__66.Target(<>o__16.<>p__66, <>o__16.<>p__65.Target(<>o__16.<>p__65, obj7)), "display_name")));
                    }
                }
                else
                {
                    str24 = stateShipping;
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("secretly", "false");
                    this._diData.Add("hardErrorize", "false");
                    this._diData.Add("billingAddressId", "n0");
                    this._diData.Add("shippingAddressId", "n0");
                    this._diData.Add("newAddresses[0][counter]", "n0");
                    this._diData.Add("newAddresses[0][first_name]", str2);
                    this._diData.Add("newAddresses[0][last_name]", str11);
                    this._diData.Add("newAddresses[0][full_name]", str2 + " " + str11);
                    this._diData.Add("newAddresses[0][street_address]", str12 + " " + str13);
                    this._diData.Add("newAddresses[0][zipcode]", str9);
                    this._diData.Add("newAddresses[0][cityName]", str16);
                    this._diData.Add("newAddresses[0][statecode]", str22);
                    if (obj2.CountryId != "RU")
                    {
                        this._diData.Add("newAddresses[0][regionId]", str21);
                    }
                    this._diData.Add("newAddresses[0][countryId]", key);
                    this._diData.Add("newAddresses[0][phone_number]", str14);
                    this._diData.Add("newAddresses[0][isDefault]", "0");
                    this._diData.Add("requestInvoice", "0");
                    this._diData.Add("notes", "");
                    this._diData.Add("toDisplay", "1");
                    this._diData.Add("extension", "holypop");
                    this._diData.Add("controller", "orders");
                    this._diData.Add("action", "review");
                    this._diData.Add("clearSession", "0");
                    this._diData.Add("language", "EN");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/orders/review");
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                    this._diData.Clear();
                    this._diData.Add("extension", "geo");
                    this._diData.Add("controller", "countries");
                    this._diData.Add("action", "retrieve_division");
                    this._diData.Add("countryId", str20);
                    this._diData.Add("language", "EN");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/orders/review");
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                    this._diData.Clear();
                    this._diData.Add("secretly", "false");
                    this._diData.Add("hardErrorize", "false");
                    this._diData.Add("billingAddressId", "n0");
                    this._diData.Add("shippingAddressId", "n1");
                    this._diData.Add("newAddresses[0][counter]", "n0");
                    this._diData.Add("newAddresses[0][first_name]", str2);
                    this._diData.Add("newAddresses[0][last_name]", str11);
                    this._diData.Add("newAddresses[0][full_name]", str2 + " " + str11);
                    this._diData.Add("newAddresses[0][street_address]", str12 + " " + str13);
                    this._diData.Add("newAddresses[0][zipcode]", str9);
                    this._diData.Add("newAddresses[0][cityName]", str16);
                    this._diData.Add("newAddresses[0][statecode]", str22);
                    if (obj2.CountryId != "RU")
                    {
                        this._diData.Add("newAddresses[0][regionId]", str21);
                    }
                    this._diData.Add("newAddresses[0][countryId]", key);
                    this._diData.Add("newAddresses[0][phone_number]", str14);
                    this._diData.Add("newAddresses[0][isDefault]", "0");
                    this._diData.Add("newAddresses[1][counter]", "n1");
                    this._diData.Add("newAddresses[1][first_name]", str4);
                    this._diData.Add("newAddresses[1][last_name]", str7);
                    this._diData.Add("newAddresses[1][full_name]", str4 + " " + str7);
                    this._diData.Add("newAddresses[1][street_address]", str8 + " " + str18);
                    this._diData.Add("newAddresses[1][zipcode]", str17);
                    this._diData.Add("newAddresses[1][cityName]", str19);
                    this._diData.Add("newAddresses[1][statecode]", str24);
                    if (obj2.CountryIdShipping != "RU")
                    {
                        this._diData.Add("newAddresses[1][regionId]", str23);
                    }
                    this._diData.Add("newAddresses[1][countryId]", str20);
                    this._diData.Add("newAddresses[1][phone_number]", str5);
                    this._diData.Add("newAddresses[1][isDefault]", "0");
                    this._diData.Add("requestInvoice", "0");
                    this._diData.Add("notes", "");
                    this._diData.Add("paymentMethodId", "12");
                    this._diData.Add("paymentMethodAccountId", "4");
                    this._diData.Add("shipments[0][addressId]", "n1");
                    this._diData.Add("shipments[0][shipperId]", "1");
                    this._diData.Add("shipments[0][shipperAccountId]", "1");
                    this._diData.Add("toDisplay", "1");
                    this._diData.Add("extension", "holypop");
                    this._diData.Add("controller", "orders");
                    this._diData.Add("action", "review");
                    this._diData.Add("clearSession", "0");
                    this._diData.Add("language", "EN");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/orders/review");
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("secretly", "false");
                    this._diData.Add("hardErrorize", "false");
                    this._diData.Add("billingAddressId", "n0");
                    this._diData.Add("shippingAddressId", "n1");
                    this._diData.Add("newAddresses[0][counter]", "n0");
                    this._diData.Add("newAddresses[0][first_name]", str2);
                    this._diData.Add("newAddresses[0][last_name]", str11);
                    this._diData.Add("newAddresses[0][full_name]", str2 + " " + str11);
                    this._diData.Add("newAddresses[0][street_address]", str12 + " " + str13);
                    this._diData.Add("newAddresses[0][zipcode]", str9);
                    this._diData.Add("newAddresses[0][cityName]", str16);
                    this._diData.Add("newAddresses[0][statecode]", str22);
                    if (obj2.CountryId != "RU")
                    {
                        this._diData.Add("newAddresses[0][regionId]", str21);
                    }
                    this._diData.Add("newAddresses[0][countryId]", key);
                    this._diData.Add("newAddresses[0][phone_number]", str14);
                    this._diData.Add("newAddresses[0][isDefault]", "0");
                    this._diData.Add("newAddresses[1][counter]", "n1");
                    this._diData.Add("newAddresses[1][first_name]", str4);
                    this._diData.Add("newAddresses[1][last_name]", str7);
                    this._diData.Add("newAddresses[1][full_name]", str4 + " " + str7);
                    this._diData.Add("newAddresses[1][street_address]", str8 + " " + str18);
                    this._diData.Add("newAddresses[1][zipcode]", str17);
                    this._diData.Add("newAddresses[1][cityName]", str19);
                    this._diData.Add("newAddresses[1][statecode]", str24);
                    if (obj2.CountryIdShipping != "RU")
                    {
                        this._diData.Add("newAddresses[1][regionId]", str23);
                    }
                    this._diData.Add("newAddresses[1][countryId]", str20);
                    this._diData.Add("newAddresses[1][phone_number]", str5);
                    this._diData.Add("newAddresses[1][isDefault]", "0");
                    this._diData.Add("requestInvoice", "0");
                    this._diData.Add("notes", "");
                    this._diData.Add("paymentMethodId", "12");
                    this._diData.Add("paymentMethodAccountId", "4");
                    this._diData.Add("shipments[0][addressId]", "n1");
                    this._diData.Add("shipments[0][shipperId]", "1");
                    this._diData.Add("shipments[0][shipperAccountId]", "1");
                    this._diData.Add("toDisplay", "1");
                    this._diData.Add("extension", "holypop");
                    this._diData.Add("controller", "orders");
                    this._diData.Add("action", "save");
                    this._diData.Add("language", "EN");
                    this._diData.Add("version", this._version);
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.holypopstore.com/en/orders/review");
                        this._srr = this._client.Post("https://www.holypopstore.com/index.php", this._diData).Text();
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
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
                this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception7)
            {
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
                this._runner.IsError = true;
                if (exception7 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception7, "", "Web request timed out");
                }
                else if (!exception7.Message.Contains("404") && ((exception7.InnerException == null) || !exception7.InnerException.Message.Contains("404")))
                {
                    if (!exception7.Message.Contains("430") && ((exception7.InnerException == null) || !exception7.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception7, "", "");
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
            public static readonly Holypopstore.<>c <>9;
            public static Func<HtmlNode, bool> <>9__14_0;
            public static Func<HtmlNode, bool> <>9__14_1;
            public static Func<HtmlNode, bool> <>9__14_2;
            public static Func<HtmlNode, bool> <>9__14_3;
            public static Func<HtmlNode, bool> <>9__14_4;
            public static Func<HtmlNode, bool> <>9__14_5;
            public static Func<HtmlNode, bool> <>9__14_6;
            public static Func<HtmlNode, bool> <>9__14_7;
            public static Func<HtmlNode, bool> <>9__14_8;
            public static Func<HtmlNode, bool> <>9__14_9;
            public static Func<HtmlNode, bool> <>9__14_10;
            public static Func<HtmlNode, bool> <>9__14_11;
            public static Func<HtmlNode, bool> <>9__14_12;
            public static Func<HtmlNode, bool> <>9__14_13;
            public static Func<HtmlNode, bool> <>9__14_14;
            public static Func<HtmlNode, bool> <>9__14_15;
            public static Func<HtmlNode, bool> <>9__14_16;
            public static Func<HtmlNode, bool> <>9__14_17;
            public static Func<HtmlNode, bool> <>9__14_18;
            public static Func<HtmlNode, bool> <>9__14_19;
            public static Func<HtmlNode, bool> <>9__14_20;
            public static Func<HtmlNode, bool> <>9__14_21;
            public static Func<HtmlNode, bool> <>9__14_22;
            public static Func<HtmlNode, bool> <>9__14_23;
            public static Func<HtmlNode, bool> <>9__14_24;
            public static Func<HtmlNode, bool> <>9__14_25;
            public static Func<HtmlNode, bool> <>9__14_26;
            public static Func<HtmlNode, bool> <>9__14_27;
            public static Func<HtmlNode, bool> <>9__14_28;
            public static Func<HtmlNode, bool> <>9__14_29;
            public static Func<HtmlNode, bool> <>9__14_30;
            public static Func<HtmlNode, bool> <>9__14_31;
            public static Func<HtmlNode, bool> <>9__14_32;
            public static Func<HtmlNode, bool> <>9__14_33;
            public static Func<HtmlNode, bool> <>9__14_34;
            public static Func<HtmlNode, bool> <>9__14_35;
            public static Func<HtmlNode, bool> <>9__14_36;
            public static Func<HtmlNode, bool> <>9__14_37;
            public static Func<HtmlNode, bool> <>9__14_38;
            public static Func<HtmlNode, bool> <>9__14_39;
            public static Func<HtmlNode, bool> <>9__14_40;
            public static Func<HtmlNode, bool> <>9__14_41;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__15_2;
            public static Func<HtmlNode, bool> <>9__15_3;
            public static Func<HtmlNode, bool> <>9__15_4;
            public static Func<HtmlNode, bool> <>9__15_5;
            public static Func<HtmlNode, bool> <>9__15_6;
            public static Func<HtmlNode, bool> <>9__15_7;
            public static Func<HtmlNode, bool> <>9__15_8;
            public static Func<HtmlNode, bool> <>9__15_9;
            public static Func<HtmlNode, bool> <>9__17_0;
            public static Func<HtmlNode, bool> <>9__17_1;
            public static Func<HtmlNode, bool> <>9__17_2;
            public static Func<HtmlNode, bool> <>9__17_3;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Holypopstore.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__17_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "item-action-btn"));

            internal bool <DirectLink>b__17_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-title"));

            internal bool <DirectLink>b__17_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "item-prices"));

            internal bool <DirectLink>b__17_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-image"));

            internal bool <SubmitBilling>b__15_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_SwitPayment"));

            internal bool <SubmitBilling>b__15_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"));

            internal bool <SubmitBilling>b__15_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"));

            internal bool <SubmitBilling>b__15_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"));

            internal bool <SubmitBilling>b__15_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"));

            internal bool <SubmitBilling>b__15_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"));

            internal bool <SubmitBilling>b__15_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"));

            internal bool <SubmitBilling>b__15_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_btnProceedi"));

            internal bool <SubmitBilling>b__15_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_btnProceedi"));

            internal bool <SubmitOrder>b__14_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "aspnetForm"));

            internal bool <SubmitOrder>b__14_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"));

            internal bool <SubmitOrder>b__14_10(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"));

            internal bool <SubmitOrder>b__14_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"));

            internal bool <SubmitOrder>b__14_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"));

            internal bool <SubmitOrder>b__14_13(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"));

            internal bool <SubmitOrder>b__14_14(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"));

            internal bool <SubmitOrder>b__14_15(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "PaReq"));

            internal bool <SubmitOrder>b__14_16(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "MD"));

            internal bool <SubmitOrder>b__14_17(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "TermUrl"));

            internal bool <SubmitOrder>b__14_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__14_19(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__14_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"));

            internal bool <SubmitOrder>b__14_20(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "form1"));

            internal bool <SubmitOrder>b__14_21(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"));

            internal bool <SubmitOrder>b__14_22(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATE"));

            internal bool <SubmitOrder>b__14_23(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"));

            internal bool <SubmitOrder>b__14_24(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"));

            internal bool <SubmitOrder>b__14_25(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"));

            internal bool <SubmitOrder>b__14_26(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"));

            internal bool <SubmitOrder>b__14_27(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "PaReq"));

            internal bool <SubmitOrder>b__14_28(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "MD"));

            internal bool <SubmitOrder>b__14_29(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "TermUrl"));

            internal bool <SubmitOrder>b__14_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__VIEWSTATEGENERATOR"));

            internal bool <SubmitOrder>b__14_30(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__14_31(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__14_32(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitOrder>b__14_33(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"));

            internal bool <SubmitOrder>b__14_34(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"));

            internal bool <SubmitOrder>b__14_35(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"));

            internal bool <SubmitOrder>b__14_36(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"));

            internal bool <SubmitOrder>b__14_37(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"));

            internal bool <SubmitOrder>b__14_38(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"));

            internal bool <SubmitOrder>b__14_39(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"));

            internal bool <SubmitOrder>b__14_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONX"));

            internal bool <SubmitOrder>b__14_40(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"));

            internal bool <SubmitOrder>b__14_41(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"));

            internal bool <SubmitOrder>b__14_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__SCROLLPOSITIONY"));

            internal bool <SubmitOrder>b__14_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTVALIDATION"));

            internal bool <SubmitOrder>b__14_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__PREVIOUSPAGE"));

            internal bool <SubmitOrder>b__14_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "form1"));

            internal bool <SubmitOrder>b__14_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "__EVENTARGUMENT"));
        }

        [CompilerGenerated]
        private static class <>o__12
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__15
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
        }

        [CompilerGenerated]
        private static class <>o__16
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
            public static CallSite<Func<CallSite, object, string, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string>> <>p__10;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, bool>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, int, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, bool>> <>p__23;
            public static CallSite<Func<CallSite, object, bool>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, string, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, string>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, object, string, object>> <>p__32;
            public static CallSite<Func<CallSite, object, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string, object>> <>p__36;
            public static CallSite<Func<CallSite, object, object>> <>p__37;
            public static CallSite<Func<CallSite, object, string, object>> <>p__38;
            public static CallSite<Func<CallSite, object, bool>> <>p__39;
            public static CallSite<Func<CallSite, object, string, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, string>> <>p__42;
            public static CallSite<Func<CallSite, object, string, object>> <>p__43;
            public static CallSite<Func<CallSite, object, object>> <>p__44;
            public static CallSite<Func<CallSite, object, string>> <>p__45;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__46;
            public static CallSite<Func<CallSite, object, object>> <>p__47;
            public static CallSite<Func<CallSite, object, object, object>> <>p__48;
            public static CallSite<Func<CallSite, object, object>> <>p__49;
            public static CallSite<Func<CallSite, object, object>> <>p__50;
            public static CallSite<Func<CallSite, object, string, object>> <>p__51;
            public static CallSite<Func<CallSite, object, object, object>> <>p__52;
            public static CallSite<Func<CallSite, object, bool>> <>p__53;
            public static CallSite<Func<CallSite, object, object>> <>p__54;
            public static CallSite<Func<CallSite, object, object>> <>p__55;
            public static CallSite<Func<CallSite, object, int, object>> <>p__56;
            public static CallSite<Func<CallSite, object, object, object>> <>p__57;
            public static CallSite<Func<CallSite, object, bool>> <>p__58;
            public static CallSite<Func<CallSite, object, bool>> <>p__59;
            public static CallSite<Func<CallSite, object, object>> <>p__60;
            public static CallSite<Func<CallSite, object, object>> <>p__61;
            public static CallSite<Func<CallSite, object, string, object>> <>p__62;
            public static CallSite<Func<CallSite, object, object>> <>p__63;
            public static CallSite<Func<CallSite, object, string>> <>p__64;
            public static CallSite<Func<CallSite, object, object>> <>p__65;
            public static CallSite<Func<CallSite, object, object>> <>p__66;
            public static CallSite<Func<CallSite, object, string, object>> <>p__67;
            public static CallSite<Func<CallSite, object, object>> <>p__68;
            public static CallSite<Func<CallSite, object, string>> <>p__69;
        }

        [CompilerGenerated]
        private static class <>o__17
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>> <>p__5;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__6;
        }

        [CompilerGenerated]
        private static class <>o__18
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, int, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
            public static CallSite<Func<CallSite, object, bool>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, int, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, bool>> <>p__22;
            public static CallSite<Func<CallSite, object, bool>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, object>> <>p__25;
            public static CallSite<Func<CallSite, object, string, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, object>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__33;
        }
    }
}

