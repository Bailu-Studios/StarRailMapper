// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using StarRailMapper.Core.Constants;
using StarRailMapper.Core.Helpers;
using StarRailMapper.Core.Models;
using StarRailMapper.Models.Outs;

Http.HttpGet(Constants.MainPage, out var result);
Console.WriteLine(result);
HttpJson? info = JsonConvert.DeserializeObject<HttpJson>(result);
List<Characters> characters = new List<Characters>();
List<Enemies> enemies = new List<Enemies>();
List<GachaPools> gachaPools = new List<GachaPools>();
List<InventoryItems> inventoryItems = new List<InventoryItems>();
List<LightCones> lightCones = new List<LightCones>();
List<Relics> relics = new List<Relics>();
foreach (AllInfo i in info!.data.list)
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
                    Console.WriteLine("      ┗ " + Constants.InfoPage + k.content_id);
                    Console.WriteLine("      ┗ " + k.icon);
                }
            }
        }
    }
}
if (args.Length > 0 && args[0] == "output")
{
    Directory.CreateDirectory(Constants.JsonFolders);
    File.WriteAllText(Constants.JsonFolders + "characters.json", JsonConvert.SerializeObject(characters));
    File.WriteAllText(Constants.JsonFolders + "enemies.json", JsonConvert.SerializeObject(enemies));
    File.WriteAllText(Constants.JsonFolders + "gacha_pools.json", JsonConvert.SerializeObject(gachaPools));
    File.WriteAllText(Constants.JsonFolders + "inventory_items.json", JsonConvert.SerializeObject(inventoryItems));
    File.WriteAllText(Constants.JsonFolders + "light_cones.json", JsonConvert.SerializeObject(lightCones));
    File.WriteAllText(Constants.JsonFolders + "relics.json", JsonConvert.SerializeObject(relics));
}
