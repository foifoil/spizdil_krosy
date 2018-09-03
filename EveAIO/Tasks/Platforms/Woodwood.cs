namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    internal class Woodwood : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;
        private HtmlDocument _currentDoc;
        private string _srr;
        private object _request;
        private object _bytes;
        private bool _repeatRequest;

        public Woodwood(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._srr = "";
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._repeatRequest = true;
                while (this._repeatRequest)
                {
                    this._repeatRequest = false;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://www.woodwood.com/cart");
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.CookieContainer = this._runner.Cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        request.Accept = "*/*";
                        request.KeepAlive = true;
                        request.Referer = this._runner.Product.Link;
                        request.Headers.Add("Origin", "https://www.woodwood.com");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        string s = "action=add&item_pid=" + this._runner.PickedSize.Value.Value + "&ajax=1";
                        byte[] bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                        this._srr = "";
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                this._srr = reader.ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (exception.Message.Contains("404") || exception.Message.Contains("407"))
                        {
                            throw;
                        }
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"web crash detected, repeating request in 1.5s"}");
                        }
                        Thread.Sleep(0x5dc);
                        this._repeatRequest = true;
                        continue;
                    }
                }
                if (!this._srr.Contains("item:" + this._runner.PickedSize.Value.Value))
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
            catch (Exception exception2)
            {
                this._runner.IsError = true;
                if (!exception2.Message.Contains("430") && ((exception2.InnerException == null) || !exception2.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception2, "", "");
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
            if (!this.SubmitBilling())
            {
                return false;
            }
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://payment.quickpay.net");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.Referer = "https://www.woodwood.com/checkout/confirm";
                request.Headers.Add("Origin", "https://www.woodwood.com");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                string str13 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "agreement_id"))).Attributes["value"].Value;
                string str14 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "amount"))).Attributes["value"].Value;
                string str2 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "autocapture"))).Attributes["value"].Value;
                string str4 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "callbackurl"))).Attributes["value"].Value;
                string str12 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "cancelurl"))).Attributes["value"].Value;
                string str8 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "continueurl"))).Attributes["value"].Value;
                string str9 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currency"))).Attributes["value"].Value;
                string str10 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "language"))).Attributes["value"].Value;
                string str11 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchant_id"))).Attributes["value"].Value;
                string str5 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "order_id"))).Attributes["value"].Value;
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "payment_methods"))).Attributes["value"].Value;
                string str6 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "version"))).Attributes["value"].Value;
                string str7 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "checksum"))).Attributes["value"].Value;
                string s = (((((((((((("agreement_id=" + str13) + "&amount=" + str14) + "&autocapture=" + str2) + "&callbackurl=" + WebUtility.UrlEncode(str4)) + "&cancelurl=" + WebUtility.UrlEncode(str12)) + "&continueurl=" + WebUtility.UrlEncode(str8)) + "&currency=" + str9) + "&language=" + str10) + "&merchant_id=" + str11) + "&order_id=" + str5) + "&payment_methods=" + str) + "&version=" + str6) + "&checksum=" + str7;
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                string requestUriString = "";
                string html = "";
                using (WebResponse response = request.GetResponse())
                {
                    requestUriString = response.ResponseUri.ToString();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                string str17 = requestUriString.Substring(requestUriString.LastIndexOf("/") + 1);
                string str18 = this._runner.Profile.CCNumber.Trim();
                this._runner.Profile.NameOnCard.Trim();
                string expiryMonth = this._runner.Profile.ExpiryMonth;
                if (expiryMonth.Length == 1)
                {
                    expiryMonth = "0" + expiryMonth;
                }
                string str20 = this._runner.Profile.ExpiryYear.Substring(2);
                string str21 = this._runner.Profile.Cvv.Trim();
                request = (HttpWebRequest) WebRequest.Create("https://payment.quickpay.net/payment-methods?bin=" + str18.Substring(0, 6) + "&_=");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.Referer = requestUriString;
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                html = "";
                using (WebResponse response2 = request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        html = reader2.ReadToEnd();
                    }
                }
                string str22 = "";
                if (!html.Contains("visa"))
                {
                    if (html.Contains("master"))
                    {
                        str22 = "master";
                    }
                }
                else
                {
                    str22 = "visa";
                }
                request = (HttpWebRequest) WebRequest.Create("https://payment.quickpay.net/calculate_fee");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.KeepAlive = true;
                request.Referer = requestUriString;
                request.Headers.Add("Origin", "https://payment.quickpay.net");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                s = "card_number=" + str18 + "&session_id=" + str17;
                bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream2 = request.GetRequestStream())
                {
                    stream2.Write(bytes, 0, bytes.Length);
                }
                html = "";
                using (WebResponse response3 = request.GetResponse())
                {
                    using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                    {
                        html = reader3.ReadToEnd();
                    }
                }
                this._currentDoc.LoadHtml(html);
                if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checkout profile already used for this website");
                    return false;
                }
                if (this._task.CheckoutDelay > 0)
                {
                    if (Global.SETTINGS.DetailedLog)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"checkout delay turned on, waiting " + this._task.CheckoutDelay + " ms"}");
                    }
                    Thread.Sleep(this._task.CheckoutDelay);
                }
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': submiting order");
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
                    request = (HttpWebRequest) WebRequest.Create("https://payment.quickpay.net/process_card");
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    request.CookieContainer = this._runner.Cookies;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    request.Accept = "application/json, text/javascript, */*; q=0.01";
                    request.KeepAlive = true;
                    request.Referer = requestUriString;
                    request.Headers.Add("Origin", "https://payment.quickpay.net");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    s = (((("session_id=" + str17) + "&cardnumber=" + str18) + "&expiration%5Bmonth%5D=" + expiryMonth) + "&expiration%5Byear%5D=" + str20) + "&cvd=" + str21;
                    bytes = Encoding.ASCII.GetBytes(s);
                    request.ContentLength = bytes.Length;
                    using (Stream stream3 = request.GetRequestStream())
                    {
                        stream3.Write(bytes, 0, bytes.Length);
                    }
                    html = "";
                    using (WebResponse response4 = request.GetResponse())
                    {
                        using (StreamReader reader4 = new StreamReader(response4.GetResponseStream()))
                        {
                            html = reader4.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order unsuccessful, reason: No payment method found for the given credit card.");
                    this._task.Status = States.GetTaskState(States.TaskState.NO_PAYMENT_METHOD_FOUND);
                    return false;
                }
                this._currentDoc.LoadHtml(html);
                if (html.ToLowerInvariant().Contains("An error happened. Please try again".ToLowerInvariant()))
                {
                    goto Label_1A31;
                }
                string str24 = "";
                if (html.Contains("Starting 3D Secure payment"))
                {
                    switch (str22)
                    {
                        case "visa":
                        {
                            request = (HttpWebRequest) WebRequest.Create(str24 = this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "secure_3d_form"))).Attributes["action"].Value);
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            request.Accept = "application/json, text/javascript, */*; q=0.01";
                            request.KeepAlive = true;
                            request.Referer = "https://payment.quickpay.net/process_card";
                            request.Headers.Add("Origin", "https://payment.quickpay.net");
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            request.ServicePoint.Expect100Continue = false;
                            string str23 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                            string str25 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                            s = (("PaReq=" + WebUtility.UrlEncode(str25)) + "&MD=" + WebUtility.UrlEncode(str23)) + "&TermUrl=" + WebUtility.UrlEncode(requestUriString + "/process_3d_secure");
                            bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream4 = request.GetRequestStream())
                            {
                                stream4.Write(bytes, 0, bytes.Length);
                            }
                            html = "";
                            using (WebResponse response5 = request.GetResponse())
                            {
                                using (StreamReader reader5 = new StreamReader(response5.GetResponseStream()))
                                {
                                    html = reader5.ReadToEnd();
                                }
                            }
                            this._currentDoc.LoadHtml(html);
                            request = (HttpWebRequest) WebRequest.Create(this._currentDoc.DocumentNode.Descendants("form").First<HtmlNode>().Attributes["action"].Value);
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            request.Accept = "application/json, text/javascript, */*; q=0.01";
                            request.KeepAlive = true;
                            request.Referer = requestUriString;
                            request.Headers.Add("Origin", "https://payment.quickpay.net");
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            str23 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                            str25 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                            s = ("PaRes=" + WebUtility.UrlEncode(str25)) + "&MD=" + WebUtility.UrlEncode(str23);
                            bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream5 = request.GetRequestStream())
                            {
                                stream5.Write(bytes, 0, bytes.Length);
                            }
                            html = "";
                            using (WebResponse response6 = request.GetResponse())
                            {
                                using (StreamReader reader6 = new StreamReader(response6.GetResponseStream()))
                                {
                                    html = reader6.ReadToEnd();
                                }
                            }
                            this._currentDoc.LoadHtml(html);
                            request = (HttpWebRequest) WebRequest.Create(requestUriString + "/poll-response");
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            request.Accept = "*/*";
                            request.KeepAlive = true;
                            request.Referer = requestUriString;
                            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            html = "";
                            using (WebResponse response7 = request.GetResponse())
                            {
                                using (StreamReader reader7 = new StreamReader(response7.GetResponseStream()))
                                {
                                    html = reader7.ReadToEnd();
                                }
                                break;
                            }
                        }
                        case "master":
                        {
                            request = (HttpWebRequest) WebRequest.Create("https://cap.attempts.securecode.com/acspage/cap?RID=8&VAA=B");
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.CookieContainer = this._runner.Cookies;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            request.Accept = "application/json, text/javascript, */*; q=0.01";
                            request.KeepAlive = true;
                            request.Referer = "https://payment.quickpay.net/process_card";
                            request.Headers.Add("Origin", "https://payment.quickpay.net");
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            request.ServicePoint.Expect100Continue = false;
                            string str26 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                            string str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"))).Attributes["value"].Value;
                            s = (("PaReq=" + WebUtility.UrlEncode(str27)) + "&MD=" + WebUtility.UrlEncode(str26)) + "&TermUrl=" + WebUtility.UrlEncode(requestUriString + "/process_3d_secure");
                            bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream6 = request.GetRequestStream())
                            {
                                stream6.Write(bytes, 0, bytes.Length);
                            }
                            html = "";
                            using (WebResponse response8 = request.GetResponse())
                            {
                                using (StreamReader reader8 = new StreamReader(response8.GetResponseStream()))
                                {
                                    html = reader8.ReadToEnd();
                                }
                            }
                            this._currentDoc.LoadHtml(html);
                            request = (HttpWebRequest) WebRequest.Create(requestUriString + "/process_3d_secure");
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.CookieContainer = this._runner.Cookies;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            request.Accept = "application/json, text/javascript, */*; q=0.01";
                            request.KeepAlive = true;
                            request.Referer = requestUriString;
                            request.Headers.Add("Origin", "https://payment.quickpay.net");
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            str26 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"))).Attributes["value"].Value;
                            str27 = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"))).Attributes["value"].Value;
                            s = ("PaRes=" + WebUtility.UrlEncode(str27)) + "&MD=" + WebUtility.UrlEncode(str26);
                            bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream7 = request.GetRequestStream())
                            {
                                stream7.Write(bytes, 0, bytes.Length);
                            }
                            html = "";
                            using (WebResponse response9 = request.GetResponse())
                            {
                                using (StreamReader reader9 = new StreamReader(response9.GetResponseStream()))
                                {
                                    html = reader9.ReadToEnd();
                                }
                            }
                            this._currentDoc.LoadHtml(html);
                            break;
                        }
                    }
                }
                if (html.Contains("3D Secure authorization failed"))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.D3_SECURE_FAILED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    return false;
                }
                request = (HttpWebRequest) WebRequest.Create(requestUriString);
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.KeepAlive = true;
                request.Referer = str24;
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                html = "";
                using (WebResponse response10 = request.GetResponse())
                {
                    using (StreamReader reader10 = new StreamReader(response10.GetResponseStream()))
                    {
                        html = reader10.ReadToEnd();
                    }
                    goto Label_1976;
                }
            Label_185E:
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                Thread.Sleep(0x5dc);
                request = (HttpWebRequest) WebRequest.Create(requestUriString + "/waiting");
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                request.CookieContainer = this._runner.Cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.KeepAlive = true;
                request.Referer = requestUriString;
                request.Headers.Add("Origin", "https://payment.quickpay.net");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                html = "";
                using (WebResponse response11 = request.GetResponse())
                {
                    using (StreamReader reader11 = new StreamReader(response11.GetResponseStream()))
                    {
                        html = reader11.ReadToEnd();
                    }
                }
            Label_1976:
                if (html.Contains("/waiting"))
                {
                    goto Label_185E;
                }
                this._currentDoc.LoadHtml(html);
                if (!html.ToLowerInvariant().Contains("An error happened. Please try again".ToLowerInvariant()))
                {
                    try
                    {
                        EveAIO.Helpers.AddDbValue("WoodWood|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(html)));
                    }
                    catch
                    {
                    }
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order successful. Congratulations!");
                    return true;
                }
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order unsuccessful, reason: An error happened. Please try again.");
                return false;
            Label_1A31:
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order unsuccessful, reason: An error happened. Please try again.");
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("Thread was being aborted")))
                {
                    this._runner.IsError = true;
                    if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                        States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_ORDER, exception, "", "");
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                    }
                    return false;
                }
                return false;
            }
        }

        public bool DirectLink(string link) => 
            this.DirectLinkInternal(link, false);

        private bool DirectLinkInternal(string link, bool isSearch = false)
        {
            try
            {
                if (!isSearch)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                string html = "";
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    request.Proxy = this._runner.Proxy;
                }
                html = "";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                    {
                        html = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    }
                }
                catch (Exception exception1)
                {
                    if (!exception1.Message.Contains("503"))
                    {
                        throw;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                    States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_IN_QUEUE, null, "", "");
                    Thread.Sleep(0x3a98);
                    request = (HttpWebRequest) WebRequest.Create(link);
                    request.KeepAlive = true;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.CookieContainer = this._runner.Cookies;
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    html = "";
                    using (HttpWebResponse response2 = (HttpWebResponse) request.GetResponse())
                    {
                        html = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                    }
                    if (!isSearch)
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                    }
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(html);
                if (document.DocumentNode.Descendants("select").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-form-size-0")))
                {
                    string str4 = this._runner.ProductPageHtml.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-title-brand"))).InnerText.Trim().Replace("</span>", " ").Replace("<span>", "");
                    string str5 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"))).InnerText.Trim();
                    string str3 = this._runner.ProductPageHtml.DocumentNode.Descendants("a").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-image"))).Descendants("img").First<HtmlNode>().Attributes["src"].Value;
                    this._task.ImgUrl = str3;
                    Product product1 = new Product {
                        ProductTitle = str4,
                        Link = link,
                        Price = str5
                    };
                    this._runner.Product = product1;
                    if (isSearch && !string.IsNullOrEmpty(this._task.Color))
                    {
                        string str2 = "";
                        if (this._runner.ProductPageHtml.DocumentNode.Descendants("span").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-color")))
                        {
                            str2 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-color"))).InnerText.Trim();
                        }
                        if (string.IsNullOrEmpty(str2))
                        {
                            States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                            if (!this._task.ColorPickRandom)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            str2 = str2.ToUpperInvariant();
                            if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                            {
                                if ((str2 != this._task.Color.ToUpperInvariant()) && !this._task.ColorPickRandom)
                                {
                                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                                    return false;
                                }
                            }
                            else if (!str2.Contains(this._task.Color.ToUpperInvariant()) && !this._task.ColorPickRandom)
                            {
                                States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                                return false;
                            }
                        }
                    }
                    foreach (HtmlNode node in from x in document.DocumentNode.Descendants("select").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-form-size-0"))).Descendants("option")
                        where !string.IsNullOrEmpty(x.Attributes["value"].Value) && !x.Attributes["value"].Value.Contains("http")
                        select x)
                    {
                        string key = node.InnerText.Trim().Replace("US ", "").Replace("UK ", "");
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(key, node.Attributes["value"].Value);
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
                                goto Label_0988;
                            Label_08B8:
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
                                    goto Label_0996;
                                }
                            Label_0988:
                                if (!enumerator2.MoveNext())
                                {
                                    continue;
                                }
                                goto Label_08B8;
                            Label_0996:
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
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': product out of stock");
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

        public bool Login() => 
            true;

        public bool Search()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': searching for products");
                string html = "";
                foreach (string str3 in this._task.Keywords)
                {
                    string requestUriString = $"https://www.woodwood.com/catalog?search={str3.Replace(" ", "+").ToLowerInvariant()}";
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                    request.KeepAlive = true;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.CookieContainer = this._runner.Cookies;
                    if (this._runner.Proxy != null)
                    {
                        request.Proxy = this._runner.Proxy;
                    }
                    html = "";
                    try
                    {
                        using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                        {
                            html = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        }
                    }
                    catch (Exception exception1)
                    {
                        if (exception1.Message.Contains("503"))
                        {
                            this._task.Status = States.GetTaskState(States.TaskState.WAITING_IN_QUEUE);
                            States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_IN_QUEUE, null, "", "");
                            Thread.Sleep(0x3a98);
                            request = (HttpWebRequest) WebRequest.Create(requestUriString);
                            request.KeepAlive = true;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            request.CookieContainer = this._runner.Cookies;
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            html = "";
                            using (HttpWebResponse response2 = (HttpWebResponse) request.GetResponse())
                            {
                                html = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                                goto Label_024C;
                            }
                        }
                        throw;
                    Label_024C:
                        States.WriteLogger(this._task, States.LOGGER_STATES.SEARCHING_FOR_PRODUCTS, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.SEARCHING);
                    }
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(html);
                    if (document.DocumentNode.Descendants("ul").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-lister-list")))
                    {
                        using (IEnumerator<HtmlNode> enumerator2 = document.DocumentNode.Descendants("ul").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-lister-list"))).Descendants("li").GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                string link = enumerator2.Current.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                                if (this.DirectLinkInternal(link, true))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': no products found matching the criteria");
                return false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted"))
                {
                    string str5 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str5 = " - " + exception.Message;
                    }
                    this._runner.IsError = true;
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error searching for products{str5}");
                    Global.Logger.Error($"Error searching for products of task '{this._task.Name}'", exception);
                    return false;
                }
                return false;
            }
        }

        public void SetClient()
        {
        }

        private bool SubmitBilling()
        {
            try
            {
                byte[] bytes;
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_BILLING);
                States.WriteLogger(this._task, States.LOGGER_STATES.SUBMITTING_BILLING, null, "", "");
                HttpWebRequest request = null;
                string s = "";
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                string str2 = obj2.Email.Trim();
                string str3 = obj2.FirstName.Trim();
                string str4 = obj2.LastName.Trim();
                string str5 = obj2.Address1.Trim();
                string str6 = obj2.Address2.Trim();
                string str7 = obj2.City.Trim();
                string str8 = obj2.Zip.Trim();
                string str9 = obj2.CountryId.ToLowerInvariant();
                string str10 = "";
                if ((str9 != "us") && (str9 != "ca"))
                {
                    str10 = obj2.State;
                }
                else
                {
                    str10 = obj2.StateId.ToLowerInvariant();
                }
                string str11 = obj2.Phone.Trim();
                string state = obj2.State;
                string html = "";
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        request = (HttpWebRequest) WebRequest.Create("https://www.woodwood.com/onestepcheckout");
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.CookieContainer = this._runner.Cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                        request.KeepAlive = true;
                        request.Referer = this._runner.Product.Link;
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        html = "";
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                html = reader.ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (((exception == null) || exception.Message.Contains("403")) || (exception.Message.Contains("404") || exception.Message.Contains("407")))
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
                        request = (HttpWebRequest) WebRequest.Create("https://www.woodwood.com/checkout/details");
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.CookieContainer = this._runner.Cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        request.Accept = "*/*";
                        request.KeepAlive = true;
                        request.Referer = "https://www.woodwood.com/onestepcheckout";
                        request.Headers.Add("Origin", "https://www.woodwood.com");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        s = "&formid=details.anonymous";
                        s = (((((((((s + "&email=" + "&first_name=") + "&surname=" + "&address=") + "&address2=" + "&zip=") + "&city=" + "&state=") + "&country=us" + "&phone=") + "&alternate_recipient=" + "&recipient_first_name=") + "&recipient_surname=" + "&recipient_address=") + "&recipient_address2=" + "&recipient_zip=") + "&recipient_city=" + "&recipient_state=") + "&recipient_country=dk" + "&newsletter=";
                        bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                        html = "";
                        using (WebResponse response2 = request.GetResponse())
                        {
                            using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                html = reader2.ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch (WebException exception2)
                    {
                        if (((exception2 == null) || exception2.Message.Contains("403")) || (exception2.Message.Contains("404") || exception2.Message.Contains("407")))
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
                        request = (HttpWebRequest) WebRequest.Create("https://www.woodwood.com/checkout/details");
                        if (this._runner.Proxy != null)
                        {
                            request.Proxy = this._runner.Proxy;
                        }
                        request.CookieContainer = this._runner.Cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                        request.Accept = "*/*";
                        request.KeepAlive = true;
                        request.Referer = "https://www.woodwood.com/onestepcheckout";
                        request.Headers.Add("Origin", "https://www.woodwood.com");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        s = "&formid=details.anonymous";
                        s = ((((((((((((((s + "&email=" + str2) + "&first_name=" + str3) + "&surname=" + str4) + "&address=" + str5) + "&address2=" + str6) + "&zip=" + str8) + "&city=" + str7) + "&state=" + str10) + "&country=" + str9) + "&phone=" + str11) + "&alternate_recipient=" + "&recipient_first_name=") + "&recipient_surname=" + "&recipient_address=") + "&recipient_address2=" + "&recipient_zip=") + "&recipient_city=" + "&recipient_state=") + "&recipient_country=dk" + "&newsletter=";
                        bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream2 = request.GetRequestStream())
                        {
                            stream2.Write(bytes, 0, bytes.Length);
                        }
                        html = "";
                        using (WebResponse response3 = request.GetResponse())
                        {
                            using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                            {
                                html = reader3.ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch (WebException exception3)
                    {
                        if (((exception3 == null) || exception3.Message.Contains("403")) || (exception3.Message.Contains("404") || exception3.Message.Contains("407")))
                        {
                            throw;
                        }
                        flag = true;
                        States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                        Thread.Sleep(0x3e8);
                        continue;
                    }
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                if (document.DocumentNode.Descendants("li").Any<HtmlNode>(x => (x.Attributes["id"] != null) && (x.Attributes["id"].Value == "handling-form-li-shipping")))
                {
                    string str13 = "";
                    try
                    {
                        str13 = document.DocumentNode.Descendants("li").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "handling-form-li-shipping"))).Descendants("input").First<HtmlNode>().Attributes["value"].Value;
                    }
                    catch
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.COUNTRY_NOT_SUPPORTED, null, "", "");
                        this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                        return false;
                    }
                    flag = true;
                    while (flag)
                    {
                        flag = false;
                        try
                        {
                            request = (HttpWebRequest) WebRequest.Create("https://www.woodwood.com/checkout/handling");
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.CookieContainer = this._runner.Cookies;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            request.Accept = "*/*";
                            request.KeepAlive = true;
                            request.Referer = "https://www.woodwood.com/onestepcheckout";
                            request.Headers.Add("Origin", "https://www.woodwood.com");
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                            s = "&mode=user&shipping=" + str13 + "&payment=quickpay10secure";
                            bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream3 = request.GetRequestStream())
                            {
                                stream3.Write(bytes, 0, bytes.Length);
                            }
                            html = "";
                            using (WebResponse response4 = request.GetResponse())
                            {
                                using (StreamReader reader4 = new StreamReader(response4.GetResponseStream()))
                                {
                                    html = reader4.ReadToEnd();
                                }
                            }
                            continue;
                        }
                        catch (WebException exception4)
                        {
                            if (((exception4 == null) || exception4.Message.Contains("403")) || (exception4.Message.Contains("404") || exception4.Message.Contains("407")))
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
                            request = (HttpWebRequest) WebRequest.Create("https://www.woodwood.com/checkout/confirm");
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            request.CookieContainer = this._runner.Cookies;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                            request.Accept = "*/*";
                            request.KeepAlive = true;
                            request.Referer = "https://www.woodwood.com/onestepcheckout";
                            request.Headers.Add("Origin", "https://www.woodwood.com");
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                            s = "confirmed=on";
                            bytes = Encoding.ASCII.GetBytes(s);
                            request.ContentLength = bytes.Length;
                            using (Stream stream4 = request.GetRequestStream())
                            {
                                stream4.Write(bytes, 0, bytes.Length);
                            }
                            html = "";
                            using (WebResponse response5 = request.GetResponse())
                            {
                                using (StreamReader reader5 = new StreamReader(response5.GetResponseStream()))
                                {
                                    html = reader5.ReadToEnd();
                                }
                            }
                            continue;
                        }
                        catch (WebException exception5)
                        {
                            if (((exception5 == null) || exception5.Message.Contains("403")) || (exception5.Message.Contains("404") || exception5.Message.Contains("407")))
                            {
                                throw;
                            }
                            flag = true;
                            States.WriteLogger(this._task, States.LOGGER_STATES.CRASH_DETECTED, null, "", "1");
                            Thread.Sleep(0x3e8);
                            continue;
                        }
                    }
                    this._currentDoc.LoadHtml(html);
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.COUNTRY_NOT_SUPPORTED, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception6)
            {
                this._runner.IsError = true;
                if (!exception6.Message.Contains("430") && ((exception6.InnerException == null) || !exception6.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_SUBMITTING_BILLING, exception6, "", "");
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
            public static readonly Woodwood.<>c <>9;
            public static Func<HtmlNode, bool> <>9__10_0;
            public static Func<HtmlNode, bool> <>9__10_1;
            public static Func<HtmlNode, bool> <>9__10_2;
            public static Func<HtmlNode, bool> <>9__10_3;
            public static Func<HtmlNode, bool> <>9__10_4;
            public static Func<HtmlNode, bool> <>9__10_5;
            public static Func<HtmlNode, bool> <>9__10_6;
            public static Func<HtmlNode, bool> <>9__10_7;
            public static Func<HtmlNode, bool> <>9__10_8;
            public static Func<HtmlNode, bool> <>9__10_9;
            public static Func<HtmlNode, bool> <>9__10_10;
            public static Func<HtmlNode, bool> <>9__10_11;
            public static Func<HtmlNode, bool> <>9__10_12;
            public static Func<HtmlNode, bool> <>9__10_14;
            public static Func<HtmlNode, bool> <>9__10_15;
            public static Func<HtmlNode, bool> <>9__10_16;
            public static Func<HtmlNode, bool> <>9__10_17;
            public static Func<HtmlNode, bool> <>9__10_18;
            public static Func<HtmlNode, bool> <>9__10_19;
            public static Func<HtmlNode, bool> <>9__10_20;
            public static Func<HtmlNode, bool> <>9__10_21;
            public static Func<HtmlNode, bool> <>9__10_22;
            public static Func<HtmlNode, bool> <>9__11_1;
            public static Func<HtmlNode, bool> <>9__11_2;
            public static Func<HtmlNode, bool> <>9__13_0;
            public static Func<HtmlNode, bool> <>9__13_1;
            public static Func<HtmlNode, bool> <>9__13_2;
            public static Func<HtmlNode, bool> <>9__13_3;
            public static Func<HtmlNode, bool> <>9__13_5;
            public static Func<HtmlNode, bool> <>9__13_6;
            public static Func<HtmlNode, bool> <>9__13_4;
            public static Func<HtmlNode, bool> <>9__13_7;
            public static Func<HtmlNode, bool> <>9__15_0;
            public static Func<HtmlNode, bool> <>9__15_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Woodwood.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Checkout>b__10_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "agreement_id"));

            internal bool <Checkout>b__10_1(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "amount"));

            internal bool <Checkout>b__10_10(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "payment_methods"));

            internal bool <Checkout>b__10_11(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "version"));

            internal bool <Checkout>b__10_12(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "checksum"));

            internal bool <Checkout>b__10_14(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "secure_3d_form"));

            internal bool <Checkout>b__10_15(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <Checkout>b__10_16(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <Checkout>b__10_17(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <Checkout>b__10_18(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <Checkout>b__10_19(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <Checkout>b__10_2(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "autocapture"));

            internal bool <Checkout>b__10_20(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaReq"));

            internal bool <Checkout>b__10_21(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "MD"));

            internal bool <Checkout>b__10_22(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "PaRes"));

            internal bool <Checkout>b__10_3(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "callbackurl"));

            internal bool <Checkout>b__10_4(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "cancelurl"));

            internal bool <Checkout>b__10_5(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "continueurl"));

            internal bool <Checkout>b__10_6(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "currency"));

            internal bool <Checkout>b__10_7(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "language"));

            internal bool <Checkout>b__10_8(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "merchant_id"));

            internal bool <Checkout>b__10_9(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "order_id"));

            internal bool <DirectLinkInternal>b__13_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-form-size-0"));

            internal bool <DirectLinkInternal>b__13_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-title-brand"));

            internal bool <DirectLinkInternal>b__13_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "price"));

            internal bool <DirectLinkInternal>b__13_3(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-image"));

            internal bool <DirectLinkInternal>b__13_4(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-form-size-0"));

            internal bool <DirectLinkInternal>b__13_5(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-color"));

            internal bool <DirectLinkInternal>b__13_6(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-show-color"));

            internal bool <DirectLinkInternal>b__13_7(HtmlNode x) => 
                (!string.IsNullOrEmpty(x.Attributes["value"].Value) && !x.Attributes["value"].Value.Contains("http"));

            internal bool <Search>b__15_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-lister-list"));

            internal bool <Search>b__15_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "commodity-lister-list"));

            internal bool <SubmitBilling>b__11_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "handling-form-li-shipping"));

            internal bool <SubmitBilling>b__11_2(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "handling-form-li-shipping"));
        }
    }
}

