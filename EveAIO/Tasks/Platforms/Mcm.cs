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
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Mcm : IPlatform
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

        public Mcm(TaskRunner runner, TaskObject task)
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
                        this._srr = this._client.Get(product.A + "&Quantity=1&format=ajax&format=ajax").Text();
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
                if (!this._srr.Contains("We're sorry. This product is currently not in stock. "))
                {
                    this._currentDoc.LoadHtml(this._srr);
                    string str = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "productID"))).InnerText.Trim();
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        this._diData.Clear();
                        this._diData.Add("cgid", "");
                        this._diData.Add("Quantity", "1");
                        this._diData.Add("uuid", "");
                        this._diData.Add("cartAction", "update");
                        this._diData.Add("pid", str);
                        this._diData.Add("joinnewsletter", "false");
                        try
                        {
                            this._srr = this._client.Post($"{this._demandwareLink}/Cart-AddProduct?format=ajax", this._diData).Text();
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
                    if (this._currentDoc.DocumentNode.Descendants("span").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-count")) && x.InnerText.Contains("1")))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                        return true;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SIZE_OOS);
                States.WriteLogger(this._task, States.LOGGER_STATES.SIZE_OOS, null, "", "");
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
            switch (((0x5aa54027 ^ 0x78442bbc) % 5))
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
            if (this.SubmitBilling())
            {
            }
            if (0x510ac389 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            try
            {
                HtmlNode node2;
                List<PumaProduct>.Enumerator enumerator3;
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
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
                string str2 = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "pdpForm"))).Attributes["action"].Value;
                this._demandwareLink = str2.Substring(0, str2.IndexOf("/Product-Detail"));
                this._homeLink = this._currentDoc.DocumentNode.Descendants("li").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "help"))).Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                this._homeLink = this._homeLink.Substring(0, this._homeLink.IndexOf("faq"));
                string str3 = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name"))).InnerText.Trim());
                string str = "";
                if (this._currentDoc.DocumentNode.Descendants("meta").Any<HtmlNode>(x => (x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price")))
                {
                    str = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("meta").Last<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).Attributes["content"].Value.Trim());
                }
                else
                {
                    str = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("span").Last<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim());
                }
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value;
                Product product1 = new Product {
                    ProductTitle = str3,
                    Link = link,
                    Price = str
                };
                this._runner.Product = product1;
                List<string> list = new List<string>();
                foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("li")
                    where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "emptyswatch")
                    select x)
                {
                    list.Add(node.Descendants("a").First<HtmlNode>().Attributes["href"].Value.Replace("&amp;", "&"));
                }
                if (this._currentDoc.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size")))
                {
                    node2 = this._currentDoc.DocumentNode.Descendants("select").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size"));
                    string str4 = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-variations"))).Attributes["data-current"].Value.Replace("&quot;", "\"");
                    this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str4);
                    if (<>o__16.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mcm), argumentInfo));
                    }
                    if (<>o__16.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__16.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mcm), argumentInfo));
                    }
                    if (<>o__16.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__16.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "color", typeof(Mcm), argumentInfo));
                    }
                    object obj2 = <>o__16.<>p__2.Target(<>o__16.<>p__2, <>o__16.<>p__1.Target(<>o__16.<>p__1, <>o__16.<>p__0.Target(<>o__16.<>p__0, this._dynObj), "displayValue"));
                    foreach (HtmlNode node3 in node2.Descendants("option"))
                    {
                        if (node3.Attributes["data-lgimg"] != null)
                        {
                            PumaProduct product6 = new PumaProduct {
                                Size = node3.InnerText.ToLowerInvariant().Replace("eur", "").Trim(),
                                A = node3.Attributes["value"].Value.Replace("&amp;", "&")
                            };
                            if (<>o__16.<>p__3 == null)
                            {
                                <>o__16.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mcm)));
                            }
                            product6.Color = <>o__16.<>p__3.Target(<>o__16.<>p__3, obj2);
                            PumaProduct item = product6;
                            this._runner.Product.AvailablePumaSizes.Add(item);
                        }
                    }
                }
                foreach (string str5 in list)
                {
                    this._srr = this._client.Get(str5).Text();
                    this._currentDoc.LoadHtml(this._srr);
                    if (this._currentDoc.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size")))
                    {
                        node2 = this._currentDoc.DocumentNode.Descendants("select").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size"));
                        string str6 = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-variations"))).Attributes["data-current"].Value.Replace("&quot;", "\"");
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str6);
                        if (<>o__16.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Mcm), argumentInfo));
                        }
                        if (<>o__16.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__16.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Mcm), argumentInfo));
                        }
                        if (<>o__16.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__16.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "color", typeof(Mcm), argumentInfo));
                        }
                        object obj3 = <>o__16.<>p__6.Target(<>o__16.<>p__6, <>o__16.<>p__5.Target(<>o__16.<>p__5, <>o__16.<>p__4.Target(<>o__16.<>p__4, this._dynObj), "displayValue"));
                        foreach (HtmlNode node4 in node2.Descendants("option"))
                        {
                            if (node4.Attributes["data-lgimg"] != null)
                            {
                                PumaProduct product7 = new PumaProduct {
                                    Size = node4.InnerText.Replace("Eur", "").Trim(),
                                    A = node4.Attributes["value"].Value.Replace("&amp;", "&")
                                };
                                if (<>o__16.<>p__7 == null)
                                {
                                    <>o__16.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Mcm)));
                                }
                                product7.Color = <>o__16.<>p__7.Target(<>o__16.<>p__7, obj3);
                                PumaProduct item = product7;
                                this._runner.Product.AvailablePumaSizes.Add(item);
                            }
                        }
                    }
                }
                if (this._runner.Product.AvailablePumaSizes.Count == 0)
                {
                    goto Label_1026;
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
                    double num4 = double.Parse(str7.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num4 < this._task.MinimumPrice) || (num4 > this._task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
                if (this._task.RandomSize)
                {
                    goto Label_0DFB;
                }
                char[] separator = new char[] { '#' };
                string[] strArray = this._task.Size.Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Trim().ToUpperInvariant();
                }
                string[] strArray2 = strArray;
                int index = 0;
            Label_0C26:
                if (index >= strArray2.Length)
                {
                    goto Label_0DD3;
                }
                string str9 = strArray2[index];
                if (this._runner.PickedPumaSize != null)
                {
                    goto Label_0DD3;
                }
                if (string.IsNullOrEmpty(this._task.Color))
                {
                    using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct product3;
                        goto Label_0C8D;
                    Label_0C74:
                        product3 = enumerator3.Current;
                        if (product3.Size == str9)
                        {
                            goto Label_0C98;
                        }
                    Label_0C8D:
                        if (!enumerator3.MoveNext())
                        {
                            goto Label_0DC8;
                        }
                        goto Label_0C74;
                    Label_0C98:
                        this._runner.PickedPumaSize = product3;
                        goto Label_0DC8;
                    }
                }
                if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                {
                    using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                    {
                        PumaProduct product4;
                        goto Label_0D1F;
                    Label_0CE3:
                        product4 = enumerator3.Current;
                        if ((product4.Size == str9) && product4.Color.ToLowerInvariant().Contains(this._task.Color.ToLowerInvariant()))
                        {
                            goto Label_0D2A;
                        }
                    Label_0D1F:
                        if (!enumerator3.MoveNext())
                        {
                            goto Label_0DC8;
                        }
                        goto Label_0CE3;
                    Label_0D2A:
                        this._runner.PickedPumaSize = product4;
                        goto Label_0DC8;
                    }
                }
                using (enumerator3 = this._runner.Product.AvailablePumaSizes.GetEnumerator())
                {
                    PumaProduct current;
                    while (enumerator3.MoveNext())
                    {
                        current = enumerator3.Current;
                        if ((current.Size == str9) && (current.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()))
                        {
                            goto Label_0DAB;
                        }
                    }
                    goto Label_0DC8;
                Label_0DAB:
                    this._runner.PickedPumaSize = current;
                }
            Label_0DC8:
                index++;
                goto Label_0C26;
            Label_0DD3:
                if (this._task.SizePickRandom || (this._runner.PickedPumaSize != null))
                {
                    goto Label_0F96;
                }
                return false;
            Label_0DFB:
                if (!string.IsNullOrEmpty(this._task.Color))
                {
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
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
                        List<PumaProduct> list2 = (from x in this._runner.Product.AvailablePumaSizes
                            where x.Color.ToLowerInvariant() == this._task.Color.ToLowerInvariant()
                            select x).ToList<PumaProduct>();
                        this._runner.PickedPumaSize = list2[this._runner.Rnd.Next(0, list2.Count)];
                    }
                }
                else
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
            Label_0F96:
                if (this._runner.PickedPumaSize == null)
                {
                    this._runner.PickedPumaSize = this._runner.Product.AvailablePumaSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailablePumaSizes.Count)];
                }
                this._runner.PickedSize = new KeyValuePair<string, string>(this._runner.PickedPumaSize.Size, this._runner.PickedPumaSize.Id);
                return true;
            Label_1026:
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
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                foreach (string str in this._task.Keywords)
                {
                    string url = $"http://us.mcmworldwide.com/en/search?q={WebUtility.UrlEncode(str)}";
                    this._srr = this._client.Get(url).Text();
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(this._srr);
                    if (!document.DocumentNode.Descendants("ul").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"))))
                    {
                        return false;
                    }
                    foreach (HtmlNode node in document.DocumentNode.Descendants("ul").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"))).Descendants("li"))
                    {
                        if (this.DirectLink(node.Descendants("a").First<HtmlNode>().Attributes["href"].Value))
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SEARCHING, null, "", "Web request timed out");
                }
                else if (!exception.Message.Contains("404") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("404")))
                {
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
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PAGE_NOT_FOUND);
                    States.WriteLogger(this._task, States.LOGGER_STATES.PAGE_NOT_FOUND, null, "", "");
                }
                return false;
            }
        }

        public void SetClient()
        {
            this._client = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, true);
            this._client.SetDesktopAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "Accept-Encoding: gzip, deflate");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                bool flag = true;
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"))).Attributes["action"].Value.Replace("http:", "https:");
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_billing_securekey"))).Attributes["value"].Value;
                string str2 = "";
                switch (profile.CardTypeId)
                {
                    case "0":
                        str2 = "Amex";
                        break;

                    case "1":
                        str2 = "Visa";
                        break;

                    case "2":
                        str2 = "MasterCard";
                        break;

                    case "3":
                        str2 = "Discover";
                        break;
                }
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_billing_paymentMethods_selectedPaymentMethodID", "CREDIT_CARD");
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_owner", profile.NameOnCard);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_type", str2);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_number", profile.CCNumber);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_month", int.Parse(profile.ExpiryMonth).ToString());
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_year", profile.ExpiryYear);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_cvn", profile.Cvv);
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_year", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_month", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_day", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_ssn", "");
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_firstName", profile.FirstName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_lastName", profile.LastName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_country", profile.CountryId);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_address1", profile.Address1);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_address2", profile.Address2);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_city", profile.City);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_states_state", profile.StateId);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_zip", profile.Zip);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_phone", profile.Phone);
                        this._diData.Add("dwfrm_billing_securekey", str3);
                        this._diData.Add("dwfrm_billing_couponCode", "");
                        this._diData.Add("dwfrm_billing_save", "Continue");
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
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
                if (!this._srr.Contains("daverror"))
                {
                    if (this._srr.Contains("Invalid Credit Card number"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.INVALID_CREDIT_CARD);
                        States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_CREDIT_CARD, null, "", "");
                        return false;
                    }
                    this._currentDoc.LoadHtml(this._srr);
                    if (this._currentDoc.DocumentNode.Descendants("button").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "order-confirmation")))
                    {
                        return true;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.INVALID_BILLING_INFO);
                    States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_BILLING_INFO, null, "", "");
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.INVALID_BILLING_INFO);
                States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_BILLING_INFO, null, "", "");
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
                using (List<Cookie>.Enumerator enumerator = this._client.Handler.CookieContainer.List().GetEnumerator())
                {
                    Cookie current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.Name == "rdf-uuid")
                        {
                            goto Label_006F;
                        }
                    }
                    goto Label_0086;
                Label_006F:
                    string text1 = current.Value;
                }
            Label_0086:
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
                    bool flag2 = true;
                    while (flag2)
                    {
                        flag2 = false;
                        try
                        {
                            this._diData.Clear();
                            this._srr = this._client.Post($"{this._homeLink}orderconfirmation".Replace("http:", "https:"), this._diData).Text();
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
                    if (!this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form")) && !string.IsNullOrEmpty(x.InnerText.Trim()))))
                    {
                        if (this._srr.Contains("Your shopping bag can currently not be ordered "))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "Your shopping bag can currently not be ordered since one or more of the products in your bag have an invalid price or are not available in the requested quantity.");
                            this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                            return false;
                        }
                        if (this._srr.Contains("We're sorry that your order could not be placed. This probably happened due to a very high order volume or temporary connection errors."))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "We're sorry that your order could not be placed. This probably happened due to a very high order volume or temporary connection errors.");
                            this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                            return false;
                        }
                        if (!this._srr.Contains("<h1>An Error Occurred!</h1>"))
                        {
                            try
                            {
                                EveAIO.Helpers.AddDbValue("Mcm|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                            }
                            catch
                            {
                            }
                            return true;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKOUT_UNSUCCESSFUL, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKOUT_UNSUCCESSFUL);
                        return false;
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
                        this._srr = this._client.Get($"{this._homeLink}/cart").Text();
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
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "continue-shopping-cart"))).Attributes["action"].Value.Replace("http:", "https:");
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_cart_checkoutCart", "Checkout");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri($"{this._homeLink}/cart");
                        this._srr = this._client.Post(url, this._diData).Text();
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
                url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_login"))).Attributes["action"].Value.Replace("http:", "https:");
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_login_securekey"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_login_unregistered", "Checkout");
                        this._diData.Add("dwfrm_login_securekey", str2);
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
                string uriString = url;
                this._currentDoc.LoadHtml(this._srr);
                url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_singleshipping_shippingAddress"))).Attributes["action"].Value.Replace("http:", "https:");
                str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_singleshipping_securekey"))).Attributes["value"].Value;
                flag = true;
                if (profile.CountryIdShipping == "US")
                {
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            url = url + "&format=ajax";
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_firstName=" + WebUtility.UrlEncode(profile.FirstNameShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_lastName=" + WebUtility.UrlEncode(profile.LastNameShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_country=" + WebUtility.UrlEncode(profile.CountryIdShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_address1=" + WebUtility.UrlEncode(profile.Address1Shipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_address2=" + WebUtility.UrlEncode(profile.Address2Shipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_city=" + WebUtility.UrlEncode(profile.CityShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_states_state=" + WebUtility.UrlEncode(profile.StateIdShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_zip=" + WebUtility.UrlEncode(profile.ZipShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_phone=" + WebUtility.UrlEncode(profile.PhoneShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_emailAddress=" + WebUtility.UrlEncode(profile.EmailShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_shippingMethodID=001";
                            url = url + "&dwfrm_singleshipping_securekey=" + str2;
                            url = url + "&dwfrm_singleshipping_shippingAddress_save=submit";
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                            this._srr = this._client.Get(url).Text();
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
                    if (this._srr.Contains("daverror"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.INVALID_SHIPPING_INFO);
                        States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_SHIPPING_INFO, null, "", "");
                        return false;
                    }
                    uriString = url;
                    this._currentDoc.LoadHtml(this._srr);
                    url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_confirmshippingaddress"))).Attributes["action"].Value.Replace("http:", "https:");
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("dwfrm_confirmshippingaddress_original", "dwfrm_confirmshippingaddress_suggested");
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                            this._srr = this._client.Post(url, this._diData).Text();
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
                else
                {
                    if (profile.CountryIdShipping != "GB")
                    {
                        goto Label_0A3E;
                    }
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            url = url + "&format=ajax";
                            url = url + "&dwfrm_singleshipping_shippingAddress_shippingMethodID=standard_delivery_GB";
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_firstName=" + WebUtility.UrlEncode(profile.FirstNameShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_lastName=" + WebUtility.UrlEncode(profile.LastNameShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_country=" + WebUtility.UrlEncode(profile.CountryIdShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_address1=" + WebUtility.UrlEncode(profile.Address1Shipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_houseNumber=" + WebUtility.UrlEncode(profile.Address2Shipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_city=" + WebUtility.UrlEncode(profile.CityShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_zip=" + WebUtility.UrlEncode(profile.ZipShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_phone=" + WebUtility.UrlEncode(profile.PhoneShipping);
                            url = url + "&dwfrm_singleshipping_shippingAddress_addressFields_emailAddress=" + WebUtility.UrlEncode(profile.EmailShipping);
                            url = url + "&dwfrm_singleshipping_securekey=" + str2;
                            url = url + "&dwfrm_singleshipping_shippingAddress_save=submit";
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                            this._srr = this._client.Get(url).Text();
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
                    if (!this._srr.Contains("app.urls.billingstart"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.INVALID_SHIPPING_INFO);
                        States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_SHIPPING_INFO, null, "", "");
                        return false;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                return true;
            Label_0A3E:
                this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                States.WriteLogger(this._task, States.LOGGER_STATES.SHIPPING_NOT_AVAILABLE, null, "", "");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception7)
            {
                this._runner.IsError = true;
                if (exception7 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, null, "", "Web request timed out");
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
            public static readonly Mcm.<>c <>9;
            public static Func<HtmlNode, bool> <>9__11_1;
            public static Func<HtmlNode, bool> <>9__11_2;
            public static Func<HtmlNode, bool> <>9__13_1;
            public static Func<HtmlNode, bool> <>9__14_0;
            public static Func<HtmlNode, bool> <>9__14_1;
            public static Func<HtmlNode, bool> <>9__14_2;
            public static Func<HtmlNode, bool> <>9__15_0;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__15_2;
            public static Func<HtmlNode, bool> <>9__15_3;
            public static Func<HtmlNode, bool> <>9__15_4;
            public static Func<HtmlNode, bool> <>9__15_5;
            public static Func<HtmlNode, bool> <>9__16_0;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__16_2;
            public static Func<HtmlNode, bool> <>9__16_3;
            public static Func<HtmlNode, bool> <>9__16_4;
            public static Func<HtmlNode, bool> <>9__16_5;
            public static Func<HtmlNode, bool> <>9__16_6;
            public static Func<HtmlNode, bool> <>9__16_10;
            public static Func<HtmlNode, bool> <>9__16_7;
            public static Func<HtmlNode, bool> <>9__16_11;
            public static Func<HtmlNode, bool> <>9__16_12;
            public static Func<HtmlNode, bool> <>9__16_13;
            public static Func<HtmlNode, bool> <>9__16_14;
            public static Func<HtmlNode, bool> <>9__16_15;
            public static Func<HtmlNode, bool> <>9__18_0;
            public static Func<HtmlNode, bool> <>9__18_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Mcm.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__11_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "productID"));

            internal bool <Atc>b__11_2(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "item-count")) && x.InnerText.Contains("1"));

            internal bool <DirectLink>b__16_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "pdpForm"));

            internal bool <DirectLink>b__16_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "help"));

            internal bool <DirectLink>b__16_10(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "emptyswatch"));

            internal bool <DirectLink>b__16_11(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size"));

            internal bool <DirectLink>b__16_12(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-variations"));

            internal bool <DirectLink>b__16_13(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size"));

            internal bool <DirectLink>b__16_14(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size"));

            internal bool <DirectLink>b__16_15(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-variations"));

            internal bool <DirectLink>b__16_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-name"));

            internal bool <DirectLink>b__16_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__16_4(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__16_5(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__16_6(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLink>b__16_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "va-size"));

            internal bool <Search>b__18_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"));

            internal bool <Search>b__18_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "search-result-items"));

            internal bool <SubmitBilling>b__14_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"));

            internal bool <SubmitBilling>b__14_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_billing_securekey"));

            internal bool <SubmitBilling>b__14_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "order-confirmation"));

            internal bool <SubmitOrder>b__13_1(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form")) && !string.IsNullOrEmpty(x.InnerText.Trim()));

            internal bool <SubmitShipping>b__15_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "continue-shopping-cart"));

            internal bool <SubmitShipping>b__15_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_login"));

            internal bool <SubmitShipping>b__15_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_login_securekey"));

            internal bool <SubmitShipping>b__15_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_singleshipping_shippingAddress"));

            internal bool <SubmitShipping>b__15_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_singleshipping_securekey"));

            internal bool <SubmitShipping>b__15_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_confirmshippingaddress"));
        }

        [CompilerGenerated]
        private static class <>o__16
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
        }
    }
}

