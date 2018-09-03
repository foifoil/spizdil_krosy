namespace EveAIO.Tasks.Platforms
{
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using EveAIO.Tasks.Dto;
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal class Barneys : IPlatform
    {
        private TaskRunner _runner;
        private TaskObject _task;
        private string _srr;
        private HtmlDocument _currentDoc;
        private object _request;
        private byte[] _bytes;
        [Dynamic]
        private object _dynObj;
        private string _data;

        public Barneys(TaskRunner runner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._currentDoc = new HtmlDocument();
            this._runner = runner;
            this._task = task;
        }

        public bool Atc()
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.ATC_SIZE, null, this._runner.PickedSize.Value.Key, "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART);
                this._request = (HttpWebRequest) WebRequest.Create("https://www.barneys.com/global/ajaxGlobalNav.jsp");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("Origin", "https://www.barneys.com");
                this._request.Method = "POST";
                this._request.Referer = this._task.Link;
                this._request.CookieContainer = this._runner.Cookies;
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.ContentLength = 0L;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.barneys.com/browse/gadgets/ajaxPickerContents.jsp");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("Origin", "https://www.barneys.com");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Referer = this._task.Link;
                this._request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._data = "size=10+M";
                this._data = this._data + "&skuId=00505057433196";
                this._data = this._data + "&productId=505743314";
                this._data = this._data + "&onHandQuantity=1";
                this._data = this._data + "&onOrderQuantity=0";
                this._data = this._data + "&availabilityStatus=1000";
                this._data = this._data + "&listPrice=1000.0";
                this._data = this._data + "&salePrice=1000.0";
                this._data = this._data + "&onSale=false";
                this._data = this._data + "&atp=1";
                this._data = this._data + "&quantity=1";
                this._data = this._data + "&expectedDeliveryMonth=";
                this._data = this._data + "&isGWPProduct=0";
                this._data = this._data + "&vendorColor=BLACK";
                this._data = this._data + "&productCurrentSite=BNY";
                this._data = this._data + "&isPrivate=0";
                this._data = this._data + "&isApplePayEnabledForProduct=true";
                this._data = this._data + "&isApplePayEnabledForSKU=true";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream = this._request.GetRequestStream())
                {
                    stream.Write(this._bytes, 0, this._bytes.Length);
                }
                using (WebResponse response2 = this._request.GetResponse())
                {
                    using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                    {
                        this._srr = reader2.ReadToEnd();
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                string str = this._currentDoc.DocumentNode.Descendants("input").First<HtmlNode>(x => ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_dynSessConf"))).Attributes["value"].Value;
                this._request = (HttpWebRequest) WebRequest.Create("https://www.barneys.com/checkout/gadgets/client.jsp");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Referer = this._task.Link;
                this._request.CookieContainer = this._runner.Cookies;
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (WebResponse response3 = this._request.GetResponse())
                {
                    using (StreamReader reader3 = new StreamReader(response3.GetResponseStream()))
                    {
                        this._srr = reader3.ReadToEnd();
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.barneys.com/navigation/gadgets/customerInfo.jsp");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("Origin", "https://www.barneys.com");
                this._request.Method = "POST";
                this._request.Referer = this._task.Link;
                this._request.CookieContainer = this._runner.Cookies;
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.ContentLength = 0L;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (WebResponse response4 = this._request.GetResponse())
                {
                    using (StreamReader reader4 = new StreamReader(response4.GetResponseStream()))
                    {
                        this._srr = reader4.ReadToEnd();
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.barneys.com/browse/productDetailColorSizePicker.jsp?productId=505743314");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Mobile Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("Origin", "https://www.barneys.com");
                this._request.Method = "POST";
                this._request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                this._request.Referer = this._task.Link;
                this._request.CookieContainer = this._runner.Cookies;
                this._request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._request.Headers.Add("X-X1WIohvy-uniqueStateKey", "AFnaf81iAQAAnKqAeCdC4t9UKVx7HBG2jeAAoCzGwgT4N0Jjk_iDvTUv2OkW");
                this._request.Headers.Add("X-X1WIohvy-c", "AGzBf81iAQAAM_IAKw_DpNtlZso4uQWQ4x__ApfESfwxB9pmRBJ3P6ux6ekM");
                this._request.Headers.Add("X-X1WIohvy-b", "-e824lr");
                this._request.Headers.Add("X-X1WIohvy-a", "8qTCfmZhcXDTdmIPcNzTcYDw0S6=0bDCENzTUiZrcYD2VNzh7LDAULc5cNzidJzPYiZpciy=0bs_ci6zEiePeiDj0S6=0q2UVN63Nmz=UiZmULDXNmZAca=hcLItdhR_037i0JebdL=hyt=OtZetXLemcLsheB=Cc0=hULI=0J6_dN2iV3ehEiDpy36_0SE=E-bbySeX0SDX0kErULEpVLyrQ8HikC76ULsrEiZpVmejc=ez6Nc=d37jdLZXUmeAlNRt03erEieACSDVCSDcr3R=0Ne=dSR=73ePVSeXkCGrx0Z4CNS=yS_5ci6=dMz5cH6pVLI=yJ6_chs_dLNWEB=CctZhEB-ukCdrxH=SVLP=6Nc=d37rx8HZr3epEBzI0J6=caemcLshCNTx9UNicmywVSZMXmywVmZhULDpVSZXXLs_ES=3VN6w0McrVJz=cLoBEiDwdiz_0QrfVNxClmDActs_dLNBVNxC7SZPc05_0BxLcNzrULDpXS6w7SDheBz_Vmw7UiZXcBE_0Se2dmsTENzXcLsT90sPVN_tdJeTUZxwULsh0k_CdiZhcSDXd0EC0SDAELRhX3xXdm6IVJ67ELfzENR=0AZ3cLshy3c=dS6w0M=mcLsAdJz7EL-bCLTx0Q7nE22nE2BnE2fnE2j4BQn4uMsb0SDJ0meXtiyIcm=p08qa4OnquMITVLsmVNR7ENxCdJzhK=xGLybbrSRu0SDPceE=V3RhdJz=4UfRVJz=VN6=6Ly=dLepEjqi412Sx-Lz1Kf-ciZhceRh0S=pc8xRdm4qlNe3W21mW2asR2tqH2aKH7VKH21q6hIt-r1XH21q-aR=d36XVLCq6NeXdJx=Wa6_9Ly5cm_hWZ65dLt51-fHciehcLRh6SDpEBHXv1wmfAPCWbIlB1ObXL=PcIRXVJR=EOibXm5_ESZ7ENxCdJzh4-fOUmesVSD_0S6ZESepEBj42-nqu-fRdLeAULZacNc5VmezcOcV2y9UuMPPdJerctemcLsh08qOuwqG11jq46VZ1jqq4H21CtB10Ix1XtnJvxk11v2_oaRwq12Mlqu111B4WO2r1GH1kCjr13bvll6xt-f4BC1x4OaLxl24WO21CHZxCBfElH=1P8qG11Bq49y2d411oaW-111xvn2qkCBr1jH2kCz47aaalen4vnBbxOjb05wTlH=1pjq_4q7T43-Q4h2zlfG4W9Wa4vzrnOR1XtnJvnBbxOjb08BTljcxVjq_4q7T43le4hazlNJ4W9Wa4vzhWpRx2ai2vnBbxOjbE2fTllIx_jq_4q7T43lo4haOlV74W9Wa4vzhepRx2hiavnBbxOjbEB2Tllsxq8q_4q7T43Vx4haHlNo4W9Wa4vzJZpRxnhZvvnBbxOjbEyJTllPxGGq_4q7T43054haflNr4W9Wa4vzJOvRxntZsvnBbxOjbEhoTll_xEjq_4q7T43E04hailLr4W9Wa4vzJdpRxxtZbvnBbxOjbEs2Tll6xLGq_4q7T43994haaled4W9Wa4vzJMvRxxaZ7vnBbxOjbEo8Tll6xtGq_4q7T43Gj4haale24W9Wa4vz4EvRxxaZluwqG11fq46VZ1jqq4H21CtB10hs1XtnJvxk11p2_oaRwq12Mlqu111B4WO2r1GH1kCjr13boll6xt-f6dJz5cLshVN65dmsZESepEBj42vfb4q1x11111-fY0ieXcSDXdLZpVme7ENxCdJzh4UfR0Bzw0ieXEB=2UieTU8qlv6WjUwA71jAe1jA6j4lo6ErsZWnbr3xXdJx=036seSZMELervnf4ZvzanAjtl9ZxX1114GqTyhIw9S=MdiBW7Seh0mR_0iNCRY4CWn_HULsI92MqlLsA0SD5cn1mfT1oWas=9BerW2tql3e5dilw7ezxR7_O-Yxx0BxMceE=VAP5EnkIHr0pHrVq-aPWeaIHfnxMULP=WaE=VmPw-Yx2UBzwdLtwRTtpHn4rHrWIfTa4HYxRdmz5ditqtmZSVNz5frtrRX4rRq2bx12Zem=pHrfZ6meTUm8WHT1CHr1yH2GC7LDKULyMVYkIfT1q-ay5d3e4OXxxdS6Xdm=AW2VpH2Mq7Se4ENHqRYxnEL=McnDRtAaIOa45WaZC0iy=emebYm=hfrtrRX4rRb1uYh_t7tCMWiy5Umtq6meTUmk5WaRu0SDPcYkmRY4CfTHrHTtpH7qyWaIwVS=McYx7VLc_0SAwR7HJfTHm1HPBdmD3ditqYLsTfM24iaB_l0qVl9aVl0T_uMsrVJz=cLsjESeX0S=AcGqi412QHBg2=-f-0J6_VmPt0SZTcGqU48x2VLspdJlq0Se_cnxC0SDCcNzh9Y13Hn0qdmVqd3eMd12zeB=CcteX0SDX48xt9Nx=6NzXdJWKWaR_dSswEnxXcLZAWBxXdJx=036sWn0CzXxwcbxpELyMnb1qWnx_EnxT0n1ueezH-luqWn1qVNlqVSkq-ZeY7nA-Wn1qWiZhWi_=Wn_etAC5nb1qWnx_EnxjVS5=VJlp0mehEN1q-ZeY7nA-Wn1qWiZhWiRCWn_etAC5nb1qWnx_EnxbdX1ueezH-luqWn1qVNlqUitq-ZeY7nA-Wn1qWiZhWaDbUSeTEn4kVLswd3=PdJerjb1ueezH-luqWn1qVNlqlNzXVNApVJ1q-ZeY7nA-Wn1qWiZhWi_=Wn_etAC51OObXJ6wELRu6Nc=d36rvxpqu-nbX3_u0=RI0Bxw037_uP_TENRhdmIGVmDp0mDMcexXdJx=0365cNjSNqn8o-ftVJerEiDPNm_50J6w03=HcLs3EiT4z1f_uPZTENRhdmIGEi=PctDptiZ3cGq=sC111LfRGDJi-m=-9YLbhSRI0J6wdeD=d3c2UieTUm=pc8Vt888CuMITENRhdmIGEmebt=62u0MyHn4IRY4XRY4JHC");
                this._request.Headers.Add("X-X1WIohvy-d", "0");
                this._request.ServicePoint.Expect100Continue = false;
                this._request.Headers.Add("Pragma", "no-cache");
                this._request.Headers.Add("Cache-Control", "no-cache");
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._data = "_dyncharset=UTF-8";
                this._data = this._data + "&_dynSessConf=" + str;
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.items%5B0%5D.quantity=1";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.items%5B0%5D.quantity=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.addItemToOrderErrorURL=";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.addItemToOrderErrorURL=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.addItemCount=1";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.addItemCount=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.addItemToOrderSuccessURL=%2Fcart";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.addItemToOrderSuccessURL=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.sessionExpirationURL=%2Fglobal%2FsessionExpired.jsp";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.sessionExpirationURL=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.ajaxAddItemToOrderSuccessUrl=%2Fcart%2Fjson%2FcartContents.jsp";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.ajaxAddItemToOrderSuccessUrl=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.ajaxAddItemToOrderErrorUrl=%2Fcart%2Fjson%2Ferrors.jsp";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.ajaxAddItemToOrderErrorUrl=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.items%5B0%5D.catalogRefId=00505057433196";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.items%5B0%5D.catalogRefId=+";
                this._data = this._data + "&%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.items%5B0%5D.productId=505743314";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.items%5B0%5D.productId=+";
                this._data = this._data + "&_D%3A%2Fatg%2Fcommerce%2Forder%2Fpurchase%2FCartModifierFormHandler.addItemToOrder=+";
                this._data = this._data + "&_DARGS=%2Fbrowse%2Fgadgets%2FajaxPickerContents.jsp.addToCart";
                this._data = this._data + "&invokeAddItemToCartHandler=true";
                this._data = this._data + "&isAddToCart=true";
                this._bytes = Encoding.ASCII.GetBytes(this._data);
                this._request.ContentLength = this._bytes.Length;
                using (Stream stream2 = this._request.GetRequestStream())
                {
                    stream2.Write(this._bytes, 0, this._bytes.Length);
                }
                using (WebResponse response5 = this._request.GetResponse())
                {
                    using (StreamReader reader5 = new StreamReader(response5.GetResponseStream()))
                    {
                        this._srr = reader5.ReadToEnd();
                    }
                }
                this._request = (HttpWebRequest) WebRequest.Create("https://www.barneys.com/cart");
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.Accept = "text/html, */*; q=0.01";
                this._request.Headers.Add("Origin", "https://www.barneys.com");
                this._request.Referer = this._task.Link;
                this._request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                using (WebResponse response6 = this._request.GetResponse())
                {
                    using (StreamReader reader6 = new StreamReader(response6.GetResponseStream()))
                    {
                        this._srr = reader6.ReadToEnd();
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                if (this._currentDoc.DocumentNode.Descendants("span").Any<HtmlNode>(x => ((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "mini-count")) && (x.InnerText.Trim() == "1")))
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_SUCCESSFUL, null, "", "");
                    return true;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.CARTING_UNSUCCESSFUL, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.ADDING_TO_CART_UNSUCCESSFUL);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_ATC, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        public bool Checkout() => 
            false;

        public bool DirectLink(string link)
        {
            try
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.CHECKING_STOCK, null, "", "");
                this._task.Status = States.GetTaskState(States.TaskState.CHECKING_STOCK);
                Cookie cookie = new Cookie {
                    Value = "7FBA7A2F8ED6E3A14C80E35F368A07D802103C07CD190000A641D45AA341130A~plo20T95xrylbjN5LXtHTtKT18fy9f4jULUUri0vHhQVnqC5Eip7tNWE5NXXy7Zb9R+cZ7mUnkrak9UmqnfZqHdNS4bCTo+wYnyEjaz9wCSN5KubYJUYtk/M0GGZVHKSDuVKGN5PgNekhw4m3cUI9zgBME4IN89++0Jj4vEMbB3AW3F74ret/NYzEwROInxKK39Zs6apgYgDUmWwqTtc/au+aYkTDT6DHrmdVW33N/gBzNMVRMn677UdbNke3ok12P",
                    Name = "ak_bmsc",
                    Domain = "barneys.com"
                };
                this._runner.Cookies.Add(cookie);
                this._request = (HttpWebRequest) WebRequest.Create(link);
                this._request.KeepAlive = true;
                this._request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
                this._request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                this._request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                this._request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                this._request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                this._request.CookieContainer = this._runner.Cookies;
                if (this._runner.Proxy != null)
                {
                    this._request.Proxy = this._runner.Proxy;
                }
                this._request.Referer = "https://www.barneys.com";
                using (WebResponse response = this._request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        this._srr = reader.ReadToEnd();
                    }
                }
                this._currentDoc.LoadHtml(this._srr);
                this._runner.ProductPageHtml = new HtmlDocument();
                this._runner.ProductPageHtml.LoadHtml(this._srr);
                string str = this._currentDoc.DocumentNode.Descendants("h1").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "prd-brand"))).InnerText.Trim() + " " + this._currentDoc.DocumentNode.Descendants("h2").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-title"))).InnerText.Trim();
                string str2 = this._currentDoc.DocumentNode.Descendants("div").First<HtmlNode>(x => ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "atg_store_productPrice"))).InnerText.Trim();
                string str3 = this._currentDoc.DocumentNode.Descendants("img").First<HtmlNode>(x => ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"))).Attributes["src"].Value;
                this._task.ImgUrl = str3;
                Product product1 = new Product {
                    ProductTitle = str,
                    Link = link,
                    Price = str2
                };
                this._runner.Product = product1;
                foreach (HtmlNode node in from x in this._currentDoc.DocumentNode.Descendants("a")
                    where (x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("atg_store_oneSize sizePicker")
                    select x)
                {
                    BarneysProduct item = new BarneysProduct {
                        AvailabilityStatus = node.Attributes["data-availabilitystatus"].Value,
                        CurrentCommerceItemId = node.Attributes["data-currentcommerceitemid"].Value,
                        EnabledForProduct = node.Attributes["data-is-ap-enabled-for-product"].Value,
                        EnabledForSku = node.Attributes["data-is-ap-enabled-for-sku"].Value,
                        ExpectedDeliveryMonth = node.Attributes["data-expected-delivery-month"].Value,
                        Atp = node.Attributes["data-atp"].Value,
                        Gwp = node.Attributes["data-gwp"].Value,
                        IsPrivate = node.Attributes["data-isprivate"].Value,
                        ListPrice = node.Attributes["data-list-price"].Value,
                        OnHandQuantity = node.Attributes["data-onhand-quantity"].Value,
                        OnOrderQuantity = node.Attributes["data-onorder-quantity"].Value,
                        OnSale = node.Attributes["data-on-sale"].Value,
                        ProductCurrentSite = node.Attributes["data-product-current-site"].Value,
                        ProductId = node.Attributes["data-productid"].Value,
                        SalePrice = node.Attributes["data-sale-price"].Value,
                        Size = node.InnerText.Trim(),
                        SkuId = node.Attributes["data-skuid"].Value,
                        VendorColor = node.Attributes["data-vendorcolor"].Value
                    };
                    item.SizeAltered = item.Size.Replace("M", "").Replace("W", "").Trim();
                    this._runner.Product.AvailableBarneysSizes.Add(item);
                }
                if (this._runner.Product.AvailableBarneysSizes.Count == 0)
                {
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRODUCT_OOS, null, "", "");
                    return false;
                }
                if (this._task.PriceCheck)
                {
                    string str4 = "";
                    foreach (char ch in this._runner.Product.Price)
                    {
                        if ((char.IsDigit(ch) || (ch == '.')) || (ch == ','))
                        {
                            str4 = str4 + ch.ToString();
                        }
                    }
                    double num4 = double.Parse(str4.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture);
                    if ((num4 < this._task.MinimumPrice) || (num4 > this._task.MaximumPrice))
                    {
                        States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_NOT_PASSED, null, "", "");
                        return false;
                    }
                    States.WriteLogger(this._task, States.LOGGER_STATES.PRICE_CHECK_PASSED, null, "", "");
                }
                if (!this._task.RandomSize)
                {
                    char[] separator = new char[] { '#' };
                    string[] strArray = this._task.Size.Split(separator);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray[i] = strArray[i].Trim().ToUpperInvariant();
                    }
                    foreach (string str6 in strArray)
                    {
                        if (this._runner.PickedBarneysSize != null)
                        {
                            break;
                        }
                        using (List<BarneysProduct>.Enumerator enumerator2 = this._runner.Product.AvailableBarneysSizes.GetEnumerator())
                        {
                            BarneysProduct current;
                            while (enumerator2.MoveNext())
                            {
                                current = enumerator2.Current;
                                if (current.SizeAltered.ToLowerInvariant().Trim() == str6.ToLowerInvariant().Trim())
                                {
                                    goto Label_07A8;
                                }
                            }
                            continue;
                        Label_07A8:
                            this._runner.PickedBarneysSize = current;
                        }
                    }
                    if (this._runner.PickedBarneysSize == null)
                    {
                        if (!this._task.SizePickRandom)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this._runner.PickedSize = new KeyValuePair<string, string>(this._runner.PickedBarneysSize.SizeAltered, this._runner.PickedBarneysSize.SkuId);
                        return true;
                    }
                }
                this._runner.PickedBarneysSize = this._runner.Product.AvailableBarneysSizes[this._runner.Rnd.Next(0, this._runner.Product.AvailableBarneysSizes.Count)];
                this._runner.PickedSize = new KeyValuePair<string, string>(this._runner.PickedBarneysSize.SizeAltered, this._runner.PickedBarneysSize.SkuId);
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception exception)
            {
                this._runner.IsError = true;
                if (!exception.Message.Contains("430") && ((exception.InnerException == null) || !exception.InnerException.Message.Contains("430")))
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.ERROR_CHECKING_STOCK, exception, "", "");
                }
                else
                {
                    this._task.Status = States.GetTaskState(States.TaskState.IP_BANNED);
                    States.WriteLogger(this._task, States.LOGGER_STATES.IP_BANNED, null, "", "");
                }
                return false;
            }
        }

        public bool Login()
        {
            throw new NotImplementedException();
        }

        public bool Search()
        {
            throw new NotImplementedException();
        }

        public void SetClient()
        {
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Barneys.<>c <>9;
            public static Func<HtmlNode, bool> <>9__10_0;
            public static Func<HtmlNode, bool> <>9__10_1;
            public static Func<HtmlNode, bool> <>9__12_0;
            public static Func<HtmlNode, bool> <>9__12_1;
            public static Func<HtmlNode, bool> <>9__12_2;
            public static Func<HtmlNode, bool> <>9__12_3;
            public static Func<HtmlNode, bool> <>9__12_4;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Barneys.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Atc>b__10_0(HtmlNode x) => 
                ((x.Attributes["name"] != null) && (x.Attributes["name"].Value == "_dynSessConf"));

            internal bool <Atc>b__10_1(HtmlNode x) => 
                (((x.Attributes["id"] != null) && (x.Attributes["id"].Value == "mini-count")) && (x.InnerText.Trim() == "1"));

            internal bool <DirectLink>b__12_0(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "prd-brand"));

            internal bool <DirectLink>b__12_1(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "product-title"));

            internal bool <DirectLink>b__12_2(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "atg_store_productPrice"));

            internal bool <DirectLink>b__12_3(HtmlNode x) => 
                ((x.Attributes["itemprop"] != null) && (x.Attributes["itemprop"].Value == "image"));

            internal bool <DirectLink>b__12_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && x.Attributes["class"].Value.Contains("atg_store_oneSize sizePicker"));
        }
    }
}

