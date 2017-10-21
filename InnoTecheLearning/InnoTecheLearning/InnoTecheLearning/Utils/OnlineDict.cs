using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using static System.Text.Encoding;
using static System.Uri;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage
    ("Style", "IDE1006:Naming Styles", Justification = "Following JSON model schema from the Pearson Dictionaries API.",
    Scope = "type", Target = "~T:InnoTecheLearning.Utils.PearsonDictionaryResponse")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage
    ("Style", "IDE1006:Naming Styles", Justification = "Following JSON model schema from the Pearson Dictionaries API.",
    Scope = "type", Target = "~T:InnoTecheLearning.Utils.PearsonDictionaryIDResponse")]

namespace InnoTecheLearnUtilities
{
    partial class Utils
    {
        //http://json2csharp.com/
        //public ([^ ]+) ([^ ]+) { get; set; }
        //public $1 $2;
        public static class OnlineDict
        {
            public struct Entry
            {
                public string Headword;
                public string PoS; //Or Pinyin
                public string Translation;
                public Entry(string Headword, string PoS, string Translation)
                {
                    this.Headword = Headword;
                    this.PoS = PoS;
                    this.Translation = Translation;
                }
            }
            public abstract class DictionaryResponse
            {
                internal DictionaryResponse() { }
                public abstract IEnumerable<Entry> Entries { get; }
            }
            public sealed class PearsonDictionaryResponse : DictionaryResponse
            {
                public sealed class Sens
                {
                    public string translation;
                    public string synonym;
                    public string lexical_unit;
                }

                public sealed class Result
                {
                    public List<string> datasets;
                    public string headword;
                    public string id;
                    public string part_of_speech;
                    public List<Sens> senses;
                    public string url;

                    public override bool Equals(object obj) => obj is Result R ? R.id == id : false;
                    public override int GetHashCode() => unchecked(headword.GetHashCode() / id.GetHashCode() - url.GetHashCode());
                    public static bool operator ==(Result R1, Result R2) => R1.id == R2.id;
                    public static bool operator !=(Result R1, Result R2) => R1.id != R2.id;
                }
                public int status;
                public int offset;
                public int limit;
                public int count;
                public int total;
                public string url;
                public List<Result> results;

                public IEnumerable<Entry> data = default;
                public override IEnumerable<Entry> Entries =>
                    data == default(IEnumerable<Entry>) ?
                    data = results.Select(r => new Entry(r.headword, r.part_of_speech, r.senses.Single().translation)) : data;
            }

            public sealed class PearsonDictionaryIDResponse
            {
                public sealed class GramaticalInfo
                {
                    public string type;
                }

                public sealed class Pronunciation
                {
                    public string ipa;
                    public string kk;
                }

                public sealed class Sens
                {
                    public string translation;
                    public string lexical_unit;
                }

                public sealed class Result
                {
                    public List<string> datasets;
                    public GramaticalInfo gramatical_info;
                    public string headword;
                    public string id;
                    public string part_of_speech;
                    public List<Pronunciation> pronunciations;
                    public List<Sens> senses;
                    public string url;
                }
                public int status;
                public string type;
                public string id;
                public string url;
                public Result result;
            }

            public sealed class PedosaResponse : DictionaryResponse
            {
                public override IEnumerable<Entry> Entries { get; }
                public PedosaResponse(IEnumerable<Entry> Entries) => this.Entries = Entries;
            }

            public sealed class NullResponse : DictionaryResponse
            {
                public override IEnumerable<Entry> Entries => Enumerable.Empty<Entry>();
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

            public static T JsonDeserialize<T>(string Data) =>
                Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Data);

            public static PedosaResponse
                PedosaDeserialize(string Data, string Headword)
            {
                Data = Data.Trim('\r', '\n');
                var Pieces = Data.Split('^', '#');
                //Pieces = Pieces.Select(s => s == "NULL" ? null : s).ToArray();
                var Return = new List<Entry>();
                for (int i = 0; i < Pieces.Length / 2; i++)
                {
                    var Processed = ProcessContent(Headword, Pieces[i * 2]);
                    Return.Add(new Entry(Processed.Headword, Processed.PoS, Pieces[i * 2 + 1]));
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


            public static async ValueTask<string> Request(Uri uri)
            {
                using (var Message = new System.Net.Http.HttpRequestMessage
                {
                    RequestUri = uri,
                    Method = System.Net.Http.HttpMethod.Get
                })
                {
                    Message.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    using (var Client = new System.Net.Http.HttpClient())
                        return await (await Client.SendAsync(Message)).Content.ReadAsStringAsync();
                }
            }


            public static async ValueTask<PearsonDictionaryResponse> PearsonToChinese(string Word) =>
                ProcessPearsonResponse(JsonDeserialize<PearsonDictionaryResponse>(await Request
            (new Uri("http://api.pearson.com/v2/dictionaries/ldec/entries?headword=" + EscapeDataString(Word.ToLower())))));
            public static async ValueTask<PearsonDictionaryIDResponse> PearsonLookupID(string ID) =>
                JsonDeserialize<PearsonDictionaryIDResponse>(await Request
                (new Uri("http://api.pearson.com/v2/dictionaries/entries/" + EscapeDataString(ID))));

            public static async ValueTask<PedosaResponse> PedosaToChinese(string Word) =>
                PedosaDeserialize(await Request
                (new Uri("http://pedosa.cloud/api/dictionary/query.php?Language=English&Word=" + EscapeDataString(Word.ToLower()))), Word.ToLower());

            public static async ValueTask<PedosaResponse> PedosaToEnglish(string Word) =>
                PedosaDeserialize(await Request
                (new Uri("http://pedosa.cloud/api/dictionary/query.php?Language=Chinese&Word=" + EscapeDataString(Word))), Word);

            public static bool ToEnglishMode = false;
            public static bool UsePearson = false;
            public static async ValueTask<DictionaryResponse> ToChinese(string Word)
            { if (UsePearson) return await PearsonToChinese(Word); else return await PedosaToChinese(Word); }
            public static async ValueTask<DictionaryResponse> ToEnglish(string Word)
            {
                if (UsePearson) return new NullResponse();
                return await PedosaToEnglish(Word);
            }
            public static ValueTask<DictionaryResponse> Convert(string Word) => ToEnglishMode ? ToEnglish(Word) : ToChinese(Word);
        }
    }
}