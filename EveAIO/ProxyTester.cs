namespace EveAIO
{
    using EveAIO.Pocos;
    using EveAIO.Views;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;

    internal class ProxyTester
    {
        private object _client;
        private ProxyObject _proxyObj;
        private bool _return;
        private string _testUrl;
        private int _timeout;
        private ProxyListWindow _window;

        public ProxyTester(ProxyListWindow window, ProxyObject proxyObj, string testUrl, int timeout)
        {
            Class7.RIuqtBYzWxthF();
            this._proxyObj = proxyObj;
            this._testUrl = testUrl;
            this._timeout = timeout;
            this._window = window;
        }

        public void Start()
        {
            new Thread(new ThreadStart(this.Test)).Start();
            while (!this._return)
            {
                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            this._return = true;
        }

        private void Test()
        {
            try
            {
                WebProxy proxy = null;
                if (this._proxyObj.IP != "localhost")
                {
                    try
                    {
                        if (string.IsNullOrEmpty(this._proxyObj.Username) && string.IsNullOrEmpty(this._proxyObj.Password))
                        {
                            proxy = new WebProxy(this._proxyObj.IP, this._proxyObj.Port);
                        }
                        else
                        {
                            ICredentials credentials = new NetworkCredential(this._proxyObj.Username, this._proxyObj.Password);
                            proxy = new WebProxy(this._proxyObj.IP + ":" + this._proxyObj.Port, true, null, credentials);
                        }
                    }
                    catch (Exception)
                    {
                        this._window.gvProxies.Dispatcher.Invoke(() => this._proxyObj.Status = "INVALID PROXY");
                        return;
                    }
                }
                if (!this._testUrl.Contains("http"))
                {
                    this._testUrl = "http://" + this._testUrl;
                }
                Client client1 = new Client(new CookieContainer(), proxy, true);
                client1.SetDesktopAgent();
                client1.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
                client1.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                client1.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "Accept-Encoding: gzip, deflate");
                client1.Session.Timeout = new TimeSpan(0, 0, this._timeout);
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Extensions.CheckError(client1.Get(this._testUrl));
                if (!this._return)
                {
                    watch.Stop();
                    this._window.gvProxies.Dispatcher.Invoke(delegate {
                        if (!this._return)
                        {
                            this._proxyObj.Status = "ALIVE (" + watch.ElapsedMilliseconds + "ms)";
                            this._proxyObj.State = ProxyObject.StateEnum.alive;
                        }
                    });
                }
                else
                {
                    return;
                }
            }
            catch (WebException exception2)
            {
                WebException ex = exception2;
                if (this._return)
                {
                    return;
                }
                this._window.gvProxies.Dispatcher.Invoke(delegate {
                    this._proxyObj.Status = "DEAD (" + ex.Status + ")";
                    this._proxyObj.State = ProxyObject.StateEnum.dead;
                });
            }
            catch (Exception)
            {
                if (!this._return)
                {
                    this._window.gvProxies.Dispatcher.Invoke(delegate {
                        this._proxyObj.Status = "DEAD";
                        this._proxyObj.State = ProxyObject.StateEnum.dead;
                    });
                }
                else
                {
                    return;
                }
            }
            this._return = true;
        }
    }
}

