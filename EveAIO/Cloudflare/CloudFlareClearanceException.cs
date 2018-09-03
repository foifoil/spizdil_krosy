namespace EveAIO.Cloudflare
{
    using System;
    using System.Net.Http;
    using System.Runtime.CompilerServices;

    public class CloudFlareClearanceException : HttpRequestException
    {
        public CloudFlareClearanceException(int attempts) : this(attempts, $"Clearance failed after {attempts} attempt(s).")
        {
            Class7.RIuqtBYzWxthF();
        }

        public CloudFlareClearanceException(int attempts, string message) : base(message)
        {
            Class7.RIuqtBYzWxthF();
            this.<Attempts>k__BackingField = attempts;
        }

        public CloudFlareClearanceException(int attempts, string message, Exception inner) : base(message, inner)
        {
            Class7.RIuqtBYzWxthF();
            this.<Attempts>k__BackingField = attempts;
        }

        public int Attempts =>
            this.<Attempts>k__BackingField;
    }
}

