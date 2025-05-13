using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using System.Reflection;

namespace Magic
{
    class MagicUnitys
    {
      public static void init()
      {
        var necromancer = AssetManager.actor_library.get("necromancer");
        var druid = AssetManager.actor_library.get("druid");
        necromancer.traits.Add("The Magic of Death");
        druid.traits.Add("The Magic of Life");
        druid.traits.Add("immune");
        create_water_spirit();
        create_fire_spirit();
        create_earth_spirit();
        create_chimers();
        create_air_spirit();
        create_defile_demon();
        create_angel();
      }
      public static bool spawn_building(WorldTile pTile, string currentBuilding)
      {
        BuildingAsset buildingAsset = AssetManager.buildings.get(currentBuilding);
        Building newBuilding = null;
        if (!World.world.buildings.canBuildFrom(pTile, buildingAsset, (City) null))
        {
          //EffectsLibrary.spawnAtTile("fx_bad_place", pTile, 0.25f);
          return false;
        }
          
        if(pTile.building == null)
        {
          newBuilding = World.world.buildings.addBuilding(currentBuilding, pTile, true, false, BuildPlacingType.New);
          return true;
        }
        if (newBuilding == null )
        {
          foreach ( WorldTile neigh in pTile.neighboursAll)
          {
              if (neigh.building != null)
              {
                  return false;
              }
          }
          newBuilding = World.world.buildings.addBuilding(currentBuilding, pTile, true, false, BuildPlacingType.New);
          if (newBuilding == null )
          {
              return false;
          }
          return true;
        }
        if (newBuilding.asset.cityBuilding && pTile.zone.city != null)
        {
          pTile.zone.city.addBuilding(newBuilding);
          newBuilding.retake();
        }
        return true;
      }
      public static bool DemonSlayer(BaseSimObject pTarget, WorldTile pTile = null)
      {
        if(Toolbox.randomChance(0.4f) && pTarget.a.asset.id == "defile_demon")
        {
          if (Toolbox.randomChance(0.05f))
          {
            spawn_building( pTile,"Flame_Tower");
          }
          else if (Toolbox.randomChance(0.33f))
          {
            spawn_building( pTile,"barracks_demons");
          }
          else if (Toolbox.randomChance(0.5f))
          {
            spawn_building( pTile,"Flame_tower");
          }
          else
          {
            spawn_building(pTile,"HellKennel");
          }
        }
        if ( pTarget == null || !pTarget.isActor())
          return false;
        BaseSimObject attackedBy = pTarget.a.attackedBy;
        if (!(attackedBy != null) || !attackedBy.isActor() || !attackedBy.isAlive())
          return false;
        attackedBy.a.addTrait("Demon Fighter");
        attackedBy.a.addTrait("fire_proof");
        
        return true;
      }
        public static void create_defile_demon()
        {
          var Defile_demon = AssetManager.actor_library.clone("defile_demon", SA.inner_demon);
          Defile_demon.base_stats[S.scale] += 0.05f;
          Defile_demon.base_stats[S.speed] = 45;
          Defile_demon.base_stats[S.armor] = 97;
          Defile_demon.race = "demon";
          Defile_demon.effect_teleport = "fx_teleport_red";
          Defile_demon.take_items = false;
          //Defile_demon.use_items = false;
          Defile_demon.action_death += new  WorldAction(DemonSlayer);
          Defile_demon.base_stats[S.damage] -= 50;
          Defile_demon.defaultWeapons = List.Of<string>("evil_staff");
          AssetManager.actor_library.add(Defile_demon);
          AssetManager.actor_library.CallMethod("addTrait", "Defiler");
          AssetManager.actor_library.CallMethod("addTrait", "Blood Magic"); 
          AssetManager.actor_library.CallMethod("addTrait", "S"); 
          AssetManager.actor_library.CallMethod("loadShadow", Defile_demon);

          var lowest_demon = AssetManager.actor_library.clone("lowest_defile_demon", SA.inner_demon);
          lowest_demon.race = "demon";
          lowest_demon.effect_teleport = "fx_teleport_red";
          lowest_demon.base_stats[S.speed] = 45;
          lowest_demon.defaultWeapons = List.Of<string>("spear", "sword", "bow");
          lowest_demon.defaultWeaponsMaterial = List.Of<string>("adamantine");
          AssetManager.actor_library.add(lowest_demon);
          AssetManager.actor_library.CallMethod("addTrait", "Defiler");
          AssetManager.actor_library.CallMethod("removeTrait", "burning_feet");
          AssetManager.actor_library.CallMethod("addTrait", "B"); 
          AssetManager.actor_library.CallMethod("loadShadow", lowest_demon);

          var hidden_demon = AssetManager.actor_library.clone("hidden_demon", SA.inner_demon);
          hidden_demon.base_stats[S.health] = 500f;
          hidden_demon.race = "demonit";
          hidden_demon.effect_teleport = "fx_teleport_red";
          AssetManager.actor_library.add(hidden_demon);
          AssetManager.actor_library.CallMethod("addTrait", "Defiler");
          AssetManager.actor_library.CallMethod("removeTrait", "burning_feet");
          AssetManager.actor_library.CallMethod("addTrait", "A");
          AssetManager.actor_library.CallMethod("addTrait", "hiddenEvil"); 
          AssetManager.actor_library.CallMethod("loadShadow", hidden_demon);

          var fel_dragon = AssetManager.actor_library.clone("fel_dragon", SA.dragon);
          fel_dragon.base_stats[S.scale] += 0.02f;
          fel_dragon.base_stats[S.armor] = 30f;
          fel_dragon.base_stats[S.health] = 6660f;
          //fel_dragon.action_death += new  WorldAction(DemonSlayer);
          fel_dragon.kingdom = "demons";
          fel_dragon.canBeKilledByDivineLight = true;
          fel_dragon.race = "dragons";
          fel_dragon.effect_teleport = "fx_teleport_red";
          AssetManager.actor_library.add(fel_dragon);
          AssetManager.actor_library.CallMethod("addTrait", "Defiler");
          //AssetManager.actor_library.CallMethod("removeTrait", "burning_feet");
          AssetManager.actor_library.CallMethod("addTrait", "S"); 
          AssetManager.actor_library.CallMethod("loadShadow", fel_dragon);
          //AssetManager.actor_library.CallMethod("addTrait", "Blood Magic"); 

          var demonKing = AssetManager.actor_library.clone("demonKing", "_mob");
          demonKing.nameLocale = "demonKings";
          demonKing.nameTemplate = "demon_name";
          demonKing.race = "demon";
          demonKing.kingdom = "demons";
          demonKing.zombieID = "zombie";
          demonKing.skeletonID = "skeleton";
          demonKing.defaultAttack = "evil_staff";
          //hellhound.defaultWeapons = List.Of<string>("white_staff");
          demonKing.animation_walk = "walk_0,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,";
          demonKing.animation_swim = "walk_1,walk_2,walk_3";
          demonKing.texture_path = "demonKings";
          demonKing.icon = "demonKings";
          demonKing.job = "evil_mage";
          demonKing.effect_teleport = "fx_teleport_red";
          demonKing.attack_spells = List.Of<string>("teleportRandom");
          demonKing.color = Toolbox.makeColor("#8c160c", -1f);
          demonKing.base_stats[S.max_age] = 10000;
          demonKing.base_stats[S.attack_speed] = 40f;
          demonKing.base_stats[S.health] = 666;
          demonKing.base_stats[S.speed] = 66f;
          demonKing.base_stats[S.damage] = -33f;
          demonKing.base_stats[S.scale] += 0.09f;
          demonKing.base_stats[S.armor] += 99f;
          demonKing.base_stats[S.knockback_reduction] = 10f;
          demonKing.canBeKilledByDivineLight = true;
          demonKing.canBeKilledByLifeEraser = false;
          demonKing.ignoredByInfinityCoin = false;
          demonKing.disableJumpAnimation = true;
          demonKing.canBeMovedByPowers = false;
          demonKing.canAttackBuildings = true;
          demonKing.canTurnIntoZombie = false;
          demonKing.canTurnIntoMush = false;
          demonKing.canTurnIntoTumorMonster = false;
          demonKing.hideFavoriteIcon = false;
          demonKing.can_edit_traits = false;
          demonKing.very_high_flyer = true;
          demonKing.damagedByOcean = false;
          demonKing.damagedByRain = true;
          demonKing.action_liquid = new WorldAction(ActionLibrary.swimToIsland);
          demonKing.landCreature = true;
          demonKing.oceanCreature = false;
          demonKing.swampCreature = true;
          demonKing.dieOnGround = false;
          demonKing.take_items = false;
          demonKing.use_items = false;
          demonKing.diet_meat = true;
          demonKing.has_soul = false;
          demonKing.dieInLava = false;
          demonKing.needFood = false;
          demonKing.flying = false;
          //hellhound.action_death += new  WorldAction(DemonSlayer);
          AssetManager.actor_library.add(demonKing);
          AssetManager.actor_library.CallMethod("addTrait", "Defiler");
          AssetManager.actor_library.CallMethod("addTrait", "immortal");
          AssetManager.actor_library.CallMethod("addTrait", "SSS");
          AssetManager.actor_library.CallMethod("addTrait", "fire_proof");
          AssetManager.actor_library.CallMethod("addTrait", "burning_feet");
          AssetManager.actor_library.CallMethod("addTrait", "Regeneration of the Lizard");
          AssetManager.actor_library.CallMethod("addTrait", "fire_blood");
          AssetManager.actor_library.CallMethod("addTrait", "Blood Magic");
          AssetManager.actor_library.CallMethod("loadShadow", demonKing);
          Localization.addLocalization(demonKing.nameLocale, demonKing.nameLocale);
          
          var hellhound = AssetManager.actor_library.clone("hellhound", "_mob");
          hellhound.nameLocale = "hellhound";
          hellhound.nameTemplate = "wolf_name";
          hellhound.race = "demon";
          hellhound.kingdom = "demons";
          hellhound.zombieID = "zombie";
          hellhound.skeletonID = "skeleton";
          //angel.defaultAttack = "white_staff";
          //hellhound.defaultWeapons = List.Of<string>("white_staff");
          hellhound.animation_walk = "walk_0,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2,walk_1,walk_2";
          hellhound.animation_swim = "swim_0,swim_1,swim_2";
          hellhound.texture_path = "hellhound";
          hellhound.icon = "hellhound";
          hellhound.job = "move_mob";
          hellhound.color = Toolbox.makeColor("#8c160c", -1f);
          hellhound.base_stats[S.max_age] = 1000;
          hellhound.base_stats[S.attack_speed] = 90f;
          hellhound.base_stats[S.health] = 200;
          hellhound.base_stats[S.speed] = 90f;
          hellhound.base_stats[S.damage] = 60f;
          hellhound.base_stats[S.scale] += 0.04f;
          hellhound.canBeKilledByDivineLight = true;
          hellhound.canBeKilledByLifeEraser = true;
          hellhound.ignoredByInfinityCoin = false;
          hellhound.disableJumpAnimation = true;
          hellhound.canBeMovedByPowers = true;
          hellhound.canAttackBuildings = true;
          hellhound.canTurnIntoZombie = false;
          hellhound.canTurnIntoMush = false;
          hellhound.canTurnIntoTumorMonster = false;
          hellhound.hideFavoriteIcon = false;
          hellhound.can_edit_traits = true;
          hellhound.very_high_flyer = false;
          hellhound.damagedByOcean = true;
          hellhound.damagedByRain = true;
          hellhound.action_liquid = new WorldAction(ActionLibrary.swimToIsland);
          hellhound.landCreature = true;
          hellhound.oceanCreature = false;
          hellhound.swampCreature = true;
          hellhound.dieOnGround = false;
          hellhound.take_items = false;
          hellhound.use_items = false;
          hellhound.diet_meat = true;
          hellhound.has_soul = false;
          hellhound.dieInLava = false;
          hellhound.needFood = false;
          hellhound.flying = false;
          //hellhound.action_death += new  WorldAction(DemonSlayer);
          AssetManager.actor_library.add(hellhound);
          AssetManager.actor_library.CallMethod("addTrait", "Defiler");
          AssetManager.actor_library.CallMethod("addTrait", "immortal");
          AssetManager.actor_library.CallMethod("addTrait", "C");
          AssetManager.actor_library.CallMethod("addTrait", "fire_proof");
          AssetManager.actor_library.CallMethod("addTrait", "regeneration");
          AssetManager.actor_library.CallMethod("addTrait", "fire_blood");
          AssetManager.actor_library.CallMethod("addTrait", "fast");
          AssetManager.actor_library.CallMethod("loadShadow", hellhound);
          Localization.addLocalization(hellhound.nameLocale, hellhound.nameLocale);
        }
        public static void create_chimers()
        {
          var chimera_bear = AssetManager.actor_library.clone("chimera_bear", SA.bear);
          chimera_bear.base_stats[S.scale] += 0.05f;
          //chimera_bear.base_stats[S.speed] = 45;
          chimera_bear.base_stats[S.armor] = 5;
          chimera_bear.race = "chimera";
          chimera_bear.kingdom = "chimera";
          chimera_bear.effect_teleport = "fx_teleport_blue";
          chimera_bear.take_items = false;
          //Defile_demon.use_items = false;
          //chimera_bear.action_death += new  WorldAction(DemonSlayer);
          //chimera_bear.base_stats[S.damage] -= 50;
          //chimera_bear.defaultWeapons = List.Of<string>("evil_staff");
          AssetManager.actor_library.add(chimera_bear);
          AssetManager.actor_library.CallMethod("addTrait", "BottomlessSource");
          AssetManager.actor_library.CallMethod("addTrait", "fast"); 
          AssetManager.actor_library.CallMethod("addTrait", "B"); 
          AssetManager.actor_library.CallMethod("addTrait", "regeneration"); 
          AssetManager.actor_library.CallMethod("addTrait", "immune"); 
          AssetManager.actor_library.CallMethod("addTrait", "giant"); 
          AssetManager.actor_library.CallMethod("addTrait", "tough"); 
          AssetManager.actor_library.CallMethod("loadShadow", chimera_bear);

          var chimera_monkey = AssetManager.actor_library.clone("chimera_monkey", SA.monkey);
          chimera_monkey.base_stats[S.scale] += 0.05f;
          chimera_monkey.base_stats[S.armor] = 5;
          chimera_monkey.base_stats[S.mod_health] += 0.2f;
          chimera_monkey.race = "chimera";
          chimera_monkey.kingdom = "chimera";
          AssetManager.actor_library.add(chimera_monkey);
          AssetManager.actor_library.CallMethod("addTrait", "BottomlessSource");
          AssetManager.actor_library.CallMethod("addTrait", "fast"); 
          AssetManager.actor_library.CallMethod("addTrait", "B"); 
          AssetManager.actor_library.CallMethod("addTrait", "healing_aura"); 
          AssetManager.actor_library.CallMethod("addTrait", "immune"); 
          AssetManager.actor_library.CallMethod("addTrait", "agile"); 
          AssetManager.actor_library.CallMethod("addTrait", "eagle_eyed"); 
          AssetManager.actor_library.CallMethod("loadShadow", chimera_monkey);

          var chimera_wolf = AssetManager.actor_library.clone("chimera_wolf", SA.wolf);
          chimera_wolf.base_stats[S.scale] += 0.05f;
          chimera_wolf.base_stats[S.armor] = 5;
          chimera_wolf.base_stats[S.mod_health] += 0.5f;
          chimera_wolf.race = "chimera";
          chimera_wolf.kingdom = "chimera";
          AssetManager.actor_library.add(chimera_wolf);
          AssetManager.actor_library.CallMethod("addTrait", "BottomlessSource");
          AssetManager.actor_library.CallMethod("addTrait", "fast"); 
          AssetManager.actor_library.CallMethod("addTrait", "B"); 
          AssetManager.actor_library.CallMethod("addTrait", "regeneration"); 
          AssetManager.actor_library.CallMethod("addTrait", "immune"); 
          AssetManager.actor_library.CallMethod("addTrait", "agile"); 
          AssetManager.actor_library.CallMethod("addTrait", "strong"); 
          AssetManager.actor_library.CallMethod("loadShadow", chimera_wolf);

          var chimera_snake = AssetManager.actor_library.clone("chimera_snake", SA.snake);
          chimera_snake.base_stats[S.scale] += 0.05f;
          chimera_snake.base_stats[S.armor] = 5;
          chimera_snake.base_stats[S.health] += 60;
          chimera_snake.base_stats[S.damage] += 5;
          chimera_snake.base_stats[S.mod_health] += 0.1f;
          chimera_snake.race = "chimera";
          chimera_snake.kingdom = "chimera";
          AssetManager.actor_library.add(chimera_snake);
          AssetManager.actor_library.CallMethod("addTrait", "BottomlessSource");
          AssetManager.actor_library.CallMethod("addTrait", "fast"); 
          AssetManager.actor_library.CallMethod("addTrait", "B"); 
          AssetManager.actor_library.CallMethod("addTrait", "regeneration"); 
          AssetManager.actor_library.CallMethod("addTrait", "immune"); 
          AssetManager.actor_library.CallMethod("addTrait", "agile"); 
          AssetManager.actor_library.CallMethod("addTrait", "poisonous"); 
          AssetManager.actor_library.CallMethod("loadShadow", chimera_snake);
        }
        public static void create_angel()
        {
          var angel = AssetManager.actor_library.clone("angel", "_mob");
          angel.nameLocale = "angel";
          angel.nameTemplate = "elf_name";
          angel.race = "angel";
          angel.kingdom = "good";
          angel.zombieID = "zombie";
          angel.skeletonID = "skeleton";
          angel.attack_spells = List.Of<string>(new string[]
          {
            "divine",
            "cure",
            "bloodRain",
            "shield"
          });
          angel.effect_cast_top = "fx_cast_top_blue";
          angel.effect_cast_ground = "fx_cast_ground_blue";
          //angel.defaultAttack = "white_staff";
          angel.defaultWeapons = List.Of<string>("white_staff");
          angel.animation_walk = "walk_0,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3";
          angel.animation_swim = "walk_1,walk_2,walk_3";
          angel.texture_path = "angel";
          angel.icon = "angel";
          angel.job = "move_mob";
          angel.color = Toolbox.makeColor("#f0f227", -1f);
          angel.base_stats[S.max_age] = 1000;
          angel.base_stats[S.attack_speed] = 90f;
          angel.base_stats[S.health] = 355;
          angel.base_stats[S.speed] = 60f;
          angel.base_stats[S.damage] = 1f;
          angel.canBeKilledByDivineLight = false;
          angel.canBeKilledByLifeEraser = true;
          angel.ignoredByInfinityCoin = false;
          angel.disableJumpAnimation = true;
          angel.canBeMovedByPowers = true;
          angel.canAttackBuildings = true;
          angel.canTurnIntoZombie = false;
          angel.canTurnIntoMush = false;
          angel.canTurnIntoTumorMonster = false;
          angel.hideFavoriteIcon = false;
          angel.can_edit_traits = true;
          angel.very_high_flyer = true;
          angel.damagedByOcean = false;
          angel.damagedByRain = false;
          angel.action_liquid = new WorldAction(ActionLibrary.swimToIsland);
          angel.landCreature = true;
          angel.oceanCreature = false;
          angel.swampCreature = true;
          angel.dieOnGround = false;
          angel.take_items = false;
          angel.use_items = true;
          angel.diet_meat = false;
          angel.has_soul = false;
          angel.dieInLava = true;
          angel.needFood = false;
          angel.flying = false;
          AssetManager.actor_library.add(angel);
          AssetManager.actor_library.CallMethod("addTrait", "immortal"); 
          AssetManager.actor_library.CallMethod("addTrait", "freeze_proof");
          AssetManager.actor_library.CallMethod("addTrait", "A");
          AssetManager.actor_library.CallMethod("addTrait", "Demon Fighter");   
          AssetManager.actor_library.CallMethod("addTrait", "blessed"); 
          //AssetManager.actor_library.CallMethod("addTrait", "fire_proof"); 
          //AssetManager.actor_library.CallMethod("addTrait", "Spirit"); 
          //AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
          //AssetManager.actor_library.CallMethod("addTrait", "C"); 
          AssetManager.actor_library.CallMethod("loadShadow", angel);
          Localization.addLocalization(angel.nameLocale, angel.nameLocale);
        }
        public static void create_water_spirit()
        {
        var water_spirit = AssetManager.actor_library.clone("water_spirit", "_mob");
        water_spirit.nameLocale = "water_spirit";
        water_spirit.nameTemplate = "phoenix_name";
        water_spirit.race = "spirit";
        water_spirit.kingdom = "spirit";
        water_spirit.zombieID = "zombie";
        water_spirit.skeletonID = "skeleton";
        water_spirit.attack_spells = List.Of<string>(new string[]
		    {
        "rain",
        "spiritInitiation",
        "cure",
        "bloodRain",
        "shield"
		    });
        water_spirit.effect_cast_top = "fx_cast_top_green";
		    water_spirit.effect_cast_ground = "fx_cast_ground_green";
        water_spirit.defaultAttack = "white_staff";
        water_spirit.animation_walk = "walk_0,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3";
        water_spirit.animation_swim = "walk_1,walk_2,walk_3";
        water_spirit.texture_path = "water_spirit";
        water_spirit.icon = "water_spirit";
        water_spirit.job = "move_mob";
        water_spirit.color = Toolbox.makeColor("#8b3aee", -1f);
        water_spirit.base_stats[S.max_age] = 1000;
        water_spirit.base_stats[S.attack_speed] = 40f;
        water_spirit.base_stats[S.health] = 200;
        water_spirit.base_stats[S.speed] = 100f;
        water_spirit.base_stats[S.damage] = 1f;
        water_spirit.canBeKilledByDivineLight = true;
        water_spirit.canBeKilledByLifeEraser = true;
        water_spirit.ignoredByInfinityCoin = false;
        water_spirit.disableJumpAnimation = true;
        water_spirit.canBeMovedByPowers = true;
        water_spirit.canAttackBuildings = true;
        water_spirit.canTurnIntoZombie = false;
        water_spirit.canTurnIntoMush = false;
        water_spirit.canTurnIntoTumorMonster = false;
        water_spirit.hideFavoriteIcon = false;
        water_spirit.can_edit_traits = true;
        water_spirit.very_high_flyer = true;
        water_spirit.damagedByOcean = false;
        water_spirit.damagedByRain = false;
        water_spirit.action_liquid = new WorldAction(ActionLibrary.swimToIsland);
        water_spirit.landCreature = true;
        water_spirit.oceanCreature = true;
        water_spirit.swampCreature = true;
        water_spirit.dieOnGround = false;
        water_spirit.take_items = false;
        water_spirit.use_items = false;
        water_spirit.diet_meat = false;
        water_spirit.has_soul = false;
		    water_spirit.dieInLava = true;
        water_spirit.needFood = false;
        water_spirit.flying = false;
        AssetManager.actor_library.add(water_spirit);
        AssetManager.actor_library.CallMethod("addTrait", "immortal"); 
        AssetManager.actor_library.CallMethod("addTrait", "freeze_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "fire_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "Spirit"); 
        AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
        AssetManager.actor_library.CallMethod("addTrait", "C"); 
        AssetManager.actor_library.CallMethod("loadShadow", water_spirit);
        Localization.addLocalization(water_spirit.nameLocale, water_spirit.nameLocale);
        }

        public static void create_fire_spirit()
        {
        var Fire_spirit = AssetManager.actor_library.clone("Fire_spirit", "_mob");
        Fire_spirit.nameLocale = "Fire_spirit";
        Fire_spirit.nameTemplate = "phoenix_name";
        Fire_spirit.race = "Fire_spirit";
        Fire_spirit.kingdom = "spirit";
        Fire_spirit.zombieID = "zombie";
        Fire_spirit.skeletonID = "skeleton";
        Fire_spirit.attack_spells = List.Of<string>(new string[]
		    {
			  "fire",
        "spiritInitiation",
        "shield",
        "lava",
        "bloodRain",
        //"Rain"
		    });
        Fire_spirit.effect_cast_top = "fx_cast_top_red";
		    Fire_spirit.effect_cast_ground = "fx_cast_ground_red";
        Fire_spirit.defaultAttack = "FireSpiritEffect";
        Fire_spirit.animation_walk = "walk_0,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3";
        Fire_spirit.animation_swim = "walk_1,walk_2,walk_3";
        Fire_spirit.texture_path = "Fire_spirit";
        Fire_spirit.icon = "Fire_spirit";
        Fire_spirit.job = "move_mob";
        Fire_spirit.color = Toolbox.makeColor("#8b3aee", -1f);
        Fire_spirit.base_stats[S.max_age] = 1000;
        Fire_spirit.base_stats[S.attack_speed] = 40f;
        Fire_spirit.base_stats[S.health] = 200;
        Fire_spirit.base_stats[S.speed] = 100f;
        Fire_spirit.base_stats[S.damage] = 20f;
        Fire_spirit.canBeKilledByDivineLight = true;
        Fire_spirit.canBeKilledByLifeEraser = true;
        Fire_spirit.ignoredByInfinityCoin = false;
        Fire_spirit.disableJumpAnimation = true;
        Fire_spirit.canBeMovedByPowers = true;
        Fire_spirit.canAttackBuildings = true;
        Fire_spirit.canTurnIntoZombie = false;
        Fire_spirit.canTurnIntoMush = false;
        Fire_spirit.canTurnIntoTumorMonster = false;
        Fire_spirit.hideFavoriteIcon = false;
        Fire_spirit.can_edit_traits = true;
        Fire_spirit.very_high_flyer = true;
        Fire_spirit.damagedByOcean = true;
        Fire_spirit.swampCreature = true;
        Fire_spirit.damagedByRain = true;
        Fire_spirit.oceanCreature = false;
        Fire_spirit.action_liquid = new WorldAction(ActionLibrary.swimToIsland);
        Fire_spirit.landCreature = true;
        Fire_spirit.dieOnGround = false;
        Fire_spirit.take_items = false;
        Fire_spirit.use_items = false;
        Fire_spirit.diet_meat = false;
        Fire_spirit.has_soul = false;
		    Fire_spirit.dieInLava = false;
        Fire_spirit.needFood = false;
        Fire_spirit.flying = false;
        Fire_spirit.disableJumpAnimation = true;
        AssetManager.actor_library.add(Fire_spirit);
        AssetManager.actor_library.CallMethod("addTrait", "immortal"); 
        AssetManager.actor_library.CallMethod("addTrait", "freeze_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "fire_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "Spirit"); 
        AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
        AssetManager.actor_library.CallMethod("addTrait", "C"); 
        AssetManager.actor_library.CallMethod("loadShadow", Fire_spirit);
        Localization.addLocalization(Fire_spirit.nameLocale, Fire_spirit.nameLocale);
        }

        public static void create_earth_spirit()
        {
        var earth_spirit = AssetManager.actor_library.clone("earth_spirit", "_mob");
        earth_spirit.nameLocale = "earth_spirit";
        earth_spirit.nameTemplate = "phoenix_name";
        earth_spirit.race = "spirit";
        earth_spirit.kingdom = "spirit";
        earth_spirit.zombieID = "zombie";
        earth_spirit.skeletonID = "skeleton";
        earth_spirit.attack_spells = List.Of<string>(new string[]
		    {
        "spiritInitiation",
        "Earthquake",
        "spawnFertilizer",
        "shield",
        //"bloodRain",
        "lava",
		    });
        earth_spirit.effect_cast_top = "fx_cast_top_green";
		    earth_spirit.effect_cast_ground = "fx_cast_ground_green";
        earth_spirit.defaultAttack = "druid_staff";
        earth_spirit.animation_walk = "walk_0,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3";
        earth_spirit.animation_swim = "walk_1,walk_2,walk_3";
        earth_spirit.texture_path = "earth_spirit";
        earth_spirit.icon = "earth_spirit";
        earth_spirit.job = "move_mob";
        earth_spirit.color = Toolbox.makeColor("#8b3aee", -1f);
        earth_spirit.base_stats[S.max_age] = 1000;
        earth_spirit.base_stats[S.attack_speed] = 40f;
        earth_spirit.base_stats[S.health] = 250;
        earth_spirit.base_stats[S.speed] = 30f;
        earth_spirit.base_stats[S.armor] = 50f;
        earth_spirit.base_stats[S.damage] = 1f;
        earth_spirit.canBeKilledByDivineLight = true;
        earth_spirit.canBeKilledByLifeEraser = true;
        earth_spirit.ignoredByInfinityCoin = false;
        earth_spirit.disableJumpAnimation = true;
        earth_spirit.canBeMovedByPowers = true;
        earth_spirit.canAttackBuildings = true;
        earth_spirit.canTurnIntoZombie = false;
        earth_spirit.canTurnIntoMush = false;
        earth_spirit.canTurnIntoTumorMonster = false;
        earth_spirit.hideFavoriteIcon = false;
        earth_spirit.can_edit_traits = true;
        earth_spirit.very_high_flyer = true;
        earth_spirit.damagedByOcean = true;
        earth_spirit.dieOnBlocks = false;
        earth_spirit.moveFromBlock = false;
        earth_spirit.ignoreTileSpeedMod = true;
        earth_spirit.ignoreBlocks = true;
        earth_spirit.swampCreature = true;
        earth_spirit.damagedByRain = false;
        earth_spirit.oceanCreature = false;
        earth_spirit.action_liquid = new WorldAction(ActionLibrary.swimToIsland);
        earth_spirit.landCreature = true;
        earth_spirit.dieOnGround = false;
        earth_spirit.take_items = false;
        earth_spirit.use_items = false;
        earth_spirit.diet_meat = false;
        earth_spirit.has_soul = false;
		    earth_spirit.dieInLava = false;
        earth_spirit.needFood = false;
        earth_spirit.flying = true;
        earth_spirit.disableJumpAnimation = true;
        AssetManager.actor_library.add(earth_spirit);
        AssetManager.actor_library.CallMethod("addTrait", "immortal"); 
        AssetManager.actor_library.CallMethod("addTrait", "flower_prints"); 
        AssetManager.actor_library.CallMethod("addTrait", "freeze_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "fire_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "Spirit"); 
        AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
        AssetManager.actor_library.CallMethod("addTrait", "C"); 
        AssetManager.actor_library.CallMethod("loadShadow", earth_spirit);
        Localization.addLocalization(earth_spirit.nameLocale, earth_spirit.nameLocale);
        }

        public static bool reborn_air_spirit(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null && 
         !pTarget.a.isRace("necromancer") && 
         !(pTarget.a.asset.id=="ghost") && 
         !pTarget.a.hasTrait("Werewolf"))
         {
            FunctionalAction.removeInfectTrait(pTarget);
            FunctionalAction.reborn(pTarget,"air_spirit",pTile,false);
            //pTarget.a.removeTrait("Blood Magic");
            //ActionLibrary.mageSlayer(pTarget);
            return true;
         }
      	 return true;
        }
        public static bool reborn_tornado(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null && 
         !pTarget.a.isRace("necromancer") && 
         !(pTarget.a.asset.id=="ghost") && 
         !pTarget.a.hasTrait("Werewolf") && 
         Toolbox.randomChance(0.2f))
         {
            FunctionalAction.removeInfectTrait(pTarget);
            FunctionalAction.reborn(pTarget,"spirit_tornado",pTile,false);
            //pTarget.a.removeTrait("Blood Magic");
            //ActionLibrary.mageSlayer(pTarget);
            return true;
         }
         else if(Toolbox.randomChance(0.2f)){
          MapBox.spawnLightningSmall(pTarget.currentTile,0.1f);
         }
      	 return true;
        }
        public static void create_air_spirit()
        {
          var spirit_tornado = AssetManager.actor_library.clone("spirit_tornado", SA.tornado);
          spirit_tornado.action_death += new  WorldAction(reborn_air_spirit);
          //spirit_tornado.ignoreJobs = true;
          //spirit_tornado.has_ai_system = false;
          //spirit_tornado.canBeMovedByPowers = false;
          //spirit_tornado.base_stats[S.scale] = 0.1f;
          //spirit_tornado.base_stats[S.health] = 1f;
          AssetManager.actor_library.add(spirit_tornado);
          AssetManager.actor_library.CallMethod("loadShadow", spirit_tornado);

        var air_spirit = AssetManager.actor_library.clone("air_spirit", "_mob");
        air_spirit.nameLocale = "air_spirit";
        air_spirit.nameTemplate = "phoenix_name";
        air_spirit.action_death += new  WorldAction(reborn_tornado);
        air_spirit.race = "spirit";
        air_spirit.kingdom = "spirit";
        air_spirit.zombieID = "zombie";
        air_spirit.skeletonID = "skeleton";
        air_spirit.attack_spells = List.Of<string>(new string[]
		    {
        "spiritInitiation",
        "tornado",
        "lightning",
        "bloodRain",
        "tornado",
		    });
        air_spirit.effect_cast_top = "fx_cast_top_blue";
		    air_spirit.effect_cast_ground = "fx_cast_ground_blue";
        air_spirit.defaultAttack = "AirSpiritEffect";
        air_spirit.animation_walk = "walk_0,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3,walk_1,walk_2,walk_3";
        air_spirit.animation_swim = "walk_1,walk_2,walk_3";
        air_spirit.texture_path = "air_spirit";
        air_spirit.icon = "air_spirit";
        air_spirit.job = "move_mob";
        air_spirit.color = Toolbox.makeColor("#8b3aee", -1f);
        air_spirit.base_stats[S.max_age] = 1000;
        air_spirit.base_stats[S.attack_speed] = 80f;
        air_spirit.base_stats[S.health] = 250;
        air_spirit.base_stats[S.speed] = 90f;
        air_spirit.base_stats[S.knockback_reduction] = 2f;
        air_spirit.base_stats[S.damage] = 30f;
        air_spirit.canBeKilledByDivineLight = true;
        air_spirit.canBeKilledByLifeEraser = true;
        air_spirit.ignoredByInfinityCoin = false;
        air_spirit.disableJumpAnimation = true;
        air_spirit.canBeMovedByPowers = true;
        air_spirit.canAttackBuildings = true;
        air_spirit.canTurnIntoZombie = false;
        air_spirit.canTurnIntoMush = false;
        air_spirit.canTurnIntoTumorMonster = false;
        air_spirit.hideFavoriteIcon = false;
        air_spirit.can_edit_traits = true;
        air_spirit.very_high_flyer = false;
        air_spirit.damagedByOcean = false;
        air_spirit.swampCreature = true;
        air_spirit.damagedByRain = false;
        air_spirit.oceanCreature = true;
        //air_spirit.flag_tornado = true;
        air_spirit.disableJumpAnimation = true;
        air_spirit.updateZ = true;
        air_spirit.action_liquid = new WorldAction(ActionLibrary.swimToIsland);
        air_spirit.landCreature = true;
        air_spirit.dieOnGround = false;
        air_spirit.dieOnBlocks = false;
        air_spirit.ignoreBlocks = true;
        air_spirit.canBeHurtByPowers = false;
        air_spirit.moveFromBlock = false;
        air_spirit.ignoreTileSpeedMod = true;
        air_spirit.take_items = false;
        air_spirit.use_items = false;
        air_spirit.diet_meat = false;
        air_spirit.has_soul = false;
		    air_spirit.dieInLava = false;
        air_spirit.needFood = false;
        air_spirit.flying = true;
        AssetManager.actor_library.add(air_spirit);
        AssetManager.actor_library.CallMethod("addTrait", "immortal"); 
        //AssetManager.actor_library.CallMethod("addTrait", "whirlwind");
        AssetManager.actor_library.CallMethod("addTrait", "freeze_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "fire_proof"); 
        AssetManager.actor_library.CallMethod("addTrait", "Spirit"); 
        AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
        AssetManager.actor_library.CallMethod("addTrait", "C"); 
        AssetManager.actor_library.CallMethod("loadShadow", air_spirit);
        Localization.addLocalization(air_spirit.nameLocale, air_spirit.nameLocale);
        }

        
       
    }
}