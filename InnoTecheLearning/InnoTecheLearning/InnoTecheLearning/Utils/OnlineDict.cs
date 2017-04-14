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
        //public ([^ ]+) ([^ ]+) { get; set; }
        //[DataMember] public $1 $2;
        public static class OnlineDict
        {
            [DataContract] public abstract class OnlineResponse { internal OnlineResponse() { } }
            [DataContract] public class DictionaryResponse : OnlineResponse
            {
                public class Sens
                {
                    [DataMember] public string translation;
                    [DataMember] public string synonym;
                }

                public class Result
                {
                    [DataMember] public List<string> datasets;
                    [DataMember] public string headword;
                    [DataMember] public string id;
                    [DataMember] public string part_of_speech;
                    [DataMember] public List<Sens> senses;
                    [DataMember] public string url;
                }
                [DataMember] public int status;
                [DataMember] public int offset;
                [DataMember] public int limit;
                [DataMember] public int count;
                [DataMember] public int total;
                [DataMember] public string url;
                [DataMember] public List<Result> results;
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