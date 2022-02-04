using Core.Models.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost("PushMessage")]
        public async Task<IActionResult> PushMessage([FromBody] PushMessageReq models)
        {

            string URL = "https://api.line.me/v2/bot/message/push";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;

            var resp = new object();
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer NndiohliHYK/CUvBE60RXaiCLh2Fe5LvLrnxUJCsbnVZiN42IBbL8aqbkLptcQ/lZ27Jtp5RADGnJrusgBYpDVYNWjzi4PhIkli4qpPqTBHcTTX0j/dWCeBbz74y0iJNaXm/E1k32p1nPLjmMbIdxwdB04t89/1O/w1cDnyilFU=");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    if (models.Type.Equals("Text"))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            to = models.To,
                            messages = new[]{
                                new LineMessage{ Type = "text", Text = models.Messages },
                            }
                        });
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    else if (models.Type.Equals("Flex"))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            to = models.To,
                            messages = new[]{
                                new {
                                        Type = "flex",
                                        AltText = "this is a flex message",
                                        Contents = new
                                        {
                                            
                                                Type = "bubble",
                                                Body = new
                                                {
                                                    Type = "box",
                                                    Layout = "horizontal",
                                                    Contents = new[]
                                                    {
                                                        new
                                                        {
                                                            Type="text",
                                                            Text= models.Messages,
                                                            Wrap = true
                                                        }
                                                    }
                                                },
                                                Footer = new
                                                {
                                                    Type = "box",
                                                    Layout = "horizontal",
                                                    Contents = new[]
                                                    {
                                                        new
                                                        {
                                                            Type="button",
                                                            Style= "primary",
                                                            Action = new
                                                            {
                                                                Type = "uri",
                                                                Label = "เข้าสู่ระบบ",
                                                                Uri = "https://ts2digitalclaim.rvp.co.th"
                                                            }
                                                        }
                                                    }
                                                }                                           
                                        }
                                    },
                            }
                        });
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    resp = JsonConvert.DeserializeObject<object>(result);
                }
            }
            catch (HttpRequestException e)
            {
                resp = "Error";
            }
            return Ok(resp);
        }

        public string UrlEncode(string str)
        {
            if (str == null || str == "")
            {
                return null;
            }

            byte[] bytesToEncode = System.Text.UTF8Encoding.UTF8.GetBytes(str);
            String returnVal = System.Convert.ToBase64String(bytesToEncode);

            return returnVal.TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public string UrlDecode(string str)
        {
            if (str == null || str == "")
            {
                return null;
            }

            str.Replace('-', '+');
            str.Replace('_', '/');

            int paddings = str.Length % 4;
            if (paddings > 0)
            {
                str += new string('=', 4 - paddings);
            }

            byte[] encodedDataAsBytes = System.Convert.FromBase64String(str);
            string returnVal = System.Text.UTF8Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnVal;
        }

        static string privateKey = @"-----BEGIN RSA PRIVATE KEY-----
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
-----END RSA PRIVATE KEY-----
";

        public class RequestDOPA
        {
            public string client_code { get; set; }
            public string key_name { get; set; }
            public string request_time { get; set; }
            public string signature { get; set; }

        }
        [HttpGet("DOPAtest")]
        public async Task<IActionResult> testDOPA()
        {
            var content = "A62" + "|" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            byte[] pemprivatekey = DecodeOpenSSLPrivateKey(privateKey);
            byte[] byteSign = Encoding.ASCII.GetBytes(content);
            var rsa = DecodeRSAPrivateKey(pemprivatekey);
            var byteRSA = rsa.SignData(byteSign, CryptoConfig.MapNameToOID("SHA256"));
            string rsaBase64 = Convert.ToBase64String(byteRSA);
            string signature = UrlEncode(rsaBase64);

            ////string URL = "https://digitalgatewaytest.digital-access.com/api/auth/token";


            //RequestDOPA req = new RequestDOPA
            //{
            //    client_code = "A62",
            //    key_name = "A62_key.pub",
            //    request_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
            //    signature = signature
            //};
            //var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    string json = new JavaScriptSerializer().Serialize(req);
            //    streamWriter.Write(json);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}

            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //}
            var url = "https://digitalgatewaytest.digital-access.com/api/auth/token";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";

            var data = @"{
    ""client_code"": ""A62"",
    ""key_name"": ""A62_key.pub"",
    ""request_time"": ""2022-02-03T16:58:33"",
    ""signature"": ""WHBLajZRc1VYKzZvMitoZmNEUmR6aDlDUUdKM0FSYXJBQ0RpaFk5TUR5ZWZaUll4NXNuVmZvaTk3bkxuWlBMalZsWE1FMC9YclNwMCs5K2Y1RG5rOEZJZ2FJbU5NMk1kMnVBSE51OW9UVXNJWGJ6czNBbWVGd0czL202K0ZxZnpFTE5QVkdUd1BZQ1JwVnlEdmpyODRKbU5EdWlpR0c1eFpIeGI4dDBnalJOaCt2WkhIUFFrR2F5RXU0b2tGM0w2ME05ZENXNG4rQXZFWk1FWXBMQnpTbGRnczhUSWxiYzdSenFnTGJPWWNhTWNHb0NUbU04bFZ0UHBiaWRhejljMGdWZWw1cXVMUzBRUDFwdTdDSEI4TGJrTnFJeWRoZXoybUxtK1NwZ21jQ2QrSmxkdzFpcXFubkFYR2dKSUdFTFNzMUdQaERhQjh5UDkwT1lOdGk1OUs5ZXV5c2hsdUkwaWFyOEZBaTU5OVR2TlVHQmhoZnQ2a1JWYk5MN1lLRGFkbENJUlZMVGNSaGxWZWFCbSs4NlAzbHV2dmtGeW9kdGVuYUVrOVY5bStiZ0FBVnJjdHN4eFBkRVZRUENDTkYrWnc0NUNNOUljVC96Wms4OFVOeTJMRmVGdUJmSEJ2SjQ4dnFqWEcxZnpzbnh3VWpmV3p5ODVWbExhdkJkZ0VYZUdWYWQyKzRDMDZJRmRVU0FiY1ZTMHY4YTd4b1FORTRscW5KVTNyWWUrb1FsdFVPUlAvOGhadjdYcVVKUCtLT1NIL29vZVBUbUJHbjFMOFdhK2FiYnE0MlhTMFM0SmUwYVM2VmtTUWEwKzQ0WEhMUmxPWThRb0hWelVNVUdmb1ZoY0xwekh2dFZYTk1nS0VVdVE4U01KYko4YlBBUkN1bmdOQitGbTBUQmFyZnM9""
}";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }



            return Ok(new { time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), signature = signature });
        }

        

        public static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();        //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();       //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        public static byte[] DecodeOpenSSLPrivateKey(String instr)
        {
            const String pemprivheader = "-----BEGIN RSA PRIVATE KEY-----";
            const String pemprivfooter = "-----END RSA PRIVATE KEY-----";
            String pemstr = instr.Trim();
            byte[] binkey;
            if (!pemstr.StartsWith(pemprivheader) || !pemstr.EndsWith(pemprivfooter))
                return null;

            StringBuilder sb = new StringBuilder(pemstr);
            sb.Replace(pemprivheader, "");  //remove headers/footers, if present
            sb.Replace(pemprivfooter, "");

            String pvkstr = sb.ToString().Trim();   //get string after removing leading/trailing whitespace

            try
            {        // if there are no PEM encryption info lines, this is an UNencrypted PEM private key
                binkey = Convert.FromBase64String(pvkstr);
                return binkey;
            }
            catch (System.FormatException)
            {       //if can't b64 decode, it must be an encrypted private key
                    //Console.WriteLine("Not an unencrypted OpenSSL PEM private key");  
            }

            StringReader str = new StringReader(pvkstr);

            //-------- read PEM encryption info. lines and extract salt -----
            if (!str.ReadLine().StartsWith("Proc-Type: 4,ENCRYPTED"))
                return null;
            String saltline = str.ReadLine();
            if (!saltline.StartsWith("DEK-Info: DES-EDE3-CBC,"))
                return null;
            String saltstr = saltline.Substring(saltline.IndexOf(",") + 1).Trim();
            byte[] salt = new byte[saltstr.Length / 2];
            for (int i = 0; i < salt.Length; i++)
                salt[i] = Convert.ToByte(saltstr.Substring(i * 2, 2), 16);
            if (!(str.ReadLine() == ""))
                return null;

            //------ remaining b64 data is encrypted RSA key ----
            String encryptedstr = str.ReadToEnd();

            try
            {   //should have b64 encrypted RSA key now
                binkey = Convert.FromBase64String(encryptedstr);
            }
            catch (System.FormatException)
            {  // bad b64 data.
                return null;
            }

            //------ Get the 3DES 24 byte key using PDK used by OpenSSL ----

            SecureString despswd = GetSecPswd("Enter password to derive 3DES key==>");
            //Console.Write("\nEnter password to derive 3DES key: ");
            //String pswd = Console.ReadLine();
            byte[] deskey = GetOpenSSL3deskey(salt, despswd, 1, 2);    // count=1 (for OpenSSL implementation); 2 iterations to get at least 24 bytes
            if (deskey == null)
                return null;
            //showBytes("3DES key", deskey) ;

            //------ Decrypt the encrypted 3des-encrypted RSA private key ------
            byte[] rsakey = DecryptKey(binkey, deskey, salt);   //OpenSSL uses salt value in PEM header also as 3DES IV
            if (rsakey != null)
                return rsakey;  //we have a decrypted RSA private key
            else
            {
                Console.WriteLine("Failed to decrypt RSA private key; probably wrong password.");
                return null;
            }
        }

        private static byte[] GetOpenSSL3deskey(byte[] salt, SecureString secpswd, int count, int miter)
        {
            IntPtr unmanagedPswd = IntPtr.Zero;
            int HASHLENGTH = 16;    //MD5 bytes
            byte[] keymaterial = new byte[HASHLENGTH * miter];     //to store contatenated Mi hashed results


            byte[] psbytes = new byte[secpswd.Length];
            unmanagedPswd = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
            Marshal.Copy(unmanagedPswd, psbytes, 0, psbytes.Length);
            Marshal.ZeroFreeGlobalAllocAnsi(unmanagedPswd);

            //UTF8Encoding utf8 = new UTF8Encoding();
            //byte[] psbytes = utf8.GetBytes(pswd);

            // --- contatenate salt and pswd bytes into fixed data array ---
            byte[] data00 = new byte[psbytes.Length + salt.Length];
            Array.Copy(psbytes, data00, psbytes.Length);        //copy the pswd bytes
            Array.Copy(salt, 0, data00, psbytes.Length, salt.Length);   //concatenate the salt bytes

            // ---- do multi-hashing and contatenate results  D1, D2 ...  into keymaterial bytes ----
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = null;
            byte[] hashtarget = new byte[HASHLENGTH + data00.Length];   //fixed length initial hashtarget

            for (int j = 0; j < miter; j++)
            {
                // ----  Now hash consecutively for count times ------
                if (j == 0)
                    result = data00;    //initialize 
                else
                {
                    Array.Copy(result, hashtarget, result.Length);
                    Array.Copy(data00, 0, hashtarget, result.Length, data00.Length);
                    result = hashtarget;
                    //Console.WriteLine("Updated new initial hash target:") ;
                    //showBytes(result) ;
                }

                for (int i = 0; i < count; i++)
                    result = md5.ComputeHash(result);
                Array.Copy(result, 0, keymaterial, j * HASHLENGTH, result.Length);  //contatenate to keymaterial
            }
            //showBytes("Final key material", keymaterial);
            byte[] deskey = new byte[24];
            Array.Copy(keymaterial, deskey, deskey.Length);

            Array.Clear(psbytes, 0, psbytes.Length);
            Array.Clear(data00, 0, data00.Length);
            Array.Clear(result, 0, result.Length);
            Array.Clear(hashtarget, 0, hashtarget.Length);
            Array.Clear(keymaterial, 0, keymaterial.Length);

            return deskey;
        }
        public static byte[] DecryptKey(byte[] cipherData, byte[] desKey, byte[] IV)
        {
            MemoryStream memst = new MemoryStream();
            TripleDES alg = TripleDES.Create();
            alg.Key = desKey;
            alg.IV = IV;
            try
            {
                CryptoStream cs = new CryptoStream(memst, alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
            byte[] decryptedData = memst.ToArray();
            return decryptedData;
        }
        private static SecureString GetSecPswd(String prompt)
        {
            SecureString password = new SecureString();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(prompt);
            Console.ForegroundColor = ConsoleColor.Magenta;

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    return password;
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    // remove the last asterisk from the screen...
                    if (password.Length > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        password.RemoveAt(password.Length - 1);
                    }
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    return password;
                }
                else if (Char.IsLetterOrDigit(cki.KeyChar) || Char.IsSymbol(cki.KeyChar))
                {
                    if (password.Length < 20)
                    {
                        password.AppendChar(cki.KeyChar);
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
                else
                {
                    Console.Beep();
                }
            }
        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)     //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
            if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }



            while (binr.ReadByte() == 0x00)
            {   //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);       //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }
}
