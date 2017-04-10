using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using static System.Text.Encoding;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage
    ("Style", "IDE1006:Naming Styles", Justification = "Following JSON model schema from the online dictionary API.",
    Scope = "type", Target = "~T:InnoTecheLearning.Utils.OnlineDict")]

namespace InnoTecheLearning
{
    partial class Utils
    {
        //http://json2csharp.com/
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles",
        //    Justification = "Following JSON model schema from Oxford Dictionaries.")]
        //public (?!class)
        //[DataMember] public 
        public static class OnlineDict
        {
            [DataContract] public abstract class OnlineResponse { internal OnlineResponse() { } }
            public class DictionaryResponse : OnlineResponse
            {
                public class Sens
                {
                    public string translation { get; set; }
                    public string synonym { get; set; }
                }

                public class Result
                {
                    public List<string> datasets { get; set; }
                    public string headword { get; set; }
                    public string id { get; set; }
                    public string part_of_speech { get; set; }
                    public List<Sens> senses { get; set; }
                    public string url { get; set; }
                }
                public int status { get; set; }
                public int offset { get; set; }
                public int limit { get; set; }
                public int count { get; set; }
                public int total { get; set; }
                public string url { get; set; }
                public List<Result> results { get; set; }
            }

            public static T Deserialize<T>(string Data)
            {
                using (var ms = new MemoryStream(UTF8.GetBytes(Data)))
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }

            public static T Request<T>(System.Uri uri) where T : OnlineResponse
            {
                using (var Message = new System.Net.Http.HttpRequestMessage
                {
                    RequestUri = uri,
                    Method = System.Net.Http.HttpMethod.Get
                })
                {
                    Message.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    using (var Client = new System.Net.Http.HttpClient())
                        return Deserialize<T>(Client.SendAsync(Message).Do().Content.ReadAsStringAsync().Do());
                }
            }

            public static DictionaryResponse ToChinese(string Word) => Request<DictionaryResponse>
            (new System.Uri("http://api.pearson.com/v2/dictionaries/ldec/entries?headword=" + Word.ToLower()));
        }
    }
}