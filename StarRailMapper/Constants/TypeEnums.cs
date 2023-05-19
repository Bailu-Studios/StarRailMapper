namespace StarRailMapper.Core.Constants;

public class TypeEnums
{
    public static readonly Dictionary<int, TypeEnums> TypeEnumsMap = new();

    public static readonly TypeEnums Empty = new();
    public static readonly TypeEnums Chars = new(18, "角色");
    public static readonly TypeEnums Cones = new(19, "光锥");
    public static readonly TypeEnums Enemy = new(23, "敌对");
    public static readonly TypeEnums Relic = new(30, "遗器");
    public static readonly TypeEnums Items = new(20, "物品");

    public static readonly TypeEnums Drain = new(36, "物品");

    // public static readonly TypeEnums Reads = new(31, "物品");
    // public static readonly TypeEnums Tasks = new(53, "物品");
    public static readonly TypeEnums Costs = new(54, "物品");
    public static readonly TypeEnums Other = new(55, "物品");

    public readonly int Id;
    public readonly string Name;

    private TypeEnums(int id, string name)
    {
        Id = id;
        Name = name;
        TypeEnumsMap.Add(Id, this);
    }

    private TypeEnums()
    {
        Id = 0;
        Name = "null";
    }
}