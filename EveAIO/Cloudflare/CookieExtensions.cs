namespace EveAIO.Cloudflare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;

    internal static class CookieExtensions
    {
        public static IEnumerable<Cookie> GetCookiesByName(this CookieContainer container, Uri uri, params string[] names) => 
            (from c in container.GetCookies(uri).Cast<Cookie>()
                where names.Contains<string>(c.Name)
                select c).ToList<Cookie>();

        public static string ToHeaderValue(this Cookie cookie) => 
            $"{cookie.Name}={cookie.Value}";
    }
}

