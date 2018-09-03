using System;
using System.Reflection;
using System.Runtime.CompilerServices;

internal class Class6
{
    internal static Module module_0;

    static Class6()
    {
        Class7.RIuqtBYzWxthF();
        module_0 = typeof(Class6).Assembly.ManifestModule;
    }

    public Class6()
    {
        Class7.RIuqtBYzWxthF();
    }

    internal static void eQCqtBYYxtgcQ(int typemdt)
    {
        Type type = module_0.ResolveType(0x2000000 + typemdt);
        foreach (FieldInfo info in type.GetFields())
        {
            MethodInfo method = (MethodInfo) module_0.ResolveMethod(info.MetadataToken + 0x6000000);
            info.SetValue(null, (MulticastDelegate) Delegate.CreateDelegate(type, method));
        }
    }

    internal delegate void Delegate0(object o);
}

