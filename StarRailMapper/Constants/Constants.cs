namespace StarRailMapper.Core.Constants;

public abstract class Constants
{
    public const string MainPage =
        "https://api-static.mihoyo.com/common/blackboard/sr_wiki/v1/home/map?app_sn=sr_wiki";

    public const string ChannelInfo =
        "https://api-static.mihoyo.com/common/blackboard/sr_wiki/v1/home/content/list?app_sn=sr_wiki&channel_id=";

    public const string GachaInfo =
        "https://api-takumi.mihoyo.com/common/blackboard/sr_wiki/v1/gacha_pool?app_sn=sr_wiki";

    public const string InfoPage =
        "https://api-static.mihoyo.com/common/blackboard/sr_wiki/v1/content/info?app_sn=sr_wiki&content_id=";

    public const string JsonFolders = "./out/";
}