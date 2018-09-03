namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Notifications;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using OpenQA.Selenium;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Solebox : IPlatform
    {
        private Client _client;
        private Client _client2;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _cnid;
        private string _aid;
        private string _parentid;
        private bool _isLoggedIn;
        private string _srr;
        private HtmlDocument _currentDoc;
        private Dictionary<string, string> _diData;

        public Solebox(TaskRunner runner, TaskObject task)
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
            if ((this._task.Payment == TaskObject.PaymentEnum.paypal) && (this._task.Driver == null))
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                return false;
            }
            try
            {
                bool flag = true;
                string url = "";
                if (this._task.Link.Contains("aid"))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.ATC, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            url = this._client2.Get(this._task.Link).Headers.First<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "Location")).Value.First<string>();
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
                    url = "https://www.solebox.com/en/cart/";
                }
                else
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("lang", "1");
                            this._diData.Add("cnid", this._cnid);
                            this._diData.Add("listtype", "list");
                            this._diData.Add("actcontrol", "details");
                            this._diData.Add("cl", "details");
                            this._diData.Add("aid", this._runner.PickedSize.Value.Value);
                            this._diData.Add("anid", this._parentid);
                            this._diData.Add("parentid", this._parentid);
                            this._diData.Add("panid", "");
                            this._diData.Add("fnc", "tobasket");
                            this._diData.Add("am", "1");
                            url = this._client2.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Headers.First<KeyValuePair<string, IEnumerable<string>>>(x => (x.Key == "Location")).Value.First<string>();
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
                }
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client2.Get(url).Text();
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
                if (((!this._srr.Contains("successfully added") && !this._srr.ToLowerInvariant().Contains("release queue")) && (!this._srr.ToLowerInvariant().Contains("in the queue") && !this._currentDoc.DocumentNode.Descendants("tr").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cartItem_1"))))) && !this._currentDoc.DocumentNode.Descendants("span").Any<HtmlNode>(x => (((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "countValue")) && (x.InnerText.Trim() == "1"))))
                {
                    if (this._srr.Contains("Shopping Cart is Empty"))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                if (this._task.Link.Contains("aid"))
                {
                    this._srr = this._client.Get("https://www.solebox.com/en/cart/").Text();
                    this._currentDoc.LoadHtml(this._srr);
                    HtmlNode node = this._currentDoc.DocumentNode.Descendants("tr").First<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cartItem_1"));
                    Product product1 = new Product {
                        ProductTitle = node.Descendants("img").First<HtmlNode>().Attributes["alt"].Value,
                        Link = node.Descendants("a").First<HtmlNode>().Attributes["href"].Value,
                        Price = node.Descendants("td").Last<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketCenter"))).InnerText.Trim()
                    };
                    this._runner.Product = product1;
                    this._task.ImgUrl = node.Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                    this._task.Size = node.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketSize"))).InnerText.Trim();
                    this._runner.PickedSize = new KeyValuePair<string, string>(this._task.Size, this._task.Link);
                    this._runner.Success.Size = this._runner.PickedSize.Value.Key;
                    this._runner.Success.Price = this._runner.Product.Price;
                    this._runner.Success.Link = this._runner.Product.Link;
                    this._runner.Success.ProductName = this._runner.Product.ProductTitle;
                    this._runner.Success.Store = EveAIO.Helpers.GetStoreUrl(this._task);
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
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception4)
            {
                this._runner.IsError = true;
                if (!exception4.Message.Contains("430") && ((exception4.InnerException == null) || !exception4.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception4, "", "");
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
            bool flag;
            try
            {
                if (this._task.Payment == TaskObject.PaymentEnum.paypal)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.OPENING_PAYPAL);
                    States.WriteLogger(this._task, States.LOGGER_STATES.OPENING_PAYPAL, null, "", "");
                    HttpResponseHeaders local1 = this._client.Get("https://www.solebox.com/en/cart/").TextHeaders().Value;
                    if (Global.SETTINGS.PayPalBeep)
                    {
                        EveAIO.Helpers.PlayBell();
                    }
                    this._runner.ForcedPaypal = true;
                    if (this._runner._notificator != null)
                    {
                        this._runner._notificator.Notify(Notificator.NotificationType.Paypal);
                    }
                    this._task.Driver.Manage().Cookies.DeleteAllCookies();
                    foreach (System.Net.Cookie cookie in this._client.Cookies.List())
                    {
                        DateTime? expiry = null;
                        this._task.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path, expiry));
                    }
                    this._task.Driver.Navigate().GoToUrl("https://www.solebox.com/index.php?lang=1&actcontrol=payment&cl=payment&fnc=validatepayment&paymentid=globalpaypal&userform=");
                    return true;
                }
                bool flag2 = true;
                string str = "";
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.solebox.com/en/cart/").Text();
                        this._currentDoc.LoadHtml(this._srr);
                        str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
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
                        flag2 = true;
                        continue;
                    }
                }
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("stoken", str);
                        this._diData.Add("lang", "1");
                        this._diData.Add("cl", "user");
                        this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
                        this._currentDoc.LoadHtml(this._srr);
                        str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
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
                        flag2 = true;
                        continue;
                    }
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                ProfileObject profile = this._runner.Profile;
                string firstName = this._runner.Profile.FirstName;
                string lastName = this._runner.Profile.LastName;
                string str4 = this._runner.Profile.Address1;
                string str5 = this._runner.Profile.Address2;
                string zip = this._runner.Profile.Zip;
                string city = this._runner.Profile.City;
                string phone = this._runner.Profile.Phone;
                string key = WebsitesInfo.SOLEBOX_COUNTRIES.First<KeyValuePair<string, string>>(x => (x.Value.ToLowerInvariant() == this._runner.Profile.Country.ToLowerInvariant())).Key;
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("stoken", str);
                        this._diData.Add("lang", "1");
                        this._diData.Add("actcontrol", "user");
                        this._diData.Add("cl", "user");
                        this._diData.Add("option", "");
                        this._diData.Add("fnc", "changeuser");
                        this._diData.Add("invadr[oxuser__oxsal]", "MRS");
                        this._diData.Add("invadr[oxuser__oxfname]", firstName);
                        this._diData.Add("invadr[oxuser__oxlname]", lastName);
                        this._diData.Add("invadr[oxuser__oxstreet]", str4);
                        this._diData.Add("invadr[oxuser__oxstreetnr]", str5);
                        this._diData.Add("invadr[oxuser__oxaddinfo]", str5);
                        this._diData.Add("invadr[oxuser__oxzip]", zip);
                        this._diData.Add("invadr[oxuser__oxcity]", city);
                        this._diData.Add("invadr[oxuser__oxcountryid]", key);
                        this._diData.Add("invadr[oxuser__oxstateid]", "");
                        this._diData.Add("invadr[oxuser__oxbirthdate][day]", "");
                        this._diData.Add("invadr[oxuser__oxbirthdate][month]", "");
                        this._diData.Add("invadr[oxuser__oxbirthdate][year]", "");
                        this._diData.Add("invadr[oxuser__oxfon]", phone);
                        if (!this._runner.Profile.SameBillingShipping)
                        {
                            string str11 = this._runner.Profile.FirstName;
                            string str15 = this._runner.Profile.LastName;
                            string str13 = this._runner.Profile.Address1;
                            string str14 = this._runner.Profile.Address2;
                            string str10 = this._runner.Profile.Zip;
                            string str17 = this._runner.Profile.City;
                            string str12 = this._runner.Profile.Phone;
                            string str16 = WebsitesInfo.SOLEBOX_COUNTRIES.First<KeyValuePair<string, string>>(x => (x.Value.ToLowerInvariant() == this._runner.Profile.Country.ToLowerInvariant())).Key;
                            this._diData.Add("blshowshipaddress", "1");
                            this._diData.Add("changeClass", "user");
                            this._diData.Add("oxaddressid", "1");
                            this._diData.Add("deladr[oxaddress__oxfname]", str11);
                            this._diData.Add("deladr[oxaddress__oxlname]", str15);
                            this._diData.Add("deladr[oxaddress__oxcompany]", "");
                            this._diData.Add("deladr[oxaddress__oxstreet]", str13);
                            this._diData.Add("deladr[oxaddress__oxstreetnr]", str14);
                            this._diData.Add("deladr[oxaddress__oxaddinfo]", str14);
                            this._diData.Add("deladr[oxaddress__oxzip]", str10);
                            this._diData.Add("deladr[oxaddress__oxcity]", str17);
                            this._diData.Add("deladr[oxaddress__oxcountryid]", str16);
                            this._diData.Add("deladr[oxaddress__oxstateid]", "");
                            this._diData.Add("deladr[oxaddress__oxfon]", str12);
                        }
                        else
                        {
                            this._diData.Add("blshowshipaddress", "0");
                            this._diData.Add("changeClass", "user");
                            this._diData.Add("oxaddressid", "-1");
                            this._diData.Add("deladr[oxaddress__oxfname]", "");
                            this._diData.Add("deladr[oxaddress__oxlname]", "");
                            this._diData.Add("deladr[oxaddress__oxcompany]", "");
                            this._diData.Add("deladr[oxaddress__oxstreet]", "");
                            this._diData.Add("deladr[oxaddress__oxstreetnr]", "");
                            this._diData.Add("deladr[oxaddress__oxaddinfo]", "");
                            this._diData.Add("deladr[oxaddress__oxzip]", "");
                            this._diData.Add("deladr[oxaddress__oxcity]", "");
                            this._diData.Add("deladr[oxaddress__oxcountryid]", "");
                            this._diData.Add("deladr[oxaddress__oxstateid]", "");
                            this._diData.Add("deladr[oxaddress__oxfon]", "");
                        }
                        this._diData.Add("userform", "");
                        this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
                        this._currentDoc.LoadHtml(this._srr);
                        str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
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
                        flag2 = true;
                        continue;
                    }
                }
                if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipCenter")))
                {
                    List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                    foreach (HtmlNode node in this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipCenter"))).Descendants("li"))
                    {
                        list.Add(new KeyValuePair<string, string>(node.Descendants("input").First<HtmlNode>().Attributes["value"].Value, node.Descendants("label").First<HtmlNode>().InnerText.Trim()));
                    }
                    KeyValuePair<string, string> pair2 = list[0];
                    string str18 = pair2.Key;
                    string s = "";
                    pair2 = list[0];
                    foreach (char ch in pair2.Value)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            s = s + ch.ToString();
                        }
                    }
                    foreach (KeyValuePair<string, string> pair3 in list)
                    {
                        string str21 = "";
                        foreach (char ch2 in pair3.Value)
                        {
                            if ((char.IsDigit(ch2) || (ch2 == '.')) || (ch2 == ','))
                            {
                                str21 = str21 + ch2.ToString();
                            }
                        }
                        if (double.Parse(str21, NumberStyles.Number, CultureInfo.InvariantCulture) < double.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture))
                        {
                            s = str21;
                            str18 = pair3.Key;
                        }
                    }
                    flag2 = true;
                    while (flag2)
                    {
                        flag2 = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("stoken", str);
                            this._diData.Add("lang", "1");
                            this._diData.Add("actcontrol", "payment");
                            this._diData.Add("cl", "payment");
                            this._diData.Add("fnc", "changeshipping");
                            this._diData.Add("shippingIsSet", "true");
                            this._diData.Add("sShipSet", str18);
                            this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
                            this._currentDoc.LoadHtml(this._srr);
                            str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
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
                            flag2 = true;
                            continue;
                        }
                    }
                }
                if (this._task.Payment == TaskObject.PaymentEnum.bankTransfer)
                {
                    if (this._srr.Contains("<b>Cash in advance</b>"))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                        States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                        flag2 = true;
                        while (flag2)
                        {
                            flag2 = false;
                            try
                            {
                                this._diData.Clear();
                                this._diData.Add("stoken", str);
                                this._diData.Add("lang", "1");
                                this._diData.Add("actcontrol", "payment");
                                this._diData.Add("cl", "payment");
                                this._diData.Add("fnc", "validatepayment");
                                this._diData.Add("paymentid", "oxidpayadvance");
                                this._diData.Add("userform", "");
                                this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
                                this._currentDoc.LoadHtml(this._srr);
                                str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
                                continue;
                            }
                            catch (WebException exception5)
                            {
                                if (exception5.Message.Contains("404") || exception5.Message.Contains("407"))
                                {
                                    throw;
                                }
                                States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                                Thread.Sleep(0x5dc);
                                flag2 = true;
                                continue;
                            }
                        }
                        string str22 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sDeliveryAddressMD5"))).Attributes["value"].Value;
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
                        this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                        States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
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
                            flag2 = true;
                            while (flag2)
                            {
                                flag2 = false;
                                try
                                {
                                    this._diData.Clear();
                                    this._diData.Add("stoken", str);
                                    this._diData.Add("lang", "1");
                                    this._diData.Add("actcontrol", "order");
                                    this._diData.Add("cl", "order");
                                    this._diData.Add("fnc", "execute");
                                    this._diData.Add("challenge", "");
                                    this._diData.Add("sDeliveryAddressMD5", str22);
                                    this._diData.Add("oxdownloadableproductsagreement", "0");
                                    this._diData.Add("oxserviceproductsagreement", "0");
                                    this._diData.Add("ord_agb", "1");
                                    this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
                                    this._currentDoc.LoadHtml(this._srr);
                                    str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
                                    continue;
                                }
                                catch (WebException exception6)
                                {
                                    if (exception6.Message.Contains("404") || exception6.Message.Contains("407"))
                                    {
                                        throw;
                                    }
                                    States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                                    Thread.Sleep(0x5dc);
                                    flag2 = true;
                                    continue;
                                }
                            }
                            if (this._srr.ToLowerInvariant().Contains("Thank you for your order".ToLowerInvariant()))
                            {
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
                    this._task.Status = States.GetTaskState(States.TaskState.CASH_IN_ADVANCE_NOT_AVAILABLE);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CASH_IN_ADVANCE_NOT_AVAILABLE, null, "", "");
                    return false;
                }
                if (!this._srr.Contains("<b>Credit Card</b>"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CREDITCART_NOT_AVAILABLE);
                    States.WriteLogger(this._task, States.LOGGER_STATES.CREDIT_CARD_NOT_AVAILABLE, null, "", "");
                    return false;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("stoken", str);
                        this._diData.Add("lang", "1");
                        this._diData.Add("actcontrol", "payment");
                        this._diData.Add("cl", "payment");
                        this._diData.Add("fnc", "validatepayment");
                        this._diData.Add("dynvalue[klarna_invoice_pclassid]", "-1");
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_gender]", "F");
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_fname]", profile.FirstName);
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_lname]", profile.LastName);
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_zip]", profile.Zip);
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_city]", profile.City);
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_available]", "1");
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_street]", profile.Address1);
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_houseno]", profile.Address2);
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_phone]", profile.Phone);
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_bday]", "");
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_bmonth]", "");
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_byear]", "");
                        this._diData.Add("dynvalue[klarna_module][klarna_invoice][-1][klarna_invoice_agree]", "0");
                        this._diData.Add("paymentid", "gs_kk_saferpay");
                        this._diData.Add("userform", "");
                        this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
                        this._currentDoc.LoadHtml(this._srr);
                        str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
                        continue;
                    }
                    catch (WebException exception7)
                    {
                        if (exception7.Message.Contains("404") || exception7.Message.Contains("407"))
                        {
                            throw;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                        Thread.Sleep(0x5dc);
                        flag2 = true;
                        continue;
                    }
                }
                string str23 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sDeliveryAddressMD5"))).Attributes["value"].Value;
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("stoken", str);
                        this._diData.Add("lang", "1");
                        this._diData.Add("actcontrol", "order");
                        this._diData.Add("cl", "order");
                        this._diData.Add("fnc", "execute");
                        this._diData.Add("challenge", "");
                        this._diData.Add("sDeliveryAddressMD5", str23);
                        this._diData.Add("ord_agb", "1");
                        this._diData.Add("oxdownloadableproductsagreement", "0");
                        this._diData.Add("oxserviceproductsagreement", "0");
                        this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
                        this._currentDoc.LoadHtml(this._srr);
                        continue;
                    }
                    catch (WebException exception8)
                    {
                        if (exception8.Message.Contains("404") || exception8.Message.Contains("407"))
                        {
                            throw;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                        Thread.Sleep(0x5dc);
                        flag2 = true;
                        continue;
                    }
                }
                string url = this._currentDoc.DocumentNode.Descendants("iframe").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "HPiFrame"))).Attributes["src"].Value;
                string uriString = "";
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.solebox.com/index.php?lang=1&");
                        KeyValuePair<string, string> pair4 = this._client.Get(url).TextResponseUri();
                        this._srr = pair4.Key;
                        uriString = pair4.Value;
                        this._currentDoc.LoadHtml(this._srr);
                        continue;
                    }
                    catch (WebException exception9)
                    {
                        if (exception9.Message.Contains("404") || exception9.Message.Contains("407"))
                        {
                            throw;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                        Thread.Sleep(0x5dc);
                        flag2 = true;
                        continue;
                    }
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_ORDER, null, "", "");
                string str26 = "";
                switch (profile.CardTypeId)
                {
                    case "1":
                        str26 = "1_Card&1091";
                        break;

                    case "2":
                        str26 = "1_Card&1090";
                        break;
                }
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("selectionId", str26);
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                        KeyValuePair<string, string> pair5 = this._client.Post(uriString, this._diData).TextResponseUri();
                        this._srr = pair5.Key;
                        uriString = pair5.Value;
                        this._currentDoc.LoadHtml(this._srr);
                        continue;
                    }
                    catch (WebException exception10)
                    {
                        if (exception10.Message.Contains("404") || exception10.Message.Contains("407"))
                        {
                            throw;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                        Thread.Sleep(0x5dc);
                        flag2 = true;
                        continue;
                    }
                }
                flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("CardNumber", profile.CCNumber);
                        this._diData.Add("ExpMonth", int.Parse(profile.ExpiryMonth).ToString());
                        this._diData.Add("ExpYear", profile.ExpiryYear);
                        this._diData.Add("HolderName", profile.NameOnCard);
                        this._diData.Add("VerificationCode", profile.Cvv);
                        this._diData.Add("SubmitToNext", "");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                        KeyValuePair<string, string> pair6 = this._client.Post(uriString, this._diData).TextResponseUri();
                        this._srr = pair6.Key;
                        uriString = pair6.Value;
                        this._currentDoc.LoadHtml(this._srr);
                        continue;
                    }
                    catch (WebException exception11)
                    {
                        if (exception11.Message.Contains("404") || exception11.Message.Contains("407"))
                        {
                            throw;
                        }
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                        Thread.Sleep(0x5dc);
                        flag2 = true;
                        continue;
                    }
                }
                if (this._srr.Contains("Invalid card number"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.INVALID_CREDIT_CARD);
                    States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_CREDIT_CARD, null, "", "");
                    return false;
                }
                if (!uriString.ToLowerInvariant().Contains("error"))
                {
                    url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "SendToAcs"))).Attributes["action"].Value;
                    string str29 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                    string str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                    string str28 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"))).Attributes["value"].Value;
                    flag2 = true;
                    while (flag2)
                    {
                        flag2 = false;
                        try
                        {
                            this._diData.Clear();
                            this._diData.Add("MD", str29);
                            this._diData.Add("PaReq", str27);
                            this._diData.Add("TermUrl", str28);
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                            KeyValuePair<string, string> pair7 = this._client.Post(url, this._diData).TextResponseUri();
                            this._srr = pair7.Key;
                            uriString = pair7.Value;
                            this._currentDoc.LoadHtml(this._srr);
                            continue;
                        }
                        catch (WebException exception12)
                        {
                            if (exception12.Message.Contains("404") || exception12.Message.Contains("407"))
                            {
                                throw;
                            }
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1.5");
                            Thread.Sleep(0x5dc);
                            flag2 = true;
                            continue;
                        }
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
                    this._currentDoc.LoadHtml(this._srr);
                    if (!this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"))))
                    {
                        str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                        str29 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                        url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                        this._diData.Clear();
                        this._diData.Add("PaRes", str27);
                        this._diData.Add("MD", str29);
                        KeyValuePair<string, string> pair9 = this._client.Post(url, this._diData).TextResponseUri();
                        uriString = pair9.Value;
                        this._srr = pair9.Key;
                        this._currentDoc.LoadHtml(this._srr);
                        url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                        pair9 = this._client.Get(url).TextResponseUri();
                        uriString = pair9.Value;
                        this._srr = pair9.Key;
                    }
                    else
                    {
                        str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                        str29 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                        string text1 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                        string str35 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"))).Attributes["value"].Value;
                        string str38 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"))).Attributes["value"].Value;
                        string str31 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"))).Attributes["value"].Value;
                        string str32 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"))).Attributes["value"].Value;
                        string str30 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"))).Attributes["value"].Value;
                        string str33 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"))).Attributes["value"].Value;
                        string str34 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"))).Attributes["value"].Value;
                        string str37 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"))).Attributes["value"].Value;
                        string str36 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"))).Attributes["value"].Value;
                        url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"))).Attributes["action"].Value;
                        this._diData.Clear();
                        this._diData.Add("PaRes", str27);
                        this._diData.Add("PaReq", str27);
                        this._diData.Add("ABSlog", str35);
                        this._diData.Add("deviceDNA", str38);
                        this._diData.Add("executionTime", str31);
                        this._diData.Add("dnaError", str32);
                        this._diData.Add("mesc", str30);
                        this._diData.Add("mescIterationCount", str33);
                        this._diData.Add("desc", str34);
                        this._diData.Add("isDNADone", str37);
                        this._diData.Add("arcotFlashCookie", str36);
                        KeyValuePair<string, string> pair8 = this._client.Post(url, this._diData).TextResponseUri();
                        uriString = pair8.Value;
                        this._srr = pair8.Key;
                        this._currentDoc.LoadHtml(this._srr);
                        url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value;
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(uriString);
                        pair8 = this._client.Get(url).TextResponseUri();
                        uriString = pair8.Value;
                        this._srr = pair8.Key;
                    }
                    this._currentDoc.LoadHtml(this._srr);
                    if (uriString.ToLowerInvariant().Contains("error"))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        return false;
                    }
                    if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "validation-summary-errors"))) && this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "validation-summary-errors"))).Descendants("li").Any<HtmlNode>())
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                        return false;
                    }
                    try
                    {
                        EveAIO.Helpers.AddDbValue("Solebox|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(uriString + "|" + this._srr)));
                    }
                    catch
                    {
                    }
                    return true;
                }
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                flag = false;
            }
            catch (ThreadAbortException)
            {
                flag = false;
            }
            catch (Exception exception13)
            {
                this._runner.IsError = true;
                if (!exception13.Message.Contains("430") && ((exception13.InnerException == null) || !exception13.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception13, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                flag = false;
            }
            finally
            {
                this._isLoggedIn = false;
                this._runner.Cookies = new CookieContainer();
            }
            return flag;
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
                this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                this._srr = this._client2.Get(link).Text();
                if (this._srr.ToLowerInvariant().Contains("release queue") || this._srr.ToLowerInvariant().Contains("in the queue"))
                {
                    this._srr = this._client.Get(link + "?redirected=1&qitq=0").Text();
                }
                this._currentDoc.LoadHtml(this._srr);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sizeBlock")))
                {
                    if (!this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "release"))))
                    {
                        HtmlNode node = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sizeBlock"));
                        if (node.Descendants("div").Any<HtmlNode>(x => (x.Attributes["class"] != null) && !x.Attributes["class"].Value.Contains("inactive")))
                        {
                            string str = EveAIO.Helpers.RemoveHtmlTags(this._runner.ProductPageHtml.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productTitle"))).InnerText.Trim());
                            string str2 = "";
                            try
                            {
                                str2 = this._srr.Substring(this._srr.IndexOf("ecomm_totalvalue"));
                                str2 = str2.Substring(str2.IndexOf(":") + 1);
                                str2 = str2.Substring(0, str2.IndexOf(",")).Trim();
                            }
                            catch
                            {
                                str2 = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productPrice"))).InnerText.Trim();
                            }
                            string str3 = this._runner.ProductPageHtml.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "morePicsContainer"))).Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                            this._task.ImgUrl = str3;
                            Product product1 = new Product {
                                ProductTitle = str,
                                Link = link,
                                Price = str2
                            };
                            this._runner.Product = product1;
                            using (IEnumerator<HtmlNode> enumerator = (from x in node.Descendants("div")
                                where (x.Attributes["class"] != null) && !x.Attributes["class"].Value.Contains("inactive")
                                select x).GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    HtmlNode node2 = enumerator.Current.Descendants("a").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "selectSize"));
                                    string str4 = node2.Attributes["data-size-us"].Value;
                                    if (string.IsNullOrEmpty(str4))
                                    {
                                        str4 = node2.Attributes["data-size-original"].Value;
                                    }
                                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(str4, node2.Attributes["id"].Value);
                                    this._runner.Product.AvailableSizes.Add(item);
                                }
                            }
                            this._cnid = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "cnid"))).Attributes["value"].Value;
                            this._aid = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "aid"))).Attributes["value"].Value;
                            this._parentid = this._runner.ProductPageHtml.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "parentid"))).Attributes["value"].Value;
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
                                            KeyValuePair<string, string> current;
                                            while (enumerator2.MoveNext())
                                            {
                                                int num6;
                                                current = enumerator2.Current;
                                                List<string> source = new List<string>();
                                                if (!current.Key.Contains(":"))
                                                {
                                                    goto Label_081D;
                                                }
                                                char[] chArray2 = new char[] { ':' };
                                                string[] strArray3 = current.Key.Split(chArray2);
                                                int index = 0;
                                                goto Label_0813;
                                            Label_07FC:
                                                source.Add(strArray3[index].Trim());
                                                index++;
                                            Label_0813:
                                                if (index >= strArray3.Length)
                                                {
                                                    goto Label_082B;
                                                }
                                                goto Label_07FC;
                                            Label_081D:
                                                source.Add(current.Key);
                                            Label_082B:
                                                num6 = 0;
                                                while (num6 < source.Count)
                                                {
                                                    source[num6] = source[num6].Trim().ToUpperInvariant();
                                                    num6++;
                                                }
                                                if (source.Any<string>(x => x == sz))
                                                {
                                                    goto Label_0898;
                                                }
                                            }
                                            continue;
                                        Label_0898:
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
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        return false;
                    }
                    Global.Logger.Info($"['{this._task.Name} - {this._task.Guid}']: Product out of stock");
                    return false;
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
                return false;
            }
        }

        public bool Login()
        {
            if ((this._task.Payment == TaskObject.PaymentEnum.paypal) && (this._task.Driver == null))
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.BROWSER_MISSING, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.BROWSER_MISSING);
                return false;
            }
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
                        this._srr = this._client.Get("https://www.solebox.com/en/my-account/").Text();
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
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._currentDoc.LoadHtml(this._srr);
                        string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
                        this._diData.Clear();
                        this._diData.Add("stoken", str);
                        this._diData.Add("lang", "1");
                        this._diData.Add("listtype", "");
                        this._diData.Add("actcontrol", "account");
                        this._diData.Add("fnc", "login_noredirect");
                        this._diData.Add("cl", "account");
                        this._diData.Add("lgn_usr", this._task.Username);
                        this._diData.Add("lgn_pwd", this._task.Password);
                        this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
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
                if (this._srr.Contains("<strong>Welcome"))
                {
                    this._isLoggedIn = true;
                    States.WriteLogger(this._task, States.LOGGER_STATES.LOGIN_SUCCESSFUL, null, "", "");
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._srr = this._client.Get("https://www.solebox.com/en/cart/").Text();
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
                    if (this._currentDoc.DocumentNode.Descendants("input").Any<HtmlNode>(x => (x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.CLEANING_CART);
                        States.WriteLogger(this._task, States.LOGGER_STATES.CLEARING_CART, null, "", "");
                        string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"))).Attributes["value"].Value;
                        if (this._currentDoc.DocumentNode.Descendants("tr").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketItem")))
                        {
                            this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                            foreach (HtmlNode local1 in from x in this._currentDoc.DocumentNode.Descendants("tr")
                                where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketItem")
                                select x)
                            {
                                string str3 = local1.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("[basketitemid]"))).Attributes["value"].Value;
                                string str4 = local1.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("[aid]"))).Attributes["value"].Value;
                                flag = true;
                                while (flag)
                                {
                                    flag = false;
                                    try
                                    {
                                        this._diData.Clear();
                                        this._diData.Add("stoken", str2);
                                        this._diData.Add("lang", "1");
                                        this._diData.Add("cl", "basket");
                                        this._diData.Add("fnc", "changebasket");
                                        this._diData.Add("CustomError", "basket");
                                        this._diData.Add("aproducts[" + str3 + "][aid]", str4);
                                        this._diData.Add("aproducts[" + str3 + "][basketitemid]", str3);
                                        this._diData.Add("aproducts[" + str3 + "][override]", "1");
                                        this._diData.Add("aproducts[" + str3 + "][am]", "0");
                                        this._srr = this._client.Post("https://www.solebox.com/index.php?lang=1&", this._diData).Text();
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
                    }
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
                this._runner.IsError = true;
                if (!exception5.Message.Contains("430") && ((exception5.InnerException == null) || !exception5.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_LOGGING_IN, exception5, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
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
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.solebox.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.solebox.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
            this._client2 = new Client((this._client == null) ? this._runner.Cookies : this._client.Cookies, this._runner.Proxy, false);
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.solebox.com");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.solebox.com");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            this._client2.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Solebox.<>c <>9;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__13_2;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__13_3;
            public static Func<HtmlNode, bool> <>9__13_0;
            public static Func<HtmlNode, bool> <>9__13_1;
            public static Func<HtmlNode, bool> <>9__13_4;
            public static Func<HtmlNode, bool> <>9__13_7;
            public static Func<HtmlNode, bool> <>9__13_5;
            public static Func<HtmlNode, bool> <>9__14_0;
            public static Func<HtmlNode, bool> <>9__14_1;
            public static Func<HtmlNode, bool> <>9__14_3;
            public static Func<HtmlNode, bool> <>9__14_4;
            public static Func<HtmlNode, bool> <>9__14_6;
            public static Func<HtmlNode, bool> <>9__14_7;
            public static Func<HtmlNode, bool> <>9__14_8;
            public static Func<HtmlNode, bool> <>9__14_9;
            public static Func<HtmlNode, bool> <>9__14_11;
            public static Func<HtmlNode, bool> <>9__14_12;
            public static Func<HtmlNode, bool> <>9__14_13;
            public static Func<HtmlNode, bool> <>9__14_14;
            public static Func<HtmlNode, bool> <>9__14_15;
            public static Func<HtmlNode, bool> <>9__14_16;
            public static Func<HtmlNode, bool> <>9__14_17;
            public static Func<HtmlNode, bool> <>9__14_18;
            public static Func<HtmlNode, bool> <>9__14_19;
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
            public static Func<HtmlNode, bool> <>9__14_20;
            public static Func<HtmlNode, bool> <>9__14_21;
            public static Func<HtmlNode, bool> <>9__15_0;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__15_2;
            public static Func<HtmlNode, bool> <>9__15_3;
            public static Func<HtmlNode, bool> <>9__15_4;
            public static Func<HtmlNode, bool> <>9__15_5;
            public static Func<HtmlNode, bool> <>9__15_6;
            public static Func<HtmlNode, bool> <>9__15_10;
            public static Func<HtmlNode, bool> <>9__15_11;
            public static Func<HtmlNode, bool> <>9__15_7;
            public static Func<HtmlNode, bool> <>9__15_8;
            public static Func<HtmlNode, bool> <>9__15_9;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__16_0;
            public static Func<HtmlNode, bool> <>9__16_2;
            public static Func<HtmlNode, bool> <>9__16_3;
            public static Func<HtmlNode, bool> <>9__16_4;
            public static Func<HtmlNode, bool> <>9__16_5;
            public static Func<HtmlNode, bool> <>9__16_6;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Solebox.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__13_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cartItem_1"));

            internal bool <Atc>b__13_1(HtmlNode x) => 
                (((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "countValue")) && (x.InnerText.Trim() == "1"));

            internal bool <Atc>b__13_2(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <Atc>b__13_3(KeyValuePair<string, IEnumerable<string>> x) => 
                (x.Key == "Location");

            internal bool <Atc>b__13_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "cartItem_1"));

            internal bool <Atc>b__13_5(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketSize"));

            internal bool <Atc>b__13_7(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketCenter"));

            internal bool <Checkout>b__14_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Checkout>b__14_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Checkout>b__14_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Checkout>b__14_12(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Checkout>b__14_13(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sDeliveryAddressMD5"));

            internal bool <Checkout>b__14_14(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "HPiFrame"));

            internal bool <Checkout>b__14_15(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "SendToAcs"));

            internal bool <Checkout>b__14_16(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <Checkout>b__14_17(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <Checkout>b__14_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "TermUrl"));

            internal bool <Checkout>b__14_19(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"));

            internal bool <Checkout>b__14_20(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "validation-summary-errors"));

            internal bool <Checkout>b__14_21(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "validation-summary-errors"));

            internal bool <Checkout>b__14_22(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <Checkout>b__14_23(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <Checkout>b__14_24(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <Checkout>b__14_25(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <Checkout>b__14_26(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <Checkout>b__14_27(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "ABSlog"));

            internal bool <Checkout>b__14_28(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "deviceDNA"));

            internal bool <Checkout>b__14_29(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "executionTime"));

            internal bool <Checkout>b__14_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Checkout>b__14_30(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "dnaError"));

            internal bool <Checkout>b__14_31(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mesc"));

            internal bool <Checkout>b__14_32(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "mescIterationCount"));

            internal bool <Checkout>b__14_33(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "desc"));

            internal bool <Checkout>b__14_34(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "isDNADone"));

            internal bool <Checkout>b__14_35(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "arcotFlashCookie"));

            internal bool <Checkout>b__14_36(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "downloadForm"));

            internal bool <Checkout>b__14_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipCenter"));

            internal bool <Checkout>b__14_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "shipCenter"));

            internal bool <Checkout>b__14_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Checkout>b__14_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Checkout>b__14_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "sDeliveryAddressMD5"));

            internal bool <DirectLink>b__15_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sizeBlock"));

            internal bool <DirectLink>b__15_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "release"));

            internal bool <DirectLink>b__15_10(HtmlNode x) => 
                ((x.Attributes["class"] != null) && !x.Attributes["class"].Value.Contains("inactive"));

            internal bool <DirectLink>b__15_11(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "selectSize"));

            internal bool <DirectLink>b__15_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "sizeBlock"));

            internal bool <DirectLink>b__15_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && !x.Attributes["class"].Value.Contains("inactive"));

            internal bool <DirectLink>b__15_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productTitle"));

            internal bool <DirectLink>b__15_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "productPrice"));

            internal bool <DirectLink>b__15_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "morePicsContainer"));

            internal bool <DirectLink>b__15_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "cnid"));

            internal bool <DirectLink>b__15_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "aid"));

            internal bool <DirectLink>b__15_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "parentid"));

            internal bool <Login>b__16_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Login>b__16_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Login>b__16_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "stoken"));

            internal bool <Login>b__16_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketItem"));

            internal bool <Login>b__16_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "basketItem"));

            internal bool <Login>b__16_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("[basketitemid]"));

            internal bool <Login>b__16_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && x.Attributes["name"].Value.Contains("[aid]"));
        }
    }
}

