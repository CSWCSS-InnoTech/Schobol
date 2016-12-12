#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace InnoTecheLearning
{
    partial class Utils
    {
        class Updater : Android.App.Activity
        {
            int count = 1;

            CancellationTokenSource currentToken;

            /// <summary>
            /// Max 10000
            /// </summary>
            int progress;

            void HandleDownloadProgress(long bytes, long totalBytes, long totalBytesExpected)
            {
                Console.WriteLine("Downloading {0}/{1}", totalBytes, totalBytesExpected);

                RunOnUiThread(() =>
                {
                    var progressPercent = (float)totalBytes / (float)totalBytesExpected;
                    var progressOffset = Convert.ToInt32(progressPercent * 10000);

                    Console.WriteLine(progressOffset);
                    progress = progressOffset;
                });
            }

            protected override void OnCreate(Bundle bundle)
            {
                base.OnCreate(bundle);

                //This API is only available in Mono and Xamarin products.
                //You can filter and/or re-order the ciphers suites that the SSL/TLS server will accept from a client.
                //The following example removes weak (export) ciphers from the list that will be offered to the server.
                ServicePointManager.ClientCipherSuitesCallback += (protocol, allCiphers) =>
                    allCiphers.Where(x => !x.Contains("EXPORT")).ToList();

                //Here we accept any certificate and just print the cert's data.
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    System.Diagnostics.Debug.WriteLine("Callback Server Certificate: " + sslPolicyErrors);

                    foreach (var el in chain.ChainElements)
                    {
                        System.Diagnostics.Debug.WriteLine(el.Certificate.GetCertHashString());
                        System.Diagnostics.Debug.WriteLine(el.Information);
                    }

                    return true;
                };

                var resp = default(HttpResponseMessage);

                Cancel += (o, e) =>
                {
                    Console.WriteLine("Canceled token {0:x8}", this.currentToken.Token.GetHashCode());
                    this.currentToken.Cancel();
                    if (resp != null) resp.Content.Dispose();
                };

                Start += async (o, e) =>
                {
                    var handler = new NativeMessageHandler();
                    var client = new HttpClient(handler);

                    currentToken = new CancellationTokenSource();
                    var st = new Stopwatch();

                    client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("ModernHttpClient", "1.0"));
                    client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("MyTest", "2.0"));
                    handler.DisableCaching = true;

                    st.Start();
                    try
                    {
                        //var url = "https://tv.eurosport.com";
                        //var url = "https://github.com/downloads/nadlabak/android/cm-9.1.0a-umts_sholes.zip";
                        var url = "https://github.com/paulcbetts/ModernHttpClient/releases/download/0.9.0/ModernHttpClient-0.9.zip";

                        var request = new HttpRequestMessage(HttpMethod.Get, url);
                        handler.RegisterForProgress(request, HandleDownloadProgress);

                        resp = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, currentToken.Token);
                        Result = "Got the headers!";

                        Status = string.Format("HTTP {0}: {1}", (int)resp.StatusCode, resp.ReasonPhrase);

                        foreach (var v in resp.Headers)
                        {
                            Console.WriteLine("{0}: {1}", v.Key, String.Join(",", v.Value));
                        }

                        var stream = await resp.Content.ReadAsStreamAsync();

                        var ms = new MemoryStream();
                        await stream.CopyToAsync(ms, 4096, currentToken.Token);
                        var bytes = ms.ToArray();

                        Result = String.Format("Read {0} bytes", bytes.Length);

                        var md5 = MD5.Create();
                        var hash = md5.ComputeHash(bytes);
                        Hash = ToHex(hash, false);
                    }
                    catch (Exception ex)
                    {
                        Result = ex.ToString();
                    }
                    finally
                    {
                        st.Stop();
                        Done?.Invoke(this, null);
                        Result = (Result ?? "") + String.Format("\n\nTook {0} milliseconds", st.ElapsedMilliseconds);
                    }
                };
            }
            public event EventHandler Cancel;
            public event EventHandler Start;
            public event EventHandler Done;
            public string Result { get; private set; }
            public string Status { get; private set; }
            public string Hash { get; private set; }
            public static string ToHex(byte[] bytes, bool upperCase)
            {
                var result = new StringBuilder(bytes.Length * 2);

                for (int i = 0; i < bytes.Length; i++)
                    result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

                return result.ToString();
            }
            public class NativeMessageHandler : HttpClientHandler
            {
                const string wrongVersion = "You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version";

                public bool DisableCaching { get; set; }

                /// <summary>
                /// Initializes a new instance of the <see
                /// cref="ModernHttpClient.Portable.NativeMessageHandler"/> class.
                /// </summary>
                public NativeMessageHandler() : base()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see
                /// cref="ModernHttpClient.Portable.NativeMessageHandler"/> class.
                /// </summary>
                /// <param name="throwOnCaptiveNetwork">If set to <c>true</c> throw on
                /// captive network (ie: a captive network is usually a wifi network
                /// where an authentication html form is shown instead of the real
                /// content).</param>
                /// <param name="customSSLVerification">Enable custom SSL certificate 
                /// verification via ServicePointManager. Disabled by default for 
                /// performance reasons (i.e. the OS default certificate verification 
                /// will take place)</param>
                /// <param name="cookieHandler">Enable native cookie handling.
                /// </param>
                public NativeMessageHandler(bool throwOnCaptiveNetwork, bool customSSLVerification, NativeCookieHandler cookieHandler = null) : base()
                {
                }

                public void RegisterForProgress(HttpRequestMessage request, ProgressDelegate callback)
                {
                    throw new Exception(wrongVersion);
                }
            }

            public class ProgressStreamContent : StreamContent
            {
                const string wrongVersion = "You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version";

                ProgressStreamContent(Stream stream) : base(stream)
                {
                    throw new Exception(wrongVersion);
                }

                ProgressStreamContent(Stream stream, int bufferSize) : base(stream, bufferSize)
                {
                    throw new Exception(wrongVersion);
                }

                public ProgressDelegate Progress
                {
                    get { throw new Exception(wrongVersion); }
                    set { throw new Exception(wrongVersion); }
                }
            }

            public delegate void ProgressDelegate(long bytes, long totalBytes, long totalBytesExpected);

            public class NativeCookieHandler
            {
                const string wrongVersion = "You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version";

                public void SetCookies(IEnumerable<Cookie> cookies)
                {
                    throw new Exception(wrongVersion);
                }

                public List<Cookie> Cookies
                {
                    get { throw new Exception(wrongVersion); }
                }
            }
        }
    }
}
#endif