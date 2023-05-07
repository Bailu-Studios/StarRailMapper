namespace StarRailMapper.Models.Outs
{
    internal class InventoryItems
    {
        public string name;        // 名称
        public int rarity;         // 稀有度
        public List<string> type;  // 类型
        public string description; // 描述

        public static InventoryItems SerializeInventoryItems(string url)
        {
            return new InventoryItems();
        }
    }
}
