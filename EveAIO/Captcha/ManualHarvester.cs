namespace EveAIO.Captcha
{
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;

    internal class ManualHarvester
    {
        private string _website;
        public TaskObject.PlatformEnum Platform;
        private string _arguments;
        private Harvester _harvester;

        public ManualHarvester(TaskObject.PlatformEnum platform)
        {
            Class7.RIuqtBYzWxthF();
            this.Platform = platform;
        }

        public string Listen(HttpListenerRequest request, string website, string siteKey)
        {
            string[] textArray1;
            try
            {
                using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string str = reader.ReadToEnd().Replace("token=", string.Empty);
                    if (!str.Equals(string.Empty))
                    {
                        try
                        {
                            CaptchaToken item = new CaptchaToken {
                                Id = Guid.NewGuid().ToString(),
                                Created = DateTime.Now,
                                CaptchaType = CaptchaSolver.CaptchaService.Manual,
                                Website = this._website
                            };
                            TaskObject.PlatformEnum platform = this.Platform;
                            if (platform == TaskObject.PlatformEnum.shopify)
                            {
                                item.Platform = "Shopify";
                            }
                            else if (platform == TaskObject.PlatformEnum.supreme)
                            {
                                item.Platform = "Supreme";
                            }
                            else if (platform == TaskObject.PlatformEnum.sneakersnstuff)
                            {
                                item.Platform = "Sneakersnstuff";
                            }
                            item.Token = str;
                            item.Timestamp = DateTime.Now;
                            item.Expires = item.Timestamp.AddMinutes(2.0);
                            TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
                            item.SolveTime = (span.Minutes * 60) + span.Seconds;
                            if (!string.IsNullOrEmpty(item.Token))
                            {
                                switch (this.Platform)
                                {
                                    case TaskObject.PlatformEnum.shopify:
                                        Global.ShopifyTokens.Add(item);
                                        goto Label_0172;

                                    case TaskObject.PlatformEnum.supreme:
                                        Global.SupremeTokens.Add(item);
                                        goto Label_0172;

                                    case TaskObject.PlatformEnum.sneakersnstuff:
                                        Global.SnsTokens.Add(item);
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        Label_0172:
            textArray1 = new string[] { "<html>\r\n<head>\r\n<style>\r\nform {\r\n  text-align: center;\r\n}\r\nbody {\r\n  text-align: center;\r\n  background-color:#E1E5EB;\r\n  \r\n}\r\n\r\nh1 {\r\n  text-align: center; color: red;\r\n}\r\nh3 {\r\n  text-align: center;\r\n}\r\ndiv-captcha {\r\n      text-align: center;\r\n}\r\n    .g-recaptcha {\r\n        display: inline-block;\r\n    }\r\n</style>\r\n<h2>Harvesting for: ", website, "</h2><br> \r\n\r\n<br>\r\nAfter you retrieved / set a new API-Key in the bot, please RELOAD this page.</h3>\r\n<br><br>\r\n<meta name=\"referrer\" content=\"never\"> <script type='text/javascript' src='https://www.google.com/recaptcha/api.js?onload=recaptchaCallback&render=explicit&hl=en'></script><script>var recaptchaCallback = function() { grecaptcha.render('g-recaptcha', {sitekey: \"", siteKey, "\",size: (window.innerWidth > 320) ? 'normal' : 'compact',callback: 'onCaptchaSuccess',});}; var onCaptchaSuccess = function() {var event;try {event = new Event('captchaSuccess', { bubbles: true, cancelable: true});} catch (e) {event = document.createEvent('Event');event.initEvent('captchaSuccess', true, true);}window.dispatchEvent(event);}</script></head> <body oncontextmenu=\"return false\"><div id=\"div-captcha\"><div id=\"g-recaptcha\"</div></div> <br>\r\n<script>\r\n\r\nwindow.setInterval(function(){\r\n    var token = document.getElementById('g-recaptcha-response').value;\r\n    document.getElementById('g-recaptcha-response').value = '';\r\n    if(token != '')\r\n    {\r\n    var http = new XMLHttpRequest();\r\n    var params = \"token=\" +token;\r\n    http.open(\"POST\", \"/destroyer\", true);\r\n   http.setRequestHeader(\"Last-Modified\", \"", DateTime.Now.AddHours(-2.0).ToString("r"), "\");\r\n http.setRequestHeader(\"Date\", \"", DateTime.Now.AddHours(-2.0).ToString("r"), "\");\r\n   http.setRequestHeader(\"Content-type\", \"application/x-www-form-urlencoded\");\r\n    http.setRequestHeader(\"Content-length\", params.length)\r\n    http.send(params);\r\n    location.reload();\r\n}\r\n}, 500);\r\n</script>\r\n</body></html>" };
            return string.Concat(textArray1);
        }

        public void Start()
        {
            string item = "";
            string str2 = "";
            string siteKey = "";
            string website = "";
            switch (this.Platform)
            {
                case TaskObject.PlatformEnum.shopify:
                    item = "http://eveaio.shopify.com:52698/";
                    str2 = "http://eveaio.shopify.com:62974/";
                    siteKey = "6LeoeSkTAAAAAA9rkZs5oS82l69OEYjKRZAiKdaF";
                    website = "http://www.shopify.com";
                    break;

                case TaskObject.PlatformEnum.supreme:
                    item = "http://eveaio.supremenewyork.com:52698/";
                    str2 = "http://eveaio.supremenewyork.com:62974/";
                    siteKey = Global.SETTINGS.SupremeCaptchaKey;
                    website = "http://www.supremenewyork.com";
                    break;

                case TaskObject.PlatformEnum.privacy:
                    item = "http://eveaio.privacy.com/auth/local:52698/";
                    str2 = "http://eveaio.privacy.com/auth/local:62974/";
                    break;

                case TaskObject.PlatformEnum.sneakersnstuff:
                    item = "http://eveaio.sneakersnstuff.com:52698/";
                    str2 = "http://eveaio.sneakersnstuff.com:62974/";
                    siteKey = Global.SETTINGS.SnsCaptchaKey;
                    website = "http://www.sneakersnstuff.com";
                    break;
            }
            if (Global.RUNNING_SERVERS.Contains(item))
            {
                Process.Start("chrome.exe", item);
            }
            else if (Global.RUNNING_SERVERS.Contains(str2))
            {
                Process.Start("chrome.exe", str2);
            }
            else
            {
                this._website = website;
                this._arguments = item;
                try
                {
                    string[] strArray = new string[] { item };
                    this._harvester = new Harvester(new Func<HttpListenerRequest, string, string, string>(this.Listen), website, siteKey, strArray);
                    this._harvester.Start();
                    Global.RUNNING_SERVERS.Add(item);
                }
                catch (Exception)
                {
                    try
                    {
                        string[] strArray2 = new string[] { str2 };
                        this._harvester = new Harvester(new Func<HttpListenerRequest, string, string, string>(this.Listen), website, siteKey, strArray2);
                        this._harvester.Start();
                        this._arguments = str2;
                        Global.RUNNING_SERVERS.Add(str2);
                    }
                    catch (Exception exception)
                    {
                        Global.Logger.Error("Error while starting the harvester: {0}", exception);
                        return;
                    }
                }
                try
                {
                    Process.Start("chrome.exe", this._arguments);
                }
                catch (Exception exception2)
                {
                    Global.Logger.Error("Error while starting the manual captcha harvester", exception2);
                }
            }
        }

        public void Stop()
        {
            this._harvester.Stop();
        }

        public string Token { get; set; }
    }
}

