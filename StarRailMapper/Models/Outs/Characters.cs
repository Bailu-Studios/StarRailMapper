namespace StarRailMapper.Models.Outs
{
    internal class Characters
    {
        public string name;                   // 角色名
        public int rarity;                    // 稀有度
        public string path;                   // 命途
        public string combat;                 // 属性
        public string function;               // 所属
        public Dictionary<int,Ascend> ascend; // 角色属性
        public List<Traces> traces;           // 角色行迹
        public List<Attribute> attributes;    // 属性加成
        public List<Eidolons> eidsolons;      // 星魂

        public static Characters SerializeCharacters(string url)
        {
            return new Characters();
        }
    }

    internal class Ascend // 角色属性
    {
        public Dictionary<string,int> growth_items; // 突破材料
        public int hp;                              // 生命值
        public int atk;                             // 攻击力
        public int def;                             // 防御力
        public int hp_after;                        // 生命值
        public int atk_after;                       // 攻击力
        public int def_after;                       // 防御力
    }

    internal class Traces // 角色行迹
    {
        public Dictionary<string, int> growth_items; // 突破材料
        public string name;                          // 名称
        public string description;                   // 描述
    }

    internal class Attribute // 属性加成
    {
        public Dictionary<string, int> growth_items; // 突破材料
        public string name;                          // 名称
        public string description;                   // 描述
    }

    internal class Eidolons // 星魂
    {
        public string name;        // 名称
        public string description; // 描述
    }
}
