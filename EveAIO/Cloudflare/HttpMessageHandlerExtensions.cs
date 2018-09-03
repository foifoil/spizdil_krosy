namespace EveAIO.Cloudflare
{
    using System;
    using System.Net.Http;
    using System.Runtime.CompilerServices;

    internal static class HttpMessageHandlerExtensions
    {
        public static HttpMessageHandler GetMostInnerHandler(this object self)
        {
            if (self is DelegatingHandler handler)
            {
                return handler.InnerHandler.GetMostInnerHandler();
            }
            return (HttpMessageHandler) self;
        }
    }
}

