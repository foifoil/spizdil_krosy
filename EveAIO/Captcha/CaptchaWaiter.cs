namespace EveAIO.Captcha
{
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    internal class CaptchaWaiter
    {
        private static Random _rnd;
        private object _mre;
        private DateTime? _timestamp;
        public string _platform;
        private TaskObject _task;

        static CaptchaWaiter()
        {
            Class7.RIuqtBYzWxthF();
            _rnd = new Random(DateTime.Now.Millisecond);
        }

        public CaptchaWaiter(TaskObject task, DateTime? timestamp, string captchaKey, string link, string platform)
        {
            Class7.RIuqtBYzWxthF();
            this._mre = task.Mre;
            this._timestamp = timestamp;
            this._platform = platform;
            this._task = task;
            if (!task.SpecificCaptcha)
            {
                List<CaptchaSolver.CaptchaService> list = new List<CaptchaSolver.CaptchaService>();
                if (!string.IsNullOrEmpty(Global.SETTINGS.TwoCaptchaApiKey))
                {
                    list.Add(CaptchaSolver.CaptchaService.TwoCaptcha);
                }
                if (!string.IsNullOrEmpty(Global.SETTINGS.AntiCaptchaApiKey))
                {
                    list.Add(CaptchaSolver.CaptchaService.AntiCaptcha);
                }
                if (!string.IsNullOrEmpty(Global.SETTINGS.ImageTypersUsername) && !string.IsNullOrEmpty(Global.SETTINGS.ImageTypersPassword))
                {
                    list.Add(CaptchaSolver.CaptchaService.ImageTypers);
                }
                if (!string.IsNullOrEmpty(Global.SETTINGS.DisolveApiKey) && !string.IsNullOrEmpty(Global.SETTINGS.DisolveIp))
                {
                    list.Add(CaptchaSolver.CaptchaService.Disolve);
                }
                if (list.Count > 0)
                {
                    for (int i = 0; i < task.CaptchaRequests; i++)
                    {
                        int num2 = _rnd.Next(0, list.Count);
                        switch (list[num2])
                        {
                            case CaptchaSolver.CaptchaService.TwoCaptcha:
                                Task.Factory.StartNew(delegate {
                                    CaptchaToken item = new CaptchaToken {
                                        Id = Guid.NewGuid().ToString(),
                                        Created = DateTime.Now,
                                        CaptchaType = CaptchaSolver.CaptchaService.TwoCaptcha,
                                        Website = task.HomeUrl,
                                        Platform = platform
                                    };
                                    item.Token = CaptchaSolver.TwoCaptchaSolve(captchaKey, link, "");
                                    item.Timestamp = DateTime.Now;
                                    item.Expires = item.Timestamp.AddMinutes(2.0);
                                    TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                                    item.SolveTime = (span.Minutes * 60) + span.Seconds;
                                    if (!string.IsNullOrEmpty(item.Token))
                                    {
                                        if (item.Platform == "Shopify")
                                        {
                                            Global.ShopifyTokens.Add(item);
                                        }
                                        else if (item.Platform == "Supreme")
                                        {
                                            Global.SupremeTokens.Add(item);
                                        }
                                        else if (item.Platform == "Sneakernstuff")
                                        {
                                            Global.SnsTokens.Add(item);
                                        }
                                        else if (item.Platform == "Hibbett")
                                        {
                                            Global.HibbettTokens.Add(item);
                                        }
                                        else if (item.Platform == "MrPorter")
                                        {
                                            Global.MrPorterTokens.Add(item);
                                        }
                                        else if (item.Platform == "OW")
                                        {
                                            Global.OWTokens.Add(item);
                                        }
                                        else if (item.Platform == "Footaction")
                                        {
                                            Global.FootactionTokens.Add(item);
                                        }
                                        else if (item.Platform == "Holypopstore")
                                        {
                                            Global.HolypopTokens.Add(item);
                                        }
                                    }
                                });
                                break;

                            case CaptchaSolver.CaptchaService.AntiCaptcha:
                                Task.Factory.StartNew(delegate {
                                    CaptchaToken item = new CaptchaToken {
                                        Id = Guid.NewGuid().ToString(),
                                        Created = DateTime.Now,
                                        CaptchaType = CaptchaSolver.CaptchaService.AntiCaptcha,
                                        Website = task.HomeUrl,
                                        Platform = platform
                                    };
                                    item.Token = CaptchaSolver.AntiCaptchaSolve(captchaKey, link, "");
                                    item.Timestamp = DateTime.Now;
                                    item.Expires = item.Timestamp.AddMinutes(2.0);
                                    TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                                    item.SolveTime = (span.Minutes * 60) + span.Seconds;
                                    if (!string.IsNullOrEmpty(item.Token))
                                    {
                                        if (item.Platform == "Shopify")
                                        {
                                            Global.ShopifyTokens.Add(item);
                                        }
                                        else if (item.Platform == "Supreme")
                                        {
                                            Global.SupremeTokens.Add(item);
                                        }
                                        else if (item.Platform == "Sneakernstuff")
                                        {
                                            Global.SnsTokens.Add(item);
                                        }
                                        else if (item.Platform == "Hibbett")
                                        {
                                            Global.HibbettTokens.Add(item);
                                        }
                                        else if (item.Platform == "MrPorter")
                                        {
                                            Global.MrPorterTokens.Add(item);
                                        }
                                        else if (item.Platform == "OW")
                                        {
                                            Global.OWTokens.Add(item);
                                        }
                                        else if (item.Platform == "Footaction")
                                        {
                                            Global.FootactionTokens.Add(item);
                                        }
                                        else if (item.Platform == "Holypopstore")
                                        {
                                            Global.HolypopTokens.Add(item);
                                        }
                                    }
                                });
                                break;

                            case CaptchaSolver.CaptchaService.ImageTypers:
                                Task.Factory.StartNew(delegate {
                                    CaptchaToken item = new CaptchaToken {
                                        Id = Guid.NewGuid().ToString(),
                                        Created = DateTime.Now,
                                        CaptchaType = CaptchaSolver.CaptchaService.ImageTypers,
                                        Website = task.HomeUrl,
                                        Platform = platform
                                    };
                                    item.Token = CaptchaSolver.ImageTypersSolve(captchaKey, link, "");
                                    item.Timestamp = DateTime.Now;
                                    item.Expires = item.Timestamp.AddMinutes(2.0);
                                    TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                                    item.SolveTime = (span.Minutes * 60) + span.Seconds;
                                    if (!string.IsNullOrEmpty(item.Token))
                                    {
                                        if (item.Platform == "Shopify")
                                        {
                                            Global.ShopifyTokens.Add(item);
                                        }
                                        else if (item.Platform == "Supreme")
                                        {
                                            Global.SupremeTokens.Add(item);
                                        }
                                        else if (item.Platform == "Sneakernstuff")
                                        {
                                            Global.SnsTokens.Add(item);
                                        }
                                        else if (item.Platform == "Hibbett")
                                        {
                                            Global.HibbettTokens.Add(item);
                                        }
                                        else if (item.Platform == "MrPorter")
                                        {
                                            Global.MrPorterTokens.Add(item);
                                        }
                                        else if (item.Platform == "OW")
                                        {
                                            Global.OWTokens.Add(item);
                                        }
                                        else if (item.Platform == "Footaction")
                                        {
                                            Global.FootactionTokens.Add(item);
                                        }
                                        else if (item.Platform == "Holypopstore")
                                        {
                                            Global.HolypopTokens.Add(item);
                                        }
                                    }
                                });
                                break;

                            case CaptchaSolver.CaptchaService.Disolve:
                                Task.Factory.StartNew(delegate {
                                    CaptchaToken item = new CaptchaToken {
                                        Id = Guid.NewGuid().ToString(),
                                        Created = DateTime.Now,
                                        CaptchaType = CaptchaSolver.CaptchaService.Disolve,
                                        Website = task.HomeUrl,
                                        Platform = platform
                                    };
                                    item.Token = CaptchaSolver.DisolveSolve(captchaKey, link, "");
                                    item.Timestamp = DateTime.Now;
                                    item.Expires = item.Timestamp.AddMinutes(2.0);
                                    TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                                    item.SolveTime = (span.Minutes * 60) + span.Seconds;
                                    if (!string.IsNullOrEmpty(item.Token))
                                    {
                                        if (item.Platform == "Shopify")
                                        {
                                            Global.ShopifyTokens.Add(item);
                                        }
                                        else if (item.Platform == "Supreme")
                                        {
                                            Global.SupremeTokens.Add(item);
                                        }
                                        else if (item.Platform == "Sneakernstuff")
                                        {
                                            Global.SnsTokens.Add(item);
                                        }
                                        else if (item.Platform == "Hibbett")
                                        {
                                            Global.HibbettTokens.Add(item);
                                        }
                                        else if (item.Platform == "MrPorter")
                                        {
                                            Global.MrPorterTokens.Add(item);
                                        }
                                        else if (item.Platform == "OW")
                                        {
                                            Global.OWTokens.Add(item);
                                        }
                                        else if (item.Platform == "Footaction")
                                        {
                                            Global.FootactionTokens.Add(item);
                                        }
                                        else if (item.Platform == "Holypopstore")
                                        {
                                            Global.HolypopTokens.Add(item);
                                        }
                                    }
                                });
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < task.TwoCaptchaRequests; i++)
                {
                    Task.Factory.StartNew(delegate {
                        CaptchaToken item = new CaptchaToken {
                            Id = Guid.NewGuid().ToString(),
                            Created = DateTime.Now,
                            CaptchaType = CaptchaSolver.CaptchaService.TwoCaptcha,
                            Website = task.HomeUrl,
                            Platform = platform
                        };
                        item.Token = CaptchaSolver.TwoCaptchaSolve(captchaKey, link, "");
                        item.Timestamp = DateTime.Now;
                        item.Expires = item.Timestamp.AddMinutes(2.0);
                        TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                        item.SolveTime = (span.Minutes * 60) + span.Seconds;
                        if (!string.IsNullOrEmpty(item.Token))
                        {
                            if (item.Platform == "Shopify")
                            {
                                Global.ShopifyTokens.Add(item);
                            }
                            else if (item.Platform == "Supreme")
                            {
                                Global.SupremeTokens.Add(item);
                            }
                            else if (item.Platform == "Sneakernstuff")
                            {
                                Global.SnsTokens.Add(item);
                            }
                            else if (item.Platform == "Hibbett")
                            {
                                Global.HibbettTokens.Add(item);
                            }
                            else if (item.Platform == "MrPorter")
                            {
                                Global.MrPorterTokens.Add(item);
                            }
                            else if (item.Platform == "OW")
                            {
                                Global.OWTokens.Add(item);
                            }
                            else if (item.Platform == "Footaction")
                            {
                                Global.FootactionTokens.Add(item);
                            }
                            else if (item.Platform == "Holypopstore")
                            {
                                Global.HolypopTokens.Add(item);
                            }
                        }
                    });
                }
                for (int j = 0; j < task.DisolveRequests; j++)
                {
                    Task.Factory.StartNew(delegate {
                        CaptchaToken item = new CaptchaToken {
                            Id = Guid.NewGuid().ToString(),
                            Created = DateTime.Now,
                            CaptchaType = CaptchaSolver.CaptchaService.Disolve,
                            Website = task.HomeUrl,
                            Platform = platform
                        };
                        item.Token = CaptchaSolver.DisolveSolve(captchaKey, link, "");
                        item.Timestamp = DateTime.Now;
                        item.Expires = item.Timestamp.AddMinutes(2.0);
                        TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                        item.SolveTime = (span.Minutes * 60) + span.Seconds;
                        if (!string.IsNullOrEmpty(item.Token))
                        {
                            if (item.Platform == "Shopify")
                            {
                                Global.ShopifyTokens.Add(item);
                            }
                            else if (item.Platform == "Supreme")
                            {
                                Global.SupremeTokens.Add(item);
                            }
                            else if (item.Platform == "Sneakernstuff")
                            {
                                Global.SnsTokens.Add(item);
                            }
                            else if (item.Platform == "Hibbett")
                            {
                                Global.HibbettTokens.Add(item);
                            }
                            else if (item.Platform == "MrPorter")
                            {
                                Global.MrPorterTokens.Add(item);
                            }
                            else if (item.Platform == "OW")
                            {
                                Global.OWTokens.Add(item);
                            }
                            else if (item.Platform == "Footaction")
                            {
                                Global.FootactionTokens.Add(item);
                            }
                            else if (item.Platform == "Holypopstore")
                            {
                                Global.HolypopTokens.Add(item);
                            }
                        }
                    });
                }
                for (int k = 0; k < task.AnticaptchaRequests; k++)
                {
                    Task.Factory.StartNew(delegate {
                        CaptchaToken item = new CaptchaToken {
                            Id = Guid.NewGuid().ToString(),
                            Created = DateTime.Now,
                            CaptchaType = CaptchaSolver.CaptchaService.AntiCaptcha,
                            Website = task.HomeUrl,
                            Platform = platform
                        };
                        item.Token = CaptchaSolver.AntiCaptchaSolve(captchaKey, link, "");
                        item.Timestamp = DateTime.Now;
                        item.Expires = item.Timestamp.AddMinutes(2.0);
                        TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                        item.SolveTime = (span.Minutes * 60) + span.Seconds;
                        if (!string.IsNullOrEmpty(item.Token))
                        {
                            if (item.Platform == "Shopify")
                            {
                                Global.ShopifyTokens.Add(item);
                            }
                            else if (item.Platform == "Supreme")
                            {
                                Global.SupremeTokens.Add(item);
                            }
                            else if (item.Platform == "Sneakernstuff")
                            {
                                Global.SnsTokens.Add(item);
                            }
                            else if (item.Platform == "Hibbett")
                            {
                                Global.HibbettTokens.Add(item);
                            }
                            else if (item.Platform == "MrPorter")
                            {
                                Global.MrPorterTokens.Add(item);
                            }
                            else if (item.Platform == "OW")
                            {
                                Global.OWTokens.Add(item);
                            }
                            else if (item.Platform == "Footaction")
                            {
                                Global.FootactionTokens.Add(item);
                            }
                            else if (item.Platform == "Holypopstore")
                            {
                                Global.HolypopTokens.Add(item);
                            }
                        }
                    });
                }
                for (int m = 0; m < task.ImagetypersRequests; m++)
                {
                    Task.Factory.StartNew(delegate {
                        CaptchaToken item = new CaptchaToken {
                            Id = Guid.NewGuid().ToString(),
                            Created = DateTime.Now,
                            CaptchaType = CaptchaSolver.CaptchaService.ImageTypers,
                            Website = task.HomeUrl,
                            Platform = platform
                        };
                        item.Token = CaptchaSolver.ImageTypersSolve(captchaKey, link, "");
                        item.Timestamp = DateTime.Now;
                        TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                        item.SolveTime = (span.Minutes * 60) + span.Seconds;
                        item.Expires = item.Timestamp.AddMinutes(2.0);
                        if (!string.IsNullOrEmpty(item.Token))
                        {
                            if (item.Platform == "Shopify")
                            {
                                Global.ShopifyTokens.Add(item);
                            }
                            else if (item.Platform == "Supreme")
                            {
                                Global.SupremeTokens.Add(item);
                            }
                            else if (item.Platform == "Sneakernstuff")
                            {
                                Global.SnsTokens.Add(item);
                            }
                            else if (item.Platform == "Hibbett")
                            {
                                Global.HibbettTokens.Add(item);
                            }
                            else if (item.Platform == "MrPorter")
                            {
                                Global.MrPorterTokens.Add(item);
                            }
                            else if (item.Platform == "OW")
                            {
                                Global.OWTokens.Add(item);
                            }
                            else if (item.Platform == "Footaction")
                            {
                                Global.FootactionTokens.Add(item);
                            }
                            else if (item.Platform == "Holypopstore")
                            {
                                Global.HolypopTokens.Add(item);
                            }
                        }
                    });
                }
            }
        }

        public void Start()
        {
            object obj2;
            goto Label_05FC;
        Label_05F2:
            Thread.Sleep(200);
        Label_05FC:
            obj2 = Global.CaptchaLocker;
            lock (obj2)
            {
                if (this._platform == "Supreme")
                {
                    if (Global.SupremeTokens.Any<CaptchaToken>(delegate (CaptchaToken x) {
                        DateTime created = x.Created;
                        DateTime? nullable = this._timestamp;
                        return ((nullable.HasValue ? (created > nullable.GetValueOrDefault()) : false) && !string.IsNullOrEmpty(x.Token)) && (x.Expires > DateTime.Now);
                    }))
                    {
                        CaptchaToken item = Global.SupremeTokens.Where<CaptchaToken>(delegate (CaptchaToken x) {
                            DateTime created = x.Created;
                            DateTime? nullable = this._timestamp;
                            return (((nullable.HasValue ? (created > nullable.GetValueOrDefault()) : false) && !string.IsNullOrEmpty(x.Token)) && (x.Expires > DateTime.Now));
                        }).FirstOrDefault<CaptchaToken>();
                        this.Token = item.Token;
                        Global.SupremeTokens.Remove(item);
                        this._mre.Set();
                        return;
                    }
                }
                else if (this._platform == "Shopify")
                {
                    if (Global.ShopifyTokens.Any<CaptchaToken>(delegate (CaptchaToken x) {
                        DateTime created = x.Created;
                        DateTime? nullable = this._timestamp;
                        return (((nullable.HasValue ? (created >= nullable.GetValueOrDefault()) : false) && !string.IsNullOrEmpty(x.Token)) && (x.Expires > DateTime.Now)) && x.Website.ToLowerInvariant().Contains(this._task.HomeUrl.ToLowerInvariant());
                    }))
                    {
                        CaptchaToken item = Global.ShopifyTokens.Where<CaptchaToken>(delegate (CaptchaToken x) {
                            DateTime created = x.Created;
                            DateTime? nullable = this._timestamp;
                            return ((((nullable.HasValue ? (created >= nullable.GetValueOrDefault()) : false) && !string.IsNullOrEmpty(x.Token)) && (x.Expires > DateTime.Now)) && x.Website.ToLowerInvariant().Contains(this._task.HomeUrl.ToLowerInvariant()));
                        }).FirstOrDefault<CaptchaToken>();
                        this.Token = item.Token;
                        Global.ShopifyTokens.Remove(item);
                        this._mre.Set();
                        return;
                    }
                }
                else if (this._platform == "Sneakernstuff")
                {
                    if (Global.SnsTokens.Any<CaptchaToken>(x => !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)))
                    {
                        CaptchaToken item = (from x in Global.SnsTokens
                            where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                            select x).FirstOrDefault<CaptchaToken>();
                        this.Token = item.Token;
                        Global.SnsTokens.Remove(item);
                        this._mre.Set();
                        return;
                    }
                }
                else if (this._platform == "MrPorter")
                {
                    if (Global.MrPorterTokens.Any<CaptchaToken>(x => !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)))
                    {
                        CaptchaToken item = (from x in Global.MrPorterTokens
                            where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                            select x).FirstOrDefault<CaptchaToken>();
                        this.Token = item.Token;
                        Global.MrPorterTokens.Remove(item);
                        this._mre.Set();
                        return;
                    }
                }
                else if (this._platform == "Sivas")
                {
                    if (Global.SivasTokens.Any<CaptchaToken>(x => !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)))
                    {
                        CaptchaToken item = (from x in Global.SivasTokens
                            where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                            select x).FirstOrDefault<CaptchaToken>();
                        this.Token = item.Token;
                        Global.SivasTokens.Remove(item);
                        this._mre.Set();
                        return;
                    }
                }
                else
                {
                    if (this._platform != "Hibbett")
                    {
                        if (this._platform != "OW")
                        {
                            if (this._platform != "Footaction")
                            {
                                if (this._platform != "Privacy")
                                {
                                    if ((this._platform != "Holypopstore") || !Global.HolypopTokens.Any<CaptchaToken>(x => (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now))))
                                    {
                                        goto Label_05F2;
                                    }
                                    CaptchaToken item = (from x in Global.HolypopTokens
                                        where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                                        select x).FirstOrDefault<CaptchaToken>();
                                    this.Token = item.Token;
                                    Global.HolypopTokens.Remove(item);
                                    this._mre.Set();
                                }
                                else
                                {
                                    if (!Global.PrivacyTokens.Any<CaptchaToken>(x => (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now))))
                                    {
                                        goto Label_05F2;
                                    }
                                    CaptchaToken item = (from x in Global.PrivacyTokens
                                        where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                                        select x).FirstOrDefault<CaptchaToken>();
                                    this.Token = item.Token;
                                    Global.PrivacyTokens.Remove(item);
                                    this._mre.Set();
                                }
                            }
                            else
                            {
                                if (!Global.FootactionTokens.Any<CaptchaToken>(x => (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now))))
                                {
                                    goto Label_05F2;
                                }
                                CaptchaToken item = (from x in Global.FootactionTokens
                                    where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                                    select x).FirstOrDefault<CaptchaToken>();
                                this.Token = item.Token;
                                Global.FootactionTokens.Remove(item);
                                this._mre.Set();
                            }
                        }
                        else
                        {
                            if (!Global.OWTokens.Any<CaptchaToken>(x => (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now))))
                            {
                                goto Label_05F2;
                            }
                            CaptchaToken item = (from x in Global.OWTokens
                                where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                                select x).FirstOrDefault<CaptchaToken>();
                            this.Token = item.Token;
                            Global.OWTokens.Remove(item);
                            this._mre.Set();
                        }
                        return;
                    }
                    if (Global.HibbettTokens.Any<CaptchaToken>(x => !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)))
                    {
                        CaptchaToken item = (from x in Global.HibbettTokens
                            where !string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now)
                            select x).FirstOrDefault<CaptchaToken>();
                        this.Token = item.Token;
                        Global.HibbettTokens.Remove(item);
                        this._mre.Set();
                        return;
                    }
                }
            }
            goto Label_05F2;
        }

        public string Token { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly CaptchaWaiter.<>c <>9;
            public static Func<CaptchaToken, bool> <>9__10_2;
            public static Func<CaptchaToken, bool> <>9__10_12;
            public static Func<CaptchaToken, bool> <>9__10_3;
            public static Func<CaptchaToken, bool> <>9__10_13;
            public static Func<CaptchaToken, bool> <>9__10_4;
            public static Func<CaptchaToken, bool> <>9__10_14;
            public static Func<CaptchaToken, bool> <>9__10_5;
            public static Func<CaptchaToken, bool> <>9__10_15;
            public static Func<CaptchaToken, bool> <>9__10_6;
            public static Func<CaptchaToken, bool> <>9__10_16;
            public static Func<CaptchaToken, bool> <>9__10_7;
            public static Func<CaptchaToken, bool> <>9__10_17;
            public static Func<CaptchaToken, bool> <>9__10_8;
            public static Func<CaptchaToken, bool> <>9__10_18;
            public static Func<CaptchaToken, bool> <>9__10_9;
            public static Func<CaptchaToken, bool> <>9__10_19;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new CaptchaWaiter.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Start>b__10_12(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_13(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_14(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_15(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_16(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_17(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_18(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_19(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_2(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_3(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_4(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_5(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_6(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_7(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_8(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));

            internal bool <Start>b__10_9(CaptchaToken x) => 
                (!string.IsNullOrEmpty(x.Token) && (x.Expires > DateTime.Now));
        }
    }
}

