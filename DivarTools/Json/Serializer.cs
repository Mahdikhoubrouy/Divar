using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DivarTools.Json
{
    internal static class Serializer
    {
        internal static TModel Jsonserializer<TModel>(string data)
        {
            try
            {
                return JsonSerializer.Deserialize<TModel>(data)!;
            }
            catch (Exception ex)
            {
                Type type = typeof(TModel);
                return (TModel)Activator.CreateInstance(type)!;
            }
        }

    }
}
