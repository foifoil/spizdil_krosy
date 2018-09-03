namespace EveAIO.Tasks
{
    using EO.WebBrowser;
    using EO.WebEngine;
    using EveAIO;
    using EveAIO.Properties;
    using System;
    using System.Runtime.InteropServices;

    internal class Sensor
    {
        private Engine _engine;
        private WebView _webView;
        private ThreadRunner _threadRunner;

        public Sensor()
        {
            Class7.RIuqtBYzWxthF();
            this._engine = Engine.Create(Guid.NewGuid().ToString());
            this._engine.Options.CachePath = Guid.NewGuid().ToString();
            this._engine.Start();
            this._threadRunner = new ThreadRunner(Guid.NewGuid().ToString(), this._engine);
            this._webView = this._threadRunner.CreateWebView();
        }

        public Tuple<string, string> EncryptBraintree(string cc, string cvv)
        {
            string outputCc = "";
            string outputCvv = "";
            string enc = Resources.braintree;
            this._webView.CustomUserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Mobile Safari/537.36";
            this._threadRunner.Send(delegate {
                this._webView.EvalScript(enc);
                this._webView.EvalScript("var brain = new Braintree.EncryptionClient('MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAoUID6FPckCjF4YHm8x7pDfoM0YeDx2ZPfdaVs7neGJWHnwYVZpj6X+hg5r8hqazHmFjonN3/SA0CahnN+MLPr4E6cAdUF1eTQnzVfqNVq3lKxYk0twT4Yv7X4oQ2EHYmisFm1A97ujgRwQ5xsbYRHgACe8US1X5S3c7pJDLcM1Ssjr4R3x3/F2e5T7+pWlG/J+tvLRyTvyPuv21KR/ZePHExO1jQ+HYf3gMh1eZfdj2jAPnfPbUSORbOKZtFms8B8ojuGPiSOr5hmBt7gy4UyJDR6tlxhpodqEOpqTv2WfZ/dRoNukETa65eZ0jnmQKnIdXRsNMFUqEF5A4cNVrLhHujwxsOXm5vIeOOWmG/HM8wnltETOF7Fdjs/cXVOicM3d09xL3ePCLe671YjSSb7T7oo/cCI5nK1xzPkQX9q+Yb3OvhoFlF3Mebf94L8te9GCUqt7Dk5Ukrnfn+G53CwH4jeuln2/8lVbE3XFVYT342IGOHpJ+XNbRd9CUTqIH8ESsK0DFeVR3qVCq4zJfQJ9UAKy6tWOHmijIPhpOijWNVgh+HTKUxoloWs3PSWUkOBJUZX4EYUThphCCf8Cedvf2nY0XNwWAmb4FDele8H4/J/NaNFYm/hWK7+Y+DIrL37rLrIb/hjHL1UqaK8osbXQkfohnFVw/pDCuXNemDvJkCAwEAAQ==');");
                this._webView.EvalScript("var cc = brain.encrypt('" + cc + "');");
                outputCc = this._webView.QueueScriptCall("cc").Output();
                this._webView.EvalScript("var cvv = brain.encrypt('" + cvv + "');");
                outputCvv = this._webView.QueueScriptCall("cvv").Output();
            });
            return new Tuple<string, string>(outputCc, outputCvv);
        }

        public string EncryptSivas(string key, string adyen, string cc, string cvv, string name, string expMonth, string expYear)
        {
            string response = "";
            string sivas = Resources.sivas.Replace("{0}", "\"" + key + "\"");
            this._webView.CustomUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
            this._threadRunner.Send(delegate {
                this._webView.EvalScript(adyen);
                this._webView.EvalScript(sivas);
                this._webView.EvalScript("var postData = getEncryptedFormData(\"" + cc + "\", \"" + cvv + "\", \"" + name + "\", \"" + expMonth + "\", \"" + expYear + "\", generationTime);");
                response = this._webView.QueueScriptCall("JSON.stringify(postData)").Output();
            });
            return response;
        }

        public string GetData(string website)
        {
            string sensor = "";
            string script = Resources.sensor.Replace(@"\r\n", "");
            this._webView.CustomUserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Mobile Safari/537.36";
            this._threadRunner.Send(delegate {
                this._webView.LoadUrlAndWait(website + "/robots.txt");
                this._webView.QueueScriptCall(script).Output();
                sensor = this._webView.QueueScriptCall("cf['sensor_data']").Output();
            });
            return sensor;
        }

        public string GetData(string html, string link, string async, bool mobile = false)
        {
            string sensor = "";
            if (mobile)
            {
                this._webView.CustomUserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.162 Mobile Safari/537.36";
            }
            else
            {
                this._webView.CustomUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";
            }
            this._threadRunner.Send(delegate {
                this._webView.LoadHtmlAndWait(html, link);
                this._webView.QueueScriptCall(async);
                this._webView.EvalScript("cf['bpd']();");
                sensor = this._webView.QueueScriptCall("cf['sensor_data']").Output();
            });
            return sensor;
        }

        public string ProcessNordStrom(string modulus, string exponent, string keyId, string payPageMerchantTxnId, string payPageId, string reportGroup, string cc, string cvv)
        {
            string output = "";
            string enc = Resources.enc.Replace("{0}", modulus).Replace("{1}", exponent).Replace("{2}", keyId);
            string eval = Resources.eval.Replace("{0}", payPageMerchantTxnId).Replace("{1}", payPageId).Replace("{2}", reportGroup).Replace("{4}", cc).Replace("{3}", cvv);
            this._webView.CustomUserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Mobile Safari/537.36";
            this._threadRunner.Send(delegate {
                this._webView.LoadUrlAndWait("https://shop.nordstrom.com/robots.txt");
                this._webView.EvalScript(enc);
                this._webView.EvalScript(eval);
                output = this._webView.QueueScriptCall("output").Output();
            });
            return output;
        }
    }
}

