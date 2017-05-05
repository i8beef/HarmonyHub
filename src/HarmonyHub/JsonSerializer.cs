using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HarmonyHub
{
    /// <summary>
    /// Internal serialization helper.
    /// </summary>
    /// <typeparam name="TType">The type to be serialized / deserialized.</typeparam>
    internal static class JsonSerializer<TType>
        where TType : class
    {
        /// <summary>
        /// Serializes an object to JSON
        /// </summary>
        /// <param name="instance">Instance to serialize.</param>
        /// <returns>Serialized string.</returns>
        public static string Serialize(TType instance)
        {
            var serializer = new DataContractJsonSerializer(typeof(TType));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// DeSerializes an object from JSON
        /// </summary>
        /// <param name="json">JSON string to serialize.</param>
        /// <returns>Deserialized object.</returns>
        public static TType Deserialize(string json)
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(TType));
                return serializer.ReadObject(stream) as TType;
            }
        }
    }
}
