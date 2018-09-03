namespace EveAIO
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class CloudFlareSolver
    {
        private string rawPageContent;
        private int stopBreakingIndex;
        private string objectName;
        private string objectMemberName;
        private string objectCombinedName;
        private string jschl_vc;
        private string pass;
        private int challengeValue;
        private string lastOpeartion;
        private bool isLastOperationUnknown;
        private CookieContainer cookieContainer;
        private string targetUrl;
        private bool _useAgent;
        private string _referer;
        private bool _insecure;
        private object _proxy;

        public CloudFlareSolver(string url, CookieContainer cookies, bool useAgent = true, string referer = "", bool insecure = false, WebProxy proxy = null)
        {
            Class7.RIuqtBYzWxthF();
            this._proxy = proxy;
            this._useAgent = useAgent;
            this._referer = referer;
            this._insecure = insecure;
            this.cookieContainer = cookies;
            this.targetUrl = url;
            this.rawPageContent = this.GetChallengePage(url);
            this.stopBreakingIndex = this.rawPageContent.IndexOf("s,t,o,p,b,r,e,a,k,i,n,g,f,") + 0x1a;
            if (this.stopBreakingIndex < 0x1a)
            {
                throw new Exception("[!] Can not find magic \"s,t,o,p,b,r,e,a,k,i,n,g,f,\" keyword.");
            }
            Console.WriteLine("[+] Magic keyword \"s,t,o,p,b,r,e,a,k,i,n,g,f,\" found.");
            string expression = this.GetExpression(this.stopBreakingIndex);
            this.challengeValue = this.ParseObject(expression);
            Console.WriteLine("[+] Object parsed successfully, challengeValue = " + this.challengeValue);
            this.stopBreakingIndex = this.rawPageContent.IndexOf(this.objectCombinedName, this.stopBreakingIndex);
            if (this.stopBreakingIndex < 0)
            {
                throw new Exception("[!] Can not find expression.");
            }
            int startIndex = this.rawPageContent.IndexOf("name=\"jschl_vc\" value=\"", this.stopBreakingIndex) + 0x17;
            if (startIndex < 0x17)
            {
                throw new Exception("[!] Can not find jschl_vc value.");
            }
            int index = this.rawPageContent.IndexOf("\"", startIndex);
            if (index < 0)
            {
                throw new Exception("[!] Can not find jschl_vc value closing quote.");
            }
            this.jschl_vc = this.rawPageContent.Substring(startIndex, index - startIndex);
            startIndex = this.rawPageContent.IndexOf("name=\"pass\" value=\"", index) + 0x13;
            if (startIndex < 0x13)
            {
                throw new Exception("[!] Can not find pass value.");
            }
            index = this.rawPageContent.IndexOf("\"", startIndex);
            if (index < 0)
            {
                throw new Exception("[!] Can not find pass value closing quote.");
            }
            this.pass = this.rawPageContent.Substring(startIndex, index - startIndex);
            Console.WriteLine("[+] Please allow up to 5 seconds...");
            Thread.Sleep(0x1388);
        }

        public HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(this.targetUrl);
            if (this._useAgent)
            {
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            }
            request.CookieContainer = this.cookieContainer;
            if (!string.IsNullOrEmpty(this._referer))
            {
                request.Referer = this._referer;
            }
            if (this._proxy != null)
            {
                request.Proxy = (IWebProxy) this._proxy;
            }
            if (this._insecure)
            {
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
            }
            return request;
        }

        public HttpWebRequest CreateWebRequest(string url)
        {
            Uri uri = new Uri(url);
            if (new Uri(this.targetUrl).Host != uri.Host)
            {
                throw new Exception("[!] Request Hosts mismatch.");
            }
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            if (this._useAgent)
            {
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            }
            if (!string.IsNullOrEmpty(this._referer))
            {
                request.Referer = this._referer;
            }
            if (this._insecure)
            {
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
            }
            if (this._proxy != null)
            {
                request.Proxy = (IWebProxy) this._proxy;
            }
            request.KeepAlive = true;
            request.CookieContainer = this.cookieContainer;
            return request;
        }

        private string GetChallengePage(string url)
        {
            string str;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            if (this._proxy != null)
            {
                request.Proxy = (IWebProxy) this._proxy;
            }
            if (this._useAgent)
            {
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            }
            if (!string.IsNullOrEmpty(this._referer))
            {
                request.Referer = this._referer;
            }
            if (this._insecure)
            {
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
            }
            request.CookieContainer = this.cookieContainer;
            try
            {
                using (request.GetResponse())
                {
                }
                str = null;
            }
            catch (WebException exception1)
            {
                using (WebResponse response2 = exception1.Response)
                {
                    using (Stream stream = response2.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            str = reader.ReadToEnd();
                        }
                    }
                }
            }
            return str;
        }

        private string GetExpression(int expressionIndex)
        {
            this.stopBreakingIndex = this.rawPageContent.IndexOf(";", expressionIndex) + 1;
            return this.rawPageContent.Substring(expressionIndex, this.stopBreakingIndex - expressionIndex).Trim();
        }

        private int ParseExpression(string expression)
        {
            string str = expression.Replace(this.objectCombinedName, "").Replace("!+[]", "1").Replace("!![]", "1").Replace("+[]", "");
            char ch = str[0];
            ch = str[1];
            string introduced3 = ch.ToString();
            this.lastOpeartion = introduced3 + ch.ToString();
            if (!str.Contains("("))
            {
                return this.ParseUniqueExpression(str);
            }
            return this.ParseParenthesisExpression(str);
        }

        private int ParseObject(string objectExpression)
        {
            int index = objectExpression.IndexOf("=");
            if (index < 0)
            {
                throw new Exception("[!] Error: Can not find initialization operator '='.");
            }
            this.objectName = objectExpression.Substring(0, index);
            Console.WriteLine("[+] Object name: \"" + this.objectName + "\"");
            int startIndex = objectExpression.IndexOf("\"", index) + 1;
            if (startIndex < 1)
            {
                throw new Exception("[!] Error: Can not find opening quote '\"'.");
            }
            int num4 = objectExpression.IndexOf("\"", startIndex);
            if (num4 < 0)
            {
                throw new Exception("[!] Error: Can not find closing quote '\"'.");
            }
            this.objectMemberName = objectExpression.Substring(startIndex, num4 - startIndex);
            Console.WriteLine("[+] Object member name: \"" + this.objectMemberName + "\"");
            this.objectCombinedName = this.objectName + "." + this.objectMemberName;
            int num5 = objectExpression.IndexOf(":", num4) + 1;
            if (num5 < 1)
            {
                throw new Exception("[!] Error: Can not find colon opeartor ':'.");
            }
            int num6 = objectExpression.IndexOf("}", num5);
            if (num6 < 0)
            {
                throw new Exception("[!] Error: Can not find closing curly bracket '}'.");
            }
            return this.ParseExpression(objectExpression.Substring(num5, num6 - num5));
        }

        private int ParseParenthesisExpression(string expression)
        {
            string[] separator = new string[] { ")+(" };
            string[] textArray2 = expression.Split(separator, StringSplitOptions.None);
            int[] numArray = new int[textArray2.Length];
            int index = 0;
            string s = "";
            foreach (string str2 in textArray2)
            {
                for (int i = 0; i < str2.Length; i++)
                {
                    if (str2[i] == '1')
                    {
                        numArray[index]++;
                    }
                }
                s = s + numArray[index];
                index++;
            }
            return int.Parse(s);
        }

        private int ParseUniqueExpression(string expression)
        {
            int num = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '1')
                {
                    num++;
                }
            }
            return num;
        }

        private void PerformOpeartion(int value)
        {
            if (this.lastOpeartion == "-=")
            {
                this.challengeValue -= value;
            }
            else if (this.lastOpeartion == "+=")
            {
                this.challengeValue += value;
            }
            else if (this.lastOpeartion != "*=")
            {
                if (this.lastOpeartion != "/=")
                {
                    this.isLastOperationUnknown = true;
                    Console.WriteLine("[!] Unknown Last Operation");
                }
                else
                {
                    this.challengeValue /= value;
                }
            }
            else
            {
                this.challengeValue *= value;
            }
        }

        public void SendChallengeRequest()
        {
            Uri uri = new Uri(this.targetUrl);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create($"{uri.GetLeftPart(UriPartial.Authority)}/cdn-cgi/l/chk_jschl?jschl_vc={this.jschl_vc}&pass={this.pass}&jschl_answer={this.challengeValue}");
            if (this._useAgent)
            {
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            }
            if (this._proxy != null)
            {
                request.Proxy = (IWebProxy) this._proxy;
            }
            request.CookieContainer = this.cookieContainer;
            if (!string.IsNullOrEmpty(this._referer))
            {
                request.Referer = this._referer;
            }
            if (this._insecure)
            {
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
            }
            using (request.GetResponse())
            {
            }
        }

        public void Solve()
        {
            int length = new Uri(this.targetUrl).Host.Length;
            while (!this.isLastOperationUnknown)
            {
                string expression = this.GetExpression(this.stopBreakingIndex);
                Console.WriteLine("[+] Found expression, expression = \"" + expression + "\"");
                this.PerformOpeartion(this.ParseExpression(expression));
                Console.WriteLine("[+] Expression parsed successfully, challengeValue = \"" + this.challengeValue + "\"");
            }
            this.challengeValue += length;
            Console.WriteLine("[+] Challenge value is: challengeValue = \"" + this.challengeValue + "\"");
        }
    }
}

