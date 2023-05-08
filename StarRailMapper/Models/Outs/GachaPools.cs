namespace StarRailMapper.Core.Models.Outs;

internal class GachaPools : Outs<GachaPools>
{
    public readonly string Type; // 卡池类型
    public readonly string StartTime; // 开始时间
    public readonly string EndTime; // 结束时间
    public readonly List<Content> Contents = new(); // 卡池内容

    public static GachaPools SerializeGachaPools(string url)
    {
        return new GachaPools();
    }
}

public class Content
{
    public readonly string Name;
    public readonly string Type;
    public readonly int Rarity;
}