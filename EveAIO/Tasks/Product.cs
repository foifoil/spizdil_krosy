namespace EveAIO.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Product
    {
        public List<KeyValuePair<string, string>> AvailableSizes;
        public List<Tuple<string, string, string, string, string>> AvailableSizesSupreme;
        public List<NordStromProduct> AvailableNordstromSizes;
        public List<BarneysProduct> AvailableBarneysSizes;
        public List<PumaProduct> AvailablePumaSizes;

        public Product()
        {
            Class7.RIuqtBYzWxthF();
            this.AvailableSizes = new List<KeyValuePair<string, string>>();
            this.AvailableSizesSupreme = new List<Tuple<string, string, string, string, string>>();
            this.AvailableNordstromSizes = new List<NordStromProduct>();
            this.AvailableBarneysSizes = new List<BarneysProduct>();
            this.AvailablePumaSizes = new List<PumaProduct>();
        }

        public string ProductTitle { get; set; }

        public string Link { get; set; }

        public string Price { get; set; }

        public string SuprimeMultiId { get; set; }

        public string Style { get; set; }

        public string Color { get; set; }

        public string SuprimetMultiStyle { get; set; }

        public DateTime? LastMod { get; set; }

        public string ExtendedTitle { get; set; }

        public KeyValuePair<string, string>? SuprimetMultiPickedSize { get; set; }
    }
}

