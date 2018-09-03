namespace EveAIO.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class MeshCustomer
    {
        public MeshCustomer()
        {
            Class7.RIuqtBYzWxthF();
            this.addresses = new List<MeshAddress>();
        }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public bool enrolledForEmailMarketing { get; set; }

        public bool isGuest { get; set; }

        public List<MeshAddress> addresses { get; set; }
    }
}

