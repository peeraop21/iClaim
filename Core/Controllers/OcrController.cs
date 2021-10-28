
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Core.Controllers
{
    public class ImageData
    {
        public string IdCardBase64 { get; set; }
        public string FaceBase64 { get; set; }

    }
    class Result
    {
        public string address { get; set; }
        public string date_of_birth { get; set; }
        public string date_of_expiry { get; set; }
        public string date_of_issue { get; set; }
        public string first_name_en { get; set; }
        public string id { get; set; }
        public string last_name_en { get; set; }
        public string name_th { get; set; }
        public string religion { get; set; }
        public string serial_number { get; set; }
    }
    class ResponseResultCompare
    {
        public float score { get; set; }
    }
    class ResponseOcrIdCard
    {
        public string address { get; set; }
        public string date_of_birth { get; set; }
        public string date_of_expiry { get; set; }
        public string date_of_issue { get; set; }
        public string first_name_en { get; set; }
        public string id { get; set; }
        public string last_name_en { get; set; }
        public string name_th { get; set; }
        public string religion { get; set; }
        public string serial_number { get; set; }
    }

    /// <summay>
    /// API response
    /// </summay>
    class Response
    {
        public string request_id { get; set; }
        public Result result { get; set; }
        public string status { get; set; }
    }
    class ResponseCompare
    {
        public string request_id { get; set; }
        public ResponseResultCompare result { get; set; }
        public string status { get; set; }
        
    }

    [Route("api/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {


        string secretId = "790cdb6a20de3b86fe7d25fc32b4f874";
        string secretKey = "dd65c758b0fffecef30e63724322b0074e863641";
        public OcrController()
        {
           
        }

        [HttpPost("IdCard")]
        public async Task<IActionResult> IdCard([FromBody] ImageData model)
        {
            string url = "https://iai.flashsoftapi.com/v1/thai-id-card-ocr";
            string body = string.Format("image={0}", HttpUtility.UrlEncode(model.IdCardBase64));
            string timestamp = GetTimestamp();
            string nonce = GenerateNonce();
            string authorization = GetAuthorization(url, body, timestamp, secretId, secretKey);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ProtocolVersion = HttpVersion.Version11;
            request.Headers.Add("Authorization", authorization);
            request.Headers.Add("X-FC-NONCE", nonce);
            request.Headers.Add("X-FC-TIMESTAMP", timestamp);
            byte[] data = Encoding.UTF8.GetBytes(body);
            request.ContentLength = data.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse response = null;
            int httpStatus = 0;
            string content;
            ResponseOcrIdCard resModel = new ResponseOcrIdCard();
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                httpStatus = (int)response.StatusCode;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = reader.ReadToEnd();
                Response res = JsonSerializer.Deserialize<Response>(content);
                resModel.id = res.result.id;                
                resModel.name_th = res.result.name_th;
                resModel.first_name_en = res.result.first_name_en;
                resModel.last_name_en = res.result.last_name_en;
                resModel.date_of_birth = res.result.date_of_birth;
                resModel.date_of_expiry = res.result.date_of_expiry;
                resModel.date_of_issue = res.result.date_of_issue;
                resModel.religion = res.result.religion;
                resModel.address = res.result.address;
                resModel.serial_number = res.result.serial_number;

            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                httpStatus = (int)response.StatusCode;
                using (Stream errData = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(errData))
                    {
                        content = reader.ReadToEnd();
                        Console.WriteLine("error content: " + content);
                        return Ok(new { status = httpStatus, errorContent = content });
                    }
                }
            }
            return Ok(new { status = httpStatus, responseOcrIdCard = resModel });
        }

        [HttpPost("CompareFace")]
        public async Task<IActionResult> CompareFace([FromBody] ImageData model)
        {
            string url = "https://iai.flashsoftapi.com/v1/compare-face-id-card";
            string body = string.Format("id_card_image={0}&selfie_image={1}", HttpUtility.UrlEncode(model.IdCardBase64), HttpUtility.UrlEncode(model.FaceBase64));                        
            string timestamp = GetTimestamp();
            string nonce = GenerateNonce();
            string authorization = GetAuthorization(url, body, timestamp, secretId, secretKey);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ProtocolVersion = HttpVersion.Version11;
            request.Headers.Add("Authorization", authorization);
            request.Headers.Add("X-FC-NONCE", nonce);
            request.Headers.Add("X-FC-TIMESTAMP", timestamp);
            byte[] data = Encoding.UTF8.GetBytes(body);
            request.ContentLength = data.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse response = null;
            int httpStatus = 0;
            string content;
            ResponseResultCompare resultCompare = new ResponseResultCompare();
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                httpStatus = (int)response.StatusCode;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = reader.ReadToEnd();
               
                ResponseCompare res = JsonSerializer.Deserialize<ResponseCompare>(content);
                if (res.status == "ERROR")
                {
                    return Ok(new { status = res.status, errorContent = "ไม่พบใบหน้าในรูปเปรียบเทียบ กรุณาลองใหม่" });
                }            
                resultCompare.score = res.result.score;                
                return Ok(new { status = res.status, resultCompare = resultCompare });

            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                httpStatus = (int)response.StatusCode;
                using (Stream errData = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(errData))
                    {
                        content = reader.ReadToEnd();
                        Console.WriteLine("error content: " + content);
                        return Ok(new { status = httpStatus, errorContent = content });
                    }
                }
            }
            return Ok(new { status = "", resultCompare = resultCompare });
        }

        private string TimestampToDate(string timestamp)
        {
            double unixTimestamp = Convert.ToDouble(timestamp);
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimestamp).ToUniversalTime(); // Assure the date is UTC+0
            return dateTime.ToString("yyyy-MM-dd");
        }        
        private string GetTimestamp()
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), localZone);
            int time = (int)(DateTime.Now - dt).TotalSeconds;
            String timestamp = time.ToString();
            return timestamp;
        }   
        private string GenerateNonce()
        {
            Random rd = new Random();
            int rd_i = rd.Next();
            string nonce = Convert.ToString(rd_i);
            return nonce;
        }      
        private string GetAuthorization(string urlString, string body, string timestamp, string secretId, string secretKey)
        {
            // Request method is always POST.
            string method = "POST";
            // Request query is always empty when method is POST.
            string query = "";

            var url = new Uri(urlString);
            string host = url.Host;
            string path = url.AbsolutePath;

            // The content type is x-www-form-urlencoded
            string contentType = "application/x-www-form-urlencoded";

            // Get the hash of request body.
            UTF8Encoding enc = new UTF8Encoding();
            string bodyHash = Sha256Hex(body);

            // Concate canonical request string.
            StringBuilder sb = new StringBuilder();
            sb.Append(method);
            sb.Append("\n");
            sb.Append(path);
            sb.Append("\n");
            sb.Append(query);
            sb.Append("\n");
            sb.Append("content-type:");
            sb.Append(contentType);
            sb.Append("\n");
            sb.Append("host:");
            sb.Append(host);
            sb.Append("\n");
            sb.Append("\n");
            sb.Append("content-type;host\n");
            sb.Append(bodyHash);
            string canonicalRequest = sb.ToString();

            // Get the hash of canonical request
            string canonicalRequestHash = Sha256Hex(canonicalRequest);

            // Parse date string from timestamp
            string date = TimestampToDate(timestamp);

            // Concate sign string
            sb.Clear();
            sb.Append("FC1-HMAC-SHA256\n");
            sb.Append(timestamp);
            sb.Append("\n");
            sb.Append(date);
            sb.Append("/th/fc1_request\n");
            sb.Append(canonicalRequestHash);
            string stringToSign = sb.ToString();

            // Get secret key bytes
            byte[] secretKeyExt = enc.GetBytes("FC1" + secretKey);
            // Hash by date
            byte[] secretDate = HmacSha256(date, secretKeyExt);
            // Hash by service location
            byte[] secretService = HmacSha256("th", secretDate);
            // Hash by request name
            byte[] secretSigning = HmacSha256("fc1_request", secretService);
            // Hash by sign string
            string signature = HmacSha256Hex(stringToSign, secretSigning);

            // Concate the signature
            sb.Clear();
            sb.Append("FC1-HMAC-SHA256");
            sb.Append(" ");
            sb.Append("Credential=");
            sb.Append(secretId);
            sb.Append("/");
            sb.Append(date);
            sb.Append("/th/fc1_request");
            sb.Append(", ");
            sb.Append("SignedHeaders=content-type;host");
            sb.Append(", ");
            sb.Append("Signature=");
            sb.Append(signature);
            return sb.ToString();
        }      
        private string Sha256Hex(string data)
        {
            UTF8Encoding enc = new UTF8Encoding();
            byte[] dataToHash = enc.GetBytes(data);
            var sha256 = SHA256.Create();
            var result = sha256.ComputeHash(dataToHash);
            string hex = BitConverter.ToString(result).Replace("-", "").ToLower();
            return hex;
        }       
        private byte[] HmacSha256(string date, byte[] key)
        {
            HMACSHA256 hmac = new HMACSHA256(key);
            UTF8Encoding enc = new UTF8Encoding();
            byte[] byteArray = enc.GetBytes(date);
            MemoryStream stream = new MemoryStream(byteArray);
            byte[] result = hmac.ComputeHash(stream);
            return result;
        }       
        private string HmacSha256Hex(string date, byte[] key)
        {
            byte[] result = HmacSha256(date, key);
            string hex = BitConverter.ToString(result).Replace("-", "").ToLower();
            return hex;
        }

       


    }
}
