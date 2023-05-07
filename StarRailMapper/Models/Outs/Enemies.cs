namespace StarRailMapper.Models.Outs
{
    internal class Enemies
    {
        public string name;           // 名称
        public string icon;           // 图标
        public string weakness;       // 弱点
        public string resistance;     // 抗性
        public string type;           // 类型
        public string description;    // 描述
        public List<string> location; // 发现地点
        public List<string> loots;    // 掉落
        public List<Skill> skills;    // 招式

        public static Enemies SerializeEnemies(string url)
        {
            return new Enemies();
        }
    }

    internal class Skill
    {
        public string name;        // 名称
        public string icon;        // 图标
        public string description; // 描述
    }
}
