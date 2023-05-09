using StarRailMapper.Core;
using StarRailMapper.Core.Constants;
using StarRailMapper.Core.Helpers;
using StarRailMapper.Core.Models.Outs;

var gachaPools = new List<GachaPools>(); // 卡池
var characters = new List<Characters>(); // 角色
var enemies = new List<Enemies>(); // 敌对
var inventoryItems = new List<InventoryItems>(); // 物品
var lightCones = new List<LightCones>(); // 光锥
var relics = new List<Relics>(); // 遗器

var itemsMap = ProgramTasks.GetMainMap();

Console.Out.WriteLine(Json.ToJson(itemsMap));
foreach (var id in itemsMap.GetValueOrDefault(TypeEnums.Chars.Name, new()).Keys)
{
    characters.Add(Characters.SerializeCharacters(Constants.InfoPage + id));
}

if (args.Length > 0 && args[0] == "output")
{
    Directory.CreateDirectory(Constants.JsonFolders);
    Json.ToJson(gachaPools, Constants.JsonFolders + "gacha_pools.json");
    Json.ToJson(characters, Constants.JsonFolders + "characters.json");
    Json.ToJson(enemies, Constants.JsonFolders + "enemies.json");
    Json.ToJson(inventoryItems, Constants.JsonFolders + "inventory_items.json");
    Json.ToJson(lightCones, Constants.JsonFolders + "light_cones.json");
    Json.ToJson(relics, Constants.JsonFolders + "relics.json");
}
