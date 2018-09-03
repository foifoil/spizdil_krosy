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

    internal class Finishline : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;
        private object _orderId;
        private object _rsaKey;
        private object _shippingMethod;
        private object _packageSku;
        private object _purchaseId;
        private string _dynSessConf;
        private string _model;
        private string _async;

        public Finishline(TaskRunner runner, TaskObject task)
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
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedPumaSize.Size, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Post("https://www.finishline.com/store/global/headerMyAccount.jsp", this._diData).Text();
                        continue;
                    }
                    catch (WebException)
                    {
                        continue;
                    }
                }
                flag = true;
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("_dyncharset", "UTF-8");
                    this._diData.Add("requiresSessionConfirmation", "false");
                    this._diData.Add("/atg/commerce/order/purchase/CartModifierFormHandler.addItemToOrderSuccessURL", $"/store/browse/productDetail.jsp?productId={this._model}");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CartModifierFormHandler.addItemToOrderSuccessURL", "");
                    this._diData.Add("/atg/commerce/order/purchase/CartModifierFormHandler.addItemToOrderErrorURL", $"/store/browse/productDetail.jsp?productId={this._model}&dontCachePDP=true");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CartModifierFormHandler.addItemToOrderErrorURL", "");
                    this._diData.Add("catalogRefIds", this._runner.PickedPumaSize.Id);
                    this._diData.Add("_D:catalogRefIds", "");
                    this._diData.Add("productId", this._model);
                    this._diData.Add("_D:productId", "");
                    this._diData.Add("items", "");
                    this._diData.Add("_D:items", "");
                    this._diData.Add("quantity", "1");
                    this._diData.Add("_D:quantity", "");
                    this._diData.Add("/atg/commerce/order/purchase/CartModifierFormHandler.dimensionId", "");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CartModifierFormHandler.dimensionId", "");
                    this._diData.Add("Add To Cart", "Add To Cart");
                    this._diData.Add("_D:Add To Cart", "");
                    this._diData.Add("_DARGS", "/store/browse/productDetailDisplay.jsp.flAddToCart");
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Post($"https://www.finishline.com/store/browse/productDetail.jsp?productId={this._model}&_DARGS=/store/browse/productDetailDisplay.jsp.flAddToCart", this._diData).Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if ((!exception.Message.Contains("504") && !exception.Message.Contains("503")) && !exception.Message.Contains("404"))
                        {
                            throw;
                        }
                        flag = true;
                        if (!exception.Message.Contains("404"))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        continue;
                    }
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.finishline.com/store/cart/gadgets/cartCount.jsp").Text();
                        continue;
                    }
                    catch (WebException exception2)
                    {
                        if ((!exception2.Message.Contains("504") && !exception2.Message.Contains("503")) && !exception2.Message.Contains("404"))
                        {
                            throw;
                        }
                        flag = true;
                        if (!exception2.Message.Contains("404"))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                        }
                        continue;
                    }
                }
                if (this._srr.Trim() == "1")
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
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception3 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_ATC);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception3.Message.Contains("404") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("404")))
                {
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
            switch (((-691835000 ^ -696750335) % 5))
            {
                case 0:
                    goto Label_001C;

                case 1:
                    break;

                case 2:
                    return false;

                case 4:
                    return false;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (!this.SubmitBilling())
            {
            }
            if (-1454877356 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            try
            {
                List<PumaProduct>.Enumerator enumerator3;
                if (this._runner.Profile.CountryIdShipping != "US")
                {
                    this._task.Status = States.GetTaskState(States.TaskState.US_SHIPPING_NEEDED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.US_SHIPPING_NEEDED, null, "", "");
                    return false;
                }
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", link);
                }
                bool flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.finishline.com").Text();
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
                try
                {
                    this._diData.Clear();
                    this._srr = this._client.Post("https://www.finishline.com/store/global/headerMyAccount.jsp", this._diData).Text();
                }
                catch (WebException)
                {
                }
                bool flag3 = false;
                flag2 = true;
            Label_0150:
                if (!flag2)
                {
                    goto Label_0225;
                }
                flag2 = false;
                try
                {
                    this._srr = this._client.Get(link).Text();
                    goto Label_0208;
                }
                catch (WebException exception2)
                {
                    if (!exception2.Message.Contains("504") && !exception2.Message.Contains("503"))
                    {
                        throw;
                    }
                    flag2 = true;
                    States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                    Thread.Sleep(0x3e8);
                    goto Label_0208;
                }
            Label_01C7:
                if (!flag3)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_IN_QUEUE, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                    flag3 = true;
                }
                Thread.Sleep(0x1388);
                flag2 = true;
                goto Label_0150;
            Label_0208:
                if (!this._srr.ToUpperInvariant().Contains("WAIT TO GET THEM"))
                {
                    goto Label_0150;
                }
                goto Label_01C7;
            Label_0225:
                this._currentDoc.LoadHtml(this._srr);
                this._dynSessConf = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_dynSessConf"))).Attributes["value"].Value;
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "bVProductName"))).Attributes["value"].Value;
                string innerText = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "fullPrice"))).InnerText;
                this._task.ImgUrl = "http://eve-robotics.com/dummy_product.png";
                Product product1 = new Product {
                    ProductTitle = str,
                    Link = link,
                    Price = innerText
                };
                this._runner.Product = product1;
                List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>();
                foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("div")
                    where (x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "offers")
                    select x)
                {
                    if (node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "availability"))).InnerText == "InStock")
                    {
                        string str4 = node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"))).InnerText;
                        string str5 = node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "sku"))).InnerText;
                        string str3 = node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "color"))).InnerText;
                        list.Add(new Tuple<string, string, string>(str4, str5, str3.ToLowerInvariant()));
                    }
                }
                this._model = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productItemId"))).Attributes["value"].Value;
                List<Tuple<string, string>> source = new List<Tuple<string, string>>();
                foreach (HtmlNode local1 in from x in this._currentDoc.DocumentNode.Descendants("a")
                    where (x.Attributes["data-size"] != null) && (x.Attributes["data-sku"] > null)
                    select x)
                {
                    string str7 = local1.Attributes["data-size"].Value;
                    string str6 = local1.Attributes["data-sku"].Value;
                    byte[] bytes = Convert.FromBase64String(str6.Substring(str6.IndexOf("_") + 1));
                    str6 = Encoding.UTF8.GetString(bytes);
                    source.Add(new Tuple<string, string>(str6, str7));
                }
                foreach (Tuple<string, string, string> prod in list)
                {
                    if (source.Any<Tuple<string, string>>(x => x.Item1 == prod.Item2))
                    {
                        Tuple<string, string> tuple = source.First<Tuple<string, string>>(x => x.Item1 == prod.Item2);
                        PumaProduct item = new PumaProduct {
                            Color = prod.Item3,
                            Size = tuple.Item2,
                            Id = prod.Item2
                        };
                        this._runner.Product.AvailablePumaSizes.Add(item);
                    }
                }
                if (this._runner.Product.AvailablePumaSizes.Count == 0)
                {
                    goto Label_0CBB;
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
                    string str9 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str9 = str9 + ch.ToString();
                        }
                    }
                    double num3 = double.Parse(str9.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num3 < this._task.MinimumPrice) || (num3 > this._task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
                if (this._task.RandomSize)
                {
                    goto Label_0A93;
                }
                char[] separator = new char[] { '#' };
                string[] strArray = this._task.Size.Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Trim().ToUpperInvariant();
                }
                string[] strArray2 = strArray;
                int index = 0;
            Label_08BF:
                if (index >= strArray2.Length)
                {
                    goto Label_0A6C;
                }
                string str10 = strArray2[index];
                if (this._runner.PickedPumaSize != null)
                {
                    goto Label_0A6C;
                }
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct current;
                        while (enumerator3.MoveNext())
                        {
                            current = enumerator3.Current;
                            if (current.Size == str10)
                            {
                                goto Label_0931;
                            }
                        }
                        goto Label_0A61;
                    Label_0931:
                        this._runner.PickedPumaSize = current;
                        goto Label_0A61;
                    }
                }
                if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                {
                    using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct current;
                        while (enumerator3.MoveNext())
                        {
                            current = enumerator3.Current;
                            if ((current.Size == str10) && current.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant()))
                            {
                                goto Label_09C3;
                            }
                        }
                        goto Label_0A61;
                    Label_09C3:
                        this._runner.PickedPumaSize = current;
                        goto Label_0A61;
                    }
                }
                using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                {
                    PumaProduct product4;
                    goto Label_0A39;
                Label_09FD:
                    product4 = enumerator3.Current;
                    if ((product4.Size == str10) && (product4.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()))
                    {
                        goto Label_0A44;
                    }
                Label_0A39:
                    if (!enumerator3.MoveNext())
                    {
                        goto Label_0A61;
                    }
                    goto Label_09FD;
                Label_0A44:
                    this._runner.PickedPumaSize = product4;
                }
            Label_0A61:
                index++;
                goto Label_08BF;
            Label_0A6C:
                if (this._task.SizePickRandom || (this._runner.PickedPumaSize != null))
                {
                    goto Label_0C2C;
                }
                return false;
            Label_0A93:
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
                else if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                {
                    if (!this._runner.Product.AvailablePumaSizes.Any<PumaProduct>(x => x.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant())))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                    List<PumaProduct> list3 = (from x in this._runner.Product.AvailablePumaSizes
                        where x.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant())
                        select x).ToList<PumaProduct>();
                    this._runner.PickedPumaSize = list3[this._runner.Rnd.Next(0, list3.Count)];
                }
                else
                {
                    if (!this._runner.Product.AvailablePumaSizes.Any<PumaProduct>(x => (x.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant())))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                    List<PumaProduct> list4 = (from x in this._runner.Product.AvailablePumaSizes
                        where x.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()
                        select x).ToList<PumaProduct>();
                    this._runner.PickedPumaSize = list4[this._runner.Rnd.Next(0, list4.Count)];
                }
            Label_0C2C:
                if (this._runner.PickedPumaSize == null)
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
                this._runner.PickedSize = new KeyValuePair<string, string>(this._runner.PickedPumaSize.Size, this._runner.PickedPumaSize.Id);
                return true;
            Label_0CBB:
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
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception3 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception3, "", "Web request timed out");
                }
                else if (!exception3.Message.Contains("404") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("404")))
                {
                    if (!exception3.Message.Contains("430") && ((exception3.InnerException == null) || !exception3.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_STOCKCHECK);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception3, "", "");
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
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.GETTING_COOKIES, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.GETTING_COOKIES);
                bool flag = true;
            Label_0032:
                if (!flag)
                {
                    return;
                }
                flag = false;
                do
                {
                }
                while (string.IsNullOrEmpty(this._async));
                string str = Global.SENSOR.GetData(this._runner.ProductPageHtml.DocumentNode.InnerHtml, "https://www.finishline.com", this._async, true);
                try
                {
                    SensorJson json1 = new SensorJson {
                        sensor_data = str
                    };
                    HttpResponseMessage response = this._client.PostJson("https://www.finishline.com/_bm/_data", Newtonsoft.Json.JsonConvert.SerializeObject(json1));
                    Extensions.CheckError(response);
                    this._srr = response.Content.ReadAsStringAsync().Result.ToString();
                    if (this._srr.Contains("Error"))
                    {
                        flag = true;
                    }
                    else
                    {
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(this._srr);
                        if (<>o__25.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Finishline), argumentInfo));
                        }
                        if (<>o__25.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Finishline), argumentInfo));
                        }
                        if (<>o__25.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__25.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Finishline), argumentInfo));
                        }
                        if (<>o__25.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__25.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Finishline), argumentInfo));
                        }
                        if (<>o__25.<>p__3.Target(<>o__25.<>p__3, <>o__25.<>p__2.Target(<>o__25.<>p__2, <>o__25.<>p__1.Target(<>o__25.<>p__1, <>o__25.<>p__0.Target(<>o__25.<>p__0, this._dynObj, "success")), true)))
                        {
                            flag = true;
                        }
                    }
                    goto Label_0032;
                }
                catch (WebException exception1)
                {
                    if (!exception1.Message.Contains("400"))
                    {
                        throw;
                    }
                    flag = true;
                    goto Label_0032;
                }
            }
            catch (ThreadAbortException)
            {
                throw;
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

        public bool Login() => 
            true;

        public bool Search()
        {
            try
            {
                if (this._runner.Profile.CountryIdShipping != "US")
                {
                    this._task.Status = States.GetTaskState(States.TaskState.US_SHIPPING_NEEDED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.US_SHIPPING_NEEDED, null, "", "");
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                foreach (string str in this._task.Keywords)
                {
                    string url = $"https://www.finishline.com/store/_/N-/Ntt-{str.Replace(" ", "%20").ToLowerInvariant()}";
                    KeyValuePair<string, string> pair = this._client.Get(url).TextResponseUri();
                    this._srr = pair.Key;
                    HtmlDocument document1 = new HtmlDocument();
                    document1.LoadHtml(this._srr);
                    foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("div")
                        where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-card")
                        select x)
                    {
                        if (this.DirectLink("https://www.finishline.com" + node.Descendants("a").First<HtmlNode>().Attributes["href"].Value))
                        {
                            return true;
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; rv:2.2) Gecko/20110201");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.finishline.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.finishline.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "no-cache");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Pragma", "no-cache");
            this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                string str = "";
                switch (profile.CardTypeId)
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
                        str = "DISCOVER";
                        break;
                }
                bool flag = true;
                while (true)
                {
                    if (!flag)
                    {
                        break;
                    }
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("_dyncharset", "UTF-8");
                        this._diData.Add("_dynSessConf", this._dynSessConf);
                        this._diData.Add("creditCardNumberTemp", profile.CCNumber);
                        this._diData.Add("_D:creditCardNumberTemp", " ");
                        this._diData.Add("_D:expirationMonthTemp", " ");
                        this._diData.Add("_D:expirationYearTemp", " ");
                        this._diData.Add("securityCodeTemp", profile.Cvv);
                        this._diData.Add("_D:securityCodeTemp", " ");
                        this._diData.Add("firstName", profile.FirstName);
                        this._diData.Add("_D:firstName", " ");
                        this._diData.Add("lastName", profile.LastName);
                        this._diData.Add("_D:lastName", " ");
                        this._diData.Add("address1", profile.Address1);
                        this._diData.Add("_D:address1", " ");
                        this._diData.Add("address2", profile.Address2);
                        this._diData.Add("_D:address2", " ");
                        this._diData.Add("city", profile.City);
                        this._diData.Add("_D:city", " ");
                        this._diData.Add("_D:state", " ");
                        this._diData.Add("zip", profile.Zip);
                        this._diData.Add("_D:zip", " ");
                        this._diData.Add("phone", profile.Phone);
                        this._diData.Add("_D:phone", " ");
                        this._diData.Add("email", profile.Email);
                        this._diData.Add("_D:email", " ");
                        this._diData.Add("country", "US");
                        this._diData.Add("_D:country", " ");
                        this._diData.Add("creditCardTypeTemp", str);
                        this._diData.Add("_D:creditCardTypeTemp", " ");
                        this._diData.Add("transactionType", "CC");
                        this._diData.Add("/atg/store/order/purchase/BillingFormHandler.creditCardNickname", "fline");
                        this._diData.Add("_D:/atg/store/order/purchase/BillingFormHandler.creditCardNickname", " ");
                        this._diData.Add("_D:checkoutWithPayPal", " ");
                        this._diData.Add("wcCustomerId", "");
                        this._diData.Add("/atg/store/order/purchase/BillingFormHandler.formGCStatishFied", "false");
                        this._diData.Add("_D:/atg/store/order/purchase/BillingFormHandler.formGCStatishFied", " ");
                        this._diData.Add("/atg/store/order/purchase/BillingFormHandler.overrideBillingAddress", "true");
                        this._diData.Add("_D:/atg/store/order/purchase/BillingFormHandler.overrideBillingAddress", " ");
                        this._diData.Add("/atg/store/order/purchase/BillingFormHandler.sessionExpirationURL", "/store/cart/cart.jsp?timeouterror=true");
                        this._diData.Add("_D:/atg/store/order/purchase/BillingFormHandler.sessionExpirationURL", " ");
                        this._diData.Add("moveToConfirmSuccessURL", "/store/checkout/review.jsp");
                        this._diData.Add("_D:moveToConfirmSuccessURL", " ");
                        this._diData.Add("moveToConfirmErrorURL", "/store/checkout/billing.jsp");
                        this._diData.Add("_D:moveToConfirmErrorURL", " ");
                        this._diData.Add("/atg/store/order/purchase/BillingFormHandler.billingWithNewAddressAndNewCard", "submit");
                        this._diData.Add("_D:/atg/store/order/purchase/BillingFormHandler.billingWithNewAddressAndNewCard", " ");
                        this._diData.Add("_DARGS", "/store/checkout/billing.jsp");
                        this._diData.Add("expirationMonthTemp", profile.ExpiryMonth);
                        this._diData.Add("expirationYearTemp", profile.ExpiryYear);
                        this._diData.Add("state", profile.StateId);
                        this._diData.Add("creditCardType", str);
                        this._srr = this._client.Post("https://www.finishline.com/store/checkout/billing.jsp?_DARGS=/store/checkout/billing.jsp", this._diData).Text();
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
                }
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception2)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("404") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("404")))
                {
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
            bool flag2;
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("_dyncharset", "UTF-8");
                    this._diData.Add("_dynSessConf", this._dynSessConf);
                    this._diData.Add("/atg/commerce/order/purchase/CommitOrderFormHandler.mobileOrder", "");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CommitOrderFormHandler.mobileOrder", " ");
                    this._diData.Add("bb_field", "");
                    this._diData.Add("_D:bb_field", " ");
                    this._diData.Add("/atg/commerce/order/purchase/CommitOrderFormHandler.sessionExpirationURL", "/store/cart/cart.jsp?timeouterror=true");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CommitOrderFormHandler.sessionExpirationURL", " ");
                    this._diData.Add("/atg/commerce/order/purchase/CommitOrderFormHandler.commitOrderSuccessURL", "/store/checkout/confirm.jsp");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CommitOrderFormHandler.commitOrderSuccessURL", " ");
                    this._diData.Add("/atg/commerce/order/purchase/CommitOrderFormHandler.commitOrderErrorURL", "/store/checkout/billing.jsp");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CommitOrderFormHandler.commitOrderErrorURL", " ");
                    this._diData.Add("maxOrderDecline", "maxOrderDecline");
                    this._diData.Add("_D:maxOrderDecline", " ");
                    this._diData.Add("/atg/commerce/order/purchase/CommitOrderFormHandler.commitOrder", "Process Order");
                    this._diData.Add("_D:/atg/commerce/order/purchase/CommitOrderFormHandler.commitOrder", " ");
                    this._diData.Add("_DARGS", "/store/checkout/review.jsp");
                    if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                    {
                        goto Label_03C4;
                    }
                    if (this._task.CheckoutDelay > 0)
                    {
                        Thread.Sleep(this._task.CheckoutDelay);
                    }
                    if (Global.SERIAL == "EVE-1111111111111")
                    {
                        goto Label_03F5;
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
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.finishline.com/store/checkout/review.jsp");
                        this._srr = this._client.Post("https://www.finishline.com/store/checkout/review.jsp?_DARGS=/store/checkout/review.jsp", this._diData).Text();
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
                if (!this._srr.Contains("There Was an Issue Processing Your Order") && !this._srr.Contains("The credit card number you have entered is invalid"))
                {
                    try
                    {
                        EveAIO.Helpers.AddDbValue("Finishline|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    }
                    catch
                    {
                    }
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            Label_03C4:
                this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                return false;
            Label_03F5:
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                flag2 = false;
            }
            catch (ThreadAbortException)
            {
                flag2 = false;
            }
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (exception2 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, null, "", "Web request timed out");
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
                flag2 = false;
            }
            finally
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
            }
            return flag2;
        }

        private bool SubmitShipping()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                this._client.Session.DefaultRequestHeaders.Remove("Origin");
                ProfileObject profile = this._runner.Profile;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.finishline.com/store/cart/cartSlide.jsp?stage=pdp").Text();
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
                this._dynSessConf = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_dynSessConf"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.finishline.com/store/cart/cart.jsp").Text();
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
                Cookie cookie = new Cookie {
                    Value = "A",
                    Name = "AKA_A2",
                    Domain = ".finishline.com"
                };
                this._client.Handler.CookieContainer.Add(cookie);
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("_dyncharset", "UTF-8");
                        this._diData.Add("_dynSessConf", this._dynSessConf);
                        this._diData.Add(this._runner.PickedPumaSize.Id, "1");
                        this._diData.Add("/atg/commerce/order/purchase/CartModifierFormHandler.shippingInfoURL", "/store/checkout/shipping.jsp");
                        this._diData.Add("_D:/atg/commerce/order/purchase/CartModifierFormHandler.shippingInfoURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/CartModifierFormHandler.moveToPurchaseInfoErrorURL", "/store/cart/cart.jsp");
                        this._diData.Add("_D:/atg/commerce/order/purchase/CartModifierFormHandler.moveToPurchaseInfoErrorURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/CartModifierFormHandler.loginDuringCheckoutURL", "/store/checkout/login.jsp");
                        this._diData.Add("_D:/atg/commerce/order/purchase/CartModifierFormHandler.loginDuringCheckoutURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/CartModifierFormHandler.moveToPurchaseInfo", "Submit Query");
                        this._diData.Add("_D:/atg/commerce/order/purchase/CartModifierFormHandler.moveToPurchaseInfo", " ");
                        this._diData.Add("PayPal", "");
                        this._diData.Add("_D:PayPal", " ");
                        this._diData.Add("_DARGS", "/store/cart/cartInner.jsp.cartform");
                        this._client.Session.DefaultRequestHeaders.Remove("Referer");
                        this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
                        this._srr = this._client.Post("https://www.finishline.com/store/cart/cart.jsp?_DARGS=/store/cart/cartInner.jsp.cartform", this._diData).Text();
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
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("_dyncharset", "UTF-8");
                        this._diData.Add("_dynSessConf", this._dynSessConf);
                        this._diData.Add("firstName", profile.FirstNameShipping);
                        this._diData.Add("_D:firstName", " ");
                        this._diData.Add("lastName", profile.LastNameShipping);
                        this._diData.Add("_D:lastName", " ");
                        this._diData.Add("address1", profile.Address1Shipping);
                        this._diData.Add("_D:address1", " ");
                        this._diData.Add("address2", profile.Address2Shipping);
                        this._diData.Add("_D:address2", " ");
                        this._diData.Add("city", profile.CityShipping);
                        this._diData.Add("_D:city", " ");
                        this._diData.Add("_D:state", " ");
                        this._diData.Add("zip", profile.ZipShipping);
                        this._diData.Add("_D:zip", " ");
                        this._diData.Add("phone", profile.PhoneShipping);
                        this._diData.Add("_D:phone", " ");
                        this._diData.Add("email", profile.EmailShipping);
                        this._diData.Add("_D:email", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.validateBadChar", "true");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.validateBadChar", " ");
                        this._diData.Add("country", "US");
                        this._diData.Add("_D:country", " ");
                        this._diData.Add("shippingMethod", "Economy");
                        this._diData.Add("_D:shippingMethod", " ");
                        this._diData.Add("receiveEmail", "false");
                        this._diData.Add("_D:receiveEmail", " ");
                        this._diData.Add("shipToAddressName", "");
                        this._diData.Add("_D:shipToAddressName", " ");
                        this._diData.Add("copyNewAddrToBilling", "false");
                        this._diData.Add("_D:copyNewAddrToBilling", " ");
                        this._diData.Add("upsValidation", "true");
                        this._diData.Add("_D:upsValidation", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.sessionExpirationURL", "/store/cart/cart.jsp?timeouterror=true");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.sessionExpirationURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsSuccessURL", "/store/checkout/billing.jsp");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsSuccessURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsErrorURL", "/store/checkout/shipping.jsp");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsErrorURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroups", "moveToBilling");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroups", " ");
                        this._diData.Add("_DARGS", "/store/checkout/shipping.jsp");
                        this._diData.Add("state", profile.StateIdShipping);
                        this._diData.Add("instoreShipLastName", profile.LastNameShipping);
                        this._diData.Add("Continue", "Continue");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.finishline.com/store/checkout/login.jsp");
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.finishline.com");
                        this._srr = this._client.Post("https://www.finishline.com/store/checkout/shipping.jsp?_DARGS=/store/checkout/shipping.jsp", this._diData).Text();
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
                        this._diData.Clear();
                        this._diData.Add("_dyncharset", "UTF-8");
                        this._diData.Add("_dynSessConf", this._dynSessConf);
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.firstName", profile.FirstNameShipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.firstName", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.lastName", profile.LastNameShipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.lastName", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.usedUpsSuggestion", "true");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.usedUpsSuggestion", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.address1", profile.Address1Shipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.address1", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.address2", profile.Address2Shipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.address2", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.city", profile.CityShipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.city", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.state", profile.StateIdShipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.state", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.postalCode", profile.ZipShipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.postalCode", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.phoneNumber", profile.PhoneShipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.phoneNumber", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.email", profile.EmailShipping);
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.email", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.address.country", "US");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.address.country", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.copyNewAddrToBilling", "false");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.copyNewAddrToBilling", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.receiveEmail", "false");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.receiveEmail", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.shippingMethod", "Economy");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.shippingMethod", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.overrideShippingAddress", "true");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.overrideShippingAddress", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsSuccessURL", "/store/checkout/billing.jsp");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsSuccessURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsErrorURL", "/store/checkout/shipping.jsp");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroupsErrorURL", " ");
                        this._diData.Add("/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroups", "Continue with Original");
                        this._diData.Add("_D:/atg/commerce/order/purchase/ShippingGroupFormHandler.applyShippingGroups", " ");
                        this._diData.Add("_DARGS", "/store/checkout/modals/upsAddressValidation.jsp.overrideShippingForm");
                        this._srr = this._client.Post("https://www.finishline.com/store/checkout/shipping.jsp?_DARGS=/store/checkout/modals/upsAddressValidation.jsp.overrideShippingForm", this._diData).Text();
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
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception6)
            {
                this._runner.Cookies = new CookieContainer();
                this.SetClient();
                this._runner.IsError = true;
                if (exception6 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, null, "", "Web request timed out");
                }
                else if (!exception6.Message.Contains("404") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("404")))
                {
                    if (!exception6.Message.Contains("430") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception6, "", "");
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
            public static readonly Finishline.<>c <>9;
            public static Func<HtmlNode, bool> <>9__21_0;
            public static Func<HtmlNode, bool> <>9__22_0;
            public static Func<HtmlNode, bool> <>9__22_1;
            public static Func<HtmlNode, bool> <>9__22_2;
            public static Func<HtmlNode, bool> <>9__22_6;
            public static Func<HtmlNode, bool> <>9__22_7;
            public static Func<HtmlNode, bool> <>9__22_8;
            public static Func<HtmlNode, bool> <>9__22_9;
            public static Func<HtmlNode, bool> <>9__22_10;
            public static Func<HtmlNode, bool> <>9__22_3;
            public static Func<HtmlNode, bool> <>9__22_11;
            public static Func<HtmlNode, bool> <>9__24_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Finishline.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__22_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_dynSessConf"));

            internal bool <DirectLink>b__22_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "bVProductName"));

            internal bool <DirectLink>b__22_10(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "color"));

            internal bool <DirectLink>b__22_11(HtmlNode x) => 
                ((x.Attributes["data-size"] != null) && (x.Attributes["data-sku"] > null));

            internal bool <DirectLink>b__22_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "fullPrice"));

            internal bool <DirectLink>b__22_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productItemId"));

            internal bool <DirectLink>b__22_6(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "offers"));

            internal bool <DirectLink>b__22_7(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "availability"));

            internal bool <DirectLink>b__22_8(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "model"));

            internal bool <DirectLink>b__22_9(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "sku"));

            internal bool <Search>b__24_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-card"));

            internal bool <SubmitShipping>b__21_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_dynSessConf"));
        }

        [CompilerGenerated]
        private static class <>o__25
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }
    }
}

