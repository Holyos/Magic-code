using System.Collections.Generic;
using UnityEngine;
using ai;
using Beebyte.Obfuscator;
using System.Reflection;
using ReflectionUtility;

namespace Magic{
       // #nullable disable
        //[ObfuscateLiterals]
        class MagicSpells //: AssetLibrary<Spell>
        {
            
            public static void init()
            {
                //base.init();
                Spell rain = new Spell();
                rain.id = "rain";
                rain.chance = 1f;
                rain.castTarget = CastTarget.Enemy;
                rain.castEntity = CastEntity.UnitsOnly;
                rain.min_distance = 0.0f;
                rain.action = new AttackAction(CastRain);
                AssetManager.spells.add(rain); 

                Spell Earthquake = new Spell();
                Earthquake.id = "Earthquake";
                Earthquake.chance = 0.01f;
                Earthquake.castTarget = CastTarget.Enemy;
                Earthquake.castEntity = CastEntity.UnitsOnly;
                Earthquake.min_distance = 3.0f;
                Earthquake.action = new AttackAction(CastEarthquake);
                AssetManager.spells.add(Earthquake); 

                Spell lava = new Spell();
                lava.id = "lava";
                lava.chance = 0.01f;
                lava.castTarget = CastTarget.Enemy;
                lava.castEntity = CastEntity.UnitsOnly;
                lava.min_distance = 10.0f;
                lava.action = new AttackAction(CastLava);
                AssetManager.spells.add(lava);

                Spell spiritInitiation = new Spell();
                spiritInitiation.id = "spiritInitiation";
                spiritInitiation.chance = Main.spiritInitiations;
                spiritInitiation.castTarget = CastTarget.Enemy;
                spiritInitiation.castEntity = CastEntity.UnitsOnly;
                spiritInitiation.min_distance = 0.0f;
                spiritInitiation.action = new AttackAction(SpiritInitiation);
                AssetManager.spells.add(spiritInitiation); 

                Spell divine = new Spell();
                divine.id = "divine";
                divine.chance = 5;
                divine.castTarget = CastTarget.Enemy;
                divine.castEntity = CastEntity.UnitsOnly;
                divine.min_distance = 0.0f;
                divine.action = new AttackAction(CastDivine);
                AssetManager.spells.add(divine); 
            }
            public static bool CastRain(
            BaseSimObject pSelf,
            BaseSimObject pTarget,
            WorldTile pTile = null)
            {
                CastDrop(5f,"rain",pTarget);
                return true;
            }
            public static bool CastDivine(
            BaseSimObject pSelf,
            BaseSimObject pTarget,
            WorldTile pTile = null)
            {
                pTile = pSelf.currentTile;
                World.world.fxDivineLight.playOn(pTile);
                AssetManager.powers.drawDivineLight(pTile,"divineLight");
                WorldBehaviourWaves.checkTile(pTile, 5);

                for (int index = -5; index < 5; ++index)
                {
                    for (int indexes = -5; indexes < 5; ++indexes)
                    {
                        int x = pTile.x + index;
                        int y = pTile.y + indexes;
                        if (x >= 0 && x < MapBox.width && y >= 0 && y < MapBox.height)
                        {
                            WorldTile tileSimple = MapBox.instance.GetTileSimple(x, y);
                            AssetManager.powers.drawDivineLight(tileSimple,"divineLight");
                        }
                    }
                    
                }
                return true;
            }
            public static bool CastLava(
            BaseSimObject pSelf,
            BaseSimObject pTarget,
            WorldTile pTile = null)
            {
                CastDrop(5f,"lava",pTarget);
                return true;
            }
            public static bool CastEarthquake(
            BaseSimObject pSelf,
            BaseSimObject pTarget,
            WorldTile pTile = null)
            {
                if (pTile == null)
                    pTile = pTarget.currentTile;
                if (pTile == null)
                    return false;
                World.world.earthquakeManager.startQuake(pTile,EarthquakeType.SmallDisaster);
                return true;
            }
            public static bool CastDrop(
            float high,
            string effect,
            BaseSimObject pTarget,
            WorldTile pTile = null)
            {
                if (pTile == null)
                    pTile = pTarget.currentTile;
                if (pTile == null)
                    return false;
                World.world.dropManager.spawn(pTile, effect, high, -1f);
                return true;
            }

            public static bool SpiritInitiation(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
            {
                if (pTarget != null)
                {
                    //Actor a = pTarget.a;
                    Actor b = pTarget.a;
                    if( 
                        (b.asset.id == "unit_orc")||
                        (b.asset.id == "unit_lizard" && Toolbox.randomChance(0.5f)) ||
                        (b.asset.id == "unit_goblin" && Toolbox.randomChance(0.2f)) ||
                        ((b.asset.id == "unit_beastmen" || b.asset.id == "unit_ancientchina") && Toolbox.randomChance(0.1f)))
                    {
                        
                        b.addTrait("Шаман");
                    }
                    return true;
                }
                return false;
            }
        }
}