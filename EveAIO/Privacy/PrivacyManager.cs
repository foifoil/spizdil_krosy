namespace EveAIO.Privacy
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Pocos;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows;

    internal class PrivacyManager
    {
        public static object PrivacyCaptchaKey;
        public bool IsLoggedIn;
        public ObservableCollection<PrivacyCard> Cards;
        private string _email;
        private string _password;
        private CookieContainer _cookies;
        private string _loginToken;
        private object _loginId;

        static PrivacyManager()
        {
            Class7.RIuqtBYzWxthF();
            PrivacyCaptchaKey = "6LcrpQ0UAAAAAISIzEbWTqNyRV7mrknUQM1wg9QH";
        }

        public PrivacyManager()
        {
            Class7.RIuqtBYzWxthF();
            this._cookies = new CookieContainer();
            this._email = Global.SETTINGS.PrivacyEmail;
            this._password = Global.SETTINGS.PrivacyPassword;
        }

        internal string CreateCard(string name, PrivacyCardType type, int? limit)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://privacy.com/api/v1/card");
                request.CookieContainer = this._cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Accept = "application/json, text/plain, */*";
                request.KeepAlive = true;
                request.Headers.Add("Authorization: Bearer " + this._loginToken);
                request.Method = "POST";
                request.Referer = "https://privacy.com/home";
                request.ContentType = "application/json;charset=UTF-8";
                string s = "{\"panWithSpaces\":\"XXXX XXXX XXXX XXXX\",\"expMonth\":\"XX\",\"expYear\":\"XXXX\",\"CVV\":\"XXX\",";
                s = (((s + "\"reloadable\":" + ((type == PrivacyCardType.merchant) ? "true" : "false") + ",") + "\"spendLimit\":" + (!limit.HasValue ? "null" : limit.Value.ToString()) + ",") + "\"spendLimitDuration\":\"MONTHLY\",") + "\"memo\":\"" + name + "\"}";
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                string str2 = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        str2 = reader.ReadToEnd();
                    }
                }
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str2);
                PrivacyCard card2 = new PrivacyCard();
                if (<>o__12.<>p__3 == null)
                {
                    <>o__12.<>p__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__12.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "card", typeof(PrivacyManager), argumentInfo));
                }
                card2.CardId = <>o__12.<>p__3.Target(<>o__12.<>p__3, <>o__12.<>p__2.Target(<>o__12.<>p__2, <>o__12.<>p__1.Target(<>o__12.<>p__1, <>o__12.<>p__0.Target(<>o__12.<>p__0, obj2), "cardID")));
                if (<>o__12.<>p__7 == null)
                {
                    <>o__12.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__12.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "card", typeof(PrivacyManager), argumentInfo));
                }
                card2.CardNumber = <>o__12.<>p__7.Target(<>o__12.<>p__7, <>o__12.<>p__6.Target(<>o__12.<>p__6, <>o__12.<>p__5.Target(<>o__12.<>p__5, <>o__12.<>p__4.Target(<>o__12.<>p__4, obj2), "PAN")));
                if (<>o__12.<>p__11 == null)
                {
                    <>o__12.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__12.<>p__10 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__9 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "card", typeof(PrivacyManager), argumentInfo));
                }
                card2.Cvv = <>o__12.<>p__11.Target(<>o__12.<>p__11, <>o__12.<>p__10.Target(<>o__12.<>p__10, <>o__12.<>p__9.Target(<>o__12.<>p__9, <>o__12.<>p__8.Target(<>o__12.<>p__8, obj2), "CVV")));
                if (<>o__12.<>p__15 == null)
                {
                    <>o__12.<>p__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__12.<>p__14 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__13 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__12 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "card", typeof(PrivacyManager), argumentInfo));
                }
                card2.ExpMonth = <>o__12.<>p__15.Target(<>o__12.<>p__15, <>o__12.<>p__14.Target(<>o__12.<>p__14, <>o__12.<>p__13.Target(<>o__12.<>p__13, <>o__12.<>p__12.Target(<>o__12.<>p__12, obj2), "expMonth")));
                if (<>o__12.<>p__19 == null)
                {
                    <>o__12.<>p__19 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__12.<>p__18 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "card", typeof(PrivacyManager), argumentInfo));
                }
                card2.ExpYear = <>o__12.<>p__19.Target(<>o__12.<>p__19, <>o__12.<>p__18.Target(<>o__12.<>p__18, <>o__12.<>p__17.Target(<>o__12.<>p__17, <>o__12.<>p__16.Target(<>o__12.<>p__16, obj2), "expYear")));
                if (<>o__12.<>p__23 == null)
                {
                    <>o__12.<>p__23 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__12.<>p__22 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__21 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__12.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__12.<>p__20 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__12.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "card", typeof(PrivacyManager), argumentInfo));
                }
                card2.Name = <>o__12.<>p__23.Target(<>o__12.<>p__23, <>o__12.<>p__22.Target(<>o__12.<>p__22, <>o__12.<>p__21.Target(<>o__12.<>p__21, <>o__12.<>p__20.Target(<>o__12.<>p__20, obj2), "memo")));
                PrivacyCard item = card2;
                this.Cards.Add(item);
                return item.CardId;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error occured while creating Privacy card", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error while creating Privacy card", exception);
                return "";
            }
        }

        internal bool DeleteCard(string cardId)
        {
            try
            {
                HttpWebRequest request1 = (HttpWebRequest) WebRequest.Create($"https://privacy.com/api/v1/card/{cardId}/close");
                request1.CookieContainer = this._cookies;
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request1.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request1.Headers.Add("Accept-Encoding", "gzip, deflate");
                request1.Accept = "application/json, text/plain, */*";
                request1.KeepAlive = true;
                request1.Headers.Add("Authorization: Bearer " + this._loginToken);
                request1.Method = "POST";
                request1.Referer = "https://privacy.com/home";
                request1.ContentType = "application/json;charset=UTF-8";
                string str = "";
                using (WebResponse response = request1.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                return (str == "{}");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error occured while deleting Privacy card", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error while deleting Privacy card", exception);
                return false;
            }
        }

        public void LoadCards()
        {
            try
            {
                HttpWebRequest request1 = (HttpWebRequest) WebRequest.Create("https://privacy.com/api/v1/card/");
                request1.CookieContainer = this._cookies;
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request1.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request1.Headers.Add("Accept-Encoding", "gzip, deflate");
                request1.Accept = "application/json, text/plain, */*";
                request1.KeepAlive = true;
                request1.Referer = "https://privacy.com/login";
                request1.Headers.Add("Authorization: Bearer " + this._loginToken);
                string str = "";
                using (WebResponse response = request1.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                this.Cards = new ObservableCollection<PrivacyCard>();
                if (<>o__11.<>p__23 == null)
                {
                    <>o__11.<>p__23 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(PrivacyManager)));
                }
                if (<>o__11.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__11.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "cardList", typeof(PrivacyManager), argumentInfo));
                }
                foreach (object obj3 in <>o__11.<>p__23.Target(<>o__11.<>p__23, <>o__11.<>p__0.Target(<>o__11.<>p__0, obj2)))
                {
                    if (<>o__11.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(PrivacyManager), argumentInfo));
                    }
                    if (<>o__11.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__11.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(PrivacyManager), argumentInfo));
                    }
                    if (<>o__11.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__11.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                    }
                    if (<>o__11.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__11.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                    }
                    if (!<>o__11.<>p__4.Target(<>o__11.<>p__4, <>o__11.<>p__3.Target(<>o__11.<>p__3, <>o__11.<>p__2.Target(<>o__11.<>p__2, <>o__11.<>p__1.Target(<>o__11.<>p__1, obj3, "state")), "OPEN")))
                    {
                        PrivacyCard item = new PrivacyCard();
                        if (<>o__11.<>p__7 == null)
                        {
                            <>o__11.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                        }
                        if (<>o__11.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__11.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                        }
                        if (<>o__11.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__11.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                        }
                        item.CardId = <>o__11.<>p__7.Target(<>o__11.<>p__7, <>o__11.<>p__6.Target(<>o__11.<>p__6, <>o__11.<>p__5.Target(<>o__11.<>p__5, obj3, "cardID")));
                        if (<>o__11.<>p__10 == null)
                        {
                            <>o__11.<>p__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                        }
                        if (<>o__11.<>p__9 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__11.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                        }
                        if (<>o__11.<>p__8 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__11.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                        }
                        item.CardNumber = <>o__11.<>p__10.Target(<>o__11.<>p__10, <>o__11.<>p__9.Target(<>o__11.<>p__9, <>o__11.<>p__8.Target(<>o__11.<>p__8, obj3, "PAN")));
                        if (<>o__11.<>p__13 == null)
                        {
                            <>o__11.<>p__13 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                        }
                        if (<>o__11.<>p__12 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__11.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                        }
                        if (<>o__11.<>p__11 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__11.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                        }
                        item.Cvv = <>o__11.<>p__13.Target(<>o__11.<>p__13, <>o__11.<>p__12.Target(<>o__11.<>p__12, <>o__11.<>p__11.Target(<>o__11.<>p__11, obj3, "CVV")));
                        if (<>o__11.<>p__16 == null)
                        {
                            <>o__11.<>p__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                        }
                        if (<>o__11.<>p__15 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__11.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                        }
                        if (<>o__11.<>p__14 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__11.<>p__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                        }
                        item.ExpMonth = <>o__11.<>p__16.Target(<>o__11.<>p__16, <>o__11.<>p__15.Target(<>o__11.<>p__15, <>o__11.<>p__14.Target(<>o__11.<>p__14, obj3, "expMonth")));
                        if (<>o__11.<>p__19 == null)
                        {
                            <>o__11.<>p__19 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                        }
                        if (<>o__11.<>p__18 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__11.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                        }
                        if (<>o__11.<>p__17 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__11.<>p__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                        }
                        item.ExpYear = <>o__11.<>p__19.Target(<>o__11.<>p__19, <>o__11.<>p__18.Target(<>o__11.<>p__18, <>o__11.<>p__17.Target(<>o__11.<>p__17, obj3, "expYear")));
                        if (<>o__11.<>p__22 == null)
                        {
                            <>o__11.<>p__22 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                        }
                        if (<>o__11.<>p__21 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__11.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                        }
                        if (<>o__11.<>p__20 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__11.<>p__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                        }
                        item.Name = <>o__11.<>p__22.Target(<>o__11.<>p__22, <>o__11.<>p__21.Target(<>o__11.<>p__21, <>o__11.<>p__20.Target(<>o__11.<>p__20, obj3, "memo")));
                        this.Cards.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error occured while loading Privacy cards", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error while loading Privacy cards", exception);
            }
        }

        public void Login()
        {
            try
            {
                object obj3;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://privacy.com/auth/local");
                request.CookieContainer = this._cookies;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Accept = "application/json, text/plain, */*";
                request.KeepAlive = true;
                request.Method = "POST";
                request.Referer = "https://privacy.com/login";
                request.ContentType = "application/json;charset=UTF-8";
                string s = (("{\"email\":\"" + Global.SETTINGS.PrivacyEmail + "\",") + "\"password\":\"" + Global.SETTINGS.PrivacyPassword + "\",") + "\"extensionInstalled\":false}";
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                string str2 = "";
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            str2 = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException exception)
                {
                    if (exception.Response == null)
                    {
                        throw exception;
                    }
                    string str3 = "";
                    using (Stream stream2 = exception.Response.GetResponseStream())
                    {
                        using (StreamReader reader2 = new StreamReader(stream2))
                        {
                            str3 = reader2.ReadToEnd();
                        }
                    }
                    if (str3.Contains("Please complete the captcha"))
                    {
                        request = (HttpWebRequest) WebRequest.Create("https://privacy.com/auth/local");
                        request.CookieContainer = this._cookies;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate");
                        request.Accept = "application/json, text/plain, */*";
                        request.KeepAlive = true;
                        request.Method = "POST";
                        request.Referer = "https://privacy.com/login";
                        request.ContentType = "application/json;charset=UTF-8";
                        string token = "";
                        switch (Global.SETTINGS.PrivacyCaptchaService)
                        {
                            case CaptchaServiceType.TwoCaptcha:
                                token = CaptchaSolver.TwoCaptchaSolve(PrivacyCaptchaKey, "http://privacy.com", "");
                                break;

                            case CaptchaServiceType.AntiCaptcha:
                                token = CaptchaSolver.AntiCaptchaSolve(PrivacyCaptchaKey, "http://privacy.com", "");
                                break;

                            case CaptchaServiceType.ImageTypers:
                                token = CaptchaSolver.ImageTypersSolve(PrivacyCaptchaKey, "http://privacy.com", "");
                                break;

                            case CaptchaServiceType.Disolve:
                                token = CaptchaSolver.DisolveSolve(PrivacyCaptchaKey, "http://privacy.com", "");
                                break;

                            case CaptchaServiceType.Manual:
                            {
                                TaskObject task = new TaskObject {
                                    Id = Guid.NewGuid().ToString(),
                                    Platform = TaskObject.PlatformEnum.privacy
                                };
                                object solverLocker = Global.SolverLocker;
                                lock (solverLocker)
                                {
                                    Global.CAPTCHA_QUEUE.Add(task);
                                }
                                task.Mre = new ManualResetEvent(false);
                                CaptchaWaiter waiter = new CaptchaWaiter(task, new DateTime?(DateTime.Now), WebsitesInfo.PRIVACY_CAPTCHA_KEY, "https://privacy.com", "Privacy");
                                waiter.Start();
                                task.Mre.WaitOne();
                                if (Global.CAPTCHA_QUEUE.Any<TaskObject>(x => x.Id == task.Id))
                                {
                                    solverLocker = Global.SolverLocker;
                                    lock (solverLocker)
                                    {
                                        Global.CAPTCHA_QUEUE.Remove(Global.CAPTCHA_QUEUE.First<TaskObject>(x => x.Id == task.Id));
                                        task.ManualSolved = false;
                                    }
                                }
                                token = waiter.Token;
                                break;
                            }
                        }
                        s = ((("{\"email\":\"" + Global.SETTINGS.PrivacyEmail + "\",") + "\"password\":\"" + Global.SETTINGS.PrivacyPassword + "\",") + "\"captchaResponse\":\"" + token + "\",") + "\"extensionInstalled\":false}";
                        bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream3 = request.GetRequestStream())
                        {
                            stream3.Write(bytes, 0, bytes.Length);
                        }
                        str2 = "";
                        using (WebResponse response2 = request.GetResponse())
                        {
                            using (StreamReader reader3 = new StreamReader(response2.GetResponseStream()))
                            {
                                str2 = reader3.ReadToEnd();
                            }
                            goto Label_04D1;
                        }
                    }
                    throw exception;
                }
            Label_04D1:
                obj3 = Newtonsoft.Json.JsonConvert.DeserializeObject(str2);
                if (<>o__10.<>p__2 == null)
                {
                    <>o__10.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__10.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__10.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                this._loginToken = <>o__10.<>p__2.Target(<>o__10.<>p__2, <>o__10.<>p__1.Target(<>o__10.<>p__1, <>o__10.<>p__0.Target(<>o__10.<>p__0, obj3, "token")));
                if (<>o__10.<>p__5 == null)
                {
                    <>o__10.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PrivacyManager)));
                }
                if (<>o__10.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__10.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(PrivacyManager), argumentInfo));
                }
                if (<>o__10.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__10.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(PrivacyManager), argumentInfo));
                }
                this._loginId = <>o__10.<>p__5.Target(<>o__10.<>p__5, <>o__10.<>p__4.Target(<>o__10.<>p__4, <>o__10.<>p__3.Target(<>o__10.<>p__3, obj3, "id")));
                this.IsLoggedIn = true;
            }
            catch (Exception exception2)
            {
                MessageBox.Show("Error occured while trying to log in", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error while loging in to Privacy.com", exception2);
            }
        }

        [CompilerGenerated]
        private static class <>o__10
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string>> <>p__5;
        }

        [CompilerGenerated]
        private static class <>o__11
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
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string>> <>p__13;
            public static CallSite<Func<CallSite, object, string, object>> <>p__14;
            public static CallSite<Func<CallSite, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string>> <>p__19;
            public static CallSite<Func<CallSite, object, string, object>> <>p__20;
            public static CallSite<Func<CallSite, object, object>> <>p__21;
            public static CallSite<Func<CallSite, object, string>> <>p__22;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__23;
        }

        [CompilerGenerated]
        private static class <>o__12
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, string, object>> <>p__17;
            public static CallSite<Func<CallSite, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, string>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string>> <>p__23;
        }

        public enum PrivacyCardType
        {
            merchant,
            burner
        }
    }
}

