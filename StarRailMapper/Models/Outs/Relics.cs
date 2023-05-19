using StarRailMapper.Core.Helpers;

namespace StarRailMapper.Core.Models.Outs;

internal class Relics : Outs<Relics>
{
    public readonly string Obtain; // 获取途径
    public readonly string SetEffect; // 描述
    public readonly List<Relic> RelicList; // 内容

    private Relics(string name, string icon, string obtain, string setEffect, List<Relic> relicList)
        : base(name, icon)
    {
        Obtain = obtain;
        SetEffect = setEffect;
        RelicList = relicList;
    }

    public static Relics SerializeRelics(string url)
    {
        var doc = Serialize(url, out string name, out string icon);
        doc.ToFile("./Relics.html");
        var obtain = "";
        var setEffect = "";
        var relicList = new List<Relic>();
        var nodes = doc.FindAll("div", ("data-tmpl", "heritage"));
        for (var i = 0; i < nodes.Count; i++)
        {
            if (i == 0)
            {
                obtain = nodes[i].Find("p", ("style", "white-space: pre-wrap;")).Find("a").InnerText;
                setEffect = nodes[i].Find("div", ("class", "obc-tmpl-relic__story")).InnerText;
            }
            else
            {
                relicList.Add(Relic.SerializeRelic(nodes[i]));
            }
        }

        return new Relics(name, icon, obtain, setEffect, relicList);
    }
}

public class Relic : Outs<Relic>
{
    public readonly string Parts; // 部位
    public readonly string Origin; // 来历
    public readonly string Description; // 描述

    private Relic(string name, string icon, string parts, string origin, string description) : base(name, icon)
    {
        Parts = parts;
        Origin = origin;
        Description = description;
    }

    public static Relic SerializeRelic(HtmlDocNode doc)
    {
        var nodes = doc.Find("table").FindAll("td");
        var name = nodes[1].Find("span").InnerText;
        var icon = nodes[0].Find("img").GetAttributeValue("src");
        var parts = nodes[1].Find("label").InnerText.Replace("：", "");
        var origin = nodes[3].InnerText.Replace("来历 ", "");
        var description = nodes[2].InnerText.Replace("描述：", "");
        return new Relic(name, icon, parts, origin, description);
    }
}