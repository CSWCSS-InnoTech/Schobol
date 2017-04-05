using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using static System.Text.Encoding;

namespace InnoTecheLearning
{
    partial class Utils
    {
        //http://json2csharp.com/
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Following JSON model schema from Oxford Dictionaries.")]
        //public (?!class)
        //[DataMember] public 
        public static class Oxford
        {
            public abstract class OxfordResponse { internal OxfordResponse() { } }
            [DataContract]
            public class TranslateResponse : OxfordResponse
            {
                public class Metadata
                {
                }

                public class DerivativeOf
                {
                    [DataMember] public List<string> domains { get; set; }
                    [DataMember] public string id { get; set; }
                    [DataMember] public string language { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public List<string> registers { get; set; }
                    [DataMember] public string text { get; set; }
                }

                public class GrammaticalFeature
                {
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Note
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Pronunciation
                {
                    [DataMember] public string audioFile { get; set; }
                    [DataMember] public List<string> dialects { get; set; }
                    [DataMember] public string phoneticNotation { get; set; }
                    [DataMember] public string phoneticSpelling { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                }

                public class CrossReference
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Note2
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class GrammaticalFeature2
                {
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Note3
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Translation
                {
                    [DataMember] public List<string> domains { get; set; }
                    [DataMember] public List<GrammaticalFeature2> grammaticalFeatures { get; set; }
                    [DataMember] public string language { get; set; }
                    [DataMember] public List<Note3> notes { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public List<string> registers { get; set; }
                    [DataMember] public string text { get; set; }
                }

                public class Example
                {
                    [DataMember] public List<string> definitions { get; set; }
                    [DataMember] public List<string> domains { get; set; }
                    [DataMember] public List<Note2> notes { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public List<string> registers { get; set; }
                    [DataMember] public List<string> senseIds { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public List<Translation> translations { get; set; }
                }

                public class Note4
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Pronunciation2
                {
                    [DataMember] public string audioFile { get; set; }
                    [DataMember] public List<string> dialects { get; set; }
                    [DataMember] public string phoneticNotation { get; set; }
                    [DataMember] public string phoneticSpelling { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                }

                public class Subsens
                {
                }

                public class GrammaticalFeature3
                {
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Note5
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Translation2
                {
                    [DataMember] public List<string> domains { get; set; }
                    [DataMember] public List<GrammaticalFeature3> grammaticalFeatures { get; set; }
                    [DataMember] public string language { get; set; }
                    [DataMember] public List<Note5> notes { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public List<string> registers { get; set; }
                    [DataMember] public string text { get; set; }
                }

                public class VariantForm
                {
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public string text { get; set; }
                }

                public class Sens
                {
                    [DataMember] public List<string> crossReferenceMarkers { get; set; }
                    [DataMember] public List<CrossReference> crossReferences { get; set; }
                    [DataMember] public List<string> definitions { get; set; }
                    [DataMember] public List<string> domains { get; set; }
                    [DataMember] public List<Example> examples { get; set; }
                    [DataMember] public string id { get; set; }
                    [DataMember] public List<Note4> notes { get; set; }
                    [DataMember] public List<Pronunciation2> pronunciations { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public List<string> registers { get; set; }
                    [DataMember] public List<Subsens> subsenses { get; set; }
                    [DataMember] public List<Translation2> translations { get; set; }
                    [DataMember] public List<VariantForm> variantForms { get; set; }
                }

                public class VariantForm2
                {
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public string text { get; set; }
                }

                public class Entry
                {
                    [DataMember] public List<string> etymologies { get; set; }
                    [DataMember] public List<GrammaticalFeature> grammaticalFeatures { get; set; }
                    [DataMember] public string homographNumber { get; set; }
                    [DataMember] public List<Note> notes { get; set; }
                    [DataMember] public List<Pronunciation> pronunciations { get; set; }
                    [DataMember] public List<Sens> senses { get; set; }
                    [DataMember] public List<VariantForm2> variantForms { get; set; }
                }

                public class GrammaticalFeature4
                {
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Note6
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public string type { get; set; }
                }

                public class Pronunciation3
                {
                    [DataMember] public string audioFile { get; set; }
                    [DataMember] public List<string> dialects { get; set; }
                    [DataMember] public string phoneticNotation { get; set; }
                    [DataMember] public string phoneticSpelling { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                }

                public class VariantForm3
                {
                    [DataMember] public List<string> regions { get; set; }
                    [DataMember] public string text { get; set; }
                }

                public class LexicalEntry
                {
                    [DataMember] public List<DerivativeOf> derivativeOf { get; set; }
                    [DataMember] public List<Entry> entries { get; set; }
                    [DataMember] public List<GrammaticalFeature4> grammaticalFeatures { get; set; }
                    [DataMember] public string language { get; set; }
                    [DataMember] public string lexicalCategory { get; set; }
                    [DataMember] public List<Note6> notes { get; set; }
                    [DataMember] public List<Pronunciation3> pronunciations { get; set; }
                    [DataMember] public string text { get; set; }
                    [DataMember] public List<VariantForm3> variantForms { get; set; }
                }

                public class Pronunciation4
                {
                    [DataMember] public string audioFile { get; set; }
                    [DataMember] public List<string> dialects { get; set; }
                    [DataMember] public string phoneticNotation { get; set; }
                    [DataMember] public string phoneticSpelling { get; set; }
                    [DataMember] public List<string> regions { get; set; }
                }

                public class Result
                {
                    [DataMember] public string id { get; set; }
                    [DataMember] public string language { get; set; }
                    [DataMember] public List<LexicalEntry> lexicalEntries { get; set; }
                    [DataMember] public List<Pronunciation4> pronunciations { get; set; }
                    [DataMember] public string type { get; set; }
                    [DataMember] public string word { get; set; }
                }

                [DataMember] public Metadata metadata { get; set; }
                [DataMember] public List<Result> results { get; set; }
            }

            public static T Deserialize<T>(string Data)
            {
                using (var ms = new MemoryStream(UTF8.GetBytes(Data)))
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }

            public static async System.Threading.Tasks.Task<T> Request<T>(System.Uri uri) where T : OxfordResponse
            {
                const string app_id = "a92894ff";
                const string app_key = "efe1df399f27706216f6e7dc6532b849";
                using (var Message = new System.Net.Http.HttpRequestMessage
                {
                    RequestUri = uri,
                    Method = System.Net.Http.HttpMethod.Get
                })
                {
                    Message.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    Message.Properties.Add("app_id", app_id);
                    Message.Properties.Add("app_key", app_key);

                    using (var Client = new System.Net.Http.HttpClient())
                        return Deserialize<T>(await (await Client.SendAsync(Message)).Content.ReadAsStringAsync());
                }
            }

            public static async System.Threading.Tasks.Task<TranslateResponse> Translate(string FromLang, string ToLang, string Word)
            => await Request<TranslateResponse>(new System.Uri(
                $"https://od-api.oxforddictionaries.com:443/api/v1/entries/{FromLang}/{Word.ToLower()}/translations={ToLang}"));
        }
    }
}