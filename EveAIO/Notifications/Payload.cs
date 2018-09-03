namespace EveAIO.Notifications
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class Payload
    {
        public Payload()
        {
            Class7.RIuqtBYzWxthF();
        }

        [Newtonsoft.Json.JsonProperty("channel")]
        public string Channel { get; set; }

        [Newtonsoft.Json.JsonProperty("username")]
        public string Username { get; set; }

        [Newtonsoft.Json.JsonProperty("text")]
        public string Text { get; set; }

        [Newtonsoft.Json.JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }
    }
}

