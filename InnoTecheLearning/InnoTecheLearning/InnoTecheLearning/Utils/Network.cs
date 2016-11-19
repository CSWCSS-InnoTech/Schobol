using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static string[] Login(ushort StudentID/*18999*/, string PassPhrase/*Y1234567*/)
        { return POST(new Uri("http://cloud.pedosa.org"), "/solutions/cswcss-innotech/test/index.php",
            "STUDENT_ID=s"+StudentID.ToString()+"&STUDENT_PASSPHRASE="+PassPhrase).Split(','); }
        public static string POST(Uri BaseAddress, string RequestPath, string Content)
            //params KeyValuePair<string, string>[] Content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                //string jsonData = @"{""username"" : ""myusername"", ""password"" : ""mypassword""}";
                //string Input = "";
                /*foreach (KeyValuePair<string, string> In in Content)
                {
                    Input += In.Key + '=' + In.Value + '&';
                }
                Input = Input.TrimEnd('&');*/
                using (var content = new StringContent(Content/*jsonData*/,
                     Encoding.UTF8/*, "application/json"*/))
                using (HttpResponseMessage response = Do(client.PostAsync(RequestPath, content)))

                    // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
                    return Do(response.Content.ReadAsStringAsync());
            }
        }
    }
}
