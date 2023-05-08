using Newtonsoft.Json.Linq;
using StarRailMapper.Core.Helpers;

namespace StarRailMapper.Core.Models.Outs;

internal class Characters : Outs<Characters>
{
    public readonly int Rarity; // 稀有度
    public readonly string Path; // 命途
    public readonly string Combat; // 属性
    public readonly string Function; // 所属
    public readonly Dictionary<int, Ascend> Ascends = new(); // 角色属性
    public readonly List<Traces> Traces = new(); // 角色行迹
    public readonly List<Attribute> Attributes = new(); // 属性加成
    public readonly List<Eidolons> Eidsolons = new(); // 星魂

    private Characters()
    {
    }

    private Characters(string name, string icon, int rarity, string path, string combat)
    {
        Name = name;
        Icon = icon;
        Rarity = rarity;
        Path = path;
        Combat = combat;
    }

    public static Characters SerializeCharacters(string url)
    {
        Http.Get(url, out var result);
        var json = Json.FromJson<JObject>(result)["data"]["content"];
        var name = Json.JStr(json["title"]);
        var icon = Json.JStr(json["icon"]);
        var ext = Json.FromJson<JObject>(Json.JStr(json["ext"]));
        var filter = Json.FromJson<JArray>(Json.JStr(ext["c_18"]["filter"]["text"]));
        int rarity = 4; // 稀有度
        string path = "巡猎"; // 命途
        string combat = "虚无"; // 属性
        foreach (var jToken in filter)
        {
            var str = Json.JStr(jToken).Split("/");
            switch (str[0])
            {
                case "属性":
                    combat = str[1];
                    break;
                case "命途":
                    path = str[1];
                    break;
                case "星级":
                    switch (str[1])
                    {
                        case "四星":
                            rarity = 4;
                            break;
                        case "五星":
                            rarity = 5;
                            break;
                    }

                    break;
            }
        }

        Console.Out.WriteLine(combat);
        Console.Out.WriteLine(path);
        Console.Out.WriteLine(rarity);
        return new Characters(name, icon, rarity, path, combat);
    }
}

public class Ascend // 角色属性
{
    public readonly Dictionary<string, int> growth_items = new(); // 突破材料
    public readonly int HP; // 生命值
    public readonly int ATK; // 攻击力
    public readonly int DEF; // 防御力
    public readonly int HPAfter; // 生命值
    public readonly int ATKAfter; // 攻击力
    public readonly int DEFAfter; // 防御力
}

public class Traces // 角色行迹
{
    public readonly Dictionary<string, int> GrowthItems = new(); // 突破材料
    public readonly string Name; // 名称
    public readonly string Description; // 描述
}

public class Attribute // 属性加成
{
    public readonly Dictionary<string, int> GrowthItems = new(); // 突破材料
    public readonly string Name; // 名称
    public readonly string Description; // 描述
    public readonly string Type; // 加成类别
    public readonly double Value; // 加成值
}

public class Eidolons // 星魂
{
    public readonly string Name; // 名称
    public readonly string Description; // 描述
}