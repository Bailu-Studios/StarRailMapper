using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using StarRailMapper.Core.Helpers;

namespace StarRailMapper.Core.Models.Outs;

internal class Characters : Outs<Characters>
{
    public readonly int Rarity; // 稀有度
    public readonly string Path; // 命途
    public readonly string Combat; // 属性
    public readonly Dictionary<int, Ascend> AscendDict = new(); // 角色属性
    public readonly List<Traces> TraceList = new(); // 角色行迹
    public readonly List<Attribute> AttributeList = new(); // 属性加成
    public readonly List<Eidolons> EidsolonList = new(); // 星魂

    private Characters(string name, string icon, int rarity, string path, string combat) : base(name, icon)
    {
        Rarity = rarity;
        Path = path;
        Combat = combat;
    }

    public static Characters SerializeCharacters(string url)
    {
        Http.Get(url, out var result);
        var json = Json.FromJson<JObject>(result)!["data"]!["content"];
        var name = Json.JStr(json!["title"]);
        var icon = Json.JStr(json["icon"]);
        var ext = Json.FromJson<JObject>(Json.JStr(json["ext"]));
        var filter = Json.FromJson<JArray>(Json.JStr(ext!["c_18"]!["filter"]!["text"]));
        int rarity = 4; // 稀有度
        string path = "巡猎"; // 命途
        string combat = "虚无"; // 属性
        foreach (var jToken in filter!)
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

        var html = Json.JStr(json["contents"]![0]!["text"]);
        // File.WriteAllText("./info.html", html);
        var doc = Html.ParseHtml(html);
        var node = doc.Find("div", ("style", "order: 1;"));
        Characters character = new Characters(name, icon, rarity, path, combat);
        // 角色属性
        for (int i = 0; i < 8; i++)
        {
            var attr = node!.FindAll("li", ("data-target", "breach.attr"), ("data-index", $"{i}"))[1];
            var attr0 = Ascend.SerializeAscend(attr.FindAll("tbody")[0]);
            if (i == 0) character.AscendDict.Add(0, attr0);
            else character.AscendDict.Add(i * 10 + 10, attr0);
        }

        // 角色行迹
        for (int i = 0; i < 8; i++)
        {
            var attr = Traces.SerializeTraces(
                node!.FindAll("li", ("data-target", "skill.attr"), ("data-index", $"{i}")));
            character.TraceList.Add(attr);
        }

        var attrHtml = node!.FindAll("div", ("data-part", "desc"));
        // 属性加成
        {
            var htmlNode = attrHtml[0];
            var htmlNodes = htmlNode.FindAll("tr");
            foreach (var htmlDocNode in htmlNodes)
            {
                if (htmlDocNode == htmlNodes[0]) continue;
                var attr = htmlDocNode.FindAll("td");
                var attrName = attr[0].InnerText.Replace("：","");
                var attrDesc = attr[1].InnerText;
                var attrDes = attrDesc.Split("提高");
                var attrUp = attr[2].InnerText;
                var attribute = new Attribute(attrName, attrDesc, attrDes[0],
                    double.Parse(attrDes[1].Replace("%", "").Replace("。","")));
                foreach (var s in Regex.Split(attrUp, "，|、"))
                {
                    var attrUps = s.Split("*");
                    attribute.GrowthItems.Add(attrUps[0], int.Parse(attrUps[1]));
                }

                character.AttributeList.Add(attribute);
            }
        }
        // 星魂
        {
            var htmlNode = attrHtml[1];
            var htmlNodes = htmlNode.FindAll("tr");
            foreach (var htmlDocNode in htmlNodes)
            {
                if (htmlDocNode == htmlNodes[0]) continue;
                var attr = htmlDocNode.FindAll("td");
                var attrName = attr[0].InnerText;
                var attrDesc = "";
                if (attr.Count >= 3) attrDesc = attr[2].InnerText;
                character.EidsolonList.Add(new Eidolons(attrName, attrDesc));
            }
        }

        return character;
    }
}

public class Ascend // 角色属性
{
    public readonly Dictionary<string, int> GrowthItems = new(); // 突破材料
    public readonly int HP; // 生命值
    public readonly int ATK; // 攻击力
    public readonly int DEF; // 防御力
    public readonly int HPAfter; // 生命值
    public readonly int ATKAfter; // 攻击力
    public readonly int DEFAfter; // 防御力

    private Ascend(int hp, int atk, int def, int hpAfter, int atkAfter, int defAfter)
    {
        HP = hp;
        ATK = atk;
        DEF = def;
        HPAfter = hpAfter;
        ATKAfter = atkAfter;
        DEFAfter = defAfter;
    }

    public static Ascend SerializeAscend(HtmlDocNode node)
    {
        int hp = -1, atk = -1, def = -1, hpAfter = -1, atkAfter = -1, defAfter = -1;
        var up = node.FindAll("tr");
        foreach (var docNode in up)
        {
            var nodes = docNode.FindAll("td");
            if (nodes.Count >= 1)
            {
                string name = nodes[0].InnerText;
                if (name == "晋阶材料" || name == "") continue;
                int value = (int)double.Parse(nodes[1].Find("span")!.InnerText.Replace("%",""));
                if (name == "晋阶前生命值") hp = value;
                if (name == "晋阶前攻击力") atk = value;
                if (name == "晋阶前防御力") def = value;
                if (name == "生命值" || name == "晋阶后生命值") hpAfter = value;
                if (name == "攻击力" || name == "晋阶后攻击力") atkAfter = value;
                if (name == "防御力" || name == "晋阶后防御力") defAfter = value;
            }

            if (nodes.Count >= 3)
            {
                string name = nodes[2].InnerText;
                if (name == "晋阶材料" || name == "") continue;
                int value = (int)double.Parse(nodes[3].Find("span")!.InnerText.Replace("%", ""));
                if (name == "生命值" || name == "晋阶前生命值") hp = value;
                if (name == "攻击力" || name == "晋阶前攻击力") atk = value;
                if (name == "防御力" || name == "晋阶前防御力") def = value;
                if (name == "晋阶后生命值") hpAfter = value;
                if (name == "晋阶后攻击力") atkAfter = value;
                if (name == "晋阶后防御力") defAfter = value;
            }
        }

        Ascend ascend = new Ascend(hp, atk, def, hpAfter, atkAfter, defAfter);
        var growth = node.Find("td", ("colspan", "3"));
        var growths = growth!.FindAll("li", ("data-target", "breach.attr.material"));
        foreach (var htmlDocNode in growths)
        {
            if (htmlDocNode.InnerText == "  ") continue;
            string itemName = htmlDocNode.Find("span", ("class", "obc-tmpl__icon-text"))!.InnerText;
            if(itemName=="无")continue;
            int itemCount =
                int.Parse(htmlDocNode.Find("span", ("class", "obc-tmpl__icon-num"))!.InnerText.Replace("*", ""));
            ascend.GrowthItems.TryAdd(itemName, itemCount);
        }

        return ascend;
    }
}

public class Traces // 角色行迹
{
    public readonly string Name; // 名称
    public readonly string Type; // 类型
    public readonly Dictionary<string, Dictionary<string, string>> Effects = new(); // 突破效果
    public readonly Dictionary<string, Dictionary<string, int>> GrowthItems = new(); // 突破材料
    public readonly string Description; // 描述

    private Traces(string name, string type, string description)
    {
        Type = type;
        Name = name;
        Description = description;
    }

    public static Traces SerializeTraces(List<HtmlDocNode> node)
    {
        var list = new List<Dictionary<string, string>>();
        var attrType = node[0]
            .InnerText
            .Replace("\n", "")
            .Replace(" ", "")
            .Split("·")[0];
        var attr = node[1];
        var attrName = attr.Find("span", ("class", "obc-tmpl__icon-text"))!.InnerText;
        var attrDesc = attr.Find("p", ("style", "white-space: pre-wrap;"))!.InnerText
            .Replace("[详情]", "").Replace(" ", "");
        var trace = new Traces(attrName, attrType, attrDesc);
        foreach (var docNode in attr.FindAll("tr"))
        {
            var nodes = docNode.FindAll("th");
            if (nodes.Count == 0) nodes = docNode.FindAll("td");
            if (nodes.Count >= 1)
            {
                var name = nodes[0].InnerText.Replace("\n", "").Replace(" ", "");
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (i == 0) continue;
                    Dictionary<string, string> dict;
                    if (list.Count >= i) dict = list[i - 1];
                    else
                    {
                        dict = new Dictionary<string, string>();
                        list.Add(dict);
                    }

                    var htmlDocNode = nodes[i];
                    var text = htmlDocNode.InnerText.Replace("\n", "").Replace(" ", "");
                    if (name == "升级材料")
                    {
                        text = "";
                        var htmlNodes = htmlDocNode.FindAll("span");
                        for (var j = 0; j < htmlNodes.Count; j++)
                        {
                            if (htmlNodes[j].InnerText == "未确认") continue;
                            if (j % 2 == 0)
                            {
                                if(htmlNodes.Count>=j+2) text += $"{htmlNodes[j].InnerText}{htmlNodes[j + 1].InnerText}|";
                            }
                        }

                        if (text.Length - 1 >= 0) text = text.Remove(text.Length - 1, 1);
                    }

                    dict.TryAdd(name, text);
                }
            }
        }

        foreach (var dict in list)
        {
            string level = "";
            string up = "";
            List<(string, string)> effect = new();
            foreach (var (key, value) in dict)
            {
                if (key == "详细属性") level = value;
                else if (key == "升级材料") up = value;
                else effect.Add((key, value));
            }

            trace.GrowthItems.TryGetValue(level, out Dictionary<string, int>? growth);
            if (null == growth)
            {
                growth = new Dictionary<string, int>();
                trace.GrowthItems.Add(level, growth);
            }

            foreach (var s in up.Split("|"))
            {
                if (s == "") continue;
                var str = s.Split("*");
                int count = 0;
                if (str.Length >= 2) count = int.Parse(str[1]);
                growth.Add(str[0], count);
            }

            trace.Effects.TryGetValue(level, out Dictionary<string, string>? effects);
            if (null == effects)
            {
                effects = new Dictionary<string, string>();
                trace.Effects.Add(level, effects);
            }

            foreach (var (name, data) in effect)
            {
                effects.TryAdd(name, data);
            }
        }

        return trace;
    }
}

public class Attribute // 属性加成
{
    public readonly Dictionary<string, int> GrowthItems = new(); // 突破材料
    public readonly string Name; // 名称
    public readonly string Description; // 描述
    public readonly string Type; // 加成类别
    public readonly double Value; // 加成值

    public Attribute(string name, string description, string type, double value)
    {
        Name = name;
        Description = description;
        Type = type;
        Value = value;
    }
}

public class Eidolons // 星魂
{
    public readonly string Name; // 名称
    public readonly string Description; // 描述

    public Eidolons(string name, string description)
    {
        Name = name;
        Description = description;
    }
}