using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chatnik.Shared.Exceptions;
using Chatnik.Shared.Interfaces;
using NetMQ;
using Newtonsoft.Json;

namespace Chatnik.Shared.Helpers
{
    public static class MessageExtensions
    {
        
        public static T Convert<T>(this IReceiveMessage response)
            where T : ITransferMessage
        {
            var result = (T)Activator.CreateInstance(typeof(T), response.Topic)!;
            result.User = response.User;
            
            var properties = result.GetFrameProperties().ToArray();
            for (int index = 0; index < properties.Count(); index ++)
            {
                var property = properties[index];
                
                if(property.Name.Equals(nameof(response.Topic))
                   || property.Name.Equals(nameof(response.User)))
                    continue;
                
                if (index < 0 || index >= response.Frames.Length)
                    throw new MessageBaseSerializationException(index, property.Name);
                
                var value = response.Frames[index];
                if (!property.PropertyType.IsSimple())
                {
                    property.SetValue(result, JsonConvert.DeserializeObject(value, property.PropertyType));
                }
                else
                {
                    if(value.CastObject(property.PropertyType, out var dataObject))
                        property.SetValue(result, dataObject);
                }
            }

            return result;
        }
        
        
        public static NetMQMessage Convert(this ITransferMessage message)
        {
            var result = new NetMQMessage();
            result.Append(message.Topic);
            result.Append(message.User);
            
            var properties = message.GetFrameProperties().ToArray();
            for (int index = 0; index < properties.Count(); index ++)
            {   
                var property = properties[index];
                
                if(property.Name.Equals(nameof(message.Topic))
                    || property.Name.Equals(nameof(message.User)))
                    continue;
                
                string? value = null;
                
                if (!property.PropertyType.IsSimple())
                    value = JsonConvert.SerializeObject(property.GetValue(message));
                else
                    value = property.GetValue(message)?.ToString();

                if (string.IsNullOrEmpty(value))
                {
                    // TODO: Hacky way to ensure empty strings
                    // TODO!! Rewrite it and support it in model
                    if (property.PropertyType == typeof(string))
                        value = " ";
                    else
                        throw new MessageBaseSerializationException(index, property.Name);
                }
                
                result.Append(value);
            }

            return result;
        }

        
        private static IEnumerable<PropertyInfo> GetFrameProperties<T>(this T input)
            where T : IMessage
        {
            foreach (var prop in input.GetType().GetProperties())
            {
                yield return prop;
            }
        }
    }
}