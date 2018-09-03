namespace EveAIO.Tasks.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    internal class Hanon : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;

        public Hanon(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            try
            {
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"adding product to cart"}");
                string str = "";
                bool flag = true;
                string requestUriString = "";
                while (flag)
                {
                    flag = false;
                    try
                    {
                        requestUriString = "https://www.hanon-shop.com/gateway/ajaxGateway.php?mode=add&_module=shop";
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
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
                        request.Referer = this._runner.Product.Link;
                        request.Headers.Add("Origin", "https://www.hanon-shop.com");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        string s = "products%5B" + this._runner.PickedSize.Value.Value + "%5D=1";
                        byte[] bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                        str = "";
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                str = reader.ReadToEnd();
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
                        Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception);
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"web crash detected, repeating request in 1.5s"}");
                        }
                        Thread.Sleep(0x5dc);
                        flag = true;
                        continue;
                    }
                }
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                if (<>o__4.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__4.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Hanon), argumentInfo));
                }
                if (<>o__4.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__4.<>p__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Hanon), argumentInfo));
                }
                if (<>o__4.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__4.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(Hanon), argumentInfo));
                }
                if (<>o__4.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__4.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Hanon), argumentInfo));
                }
                if (!<>o__4.<>p__3.Target(<>o__4.<>p__3, <>o__4.<>p__2.Target(<>o__4.<>p__2, <>o__4.<>p__1.Target(<>o__4.<>p__1, <>o__4.<>p__0.Target(<>o__4.<>p__0, obj2, "numitems")), 1)))
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"failed adding product to cart"}");
                    return false;
                }
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"product successfully added to cart"}");
                return true;
            }
            catch (Exception exception2)
            {
                if (!exception2.Message.Contains("Thread was being aborted"))
                {
                    this._runner.IsError = true;
                    string str4 = "";
                    if (exception2.GetType() == typeof(WebException))
                    {
                        str4 = " - " + exception2.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error while adding product to cart{str4}");
                    Global.Logger.Error($"Error while adding product to cart of task '{this._task.Name} - {this._task.Guid}'", exception2);
                }
                return false;
            }
        }

        public bool Checkout()
        {
            try
            {
                byte[] bytes;
                HttpWebRequest request = null;
                string s = "";
                string html = "";
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checkout started");
                ProfileObject obj2 = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                string requestUriString = "";
                HtmlDocument document = new HtmlDocument();
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        requestUriString = "https://www.hanon-shop.com/basket";
                        request = (HttpWebRequest) WebRequest.Create(requestUriString);
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
                        request.Referer = "https://www.hanon-shop.com/basket";
                        request.Headers.Add("Origin", "https://www.hanon-shop.com");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        s = "billing%5Bcountry%5D=" + obj2.CountryId;
                        bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                        html = "";
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                html = reader.ReadToEnd();
                            }
                        }
                        Global.Logger.Debug($"Task '{this._task.Name} - {this._task.Guid}': {"https://www.hanon-shop.com/basket : " + html}");
                        continue;
                    }
                    catch (WebException exception)
                    {
                        if (exception.Message.Contains("404") || exception.Message.Contains("407"))
                        {
                            throw;
                        }
                        Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception);
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"web crash detected, repeating request in 1.5s"}");
                        }
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
                        requestUriString = "https://www.hanon-shop.com/checkout/q/card";
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
                        request.Referer = "https://www.hanon-shop.com/checkout/q/card";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
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
                        if (exception2.Message.Contains("404") || exception2.Message.Contains("407"))
                        {
                            throw;
                        }
                        Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception2);
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog(string.Format("Task '{0} - {2}': {1}", this._task.Name, "web crash detected, repeating request in 1.5s", this._task.Guid));
                        }
                        Thread.Sleep(0x5dc);
                        flag = true;
                        continue;
                    }
                }
                string str10 = obj2.Email.Trim();
                string str11 = obj2.FirstName.Trim();
                string str14 = obj2.LastName.Trim();
                string str6 = obj2.Address1.Trim();
                string str13 = obj2.Address2.Trim();
                string str12 = obj2.City.Trim();
                string str4 = obj2.Zip.Trim();
                string stateId = obj2.StateId;
                string str7 = obj2.Phone.Trim();
                string countryId = obj2.CountryId;
                string state = obj2.State;
                flag = true;
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_SHIPPING);
                while (flag)
                {
                    flag = false;
                    try
                    {
                        requestUriString = "https://www.hanon-shop.com/checkout/q/card";
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
                        request.Referer = "https://www.hanon-shop.com/checkout/q/card";
                        request.Headers.Add("Origin", "https://www.hanon-shop.com");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        s = ((((((((("billing%5Bemail%5D=" + WebUtility.UrlEncode(str10)) + "&billing%5Bfirstname%5D=" + WebUtility.UrlEncode(str11)) + "&billing%5Blastname%5D=" + WebUtility.UrlEncode(str14)) + "&billing%5Bphone%5D=" + WebUtility.UrlEncode(str7)) + "&billing%5Bcompany_name%5D=") + "&billing%5Bcountry%5D=" + countryId) + "&billing%5Bpostcode%5D=" + WebUtility.UrlEncode(str4)) + "&billing%5Baddress_line_1%5D=" + WebUtility.UrlEncode(str6)) + "&billing%5Baddress_line_2%5D=" + WebUtility.UrlEncode(str13)) + "&billing%5Btown%5D=" + WebUtility.UrlEncode(str12);
                        if ((countryId != "US") && (countryId != "CA"))
                        {
                            s = (s + "&billing%5Bcounty%5D=" + state) + "&billing%5Bstate%5D=";
                        }
                        else
                        {
                            s = (s + "&billing%5Bcounty%5D=") + "&billing%5Bstate%5D=" + stateId;
                        }
                        if (!obj2.SameBillingShipping)
                        {
                            string str22 = obj2.FirstNameShipping.Trim();
                            string str15 = obj2.LastNameShipping.Trim();
                            string str19 = obj2.Address1Shipping.Trim();
                            string str16 = obj2.Address2Shipping.Trim();
                            string str20 = obj2.CityShipping.Trim();
                            string str18 = obj2.ZipShipping.Trim();
                            string stateIdShipping = obj2.StateIdShipping;
                            string str23 = obj2.PhoneShipping.Trim();
                            string countryIdShipping = obj2.CountryIdShipping;
                            string stateShipping = obj2.StateShipping;
                            s = (((((((((s + "&extra%5Bbilling_is_delivery%5D=0") + "&invoice%5Bfirstname%5D=" + WebUtility.UrlEncode(str22)) + "&invoice%5Blastname%5D=" + WebUtility.UrlEncode(str15)) + "&invoice%5Bphone%5D=" + WebUtility.UrlEncode(str23)) + "&invoice%5Bcompany_name%5D=") + "&invoice%5Bcountry%5D=" + countryIdShipping) + "&invoice%5Bpostcode%5D=" + WebUtility.UrlEncode(str18)) + "&invoice%5Baddress_line_1%5D=" + WebUtility.UrlEncode(str19)) + "&invoice%5Baddress_line_2%5D=" + WebUtility.UrlEncode(str16)) + "&invoice%5Btown%5D=" + WebUtility.UrlEncode(str20);
                            if ((countryId != "US") && (countryId != "CA"))
                            {
                                s = (s + "&invoice%5Bcounty%5D=" + stateShipping) + "&invoice%5Bstate%5D=";
                            }
                            else
                            {
                                s = (s + "&invoice%5Bcounty%5D=") + "&invoice%5Bstate%5D=" + stateIdShipping;
                            }
                        }
                        else
                        {
                            s = (((((s + "&extra%5Bbilling_is_delivery%5D=0" + "&extra%5Bbilling_is_delivery%5D=1") + "&invoice%5Bfirstname%5D=" + "&invoice%5Blastname%5D=") + "&invoice%5Bphone%5D=" + "&invoice%5Bcompany_name%5D=") + "&invoice%5Bcountry%5D=" + countryId) + "&invoice%5Bpostcode%5D=" + "&invoice%5Baddress_line_1%5D=") + "&invoice%5Baddress_line_2%5D=" + "&invoice%5Btown%5D=";
                            if ((countryId != "US") && (countryId != "CA"))
                            {
                                s = (s + "&invoice%5Bcounty%5D=" + state) + "&invoice%5Bstate%5D=";
                            }
                            else
                            {
                                s = (s + "&invoice%5Bcounty%5D=") + "&invoice%5Bstate%5D=" + stateId;
                            }
                        }
                        s = (s + "&extra%5Bterms_and_conditions%5D=1") + "&payby=card" + "&checkout_form=1";
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
                        document.LoadHtml(html);
                        Global.Logger.Debug($"Task '{this._task.Name} -{this._task.Guid}': {"POST-https://www.hanon-shop.com/checkout/q/card : " + html}");
                        continue;
                    }
                    catch (WebException exception3)
                    {
                        if (exception3.Message.Contains("404") || exception3.Message.Contains("407"))
                        {
                            throw;
                        }
                        Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception3);
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog(string.Format("Task '{0} - {2}': {1}", this._task.Name, "web crash detected, repeating request in 1.5s", this._task.Guid));
                        }
                        Thread.Sleep(0x5dc);
                        flag = true;
                        continue;
                    }
                }
                string encodedValue = document.DocumentNode.Descendants("iframe").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sagepay"))).Attributes["src"].Value;
                flag = true;
                while (flag)
                {
                    flag = false;
                    try
                    {
                        requestUriString = WebUtility.UrlDecode(encodedValue);
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
                        request.Referer = "https://www.hanon-shop.com/checkout/q/card/payment";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
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
                        if (exception4.Message.Contains("404") || exception4.Message.Contains("407"))
                        {
                            throw;
                        }
                        Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception4);
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog(string.Format("Task '{0} - {2}': {1}", this._task.Name, "web crash detected, repeating request in 1.5s", this._task.Guid));
                        }
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
                        requestUriString = "https://live.sagepay.com/gateway/service/carddetails";
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
                        request.Headers.Add("Upgrade-Insecure-Requests", "1");
                        request.KeepAlive = true;
                        request.Referer = "https://www.hanon-shop.com/checkout/q/card/payment";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
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
                        if (exception5.Message.Contains("404") || exception5.Message.Contains("407"))
                        {
                            throw;
                        }
                        Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception5);
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"web crash detected, repeating request in 1.5s"}");
                        }
                        Thread.Sleep(0x5dc);
                        flag = true;
                        continue;
                    }
                }
                if (this._runner.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this._runner.HomeUrl))))
                {
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checkout profile already used for this website");
                    this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
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
                string str26 = "";
                flag = true;
                this._task.Status = States.GetTaskState(States.TaskState.SUBMITTING_ORDER);
                while (flag)
                {
                    flag = false;
                    try
                    {
                        requestUriString = "https://live.sagepay.com/gateway/service/carddetails";
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
                        request.Referer = "https://live.sagepay.com/gateway/service/carddetails";
                        request.Headers.Add("Origin", "https://live.sagepay.com");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        string str29 = obj2.CCNumber.Trim();
                        string str30 = obj2.NameOnCard.Trim();
                        string expiryMonth = obj2.ExpiryMonth;
                        if (expiryMonth.Length == 1)
                        {
                            expiryMonth = "0" + expiryMonth;
                        }
                        s = ((((("cardholder=" + WebUtility.UrlEncode(str30)) + "&cardnumber=" + WebUtility.UrlEncode(str29)) + "&expirymonth=" + expiryMonth) + "&expiryyear=" + obj2.ExpiryYear.Substring(2)) + "&securitycode=" + obj2.Cvv.Trim()) + "&action=proceed";
                        bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream3 = request.GetRequestStream())
                        {
                            stream3.Write(bytes, 0, bytes.Length);
                        }
                        html = "";
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
                        using (WebResponse response6 = request.GetResponse())
                        {
                            if (response6.Headers["Location"] != null)
                            {
                                str26 = "x";
                            }
                            using (StreamReader reader6 = new StreamReader(response6.GetResponseStream()))
                            {
                                html = reader6.ReadToEnd();
                            }
                        }
                        Global.Logger.Debug($"Task '{this._task.Name} - {this._task.Guid}': {"https://live.sagepay.com/gateway/service/carddetails : " + html}");
                        continue;
                    }
                    catch (WebException exception6)
                    {
                        if (exception6.Message.Contains("404") || exception6.Message.Contains("407"))
                        {
                            throw;
                        }
                        Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception6);
                        if (Global.SETTINGS.DetailedLog)
                        {
                            EveAIO.Helpers.WriteLog(string.Format("Task '{0} - {2}': {1}", this._task.Name, "web crash detected, repeating request in 1.5s", this._task.Guid));
                        }
                        Thread.Sleep(0x5dc);
                        flag = true;
                        continue;
                    }
                }
                if (html.Contains("Please click button below to continue with the 3-D Secure checks"))
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order unsuccessful, reason: 3-D secure card check needed. Use different credit card.");
                    this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                    return false;
                }
                if (!string.IsNullOrEmpty(str26))
                {
                    goto Label_140E;
                }
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order unsuccessful, reason: please correct your credit card information");
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                return false;
            Label_1312:
                this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_ORDER);
                Thread.Sleep(0x5dc);
                request = (HttpWebRequest) WebRequest.Create("https://live.sagepay.com/gateway/service/authorisation");
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
                request.Referer = "https://live.sagepay.com/gateway/service/carddetails";
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                html = "";
                using (WebResponse response7 = request.GetResponse())
                {
                    using (StreamReader reader7 = new StreamReader(response7.GetResponseStream()))
                    {
                        html = reader7.ReadToEnd();
                    }
                }
            Label_140E:
                if (html.Contains("Your payment is being authorised"))
                {
                    goto Label_1312;
                }
                Global.Logger.Debug($"Task '{this._task.Name} - {this._task.Guid}': {"https://live.sagepay.com/gateway/service/authorisation : " + html}");
                if (!html.ToLowerInvariant().Contains("The Authorisation has been declined".ToLowerInvariant()))
                {
                    EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order successful. Congratulations!");
                    return true;
                }
                this._task.Status = States.GetTaskState(States.TaskState.CARD_DECLINED);
                EveAIO.Helpers.WriteLog($"task '{this._task.Name}': order unsuccessful, reason: The Authorisation has been declined by the bank. Please try a different card.");
                return false;
            }
            catch (Exception exception7)
            {
                if (!exception7.Message.Contains("Thread was being aborted"))
                {
                    this._runner.IsError = true;
                    string str32 = "";
                    if (exception7.GetType() == typeof(WebException))
                    {
                        str32 = " - " + exception7.Message;
                    }
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error while product checkout{str32}");
                    Global.Logger.Error($"Error while product checkout of task '{this._task.Name} - {this._task.Guid}'", exception7);
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
                HttpWebRequest request2;
                if (!isSearch)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                }
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': checking stock availability - {link}");
                string html = "";
                bool flag = false;
                bool flag2 = true;
                while (flag2)
                {
                    flag2 = false;
                    try
                    {
                        try
                        {
                            CloudFlareSolver solver1 = new CloudFlareSolver(link, this._runner.Cookies, true, this._task.Link, true, this._runner.Proxy);
                            solver1.Solve();
                            solver1.SendChallengeRequest();
                            HttpWebRequest request = solver1.CreateWebRequest(link);
                            if (this._runner.Proxy != null)
                            {
                                request.Proxy = this._runner.Proxy;
                            }
                            using (WebResponse response = request.GetResponse())
                            {
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    html = reader.ReadToEnd();
                                }
                            }
                        }
                        catch
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            request2 = (HttpWebRequest) WebRequest.Create(link);
                            request2.KeepAlive = true;
                            request2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request2.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request2.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                            request2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                            request2.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            request2.CookieContainer = this._runner.Cookies;
                            if (this._runner.Proxy != null)
                            {
                                request2.Proxy = this._runner.Proxy;
                            }
                            html = "";
                            using (HttpWebResponse response2 = (HttpWebResponse) request2.GetResponse())
                            {
                                html = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                            }
                        }
                        continue;
                    }
                    catch (Exception exception1)
                    {
                        if (exception1.Message.Contains("The remote server returned an error: (503) Server Unavailable"))
                        {
                            if (Global.SETTINGS.DetailedLog)
                            {
                                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': CloudFlare computation failed. Re-trying in 2 seconds");
                            }
                            Thread.Sleep(0x7d0);
                            flag2 = true;
                        }
                        continue;
                    }
                }
                new HtmlDocument().LoadHtml(html);
                Global.Logger.Debug($"Task '{this._task.Name} - {this._task.Guid}': {link + " : " + html}");
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(html);
                if (html.Contains("var opts"))
                {
                    string str4 = html.Substring(html.IndexOf("var opts"));
                    str4 = str4.Substring(str4.IndexOf("=") + 1);
                    object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str4.Substring(0, str4.IndexOf(";")));
                    string str5 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"))).InnerText.Trim();
                    string str2 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "prod-price"))).InnerText.Trim();
                    if (str2.Contains(";"))
                    {
                        str2 = str2.Substring(str2.IndexOf(";") + 1);
                    }
                    this._task.ImgUrl = "http://eve-robotics.com/dummy_product.png";
                    Product product1 = new Product {
                        ProductTitle = str5,
                        Link = link,
                        Price = str2
                    };
                    this._runner.Product = product1;
                    if (isSearch && !string.IsNullOrEmpty(this._task.Color))
                    {
                        string str3 = "";
                        if (this._runner.ProductPageHtml.DocumentNode.Descendants("span").Any<HtmlNode>(x => (x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "description")))
                        {
                            str3 = this._runner.ProductPageHtml.DocumentNode.Descendants("span").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "description"))).InnerText.Trim();
                        }
                        if (!string.IsNullOrEmpty(str3))
                        {
                            str3 = str3.ToUpperInvariant();
                            if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                            {
                                if (str3 != this._task.Color.ToUpperInvariant())
                                {
                                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': specified color/model not found");
                                    if (!this._task.ColorPickRandom)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else if (!str3.Contains(this._task.Color.ToUpperInvariant()))
                            {
                                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': specified color/model not found");
                                if (!this._task.ColorPickRandom)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (Global.SETTINGS.DetailedLog)
                            {
                                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': product color not found");
                            }
                            if (!this._task.ColorPickRandom)
                            {
                                return false;
                            }
                        }
                    }
                    if (<>o__7.<>p__5 == null)
                    {
                        <>o__7.<>p__5 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Hanon)));
                    }
                    foreach (object obj3 in <>o__7.<>p__5.Target(<>o__7.<>p__5, obj2))
                    {
                        if (<>o__7.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__7.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "First", typeof(Hanon), argumentInfo));
                        }
                        object obj4 = <>o__7.<>p__0.Target(<>o__7.<>p__0, obj3);
                        if (<>o__7.<>p__2 == null)
                        {
                            <>o__7.<>p__2 = CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(Newtonsoft.Json.Linq.JObject), typeof(Hanon)));
                        }
                        if (<>o__7.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__7.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Hanon), argumentInfo));
                        }
                        Newtonsoft.Json.Linq.JObject obj5 = <>o__7.<>p__2.Target(<>o__7.<>p__2, <>o__7.<>p__1.Target(<>o__7.<>p__1, obj4, "1"));
                        if (<>o__7.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__7.<>p__4 = CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Hanon), argumentInfo));
                        }
                        if (<>o__7.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__7.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(Hanon), argumentInfo));
                        }
                        KeyValuePair<string, string> item = <>o__7.<>p__4.Target(<>o__7.<>p__4, typeof(KeyValuePair<string, string>), obj5["description"].ToString(), <>o__7.<>p__3.Target(<>o__7.<>p__3, obj3));
                        this._runner.Product.AvailableSizes.Add(item);
                    }
                    string str7 = "available sizes:\n";
                    for (int i = 0; i < this._runner.Product.AvailableSizes.Count; i++)
                    {
                        KeyValuePair<string, string> pair2;
                        if (i == (this._runner.Product.AvailableSizes.Count - 1))
                        {
                            string[] textArray1 = new string[6];
                            textArray1[0] = str7;
                            textArray1[1] = "\t";
                            pair2 = this._runner.Product.AvailableSizes[i];
                            textArray1[2] = pair2.Key;
                            textArray1[3] = " (";
                            pair2 = this._runner.Product.AvailableSizes[i];
                            textArray1[4] = pair2.Value;
                            textArray1[5] = ")";
                            str7 = string.Concat(textArray1);
                        }
                        else
                        {
                            string[] textArray2 = new string[6];
                            textArray2[0] = str7;
                            textArray2[1] = "\t";
                            pair2 = this._runner.Product.AvailableSizes[i];
                            textArray2[2] = pair2.Key;
                            textArray2[3] = " (";
                            pair2 = this._runner.Product.AvailableSizes[i];
                            textArray2[4] = pair2.Value;
                            textArray2[5] = ")\n";
                            str7 = string.Concat(textArray2);
                        }
                    }
                    if (this._runner.Product.AvailableSizes.Count == 0)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': product out of stock");
                        return false;
                    }
                    if (Global.SETTINGS.DetailedLog)
                    {
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {str7}");
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
                        double num4 = double.Parse(str9.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                        if ((num4 < this._task.MinimumPrice) || (num4 > this._task.MaximumPrice))
                        {
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': price check didn't PASS (product price: '{this._runner.Product.Price}', minimum: '{this._task.MinimumPrice.ToString()}', maximum: '{this._task.MaximumPrice.ToString()}')");
                            return false;
                        }
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': price check PASSED (product price: '{this._runner.Product.Price}', minimum: '{this._task.MinimumPrice.ToString()}', maximum: '{this._task.MaximumPrice.ToString()}')");
                    }
                    bool flag4 = true;
                    string requestUriString = "";
                    while (flag4)
                    {
                        flag4 = false;
                        try
                        {
                            requestUriString = "https://www.hanon-shop.com/gateway/ajaxGateway.php?mode=cart&_module=shop";
                            request2 = (HttpWebRequest) WebRequest.Create(requestUriString);
                            request2.KeepAlive = true;
                            request2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                            request2.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request2.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                            request2.Accept = "application/json";
                            request2.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                            request2.CookieContainer = this._runner.Cookies;
                            request2.Referer = this._runner.Product.Link;
                            request2.Headers.Add("X-Requested-With", "XMLHttpRequest");
                            request2.Headers.Add("X-Request", "JSON");
                            if (this._runner.Proxy != null)
                            {
                                request2.Proxy = this._runner.Proxy;
                            }
                            html = "";
                            using (HttpWebResponse response3 = (HttpWebResponse) request2.GetResponse())
                            {
                                html = new StreamReader(response3.GetResponseStream()).ReadToEnd();
                            }
                            continue;
                        }
                        catch (WebException exception)
                        {
                            if (exception.Message.Contains("404") || exception.Message.Contains("407"))
                            {
                                throw;
                            }
                            Global.Logger.Error($"Site crash ({requestUriString} - {this._task.Guid})", exception);
                            if (Global.SETTINGS.DetailedLog)
                            {
                                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': {"web crash detected, repeating request in 1.5s"}");
                            }
                            Thread.Sleep(0x5dc);
                            flag4 = true;
                            continue;
                        }
                    }
                    if (!this._task.RandomSize)
                    {
                        char[] separator = new char[] { '#' };
                        string[] strArray = this._task.Size.Split(separator);
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            strArray[j] = strArray[j].Trim().ToUpperInvariant();
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
                                    current = enumerator2.Current;
                                    List<string> source = new List<string>();
                                    if (current.Key.Contains(":"))
                                    {
                                        char[] chArray2 = new char[] { ':' };
                                        string[] strArray3 = current.Key.Split(chArray2);
                                        for (int m = 0; m < strArray3.Length; m++)
                                        {
                                            source.Add(strArray3[m].Trim());
                                        }
                                    }
                                    else
                                    {
                                        source.Add(current.Key);
                                    }
                                    for (int k = 0; k < source.Count; k++)
                                    {
                                        source[k] = source[k].Trim().ToUpperInvariant();
                                    }
                                    if (source.Any<string>(x => x == sz))
                                    {
                                        goto Label_0E0C;
                                    }
                                }
                                continue;
                            Label_0E0C:
                                this._runner.PickedSize = new KeyValuePair<string, string>?(current);
                            }
                        }
                        if (this._runner.PickedSize.HasValue)
                        {
                            EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': picked size '{this._runner.PickedSize.Value.Key}'");
                            return true;
                        }
                        EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': defined size not available");
                        if (!this._task.SizePickRandom)
                        {
                            return false;
                        }
                    }
                    this._runner.PickedSize = new KeyValuePair<string, string>?(this._runner.Product.AvailableSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableSizes.Count)]);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': picked random size '{this._runner.PickedSize.Value.Key}'");
                    return true;
                }
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': product out of stock");
                return false;
            }
            catch (Exception exception2)
            {
                if (!exception2.Message.Contains("Thread was being aborted"))
                {
                    string str11 = "";
                    if (exception2.GetType() == typeof(WebException))
                    {
                        str11 = " - " + exception2.Message;
                    }
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error checking products availability{str11}");
                    Global.Logger.Error($"Error checking products availability of task '{this._task.Name} - {this._task.Guid}'", exception2);
                    return false;
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
                bool flag = false;
                string link = "";
                foreach (string str3 in this._task.Keywords)
                {
                    string url = $"https://www.hanon-shop.com/s?q={str3.Replace(" ", "+").ToLowerInvariant()}";
                    bool flag2 = true;
                    while (flag2)
                    {
                        flag2 = false;
                        try
                        {
                            try
                            {
                                CloudFlareSolver solver1 = new CloudFlareSolver(url, this._runner.Cookies, true, "https://www.hanon-shop.com", true, this._runner.Proxy);
                                solver1.Solve();
                                solver1.SendChallengeRequest();
                                HttpWebRequest request = solver1.CreateWebRequest(url);
                                if (this._runner.Proxy != null)
                                {
                                    request.Proxy = this._runner.Proxy;
                                }
                                using (WebResponse response = request.GetResponse())
                                {
                                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                    {
                                        html = reader.ReadToEnd();
                                    }
                                    link = response.ResponseUri.ToString();
                                }
                            }
                            catch (Exception)
                            {
                                flag = true;
                            }
                            if (flag)
                            {
                                HttpWebRequest request2 = (HttpWebRequest) WebRequest.Create(url);
                                request2.KeepAlive = true;
                                request2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                                request2.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                                request2.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                                request2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                                request2.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                                request2.CookieContainer = this._runner.Cookies;
                                if (this._runner.Proxy != null)
                                {
                                    request2.Proxy = this._runner.Proxy;
                                }
                                html = "";
                                using (HttpWebResponse response2 = (HttpWebResponse) request2.GetResponse())
                                {
                                    html = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                                    link = response2.ResponseUri.ToString();
                                }
                            }
                            continue;
                        }
                        catch (Exception exception2)
                        {
                            if (exception2.Message.Contains("The remote server returned an error: (503) Server Unavailable"))
                            {
                                if (Global.SETTINGS.DetailedLog)
                                {
                                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': CloudFlare computation failed. Re-trying in 2 seconds");
                                }
                                Thread.Sleep(0x7d0);
                                flag2 = true;
                            }
                            continue;
                        }
                    }
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(html);
                    Global.Logger.Debug($"Task '{this._task.Name} -{this._task.Guid}:' {url + " : " + html}");
                    if (!html.ToLowerInvariant().Contains("Product Code:".ToLowerInvariant()))
                    {
                        if (!document.DocumentNode.Descendants("div").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "result-products"))))
                        {
                            continue;
                        }
                        using (IEnumerator<HtmlNode> enumerator2 = (from x in document.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "result-products"))).Descendants("div")
                            where (x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("product col3")
                            select x).GetEnumerator())
                        {
                            HtmlNode node;
                            goto Label_03BA;
                        Label_0372:
                            node = enumerator2.Current;
                            string str5 = "https://www.hanon-shop.com" + node.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                            if (this.DirectLinkInternal(str5, true))
                            {
                                return true;
                            }
                        Label_03BA:
                            if (!enumerator2.MoveNext())
                            {
                                continue;
                            }
                            goto Label_0372;
                        }
                    }
                    if (this.DirectLinkInternal(link, true))
                    {
                        return true;
                    }
                }
                EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': no products found matching the criteria");
                return false;
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Thread was being aborted"))
                {
                    string str6 = "";
                    if (exception.GetType() == typeof(WebException))
                    {
                        str6 = " - " + exception.Message;
                    }
                    this._runner.IsError = true;
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    EveAIO.Helpers.WriteLog($"Task '{this._task.Name}': error searching for products{str6}");
                    Global.Logger.Error($"Error searching for products of task '{this._task.Name} - {this._task.Guid}'", exception);
                    return false;
                }
                return false;
            }
        }

        public void SetClient()
        {
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Hanon.<>c <>9;
            public static Func<HtmlNode, bool> <>9__5_1;
            public static Func<HtmlNode, bool> <>9__7_0;
            public static Func<HtmlNode, bool> <>9__7_1;
            public static Func<HtmlNode, bool> <>9__7_2;
            public static Func<HtmlNode, bool> <>9__7_3;
            public static Func<HtmlNode, bool> <>9__9_0;
            public static Func<HtmlNode, bool> <>9__9_1;
            public static Func<HtmlNode, bool> <>9__9_2;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Hanon.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Checkout>b__5_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "sagepay"));

            internal bool <DirectLinkInternal>b__7_0(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "name"));

            internal bool <DirectLinkInternal>b__7_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "prod-price"));

            internal bool <DirectLinkInternal>b__7_2(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "description"));

            internal bool <DirectLinkInternal>b__7_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "description"));

            internal bool <Search>b__9_0(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "result-products"));

            internal bool <Search>b__9_1(HtmlNode x) => 
                ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "result-products"));

            internal bool <Search>b__9_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("product col3"));
        }

        [CompilerGenerated]
        private static class <>o__4
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, int, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__7
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, Newtonsoft.Json.Linq.JObject>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, Type, string, object, KeyValuePair<string, string>>> <>p__4;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__5;
        }
    }
}

