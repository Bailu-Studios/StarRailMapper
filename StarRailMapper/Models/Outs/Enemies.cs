namespace StarRailMapper.Core.Models.Outs;

internal class Enemies : Outs<Enemies>
{
    public readonly string Weakness; // 弱点
    public readonly string Resistance; // 抗性
    public readonly string Type; // 类型
    public readonly string Description; // 描述
    public readonly List<string> Location = new(); // 发现地点
    public readonly List<string> Loots = new(); // 掉落
    public readonly List<Skill> Skills = new(); // 招式

    public static Enemies SerializeEnemies(string url)
    {
        return new Enemies();
    }
}

public class Skill
{
    public readonly string Name; // 名称
    public readonly string Icon; // 图标
    public readonly string Description; // 描述
}