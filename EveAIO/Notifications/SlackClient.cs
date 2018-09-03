namespace EveAIO.Notifications
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class SlackClient
    {
        private readonly Uri _uri;
        private readonly UTF8Encoding _encoding;

        public SlackClient(string urlWithAccessToken)
        {
            Class7.RIuqtBYzWxthF();
            this._encoding = new UTF8Encoding();
            this._uri = new Uri(urlWithAccessToken);
        }

        public void PostMessage(Payload payload)
        {
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection {
                    ["payload"] = str
                };
                byte[] bytes = client.UploadValues(this._uri, "POST", data);
                this._encoding.GetString(bytes);
            }
        }

        public void PostMessage(string text, string username = null, string channel = null, Attachment attachment = null)
        {
            Payload payload1 = new Payload {
                Channel = channel,
                Username = username,
                Text = text
            };
            List<Attachment> list1 = new List<Attachment> {
                attachment
            };
            payload1.Attachments = list1;
            Payload payload = payload1;
            this.PostMessage(payload);
        }
    }
}

