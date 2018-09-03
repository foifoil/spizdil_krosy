namespace EveAIO.Tasks.Platforms
{
    using System;

    internal interface IPlatform
    {
        bool Atc();
        bool Checkout();
        bool DirectLink(string link);
        bool Login();
        bool Search();
        void SetClient();
    }
}

