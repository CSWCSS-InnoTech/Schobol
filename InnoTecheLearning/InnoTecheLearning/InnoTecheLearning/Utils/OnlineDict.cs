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
            [DataContract] public sealed class DictionaryResponse : OnlineResponse
            {
                public sealed class Sens
                {
                    [DataMember] public string translation;
                    [DataMember] public string synonym;
                }

                public sealed class Result
                {
                    [DataMember] public List<string> datasets;
                    [DataMember] public string headword;
                    [DataMember] public string id;
                    [DataMember] public string part_of_speech;
                    [DataMember] public List<Sens> senses;
                    [DataMember] public string url;

                    public override bool Equals(object obj) => obj is Result R ? R.id == id : false;
                    public override int GetHashCode() => unchecked(headword.GetHashCode() / id.GetHashCode() - url.GetHashCode());
                    public static bool operator ==(Result R1, Result R2) => R1.id == R2.id;
                    public static bool operator !=(Result R1, Result R2) => R1.id != R2.id;
                }
                [DataMember] public int status;
                [DataMember] public int offset;
                [DataMember] public int limit;
                [DataMember] public int count;
                [DataMember] public int total;
                [DataMember] public string url;
                [DataMember] public List<Result> results;
            }

            [DataContract] public sealed class DictionaryIDResponse : OnlineResponse
            {
                public sealed class GramaticalInfo
                {
                    [DataMember] public string type;
                }

                public sealed class Pronunciation
                {
                    [DataMember] public string ipa;
                    [DataMember] public string kk;
                }

                public sealed class Sens
                {
                    [DataMember] public string translation;
                    [DataMember] public string lexical_unit;
                }

                public sealed class Result
                {
                    [DataMember] public List<string> datasets;
                    [DataMember] public GramaticalInfo gramatical_info;
                    [DataMember] public string headword;
                    [DataMember] public string id;
                    [DataMember] public string part_of_speech;
                    [DataMember] public List<Pronunciation> pronunciations;
                    [DataMember] public List<Sens> senses;
                    [DataMember] public string url;
                }
                [DataMember] public int status;
                [DataMember] public string type;
                [DataMember] public string id;
                [DataMember] public string url;
                [DataMember] public Result result;
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

            static DictionaryResponse ProcessPoS(DictionaryResponse Data)
            {
                for (int i = 0; i < Data.results.Count; i++)
                {
                    Data.results[i].headword = Data.results[i].headword.Replace('’', '\'');
                    Data.results[i].part_of_speech =
                        new System.Text.StringBuilder(Data.results[i].part_of_speech ??
#if DEBUG
                        "noun."
#else
                        "noun"
#endif
                        )
                        .Replace("modal v", "modal verb")
                        .Replace("sfx", "suffix")
                        .Replace("interj", "interjection")
                        .Replace("conj", "conjunction").ToString();
                }
                return Data;
            }

            public static DictionaryResponse ToChinese(string Word) => ProcessPoS(Request<DictionaryResponse>
            (new System.Uri("http://api.pearson.com/v2/dictionaries/ldec/entries?headword=" + Word.ToLower())));
            public static DictionaryIDResponse LookupID(string ID) => Request<DictionaryIDResponse>
            (new System.Uri("http://api.pearson.com/v2/dictionaries/entries/" + ID));
        }
    }
}