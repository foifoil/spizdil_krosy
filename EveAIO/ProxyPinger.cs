namespace EveAIO
{
    using EveAIO.Pocos;
    using EveAIO.Views;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading;

    internal class ProxyPinger
    {
        private ProxyObject _proxyObj;
        private bool _return;
        private int _timeout;
        private ProxyListWindow _window;

        public ProxyPinger(ProxyListWindow window, ProxyObject proxyObj, int timeout)
        {
            Class7.RIuqtBYzWxthF();
            this._proxyObj = proxyObj;
            this._timeout = timeout;
            this._window = window;
        }

        public void Start()
        {
            // This item is obfuscated and can not be translated.
            new Thread(new ThreadStart(this.Test)).Start();
            while (this._return)
            {
            Label_0022:
                if (!0x2450501b && !true)
                {
                }
                goto Label_0038;
            Label_002C:
                Thread.Sleep(100);
            Label_0038:
                switch (((0x237c4260 ^ 0x3a213a0) % 4))
                {
                    case 0:
                    {
                        continue;
                    }
                    case 1:
                        goto Label_002C;

                    case 2:
                        goto Label_0038;
                }
                return;
            }
            goto Label_0022;
        }

        public void Stop()
        {
            this._return = true;
        }

        private void Test()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                PingReply reply = new Ping().Send(this._proxyObj.IP, this._proxyObj.Port);
                if (this._return)
                {
                    return;
                }
                stopwatch.Stop();
                if (reply != null)
                {
                    Console.WriteLine(string.Concat(new object[] { "Status :  ", reply.Status, " \n Time : ", reply.RoundtripTime.ToString(), " \n Address : ", reply.Address }));
                    this._window.gvProxies.Dispatcher.Invoke(delegate {
                        if (!this._return)
                        {
                            if (reply.Status != IPStatus.TimedOut)
                            {
                                if (reply.Status != IPStatus.Success)
                                {
                                    this._proxyObj.Status = reply.Status.ToString();
                                    this._proxyObj.State = ProxyObject.StateEnum.dead;
                                }
                                else
                                {
                                    this._proxyObj.Status = "ALIVE (" + reply.RoundtripTime.ToString() + "ms)";
                                    this._proxyObj.State = ProxyObject.StateEnum.alive;
                                }
                            }
                            else
                            {
                                this._proxyObj.Status = "timeout";
                                this._proxyObj.State = ProxyObject.StateEnum.dead;
                            }
                        }
                    });
                }
            }
            catch (WebException exception1)
            {
                WebException ex = exception1;
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

