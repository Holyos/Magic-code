using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    
    public class FelTile
    {
        public static void init()
        {
            //init_biome();
            init_tile();
        }
        public static void init_biome()
        {
            BiomeAsset biome_fel = new BiomeAsset();
            biome_fel.id = "biome_fel";
            biome_fel.tile_low = "fel_low";
            biome_fel.tile_high = "fel_high";
            biome_fel.addUnit("lowest_defile_demon", 10);
            biome_fel.addMineral("HellKennel", 9);
            AssetManager.biome_library.add(biome_fel);
            AssetManager.biome_library.addBiomeToPool(biome_fel);
            
        }
        public static void init_tile()
        {
            TopTileType anomaly = AssetManager.topTiles.clone("anomaly", ST.landmine);
            anomaly.cost = 10;
            anomaly.drawPixel = true;
            //anomaly.id = "anomaly";
            anomaly.color = (Color32) Toolbox.makeColor("#990099");
            anomaly.burnable = false;
            anomaly.explodable = false;
            anomaly.explodeRange = 0;
            anomaly.ground = true;
            anomaly.strength = 0;
            anomaly.canErrodeToSand = false;
            anomaly.can_be_frozen = false;
            anomaly.stepAction = new TileStepAction(teleport);
            anomaly.stepActionChance = 0.9f;
            anomaly.setDrawLayer(TileZIndexes.landmine);
            anomaly.canBeRemovedWithDemolish = true;
            AssetManager.topTiles.add(anomaly);
            loadSprites(anomaly);
            /*
            TopTileType fel_low = AssetManager.topTiles.clone("fel_low", ST.tumor_low);
            fel_low.drawPixel = true;
            fel_low.id = "fel_low";
            fel_low.creep = true;
            fel_low.color = new Color32(61, 9, 67, 245);
            fel_low.heightMin = 108;
            fel_low.ground = true;
            fel_low.walkMod = 1f;
            fel_low.burnable = false;
            fel_low.life = true;
            fel_low.fireChance = 0f;
            fel_low.strength = 0;
            fel_low.canBuildOn = false;
            fel_low.remove_on_freeze = true;
            fel_low.can_be_frozen = false;
            fel_low.setDrawLayer(TileZIndexes.tumor_low);
            fel_low.setBiome("fel_biome");
            /*fel_low.setAutoGrowMinerals(new GrowTypeSelector(TileActionLibrary.getGrowTypeRandomMineral), new string[]
			{
			"HellKennel#4",
			"barracks_demons#3"
			});*/
            //fel_low.setBiome(ST.biome_tumor);
            /*AssetManager.topTiles.add(fel_low);
            loadSprites(fel_low);

            TopTileType fel_high = AssetManager.topTiles.clone("fel_high", ST.tumor_high);
            fel_high.drawPixel = true;
            fel_high.id = "fel_high";
            fel_high.creep = true;
            fel_high.color = new Color32(61, 9, 67, 245);
            fel_high.heightMin = 108;
            fel_high.ground = true;
            fel_high.walkMod = 1f;
            fel_high.burnable = false;
            fel_high.life = true;
            fel_high.fireChance = 0f;
            fel_high.strength = 0;
            fel_high.canBuildOn = false;
            fel_high.remove_on_freeze = true;
            fel_high.can_be_frozen = false;
            fel_high.setDrawLayer(TileZIndexes.tumor_low);
            fel_high.setBiome("fel_biome");
            /*fel_high.setAutoGrowMinerals(new GrowTypeSelector(TileActionLibrary.getGrowTypeRandomMineral), new string[]
			{
			"HellKennel#4",
			"barracks_demons#3"
			});
            //fel_low.setBiome(ST.biome_tumor);
            AssetManager.topTiles.add(fel_high);
            loadSprites(fel_high);*/
        }
        public static bool teleport (WorldTile pTile, ActorBase pActor)
        {
            if (Toolbox.randomChance(0.2f))
                ActionLibrary.teleportRandom(pActor.a,pActor.a);
            else if (Toolbox.randomChance(0.25f))
                pActor.a.addStatusEffect("invincible",100);
            else if (Toolbox.randomChance(0.333f))
                pActor.a.addStatusEffect("burning",1000);
            else if (Toolbox.randomChance(0.5f))
                pActor.a.restoreHealth(1000);
            else if (!pActor.hasTrait("Magical Gift") && Toolbox.randomChance(0.4f))
                pActor.a.addTrait("Magical Gift");
            else if (!pActor.hasTrait("Blood Magic") && Toolbox.randomChance(0.05f))
                pActor.a.addTrait("Blood Magic");
                
            //World.world.explosionLayer.explodeBomb(pTile);
            return true;
        }
        private static void loadSprites(TopTileType pTile)
        {
            string folder = pTile.id;
            if (folder == string.Empty)
            {
                folder = "Others";
            }
            //older = folder + "/" + pTile.id;
            Sprite[] array = Utils.ResourcesHelper.loadAllSprite("tiles/" + folder, 0.5f, 0.0f);

            if (array == null || array.Length == 0)
                return;
            pTile.sprites = new TileSprites();
            foreach (Sprite pSprite in array)
                pTile.sprites.addVariation(pSprite);
        
           
        }
        
    }
}