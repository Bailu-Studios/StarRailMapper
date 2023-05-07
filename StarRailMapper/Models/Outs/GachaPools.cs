namespace StarRailMapper.Models.Outs
{
    internal class GachaPools
    {
        public string name;            // 卡池名称
        public string icon;            // 卡池图标
        public string type;            // 卡池类型
        public string start_time;      // 开始时间
        public string end_time;        // 结束时间
        public List<string> content_5; // 卡池内容（五星）
        public List<string> content_4; // 卡池内容（四星）

        public static GachaPools SerializeGachaPools(string url)
        {
            return new GachaPools();
        }
    }
}
