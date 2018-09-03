namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using EveAIO.Tasks.Dto;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
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

    internal class Puma : IPlatform
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

        public Puma(TaskRunner runner, TaskObject task)
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
                PumaProduct product = this._runner.Product.AvailablePumaSizes.First<PumaProduct>(x => x.Id == this._runner.PickedSize.Value.Value);
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._dynObj = this._client.Get(product.A + "&quantity=1").Json();
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
                    if (<>o__11.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__3 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__11.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "product", typeof(Puma), argumentInfo));
                    }
                    <>o__11.<>p__3.Target(<>o__11.<>p__3, this._diData, "pid", <>o__11.<>p__2.Target(<>o__11.<>p__2, <>o__11.<>p__1.Target(<>o__11.<>p__1, <>o__11.<>p__0.Target(<>o__11.<>p__0, this._dynObj), "id")));
                    this._diData.Add("quantity", "1");
                    try
                    {
                        this._dynObj = this._client.Post($"{this._demandwareLink}/Cart-AddProduct", this._diData).Json();
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
                if (<>o__11.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__11.<>p__12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Puma), argumentInfo));
                }
                if (<>o__11.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__11.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Puma), argumentInfo));
                }
                if (<>o__11.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__11.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                }
                object obj2 = <>o__11.<>p__5.Target(<>o__11.<>p__5, <>o__11.<>p__4.Target(<>o__11.<>p__4, this._dynObj, "adjustQuantityMessage"), null);
                if (<>o__11.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__11.<>p__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Puma), argumentInfo));
                }
                if (!<>o__11.<>p__11.Target(<>o__11.<>p__11, obj2))
                {
                    if (<>o__11.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__10 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__11.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                }
                if (!<>o__11.<>p__12.Target(<>o__11.<>p__12, (<>o__11.<>p__6 != null) ? obj2 : <>o__11.<>p__10.Target(<>o__11.<>p__10, obj2, <>o__11.<>p__9.Target(<>o__11.<>p__9, <>o__11.<>p__8.Target(<>o__11.<>p__8, <>o__11.<>p__7.Target(<>o__11.<>p__7, <>o__11.<>p__6.Target(<>o__11.<>p__6, this._dynObj, "adjustQuantityMessage"))), "Only 0 available. 1 in your bag"))))
                {
                    if (<>o__11.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__11.<>p__15 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                    if (<>o__11.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__11.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                    }
                    if (!<>o__11.<>p__16.Target(<>o__11.<>p__16, <>o__11.<>p__15.Target(<>o__11.<>p__15, <>o__11.<>p__14.Target(<>o__11.<>p__14, <>o__11.<>p__13.Target(<>o__11.<>p__13, this._dynObj, "quantityAdded")), 1)))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.SIZE_OOS, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SIZE_OOS);
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
            switch (((0x4267615d ^ 0x483967d7) % 5))
            {
                case 0:
                    goto Label_001C;

                case 1:
                    break;

                case 3:
                    return false;

                case 4:
                    return false;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (this.SubmitBilling())
            {
            }
            if (0x5e086068 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            try
            {
                List<PumaProduct>.Enumerator enumerator3;
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                bool flag = true;
                string url = link;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get(url).Text();
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
                string str4 = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "country-selector"))).Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                if (str4.Contains("https"))
                {
                    this._demandwareLink = str4.Substring(0, str4.IndexOf("/Page-LocaleSelector"));
                }
                else
                {
                    this._demandwareLink = this._task.HomeUrl + str4.Substring(0, str4.IndexOf("/Page-LocaleSelector"));
                }
                this._homeLink = this._demandwareLink.Substring(0, this._demandwareLink.IndexOf(".com") + 4);
                char[] separator = new char[] { '/' };
                string[] strArray = this._task.Link.Replace(this._homeLink, "").Split(separator);
                string[] textArray1 = new string[] { this._homeLink, "/", strArray[1], "/", strArray[2] };
                this._homeLink = string.Concat(textArray1);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str3 = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "h1 product-name"))).InnerText.Trim();
                string str2 = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "row prices-reviewsummary-row"))).Descendants("span").First<HtmlNode>(x => (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sales")) && (x.Attributes["data-price-value"] > null))).InnerText.Trim());
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value;
                Product product1 = new Product {
                    ProductTitle = str3,
                    Link = link,
                    Price = str2
                };
                this._runner.Product = product1;
                List<string> list = new List<string>();
                foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("a")
                    where (x.Attributes["data-attr-value"] != null) && (x.Attributes["selectable"] > null)
                    select x)
                {
                    list.Add(node.Attributes["href"].Value.Replace("&amp;", "&"));
                }
                foreach (string str5 in list)
                {
                    this._srr = this._client.Get(str5).Text();
                    this._currentDoc.LoadHtml(this._srr);
                    if (this._currentDoc.DocumentNode.Descendants("select").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "attribute-select-size"))) || this._currentDoc.DocumentNode.Descendants("select").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "select-size"))))
                    {
                        HtmlNode node2 = null;
                        if (this._currentDoc.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "attribute-select-size")))
                        {
                            node2 = this._currentDoc.DocumentNode.Descendants("select").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "attribute-select-size"));
                        }
                        if (this._currentDoc.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "select-size")))
                        {
                            node2 = this._currentDoc.DocumentNode.Descendants("select").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "select-size"));
                        }
                        string str6 = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "color"))).InnerText.Trim();
                        foreach (HtmlNode node3 in node2.Descendants("option"))
                        {
                            if (node3.Attributes["data-attr-value"] != null)
                            {
                                PumaProduct item = new PumaProduct {
                                    Size = node3.InnerText.Trim(),
                                    A = node3.Attributes["value"].Value.Replace("&amp;", "&"),
                                    Color = str6
                                };
                                this._runner.Product.AvailablePumaSizes.Add(item);
                            }
                        }
                    }
                }
                if (this._runner.Product.AvailablePumaSizes.Count == 0)
                {
                    goto Label_0CAE;
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
                    string str7 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str7 = str7 + ch.ToString();
                        }
                    }
                    double num3 = double.Parse(str7.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num3 < this._task.MinimumPrice) || (num3 > this._task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
                if (this._task.RandomSize)
                {
                    goto Label_0A83;
                }
                char[] chArray2 = new char[] { '#' };
                string[] strArray2 = this._task.Size.Split(chArray2);
                for (int i = 0; i < strArray2.Length; i++)
                {
                    strArray2[i] = strArray2[i].Trim().ToUpperInvariant();
                }
                string[] strArray3 = strArray2;
                int index = 0;
            Label_08AE:
                if (index >= strArray3.Length)
                {
                    goto Label_0A5B;
                }
                string str9 = strArray3[index];
                if (this._runner.PickedPumaSize != null)
                {
                    goto Label_0A5B;
                }
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct current;
                        while (enumerator3.MoveNext())
                        {
                            current = enumerator3.Current;
                            if (current.Size == str9)
                            {
                                goto Label_0920;
                            }
                        }
                        goto Label_0A50;
                    Label_0920:
                        this._runner.PickedPumaSize = current;
                        goto Label_0A50;
                    }
                }
                if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                {
                    using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct product3;
                        goto Label_09A7;
                    Label_096B:
                        product3 = enumerator3.Current;
                        if ((product3.Size == str9) && product3.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant()))
                        {
                            goto Label_09B2;
                        }
                    Label_09A7:
                        if (!enumerator3.MoveNext())
                        {
                            goto Label_0A50;
                        }
                        goto Label_096B;
                    Label_09B2:
                        this._runner.PickedPumaSize = product3;
                        goto Label_0A50;
                    }
                }
                using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                {
                    PumaProduct product4;
                    goto Label_0A28;
                Label_09EC:
                    product4 = enumerator3.Current;
                    if ((product4.Size == str9) && (product4.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()))
                    {
                        goto Label_0A33;
                    }
                Label_0A28:
                    if (!enumerator3.MoveNext())
                    {
                        goto Label_0A50;
                    }
                    goto Label_09EC;
                Label_0A33:
                    this._runner.PickedPumaSize = product4;
                }
            Label_0A50:
                index++;
                goto Label_08AE;
            Label_0A5B:
                if (this._task.SizePickRandom || (this._runner.PickedPumaSize != null))
                {
                    goto Label_0C1E;
                }
                return false;
            Label_0A83:
                if (!string.IsNullOrEmpty(this._task.Color))
                {
                    if (this._task.SupremeColorPick != TaskObject.SuprimeColorPickEnum.contains)
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
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                            return false;
                        }
                        List<PumaProduct> list3 = (from x in this._runner.Product.AvailablePumaSizes
                            where x.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant())
                            select x).ToList<PumaProduct>();
                        this._runner.PickedPumaSize = list3[this._runner.Rnd.Next(0, list3.Count)];
                    }
                }
                else
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
            Label_0C1E:
                if (this._runner.PickedPumaSize == null)
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
                this._runner.PickedSize = new KeyValuePair<string, string>(this._runner.PickedPumaSize.Size, this._runner.PickedPumaSize.Id);
                return true;
            Label_0CAE:
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
            catch (Exception exception2)
            {
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
            throw new NotSupportedException();
        }

        public bool Search()
        {
            throw new NotSupportedException();
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, true);
            this._client.SetDesktopAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.puma.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.puma.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                string cardTypeId = this._runner.Profile.CardTypeId;
                string str2 = "";
                switch (cardTypeId)
                {
                    case "0":
                        str2 = "Amex";
                        break;

                    case "1":
                        str2 = "Visa";
                        break;

                    case "2":
                        str2 = "Mastercard";
                        break;

                    case "3":
                        str2 = "Discover";
                        break;
                }
                bool flag = true;
                string str3 = "CREDIT_CARD";
                if (this._task.HomeUrl.Contains("eu.puma.com"))
                {
                    str3 = "PAYMENTOPERATOR_CREDIT_DIRECT";
                }
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_billing_paymentMethods_selectedPaymentMethodID", str3);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditdirect_owner", this._runner.Profile.NameOnCard);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditdirect_type", str2);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditdirect_number", this._runner.Profile.CCNumber);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditdirect_cvn", this._runner.Profile.Cvv);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditdirect_expiration_month", int.Parse(this._runner.Profile.ExpiryMonth).ToString());
                        this._diData.Add("dwfrm_billing_paymentMethods_creditdirect_expiration_year", this._runner.Profile.ExpiryYear);
                        this._dynObj = this._client.Post($"{this._demandwareLink}/Checkout-SubmitPayment", this._diData).Json();
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
                if (<>o__14.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__14.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Puma), argumentInfo));
                }
                if (<>o__14.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__14.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Puma), argumentInfo));
                }
                if (<>o__14.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__14.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                }
                object obj2 = <>o__14.<>p__1.Target(<>o__14.<>p__1, <>o__14.<>p__0.Target(<>o__14.<>p__0, this._dynObj, "error"), null);
                if (<>o__14.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__14.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Puma), argumentInfo));
                }
                if (!<>o__14.<>p__6.Target(<>o__14.<>p__6, obj2))
                {
                    if (<>o__14.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__14.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Puma), argumentInfo));
                    }
                    if (<>o__14.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__14.<>p__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Puma), argumentInfo));
                    }
                    if (<>o__14.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__14.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                }
                if (<>o__14.<>p__7.Target(<>o__14.<>p__7, (<>o__14.<>p__2 != null) ? obj2 : <>o__14.<>p__5.Target(<>o__14.<>p__5, obj2, <>o__14.<>p__4.Target(<>o__14.<>p__4, <>o__14.<>p__3.Target(<>o__14.<>p__3, <>o__14.<>p__2.Target(<>o__14.<>p__2, this._dynObj, "error")), true))))
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
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("dwfrm_billing_fraudFields_acceptCharset", "UTF-8");
                    this._diData.Add("dwfrm_billing_fraudFields_det1", $"TF1;015;;;;;;;;;;;;;;;;;;;;;;Mozilla;Netscape;5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36;20030107;undefined;true;;true;Win32;undefined;Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36;sk-SK;undefined;{this._task.HomeUrl.Replace("https://", "")}undefined;undefined;undefined;undefined;true;true;1525115552855;1;6/7/2005, 9:33:44 PM;1920;1080;;;;;;;11;-60;-120;{DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}, {DateTime.Now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture)};24;1920;1040;0;0;;;;;;;;;;;;;;;;;;;24;");
                    if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                    {
                        goto Label_1B08;
                    }
                    if (this._task.CheckoutDelay > 0)
                    {
                        Thread.Sleep(this._task.CheckoutDelay);
                    }
                    if (Global.SERIAL == "EVE-1111111111111")
                    {
                        goto Label_1B39;
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
                        this._dynObj = this._client.Post($"{this._demandwareLink}/Checkout-PlaceOrder", this._diData).Json();
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
                if (<>o__13.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__13.<>p__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Puma), argumentInfo));
                }
                if (<>o__13.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__13.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Puma), argumentInfo));
                }
                if (<>o__13.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__13.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                }
                object obj2 = <>o__13.<>p__1.Target(<>o__13.<>p__1, <>o__13.<>p__0.Target(<>o__13.<>p__0, this._dynObj, "error"), null);
                if (<>o__13.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__13.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Puma), argumentInfo));
                }
                if (!<>o__13.<>p__6.Target(<>o__13.<>p__6, obj2))
                {
                    if (<>o__13.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__13.<>p__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Puma), argumentInfo));
                    }
                    if (<>o__13.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__13.<>p__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Puma), argumentInfo));
                    }
                    if (<>o__13.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__13.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                }
                if (<>o__13.<>p__7.Target(<>o__13.<>p__7, (<>o__13.<>p__2 != null) ? obj2 : <>o__13.<>p__5.Target(<>o__13.<>p__5, obj2, <>o__13.<>p__4.Target(<>o__13.<>p__4, <>o__13.<>p__3.Target(<>o__13.<>p__3, <>o__13.<>p__2.Target(<>o__13.<>p__2, this._dynObj, "error")), true))))
                {
                    goto Label_1AD7;
                }
                if (<>o__13.<>p__9 == null)
                {
                    <>o__13.<>p__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Puma)));
                }
                if (<>o__13.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__13.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Puma), argumentInfo));
                }
                this._srr = <>o__13.<>p__9.Target(<>o__13.<>p__9, <>o__13.<>p__8.Target(<>o__13.<>p__8, this._dynObj));
                string str = "";
                if (<>o__13.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__13.<>p__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Puma), argumentInfo));
                }
                if (<>o__13.<>p__11 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null) };
                    <>o__13.<>p__11 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Puma), argumentInfo));
                }
                if (<>o__13.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__13.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                }
                obj2 = <>o__13.<>p__11.Target(<>o__13.<>p__11, <>o__13.<>p__10.Target(<>o__13.<>p__10, this._dynObj, "continueUrl"), null);
                if (<>o__13.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__13.<>p__17 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(Puma), argumentInfo));
                }
                if (!<>o__13.<>p__17.Target(<>o__13.<>p__17, obj2))
                {
                    if (<>o__13.<>p__16 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__13.<>p__16 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Puma), argumentInfo));
                    }
                    if (<>o__13.<>p__15 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__13.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(Puma), argumentInfo));
                    }
                    if (<>o__13.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__13.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Puma), argumentInfo));
                    }
                    if (<>o__13.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__13.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                }
                if (<>o__13.<>p__18.Target(<>o__13.<>p__18, (<>o__13.<>p__12 != null) ? obj2 : <>o__13.<>p__16.Target(<>o__13.<>p__16, obj2, <>o__13.<>p__15.Target(<>o__13.<>p__15, <>o__13.<>p__14.Target(<>o__13.<>p__14, <>o__13.<>p__13.Target(<>o__13.<>p__13, <>o__13.<>p__12.Target(<>o__13.<>p__12, this._dynObj, "continueUrl"))), "Redirect3DS"))))
                {
                    string str6;
                    States.WriteLogger(this._task, States.LOGGER_STATES.D3_SECURE_CARD_CHECK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.D3_SECURE_CARD_CHECK);
                    flag = true;
                    do
                    {
                        if (!flag)
                        {
                            goto Label_1AA1;
                        }
                        flag = false;
                        try
                        {
                            if (<>o__13.<>p__21 == null)
                            {
                                <>o__13.<>p__21 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Puma)));
                            }
                            if (<>o__13.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__19 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                            }
                            string str2 = <>o__13.<>p__21.Target(<>o__13.<>p__21, <>o__13.<>p__20.Target(<>o__13.<>p__20, <>o__13.<>p__19.Target(<>o__13.<>p__19, this._dynObj, "continueUrl")));
                            str = this._demandwareLink + str2.Substring(str2.IndexOf("/PaymentOperator"));
                            if (<>o__13.<>p__28 == null)
                            {
                                <>o__13.<>p__28 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Puma)));
                            }
                            if (<>o__13.<>p__27 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__27 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__26 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__26 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__25 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__25 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "UrlEncode", null, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__24 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__23 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__22 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "urlParams", typeof(Puma), argumentInfo));
                            }
                            str = <>o__13.<>p__28.Target(<>o__13.<>p__28, <>o__13.<>p__27.Target(<>o__13.<>p__27, str, <>o__13.<>p__26.Target(<>o__13.<>p__26, "?PaymentOperatorUrl=", <>o__13.<>p__25.Target(<>o__13.<>p__25, typeof(WebUtility), <>o__13.<>p__24.Target(<>o__13.<>p__24, <>o__13.<>p__23.Target(<>o__13.<>p__23, <>o__13.<>p__22.Target(<>o__13.<>p__22, this._dynObj), "PaymentOperatorUrl"))))));
                            if (<>o__13.<>p__35 == null)
                            {
                                <>o__13.<>p__35 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Puma)));
                            }
                            if (<>o__13.<>p__34 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__34 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__33 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__33 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__32 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__32 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "UrlEncode", null, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__31 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__30 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__30 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__29 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "urlParams", typeof(Puma), argumentInfo));
                            }
                            str = <>o__13.<>p__35.Target(<>o__13.<>p__35, <>o__13.<>p__34.Target(<>o__13.<>p__34, str, <>o__13.<>p__33.Target(<>o__13.<>p__33, "&PaymentRequest=", <>o__13.<>p__32.Target(<>o__13.<>p__32, typeof(WebUtility), <>o__13.<>p__31.Target(<>o__13.<>p__31, <>o__13.<>p__30.Target(<>o__13.<>p__30, <>o__13.<>p__29.Target(<>o__13.<>p__29, this._dynObj), "PaymentRequest"))))));
                            if (<>o__13.<>p__42 == null)
                            {
                                <>o__13.<>p__42 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Puma)));
                            }
                            if (<>o__13.<>p__41 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__41 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__40 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__40 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__39 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__39 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "UrlEncode", null, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__38 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__37 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__13.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                            }
                            if (<>o__13.<>p__36 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__13.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "urlParams", typeof(Puma), argumentInfo));
                            }
                            str = <>o__13.<>p__42.Target(<>o__13.<>p__42, <>o__13.<>p__41.Target(<>o__13.<>p__41, str, <>o__13.<>p__40.Target(<>o__13.<>p__40, "&TermUrl=", <>o__13.<>p__39.Target(<>o__13.<>p__39, typeof(WebUtility), <>o__13.<>p__38.Target(<>o__13.<>p__38, <>o__13.<>p__37.Target(<>o__13.<>p__37, <>o__13.<>p__36.Target(<>o__13.<>p__36, this._dynObj), "TermUrl"))))));
                            this._srr = this._client.Get(str).Text();
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
                        }
                        this._currentDoc.LoadHtml(this._srr);
                        str = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value);
                        string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                        string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                        string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"))).Attributes["value"].Value;
                        str6 = "";
                        flag = true;
                        while (flag)
                        {
                            flag = false;
                            try
                            {
                                this._diData.Clear();
                                this._diData.Add("MD", str3);
                                this._diData.Add("PaReq", str4);
                                this._diData.Add("TermUrl", str5);
                                KeyValuePair<string, string> pair = this._client.Post(str, this._diData).TextResponseUri();
                                this._srr = pair.Key;
                                str6 = pair.Value;
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
                        if (str6.Contains("verifiedbyvisa"))
                        {
                            str = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value);
                            str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                            string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                            flag = true;
                            while (flag)
                            {
                                flag = false;
                                try
                                {
                                    this._diData.Clear();
                                    this._diData.Add("MD", str3);
                                    this._diData.Add("PaRes", str7);
                                    this._client.Session.DefaultRequestHeaders.Referrer = new Uri(str6);
                                    KeyValuePair<string, string> pair2 = this._client.Post(str, this._diData).TextResponseUri();
                                    this._srr = pair2.Key;
                                    str6 = pair2.Value;
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
                        }
                        else
                        {
                            str = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"))).Attributes["action"].Value);
                            str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                            string str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                            str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                            string str14 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"))).Attributes["value"].Value;
                            string str15 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"))).Attributes["value"].Value;
                            string str17 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"))).Attributes["value"].Value;
                            string str16 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"))).Attributes["value"].Value;
                            string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"))).Attributes["value"].Value;
                            string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"))).Attributes["value"].Value;
                            string str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"))).Attributes["value"].Value;
                            string str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"))).Attributes["value"].Value;
                            string str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"))).Attributes["value"].Value;
                            flag = true;
                            while (flag)
                            {
                                flag = false;
                                try
                                {
                                    this._diData.Clear();
                                    this._diData.Add("PaRes", str13);
                                    this._diData.Add("MD", str3);
                                    this._diData.Add("PaReq", str4);
                                    this._diData.Add("ABSlog", str14);
                                    this._diData.Add("deviceDNA", str15);
                                    this._diData.Add("executionTime", str17);
                                    this._diData.Add("dnaError", str16);
                                    this._diData.Add("mesc", str8);
                                    this._diData.Add("mescIterationCount", str9);
                                    this._diData.Add("desc", str10);
                                    this._diData.Add("isDNADone", str11);
                                    this._diData.Add("arcotFlashCookie", str12);
                                    this._client.Session.DefaultRequestHeaders.Referrer = new Uri(str6);
                                    KeyValuePair<string, string> pair3 = this._client.Post(str, this._diData).TextResponseUri();
                                    this._srr = pair3.Key;
                                    str6 = pair3.Value;
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
                    }
                    while (!str6.Contains("checkout/start?stage=payment") && !str6.Contains("/cart"));
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                    return false;
                }
            Label_1AA1:;
                try
                {
                    EveAIO.Helpers.AddDbValue("Puma|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                }
                catch
                {
                }
                return true;
            Label_1AD7:
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            Label_1B08:
                this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                return false;
            Label_1B39:
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception6)
            {
                this._runner.IsError = true;
                if (exception6 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, null, "", "Web request timed out");
                }
                else if (!exception6.Message.Contains("404") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("404")))
                {
                    if (!exception6.Message.Contains("430") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_PAYMENT);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception6, "", "");
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
                ProfileObject profile = this._runner.Profile;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Get($"{this._homeLink}/checkout/start").Text();
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
                this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._currentDoc.LoadHtml(this._srr);
                        str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipmentUUID"))).Attributes["value"].Value;
                        this._diData.Clear();
                        if (this._task.HomeUrl.Contains("eu.puma.com"))
                        {
                            this._diData.Add("shipmentUUID", str);
                            this._diData.Add("salutation", "H");
                            this._diData.Add("firstName", profile.FirstNameShipping);
                            this._diData.Add("lastName", profile.LastNameShipping);
                            this._diData.Add("companyName", "");
                            this._diData.Add("suite", "n/a");
                            this._diData.Add("address1", profile.Address1Shipping);
                            this._diData.Add("address2", profile.Address2Shipping);
                            this._diData.Add("city", profile.CityShipping);
                            this._diData.Add("postalCode", profile.ZipShipping);
                            this._diData.Add("countryCode", profile.CountryIdShipping);
                        }
                        else
                        {
                            this._diData.Add("shipmentUUID", str);
                            this._diData.Add("salutation", "H");
                            this._diData.Add("firstName", profile.FirstNameShipping);
                            this._diData.Add("lastName", profile.LastNameShipping);
                            this._diData.Add("address1", profile.Address1Shipping);
                            this._diData.Add("address2", profile.Address2Shipping);
                            this._diData.Add("city", profile.CityShipping);
                            this._diData.Add("postalCode", profile.ZipShipping);
                            this._diData.Add("stateCode", profile.StateIdShipping);
                            this._diData.Add("countryCode", profile.CountryIdShipping);
                        }
                        this._dynObj = this._client.Post($"{this._demandwareLink}/Checkout-UpdateShippingMethodsList", this._diData).Json();
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
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                if (<>o__15.<>p__9 == null)
                {
                    <>o__15.<>p__9 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Puma)));
                }
                if (<>o__15.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "applicableShippingMethods", typeof(Puma), argumentInfo));
                }
                if (<>o__15.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Puma), argumentInfo));
                }
                if (<>o__15.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "shipping", typeof(Puma), argumentInfo));
                }
                if (<>o__15.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__15.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "order", typeof(Puma), argumentInfo));
                }
                foreach (object obj3 in <>o__15.<>p__9.Target(<>o__15.<>p__9, <>o__15.<>p__3.Target(<>o__15.<>p__3, <>o__15.<>p__2.Target(<>o__15.<>p__2, <>o__15.<>p__1.Target(<>o__15.<>p__1, <>o__15.<>p__0.Target(<>o__15.<>p__0, this._dynObj))))))
                {
                    if (<>o__15.<>p__8 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__15.<>p__8 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                    }
                    if (<>o__15.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__15.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                    if (<>o__15.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__15.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                    }
                    if (<>o__15.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__15.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Puma), argumentInfo));
                    }
                    if (<>o__15.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__15.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Puma), argumentInfo));
                    }
                    list.Add(<>o__15.<>p__8.Target(<>o__15.<>p__8, typeof(KeyValuePair<string, string>), <>o__15.<>p__5.Target(<>o__15.<>p__5, <>o__15.<>p__4.Target(<>o__15.<>p__4, obj3, "ID")), <>o__15.<>p__7.Target(<>o__15.<>p__7, <>o__15.<>p__6.Target(<>o__15.<>p__6, obj3, "shippingCost"))));
                }
                KeyValuePair<string, string> pair = list[0];
                string key = pair.Key;
                string s = "";
                pair = list[0];
                foreach (char ch in pair.Value)
                {
                    if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                    {
                        s = s + ch.ToString();
                    }
                }
                foreach (KeyValuePair<string, string> pair2 in list)
                {
                    string str5 = "";
                    foreach (char ch2 in pair2.Value)
                    {
                        if ((char.IsDigit(ch2) || (ch2 == '.')) || (ch2 == ','))
                        {
                            str5 = str5 + ch2.ToString();
                        }
                    }
                    if (double.Parse(str5, NumberStyles.Number, CultureInfo.InvariantCulture) < double.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture))
                    {
                        s = str5;
                        key = pair2.Key;
                    }
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._currentDoc.LoadHtml(this._srr);
                        str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipmentUUID"))).Attributes["value"].Value;
                        this._diData.Clear();
                        if (this._task.HomeUrl.Contains("eu.puma.com"))
                        {
                            this._diData.Add("shipmentUUID", str);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_salutation", "H");
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_firstName", profile.FirstNameShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_lastName", profile.LastNameShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_companyName", "");
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_suite", "n/a");
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_address1", profile.Address1Shipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_address2", profile.Address2Shipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_city", profile.CityShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_postalCode", profile.ZipShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_countries_countryCode", profile.CountryIdShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_shippingMethodID", key);
                            this._diData.Add("dwfrm_billing_addressFields_salutation", "H");
                            this._diData.Add("dwfrm_billing_addressFields_firstName", profile.FirstName);
                            this._diData.Add("dwfrm_billing_addressFields_lastName", profile.LastName);
                            this._diData.Add("dwfrm_billing_addressFields_companyName", "");
                            this._diData.Add("dwfrm_billing_addressFields_suite", "n/a");
                            this._diData.Add("dwfrm_billing_addressFields_address1", profile.Address1);
                            this._diData.Add("dwfrm_billing_addressFields_address2", profile.Address2);
                            this._diData.Add("dwfrm_billing_addressFields_city", profile.City);
                            this._diData.Add("dwfrm_billing_addressFields_postalCode", profile.Zip);
                            this._diData.Add("dwfrm_billing_addressFields_countries_countryCode", profile.CountryId);
                            this._diData.Add("dwfrm_shipping_shippingAddress_email", profile.Email);
                            this._diData.Add("dwfrm_shipping_shippingAddress_emailconfirm", profile.Email);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_phone", profile.Phone);
                        }
                        else
                        {
                            this._diData.Add("shipmentUUID", str);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_salutation", "H");
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_firstName", profile.FirstNameShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_lastName", profile.LastNameShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_address1", profile.Address1Shipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_address2", profile.Address2Shipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_city", profile.CityShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_postalCode", profile.ZipShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_states_stateCode", profile.StateIdShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_countries_countryCode", profile.CountryIdShipping);
                            this._diData.Add("dwfrm_shipping_shippingAddress_shippingMethodID", key);
                            this._diData.Add("dwfrm_billing_addressFields_salutation", "H");
                            this._diData.Add("dwfrm_billing_addressFields_firstName", profile.FirstName);
                            this._diData.Add("dwfrm_billing_addressFields_lastName", profile.LastName);
                            this._diData.Add("dwfrm_billing_addressFields_address1", profile.Address1);
                            this._diData.Add("dwfrm_billing_addressFields_address2", profile.Address2);
                            this._diData.Add("dwfrm_billing_addressFields_city", profile.City);
                            this._diData.Add("dwfrm_billing_addressFields_postalCode", profile.Zip);
                            this._diData.Add("dwfrm_billing_addressFields_states_stateCode", profile.StateId);
                            this._diData.Add("dwfrm_billing_addressFields_countries_countryCode", profile.CountryId);
                            this._diData.Add("dwfrm_shipping_shippingAddress_email", profile.Email);
                            this._diData.Add("dwfrm_shipping_shippingAddress_emailconfirm", profile.Email);
                            this._diData.Add("dwfrm_shipping_shippingAddress_addressFields_phone", profile.Phone);
                        }
                        this._srr = this._client.Post($"{this._demandwareLink}/Checkout-SubmitShipping?verified=true", this._diData).Text();
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
                return true;
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, null, "", "Web request timed out");
                }
                else if (!exception4.Message.Contains("404") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("404")))
                {
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
            public static readonly Puma.<>c <>9;
            public static Func<HtmlNode, bool> <>9__13_1;
            public static Func<HtmlNode, bool> <>9__13_2;
            public static Func<HtmlNode, bool> <>9__13_3;
            public static Func<HtmlNode, bool> <>9__13_4;
            public static Func<HtmlNode, bool> <>9__13_5;
            public static Func<HtmlNode, bool> <>9__13_6;
            public static Func<HtmlNode, bool> <>9__13_7;
            public static Func<HtmlNode, bool> <>9__13_8;
            public static Func<HtmlNode, bool> <>9__13_9;
            public static Func<HtmlNode, bool> <>9__13_10;
            public static Func<HtmlNode, bool> <>9__13_11;
            public static Func<HtmlNode, bool> <>9__13_12;
            public static Func<HtmlNode, bool> <>9__13_13;
            public static Func<HtmlNode, bool> <>9__13_14;
            public static Func<HtmlNode, bool> <>9__13_15;
            public static Func<HtmlNode, bool> <>9__13_16;
            public static Func<HtmlNode, bool> <>9__13_17;
            public static Func<HtmlNode, bool> <>9__13_18;
            public static Func<HtmlNode, bool> <>9__15_0;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__16_0;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__16_2;
            public static Func<HtmlNode, bool> <>9__16_3;
            public static Func<HtmlNode, bool> <>9__16_4;
            public static Func<HtmlNode, bool> <>9__16_7;
            public static Func<HtmlNode, bool> <>9__16_8;
            public static Func<HtmlNode, bool> <>9__16_9;
            public static Func<HtmlNode, bool> <>9__16_10;
            public static Func<HtmlNode, bool> <>9__16_11;
            public static Func<HtmlNode, bool> <>9__16_12;
            public static Func<HtmlNode, bool> <>9__16_13;
            public static Func<HtmlNode, bool> <>9__16_14;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Puma.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__16_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "country-selector"));

            internal bool <DirectLink>b__16_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "h1 product-name"));

            internal bool <DirectLink>b__16_10(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "attribute-select-size"));

            internal bool <DirectLink>b__16_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "attribute-select-size"));

            internal bool <DirectLink>b__16_12(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "select-size"));

            internal bool <DirectLink>b__16_13(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "select-size"));

            internal bool <DirectLink>b__16_14(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "color"));

            internal bool <DirectLink>b__16_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "row prices-reviewsummary-row"));

            internal bool <DirectLink>b__16_3(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sales")) && (x.Attributes["data-price-value"] > null));

            internal bool <DirectLink>b__16_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLink>b__16_7(HtmlNode x) => 
                ((x.Attributes["data-attr-value"] != null) && (x.Attributes["selectable"] > null));

            internal bool <DirectLink>b__16_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "attribute-select-size"));

            internal bool <DirectLink>b__16_9(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "select-size"));

            internal bool <SubmitOrder>b__13_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__13_10(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"));

            internal bool <SubmitOrder>b__13_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"));

            internal bool <SubmitOrder>b__13_12(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"));

            internal bool <SubmitOrder>b__13_13(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"));

            internal bool <SubmitOrder>b__13_14(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"));

            internal bool <SubmitOrder>b__13_15(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"));

            internal bool <SubmitOrder>b__13_16(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"));

            internal bool <SubmitOrder>b__13_17(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"));

            internal bool <SubmitOrder>b__13_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"));

            internal bool <SubmitOrder>b__13_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitOrder>b__13_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"));

            internal bool <SubmitOrder>b__13_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__13_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__13_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <SubmitOrder>b__13_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <SubmitOrder>b__13_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <SubmitOrder>b__13_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <SubmitShipping>b__15_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipmentUUID"));

            internal bool <SubmitShipping>b__15_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "shipmentUUID"));
        }

        [CompilerGenerated]
        private static class <>o__11
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, bool>> <>p__11;
            public static CallSite<Func<CallSite, object, bool>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, int, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool>> <>p__16;
        }

        [CompilerGenerated]
        private static class <>o__13
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
            public static CallSite<Func<CallSite, object, string>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, bool>> <>p__17;
            public static CallSite<Func<CallSite, object, bool>> <>p__18;
            public static CallSite<Func<CallSite, object, string, object>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, object>> <>p__24;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__25;
            public static CallSite<Func<CallSite, string, object, object>> <>p__26;
            public static CallSite<Func<CallSite, string, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string>> <>p__28;
            public static CallSite<Func<CallSite, object, object>> <>p__29;
            public static CallSite<Func<CallSite, object, string, object>> <>p__30;
            public static CallSite<Func<CallSite, object, object>> <>p__31;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__32;
            public static CallSite<Func<CallSite, string, object, object>> <>p__33;
            public static CallSite<Func<CallSite, string, object, object>> <>p__34;
            public static CallSite<Func<CallSite, object, string>> <>p__35;
            public static CallSite<Func<CallSite, object, object>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__39;
            public static CallSite<Func<CallSite, string, object, object>> <>p__40;
            public static CallSite<Func<CallSite, string, object, object>> <>p__41;
            public static CallSite<Func<CallSite, object, string>> <>p__42;
        }

        [CompilerGenerated]
        private static class <>o__14
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
            public static CallSite<Func<CallSite, object, bool>> <>p__7;
        }

        [CompilerGenerated]
        private static class <>o__15
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__8;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__9;
        }
    }
}

