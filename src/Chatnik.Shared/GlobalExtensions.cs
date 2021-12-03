using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Chatnik.Shared
{
    public static class GlobalExtensions
    {
        public static object CastObject(this object input, Type to)
        {
            try
            {
                return TypeDescriptor.GetConverter(to).ConvertFrom(input.ToString());
            }
            catch(Exception ex)
            {
                try
                {
                    return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(input), to);
                }
                catch (Exception jsonEx)
                {
                    return null;   
                }
            }
        }
        
        public static bool IsSimple(this Type type) =>
            TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));

        public static Guid ToGuid(this string input)
            => Guid.TryParse(input, out var result) ? result : Guid.Empty;
    }
}