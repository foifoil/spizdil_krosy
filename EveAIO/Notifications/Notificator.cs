namespace EveAIO.Notifications
{
    using Discord;
    using Discord.Commands;
    using Discord.Webhook;
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Tweetinvi;
    using Tweetinvi.Models;
    using Tweetinvi.Parameters;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;

    internal class Notificator
    {
        private static Random _rnd;
        private TaskObject _task;
        private string _url;
        private string _productName;

        static Notificator()
        {
            Class7.RIuqtBYzWxthF();
            _rnd = new Random(DateTime.Now.Millisecond);
        }

        public Notificator()
        {
            Class7.RIuqtBYzWxthF();
        }

        public Notificator(TaskObject task, string url, string productName)
        {
            Class7.RIuqtBYzWxthF();
            this._task = task;
            this._url = url;
            this._productName = productName;
        }

        internal void Discord(NotificationType type, DiscordObject rec = null)
        {
            List<DiscordObject> list = null;
            if (rec == null)
            {
                if (type == NotificationType.Restock)
                {
                    list = (from x in Global.SETTINGS.DISCORD
                        where x.DiscordType == DiscordObject.DiscordMessageEnum.Restock
                        select x).ToList<DiscordObject>();
                }
                else if (type == NotificationType.Atc)
                {
                    list = (from x in Global.SETTINGS.DISCORD
                        where x.DiscordType == DiscordObject.DiscordMessageEnum.Atc
                        select x).ToList<DiscordObject>();
                }
                else if (type == NotificationType.Paypal)
                {
                    list = (from x in Global.SETTINGS.DISCORD
                        where x.DiscordType == DiscordObject.DiscordMessageEnum.PayPal
                        select x).ToList<DiscordObject>();
                }
                else
                {
                    list = (from x in Global.SETTINGS.DISCORD
                        where x.DiscordType == DiscordObject.DiscordMessageEnum.Checkout
                        select x).ToList<DiscordObject>();
                }
            }
            else
            {
                list = new List<DiscordObject> {
                    rec
                };
            }
            if (list != null)
            {
                foreach (DiscordObject obj2 in list)
                {
                    try
                    {
                        string webhookId = obj2.WebhookId;
                        string webhookToken = obj2.WebhookToken;
                        string message = obj2.Message;
                        if (rec != null)
                        {
                            message = "TESTING MESSAGE";
                        }
                        else
                        {
                            message = message.Replace("#product_url#", this._url).Replace("#product_title#", this._productName).Replace("#time#", DateTime.Now.ToLocalTime().ToString("dd/MM/yy hh:mm:sstt", CultureInfo.InvariantCulture)).Replace("#rnd#", "(" + _rnd.Next(0, 20).ToString() + ")").Replace("#paypal#", this._task.PaypalLink).Replace("#orderno#", this._task.OrderNo).Replace("#profile_name#", this._task.CheckoutProfile).Replace("#size#", this._task.PickedSize).Replace("#website#", this._task.HomeUrl).Replace("#task_name#", this._task.Name);
                        }
                        if (this._task != null)
                        {
                            Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                            if (this._task.Login)
                            {
                                message = message.Replace("#username#", this._task.Username).Replace("#password#", this._task.Password);
                            }
                        }
                        DiscordWebhookClient client = new DiscordWebhookClient(ulong.Parse(webhookId), webhookToken);
                        CommandServiceConfig config = new CommandServiceConfig {
                            LogLevel = LogSeverity.Info,
                            CaseSensitiveCommands = false
                        };
                        new CommandService(config);
                        if (message.Contains("#img#") && !string.IsNullOrEmpty(this._task.ImgUrl))
                        {
                            message = message.Replace("#img#", "");
                            EmbedBuilder builder = new EmbedBuilder {
                                ImageUrl = this._task.ImgUrl
                            };
                            Embed[] embedArray1 = new Embed[] { builder };
                            client.SendMessageAsync(message, false, embedArray1, null, null, null);
                        }
                        else
                        {
                            message = message.Replace("#img#", "");
                            client.SendMessageAsync(message, false, null, null, null, null);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (this._task != null)
                        {
                            Global.Logger.Error($"Task '{this._task.Name}': error sending discord message", exception);
                        }
                        else
                        {
                            Global.Logger.Error("Error sending discord test message", exception);
                        }
                    }
                }
            }
        }

        public void Notify(NotificationType type)
        {
            Task.Factory.StartNew(delegate {
                if (Global.SETTINGS.TwitterOn)
                {
                    this.Tweet(type, null);
                }
                if (Global.SETTINGS.SlackOn)
                {
                    this.Slack(type, null);
                }
                if (Global.SETTINGS.SmsOn)
                {
                    this.Twilio(type, null);
                }
                if (Global.SETTINGS.DiscordOn)
                {
                    this.Discord(type, null);
                }
            });
        }

        internal static void SendSuccess(TaskObject task, object speed)
        {
            try
            {
                string webhookToken = "XPBE_Hs40n4C0lj8Oja6-AlrfXFGyd5qPaA4QLO2ok2BOcWtH5yWXx0BDy8SnTfmMbic";
                string str2 = "<@&402948768560840704>";
                DiscordWebhookClient client = new DiscordWebhookClient(ulong.Parse("463722783751536670"), webhookToken);
                CommandServiceConfig config = new CommandServiceConfig {
                    LogLevel = LogSeverity.Info,
                    CaseSensitiveCommands = false
                };
                new CommandService(config);
                string str4 = "";
                string variant = "-";
                switch (task.TaskType)
                {
                    case TaskObject.TaskTypeEnum.directlink:
                        str4 = "Direct link";
                        variant = "-";
                        break;

                    case TaskObject.TaskTypeEnum.keywords:
                        str4 = "Keywords";
                        variant = "";
                        foreach (string str5 in task.Keywords)
                        {
                            variant = variant + "[" + str5.Trim() + "],";
                        }
                        break;

                    case TaskObject.TaskTypeEnum.variant:
                        str4 = "Variant";
                        variant = task.Variant;
                        break;
                }
                EmbedBuilder builder1 = new EmbedBuilder {
                    ThumbnailUrl = task.ImgUrl,
                    Title = "NEW CHECKOUT @Admin",
                    Color = new Color?(Color.Green)
                };
                List<EmbedFieldBuilder> list1 = new List<EmbedFieldBuilder>();
                EmbedFieldBuilder item = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Serial",
                    Value = Global.SERIAL
                };
                list1.Add(item);
                EmbedFieldBuilder builder3 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Name",
                    Value = Environment.UserName
                };
                list1.Add(builder3);
                EmbedFieldBuilder builder4 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Store",
                    Value = task.HomeUrl
                };
                list1.Add(builder4);
                EmbedFieldBuilder builder5 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Platform",
                    Value = task.Platform.ToString()
                };
                list1.Add(builder5);
                EmbedFieldBuilder builder6 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Link",
                    Value = task.SuccessUrl
                };
                list1.Add(builder6);
                EmbedFieldBuilder builder7 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Product",
                    Value = WebUtility.HtmlDecode(task.SuccessProductName)
                };
                list1.Add(builder7);
                EmbedFieldBuilder builder8 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Size",
                    Value = task.SuccessSize
                };
                list1.Add(builder8);
                EmbedFieldBuilder builder9 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Country",
                    Value = task.SuccessCountry
                };
                list1.Add(builder9);
                EmbedFieldBuilder builder10 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Speed",
                    Value = speed
                };
                list1.Add(builder10);
                EmbedFieldBuilder builder11 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Various",
                    Value = task.Various
                };
                list1.Add(builder11);
                EmbedFieldBuilder builder12 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "When",
                    Value = DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToString("HH:mm:ss")
                };
                list1.Add(builder12);
                EmbedFieldBuilder builder13 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Task type",
                    Value = str4
                };
                list1.Add(builder13);
                EmbedFieldBuilder builder14 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Keywords",
                    Value = variant
                };
                list1.Add(builder14);
                EmbedFieldBuilder builder15 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Price check",
                    Value = task.PriceCheck ? (task.MinimumPrice.ToString() + "-" + task.MaximumPrice.ToString()) : "-"
                };
                list1.Add(builder15);
                EmbedFieldBuilder builder16 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Quantity",
                    Value = task.Quantity
                };
                list1.Add(builder16);
                EmbedFieldBuilder builder17 = new EmbedFieldBuilder {
                    IsInline = true,
                    Name = "Version",
                    Value = Global.VERSION
                };
                list1.Add(builder17);
                builder1.Fields = list1;
                EmbedBuilder builder = builder1;
                Embed[] embedArray1 = new Embed[] { builder };
                client.SendMessageAsync(str2, false, embedArray1, null, null, null);
            }
            catch (Exception)
            {
            }
        }

        internal void Slack(NotificationType type, SlackObject rec = null)
        {
            List<SlackObject> list = null;
            if (rec != null)
            {
                list = new List<SlackObject> {
                    rec
                };
            }
            else if (type == NotificationType.Restock)
            {
                list = (from x in Global.SETTINGS.SLACK
                    where x.SlackType == SlackObject.SlackMessageEnum.Restock
                    select x).ToList<SlackObject>();
            }
            else if (type == NotificationType.Atc)
            {
                list = (from x in Global.SETTINGS.SLACK
                    where x.SlackType == SlackObject.SlackMessageEnum.Atc
                    select x).ToList<SlackObject>();
            }
            else if (type != NotificationType.Paypal)
            {
                list = (from x in Global.SETTINGS.SLACK
                    where x.SlackType == SlackObject.SlackMessageEnum.Checkout
                    select x).ToList<SlackObject>();
            }
            else
            {
                list = (from x in Global.SETTINGS.SLACK
                    where x.SlackType == SlackObject.SlackMessageEnum.PayPal
                    select x).ToList<SlackObject>();
            }
            if (list != null)
            {
                foreach (SlackObject obj2 in list)
                {
                    try
                    {
                        string username;
                        SlackClient client = new SlackClient(obj2.Hook);
                        string message = obj2.Message;
                        if (rec != null)
                        {
                            message = "TESTING MESSAGE";
                        }
                        else
                        {
                            message = message.Replace("#product_url#", this._url).Replace("#product_title#", this._productName).Replace("#time#", DateTime.Now.ToLocalTime().ToString("dd/MM/yy hh:mm:sstt", CultureInfo.InvariantCulture)).Replace("#rnd#", "(" + _rnd.Next(0, 20).ToString() + ")").Replace("#paypal#", this._task.PaypalLink).Replace("#orderno#", this._task.OrderNo).Replace("#profile_name#", this._task.CheckoutProfile).Replace("#size#", this._task.PickedSize).Replace("#website#", this._task.HomeUrl).Replace("#task_name#", this._task.Name);
                        }
                        if (this._task != null)
                        {
                            Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                            if (this._task.Login)
                            {
                                message = message.Replace("#username#", this._task.Username).Replace("#password#", this._task.Password);
                            }
                        }
                        string channel = obj2.Channel;
                        if (channel[0] != '#')
                        {
                            channel = "#" + channel;
                        }
                        if (message.Contains("#img#") && !string.IsNullOrEmpty(this._task.ImgUrl))
                        {
                            message = message.Replace("#img#", "");
                            username = obj2.Username;
                            EveAIO.Notifications.Attachment attachment = new EveAIO.Notifications.Attachment {
                                ImageUrl = this._task.ImgUrl,
                                Title = ""
                            };
                            client.PostMessage(message, username, channel, attachment);
                        }
                        else
                        {
                            username = obj2.Username;
                            client.PostMessage(message, username, channel, null);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (this._task != null)
                        {
                            Global.Logger.Error($"Task '{this._task.Name}': error sending slack message", exception);
                        }
                        else
                        {
                            Global.Logger.Error("Error sending slack test message", exception);
                        }
                    }
                }
            }
        }

        internal void Tweet(NotificationType type, TwitterObject rec = null)
        {
            List<TwitterObject> list = null;
            if (rec != null)
            {
                list = new List<TwitterObject> {
                    rec
                };
            }
            else if (type != NotificationType.Restock)
            {
                if (type != NotificationType.Atc)
                {
                    if (type != NotificationType.Paypal)
                    {
                        list = (from x in Global.SETTINGS.TWITTER
                            where x.TwitterType == TwitterObject.TwitterMessageEnum.Checkout
                            select x).ToList<TwitterObject>();
                    }
                    else
                    {
                        list = (from x in Global.SETTINGS.TWITTER
                            where x.TwitterType == TwitterObject.TwitterMessageEnum.PayPal
                            select x).ToList<TwitterObject>();
                    }
                }
                else
                {
                    list = (from x in Global.SETTINGS.TWITTER
                        where x.TwitterType == TwitterObject.TwitterMessageEnum.Atc
                        select x).ToList<TwitterObject>();
                }
            }
            else
            {
                list = (from x in Global.SETTINGS.TWITTER
                    where x.TwitterType == TwitterObject.TwitterMessageEnum.Restock
                    select x).ToList<TwitterObject>();
            }
            if (list != null)
            {
                foreach (TwitterObject obj2 in list)
                {
                    try
                    {
                        string consumerKeySecret = obj2.ConsumerKeySecret;
                        string accessToken = obj2.AccessToken;
                        string accessTokenSecret = obj2.AccessTokenSecret;
                        string msg = obj2.Message;
                        if (rec != null)
                        {
                            msg = "TESTING MESSAGE";
                        }
                        else
                        {
                            msg = msg.Replace("#product_url#", this._url);
                            msg = msg.Replace("#product_title#", this._productName);
                            msg = msg.Replace("#time#", DateTime.Now.ToLocalTime().ToString("dd/MM/yy hh:mm:sstt", CultureInfo.InvariantCulture));
                            msg = msg.Replace("#rnd#", "(" + _rnd.Next(0, 20).ToString() + ")");
                            msg = msg.Replace("#paypal#", this._task.PaypalLink);
                            msg = msg.Replace("#orderno#", this._task.OrderNo);
                            msg = msg.Replace("#profile_name#", this._task.CheckoutProfile);
                            msg = msg.Replace("#size#", this._task.PickedSize);
                            msg = msg.Replace("#website#", this._task.HomeUrl);
                            msg = msg.Replace("#task_name#", this._task.Name);
                        }
                        if (this._task != null)
                        {
                            Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                            if (this._task.Login)
                            {
                                msg = msg.Replace("#username#", this._task.Username);
                                msg = msg.Replace("#password#", this._task.Password);
                            }
                        }
                        TwitterCredentials credentials = new TwitterCredentials(obj2.ConsumerKey, consumerKeySecret, accessToken, accessTokenSecret);
                        Auth.SetCredentials(credentials);
                        if (msg.Contains("#img#") && !string.IsNullOrEmpty(this._task.ImgUrl))
                        {
                            msg = msg.Replace("#img#", "");
                            IMedia media = Upload.UploadImage(new WebClient().DownloadData(this._task.ImgUrl));
                            PublishTweetOptionalParameters publishTweetOptionalParameters = new PublishTweetOptionalParameters();
                            List<IMedia> list1 = new List<IMedia> {
                                media
                            };
                            publishTweetOptionalParameters.Medias = list1;
                            Tweetinvi.Tweet.PublishTweet(msg, publishTweetOptionalParameters);
                        }
                        else
                        {
                            Auth.ExecuteOperationWithCredentials<ITweet>(credentials, () => Tweetinvi.Tweet.PublishTweet(msg, null));
                        }
                    }
                    catch (Exception exception)
                    {
                        if (this._task != null)
                        {
                            Global.Logger.Error($"Task '{this._task.Name}': error sending tweet", exception);
                        }
                        else
                        {
                            Global.Logger.Error("Error sending tweet test message", exception);
                        }
                    }
                }
            }
        }

        internal void Twilio(NotificationType type, SmsObject rec = null)
        {
            List<SmsObject> list = null;
            if (rec != null)
            {
                list = new List<SmsObject> {
                    rec
                };
            }
            else if (type == NotificationType.Restock)
            {
                list = (from x in Global.SETTINGS.SMS
                    where x.SmsType == SmsObject.SmsMessageEnum.Restock
                    select x).ToList<SmsObject>();
            }
            else if (type != NotificationType.Atc)
            {
                if (type == NotificationType.Paypal)
                {
                    list = (from x in Global.SETTINGS.SMS
                        where x.SmsType == SmsObject.SmsMessageEnum.PayPal
                        select x).ToList<SmsObject>();
                }
                else
                {
                    list = (from x in Global.SETTINGS.SMS
                        where x.SmsType == SmsObject.SmsMessageEnum.Checkout
                        select x).ToList<SmsObject>();
                }
            }
            else
            {
                list = (from x in Global.SETTINGS.SMS
                    where x.SmsType == SmsObject.SmsMessageEnum.Atc
                    select x).ToList<SmsObject>();
            }
            if (list != null)
            {
                foreach (SmsObject obj2 in list)
                {
                    try
                    {
                        decimal? nullable;
                        bool? nullable2;
                        int? nullable3;
                        string message = obj2.Message;
                        if (rec == null)
                        {
                            message = message.Replace("#product_url#", this._url).Replace("#product_title#", this._productName).Replace("#time#", DateTime.Now.ToLocalTime().ToString("dd/MM/yy hh:mm:sstt", CultureInfo.InvariantCulture)).Replace("#rnd#", "(" + _rnd.Next(0, 20).ToString() + ")").Replace("#paypal#", this._task.PaypalLink).Replace("#orderno#", this._task.OrderNo).Replace("#profile_name#", this._task.CheckoutProfile).Replace("#size#", this._task.PickedSize).Replace("#website#", this._task.HomeUrl).Replace("#task_name#", this._task.Name);
                        }
                        else
                        {
                            message = "TESTING MESSAGE";
                        }
                        if (this._task != null)
                        {
                            Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                            if (this._task.Login)
                            {
                                message = message.Replace("#username#", this._task.Username).Replace("#password#", this._task.Password);
                            }
                        }
                        TwilioClient.Init(obj2.AccountSid, obj2.AuthToken);
                        List<Uri> mediaUrl = null;
                        if (message.Contains("#img#") && !string.IsNullOrEmpty(this._task.ImgUrl))
                        {
                            message = message.Replace("#img#", "");
                            mediaUrl = new List<Uri> {
                                new Uri(this._task.ImgUrl)
                            };
                        }
                        PhoneNumber to = new PhoneNumber(obj2.NumberTo);
                        if (mediaUrl != null)
                        {
                            nullable = null;
                            nullable2 = null;
                            nullable3 = null;
                            nullable2 = null;
                            nullable2 = null;
                            MessageResource.Create(to, null, new PhoneNumber(obj2.NumberFrom), null, message, mediaUrl, null, null, nullable, nullable2, nullable3, null, nullable2, null, null, null, nullable2, null);
                        }
                        else
                        {
                            nullable = null;
                            nullable2 = null;
                            nullable3 = null;
                            nullable2 = null;
                            nullable2 = null;
                            MessageResource.Create(to, null, new PhoneNumber(obj2.NumberFrom), null, message, null, null, null, nullable, nullable2, nullable3, null, nullable2, null, null, null, nullable2, null);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (this._task == null)
                        {
                            Global.Logger.Error("Error sending sms test message", exception);
                        }
                        else
                        {
                            Global.Logger.Error($"Task '{this._task.Name}': error sending sms message", exception);
                        }
                    }
                }
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Notificator.<>c <>9;
            public static Func<DiscordObject, bool> <>9__8_0;
            public static Func<DiscordObject, bool> <>9__8_1;
            public static Func<DiscordObject, bool> <>9__8_2;
            public static Func<DiscordObject, bool> <>9__8_3;
            public static Func<TwitterObject, bool> <>9__9_0;
            public static Func<TwitterObject, bool> <>9__9_1;
            public static Func<TwitterObject, bool> <>9__9_2;
            public static Func<TwitterObject, bool> <>9__9_3;
            public static Func<SlackObject, bool> <>9__10_0;
            public static Func<SlackObject, bool> <>9__10_1;
            public static Func<SlackObject, bool> <>9__10_2;
            public static Func<SlackObject, bool> <>9__10_3;
            public static Func<SmsObject, bool> <>9__11_0;
            public static Func<SmsObject, bool> <>9__11_1;
            public static Func<SmsObject, bool> <>9__11_2;
            public static Func<SmsObject, bool> <>9__11_3;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Notificator.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Discord>b__8_0(DiscordObject x) => 
                (x.DiscordType == DiscordObject.DiscordMessageEnum.Restock);

            internal bool <Discord>b__8_1(DiscordObject x) => 
                (x.DiscordType == DiscordObject.DiscordMessageEnum.Atc);

            internal bool <Discord>b__8_2(DiscordObject x) => 
                (x.DiscordType == DiscordObject.DiscordMessageEnum.PayPal);

            internal bool <Discord>b__8_3(DiscordObject x) => 
                (x.DiscordType == DiscordObject.DiscordMessageEnum.Checkout);

            internal bool <Slack>b__10_0(SlackObject x) => 
                (x.SlackType == SlackObject.SlackMessageEnum.Restock);

            internal bool <Slack>b__10_1(SlackObject x) => 
                (x.SlackType == SlackObject.SlackMessageEnum.Atc);

            internal bool <Slack>b__10_2(SlackObject x) => 
                (x.SlackType == SlackObject.SlackMessageEnum.PayPal);

            internal bool <Slack>b__10_3(SlackObject x) => 
                (x.SlackType == SlackObject.SlackMessageEnum.Checkout);

            internal bool <Tweet>b__9_0(TwitterObject x) => 
                (x.TwitterType == TwitterObject.TwitterMessageEnum.Restock);

            internal bool <Tweet>b__9_1(TwitterObject x) => 
                (x.TwitterType == TwitterObject.TwitterMessageEnum.Atc);

            internal bool <Tweet>b__9_2(TwitterObject x) => 
                (x.TwitterType == TwitterObject.TwitterMessageEnum.PayPal);

            internal bool <Tweet>b__9_3(TwitterObject x) => 
                (x.TwitterType == TwitterObject.TwitterMessageEnum.Checkout);

            internal bool <Twilio>b__11_0(SmsObject x) => 
                (x.SmsType == SmsObject.SmsMessageEnum.Restock);

            internal bool <Twilio>b__11_1(SmsObject x) => 
                (x.SmsType == SmsObject.SmsMessageEnum.Atc);

            internal bool <Twilio>b__11_2(SmsObject x) => 
                (x.SmsType == SmsObject.SmsMessageEnum.PayPal);

            internal bool <Twilio>b__11_3(SmsObject x) => 
                (x.SmsType == SmsObject.SmsMessageEnum.Checkout);
        }

        public enum NotificationType
        {
            Restock,
            Atc,
            Checkout,
            Paypal
        }
    }
}

