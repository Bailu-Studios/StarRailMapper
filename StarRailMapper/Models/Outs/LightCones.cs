namespace StarRailMapper.Models.Outs
{
    internal class LightCones
    {
        public string name;                                    // 名称
        public int rarity;                                     // 稀有度
        public string path;                                    // 命途
        public string icon;                                    // 图标
        public string skill;                                   // 技能
        public string description;                             // 描述
        public Dictionary<int, GrowthValue> growth_values;     // 成长数值
        public Dictionary<int, List<GrowthItem>> growth_items; // 进阶材料

        private LightCones(string name, int rarity, string path, string icon, string skill, string description) {
            this.name = name;
            this.rarity = rarity;
            this.path = path;
            this.icon = icon;
            this.skill = skill;
            this.description = description;
            growth_values = new Dictionary<int, GrowthValue>();
            growth_items = new Dictionary<int, List<GrowthItem>>();
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
    internal class GrowthValue { 
        public int hp;  // 生命值
        public int atk; // 攻击力
        public int def; // 防御力
    }
    internal class GrowthItem {
        public string name; // 名称
        public int count;   // 数量
    }
}
