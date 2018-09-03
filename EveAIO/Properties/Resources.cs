namespace EveAIO.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0"), CompilerGenerated]
    internal class Resources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal Resources()
        {
            Class7.RIuqtBYzWxthF();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    resourceMan = new System.Resources.ResourceManager("EveAIO.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            set => 
                (resourceCulture = value);
        }

        internal static string adyen =>
            ResourceManager.GetString("adyen", resourceCulture);

        internal static UnmanagedMemoryStream bell =>
            ResourceManager.GetStream("bell", resourceCulture);

        internal static string braintree =>
            ResourceManager.GetString("braintree", resourceCulture);

        internal static string enc =>
            ResourceManager.GetString("enc", resourceCulture);

        internal static string eval =>
            ResourceManager.GetString("eval", resourceCulture);

        internal static string funko =>
            ResourceManager.GetString("funko", resourceCulture);

        internal static string ow =>
            ResourceManager.GetString("ow", resourceCulture);

        internal static string profileJson =>
            ResourceManager.GetString("profileJson", resourceCulture);

        internal static string profiles =>
            ResourceManager.GetString("profiles", resourceCulture);

        internal static string profilesCsv =>
            ResourceManager.GetString("profilesCsv", resourceCulture);

        internal static string sensor =>
            ResourceManager.GetString("sensor", resourceCulture);

        internal static string sivas =>
            ResourceManager.GetString("sivas", resourceCulture);
    }
}

