namespace StarRailMapper.Core.Models.Outs;

internal class InventoryItems : Outs<InventoryItems>
{
    public readonly int Rarity; // 稀有度
    public readonly List<string> Type = new(); // 类型
    public readonly string Description; // 描述

    public static InventoryItems SerializeInventoryItems(string url)
    {
        return new InventoryItems();
    }
}