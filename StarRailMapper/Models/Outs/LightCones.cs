namespace StarRailMapper.Core.Models.Outs;

internal class LightCones : Outs<LightCones>
{
    public readonly int Rarity; // 稀有度
    public readonly string Path; // 命途
    public readonly string Skill; // 技能
    public readonly string Description; // 描述
    public readonly Dictionary<int, GrowthValue> GrowthValues = new(); // 成长数值
    public readonly Dictionary<int, List<GrowthItem>> GrowthItems = new(); // 进阶材料

    private LightCones(string name, int rarity, string path, string icon, string skill, string description)
        : base(name, icon)
    {
        Rarity = rarity;
        Path = path;
        Skill = skill;
        Description = description;
    }

    public static LightCones SerializeLightCones(string url)
    {
        string name = "";
        int rarity = 4;
        string path = "";
        string icon = "";
        string skill = "";
        string description = "";
        return new LightCones(name, rarity, path, icon, skill, description);
    }
}

public class GrowthValue
{
    public readonly int HP; // 生命值
    public readonly int ATK; // 攻击力
    public readonly int DEF; // 防御力
}

public class GrowthItem
{
    public readonly string NAME; // 名称
    public readonly int Count; // 数量
}