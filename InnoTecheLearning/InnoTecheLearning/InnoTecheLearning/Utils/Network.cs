using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static string[] POST(Uri BaseAddress, Uri Request, params KeyValuePair<string, string>[] Content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("cloud.pedosa.org");

                //string jsonData = @"{""username"" : ""myusername"", ""password"" : ""mypassword""}";
                string Input = "";//"STUDENT_ID=s18999&STUDENT_PASSPHRASE=Y1234567"
                foreach (KeyValuePair<string, string> In in Content)
                {
                    Input += In.Key + '=' + In.Value + '&';
                }
                Input = Input.TrimEnd('&');
                using (var content = new StringContent(Input/*jsonData*/,
                     Encoding.UTF8/*, "application/json"*/))
                using (HttpResponseMessage response = Do(client.PostAsync("/solutions/cswcss-innotech/test/index.php", content)))

                    // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
                    return Do(response.Content.ReadAsStringAsync()).Split(',');
            }
        }
    }
}
