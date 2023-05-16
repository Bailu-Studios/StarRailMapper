using Newtonsoft.Json.Linq;
using StarRailMapper.Core.Constants;
using StarRailMapper.Core.Helpers;
using StarRailMapper.Core.Models.Outs;

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

    public static void GetGachaPool()
    {
        var itemsMap = GetMainMap();
        Http.Get(Constants.Constants.GachaInfo, out var result);
        var json = Json.FromJson<JObject>(result);
        foreach (var pool in json!["data"]!["list"]!)
        {
            var charsMap = itemsMap.GetValueOrDefault(TypeEnums.Chars.Name, new());
            var conesMap = itemsMap.GetValueOrDefault(TypeEnums.Cones.Name, new());
            foreach (var item in pool["pool"]!)
            {
                var id = int.Parse(Json.JStr(item["url"]).Split("/")[6]);
                if (Json.JStr(pool["title"]) == "流光定影")
                {
                    if (conesMap.ContainsKey(id))
                        Console.Out.WriteLine(Json.JStr(pool["title"]) + "  " + conesMap.GetValueOrDefault(id, "null"));
                    else
                        Console.Out.WriteLine(Json.JStr(pool["title"]) + "  " + charsMap.GetValueOrDefault(id, "null"));
                }
                else
                {
                    if (conesMap.ContainsKey(id))
                        Console.Out.WriteLine(Json.JStr(pool["title"]) + "  " + conesMap.GetValueOrDefault(id, "null"));
                    else
                        Console.Out.WriteLine(Json.JStr(pool["title"]) + "  " + charsMap.GetValueOrDefault(id, "null"));
                }
            }
        }
    }

    public static void Test()
    {
        Enemies enemy = Enemies.SerializeEnemies(Constants.Constants.InfoPage+"873");
        Console.Out.WriteLine(Json.ToJson(enemy));
    }
}