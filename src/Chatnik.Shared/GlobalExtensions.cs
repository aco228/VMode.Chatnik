using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Chatnik.Shared
{
    public static class GlobalExtensions
    {
        public static bool CastObject(this object input, Type to, out object? response)
        {
            response = null;
            try
            {
                response = TypeDescriptor.GetConverter(to).ConvertFrom(input.ToString()) ?? null;
            }
            catch(Exception ex)
            {
                try
                {
                    response = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(input), to);
                }
                catch (Exception jsonEx)
                {
                    return false;
                }
            }

            return response != null;
        }
        
        public static bool IsSimple(this Type type) =>
            TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));

        public static Guid ToGuid(this string input)
            => Guid.TryParse(input, out var result) ? result : Guid.Empty;
    }
}