namespace StarRailMapper.Core.Models.Outs;

internal class Relics : Outs<Relics>
{
    public readonly string Obtain; // 获取途径
    public readonly string Description; // 描述
    public readonly List<Relic> RelicList = new(); // 内容

    public static Relics SerializeRelics(string url)
    {
        return new Relics();
    }
}

public class Relic
{
    public readonly string Name; // 名称
    public readonly string Origin; // 来历
    public readonly string Description; // 描述
}