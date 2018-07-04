using System;
using System.Collections.Generic;

namespace HarmonyHub
{
    /// <summary>
    /// String extensions
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Converts a Logitech param string into a dictionary.
        /// </summary>
        /// <param name="paramString">String to parse.</param>
        /// <returns>A dictionary of parameters.</returns>
        public static IDictionary<string, string> ToParamDictionary(this string paramString)
        {
            var result = new Dictionary<string, string>();
            var parameters = paramString.Split(':');

            foreach (var parameter in parameters)
            {
                var parts = parameter.Split('=');
                if (parts.Length != 2)
                {
                    throw new ArgumentException("String conversion to KeyValuePair failed on incorrect format");
                }

                result.Add(parts[0], parts[1]);
            }

            return result;
        }
    }
}
