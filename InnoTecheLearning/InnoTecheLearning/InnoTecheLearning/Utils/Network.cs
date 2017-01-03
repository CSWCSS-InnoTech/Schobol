using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class AbnormalReturnException<TReturn> : HttpRequestException
        {   public TReturn ReturnValue { get; }
            public AbnormalReturnException(TReturn Value) : base()
            { ReturnValue = Value; }
            public AbnormalReturnException(TReturn Value, string Message) : base(Message)
            { ReturnValue = Value; }
            public AbnormalReturnException(TReturn Value, string Message, Exception Inner) : base(Message, Inner)
            { ReturnValue = Value; }
        }
        public static string[] Login(ushort StudentID = 18999, string PassPhrase = "Y1234567")
        {
            return Request(Post, "http://cloud.pedosa.org/platform/solutions/cswcss-innotech/test/index.php",
             "STUDENT_ID=s" + StudentID.ToString() + "&STUDENT_PASSPHRASE=" + PassPhrase).Split(',');
        }
        public const string Get = "GET";
        public const string Post = "POST";
        public static string Request(string Method, string URI, string Parameters)
        //, string ProxyString
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = Method;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Do(req.GetRequestStreamAsync())))
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            {
                //req.ContentLength = sw.Encoding.GetBytes(Parameters).Length;
                sw.Write(Parameters); //Push it out there
                sw.Flush();
            }
            System.Net.WebResponse resp = Do(req.GetResponseAsync());
            if (resp == null) return null;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream()))
                return sr.ReadToEnd().Trim();
        }
        public static async Task<string> RequestAsync(string Method, string URI, string Parameters)
            //, string ProxyString
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = Method;
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
