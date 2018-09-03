namespace EveAIO.Tasks
{
    using System;
    using System.Runtime.CompilerServices;

    internal class ShopifyWebsiteInfo
    {
        public ShopifyWebsiteInfo()
        {
            Class7.RIuqtBYzWxthF();
            this.PaypalOnly = false;
            this.ValidGateway = true;
            this.OldMode = false;
        }

        internal string Website { get; set; }

        internal string SearchLink { get; set; }

        internal string HomeLink { get; set; }

        internal bool CheckoutLinks { get; set; }

        internal bool AtcLinks { get; set; }

        internal bool Login { get; set; }

        internal bool SmartCheckout { get; set; }

        internal string AdditionalInfo { get; set; }

        internal string ApiToken { get; set; }

        internal bool PaypalOnly { get; set; }

        internal bool ValidGateway { get; set; }

        internal bool Color { get; set; }

        internal bool OldMode { get; set; }
    }
}

