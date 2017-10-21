using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InnoTecheLearnUtilities
{
    partial class Utils
    {

        /// <summary>
        /// Utility class that can be used to find, get and load embedded resources into memory.
        /// </summary>
        public static class Resources
        {
            // NOTE: These convenience methods are not available in WinRT, but they're available 
            // in Xamarin.iOS and Xamarin.Android, so i'm commenting them out so they build as
            // a PCL lib, but you may want them in your own code if you're not targeting WinRT.
            /// <summary>
            /// Attempts to find and return the given resource from within the calling assembly.
            /// </summary>
            /// <returns>The embedded resource as a stream.</returns>
            /// <param name="resourceFileName">Resource file name.</param>
            /// <exception cref="KeyNotFoundException">Thrown when the resource ending with 
            /// <paramref name="resourceFileName"/> is not found.</exception>
            /// <exception cref="RankException">Thrown when multiple resources ending with 
            /// <paramref name="resourceFileName"/> is found.</exception>
            public static Stream FindStream(string resourceFileName)
            {
                return FindStream(typeof(Resources).GetTypeInfo().Assembly, resourceFileName);
            }

            /// <summary>
            /// Attempts to find and return the given resource from within the calling assembly.
            /// </summary>
            /// <returns>The embedded resource as a byte array.</returns>
            /// <param name="resourceFileName">Resource file name.</param>
            /// <exception cref="KeyNotFoundException">Thrown when the resource ending with 
            /// <paramref name="resourceFileName"/> is not found.</exception>
            /// <exception cref="RankException">Thrown when multiple resources ending with 
            /// <paramref name="resourceFileName"/> is found.</exception>
            public static byte[] FindBytes(string resourceFileName)
            {
                return FindBytes(typeof(Resources).GetTypeInfo().Assembly, resourceFileName);
            }

            /// <summary>
            /// Attempts to find and return the given resource from within the calling assembly.
            /// </summary>
            /// <returns>The embedded resource as a string.</returns>
            /// <param name="resourceFileName">Resource file name.</param>
            /// <exception cref="KeyNotFoundException">Thrown when the resource ending with 
            /// <paramref name="resourceFileName"/> is not found.</exception>
            /// <exception cref="RankException">Thrown when multiple resources ending with 
            /// <paramref name="resourceFileName"/> is found.</exception>
            public static string FindString(string resourceFileName)
            {
                return FindString(typeof(Resources).GetTypeInfo().Assembly, resourceFileName);
            }

            /// <summary>
            /// Attempts to find and return the given resource from within the specified assembly.
            /// </summary>
            /// <returns>The embedded resource stream.</returns>
            /// <param name="assembly">Assembly.</param>
            /// <param name="resourceFileName">Resource file name.</param>
            /// <exception cref="KeyNotFoundException">Thrown when the resource ending with 
            /// <paramref name="resourceFileName"/> is not found.</exception>
            /// <exception cref="RankException">Thrown when multiple resources ending with 
            /// <paramref name="resourceFileName"/> is found.</exception>
            public static Stream FindStream(Assembly assembly, string resourceFileName)
            {
                var resourceNames = assembly.GetManifestResourceNames();

                var resourcePaths = resourceNames
                    .Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
                    .ToArray();

                if (!resourcePaths.Any())
                    throw new KeyNotFoundException(string.Format("Resource ending with {0} not found.", resourceFileName));

                if (resourcePaths.Count() > 1)
                    throw new RankException(string.Format("Multiple resources ending with {0} found: {1}{2}", resourceFileName,
                        Environment.NewLine, string.Join(Environment.NewLine, resourcePaths)));
                
                return assembly.GetManifestResourceStream(resourcePaths.Single());
            }

            /// <summary>
            /// Attempts to find and return the given resource from within the specified assembly.
            /// </summary>
            /// <returns>The embedded resource as a byte array.</returns>
            /// <param name="assembly">Assembly.</param>
            /// <param name="resourceFileName">Resource file name.</param>
            /// <exception cref="KeyNotFoundException">Thrown when the resource ending with 
            /// <paramref name="resourceFileName"/> is not found.</exception>
            /// <exception cref="RankException">Thrown when multiple resources ending with 
            /// <paramref name="resourceFileName"/> is found.</exception>
            public static byte[] FindBytes(Assembly assembly, string resourceFileName)
            {
                var stream = FindStream(assembly, resourceFileName);

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            /// <summary>
            /// Attempts to find and return the given resource from within the specified assembly.
            /// </summary>
            /// <returns>The embedded resource as a string.</returns>
            /// <param name="assembly">Assembly.</param>
            /// <param name="resourceFileName">Resource file name.</param>
            /// <exception cref="KeyNotFoundException">Thrown when the resource ending with 
            /// <paramref name="resourceFileName"/> is not found.</exception>
            /// <exception cref="RankException">Thrown when multiple resources ending with 
            /// <paramref name="resourceFileName"/> is found.</exception>
            public static string FindString(Assembly assembly, string resourceFileName)
            {
                var stream = FindStream(assembly, resourceFileName);
                using (var streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }

            /// <summary>
            /// Attempts to get and return the given resource from within the calling assembly.
            /// </summary>
            /// <returns>The embedded resource as a stream.</returns>
            /// <param name="resourceFilePath">Resource file name.</param>
            public static Stream GetStream(string resourceFilePath)
            {
                return GetStream(typeof(Resources).GetTypeInfo().Assembly, resourceFilePath);
            }

            /// <summary>
            /// Attempts to get and return the given resource from within the calling assembly.
            /// </summary>
            /// <returns>The embedded resource as a byte array.</returns>
            /// <param name="resourceFilePath">Resource file name.</param>
            public static byte[] GetBytes(string resourceFilePath)
            {
                return GetBytes(typeof(Resources).GetTypeInfo().Assembly, resourceFilePath);
            }

            /// <summary>
            /// Attempts to get and return the given resource from within the calling assembly.
            /// </summary>
            /// <returns>The embedded resource as a string.</returns>
            /// <param name="resourceFilePath">Resource file name.</param>
            public static string GetString(string resourceFilePath)
            {
                return GetString(typeof(Resources).GetTypeInfo().Assembly, resourceFilePath);
            }

            /// <summary>
            /// Attempts to get and return the given resource from within the specified assembly.
            /// </summary>
            /// <returns>The embedded resource stream.</returns>
            /// <param name="assembly">Assembly.</param>
            /// <param name="resourceFilePath">Resource file name.</param>
            public static Stream GetStream(Assembly assembly, string resourceFilePath)
            {
                return assembly.GetManifestResourceStream(CurrentNamespace + '.' + resourceFilePath);
            }

            /// <summary>
            /// Attempts to get and return the given resource from within the specified assembly.
            /// </summary>
            /// <returns>The embedded resource as a byte array.</returns>
            /// <param name="assembly">Assembly.</param>
            /// <param name="resourceFilePath">Resource file name.</param>
            public static byte[] GetBytes(Assembly assembly, string resourceFilePath)
            {
                var stream = GetStream(assembly, resourceFilePath);

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            /// <summary>
            /// Attempts to get and return the given resource from within the specified assembly.
            /// </summary>
            /// <returns>The embedded resource as a string.</returns>
            /// <param name="assembly">Assembly.</param>
            /// <param name="resourceFilePath">Resource file name.</param>
            public static string GetString(Assembly assembly, string resourceFilePath)
            {
                var stream = GetStream(assembly, resourceFilePath);
                using (var streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }
    }
}
