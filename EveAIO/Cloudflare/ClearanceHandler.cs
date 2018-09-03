namespace EveAIO.Cloudflare
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    public class ClearanceHandler : DelegatingHandler
    {
        public static readonly int DefaultMaxRetries;
        public static readonly int DefaultClearanceDelay;
        private static readonly IEnumerable<string> CloudFlareServerNames;
        public readonly CookieContainer _cookies;
        private readonly HttpClient _client;
        public WebProxy Proxy;

        static ClearanceHandler()
        {
            Class7.RIuqtBYzWxthF();
            DefaultMaxRetries = 20;
            DefaultClearanceDelay = 0x1388;
            string[] textArray1 = new string[] { "cloudflare", "cloudflare-nginx" };
            CloudFlareServerNames = textArray1;
        }

        public ClearanceHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
            Class7.RIuqtBYzWxthF();
            this._cookies = new CookieContainer();
            this.<MaxRetries>k__BackingField = DefaultMaxRetries;
            this.<ClearanceDelay>k__BackingField = DefaultClearanceDelay;
            HttpClientHandler handler = new HttpClientHandler {
                AllowAutoRedirect = false,
                CookieContainer = this._cookies,
                Proxy = ((HttpClientHandler) innerHandler).Proxy
            };
            this._client = new HttpClient(handler);
        }

        public ClearanceHandler(WebProxy proxy) : this(handler1)
        {
            Class7.RIuqtBYzWxthF();
            HttpClientHandler handler1 = new HttpClientHandler {
                Proxy = proxy,
                UseProxy = true
            };
            this.Proxy = proxy;
        }

        [DebuggerHidden, CompilerGenerated]
        private Task<HttpResponseMessage> <>n__0(HttpRequestMessage request, CancellationToken cancellationToken) => 
            base.SendAsync(request, cancellationToken);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._client.Dispose();
            }
            base.Dispose(disposing);
        }

        private static void EnsureClientHeader(HttpRequestMessage request)
        {
            if (!request.Headers.UserAgent.Any<ProductInfoHeaderValue>())
            {
                request.Headers.Add("User-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            }
        }

        private void InjectCookies(HttpRequestMessage request)
        {
            List<Cookie> source = this._cookies.GetCookies(request.RequestUri).Cast<Cookie>().ToList<Cookie>();
            Cookie cookie = source.FirstOrDefault<Cookie>(c => c.Name == "__cfduid");
            Cookie cookie2 = source.FirstOrDefault<Cookie>(c => c.Name == "cf_clearance");
            if ((cookie != null) && (cookie2 != null))
            {
                if (!this.ClientHandler.UseCookies)
                {
                    request.Headers.Add("Cookie", cookie.ToHeaderValue());
                    request.Headers.Add("Cookie", cookie2.ToHeaderValue());
                }
                else
                {
                    this.ClientHandler.CookieContainer.Add(request.RequestUri, cookie);
                    this.ClientHandler.CookieContainer.Add(request.RequestUri, cookie2);
                }
            }
        }

        private static bool IsClearanceRequired(HttpResponseMessage response) => 
            ((response.StatusCode == HttpStatusCode.ServiceUnavailable) & response.Headers.Server.Any<ProductInfoHeaderValue>(i => ((i.Product != null) && CloudFlareServerNames.Any<string>(s => (string.Compare(s, i.Product.Name, StringComparison.OrdinalIgnoreCase) == 0)))));

        [AsyncStateMachine(typeof(<PassClearance>d__25))]
        private Task PassClearance(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            <PassClearance>d__25 d__;
            d__.<>4__this = this;
            d__.response = response;
            d__.cancellationToken = cancellationToken;
            d__.<>t__builder = AsyncTaskMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<PassClearance>d__25>(ref d__);
            return d__.<>t__builder.Task;
        }

        private void SaveIdCookie(HttpResponseMessage response)
        {
            List<string> source = (from pair in response.Headers
                where pair.Key == "Set-Cookie"
                select pair.Value into cookie
                where cookie.StartsWith($"{"__cfduid"}=")
                select cookie).ToList<string>();
            if (source.Any<string>())
            {
                foreach (Cookie cookie in this.ClientHandler.CookieContainer.GetCookies(response.RequestMessage.RequestUri))
                {
                    if ((cookie.Name == "__cfduid") || (cookie.Name == "cf_clearance"))
                    {
                        cookie.Expired = true;
                    }
                }
                foreach (string str in source)
                {
                    this._cookies.SetCookies(response.RequestMessage.RequestUri, str);
                }
            }
        }

        [AsyncStateMachine(typeof(<SendAsync>d__21))]
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            <SendAsync>d__21 d__;
            d__.<>4__this = this;
            d__.request = request;
            d__.cancellationToken = cancellationToken;
            d__.<>t__builder = AsyncTaskMethodBuilder<HttpResponseMessage>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SendAsync>d__21>(ref d__);
            return d__.<>t__builder.Task;
        }

        public int MaxRetries { get; set; }

        public int ClearanceDelay { get; set; }

        private HttpClientHandler ClientHandler =>
            (base.InnerHandler.GetMostInnerHandler() as HttpClientHandler);

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ClearanceHandler.<>c <>9;
            public static Func<ProductInfoHeaderValue, bool> <>9__23_0;
            public static Func<Cookie, bool> <>9__24_0;
            public static Func<Cookie, bool> <>9__24_1;
            public static Func<KeyValuePair<string, IEnumerable<string>>, bool> <>9__26_0;
            public static Func<KeyValuePair<string, IEnumerable<string>>, IEnumerable<string>> <>9__26_1;
            public static Func<string, bool> <>9__26_2;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new ClearanceHandler.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <InjectCookies>b__24_0(Cookie c) => 
                (c.Name == "__cfduid");

            internal bool <InjectCookies>b__24_1(Cookie c) => 
                (c.Name == "cf_clearance");

            internal bool <IsClearanceRequired>b__23_0(ProductInfoHeaderValue i) => 
                ((i.Product != null) && ClearanceHandler.CloudFlareServerNames.Any<string>(s => (string.Compare(s, i.Product.Name, StringComparison.OrdinalIgnoreCase) == 0)));

            internal bool <SaveIdCookie>b__26_0(KeyValuePair<string, IEnumerable<string>> pair) => 
                (pair.Key == "Set-Cookie");

            internal IEnumerable<string> <SaveIdCookie>b__26_1(KeyValuePair<string, IEnumerable<string>> pair) => 
                pair.Value;

            internal bool <SaveIdCookie>b__26_2(string cookie) => 
                cookie.StartsWith($"{"__cfduid"}=");
        }

        [CompilerGenerated]
        private struct <PassClearance>d__25 : IAsyncStateMachine
        {
            public int <>1__state;
            public AsyncTaskMethodBuilder <>t__builder;
            public ClearanceHandler <>4__this;
            public HttpResponseMessage response;
            public CancellationToken cancellationToken;
            private string <clearanceUri>5__1;
            private ConfiguredTaskAwaitable<string>.ConfiguredTaskAwaiter <>u__1;
            private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter <>u__2;
            private ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter <>u__3;

            private void MoveNext()
            {
                int num = this.<>1__state;
                ClearanceHandler handler = this.<>4__this;
                try
                {
                    HttpResponseMessage message;
                    ConfiguredTaskAwaitable<string>.ConfiguredTaskAwaiter awaiter2;
                    string str;
                    switch (num)
                    {
                        case 0:
                            break;

                        case 1:
                            goto Label_0170;

                        case 2:
                            goto Label_0225;

                        default:
                            handler.SaveIdCookie(this.response);
                            awaiter2 = this.response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter();
                            if (awaiter2.IsCompleted)
                            {
                                goto Label_0097;
                            }
                            num = 0;
                            this.<>1__state = 0;
                            this.<>u__1 = awaiter2;
                            this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable<string>.ConfiguredTaskAwaiter, ClearanceHandler.<PassClearance>d__25>(ref awaiter2, ref this);
                            return;
                    }
                    awaiter2 = this.<>u__1;
                    this.<>u__1 = new ConfiguredTaskAwaitable<string>.ConfiguredTaskAwaiter();
                    num = -1;
                    this.<>1__state = -1;
                Label_0097:
                    str = this.response.RequestMessage.RequestUri.Scheme;
                    string host = this.response.RequestMessage.RequestUri.Host;
                    int port = this.response.RequestMessage.RequestUri.Port;
                    ChallengeSolution solution = ChallengeSolver.Solve(awaiter2.GetResult(), host);
                    this.<clearanceUri>5__1 = $"{str}://{host}:{port}{solution.ClearanceQuery}";
                    ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter3 = Task.Delay(handler.ClearanceDelay, this.cancellationToken).ConfigureAwait(false).GetAwaiter();
                    if (awaiter3.IsCompleted)
                    {
                        goto Label_018D;
                    }
                    num = 1;
                    this.<>1__state = 1;
                    this.<>u__2 = awaiter3;
                    this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, ClearanceHandler.<PassClearance>d__25>(ref awaiter3, ref this);
                    return;
                Label_0170:
                    awaiter3 = this.<>u__2;
                    this.<>u__2 = new ConfiguredTaskAwaitable.ConfiguredTaskAwaiter();
                    num = -1;
                    this.<>1__state = -1;
                Label_018D:
                    awaiter3.GetResult();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, this.<clearanceUri>5__1);
                    if (this.response.RequestMessage.Headers.TryGetValues("User-Agent", out IEnumerable<string> enumerable))
                    {
                        request.Headers.Add("User-Agent", enumerable);
                    }
                    ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter awaiter = handler._client.SendAsync(request, this.cancellationToken).ConfigureAwait(false).GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0241;
                    }
                    num = 2;
                    this.<>1__state = 2;
                    this.<>u__3 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter, ClearanceHandler.<PassClearance>d__25>(ref awaiter, ref this);
                    return;
                Label_0225:
                    awaiter = this.<>u__3;
                    this.<>u__3 = new ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter();
                    num = -1;
                    this.<>1__state = -1;
                Label_0241:
                    message = awaiter.GetResult();
                    handler.SaveIdCookie(message);
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SendAsync>d__21 : IAsyncStateMachine
        {
            public int <>1__state;
            public AsyncTaskMethodBuilder<HttpResponseMessage> <>t__builder;
            public ClearanceHandler <>4__this;
            public HttpRequestMessage request;
            public CancellationToken cancellationToken;
            private int <retries>5__1;
            private Cookie <idCookieBefore>5__2;
            private Cookie <clearanceCookieBefore>5__3;
            private ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter <>u__1;
            private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter <>u__2;

            private void MoveNext()
            {
                HttpResponseMessage message;
                int num = this.<>1__state;
                ClearanceHandler handler = this.<>4__this;
                try
                {
                    ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter awaiter;
                    HttpResponseMessage message2;
                    ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter2;
                    switch (num)
                    {
                        case 0:
                            break;

                        case 1:
                            awaiter2 = this.<>u__2;
                            this.<>u__2 = new ConfiguredTaskAwaitable.ConfiguredTaskAwaiter();
                            num = -1;
                            this.<>1__state = -1;
                            goto Label_01BD;

                        case 2:
                            awaiter = this.<>u__1;
                            this.<>u__1 = new ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter();
                            num = -1;
                            this.<>1__state = -1;
                            goto Label_01FC;

                        default:
                        {
                            string[] textArray1 = new string[] { "__cfduid" };
                            this.<idCookieBefore>5__2 = handler.ClientHandler.CookieContainer.GetCookiesByName(this.request.RequestUri, textArray1).FirstOrDefault<Cookie>();
                            string[] textArray2 = new string[] { "cf_clearance" };
                            this.<clearanceCookieBefore>5__3 = handler.ClientHandler.CookieContainer.GetCookiesByName(this.request.RequestUri, textArray2).FirstOrDefault<Cookie>();
                            ClearanceHandler.EnsureClientHeader(this.request);
                            handler.InjectCookies(this.request);
                            awaiter = handler.<>n__0(this.request, this.cancellationToken).ConfigureAwait(false).GetAwaiter();
                            if (awaiter.IsCompleted)
                            {
                                goto Label_010E;
                            }
                            num = 0;
                            this.<>1__state = 0;
                            this.<>u__1 = awaiter;
                            this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter, ClearanceHandler.<SendAsync>d__21>(ref awaiter, ref this);
                            return;
                        }
                    }
                    awaiter = this.<>u__1;
                    this.<>u__1 = new ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter();
                    num = -1;
                    this.<>1__state = -1;
                Label_010E:
                    message2 = awaiter.GetResult();
                    this.<retries>5__1 = 0;
                Label_0161:
                    if (!ClearanceHandler.IsClearanceRequired(message2) || ((handler.MaxRetries >= 0) && (this.<retries>5__1 > handler.MaxRetries)))
                    {
                        goto Label_0264;
                    }
                    this.cancellationToken.ThrowIfCancellationRequested();
                    awaiter2 = handler.PassClearance(message2, this.cancellationToken).ConfigureAwait(false).GetAwaiter();
                    if (!awaiter2.IsCompleted)
                    {
                        goto Label_0240;
                    }
                Label_01BD:
                    awaiter2.GetResult();
                    handler.InjectCookies(this.request);
                    awaiter = handler.<>n__0(this.request, this.cancellationToken).ConfigureAwait(false).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_021C;
                    }
                Label_01FC:
                    message2 = awaiter.GetResult();
                    int num3 = this.<retries>5__1;
                    this.<retries>5__1 = num3 + 1;
                    goto Label_0161;
                Label_021C:
                    num = 2;
                    this.<>1__state = 2;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter, ClearanceHandler.<SendAsync>d__21>(ref awaiter, ref this);
                    return;
                Label_0240:
                    num = 1;
                    this.<>1__state = 1;
                    this.<>u__2 = awaiter2;
                    this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, ClearanceHandler.<SendAsync>d__21>(ref awaiter2, ref this);
                    return;
                Label_0264:
                    if (ClearanceHandler.IsClearanceRequired(message2))
                    {
                        throw new CloudFlareClearanceException(this.<retries>5__1);
                    }
                    string[] names = new string[] { "__cfduid" };
                    Cookie cookie2 = handler.ClientHandler.CookieContainer.GetCookiesByName(this.request.RequestUri, names).FirstOrDefault<Cookie>();
                    string[] textArray4 = new string[] { "cf_clearance" };
                    Cookie cookie = handler.ClientHandler.CookieContainer.GetCookiesByName(this.request.RequestUri, textArray4).FirstOrDefault<Cookie>();
                    if ((cookie2 != null) && (cookie2 != this.<idCookieBefore>5__2))
                    {
                        message2.Headers.Add("Set-Cookie", cookie2.ToHeaderValue());
                    }
                    if ((cookie != null) && (cookie != this.<clearanceCookieBefore>5__3))
                    {
                        message2.Headers.Add("Set-Cookie", cookie.ToHeaderValue());
                    }
                    message = message2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(message);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }
    }
}

