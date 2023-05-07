// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using StarRailMapper.Helpers;
using StarRailMapper.Models;
string allInfo = "https://api-static.mihoyo.com/common/blackboard/sr_wiki/v1/home/map?app_sn=sr_wiki";
string result = "";
Http.HttpGet(allInfo, out result);
HttpJson info = JsonConvert.DeserializeObject<HttpJson>(result);
foreach (AllInfo i in info.data.list)
{
    if (i.id == 17)
    {
        Console.WriteLine("┗ " + i.name);
        foreach (Info j in i.children)
        {
            if (j.id == 18)
            {
                Console.WriteLine("  ┗ " + j.name);
                foreach (InfoData k in j.list)
                {
                    Console.WriteLine("    ┗ " + k.title);
                    Console.WriteLine("      ┗ " + k.icon);
                }
            }
        }
    }
}
