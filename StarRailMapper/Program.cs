using StarRailMapper.Core;
using StarRailMapper.Core.Constants;
using StarRailMapper.Core.Helpers;
using StarRailMapper.Core.Models.Outs;

var gachaPools = new List<GachaPools>(); // 卡池
var characters = new Dictionary<string, Characters>(); // 角色
var enemies = new Dictionary<string, Enemies>(); // 敌对
var inventoryItems = new Dictionary<string, InventoryItems>(); // 物品
var lightCones = new Dictionary<string, LightCones>(); // 光锥
var relics = new Dictionary<string, Relics>(); // 遗器

var itemsMap = ProgramTasks.GetMainMap();

foreach (var id in itemsMap.GetValueOrDefault(TypeEnums.Chars.Name, new()).Keys)
{
    var character = Characters.SerializeCharacters(Constants.InfoPage + id);
    characters.Add(character.Name, character);
}

foreach (var id in itemsMap.GetValueOrDefault(TypeEnums.Enemy.Name, new()).Keys)
{
    var enemy = Enemies.SerializeEnemies(Constants.InfoPage + id);
    enemies.Add(enemy.Name, enemy);
}

foreach (var id in itemsMap.GetValueOrDefault(TypeEnums.Items.Name, new()).Keys)
{
    var item = InventoryItems.SerializeInventoryItems(Constants.InfoPage + id);
    //inventoryItems.Add(item.Name, item);
}

foreach (var id in itemsMap.GetValueOrDefault(TypeEnums.Relic.Name, new()).Keys)
{
    var relic = Relics.SerializeRelics(Constants.InfoPage + id);
    //relics.Add(relic.Name, relic);
}

foreach (var id in itemsMap.GetValueOrDefault(TypeEnums.Cones.Name, new()).Keys)
{
    var lightCone = LightCones.SerializeLightCones(Constants.InfoPage + id);
    //lightCones.Add(lightCone.Name, lightCone);
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