namespace EveAIO.Tasks
{
    using EveAIO.Pocos;
    using System;
    using System.Collections.Generic;

    internal static class ShopifyCommon
    {
        internal static void UpdatePredefinedSizing(Product runner, TaskObject task)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            string shopifyWebsite = task.ShopifyWebsite;
            uint num = <PrivateImplementationDetails>.ComputeStringHash(shopifyWebsite);
            if (num <= 0x8fb99128)
            {
                if (num > 0x5134779d)
                {
                    if (num > 0x783ca8cb)
                    {
                        if (num <= 0x85732999)
                        {
                            switch (num)
                            {
                                case 0x81836124:
                                {
                                    bool flag1 = shopifyWebsite == "eflashsg";
                                    return;
                                }
                                case 0x819e199a:
                                    if (shopifyWebsite == "rsvpgallery")
                                    {
                                        foreach (KeyValuePair<string, string> pair23 in runner.Product.AvailableSizes)
                                        {
                                            string key = pair23.Key;
                                            while (key.Contains("/"))
                                            {
                                                if (key.Contains("/"))
                                                {
                                                    key = key.Substring(key.IndexOf("/") + 1).Trim();
                                                }
                                            }
                                            list.Add(new KeyValuePair<string, string>(key, pair23.Value));
                                        }
                                        runner.Product.AvailableSizes = list;
                                    }
                                    return;

                                case 0x85732999:
                                {
                                    bool flag2 = shopifyWebsite == "trophyroomstore";
                                    return;
                                }
                                case 0x7a949f40:
                                    if (shopifyWebsite == "eflashjp")
                                    {
                                        foreach (KeyValuePair<string, string> pair3 in runner.Product.AvailableSizes)
                                        {
                                            string key = pair3.Key;
                                            if (key.Contains(" "))
                                            {
                                                key = key.Substring(key.LastIndexOf(" ") + 1).Trim();
                                            }
                                            list.Add(new KeyValuePair<string, string>(key, pair3.Value));
                                        }
                                        runner.Product.AvailableSizes = list;
                                    }
                                    return;

                                case 0x7aade1ed:
                                    if (shopifyWebsite != "pilgrim")
                                    {
                                        return;
                                    }
                                    foreach (KeyValuePair<string, string> pair37 in runner.Product.AvailableSizes)
                                    {
                                        string key = pair37.Key;
                                        if (key.Contains("/"))
                                        {
                                            key = key.Substring(key.IndexOf("/") + 1).Trim();
                                        }
                                        list.Add(new KeyValuePair<string, string>(key, pair37.Value));
                                    }
                                    runner.Product.AvailableSizes = list;
                                    break;
                            }
                        }
                        else if (num <= 0x8b55eb57)
                        {
                            switch (num)
                            {
                                case 0x86c249a6:
                                {
                                    bool flag3 = shopifyWebsite == "cncpts";
                                    return;
                                }
                                case 0x8b55eb57:
                                {
                                    bool flag4 = shopifyWebsite == "sneakerpolitics";
                                    return;
                                }
                            }
                        }
                        else if (num == 0x8c389037)
                        {
                            if (shopifyWebsite == "bbbranded")
                            {
                                foreach (KeyValuePair<string, string> pair12 in runner.Product.AvailableSizes)
                                {
                                    string key = pair12.Key;
                                    if (key.Contains("/"))
                                    {
                                        key = pair12.Key.Substring(pair12.Key.IndexOf("/") + 1).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair12.Value));
                                }
                                runner.Product.AvailableSizes = list;
                            }
                        }
                        else if (num == 0x8f74e9e0)
                        {
                            bool flag5 = shopifyWebsite == "noirfoncees";
                        }
                        else if ((num == 0x8fb99128) && (shopifyWebsite == "eflasheu"))
                        {
                            foreach (KeyValuePair<string, string> pair39 in runner.Product.AvailableSizes)
                            {
                                string key = pair39.Key;
                                if (key.Contains(" "))
                                {
                                    key = key.Substring(key.LastIndexOf(" ") + 1).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair39.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num > 0x6d6601da)
                    {
                        switch (num)
                        {
                            case 0x73761b67:
                            {
                                bool flag7 = shopifyWebsite == "a-ma-maniere";
                                return;
                            }
                            case 0x756a879f:
                            {
                                bool flag8 = shopifyWebsite == "abpstore";
                                return;
                            }
                            case 0x783ca8cb:
                            {
                                bool flag9 = shopifyWebsite == "shoegallerymiami";
                                return;
                            }
                            case 0x6d970277:
                                if (shopifyWebsite == "notre")
                                {
                                    foreach (KeyValuePair<string, string> pair18 in runner.Product.AvailableSizes)
                                    {
                                        string key = pair18.Key;
                                        if (key.Contains("/"))
                                        {
                                            key = pair18.Key.Substring(0, pair18.Key.IndexOf("/")).Trim();
                                        }
                                        list.Add(new KeyValuePair<string, string>(key, pair18.Value));
                                    }
                                    runner.Product.AvailableSizes = list;
                                }
                                return;

                            case 0x6f7f4ed7:
                            {
                                bool flag6 = shopifyWebsite == "packer";
                                return;
                            }
                        }
                    }
                    else if (num <= 0x5991c81a)
                    {
                        switch (num)
                        {
                            case 0x52e777e3:
                                if (shopifyWebsite == "bapeus")
                                {
                                    foreach (KeyValuePair<string, string> pair14 in runner.Product.AvailableSizes)
                                    {
                                        char[] separator = new char[] { '/' };
                                        string[] strArray = pair14.Key.Split(separator);
                                        for (int i = 0; i < strArray.Length; i++)
                                        {
                                            strArray[i] = strArray[i].Trim();
                                        }
                                        if (string.IsNullOrEmpty(task.Color))
                                        {
                                            list.Add(new KeyValuePair<string, string>(strArray[2], pair14.Value));
                                        }
                                        else
                                        {
                                            bool flag = false;
                                            if ((task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains) && strArray[1].ToLowerInvariant().Contains(task.Color.ToLowerInvariant()))
                                            {
                                                flag = true;
                                            }
                                            else if (strArray[1].ToLowerInvariant() == task.Color.ToLowerInvariant())
                                            {
                                                flag = true;
                                            }
                                            if (flag)
                                            {
                                                list.Add(new KeyValuePair<string, string>(strArray[2], pair14.Value));
                                            }
                                        }
                                    }
                                    if ((list.Count <= 0) && task.ColorPickRandom)
                                    {
                                        list = new List<KeyValuePair<string, string>>();
                                        foreach (KeyValuePair<string, string> pair15 in runner.Product.AvailableSizes)
                                        {
                                            char[] separator = new char[] { '/' };
                                            string[] strArray2 = pair15.Key.Split(separator);
                                            for (int i = 0; i < strArray2.Length; i++)
                                            {
                                                strArray2[i] = strArray2[i].Trim();
                                            }
                                            list.Add(new KeyValuePair<string, string>(strArray2[2], pair15.Value));
                                        }
                                        runner.Product.AvailableSizes = list;
                                        return;
                                    }
                                    runner.Product.AvailableSizes = list;
                                }
                                return;

                            case 0x5991c81a:
                                if (shopifyWebsite != "usg")
                                {
                                    return;
                                }
                                foreach (KeyValuePair<string, string> pair34 in runner.Product.AvailableSizes)
                                {
                                    string key = pair34.Key;
                                    if (key.Contains("/"))
                                    {
                                        key = key.Substring(key.LastIndexOf("/") + 1).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair34.Value));
                                }
                                runner.Product.AvailableSizes = list;
                                break;
                        }
                    }
                    else if (num == 0x63c27943)
                    {
                        if (shopifyWebsite == "havenshop")
                        {
                            foreach (KeyValuePair<string, string> pair24 in runner.Product.AvailableSizes)
                            {
                                string key = pair24.Key;
                                if (!char.IsDigit(key[0]))
                                {
                                    key = key.Substring(key.IndexOf("/") + 1);
                                }
                                if (key.Contains("/"))
                                {
                                    key = key.Substring(0, key.IndexOf("/")).Trim();
                                }
                                key = key.Replace("US", "").Trim();
                                list.Add(new KeyValuePair<string, string>(key, pair24.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num == 0x63f2cf1c)
                    {
                        bool flag10 = shopifyWebsite == "socialstatus";
                    }
                    else if (num == 0x6d6601da)
                    {
                        bool flag11 = shopifyWebsite == "shophny";
                    }
                }
                else if (num > 0x367ce9a5)
                {
                    if (num > 0x4b0d5ebb)
                    {
                        if (num > 0x4e70bbdf)
                        {
                            if (num == 0x4fbf7fe8)
                            {
                                if (shopifyWebsite == "munkyking")
                                {
                                    foreach (KeyValuePair<string, string> pair20 in runner.Product.AvailableSizes)
                                    {
                                        pair20.Key.Replace("Default Title", "-");
                                    }
                                    runner.Product.AvailableSizes = list;
                                }
                            }
                            else if (num == 0x4fe5226a)
                            {
                                bool flag12 = shopifyWebsite == "ficegallery";
                            }
                            else if (num == 0x5134779d)
                            {
                                bool flag13 = shopifyWebsite == "palaceus";
                            }
                        }
                        else if (num == 0x4e1453cf)
                        {
                            bool flag14 = shopifyWebsite == "proper";
                        }
                        else if ((num == 0x4e70bbdf) && (shopifyWebsite == "premierestore"))
                        {
                            foreach (KeyValuePair<string, string> pair36 in runner.Product.AvailableSizes)
                            {
                                string key = pair36.Key;
                                if (key.Contains("/"))
                                {
                                    key = pair36.Key.Substring(0, pair36.Key.IndexOf("/")).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair36.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num > 0x396a78ce)
                    {
                        switch (num)
                        {
                            case 0x3c311fad:
                            {
                                bool flag17 = shopifyWebsite == "bimtoy";
                                return;
                            }
                            case 0x4242753b:
                            {
                                bool flag15 = shopifyWebsite == "laceup";
                                return;
                            }
                        }
                        if (num == 0x4b0d5ebb)
                        {
                            bool flag16 = shopifyWebsite == "palaceeu";
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 0x38b6e100:
                            {
                                bool flag19 = shopifyWebsite == "funko";
                                return;
                            }
                            case 0x396a78ce:
                            {
                                bool flag18 = shopifyWebsite == "lapstoneandhammer";
                                return;
                            }
                        }
                    }
                }
                else if (num <= 0x2315585e)
                {
                    if (num > 0x494f792)
                    {
                        if (num == 0x2132f0cd)
                        {
                            if (shopifyWebsite == "hanon")
                            {
                                foreach (KeyValuePair<string, string> pair4 in runner.Product.AvailableSizes)
                                {
                                    string key = pair4.Key;
                                    if (key.Contains("US"))
                                    {
                                        key = key.Substring(key.IndexOf("US") + 2).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair4.Value));
                                }
                                runner.Product.AvailableSizes = list;
                            }
                        }
                        else if ((num == 0x2315585e) && (shopifyWebsite == "offthehook"))
                        {
                            foreach (KeyValuePair<string, string> pair19 in runner.Product.AvailableSizes)
                            {
                                string key = pair19.Key;
                                if (key.Contains("/"))
                                {
                                    key = pair19.Key.Substring(pair19.Key.LastIndexOf("/") + 1).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair19.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 0x2850ca0:
                                if (shopifyWebsite == "undefeated")
                                {
                                    foreach (KeyValuePair<string, string> pair2 in runner.Product.AvailableSizes)
                                    {
                                        string key = pair2.Key.Substring(pair2.Key.IndexOf("/") + 1).Trim();
                                        list.Add(new KeyValuePair<string, string>(key, pair2.Value));
                                    }
                                    runner.Product.AvailableSizes = list;
                                }
                                return;

                            case 0x494f792:
                            {
                                bool flag20 = shopifyWebsite == "saintalfred";
                                return;
                            }
                        }
                    }
                }
                else if (num > 0x28553330)
                {
                    switch (num)
                    {
                        case 0x33857181:
                        {
                            bool flag21 = shopifyWebsite == "mralans";
                            return;
                        }
                        case 0x339ec4fc:
                        {
                            bool flag22 = shopifyWebsite == "renarts";
                            return;
                        }
                    }
                    if ((num == 0x367ce9a5) && (shopifyWebsite == "kylie"))
                    {
                        foreach (KeyValuePair<string, string> pair13 in runner.Product.AvailableSizes)
                        {
                            string key = pair13.Key.Replace("Default Title", "-");
                            list.Add(new KeyValuePair<string, string>(key, pair13.Value));
                        }
                        runner.Product.AvailableSizes = list;
                    }
                }
                else if (num == 0x23ef603b)
                {
                    bool flag23 = shopifyWebsite == "assc";
                }
                else if ((num == 0x28553330) && (shopifyWebsite == "nrml"))
                {
                    foreach (KeyValuePair<string, string> pair32 in runner.Product.AvailableSizes)
                    {
                        string key = pair32.Key;
                        while (key.Contains("/"))
                        {
                            if (key.Contains("/"))
                            {
                                key = key.Substring(key.LastIndexOf("/") + 1).Trim();
                            }
                            list.Add(new KeyValuePair<string, string>(key, pair32.Value));
                        }
                    }
                    runner.Product.AvailableSizes = list;
                }
            }
            else if (num > 0xc187c693)
            {
                if (num <= 0xe5c648dd)
                {
                    if (num <= 0xda304ded)
                    {
                        if (num <= 0xcea735af)
                        {
                            switch (num)
                            {
                                case 0xc5246ded:
                                    if (shopifyWebsite == "wishatl")
                                    {
                                        foreach (KeyValuePair<string, string> pair16 in runner.Product.AvailableSizes)
                                        {
                                            string key = pair16.Key;
                                            if (key.Contains("/"))
                                            {
                                                key = pair16.Key.Substring(pair16.Key.IndexOf("/") + 1).Trim();
                                            }
                                            list.Add(new KeyValuePair<string, string>(key, pair16.Value));
                                        }
                                        runner.Product.AvailableSizes = list;
                                    }
                                    return;

                                case 0xcea735af:
                                    if (shopifyWebsite != "subliminal")
                                    {
                                        return;
                                    }
                                    foreach (KeyValuePair<string, string> pair6 in runner.Product.AvailableSizes)
                                    {
                                        string key = pair6.Key.Replace("Default Title", "-");
                                        list.Add(new KeyValuePair<string, string>(key, pair6.Value));
                                    }
                                    runner.Product.AvailableSizes = list;
                                    break;
                            }
                        }
                        else if (num == 0xcfc98935)
                        {
                            bool flag24 = shopifyWebsite == "awakenyclothing";
                        }
                        else if (num == 0xd39d8c83)
                        {
                            if (shopifyWebsite == "xhibition")
                            {
                                foreach (KeyValuePair<string, string> pair7 in runner.Product.AvailableSizes)
                                {
                                    string key = pair7.Key;
                                    if (key.Contains("/"))
                                    {
                                        key = key.Substring(0, key.IndexOf("/")).Trim();
                                    }
                                    if (key.Contains(" "))
                                    {
                                        key = key.Substring(0, key.IndexOf(" ")).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair7.Value));
                                }
                                runner.Product.AvailableSizes = list;
                            }
                        }
                        else if ((num == 0xda304ded) && (shopifyWebsite == "blendsus"))
                        {
                            foreach (KeyValuePair<string, string> pair27 in runner.Product.AvailableSizes)
                            {
                                string key = pair27.Key;
                                if (key.Contains("/"))
                                {
                                    key = pair27.Key.Substring(0, pair27.Key.IndexOf("/")).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair27.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num > 0xdfdfcb39)
                    {
                        switch (num)
                        {
                            case 0xe11403c8:
                                if (shopifyWebsite == "ultrafootball")
                                {
                                    foreach (KeyValuePair<string, string> pair21 in runner.Product.AvailableSizes)
                                    {
                                        string key = pair21.Key;
                                        if (key.Contains("/"))
                                        {
                                            key = key.Substring(0, key.LastIndexOf("/")).Trim();
                                        }
                                        list.Add(new KeyValuePair<string, string>(key, pair21.Value));
                                    }
                                    runner.Product.AvailableSizes = list;
                                }
                                return;

                            case 0xe1c6240d:
                                if (shopifyWebsite == "commonwealth")
                                {
                                    foreach (KeyValuePair<string, string> pair25 in runner.Product.AvailableSizes)
                                    {
                                        string key = pair25.Key;
                                        if (key.Contains("/"))
                                        {
                                            key = pair25.Key.Substring(0, pair25.Key.IndexOf("/")).Trim();
                                        }
                                        list.Add(new KeyValuePair<string, string>(key, pair25.Value));
                                    }
                                    runner.Product.AvailableSizes = list;
                                }
                                return;
                        }
                        if ((num == 0xe5c648dd) && (shopifyWebsite == "solefly"))
                        {
                            foreach (KeyValuePair<string, string> pair10 in runner.Product.AvailableSizes)
                            {
                                string key = pair10.Key;
                                if (key.Contains("/"))
                                {
                                    key = pair10.Key.Substring(pair10.Key.LastIndexOf("/") + 1).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair10.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num == 0xdfd25109)
                    {
                        if (shopifyWebsite == "18montrose")
                        {
                            foreach (KeyValuePair<string, string> pair38 in runner.Product.AvailableSizes)
                            {
                                string key = pair38.Key;
                                if (key.Contains("/"))
                                {
                                    key = pair38.Key.Substring(0, pair38.Key.IndexOf("/")).Trim();
                                }
                                key = key.Replace("UK", "").Replace("US", "").Trim();
                                list.Add(new KeyValuePair<string, string>(key, pair38.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num == 0xdfdfcb39)
                    {
                        bool flag25 = shopifyWebsite == "sneakerjunkies";
                    }
                }
                else if (num > 0xe9775203)
                {
                    switch (num)
                    {
                        case 0xef531df0:
                        {
                            bool flag28 = shopifyWebsite == "mondo";
                            return;
                        }
                        case 0xf76a8c18:
                        {
                            bool flag30 = shopifyWebsite == "travis";
                            return;
                        }
                        case 0xfffe9b0d:
                        {
                            bool flag29 = shopifyWebsite == "soleclassics";
                            return;
                        }
                        case 0xec5749a4:
                        {
                            bool flag27 = shopifyWebsite == "noirfonce";
                            return;
                        }
                        case 0xed429ab7:
                        {
                            bool flag26 = shopifyWebsite == "ellenshop";
                            return;
                        }
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0xe8f780f3:
                            if (shopifyWebsite == "likelihood")
                            {
                                foreach (KeyValuePair<string, string> pair17 in runner.Product.AvailableSizes)
                                {
                                    string key = pair17.Key;
                                    if (key.Contains("M"))
                                    {
                                        key = key.Substring(0, key.LastIndexOf("M")).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair17.Value));
                                }
                                runner.Product.AvailableSizes = list;
                            }
                            return;

                        case 0xe8fbf68d:
                        {
                            bool flag31 = shopifyWebsite == "kith";
                            return;
                        }
                        case 0xe9775203:
                        {
                            bool flag32 = shopifyWebsite == "fuct";
                            return;
                        }
                        case 0xe72487ea:
                            if (shopifyWebsite == "unknwn")
                            {
                                foreach (KeyValuePair<string, string> pair8 in runner.Product.AvailableSizes)
                                {
                                    if (!pair8.Key.Contains("/"))
                                    {
                                        string key = pair8.Key.Trim();
                                        list.Add(new KeyValuePair<string, string>(key, pair8.Value));
                                    }
                                    else
                                    {
                                        string key = pair8.Key.Substring(pair8.Key.LastIndexOf("/") + 1).Trim();
                                        list.Add(new KeyValuePair<string, string>(key, pair8.Value));
                                    }
                                }
                                runner.Product.AvailableSizes = list;
                            }
                            return;

                        case 0xe8e8c154:
                            if (shopifyWebsite != "aboveclouds")
                            {
                                return;
                            }
                            foreach (KeyValuePair<string, string> pair35 in runner.Product.AvailableSizes)
                            {
                                string key = pair35.Key;
                                if (key.ToLowerInvariant().Contains("US".ToLowerInvariant()))
                                {
                                    key = pair35.Key.Substring(pair35.Key.IndexOf("US") + 2);
                                    if (key.Contains("|"))
                                    {
                                        key = key.Substring(0, key.IndexOf("|"));
                                    }
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair35.Value));
                            }
                            runner.Product.AvailableSizes = list;
                            break;
                    }
                }
            }
            else if (num <= 0x9cd661de)
            {
                if (num > 0x9553d8f4)
                {
                    if (num > 0x9677337c)
                    {
                        if (num == 0x98f09984)
                        {
                            if (shopifyWebsite == "oipolloi")
                            {
                                foreach (KeyValuePair<string, string> pair30 in runner.Product.AvailableSizes)
                                {
                                    string key = pair30.Key;
                                    if (key.Contains(" "))
                                    {
                                        key = key.Substring(key.IndexOf(" ")).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair30.Value));
                                }
                                runner.Product.AvailableSizes = list;
                            }
                        }
                        else if (num == 0x9c7b3b5a)
                        {
                            if (shopifyWebsite == "amongstfew")
                            {
                                foreach (KeyValuePair<string, string> pair9 in runner.Product.AvailableSizes)
                                {
                                    string key = pair9.Key.Replace("US", "").Trim();
                                    if (key.Contains("/"))
                                    {
                                        key = key.Substring(0, key.IndexOf("/")).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair9.Value));
                                }
                                runner.Product.AvailableSizes = list;
                            }
                        }
                        else if ((num == 0x9cd661de) && (shopifyWebsite == "darkside"))
                        {
                            foreach (KeyValuePair<string, string> pair22 in runner.Product.AvailableSizes)
                            {
                                string key = pair22.Key;
                                if (key.Contains("/"))
                                {
                                    key = key.Substring(0, key.IndexOf("/")).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair22.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num == 0x95928b2a)
                    {
                        if (shopifyWebsite == "eflashus")
                        {
                            foreach (KeyValuePair<string, string> pair26 in runner.Product.AvailableSizes)
                            {
                                string key = pair26.Key;
                                if (key.Contains(" "))
                                {
                                    key = key.Substring(key.LastIndexOf(" ") + 1).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair26.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num == 0x9677337c)
                    {
                        bool flag33 = shopifyWebsite == "noirfoncefr";
                    }
                }
                else if (num > 0x94481229)
                {
                    if (num == 0x94e14619)
                    {
                        if (shopifyWebsite == "bodega")
                        {
                            foreach (KeyValuePair<string, string> pair29 in runner.Product.AvailableSizes)
                            {
                                string key = pair29.Key;
                                if (key.Contains("/"))
                                {
                                    key = pair29.Key.Substring(pair29.Key.IndexOf("/") + 1).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair29.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                    }
                    else if (num == 0x9553d8f4)
                    {
                        bool flag34 = shopifyWebsite == "leaders1354";
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0x90e1a871:
                            if (shopifyWebsite == "koolkidtoys")
                            {
                                foreach (KeyValuePair<string, string> pair in runner.Product.AvailableSizes)
                                {
                                    pair.Key.Replace("Default Title", "-");
                                }
                                runner.Product.AvailableSizes = list;
                            }
                            return;

                        case 0x94481229:
                        {
                            bool flag35 = shopifyWebsite == "capsuletoronto";
                            break;
                        }
                    }
                }
            }
            else if (num <= 0xac1f3994)
            {
                switch (num)
                {
                    case 0x9e50970b:
                        if (shopifyWebsite == "soleheaven")
                        {
                            foreach (KeyValuePair<string, string> pair33 in runner.Product.AvailableSizes)
                            {
                                if (!pair33.Key.Contains("US"))
                                {
                                    string key = pair33.Key.Trim();
                                    list.Add(new KeyValuePair<string, string>(key, pair33.Value));
                                }
                                else
                                {
                                    string key = pair33.Key.Substring(pair33.Key.LastIndexOf("US") + 2).Trim();
                                    if (key.Contains("/"))
                                    {
                                        key = key.Substring(0, key.IndexOf("/")).Trim();
                                    }
                                    list.Add(new KeyValuePair<string, string>(key, pair33.Value));
                                }
                            }
                            runner.Product.AvailableSizes = list;
                        }
                        return;

                    case 0xa2cb1dd3:
                        if (shopifyWebsite == "stashed")
                        {
                            foreach (KeyValuePair<string, string> pair11 in runner.Product.AvailableSizes)
                            {
                                string key = pair11.Key;
                                if (key.Contains("/"))
                                {
                                    key = key.Substring(key.LastIndexOf("/") + 1).Trim();
                                }
                                list.Add(new KeyValuePair<string, string>(key, pair11.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                        return;

                    case 0xac1f3994:
                    {
                        bool flag36 = shopifyWebsite == "machus";
                        return;
                    }
                    case 0x9e145f8b:
                        if (shopifyWebsite == "deadstock")
                        {
                            foreach (KeyValuePair<string, string> pair5 in runner.Product.AvailableSizes)
                            {
                                string key = pair5.Key.Replace("- / ", "").Trim();
                                list.Add(new KeyValuePair<string, string>(key, pair5.Value));
                            }
                            runner.Product.AvailableSizes = list;
                        }
                        return;

                    case 0x9e4864ef:
                        if (shopifyWebsite != "addictmiami")
                        {
                            return;
                        }
                        foreach (KeyValuePair<string, string> pair28 in runner.Product.AvailableSizes)
                        {
                            string key = pair28.Key;
                            if (key.Contains("US"))
                            {
                                key = pair28.Key.Substring(0, pair28.Key.IndexOf("US")).Trim();
                            }
                            list.Add(new KeyValuePair<string, string>(key, pair28.Value));
                        }
                        runner.Product.AvailableSizes = list;
                        break;
                }
            }
            else if (num <= 0xb4e9bd0e)
            {
                switch (num)
                {
                    case 0xb0d66017:
                    {
                        bool flag37 = shopifyWebsite == "closetinc";
                        return;
                    }
                    case 0xb4e9bd0e:
                        if (shopifyWebsite != "obey")
                        {
                            return;
                        }
                        foreach (KeyValuePair<string, string> pair31 in runner.Product.AvailableSizes)
                        {
                            string key = pair31.Key;
                            if (key.ToLowerInvariant().Contains("Default Title".ToLowerInvariant()))
                            {
                                key = "-";
                            }
                            list.Add(new KeyValuePair<string, string>(key, pair31.Value));
                        }
                        runner.Product.AvailableSizes = list;
                        break;
                }
            }
            else
            {
                switch (num)
                {
                    case 0xb77b9be7:
                    {
                        bool flag40 = shopifyWebsite == "shopnicekicks";
                        return;
                    }
                    case 0xc0c6237f:
                    {
                        bool flag38 = shopifyWebsite == "creme321";
                        return;
                    }
                }
                if (num == 0xc187c693)
                {
                    bool flag39 = shopifyWebsite == "yeezy";
                }
            }
        }

        internal static string UpdateSizeString(TaskObject task, string size)
        {
            string shopifyWebsite = task.ShopifyWebsite;
            uint num2 = <PrivateImplementationDetails>.ComputeStringHash(shopifyWebsite);
            if (num2 <= 0x8fb99128)
            {
                if (num2 > 0x5134779d)
                {
                    if (num2 <= 0x783ca8cb)
                    {
                        if (num2 <= 0x6d6601da)
                        {
                            if (num2 <= 0x5991c81a)
                            {
                                if (num2 == 0x52e777e3)
                                {
                                    if (shopifyWebsite == "bapeus")
                                    {
                                    }
                                    return size;
                                }
                                if ((num2 == 0x5991c81a) && (shopifyWebsite == "usg"))
                                {
                                    size = size.Replace("Default Title", "-");
                                    if (size.Contains("/"))
                                    {
                                        size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                                    }
                                }
                                return size;
                            }
                            switch (num2)
                            {
                                case 0x63c27943:
                                    if (shopifyWebsite == "havenshop")
                                    {
                                        if (size.Contains("/"))
                                        {
                                            size = size.Substring(0, size.LastIndexOf("/")).Trim();
                                        }
                                        size = size.Replace("US", "").Trim();
                                    }
                                    return size;

                                case 0x63f2cf1c:
                                    if (shopifyWebsite == "socialstatus")
                                    {
                                    }
                                    return size;
                            }
                            if ((num2 == 0x6d6601da) && (shopifyWebsite != "shophny"))
                            {
                            }
                            return size;
                        }
                        if (num2 <= 0x6f7f4ed7)
                        {
                            if (num2 == 0x6d970277)
                            {
                                if ((shopifyWebsite == "notre") && size.Contains("/"))
                                {
                                    size = size.Substring(0, size.IndexOf("/")).Trim();
                                }
                                return size;
                            }
                            if ((num2 != 0x6f7f4ed7) || (shopifyWebsite != "packer"))
                            {
                            }
                            return size;
                        }
                        switch (num2)
                        {
                            case 0x73761b67:
                                if (shopifyWebsite == "a-ma-maniere")
                                {
                                }
                                return size;

                            case 0x756a879f:
                                if (shopifyWebsite == "abpstore")
                                {
                                }
                                return size;
                        }
                        if ((num2 == 0x783ca8cb) && (shopifyWebsite != "shoegallerymiami"))
                        {
                        }
                        return size;
                    }
                    if (num2 > 0x85732999)
                    {
                        if (num2 <= 0x8b55eb57)
                        {
                            if (num2 == 0x86c249a6)
                            {
                                if (shopifyWebsite == "cncpts")
                                {
                                }
                                return size;
                            }
                            if ((num2 == 0x8b55eb57) && (shopifyWebsite == "sneakerpolitics"))
                            {
                            }
                            return size;
                        }
                        switch (num2)
                        {
                            case 0x8c389037:
                                if ((shopifyWebsite == "bbbranded") && size.Contains("/"))
                                {
                                    size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                                }
                                return size;

                            case 0x8f74e9e0:
                                if (shopifyWebsite == "noirfoncees")
                                {
                                }
                                return size;
                        }
                        if ((num2 == 0x8fb99128) && ((shopifyWebsite == "eflasheu") && size.Contains(" ")))
                        {
                            size = size.Substring(size.LastIndexOf(" ") + 1).Trim();
                        }
                        return size;
                    }
                    if (num2 > 0x7aade1ed)
                    {
                        switch (num2)
                        {
                            case 0x81836124:
                                if (shopifyWebsite == "eflashsg")
                                {
                                }
                                return size;

                            case 0x819e199a:
                                if (shopifyWebsite == "rsvpgallery")
                                {
                                    while (size.Contains("/"))
                                    {
                                        size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                                    }
                                }
                                return size;
                        }
                        if ((num2 == 0x85732999) && (shopifyWebsite == "trophyroomstore"))
                        {
                        }
                        return size;
                    }
                    if (num2 == 0x7a949f40)
                    {
                        if ((shopifyWebsite == "eflashjp") && size.Contains(" "))
                        {
                            size = size.Substring(size.LastIndexOf(" ") + 1).Trim();
                        }
                        return size;
                    }
                    if (((num2 == 0x7aade1ed) && (shopifyWebsite == "pilgrim")) && size.Contains("/"))
                    {
                        size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                    }
                    return size;
                }
                if (num2 > 0x367ce9a5)
                {
                    if (num2 > 0x4b0d5ebb)
                    {
                        if (num2 <= 0x4e70bbdf)
                        {
                            if (num2 == 0x4e1453cf)
                            {
                                if (shopifyWebsite == "proper")
                                {
                                }
                                return size;
                            }
                            if (((num2 == 0x4e70bbdf) && (shopifyWebsite == "premierestore")) && size.Contains("/"))
                            {
                                size = size.Substring(0, size.IndexOf("/")).Trim();
                            }
                            return size;
                        }
                        switch (num2)
                        {
                            case 0x4fbf7fe8:
                                if (shopifyWebsite == "munkyking")
                                {
                                    size = size.Replace("Default Title", "-");
                                }
                                return size;

                            case 0x4fe5226a:
                                if (shopifyWebsite == "ficegallery")
                                {
                                }
                                return size;
                        }
                        if ((num2 != 0x5134779d) || (shopifyWebsite != "palaceus"))
                        {
                        }
                        return size;
                    }
                    if (num2 <= 0x396a78ce)
                    {
                        if (num2 == 0x38b6e100)
                        {
                            if (shopifyWebsite == "funko")
                            {
                            }
                            return size;
                        }
                        if ((num2 == 0x396a78ce) && (shopifyWebsite != "lapstoneandhammer"))
                        {
                        }
                        return size;
                    }
                    switch (num2)
                    {
                        case 0x3c311fad:
                            if (shopifyWebsite == "bimtoy")
                            {
                            }
                            return size;

                        case 0x4242753b:
                            if (shopifyWebsite == "laceup")
                            {
                            }
                            return size;
                    }
                    if ((num2 == 0x4b0d5ebb) && (shopifyWebsite != "palaceeu"))
                    {
                    }
                    return size;
                }
                if (num2 <= 0x2315585e)
                {
                    if (num2 > 0x494f792)
                    {
                        if (num2 == 0x2132f0cd)
                        {
                            if ((shopifyWebsite == "hanon") && size.Contains("US"))
                            {
                                size = size.Substring(size.IndexOf("US") + 2).Trim();
                            }
                            return size;
                        }
                        if (((num2 == 0x2315585e) && (shopifyWebsite == "offthehook")) && size.Contains("/"))
                        {
                            size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                        }
                        return size;
                    }
                    if (num2 == 0x2850ca0)
                    {
                        if ((shopifyWebsite == "undefeated") && size.Contains("/"))
                        {
                            size = size.Substring(size.IndexOf("/") + 1).Trim();
                        }
                        return size;
                    }
                    if ((num2 == 0x494f792) && (shopifyWebsite != "saintalfred"))
                    {
                    }
                    return size;
                }
                if (num2 <= 0x28553330)
                {
                    if (num2 == 0x23ef603b)
                    {
                        if (shopifyWebsite == "assc")
                        {
                            size = size.Replace("Default Title", "-");
                        }
                        return size;
                    }
                    if ((num2 == 0x28553330) && (shopifyWebsite == "nrml"))
                    {
                        if (size.Contains("/"))
                        {
                            size = size.Substring(0, size.LastIndexOf("/")).Trim();
                        }
                        if (size.Contains("/"))
                        {
                            size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                        }
                    }
                    return size;
                }
                switch (num2)
                {
                    case 0x33857181:
                        if (shopifyWebsite == "mralans")
                        {
                        }
                        return size;

                    case 0x339ec4fc:
                        if (shopifyWebsite == "renarts")
                        {
                        }
                        return size;
                }
                if ((num2 == 0x367ce9a5) && (shopifyWebsite == "kylie"))
                {
                    size = size.Replace("Default Title", "-");
                }
                return size;
            }
            if (num2 <= 0xc5246ded)
            {
                if (num2 > 0x9e145f8b)
                {
                    if (num2 > 0xb0d66017)
                    {
                        if (num2 <= 0xb77b9be7)
                        {
                            if (num2 == 0xb4e9bd0e)
                            {
                                if ((shopifyWebsite == "obey") && size.Contains("Default title"))
                                {
                                    size = "-";
                                }
                                return size;
                            }
                            if ((num2 == 0xb77b9be7) && (shopifyWebsite == "shopnicekicks"))
                            {
                            }
                            return size;
                        }
                        switch (num2)
                        {
                            case 0xc0c6237f:
                                if (shopifyWebsite == "creme321")
                                {
                                }
                                return size;

                            case 0xc187c693:
                                if (shopifyWebsite == "yeezy")
                                {
                                }
                                return size;
                        }
                        if ((num2 == 0xc5246ded) && ((shopifyWebsite == "wishatl") && size.Contains("/")))
                        {
                            size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                        }
                        return size;
                    }
                    if (num2 <= 0x9e50970b)
                    {
                        switch (num2)
                        {
                            case 0x9e4864ef:
                                if ((shopifyWebsite == "addictmiami") && size.Contains("US"))
                                {
                                    size = size.Substring(0, size.IndexOf("US")).Trim();
                                }
                                return size;

                            case 0x9e50970b:
                            {
                                if ((shopifyWebsite != "soleheaven") || !size.Contains("US"))
                                {
                                    return size;
                                }
                                string str2 = size.Substring(size.LastIndexOf("US") + 2).Trim();
                                if (str2.Contains("/"))
                                {
                                    str2 = str2.Substring(0, str2.IndexOf("/")).Trim();
                                }
                                size = str2;
                                break;
                            }
                        }
                        return size;
                    }
                    switch (num2)
                    {
                        case 0xa2cb1dd3:
                            if ((shopifyWebsite == "stashed") && size.Contains("/"))
                            {
                                size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                            }
                            return size;

                        case 0xac1f3994:
                            if (shopifyWebsite == "machus")
                            {
                            }
                            return size;
                    }
                    if ((num2 != 0xb0d66017) || (shopifyWebsite != "closetinc"))
                    {
                    }
                    return size;
                }
                if (num2 <= 0x95928b2a)
                {
                    if (num2 > 0x94481229)
                    {
                        switch (num2)
                        {
                            case 0x94e14619:
                                if ((shopifyWebsite == "bodega") && size.Contains("/"))
                                {
                                    size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                                }
                                return size;

                            case 0x9553d8f4:
                                if (shopifyWebsite == "leaders1354")
                                {
                                }
                                return size;
                        }
                        if (((num2 == 0x95928b2a) && (shopifyWebsite == "eflashus")) && size.Contains(" "))
                        {
                            size = size.Substring(size.LastIndexOf(" ") + 1).Trim();
                        }
                        return size;
                    }
                    if (num2 == 0x90e1a871)
                    {
                        if (shopifyWebsite == "koolkidtoys")
                        {
                            size = size.Replace("Default Title", "-");
                        }
                        return size;
                    }
                    if ((num2 == 0x94481229) && (shopifyWebsite == "capsuletoronto"))
                    {
                    }
                    return size;
                }
                if (num2 > 0x98f09984)
                {
                    switch (num2)
                    {
                        case 0x9c7b3b5a:
                            if (shopifyWebsite == "amongstfew")
                            {
                                size = size.Replace("US", "").ToString();
                                if (size.Contains("/"))
                                {
                                    size = size.Substring(0, size.LastIndexOf("/")).Trim();
                                }
                            }
                            return size;

                        case 0x9cd661de:
                            if (shopifyWebsite == "darkside")
                            {
                                size = size.Replace("Default Title", "-");
                                if (size.Contains("/"))
                                {
                                    size = size.Substring(0, size.LastIndexOf("/")).Trim();
                                }
                            }
                            return size;
                    }
                    if ((num2 == 0x9e145f8b) && (shopifyWebsite != "deadstock"))
                    {
                    }
                    return size;
                }
                if (num2 == 0x9677337c)
                {
                    if (shopifyWebsite == "noirfoncefr")
                    {
                    }
                    return size;
                }
                if ((num2 == 0x98f09984) && ((shopifyWebsite == "oipolloi") && size.Contains(" ")))
                {
                    size = size.Substring(size.IndexOf(" ")).Trim();
                }
                return size;
            }
            if (num2 <= 0xe5c648dd)
            {
                if (num2 <= 0xda304ded)
                {
                    if (num2 > 0xcea735af)
                    {
                        switch (num2)
                        {
                            case 0xcfc98935:
                                if (shopifyWebsite == "awakenyclothing")
                                {
                                }
                                return size;

                            case 0xd39d8c83:
                                if (shopifyWebsite == "xhibition")
                                {
                                    if (size.Contains("/"))
                                    {
                                        size = size.Substring(0, size.IndexOf("/")).Trim();
                                    }
                                    if (size.Contains(" "))
                                    {
                                        size = size.Substring(0, size.IndexOf(" ")).Trim();
                                    }
                                }
                                return size;
                        }
                        if ((num2 == 0xda304ded) && ((shopifyWebsite == "blendsus") && size.Contains("/")))
                        {
                            size = size.Substring(0, size.IndexOf("/")).Trim();
                        }
                        return size;
                    }
                    if (num2 == 0xcca4340f)
                    {
                        if (shopifyWebsite == "xxx")
                        {
                        }
                        return size;
                    }
                    if ((num2 == 0xcea735af) && (shopifyWebsite == "subliminal"))
                    {
                        size = size.Replace("Default Title", "-");
                    }
                    return size;
                }
                if (num2 > 0xdfdfcb39)
                {
                    switch (num2)
                    {
                        case 0xe11403c8:
                            if (shopifyWebsite == "ultrafootball")
                            {
                            }
                            return size;

                        case 0xe1c6240d:
                            if ((shopifyWebsite == "commonwealth") && size.Contains("/"))
                            {
                                size = size.Substring(0, size.IndexOf("/")).Trim();
                            }
                            return size;
                    }
                    if ((num2 == 0xe5c648dd) && (shopifyWebsite == "solefly"))
                    {
                    }
                    return size;
                }
                if (num2 == 0xdfd25109)
                {
                    if (shopifyWebsite == "18montrose")
                    {
                        if (size.Contains("/"))
                        {
                            size = size.Substring(0, size.IndexOf("/")).Trim();
                        }
                        size = size.Replace("US", "").Replace("UK", "").Trim();
                    }
                    return size;
                }
                if ((num2 == 0xdfdfcb39) && (shopifyWebsite == "sneakerjunkies"))
                {
                }
                return size;
            }
            if (num2 <= 0xe9775203)
            {
                if (num2 <= 0xe8e8c154)
                {
                    if (num2 == 0xe72487ea)
                    {
                        if ((shopifyWebsite == "unknwn") && size.Contains("/"))
                        {
                            size = size.Substring(size.LastIndexOf("/") + 1).Trim();
                        }
                        return size;
                    }
                    if ((num2 == 0xe8e8c154) && (shopifyWebsite != "aboveclouds"))
                    {
                    }
                    return size;
                }
                switch (num2)
                {
                    case 0xe8f780f3:
                        if (shopifyWebsite == "likelihood")
                        {
                        }
                        return size;

                    case 0xe8fbf68d:
                        if (shopifyWebsite == "kith")
                        {
                        }
                        return size;
                }
                if ((num2 == 0xe9775203) && (shopifyWebsite == "fuct"))
                {
                }
                return size;
            }
            if (num2 > 0xed429ab7)
            {
                switch (num2)
                {
                    case 0xef531df0:
                        if (shopifyWebsite == "mondo")
                        {
                        }
                        return size;

                    case 0xf76a8c18:
                        if (shopifyWebsite == "travis")
                        {
                        }
                        return size;
                }
                if ((num2 == 0xfffe9b0d) && (shopifyWebsite != "soleclassics"))
                {
                }
                return size;
            }
            if (num2 == 0xec5749a4)
            {
                if (shopifyWebsite == "noirfonce")
                {
                }
                return size;
            }
            if ((num2 == 0xed429ab7) && (shopifyWebsite != "ellenshop"))
            {
            }
            return size;
        }
    }
}

