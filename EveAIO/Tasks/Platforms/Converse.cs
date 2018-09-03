namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using EveAIO.Tasks.Dto;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Converse : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;
        private string _homeLink;
        private string _demandwareLink;
        private bool _isStyleSearch;
        private string _async;

        public Converse(TaskRunner runner, TaskObject task)
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
            this.GetSensorData();
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                this._client.Handler.CookieContainer.List();
                bool flag = true;
                PumaProduct product = this._runner.Product.AvailablePumaSizes.First<PumaProduct>(x => x.Id == this._runner.PickedSize.Value.Value);
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Get(product.A + "&Quantity=1&format=ajax").Text();
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
                flag = true;
                while (flag)
                {
                    this._currentDoc.LoadHtml(this._srr);
                    string str = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "productId"))).InnerText.Trim();
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("pid", str);
                    this._diData.Add("Quantity", "1");
                    this._diData.Add("cartAction", "add");
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        KeyValuePair<string, string> pair2 = this._client.Post($"{this._demandwareLink}/Cart-AddProduct?format=ajax", this._diData).TextResponseUri();
                        string local1 = pair2.Value;
                        this._srr = pair2.Key;
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
                this._currentDoc.LoadHtml(this._srr);
                if (!this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-image"))))
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
                this._runner.IsError = true;
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
            switch (((0x744f02a9 ^ 0x78616869) % 5))
            {
                case 0:
                    goto Label_001C;

                case 1:
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
            if (0x7aa97d32 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            bool flag2;
            try
            {
                List<PumaProduct>.Enumerator enumerator4;
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                if (string.IsNullOrEmpty(this._async))
                {
                    Task.Factory.StartNew(delegate {
                        this._async = this._client.Get("http://www.converse.com/_bm/async.js").Text();
                    });
                }
                bool flag = true;
                string url = link;
                if (url.Contains("?"))
                {
                    url = url.Substring(0, link.LastIndexOf("?"));
                }
                CookieContainer container = new CookieContainer();
                if (!this._isStyleSearch)
                {
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            HttpResponseMessage message = this._client.Get(url);
                            this._srr = message.Content.ReadAsStringAsync().Result.ToString();
                            foreach (string str2 in message.Headers.GetValues("Set-Cookie"))
                            {
                                string str3 = str2.Substring(0, str2.IndexOf(";"));
                                string str5 = str3.Substring(0, str3.IndexOf("="));
                                string str4 = str3.Substring(str3.IndexOf("=") + 1);
                                if (str4.Contains(","))
                                {
                                    str4 = WebUtility.UrlEncode(str4);
                                }
                                Cookie cookie = new Cookie {
                                    Name = str5,
                                    Value = str4,
                                    Domain = ".converse.com"
                                };
                                container.Add(cookie);
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
                }
                this._runner.Cookies = container;
                this._client = null;
                this.SetClient();
                this._client.Handler.CookieContainer.List();
                this._currentDoc.LoadHtml(this._srr);
                string str6 = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_login"))).Attributes["action"].Value;
                this._demandwareLink = str6.Substring(0, str6.IndexOf("/Account-LoginUser"));
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    this._homeLink = this._task.HomeUrl;
                    char[] chArray1 = new char[] { '/' };
                    string[] strArray = this._task.Link.Replace(this._homeLink, "").Split(chArray1);
                    string[] textArray1 = new string[] { this._homeLink, "/", strArray[1], "/", strArray[2] };
                    this._homeLink = string.Concat(textArray1);
                    this._runner.ProductPageHtml = new HtmlDocument();
                    this._runner.ProductPageHtml.LoadHtml(this._srr);
                    this._homeLink = this._homeLink.Replace("http:", "https:").Replace("/regular", "");
                }
                else
                {
                    this._homeLink = this._task.Link.Replace("http:", "https:").Replace("/regular", "");
                }
                string str7 = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name single-line"))).InnerText.Trim());
                string str8 = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim());
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value;
                Product product1 = new Product {
                    ProductTitle = str7,
                    Link = link,
                    Price = str8
                };
                this._runner.Product = product1;
                if (!this._isStyleSearch)
                {
                    List<string> list = new List<string>();
                    foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "color-variations-wrapper"))).Descendants("li")
                        where ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("swatch ")) && (x.Attributes["data-sku"] > null)
                        select x)
                    {
                        list.Add(node.Descendants("a").First<HtmlNode>().Attributes["href"].Value.Replace("&amp;", "&"));
                    }
                    foreach (string str9 in list)
                    {
                        this._srr = this._client.Get(str9).Text();
                        this._currentDoc.LoadHtml(this._srr);
                        if (this._currentDoc.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sizes")))
                        {
                            string str10 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-color"))).InnerText.Trim();
                            foreach (HtmlNode node2 in this._currentDoc.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sizes"))).Descendants("option"))
                            {
                                if (!string.IsNullOrEmpty(node2.Attributes["value"].Value))
                                {
                                    PumaProduct item = new PumaProduct {
                                        Size = node2.InnerText.Replace("EU", "").Replace("UK", "").Replace("US", "").Trim(),
                                        A = node2.Attributes["value"].Value.Replace("&amp;", "&"),
                                        Color = str10
                                    };
                                    this._runner.Product.AvailablePumaSizes.Add(item);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string str11 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-color"))).InnerText.Trim();
                    foreach (HtmlNode node3 in this._currentDoc.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sizes"))).Descendants("option"))
                    {
                        if (!string.IsNullOrEmpty(node3.Attributes["value"].Value))
                        {
                            PumaProduct item = new PumaProduct {
                                Size = node3.InnerText.Replace("EU", "").Replace("UK", "").Replace("US", "").Trim(),
                                A = node3.Attributes["value"].Value.Replace("&amp;", "&"),
                                Color = str11
                            };
                            this._runner.Product.AvailablePumaSizes.Add(item);
                        }
                    }
                }
                if (this._runner.Product.AvailablePumaSizes.Count == 0)
                {
                    goto Label_0F0E;
                }
                if (this._task.IsNegativeSizing)
                {
                    this._runner.Product.AvailablePumaSizes = EveAIO.Helpers.ProcessNegativeSizing(this._runner.Product.AvailablePumaSizes, this._task);
                    if (this._runner.Product.AvailablePumaSizes.Count == 0)
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        return false;
                    }
                }
                if (this._task.PriceCheck)
                {
                    string str12 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str12 = str12 + ch.ToString();
                        }
                    }
                    double num3 = double.Parse(str12.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num3 < this._task.MinimumPrice) || (num3 > this._task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
                if (this._task.RandomSize)
                {
                    goto Label_0CE3;
                }
                char[] separator = new char[] { '#' };
                string[] strArray2 = this._task.Size.Split(separator);
                for (int i = 0; i < strArray2.Length; i++)
                {
                    strArray2[i] = strArray2[i].Trim().ToUpperInvariant();
                }
                string[] strArray3 = strArray2;
                int index = 0;
            Label_0B09:
                if (index >= strArray3.Length)
                {
                    goto Label_0CBB;
                }
                string str14 = strArray3[index];
                if (this._runner.PickedSize.HasValue)
                {
                    goto Label_0CBB;
                }
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    using (enumerator4 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct current;
                        while (enumerator4.MoveNext())
                        {
                            current = enumerator4.Current;
                            if (current.Size == str14)
                            {
                                goto Label_0B80;
                            }
                        }
                        goto Label_0CB0;
                    Label_0B80:
                        this._runner.PickedPumaSize = current;
                        goto Label_0CB0;
                    }
                }
                if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                {
                    using (enumerator4 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct current;
                        while (enumerator4.MoveNext())
                        {
                            current = enumerator4.Current;
                            if ((current.Size == str14) && current.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant()))
                            {
                                goto Label_0C12;
                            }
                        }
                        goto Label_0CB0;
                    Label_0C12:
                        this._runner.PickedPumaSize = current;
                        goto Label_0CB0;
                    }
                }
                using (enumerator4 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                {
                    PumaProduct product5;
                    goto Label_0C88;
                Label_0C4C:
                    product5 = enumerator4.Current;
                    if ((product5.Size == str14) && (product5.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()))
                    {
                        goto Label_0C93;
                    }
                Label_0C88:
                    if (!enumerator4.MoveNext())
                    {
                        goto Label_0CB0;
                    }
                    goto Label_0C4C;
                Label_0C93:
                    this._runner.PickedPumaSize = product5;
                }
            Label_0CB0:
                index++;
                goto Label_0B09;
            Label_0CBB:
                if (this._task.SizePickRandom || (this._runner.PickedPumaSize != null))
                {
                    goto Label_0E5F;
                }
                return false;
            Label_0CE3:
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
                else if (this._task.SupremeColorPick != TaskObject.SuprimeColorPickEnum.contains)
                {
                    if (!this._runner.Product.AvailablePumaSizes.Any<PumaProduct>(x => (x.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant())))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                    List<PumaProduct> list2 = (from x in this._runner.Product.AvailablePumaSizes
                        where x.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()
                        select x).ToList<PumaProduct>();
                    this._runner.PickedPumaSize = list2[this._runner.Rnd.Next(0, list2.Count)];
                }
                else
                {
                    if (!this._runner.Product.AvailablePumaSizes.Any<PumaProduct>(x => x.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant())))
                    {
                        goto Label_0EEF;
                    }
                    List<PumaProduct> list3 = (from x in this._runner.Product.AvailablePumaSizes
                        where x.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant())
                        select x).ToList<PumaProduct>();
                    this._runner.PickedPumaSize = list3[this._runner.Rnd.Next(0, list3.Count)];
                }
            Label_0E5F:
                if (this._runner.PickedPumaSize == null)
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
                this._runner.PickedSize = new KeyValuePair<string, string>(this._runner.PickedPumaSize.Size, this._runner.PickedPumaSize.Id);
                return true;
            Label_0EEF:
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                return false;
            Label_0F0E:
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                }
                flag2 = false;
            }
            catch (ThreadAbortException)
            {
                flag2 = false;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
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
                flag2 = false;
            }
            finally
            {
                this._isStyleSearch = false;
            }
            return flag2;
        }

        internal void GetSensorData()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.GETTING_COOKIES, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.GETTING_COOKIES);
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    while (string.IsNullOrEmpty(this._async))
                    {
                    }
                    string str = Global.SENSOR.GetData(this._runner.ProductPageHtml.DocumentNode.InnerHtml, "https://www.converse.com", this._async, false);
                    try
                    {
                        this.SetClient();
                        SensorJson json1 = new SensorJson {
                            sensor_data = str
                        };
                        HttpResponseMessage response = this._client.PostJson("https://www.converse.com/_bm/_data", Newtonsoft.Json.JsonConvert.SerializeObject(json1));
                        Extensions.CheckError(response);
                        this._srr = response.Content.ReadAsStringAsync().Result.ToString();
                        if (this._srr.Contains("Error"))
                        {
                            flag = true;
                        }
                        else
                        {
                            this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                            if (<>o__13.<>p__3 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Converse), argumentInfo));
                            }
                            if (<>o__13.<>p__2 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Converse), argumentInfo));
                            }
                            if (<>o__13.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Converse), argumentInfo));
                            }
                            if (<>o__13.<>p__0 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Converse), argumentInfo));
                            }
                            if (<>o__13.<>p__3.Target(<>o__13.<>p__3, <>o__13.<>p__2.Target(<>o__13.<>p__2, <>o__13.<>p__1.Target(<>o__13.<>p__1, <>o__13.<>p__0.Target(<>o__13.<>p__0, this._dynObj, "success")), true)))
                            {
                                flag = true;
                            }
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
                flag = true;
            Label_0250:
                if (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get(this._runner.Product.Link).Text();
                        goto Label_0250;
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
                        goto Label_0250;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_GETTING_COOKIES, exception2, "", "");
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
            throw new NotSupportedException();
        }

        public bool Search()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                foreach (string str2 in this._task.Keywords)
                {
                    string link = this._task.Link;
                    if (link[link.Length - 1] != '/')
                    {
                        link = link + "/";
                    }
                    string url = $"{link}search?q={str2.Replace(" ", "+").ToLowerInvariant()}";
                    KeyValuePair<string, string> pair = this._client.Get(url).TextResponseUri();
                    this._srr = pair.Key;
                    string str4 = pair.Value;
                    if (str4.ToLowerInvariant().Contains("search"))
                    {
                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(this._srr);
                        if (!document.DocumentNode.Descendants("ul").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"))))
                        {
                            continue;
                        }
                        using (IEnumerator<HtmlNode> enumerator2 = document.DocumentNode.Descendants("ul").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"))).Descendants("li").GetEnumerator())
                        {
                            HtmlNode node;
                            goto Label_01AD;
                        Label_0175:
                            node = enumerator2.Current;
                            if (this.DirectLink(node.Descendants("a").First<HtmlNode>().Attributes["href"].Value))
                            {
                                return true;
                            }
                        Label_01AD:
                            if (!enumerator2.MoveNext())
                            {
                                continue;
                            }
                            goto Label_0175;
                        }
                    }
                    this._isStyleSearch = true;
                    if (this.DirectLink(str4))
                    {
                        return true;
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
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
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
            this._client.SetMobileAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.converse.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.converse.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"))).Attributes["action"].Value;
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_billing_securekey"))).Attributes["value"].Value;
                ProfileObject profile = this._runner.Profile;
                string cardTypeId = this._runner.Profile.CardTypeId;
                string str4 = "";
                if (cardTypeId == "0")
                {
                    str4 = "Amex";
                }
                else if (cardTypeId != "1")
                {
                    if (cardTypeId != "2")
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.INVALID_CREDIT_CARD);
                        States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_CREDIT_CARD, null, "", "");
                        return false;
                    }
                    str4 = "Master";
                }
                else
                {
                    str4 = "Visa";
                }
                bool flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_billing_securekey", str3);
                        this._diData.Add("dwfrm_billing_paymentMethods_selectedPaymentMethodID", "CREDIT_CARD");
                        this._diData.Add("dwfrm_billing_paymentMethods_regionalpaymentfields_idealBankName", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_type", str4);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_number", profile.CCNumber);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_month", int.Parse(profile.ExpiryMonth).ToString());
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_year", profile.ExpiryYear);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_cvn", profile.Cvv);
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_year", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_month", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_day", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_ssn", "");
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_common_firstName", profile.FirstName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_common_lastName", profile.LastName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_common_address1", profile.Address1);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_common_address2", profile.Address2);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_common_city", profile.City);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_regional_zip", profile.Zip);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_regional_phoneOpt", profile.Phone);
                        this._diData.Add("dwfrm_billing_billingAddress_email_emailAddress", profile.Email);
                        this._diData.Add("dwfrm_billing_billingAddress_email_confirmationEmailAddress", profile.Email);
                        this._diData.Add("dwfrm_billing_save", "Next Step: Order Preview");
                        this._srr = this._client.Post(url, this._diData).Text();
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
                if (!this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "form-column-right place-order"))))
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
            catch (Exception exception2)
            {
                this._runner.IsError = true;
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
                return false;
            }
        }

        private bool SubmitOrder()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("submit", "Order & Pay");
                    this._diData.Add("termsCheckbox", "on");
                    if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                    {
                        goto Label_026E;
                    }
                    if (this._task.CheckoutDelay > 0)
                    {
                        Thread.Sleep(this._task.CheckoutDelay);
                    }
                    if (Global.SERIAL == "EVE-1111111111111")
                    {
                        goto Label_029F;
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
                        this._srr = this._client.Post($"{this._homeLink}/checkout-confirmation", this._diData).Text();
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
                if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-message padded")) && !string.IsNullOrEmpty(x.InnerText)))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                try
                {
                    EveAIO.Helpers.AddDbValue("Converse|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                return true;
            Label_026E:
                this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                return false;
            Label_029F:
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
                return false;
            }
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
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri($"{this._homeLink}/cart");
                        this._srr = this._client.Get($"{this._homeLink}/checkout-shipping").Text();
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
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_singleshipping_shippingAddress"))).Attributes["action"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_singleshipping_securekey"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri($"{this._homeLink}/checkout-shipping");
                        string str3 = ((($"{this._demandwareLink}/COShipping-GetApplicableShippingMethodsJSON" + "?postalCode=" + WebUtility.UrlEncode(profile.ZipShipping)) + "&city=" + WebUtility.UrlEncode(profile.CityShipping)) + "&address1=" + WebUtility.UrlEncode(profile.Address1Shipping)) + "&address2=" + WebUtility.UrlEncode(profile.Address2Shipping);
                        this._dynObj = this._client.Get(str3).Json();
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
                if (<>o__18.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Converse), argumentInfo));
                }
                if (<>o__18.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__18.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Converse), argumentInfo));
                }
                if (<>o__18.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "shippingMethod", typeof(Converse), argumentInfo));
                }
                if (<>o__18.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Converse), argumentInfo));
                }
                if (<>o__18.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "regular", typeof(Converse), argumentInfo));
                }
                if (<>o__18.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__18.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "methods", typeof(Converse), argumentInfo));
                }
                object obj3 = <>o__18.<>p__5.Target(<>o__18.<>p__5, <>o__18.<>p__4.Target(<>o__18.<>p__4, <>o__18.<>p__3.Target(<>o__18.<>p__3, <>o__18.<>p__2.Target(<>o__18.<>p__2, <>o__18.<>p__1.Target(<>o__18.<>p__1, <>o__18.<>p__0.Target(<>o__18.<>p__0, this._dynObj)))), "id"));
                this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_common_firstName", profile.FirstNameShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_common_lastName", profile.LastNameShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_common_address1", profile.Address1Shipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_common_address2", profile.Address2Shipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_common_city", profile.CityShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_regional_zip", profile.ZipShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_regional_phoneOpt", profile.PhoneShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_giftmessageFields_giftMessage", "");
                        if (<>o__18.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__18.<>p__6 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Converse), argumentInfo));
                        }
                        <>o__18.<>p__6.Target(<>o__18.<>p__6, this._diData, "regular-shipping-methods", obj3);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_save", "Next Step: Billing Details");
                        this._diData.Add("dwfrm_singleshipping_securekey", str2);
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
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception4)
            {
                this._runner.IsError = true;
                if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
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
            public static readonly Converse.<>c <>9;
            public static Func<HtmlNode, bool> <>9__14_2;
            public static Func<HtmlNode, bool> <>9__14_1;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__17_0;
            public static Func<HtmlNode, bool> <>9__17_1;
            public static Func<HtmlNode, bool> <>9__17_2;
            public static Func<HtmlNode, bool> <>9__18_0;
            public static Func<HtmlNode, bool> <>9__18_1;
            public static Func<HtmlNode, bool> <>9__19_1;
            public static Func<HtmlNode, bool> <>9__19_2;
            public static Func<HtmlNode, bool> <>9__19_3;
            public static Func<HtmlNode, bool> <>9__19_4;
            public static Func<HtmlNode, bool> <>9__19_7;
            public static Func<HtmlNode, bool> <>9__19_8;
            public static Func<HtmlNode, bool> <>9__19_9;
            public static Func<HtmlNode, bool> <>9__19_10;
            public static Func<HtmlNode, bool> <>9__19_11;
            public static Func<HtmlNode, bool> <>9__19_12;
            public static Func<HtmlNode, bool> <>9__19_13;
            public static Func<HtmlNode, bool> <>9__21_0;
            public static Func<HtmlNode, bool> <>9__21_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Converse.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__14_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-image"));

            internal bool <Atc>b__14_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "productId"));

            internal bool <DirectLink>b__19_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_login"));

            internal bool <DirectLink>b__19_10(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sizes"));

            internal bool <DirectLink>b__19_11(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-color"));

            internal bool <DirectLink>b__19_12(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sizes"));

            internal bool <DirectLink>b__19_13(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-color"));

            internal bool <DirectLink>b__19_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name single-line"));

            internal bool <DirectLink>b__19_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__19_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLink>b__19_7(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "color-variations-wrapper"));

            internal bool <DirectLink>b__19_8(HtmlNode x) => 
                (((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("swatch ")) && (x.Attributes["data-sku"] > null));

            internal bool <DirectLink>b__19_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sizes"));

            internal bool <Search>b__21_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"));

            internal bool <Search>b__21_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"));

            internal bool <SubmitBilling>b__17_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"));

            internal bool <SubmitBilling>b__17_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_billing_securekey"));

            internal bool <SubmitBilling>b__17_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "form-column-right place-order"));

            internal bool <SubmitOrder>b__16_1(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-message padded")) && !string.IsNullOrEmpty(x.InnerText));

            internal bool <SubmitShipping>b__18_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_singleshipping_shippingAddress"));

            internal bool <SubmitShipping>b__18_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_singleshipping_securekey"));
        }

        [CompilerGenerated]
        private static class <>o__13
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__18
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__6;
        }
    }
}

