namespace EveAIO
{
    using EveAIO.Tasks;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    internal class WebsitesInfo
    {
        public static List<KeyValuePair<string, string>> SUPPORTED_PLATFORMS;
        private static Random _rnd;
        internal static KeyValuePair<string, string> KITH_PROPERTIES;
        internal static KeyValuePair<string, string> FUNKO_PROPERTIES;
        internal static KeyValuePair<string, string> EFLESH_US_PROPERTIES;
        internal static KeyValuePair<string, string> KITH_PROPERTIES_DOWN;
        internal static KeyValuePair<string, string>? EFLESH_UK_PROPERTIES;
        public static List<KeyValuePair<string, string>> PREDEFINED_SHOPIFY;
        public static List<ShopifyWebsiteInfo> SHOPIFY_WEBS;
        public static string SUPREME_CAPTCHA_KEY;
        public static object SNS_CAPTCHA_KEY;
        public static string SHOPIFY_CAPTCHA_KEY;
        public static string MR_PORTER_CAPTCHA_KEY;
        public static string SIVAS_CAPTCHA_KEY;
        public static string HIBBET_CAPTCHA_KEY;
        public static string HOLYPOP_CAPTCHA_KEY;
        public static string OW_CAPTCHA_KEY;
        public static string FOOTACTION_CAPTCHA_KEY;
        public static string PRIVACY_CAPTCHA_KEY;
        internal static List<KeyValuePair<string, string>> SOLEBOX_COUNTRIES;
        internal static List<KeyValuePair<string, string>> HOLYPOP_COUNTRIES;
        internal static List<KeyValuePair<string, string>> OFFWHITE_COUNTRIES;

        static WebsitesInfo()
        {
            Class7.RIuqtBYzWxthF();
            _rnd = new Random(DateTime.Now.Millisecond);
            EFLESH_US_PROPERTIES = new KeyValuePair<string, string>("_GGXX", "483e929bGG86aa9202eXX93670ac542e9");
            KITH_PROPERTIES_DOWN = new KeyValuePair<string, string>("kRg5GNuFdmDMAc5M", "YYQMk2bR4fw6LU69");
            EFLESH_UK_PROPERTIES = null;
            List<KeyValuePair<string, string>> list1 = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("18montrose", "18montrose"),
                new KeyValuePair<string, string>("aboveclouds", "Above the Clouds Store"),
                new KeyValuePair<string, string>("abpstore", "APB Store"),
                new KeyValuePair<string, string>("addictmiami", "ADDICT Miami"),
                new KeyValuePair<string, string>("a-ma-maniere", "A-Ma-Maniere"),
                new KeyValuePair<string, string>("amongstfew", "Amongstfew"),
                new KeyValuePair<string, string>("assc", "Anti Social Club"),
                new KeyValuePair<string, string>("awakenyclothing", "Awake NY"),
                new KeyValuePair<string, string>("bapeus", "Bape US"),
                new KeyValuePair<string, string>("bbbranded", "BB Branded"),
                new KeyValuePair<string, string>("bimtoy", "Bimtoy"),
                new KeyValuePair<string, string>("blendsus", "Blendsus"),
                new KeyValuePair<string, string>("bodega", "Bodega"),
                new KeyValuePair<string, string>("capsuletoronto", "Capsule Toronto"),
                new KeyValuePair<string, string>("closetinc", "Closet Inc"),
                new KeyValuePair<string, string>("commonwealth", "Commonwealth"),
                new KeyValuePair<string, string>("cncpts", "Concepts"),
                new KeyValuePair<string, string>("creme321", "Creme321"),
                new KeyValuePair<string, string>("deadstock", "Deadstock"),
                new KeyValuePair<string, string>("eflashjp", "Eflash JP"),
                new KeyValuePair<string, string>("eflashsg", "Eflash SG"),
                new KeyValuePair<string, string>("eflasheu", "Eflash UK"),
                new KeyValuePair<string, string>("eflashus", "Eflash US"),
                new KeyValuePair<string, string>("ellenshop", "Ellen shop"),
                new KeyValuePair<string, string>("ficegallery", "FICE gallery"),
                new KeyValuePair<string, string>("fuct", "Fuct"),
                new KeyValuePair<string, string>("funko", "Funko"),
                new KeyValuePair<string, string>("hanon", "Hanon-shop"),
                new KeyValuePair<string, string>("havenshop", "Havenshop"),
                new KeyValuePair<string, string>("kith", "Kith"),
                new KeyValuePair<string, string>("koolkidtoys", "Kool Kid Toys"),
                new KeyValuePair<string, string>("kylie", "Kylie Cosmetics"),
                new KeyValuePair<string, string>("laceup", "Lace Up"),
                new KeyValuePair<string, string>("lapstoneandhammer", "Lapstone & Hammer"),
                new KeyValuePair<string, string>("leaders1354", "Leaders 1354"),
                new KeyValuePair<string, string>("likelihood", "Likelihood"),
                new KeyValuePair<string, string>("machus", "Machus"),
                new KeyValuePair<string, string>("mondo", "Mondo"),
                new KeyValuePair<string, string>("mralans", "Mr. Alan's"),
                new KeyValuePair<string, string>("munkyking", "Munkyking"),
                new KeyValuePair<string, string>("noirfonce", "Noirfonce EU"),
                new KeyValuePair<string, string>("noirfoncefr", "Noirfonce FR"),
                new KeyValuePair<string, string>("noirfoncees", "Noirfonce ES"),
                new KeyValuePair<string, string>("notre", "Notre"),
                new KeyValuePair<string, string>("nrml", "Nrml"),
                new KeyValuePair<string, string>("obey", "ObeyGiant"),
                new KeyValuePair<string, string>("offthehook", "Offthehook CA"),
                new KeyValuePair<string, string>("oipolloi", "Oi Polloi"),
                new KeyValuePair<string, string>("packer", "Packershoes"),
                new KeyValuePair<string, string>("palaceeu", "Palace UK"),
                new KeyValuePair<string, string>("palaceus", "Palace US"),
                new KeyValuePair<string, string>("palacejp", "Palace JP"),
                new KeyValuePair<string, string>("pilgrim", "Pilgrim"),
                new KeyValuePair<string, string>("proper", "Proper"),
                new KeyValuePair<string, string>("renarts", "Renarts"),
                new KeyValuePair<string, string>("rsvpgallery", "RSVP Gallery"),
                new KeyValuePair<string, string>("saintalfred", "Saint Alfred"),
                new KeyValuePair<string, string>("shoegallerymiami", "Shoe Gallery Miami"),
                new KeyValuePair<string, string>("shophny", "Shophny"),
                new KeyValuePair<string, string>("shopnicekicks", "Shopnicekicks"),
                new KeyValuePair<string, string>("sneakerjunkies", "Sneaker Junkies"),
                new KeyValuePair<string, string>("sneakerpolitics", "Sneakerpolitics"),
                new KeyValuePair<string, string>("socialstatus", "Social Status"),
                new KeyValuePair<string, string>("soleclassics", "Sole Classics"),
                new KeyValuePair<string, string>("solefly", "Solefly"),
                new KeyValuePair<string, string>("stashed", "STASHED"),
                new KeyValuePair<string, string>("subliminal", "Subliminal"),
                new KeyValuePair<string, string>("darkside", "The Darkside Initiative"),
                new KeyValuePair<string, string>("premierestore", "The Premiere Store"),
                new KeyValuePair<string, string>("travis", "Travis Scott"),
                new KeyValuePair<string, string>("trophyroomstore", "Trophy Room Store"),
                new KeyValuePair<string, string>("ultrafootball", "Ultra Football"),
                new KeyValuePair<string, string>("undefeated", "Undefeated"),
                new KeyValuePair<string, string>("unknwn", "Unknwn"),
                new KeyValuePair<string, string>("usg", "USG Store"),
                new KeyValuePair<string, string>("wishatl", "Wish Atlanta"),
                new KeyValuePair<string, string>("xhibition", "Xhibition"),
                new KeyValuePair<string, string>("yeezy", "YeezySupply"),
                new KeyValuePair<string, string>("other", "Other")
            };
            PREDEFINED_SHOPIFY = list1;
            List<ShopifyWebsiteInfo> list2 = new List<ShopifyWebsiteInfo>();
            ShopifyWebsiteInfo item = new ShopifyWebsiteInfo {
                Website = "kith",
                HomeLink = "https://kith.com",
                SearchLink = "https://kith.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Manual PayPal checkout only for international users; Captcha for most hyped products",
                ApiToken = "08430b96c47dd2ac8e17e305db3b71e8",
                OldMode = true
            };
            list2.Add(item);
            ShopifyWebsiteInfo info2 = new ShopifyWebsiteInfo {
                Website = "undefeated",
                HomeLink = "https://undefeated.com",
                SearchLink = "https://undefeated.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = true,
                SmartCheckout = true,
                AdditionalInfo = "Login required; Captcha for most hyped products; Use different login for the same product to prevent cancels",
                ApiToken = "a0faf54ad7ec6fbbab86cd3f949c3cb9"
            };
            list2.Add(info2);
            ShopifyWebsiteInfo info3 = new ShopifyWebsiteInfo {
                Website = "unknwn",
                HomeLink = "https://www.unknwn.com",
                SearchLink = "https://www.unknwn.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping only to 9 countries (Australia, Canada, France, Germany, Hong Kong, Japan, South Korea, USA, UK)",
                ApiToken = "5ba2ba340438b8d9aaca1a5ddb1ad0f5"
            };
            list2.Add(info3);
            ShopifyWebsiteInfo info4 = new ShopifyWebsiteInfo {
                Website = "deadstock",
                HomeLink = "https://www.deadstock.ca",
                SearchLink = "https://www.deadstock.ca/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha for most hyped products",
                ApiToken = "29c1e1054770e9694717256c270f4359"
            };
            list2.Add(info4);
            ShopifyWebsiteInfo info5 = new ShopifyWebsiteInfo {
                Website = "soleheaven",
                HomeLink = "https://soleheaven.com",
                SearchLink = "https://soleheaven.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha for most hyped products",
                ApiToken = "f66897bb7b81a7e1e39961a21b014629"
            };
            list2.Add(info5);
            ShopifyWebsiteInfo info6 = new ShopifyWebsiteInfo {
                Website = "shopnicekicks",
                HomeLink = "https://shopnicekicks.com",
                SearchLink = "https://shopnicekicks.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA only!",
                ApiToken = "9d6556dc3ee20bf6b1a0971ad22f8238"
            };
            list2.Add(info6);
            ShopifyWebsiteInfo info7 = new ShopifyWebsiteInfo {
                Website = "funko",
                HomeLink = "https://www.funko-shop.com",
                SearchLink = "https://www.funko-shop.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA only!; Captcha for most hyped products",
                ApiToken = "a861544cb572058d21ab033f62010615"
            };
            list2.Add(info7);
            ShopifyWebsiteInfo info8 = new ShopifyWebsiteInfo {
                Website = "xhibition",
                HomeLink = "https://www.xhibition.co",
                SearchLink = "https://www.xhibition.co/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha during checkout",
                ApiToken = "7f11f1a79cb6f8d07bca2f1d113177ef",
                ValidGateway = false
            };
            list2.Add(info8);
            ShopifyWebsiteInfo info9 = new ShopifyWebsiteInfo {
                Website = "yeezy",
                HomeLink = "https://yeezysupply.com",
                SearchLink = "-",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha during checkout; Password page before drops",
                ApiToken = "afa13d942580749aa2985b086cc0bdcb",
                OldMode = true
            };
            list2.Add(info9);
            ShopifyWebsiteInfo info10 = new ShopifyWebsiteInfo {
                Website = "packer",
                HomeLink = "https://packershoes.com",
                SearchLink = "https://packershoes.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha during checkout",
                ApiToken = "b26bacc7a6c3fb5ea949af1df91f3d1b"
            };
            list2.Add(info10);
            ShopifyWebsiteInfo info11 = new ShopifyWebsiteInfo {
                Website = "oipolloi",
                HomeLink = "https://www.oipolloi.com",
                SearchLink = "https://www.oipolloi.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "fc5579f3adf2889237757219239a7f40",
                ValidGateway = false
            };
            list2.Add(info11);
            ShopifyWebsiteInfo info12 = new ShopifyWebsiteInfo {
                Website = "palaceeu",
                HomeLink = "https://shop.palaceskateboards.com",
                SearchLink = "https://shop.palaceskateboards.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout; Europe shipping only",
                ApiToken = "c1369ef0027f554f66e555f2c1347048",
                ValidGateway = false
            };
            list2.Add(info12);
            ShopifyWebsiteInfo info13 = new ShopifyWebsiteInfo {
                Website = "palaceus",
                HomeLink = "https://shop-usa.palaceskateboards.com",
                SearchLink = "https://shop-usa.palaceskateboards.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout; US/Canada shipping only",
                ApiToken = "59571a4e503fc3164848c544a5fda777",
                ValidGateway = false
            };
            list2.Add(info13);
            ShopifyWebsiteInfo info14 = new ShopifyWebsiteInfo {
                Website = "palacejp",
                HomeLink = "https://shop-jp.palaceskateboards.com",
                SearchLink = "https://shop-jp.palaceskateboards.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "JP shipping only",
                ApiToken = "59571a4e503fc3164848c544a5fda777",
                ValidGateway = false
            };
            list2.Add(info14);
            ShopifyWebsiteInfo info15 = new ShopifyWebsiteInfo {
                Website = "eflasheu",
                HomeLink = "https://eflash.doverstreetmarket.com",
                SearchLink = "https://eflash.doverstreetmarket.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout; Europe shipping only",
                ApiToken = "0386ed516cfb121c71eee5a33b357cc6",
                ValidGateway = false,
                OldMode = true
            };
            list2.Add(info15);
            ShopifyWebsiteInfo info16 = new ShopifyWebsiteInfo {
                Website = "eflashus",
                HomeLink = "https://eflash-us.doverstreetmarket.com",
                SearchLink = "https://eflash-us.doverstreetmarket.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout; US/Canada shipping only",
                ApiToken = "c61dfd7121456f05d58cedf7a0b068b6",
                ValidGateway = false,
                OldMode = true
            };
            list2.Add(info16);
            ShopifyWebsiteInfo info17 = new ShopifyWebsiteInfo {
                Website = "eflashsg",
                HomeLink = "https://eflash-sg.doverstreetmarket.com",
                SearchLink = "https://eflash-sg.doverstreetmarket.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout; Asia shipping only",
                ApiToken = "0867cfd02e49786c54fd1a9fd02f03cd",
                ValidGateway = false,
                OldMode = true
            };
            list2.Add(info17);
            ShopifyWebsiteInfo info18 = new ShopifyWebsiteInfo {
                Website = "saintalfred",
                HomeLink = "https://www.saintalfred.com",
                SearchLink = "https://www.saintalfred.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "USA shipping only",
                ApiToken = "849af6fa065de14b021e119a4ad12bc0"
            };
            list2.Add(info18);
            ShopifyWebsiteInfo info19 = new ShopifyWebsiteInfo {
                Website = "socialstatus",
                HomeLink = "https://www.socialstatuspgh.com",
                SearchLink = "https://www.socialstatuspgh.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "a62a7cdb1689c171a551ac61e92dc08a",
                ValidGateway = false
            };
            list2.Add(info19);
            ShopifyWebsiteInfo info20 = new ShopifyWebsiteInfo {
                Website = "hanon",
                HomeLink = "https://www.hanon-shop.com",
                SearchLink = "https://www.hanon-shop.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Use US sizes for specific sizing (e.g. 8.5)",
                ApiToken = "f73913c594921bdd6a18a32bc2e2d7df"
            };
            list2.Add(info20);
            ShopifyWebsiteInfo info21 = new ShopifyWebsiteInfo {
                Website = "obey",
                HomeLink = "https://store.obeygiant.com",
                SearchLink = "https://store.obeygiant.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "2aaaddc9a967cab510a9f6e00dfba13c",
                ValidGateway = false
            };
            list2.Add(info21);
            ShopifyWebsiteInfo info22 = new ShopifyWebsiteInfo {
                Website = "trophyroomstore",
                HomeLink = "https://www.trophyroomstore.com",
                SearchLink = "https://www.trophyroomstore.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping only to few countries; Captcha on checkout",
                ApiToken = "1b124ca786ab5fc9065699f1a383a139",
                ValidGateway = false
            };
            list2.Add(info22);
            ShopifyWebsiteInfo info23 = new ShopifyWebsiteInfo {
                Website = "aboveclouds",
                HomeLink = "https://www.abovethecloudsstore.com",
                SearchLink = "https://www.abovethecloudsstore.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "f2be3597cd0ea22e67c89a3b5b5b22fd"
            };
            list2.Add(info23);
            ShopifyWebsiteInfo info24 = new ShopifyWebsiteInfo {
                Website = "shophny",
                HomeLink = "https://shophny.com",
                SearchLink = "https://shophny.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "f3eaf4a05d9a5476835fd1ca23b1845b"
            };
            list2.Add(info24);
            ShopifyWebsiteInfo info25 = new ShopifyWebsiteInfo {
                Website = "lapstoneandhammer",
                HomeLink = "https://www.lapstoneandhammer.com",
                SearchLink = "https://www.lapstoneandhammer.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping only within USA",
                ApiToken = "49b214ac5d8865c0b591436dc44be785"
            };
            list2.Add(info25);
            ShopifyWebsiteInfo info26 = new ShopifyWebsiteInfo {
                Website = "proper",
                HomeLink = "https://properlbc.com",
                SearchLink = "https://properlbc.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "5c6d8f621dc3cd0cc5f37886ea8aa34f",
                ValidGateway = false
            };
            list2.Add(info26);
            ShopifyWebsiteInfo info27 = new ShopifyWebsiteInfo {
                Website = "leaders1354",
                HomeLink = "https://www.leaders1354.com",
                SearchLink = "https://www.leaders1354.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA and Canada only",
                ApiToken = "3ade5885675b84375b59cd697aa164d9"
            };
            list2.Add(info27);
            ShopifyWebsiteInfo info28 = new ShopifyWebsiteInfo {
                Website = "xxx",
                HomeLink = "https://eveaiotest.myshopify.com",
                SearchLink = "https://eveaiotest.myshopify.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "ddac6ca1f8e770ec0ca606b5e7bc404c"
            };
            list2.Add(info28);
            ShopifyWebsiteInfo info29 = new ShopifyWebsiteInfo {
                Website = "capsuletoronto",
                HomeLink = "https://www.capsuletoronto.com",
                SearchLink = "https://www.capsuletoronto.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "748d6a9d626b2a10df7fd111523a3136"
            };
            list2.Add(info29);
            ShopifyWebsiteInfo info30 = new ShopifyWebsiteInfo {
                Website = "notre",
                HomeLink = "https://www.notre-shop.com",
                SearchLink = "https://www.notre-shop.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "2f85af10bb30c1b7a78259b6c1c49d53"
            };
            list2.Add(info30);
            ShopifyWebsiteInfo info31 = new ShopifyWebsiteInfo {
                Website = "renarts",
                HomeLink = "https://renarts.com",
                SearchLink = "https://renarts.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "fe288dcbb1f39f890295118d31cef77e"
            };
            list2.Add(info31);
            ShopifyWebsiteInfo info32 = new ShopifyWebsiteInfo {
                Website = "offthehook",
                HomeLink = "https://www.offthehook.ca",
                SearchLink = "https://www.offthehook.ca/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "2e0e6c2f045e1c04714f65b0a0b30b0c",
                ValidGateway = false
            };
            list2.Add(info32);
            ShopifyWebsiteInfo info33 = new ShopifyWebsiteInfo {
                Website = "solefly",
                HomeLink = "https://www.solefly.com",
                SearchLink = "https://www.solefly.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "PayPayl payment only",
                ApiToken = "b51037e17cfc5e142a20ef01f0b44751",
                PaypalOnly = true
            };
            list2.Add(info33);
            ShopifyWebsiteInfo info34 = new ShopifyWebsiteInfo {
                Website = "premierestore",
                HomeLink = "https://thepremierstore.com",
                SearchLink = "https://thepremierstore.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA only",
                ApiToken = "8c89a0773c5d8422ed2309eccd694464"
            };
            list2.Add(info34);
            ShopifyWebsiteInfo info35 = new ShopifyWebsiteInfo {
                Website = "creme321",
                HomeLink = "https://creme321.com",
                SearchLink = "https://creme321.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA only; PayPayl payment only",
                ApiToken = "195828e2ef14c58600012bc14a1f7081",
                PaypalOnly = true
            };
            list2.Add(info35);
            ShopifyWebsiteInfo info36 = new ShopifyWebsiteInfo {
                Website = "addictmiami",
                HomeLink = "https://www.addictmiami.com",
                SearchLink = "https://www.addictmiami.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA and Canada only; PayPayl payment only",
                ApiToken = "902bdaf4657c0001f57576ff2d7e9950",
                PaypalOnly = true
            };
            list2.Add(info36);
            ShopifyWebsiteInfo info37 = new ShopifyWebsiteInfo {
                Website = "commonwealth",
                HomeLink = "https://commonwealth-ftgg.com",
                SearchLink = "https://commonwealth-ftgg.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "9f57a59e7362969406227b0d77620ba9"
            };
            list2.Add(info37);
            ShopifyWebsiteInfo info38 = new ShopifyWebsiteInfo {
                Website = "abpstore",
                HomeLink = "https://www.apbstore.com",
                SearchLink = "https://www.apbstore.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "cf7dd82098a4388648acb12a5a9aa063"
            };
            list2.Add(info38);
            ShopifyWebsiteInfo info39 = new ShopifyWebsiteInfo {
                Website = "nrml",
                HomeLink = "https://nrml.ca",
                SearchLink = "https://nrml.ca/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "6f55f8c75577c1063d8489c94d9ab122",
                ValidGateway = false
            };
            list2.Add(info39);
            ShopifyWebsiteInfo info40 = new ShopifyWebsiteInfo {
                Website = "blendsus",
                HomeLink = "https://www.blendsus.com",
                SearchLink = "https://www.blendsus.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "a695a9ac76b89f50663628617f8498f6",
                ValidGateway = false
            };
            list2.Add(info40);
            ShopifyWebsiteInfo info41 = new ShopifyWebsiteInfo {
                Website = "18montrose",
                HomeLink = "https://18montrose.com",
                SearchLink = "https://18montrose.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "c59407c143b7e9ae989cfb4d693229e3"
            };
            list2.Add(info41);
            ShopifyWebsiteInfo info42 = new ShopifyWebsiteInfo {
                Website = "wishatl",
                HomeLink = "https://wishatl.com",
                SearchLink = "https://wishatl.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "7678df516143cdcb4c72168e8556b583"
            };
            list2.Add(info42);
            ShopifyWebsiteInfo info43 = new ShopifyWebsiteInfo {
                Website = "havenshop",
                HomeLink = "https://shop.havenshop.ca",
                SearchLink = "https://shop.havenshop.ca/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "b2d28cc6049fca1d79dbc390a0fef334",
                ValidGateway = false
            };
            list2.Add(info43);
            ShopifyWebsiteInfo info44 = new ShopifyWebsiteInfo {
                Website = "eflashjp",
                HomeLink = "https://eflash-jp.doverstreetmarket.com",
                SearchLink = "https://eflash-jp.doverstreetmarket.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout; Asia shipping only; Paypal only",
                ApiToken = "8cbefa53e1484d3a39a41396a00b9fe9",
                PaypalOnly = true
            };
            list2.Add(info44);
            ShopifyWebsiteInfo info45 = new ShopifyWebsiteInfo {
                Website = "sneakerpolitics",
                HomeLink = "https://sneakerpolitics.com",
                SearchLink = "https://sneakerpolitics.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "e7f4837ec5196326af7949de4b8381fe"
            };
            list2.Add(info45);
            ShopifyWebsiteInfo info46 = new ShopifyWebsiteInfo {
                Website = "a-ma-maniere",
                HomeLink = "https://www.a-ma-maniere.com",
                SearchLink = "https://www.a-ma-maniere.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "23348e90253d0a4895d5524b79914bbe"
            };
            list2.Add(info46);
            ShopifyWebsiteInfo info47 = new ShopifyWebsiteInfo {
                Website = "bodega",
                HomeLink = "https://shop.bdgastore.com",
                SearchLink = "https://shop.bdgastore.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = true,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "dbd316d5c797eb8e3caede9dd08f92ef"
            };
            list2.Add(info47);
            ShopifyWebsiteInfo info48 = new ShopifyWebsiteInfo {
                Website = "bbbranded",
                HomeLink = "https://www.bbbranded.com",
                SearchLink = "https://www.bbbranded.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA and Canada only",
                ApiToken = "84a4f0bae78edb9c62de04624fa11cc5"
            };
            list2.Add(info48);
            ShopifyWebsiteInfo info49 = new ShopifyWebsiteInfo {
                Website = "cncpts",
                HomeLink = "https://cncpts.com",
                SearchLink = "https://cncpts.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "41d0a419f6f4c191d52037d17a58e6df"
            };
            list2.Add(info49);
            ShopifyWebsiteInfo info50 = new ShopifyWebsiteInfo {
                Website = "closetinc",
                HomeLink = "https://www.theclosetinc.com",
                SearchLink = "https://www.theclosetinc.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "5e2769503be62e4bc6e746fb31678180"
            };
            list2.Add(info50);
            ShopifyWebsiteInfo info51 = new ShopifyWebsiteInfo {
                Website = "ellenshop",
                HomeLink = "https://www.ellenshop.com",
                SearchLink = "https://www.ellenshop.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "7ab71d2c78d37b30c90d7cdd32aa92e4"
            };
            list2.Add(info51);
            ShopifyWebsiteInfo info52 = new ShopifyWebsiteInfo {
                Website = "bimtoy",
                HomeLink = "https://bimtoy.com",
                SearchLink = "https://bimtoy.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "1ac64d0a282df93b476634869eb401f6"
            };
            list2.Add(info52);
            ShopifyWebsiteInfo info53 = new ShopifyWebsiteInfo {
                Website = "amongstfew",
                HomeLink = "https://www.amongstfew.com",
                SearchLink = "https://www.amongstfew.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "01140df5ea08d6c66f20446ab350acdc"
            };
            list2.Add(info53);
            ShopifyWebsiteInfo info54 = new ShopifyWebsiteInfo {
                Website = "mralans",
                HomeLink = "https://www.mralans.com",
                SearchLink = "https://www.mralans.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to US only",
                ApiToken = "fd537ef0bc0006527008e09c012b00a8"
            };
            list2.Add(info54);
            ShopifyWebsiteInfo info55 = new ShopifyWebsiteInfo {
                Website = "noirfonce",
                HomeLink = "https://noirfonce.eu",
                SearchLink = "https://noirfonce.eu/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "39aa752c77d4ac53e9daf9d49f61c0ad"
            };
            list2.Add(info55);
            ShopifyWebsiteInfo info56 = new ShopifyWebsiteInfo {
                Website = "noirfoncees",
                HomeLink = "https://noirfonce.es",
                SearchLink = "https://noirfonce.es/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "39aa752c77d4ac53e9daf9d49f61c0ad"
            };
            list2.Add(info56);
            ShopifyWebsiteInfo info57 = new ShopifyWebsiteInfo {
                Website = "noirfoncefr",
                HomeLink = "https://noirfonce.fr",
                SearchLink = "https://noirfonce.fr/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "39aa752c77d4ac53e9daf9d49f61c0ad"
            };
            list2.Add(info57);
            ShopifyWebsiteInfo info58 = new ShopifyWebsiteInfo {
                Website = "koolkidtoys",
                HomeLink = "https://www.koolkidtoys.com",
                SearchLink = "https://www.koolkidtoys.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "a7ca5a8d2c94e7bbd78ab4505ecb2598"
            };
            list2.Add(info58);
            ShopifyWebsiteInfo info59 = new ShopifyWebsiteInfo {
                Website = "munkyking",
                HomeLink = "https://munkyking.com",
                SearchLink = "https://munkyking.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "e17852d328e00d8b496e85e2d1e183ca"
            };
            list2.Add(info59);
            ShopifyWebsiteInfo info60 = new ShopifyWebsiteInfo {
                Website = "awakenyclothing",
                HomeLink = "https://awakenyclothing.com",
                SearchLink = "https://awakenyclothing.com/products.json",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "9c1b1bef7f8efdd41994a8c531165b77"
            };
            list2.Add(info60);
            ShopifyWebsiteInfo info61 = new ShopifyWebsiteInfo {
                Website = "rsvpgallery",
                HomeLink = "https://rsvpgallery.com",
                SearchLink = "https://rsvpgallery.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "68861eece3d19735546a05faf429e759"
            };
            list2.Add(info61);
            ShopifyWebsiteInfo info62 = new ShopifyWebsiteInfo {
                Website = "fuct",
                HomeLink = "https://fuct.com",
                SearchLink = "https://fuct.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "a379dadba8659d806698dd1498d62438"
            };
            list2.Add(info62);
            ShopifyWebsiteInfo info63 = new ShopifyWebsiteInfo {
                Website = "ficegallery",
                HomeLink = "https://www.ficegallery.com",
                SearchLink = "https://www.ficegallery.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Shipping to USA only",
                ApiToken = "1b5be89fa110de5f11243a85cce0e119"
            };
            list2.Add(info63);
            ShopifyWebsiteInfo info64 = new ShopifyWebsiteInfo {
                Website = "pilgrim",
                HomeLink = "https://pilgrimsurfsupply.com",
                SearchLink = "https://pilgrimsurfsupply.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "853df09c87b0cac39e6bc3f85a979a5b"
            };
            list2.Add(info64);
            ShopifyWebsiteInfo info65 = new ShopifyWebsiteInfo {
                Website = "kylie",
                HomeLink = "https://www.kyliecosmetics.com",
                SearchLink = "https://www.kyliecosmetics.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "d3ec1d3b053a5239a379cb3f1d99cf90"
            };
            list2.Add(info65);
            ShopifyWebsiteInfo info66 = new ShopifyWebsiteInfo {
                Website = "assc",
                HomeLink = "https://shop.antisocialsocialclub.com/",
                SearchLink = "https://shop.antisocialsocialclub.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "4189262a3c6b7c3fea963c5602c51182"
            };
            list2.Add(info66);
            ShopifyWebsiteInfo info67 = new ShopifyWebsiteInfo {
                Website = "darkside",
                HomeLink = "https://www.thedarksideinitiative.com",
                SearchLink = "https://www.thedarksideinitiative.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "3332f96a11efce8997d46bc4962c9737"
            };
            list2.Add(info67);
            ShopifyWebsiteInfo info68 = new ShopifyWebsiteInfo {
                Website = "usg",
                HomeLink = "https://usgstore.com.au",
                SearchLink = "https://usgstore.com.au/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "735d8bb4e833bde11ade9f9acce58971"
            };
            list2.Add(info68);
            ShopifyWebsiteInfo info69 = new ShopifyWebsiteInfo {
                Website = "bapeus",
                HomeLink = "https://us.bape.com",
                SearchLink = "https://us.bape.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "USA / Canada shipping only; captcha on checkout",
                ApiToken = "a45de9ba54947c6286e329042853dafc",
                Color = true
            };
            list2.Add(info69);
            ShopifyWebsiteInfo info70 = new ShopifyWebsiteInfo {
                Website = "subliminal",
                HomeLink = "https://store.subliminalprojects.com",
                SearchLink = "https://store.subliminalprojects.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "fc7accffa7a8ec573da904c59eab8279"
            };
            list2.Add(info70);
            ShopifyWebsiteInfo info71 = new ShopifyWebsiteInfo {
                Website = "shoegallerymiami",
                HomeLink = "https://shoegallerymiami.com",
                SearchLink = "https://shoegallerymiami.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "4b29eb8c96c04b163ae2fac313043717"
            };
            list2.Add(info71);
            ShopifyWebsiteInfo info72 = new ShopifyWebsiteInfo {
                Website = "travis",
                HomeLink = "https://shop.travisscott.com",
                SearchLink = "https://shop.travisscott.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "8eadffbda47bb8f4167fbf8a086590e0"
            };
            list2.Add(info72);
            ShopifyWebsiteInfo info73 = new ShopifyWebsiteInfo {
                Website = "laceup",
                HomeLink = "https://laceupnyc.com",
                SearchLink = "https://laceupnyc.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "299490a7a803c9be7370c4805e897bca"
            };
            list2.Add(info73);
            ShopifyWebsiteInfo info74 = new ShopifyWebsiteInfo {
                Website = "soleclassics",
                HomeLink = "https://soleclassics.com",
                SearchLink = "https://soleclassics.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "6d0fe0a3b2f6e2da974f14a7a0249d55"
            };
            list2.Add(info74);
            ShopifyWebsiteInfo info75 = new ShopifyWebsiteInfo {
                Website = "sneakerjunkies",
                HomeLink = "https://sneakerjunkiesusa.com",
                SearchLink = "https://sneakerjunkiesusa.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "09df51172c0eb9157079f4c50a064435"
            };
            list2.Add(info75);
            ShopifyWebsiteInfo info76 = new ShopifyWebsiteInfo {
                Website = "stashed",
                HomeLink = "https://stashedsf.com",
                SearchLink = "https://stashedsf.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "Captcha on checkout",
                ApiToken = "81fea2835f74adda733f705386e22b28"
            };
            list2.Add(info76);
            ShopifyWebsiteInfo info77 = new ShopifyWebsiteInfo {
                Website = "machus",
                HomeLink = "https://www.machusonline.com",
                SearchLink = "https://www.machusonline.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "85ef1c2852f958c66504c5bf1768c729"
            };
            list2.Add(info77);
            ShopifyWebsiteInfo info78 = new ShopifyWebsiteInfo {
                Website = "mondo",
                HomeLink = "https://mondotees.com",
                SearchLink = "https://mondotees.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "b859f8599f173a8df53ebde2bb89828a"
            };
            list2.Add(info78);
            ShopifyWebsiteInfo info79 = new ShopifyWebsiteInfo {
                Website = "likelihood",
                HomeLink = "https://likelihood.us",
                SearchLink = "https://likelihood.us/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "ae488d768336f38126f7d163c8738900"
            };
            list2.Add(info79);
            ShopifyWebsiteInfo info80 = new ShopifyWebsiteInfo {
                Website = "ultrafootball",
                HomeLink = "https://www.ultrafootball.com/",
                SearchLink = "https://www.ultrafootball.com/sitemap_products_1.xml",
                AtcLinks = true,
                CheckoutLinks = true,
                Login = false,
                SmartCheckout = true,
                AdditionalInfo = "-",
                ApiToken = "8dea3a80950c8a893de627fac43d68f6"
            };
            list2.Add(info80);
            SHOPIFY_WEBS = list2;
            SUPREME_CAPTCHA_KEY = "6LeWwRkUAAAAAOBsau7KpuC9AV-6J8mhw4AjC3Xz";
            SNS_CAPTCHA_KEY = "6LfBixYUAAAAABhdHynFUIMA_sa4s-XsJvnjtgB0";
            SHOPIFY_CAPTCHA_KEY = "6LeoeSkTAAAAAA9rkZs5oS82l69OEYjKRZAiKdaF";
            MR_PORTER_CAPTCHA_KEY = "6LcOaQcTAAAAAKQSyDMzErO51xmekwZN4wwWP-Sb";
            SIVAS_CAPTCHA_KEY = "6LcuURUTAAAAAK_b8wWvbNLY0awdFT27EJYcx-M1";
            HIBBET_CAPTCHA_KEY = "6Lcj-R8TAAAAABs3FrRPuQhLMbp5QrHsHufzLf7b";
            HOLYPOP_CAPTCHA_KEY = "6LcuMRgUAAAAAJPg74N3RzDgXYnm3RPyLkbRMB3P";
            OW_CAPTCHA_KEY = "6LfBixYUAAAAABhdHynFUIMA_sa4s-XsJvnjtgB0";
            FOOTACTION_CAPTCHA_KEY = "6Lf9IxgUAAAAAHHz804d0SNSePsBEY7ZYsBALSHT";
            PRIVACY_CAPTCHA_KEY = "6LcrpQ0UAAAAAISIzEbWTqNyRV7mrknUQM1wg9QH";
            List<KeyValuePair<string, string>> list3 = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("8f241f11095306451.36998225", "Afghanistan"),
                new KeyValuePair<string, string>("a7c40f632a0804ab5.18804099", "\x00c5land Islands"),
                new KeyValuePair<string, string>("8f241f110953265a5.25286134", "Albania"),
                new KeyValuePair<string, string>("8f241f1109533b943.50287900", "Algeria"),
                new KeyValuePair<string, string>("8f241f1109534f8c7.80349931", "American Samoa"),
                new KeyValuePair<string, string>("8f241f11095363464.89657222", "Andorra"),
                new KeyValuePair<string, string>("8f241f11095377d33.28678901", "Angola"),
                new KeyValuePair<string, string>("8f241f11095392e41.74397491", "Anguilla"),
                new KeyValuePair<string, string>("8f241f110953a8d10.29474848", "Antarctica"),
                new KeyValuePair<string, string>("8f241f110953be8f2.56248134", "Antigua and Barbuda"),
                new KeyValuePair<string, string>("8f241f110953d2fb0.54260547", "Argentina"),
                new KeyValuePair<string, string>("8f241f110953e7993.88180360", "Armenia"),
                new KeyValuePair<string, string>("8f241f110953facc6.31621036", "Aruba"),
                new KeyValuePair<string, string>("8f241f11095410f38.37165361", "Australia"),
                new KeyValuePair<string, string>("a7c40f6320aeb2ec2.72885259", "Austria"),
                new KeyValuePair<string, string>("8f241f1109543cf47.17877015", "Azerbaijan"),
                new KeyValuePair<string, string>("8f241f11095451379.72078871", "Bahamas"),
                new KeyValuePair<string, string>("8f241f110954662e3.27051654", "Bahrain"),
                new KeyValuePair<string, string>("8f241f1109547ae49.60154431", "Bangladesh"),
                new KeyValuePair<string, string>("8f241f11095497083.21181725", "Barbados"),
                new KeyValuePair<string, string>("8f241f110954ac5b9.63105203", "Belarus"),
                new KeyValuePair<string, string>("a7c40f632e04633c9.47194042", "Belgium"),
                new KeyValuePair<string, string>("8f241f110954d3621.45362515", "Belize"),
                new KeyValuePair<string, string>("8f241f110954ea065.41455848", "Benin"),
                new KeyValuePair<string, string>("8f241f110954fee13.50011948", "Bermuda"),
                new KeyValuePair<string, string>("8f241f11095513ca0.75349731", "Bhutan"),
                new KeyValuePair<string, string>("8f241f1109552aee2.91004965", "Bolivia"),
                new KeyValuePair<string, string>("8f241f1109553f902.06960438", "Bosnia and Herzegovina"),
                new KeyValuePair<string, string>("8f241f11095554834.54199483", "Botswana"),
                new KeyValuePair<string, string>("8f241f1109556dd57.84292282", "Bouvet Island"),
                new KeyValuePair<string, string>("8f241f11095592407.89986143", "Brazil"),
                new KeyValuePair<string, string>("8f241f110955a7644.68859180", "British Indian Ocean Territory"),
                new KeyValuePair<string, string>("8f241f110955bde61.63256042", "Brunei Darussalam"),
                new KeyValuePair<string, string>("8f241f110955d3260.55487539", "Bulgaria"),
                new KeyValuePair<string, string>("8f241f110955ea7c8.36762654", "Burkina Faso"),
                new KeyValuePair<string, string>("8f241f110956004d5.11534182", "Burundi"),
                new KeyValuePair<string, string>("8f241f110956175f9.81682035", "Cambodia"),
                new KeyValuePair<string, string>("8f241f11095632828.20263574", "Cameroon"),
                new KeyValuePair<string, string>("8f241f11095649d18.02676059", "Canada"),
                new KeyValuePair<string, string>("8f241f1109565e671.48876354", "Cape Verde"),
                new KeyValuePair<string, string>("8f241f11095673248.50405852", "Cayman Islands"),
                new KeyValuePair<string, string>("8f241f1109568a509.03566030", "Central African Republic"),
                new KeyValuePair<string, string>("8f241f1109569d4c2.42800039", "Chad"),
                new KeyValuePair<string, string>("8f241f110956b3ea7.11168270", "Chile"),
                new KeyValuePair<string, string>("8f241f110956c8860.37981845", "China"),
                new KeyValuePair<string, string>("8f241f110956df6b2.52283428", "Christmas Island"),
                new KeyValuePair<string, string>("8f241f110956f54b4.26327849", "Cocos (Keeling) Islands"),
                new KeyValuePair<string, string>("8f241f1109570a1e3.69772638", "Colombia"),
                new KeyValuePair<string, string>("8f241f1109571f018.46251535", "Comoros"),
                new KeyValuePair<string, string>("8f241f11095732184.72771986", "Congo"),
                new KeyValuePair<string, string>("8f241f1109575d708.20084199", "Congo, The Democratic Republic Of The"),
                new KeyValuePair<string, string>("8f241f11095746a92.94878441", "Cook Islands"),
                new KeyValuePair<string, string>("8f241f1109575d708.20084150", "Costa Rica"),
                new KeyValuePair<string, string>("8f241f11095771f76.87904122", "Cote d'Ivoire"),
                new KeyValuePair<string, string>("8f241f11095789a04.65154246", "Croatia"),
                new KeyValuePair<string, string>("8f241f1109579ef49.91803242", "Cuba"),
                new KeyValuePair<string, string>("8f241f110957b6896.52725150", "Cyprus"),
                new KeyValuePair<string, string>("8f241f110957cb457.97820918", "Czech Republic"),
                new KeyValuePair<string, string>("8f241f110957e6ef8.56458418", "Denmark"),
                new KeyValuePair<string, string>("8f241f110957fd356.02918645", "Djibouti"),
                new KeyValuePair<string, string>("8f241f11095811ea5.84717844", "Dominica"),
                new KeyValuePair<string, string>("8f241f11095825bf2.61063355", "Dominican Republic"),
                new KeyValuePair<string, string>("8f241f1109584d512.06663789", "Ecuador"),
                new KeyValuePair<string, string>("8f241f11095861fb7.55278256", "Egypt"),
                new KeyValuePair<string, string>("8f241f110958736a9.06061237", "El Salvador"),
                new KeyValuePair<string, string>("8f241f1109588d077.74284490", "Equatorial Guinea"),
                new KeyValuePair<string, string>("8f241f110958a2216.38324531", "Eritrea"),
                new KeyValuePair<string, string>("8f241f110958b69e4.93886171", "Estonia"),
                new KeyValuePair<string, string>("8f241f110958caf67.08982313", "Ethiopia"),
                new KeyValuePair<string, string>("8f241f110958e2cc3.90770249", "Falkland Islands (Malvinas)"),
                new KeyValuePair<string, string>("8f241f110958f7ba4.96908065", "Faroe Islands"),
                new KeyValuePair<string, string>("8f241f1109590d226.07938729", "Fiji"),
                new KeyValuePair<string, string>("a7c40f63293c19d65.37472814", "Finland"),
                new KeyValuePair<string, string>("a7c40f63272a57296.32117580", "France"),
                new KeyValuePair<string, string>("8f241f1109594fcb1.79441780", "French Guiana"),
                new KeyValuePair<string, string>("8f241f110959636f5.71476354", "French Polynesia"),
                new KeyValuePair<string, string>("8f241f110959784a3.34264829", "French Southern Territories"),
                new KeyValuePair<string, string>("8f241f11095994cb6.59353392", "Gabon"),
                new KeyValuePair<string, string>("8f241f110959ace77.17379319", "Gambia"),
                new KeyValuePair<string, string>("8f241f110959c2341.01830199", "Georgia"),
                new KeyValuePair<string, string>("a7c40f631fc920687.20179984", "Germany"),
                new KeyValuePair<string, string>("8f241f110959e96b3.05752152", "Ghana"),
                new KeyValuePair<string, string>("8f241f110959fdde0.68919405", "Gibraltar"),
                new KeyValuePair<string, string>("a7c40f633114e8fc6.25257477", "Greece"),
                new KeyValuePair<string, string>("8f241f11095a29f47.04102343", "Greenland"),
                new KeyValuePair<string, string>("8f241f11095a3f195.88886789", "Grenada"),
                new KeyValuePair<string, string>("8f241f11095a52578.45413493", "Guadeloupe"),
                new KeyValuePair<string, string>("8f241f11095a717b3.68126681", "Guam"),
                new KeyValuePair<string, string>("8f241f11095a870a5.42235635", "Guatemala"),
                new KeyValuePair<string, string>("56d308a822c18e106.3ba59099", "Guernsey"),
                new KeyValuePair<string, string>("8f241f11095a9bf82.19989557", "Guinea"),
                new KeyValuePair<string, string>("8f241f11095ab2b56.83049280", "Guinea-Bissau"),
                new KeyValuePair<string, string>("8f241f11095ac9d30.56640429", "Guyana"),
                new KeyValuePair<string, string>("8f241f11095aebb06.34405179", "Haiti"),
                new KeyValuePair<string, string>("8f241f11095aff2c3.98054755", "Heard Island And Mcdonald Islands"),
                new KeyValuePair<string, string>("8f241f110968ebc30.63792746", "Holy See (Vatican City State)"),
                new KeyValuePair<string, string>("8f241f11095b13f57.56022305", "Honduras"),
                new KeyValuePair<string, string>("8f241f11095b29021.49657118", "Hong Kong"),
                new KeyValuePair<string, string>("8f241f11095b3e016.98213173", "Hungary"),
                new KeyValuePair<string, string>("8f241f11095b55846.26192602", "Iceland"),
                new KeyValuePair<string, string>("8f241f11095b6bb86.01364904", "India"),
                new KeyValuePair<string, string>("8f241f11095b94476.05195832", "Iran"),
                new KeyValuePair<string, string>("8f241f11095bad5b2.42645724", "Iraq"),
                new KeyValuePair<string, string>("a7c40f632be4237c2.48517912", "Ireland"),
                new KeyValuePair<string, string>("8f241f11096982354.73448999", "Isle Of Man"),
                new KeyValuePair<string, string>("8f241f11095bd65e1.59459683", "Israel"),
                new KeyValuePair<string, string>("a7c40f6323c4bfb36.59919433", "Italy"),
                new KeyValuePair<string, string>("8f241f11095bfe834.63390185", "Jamaica"),
                new KeyValuePair<string, string>("8f241f11095c11d43.73419747", "Japan"),
                new KeyValuePair<string, string>("8f241f11096944468.61956599", "Jersey"),
                new KeyValuePair<string, string>("8f241f11095c2b304.75906962", "Jordan"),
                new KeyValuePair<string, string>("8f241f11095c3e2d1.36714463", "Kazakhstan"),
                new KeyValuePair<string, string>("8f241f11095c5b8e8.66333679", "Kenya"),
                new KeyValuePair<string, string>("8f241f11095c6e184.21450618", "Kiribati"),
                new KeyValuePair<string, string>("8f241f11095cb1546.46652174", "Kuwait"),
                new KeyValuePair<string, string>("8f241f11095cc7ef5.28043767", "Kyrgyzstan"),
                new KeyValuePair<string, string>("8f241f11095cdccd5.96388808", "Laos"),
                new KeyValuePair<string, string>("8f241f11095cf2ea6.73925511", "Latvia"),
                new KeyValuePair<string, string>("8f241f11095d07d87.58986129", "Lebanon"),
                new KeyValuePair<string, string>("8f241f11095d1c9b2.21548132", "Lesotho"),
                new KeyValuePair<string, string>("8f241f11095d2fd28.91858908", "Liberia"),
                new KeyValuePair<string, string>("8f241f11095d46188.64679605", "Libya"),
                new KeyValuePair<string, string>("a7c40f6322d842ae3.83331920", "Liechtenstein"),
                new KeyValuePair<string, string>("8f241f11095d6ffa8.86593236", "Lithuania"),
                new KeyValuePair<string, string>("a7c40f63264309e05.58576680", "Luxembourg"),
                new KeyValuePair<string, string>("8f241f11095d9c1b2.13577033", "Macao"),
                new KeyValuePair<string, string>("8f241f11095db2291.58912887", "Macedonia"),
                new KeyValuePair<string, string>("8f241f11095dccf17.06266806", "Madagascar"),
                new KeyValuePair<string, string>("8f241f11095de2119.60795833", "Malawi"),
                new KeyValuePair<string, string>("8f241f11095df78a8.44559506", "Malaysia"),
                new KeyValuePair<string, string>("8f241f11095e0c6c9.43746477", "Maldives"),
                new KeyValuePair<string, string>("8f241f11095e24006.17141715", "Mali"),
                new KeyValuePair<string, string>("8f241f11095e36eb3.69050509", "Malta"),
                new KeyValuePair<string, string>("8f241f11095e4e338.26817244", "Marshall Islands"),
                new KeyValuePair<string, string>("8f241f11095e631e1.29476484", "Martinique"),
                new KeyValuePair<string, string>("8f241f11095e7bff9.09518271", "Mauritania"),
                new KeyValuePair<string, string>("8f241f11095e90a81.01156393", "Mauritius"),
                new KeyValuePair<string, string>("8f241f11095ea6249.81474246", "Mayotte"),
                new KeyValuePair<string, string>("8f241f11095ebf3a6.86388577", "Mexico"),
                new KeyValuePair<string, string>("8f241f11095ed4902.49276197", "Micronesia, Federated States Of"),
                new KeyValuePair<string, string>("8f241f11095ee9923.85175653", "Moldova"),
                new KeyValuePair<string, string>("8f241f11095f00d65.30318330", "Monaco"),
                new KeyValuePair<string, string>("8f241f11095f160c9.41059441", "Mongolia"),
                new KeyValuePair<string, string>("56d308a822c18e106.3ba59048", "Montenegro"),
                new KeyValuePair<string, string>("8f241f11095f314f5.05830324", "Montserrat"),
                new KeyValuePair<string, string>("8f241f11096006828.49285591", "Morocco"),
                new KeyValuePair<string, string>("8f241f1109601b419.55269691", "Mozambique"),
                new KeyValuePair<string, string>("8f241f11096030af5.65449043", "Myanmar"),
                new KeyValuePair<string, string>("8f241f11096046575.31382060", "Namibia"),
                new KeyValuePair<string, string>("8f241f1109605b1f4.20574895", "Nauru"),
                new KeyValuePair<string, string>("8f241f1109607a9e7.03486450", "Nepal"),
                new KeyValuePair<string, string>("a7c40f632cdd63c52.64272623", "Netherlands"),
                new KeyValuePair<string, string>("8f241f110960aeb64.09757010", "Netherlands Antilles"),
                new KeyValuePair<string, string>("8f241f110960c3e97.21901471", "New Caledonia"),
                new KeyValuePair<string, string>("8f241f110960d8e58.96466103", "New Zealand"),
                new KeyValuePair<string, string>("8f241f110960ec345.71805056", "Nicaragua"),
                new KeyValuePair<string, string>("8f241f11096101a79.70513227", "Niger"),
                new KeyValuePair<string, string>("8f241f11096116744.92008092", "Nigeria"),
                new KeyValuePair<string, string>("8f241f1109612dc68.63806992", "Niue"),
                new KeyValuePair<string, string>("8f241f110961442c2.82573898", "Norfolk Island"),
                new KeyValuePair<string, string>("8f241f11096162678.71164081", "Northern Mariana Islands"),
                new KeyValuePair<string, string>("8f241f11096176795.61257067", "Norway"),
                new KeyValuePair<string, string>("8f241f1109618d825.87661926", "Oman"),
                new KeyValuePair<string, string>("8f241f110961a2401.59039740", "Pakistan"),
                new KeyValuePair<string, string>("8f241f110961b7729.14290490", "Palau"),
                new KeyValuePair<string, string>("8f241f110968ebc30.63792799", "Palestinian Territory, Occupied"),
                new KeyValuePair<string, string>("8f241f110961cc384.18166560", "Panama"),
                new KeyValuePair<string, string>("8f241f110961e3538.78435307", "Papua New Guinea"),
                new KeyValuePair<string, string>("8f241f110961f9d61.52794273", "Paraguay"),
                new KeyValuePair<string, string>("8f241f1109620b245.16261506", "Peru"),
                new KeyValuePair<string, string>("8f241f1109621faf8.40135556", "Philippines"),
                new KeyValuePair<string, string>("8f241f11096234d62.44125992", "Pitcairn"),
                new KeyValuePair<string, string>("8f241f1109624d3f8.50953605", "Poland"),
                new KeyValuePair<string, string>("a7c40f632f65bd8e2.84963272", "Portugal"),
                new KeyValuePair<string, string>("8f241f11096279a22.50582479", "Puerto Rico"),
                new KeyValuePair<string, string>("8f241f1109628f903.51478291", "Qatar"),
                new KeyValuePair<string, string>("8f241f110962a3ec5.65857240", "R\x00e9union"),
                new KeyValuePair<string, string>("8f241f110962c3007.60363573", "Romania"),
                new KeyValuePair<string, string>("8f241f110962f8615.93666560", "Rwanda"),
                new KeyValuePair<string, string>("8f241f110968a7cc9.56710199", "Saint Barth\x00e9lemy"),
                new KeyValuePair<string, string>("8f241f1109654dca4.99466434", "Saint Helena"),
                new KeyValuePair<string, string>("8f241f110963177a7.49289900", "Saint Kitts and Nevis"),
                new KeyValuePair<string, string>("8f241f1109632fab4.68646740", "Saint Lucia"),
                new KeyValuePair<string, string>("a7c40f632f65bd8e2.84963299", "Saint Martin"),
                new KeyValuePair<string, string>("8f241f1109656cde9.10816078", "Saint Pierre and Miquelon"),
                new KeyValuePair<string, string>("8f241f110963443c3.29598809", "Saint Vincent and The Grenadines"),
                new KeyValuePair<string, string>("8f241f11096359986.06476221", "Samoa"),
                new KeyValuePair<string, string>("8f241f11096375757.44126946", "San Marino"),
                new KeyValuePair<string, string>("8f241f1109639b8c4.57484984", "Sao Tome and Principe"),
                new KeyValuePair<string, string>("8f241f110963b9b20.41500709", "Saudi Arabia"),
                new KeyValuePair<string, string>("8f241f110963d9962.36307144", "Senegal"),
                new KeyValuePair<string, string>("8f241f110963f98d8.68428379", "Serbia"),
                new KeyValuePair<string, string>("8f241f11096418496.77253079", "Seychelles"),
                new KeyValuePair<string, string>("8f241f11096436968.69551351", "Sierra Leone"),
                new KeyValuePair<string, string>("8f241f11096456a48.79608805", "Singapore"),
                new KeyValuePair<string, string>("8f241f1109647a265.29938154", "Slovakia"),
                new KeyValuePair<string, string>("8f241f11096497149.85116254", "Slovenia"),
                new KeyValuePair<string, string>("8f241f110964b7bf9.49501835", "Solomon Islands"),
                new KeyValuePair<string, string>("8f241f110964d5f29.11398308", "Somalia"),
                new KeyValuePair<string, string>("8f241f110964f2623.74976876", "South Africa"),
                new KeyValuePair<string, string>("8f241f1109533b943.50287999", "South Georgia and The South Sandwich Islands"),
                new KeyValuePair<string, string>("8f241f11095c9de64.01275726", "South Korea"),
                new KeyValuePair<string, string>("a7c40f633038cd578.22975442", "Spain"),
                new KeyValuePair<string, string>("8f241f11096531330.03198083", "Sri Lanka"),
                new KeyValuePair<string, string>("8f241f1109658cbe5.08293991", "Sudan"),
                new KeyValuePair<string, string>("8f241f110965c7347.75108681", "Suriname"),
                new KeyValuePair<string, string>("8f241f110965eb7b7.26149742", "Svalbard and Jan Mayen"),
                new KeyValuePair<string, string>("8f241f1109660c113.62780718", "Swaziland"),
                new KeyValuePair<string, string>("a7c40f632848c5217.53322339", "Sweden"),
                new KeyValuePair<string, string>("a7c40f6321c6f6109.43859248", "Switzerland"),
                new KeyValuePair<string, string>("8f241f1109666b7f3.81435898", "Syria"),
                new KeyValuePair<string, string>("8f241f11096687ec7.58824735", "Taiwan, Province of China"),
                new KeyValuePair<string, string>("8f241f110966a54d1.43798997", "Tajikistan"),
                new KeyValuePair<string, string>("8f241f110966c3a75.68297960", "Tanzania"),
                new KeyValuePair<string, string>("8f241f11096707e08.60512709", "Thailand"),
                new KeyValuePair<string, string>("8f241f11095839323.86755169", "Timor-Leste"),
                new KeyValuePair<string, string>("8f241f110967241e1.34925220", "Togo"),
                new KeyValuePair<string, string>("8f241f11096742565.72138875", "Tokelau"),
                new KeyValuePair<string, string>("8f241f11096762b31.03069244", "Tonga"),
                new KeyValuePair<string, string>("8f241f1109677ed23.84886671", "Trinidad and Tobago"),
                new KeyValuePair<string, string>("8f241f1109679d988.46004322", "Tunisia"),
                new KeyValuePair<string, string>("8f241f110967d8f65.52699796", "Turkmenistan"),
                new KeyValuePair<string, string>("8f241f110967f73f8.13141492", "Turks and Caicos Islands"),
                new KeyValuePair<string, string>("8f241f1109680ec30.97426963", "Tuvalu"),
                new KeyValuePair<string, string>("8f241f11096823019.47846368", "Uganda"),
                new KeyValuePair<string, string>("8f241f110968391d2.37199812", "Ukraine"),
                new KeyValuePair<string, string>("8f241f1109684bf15.63071279", "United Arab Emirates"),
                new KeyValuePair<string, string>("a7c40f632a0804ab5.18804076", "United Kingdom"),
                new KeyValuePair<string, string>("8f241f11096877ac0.98748826", "United States"),
                new KeyValuePair<string, string>("8f241f110968a7cc9.56710143", "Uruguay"),
                new KeyValuePair<string, string>("8f241f110968bec45.44161857", "Uzbekistan"),
                new KeyValuePair<string, string>("8f241f110968d3f03.13630334", "Vanuatu"),
                new KeyValuePair<string, string>("8f241f11096902d92.14742486", "Venezuela"),
                new KeyValuePair<string, string>("8f241f11096919d00.92534927", "Viet Nam"),
                new KeyValuePair<string, string>("8f241f1109692fc04.15216034", "Virgin Islands, British"),
                new KeyValuePair<string, string>("8f241f11096944468.61956573", "Virgin Islands, U.S."),
                new KeyValuePair<string, string>("8f241f110969598c8.76966113", "Wallis and Futuna"),
                new KeyValuePair<string, string>("8f241f1109696e4e9.33006292", "Western Sahara"),
                new KeyValuePair<string, string>("8f241f11096982354.73448958", "Yemen"),
                new KeyValuePair<string, string>("8f241f110969c34a2.42564730", "Zambia"),
                new KeyValuePair<string, string>("8f241f110969da699.04185888", "Zimbabwe")
            };
            SOLEBOX_COUNTRIES = list3;
            List<KeyValuePair<string, string>> list4 = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("3", "Afghanistan"),
                new KeyValuePair<string, string>("6", "Albania"),
                new KeyValuePair<string, string>("62", "Algeria"),
                new KeyValuePair<string, string>("8", "Angola"),
                new KeyValuePair<string, string>("4", "Antigua and Barbuda"),
                new KeyValuePair<string, string>("10", "Argentina"),
                new KeyValuePair<string, string>("7", "Armenia"),
                new KeyValuePair<string, string>("14", "Aruba"),
                new KeyValuePair<string, string>("13", "Australia"),
                new KeyValuePair<string, string>("12", "Austria"),
                new KeyValuePair<string, string>("16", "Azerbaijan"),
                new KeyValuePair<string, string>("32", "Bahamas"),
                new KeyValuePair<string, string>("19", "Bangladesh"),
                new KeyValuePair<string, string>("18", "Barbados"),
                new KeyValuePair<string, string>("36", "Belarus"),
                new KeyValuePair<string, string>("20", "Belgium"),
                new KeyValuePair<string, string>("25", "Benin"),
                new KeyValuePair<string, string>("27", "Bermuda"),
                new KeyValuePair<string, string>("29", "Bolivia"),
                new KeyValuePair<string, string>("17", "Bosnia and Herzegovina"),
                new KeyValuePair<string, string>("31", "Brazil"),
                new KeyValuePair<string, string>("28", "Brunei Darussalam"),
                new KeyValuePair<string, string>("22", "Bulgaria"),
                new KeyValuePair<string, string>("21", "Burkina Faso"),
                new KeyValuePair<string, string>("24", "Burundi"),
                new KeyValuePair<string, string>("117", "Cambodia"),
                new KeyValuePair<string, string>("47", "Cameroon"),
                new KeyValuePair<string, string>("38", "Canada"),
                new KeyValuePair<string, string>("52", "Cape Verde"),
                new KeyValuePair<string, string>("124", "Cayman Islands"),
                new KeyValuePair<string, string>("41", "Central African Republic"),
                new KeyValuePair<string, string>("215", "Chad"),
                new KeyValuePair<string, string>("46", "Chile"),
                new KeyValuePair<string, string>("48", "China"),
                new KeyValuePair<string, string>("49", "Colombia"),
                new KeyValuePair<string, string>("119", "Comoros"),
                new KeyValuePair<string, string>("40", "Congo"),
                new KeyValuePair<string, string>("42", "Congo"),
                new KeyValuePair<string, string>("50", "Costa Rica"),
                new KeyValuePair<string, string>("44", "Cote d'Ivoire"),
                new KeyValuePair<string, string>("98", "Croatia"),
                new KeyValuePair<string, string>("51", "Cuba"),
                new KeyValuePair<string, string>("55", "Cyprus"),
                new KeyValuePair<string, string>("56", "Czech Republic"),
                new KeyValuePair<string, string>("59", "Denmark"),
                new KeyValuePair<string, string>("60", "Dominica"),
                new KeyValuePair<string, string>("61", "Dominican Republic"),
                new KeyValuePair<string, string>("63", "Ecuador"),
                new KeyValuePair<string, string>("65", "Egypt"),
                new KeyValuePair<string, string>("210", "El Salvador"),
                new KeyValuePair<string, string>("67", "Eritrea"),
                new KeyValuePair<string, string>("64", "Estonia"),
                new KeyValuePair<string, string>("69", "Ethiopia"),
                new KeyValuePair<string, string>("71", "Fiji"),
                new KeyValuePair<string, string>("70", "Finland"),
                new KeyValuePair<string, string>("75", "France"),
                new KeyValuePair<string, string>("175", "French Polynesia"),
                new KeyValuePair<string, string>("76", "Gabon"),
                new KeyValuePair<string, string>("85", "Gambia"),
                new KeyValuePair<string, string>("79", "Georgia"),
                new KeyValuePair<string, string>("57", "Germany"),
                new KeyValuePair<string, string>("82", "Ghana"),
                new KeyValuePair<string, string>("83", "Gibraltar"),
                new KeyValuePair<string, string>("89", "Greece"),
                new KeyValuePair<string, string>("91", "Guatemala"),
                new KeyValuePair<string, string>("86", "Guinea"),
                new KeyValuePair<string, string>("93", "Guinea-Bissau"),
                new KeyValuePair<string, string>("94", "Guyana"),
                new KeyValuePair<string, string>("99", "Haiti"),
                new KeyValuePair<string, string>("97", "Honduras"),
                new KeyValuePair<string, string>("95", "Hong Kong"),
                new KeyValuePair<string, string>("100", "Hungary"),
                new KeyValuePair<string, string>("109", "Iceland"),
                new KeyValuePair<string, string>("105", "India"),
                new KeyValuePair<string, string>("101", "Indonesia"),
                new KeyValuePair<string, string>("108", "Iran"),
                new KeyValuePair<string, string>("107", "Iraq"),
                new KeyValuePair<string, string>("102", "Ireland"),
                new KeyValuePair<string, string>("103", "Israel"),
                new KeyValuePair<string, string>("110", "Italy"),
                new KeyValuePair<string, string>("112", "Jamaica"),
                new KeyValuePair<string, string>("114", "Japan"),
                new KeyValuePair<string, string>("113", "Jordan"),
                new KeyValuePair<string, string>("125", "Kazakhstan"),
                new KeyValuePair<string, string>("115", "Kenya"),
                new KeyValuePair<string, string>("255", "Kosovo"),
                new KeyValuePair<string, string>("123", "Kuwait"),
                new KeyValuePair<string, string>("126", "Lao People's Democratic Republic"),
                new KeyValuePair<string, string>("135", "Latvia"),
                new KeyValuePair<string, string>("127", "Lebanon"),
                new KeyValuePair<string, string>("136", "Libya"),
                new KeyValuePair<string, string>("129", "Liechtenstein"),
                new KeyValuePair<string, string>("133", "Lithuania"),
                new KeyValuePair<string, string>("134", "Luxembourg"),
                new KeyValuePair<string, string>("148", "Macao"),
                new KeyValuePair<string, string>("144", "Macedonia"),
                new KeyValuePair<string, string>("142", "Madagascar"),
                new KeyValuePair<string, string>("156", "Malawi"),
                new KeyValuePair<string, string>("158", "Malaysia"),
                new KeyValuePair<string, string>("155", "Maldives"),
                new KeyValuePair<string, string>("145", "Mali"),
                new KeyValuePair<string, string>("153", "Malta"),
                new KeyValuePair<string, string>("151", "Mauritania"),
                new KeyValuePair<string, string>("154", "Mauritius"),
                new KeyValuePair<string, string>("157", "Mexico"),
                new KeyValuePair<string, string>("139", "Moldova"),
                new KeyValuePair<string, string>("138", "Monaco"),
                new KeyValuePair<string, string>("147", "Mongolia"),
                new KeyValuePair<string, string>("140", "Montenegro"),
                new KeyValuePair<string, string>("137", "Morocco"),
                new KeyValuePair<string, string>("159", "Mozambique"),
                new KeyValuePair<string, string>("168", "Nepal"),
                new KeyValuePair<string, string>("166", "Netherlands"),
                new KeyValuePair<string, string>("171", "New Zealand"),
                new KeyValuePair<string, string>("165", "Nicaragua"),
                new KeyValuePair<string, string>("162", "Niger"),
                new KeyValuePair<string, string>("164", "Nigeria"),
                new KeyValuePair<string, string>("257", "Norhtern Ireland"),
                new KeyValuePair<string, string>("167", "Norway"),
                new KeyValuePair<string, string>("172", "Oman"),
                new KeyValuePair<string, string>("178", "Pakistan"),
                new KeyValuePair<string, string>("183", "Palestine"),
                new KeyValuePair<string, string>("173", "Panama"),
                new KeyValuePair<string, string>("176", "Papua New Guinea"),
                new KeyValuePair<string, string>("186", "Paraguay"),
                new KeyValuePair<string, string>("174", "Peru"),
                new KeyValuePair<string, string>("177", "Philippines"),
                new KeyValuePair<string, string>("179", "Poland"),
                new KeyValuePair<string, string>("184", "Portugal"),
                new KeyValuePair<string, string>("253", "Portugal (Azzorre)"),
                new KeyValuePair<string, string>("252", "Portugal (Madeira)"),
                new KeyValuePair<string, string>("182", "Puerto Rico"),
                new KeyValuePair<string, string>("187", "Qatar"),
                new KeyValuePair<string, string>("189", "Romania"),
                new KeyValuePair<string, string>("191", "Russia"),
                new KeyValuePair<string, string>("192", "Rwanda"),
                new KeyValuePair<string, string>("128", "Saint Lucia"),
                new KeyValuePair<string, string>("244", "Samoa"),
                new KeyValuePair<string, string>("193", "Saudi Arabia"),
                new KeyValuePair<string, string>("205", "Senegal"),
                new KeyValuePair<string, string>("190", "Serbia"),
                new KeyValuePair<string, string>("195", "Seychelles"),
                new KeyValuePair<string, string>("203", "Sierra Leone"),
                new KeyValuePair<string, string>("198", "Singapore"),
                new KeyValuePair<string, string>("202", "Slovakia"),
                new KeyValuePair<string, string>("200", "Slovenia"),
                new KeyValuePair<string, string>("194", "Solomon Islands"),
                new KeyValuePair<string, string>("247", "South Africa"),
                new KeyValuePair<string, string>("122", "South Korea"),
                new KeyValuePair<string, string>("68", "Spain"),
                new KeyValuePair<string, string>("254", "Spain (Canarias)"),
                new KeyValuePair<string, string>("250", "Spain (Ceuta)"),
                new KeyValuePair<string, string>("251", "Spain (Melilla)"),
                new KeyValuePair<string, string>("130", "Sri Lanka"),
                new KeyValuePair<string, string>("196", "Sudan"),
                new KeyValuePair<string, string>("197", "Sweden"),
                new KeyValuePair<string, string>("43", "Switzerland"),
                new KeyValuePair<string, string>("212", "Syrian Arab Republic"),
                new KeyValuePair<string, string>("228", "Taiwan"),
                new KeyValuePair<string, string>("229", "Tanzania"),
                new KeyValuePair<string, string>("218", "Thailand"),
                new KeyValuePair<string, string>("217", "Togo"),
                new KeyValuePair<string, string>("224", "Tonga"),
                new KeyValuePair<string, string>("223", "Tunisia"),
                new KeyValuePair<string, string>("225", "Turkey"),
                new KeyValuePair<string, string>("231", "Uganda"),
                new KeyValuePair<string, string>("230", "Ukraine"),
                new KeyValuePair<string, string>("2", "United Arab Emirates"),
                new KeyValuePair<string, string>("77", "United Kingdom"),
                new KeyValuePair<string, string>("233", "United States"),
                new KeyValuePair<string, string>("234", "Uruguay"),
                new KeyValuePair<string, string>("238", "Venezuela"),
                new KeyValuePair<string, string>("241", "Vietnam"),
                new KeyValuePair<string, string>("245", "Yemen"),
                new KeyValuePair<string, string>("248", "Zambia"),
                new KeyValuePair<string, string>("249", "Zimbabwe")
            };
            HOLYPOP_COUNTRIES = list4;
            List<KeyValuePair<string, string>> list5 = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("178", "Albania"),
                new KeyValuePair<string, string>("179", "Algeria"),
                new KeyValuePair<string, string>("181", "American Samoa"),
                new KeyValuePair<string, string>("184", "Andorra"),
                new KeyValuePair<string, string>("187", "Angola"),
                new KeyValuePair<string, string>("191", "Anguilla"),
                new KeyValuePair<string, string>("197", "Antigua and Barbuda"),
                new KeyValuePair<string, string>("203", "Argentina"),
                new KeyValuePair<string, string>("107", "Armenia"),
                new KeyValuePair<string, string>("108", "Aruba"),
                new KeyValuePair<string, string>("109", "Australia"),
                new KeyValuePair<string, string>("111", "Austria"),
                new KeyValuePair<string, string>("114", "Azerbaijan"),
                new KeyValuePair<string, string>("118", "Bahamas"),
                new KeyValuePair<string, string>("122", "Bahrain"),
                new KeyValuePair<string, string>("126", "Bangladesh"),
                new KeyValuePair<string, string>("132", "Barbados"),
                new KeyValuePair<string, string>("142", "Belarus"),
                new KeyValuePair<string, string>("29", "Belgium"),
                new KeyValuePair<string, string>("30", "Belize"),
                new KeyValuePair<string, string>("33", "Benin"),
                new KeyValuePair<string, string>("36", "Bermuda"),
                new KeyValuePair<string, string>("40", "Bhutan"),
                new KeyValuePair<string, string>("45", "Bolivia, Plurinational State of"),
                new KeyValuePair<string, string>("50", "Bosnia and Herzegovina"),
                new KeyValuePair<string, string>("55", "Botswana"),
                new KeyValuePair<string, string>("61", "Brazil"),
                new KeyValuePair<string, string>("68", "Brunei Darussalam"),
                new KeyValuePair<string, string>("182", "Bulgaria"),
                new KeyValuePair<string, string>("185", "Burkina Faso"),
                new KeyValuePair<string, string>("188", "Burundi"),
                new KeyValuePair<string, string>("192", "Cambodia"),
                new KeyValuePair<string, string>("198", "Cameroon"),
                new KeyValuePair<string, string>("204", "Canada"),
                new KeyValuePair<string, string>("209", "Cape Verde"),
                new KeyValuePair<string, string>("215", "Cayman Islands"),
                new KeyValuePair<string, string>("221", "Central African Republic"),
                new KeyValuePair<string, string>("1", "Chad"),
                new KeyValuePair<string, string>("115", "Chile"),
                new KeyValuePair<string, string>("119", "China"),
                new KeyValuePair<string, string>("123", "Colombia"),
                new KeyValuePair<string, string>("127", "Comoros"),
                new KeyValuePair<string, string>("133", "Congo"),
                new KeyValuePair<string, string>("138", "Congo, The Democratic Republic of the"),
                new KeyValuePair<string, string>("143", "Cook Islands"),
                new KeyValuePair<string, string>("154", "Costa Rica"),
                new KeyValuePair<string, string>("158", "C\x00f4te dIvoireIvoire"),
                new KeyValuePair<string, string>("161", "Croatia"),
                new KeyValuePair<string, string>("41", "Cuba"),
                new KeyValuePair<string, string>("46", "Cyprus"),
                new KeyValuePair<string, string>("51", "Czech Republic"),
                new KeyValuePair<string, string>("56", "Denmark"),
                new KeyValuePair<string, string>("62", "Djibouti"),
                new KeyValuePair<string, string>("69", "Dominica"),
                new KeyValuePair<string, string>("74", "Dominican Republic"),
                new KeyValuePair<string, string>("79", "Ecuador"),
                new KeyValuePair<string, string>("85", "Egypt"),
                new KeyValuePair<string, string>("90", "El Salvador"),
                new KeyValuePair<string, string>("193", "Equatorial Guinea"),
                new KeyValuePair<string, string>("205", "Eritrea"),
                new KeyValuePair<string, string>("210", "Estonia"),
                new KeyValuePair<string, string>("216", "Ethiopia"),
                new KeyValuePair<string, string>("222", "Falkland Islands (Malvinas)"),
                new KeyValuePair<string, string>("2", "Faroe Islands"),
                new KeyValuePair<string, string>("6", "Fiji"),
                new KeyValuePair<string, string>("10", "Finland"),
                new KeyValuePair<string, string>("13", "France"),
                new KeyValuePair<string, string>("17", "French Guiana"),
                new KeyValuePair<string, string>("128", "French Polynesia"),
                new KeyValuePair<string, string>("134", "Gabon"),
                new KeyValuePair<string, string>("144", "Gambia"),
                new KeyValuePair<string, string>("149", "Georgia"),
                new KeyValuePair<string, string>("155", "Germany"),
                new KeyValuePair<string, string>("162", "Ghana"),
                new KeyValuePair<string, string>("165", "Gibraltar"),
                new KeyValuePair<string, string>("168", "Greece"),
                new KeyValuePair<string, string>("171", "Greenland"),
                new KeyValuePair<string, string>("173", "Grenada"),
                new KeyValuePair<string, string>("57", "Guadeloupe"),
                new KeyValuePair<string, string>("63", "Guam"),
                new KeyValuePair<string, string>("70", "Guatemala"),
                new KeyValuePair<string, string>("80", "Guinea"),
                new KeyValuePair<string, string>("86", "Guinea-Bissau"),
                new KeyValuePair<string, string>("91", "Guyana"),
                new KeyValuePair<string, string>("93", "Haiti"),
                new KeyValuePair<string, string>("96", "Holy See (Vatican City State)"),
                new KeyValuePair<string, string>("99", "Honduras"),
                new KeyValuePair<string, string>("102", "Hong Kong"),
                new KeyValuePair<string, string>("217", "Hungary"),
                new KeyValuePair<string, string>("223", "Iceland"),
                new KeyValuePair<string, string>("3", "India"),
                new KeyValuePair<string, string>("7", "Indonesia"),
                new KeyValuePair<string, string>("14", "Iran, Islamic Republic of"),
                new KeyValuePair<string, string>("18", "Iraq"),
                new KeyValuePair<string, string>("20", "Ireland"),
                new KeyValuePair<string, string>("22", "Israel"),
                new KeyValuePair<string, string>("24", "Italy"),
                new KeyValuePair<string, string>("26", "Jamaica"),
                new KeyValuePair<string, string>("27", "Japan"),
                new KeyValuePair<string, string>("28", "Jordan"),
                new KeyValuePair<string, string>("31", "Kazakhstan"),
                new KeyValuePair<string, string>("34", "Kenya"),
                new KeyValuePair<string, string>("37", "Kiribati"),
                new KeyValuePair<string, string>("42", "Korea, Democratic Peoples Republic ofs Republic of"),
                new KeyValuePair<string, string>("47", "Korea, Republic of"),
                new KeyValuePair<string, string>("52", "Kuwait"),
                new KeyValuePair<string, string>("58", "Kyrgyzstan"),
                new KeyValuePair<string, string>("64", "Lao Peoples Democratic Republics Democratic Republic"),
                new KeyValuePair<string, string>("180", "Latvia"),
                new KeyValuePair<string, string>("183", "Lebanon"),
                new KeyValuePair<string, string>("186", "Lesotho"),
                new KeyValuePair<string, string>("189", "Liberia"),
                new KeyValuePair<string, string>("194", "Libya"),
                new KeyValuePair<string, string>("199", "Liechtenstein"),
                new KeyValuePair<string, string>("206", "Lithuania"),
                new KeyValuePair<string, string>("211", "Luxembourg"),
                new KeyValuePair<string, string>("218", "Macao"),
                new KeyValuePair<string, string>("224", "Macedonia, Republic of"),
                new KeyValuePair<string, string>("112", "Madagascar"),
                new KeyValuePair<string, string>("116", "Malawi"),
                new KeyValuePair<string, string>("120", "Malaysia"),
                new KeyValuePair<string, string>("124", "Maldives"),
                new KeyValuePair<string, string>("129", "Mali"),
                new KeyValuePair<string, string>("135", "Malta"),
                new KeyValuePair<string, string>("139", "Marshall Islands"),
                new KeyValuePair<string, string>("145", "Martinique"),
                new KeyValuePair<string, string>("150", "Mauritania"),
                new KeyValuePair<string, string>("156", "Mauritius"),
                new KeyValuePair<string, string>("38", "Mexico"),
                new KeyValuePair<string, string>("43", "Micronesia, Federated States of"),
                new KeyValuePair<string, string>("48", "Moldova, Republic of"),
                new KeyValuePair<string, string>("53", "Monaco"),
                new KeyValuePair<string, string>("59", "Mongolia"),
                new KeyValuePair<string, string>("229", "Montenegro"),
                new KeyValuePair<string, string>("65", "Montserrat"),
                new KeyValuePair<string, string>("71", "Morocco"),
                new KeyValuePair<string, string>("75", "Mozambique"),
                new KeyValuePair<string, string>("81", "Myanmar"),
                new KeyValuePair<string, string>("87", "Namibia"),
                new KeyValuePair<string, string>("195", "Nauru"),
                new KeyValuePair<string, string>("200", "Nepal"),
                new KeyValuePair<string, string>("207", "Netherlands"),
                new KeyValuePair<string, string>("212", "Netherlands Antilles"),
                new KeyValuePair<string, string>("219", "New Caledonia"),
                new KeyValuePair<string, string>("225", "New Zealand"),
                new KeyValuePair<string, string>("4", "Nicaragua"),
                new KeyValuePair<string, string>("8", "Niger"),
                new KeyValuePair<string, string>("11", "Nigeria"),
                new KeyValuePair<string, string>("15", "Niue"),
                new KeyValuePair<string, string>("130", "Norfolk Island"),
                new KeyValuePair<string, string>("136", "Northern Mariana Islands"),
                new KeyValuePair<string, string>("140", "Norway"),
                new KeyValuePair<string, string>("146", "Oman"),
                new KeyValuePair<string, string>("151", "Pakistan"),
                new KeyValuePair<string, string>("157", "Palau"),
                new KeyValuePair<string, string>("159", "Panama"),
                new KeyValuePair<string, string>("163", "Papua New Guinea"),
                new KeyValuePair<string, string>("166", "Paraguay"),
                new KeyValuePair<string, string>("169", "Peru"),
                new KeyValuePair<string, string>("60", "Philippines"),
                new KeyValuePair<string, string>("66", "Pitcairn"),
                new KeyValuePair<string, string>("72", "Poland"),
                new KeyValuePair<string, string>("76", "Portugal"),
                new KeyValuePair<string, string>("82", "Puerto Rico"),
                new KeyValuePair<string, string>("88", "Qatar"),
                new KeyValuePair<string, string>("92", "R\x00e9union"),
                new KeyValuePair<string, string>("94", "Romania"),
                new KeyValuePair<string, string>("97", "Russia"),
                new KeyValuePair<string, string>("100", "Rwanda"),
                new KeyValuePair<string, string>("213", "Saint Helena, Ascension and Tristan da Cunha"),
                new KeyValuePair<string, string>("226", "Saint Kitts and Nevis"),
                new KeyValuePair<string, string>("5", "Saint Lucia"),
                new KeyValuePair<string, string>("9", "Saint Pierre and Miquelon"),
                new KeyValuePair<string, string>("12", "Saint Vincent and the Grenadines"),
                new KeyValuePair<string, string>("16", "Samoa"),
                new KeyValuePair<string, string>("19", "San Marino"),
                new KeyValuePair<string, string>("21", "Sao Tome and Principe"),
                new KeyValuePair<string, string>("23", "Saudi Arabia"),
                new KeyValuePair<string, string>("25", "Senegal"),
                new KeyValuePair<string, string>("227", "Serbia"),
                new KeyValuePair<string, string>("147", "Seychelles"),
                new KeyValuePair<string, string>("152", "Sierra Leone"),
                new KeyValuePair<string, string>("160", "Singapore"),
                new KeyValuePair<string, string>("164", "Slovakia"),
                new KeyValuePair<string, string>("167", "Slovenia"),
                new KeyValuePair<string, string>("170", "Solomon Islands"),
                new KeyValuePair<string, string>("172", "Somalia"),
                new KeyValuePair<string, string>("174", "South Africa"),
                new KeyValuePair<string, string>("175", "Spain"),
                new KeyValuePair<string, string>("176", "Sri Lanka"),
                new KeyValuePair<string, string>("77", "Sudan"),
                new KeyValuePair<string, string>("83", "Suriname"),
                new KeyValuePair<string, string>("89", "Svalbard and Jan Mayen"),
                new KeyValuePair<string, string>("95", "Swaziland"),
                new KeyValuePair<string, string>("98", "Sweden"),
                new KeyValuePair<string, string>("101", "Switzerland"),
                new KeyValuePair<string, string>("103", "Syrian Arab Republic"),
                new KeyValuePair<string, string>("104", "Taiwan"),
                new KeyValuePair<string, string>("105", "Tajikistan"),
                new KeyValuePair<string, string>("106", "Tanzania, United Republic of"),
                new KeyValuePair<string, string>("110", "Thailand"),
                new KeyValuePair<string, string>("113", "Togo"),
                new KeyValuePair<string, string>("117", "Tokelau"),
                new KeyValuePair<string, string>("121", "Tonga"),
                new KeyValuePair<string, string>("125", "Trinidad and Tobago"),
                new KeyValuePair<string, string>("131", "Tunisia"),
                new KeyValuePair<string, string>("137", "Turkey"),
                new KeyValuePair<string, string>("141", "Turkmenistan"),
                new KeyValuePair<string, string>("148", "Turks and Caicos Islands"),
                new KeyValuePair<string, string>("153", "Tuvalu"),
                new KeyValuePair<string, string>("32", "Uganda"),
                new KeyValuePair<string, string>("35", "Ukraine"),
                new KeyValuePair<string, string>("39", "United Arab Emirates"),
                new KeyValuePair<string, string>("44", "United Kingdom"),
                new KeyValuePair<string, string>("49", "United States"),
                new KeyValuePair<string, string>("54", "Uruguay"),
                new KeyValuePair<string, string>("67", "Uzbekistan"),
                new KeyValuePair<string, string>("73", "Vanuatu"),
                new KeyValuePair<string, string>("78", "Venezuela, Bolivarian Republic of"),
                new KeyValuePair<string, string>("84", "Viet Nam"),
                new KeyValuePair<string, string>("190", "Virgin Islands, British"),
                new KeyValuePair<string, string>("196", "Virgin Islands, U.S."),
                new KeyValuePair<string, string>("201", "Wallis and Futuna"),
                new KeyValuePair<string, string>("202", "Western Sahara"),
                new KeyValuePair<string, string>("208", "Yemen"),
                new KeyValuePair<string, string>("214", "Zambia"),
                new KeyValuePair<string, string>("220", "Zimbabwe")
            };
            OFFWHITE_COUNTRIES = list5;
        }

        public WebsitesInfo()
        {
            Class7.RIuqtBYzWxthF();
        }

        internal static string GetNordstromCountryId(object countryName)
        {
            object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject("[{\"Id\":1,\"Name\":\"Afghanistan\"},{\"Id\":2,\"Name\":\"Albania\"},{\"Id\":3,\"Name\":\"Algeria\"},{\"Id\":4,\"Name\":\"American Samoa\"},{\"Id\":5,\"Name\":\"Andorra\"},{\"Id\":6,\"Name\":\"Angola\"},{\"Id\":7,\"Name\":\"Anguilla\"},{\"Id\":8,\"Name\":\"Antigua And Barbuda\"},{\"Id\":9,\"Name\":\"Argentina\"},{\"Id\":10,\"Name\":\"Aruba\"},{\"Id\":12,\"Name\":\"Australia\"},{\"Id\":13,\"Name\":\"Austria\"},{\"Id\":293,\"Name\":\"Azerbaijan\"},{\"Id\":15,\"Name\":\"Bahamas\"},{\"Id\":16,\"Name\":\"Bahrain\"},{\"Id\":17,\"Name\":\"Bangladesh\"},{\"Id\":18,\"Name\":\"Barbados\"},{\"Id\":20,\"Name\":\"Belgium\"},{\"Id\":21,\"Name\":\"Belize\"},{\"Id\":22,\"Name\":\"Benin\"},{\"Id\":23,\"Name\":\"Bermuda\"},{\"Id\":24,\"Name\":\"Bhutan\"},{\"Id\":25,\"Name\":\"Bolivia\"},{\"Id\":28,\"Name\":\"Botswana\"},{\"Id\":29,\"Name\":\"Brazil\"},{\"Id\":30,\"Name\":\"Brunei Darussalam\"},{\"Id\":31,\"Name\":\"Bulgaria\"},{\"Id\":32,\"Name\":\"Burkina Faso\"},{\"Id\":35,\"Name\":\"Burundi\"},{\"Id\":36,\"Name\":\"Cambodia\"},{\"Id\":37,\"Name\":\"Cameroon\"},{\"Id\":38,\"Name\":\"Canada\"},{\"Id\":40,\"Name\":\"Cape Verde\"},{\"Id\":42,\"Name\":\"Cayman Is.\"},{\"Id\":43,\"Name\":\"Central African Republic\"},{\"Id\":44,\"Name\":\"Chad\"},{\"Id\":46,\"Name\":\"Chile\"},{\"Id\":47,\"Name\":\"China\"},{\"Id\":48,\"Name\":\"Colombia\"},{\"Id\":49,\"Name\":\"Comoros\"},{\"Id\":52,\"Name\":\"Cook Is.\"},{\"Id\":54,\"Name\":\"Costa Rica\"},{\"Id\":56,\"Name\":\"Croatia\"},{\"Id\":59,\"Name\":\"Cyprus\"},{\"Id\":60,\"Name\":\"Czech Republic\"},{\"Id\":61,\"Name\":\"Denmark\"},{\"Id\":62,\"Name\":\"Djibouti\"},{\"Id\":63,\"Name\":\"Dominica\"},{\"Id\":65,\"Name\":\"Dominican Republic\"},{\"Id\":66,\"Name\":\"East Timor\"},{\"Id\":67,\"Name\":\"Ecuador\"},{\"Id\":68,\"Name\":\"Egypt\"},{\"Id\":69,\"Name\":\"El Salvador\"},{\"Id\":71,\"Name\":\"Equatorial Guinea\"},{\"Id\":72,\"Name\":\"Eritrea\"},{\"Id\":73,\"Name\":\"Estonia\"},{\"Id\":74,\"Name\":\"Ethiopia\"},{\"Id\":75,\"Name\":\"Falkland Is.\"},{\"Id\":76,\"Name\":\"Faroe Is.\"},{\"Id\":77,\"Name\":\"Fiji\"},{\"Id\":78,\"Name\":\"Finland\"},{\"Id\":79,\"Name\":\"France\"},{\"Id\":80,\"Name\":\"French Guiana\"},{\"Id\":81,\"Name\":\"French Polynesia\"},{\"Id\":82,\"Name\":\"Gabon\"},{\"Id\":83,\"Name\":\"Gambia\"},{\"Id\":84,\"Name\":\"Georgia\"},{\"Id\":85,\"Name\":\"Germany\"},{\"Id\":87,\"Name\":\"Gibraltar\"},{\"Id\":89,\"Name\":\"Greece\"},{\"Id\":90,\"Name\":\"Greenland\"},{\"Id\":91,\"Name\":\"Grenada\"},{\"Id\":92,\"Name\":\"Guadeloupe\"},{\"Id\":93,\"Name\":\"Guam\"},{\"Id\":94,\"Name\":\"Guatemala\"},{\"Id\":95,\"Name\":\"Guinea\"},{\"Id\":96,\"Name\":\"Guinea-Bissau\"},{\"Id\":97,\"Name\":\"Guyana\"},{\"Id\":98,\"Name\":\"Haiti\"},{\"Id\":99,\"Name\":\"Honduras\"},{\"Id\":100,\"Name\":\"Hong Kong\"},{\"Id\":101,\"Name\":\"Hungary\"},{\"Id\":102,\"Name\":\"Iceland\"},{\"Id\":103,\"Name\":\"India\"},{\"Id\":104,\"Name\":\"Indonesia\"},{\"Id\":107,\"Name\":\"Ireland\"},{\"Id\":108,\"Name\":\"Israel\"},{\"Id\":109,\"Name\":\"Italy\"},{\"Id\":110,\"Name\":\"Jamaica\"},{\"Id\":111,\"Name\":\"Japan\"},{\"Id\":112,\"Name\":\"Jordan\"},{\"Id\":114,\"Name\":\"Kazakhstan\"},{\"Id\":115,\"Name\":\"Kenya\"},{\"Id\":116,\"Name\":\"Kiribati\"},{\"Id\":120,\"Name\":\"Kuwait\"},{\"Id\":121,\"Name\":\"Kyrgyzstan\"},{\"Id\":122,\"Name\":\"Laos\"},{\"Id\":123,\"Name\":\"Latvia\"},{\"Id\":124,\"Name\":\"Lebanon\"},{\"Id\":126,\"Name\":\"Lesotho\"},{\"Id\":128,\"Name\":\"Libya\"},{\"Id\":129,\"Name\":\"Liechtenstein\"},{\"Id\":130,\"Name\":\"Lithuania\"},{\"Id\":131,\"Name\":\"Luxembourg\"},{\"Id\":132,\"Name\":\"Macau\"},{\"Id\":133,\"Name\":\"Macedonia\"},{\"Id\":134,\"Name\":\"Madagascar\"},{\"Id\":136,\"Name\":\"Malawi\"},{\"Id\":137,\"Name\":\"Malaysia\"},{\"Id\":138,\"Name\":\"Maldives\"},{\"Id\":139,\"Name\":\"Mali\"},{\"Id\":140,\"Name\":\"Malta\"},{\"Id\":141,\"Name\":\"Martinique\"},{\"Id\":142,\"Name\":\"Mauritania\"},{\"Id\":143,\"Name\":\"Mauritius\"},{\"Id\":144,\"Name\":\"Mexico\"},{\"Id\":145,\"Name\":\"Micronesia\"},{\"Id\":146,\"Name\":\"Moldova (Republic of)\"},{\"Id\":147,\"Name\":\"Monaco\"},{\"Id\":148,\"Name\":\"Mongolia\"},{\"Id\":294,\"Name\":\"Montenegro\"},{\"Id\":149,\"Name\":\"Montserrat\"},{\"Id\":150,\"Name\":\"Morocco\"},{\"Id\":151,\"Name\":\"Mozambique\"},{\"Id\":153,\"Name\":\"Namibia\"},{\"Id\":154,\"Name\":\"Nauru\"},{\"Id\":155,\"Name\":\"Nepal\"},{\"Id\":156,\"Name\":\"Netherlands\"},{\"Id\":157,\"Name\":\"Netherlands Antilles\"},{\"Id\":158,\"Name\":\"New Caledonia\"},{\"Id\":159,\"Name\":\"New Zealand\"},{\"Id\":160,\"Name\":\"Nicaragua\"},{\"Id\":161,\"Name\":\"Niger\"},{\"Id\":163,\"Name\":\"Norfolk Island\"},{\"Id\":165,\"Name\":\"Northern Mariana Is.\"},{\"Id\":166,\"Name\":\"Norway\"},{\"Id\":167,\"Name\":\"Oman\"},{\"Id\":168,\"Name\":\"Pakistan\"},{\"Id\":169,\"Name\":\"Palau\"},{\"Id\":170,\"Name\":\"Panama\"},{\"Id\":171,\"Name\":\"Papua New Guinea\"},{\"Id\":172,\"Name\":\"Paraguay\"},{\"Id\":173,\"Name\":\"Peru\"},{\"Id\":174,\"Name\":\"Philippines\"},{\"Id\":175,\"Name\":\"Pitcairn\"},{\"Id\":176,\"Name\":\"Poland\"},{\"Id\":178,\"Name\":\"Portugal\"},{\"Id\":179,\"Name\":\"Puerto Rico\"},{\"Id\":180,\"Name\":\"Qatar\"},{\"Id\":181,\"Name\":\"Reunion\"},{\"Id\":183,\"Name\":\"Romania\"},{\"Id\":185,\"Name\":\"Russia\"},{\"Id\":186,\"Name\":\"Rwanda\"},{\"Id\":188,\"Name\":\"Saint Kitts and Nevis\"},{\"Id\":189,\"Name\":\"Saint Lucia\"},{\"Id\":190,\"Name\":\"Saint Vincent & Grenadines\"},{\"Id\":192,\"Name\":\"Samoa (Independent)\"},{\"Id\":194,\"Name\":\"Sao Tome and Principe\"},{\"Id\":195,\"Name\":\"Saudi Arabia\"},{\"Id\":197,\"Name\":\"Senegal\"},{\"Id\":292,\"Name\":\"Serbia\"},{\"Id\":198,\"Name\":\"Seychelles\"},{\"Id\":199,\"Name\":\"Sierra Leone\"},{\"Id\":200,\"Name\":\"Singapore\"},{\"Id\":201,\"Name\":\"Slovakia\"},{\"Id\":202,\"Name\":\"Slovenia\"},{\"Id\":203,\"Name\":\"Solomon Is.\"},{\"Id\":205,\"Name\":\"Somalia Southern Reg\"},{\"Id\":206,\"Name\":\"South Africa\"},{\"Id\":207,\"Name\":\"South Korea\"},{\"Id\":208,\"Name\":\"Spain\"},{\"Id\":209,\"Name\":\"Sri Lanka\"},{\"Id\":214,\"Name\":\"St. Helena\"},{\"Id\":219,\"Name\":\"St. Pierre and Miquelon\"},{\"Id\":222,\"Name\":\"Suriname\"},{\"Id\":223,\"Name\":\"Swaziland\"},{\"Id\":224,\"Name\":\"Sweden\"},{\"Id\":225,\"Name\":\"Switzerland\"},{\"Id\":228,\"Name\":\"Taiwan\"},{\"Id\":229,\"Name\":\"Tajikistan\"},{\"Id\":230,\"Name\":\"Tanzania\"},{\"Id\":231,\"Name\":\"Thailand\"},{\"Id\":233,\"Name\":\"Togo\"},{\"Id\":234,\"Name\":\"Tonga\"},{\"Id\":236,\"Name\":\"Trinidad\"},{\"Id\":239,\"Name\":\"Tunisia\"},{\"Id\":240,\"Name\":\"Turkey\"},{\"Id\":241,\"Name\":\"Turks and Caicos Is.\"},{\"Id\":242,\"Name\":\"Tuvalu\"},{\"Id\":243,\"Name\":\"Uganda\"},{\"Id\":244,\"Name\":\"Ukraine\"},{\"Id\":246,\"Name\":\"United Arab Emirates\"},{\"Id\":247,\"Name\":\"United Kingdom\"},{\"Id\":249,\"Name\":\"United States\"},{\"Id\":248,\"Name\":\"Uruguay\"},{\"Id\":250,\"Name\":\"Uzbekistan\"},{\"Id\":251,\"Name\":\"Vanuatu\"},{\"Id\":253,\"Name\":\"Venezuela\"},{\"Id\":254,\"Name\":\"Viet Nam\"},{\"Id\":256,\"Name\":\"Virgin Is. (British)\"},{\"Id\":257,\"Name\":\"Virgin Is. (U.S.)\"},{\"Id\":263,\"Name\":\"Yemen Arab Rep\"},{\"Id\":264,\"Name\":\"Yugoslavia\"},{\"Id\":265,\"Name\":\"Zambia\"}]");
            if (<>o__23.<>p__8 == null)
            {
                <>o__23.<>p__8 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(WebsitesInfo)));
            }
            using (IEnumerator enumerator = <>o__23.<>p__8.Target(<>o__23.<>p__8, obj2).GetEnumerator())
            {
                object obj3;
                goto Label_019B;
            Label_0058:
                obj3 = enumerator.Current;
                if (<>o__23.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__23.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__23.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__23.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__23.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__23.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__23.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__23.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__23.<>p__3.Target(<>o__23.<>p__3, <>o__23.<>p__2.Target(<>o__23.<>p__2, <>o__23.<>p__1.Target(<>o__23.<>p__1, <>o__23.<>p__0.Target(<>o__23.<>p__0, obj3, "Name")), (string) countryName)))
                {
                    goto Label_01AB;
                }
            Label_019B:
                if (!enumerator.MoveNext())
                {
                    goto Label_02F1;
                }
                goto Label_0058;
            Label_01AB:
                if (<>o__23.<>p__7 == null)
                {
                    <>o__23.<>p__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(WebsitesInfo)));
                }
                if (<>o__23.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__23.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__23.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__23.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__23.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__23.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                }
                return <>o__23.<>p__7.Target(<>o__23.<>p__7, <>o__23.<>p__6.Target(<>o__23.<>p__6, <>o__23.<>p__5.Target(<>o__23.<>p__5, <>o__23.<>p__4.Target(<>o__23.<>p__4, obj3, "Id"))));
            }
        Label_02F1:
            return "";
        }

        internal static string GetNordstromgStateId(object stateCode)
        {
            IEnumerator enumerator;
            object obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject("{\"UsMainland\":[{\"Id\":73,\"Name\":\"Alabama\",\"Code\":\"AL\",\"CountryId\":249},{\"Id\":16,\"Name\":\"Alaska\",\"Code\":\"AK\",\"CountryId\":249},{\"Id\":70,\"Name\":\"Arizona\",\"Code\":\"AZ\",\"CountryId\":249},{\"Id\":75,\"Name\":\"Arkansas\",\"Code\":\"AR\",\"CountryId\":249},{\"Id\":71,\"Name\":\"California\",\"Code\":\"CA\",\"CountryId\":249},{\"Id\":72,\"Name\":\"Colorado\",\"Code\":\"CO\",\"CountryId\":249},{\"Id\":67,\"Name\":\"Connecticut\",\"Code\":\"CT\",\"CountryId\":249},{\"Id\":69,\"Name\":\"Delaware\",\"Code\":\"DE\",\"CountryId\":249},{\"Id\":68,\"Name\":\"District of Columbia\",\"Code\":\"DC\",\"CountryId\":249},{\"Id\":65,\"Name\":\"Florida\",\"Code\":\"FL\",\"CountryId\":249},{\"Id\":66,\"Name\":\"Georgia\",\"Code\":\"GA\",\"CountryId\":249},{\"Id\":62,\"Name\":\"Hawaii\",\"Code\":\"HI\",\"CountryId\":249},{\"Id\":63,\"Name\":\"Idaho\",\"Code\":\"ID\",\"CountryId\":249},{\"Id\":58,\"Name\":\"Illinois\",\"Code\":\"IL\",\"CountryId\":249},{\"Id\":59,\"Name\":\"Indiana\",\"Code\":\"IN\",\"CountryId\":249},{\"Id\":60,\"Name\":\"Iowa\",\"Code\":\"IA\",\"CountryId\":249},{\"Id\":55,\"Name\":\"Kansas\",\"Code\":\"KS\",\"CountryId\":249},{\"Id\":56,\"Name\":\"Kentucky\",\"Code\":\"KY\",\"CountryId\":249},{\"Id\":57,\"Name\":\"Louisiana\",\"Code\":\"LA\",\"CountryId\":249},{\"Id\":52,\"Name\":\"Maine\",\"Code\":\"ME\",\"CountryId\":249},{\"Id\":50,\"Name\":\"Maryland\",\"Code\":\"MD\",\"CountryId\":249},{\"Id\":51,\"Name\":\"Massachusetts\",\"Code\":\"MA\",\"CountryId\":249},{\"Id\":47,\"Name\":\"Michigan\",\"Code\":\"MI\",\"CountryId\":249},{\"Id\":48,\"Name\":\"Minnesota\",\"Code\":\"MN\",\"CountryId\":249},{\"Id\":49,\"Name\":\"Mississippi\",\"Code\":\"MS\",\"CountryId\":249},{\"Id\":44,\"Name\":\"Missouri\",\"Code\":\"MO\",\"CountryId\":249},{\"Id\":45,\"Name\":\"Montana\",\"Code\":\"MT\",\"CountryId\":249},{\"Id\":46,\"Name\":\"Nebraska\",\"Code\":\"NE\",\"CountryId\":249},{\"Id\":41,\"Name\":\"Nevada\",\"Code\":\"NV\",\"CountryId\":249},{\"Id\":42,\"Name\":\"New Hampshire\",\"Code\":\"NH\",\"CountryId\":249},{\"Id\":43,\"Name\":\"New Jersey\",\"Code\":\"NJ\",\"CountryId\":249},{\"Id\":38,\"Name\":\"New Mexico\",\"Code\":\"NM\",\"CountryId\":249},{\"Id\":39,\"Name\":\"New York\",\"Code\":\"NY\",\"CountryId\":249},{\"Id\":40,\"Name\":\"North Carolina\",\"Code\":\"NC\",\"CountryId\":249},{\"Id\":35,\"Name\":\"North Dakota\",\"Code\":\"ND\",\"CountryId\":249},{\"Id\":36,\"Name\":\"Ohio\",\"Code\":\"OH\",\"CountryId\":249},{\"Id\":37,\"Name\":\"Oklahoma\",\"Code\":\"OK\",\"CountryId\":249},{\"Id\":32,\"Name\":\"Oregon\",\"Code\":\"OR\",\"CountryId\":249},{\"Id\":34,\"Name\":\"Pennsylvania\",\"Code\":\"PA\",\"CountryId\":249},{\"Id\":30,\"Name\":\"Rhode Island\",\"Code\":\"RI\",\"CountryId\":249},{\"Id\":31,\"Name\":\"South Carolina\",\"Code\":\"SC\",\"CountryId\":249},{\"Id\":26,\"Name\":\"South Dakota\",\"Code\":\"SD\",\"CountryId\":249},{\"Id\":27,\"Name\":\"Tennessee\",\"Code\":\"TN\",\"CountryId\":249},{\"Id\":28,\"Name\":\"Texas\",\"Code\":\"TX\",\"CountryId\":249},{\"Id\":23,\"Name\":\"Utah\",\"Code\":\"UT\",\"CountryId\":249},{\"Id\":24,\"Name\":\"Vermont\",\"Code\":\"VT\",\"CountryId\":249},{\"Id\":25,\"Name\":\"Virginia\",\"Code\":\"VA\",\"CountryId\":249},{\"Id\":21,\"Name\":\"Washington\",\"Code\":\"WA\",\"CountryId\":249},{\"Id\":22,\"Name\":\"West Virginia\",\"Code\":\"WV\",\"CountryId\":249},{\"Id\":17,\"Name\":\"Wisconsin\",\"Code\":\"WI\",\"CountryId\":249},{\"Id\":18,\"Name\":\"Wyoming\",\"Code\":\"WY\",\"CountryId\":249}],\"MilitaryBase\":[{\"Id\":19,\"Name\":\"Armed Forces Americas\",\"Code\":\"AA\",\"CountryId\":249},{\"Id\":14,\"Name\":\"Armed Forces Europe\",\"Code\":\"AE\",\"CountryId\":249},{\"Id\":15,\"Name\":\"Armed Forces Pacific\",\"Code\":\"AP\",\"CountryId\":249}],\"UsTerritory\":[{\"Id\":74,\"Name\":\"American Samoa\",\"Code\":\"AS\",\"CountryId\":249},{\"Id\":61,\"Name\":\"Guam\",\"Code\":\"GU\",\"CountryId\":249},{\"Id\":53,\"Name\":\"Marianas Islands\",\"Code\":\"MP\",\"CountryId\":249},{\"Id\":54,\"Name\":\"Marshall Islands\",\"Code\":\"MH\",\"CountryId\":249},{\"Id\":64,\"Name\":\"Micronesia\",\"Code\":\"FM\",\"CountryId\":249},{\"Id\":33,\"Name\":\"Palau\",\"Code\":\"PW\",\"CountryId\":249},{\"Id\":29,\"Name\":\"Puerto Rico\",\"Code\":\"PR\",\"CountryId\":249},{\"Id\":20,\"Name\":\"US Virgin Islands\",\"Code\":\"VI\",\"CountryId\":249}],\"CanadianProvince\":[{\"Id\":11,\"Name\":\"Alberta\",\"Code\":\"AB\",\"CountryId\":38},{\"Id\":12,\"Name\":\"British Columbia\",\"Code\":\"BC\",\"CountryId\":38},{\"Id\":13,\"Name\":\"Manitoba\",\"Code\":\"MB\",\"CountryId\":38},{\"Id\":8,\"Name\":\"New Brunswick\",\"Code\":\"NB\",\"CountryId\":38},{\"Id\":9,\"Name\":\"Newfoundland\",\"Code\":\"NF\",\"CountryId\":38},{\"Id\":5,\"Name\":\"Northwest Territories\",\"Code\":\"NT\",\"CountryId\":38},{\"Id\":10,\"Name\":\"Nova Scotia\",\"Code\":\"NS\",\"CountryId\":38},{\"Id\":6,\"Name\":\"Nunavut\",\"Code\":\"NU\",\"CountryId\":38},{\"Id\":7,\"Name\":\"Ontario\",\"Code\":\"ON\",\"CountryId\":38},{\"Id\":2,\"Name\":\"Prince Edward Island\",\"Code\":\"PE\",\"CountryId\":38},{\"Id\":3,\"Name\":\"Quebec\",\"Code\":\"QC\",\"CountryId\":38},{\"Id\":4,\"Name\":\"Saskatchewan\",\"Code\":\"SK\",\"CountryId\":38},{\"Id\":1,\"Name\":\"Yukon\",\"Code\":\"YT\",\"CountryId\":38}]}");
            if (<>o__22.<>p__9 == null)
            {
                <>o__22.<>p__9 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(WebsitesInfo)));
            }
            if (<>o__22.<>p__0 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__22.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UsMainland", typeof(WebsitesInfo), argumentInfo));
            }
            using (enumerator = <>o__22.<>p__9.Target(<>o__22.<>p__9, <>o__22.<>p__0.Target(<>o__22.<>p__0, obj2)).GetEnumerator())
            {
                object current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (<>o__22.<>p__4 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__3 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__2 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__1 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__4.Target(<>o__22.<>p__4, <>o__22.<>p__3.Target(<>o__22.<>p__3, <>o__22.<>p__2.Target(<>o__22.<>p__2, <>o__22.<>p__1.Target(<>o__22.<>p__1, current, "Code")), (string) stateCode)))
                    {
                        goto Label_01F8;
                    }
                }
                goto Label_0340;
            Label_01F8:
                if (<>o__22.<>p__8 == null)
                {
                    <>o__22.<>p__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(WebsitesInfo)));
                }
                if (<>o__22.<>p__7 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__22.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__22.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__22.<>p__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                }
                return <>o__22.<>p__8.Target(<>o__22.<>p__8, <>o__22.<>p__7.Target(<>o__22.<>p__7, <>o__22.<>p__6.Target(<>o__22.<>p__6, <>o__22.<>p__5.Target(<>o__22.<>p__5, current, "Id"))));
            }
        Label_0340:
            if (<>o__22.<>p__19 == null)
            {
                <>o__22.<>p__19 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(WebsitesInfo)));
            }
            if (<>o__22.<>p__10 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__22.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MilitaryBase", typeof(WebsitesInfo), argumentInfo));
            }
            using (enumerator = <>o__22.<>p__19.Target(<>o__22.<>p__19, <>o__22.<>p__10.Target(<>o__22.<>p__10, obj2)).GetEnumerator())
            {
                object current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (<>o__22.<>p__14 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__13 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__12 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__11 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__14.Target(<>o__22.<>p__14, <>o__22.<>p__13.Target(<>o__22.<>p__13, <>o__22.<>p__12.Target(<>o__22.<>p__12, <>o__22.<>p__11.Target(<>o__22.<>p__11, current, "Code")), (string) stateCode)))
                    {
                        goto Label_052A;
                    }
                }
                goto Label_0671;
            Label_052A:
                if (<>o__22.<>p__18 == null)
                {
                    <>o__22.<>p__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(WebsitesInfo)));
                }
                if (<>o__22.<>p__17 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__22.<>p__16 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__22.<>p__15 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__22.<>p__15 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                }
                return <>o__22.<>p__18.Target(<>o__22.<>p__18, <>o__22.<>p__17.Target(<>o__22.<>p__17, <>o__22.<>p__16.Target(<>o__22.<>p__16, <>o__22.<>p__15.Target(<>o__22.<>p__15, current, "Id"))));
            }
        Label_0671:
            if (<>o__22.<>p__29 == null)
            {
                <>o__22.<>p__29 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(WebsitesInfo)));
            }
            if (<>o__22.<>p__20 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                <>o__22.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UsTerritory", typeof(WebsitesInfo), argumentInfo));
            }
            using (enumerator = <>o__22.<>p__29.Target(<>o__22.<>p__29, <>o__22.<>p__20.Target(<>o__22.<>p__20, obj2)).GetEnumerator())
            {
                object current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (<>o__22.<>p__24 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__24 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__23 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__22 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                        <>o__22.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__21 == null)
                    {
                        CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                        <>o__22.<>p__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                    }
                    if (<>o__22.<>p__24.Target(<>o__22.<>p__24, <>o__22.<>p__23.Target(<>o__22.<>p__23, <>o__22.<>p__22.Target(<>o__22.<>p__22, <>o__22.<>p__21.Target(<>o__22.<>p__21, current, "Code")), (string) stateCode)))
                    {
                        goto Label_085D;
                    }
                }
                return "";
            Label_085D:
                if (<>o__22.<>p__28 == null)
                {
                    <>o__22.<>p__28 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(WebsitesInfo)));
                }
                if (<>o__22.<>p__27 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__22.<>p__26 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__22.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof(WebsitesInfo), argumentInfo));
                }
                if (<>o__22.<>p__25 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant | CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__22.<>p__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(WebsitesInfo), argumentInfo));
                }
                return <>o__22.<>p__28.Target(<>o__22.<>p__28, <>o__22.<>p__27.Target(<>o__22.<>p__27, <>o__22.<>p__26.Target(<>o__22.<>p__26, <>o__22.<>p__25.Target(<>o__22.<>p__25, current, "Id"))));
            }
        }

        [CompilerGenerated]
        private static class <>o__22
        {
            public static CallSite<Func<CallSite, object, object>> <>p__0;
            public static CallSite<Func<CallSite, object, string, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, string, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
            public static CallSite<Func<CallSite, object, string, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, object>> <>p__7;
            public static CallSite<Func<CallSite, object, string>> <>p__8;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__9;
            public static CallSite<Func<CallSite, object, object>> <>p__10;
            public static CallSite<Func<CallSite, object, string, object>> <>p__11;
            public static CallSite<Func<CallSite, object, object>> <>p__12;
            public static CallSite<Func<CallSite, object, string, object>> <>p__13;
            public static CallSite<Func<CallSite, object, bool>> <>p__14;
            public static CallSite<Func<CallSite, object, string, object>> <>p__15;
            public static CallSite<Func<CallSite, object, object>> <>p__16;
            public static CallSite<Func<CallSite, object, object>> <>p__17;
            public static CallSite<Func<CallSite, object, string>> <>p__18;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__19;
            public static CallSite<Func<CallSite, object, object>> <>p__20;
            public static CallSite<Func<CallSite, object, string, object>> <>p__21;
            public static CallSite<Func<CallSite, object, object>> <>p__22;
            public static CallSite<Func<CallSite, object, string, object>> <>p__23;
            public static CallSite<Func<CallSite, object, bool>> <>p__24;
            public static CallSite<Func<CallSite, object, string, object>> <>p__25;
            public static CallSite<Func<CallSite, object, object>> <>p__26;
            public static CallSite<Func<CallSite, object, object>> <>p__27;
            public static CallSite<Func<CallSite, object, string>> <>p__28;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__29;
        }

        [CompilerGenerated]
        private static class <>o__23
        {
            public static CallSite<Func<CallSite, object, string, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, string, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, string, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, object>> <>p__6;
            public static CallSite<Func<CallSite, object, string>> <>p__7;
            public static CallSite<Func<CallSite, object, IEnumerable>> <>p__8;
        }
    }
}

