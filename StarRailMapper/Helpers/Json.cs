using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StarRailMapper.Core.Helpers;

public static class Json
{
    public static int JInt(JToken? json)
    {
        return json!.ToObject<int>();
    }

    public static string JStr(JToken? json)
    {
        return json!.ToObject<string>()!;
    }

    public static string ToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static void ToJson(object obj, string file)
    {
        File.WriteAllText(file, ToJson(obj));
    }

    public static T? FromJson<T>(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (JsonSerializationException)
        {
            if (!File.Exists(json)) throw new JsonSerializationException();
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(json));
        }
    }
}