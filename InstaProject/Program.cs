using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xNet;
using System.Threading;
using RandomNameGenerator;


namespace InstaProject
{
    class Program
    {
        private static List<string> AccList;

        public static CookieDictionary[] LogedAccs;
        public static string[] ChallangedAccs, Links,BannedAccs;
        static CookieDictionary Cookie = new CookieDictionary();
        static Random _rnd = new Random();
        private static string proxy, device_id, guid;
        static string Command;
       
        static void CreateData()
        {
            AccList = new List<string>();

        }

        static void Main(string[] args)
        {
           // Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Title = "InstaProject";
            Thread MainThread = new Thread(MainThr);
            MainThread.IsBackground = true;
            MainThread.Start();
            MainThread.Join();
            Console.Read();
        }

       static void MainThr()
        {
            reset:
            CreateData();
            LoadAccs();
            

            for (int i = 0; i <= AccList.Count - 1; i++)
            {
                Console.WriteLine("["+i.ToString()+"] " + AccList[i]);
            }

            start:
            
            Console.WriteLine("Выберите Аккаунт");
           here:
            Command = Console.ReadLine();
        
           if (Command == "reset")
           {
                Console.WriteLine("Очистили все");
                AccList.Clear();
                Console.Clear();
                goto reset;
               
           }
            
            int id = Convert.ToInt32(Command);
            int loged = Login(id);
            
            Console.WriteLine("Выбран аккаунт номер[" + Command + "]");
    

            if (loged == 0)
                {
                    Console.WriteLine(AccList[id] + "[LOGGED]");
                    Command = Console.ReadLine();
                    if (Command == "Change")
                    {
                        ChangeData(LogedAccs[id], id);
                        goto start;
                    }
                if (Command == "reset")
                {
                    Console.WriteLine("Очистили все");
                    AccList.Clear();
                    Console.Clear();
                    goto reset;

                }

            }
            else
                {
                    if (loged == 1)
                    {
                        Console.WriteLine(AccList[id] + "[SMS]");
                        Command = Console.ReadLine();
                        if (Command == "SMS")
                        {
                            smsAccept(LogedAccs[id], id);
                            goto start;
                        }
                    }

                    if (loged == 2)
                    {
                        Console.WriteLine(AccList[id] + "[BAD/BAN/]");
                        goto start;

                    }
                }
            goto here;

        }



        static void LoadAccs()
        {
            string FILE_NAME = "Accounts.dat";
            string[] readText = File.ReadAllLines(FILE_NAME);
            Links = File.ReadAllLines("link.txt");
            AccList = readText.ToList();
            LogedAccs = new CookieDictionary[readText.Length];
            ChallangedAccs = new string[readText.Length];
            BannedAccs = new string[readText.Length];

            Console.WriteLine("Всего Аккаунтов: " + LogedAccs.Length);
            Console.WriteLine("Всего Линков: " + Links.Length);
        }

        static void smsAccept(CookieDictionary Cook, int id)
        {
            HttpRequest http = new HttpRequest();
            proxy = AccList[id].Split(';')[2];
            if (proxy != "")
                http.Proxy = Socks5ProxyClient.Parse(proxy);
            //http.Proxy.Username = "proxy370";
            //   http.Proxy.Password = "daf26n4d";
            http.Cookies = Cook;
            string token = Functions.Pars(Cook.ToString()+";", "csrftoken=", ";", 0);
           // Console.WriteLine(token);
           // Console.WriteLine(Cook.ToString());
            http.UserAgent = Functions.RandomUserAgentInsta();
            Console.WriteLine(ChallangedAccs[id]);
          //  Console.WriteLine(Cook.ToString());
            string checkUrl = Functions.Pars(ChallangedAccs[id], "checkpoint_url\": \"", "\"", 0);
            string pg = http.Get(checkUrl).ToString();
            var reqParams = new RequestParams();
            reqParams.Clear();
    
            Console.WriteLine("PHONE NUMBER: ");
            //Console.WriteLine(token);
            reqParams["csrfmiddlewaretoken"] = token;
            reqParams["phone_number"] = Console.ReadLine();
            http.AddHeader(HttpHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            http.AddHeader(HttpHeader.DNT, "1");
            http.AddHeader("Upgrade-Insecure-Requests", "1");
            http.KeepAlive = true;
            http.Referer = checkUrl;
            http.Post(checkUrl, reqParams);
            Console.WriteLine("GET SMS AND WRITE CODE: ");
            string code = Console.ReadLine();
            reqParams.Clear();
            reqParams["csrfmiddlewaretoken"] = token;
            reqParams["security_code"] = code;
            http.Post(checkUrl, reqParams);
        }


        static void MassFollow()
        {

        }


        static void ChangeData(CookieDictionary Cook, int id)
        {
            HttpRequest http = new HttpRequest();
            proxy = AccList[id].Split(';')[2];
            if (proxy != "")
                http.Proxy = Socks5ProxyClient.Parse(proxy);
         
            http.Cookies = Cook;
            string token = Functions.Pars(Cook.ToString(), "csrftoken=", ";", 0);
            string userid = Functions.Pars(Cook.ToString(), "ds_user_id=", ";", 0);
            http.UserAgent = Functions.RandomUserAgentInsta();
            string pg = http.Get("https://i.instagram.com/api/v1/accounts/current_user/?edit=true").ToString();
            var reqParams = new RequestParams();
            var Mail = new TempMail();
            Mail.GetDomains();
            Mail.GetNewMail();

           

            string[] arrows =
            {
                "\u2192", "\u25B6", "\u2794", "\u2799", "\u279C", "\u279D", "\u279E", "\u279F", "\u27A0",
                "\u27A1", "\u27A4", "\u27A5", "\u27A8", "\u27A9"
            };

            string[] phostos =
            {
               "HI","HELLO"
            };

            string al = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏ";
            string al1 = "ΑΒΓΔΕΖΗΘΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ";

            var randarrownum = _rnd.Next(arrows.Length);
            var randarrow = arrows[randarrownum];
            string[] colors = { "\uDC9A", "\uDC9C", "\uDC99", "\uDC9B" };

            var usern = new List<string>
            {
                
                "\uD83D" + colors[_rnd.Next(colors.Length)] +"M"+"\uD83D" + colors[_rnd.Next(colors.Length)] +"O"+"\uD83D" + colors[_rnd.Next(colors.Length)] +"R"+"\uD83D" + colors[_rnd.Next(colors.Length)] + "E"+"\uD83D" + colors[_rnd.Next(colors.Length)] +phostos[_rnd.Next(phostos.Length)]+ randarrow,
            };

            var bios = new List<string>
            {
                "████████████████████████"+randarrow,
                randarrow
            };




            string phone = Functions.Pars(pg, "\"phone_number\": \"", "\"", 0);
            string email = Functions.Pars(pg, "\"email\": \"", "\"", 0);
           // email = "maxim.asatryan2016+" + Convert.ToString(_rnd.Next(0,10000000)) + "@yandex.ru";
            var ix = _rnd.Next(bios.Count);
            var x = _rnd.Next(usern.Count);
       
            var colornum1 = _rnd.Next(colors.Length);
            var colornum2 = _rnd.Next(colors.Length);
            var usr = "\uD83D" + colors[colornum1] + "\uD83D" + colors[colornum2] + usern[x];
            var bio = bios[ix];
            var name = usern[x];
            var ppslName = NameGenerator.Generate(Gender.Female);
            
            string link;
            try
            {
                link = Links[id];
            }
            catch (Exception)
            {
                link = "";
            }

            string data = "{\"_csrftoken\":\"" + token + "\",\"_uid\":\"" + userid + "\",\"_uuid\":\"" + guid +
                      "\"}";
            string sig = Functions.GenerateSignature(data);

            try
            {
               
                 data = sig.ToLower() + "." + data;

                var files = Directory.GetFiles(@"D:\4K Stogram\classy_queenz");
                var picture = files[_rnd.Next(files.Length)];
                var multipartContent = new MultipartContent
                {
                    {
                        new StringContent(data), "signed_body"
                    },
                    {
                        new StringContent("4"), "ig_sig_key_version"
                    },
                    {
                        new FileContent(picture), "profile_pic", "profile_pic"
                    }
                };
                http.Post("https://i.instagram.com/api/v1/accounts/change_profile_picture/", multipartContent)
                    .ToString();
                Console.WriteLine("AVATAR CHANGED");
            }
            catch (Exception)
            {

                Console.WriteLine("AVATAR NOT CHANGED");
            }
            name = "";

             data = "{\"external_url\":\"" + link + "\",\"gender\":\"2\",\"phone_number\":\""+phone
                          + "\",\"_csrftoken\":\"" +
                          token + "\",\"username\":\"" + AccList[id].Split(';')[0].ToLower() + "\",\"first_name\":\"" +
                          ppslName +
                          "\",\"_uid\":\"" + userid + "\",\"biography\":\"" + name + "\",\"_uuid\":\"" + guid +
                          "\",\"email\":\"" + email + "\"}";

             sig = Functions.GenerateSignature(data);
            reqParams.Clear();
            reqParams["signed_body"] = sig.ToLower() + "." + data;
            reqParams["ig_sig_key_version"] = "4";
            http.Post("https://i.instagram.com/api/v1/accounts/edit_profile/", reqParams);
            Console.WriteLine("LINK CHANGED");

   

        }


        static int Login(int id)
        {


            if (LogedAccs[id] == null && BannedAccs[id]==null)
            {
                string username = AccList[id].Split(';')[0];
                string password = AccList[id].Split(';')[1];
                proxy = AccList[id].Split(';')[2];
                device_id = "android-" +
                            Functions.HMAC(_rnd.Next(1000, 99999).ToString()).Substring(0, Math.Min(64, 16));
                guid = Guid.NewGuid().ToString();
                HttpRequest http = new HttpRequest();
                if (proxy != "")
                    http.Proxy = Socks5ProxyClient.Parse(proxy);
                //http.Proxy.Username = "proxy370";
                //   http.Proxy.Password = "daf26n4d";

                var reqParams = new RequestParams();
                http.UserAgent = Functions.RandomUserAgentInsta();
                http.KeepAlive = false;
                http.AddHeader(HttpHeader.Accept, "*/*");
                http.AddHeader(HttpHeader.AcceptLanguage, "en-US");
                http.AddHeader("X-IG-Connection-Type", "WIFI");
                http.AddHeader("X-IG-Capabilities", "3ToAAA==");
                http.AllowAutoRedirect = true;
                http.ConnectTimeout = 20000;
                http.ReadWriteTimeout = 20000;
                http.EnableEncodingContent = true;
                char[] charsToTrim = {' '};
                reqParams.Clear();
                var data = "{\"device_id\":\"" + device_id + "\",\"username\":\"" + username + "\",\"password\":\"" +
                           password + "\"} ";
                var sig = Functions.GenerateSignature(data);

                sig.Trim(charsToTrim);
                reqParams["signed_body"] = sig.ToLower() + "." + data;
                reqParams["ig_sig_key_version"] = "4";
                try
                {
                    var pg = http.Post("https://i.instagram.com/api/v1/accounts/login/", reqParams).ToString();
                    Cookie = http.Response.Cookies;
                    LogedAccs[id] = Cookie;
                    return 0;
                }
                catch (HttpException err)
                {

                    if (err.Status == HttpExceptionStatus.ConnectFailure)
                    {
                        BannedAccs[id] = AccList[id];
                        return 2;
                    }
                  
                    Cookie = http.Response.Cookies;
                    LogedAccs[id] = Cookie;
                    ChallangedAccs[id] = http.Response.ToString();
                    return 1;
              

                }
                catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                    return 1;
                }
            }
            else
            {
                if(BannedAccs[id]!=null)
                return 2;
                else
                {
                    return 0;
                }
            }

        }


    }
    }

