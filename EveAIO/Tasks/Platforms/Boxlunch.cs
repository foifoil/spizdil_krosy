namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Boxlunch : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        [Dynamic]
        private object _dynObj;
        private Dictionary<string, string> _diData;
        private string _payLink;
        private string _category;
        private bool _isLoggedIn;

        public Boxlunch(TaskRunner runner, TaskObject task)
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
                if (string.IsNullOrEmpty(this._task.VariousStringData2))
                {
                    this._task.VariousStringData2 = "1";
                }
                while (flag)
                {
                    flag = false;
                    this._diData.Clear();
                    this._diData.Add("Quantity", this._task.VariousStringData2);
                    this._diData.Add("cgid", this._category);
                    this._diData.Add("cartAction", "add");
                    this._diData.Add("pid", this._runner.PickedSize.Value.Value);
                    try
                    {
                        this._srr = this._client.Post("https://www.boxlunch.com/on/demandware.store/Sites-boxlunch-Site/default/Cart-AddProduct?format=ajax", this._diData).Text();
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
                if (!this._srr.Contains("Out of stock"))
                {
                    if (!this._currentDoc.DocumentNode.Descendants("span").Any<HtmlNode>(x => (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "value")) && (x.InnerText == this._task.VariousStringData2))))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                return false;
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
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
            switch (((-16262353 ^ -1269851310) % 5))
            {
                case 1:
                    return false;

                case 2:
                    return false;

                case 3:
                    goto Label_001C;

                case 4:
                    break;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (this.SubmitBilling())
            {
            }
            if (-38533117 || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            if ((!this._isLoggedIn && this._task.Login) && !this.Login())
            {
                return false;
            }
            try
            {
                Product product1;
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
                if (this._srr.Contains("<li>Out Of Stock</li>"))
                {
                    goto Label_07D3;
                }
                this._currentDoc.LoadHtml(this._srr);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str2 = WebUtility.HtmlDecode(this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText);
                string str = this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim();
                this._task.ImgUrl = this._currentDoc.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value;
                if ((this._task.TaskType == TaskObject.TaskTypeEnum.keywords) && (this._task.NegativeKeywords.Count > 0))
                {
                    using (IEnumerator<string> enumerator = this._task.NegativeKeywords.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            char[] separator = new char[] { ' ' };
                            foreach (string str3 in enumerator.Current.ToUpperInvariant().Split(separator))
                            {
                                if (str2.ToUpperInvariant().Contains(str3))
                                {
                                    goto Label_029F;
                                }
                            }
                        }
                        goto Label_02CB;
                    Label_029F:
                        States.WriteLogger(this._task, States.LOGGER_STATES.NEGATIVE_KWS_MATCH, null, "", "");
                        return false;
                    }
                }
            Label_02CB:
                product1 = new Product();
                product1.ProductTitle = str2 + " (" + this._task.VariousStringData2 + "x)";
                product1.Link = link;
                product1.Price = str;
                this._runner.Product = product1;
                this._category = this._srr.Substring(this._srr.IndexOf("dw.ac.applyContext({category"));
                this._category = this._category.Substring(this._category.IndexOf("\"") + 1);
                this._category = this._category.Substring(0, this._category.IndexOf("\""));
                this._runner.Product.AvailableSizes.Add(new KeyValuePair<string, string>("-", this._currentDoc.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "sku"))).InnerText));
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
                        string str5 = "";
                        foreach (char ch in this._runner.Product.Price)
                        {
                            if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                            {
                                str5 = str5 + ch.ToString();
                            }
                        }
                        double num4 = double.Parse(str5.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                        string[] strArray2 = this._task.Size.Split(separator);
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            strArray2[i] = strArray2[i].Trim().ToUpperInvariant();
                        }
                        foreach (string sz in strArray2)
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
                                    List<string> source = new List<string>();
                                    if (current.Key.Contains(":"))
                                    {
                                        char[] chArray3 = new char[] { ':' };
                                        string[] strArray3 = current.Key.Split(chArray3);
                                        for (int k = 0; k < strArray3.Length; k++)
                                        {
                                            source.Add(strArray3[k].Trim());
                                        }
                                    }
                                    else
                                    {
                                        source.Add(current.Key);
                                    }
                                    for (int j = 0; j < source.Count; j++)
                                    {
                                        source[j] = source[j].Trim().ToUpperInvariant();
                                    }
                                    if (source.Any<string>(x => x == sz))
                                    {
                                        goto Label_06E0;
                                    }
                                }
                                continue;
                            Label_06E0:
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
                if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                }
                return false;
            Label_07D3:
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
                this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGGING_IN, null, "", "");
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.boxlunch.com/account").Text();
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (exception.Message.Contains("404") || exception.Message.Contains("407"))
                        {
                            throw;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                        Thread.Sleep(0x5dc);
                        flag = true;
                        continue;
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_login"))).Attributes["action"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_login_securekey"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_login_username_d0wvbsegniww", this._task.Username);
                        this._diData.Add("dwfrm_login_password", this._task.Password);
                        this._diData.Add("dwfrm_login_login", "Sign In");
                        this._diData.Add("dwfrm_login_rememberme", "true");
                        this._diData.Add("dwfrm_login_securekey", str2);
                        this._diData.Add("reCaptcha", "");
                        this._srr = this._client.Post(url, this._diData).Text();
                        continue;
                    }
                    catch (WebException exception2)
                    {
                        if (exception2.Message.Contains("404") || exception2.Message.Contains("407"))
                        {
                            throw;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                        Thread.Sleep(0x5dc);
                        flag = true;
                        continue;
                    }
                }
                if (this._srr.Contains("class=\"welcome\">"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_SUCCESSFUL, null, "", "");
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._srr = this._client.Get("https://www.boxlunch.com/cart").Text();
                            continue;
                        }
                        catch (WebException exception3)
                        {
                            if (exception3.Message.Contains("404") || exception3.Message.Contains("407"))
                            {
                                throw;
                            }
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                            Thread.Sleep(0x5dc);
                            flag = true;
                            continue;
                        }
                    }
                    this._currentDoc.LoadHtml(this._srr);
                    if (!this._srr.Contains("There are no items in your shopping bag"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CLEANING_CART);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CLEARING_CART, null, "", "");
                        while (!this._srr.Contains("There are no items in your shopping bag"))
                        {
                            this._currentDoc.LoadHtml(this._srr);
                            url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cart-items-form"))).Attributes["action"].Value;
                            flag = true;
                            while (flag)
                            {
                                flag = false;
                                try
                                {
                                    this._diData.Clear();
                                    this._diData.Add("dwfrm_cart_shipments_i0_items_i0_quantity", "1");
                                    this._diData.Add("dwfrm_cart_shipments_i0_items_i0_deleteProduct", "Remove");
                                    this._diData.Add("dwfrm_cart_updateCart", "dwfrm_cart_updateCart");
                                    this._srr = this._client.Post(url, this._diData).Text();
                                    continue;
                                }
                                catch (WebException exception4)
                                {
                                    if (exception4.Message.Contains("404") || exception4.Message.Contains("407"))
                                    {
                                        throw;
                                    }
                                    States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                                    Thread.Sleep(0x5dc);
                                    flag = true;
                                    continue;
                                }
                            }
                        }
                    }
                    this._isLoggedIn = true;
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_UNSUCCESSFUL, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.LOGIN_UNSUCCESSFUL);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception5)
            {
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
                this._runner.IsError = true;
                if (exception5 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_LOGIN);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_LOGGING_IN, null, "", "Web request timed out");
                }
                else if (!exception5.Message.Contains("404") && ((exception5.InnerException == null) || !exception5.InnerException.Message.Contains("404")))
                {
                    if (!exception5.Message.Contains("430") && ((exception5.InnerException == null) || !exception5.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_LOGIN);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_LOGGING_IN, exception5, "", "");
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
            if ((!this._isLoggedIn && this._task.Login) && !this.Login())
            {
                return false;
            }
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                foreach (string str in this._task.Keywords)
                {
                    string url = $"https://www.boxlunch.com/search?q={str.Replace(" ", "+").ToLowerInvariant()}";
                    KeyValuePair<string, string> pair = this._client.Get(url).TextResponseUri();
                    this._srr = pair.Key;
                    if (pair.Value.Contains("/product/"))
                    {
                        return this.DirectLink(pair.Value);
                    }
                    HtmlDocument document1 = new HtmlDocument();
                    document1.LoadHtml(this._srr);
                    foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("div")
                        where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-tile")
                        select x)
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.boxlunch.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.boxlunch.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                this._payLink = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"))).Attributes["action"].Value;
                string str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_billing_securekey"))).Attributes["value"].Value;
                string str7 = this._srr.Substring(this._srr.IndexOf("Payeezy.setApiKey"));
                str7 = str7.Substring(str7.IndexOf("\"") + 1);
                str7 = str7.Substring(0, str7.IndexOf("\""));
                string str3 = this._srr.Substring(this._srr.IndexOf("Payeezy.setMerchantIdentifier"));
                str3 = str3.Substring(str3.IndexOf("\"") + 1);
                str3 = str3.Substring(0, str3.IndexOf("\""));
                string str8 = this._srr.Substring(this._srr.IndexOf("Payeezy.setTaToken"));
                str8 = str8.Substring(str8.IndexOf("\"") + 1);
                str8 = str8.Substring(0, str8.IndexOf("\""));
                string str4 = this._srr.Substring(this._srr.IndexOf("Payeezy.setJSSecurityKey"));
                str4 = str4.Substring(str4.IndexOf("\"") + 1);
                str4 = str4.Substring(0, str4.IndexOf("\""));
                bool flag = true;
                string cardTypeId = this._runner.Profile.CardTypeId;
                string str5 = "";
                string str = "";
                switch (cardTypeId)
                {
                    case "0":
                        str5 = "American Express";
                        str = "American Express";
                        break;

                    case "1":
                        str5 = "Visa";
                        str = "Visa";
                        break;

                    case "2":
                        str5 = "Mastercard";
                        str = "Master Card";
                        break;

                    case "3":
                        str5 = "Discover";
                        str = "Discover";
                        break;
                }
                while (flag)
                {
                    flag = false;
                    try
                    {
                        string url = "https://api.payeezy.com/v1/securitytokens?auth=false";
                        url = (((((((((url + "&js_security_key=" + str4) + "&ta_token=" + str8) + "&apikey=" + str7) + "&trtoken=" + str3) + "&callback=Payeezy.callback" + "&type=FDToken") + "&credit_card.type=" + WebUtility.UrlEncode(str5)) + "&credit_card.cardholder_name=" + WebUtility.UrlEncode(this._runner.Profile.NameOnCard)) + "&credit_card.card_number=" + this._runner.Profile.CCNumber) + "&credit_card.exp_date=" + this._runner.Profile.ExpiryMonth + this._runner.Profile.ExpiryYear.Substring(2)) + "&credit_card.cvv=" + this._runner.Profile.Cvv;
                        this._srr = this._client.Get(url).Text();
                        string str10 = this._srr.Substring(this._srr.IndexOf("{"));
                        str10 = str10.Substring(0, str10.LastIndexOf("}") + 1);
                        this._dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(str10);
                        if (<>o__15.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__15.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__15.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__15.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__15.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__15.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "results", typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__4.Target(<>o__15.<>p__4, <>o__15.<>p__3.Target(<>o__15.<>p__3, <>o__15.<>p__2.Target(<>o__15.<>p__2, <>o__15.<>p__1.Target(<>o__15.<>p__1, <>o__15.<>p__0.Target(<>o__15.<>p__0, this._dynObj), "status")), "failed")))
                        {
                            this._task.Status = States.GetTaskState(States.TaskState.INVALID_CREDIT_CARD);
                            States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_CREDIT_CARD, null, "", "");
                            return false;
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
                string str11 = "";
                if (this._runner.Profile.CountryId == "US")
                {
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                            object obj2 = this._client.Get($"https://www.boxlunch.com/on/demandware.store/Sites-boxlunch-Site/default/InlineTypeDown-GetCityState?zip={this._runner.Profile.Zip}&country=USA").Json();
                            this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                            if (<>o__15.<>p__7 == null)
                            {
                                <>o__15.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Boxlunch)));
                            }
                            if (<>o__15.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__15.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Boxlunch), argumentInfo));
                            }
                            if (<>o__15.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__15.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Boxlunch), argumentInfo));
                            }
                            str11 = <>o__15.<>p__7.Target(<>o__15.<>p__7, <>o__15.<>p__6.Target(<>o__15.<>p__6, <>o__15.<>p__5.Target(<>o__15.<>p__5, obj2, "moniker")));
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
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        string str12 = "";
                        for (int i = 0; i < (this._runner.Profile.CCNumber.Length - 4); i++)
                        {
                            str12 = str12 + "*";
                        }
                        str12 = str12 + this._runner.Profile.CCNumber.Substring(this._runner.Profile.CCNumber.Length - 4);
                        this._diData.Clear();
                        this._diData.Add("dwfrm_billing_addressChoice_addressChoices", "new");
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_firstName", this._runner.Profile.FirstName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_lastName", this._runner.Profile.LastName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_country", this._runner.Profile.CountryId);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_postal", this._runner.Profile.Zip);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_address1", this._runner.Profile.Address1);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_address2", this._runner.Profile.Address2);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_city", this._runner.Profile.City);
                        this._diData.Add("iMoniker", str11);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_states_state", (this._runner.Profile.CountryId == "US") ? this._runner.Profile.StateId : "OTHER");
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_phone", this._runner.Profile.Phone);
                        this._diData.Add("dwfrm_billing_securekey", str6);
                        this._diData.Add("dwfrm_billing_couponCode", "");
                        this._diData.Add("dwfrm_billing_giftCertCode", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_selectedPaymentMethodID", "CREDIT_CARD");
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_owner", this._runner.Profile.NameOnCard);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_number", str12);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_type", str);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_month", int.Parse(this._runner.Profile.ExpiryMonth).ToString());
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_year", this._runner.Profile.ExpiryYear);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_cvn", this._runner.Profile.Cvv);
                        if (<>o__15.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__15.<>p__12 = CallSite<Action<CallSite, Dictionary<string, string>, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__15.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__10 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__15.<>p__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__15.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "token", typeof(Boxlunch), argumentInfo));
                        }
                        if (<>o__15.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__15.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "results", typeof(Boxlunch), argumentInfo));
                        }
                        <>o__15.<>p__12.Target(<>o__15.<>p__12, this._diData, "cardToken", <>o__15.<>p__11.Target(<>o__15.<>p__11, <>o__15.<>p__10.Target(<>o__15.<>p__10, <>o__15.<>p__9.Target(<>o__15.<>p__9, <>o__15.<>p__8.Target(<>o__15.<>p__8, this._dynObj)), "value")));
                        this._diData.Add("cardBin", this._runner.Profile.CCNumber.Substring(0, 6));
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_year", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_month", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_day", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_bml_ssn", "");
                        this._diData.Add("dwfrm_billing_save", "Continue to Review");
                        this._srr = this._client.Post(this._payLink, this._diData).Text();
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
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
                this._runner.IsError = true;
                if (exception4 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_BILLING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, null, "", "Web request timed out");
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
                    this._diData.Add("cardBin", this._runner.Profile.CCNumber.Substring(0, 6));
                    this._diData.Add("addToEmailList", "false");
                    this._diData.Add("submit", "Place Order");
                    if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                    {
                        goto Label_02F3;
                    }
                    if (this._task.CheckoutDelay > 0)
                    {
                        Thread.Sleep(this._task.CheckoutDelay);
                    }
                    if (Global.SERIAL == "EVE-1111111111111")
                    {
                        goto Label_0324;
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
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._payLink);
                        this._srr = this._client.Post("https://www.boxlunch.com/orderconfirmation", this._diData).Text();
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
                if (!this._srr.Contains("There is currently a Box Lunch account for this email address, please login or reset your password"))
                {
                    if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form")) && !string.IsNullOrEmpty(x.InnerText.Trim())))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        return false;
                    }
                    try
                    {
                        EveAIO.Helpers.AddDbValue("Boxlunch|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
                    }
                    catch
                    {
                    }
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_REQUIRED, null, "", "There is currently a Box Lunch account for this email address, please login or reset your password");
                this._task.Status = States.GetTaskState(States.TaskState.LOGIN_REQUIRED);
                return false;
            Label_02F3:
                this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                return false;
            Label_0324:
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
                this._isLoggedIn = false;
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
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Get("https://www.boxlunch.com/cart").Text();
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
                if (!string.IsNullOrEmpty(this._task.VariousStringData))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.APPLYING_COUPON_CODE);
                    States.WriteLogger(this._task, States.LOGGER_STATES.APPLYING_COUPON_CODE, null, "", "");
                    string str = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_cart"))).Attributes["action"].Value;
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.boxlunch.com/cart");
                            this._diData.Clear();
                            this._diData.Add("dwfrm_cart_couponCode", this._task.VariousStringData);
                            this._diData.Add("dwfrm_cart_addCoupon", "dwfrm_cart_addCoupon");
                            this._srr = this._client.Post(str, this._diData).Text();
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
                    if (this._srr.Contains("<span class=\"bonus-item\">Applied</span>"))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.COUPON_APPLIED, null, "", "");
                    }
                    else
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.COUPON_NOT_APPLIED, null, "", "");
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        string str2 = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "checkout-form"))).Attributes["action"].Value;
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.boxlunch.com/cart");
                        this._diData.Clear();
                        this._diData.Add("dwfrm_cart_shippingMethod", "shipToHome");
                        this._diData.Add("dwfrm_cart_checkoutCart", "Checkout");
                        this._srr = this._client.Post(str2, this._diData).Text();
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
                if (!this._isLoggedIn)
                {
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._currentDoc.LoadHtml(this._srr);
                            string str3 = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                            string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_login_securekey"))).Attributes["value"].Value;
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.boxlunch.com/cart");
                            this._diData.Clear();
                            this._diData.Add("dwfrm_login_unregistered", "Checkout As a Guest");
                            this._diData.Add("dwfrm_login_securekey", str4);
                            this._srr = this._client.Post(str3, this._diData).Text();
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
                    this._currentDoc.LoadHtml(this._srr);
                }
                string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_singleshipping_securekey"))).Attributes["value"].Value;
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_singleshipping_shippingAddress"))).Attributes["action"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Get("https://www.boxlunch.com/on/demandware.store/Sites-boxlunch-Site/default/COShipping-UpdateShippingMethodList?address1=&address2=&countryCode=" + this._runner.Profile.CountryIdShipping + "&stateCode=&postalCode=&city").Text();
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
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                foreach (HtmlNode node in this._currentDoc.DocumentNode.Descendants("label"))
                {
                    string forr = node.Attributes["for"].Value;
                    string str8 = node.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "value"))).InnerText.Trim();
                    string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == forr))).Attributes["value"].Value;
                    list.Add(new KeyValuePair<string, string>(str7, str8));
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
                    string str12 = "";
                    foreach (char ch2 in pair2.Value)
                    {
                        if ((char.IsDigit(ch2) || (ch2 == '.')) || (ch2 == ','))
                        {
                            str12 = str12 + ch2.ToString();
                        }
                    }
                    if (double.Parse(str12, NumberStyles.Number, CultureInfo.InvariantCulture) < double.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture))
                    {
                        s = str12;
                        key = pair2.Key;
                    }
                }
                string str13 = "";
                if (this._runner.Profile.CountryIdShipping == "US")
                {
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                            this._dynObj = this._client.Get($"https://www.boxlunch.com/on/demandware.store/Sites-boxlunch-Site/default/InlineTypeDown-GetCityState?zip={this._runner.Profile.ZipShipping}&country=USA").Json();
                            this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                            if (<>o__16.<>p__2 == null)
                            {
                                <>o__16.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Boxlunch)));
                            }
                            if (<>o__16.<>p__1 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__16.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Boxlunch), argumentInfo));
                            }
                            if (<>o__16.<>p__0 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__16.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Boxlunch), argumentInfo));
                            }
                            str13 = <>o__16.<>p__2.Target(<>o__16.<>p__2, <>o__16.<>p__1.Target(<>o__16.<>p__1, <>o__16.<>p__0.Target(<>o__16.<>p__0, this._dynObj, "moniker")));
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
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_singleshipping_email_emailAddress", this._runner.Profile.EmailShipping);
                        this._diData.Add("dwfrm_singleshipping_addToEmailList", "true");
                        this._diData.Add("dwfrm_singleshipping_shipmentType_shipmentTypes", "Checkout");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_firstName", this._runner.Profile.FirstNameShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_lastName", this._runner.Profile.LastNameShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_country", this._runner.Profile.CountryIdShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_postal", this._runner.Profile.ZipShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_address1", this._runner.Profile.Address1Shipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_address2", this._runner.Profile.Address2Shipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_city", this._runner.Profile.CityShipping);
                        this._diData.Add("iMoniker", str13);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_states_state", (this._runner.Profile.CountryIdShipping == "US") ? this._runner.Profile.StateIdShipping : "OTHER");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_phone", this._runner.Profile.PhoneShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_useAsBillingAddress", "false");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_shippingMethodID", key);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_giftMessage", "");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_isGift", "false");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_save", "Continue to Billing");
                        this._diData.Add("dwfrm_singleshipping_securekey", str5);
                        this._srr = this._client.Post(url, this._diData).Text();
                        continue;
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
                        continue;
                    }
                }
                if (this._runner.Profile.CountryIdShipping == "US")
                {
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._currentDoc.LoadHtml(this._srr);
                            string str14 = "";
                            if (this._currentDoc.DocumentNode.Descendants("form").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "edit-address-form")))
                            {
                                str14 = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "edit-address-form"))).Attributes["action"].Value;
                            }
                            else
                            {
                                str14 = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"))).Attributes["action"].Value;
                            }
                            this._diData.Clear();
                            this._diData.Add("dwfrm_addForm_useOrig", "");
                            this._srr = this._client.Post(str14, this._diData).Text();
                            continue;
                        }
                        catch (WebException exception8)
                        {
                            if (!exception8.Message.Contains("504") && !exception8.Message.Contains("503"))
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
                this._currentDoc.LoadHtml(this._srr);
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception9)
            {
                this._runner.Cookies = new CookieContainer();
                this._isLoggedIn = false;
                this.SetClient();
                this._runner.IsError = true;
                if (exception9 is AggregateException)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, null, "", "Web request timed out");
                }
                else if (!exception9.Message.Contains("404") && ((exception9.InnerException == null) || !exception9.InnerException.Message.Contains("404")))
                {
                    if (!exception9.Message.Contains("430") && ((exception9.InnerException == null) || !exception9.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception9, "", "");
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
            public static readonly Boxlunch.<>c <>9;
            public static Func<HtmlNode, bool> <>9__14_1;
            public static Func<HtmlNode, bool> <>9__15_0;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__16_2;
            public static Func<HtmlNode, bool> <>9__16_3;
            public static Func<HtmlNode, bool> <>9__16_4;
            public static Func<HtmlNode, bool> <>9__16_0;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__16_5;
            public static Func<HtmlNode, bool> <>9__16_7;
            public static Func<HtmlNode, bool> <>9__16_8;
            public static Func<HtmlNode, bool> <>9__16_9;
            public static Func<HtmlNode, bool> <>9__17_0;
            public static Func<HtmlNode, bool> <>9__17_1;
            public static Func<HtmlNode, bool> <>9__17_2;
            public static Func<HtmlNode, bool> <>9__17_3;
            public static Func<HtmlNode, bool> <>9__18_0;
            public static Func<HtmlNode, bool> <>9__18_1;
            public static Func<HtmlNode, bool> <>9__18_2;
            public static Func<HtmlNode, bool> <>9__19_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Boxlunch.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <DirectLink>b__17_0(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLink>b__17_1(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__17_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLink>b__17_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "sku"));

            internal bool <Login>b__18_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_login"));

            internal bool <Login>b__18_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_login_securekey"));

            internal bool <Login>b__18_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cart-items-form"));

            internal bool <Search>b__19_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-tile"));

            internal bool <SubmitBilling>b__15_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"));

            internal bool <SubmitBilling>b__15_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_billing_securekey"));

            internal bool <SubmitOrder>b__14_1(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form")) && !string.IsNullOrEmpty(x.InnerText.Trim()));

            internal bool <SubmitShipping>b__16_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_singleshipping_securekey"));

            internal bool <SubmitShipping>b__16_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_singleshipping_shippingAddress"));

            internal bool <SubmitShipping>b__16_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_cart"));

            internal bool <SubmitShipping>b__16_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "checkout-form"));

            internal bool <SubmitShipping>b__16_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dwfrm_login_securekey"));

            internal bool <SubmitShipping>b__16_5(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "value"));

            internal bool <SubmitShipping>b__16_7(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "edit-address-form"));

            internal bool <SubmitShipping>b__16_8(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "edit-address-form"));

            internal bool <SubmitShipping>b__16_9(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_billing"));
        }

        [CompilerGenerated]
        private static class <>o__15
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, object>> <>p__11;
            public static CallSite<Action<CallSite, Dictionary<string, string>, string, object>> <>p__12;
        }

        [CompilerGenerated]
        private static class <>o__16
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
        }
    }
}

