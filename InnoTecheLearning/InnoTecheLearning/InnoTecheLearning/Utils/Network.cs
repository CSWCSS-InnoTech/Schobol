using System.Net.Http;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static async ValueTask<string[]> Login(ushort StudentID = 18999, string PassPhrase = "Y1234567")
        {
            return (await Request(HttpMethod.Post, "http://cloud.pedosa.org/platform/solutions/cswcss-innotech/test/index.php",
             "STUDENT_ID=s" + StudentID.ToString() + "&STUDENT_PASSPHRASE=" + PassPhrase)).Split(',');
        }
        public static async ValueTask<string> Request(HttpMethod Method, string URI, string Parameters = null)
        //, string ProxyString
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = Method.Method;
            if(!string.IsNullOrEmpty(Parameters))
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(await req.GetRequestStreamAsync()))
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            {
                //req.ContentLength = sw.Encoding.GetBytes(Parameters).Length;
                sw.Write(Parameters); //Push it out there
                sw.Flush();
            }
            System.Net.WebResponse resp = await req.GetResponseAsync();
            if (resp == null) return null;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream()))
                return sr.ReadToEnd().Trim();
        }
        public static async Task<string> RequestAsync(HttpMethod Method, string URI, string Parameters)
            //, string ProxyString
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = Method.Method;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(await req.GetRequestStreamAsync()))
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            {
                //req.ContentLength = sw.Encoding.GetBytes(Parameters).Length;
                sw.Write(Parameters); //Push it out there
                sw.Flush();
            }
            System.Net.WebResponse resp = await req.GetResponseAsync();
            if (resp == null) return null;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream()))
                return sr.ReadToEnd().Trim();
        }
    }
}
