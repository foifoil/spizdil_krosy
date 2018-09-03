namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Hibbett : IPlatform
    {
        private Client _client;
        private TaskRunner _runner;
        private TaskObject _task;
        private string _encryptedCvv;
        private string _encryptedCc;
        private bool _encryptionRunning;
        private string _srr;
        private HtmlDocument _currentDoc;
        private string _token;
        private Dictionary<string, string> _diData;
        private string _color;

        public Hibbett(TaskRunner runner, TaskObject task)
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
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri(this._runner.Product.Link);
                        this._srr = this._client.Get(this._runner.PickedSize.Value.Value + ((string.IsNullOrEmpty(this._color) || this._runner.PickedSize.Value.Value.Contains("color")) ? "" : ("&" + this._color)) + "&Quantity=1&format=ajax&productlistid=undefined").Text();
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
                this._token = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"))).Attributes["value"].Value;
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pid"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("select-Quantity", "1");
                        this._diData.Add("Quantity", "1");
                        this._diData.Add("cartAction", "add");
                        this._diData.Add("pid", str);
                        this._diData.Add("csrf_token", this._token);
                        this._srr = this._client.Post("https://www.hibbett.com/on/demandware.store/Sites-Hibbett-US-Site/default/Cart-AddProduct?format=ajax", this._diData).Text();
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
                if (!this._srr.Contains("This item is currently not available."))
                {
                    if (this._currentDoc.DocumentNode.Descendants("span").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "minicart-quantity")) && (x.InnerHtml.Trim() == "1")))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                        return true;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                    return false;
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
            switch (((0x7e8b1815 ^ 0x3ef7ce59) % 5))
            {
                case 0:
                    goto Label_001C;

                case 2:
                    return false;

                case 3:
                    return false;

                case 4:
                    break;

                default:
                    return this.SubmitOrder();
            }
        Label_004B:
            if (!this.SubmitBilling())
            {
            }
            if (0x5bd0f6db || true)
            {
            }
            goto Label_001C;
        }

        public bool DirectLink(string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                if ((string.IsNullOrEmpty(this._encryptedCc) || string.IsNullOrEmpty(this._encryptedCvv)) && !this._encryptionRunning)
                {
                    Task.Factory.StartNew(delegate {
                        Tuple<string, string> tuple = Global.SENSOR.EncryptBraintree(this._runner.Profile.CCNumber, this._runner.Profile.Cvv);
                        this._encryptedCc = tuple.Item1;
                        this._encryptedCvv = tuple.Item2;
                    });
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
                if (this._srr.Contains("Unfortunately at this time"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.US_IP_NEEDED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.US_IP_NEEDED, null, "", "");
                    return false;
                }
                if (this._srr.Contains("data-sitekey"))
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.HIBBET_CAPTCHA_KEY, this._task.Link, "Hibbett");
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
                    Cookie cookie = new Cookie {
                        Name = "_pxCaptcha",
                        Value = waiter.Token,
                        Domain = "hibbett.com"
                    };
                    this._client.Handler.CookieContainer.Add(cookie);
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._srr.Contains("var productLaunchDate"))
                {
                    string str2 = this._srr.Substring(this._srr.IndexOf("var productLaunchDate"));
                    str2 = str2.Substring(str2.IndexOf("'") + 1);
                    DateTime time2 = DateTime.ParseExact(str2.Substring(0, str2.IndexOf("'")), "ddd MMM dd HH:mm:ss 'GMT'K yyyy", CultureInfo.InvariantCulture);
                    if (time2.ToLocalTime().AddSeconds(-20.0) > DateTime.Now.ToLocalTime().ToLocalTime())
                    {
                        this._task.State = TaskObject.StateEnum.scheduled;
                        this._task.IsStartScheduled = true;
                        this._task.ScheduleStart = time2.ToLocalTime().AddSeconds(-20.0);
                        States.WriteLogger(this._task, States.LOGGER_STATES.SCHEDULLING, null, "", this._task.ScheduleStart.ToString("MM-dd-yyyy HH:mm:ss tt", CultureInfo.InvariantCulture));
                        return false;
                    }
                }
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                if (!this._currentDoc.DocumentNode.Descendants("ul").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches size"))))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                string str = WebUtility.HtmlDecode(this._runner.ProductPageHtml.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim());
                string str3 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"))).InnerText.Trim();
                string str4 = "http://eve-robotics.com/dummy_product.png";
                try
                {
                    string str5 = this._srr.Substring(this._srr.IndexOf("set:"));
                    str5 = str5.Substring(str5.IndexOf("'") + 1);
                    str5 = str5.Substring(0, str5.IndexOf("'"));
                    str4 = $"https://i1.adis.ws/i/hibbett/{str5}_right2?w=580&h=580&fmt=jpg&bg=rgb(255,255,255)&img404=404&v=0";
                }
                catch
                {
                }
                this._task.ImgUrl = str4;
                Product product1 = new Product {
                    ProductTitle = str,
                    Link = link,
                    Price = str3
                };
                this._runner.Product = product1;
                if (this._currentDoc.DocumentNode.Descendants("ul").Any<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches color")))
                {
                    HtmlNode node = this._currentDoc.DocumentNode.Descendants("ul").First<HtmlNode>(x => (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches color"));
                    if (node.Descendants("a").Any<HtmlNode>())
                    {
                        string str6 = node.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                        this._color = str6.Substring(str6.IndexOf("&amp;") + 5);
                    }
                }
                foreach (HtmlNode node2 in from x in this._currentDoc.DocumentNode.Descendants("ul").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches size"))).Descendants("li")
                    where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "selectable")
                    select x)
                {
                    string key = node2.InnerText.Trim().Replace("size", "").Trim();
                    KeyValuePair<string, string> item = new KeyValuePair<string, string>(key, node2.Descendants("a").First<HtmlNode>().Attributes["href"].Value.Replace("amp;", ""));
                    this._runner.Product.AvailableSizes.Add(item);
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
                    string str8 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str8 = str8 + ch.ToString();
                        }
                    }
                    double num4 = double.Parse(str8.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
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
                            KeyValuePair<string, string> pair2;
                            goto Label_0B12;
                        Label_0A42:
                            pair2 = enumerator2.Current;
                            List<string> source = new List<string>();
                            if (!pair2.Key.Contains(":"))
                            {
                                source.Add(pair2.Key);
                            }
                            else
                            {
                                char[] chArray2 = new char[] { ':' };
                                string[] strArray3 = pair2.Key.Split(chArray2);
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
                                goto Label_0B20;
                            }
                        Label_0B12:
                            if (!enumerator2.MoveNext())
                            {
                                continue;
                            }
                            goto Label_0A42;
                        Label_0B20:
                            this._runner.PickedSize = new KeyValuePair<string, string>?(pair2);
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
            this._client.SetDesktopAgent();
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.hibbett.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.hibbett.com");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            this._client.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        private bool SubmitBilling()
        {
            try
            {
                string str8;
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                while (string.IsNullOrEmpty(this._encryptedCc))
                {
                    continue;
                Label_0030:
                    if (!string.IsNullOrEmpty(this._encryptedCvv))
                    {
                        goto Label_004C;
                    }
                }
                goto Label_0030;
            Label_004C:
                str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "radial-nonce"))).Attributes["value"].Value.ToString();
                string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "radial-token"))).Attributes["value"].Value.ToString();
                string str3 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "numberOfAdditionalTokenizatioRetries"))).Attributes["value"].Value.ToString();
                string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "additionalTokenizatioRetriesErroLevel"))).Attributes["value"].Value.ToString();
                string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "resetRetries"))).Attributes["value"].Value.ToString();
                string str = "";
                switch (this._runner.Profile.CardTypeId)
                {
                    case "0":
                        str = "Amex";
                        break;

                    case "1":
                        str = "Visa";
                        break;

                    case "2":
                        str = "Master Card";
                        break;

                    case "3":
                        str = "Discover";
                        break;
                }
                string str6 = "";
                string cvv = this._runner.Profile.Cvv;
                for (int i = 0; i < cvv.Length; i++)
                {
                    char ch1 = cvv[i];
                    str6 = str6 + "*";
                }
                string str2 = "";
                for (int j = 0; j < (this._runner.Profile.CCNumber.Length - 4); j++)
                {
                    str2 = str2 + "*";
                }
                str2 = str2 + this._runner.Profile.CCNumber.Substring(this._runner.Profile.CCNumber.Length - 4);
                ProfileObject profile = this._runner.Profile;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_firstName", profile.FirstName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_lastName", profile.LastName);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_address1", profile.Address1);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_address2", profile.Address2);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_city", profile.City);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_states_state", profile.StateId);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_postal", profile.Zip);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_country", "US");
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_phone", profile.Phone);
                        this._diData.Add("dwfrm_billing_billingAddress_addressFields_radialAVSValidated", "true");
                        this._diData.Add("dwfrm_billing_billingAddress_email_emailAddress", profile.Email);
                        this._diData.Add("dwfrm_billing_billingAddress_addToEmailList", "true");
                        this._diData.Add("dwfrm_billing_couponCode", "");
                        this._diData.Add("dwfrm_billing_giftCertCode", "");
                        this._diData.Add("dwfrm_billing_paymentMethods_selectedPaymentMethodID", "CREDIT_CARD");
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_owner", profile.NameOnCard);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_type", str);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_number", str2);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_encryptedNumber", this._encryptedCc);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_encryptedCVV", this._encryptedCvv);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_expiration_month", int.Parse(profile.ExpiryMonth).ToString());
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_expiration_year", profile.ExpiryYear);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_cvn", str6);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_radialNonce", str8);
                        this._diData.Add("dwfrm_billing_paymentMethods_creditCard_radialToken", str7);
                        this._diData.Add("numberOfAdditionalTokenizatioRetries", str9);
                        this._diData.Add("additionalTokenizatioRetriesErroLevel", str5);
                        this._diData.Add("resetRetries", str3);
                        this._diData.Add("dwfrm_profile_customer_firstname", "");
                        this._diData.Add("dwfrm_profile_customer_lastname", "");
                        this._diData.Add("dwfrm_profile_customer_email", "");
                        this._diData.Add("dwfrm_billing_password", "");
                        this._diData.Add("dwfrm_billing_passwordconfirm", "");
                        this._diData.Add("dwfrm_billing_save", "Continue to Review");
                        this._diData.Add("csrf_token", this._token);
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.hibbett.com/billing");
                        this._srr = this._client.Post("https://www.hibbett.com/review", this._diData).Text();
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
                if (!this._currentDoc.DocumentNode.Descendants("button").Any<HtmlNode>(x => ((x.Attributes["value"] != null) && (x.Attributes["value"].Value == "Place Order"))))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.INVALID_CREDIT_CARD);
                    States.WriteLogger(this._task, States.LOGGER_STATES.INVALID_CREDIT_CARD, null, "", "");
                    return false;
                }
                this._token = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"))).Attributes["value"].Value;
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
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
                using (List<Cookie>.Enumerator enumerator = this._client.Handler.CookieContainer.List().GetEnumerator())
                {
                    Cookie cookie;
                    goto Label_0064;
                Label_0049:
                    cookie = enumerator.Current;
                    if (cookie.Name == "rdf-uuid")
                    {
                        goto Label_006F;
                    }
                Label_0064:
                    if (!enumerator.MoveNext())
                    {
                        goto Label_0086;
                    }
                    goto Label_0049;
                Label_006F:
                    string text1 = cookie.Value;
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
                            string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "radialToken"))).Attributes["value"].Value;
                            this._diData.Clear();
                            this._diData.Add("det1", "TF1;015;;;;;;;;;;;;;;;;;;;;;;Mozilla;Netscape;5.0%20%28Windows%20NT%2010.0%3B%20Win64%3B%20x64%29%20AppleWebKit/537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome/67.0.3396.87%20Safari/537.36;20030107;undefined;true;;true;Win32;undefined;Mozilla/5.0%20%28Windows%20NT%2010.0%3B%20Win64%3B%20x64%29%20AppleWebKit/537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome/67.0.3396.87%20Safari/537.36;sk-SK;undefined;www.hibbett.com;undefined;undefined;undefined;undefined;true;true;1529511919977;-8;6/7/2005%2C%209%3A33%3A44%20PM;1920;975;;;;;;;14;480;420;6/20/2018%2C%209%3A25%3A19%20AM;24;1920;935;0;0;;;;;;;;;;;;;;;;;;;14;");
                            this._diData.Add("acceptCharset", "UTF-8");
                            this._diData.Add("csrf_token", this._token);
                            this._diData.Add("encryptedCardCVV", this._encryptedCvv);
                            this._diData.Add("encryptedCardNumber", this._encryptedCc);
                            this._diData.Add("radialToken", str);
                            this._diData.Add("RDFUID", "");
                            this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.hibbett.com/review");
                            this._srr = this._client.Post("https://www.hibbett.com/order-confirmation", this._diData).Text();
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
                    if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "checkout-mini-cart")) && x.InnerText.Contains("This item is currently not available.")))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.PRODUCT_OOS);
                        return false;
                    }
                    if (this._currentDoc.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form"))) && !string.IsNullOrEmpty(this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form"))).InnerText.Trim()))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CARD_DECLINED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
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
                            EveAIO.Helpers.AddDbValue("Hibbett|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this._srr)));
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
                ProfileObject profile = this._runner.Profile;
                string str7 = profile.EmailShipping.Trim();
                string str8 = profile.FirstNameShipping.Trim();
                string str9 = profile.LastNameShipping.Trim();
                string str = profile.Address1Shipping.Trim();
                string str2 = profile.Address2Shipping.Trim();
                string str6 = profile.CityShipping.Trim();
                string str5 = profile.ZipShipping.Trim();
                string str4 = profile.CountryIdShipping.ToLowerInvariant();
                string stateIdShipping = "";
                if ((str4 != "us") && (str4 != "ca"))
                {
                    stateIdShipping = profile.StateShipping;
                }
                else
                {
                    stateIdShipping = profile.StateIdShipping;
                }
                string str10 = profile.PhoneShipping.Trim();
                string stateShipping = profile.StateShipping;
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get("https://www.hibbett.com/checkout").Text();
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
                if (this._srr.Contains("data-sitekey"))
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Add(this._task);
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_CATPCHA);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_CAPTCHA, null, "", "");
                    this._task.Mre = new ManualResetEvent(false);
                    CaptchaWaiter waiter = new CaptchaWaiter(this._task, new DateTime?(DateTime.Now), WebsitesInfo.HIBBET_CAPTCHA_KEY, this._task.Link, "Hibbett");
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
                    Cookie cookie = new Cookie {
                        Name = "_pxCaptcha",
                        Value = waiter.Token,
                        Domain = "hibbett.com"
                    };
                    this._client.Handler.CookieContainer.Add(cookie);
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            this._srr = this._client.Get("https://www.hibbett.com/checkout").Text();
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
                    this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_SHIPPING, null, "", "");
                }
                this._currentDoc.LoadHtml(this._srr);
                string url = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_oauthlogin"))).Attributes["action"].Value;
                this._token = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"))).Attributes["value"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_login_unregistered", "Checkout as Guest");
                        this._diData.Add("csrf_token", this._token);
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.hibbett.com/checkout");
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
                this._token = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"))).Attributes["value"].Value;
                url = "https://www.hibbett.com/on/demandware.store/Sites-Hibbett-US-Site/default/COShipping-GetApplicableShippingMethodsJSON?address1={0}&address2={1}&countryCode=US&stateCode={2}&postalCode={3}&city={4}";
                url = string.Format(url, new object[] { WebUtility.UrlEncode(str), WebUtility.UrlEncode(str2), stateIdShipping, WebUtility.UrlEncode(str5), WebUtility.UrlEncode(str6) });
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.hibbett.com/shipping");
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
                url = "https://www.hibbett.com/on/demandware.store/Sites-Hibbett-US-Site/default/RadialAVS-GetResultCode?address1={0}&address2={1}&countryCode=US&stateCode={2}&postalCode={3}&city={4}";
                url = string.Format(url, new object[] { WebUtility.UrlEncode(str), WebUtility.UrlEncode(str2), stateIdShipping, WebUtility.UrlEncode(str5), WebUtility.UrlEncode(str6) });
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._srr = this._client.Get(url).Text();
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
                this._client.Get("https://www.hibbett.com/on/demandware.store/Sites-Hibbett-US-Site/default/COBilling-UpdateSummary").Text();
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        this._diData.Clear();
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_email_emailAddress", str7);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_firstName", str8);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_lastName", str9);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_address1", str);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_address2", str2);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_city", str6);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_states_state", stateIdShipping);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_postal", str5);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_country", "US");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_phone", str10);
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_addressFields_radialAVSValidated", "true");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_useAsBillingAddress", "true");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_shippingMethodID", "ANY_GND");
                        this._diData.Add("dwfrm_singleshipping_shippingAddress_save", "Continue to Billing");
                        this._diData.Add("enableExcludeDropShipVendorFromShippingToPO", "true");
                        this._diData.Add("csrf_token", this._token);
                        this._client.Session.DefaultRequestHeaders.ExpectContinue = false;
                        this._client.Session.DefaultRequestHeaders.Referrer = new Uri("https://www.hibbett.com/shipping");
                        this._client.Session.DefaultRequestHeaders.Remove("X-Requested-With");
                        this._srr = this._client.Post("https://www.hibbett.com/billing", this._diData).Text();
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
                this._currentDoc.LoadHtml(this._srr);
                this._token = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"))).Attributes["value"].Value;
                return true;
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
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, null, "", "Web request timed out");
                }
                else if (!exception7.Message.Contains("430") && ((exception7.InnerException == null) || !exception7.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR_SHIPPING);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_SHIPPING, exception7, "", "");
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
            public static readonly Hibbett.<>c <>9;
            public static Func<HtmlNode, bool> <>9__13_0;
            public static Func<HtmlNode, bool> <>9__13_1;
            public static Func<HtmlNode, bool> <>9__13_2;
            public static Func<HtmlNode, bool> <>9__15_4;
            public static Func<HtmlNode, bool> <>9__15_1;
            public static Func<HtmlNode, bool> <>9__15_2;
            public static Func<HtmlNode, bool> <>9__15_3;
            public static Func<HtmlNode, bool> <>9__16_0;
            public static Func<HtmlNode, bool> <>9__16_1;
            public static Func<HtmlNode, bool> <>9__16_2;
            public static Func<HtmlNode, bool> <>9__16_3;
            public static Func<HtmlNode, bool> <>9__16_4;
            public static Func<HtmlNode, bool> <>9__16_5;
            public static Func<HtmlNode, bool> <>9__16_6;
            public static Func<HtmlNode, bool> <>9__17_0;
            public static Func<HtmlNode, bool> <>9__17_1;
            public static Func<HtmlNode, bool> <>9__17_2;
            public static Func<HtmlNode, bool> <>9__17_3;
            public static Func<HtmlNode, bool> <>9__18_1;
            public static Func<HtmlNode, bool> <>9__18_2;
            public static Func<HtmlNode, bool> <>9__18_3;
            public static Func<HtmlNode, bool> <>9__18_4;
            public static Func<HtmlNode, bool> <>9__18_8;
            public static Func<HtmlNode, bool> <>9__18_5;
            public static Func<HtmlNode, bool> <>9__18_9;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Hibbett.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__13_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"));

            internal bool <Atc>b__13_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "pid"));

            internal bool <Atc>b__13_2(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "minicart-quantity")) && (x.InnerHtml.Trim() == "1"));

            internal bool <DirectLink>b__18_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches size"));

            internal bool <DirectLink>b__18_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLink>b__18_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "price"));

            internal bool <DirectLink>b__18_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches color"));

            internal bool <DirectLink>b__18_5(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches size"));

            internal bool <DirectLink>b__18_8(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "swatches color"));

            internal bool <DirectLink>b__18_9(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "selectable"));

            internal bool <SubmitBilling>b__16_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "radial-nonce"));

            internal bool <SubmitBilling>b__16_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "radial-token"));

            internal bool <SubmitBilling>b__16_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "numberOfAdditionalTokenizatioRetries"));

            internal bool <SubmitBilling>b__16_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "additionalTokenizatioRetriesErroLevel"));

            internal bool <SubmitBilling>b__16_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "resetRetries"));

            internal bool <SubmitBilling>b__16_5(HtmlNode x) => 
                ((x.Attributes["value"] != null) && (x.Attributes["value"].Value == "Place Order"));

            internal bool <SubmitBilling>b__16_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"));

            internal bool <SubmitOrder>b__15_1(HtmlNode x) => 
                (((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "checkout-mini-cart")) && x.InnerText.Contains("This item is currently not available."));

            internal bool <SubmitOrder>b__15_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form"));

            internal bool <SubmitOrder>b__15_3(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "error-form"));

            internal bool <SubmitOrder>b__15_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "radialToken"));

            internal bool <SubmitShipping>b__17_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "dwfrm_oauthlogin"));

            internal bool <SubmitShipping>b__17_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"));

            internal bool <SubmitShipping>b__17_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"));

            internal bool <SubmitShipping>b__17_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "csrf_token"));
        }
    }
}

