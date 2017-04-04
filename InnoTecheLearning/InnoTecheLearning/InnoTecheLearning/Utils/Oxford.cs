using System;
using System.IO;
using static System.Text.Encoding;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static class Oxford
        {
            [DataContract]
            public struct TranslateResponse
            {
                public struct Results
                {
                    [DataMember] public string id;
                    [DataMember] public string language;
                    [DataMember] public LexicalEntry[] lexicalEntries;
                    public struct LexicalEntry
                    {
                        [DataMember] public DerivativeOf[] derivativeOf;
                        public struct DerivativeOf
                        {[DataMember] public string[] domains;
                            [DataMember] public string id;
                            [DataMember] public string language;
                            [DataMember] public string[] regions;
                            [DataMember] public string[] registers;
                            [DataMember] public string text;
                        }
                        [DataMember] public Entry[] entries;
                        public struct Entry
                        {
                            [DataMember] public string[] etymologies;
                            [DataMember] public GrammaticalFeature[] grammaticalFeatures;
                            public struct GrammaticalFeature
                            {
                                [DataMember] public string text;
                                [DataMember] public string type;
                            }
                        }
                    }
                }
                public struct Metadata
                {
                    [DataMember] public int limit;
                    [DataMember] public string provider;
                    [DataMember] public int offset;
                    [DataMember] public string sourceLanguage;
                    [DataMember] public int total;
                }
                [DataMember] public Results[] results;
                [DataMember] public Metadata metadata;
            }
            public static T Deserialize<T>(string Data)
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(new MemoryStream(UTF8.GetBytes(Data)));
            }
        }
    }
}