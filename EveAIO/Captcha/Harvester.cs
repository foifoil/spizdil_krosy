namespace EveAIO.Captcha
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading;

    public sealed class Harvester
    {
        private readonly Func<HttpListenerRequest, string, string, string> _listenFunc;
        private readonly HttpListener _listener;
        public string Website;
        public string Sitekey;
        private CookieContainer _cookies;

        public Harvester(Func<HttpListenerRequest, string, string, string> func_1, string website, string siteKey, params string[] string_0) : this(string_0, func_1, website, siteKey, null)
        {
            Class7.RIuqtBYzWxthF();
        }

        public Harvester(string[] string_0, Func<HttpListenerRequest, string, string, string> listenFunc, string website, string siteKey, CookieContainer cookies)
        {
            Class7.RIuqtBYzWxthF();
            this.Website = website;
            this.Sitekey = siteKey;
            this._listener = new HttpListener();
            this._cookies = cookies;
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            }
            if ((string_0 == null) || (string_0.Length == 0))
            {
                throw new ArgumentException("prefixes");
            }
            if (listenFunc == null)
            {
                throw new ArgumentException("method");
            }
            foreach (string str in string_0)
            {
                this._listener.Prefixes.Add(str);
            }
            this._listener.IgnoreWriteExceptions = true;
            this._listenFunc = listenFunc;
            this._listener.Start();
        }

        private void GetResponse(object obj)
        {
            HttpListenerContext context = obj as HttpListenerContext;
            try
            {
                string s = this._listenFunc(context.Request, this.Website, this.Sitekey);
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                context.Response.ContentLength64 = bytes.Length;
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.ContentType = "text/html";
                context.Response.AddHeader("Date", DateTime.Now.AddHours(-2.0).ToString("r"));
                context.Response.AddHeader("Last-Modified", DateTime.Now.AddHours(-2.0).ToString("r"));
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Cache-Control", "no-cache");
                context.Response.StatusCode = 200;
                context.Response.OutputStream.Flush();
            }
            catch
            {
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }

        private void Listen(object object_0)
        {
            Console.WriteLine("Webserver running...");
            try
            {
                while (this._listener.IsListening)
                {
                Label_0018:
                    if (!0x664c9f9d && !true)
                    {
                    }
                    goto Label_0044;
                Label_0022:
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetResponse), this._listener.GetContext());
                Label_0044:
                    switch (((0x11a175c0 ^ 0xd543ff4) % 4))
                    {
                        case 0:
                        {
                            continue;
                        }
                        case 1:
                            goto Label_0022;

                        case 3:
                            goto Label_0044;
                    }
                    return;
                }
                goto Label_0018;
            }
            catch
            {
            }
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Listen));
        }

        public void Stop()
        {
            try
            {
                this._listener.Stop();
                this._listener.Close();
            }
            catch
            {
            }
        }
    }
}

