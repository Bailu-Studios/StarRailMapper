using System.Text.RegularExpressions;
using StarRailMapper.Core.Helpers;

namespace StarRailMapper.Core.Models.Outs;

internal class LightCones : Outs<LightCones>
{
    public readonly int Rarity; // 稀有度
    public readonly string Path; // 命途
    public readonly string Skill; // 技能
    public readonly string Description; // 描述
    public readonly Dictionary<string, GrowthValue> GrowthValues; // 成长数值
    public readonly Dictionary<int, Dictionary<string, int>> GrowthItems; // 进阶材料

    private LightCones(string name, string icon, int rarity, string path, string skill, string description,
        Dictionary<string, GrowthValue> growthValues, Dictionary<int, Dictionary<string, int>> growthItems) : base(name,
        icon)
    {
        Rarity = rarity;
        Path = path;
        Skill = skill;
        Description = description;
        GrowthValues = growthValues;
        GrowthItems = growthItems;
    }

    public static LightCones SerializeLightCones(string url)
    {
        var doc = Serialize(url, out string name, out string icon);
        var rarity = 4;
        var path = "";
        var skill = "";
        var description = "";
        var growthValues = GrowthValue.SerializeGrowthValues(doc.Find("div", ("style", "order: 1;")));
        var growthItems = GrowthItem.SerializeGrowthItems(doc.Find("div", ("style", "order: 2;")));
        foreach (var htmlDocNode in doc.Find("table", ("class", "obc-tml-light-table--mobile")).FindAll("tr"))
        {
            var nodes = htmlDocNode.FindAll("td");
            var nodeType = nodes[0].InnerText;
            if (nodeType == "命途")
            {
                path = nodes[1].InnerText;
            }
            else if (nodeType == "稀有度")
            {
                rarity = int.Parse(nodes[1].InnerText.Replace("星", "").Replace("级", ""));
            }
            else if (nodeType == "技能")
            {
                skill = nodes[1].InnerText;
            }
            else if (nodeType == "光锥描述")
            {
                description = nodes[1].InnerText;
            }
        }

        //<div style="order: 1;">

        //<div style="order: 2;"
        return new LightCones(name, icon, rarity, path, skill, description, growthValues, growthItems);
    }
}

public class GrowthValue
{
    public readonly int HP; // 生命值
    public readonly int ATK; // 攻击力
    public readonly int DEF; // 防御力

    private GrowthValue(int hp, int atk, int def)
    {
        HP = hp;
        ATK = atk;
        DEF = def;
    }

    public static Dictionary<string, GrowthValue> SerializeGrowthValues(HtmlDocNode node)
    {
        var dict = new Dictionary<string, GrowthValue>();
        var docNodes = node.FindAll("tr");
        for (var i = 1; i < docNodes.Count; i++)
        {
            var nodes = docNodes[i].FindAll("td");
            var level = nodes[0].InnerText.Replace("\t", "");
            var hp = 0;
            var atk = 0;
            var def = 0;
            if (nodes[1].InnerText != "" && nodes[1].InnerText != "-")
                hp = int.Parse(nodes[1].InnerText);
            if (nodes[2].InnerText != "" && nodes[2].InnerText != "-")
                atk = int.Parse(nodes[2].InnerText);
            if (nodes[3].InnerText != "" && nodes[3].InnerText != "-")
                def = int.Parse(nodes[3].InnerText);
            dict.TryAdd(level, new GrowthValue(hp, atk, def));
        }

        return dict;
    }
}

public static class GrowthItem
{
    public static Dictionary<int, Dictionary<string, int>> SerializeGrowthItems(HtmlDocNode node)
    {
        var dict = new Dictionary<int, Dictionary<string, int>>();
        var docNodes = node.FindAll("tr");
        for (var i = 1; i < docNodes.Count; i++)
        {
            var dic = new Dictionary<string, int>();
            var nodes = docNodes[i].FindAll("td");
            var level = int.Parse(Regex.Replace(nodes[0].InnerText, @"[^0-9]+", ""));
            foreach (var docNode in nodes[1].FindAll("li"))
            {
                var name = docNode.Find("p").InnerText;
                var count = docNode.Find("span").InnerText;
                if (name != "" && count != "" && count != "-") dic.TryAdd(name, int.Parse(count));
            }

            dict.TryAdd(level, dic);
        }

        return dict;
    }
}