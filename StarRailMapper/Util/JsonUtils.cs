using Newtonsoft.Json.Linq;

namespace StarRailMapper.Core.Util; 

public static class JsonUtils {
    public static int JInt(JToken? json) {
        return json!.ToObject<int>();
    }
    
    public static string JStr(JToken? json) {
        return json!.ToObject<string>()!;
    }
}