// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using StarRailMapper.Helpers;
using StarRailMapper.Models;
using StarRailMapper.Models.Outs;

string allInfo = "https://api-static.mihoyo.com/common/blackboard/sr_wiki/v1/home/map?app_sn=sr_wiki";
string infoUrl = "https://api-static.mihoyo.com/common/blackboard/sr_wiki/v1/content/info?app_sn=sr_wiki&content_id=";
string result;
string outDir = "./out/";
Http.HttpGet(allInfo, out result);
HttpJson info = JsonConvert.DeserializeObject<HttpJson>(result);
List<Characters> characters = new List<Characters>();
List<Enemies> enemies = new List<Enemies>();
List<GachaPools> gachaPools = new List<GachaPools>();
List<InventoryItems> inventoryItems = new List<InventoryItems>();
List<LightCones> lightCones = new List<LightCones>();
List<Relics> relics = new List<Relics>();
foreach (AllInfo i in info.data.list)
{
    if (i.id == 17 || i.id == 21)
    {
        Console.WriteLine("┗ " + i.name);
        foreach (Info j in i.children)
        {
            //if (j.id == 18)
            {
                Console.WriteLine("  ┗ " + j.id + " " + j.name);
                foreach (InfoData k in j.list)
                {
                    Console.WriteLine("    ┗ " + k.title);
                    Console.WriteLine("      ┗ " + infoUrl + k.content_id);
                    Console.WriteLine("      ┗ " + k.icon);
                }
            }
        }
    }
}
if (args.Length > 0 && args[0] == "output")
{
    Directory.CreateDirectory(outDir);
    File.WriteAllText(outDir + "characters.json", JsonConvert.SerializeObject(characters));
    File.WriteAllText(outDir + "enemies.json", JsonConvert.SerializeObject(enemies));
    File.WriteAllText(outDir + "gacha_pools.json", JsonConvert.SerializeObject(gachaPools));
    File.WriteAllText(outDir + "inventory_items.json", JsonConvert.SerializeObject(inventoryItems));
    File.WriteAllText(outDir + "light_cones.json", JsonConvert.SerializeObject(lightCones));
    File.WriteAllText(outDir + "relics.json", JsonConvert.SerializeObject(relics));
}
