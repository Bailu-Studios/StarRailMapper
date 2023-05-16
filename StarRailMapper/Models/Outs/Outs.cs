namespace StarRailMapper.Core.Models.Outs;

public abstract class Outs<T>
{
    public string Name; // 名称
    public string Icon; // 图标

    protected Outs()
    {}

    protected Outs(string name, string icon)
    {
        Name = name;
        Icon = icon;
    }
}