namespace StarRailMapper.Models.Outs
{
    internal class Relics
    {
        public string name;        // 名称
        public string obtain;      // 获取途径
        public string description; // 描述
        public List<Relic> relics; // 内容

        public static Relics SerializeRelics(string url)
        {
            return new Relics();
        }
    }
    internal class Relic
    {
        public string name;        // 名称
        public string origin;      // 来历
        public string description; // 描述
    }
}
