using Newtonsoft.Json.Linq;
using StarRailMapper.Core.Helpers;

namespace StarRailMapper.Core.Models.Outs;

internal class Enemies : Outs<Enemies>
{
    public readonly string Resistance; // 抗性
    public readonly string Type; // 类型
    public readonly string Description; // 描述
    public readonly List<string> Weakness; // 弱点
    public readonly List<string> Location; // 发现地点
    public readonly List<string> Loots; // 掉落
    public readonly Dictionary<string, string> Skills; // 招式

    private Enemies(string name, string icon, string resistance, string type, string description,
        List<string> weakness, List<string> location, List<string> loots, Dictionary<string, string> skills) :
        base(name, icon)
    {
        Resistance = resistance;
        Type = type;
        Description = description;
        Weakness = weakness;
        Location = location;
        Loots = loots;
        Skills = skills;
    }

    public static Enemies SerializeEnemies(string url)
    {
        Http.Get(url, out var result);
        var json = Json.FromJson<JObject>(result)!["data"]!["content"];
        var name = Json.JStr(json!["title"]);
        var icon = Json.JStr(json["icon"]);
        var html = Json.JStr(json["contents"]![0]!["text"]);
        var doc = Html.ParseHtml(html);
        List<string> weakness = new();
        List<string> location = new(); // 发现地点
        List<string> loots = new(); // 掉落
        Dictionary<string, string> skills = new(); // 招式
        var type = "";
        var resistance = "";
        var description = "";
        foreach (var htmlDocNode in doc.FindAll("tr"))
        {
            var attrName = htmlDocNode.Find("td", ("class", "h3")).InnerText;
            if (attrName == "弱点")
            {
                if (weakness.Count != 0) continue;
                foreach (var docNode in htmlDocNode.FindAll("td")[1].FindAll("img"))
                {
                    var elem = docNode.GetAttributeValue("src");
                    if (!weakness.Contains(elem)) weakness.Add(elem);
                }
            }

            if (attrName == "抗性")
            {
                resistance = htmlDocNode.FindAll("td")[1].InnerText.Replace("抗性", "");
            }

            if (attrName == "种类")
            {
                type = htmlDocNode.FindAll("td")[1].InnerText;
            }

            if (attrName == "掉落材料")
            {
                foreach (var docNode in htmlDocNode.FindAll("td")[1].FindAll("p"))
                {
                    var item = docNode.InnerText;
                    if (!loots.Contains(item)) loots.Add(item);
                }
            }

            if (attrName == "发现地点")
            {
                foreach (var loc in htmlDocNode.FindAll("td")[1].InnerText.Split("；"))
                {
                    if (!location.Contains(loc)) location.Add(loc);
                }
            }

            if (attrName == "敌人介绍")
            {
                description = htmlDocNode.FindAll("td")[1].InnerText;
            }
        }

        var skillDoc = doc.FindAll("li", ("data-target", "skill.attr"));
        foreach (var htmlDocNode in skillDoc)
        {
            var skillName = htmlDocNode.Find("span").InnerText;
            var skillDesc = htmlDocNode.Find("pre").InnerText;
            if (skillName != "")
            {
                skills.TryAdd(skillName, skillDesc);
            }
        }

        Enemies enemy = new Enemies(name, icon, resistance, type, description, weakness, location, loots, skills);
        return enemy;
    }
}