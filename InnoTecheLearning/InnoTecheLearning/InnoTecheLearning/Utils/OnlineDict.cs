using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using static System.Text.Encoding;
using static System.Uri;

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
            public struct Entry
            {
                public readonly string Headword;
                public readonly string PoS;
                public readonly string Translation;
                public readonly string Synonym;
                public readonly string LexicalUnit;
                public Entry(string Headword, string PoS, string Translation, string Synonym, string LexicalUnit)
                {
                    this.Headword = Headword;
                    this.PoS = PoS;
                    this.Translation = Translation;
                    this.Synonym = Synonym;
                    this.LexicalUnit = LexicalUnit;
                }
            }
            public abstract class DictionaryResponse
            {
                internal DictionaryResponse() { }
                public abstract IEnumerable<Entry> Entries
                { get; }
            }
            [DataContract] public sealed class PearsonDictionaryResponse : DictionaryResponse
            {
                public sealed class Sens
                {
                    [DataMember] public string translation;
                    [DataMember] public string synonym;
                    [DataMember] public string lexical_unit;
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

                public IEnumerable<Entry> data = default(IEnumerable<Entry>);
                public override IEnumerable<Entry> Entries =>
                    data ==  default(IEnumerable<Entry>) ?
                    data = results.Select(r => new Entry(
                    r.headword, r.part_of_speech, r.senses.Single().translation, r.senses.Single().synonym, r.senses.Single().lexical_unit
                    )) : data;
                }

            [DataContract] public sealed class PearsonDictionaryIDResponse
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

            public sealed class PedosaResponse : DictionaryResponse
            {
                public override 
                    IEnumerable<Entry> Entries { get; }
                public PedosaResponse (IEnumerable<Entry> Entries) => this.Entries = Entries;
            }
            static PearsonDictionaryResponse ProcessPearsonResponse(PearsonDictionaryResponse Data)
            {
                for (int i = 0; i < Data.results.Count; i++)
                {
                    (Data.results[i].headword, Data.results[i].part_of_speech) =
                        ProcessContent(Data.results[i].headword, Data.results[i].part_of_speech);
                }
                return Data;
            }

            public static T JsonDeserialize<T>(string Data)
            {
                using (var ms = new MemoryStream(UTF8.GetBytes(Data)))
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }

            public static PedosaResponse
                PedosaDeserialize(string Data)
            {
                var Pieces = Data.Split('^', '-');
                Pieces = Pieces.Select(s => s == "NULL" ? null : s).ToArray();
                var Return = new Entry[int.Parse(Pieces[0])];
                for (int i = 0; i < Return.Length; i++)
                {
                    var Processed = ProcessContent(Pieces[i * 5 + 1], Pieces[i * 5 + 2]);
                    Return[i] = new Entry(Processed.Headword, Processed.PoS, Pieces[i * 5 + 3], Pieces[i * 5 + 4], Pieces[i * 5 + 5]);
                }
                return new PedosaResponse(Return);
            }
            public static (string Headword, string PoS) ProcessContent(string Headword, string PoS) =>
                (Headword.Replace('’', '\''), 
                new System.Text.StringBuilder(PoS ??
#if DEBUG
                        "noun."
#else
                        "noun"
#endif
                        )
                        .Replace("modal v", "modal verb")
                        .Replace("sfx", "suffix")
                        .Replace("interj", "interjection")
                        .Replace("conj", "conjunction").ToString());


            public static async ValueTask<string> Request(System.Uri uri)
            {
                using (var Message = new System.Net.Http.HttpRequestMessage
                {
                    RequestUri = uri,
                    Method = System.Net.Http.HttpMethod.Get
                })
                {
                    Message.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    using (var Client = new System.Net.Http.HttpClient())
                        return await(await Client.SendAsync(Message)).Content.ReadAsStringAsync();
                }
            }


            public static async ValueTask<PearsonDictionaryResponse> PearsonToChinese(string Word) => 
                ProcessPearsonResponse(JsonDeserialize<PearsonDictionaryResponse>(await Request
            (new System.Uri("http://api.pearson.com/v2/dictionaries/ldec/entries?headword=" + EscapeDataString(Word.ToLower())))));
            public static async ValueTask<PearsonDictionaryIDResponse> PearsonLookupID(string ID) =>
                JsonDeserialize<PearsonDictionaryIDResponse>(await Request
                (new System.Uri("http://api.pearson.com/v2/dictionaries/entries/" + EscapeDataString(ID))));

            public static async ValueTask<PedosaResponse> PedosaToChinese(string Word) =>
                PedosaDeserialize(await Request
                (new System.Uri("http://pedosa.cloud/api/dictionary/eng-chi/query.php?word=" + EscapeDataString(Word.ToLower()))));

            public static bool UsePearson = false;
            public static async ValueTask<DictionaryResponse> ToChinese(string Word) =>
                UsePearson ? await PearsonToChinese(Word) as DictionaryResponse : await PedosaToChinese(Word);
        }
    }
}