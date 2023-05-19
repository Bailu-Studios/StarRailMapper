namespace StarRailMapper.Core.Models.Outs;

internal class InventoryItems : Outs<InventoryItems>
{
    public readonly int Rarity; // 稀有度
    public readonly string Description; // 描述
    public readonly List<string> Source; // 来源
    public readonly List<string> Type; // 类型

    private InventoryItems(string name, string icon, int rarity, string description, List<string> source,
        List<string> type)
        : base(name, icon)
    {
        Rarity = rarity;
        Description = description;
        Source = source;
        Type = type;
    }

    public static InventoryItems SerializeInventoryItems(string url)
    {
        var doc = Serialize(url, out string name, out string icon);
        var main = doc.Find("table", ("class", "material-table--mobile"));
        var rarity = 0;
        var description = "";
        var source = new List<string>();
        var type = new List<string>();
        foreach (var htmlDocNode in main.FindAll("tr"))
        {
            var nodes = htmlDocNode.FindAll("td");
            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].InnerText == "稀有度" && rarity == 0)
                {
                    rarity = int.Parse(nodes[i + 1].InnerText
                        .Replace("星", "")
                        .Replace("级", "")
                        .Replace("一", "1")
                        .Replace("二", "2")
                        .Replace("三", "3")
                        .Replace("四", "4")
                        .Replace("五", "5")
                        .Replace(" ","")
                    );
                }
                else if (nodes[i].InnerText == "类型")
                {
                    foreach (var str in nodes[i + 1].InnerText.Split("，"))
                    {
                        var s = str.Replace(" ", "");
                        if(!type.Contains(s))type.Add(s);
                    }
                }
                else if (nodes[i].InnerText == "介绍")
                {
                    description = nodes[i + 1].InnerText.Replace(" ", "");
                }
                else if (nodes[i].InnerText == "来源")
                {
                    foreach (var docNode in nodes[i + 1].FindAll("li"))
                    {
                        var s = docNode.InnerText.Replace(" ", "");
                        if(!source.Contains(s))source.Add(s);
                    }
                }
            }
        }

        var item = new InventoryItems(name, icon, rarity, description, source, type);
        return item;
    }
}