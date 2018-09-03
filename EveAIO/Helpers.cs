namespace EveAIO
{
    using Discord;
    using Discord.Commands;
    using Discord.Webhook;
    using EveAIO.Pocos;
    using EveAIO.Properties;
    using EveAIO.Tasks;
    using EveAIO.Tasks.Dto;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Management;
    using System.Media;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml.Serialization;

    internal static class Helpers
    {
        internal static List<string> KEYS;
        private static XmlSerializer _serSettings;
        private static Random _rnd;

        static Helpers()
        {
            Class7.RIuqtBYzWxthF();
            List<string> list1 = new List<string> { 
                "RVZFLVpQMzMxTFlBWDE3UTJU",
                "RVZFLTVXM0gxTEs5VjE1WVVR",
                "RVZFLVIzTEcxR1VYTTFNVkJB",
                "RVZFLVhQSFgxUjVNTjFXSEIz",
                "RVZFLUE3VDMxRkhCUjFBQ1ZS",
                "RVZFLVlWRDIxSFRNNjFLQUZa",
                "RVZFLTlWRDIxSFRNNjFLQUZa",
                "RVZFLUE3VDMxRkhCUjFBQ1ZSQQ==",
                "RVZFLVIzTEcxR1VYTTFNVkJB",
                "RVZFLVhQSFgxUjVNTjFXSEIz",
                "RVZFLTlNRFoxSkZCMzFXQlVZ",
                "RVZFLUJSWEMxWkJITjFMSDZD",
                "RVZFLTRLMjUxVFdEWTE3RVNR",
                "RVZFLThQMzMxTFlBWDE3UTJU",
                "RVZFLVlWRDIxSFRNNjFLQUZa"
            };
            KEYS = list1;
            _serSettings = new XmlSerializer(typeof(EveAIO.Settings));
            _rnd = new Random(DateTime.Now.Millisecond);
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(<>c.<>9.<.cctor>b__1_0));
            string dbValue = GetDbValue("api_tokens");
            if (!string.IsNullOrEmpty(dbValue))
            {
                object obj3 = Newtonsoft.Json.JsonConvert.DeserializeObject(dbValue);
                if (<>o__1.<>p__5 == null)
                {
                    <>o__1.<>p__5 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(EveAIO.Helpers)));
                }
                foreach (object obj2 in <>o__1.<>p__5.Target(<>o__1.<>p__5, obj3))
                {
                    if (<>o__1.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__1.<>p__4 = CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__1.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__1.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__1.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__1.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__1.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__1.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__1.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__1.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    Global.SHOPIFY_API_TOKENS.Add(<>o__1.<>p__4.Target(<>o__1.<>p__4, typeof(KeyValuePair<string, string>), <>o__1.<>p__1.Target(<>o__1.<>p__1, <>o__1.<>p__0.Target(<>o__1.<>p__0, obj2, "website")), <>o__1.<>p__3.Target(<>o__1.<>p__3, <>o__1.<>p__2.Target(<>o__1.<>p__2, obj2, "token"))));
                }
            }
            string str = GetDbValue("kith_props");
            if (string.IsNullOrEmpty(str))
            {
                WebsitesInfo.KITH_PROPERTIES = WebsitesInfo.KITH_PROPERTIES_DOWN;
            }
            else
            {
                char[] separator = new char[] { '=' };
                string[] strArray = str.Split(separator);
                WebsitesInfo.KITH_PROPERTIES = new KeyValuePair<string, string>(strArray[0], strArray[1]);
            }
        }

        internal static void AddCheckoutLink(string link, string domain, string product, string size)
        {
            Task.Factory.StartNew(delegate {
                try
                {
                    if ((!string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(domain)) && !string.IsNullOrEmpty(product))
                    {
                        product = product.Replace(@"\xe2", "").Replace(@"\x80", "").Replace(@"\x99S", "").Replace(@"\x93", "");
                        string str = Encrypt("{\"call_id\":\"" + Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", \"data\": \"{'link':'" + link + "', 'domain_name':'" + domain.Replace("http://", "").Replace("https://", "") + "', 'product_name':'" + product.Replace("'", "").Replace("\"", "") + "', 'product_size':'" + size.Replace("'", "").Replace("\"", "").ToUpperInvariant() + "'}\"}");
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://everoboticslm.herokuapp.com/aio_links/add");
                        string s = "call=" + WebUtility.UrlEncode(str);
                        request.UserAgent = "python-requests/2.14.2";
                        request.Accept = "*/*";
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = s.Length;
                        request.KeepAlive = true;
                        request.ServicePoint.Expect100Continue = false;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate");
                        byte[] bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                        string cypher = "";
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                cypher = reader.ReadToEnd();
                            }
                        }
                        object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(Decrypt(cypher));
                        if (<>o__28.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__28.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__28.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__28.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__28.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__28.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__28.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__28.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__28.<>p__3.Target(<>o__28.<>p__3, <>o__28.<>p__2.Target(<>o__28.<>p__2, <>o__28.<>p__1.Target(<>o__28.<>p__1, <>o__28.<>p__0.Target(<>o__28.<>p__0, obj2, "result")), "success")))
                        {
                            if (<>o__28.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__28.<>p__7 = CallSite<Action<CallSite, ILog, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Error", null, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__28.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__28.<>p__6 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__28.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__28.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__28.<>p__4 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__28.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            <>o__28.<>p__7.Target(<>o__28.<>p__7, Global.Logger, <>o__28.<>p__6.Target(<>o__28.<>p__6, "Error adding checkout link - ", <>o__28.<>p__5.Target(<>o__28.<>p__5, <>o__28.<>p__4.Target(<>o__28.<>p__4, obj2, "debug"))));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error adding checkout link", exception);
                }
            });
        }

        internal static void AddDbValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Task.Factory.StartNew(delegate {
                    try
                    {
                        string str = Encrypt("{\"call_id\":\"" + Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", \"data\": \"{'name':'" + Guid.NewGuid().ToString() + "', 'value':'" + value + "'}\"}");
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://everoboticslm.herokuapp.com/aio_store/add");
                        string s = "call=" + WebUtility.UrlEncode(str);
                        request.UserAgent = "python-requests/2.14.2";
                        request.Accept = "*/*";
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                        request.ContentLength = s.Length;
                        request.KeepAlive = true;
                        request.ServicePoint.Expect100Continue = false;
                        request.Headers.Add("Accept-Encoding", "gzip, deflate");
                        byte[] bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                        string cypher = "";
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                cypher = reader.ReadToEnd();
                            }
                        }
                        object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(Decrypt(cypher));
                        if (<>o__32.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__32.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__32.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__32.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__32.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__32.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__32.<>p__3.Target(<>o__32.<>p__3, <>o__32.<>p__2.Target(<>o__32.<>p__2, <>o__32.<>p__1.Target(<>o__32.<>p__1, <>o__32.<>p__0.Target(<>o__32.<>p__0, obj2, "result")), "success")))
                        {
                            if (<>o__32.<>p__7 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__32.<>p__7 = CallSite<Action<CallSite, ILog, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Error", null, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__32.<>p__6 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__32.<>p__6 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__32.<>p__5 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__32.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__32.<>p__4 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__32.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            <>o__32.<>p__7.Target(<>o__32.<>p__7, Global.Logger, <>o__32.<>p__6.Target(<>o__32.<>p__6, "Error adding session - ", <>o__32.<>p__5.Target(<>o__32.<>p__5, <>o__32.<>p__4.Target(<>o__32.<>p__4, obj2, "debug"))));
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        internal static void AddSessionId(string sessionId, string domain)
        {
            Task.Factory.StartNew(delegate {
                try
                {
                    string str = Encrypt("{\"call_id\":\"" + Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", \"data\": \"{'session_id':'" + sessionId + "', 'website':'" + domain.Replace("http://", "").Replace("https://", "") + "'}\"}");
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://everoboticslm.herokuapp.com/aio_queue/add");
                    string s = "call=" + WebUtility.UrlEncode(str);
                    request.UserAgent = "python-requests/2.14.2";
                    request.Accept = "*/*";
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = s.Length;
                    request.KeepAlive = true;
                    request.ServicePoint.Expect100Continue = false;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    byte[] bytes = Encoding.ASCII.GetBytes(s);
                    request.ContentLength = bytes.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    string cypher = "";
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            cypher = reader.ReadToEnd();
                        }
                    }
                    object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(Decrypt(cypher));
                    if (<>o__29.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__29.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__29.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__29.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__29.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__29.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__29.<>p__0 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__29.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__29.<>p__3.Target(<>o__29.<>p__3, <>o__29.<>p__2.Target(<>o__29.<>p__2, <>o__29.<>p__1.Target(<>o__29.<>p__1, <>o__29.<>p__0.Target(<>o__29.<>p__0, obj2, "result")), "success")))
                    {
                        if (<>o__29.<>p__7 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__7 = CallSite<Action<CallSite, ILog, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Error", null, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__29.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__6 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__29.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__29.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__29.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__29.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        <>o__29.<>p__7.Target(<>o__29.<>p__7, Global.Logger, <>o__29.<>p__6.Target(<>o__29.<>p__6, "Error adding session - ", <>o__29.<>p__5.Target(<>o__29.<>p__5, <>o__29.<>p__4.Target(<>o__29.<>p__4, obj2, "debug"))));
                    }
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error adding session", exception);
                }
            });
        }

        public static ProxyObject CheckProxyValidity(string proxyStr)
        {
            char[] separator = new char[] { ':' };
            string[] strArray = proxyStr.Split(separator);
            if ((strArray.Length != 2) && (strArray.Length != 4))
            {
                return null;
            }
            if (!int.TryParse(strArray[1], out _))
            {
                return null;
            }
            ProxyObject obj2 = new ProxyObject {
                Id = Guid.NewGuid().ToString(),
                IP = strArray[0],
                Port = int.Parse(strArray[1])
            };
            if (strArray.Length == 4)
            {
                obj2.Username = strArray[2];
                obj2.Password = strArray[3];
            }
            return obj2;
        }

        internal static string CreateSuccessRequest(string data) => 
            (("{\"call_id\":\"" + Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", ") + "\"data\":\"" + data + "\"}");

        internal static void Deactivate()
        {
            try
            {
                string str = Encrypt(Global.CreateRequest("deactivate").Replace("{0}", Global.SERIAL));
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://everoboticslm.herokuapp.com/lm");
                string s = "call=" + WebUtility.UrlEncode(str);
                request.UserAgent = "python-requests/2.14.2";
                request.Accept = "*/*";
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = s.Length;
                request.KeepAlive = true;
                request.ServicePoint.Expect100Continue = false;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        reader.ReadToEnd();
                    }
                }
                System.IO.File.Delete("license.key");
                MessageBox.Show("License deactivated", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Environment.Exit(0);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error license deactivation", "Error occured", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error license deactivation", exception);
            }
        }

        internal static string Decrypt(string cypher)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            AesManaged managed1 = new AesManaged {
                Key = encoding.GetBytes(Global.AES_KEY),
                IV = encoding.GetBytes(Global.AES_IV)
            };
            byte[] inputBuffer = Convert.FromBase64String(cypher);
            byte[] bytes = managed1.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return encoding.GetString(bytes);
        }

        internal static string Encrypt(string text)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            AesManaged managed1 = new AesManaged {
                Key = encoding.GetBytes(Global.AES_KEY),
                IV = encoding.GetBytes(Global.AES_IV)
            };
            byte[] bytes = encoding.GetBytes(text);
            return Convert.ToBase64String(managed1.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
        }

        internal static string ExtractNumbers(string input)
        {
            string str = "";
            foreach (char ch in input)
            {
                if (char.IsDigit(ch))
                {
                    str = str + ch.ToString();
                }
            }
            return str;
        }

        internal static List<CheckoutLink> GetCheckoutLinks(string domain)
        {
            try
            {
                string str = Encrypt("{\"call_id\":\"" + Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", \"data\": \"{'domain':'" + domain.Replace("http://", "").Replace("https://", "") + "', 'interval': 'week'}\"}");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://everoboticslm.herokuapp.com/aio_links/get");
                string s = "call=" + WebUtility.UrlEncode(str);
                request.UserAgent = "python-requests/2.14.2";
                request.Accept = "*/*";
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = s.Length;
                request.KeepAlive = true;
                request.ServicePoint.Expect100Continue = false;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                string cypher = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        cypher = reader.ReadToEnd();
                    }
                }
                string str4 = Decrypt(cypher);
                if (str4.Contains("No links found"))
                {
                    return null;
                }
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str4.Replace(@"\xe2", "").Replace(@"\x80", "").Replace(@"\x99S", "").Replace(@"\x93", ""));
                if (<>o__30.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__30.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__30.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__30.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__30.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__30.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__30.<>p__3.Target(<>o__30.<>p__3, <>o__30.<>p__2.Target(<>o__30.<>p__2, <>o__30.<>p__1.Target(<>o__30.<>p__1, <>o__30.<>p__0.Target(<>o__30.<>p__0, obj2, "result")), "success")))
                {
                    if (<>o__30.<>p__7 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__7 = CallSite<Action<CallSite, ILog, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Error", null, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__30.<>p__6 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__6 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__30.<>p__5 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__30.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    <>o__30.<>p__7.Target(<>o__30.<>p__7, Global.Logger, <>o__30.<>p__6.Target(<>o__30.<>p__6, "Error getting checkout links - ", <>o__30.<>p__5.Target(<>o__30.<>p__5, <>o__30.<>p__4.Target(<>o__30.<>p__4, obj2, "debug"))));
                    return null;
                }
                List<CheckoutLink> source = new List<CheckoutLink>();
                if (<>o__30.<>p__43 == null)
                {
                    <>o__30.<>p__43 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(EveAIO.Helpers)));
                }
                if (<>o__30.<>p__8 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__30.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "links", typeof(EveAIO.Helpers), argumentInfo));
                }
                foreach (object rec in <>o__30.<>p__43.Target(<>o__30.<>p__43, <>o__30.<>p__8.Target(<>o__30.<>p__8, obj2)))
                {
                    if (<>o__30.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__30.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__11 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "IsNullOrEmpty", null, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__30.<>p__10 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__30.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (<>o__30.<>p__9 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__30.<>p__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                    }
                    if (!<>o__30.<>p__12.Target(<>o__30.<>p__12, <>o__30.<>p__11.Target(<>o__30.<>p__11, typeof(string), <>o__30.<>p__10.Target(<>o__30.<>p__10, <>o__30.<>p__9.Target(<>o__30.<>p__9, rec, "url")))))
                    {
                        if (source.Any<CheckoutLink>(delegate (CheckoutLink x) {
                            if (<>o__30.<>p__21 == null)
                            {
                                <>o__30.<>p__21 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(EveAIO.Helpers)));
                            }
                            if (<>o__30.<>p__15 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__15 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__14 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__13 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__30.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            object obj2 = <>o__30.<>p__15.Target(<>o__30.<>p__15, x.Name, <>o__30.<>p__14.Target(<>o__30.<>p__14, <>o__30.<>p__13.Target(<>o__30.<>p__13, rec, "product_name")));
                            if (<>o__30.<>p__20 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__20 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (!<>o__30.<>p__20.Target(<>o__30.<>p__20, obj2))
                            {
                                if (<>o__30.<>p__19 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__30.<>p__19 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(EveAIO.Helpers), argumentInfo));
                                }
                                if (<>o__30.<>p__18 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__30.<>p__18 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(EveAIO.Helpers), argumentInfo));
                                }
                                if (<>o__30.<>p__17 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__30.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                                }
                            }
                            return <>o__30.<>p__21.Target(<>o__30.<>p__21, (<>o__30.<>p__16 != null) ? obj2 : <>o__30.<>p__19.Target(<>o__30.<>p__19, obj2, <>o__30.<>p__18.Target(<>o__30.<>p__18, x.Size, <>o__30.<>p__17.Target(<>o__30.<>p__17, <>o__30.<>p__16.Target(<>o__30.<>p__16, rec, "size")))));
                        }))
                        {
                            CheckoutLink link = source.First<CheckoutLink>(delegate (CheckoutLink x) {
                                if (<>o__30.<>p__30 == null)
                                {
                                    <>o__30.<>p__30 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(EveAIO.Helpers)));
                                }
                                if (<>o__30.<>p__24 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__30.<>p__24 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(EveAIO.Helpers), argumentInfo));
                                }
                                if (<>o__30.<>p__23 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__30.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                                }
                                if (<>o__30.<>p__22 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                    <>o__30.<>p__22 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                                }
                                object obj2 = <>o__30.<>p__24.Target(<>o__30.<>p__24, x.Name, <>o__30.<>p__23.Target(<>o__30.<>p__23, <>o__30.<>p__22.Target(<>o__30.<>p__22, rec, "product_name")));
                                if (<>o__30.<>p__29 == null)
                                {
                                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                    <>o__30.<>p__29 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(EveAIO.Helpers), argumentInfo));
                                }
                                if (!<>o__30.<>p__29.Target(<>o__30.<>p__29, obj2))
                                {
                                    if (<>o__30.<>p__28 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__30.<>p__28 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(EveAIO.Helpers), argumentInfo));
                                    }
                                    if (<>o__30.<>p__27 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__30.<>p__27 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(EveAIO.Helpers), argumentInfo));
                                    }
                                    if (<>o__30.<>p__26 == null)
                                    {
                                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                        <>o__30.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                                    }
                                }
                                return <>o__30.<>p__30.Target(<>o__30.<>p__30, (<>o__30.<>p__25 != null) ? obj2 : <>o__30.<>p__28.Target(<>o__30.<>p__28, obj2, <>o__30.<>p__27.Target(<>o__30.<>p__27, x.Size, <>o__30.<>p__26.Target(<>o__30.<>p__26, <>o__30.<>p__25.Target(<>o__30.<>p__25, rec, "size")))));
                            });
                            if (<>o__30.<>p__33 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__33 = CallSite<Action<CallSite, List<string>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__32 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__31 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__30.<>p__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            <>o__30.<>p__33.Target(<>o__30.<>p__33, link.Links, <>o__30.<>p__32.Target(<>o__30.<>p__32, <>o__30.<>p__31.Target(<>o__30.<>p__31, rec, "url")));
                        }
                        else
                        {
                            CheckoutLink link3 = new CheckoutLink();
                            if (<>o__30.<>p__36 == null)
                            {
                                <>o__30.<>p__36 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(EveAIO.Helpers)));
                            }
                            if (<>o__30.<>p__35 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__34 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__30.<>p__34 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            link3.Name = <>o__30.<>p__36.Target(<>o__30.<>p__36, <>o__30.<>p__35.Target(<>o__30.<>p__35, <>o__30.<>p__34.Target(<>o__30.<>p__34, rec, "product_name")));
                            if (<>o__30.<>p__39 == null)
                            {
                                <>o__30.<>p__39 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(EveAIO.Helpers)));
                            }
                            if (<>o__30.<>p__38 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__37 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__30.<>p__37 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            link3.Size = <>o__30.<>p__39.Target(<>o__30.<>p__39, <>o__30.<>p__38.Target(<>o__30.<>p__38, <>o__30.<>p__37.Target(<>o__30.<>p__37, rec, "size")));
                            CheckoutLink item = link3;
                            if (<>o__30.<>p__42 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__42 = CallSite<Action<CallSite, List<string>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__41 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                                <>o__30.<>p__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                            }
                            if (<>o__30.<>p__40 == null)
                            {
                                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                                <>o__30.<>p__40 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                            }
                            <>o__30.<>p__42.Target(<>o__30.<>p__42, item.Links, <>o__30.<>p__41.Target(<>o__30.<>p__41, <>o__30.<>p__40.Target(<>o__30.<>p__40, rec, "url")));
                            source.Add(item);
                        }
                    }
                }
                if (source.Count > 0)
                {
                    source = (from x in source
                        orderby x.Name
                        orderby x.Size
                        select x).ToList<CheckoutLink>();
                }
                return source;
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error getting checkout links", exception);
                return null;
            }
        }

        internal static string GetDbValue(object value)
        {
            try
            {
                string str = Encrypt("{\"call_id\":\"" + Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", \"data\": \"{'name':'" + value + "'}\"}");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://everoboticslm.herokuapp.com/aio_store/get");
                string s = "call=" + WebUtility.UrlEncode(str);
                request.UserAgent = "python-requests/2.14.2";
                request.Accept = "*/*";
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = s.Length;
                request.KeepAlive = true;
                request.ServicePoint.Expect100Continue = false;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                string cypher = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        cypher = reader.ReadToEnd();
                    }
                }
                object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(Decrypt(cypher));
                if (<>o__33.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__33.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__33.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__33.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                }
                if (!<>o__33.<>p__3.Target(<>o__33.<>p__3, <>o__33.<>p__2.Target(<>o__33.<>p__2, <>o__33.<>p__1.Target(<>o__33.<>p__1, <>o__33.<>p__0.Target(<>o__33.<>p__0, obj2, "result")), "success")))
                {
                    return "";
                }
                if (<>o__33.<>p__6 == null)
                {
                    <>o__33.<>p__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(EveAIO.Helpers)));
                }
                if (<>o__33.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__33.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(EveAIO.Helpers), argumentInfo));
                }
                if (<>o__33.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__33.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(EveAIO.Helpers), argumentInfo));
                }
                return <>o__33.<>p__6.Target(<>o__33.<>p__6, <>o__33.<>p__5.Target(<>o__33.<>p__5, <>o__33.<>p__4.Target(<>o__33.<>p__4, obj2, "value")));
            }
            catch
            {
                return "";
            }
        }

        internal static string GetHardDiskSerialNo()
        {
            string str = "";
            foreach (ManagementObject obj2 in new ManagementClass("Win32_LogicalDisk").GetInstances())
            {
                if (obj2.ToString().Contains("C:"))
                {
                    str = Convert.ToString(obj2["VolumeSerialNumber"]);
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("C: drive not found");
            }
            return str;
        }

        internal static string GetIP()
        {
            try
            {
                HttpWebRequest request1 = (HttpWebRequest) WebRequest.Create("http://checkip.dyndns.org");
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                request1.ContentType = "application/json";
                request1.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request1.AllowAutoRedirect = true;
                request1.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                string str = "";
                using (HttpWebResponse response = (HttpWebResponse) request1.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                char[] separator = new char[] { ':' };
                char[] chArray2 = new char[] { '<' };
                return str.Split(separator)[1].Substring(1).Split(chArray2)[0];
            }
            catch
            {
                return "exp";
            }
        }

        public static ProxyObject GetProxy(ProxyListObject proxyList)
        {
            try
            {
                if (proxyList.Proxies.Any<ProxyObject>(x => x.Enabled))
                {
                    object proxyLocker;
                    if (proxyList.Rotation == ProxyListObject.RotationEnum.random)
                    {
                        proxyLocker = Global.ProxyLocker;
                        lock (proxyLocker)
                        {
                            List<ProxyObject> list = (from x in proxyList.Proxies
                                where x.Enabled && !string.IsNullOrEmpty(x.IP)
                                select x).ToList<ProxyObject>();
                            return list[_rnd.Next(0, list.Count)];
                        }
                    }
                    proxyLocker = Global.ProxyLocker;
                    lock (proxyLocker)
                    {
                        List<ProxyObject> list2 = (from x in proxyList.Proxies
                            where x.Enabled && !string.IsNullOrEmpty(x.IP)
                            select x).ToList<ProxyObject>();
                        if (!Global.PROXY_USAGE.Any<KeyVal<string, int>>(x => (x.Id == proxyList.Id)))
                        {
                            Global.PROXY_USAGE.Add(new KeyVal<string, int>(proxyList.Id, 0));
                            return list2[0];
                        }
                        KeyVal<string, int> val = Global.PROXY_USAGE.First<KeyVal<string, int>>(x => x.Id == proxyList.Id);
                        if ((val.Text + 1) >= list2.Count)
                        {
                            val.Text = 0;
                        }
                        else
                        {
                            val.Text += 1;
                        }
                        return list2[val.Text];
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error while assigning proxy", exception);
                return null;
            }
        }

        public static ProxyObject GetProxy(ProxyListObject proxyList, Random rnd)
        {
            try
            {
                if (proxyList.Proxies.Any<ProxyObject>(x => x.Enabled))
                {
                    object proxyLocker;
                    if (proxyList.Rotation == ProxyListObject.RotationEnum.random)
                    {
                        proxyLocker = Global.ProxyLocker;
                        lock (proxyLocker)
                        {
                            List<ProxyObject> list = (from x in proxyList.Proxies
                                where x.Enabled && !string.IsNullOrEmpty(x.IP)
                                select x).ToList<ProxyObject>();
                            return list[rnd.Next(0, list.Count)];
                        }
                    }
                    proxyLocker = Global.ProxyLocker;
                    lock (proxyLocker)
                    {
                        List<ProxyObject> list2 = (from x in proxyList.Proxies
                            where x.Enabled && !string.IsNullOrEmpty(x.IP)
                            select x).ToList<ProxyObject>();
                        if (!Global.PROXY_USAGE.Any<KeyVal<string, int>>(x => (x.Id == proxyList.Id)))
                        {
                            Global.PROXY_USAGE.Add(new KeyVal<string, int>(proxyList.Id, 0));
                            return list2[0];
                        }
                        KeyVal<string, int> val = Global.PROXY_USAGE.First<KeyVal<string, int>>(x => x.Id == proxyList.Id);
                        if ((val.Text + 1) < list2.Count)
                        {
                            val.Text += 1;
                        }
                        else
                        {
                            val.Text = 0;
                        }
                        return list2[val.Text];
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error while assigning proxy", exception);
                return null;
            }
        }

        internal static void GetQueueSession(TaskObject task)
        {
            Task.Factory.StartNew(delegate {
                try
                {
                    string str = Encrypt("{\"call_id\":\"" + Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", \"data\": \"{'website':'" + task.HomeUrl.Replace("http://", "").Replace("https://", "") + "'}\"}");
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://everoboticslm.herokuapp.com/aio_queue/get");
                    string s = "call=" + WebUtility.UrlEncode(str);
                    request.UserAgent = "python-requests/2.14.2";
                    request.Accept = "*/*";
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = s.Length;
                    request.KeepAlive = true;
                    request.ServicePoint.Expect100Continue = false;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    byte[] bytes = Encoding.ASCII.GetBytes(s);
                    request.ContentLength = bytes.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    string cypher = "";
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            cypher = reader.ReadToEnd();
                        }
                    }
                    string str4 = Decrypt(cypher);
                    object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(str4);
                    if (!str4.Contains("No sessions found") && !str4.Contains("'result': '[]'"))
                    {
                        if (<>o__31.<>p__7 == null)
                        {
                            <>o__31.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(EveAIO.Helpers)));
                        }
                        if (<>o__31.<>p__6 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__31.<>p__6 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__31.<>p__5 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__31.<>p__5 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__31.<>p__4 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__31.<>p__4 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__31.<>p__3 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__31.<>p__3 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__31.<>p__2 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                            <>o__31.<>p__2 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__31.<>p__1 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__31.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(EveAIO.Helpers), argumentInfo));
                        }
                        if (<>o__31.<>p__0 == null)
                        {
                            CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                            <>o__31.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "result", typeof(EveAIO.Helpers), argumentInfo));
                        }
                        char[] separator = new char[] { ',' };
                        List<string> list = (from tag in <>o__31.<>p__7.Target(<>o__31.<>p__7, <>o__31.<>p__6.Target(<>o__31.<>p__6, <>o__31.<>p__5.Target(<>o__31.<>p__5, <>o__31.<>p__4.Target(<>o__31.<>p__4, <>o__31.<>p__3.Target(<>o__31.<>p__3, <>o__31.<>p__2.Target(<>o__31.<>p__2, <>o__31.<>p__1.Target(<>o__31.<>p__1, <>o__31.<>p__0.Target(<>o__31.<>p__0, obj2)), "[", ""), "]", ""), "'", ""), "(", ""), ")", "")).Split(separator)
                            select tag.Trim() into tag
                            where !string.IsNullOrEmpty(tag)
                            select tag).ToList<string>();
                        task.SessionsId = list[_rnd.Next(0, list.Count)];
                    }
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error getting session", exception);
                }
            });
        }

        public static List<KeyValuePair<string, string>> GetSNSCanadaStates() => 
            new List<KeyValuePair<string, string>> { 
                new KeyValuePair<string, string>("2060", "Alberta"),
                new KeyValuePair<string, string>("2061", "British Columbia"),
                new KeyValuePair<string, string>("2062", "Manitoba"),
                new KeyValuePair<string, string>("2063", "New Brunswick"),
                new KeyValuePair<string, string>("2064", "Newfoundland and Labrador"),
                new KeyValuePair<string, string>("2070", "Northwest Territories"),
                new KeyValuePair<string, string>("2065", "Nova Scotia"),
                new KeyValuePair<string, string>("2071", "Nunavut"),
                new KeyValuePair<string, string>("2066", "Ontario"),
                new KeyValuePair<string, string>("2067", "Prince Edward Island"),
                new KeyValuePair<string, string>("2068", "Quebec"),
                new KeyValuePair<string, string>("2069", "Saskatchewan"),
                new KeyValuePair<string, string>("2072", "Yukon Territory")
            };

        public static List<KeyValuePair<string, string>> GetSNSUsaStates() => 
            new List<KeyValuePair<string, string>> { 
                new KeyValuePair<string, string>("2293", "AL"),
                new KeyValuePair<string, string>("2294", "AK"),
                new KeyValuePair<string, string>("2295", "AZ"),
                new KeyValuePair<string, string>("2296", "AR"),
                new KeyValuePair<string, string>("2297", "CA"),
                new KeyValuePair<string, string>("2298", "CO"),
                new KeyValuePair<string, string>("2299", "CT"),
                new KeyValuePair<string, string>("2300", "DE"),
                new KeyValuePair<string, string>("2343", "DC"),
                new KeyValuePair<string, string>("2301", "FL"),
                new KeyValuePair<string, string>("2302", "GA"),
                new KeyValuePair<string, string>("2303", "HI"),
                new KeyValuePair<string, string>("2304", "ID"),
                new KeyValuePair<string, string>("2305", "IL"),
                new KeyValuePair<string, string>("2306", "IN"),
                new KeyValuePair<string, string>("2307", "IA"),
                new KeyValuePair<string, string>("2308", "KS"),
                new KeyValuePair<string, string>("2309", "KY"),
                new KeyValuePair<string, string>("2310", "LA"),
                new KeyValuePair<string, string>("2311", "ME"),
                new KeyValuePair<string, string>("2312", "MD"),
                new KeyValuePair<string, string>("2313", "MD"),
                new KeyValuePair<string, string>("2314", "MI"),
                new KeyValuePair<string, string>("2315", "MN"),
                new KeyValuePair<string, string>("2316", "MS"),
                new KeyValuePair<string, string>("2317", "MO"),
                new KeyValuePair<string, string>("2318", "MT"),
                new KeyValuePair<string, string>("2319", "NE"),
                new KeyValuePair<string, string>("2320", "NV"),
                new KeyValuePair<string, string>("2321", "NH"),
                new KeyValuePair<string, string>("2322", "NJ"),
                new KeyValuePair<string, string>("2323", "NM"),
                new KeyValuePair<string, string>("2324", "NY"),
                new KeyValuePair<string, string>("2325", "NC"),
                new KeyValuePair<string, string>("2326", "ND"),
                new KeyValuePair<string, string>("2327", "OH"),
                new KeyValuePair<string, string>("2328", "OK"),
                new KeyValuePair<string, string>("2329", "OR"),
                new KeyValuePair<string, string>("2330", "PA"),
                new KeyValuePair<string, string>("2359", "PR"),
                new KeyValuePair<string, string>("2331", "RI"),
                new KeyValuePair<string, string>("2332", "SC"),
                new KeyValuePair<string, string>("2333", "SD"),
                new KeyValuePair<string, string>("2334", "TN"),
                new KeyValuePair<string, string>("2335", "TX"),
                new KeyValuePair<string, string>("2336", "UT"),
                new KeyValuePair<string, string>("2337", "VT"),
                new KeyValuePair<string, string>("2338", "VI"),
                new KeyValuePair<string, string>("2339", "WA"),
                new KeyValuePair<string, string>("2340", "WV"),
                new KeyValuePair<string, string>("2341", "WI"),
                new KeyValuePair<string, string>("2342", "WY")
            };

        internal static string GetStoreUrl(TaskObject task)
        {
            string homeUrl = "";
            switch (task.Platform)
            {
                case TaskObject.PlatformEnum.shopify:
                    homeUrl = task.HomeUrl;
                    break;

                case TaskObject.PlatformEnum.supreme:
                    homeUrl = "http://www.supremenewyork.com";
                    switch (task.SupremeRegion)
                    {
                        case TaskObject.SupremeRegionEnum.USA:
                            homeUrl = homeUrl + " (USA)";
                            goto Label_02C7;

                        case TaskObject.SupremeRegionEnum.EU:
                            homeUrl = homeUrl + " (EU)";
                            goto Label_02C7;

                        case TaskObject.SupremeRegionEnum.JP:
                            homeUrl = homeUrl + " (JP)";
                            goto Label_02C7;
                    }
                    break;

                case TaskObject.PlatformEnum.sivasdescalzo:
                    homeUrl = "https://www.sivasdescalzo.com";
                    break;

                case TaskObject.PlatformEnum.sneakersnstuff:
                    homeUrl = "https://www.sneakersnstuff.com";
                    break;

                case TaskObject.PlatformEnum.footsites:
                    homeUrl = task.HomeUrl;
                    break;

                case TaskObject.PlatformEnum.woodwood:
                    homeUrl = "https://www.woodwood.com";
                    break;

                case TaskObject.PlatformEnum.hibbett:
                    homeUrl = "https://www.hibbett.com";
                    break;

                case TaskObject.PlatformEnum.solebox:
                    homeUrl = "https://www.solebox.com";
                    break;

                case TaskObject.PlatformEnum.nordstrom:
                    homeUrl = "https://shop.nordstrom.com";
                    break;

                case TaskObject.PlatformEnum.mesh:
                {
                    string variousStringData = task.VariousStringData;
                    switch (variousStringData)
                    {
                        case "size":
                            homeUrl = "https://www.size.co.uk";
                            goto Label_02C7;

                        case "hipstore":
                            homeUrl = "https://www.thehipstore.co.uk";
                            goto Label_02C7;
                    }
                    if (variousStringData != "jd")
                    {
                        if (variousStringData == "footpatrol")
                        {
                            homeUrl = "https://www.footpatrol.com";
                        }
                        break;
                    }
                    homeUrl = "https://www.jdsports.co.uk";
                    break;
                }
                case TaskObject.PlatformEnum.mrporter:
                    homeUrl = "https://www.mrporter.com";
                    break;

                case TaskObject.PlatformEnum.holypopstore:
                    homeUrl = "https://www.holypopstore.com";
                    break;

                case TaskObject.PlatformEnum.backdoor:
                    homeUrl = "https://www.back-door.it";
                    break;

                case TaskObject.PlatformEnum.barneys:
                    homeUrl = "https://www.barneys.com";
                    break;

                case TaskObject.PlatformEnum.offwhite:
                    homeUrl = "https://www.off---white.com";
                    break;

                case TaskObject.PlatformEnum.titolo:
                    homeUrl = "https://en.titoloshop.com";
                    break;

                case TaskObject.PlatformEnum.funko:
                    homeUrl = "shop.funko.com";
                    break;

                case TaskObject.PlatformEnum.oneblockdown:
                    homeUrl = "http://www.oneblockdown.it";
                    break;

                case TaskObject.PlatformEnum.boxlunch:
                    homeUrl = "https://www.boxlunch.com";
                    break;

                case TaskObject.PlatformEnum.puma:
                    homeUrl = task.HomeUrl;
                    break;

                case TaskObject.PlatformEnum.converse:
                    homeUrl = task.HomeUrl;
                    break;

                case TaskObject.PlatformEnum.footlockerau:
                    homeUrl = "https://www.footlocker.com.au";
                    break;

                case TaskObject.PlatformEnum.footlockereu:
                    homeUrl = task.HomeUrl;
                    break;

                case TaskObject.PlatformEnum.footaction:
                    homeUrl = "https://www.footaction.com";
                    break;

                case TaskObject.PlatformEnum.footlocker:
                    homeUrl = "https://www.footlocker.com";
                    break;

                case TaskObject.PlatformEnum.supremeinstore:
                    switch (task.SupremeRegion)
                    {
                        case TaskObject.SupremeRegionEnum.USA:
                            homeUrl = homeUrl + "https://register.supremenewyork.com";
                            goto Label_02C7;

                        case TaskObject.SupremeRegionEnum.EU:
                            homeUrl = "https://london.supremenewyork.com";
                            break;
                    }
                    break;

                case TaskObject.PlatformEnum.mcm:
                    homeUrl = "http://us.mcmworldwide.com";
                    break;

                case TaskObject.PlatformEnum.sevres:
                    homeUrl = "https://www.24sevres.com";
                    break;

                case TaskObject.PlatformEnum.finishline:
                    homeUrl = "https://www.finishline.com";
                    break;

                case TaskObject.PlatformEnum.hottopic:
                    homeUrl = "https://www.hottopic.com";
                    break;
            }
        Label_02C7:
            return homeUrl.Replace("https://", "").Replace("http://", "").Replace("www.", "");
        }

        public static void LoadSettings()
        {
            try
            {
                if (System.IO.File.Exists("EveAIO.data"))
                {
                    using (StringReader reader = new StringReader(EncryptorAes.Decrypt(System.IO.File.ReadAllText("EveAIO.data"))))
                    {
                        Global.SETTINGS = (EveAIO.Settings) _serSettings.Deserialize(reader);
                    }
                    for (int i = 0; i < Global.SETTINGS.PROFILES.Count; i++)
                    {
                        Global.SETTINGS.PROFILES[i].No = i + 1;
                    }
                    if (string.IsNullOrEmpty(Global.SETTINGS.ShopifyCaptchaKey))
                    {
                        Global.SETTINGS.ShopifyCaptchaKey = WebsitesInfo.SHOPIFY_CAPTCHA_KEY;
                    }
                    if (string.IsNullOrEmpty(Global.SETTINGS.SupremeCaptchaKey))
                    {
                        Global.SETTINGS.SupremeCaptchaKey = WebsitesInfo.SUPREME_CAPTCHA_KEY;
                    }
                    if (string.IsNullOrEmpty(Global.SETTINGS.SnsCaptchaKey))
                    {
                        Global.SETTINGS.SnsCaptchaKey = (string) WebsitesInfo.SNS_CAPTCHA_KEY;
                    }
                    if ((Global.SETTINGS.TASKS != null) && (Global.SETTINGS.TASKS.Count > 0))
                    {
                        foreach (TaskObject obj2 in Global.SETTINGS.TASKS)
                        {
                            if (string.IsNullOrEmpty(obj2.Guid))
                            {
                                obj2.Guid = Guid.NewGuid().ToString();
                            }
                        }
                    }
                }
                else
                {
                    Global.SETTINGS = new EveAIO.Settings();
                }
            }
            catch (Exception exception)
            {
                Global.SETTINGS = new EveAIO.Settings();
                Global.Logger.Error("Error while loading the settings file", exception);
                MessageBox.Show("Error occured while loading the settings file", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        internal static string Md5(string inpput)
        {
            byte[] buffer2 = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(inpput));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer2.Length; i++)
            {
                builder.Append(buffer2[i].ToString("X2"));
            }
            return builder.ToString().ToLowerInvariant();
        }

        public static void Notify(string what, string paramss)
        {
            // This item is obfuscated and can not be translated.
        Label_0056:
            switch (((-1264010830 ^ -979133869) % 3))
            {
                case 1:
                    Task.Factory.StartNew(delegate {
                        try
                        {
                            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://sinisterbot.org/scripts/notify.php");
                            request.Headers.Add("API", "EveAIO");
                            string[] textArray1 = new string[] { "app=eveaio&key=", Global.SERIAL, "&who=", Environment.UserName, "&what=", what, "&params=", paramss };
                            string s = string.Concat(textArray1);
                            byte[] bytes = Encoding.ASCII.GetBytes(s);
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.ContentLength = bytes.Length;
                            request.Timeout = 0x2710;
                            using (Stream stream = request.GetRequestStream())
                            {
                                stream.Write(bytes, 0, bytes.Length);
                            }
                            using (WebResponse response = request.GetResponse())
                            {
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    reader.ReadToEnd();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }).ContinueWith(delegate (Task t) {
                    });
                    goto Label_0056;

                case 2:
                    goto Label_0056;
            }
        }

        public static void NotifySuccess(string homeurl, string productName, string size)
        {
            Task.Factory.StartNew(delegate {
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://sinisterbot.org/scripts/notify_success.php");
                    request.Headers.Add("API", "EveAIO");
                    string[] textArray1 = new string[] { "app=eveaio&homeurl=", homeurl, "&productname=", productName, "&size=", size, "&dt=", DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm:ss") };
                    string s = string.Concat(textArray1).Replace("'", "").Replace("\"", "");
                    byte[] bytes = Encoding.ASCII.GetBytes(s);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = bytes.Length;
                    request.Timeout = 0x2710;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }).ContinueWith(delegate (Task t) {
            });
        }

        internal static void PlayBell()
        {
            try
            {
                Task.Factory.StartNew(() => new SoundPlayer(Resources.bell).Play());
            }
            catch
            {
            }
        }

        internal static void PostDiscordSuccess()
        {
            Task.Factory.StartNew(delegate {
                try
                {
                    Thread.Sleep(0x5dc);
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://everoboticslm.herokuapp.com/upload");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.KeepAlive = true;
                    request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    request.Headers.Add("Cache-Control", "max-age=0");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    List<KeyValuePair<string, string>> proxList = new List<KeyValuePair<string, string>>();
                    List<KeyValuePair<string, DateTime>> dateList = new List<KeyValuePair<string, DateTime>>();
                    Global.ViewSuccess.listSuccess.Dispatcher.Invoke(delegate {
                        foreach (SuccessObject obj2 in Global.SUCCESS)
                        {
                            proxList.Add(new KeyValuePair<string, string>(obj2.Id, obj2.Proxy));
                            DateTime? time = obj2.Time;
                            dateList.Add(new KeyValuePair<string, DateTime>(obj2.Id, time.Value));
                            obj2.Proxy = "***********";
                            obj2.Time = null;
                        }
                    });
                    byte[] arr = null;
                    Thread.Sleep(800);
                    Global.MAIN_WINDOW.Dispatcher.Invoke(delegate {
                        RenderTargetBitmap source = new RenderTargetBitmap((int) Global.MAIN_WINDOW.ActualWidth, (int) Global.MAIN_WINDOW.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                        source.Render((Visual) Global.MAIN_WINDOW);
                        DrawingVisual visual = new DrawingVisual();
                        using (DrawingContext context = visual.RenderOpen())
                        {
                            VisualBrush brush = new VisualBrush((Visual) Global.MAIN_WINDOW);
                            context.DrawRectangle(brush, null, new Rect(new Point(), new Size(Global.MAIN_WINDOW.Width, Global.MAIN_WINDOW.Height)));
                        }
                        source.Render(visual);
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder {
                            QualityLevel = 100,
                            Frames = { BitmapFrame.Create(source) }
                        };
                        using (MemoryStream stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            arr = stream.ToArray();
                        }
                    });
                    Thread.Sleep(0x1194);
                    string s = "call=" + WebUtility.UrlEncode(Convert.ToBase64String(arr));
                    Encoding.ASCII.GetBytes(s);
                    Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                        foreach (KeyValuePair<string, string> prox in proxList)
                        {
                            Global.SUCCESS.First<SuccessObject>(x => (x.Id == prox.Key)).Proxy = prox.Value;
                        }
                        foreach (KeyValuePair<string, DateTime> date in dateList)
                        {
                            Global.SUCCESS.First<SuccessObject>(x => (x.Id == date.Key)).Time = new DateTime?(date.Value);
                        }
                    }, Array.Empty<object>());
                    request.ContentLength = s.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(s, s.Length);
                        }
                    }
                    string cypher = "";
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            cypher = reader.ReadToEnd();
                        }
                    }
                    string str3 = Decrypt(cypher);
                    if (str3.ToLowerInvariant().Contains("ever") && str3.ToLowerInvariant().Contains("jpg"))
                    {
                        Thread.Sleep(0x1194);
                        CommandServiceConfig config = new CommandServiceConfig {
                            LogLevel = LogSeverity.Info,
                            CaseSensitiveCommands = false
                        };
                        new CommandService(config);
                        EmbedBuilder builder = new EmbedBuilder {
                            ImageUrl = str3
                        };
                        Embed[] embedArray1 = new Embed[] { builder };
                        new DiscordWebhookClient(0x5db6f90ed800017L, "OV7Jo0TEzdV3Pdzusal_UKJnHfhqin1wCz420d2q-wuBa4LUe1FtbeT6Aymf9QC_fVjR").SendMessageAsync("", false, embedArray1, null, null, null);
                    }
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error posting success", exception);
                }
            });
        }

        internal static List<PumaProduct> ProcessNegativeSizing(List<PumaProduct> sizes, TaskObject task)
        {
            List<PumaProduct> list = new List<PumaProduct>();
            List<string> source = new List<string>();
            if (!task.Size.Contains("#"))
            {
                source.Add(task.Size);
            }
            else
            {
                char[] separator = new char[] { '#' };
                source = task.Size.Split(separator).ToList<string>();
            }
            foreach (PumaProduct size in sizes)
            {
                if (!source.Any<string>(x => (x == size.Size)))
                {
                    list.Add(size);
                }
            }
            return list;
        }

        internal static List<NordStromProduct> ProcessNegativeSizing(List<NordStromProduct> sizes, TaskObject task)
        {
            List<NordStromProduct> list = new List<NordStromProduct>();
            List<string> source = new List<string>();
            if (!task.Size.Contains("#"))
            {
                source.Add(task.Size);
            }
            else
            {
                char[] separator = new char[] { '#' };
                source = task.Size.Split(separator).ToList<string>();
            }
            foreach (NordStromProduct size in sizes)
            {
                if (!source.Any<string>(delegate (string x) {
                    if (x != size.Size)
                    {
                        return (x == size.SizeAltered);
                    }
                    return true;
                }))
                {
                    list.Add(size);
                }
            }
            return list;
        }

        internal static List<KeyValuePair<string, string>> ProcessNegativeSizing(List<KeyValuePair<string, string>> sizes, TaskObject task)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            List<string> source = new List<string>();
            if (task.Size.Contains("#"))
            {
                char[] separator = new char[] { '#' };
                source = task.Size.Split(separator).ToList<string>();
            }
            else
            {
                source.Add(task.Size);
            }
            foreach (KeyValuePair<string, string> size in sizes)
            {
                if (!source.Any<string>(x => (x == size.Key)))
                {
                    list.Add(size);
                }
            }
            return list;
        }

        internal static void ProxyCheck()
        {
            if (WebRequest.GetSystemWebProxy().GetProxy(new Uri("http://www.google.de")).Host.Equals("127.0.0.1"))
            {
                Environment.Exit(0);
            }
        }

        internal static bool QuickEnabled()
        {
            if ((!string.IsNullOrEmpty(Global.SETTINGS.QUICK_TASK.IdProfile) && ((Global.SETTINGS.QUICK_TASK.RetryDelay != TaskObject.RetryDelayEnum.exact) || !string.IsNullOrEmpty(Global.SETTINGS.QUICK_TASK.Delay))) && ((Global.SETTINGS.QUICK_TASK.RetryDelay != TaskObject.RetryDelayEnum.random) || (!string.IsNullOrEmpty(Global.SETTINGS.QUICK_TASK.DelayFrom) && !string.IsNullOrEmpty(Global.SETTINGS.QUICK_TASK.DelayFrom))))
            {
                if (!Global.SETTINGS.PROFILES.Any<ProfileObject>(x => (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile)) && !Global.SETTINGS.PROFILES_GROUPS.Any<ProfileGroupObject>(x => (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile)))
                {
                    MessageBox.Show("Quick tasks settings not set", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return false;
                }
                return true;
            }
            MessageBox.Show("Quick tasks settings not set", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            return false;
        }

        internal static string RandomString(int length) => 
            new string((from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length) select s[_rnd.Next(s.Length)]).ToArray<char>());

        public static string RemoveHtmlTags(string input)
        {
            input = input.Replace("<br>", " ").Replace("<br/>", " ");
            return Regex.Replace(Regex.Replace(input, "<.*?>", string.Empty), @"\s+", " ", RegexOptions.Multiline).Trim();
        }

        internal static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char ch in str)
            {
                if ((((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'Z'))) || (((ch >= 'a') && (ch <= 'z')) || ((ch == ' ') || (ch == '-'))))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        internal static string Reverse(string s)
        {
            char[] array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static void SaveSettings()
        {
            try
            {
                string plainText = "";
                using (MemoryStream stream = new MemoryStream())
                {
                    _serSettings.Serialize((Stream) stream, Global.SETTINGS);
                    plainText = Encoding.UTF8.GetString(stream.ToArray());
                }
                string contents = EncryptorAes.Encrypt(plainText);
                System.IO.File.WriteAllText("EveAIO.data", contents);
            }
            catch (Exception exception)
            {
                Global.SETTINGS = new EveAIO.Settings();
                Global.Logger.Error("Error while saving the settings file", exception);
                MessageBox.Show("Error occured while saving the settings file", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        internal static void SerialCheck()
        {
            if (string.IsNullOrEmpty(Global.SERIAL) || !Global.SERIAL.Contains("EVE"))
            {
                Environment.Exit(0);
            }
        }

        internal static long ToUnixTime(DateTime date)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (TimeSpan) (date.ToUniversalTime() - time);
            return Convert.ToInt64(span.TotalSeconds);
        }

        public static void WriteLog(string text)
        {
            if (!Global.LOG_PAUSED)
            {
                Global.ViewLog.txtLog.Dispatcher.BeginInvoke(delegate {
                    if (Global.ViewLog.txtLog.LineCount > 100)
                    {
                        Global.ViewLog.txtLog.Clear();
                    }
                    Global.ViewLog.txtLog.AppendText(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss:fff") + ": " + text + "\n");
                }, Array.Empty<object>());
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly EveAIO.Helpers.<>c <>9;
            public static Func<string, char> <>9__7_0;
            public static Func<ProxyObject, bool> <>9__15_0;
            public static Func<ProxyObject, bool> <>9__15_1;
            public static Func<ProxyObject, bool> <>9__15_2;
            public static Func<ProxyObject, bool> <>9__16_0;
            public static Func<ProxyObject, bool> <>9__16_1;
            public static Func<ProxyObject, bool> <>9__16_2;
            public static Action<Task> <>9__19_1;
            public static Action<Task> <>9__20_1;
            public static Action <>9__21_0;
            public static Action <>9__23_0;
            public static Func<CheckoutLink, string> <>9__30_0;
            public static Func<CheckoutLink, string> <>9__30_1;
            public static Func<string, string> <>9__31_1;
            public static Func<string, bool> <>9__31_2;
            public static Func<ProfileObject, bool> <>9__37_0;
            public static Func<ProfileGroupObject, bool> <>9__37_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new EveAIO.Helpers.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <.cctor>b__1_0(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => 
                true;

            internal string <GetCheckoutLinks>b__30_0(CheckoutLink x) => 
                x.Name;

            internal string <GetCheckoutLinks>b__30_1(CheckoutLink x) => 
                x.Size;

            internal bool <GetProxy>b__15_0(ProxyObject x) => 
                x.Enabled;

            internal bool <GetProxy>b__15_1(ProxyObject x) => 
                (x.Enabled && !string.IsNullOrEmpty(x.IP));

            internal bool <GetProxy>b__15_2(ProxyObject x) => 
                (x.Enabled && !string.IsNullOrEmpty(x.IP));

            internal bool <GetProxy>b__16_0(ProxyObject x) => 
                x.Enabled;

            internal bool <GetProxy>b__16_1(ProxyObject x) => 
                (x.Enabled && !string.IsNullOrEmpty(x.IP));

            internal bool <GetProxy>b__16_2(ProxyObject x) => 
                (x.Enabled && !string.IsNullOrEmpty(x.IP));

            internal string <GetQueueSession>b__31_1(string tag) => 
                tag.Trim();

            internal bool <GetQueueSession>b__31_2(string tag) => 
                !string.IsNullOrEmpty(tag);

            internal void <Notify>b__19_1(Task t)
            {
            }

            internal void <NotifySuccess>b__20_1(Task t)
            {
            }

            internal void <PlayBell>b__23_0()
            {
                new SoundPlayer(Resources.bell).Play();
            }

            internal void <PostDiscordSuccess>b__21_0()
            {
                try
                {
                    Thread.Sleep(0x5dc);
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://everoboticslm.herokuapp.com/upload");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.KeepAlive = true;
                    request.Headers.Add("Upgrade-Insecure-Requests", "1");
                    request.Headers.Add("Cache-Control", "max-age=0");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    List<KeyValuePair<string, string>> proxList = new List<KeyValuePair<string, string>>();
                    List<KeyValuePair<string, DateTime>> dateList = new List<KeyValuePair<string, DateTime>>();
                    Global.ViewSuccess.listSuccess.Dispatcher.Invoke(delegate {
                        foreach (SuccessObject obj2 in Global.SUCCESS)
                        {
                            proxList.Add(new KeyValuePair<string, string>(obj2.Id, obj2.Proxy));
                            DateTime? time = obj2.Time;
                            dateList.Add(new KeyValuePair<string, DateTime>(obj2.Id, time.Value));
                            obj2.Proxy = "***********";
                            obj2.Time = null;
                        }
                    });
                    byte[] arr = null;
                    Thread.Sleep(800);
                    Global.MAIN_WINDOW.Dispatcher.Invoke(delegate {
                        RenderTargetBitmap source = new RenderTargetBitmap((int) Global.MAIN_WINDOW.ActualWidth, (int) Global.MAIN_WINDOW.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                        source.Render((Visual) Global.MAIN_WINDOW);
                        DrawingVisual visual = new DrawingVisual();
                        using (DrawingContext context = visual.RenderOpen())
                        {
                            VisualBrush brush = new VisualBrush((Visual) Global.MAIN_WINDOW);
                            context.DrawRectangle(brush, null, new Rect(new Point(), new Size(Global.MAIN_WINDOW.Width, Global.MAIN_WINDOW.Height)));
                        }
                        source.Render(visual);
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder {
                            QualityLevel = 100,
                            Frames = { BitmapFrame.Create(source) }
                        };
                        using (MemoryStream stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            arr = stream.ToArray();
                        }
                    });
                    Thread.Sleep(0x1194);
                    string s = "call=" + WebUtility.UrlEncode(Convert.ToBase64String(arr));
                    Encoding.ASCII.GetBytes(s);
                    Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                        foreach (KeyValuePair<string, string> prox in proxList)
                        {
                            Global.SUCCESS.First<SuccessObject>(x => (x.Id == prox.Key)).Proxy = prox.Value;
                        }
                        foreach (KeyValuePair<string, DateTime> date in dateList)
                        {
                            Global.SUCCESS.First<SuccessObject>(x => (x.Id == date.Key)).Time = new DateTime?(date.Value);
                        }
                    }, Array.Empty<object>());
                    request.ContentLength = s.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(s, s.Length);
                        }
                    }
                    string cypher = "";
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            cypher = reader.ReadToEnd();
                        }
                    }
                    string str3 = EveAIO.Helpers.Decrypt(cypher);
                    if (str3.ToLowerInvariant().Contains("ever") && str3.ToLowerInvariant().Contains("jpg"))
                    {
                        Thread.Sleep(0x1194);
                        CommandServiceConfig config = new CommandServiceConfig {
                            LogLevel = LogSeverity.Info,
                            CaseSensitiveCommands = false
                        };
                        new CommandService(config);
                        EmbedBuilder builder = new EmbedBuilder {
                            ImageUrl = str3
                        };
                        Embed[] embedArray1 = new Embed[] { builder };
                        new DiscordWebhookClient(0x5db6f90ed800017L, "OV7Jo0TEzdV3Pdzusal_UKJnHfhqin1wCz420d2q-wuBa4LUe1FtbeT6Aymf9QC_fVjR").SendMessageAsync("", false, embedArray1, null, null, null);
                    }
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error posting success", exception);
                }
            }

            internal bool <QuickEnabled>b__37_0(ProfileObject x) => 
                (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile);

            internal bool <QuickEnabled>b__37_1(ProfileGroupObject x) => 
                (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile);

            internal char <RandomString>b__7_0(string s) => 
                s[EveAIO.Helpers._rnd.Next(s.Length)];
        }

        [CompilerGenerated]
        private static class <>o__1
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, Type, object, object, KeyValuePair<string, string>>> <>p__4;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__5;
        }

        [CompilerGenerated]
        private static class <>o__28
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, string, object, object>> <>p__6;
            public static CallSite<Action<CallSite, ILog, object>> <>p__7;
        }

        [CompilerGenerated]
        private static class <>o__29
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, string, object, object>> <>p__6;
            public static CallSite<Action<CallSite, ILog, object>> <>p__7;
        }

        [CompilerGenerated]
        private static class <>o__30
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, string, object, object>> <>p__6;
            public static CallSite<Action<CallSite, ILog, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, string, object>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, Type, object, object>> <>p__11;
            public static CallSite<Func<CallSite, object, bool>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, string, object, object>> <>p__15;
            public static CallSite<Func<CallSite, object, string, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, string, object, object>> <>p__18;
            public static CallSite<Func<CallSite, object, object, object>> <>p__19;
            public static CallSite<Func<CallSite, object, bool>> <>p__20;
            public static CallSite<Func<CallSite, object, bool>> <>p__21;
            public static CallSite<Func<CallSite, object, string, object>> <>p__22;
            public static CallSite<Func<CallSite, object, object>> <>p__23;
            public static CallSite<Func<CallSite, string, object, object>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, string, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, object, object>> <>p__28;
            public static CallSite<Func<CallSite, object, bool>> <>p__29;
            public static CallSite<Func<CallSite, object, bool>> <>p__30;
            public static CallSite<Func<CallSite, object, string, object>> <>p__31;
            public static CallSite<Func<CallSite, object, object>> <>p__32;
            public static CallSite<Action<CallSite, List<string>, object>> <>p__33;
            public static CallSite<Func<CallSite, object, string, object>> <>p__34;
            public static CallSite<Func<CallSite, object, object>> <>p__35;
            public static CallSite<Func<CallSite, object, string>> <>p__36;
            public static CallSite<Func<CallSite, object, string, object>> <>p__37;
            public static CallSite<Func<CallSite, object, object>> <>p__38;
            public static CallSite<Func<CallSite, object, string>> <>p__39;
            public static CallSite<Func<CallSite, object, string, object>> <>p__40;
            public static CallSite<Func<CallSite, object, object>> <>p__41;
            public static CallSite<Action<CallSite, List<string>, object>> <>p__42;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__43;
        }

        [CompilerGenerated]
        private static class <>o__31
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
        }

        [CompilerGenerated]
        private static class <>o__32
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, string, object, object>> <>p__6;
            public static CallSite<Action<CallSite, ILog, object>> <>p__7;
        }

        [CompilerGenerated]
        private static class <>o__33
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, string>> <>p__6;
        }
    }
}

