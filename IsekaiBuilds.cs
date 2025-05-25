using System;
using System.Linq;
using System.Collections.Generic;
using NCMS.Utils;
using UnityEngine;
using HarmonyLib;

namespace Isekai
{
    class IsekaiBuilds
    {
        private List<BuildingAsset> humanBuildings = new List<BuildingAsset>();
        public void init()
        {
            foreach (BuildingAsset humanBuilding in AssetManager.buildings.list)
            {
                if (humanBuilding.race == "human")
                {
                    humanBuildings.Add(humanBuilding);
                }
            }
            IsekaiBuilds_init();
            foreach (var race in IsekaiRaceLibrary.additionalRaces)
            {
                if (race == "android")
                {
                    initandroid();
                }
                if (race == "darkelve")
                {
                    initdarkelve();
                }
                if (race == "beastmen")
                {
                    initbeastmen();
                }
                if (race == "gnome")
                {
                    initgnome();
                }
                if (race == "angel")
                {
                    initangel();
                }
                if (race == "demonic")
                {
                    initdemonic();
                }
                if (race == "japaneses")
                {
                    initjapaneses();
                }
                if (race == "ancientchina")
                {
                    initancientchina();
                }
            }
        }

        private void IsekaiBuilds_init()
        {
            BuildingAsset Gate = AssetManager.buildings.clone("Gate", "!building");
            Gate.id = "Gate";
            Gate.type = "Gate";
            Gate.race = "Japan";
            Gate.kingdom = "Japan";
            Gate.spawnUnits_asset = "unit_japaneses";
            Gate.spawnUnits_asset = "japansoldier";
            Gate.spawnUnits_asset = "japanhelicopter";
            Gate.spawnUnits_asset = "japantank";
            Gate.spawnUnits_asset = "unit_japaneses";
            Gate.material = "building";
            Gate.sound_idle = "event:/SFX/BUILDINGS_IDLE/IdleIceTower";
            Gate.sound_built = "event:/SFX/BUILDINGS/SpawnBuildingStone";
            Gate.sound_destroyed = "event:/SFX/BUILDINGS/DestroyBuildingStone";
            Gate.base_stats[S.health] = 100000;
            Gate.fundament = new BuildingFundament(1, 1, 1, 0);
            Gate.damagedByRain = false;
            Gate.burnable = false;
            Gate.buildRoadTo = false;
            Gate.destroyOnLiquid = false;
            Gate.affectedByAcid = false;
            Gate.affectedByLava = false;
            Gate.canBePlacedOnLiquid = true;
            Gate.canBePlacedOnBlocks = true;
            Gate.canBeDamagedByTornado = false;
            Gate.ignoredByCities = false;
            Gate.canBeLivingHouse = false;
            Gate.checkForCloseBuilding = false;
            Gate.ignoreBuildings = true;
            Gate.spawnUnits = true;
            AssetManager.buildings.add(Gate);
            AssetManager.buildings.loadSprites(Gate);

            BuildingAsset WitchTower = AssetManager.buildings.clone("WitchTower", "!building");
            WitchTower.id = "WitchTower";
            WitchTower.type = "WitchTower";
            WitchTower.race = "Witch";
            WitchTower.kingdom = "undead";
            WitchTower.spawnUnits_asset = "witch";
            WitchTower.material = "building";
            WitchTower.sound_idle = "event:/SFX/BUILDINGS_IDLE/IdleIceTower";
            WitchTower.sound_built = "event:/SFX/BUILDINGS/SpawnBuildingStone";
            WitchTower.sound_destroyed = "event:/SFX/BUILDINGS/DestroyBuildingStone";
            WitchTower.base_stats[S.health] = 1000f;
            WitchTower.fundament = new BuildingFundament(3, 3, 4, 0);
            WitchTower.damagedByRain = false;
            WitchTower.burnable = false;
            WitchTower.buildRoadTo = false;
            WitchTower.destroyOnLiquid = false;
            WitchTower.affectedByAcid = false;
            WitchTower.affectedByLava = false;
            WitchTower.canBePlacedOnLiquid = true;
            WitchTower.canBePlacedOnBlocks = true;
            WitchTower.canBeDamagedByTornado = false;
            WitchTower.ignoredByCities = false;
            WitchTower.canBeLivingHouse = false;
            WitchTower.checkForCloseBuilding = false;
            WitchTower.ignoreBuildings = true;
            WitchTower.spawnUnits = true;
            AssetManager.buildings.add(WitchTower);
            AssetManager.buildings.loadSprites(WitchTower);
        }

        private static void initandroid()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "android";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_android", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_android");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_android");
            addVariantsUpgrade(pAsset, "house_android", List.Of<string>("hall_android"));
            addVariantsUpgrade(pAsset, "1house_android", List.Of<string>("1hall_android"));
            addVariantsUpgrade(pAsset, "2house_android", List.Of<string>("1hall_android"));
            addVariantsUpgrade(pAsset, "3house_android", List.Of<string>("2hall_android"));
            addVariantsUpgrade(pAsset, "4house_android", List.Of<string>("2hall_android"));
            pAsset.addUpgrade("hall_android", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_android", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_android");
            pAsset.addUpgrade("fishing_docks_android");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_android");
            pAsset.addBuilding("windmill_android", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_android", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_android", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_android");
            pAsset.addBuilding("barracks_android", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_android");
            pAsset.addBuilding("watch_tower_android", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_android", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_android");
            pAsset.addBuilding("temple_android", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_android", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_android");
        }

        private static void initdarkelve()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "darkelve";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_darkelve", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_darkelve");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_darkelve");
            addVariantsUpgrade(pAsset, "house_darkelve", List.Of<string>("hall_darkelve"));
            addVariantsUpgrade(pAsset, "1house_darkelve", List.Of<string>("1hall_darkelve"));
            addVariantsUpgrade(pAsset, "2house_darkelve", List.Of<string>("1hall_darkelve"));
            addVariantsUpgrade(pAsset, "3house_darkelve", List.Of<string>("2hall_darkelve"));
            addVariantsUpgrade(pAsset, "4house_darkelve", List.Of<string>("2hall_darkelve"));
            pAsset.addUpgrade("hall_darkelve", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_darkelve", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_darkelve");
            pAsset.addUpgrade("fishing_docks_darkelve");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_darkelve");
            pAsset.addBuilding("windmill_darkelve", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_darkelve", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_darkelve", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_darkelve");
            pAsset.addBuilding("barracks_darkelve", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_darkelve");
            pAsset.addBuilding("watch_tower_darkelve", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_darkelve", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_darkelve");
            pAsset.addBuilding("temple_darkelve", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_darkelve", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_darkelve");
        }

        private static void initbeastmen()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "beastmen";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_beastmen", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_beastmen");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_beastmen");
            addVariantsUpgrade(pAsset, "house_beastmen", List.Of<string>("hall_beastmen"));
            addVariantsUpgrade(pAsset, "1house_beastmen", List.Of<string>("1hall_beastmen"));
            addVariantsUpgrade(pAsset, "2house_beastmen", List.Of<string>("1hall_beastmen"));
            addVariantsUpgrade(pAsset, "3house_beastmen", List.Of<string>("2hall_beastmen"));
            addVariantsUpgrade(pAsset, "4house_beastmen", List.Of<string>("2hall_beastmen"));
            pAsset.addUpgrade("hall_beastmen", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_beastmen", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_beastmen");
            pAsset.addUpgrade("fishing_docks_beastmen");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_beastmen");
            pAsset.addBuilding("windmill_beastmen", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_beastmen", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_beastmen", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_beastmen");
            pAsset.addBuilding("barracks_beastmen", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_beastmen");
            pAsset.addBuilding("watch_tower_beastmen", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_beastmen", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_beastmen");
            pAsset.addBuilding("temple_beastmen", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_beastmen", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_beastmen");
        }

        private static void initgnome()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "gnome";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_gnome", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_gnome");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_gnome");
            addVariantsUpgrade(pAsset, "house_gnome", List.Of<string>("hall_gnome"));
            addVariantsUpgrade(pAsset, "1house_gnome", List.Of<string>("1hall_gnome"));
            addVariantsUpgrade(pAsset, "2house_gnome", List.Of<string>("1hall_gnome"));
            addVariantsUpgrade(pAsset, "3house_gnome", List.Of<string>("2hall_gnome"));
            addVariantsUpgrade(pAsset, "4house_gnome", List.Of<string>("2hall_gnome"));
            pAsset.addUpgrade("hall_gnome", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_gnome", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_gnome");
            pAsset.addUpgrade("fishing_docks_gnome");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_gnome");
            pAsset.addBuilding("windmill_gnome", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_gnome", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_gnome", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_gnome");
            pAsset.addBuilding("barracks_gnome", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_gnome");
            pAsset.addBuilding("watch_tower_gnome", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_gnome", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_gnome");
            pAsset.addBuilding("temple_gnome", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_gnome", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_gnome");
        }

        private static void initangel()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "angel";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_angel", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_angel");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_angel");
            addVariantsUpgrade(pAsset, "house_angel", List.Of<string>("hall_angel"));
            addVariantsUpgrade(pAsset, "1house_angel", List.Of<string>("1hall_angel"));
            addVariantsUpgrade(pAsset, "2house_angel", List.Of<string>("1hall_angel"));
            addVariantsUpgrade(pAsset, "3house_angel", List.Of<string>("2hall_angel"));
            addVariantsUpgrade(pAsset, "4house_angel", List.Of<string>("2hall_angel"));
            pAsset.addUpgrade("hall_angel", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_angel", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_angel");
            pAsset.addUpgrade("fishing_docks_angel");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_angel");
            pAsset.addBuilding("windmill_angel", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_angel", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_angel", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_angel");
            pAsset.addBuilding("barracks_angel", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_angel");
            pAsset.addBuilding("watch_tower_angel", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_angel", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_angel");
            pAsset.addBuilding("temple_angel", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_angel", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_angel");
        }

        private static void initdemonic()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "demonic";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_demonic", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_demonic");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_demonic");
            addVariantsUpgrade(pAsset, "house_demonic", List.Of<string>("hall_demonic"));
            addVariantsUpgrade(pAsset, "1house_demonic", List.Of<string>("1hall_demonic"));
            addVariantsUpgrade(pAsset, "2house_demonic", List.Of<string>("1hall_demonic"));
            addVariantsUpgrade(pAsset, "3house_demonic", List.Of<string>("2hall_demonic"));
            addVariantsUpgrade(pAsset, "4house_demonic", List.Of<string>("2hall_demonic"));
            pAsset.addUpgrade("hall_demonic", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_demonic", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_demonic");
            pAsset.addUpgrade("fishing_docks_demonic");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_demonic");
            pAsset.addBuilding("windmill_demonic", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_demonic", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_demonic", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_demonic");
            pAsset.addBuilding("barracks_demonic", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_demonic");
            pAsset.addBuilding("watch_tower_demonic", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_demonic", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_demonic");
            pAsset.addBuilding("temple_demonic", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_demonic", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_demonic");
        }

        private static void initjapaneses()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "japaneses";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_japaneses", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_japaneses");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_japaneses");
            addVariantsUpgrade(pAsset, "house_japaneses", List.Of<string>("hall_japaneses"));
            addVariantsUpgrade(pAsset, "1house_japaneses", List.Of<string>("1hall_japaneses"));
            addVariantsUpgrade(pAsset, "2house_japaneses", List.Of<string>("1hall_japaneses"));
            addVariantsUpgrade(pAsset, "3house_japaneses", List.Of<string>("2hall_japaneses"));
            addVariantsUpgrade(pAsset, "4house_japaneses", List.Of<string>("2hall_japaneses"));
            pAsset.addUpgrade("hall_japaneses", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_japaneses", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_japaneses");
            pAsset.addUpgrade("fishing_docks_japaneses");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_japaneses");
            pAsset.addBuilding("windmill_japaneses", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_japaneses", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_japaneses", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_japaneses");
            pAsset.addBuilding("barracks_japaneses", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_japaneses");
            pAsset.addBuilding("watch_tower_japaneses", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_japaneses", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_japaneses");
            pAsset.addBuilding("temple_japaneses", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_japaneses", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_japaneses");
        }

        private static void initancientchina()
        {
            RaceBuildOrderAsset pAsset = new RaceBuildOrderAsset();
            pAsset.id = "ancientchina";
            AssetManager.race_build_orders.add(pAsset);
            pAsset.addBuilding("bonfire", 1);
            pAsset.addBuilding("tent_ancientchina", pHouseLimit: true);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addUpgrade("tent_ancientchina");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("tent_ancientchina");
            addVariantsUpgrade(pAsset, "house_ancientchina", List.Of<string>("hall_ancientchina"));
            addVariantsUpgrade(pAsset, "1house_ancientchina", List.Of<string>("1hall_ancientchina"));
            addVariantsUpgrade(pAsset, "2house_ancientchina", List.Of<string>("1hall_ancientchina"));
            addVariantsUpgrade(pAsset, "3house_ancientchina", List.Of<string>("2hall_ancientchina"));
            addVariantsUpgrade(pAsset, "4house_ancientchina", List.Of<string>("2hall_ancientchina"));
            pAsset.addUpgrade("hall_ancientchina", pPop: 30, pBuildings: 8);
            pAsset.addUpgrade("1hall_ancientchina", pPop: 100, pBuildings: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("statue", "mine", "barracks_ancientchina");
            pAsset.addUpgrade("fishing_docks_ancientchina");
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("fishing_docks_ancientchina");
            pAsset.addBuilding("windmill_ancientchina", 1, pPop: 6, pBuildings: 5);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("fishing_docks_ancientchina", 5, pBuildings: 2);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            pAsset.addBuilding("well", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_types = List.Of<string>("hall");
            pAsset.addBuilding("hall_ancientchina", 1, pPop: 10, pBuildings: 6);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire");
            BuildOrderLibrary.b.requirements_types = List.Of<string>("house");
            pAsset.addBuilding("mine", 1, pPop: 20, pBuildings: 10);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_ancientchina");
            pAsset.addBuilding("barracks_ancientchina", 1, pPop: 50, pBuildings: 16, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_ancientchina");
            pAsset.addBuilding("watch_tower_ancientchina", 1, pPop: 30, pBuildings: 10);
            pAsset.addUpgrade("watch_tower_ancientchina", 0, 0, 3, 3, false, false, 0);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "hall_ancientchina");
            pAsset.addBuilding("temple_ancientchina", 1, pPop: 90, pBuildings: 20, pMinZones: 20);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("bonfire", "1hall_ancientchina", "statue");
            pAsset.addBuilding("statue", 1, pPop: 70, pBuildings: 15);
            BuildOrderLibrary.b.requirements_orders = List.Of<string>("1hall_ancientchina");
        }

        private void loadSprites(BuildingAsset pTemplate)
        {
            string folder = pTemplate.race;
            if (folder == string.Empty)
            {
                folder = "Others";
            }
            folder = folder + "/" + pTemplate.id;
            Sprite[] array = Utils.ResourcesHelper.loadAllSprite("buildings/" + folder, 0.5f, 0.0f);

            pTemplate.sprites = new BuildingSprites();
            foreach (Sprite sprite in array)
            {
                string[] array2 = sprite.name.Split(new char[] { '_' });
                string text = array2[0];
                int num = int.Parse(array2[1]);

                if (array2.Length == 3)
                {
                    int.Parse(array2[2]);
                }
                while (pTemplate.sprites.animationData.Count < num + 1)
                {
                    pTemplate.sprites.animationData.Add(null);
                }
                if (pTemplate.sprites.animationData[num] == null)
                {
                    pTemplate.sprites.animationData[num] = new BuildingAnimationDataNew();
                }
                BuildingAnimationDataNew buildingAnimationDataNew = pTemplate.sprites.animationData[num];
                if (text.Equals("main"))
                {
                    buildingAnimationDataNew.list_main.Add(sprite);
                    if (buildingAnimationDataNew.list_main.Count > 1)
                    {
                        buildingAnimationDataNew.animated = true;
                    }
                }
                else if (text.Equals("ruin"))
                {
                    buildingAnimationDataNew.list_ruins.Add(sprite);
                }
                else if (text.Equals("special"))
                {
                    buildingAnimationDataNew.list_special.Add(sprite);
                }
                else if (text.Equals("mini"))
                {
                    pTemplate.sprites.mapIcon = new BuildingMapIcon(sprite);
                }
            }
            foreach (BuildingAnimationDataNew buildingAnimationDataNew2 in pTemplate.sprites.animationData)
            {
                buildingAnimationDataNew2.main = buildingAnimationDataNew2.list_main.ToArray();
                buildingAnimationDataNew2.ruins = buildingAnimationDataNew2.list_ruins.ToArray();
                buildingAnimationDataNew2.special = buildingAnimationDataNew2.list_special.ToArray();
            }
        }

        private static void addVariantsUpgrade(RaceBuildOrderAsset pAsset, string name, List<string> requirementsBuildings)
        {
            foreach (var race in IsekaiRaceLibrary.defaultRaces)
            {
                pAsset.addUpgrade($"{name}_{race}");
                BuildOrderLibrary.b.requirements_orders = requirementsBuildings;
            }
        }
    }
}