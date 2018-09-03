namespace EveAIO
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    internal static class Extensions
    {
        public static void CheckError(HttpResponseMessage response)
        {
            // This item is obfuscated and can not be translated.
            if (response.StatusCode != HttpStatusCode.Forbidden)
            {
                goto Label_0241;
            }
            goto Label_01BB;
        Label_014B:
            if (-759099756 || true)
            {
                goto Label_01BB;
            }
        Label_0171:
            if (-2010927607 || true)
            {
            }
        Label_01BB:
            switch (((((response.StatusCode != ((HttpStatusCode) 0x1a6)) ? -266060074 : -679477714) ^ -1920788466) % 0x19))
            {
                case 0:
                    return;

                case 1:
                    throw new WebException("The remote server returned an error: (500) Internal Server Error");

                case 2:
                case 11:
                case 13:
                case 14:
                case 15:
                case 0x10:
                case 0x13:
                case 0x16:
                    goto Label_01BB;

                case 3:
                    goto Label_0171;

                case 4:
                    goto Label_014B;

                case 5:
                    throw new WebException("The remote server returned an error: (404) Not found");

                case 6:
                    if (-586962790 || true)
                    {
                        goto Label_01BB;
                    }
                    goto Label_014B;

                case 7:
                    throw new WebException("The remote server returned an error: (424) Failed Dependency");

                case 8:
                    throw new WebException("The remote server returned an error: (504) Gateway Timeout");

                case 9:
                    throw new WebException("The remote server returned an error: (412) Precondition Failed");

                case 10:
                    throw new WebException("The remote server returned an error: (429) Too Many Requests");

                case 12:
                    throw new WebException("The remote server returned an error: (503) Server unavailable");

                case 0x11:
                    throw new WebException("The remote server returned an error: (430) Forbidden");

                case 0x12:
                    throw new WebException("The remote server returned an error: (400) Bad Request");

                case 20:
                    break;

                case 0x15:
                    throw new WebException("The remote server returned an error: (403) Forbidden");

                case 0x17:
                    throw new WebException("The remote server returned an error: (407) Proxy Authentication Required");

                case 0x18:
                    throw new WebException("The remote server returned an error: (422) Unprocessable Entity");

                default:
                    return;
            }
        Label_0241:
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
            }
            goto Label_01BB;
        }

        public static CookieContainer FixCookies(this CookieContainer collection)
        {
            foreach (Cookie cookie in collection.List())
            {
                if (cookie.Version == 1)
                {
                    cookie.Version = 0;
                }
                if (!string.IsNullOrEmpty(cookie.Path))
                {
                    int length = cookie.Path.LastIndexOf('/');
                    if (length != -1)
                    {
                        cookie.Path = cookie.Path.Substring(0, length);
                    }
                }
            }
            return collection;
        }

        [return: Dynamic]
        public static object Json(this HttpResponseMessage response)
        {
            CheckError(response);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
        }

        public static List<Cookie> List(this CookieContainer container)
        {
            List<Cookie> list = new List<Cookie>();
            foreach (string str in ((Hashtable) container.GetType().InvokeMember("m_domainTable", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, container, new object[0])).Keys)
            {
                Uri result = null;
                if (str != null)
                {
                    if (str.StartsWith("."))
                    {
                        str = str.Substring(1);
                    }
                    if (Uri.TryCreate($"http://{str}/", UriKind.RelativeOrAbsolute, out result))
                    {
                        foreach (Cookie cookie in container.GetCookies(result))
                        {
                            list.Add(cookie);
                        }
                    }
                }
            }
            return list;
        }

        public static string Output(this object script)
        {
            script.WaitOne(0x7d0);
            return script.Result.ToString();
        }

        [AsyncStateMachine(typeof(<PatchAsync>d__8))]
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent)
        {
            <PatchAsync>d__8 d__;
            d__.client = client;
            d__.requestUri = requestUri;
            d__.iContent = iContent;
            d__.<>t__builder = AsyncTaskMethodBuilder<HttpResponseMessage>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<PatchAsync>d__8>(ref d__);
            return d__.<>t__builder.Task;
        }

        public static string Text(this HttpResponseMessage response)
        {
            CheckError(response);
            return response.Content.ReadAsStringAsync().Result.ToString();
        }

        public static KeyValuePair<string, HttpResponseHeaders> TextHeaders(this HttpResponseMessage response)
        {
            CheckError(response);
            return new KeyValuePair<string, HttpResponseHeaders>(response.Content.ReadAsStringAsync().Result.ToString(), response.Headers);
        }

        public static KeyValuePair<string, string> TextResponseUri(this HttpResponseMessage response)
        {
            CheckError(response);
            return new KeyValuePair<string, string>(response.Content.ReadAsStringAsync().Result.ToString(), response.RequestMessage.RequestUri.ToString());
        }

        [CompilerGenerated]
        private struct <PatchAsync>d__8 : IAsyncStateMachine
        {
            public int <>1__state;
            public AsyncTaskMethodBuilder<HttpResponseMessage> <>t__builder;
            public Uri requestUri;
            public HttpContent iContent;
            public HttpClient client;
            private HttpResponseMessage <response>5__1;
            private TaskAwaiter<HttpResponseMessage> <>u__1;

            private void MoveNext()
            {
                HttpResponseMessage message3;
                int num = this.<>1__state;
                try
                {
                    HttpRequestMessage message;
                    if (num != 0)
                    {
                        message = new HttpRequestMessage(new HttpMethod("PATCH"), this.requestUri) {
                            Content = this.iContent
                        };
                        this.<response>5__1 = new HttpResponseMessage();
                    }
                    try
                    {
                        TaskAwaiter<HttpResponseMessage> awaiter;
                        if (num != 0)
                        {
                            awaiter = this.client.SendAsync(message).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                num = 0;
                                this.<>1__state = 0;
                                this.<>u__1 = awaiter;
                                this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<HttpResponseMessage>, Extensions.<PatchAsync>d__8>(ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter = this.<>u__1;
                            this.<>u__1 = new TaskAwaiter<HttpResponseMessage>();
                            num = -1;
                            this.<>1__state = -1;
                        }
                        HttpResponseMessage result = awaiter.GetResult();
                        this.<response>5__1 = result;
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    message3 = this.<response>5__1;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(message3);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }
    }
}

