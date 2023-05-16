using System.Reflection;
using HtmlAgilityPack;

namespace StarRailMapper.Core.Helpers;

public class HtmlDocNode
{
    private readonly HtmlNode _node;
    public readonly string OuterHtml;
    public readonly string InnerText;

    public HtmlDocNode(HtmlNode node)
    {
        _node = node;
        OuterHtml = _node.OuterHtml;
        InnerText = _node.InnerText;
    }

    public HtmlDocNode Find(string tag, params (string, string)[] attrs)
    {
        try
        {
            foreach (var node in _node.SelectNodes($"//{tag}"))
            {
                var flag = true;
                foreach (var (key, value) in attrs)
                    if (!(node.Attributes.Contains(key) && node.Attributes[key].Value == value))
                        flag = false;
                if (flag) return new HtmlDocNode(node);
            }
        }
        catch (Exception)
        {
        }

        return new HtmlDocNode(HtmlNode.CreateNode("<a></a>"));
    }

    public string GetAttributeValue(string key)
    {
        if (_node.Attributes.Contains(key)) return _node.Attributes[key].Value;
        return "";
    }

    public List<HtmlDocNode> FindAll(string tag, params (string, string)[] attrs)
    {
        var list = new List<HtmlDocNode>();
        var nodes = _node.SelectNodes($"//{tag}");
        if (nodes != null)
            foreach (var node in nodes)
            {
                var flag = true;
                foreach (var (key, value) in attrs)
                    if (!(node.Attributes.Contains(key) && node.Attributes[key].Value == value))
                        flag = false;
                if (flag) list.Add(new HtmlDocNode(HtmlNode.CreateNode(node.OuterHtml)));
            }

        return list;
    }
}

public class Html : HtmlDocNode
{
    //public readonly HtmlDocument Document;

    private Html(HtmlDocument document) : base(document.DocumentNode)
    {
        //Document = document;
    }

    public static Html ParseHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return new Html(doc);
    }

    public static Html ParseFile(string file)
    {
        var doc = new HtmlDocument();
        doc.Load(file);
        return new Html(doc);
    }

    public static Html ParseUrl(string url)
    {
        HtmlWeb web = new HtmlWeb();
        var doc = web.Load(url);
        return new Html(doc);
    }
}