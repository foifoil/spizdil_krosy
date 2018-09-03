namespace EveAIO.Captcha
{
    using EveAIO;
    using System;
    using System.Reflection;

    internal static class CaptchaSolver
    {
        public static double AntiCaptchaGetBalance()
        {
            double num = -1.0;
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha.AntiCaptcha").GetMethod("GetBalance");
                object[] parameters = new object[] { Global.SETTINGS.AntiCaptchaApiKey };
                return double.Parse(method.Invoke(method, parameters).ToString());
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error checking AntiCaptcha balance - " + ((exception.InnerException == null) ? exception.Message : exception.InnerException.Message));
                Global.Logger.Error("Error checking AntiCaptcha balance", exception);
                return num;
            }
        }

        public static string AntiCaptchaSolve(object googleKey, object pageUrl, object proxy)
        {
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha.AntiCaptcha").GetMethod("Solve");
                object[] parameters = new object[] { googleKey, pageUrl, proxy, Global.SETTINGS.AntiCaptchaApiKey };
                return method.Invoke(method, parameters).ToString();
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error getting AntiCaptcha token - " + ((exception.InnerException == null) ? exception.Message : exception.InnerException.Message));
                Global.Logger.Error("Error getting AntiCaptcha token", exception);
                return "";
            }
        }

        public static double DisolveGetBalance()
        {
            double num = -1.0;
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha.Disolve").GetMethod("GetBalance");
                object[] parameters = new object[] { Global.SETTINGS.DisolveApiKey, Global.SETTINGS.DisolveIp };
                return double.Parse(method.Invoke(method, parameters).ToString());
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error checking Disolve balance");
                Global.Logger.Error("Error checking Disolve balance", exception);
                return num;
            }
        }

        public static string DisolveSolve(object googleKey, object pageUrl, object proxy)
        {
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha.Disolve").GetMethod("SolveRecaptchaV3");
                object[] parameters = new object[] { Global.SETTINGS.DisolveApiKey, googleKey, pageUrl, proxy, Global.SETTINGS.DisolveIp };
                object obj2 = method.Invoke(method, parameters);
                if (obj2.ToString().Contains("err|"))
                {
                    char[] separator = new char[] { '|' };
                    throw new Exception(obj2.ToString().Split(separator)[1]);
                }
                return obj2.ToString();
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error getting Disolve token - " + exception.Message);
                Global.Logger.Error("Error getting Disolve token", exception);
                return "";
            }
        }

        public static double ImageTypersGetBalance()
        {
            double num = -1.0;
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha.ImageTypers").GetMethod("GetBalance");
                object[] parameters = new object[] { Global.SETTINGS.ImageTypersUsername, Global.SETTINGS.ImageTypersPassword };
                return double.Parse(method.Invoke(method, parameters).ToString());
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error checking ImageTypers balance - " + ((exception.InnerException == null) ? exception.Message : exception.InnerException.Message));
                Global.Logger.Error("Error checking ImageTypers balance", exception);
                return num;
            }
        }

        public static string ImageTypersSolve(object googleKey, object pageUrl, object proxy)
        {
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha.ImageTypers").GetMethod("Solve");
                object[] parameters = new object[] { googleKey, pageUrl, proxy, Global.SETTINGS.ImageTypersUsername, Global.SETTINGS.ImageTypersPassword };
                return method.Invoke(method, parameters).ToString();
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error getting ImageTypers token - " + ((exception.InnerException == null) ? exception.Message : exception.InnerException.Message));
                Global.Logger.Error("Error getting ImageTypers token", exception);
                return "";
            }
        }

        public static double TwoCaptchaGetBalance()
        {
            double num = -1.0;
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha._2Captcha").GetMethod("GetBalance");
                object[] parameters = new object[] { Global.SETTINGS.TwoCaptchaApiKey };
                return double.Parse(method.Invoke(method, parameters).ToString());
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error checking 2Captcha balance");
                Global.Logger.Error("Error checking 2Captcha balance", exception);
                return num;
            }
        }

        public static string TwoCaptchaSolve(object googleKey, object pageUrl, object proxy)
        {
            try
            {
                MethodInfo method = Global.ASM.GetType("SvcHost.Captcha._2Captcha").GetMethod("SolveRecaptchaV3");
                object[] parameters = new object[] { Global.SETTINGS.TwoCaptchaApiKey, googleKey, pageUrl, proxy };
                object obj2 = method.Invoke(method, parameters);
                if (obj2.ToString().Contains("err|"))
                {
                    char[] separator = new char[] { '|' };
                    throw new Exception(obj2.ToString().Split(separator)[1]);
                }
                return obj2.ToString();
            }
            catch (Exception exception)
            {
                Helpers.WriteLog("Error getting 2Captcha token - " + exception.Message);
                Global.Logger.Error("Error getting 2Captcha token", exception);
                return "";
            }
        }

        public enum CaptchaService
        {
            TwoCaptcha,
            AntiCaptcha,
            ImageTypers,
            Disolve,
            Manual
        }
    }
}

