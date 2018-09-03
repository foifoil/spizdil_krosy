namespace EveAIO.Tasks.Dto
{
    using System;
    using System.Runtime.CompilerServices;

    internal class ShopifyInit
    {
        public ShopifyInit()
        {
            Class7.RIuqtBYzWxthF();
            this.shipping_address = new ShopifyAddress();
            this.billing_address = new ShopifyAddress();
        }

        public string email { get; set; }

        public ShopifyAddress shipping_address { get; set; }

        public ShopifyAddress billing_address { get; set; }
    }
}

