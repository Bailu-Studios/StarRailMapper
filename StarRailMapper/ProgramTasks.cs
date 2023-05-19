using Newtonsoft.Json.Linq;
using StarRailMapper.Core.Constants;
using StarRailMapper.Core.Helpers;

namespace StarRailMapper.Core;

public abstract class ProgramTasks
{
    public static Dictionary<string, Dictionary<int, string>>? MainMap;

    public static Dictionary<string, Dictionary<int, string>> GetMainMap()
    {
        if (null != MainMap) return MainMap;
        var map = new Dictionary<string, Dictionary<int, string>>();
        foreach (var entry in TypeEnums.TypeEnumsMap)
        {
            var typeId = entry.Key;
            var typeName = entry.Value.Name;
            Http.Get(Constants.Constants.ChannelInfo + typeId, out var result);
            var json = Json.FromJson<JObject>(result);
            foreach (var item in json!["data"]!["list"]![0]!["list"]!)
            {
                var itemId = Json.JInt(item["content_id"]);
                var itemName = Json.JStr(item["title"]);
                var subMap = map.GetValueOrDefault(typeName, new Dictionary<int, string>());
                subMap.Add(itemId, itemName);
                map.TryAdd(typeName, subMap);
            }
        }

        MainMap = map;
        return map;
    }
}