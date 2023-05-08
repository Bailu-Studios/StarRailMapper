// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarRailMapper.Core.Constants;
using StarRailMapper.Core.Helpers;
using StarRailMapper.Models.Outs;

using static StarRailMapper.Core.Util.JsonUtils;


var task = Http.Download(
    "https://uploadstatic.mihoyo.com/sr-wiki/2023/03/09/187636729/af90c6a64b2be8f65187bdd432819f2f_6004427954716905156.png",
    new FileInfo("test.png"));

Http.Get(Constants.MainPage, out var result);
var json = JsonConvert.DeserializeObject<JObject>(result);

var characters = new List<Characters>();
var enemies = new List<Enemies>();
var gachaPools = new List<GachaPools>();
var inventoryItems = new List<InventoryItems>();
var lightCones = new List<LightCones>();
var relics = new List<Relics>();

foreach (var info in json["data"]["list"]) {
    if (JInt(info["id"]) == 17 || JInt(info["id"]) == 21) {
        Console.WriteLine($"┗ {info["name"]} (id={info["id"]})");
        foreach (var type in info["children"]!) {
            //if (j.id == 18)
            Console.WriteLine($"  ┗ {type["name"]} (id={type["id"]})");
            foreach (var msg in type["list"]!) {
                Console.WriteLine($"    ┗ {msg["title"]} (id={msg["content_id"]})");
                Console.WriteLine($"      ┗ {Constants.InfoPage + msg["content_id"]}");
                Console.WriteLine($"      ┗ {msg["icon"]}");
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
