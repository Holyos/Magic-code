using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using Beebyte.Obfuscator;
using ai;
using ai.behaviours;
using EpPathFinding.cs;
using HarmonyLib;
using Newtonsoft.Json;
namespace Magic{
    public class FunctionalAction
    {
        #region функции
#region Существуешь?
        public static bool exsisting (BaseSimObject pTarget)
        {
            if (pTarget != null)
                if (pTarget.isActor())
                    if (pTarget.a.isAlive())
                        return true;
            return false;
        }   
    #endregion     
#region Восстанови
        public static bool Regen(BaseSimObject pTarget, int pHealth = 1, WorldTile pTile = null)
        {
            if (pTarget.a.hasTrait("infected"))
            {
                return true;
            }
            if (pTarget.a.hasStatus("curse")||pTarget.a.hasStatus("ash_fever"))
                return true;
            bool flag = true;
            if (pTarget.a.asset.needFood)
            {
                flag = (pTarget.a.data.hunger > 0);
            }
            if (pTarget.a.data.health != pTarget.getMaxHealth() && flag && Toolbox.randomChance(0.2f))
            {
                spellCost(pTarget,0,-pHealth);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            if (pTarget.a.data.health == pTarget.getMaxHealth() && 
            flag && 
            (pTarget.a.hasTrait("eyepatch") ||
            pTarget.a.hasTrait("skin_burns") ||
            pTarget.a.hasTrait("crippled")))
            {
                pTarget.a.removeTrait("eyepatch");
                pTarget.a.removeTrait("skin_burns");
                pTarget.a.removeTrait("crippled"); 
            }
            return true;
        }
        #endregion
#region Очисти
        public static bool removeBadTrait(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a != null )
            {
                
                //insert bad traits here
                pTarget.a.removeTrait("madness");
                pTarget.a.removeTrait("crippled");
                pTarget.a.removeTrait("cursed");
                pTarget.a.removeTrait("death_mark");
                pTarget.a.removeTrait("voices_in_my_head");
                pTarget.a.removeTrait("eyepatch");
                pTarget.a.removeTrait("infected");
                pTarget.a.removeTrait("plague");
                pTarget.a.removeTrait("mushSpores");
                pTarget.a.removeTrait("tumorInfection");
                pTarget.a.removeTrait("skin_burns");
                //pTarget.a.removeTrait("scar_of_divinity");
                pTarget.a.removeTrait("Lycanthropy");
                pTarget.a.removeTrait("Vampirism");
                pTarget.a.removeTrait("AndroidPower2");  //kinda hate this trait
            }
            
            return true;
        }
        public static bool removeInfectTrait(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a != null)
            {
                
                //insert bad traits here
                pTarget.a.removeTrait("infected");
                pTarget.a.removeTrait("plague");
                pTarget.a.removeTrait("mushSpores");
                pTarget.a.removeTrait("tumorInfection");
                pTarget.a.removeTrait("zombie");
                pTarget.a.removeTrait("AndroidPower1");
                //pTarget.a.removeTrait("scar_of_divinity");
                pTarget.a.removeTrait("Lycanthropy");
                pTarget.a.removeTrait("Werewolf");
                pTarget.a.removeTrait("Vampire");
                pTarget.a.removeTrait("Elder Vampire");
                pTarget.a.removeTrait("Vampirism");
                pTarget.a.removeTrait("AndroidPower2");  //kinda hate this trait
            }
            
            return true;
        }
        #endregion
#region Восстань
        public static bool rebornANew(BaseSimObject pTarget, WorldTile pTile)
        {
            if (!pTarget.a.hasTrait("blessed")){
                Actor a = pTarget.a;
                pTarget.a.removeTrait("cursed");
                pTarget.a.removeTrait("infected");
                pTarget.a.removeTrait("mushSpores");
                pTarget.a.removeTrait("tumorInfection");
                pTarget.a.removeTrait("madness");
                pTarget.a.removeTrait("eyepatch");
                pTarget.a.removeTrait("plague");
                pTarget.a.removeTrait("voices_in_my_head");
                pTarget.a.removeTrait("death_mark");
                pTarget.a.removeTrait("Phoenix");
                pTarget.a.removeTrait("crippled");
                pTarget.a.removeTrait("skin_burns");
                pTarget.a.removeTrait("Subordinate");
                TraitEffect.DeathBelive(pTarget, pTile);
                if (((Component) a).gameObject == null ||  a == null || !a.inMapBorder())
                    return false;
                a.addTrait("fire_proof"); //what kind of phoenix that got burned lol
                //a.removeTrait("Pheonix");
                removeInfectTrait(pTarget);
                var act = World.world.units.createNewUnit(a.asset.id, pTile, 0f);
                ActorTool.copyUnitToOtherUnit(a, act);
                if (pTarget.kingdom.isAlive())
                    act.kingdom = pTarget.kingdom;
                act.data.setName(pTarget.a.getName());
                act.data.health += 1000;
                //EffectsLibrary.spawn("fx_nuke_flash", pTarget.a.currentTile, null, null, 0f, -1f, -1f);
                //act.addStatusEffect("Phoenix", 7f);
                act.a.makeWait(3);
                act.addStatusEffect("invincible", 5);
                //spawn effect for cooler looks
                ActionLibrary.castLightning(null, act, null);
                //castLightningWithoutLava(pTarget, pTarget, null);
                PowerLibrary pb = new PowerLibrary();
                pb.divineLightFX(pTarget.a.currentTile, null);
                EffectsLibrary.spawnExplosionWave(pTile.posV3, 1f, 1f);
                World.world.applyForce(pTile, 10, 1.5f, true, true, 0, null, null, null);
            }
            return true;
        }
        #endregion
#region Излечи
        public static bool healing(BaseSimObject pTarget, int pHealth = 10, float pChance = 0.2f, int pDistance = 4, 
        bool Life_energy = false, WorldTile pTile = null, int cost = 10 
        )
      	{
            if (Toolbox.randomChance(pChance))
            {
                World.world.getObjectsInChunks(pTarget.currentTile, pDistance, MapObjectType.Actor);
                if(World.world.temp_map_objects.Count>1)
                    {
                        if (Life_energy)
                            Main.listOfEnergy["Life energy"]-=1;
                        else
                            spellCost(pTarget,10);
                    }
                for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
                {
                    Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                    if (tempMapObject != pTarget.a && 
                    tempMapObject.data.health < tempMapObject.getMaxHealth() &&
                    tempMapObject.kingdom == pTarget.a.kingdom)
                    {
                        tempMapObject.restoreHealth(pHealth);
                        tempMapObject.spawnParticle(Toolbox.color_heal);
                        tempMapObject.removeTrait("plague");
                        tempMapObject.removeTrait("infected");
                        tempMapObject.removeTrait("mushSpores");
                        tempMapObject.removeTrait("tumorInfection");
                        tempMapObject.removeTrait("skin_burns");
                        tempMapObject.removeTrait("crippled");
                        tempMapObject.removeTrait("eyepatch");
                    }
                }
            }
            return true;
        }
        #endregion
#region Защити
        public static bool Protect(BaseSimObject pTarget, int pDistance = 5, float pOverrideTimer = 5f, 
        string effect = "shield")
      	{
            World.world.getObjectsInChunks(pTarget.currentTile, pDistance, MapObjectType.Actor);
            for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
            {
                Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                if (tempMapObject.kingdom == pTarget.a.kingdom)
                {
                    addStatusOnTarget(pTarget, tempMapObject, effect, 0,0,pOverrideTimer);
                }
            }
            return true;
        }
        #endregion
#region Подчинись
        public static bool obey(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null) 
      	{
            Actor b = pTarget.a;
            if (b!= null && b.isActor())
            {
                if (!Main.listOfTamedBeasts.ContainsKey(b))
                {
                    if (b.kingdom != pSelf.a.kingdom)
                    {
                        b.kingdom.removeUnit(b);
                        b.kingdom = pSelf.kingdom;
                        pSelf.kingdom.addUnit(b);
                        Main.listOfTamedBeasts.Add(b, pSelf.a);
                        if (Main.listOfOwners.ContainsKey(pSelf.a))
                        {
                            Main.listOfOwners[pSelf.a] +=1;
                        }
                        else {
                            Main.listOfOwners.Add(pSelf.a, 0);
                            Main.listOfOwners[pSelf.a] +=1;
                        }
                    }
                    b.addTrait("Subordinate");
                }
            }
            return true;
        }
        #endregion
#region Получи
        public static bool addStatusOnTarget(BaseSimObject pSelf, BaseSimObject pTarget, string pStatus, int Hunger, int Health = 0, 
        float pOverrideTimer = -1f)
      	{
            if(pTarget!=null)
            {
                if(pTarget.isAlive() && pTarget.isActor()){
                        pTarget.a.addStatusEffect(pStatus,pOverrideTimer);
                    spellCost(pSelf, Hunger, Health);
                }
            }
            return true;
        }
        #endregion
#region Приди
        public static bool summon(BaseSimObject pTarget, 
        string entity, int Hunger, int Health = 0, 
        string trait1 = "",string trait2 = "", string trait3 = "", 
        WorldTile pTile = null)
        {
            Actor a = pTarget.a;
            float diplomacy= a.stats[S.diplomacy];
            if (pTarget==null)
                return false;
            if (!pTarget.isAlive())
                return false;
            if (pTile==null)
                pTile = pTarget.a.currentTile;
            if (entity=="Fire_spirit"||
            entity=="earth_spirit"||
            entity=="water_spirit"||
            entity=="air_spirit")
            {
                
                if (!Main.countSpirit.ContainsKey(pTarget.a))
                    Main.countSpirit.Add(pTarget.a,new HashSet<int>());
                if(Main.countSpirit[pTarget.a].Count>=5+diplomacy)
                    return false;
                
            }
            spellCost(pTarget, Hunger, Health);
            Actor act = World.world.units.createNewUnit(entity, pTile, 0f);
            if(act.isRace("spirit")){
                if(Main.countSpirit[pTarget.a].Count>=diplomacy)
                    return false;
                Main.countSpirit[pTarget.a].Add(act.GetHashCode());}
            act.kingdom = pTarget.kingdom;
            pTarget.kingdom.addUnit(act);
            act.addTrait("Subordinate");
            act.addTrait(trait1);
            act.addTrait(trait2);
            act.addTrait(trait3);
            //act.data.setName(pTarget.a.getName());
            Main.listOfTamedBeasts.Add(act, pTarget.a);
            if (Main.listOfOwners.ContainsKey(pTarget.a))
            {
                Main.listOfOwners[pTarget.a] +=1;
            }
            else {
                Main.listOfOwners.Add(pTarget.a, 0);
                Main.listOfOwners[pTarget.a] +=1;
            }
            EffectsLibrary.spawn("fx_spawn", act.currentTile, null, null, 0f, -1f, -1f);
            
            
            
            return true;
        }
        #endregion
#region Заплати
        public static bool spellCost(BaseSimObject pTarget, int Hunger, int Health = 0)
      	{
            if (pTarget==null)
                return false;
            if (!pTarget.isAlive())
                return false;
            int hungVal = pTarget.a.data.hunger - Hunger;
            hungVal = Mathf.Clamp(hungVal, 1, 100);
            pTarget.a.data.hunger = hungVal;
            int healVal = pTarget.a.data.health - Health;
            healVal = Mathf.Clamp(healVal, 1, pTarget.a.getMaxHealth());
            pTarget.a.data.health = healVal;
            return true;
        }
        #endregion
#region Следуй
        public static bool following (BaseSimObject pTarget, WorldTile pTile = null)
        {
            Actor b = pTarget.a;
            if (Toolbox.randomChance(0.8f))
            {
            if(Main.listOfTamedBeasts.ContainsKey(b))
            {
                if(Main.listOfTamedBeasts[b].isAlive())
                {
                    if (b.isRace("spirit"))
                    {
                        //float diplomacy= pTarget.a.stats[S.diplomacy];
                        if (!Main.countSpirit.ContainsKey(Main.listOfTamedBeasts[b]))
                            Main.countSpirit.Add(Main.listOfTamedBeasts[b],new HashSet<int>());
                        Main.countSpirit[Main.listOfTamedBeasts[b]].Add(pTarget.GetHashCode());
                    } 
                    if(Main.listOfTamedBeasts[b].hasTrait("The Magic of Death"))
                    {
                        if (Main.NewMagicOfDeath)
                        {
                            if(Main.deadBodies.ContainsKey(Main.listOfTamedBeasts[b]))
                            {
                                Main.deadBodies[Main.listOfTamedBeasts[b]]+=pTarget.a.data.kills;
                                pTarget.a.data.kills=0;
                            }
                            else
                            {
                                Main.deadBodies.Add(Main.listOfTamedBeasts[b],0);
                                if (Main.NewMagicOfDeath)
                                {
                                    Main.deadBodies[Main.listOfTamedBeasts[b]]+=pTarget.a.data.kills;
                                    pTarget.a.data.kills=0;
                                }
                                
                            }
                        }
                    }
                    if (Main.listOfTamedBeasts[b].kingdom != b.kingdom)
                    {
                        b.kingdom.removeUnit(b);
                        b.kingdom = Main.listOfTamedBeasts[b].kingdom;
                        Main.listOfTamedBeasts[b].kingdom.addUnit(b);
                    }
                    if (b.is_moving && !b.has_attack_target && b.asset.id != "fel_dragon" && 
                    b.asset.id != "dragon" &&
                    b.asset.id != "zombie_dragon")
                    {
                        pTile = Main.listOfTamedBeasts[b].currentTile.region.tiles.GetRandom<WorldTile>();
                        b.goTo(pTile, true, true);

                    }
                }
                else 
                {
                    Main.listOfOwners.Remove(Main.listOfTamedBeasts[b]);
                    Main.listOfTamedBeasts.Remove(b);
                    b.removeTrait("Subordinate");
                }
            }
            else
                b.removeTrait("Subordinate");
                return false;
            }
            return true;
        }
        public static bool following1 (BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(Main.listOfTamedBeasts.ContainsKey(pTarget.a))
            {
                if (pTarget.a.isRace("spirit"))
                {
                    //float diplomacy= pTarget.a.stats[S.diplomacy];
                    Main.countSpirit[Main.listOfTamedBeasts[pTarget.a]].Remove(pTarget.GetHashCode());
                } 
                if (Main.listOfOwners.ContainsKey(Main.listOfTamedBeasts[pTarget.a]))
                {
                    Main.listOfOwners[Main.listOfTamedBeasts[pTarget.a]] -= 1;
                }
                Main.listOfTamedBeasts.Remove(pTarget.a);
            }
            
            return true;
        }
        #endregion
#region Переродись
        public static bool reborn(BaseSimObject pTarget, string pStatsID, WorldTile pTile = null, bool saveKingdom=false) 
      	{
         if (pTarget != null)
         {
            TraitEffect.DeathBelive(pTarget, pTile);
            Actor a = pTarget.a;
            if (((Component) a).gameObject == null ||  a == null || !a.inMapBorder())
                return false;
            pTarget.a.removeTrait("Subordinate");
            removeBadTrait(pTarget);
            removeInfectTrait(pTarget);
            
            var act = World.world.units.createNewUnit(pStatsID, pTarget.currentTile, 0f);
            ActorTool.copyUnitToOtherUnit(a, act);
            
            act.data.health += 1000;
            EffectsLibrary.spawn("fx_spawn", act.currentTile, null, null, 0f, -1f, -1f);
            if (Main.deadBodies.ContainsKey(a))
            {
                Main.deadBodies.Add(act,Main.deadBodies[a]);
                Main.deadBodies.Remove(a);
            }
            if (Main.listOfKingdoms.ContainsKey(a))
            {
                if (Main.listOfKingdoms[a].isAlive())
                {
                    act.kingdom = Main.listOfKingdoms[a];
                    Main.listOfKingdoms[a].addUnit(act);
                }
            }
            else
                if (a.kingdom.isAlive() && !a.hasTrait("madness") && saveKingdom)
                {
                    act.kingdom = a.kingdom;
                    a.kingdom.addUnit(act);
                }
            ActionLibrary.removeUnit(pTarget.a);
            return true;
         }
         return true;
        }
        #endregion
#region Строй
        public static Building spawn_building(Actor act, WorldTile pTile, string currentBuilding,
         int units_limit = 5, bool saveKingdom = false, bool unique = true, City pCity = null)
        {
            BuildingAsset buildingAsset = AssetManager.buildings.get(currentBuilding);
            Building newBuilding = null;
            if (!World.world.buildings.canBuildFrom(pTile, buildingAsset, pCity))
            {
                return newBuilding;
            }
            int index = 0;
            if(pTile.building == null)
            {
                foreach ( WorldTile neigh in pTile.neighboursAll)
                {
                    if (neigh.building == null)
                    {
                        index+=1;
                        
                    }
                }
                if (index == pTile.neighboursAll.Length)
                {
                    newBuilding = World.world.buildings.addBuilding(currentBuilding, pTile, true, false, BuildPlacingType.New);
                    if (newBuilding == null )
                    {
                        return newBuilding;
                    }
                    if (unique)
                        Main.listOfBuilding.Add(act,newBuilding);
                    newBuilding.component_unit_spawner.units_limit = units_limit;
                    act.city.addBuilding(newBuilding);
				    newBuilding.retake();
                    if (saveKingdom)
                        newBuilding.kingdom = act.kingdom;
                    return newBuilding;
                }
                
            }
            
            return newBuilding;
        }
        #endregion
#region Иди к
        public static bool goToTarget(BaseSimObject pTarget, string kingdom, WorldTile pTile = null)
        {
            Actor pActor = pTarget.a;
            if (BehaviourActionBase<Actor>.world.worldLaws.world_law_peaceful_monsters.boolVal)
                return false;
            Building building1 = (Building) null;
            float num1 = 0.0f;
            foreach (Building building2 in (ObjectContainer<Building>) World.world.kingdoms.getKingdomByID(kingdom).buildings)
            {
                if (building2.currentTile.isSameIsland(pActor.currentTile))
                {
                    float num2 = Toolbox.DistTile(building2.currentTile, pActor.currentTile);
                    if ( (building1 == null || 
                    (double) num2 < (double) num1) && 
                    (!GodMagic.insquad(building2.currentTile.x, building2.currentTile.y)) )
                    {
                        building1 = building2;
                        num1 = num2;
                    }
                }
            }
            if (building1 == null)
                return false;
            if ( building1 !=  null && pTarget.a.asset.id != "fel_dragon")
            {
                pActor.goTo(building1.currentTile);
            }
            return true;
        }
#endregion
#region Телепортируйся
        public static bool teleportToTarget(BaseSimObject pTarget, WorldTile pTile = null)
        {
            WorldTile worldTile = pTile;
            WorldTile pTile1 = worldTile;
            if (pTile1 == null || pTile1.Type.block || !pTile1.Type.ground)
            return false;
            string pID = pTarget.a.asset.effect_teleport;
            if (string.IsNullOrEmpty(pID))
                pID = "fx_teleport_blue";
            EffectsLibrary.spawnAt(pID, pTarget.currentTile.posV3, pTarget.a.stats[S.scale]);
            BaseEffect baseEffect = EffectsLibrary.spawnAt(pID, pTile1.posV3, pTarget.a.stats[S.scale]);
            if (baseEffect != null)
            baseEffect.spriteAnimation.setFrameIndex(9);
            //pTarget.a.cancelAllBeh();
            pTarget.a.spawnOn(pTile1, 0.0f);
            return true;
        }
  #endregion
#region Эвакуируй

        public static void Evacuation(BaseSimObject pTarget)
        {
            if (pTarget.kingdom.hasEnemies() && pTarget.kingdom.units.Count<=9)
            {
                ActionLibrary.teleportRandom(pTarget.a, pTarget.a);
                foreach (Actor unit in (ObjectContainer<Actor>) pTarget.kingdom.units)
                {
                    if (unit != pTarget.a)
                    {
                        teleportToTarget(unit, pTarget.currentTile);
                        spellCost(pTarget, 10);
                    }
                    
                }
                spellCost(pTarget, 10);
                //spellCost(pTarget,90);
            }
        }
#endregion
#region Бойся серебра
        public static bool fearOfSilver(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget!=null)
            {
                if (pTarget.isActor() && pTarget.isAlive())
                {
                    if (pTarget.a.equipment != null && !pTarget.a.equipment.getSlot(EquipmentType.Weapon).isEmpty())
                    {
                        if (pTarget.a.equipment.getSlot(EquipmentType.Weapon).data.material == "silver" && pSelf.a.data.health > 10)
                        {
                            spellCost(pSelf,0,(int) (pSelf.getMaxHealth()*0.2));
                        }
                    }
                        
                }
            }
                    
            return true;
        }
#endregion
#region Ищи Воду/Лаву
        public static ExecuteEvent damage_goTo(
            Actor actor,
            WorldTile target,
            bool pPathOnLiquid = true,
            bool pWalkOnBlocks = true)
        {
            bool flag1 = false;
            actor.clearOldPath();
            /*if (!DebugConfig.isOn(DebugOption.SystemUnitPathfinding))
            {
                actor.current_path.Add(target);
                return ExecuteEvent.True;
            }*/
            if (actor.asset.isBoat && target.isGoodForBoat())
                return ExecuteEvent.False;
            if (flag1)
            {
                actor.current_path.Add(target);
                return ExecuteEvent.True;
            }
            if (actor.isFlying())
            {
                actor.current_path.Add(target);
                return ExecuteEvent.True;
            }
            bool flag2 = actor.currentTile.isSameIsland(target);
            if (flag2 && !actor.isInLiquid())
                pPathOnLiquid = false;
            World.world.pathfindingParam.resetParam();
            World.world.pathfindingParam.block = true;
            World.world.pathfindingParam.lava = true;
            if (actor.currentTile.Type.lava)
                World.world.pathfindingParam.lava = true;
            World.world.pathfindingParam.ocean = true;
            if (pPathOnLiquid && !actor.asset.damagedByOcean)
                World.world.pathfindingParam.ocean = true;
            else if (actor.isInLiquid())
                World.world.pathfindingParam.ocean = true;
            World.world.pathfindingParam.ground = true;
            World.world.pathfindingParam.boat = actor.asset.isBoat && actor.currentTile.isGoodForBoat();
            World.world.pathfindingParam.limit = true;
            if (!PathfinderTools.tryToGetSimplePath(actor.currentTile, target, actor.current_path, actor.asset, World.world.pathfindingParam))
                actor.current_path.Clear();
            World.world.pathFindingVisualiser.showPath((StaticGrid) null, actor.current_path);
            if (!actor.isUsingPath())
            {
                actor.setTileTarget(target);
                return ExecuteEvent.True;
            }
            actor.setTileTarget(target);
            return ExecuteEvent.True;
        }
        public static bool find_water_lava(Actor pActor, 
        TileLayerType type = TileLayerType.Lava, 
        bool in_chunk = true, 
        bool danger_goto = true)
        {
            WorldTile waterIn = null;
            if (in_chunk)
                waterIn = findWaterOrLava(pActor.currentTile.chunk, type);
            if (waterIn == null)
            {
                for (int index = 0; index > pActor.currentTile.chunk.neighboursAll.Count; index++)
                //foreach (MapChunk pChunk in pActor.currentTile.chunk.neighboursAll)
                {
                    pActor.currentTile.chunk.neighboursAll.ShuffleOne<MapChunk>(index);
                    MapChunk pChunk = pActor.currentTile.chunk.neighboursAll[index];
                    waterIn = findWaterOrLava(pChunk, type);
                    if (waterIn != null)
                        break;
                }
            }
            if (waterIn == null)
                return false;
            //pActor.beh_tile_target = waterIn;
            if (danger_goto)
                damage_goTo(pActor, waterIn);
            else
                pActor.goTo(waterIn);
            return true;
        }

        public static WorldTile findWaterOrLava(MapChunk pChunk, TileLayerType type)
        {
            for (int index = 0; index < pChunk.regions.Count; ++index)
            {
                pChunk.regions.ShuffleOne<MapRegion>(index);
                MapRegion region = pChunk.regions[index];
                if (region.type == type)
                    return region.tiles.GetRandom<WorldTile>();
            }
            return (WorldTile) null;
        }
#endregion
#region Подчиняйтесь
        public static void massSummon(BaseSimObject pTarget, 
        string entity1, string entity2, string entity3, string entity4, 
        int cost, bool Life_energy)
        {
            //int land_power = 0;
            World.world.getObjectsInChunks(pTarget.currentTile, 5, MapObjectType.Actor);
            //DropsLibrary._tempList.AddRange((IEnumerable<Building>) pList);
            for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
            {
                Actor temp = World.world.temp_map_objects[index].a;
                if ((temp.asset.id == entity1 || 
                temp.asset.id == entity2 || 
                temp.asset.id == entity3 || 
                temp.asset.id == entity4) && 
                temp.isAlive() && !temp.hasTrait("Subordinate")
                 )
                {
                    
                    if (Life_energy){
                        Main.listOfEnergy["Life energy"] -=2;
                        obey(pTarget,temp);
                    }
                    else if (pTarget.a.data.hunger>cost || !pTarget.a.asset.needFood){
                        spellCost(pTarget, cost);
                        obey(pTarget,temp);
                    }
                    /*summon( pTarget, 
                            entity, cost, Health, 
                            trait1, trait2, trait3, pTarget.currentTile);*/
                    //land_power++;
                    /*summon( pTarget, 
                    entity, cost, Health, 
                    trait1, trait2, trait3, temp.currentTile);
                    //Actor newUnit = World.world.units.createNewUnit(entity, temp.currentTile);
                    //newUnit.data.set("special_sprite_id", temp.asset.id);
                    //newUnit.data.set("special_sprite_index", temp.animData_index); интересно
                    temp.removeBuildingFinal();*/
                    //return;
                    //newUnit.startColorEffect();
                    return;
                }
            }
            
            //DropsLibrary._tempList.Clear();
        }
        #endregion
/*#region смени облик но не суть
        public static void illusion(BaseSimObject pTarget, 
        string special_id, int special_index, string effect,
        //string entity1, string entity2, string entity3, string entity4, 
        int cost)
        {
            //int land_power = 0;
            //World.world.getObjectsInChunks(pTarget.currentTile, 5, MapObjectType.Actor);
            
            if (pTarget.a.data.hunger>cost || !pTarget.a.asset.needFood)
                {
                    spellCost(pTarget, cost);
                    
                    pTarget.a.is_visible = false;
                    //pTarget.a.data.set("special_sprite_index", special_index);
                    //Protect(pTarget,20,10,effect);
                }
                    //newUnit.data.set("special_sprite_index", temp.animData_index); интересно
            //DropsLibrary._tempList.AddRange((IEnumerable<Building>) pList);
            /*for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
            {
                Actor temp = World.world.temp_map_objects[index].a;
                if ((temp.asset.id == entity1 || 
                temp.asset.id == entity2 || 
                temp.asset.id == entity3 || 
                temp.asset.id == entity4) && 
                temp.isAlive() && !temp.hasTrait("Subordinate")
                 )
                {
                    
                    if (Life_energy){
                        Main.listOfEnergy["Life energy"] -=2;
                        obey(pTarget,temp);
                    }
                    else if (pTarget.a.data.hunger>cost || !pTarget.a.asset.needFood){
                        spellCost(pTarget, cost);
                        obey(pTarget,temp);
                    }
                    /*summon( pTarget, 
                            entity, cost, Health, 
                            trait1, trait2, trait3, pTarget.currentTile);*/
                    //land_power++;
                    /*summon( pTarget, 
                    entity, cost, Health, 
                    trait1, trait2, trait3, temp.currentTile);
                    //Actor newUnit = World.world.units.createNewUnit(entity, temp.currentTile);
                    //newUnit.data.set("special_sprite_id", temp.asset.id);
                    //newUnit.data.set("special_sprite_index", temp.animData_index); интересно
                    temp.removeBuildingFinal();
                    //return;
                    //newUnit.startColorEffect();
                    return;
                }
            }
            
            //DropsLibrary._tempList.Clear();
        }
#endregion*/
#endregion
    }
}