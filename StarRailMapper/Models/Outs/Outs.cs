using Newtonsoft.Json.Linq;
using StarRailMapper.Core.Helpers;

namespace StarRailMapper.Core.Models.Outs;

public abstract class Outs<T>
{
    public string Name; // 名称
    public string Icon; // 图标

    protected Outs()
    {
    }

    protected Outs(string name, string icon)
    {
        Name = name;
        Icon = icon;
    }

    public static Html Serialize(string url, out string name, out string icon)
    {
        return Serialize(url, out name, out icon, out JToken json);
    }

    public static Html Serialize(string url, out string name, out string icon, out JToken json)
    {
        Http.Get(url, out var result);
        json = Json.FromJson<JObject>(result)!["data"]!["content"]!;
        name = Json.JStr(json!["title"]);
        icon = Json.JStr(json["icon"]);
        var html = Json.JStr(json["contents"]![0]!["text"]);
        return Html.ParseHtml(html);
    }
}