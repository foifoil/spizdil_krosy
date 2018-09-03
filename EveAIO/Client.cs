namespace EveAIO
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class Client
    {
        public Client(CookieContainer cookies, WebProxy proxy = null, bool redirect = true)
        {
            Class7.RIuqtBYzWxthF();
            this.Cookies = cookies;
            HttpClientHandler handler1 = new HttpClientHandler {
                UseCookies = (cookies != null) ? true : false,
                AllowAutoRedirect = redirect,
                Proxy = proxy,
                PreAuthenticate = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
            this.Handler = handler1;
            if (this.Cookies != null)
            {
                this.Handler.CookieContainer = this.Cookies;
            }
            this.Session = new HttpClient(this.Handler);
        }

        public HttpResponseMessage Get(string url) => 
            this.Session.GetAsync(url).Result;

        public HttpResponseMessage PatchJson(string url, string json) => 
            this.Session.PatchAsync(new Uri(url), new StringContent(json, Encoding.UTF8, "application/json")).Result;

        public HttpResponseMessage Post(string url, Dictionary<string, string> data) => 
            this.Session.PostAsync(url, PostFormat(data)).Result;

        public HttpResponseMessage Post(string url, List<KeyValuePair<string, string>> data) => 
            this.Session.PostAsync(url, new FormUrlEncodedContent(data)).Result;

        public static FormUrlEncodedContent PostFormat(Dictionary<string, string> data)
        {
            List<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> pair in data)
            {
                string key = pair.Key.ToString();
                nameValueCollection.Add(new KeyValuePair<string, string>(key, pair.Value.ToString()));
            }
            return new FormUrlEncodedContent(nameValueCollection);
        }

        public HttpResponseMessage PostJson(string url, string json) => 
            this.Session.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

        public HttpResponseMessage PostPlain(string url, string data) => 
            this.Session.PostAsync(url, new StringContent(data, Encoding.UTF8, "text/plain")).Result;

        public HttpResponseMessage Put(string url, Dictionary<string, string> data) => 
            this.Session.PutAsync(url, new FormUrlEncodedContent(data)).Result;

        public HttpResponseMessage PutJson(string url, string json) => 
            this.Session.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

        public void SetDesktopAgent()
        {
            this.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36");
        }

        public void SetMobileAgent()
        {
            this.Session.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.162 Mobile Safari/537.36");
        }

        public HttpClient Session { get; set; }

        public CookieContainer Cookies { get; set; }

        public HttpClientHandler Handler { get; set; }
    }
}

