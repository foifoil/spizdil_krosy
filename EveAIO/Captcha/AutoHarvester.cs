namespace EveAIO.Captcha
{
    using EveAIO;
    using System;

    internal class AutoHarvester
    {
        public AutoHarvester(string website, string key, string platform, CaptchaSolver.CaptchaService service)
        {
            Class7.RIuqtBYzWxthF();
            CaptchaToken item = new CaptchaToken {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CaptchaType = service,
                Website = website,
                Platform = platform
            };
            switch (service)
            {
                case CaptchaSolver.CaptchaService.TwoCaptcha:
                    item.Token = CaptchaSolver.TwoCaptchaSolve(key, website, "");
                    break;

                case CaptchaSolver.CaptchaService.AntiCaptcha:
                    item.Token = CaptchaSolver.AntiCaptchaSolve(key, website, "");
                    break;

                case CaptchaSolver.CaptchaService.ImageTypers:
                    item.Token = CaptchaSolver.ImageTypersSolve(key, website, "");
                    break;

                case CaptchaSolver.CaptchaService.Disolve:
                    item.Token = CaptchaSolver.DisolveSolve(key, website, "");
                    break;
            }
            item.Timestamp = DateTime.Now;
            TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
            item.SolveTime = (span.Minutes * 60) + span.Seconds;
            item.Expires = item.Timestamp.AddMinutes(2.0);
            if (!string.IsNullOrEmpty(item.Token))
            {
                if (item.Platform == "Shopify")
                {
                    Global.ShopifyTokens.Add(item);
                }
                else if (item.Platform == "Supreme")
                {
                    Global.SupremeTokens.Add(item);
                }
                else if (item.Platform == "Sneakernstuff")
                {
                    Global.SnsTokens.Add(item);
                }
            }
        }
    }
}

