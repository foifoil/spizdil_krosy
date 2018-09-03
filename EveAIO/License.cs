namespace EveAIO
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    internal static class License
    {
        private static string _licenseUrl;

        static License()
        {
            Class7.RIuqtBYzWxthF();
            _licenseUrl = "https://everoboticslm.herokuapp.com/lm";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(<>c.<>9.<.cctor>b__2_0);
        }

        public static Result Activate(string key) => 
            Result.success;

        public static Result Check(string key) => 
            Result.success;

        public static byte[] Down(string key)
        {
            Global.AsmLoaded = true;
            return System.IO.File.ReadAllBytes("EO.ScreaBase.dll");
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly License.<>c <>9;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new License.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <.cctor>b__2_0(object <p0>, X509Certificate <p1>, X509Chain <p2>, SslPolicyErrors <p3>) => 
                true;
        }

        [CompilerGenerated]
        private static class <>o__3
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, bool>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, bool>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool>> <>p__16;
        }

        [CompilerGenerated]
        private static class <>o__4
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, bool>> <>p__5;
            public static CallSite<Func<CallSite, object, string, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, object>> <>p__8;
            public static CallSite<Func<CallSite, object, object>> <>p__9;
            public static CallSite<Func<CallSite, object, string, object>> <>p__10;
            public static CallSite<Func<CallSite, object, bool>> <>p__11;
            public static CallSite<Func<CallSite, object, string, object>> <>p__12;
            public static CallSite<Func<CallSite, object, object>> <>p__13;
            public static CallSite<Func<CallSite, object, object>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, bool>> <>p__16;
        }

        public enum Result
        {
            success,
            failed,
            error,
            alreadyActivated,
            notActivated,
            invalidKey
        }
    }
}

