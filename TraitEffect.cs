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
using HarmonyLib;
using Newtonsoft.Json;
namespace Magic{
    public class GodPowers
    {
        public float health = 0f;
        public float damage = 0f;
        public float attack_speed = 0f;
        public float speed = 0f;
        //public int max_age = 0;
        public float armor = 0f;
    }
    public class TraitEffect
    {
        public static Dictionary<string, string> GodName = new Dictionary<string, string>(){
            {"unit_dwarf","Svarog"},
            {"unit_elf","Zhiva"},
            {"unit_human","Rod"},
            {"unit_orc","Perun"},
            {"unit_vampire","Koschey"},
            {"unit_angel","Belobog"},
            {"unit_demonic","Chernobog"},
            {"unit_beastmen","Veles"},
            {"unit_gnome","Semargl"},
            {"unit_darkelve","Morana"},
            {"unit_goblin","Morok"},
            {"unit_lizard","Gorynya"},
            {"unit_android","AA-000"},
            {"unit_ancientchina","Shan di"},
            {"unit_illithiiry","Kitovras"},
            {"unit_japaneses","Amaterasu"}
        };
        public static Dictionary<Actor, GodPowers> GodEnhancement = new Dictionary<Actor, GodPowers>();
#region Эффекты
#region Бездонный
        public static bool BottomlessSources(BaseSimObject pTarget, WorldTile pTile = null)
      	{
            if (pTarget.a.data.hunger<95)
            {
                pTarget.a.data.hunger = 150;
            }
            return true;
        }
#endregion
#region Бог
        public static bool God(BaseSimObject pTarget, WorldTile pTile = null)
      	{
            
            Actor a = pTarget.a;
            #region проверка божественности
            if (Main.godID.ContainsKey(pTarget.a.asset.id))
            {
                if (!FunctionalAction.exsisting(Main.godID[pTarget.a.asset.id]))
                {
                    Main.godID.Remove(pTarget.a.asset.id);
                }
            }
            //if (World.world.units)
            
            if (Main.godID.ContainsKey(pTarget.a.asset.id) && Main.godID[pTarget.a.asset.id] != pTarget.a)
            {
                pTarget.a.removeTrait("God");
                pTarget.a.addTrait("immortal");
                pTarget.a.addTrait("strong");
                //ActionLibrary.removeUnit(pTarget.a);
                return false;
            }
            if (!Main.Сongregation.ContainsKey(a))
            {
                Main.Сongregation.Add(a,0);
            }
            if(!Main.godID.ContainsKey(pTarget.a.asset.id))
            {
                Main.godID.Add(pTarget.a.asset.id, pTarget.a);
                pTarget.a.addTrait("immortal");
                //pTarget.a.addTrait("Hero");
                foreach (KeyValuePair<string, string> name in GodName)
                {
                    if (pTarget.a.asset.id == name.Key){
                        pTarget.a.data.setName(name.Value);
                    }
                }
            }
            #endregion
            #region Вера
            World.world.getObjectsInChunks(pTarget.currentTile, 10, MapObjectType.Actor);
            for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
            {
                Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                if (tempMapObject.isRace(pTarget.a.asset.race) && 
                !tempMapObject.hasTrait("Believer") &&
                !tempMapObject.hasTrait("Lycanthropy") &&
                !tempMapObject.hasTrait("God"))
                {
                    tempMapObject.addTrait("Believer");
                    Main.Faith.Add(tempMapObject, pTarget.a);
                    Main.Сongregation[a] += 1;
                    pTarget.a.addTrait("non_existent_trait");
                    pTarget.a.removeTrait("non_existent_trait");
                }
            }
            foreach (KeyValuePair<Actor, Actor> alive in Main.Faith)
            {
                if (!FunctionalAction.exsisting(alive.Key) && alive.Value == pTarget.a)
                {
                    Main.Faith.Remove(alive.Key);
                    Main.Сongregation[alive.Value] -= 1;
                }
            }
            //pTarget.a.addTrait("non_existent_trait");
            if(!GodEnhancement.ContainsKey(a))
            {
                GodEnhancement.Add(a, new GodPowers());
            }
            #endregion
            #region Божественные заклинания
            if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_dwarf"))
            {
                if (Main.listOfEnergy["Earth energy"]>15)
                {
                    GodMagic.ActivatedSpell("Svarog", "metals", 0, "Earth");
                }
                Main.listOfEnergy["Fire energy"] += Main.Сongregation[pTarget.a]*4/27;
                Main.listOfEnergy["Earth energy"] += Main.Сongregation[pTarget.a]/27;
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_orc" )){
                Main.listOfEnergy["Air energy"] += Main.Сongregation[pTarget.a]*2/9;
                if (Main.listOfEnergy["Air energy"]>15)
                {
                    if (GodMagic.activeSpell("Perun"))
                    {
                        GodMagic.tornadoInBound(
                            (int)(MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions["Perun Left (0-1000)"].value)/1000),
                            (int)(MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions["Perun Right (0-1000)"].value)/1000),
                            (int)(MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions["Perun Lower (0-1000)"].value)/1000),
                            (int)(MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions["Perun Upper (0-1000)"].value)/1000));
                        Main.listOfEnergy["Air energy"] -= 0;
                    }
                    else
                        GodMagic.tornadoInBound(0,0,0,0);
                }
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_elf" ))
            {
                GodMagic.ActivatedSpell("Zhiva","rain", 0, "Water");
                GodMagic.ActivatedSpell("Zhiva","fertilizerPlants", 0, "Water");
                GodMagic.ActivatedSpell("Zhiva","fertilizerTrees", 0, "Water");
                GodMagic.ActivatedSpell("Zhiva","livingPlants", 0 , "Life");
                Main.listOfEnergy["Life energy"] += Main.Сongregation[pTarget.a]/9;
                //Main.listOfEnergy["Earth energy"] += 20;
                Main.listOfEnergy["Water energy"] += Main.Сongregation[pTarget.a]/9;
            }
            else if (Toolbox.randomChance(0.1f) && (pTarget.a.asset.id == "unit_human" ))
            {
                if (//Main.listOfEnergy["Life energy"]>2 && 
                Main.listOfEnergy["Fire energy"]>2 && 
                Main.listOfEnergy["Water energy"]>2 && 
                Main.listOfEnergy["Earth energy"]>2 && 
                Main.listOfEnergy["Air energy"]>2 )
                {
                    if (GodMagic.activeSpell("Rod"))
                        {
                            FunctionalAction.summon(pTarget, "angel", 0);
                            FunctionalAction.summon(pTarget, "whiteMage", 0,0, "Demon Fighter");
                            Main.listOfEnergy["Fire energy"] -= 0;
                            Main.listOfEnergy["Water energy"] -= 0;
                            Main.listOfEnergy["Earth energy"] -= 0;
                            Main.listOfEnergy["Air energy"] -= 0;
                            //Main.listOfEnergy["Life energy"] -= 2;
                        }
                }
                Main.listOfEnergy["Fire energy"] += Main.Сongregation[pTarget.a]*2/9;
                Main.listOfEnergy["Air energy"] += Main.Сongregation[pTarget.a]*2/9;
                Main.listOfEnergy["Earth energy"] += Main.Сongregation[pTarget.a]*2/9;
                Main.listOfEnergy["Water energy"] += Main.Сongregation[pTarget.a]*2/9;
                //Main.listOfEnergy["Life energy"] += 4;
                //GodMagic.ActivatedSpell("Sole","");
                //GodMagic.ActivatedSpell("Sole","");
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_vampire" )){
                pTarget.a.addTrait("Blood Magic");
                pTarget.a.addTrait("freeze_proof");
                
                if (Main.listOfEnergy["Death energy"]>15 && GodMagic.activeSpell("Koschey"))
                {
                    GodMagic.ActivatedSpell("Koschey","plague", 0, "Death");
                    FunctionalAction.summon(pTarget, "zombie_orc", 0, 0, "evil","fast","agile");
                    FunctionalAction.summon(pTarget, "skeleton", 0,0, "evil","eagle_eyed","agile");
                    FunctionalAction.summon(pTarget, "ghost", 0,0, "evil","fast","agile");
                }
                Main.listOfEnergy["Death energy"] += Main.Сongregation[pTarget.a]*2/9;}
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_angel" )){
                Main.listOfEnergy["Life energy"] += Main.Сongregation[pTarget.a]*2/27;
                //Main.listOfEnergy["Earth energy"] += 20;
                Main.listOfEnergy["Water energy"] += Main.Сongregation[pTarget.a]*2/27;
                Main.listOfEnergy["Air energy"] += Main.Сongregation[pTarget.a]*2/27;}
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_demonic" )){
                Main.listOfEnergy["Death energy"] += Main.Сongregation[pTarget.a]*2/27;
                //Main.listOfEnergy["Earth energy"] += 20;
                Main.listOfEnergy["Fire energy"] += Main.Сongregation[pTarget.a]*2/27;
                Main.listOfEnergy["Earth energy"] += Main.Сongregation[pTarget.a]*2/27;}
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_beastmen" )){
                Main.listOfEnergy["Life energy"] += Main.Сongregation[pTarget.a]*2/9;
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_gnome" )){

                if (Main.listOfEnergy["Fire energy"]>15)
                {
                    GodMagic.ActivatedSpell("Svarog", "fire", 0, "Fire");
                }
                Main.listOfEnergy["Fire energy"] += Main.Сongregation[pTarget.a]/27;
                Main.listOfEnergy["Earth energy"] += Main.Сongregation[pTarget.a]*2/27;
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_darkelve" )){
                Main.listOfEnergy["Death energy"] += Main.Сongregation[pTarget.a]/9;
                Main.listOfEnergy["Earth energy"] += Main.Сongregation[pTarget.a]/9;
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_goblin" )){
                
                TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                MapRegion random = randomIslandGround.regions.GetRandom();
                WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;

                Actor goblin = World.world.units.createNewUnit("unit_goblin", worldTile, 0f);
                Main.listOfEnergy["Death energy"] += Main.Сongregation[pTarget.a]*2/9;

            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_lizard" )){
                Main.listOfEnergy["Fire energy"] += Main.Сongregation[pTarget.a]/9;
                Main.listOfEnergy["Air energy"] += Main.Сongregation[pTarget.a]/9;
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_android" )){
                for (int i=0; i<Main.Сongregation[pTarget.a]*2/9; i++)
                {
                    if (Main.listOfEnergy["Fire energy"]>0)
                        Main.listOfEnergy["Fire energy"]-=1;
                    if (Main.listOfEnergy["Air energy"]>0)
                        Main.listOfEnergy["Air energy"]-=1;
                    if (Main.listOfEnergy["Water energy"]>0)
                        Main.listOfEnergy["Water energy"]-=1;
                    if (Main.listOfEnergy["Earth energy"]>0)
                        Main.listOfEnergy["Earth energy"]-=1;
                    if (Main.listOfEnergy["Life energy"]>0)
                        Main.listOfEnergy["Life energy"]-=1;
                    if (Main.listOfEnergy["Death energy"]>0)
                        Main.listOfEnergy["Death energy"]-=1;
                }
            }
            /*else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_ancientchina" ))
            {
            }
            else if (Toolbox.randomChance(0.2f) && (pTarget.a.asset.id == "unit_japaneses" ))
            {
            }*/
            else if (!GodName.ContainsKey(pTarget.a.asset.id) && Toolbox.randomChance(0.2f))
            {
                Main.listOfEnergy["Fire energy"] += Main.Сongregation[pTarget.a]/27;
                Main.listOfEnergy["Air energy"] += Main.Сongregation[pTarget.a]/27;
                Main.listOfEnergy["Earth energy"] += Main.Сongregation[pTarget.a]/27;
                Main.listOfEnergy["Water energy"] += Main.Сongregation[pTarget.a]/27;
                Main.listOfEnergy["Life energy"] += Main.Сongregation[pTarget.a]/27;
                Main.listOfEnergy["Death energy"] += Main.Сongregation[pTarget.a]/27;
            }
            #endregion
            //pTarget.a.addTrait("non_existent_trait");
            return true;
        }
        public static bool GetHitGod(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
            if (!Main.Сongregation.ContainsKey(pSelf.a))
            {
                Main.Сongregation.Add(pSelf.a,0);
            }
            
            if (Main.Сongregation[pSelf.a]>0 )
            {

                if (FunctionalAction.exsisting(pTarget))
                {
                    if (!pTarget.a.hasTrait("God"))
                    {
                        FunctionalAction.spellCost(pSelf,0,-1000);
                    }
                    else{
                        if (!Main.Сongregation.ContainsKey(pTarget.a))
                        {
                            Main.Сongregation.Add(pTarget.a,0);
                        }    
                        God(pTarget,pTile);
                        God(pSelf,pTile);
                        FunctionalAction.spellCost(pSelf,0,-777-Main.Сongregation[pSelf.a]+Main.Сongregation[pTarget.a]);
                    }
                }
                else
                    FunctionalAction.spellCost(pSelf,0,-1000);
                
                
                
                pSelf.a.removeTrait("eyepatch");
                pSelf.a.removeTrait("skin_burns");
                pSelf.a.removeTrait("crippled"); 
                string[] kingdom = new []{pSelf.a.kingdom.id};
                if(FunctionalAction.exsisting(pTarget)){
                    if(!pTarget.a.hasTrait("God")){
                        if (pSelf.a.asset.id == "unit_dwarf"){
                            World.world.applyForce(pTile, 4, 1.0f, pForceOut: false, useOnNature: true, pDamage: 60,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                            pTarget.a.addStatusEffect("burning");
                        }
                        else if (pSelf.a.asset.id == "unit_orc"){
                            
                            World.world.applyForce(pTile, 5, 1.3f,pForceOut: false, useOnNature: true, pDamage: 50,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                            //EffectsLibrary.spawnExplosionWave(pTile.posV3, 3f, 0.5f);
                            //ActionLibrary.whirlwind(pSelf,pTile);
                        }
                        else if (pSelf.a.asset.id == "unit_vampire"){
                            //World.world.applyForce(pTile, 3, 0.3f, useOnNature: true, pByWho: pTarget);
                            World.world.applyForce(pTile, 5, 1.3f,pForceOut: false, useOnNature: true, pDamage: 1,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                            pTarget.a.addStatusEffect("curse");
                        }
                        else if (pSelf.a.asset.id == "unit_human"){
                            World.world.applyForce(pTile, 4, 1.3f, pForceOut: false, useOnNature: true, pDamage: 55,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                            //ActionLibrary.megaHeartbeat(pSelf,pTile);
                        }
                        else if (pSelf.a.asset.id == "unit_elf"){
                            World.world.applyForce(pTile, 7, 1.1f, pForceOut: false, useOnNature: true, pDamage: 40,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                            pTarget.a.addStatusEffect("slowness");
                        }
                        else
                            World.world.applyForce(pTile, 4, 1.1f, pForceOut: false, useOnNature: true, pDamage: 30,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                    }}
                //FunctionalAction.removeInfectTrait(pSelf);
                FunctionalAction.removeBadTrait(pSelf);
            }
            return true;
        }
        public static bool DeathGod(BaseSimObject pTarget, WorldTile pTile = null){
            Main.Сongregation.Remove(pTarget.a);
            Main.godID.Remove(pTarget.a.asset.id);
            
            if (pTarget.a.asset.id == "unit_dwarf" || pTarget.a.asset.id == "unit_gnome"){
                ActionLibrary.deathNuke(pTarget);
                ActionLibrary.deathNuke(pTarget);
                
                for (int i = 0; i<7; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;

                    World.world.earthquakeManager.startQuake(worldTile,EarthquakeType.SmallDisaster);
                    World.world.buildings.addBuilding(SB.volcano, worldTile, true, false, BuildPlacingType.New);
                }
            }
            else if (pTarget.a.asset.id == "unit_orc"|| pTarget.a.asset.id == "unit_angel"){
                for (int i = 0; i<50; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;
                    MapBox.spawnLightningBig(worldTile);
                    World.world.units.createNewUnit("tornado", worldTile, 0f);
                    

                }
            }
            else if (pTarget.a.asset.id == "unit_elf"){
                for (int i = 0; i<90; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;
                    
                    World.world.buildings.addBuilding(SB.super_pumpkin, worldTile, true, false, BuildPlacingType.New);
                }
            }
            else if (pTarget.a.asset.id == "unit_human" || pTarget.a.asset.id == "unit_demonic"){
                for (int i = 0; i<15; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;
                    Actor Demon = World.world.units.createNewUnit("defile_demon", worldTile, 0f);
                    //Angel.addTrait("madness");
                }
            }
            else if (pTarget.a.asset.id == "unit_vampire" || pTarget.a.asset.id == "unit_darkelve"){
                for (int i = 0; i<13; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;

                    Actor Necro = World.world.units.createNewUnit("necromancer", worldTile, 0f);
                    Necro.addTrait("The Magic of Death");
                    Main.deadBodies.Add(Necro, 30);
                    //Actor Necro = necroSummon(pTarget, "necromancer","The Magic of Death");
                    for (int j = 0; j<30; j++)
                    {
                        necroSummon(Necro, "zombie_orc","fast");
                        necroSummon(Necro, "skeleton","eagle_eyed");
                    }
                }
            }
            else if (pTarget.a.asset.id == "unit_beastmen" ){
                for (int i = 0; i<13; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;

                    Actor Beast = World.world.units.createNewUnit("bear", worldTile, 0f);
                    Beast.addTrait("regeneration");
                    Beast.addTrait("immortal");
                    Beast.addTrait("giant");
                    Beast.addTrait("Blood Magic");
                    //Actor Necro = necroSummon(pTarget, "necromancer","The Magic of Death");
                    for (int j = 0; j<30; j++)
                    {
                        FunctionalAction.summon(Beast,"wolf",0,0,"fast","regeneration","immortal");
                        //necroSummon(Necro, "skeleton","eagle_eyed");
                    }
                }
            }
            else if (pTarget.a.asset.id == "unit_goblin" ){
                for (int i = 0; i<20; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;

                    Actor goblin = World.world.units.createNewUnit("unit_goblin", worldTile, 0f);
                    goblin.addTrait("regeneration");
                    goblin.addTrait("immortal");
                    goblin.addTrait("Shaman");
                    goblin.addTrait("BottomlessSource");
                    //Actor Necro = necroSummon(pTarget, "necromancer","The Magic of Death");
                    for (int j = 0; j<30; j++)
                    {
                        Actor goblins = World.world.units.createNewUnit("unit_goblin", worldTile, 0f);
                        goblins.addTrait("regeneration");
                        goblins.addTrait("fast");
                        goblins.addTrait("agile");
                        //goblin.addTrait("BottomlessSource");
                        //FunctionalAction.summon(goblin,"unit_goblin",0,0,"fast","regeneration","agile");
                        //necroSummon(Necro, "skeleton","eagle_eyed");
                    }
                }
            }
            else if (pTarget.a.asset.id == "unit_android")
            {
                for (int i = 0; i<5; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;
                    DropsLibrary.action_atomicBomb(worldTile);
                }
            }
            else if (pTarget.a.asset.id == "unit_lizard"){
                for (int i = 0; i<33; i++)
                {
                    TileIsland randomIslandGround = World.world.islandsCalculator.getRandomIslandGround();
                    MapRegion random = randomIslandGround.regions.GetRandom();
                    WorldTile worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;

                    Actor Beast = World.world.units.createNewUnit("dragon", worldTile, 0f);
                    Beast.addTrait("regeneration");
                    Beast.addTrait("immortal");
                    //Beast.addTrait("Blood Magic");
                    //Actor Necro = necroSummon(pTarget, "necromancer","The Magic of Death");
                }
            }
            else{
                ActionLibrary.deathNuke(pTarget);
                ActionLibrary.deathNuke(pTarget);
            }
            return true;
        }
#endregion
#region Верующий
        public static bool SpreadingTheFaith(BaseSimObject pTarget, WorldTile pTile = null)
      	{
            if (Main.Faith.ContainsKey(pTarget.a))
            {
                if  (Main.Faith[pTarget.a].isAlive())
                {
                    if (FunctionalAction.exsisting(Main.Faith[pTarget.a])){
                        World.world.getObjectsInChunks(pTarget.currentTile, 2, MapObjectType.Actor);
                        for (int index = 0; index < World.world.temp_map_objects.Count; ++index){
                            Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                            if (tempMapObject.isRace(pTarget.a.asset.race) && 
                            !tempMapObject.hasTrait("Believer") && 
                            !tempMapObject.hasTrait("God") && 
                            !tempMapObject.hasTrait("Lycanthropy") && 
                            Toolbox.randomChance(0.1f)){
                                tempMapObject.addTrait("Believer");
                                Main.Faith.Add(tempMapObject, Main.Faith[pTarget.a]);
                                Main.Сongregation[Main.Faith[pTarget.a]] += 1;
                                Main.Faith[pTarget.a].addTrait("non_existent_trait");
                            }
                        }
                    }
                    else{
                        pTarget.a.removeTrait("Believer");
                        Main.Сongregation.Remove(Main.Faith[pTarget.a]);
                        Main.Faith.Remove(pTarget.a);
                    }
                    if (!Main.Saint.ContainsKey(Main.Faith[pTarget.a])){
                        Main.Saint.Add(Main.Faith[pTarget.a], 0);
                    }
                    if (Toolbox.randomChance(0.1f) && 
                    Main.Сongregation[Main.Faith[pTarget.a]]/30 > Main.Saint[Main.Faith[pTarget.a]] &&
                    !pTarget.a.hasTrait("holy_magic")){
                        pTarget.a.addTrait("holy_magic");
                        Main.Saint[Main.Faith[pTarget.a]]+=1;
                    }
                    
                }
                else{
                    pTarget.a.removeTrait("Believer");
                    Main.Сongregation.Remove(Main.Faith[pTarget.a]);
                    Main.Faith.Remove(pTarget.a);
                }
            }
            else{
                pTarget.a.removeTrait("Believer");
            }
            
            /*GodEnhancement[pTarget.a].health += 2;
            GodEnhancement[pTarget.a].damage += 5;
            GodEnhancement[pTarget.a].attack_speed += 0.1;
            GodEnhancement[pTarget.a].speed += 0.05;
            GodEnhancement[pTarget.a].armor += 0.001;*/
            return true;
        }
        public static bool DeathBelive(BaseSimObject pTarget, WorldTile pTile = null)
      	{
            if (Main.Faith.ContainsKey(pTarget.a)){
                if(Main.Faith[pTarget.a].isAlive())
                {
                    Main.Сongregation[Main.Faith[pTarget.a]] -= 1;
                    if (pTarget.a.hasTrait("holy_magic")){
                        Main.Saint[Main.Faith[pTarget.a]] = Mathf.Clamp(Main.Saint[Main.Faith[pTarget.a]]-1, 0, 9999);
                    }
                    Main.Faith[pTarget.a].addTrait("immortalissts");
                    Main.Faith[pTarget.a].removeTrait("immortalissts");
                    Main.Faith.Remove(pTarget.a);
                }
            }
            
            /*GodEnhancement[pTarget.a].health += 2;
            GodEnhancement[pTarget.a].damage += 5;
            GodEnhancement[pTarget.a].attack_speed += 0.1;
            GodEnhancement[pTarget.a].speed += 0.05;
            GodEnhancement[pTarget.a].armor += 0.001;*/
            return true;
        }
#endregion
#region Магия
        public static bool MagicUpgrade(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            Actor a = pTarget.a;
            int hungVal = a.data.hunger + 3;
            hungVal = Mathf.Clamp(hungVal, 1, 100);
            a.data.hunger = hungVal;
            if (a.getAge()>30)
            {
                if(a.isRace(SK.orc))
                {
                    if (Toolbox.randomChance(0.9f) && !a.hasTrait("Air Magic"))
                        a.addTrait("Shaman");
                    else if (!a.hasTrait("Shaman"))
                        a.addTrait("Air Magic");
                }
                if (a.isRace(SK.human))
                {
                    if (Toolbox.randomChance(0.5f) && !a.hasTrait("Water Magic"))
                        {
                        a.addTrait("Fire Magic");
                        a.addTrait("Air Magic");
                        }
                    else if (!a.hasTrait("Fire Magic"))
                       {
                        a.addTrait("Water Magic");
                        a.addTrait("Earth Magic");
                       } 
                }
                if (a.isRace(SK.dwarf))
                {
                    if (Toolbox.randomChance(0.4f) && !a.hasTrait("Earth Magic"))
                        a.addTrait("Fire Magic");
                    else if (!a.hasTrait("Fire Magic"))
                        a.addTrait("Earth Magic");
                }
                if (a.isRace(SK.elf))
                {
                    if (Toolbox.randomChance(0.5f) && !a.hasTrait("Water Magic"))
                        a.addTrait("The Magic of Life");
                    else if (!a.hasTrait("The Magic of Life"))
                        a.addTrait("Water Magic");
                }
                if (a.isRace("goblin"))
                {
                    a.addTrait("MagicOfSpace");
                    if (Toolbox.randomChance(0.6f) && !a.hasTrait("The Magic of Death"))
                        a.addTrait("Shaman");
                    else if (!a.hasTrait("Shaman"))
                        a.addTrait("The Magic of Death");

                }
                if (a.isRace("lizard"))
                {
                    if (Toolbox.randomChance(0.5f) && !a.hasTrait("Fire Magic"))
                        a.addTrait("Air Magic");
                    else if (!a.hasTrait("Air Magic"))
                        a.addTrait("Fire Magic");
                }
                if (a.isRace("beastmen"))
                {
                    if (Toolbox.randomChance(0.5f) && !a.hasTrait("The Magic of Life"))
                        a.addTrait("Shaman");
                    else if (!a.hasTrait("Shaman"))
                        a.addTrait("The Magic of Life");
                }
                if (a.isRace("vampire"))
                {
                    if (Toolbox.randomChance(0.5f) && !a.hasTrait("The Magic of Death"))
                    {
                        a.addTrait("MagicOfSpace");
                        a.addTrait("MindMagic");
                    }
                    else if (!a.hasTrait("MagicOfSpace"))
                        a.addTrait("The Magic of Death");
                }
                if (a.isRace("ancientchina"))
                {
                    a.addTrait("Shaman");
                }
                if (a.isRace("japaneses"))
                {
                    a.addTrait("MagicOfSpace");
                }
                if (a.isRace("demonic"))
                {
                    if (Toolbox.randomChance(0.6f) && !a.hasTrait("Earth Magic"))
                        {a.addTrait("Fire Magic");
                        a.addTrait("MagicOfSpace");}
                    else if (!a.hasTrait("Fire Magic"))
                    {
                        a.addTrait("Earth Magic");
                        a.addTrait("MagicOfSpace");
                    }
                        //a.addTrait("The Magic of Death");
                }
                if (a.isRace("angel"))
                {
                    if (Toolbox.randomChance(0.4f) && !a.hasTrait("Air Magic")){
                        a.addTrait("Water Magic");
                        a.addTrait("MagicOfSpace");
                    }
                        //a.addTrait("The Magic of Life");
                    else if (!a.hasTrait("Water Magic"))
                        {a.addTrait("Air Magic");
                        a.addTrait("The Magic of Life");}
                }
                if (a.isRace("gnome"))
                {
                    if (Toolbox.randomChance(0.6f) && !a.hasTrait("Earth Magic"))
                        a.addTrait("Fire Magic");
                    else if (!a.hasTrait("Fire Magic"))
                        a.addTrait("Earth Magic");
                }
                if (a.isRace("darkelve"))
                {
                    if (Toolbox.randomChance(0.5f) && !a.hasTrait("Earth Magic"))
                        a.addTrait("The Magic of Death");
                    else if (!a.hasTrait("The Magic of Death"))
                        a.addTrait("Earth Magic");
                }
                if (!GodName.ContainsKey(pTarget.a.asset.id))
                {
                    foreach (string magia in Main.Magic)
                    {
                        if (pTarget.a.hasTrait(magia))
                            return false;
                        
                    }
                    System.Random rand = new System.Random(DateTime.Now.ToString().GetHashCode());
                    var index = rand.Next(0, Main.Magic.Count);
                    pTarget.a.addTrait(Main.Magic[index]);
                }
            }
                
         }
      		return true;
        }
        #endregion
#region Огонь
//Огонь
        public static void meteor (BaseSimObject pSelf, BaseSimObject pTarget)
        {
            pSelf.a.addStatusEffect("invincible");
            FunctionalAction.Protect(pSelf, 10);
            EffectsLibrary.spawn("fx_meteorite", pTarget.currentTile, "meteorite_disaster", null, 0f, -1f, -1f);
            //FunctionalAction.spellCost(pSelf, 90);
        }
        public static bool spellOfFire1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isBuilding())
            {
                
                if (Main.listOfEnergy["Fire energy"] > 8)
                {
                    if (Toolbox.randomChance(0.1f))
                    {
                        meteor(pSelf, pTarget);
                        Main.listOfEnergy["Fire energy"] -= 9;
                    }
                }
                else if (pSelf.a.data.hunger > 90)
                {
                    if (Toolbox.randomChance(0.05f))
                        meteor(pSelf, pTarget);
                        FunctionalAction.spellCost(pSelf, 90);
                }
            }
            if (pTarget.isAlive())
            {
                
                if (pTarget.isActor() && Toolbox.randomChance(0.33f))
                {
                    if (Main.listOfEnergy["Fire energy"]>1)
                    {
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "burning", 0);
                        Main.listOfEnergy["Fire energy"]-=1;
                    }
                    else if (pSelf.a.data.hunger > 10 && Toolbox.randomChance(0.67f))
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "burning", 10);
                }
                else if (pSelf.a.data.hunger > 20 && Toolbox.randomChance(0.67f))
                {
                    if (Main.listOfEnergy["Fire energy"]>2)
                    {
                        FunctionalAction.addStatusOnTarget(pSelf, pSelf, "fireEnhancement", 0);
                        Main.listOfEnergy["Fire energy"]-=2;
                    }
                    else if (pSelf.a.data.hunger > 20 && Toolbox.randomChance(0.67f))
                        FunctionalAction.addStatusOnTarget(pSelf, pSelf, "fireEnhancement", 20);
                }
                else if (Toolbox.randomChance(0.9f) && pSelf.a.data.hunger > 50)
                {
                    if (Main.listOfEnergy["Fire energy"]>5)
                    {
                        FunctionalAction.summon(pSelf, "Fire_spirit", 0);
                        Main.listOfEnergy["Fire energy"]-=5;
                    }
                    else if (pSelf.a.data.hunger > 50 && Toolbox.randomChance(0.5f))
                        FunctionalAction.summon(pSelf, "Fire_spirit", 50);
                }
                
            }
            
         }
      	 return true;
        }
        public static bool spellOfFire2(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (Toolbox.randomChance(0.05f) && pTarget.isAlive())
            {
                if (Main.listOfEnergy["Fire energy"]>5)
                    {
                        Main.listOfEnergy["Fire energy"]-=6;
                        MagicSpells.CastDrop(3f,"adamantine",pTarget);
                        for (int index = 0; index < 5; ++index)
                        {
                            MagicSpells.CastDrop(3f,"adamantine",pTarget,pTile.neighboursAll.GetRandom<WorldTile>());
                        }
                    }
                    else if (pTarget.a.data.hunger > 60)
                    {
                        MagicSpells.CastDrop(3f,"adamantine",pTarget);
                        for (int index = 0; index < 5; ++index)
                        {
                            MagicSpells.CastDrop(3f,"adamantine",pTarget,pTile.neighboursAll.GetRandom<WorldTile>());
                        }
                        FunctionalAction.spellCost(pTarget,60);
                    }
            }
            
         }
      	 return true;
        }
        #endregion
#region Вода
//Вода
        public static bool spellOfWater1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive())
            {
                if (pTarget.hasStatus("burning"))
                {
                    if (Main.listOfEnergy["Water energy"]>1)
                    {
                        Main.listOfEnergy["Water energy"]-=1;
                        MagicSpells.CastDrop(3f,"rain",pTarget);
                        for (int index = 0; index < 5; ++index)
                        {
                            MagicSpells.CastDrop(3f,"rain",pTarget,pTile.neighboursAll.GetRandom<WorldTile>());
                        }
                    }
                    else if (pTarget.a.data.hunger > 10)
                    {
                        MagicSpells.CastDrop(3f,"rain",pTarget);
                        for (int index = 0; index < 5; ++index)
                        {
                            MagicSpells.CastDrop(3f,"rain",pTarget,pTile.neighboursAll.GetRandom<WorldTile>());
                        }
                        FunctionalAction.spellCost(pTarget,10);
                    }
                }
                else if (Toolbox.randomChance(0.01f))
                {
                    if (Main.listOfEnergy["Water energy"]>5)
                    {
                        AssetManager.powers.spawnCloudRain(pTile,"cloudRain");
                        Main.listOfEnergy["Water energy"]-=6;
                    }
                    if (pTarget.a.data.hunger > 60 && Toolbox.randomChance(0.5f))
                    {
                        AssetManager.powers.spawnCloudRain(pTile,"cloudRain");
                        FunctionalAction.spellCost(pTarget,60);
                    }
                    
                }
            }
            
         }
      	 return true;
        }

        public static bool spellOfWater2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive())
            {
                if (Toolbox.randomChance(0.21f) )
                {
                    if (Main.listOfEnergy["Water energy"]>5)
                    {
                        FunctionalAction.summon(pSelf, "water_spirit", 0);
                        Main.listOfEnergy["Water energy"]-=5;
                    }
                    else if(Toolbox.randomChance(0.67f))
                    {
                        if (Main.listOfEnergy["Water energy"]>5)
                        {
                            FunctionalAction.summon(pSelf, "water_spirit", 0);
                            Main.listOfEnergy["Water energy"]-=5;
                        }
                        else if (pSelf.a.data.hunger > 50)
                            FunctionalAction.summon(pSelf, "water_spirit", 50);
                    }
                }
                else if (Toolbox.randomChance(0.3f) )
                {
                    if (Main.listOfEnergy["Water energy"]>1)
                        {
                            FunctionalAction.addStatusOnTarget(pSelf, pTarget, "frozen", 0);
                            Main.listOfEnergy["Water energy"]-=1;
                        }
                        else if(Toolbox.randomChance(0.5f) && pSelf.a.data.hunger > 10 )
                            FunctionalAction.addStatusOnTarget(pSelf, pTarget, "frozen", 10);
                }
                else if (Toolbox.randomChance(0.4f) )
                {
                    if (Main.listOfEnergy["Water energy"]>2)
                        {
                            FunctionalAction.addStatusOnTarget(pSelf, pSelf, "waterEnhancement", 0);
                            Main.listOfEnergy["Water energy"]-=2;
                        }
                        else if(Toolbox.randomChance(0.5f) && pSelf.a.data.hunger > 20 )
                            FunctionalAction.addStatusOnTarget(pSelf, pSelf, "waterEnhancement", 20);
                }
            }
            
         }
         return true;
        }
        #endregion
#region Воздух
//Воздух
        public static void tornado(BaseSimObject pSelf, BaseSimObject pTarget, int cost)
        {
            pSelf.a.addStatusEffect("invincible");
            ActionLibrary.castTornado(pSelf,pTarget);
            FunctionalAction.Protect(pSelf,10);
            FunctionalAction.spellCost(pSelf,cost);
        }
        public static bool spellOfAir1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive() && Toolbox.randomChance(0.05f))
            {
                if (Main.listOfEnergy["Air energy"]>5)
                {
                    Main.listOfEnergy["Air energy"]-=6;
                    FunctionalAction.Protect(pTarget, 5, 30, "caffeinated");
                }
                else if (pTarget.a.data.hunger > 60)
                {
                    FunctionalAction.Protect(pTarget, 10, 30, "caffeinated");
                    FunctionalAction.spellCost(pTarget,60);
                }
            }
            
         }
      	 return true;
        }
        public static bool spellOfAir2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive())
            {
                if (Toolbox.randomChance(0.03f) )
                {
                    if (Main.listOfEnergy["Air energy"]>9)
                    {
                        tornado(pSelf,pTarget,0);
                        Main.listOfEnergy["Air energy"]-=9;
                    }
                    else if (Toolbox.randomChance(0.03f) && pSelf.a.data.hunger > 90)
                    {
                        tornado(pSelf,pTarget,90);
                    }
                }

                else if (Toolbox.randomChance(0.25f) && 
                Toolbox.DistTile(pSelf.a.currentTile, pTarget.currentTile) > 3)
                {
                    if (Main.listOfEnergy["Air energy"]>1)
                    {
                        MapBox.spawnLightningSmall(pTarget.currentTile,0.1f);
                        Main.listOfEnergy["Air energy"]-=1;
                    }
                    else if (Toolbox.randomChance(0.8f) && pSelf.a.data.hunger > 10)
                    {
                        MapBox.spawnLightningSmall(pTarget.currentTile,0.1f);
                        FunctionalAction.spellCost(pSelf,10);
                    }
                }
                else if (Toolbox.randomChance(0.33f) )
                {
                    if (Main.listOfEnergy["Air energy"]>2)
                    {
                        FunctionalAction.addStatusOnTarget(pSelf, pSelf, "airEnhancement", 0);
                        Main.listOfEnergy["Air energy"]-=2;
                    }
                    else if (Toolbox.randomChance(0.8f) && pSelf.a.data.hunger > 20)
                        FunctionalAction.addStatusOnTarget(pSelf, pSelf, "airEnhancement", 20);
                }
                else if (Toolbox.randomChance(0.5f))
                {
                    if (Main.listOfEnergy["Air energy"]>5)
                    {
                        FunctionalAction.summon(pSelf, "air_spirit", 0);
                        Main.listOfEnergy["Air energy"]-=5;
                    }
                    else if (Toolbox.randomChance(0.8f) && pSelf.a.data.hunger > 50)
                        FunctionalAction.summon(pSelf, "air_spirit", 50);
                }

                else if (Toolbox.randomChance(1f) && 
                Toolbox.DistTile(pSelf.a.currentTile, pTarget.currentTile) > 5)
                {
                    if (Main.listOfEnergy["Air energy"]>3)
                    {
                        MapBox.spawnLightningMedium(pTarget.currentTile);
                        Main.listOfEnergy["Air energy"]-=3;
                    }
                    else if (Toolbox.randomChance(0.8f) && pSelf.a.data.hunger > 30)
                    {
                        MapBox.spawnLightningMedium(pTarget.currentTile);
                        FunctionalAction.spellCost(pSelf,30);
                    }

                }
                
                
                
            }
            
         }
         return true;
        }
        #endregion
#region Земля
//Земля
        public static void Earthquake(BaseSimObject pSelf, BaseSimObject pTarget, int cost)
      	{
            pSelf.a.addStatusEffect("invincible");
            FunctionalAction.Protect(pSelf,10);
            MagicSpells.CastEarthquake(pSelf,pTarget);
            FunctionalAction.spellCost(pSelf,cost);
        }
        public static bool spellOfEarth1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive() && Toolbox.randomChance(0.05f))
            {
                if (Main.listOfEnergy["Earth energy"]>5)
                {
                    Main.listOfEnergy["Earth energy"]-=6;
                    MagicSpells.CastDrop(3f,"gold",pTarget);
                        for (int index = 0; index < 5; ++index)
                        {
                            MagicSpells.CastDrop(3f,"metals",pTarget,pTile.neighboursAll.GetRandom<WorldTile>());
                        }
                }
                else if (pTarget.a.data.hunger > 60)
                {
                    MagicSpells.CastDrop(3f,"gold",pTarget);
                        for (int index = 0; index < 5; ++index)
                        {
                            MagicSpells.CastDrop(3f,"metals",pTarget,pTile.neighboursAll.GetRandom<WorldTile>());
                        }
                    FunctionalAction.spellCost(pTarget,60);
                }
            }
            
         }
      	 return true;
        }
        public static bool spellOfEarth2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isBuilding()&& Toolbox.randomChance(0.1f))
            {
                if (Main.listOfEnergy["Earth energy"]>9)
                {
                    Earthquake(pSelf, pTarget, 0);
                    Main.listOfEnergy["Earth energy"]-=9;
                }
                if (pSelf.a.data.hunger > 90 && Toolbox.randomChance(0.5f))
                    Earthquake(pSelf, pTarget, 90);
            }
            if (pTarget.isAlive())
            {
                if (Toolbox.randomChance(0.5f))
                {
                    if (Main.listOfEnergy["Earth energy"]>1)
                    {
                        Main.listOfEnergy["Earth energy"]-=1;
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "slowness", 0);
                    }
                    else if (pSelf.a.data.hunger > 10 && Toolbox.randomChance(0.8f))
                    {
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "slowness", 10);
                    } 
                }

                else if (Toolbox.randomChance(0.8f))
                {
                    if (Main.listOfEnergy["Earth energy"]>2)
                    {
                        FunctionalAction.addStatusOnTarget(pSelf, pSelf, "earthEnhancement", 0);
                        Main.listOfEnergy["Earth energy"]-=2;
                    }
                    else if (Toolbox.randomChance(0.75f) && pSelf.a.data.hunger > 20)
                        FunctionalAction.addStatusOnTarget(pSelf, pSelf, "earthEnhancement", 20);
                }
                else if (Toolbox.randomChance(0.75f) )
                {
                    if (Main.listOfEnergy["Earth energy"]>5)
                    {
                        Main.listOfEnergy["Earth energy"]-=5;
                        if(Toolbox.randomChance(0.5f))
                        {
                            FunctionalAction.summon(pSelf, "earth_spirit", 0);
                        }
                        else
                        {
                            FunctionalAction.summon(pSelf, "crystal_golem", 0);
                        }
                    }
                    else if(Toolbox.randomChance(0.67f) && pSelf.a.data.hunger > 50)
                    {
                        if(Toolbox.randomChance(0.5f))
                        {
                            FunctionalAction.summon(pSelf, "earth_spirit", 50);
                        }
                        else
                        {
                            FunctionalAction.summon(pSelf, "crystal_golem", 50);
                        }
                    }
                }
                
            }

         }
         return true;
        }
        #endregion
#region Жизнь
//Жизнь
        public static bool spellOfLife1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (FunctionalAction.exsisting(pTarget) )
         {
            Building Great = null;
            /*if (!Main.countGreatTree.ContainsKey(pTarget.kingdom))
            {
                Main.countGreatTree.Add(pTarget.kingdom, new List<Building>());
            }*/
            
            if (pTarget.a.hasTrait("death_mark"))
            {
                if (Main.listOfEnergy["Life energy"]>1)
                {
                    Main.listOfEnergy["Life energy"]-=1;
                    
                    FunctionalAction.removeBadTrait(pTarget);
                    FunctionalAction.spellCost(pTarget, 0);
                }
                else if (pTarget.a.data.hunger > 10 || pTarget.a.isRace("good"))
                {
                    
                    FunctionalAction.removeBadTrait(pTarget);
                    FunctionalAction.spellCost(pTarget, 10);
                }    
            }
            FunctionalAction.massSummon(pTarget,"chimera_bear","chimera_wolf","chimera_snake","chimera_monkey",20,
            Main.listOfEnergy["Life energy"]>2);
            /*if (Main.listOfEnergy["Life energy"]>3 && Toolbox.randomChance(0.4f))
            {
                Main.listOfEnergy["Life energy"]-=3;
                //FunctionalAction.summon(pTarget, "sheep", 0,0, "immune");
                Beast_summon(pTarget, pTarget, 0);
            }
            else if ((pTarget.a.data.hunger > 30 || pTarget.a.isRace("good")) && Toolbox.randomChance(0.75f))
            {
                //FunctionalAction.summon(pTarget, "sheep", 0,0, "immune");
                //FunctionalAction.spellCost(pTarget, 10);
                Beast_summon(pTarget, pTarget, 30);
            }
            else */if (Main.listOfEnergy["Life energy"]>1 && Toolbox.randomChance(0.5f))
            {
                Main.listOfEnergy["Life energy"]-=1;
                MagicSpells.CastDrop(5f,"fertilizerTrees",pTarget);
                
            }
            else if ((pTarget.a.data.hunger > 10 || pTarget.a.isRace("good")) && Toolbox.randomChance(0.66f))
            {
                MagicSpells.CastDrop(5f,"fertilizerTrees",pTarget);
                FunctionalAction.spellCost(pTarget, 10);
                
            }

            if (Toolbox.randomChance(0.6f))
            {
                if (Main.listOfEnergy["Life energy"]>1)
                {
                    //Main.listOfEnergy["Life energy"]-=1;
                    FunctionalAction.healing(pTarget,10, 0.5f, cost: 0, Life_energy: true);
                }
                else if (Toolbox.randomChance(0.83f) && (pTarget.a.data.hunger > 10 || pTarget.a.isRace("good")))
                    FunctionalAction.healing(pTarget,10, 0.5f);
                //spellCost(pTarget,10);
            }
            FunctionalAction.Regen(pTarget, 100);
            #region строительство
            string build = "";
            if (Toolbox.randomChance(0.25f))
                build = "Great_tree";
            else if (Toolbox.randomChance(0.33333f))
                build = "Great_tree_wolf";
            else if (Toolbox.randomChance(0.5f))
                build = "Great_tree_monkey";
            else 
                build = "Great_tree_snake";
            if (pTarget.kingdom.asset.civ && pTarget.city!=null){
                if (pTarget.city.countBuildingsType("Great_tree_druid")<1 && 
                pTarget.currentTile.zone.city == pTarget.city)
                {
                    Great = FunctionalAction.spawn_building(pTarget.a, pTile, "Great_tree_druid", 3, true,false,pTarget.city);
                }
                else
                    FunctionalAction.massSummon(pTarget,"druid","rhino","buffalo","",10,
                    Main.listOfEnergy["Life energy"]>1);
                if (pTarget.city.countBuildingsType("Great_tree")+
                pTarget.city.countBuildingsType("Great_tree_wolf")+
                pTarget.city.countBuildingsType("Great_tree_monkey")+
                pTarget.city.countBuildingsType("Great_tree_snake")<2 && 
                pTarget.currentTile.zone.city == pTarget.city)
                {
                    Great = FunctionalAction.spawn_building(pTarget.a, pTile, build, (int)(pTarget.a.stats[S.max_age]/40), true,false,pTarget.city);
                }
                else 
                    FunctionalAction.massSummon(pTarget,"bear","wolf","snake","monkey",10,
                    Main.listOfEnergy["Life energy"]>1); 
                    
            }
            else{
                FunctionalAction.massSummon(pTarget,"bear","wolf","snake","monkey",10,
                Main.listOfEnergy["Life energy"]>1);
            }

            /*else if (!pTarget.kingdom.asset.civ)
            {
                Great = FunctionalAction.spawn_building(pTarget.a, pTile, build, (int)(pTarget.a.stats[S.max_age]/40), true,false);
            }
            /*if (!Main.countGreatTree.ContainsKey(pTarget.kingdom))
            {
                Main.countGreatTree.Add(pTarget.kingdom, new List<Building>());
            }
            for (int i=0; i<Main.countGreatTree[pTarget.kingdom].Count; i++)
            {
                if (Main.countGreatTree[pTarget.kingdom][i]==null)
                    Main.countGreatTree[pTarget.kingdom].Remove(Main.countGreatTree[pTarget.kingdom][i]);
                    //FunctionalAction.spawn_building(pTarget.a, pTile, "Great_tree");
                
                else if (Main.countGreatTree[pTarget.kingdom][i].isRuin())
                    Main.countGreatTree[pTarget.kingdom].Remove(Main.countGreatTree[pTarget.kingdom][i]);
            }
            if (Main.countGreatTree[pTarget.kingdom].Count<1+pTarget.kingdom.cities.Count){
                if (!Main.listOfBuilding.ContainsKey(pTarget.a))
                {
                    Great = FunctionalAction.spawn_building(pTarget.a, pTile, build, (int)(pTarget.a.stats[S.max_age]/40), true);
                    Main.countGreatTree[pTarget.kingdom].Add(Great);
                }
                else if (Main.listOfBuilding[pTarget.a]==null )
                {
                    //Main.countGreatTree[pTarget.kingdom].Remove(Main.listOfBuilding[pTarget.a]);
                    Main.listOfBuilding.Remove(pTarget.a);
                    
                    //FunctionalAction.spawn_building(pTarget.a, pTile, "Great_tree");
                }
                else if (Main.listOfBuilding[pTarget.a].isRuin())
                {
                    //Main.countGreatTree[pTarget.kingdom].Remove(Main.listOfBuilding[pTarget.a]);
                    Main.listOfBuilding.Remove(pTarget.a);
                    
                    //FunctionalAction.spawn_building(pTarget.a, pTile, "Great_tree");   
                }
            }
            else 
            {
                
                if (!Main.listOfBuilding.ContainsKey(pTarget.a))
                    return false;
                if (Main.listOfBuilding[pTarget.a]==null )
                {
                    //Main.countGreatTree[pTarget.kingdom].Remove(Main.listOfBuilding[pTarget.a]);
                    Main.listOfBuilding.Remove(pTarget.a);
                    
                    //FunctionalAction.spawn_building(pTarget.a, pTile, "Great_tree");   
                }
                else if (Main.listOfBuilding[pTarget.a].isRuin())
                {
                    //Main.countGreatTree[pTarget.kingdom].Remove(Main.listOfBuilding[pTarget.a]);
                    Main.listOfBuilding.Remove(pTarget.a);
                    
                    //FunctionalAction.spawn_building(pTarget.a, pTile, "Great_tree");   
                }
            }*/
            #endregion
         }
      	 return true;
        }

        public static bool Beast_summon (BaseSimObject pSelf, BaseSimObject pTarget, int cost = 30)
        {
            /*if(Toolbox.randomChance(0.01f) )
                //FunctionalAction.summon(pSelf, "druid", cost);
                FunctionalAction.checkListForPlantsBeast(pSelf, (HashSet<Building>) pSelf.currentTile.zone.trees,
                "druid", cost,  0, "immune", "regeneration");
            /*else if(Toolbox.randomChance(0.05f)){
                //MagicSpells.CastDrop(5f,"fertilizerTrees",pSelf);
                MagicSpells.CastDrop(5f,"livingPlants",pSelf);
                FunctionalAction.spellCost(pSelf,cost);}*/
                //summon(pSelf, "livingPlants", 30);
           /* else if(Toolbox.randomChance(0.25f))
                FunctionalAction.checkListForPlantsBeast(pSelf, (HashSet<Building>) pSelf.currentTile.zone.trees,
                "chimera_wolf", cost,0, "immune","BottomlessSource","fast");
                //FunctionalAction.summon(pSelf, "wolf", cost,0, "immune","regeneration","fast");
            else if(Toolbox.randomChance(0.333f))
                FunctionalAction.checkListForPlantsBeast(pSelf, (HashSet<Building>) pSelf.currentTile.zone.trees,
                "chimera_snake", cost,0, "immune","agile","BottomlessSource");
                //FunctionalAction.summon(pSelf, "snake", cost,0, "immune","regeneration","agile");
            else if(Toolbox.randomChance(0.5f))
                FunctionalAction.checkListForPlantsBeast(pSelf, (HashSet<Building>) pSelf.currentTile.zone.trees,
                "chimera_bear", cost,0, "immune","regeneration","BottomlessSource");
                //FunctionalAction.summon(pSelf, "bear", cost,0, "immune","regeneration");
            else 
                FunctionalAction.checkListForPlantsBeast(pSelf, (HashSet<Building>) pSelf.currentTile.zone.trees,
                "chimera_monkey", cost,0, "immune","regeneration","BottomlessSource");
                //FunctionalAction.summon(pSelf, "monkey", cost,0, "immune","regeneration");*/
            return true;
        }

        public static bool spellOfLife2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive())
            {
                /*if (Toolbox.randomChance(0.21f) )
                {
                    if (Main.listOfEnergy["Life energy"]>3)
                    {
                        Main.listOfEnergy["Life energy"]-=3;
                        Beast_summon(pSelf, pTarget, 0);
                    }
                    else if (Toolbox.randomChance(0.6f) && (pSelf.a.data.hunger > 30 || pSelf.a.isRace("good")))
                    {
                        Beast_summon(pSelf, pTarget);
                    }
                }
                else */if ((pSelf.a.data.hunger > 50 || pSelf.a.isRace("good")) && Toolbox.randomChance(0.5f))
                {
                    if (Main.listOfEnergy["Life energy"]>5)
                    {
                        Main.listOfEnergy["Life energy"]-=5;
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "ash_fever", 0);
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "cough", 0);
                    }
                    else if ((pSelf.a.data.hunger > 50 || pSelf.a.isRace("good")) && Toolbox.randomChance(0.75f))
                    {
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "ash_fever", 25);
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "cough", 25);
                    }
                }
            }
            
         }
         return true;
        }
        #endregion
#region Смерть
//Смерть
        
        public static bool spellOfDeath1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive())
            {
                Actor a = pTarget.a;
                
                if(Main.NewMagicOfDeath)
                {
                    if(!Main.deadBodies.ContainsKey(a))
                    {
                        Main.deadBodies.Add(a,0);
                    }
                    Main.deadBodies[a]+=pTarget.a.data.kills;
                    a.data.kills = 0; 
                }
                
                if (pTarget.a.hasTrait("death_mark"))
                {
                    if (Main.listOfEnergy["Death energy"]>1)
                    {
                        Main.listOfEnergy["Death energy"]-=1;
                        pTarget.a.removeTrait("death_mark");
                    }
                    else if (pTarget.a.data.hunger > 10 || pTarget.a.isRace("necromancer"))
                    {
                        pTarget.a.removeTrait("death_mark");
                        FunctionalAction.spellCost(pTarget, 10);
                    }
                    
                }
                if (Toolbox.randomChance(0.5f) && 
                Main.NewMagicOfDeath)
                {
                    if (Main.listOfEnergy["Death energy"]>3)
                    {
                        Main.listOfEnergy["Death energy"]-=3;
                        NewNecroSummon(pTarget,0);
                    }
                    else if (Toolbox.randomChance(0.6f) && 
                    (pTarget.a.data.hunger > 30 || pTarget.a.isRace("necromancer")))
                        NewNecroSummon(pTarget,30);
                }
            
            FunctionalAction.Regen(pTarget, 50);
            }

            
         }
      	 return true;
        }
        #region Некромантия

        
        public static Actor necroSummon(BaseSimObject pTarget, string entity, string trait = "Subordinate", WorldTile pTile = null)
        {
            if (pTarget!=null)
            {
                if (pTarget.isAlive())
                {
                    Actor a = pTarget.a;
                    Main.deadBodies[a]-=1;
                    Actor act = World.world.units.createNewUnit(entity, a.currentTile, 0f);
                    act.kingdom = pTarget.kingdom;
                    pTarget.kingdom.addUnit(act);
                    act.addTrait("Subordinate");
                    act.addTrait("evil");
                    act.addTrait(trait);
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
                    return act;
                }
            }
            return null;
        }
        public static bool NewNecroSummon(BaseSimObject pTarget, int cost)
        {
            Actor a = pTarget.a;
            if (Main.deadBodies[a]>0)
            {
                if(a.hasTrait("mageslayer"))
                {
                    necroSummon(pTarget, "necromancer");
                    a.removeTrait("mageslayer");
                }
                else if (a.asset.race=="vampire" && Toolbox.randomChance(0.06f))
                {
                    Actor Vamp = necroSummon(pTarget, "unit_vampire", "Magical Gift");
                    Vamp.removeTrait("Subordinate");
                }
                else if(Toolbox.randomChance(0.4f))
                {
                    necroSummon(pTarget, "skeleton","tough");
                }
                else if(Toolbox.randomChance(0.5f))
                {
                    necroSummon(pTarget, "zombie_orc","fast");
                    
                }
                else if(Toolbox.randomChance(0.142857f))
                {
                    necroSummon(pTarget, "skeleton","agile");
                }
                else if(Toolbox.randomChance(0.166f))
                {
                    necroSummon(pTarget, "zombie_orc","strong");
                }
                else if(Toolbox.randomChance(0.2f))
                {
                    necroSummon(pTarget, "zombie_orc","agile");
                }
                else if(Toolbox.randomChance(0.25f))
                {
                    necroSummon(pTarget, "skeleton","eagle_eyed");
                }
                else if(Toolbox.randomChance(0.333f))
                {
                    necroSummon(pTarget, "zombie_orc","venomous");
                }
                else if(Toolbox.randomChance(0.5f))
                {
                    necroSummon(pTarget, "zombie_orc","giant");
                }
                else 
                {
                    necroSummon(pTarget, "ghost","fast");
                }
                FunctionalAction.spellCost(pTarget,cost);
            }
            return true;
        }
        public static bool OldNecroSummon(BaseSimObject pSelf,BaseSimObject pTarget, int cost)
        {
            if(Toolbox.randomChance(0.01f))
            {
                FunctionalAction.summon(pSelf, "necromancer", cost);
            }
            else if(Toolbox.randomChance(0.25f))
            {
                FunctionalAction.summon(pSelf, "zombie", cost);
            }
            else if(Toolbox.randomChance(0.333f))
            {
                FunctionalAction.summon(pSelf, "skeleton", cost);
            }
            else if(Toolbox.randomChance(0.5f))
            {
                FunctionalAction.summon(pSelf, "zombie_orc", cost);
            }
            else 
            {
                FunctionalAction.summon(pSelf, "ghost", cost);
            }
            return true;
        }
#endregion

        public static bool spellOfDeath2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive() && pTarget.isActor())
            {
                if (Toolbox.randomChance(0.5f) && 
                !Main.NewMagicOfDeath)
                {
                    if (Main.listOfEnergy["Death energy"]>3)
                    {
                        Main.listOfEnergy["Death energy"]-=3;
                        OldNecroSummon(pSelf,pTarget,0);
                    }
                    else if (Toolbox.randomChance(0.6f) && 
                    (pTarget.a.data.hunger > 30 || pTarget.a.isRace("necromancer")))
                        OldNecroSummon(pSelf,pTarget,30);
                
                }
                else if (Toolbox.randomChance(0.4f))
                {
                    if (Main.listOfEnergy["Death energy"]>5)
                    {
                        Main.listOfEnergy["Death energy"]-=5;
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "poisoned", 0);
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "curse", 0);
                    }
                    else if ((pSelf.a.data.hunger > 50 || pSelf.a.isRace("necromancer")) && Toolbox.randomChance(0.8f))
                    {
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "poisoned", 25);
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "curse", 25);
                    }
                    
                    //addStatusOnTarget(pSelf, pTarget, "cough", 25);
                }
                else if (Toolbox.randomChance(0.05f))
                {
                    if (Main.listOfEnergy["Death energy"]>9)
                    {
                        Main.listOfEnergy["Death energy"]-=9;
                        pTarget.a.addTrait("death_mark");
                    }
                    else if ((pSelf.a.data.hunger > 90 || pSelf.a.isRace("necromancer")) && Toolbox.randomChance(0.2f))
                    {
                        pTarget.a.addTrait("death_mark");
                        FunctionalAction.spellCost(pSelf,90);
                    }
                    //addStatusOnTarget(pSelf, pTarget, "cough", 25);
                }
            }
            
         }
         return true;
        }
        public static bool spellOfDeath3(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null && 
         !pTarget.a.isRace("necromancer") && 
         !(pTarget.a.asset.id=="ghost") && 
         !pTarget.a.hasTrait("Werewolf")&&
         Toolbox.randomChance(0.3f))
         {
            FunctionalAction.removeInfectTrait(pTarget);
            FunctionalAction.reborn(pTarget,"necromancer",pTile,true);
            pTarget.a.removeTrait("Blood Magic");
            ActionLibrary.mageSlayer(pTarget);
            return true;
         }
      	 return true;
        }
        #endregion
#region Святость
        public static bool holySpell1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (FunctionalAction.exsisting(pTarget) )
         {
            if (pTarget.a.hasTrait("death_mark"))
            {
                    FunctionalAction.removeBadTrait(pTarget);
                    FunctionalAction.spellCost(pTarget, 0);
            }
            
            if (Toolbox.randomChance(0.3f) && (pTarget.a.data.hunger > 10 || !pTarget.a.asset.needFood))
            {
                FunctionalAction.healing(pTarget,50, 0.5f);
                //spellCost(pTarget,10);
            }
         }
      	 return true;
        }
        public static bool holySpell2( BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
            string[] kingdom = new []{pSelf.a.kingdom.id};
            if (!FunctionalAction.exsisting(pTarget))
                return false;
            if (!pSelf.a.asset.canBeKilledByDivineLight && pTarget.a.asset.canBeKilledByDivineLight &&
            !pSelf.a.isRace("darkelve") && 
            !pSelf.a.isRace("goblin")){
                if (Toolbox.randomChance(0.6f)){
                    MagicSpells.CastDivine(pSelf, pTarget, pTile);
                    World.world.applyForce(pTile, 3, 0.5f, useOnNature: true, pDamage: 0,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                    //EffectsLibrary.spawnExplosionWave(pTile.posV3, 3f, 0.5f);
                }}
            else if(Toolbox.randomChance(0.6f) && !pTarget.a.asset.canBeKilledByDivineLight &&
            (pSelf.a.asset.canBeKilledByDivineLight || pSelf.a.isRace("darkelve") || pSelf.a.isRace("goblin"))){//исправить
                World.world.getObjectsInChunks(pTarget.currentTile, 5, MapObjectType.Actor);
                for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
                {
                    Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                    if (!tempMapObject.asset.canBeKilledByDivineLight && tempMapObject.kingdom != pSelf.kingdom)
                    {
                        
                        //World.world.loopWithBrush(AntimatterBombEffect.tile, Brush.get(1), new PowerActionWithID(AntimatterBombEffect.tileAntimatter), "antimatter");
                        tempMapObject.addTrait("madness");
                        tempMapObject.getHit((float) tempMapObject.getMaxHealth() * 0.4f, true, AttackType.Other, (BaseSimObject) null, true, false);
                        FunctionalAction.addStatusOnTarget(pSelf,pTarget,"ash_fever",0);
                        World.world.applyForce(pTile, 3, 0.5f, useOnNature: true, pDamage: 0,pIgnoreKingdoms:kingdom, pByWho: pSelf);
                        EffectsLibrary.spawnExplosionWave(pTile.posV3, 1f, 0.5f);
                        //FunctionalAction.addStatusOnTarget(pSelf,pTarget,"curse",0)
                    }
                }

            }
            return true;
        }
        /*public static bool holyDeath(BaseSimObject pTarget, WorldTile pTile = null)
      	{
            if (Main.Faith.ContainsKey(pTarget.a))
            {
                if (Main.Saint.ContainsKey(Main.Faith[pTarget.a]))
                    Main.Saint[Main.Faith[pTarget.a]]-=Mathf.Clamp(hungVal, 0, 100);
            }
            
            return true;
        }*/
#endregion
#region Кровь
//Кровь
        public static bool spellOfBlood1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null)
         {
            if (pTarget.isAlive())
            {
                pTarget.a.addTrait("non_existent_trait");
                pTarget.a.removeTrait("non_existent_trait");
                if (!MagicTraits.bloodEnhancement.ContainsKey(pTarget.a))
                {
                    MagicTraits.bloodEnhancement.Add(pTarget.a,new bloodStats());
                }
                if (pTarget.a.hasTrait("madness"))
                {
                    bloodMadness(pTarget,0.5f);
                    pTarget.a.addTrait("Rhjdfdsq lj;lm");
                    FunctionalAction.spellCost(pTarget,1,1);
                    //spellCost(pTarget,0,World.world.temp_map_objects.Count*10-10);
                }
                if (pTarget.a.data.health<=10)
                {
                    pTarget.a.removeTrait("strong_minded");
                    pTarget.a.addTrait("madness");
                }
            }
            
            //Regen(pTarget, 10);
            //pTarget.a.removeTrait("Rhjdfdsq lj;lm");
         }
      	 return true;
        }

        public static bool spellOfBlood2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
         if (pTarget != null && pTarget.isActor() && pTarget.isAlive())
         {
            if (!MagicTraits.bloodEnhancement.ContainsKey(pSelf.a))
                {
                    MagicTraits.bloodEnhancement.Add(pSelf.a,new bloodStats());
                }
            if ((pSelf.a.data.health > 70) && Toolbox.randomChance(0.33f) && pTarget != null)
            {
                MagicTraits.bloodEnhancement[pSelf.a].health+=10f;
                MagicTraits.bloodEnhancement[pSelf.a].max_age+=1;
                MagicTraits.bloodEnhancement[pSelf.a].speed+=0.5f;
                MagicTraits.bloodEnhancement[pSelf.a].attack_speed+=0.1f;
                
                pSelf.a.addTrait("Rhjdfdsq lj;lm");
                //pSelf.a.stats[S.health]+=10f;
                //pSelf.a.stats[S.max_age]+=1f;
                //pSelf.a.stats[S.damage]+=0.1f;
                //pSelf.a.restoreHealth(10);
                FunctionalAction.spellCost(pTarget,0,10);
                FunctionalAction.spellCost(pSelf,0,60);
                //addStatusOnTarget(pSelf, pTarget, "cough", 25);
            }
            else if ((pSelf.a.data.health > 95) && Toolbox.randomChance(0.25f) && pTarget != null)
            {
                FunctionalAction.spellCost(pTarget,0,100);
                MagicTraits.bloodEnhancement[pSelf.a].armor+=1f;
                pSelf.a.addTrait("Rhjdfdsq lj;lm");
                
                //pSelf.a.stats[S.armor]+=1f;
                FunctionalAction.spellCost(pSelf,0,90);
                //addStatusOnTarget(pSelf, pTarget, "cough", 25);
            }
            else if (Toolbox.randomChance(0.2f) && (pSelf.a.data.health > 140) && pTarget != null)
            {
                FunctionalAction.spellCost(pTarget,0,10);
                MagicTraits.bloodEnhancement[pSelf.a].damage+=1f;
                //pSelf.a.stats[S.speed]+=0.1f;
                //pSelf.a.stats[S.attack_speed]+=0.01f;
                pSelf.a.addTrait("Rhjdfdsq lj;lm");
                FunctionalAction.spellCost(pSelf,0,130);
            }
            
            //pSelf.a.removeTrait("Rhjdfdsq lj;lm");
            bloodRestore(pSelf,pTarget);
         }
         return true;
        }
        public static bool spellOfBlood3(BaseSimObject pTarget, WorldTile pTile = null)
      	{
            foreach (string race in Main.Races)
            {
                if (pTarget != null && 
                !pTarget.a.isRace("vampire") && 
                //!(pTarget.a.asset.id=="ghost") && 
                //!pTarget.a.isRace("necromancer") && 
                pTarget.a.isRace(race)&& 
                !pTarget.a.hasTrait("Werewolf") &&
                Toolbox.randomChance(0.2f))
                {
                    pTarget.a.removeTrait("Werewolf");
                    pTarget.a.removeTrait("Lycanthropy");
                    pTarget.a.removeTrait("Phoenix");
                    FunctionalAction.removeInfectTrait(pTarget);
                    FunctionalAction.reborn(pTarget,"unit_vampire",pTile);
                    ActionLibrary.mageSlayer(pTarget);
                }
            }
      	 return true;
        }
        public static bool bloodMadness(BaseSimObject pTarget,float pChance = 0.2f, int pRad = 10, int pHealth = 10, WorldTile pTile = null)
        {
            if (Toolbox.randomChance(pChance))
            {
                World.world.getObjectsInChunks(pTarget.currentTile, pRad, MapObjectType.Actor);
                for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
                {
                    Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                    if (tempMapObject != pTarget.a && 
                    !tempMapObject.isRace("vampire"))
                    {
                        if(tempMapObject.data.health<pHealth)
                        {
                            tempMapObject.killHimself();
                            tempMapObject.getHit(877,false, AttackType.Eaten, pTarget, false, false);
                            //pTarget.a.data.kills++;
                            //pTarget.a.data.experience += 10;
                            //tempMapObject.data.health -= tempMapObject.data.health;
                            continue;
                        }
                        tempMapObject.data.health -= pHealth;
                        tempMapObject.getHit(1,false, AttackType.Eaten, pTarget, false, false);
                        tempMapObject.spawnParticle(Toolbox.color_red);
                        //pTarget.a.stats[S.health]+=pHealth/3;
                        MagicTraits.bloodEnhancement[pTarget.a].health+=pHealth/3;
                        pTarget.a.restoreHealth(pHealth);
                        pTarget.a.data.diplomacy -= 1;
                        pTarget.a.data.intelligence -= 1;
                        //pTarget.a.restoreHealth(pHealth);
                    }
                    
                }
            }
            return true;
        }
        #endregion
/*#region Иллюзии
        public static bool spellOfIllusion1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
      	{
            if (FunctionalAction.exsisting(pTarget))
            {
                #region иллюзия
                FunctionalAction.illusion(pSelf,"necromancer",1,"powerup",0);
                #endregion
                
            }
            return true;
        }
#endregion*/
#region Разум
        public static bool spellOfMind1(BaseSimObject pTarget, WorldTile pTile = null)
      	{
            if (FunctionalAction.exsisting(pTarget))
            {
                #region подчинение свободных рабов
                #endregion
                
            }
            return true;
        }
        public static bool spellOfMind2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            float intellect= pSelf.a.stats[S.intelligence];
            if (FunctionalAction.exsisting(pTarget))
            {
                if (pTarget == null)
                    return false;
                if (!pTarget.isAlive() || !pTarget.isActor())
                    return false;
                if (pTarget.a.hasTrait("Magical Gift") && Toolbox.randomChance(0.8f))
                    return false;
                if (!pTarget.a.hasTrait("strong_minded") && !pTarget.a.hasTrait("boat")){
                    #region безумие
                    if (Toolbox.randomChance(0.05f) && pSelf.a.data.hunger > 50-intellect/2 && !pTarget.a.hasTrait("Subordinate"))
                    {
                        pTarget.a.addTrait("madness");
                        FunctionalAction.spellCost(pSelf,(int)(50-intellect/2));
                    }
                    #endregion
                    #region забвение
                    else if (Toolbox.randomChance(0.2f) && 
                    pSelf.a.data.hunger > (int)(20-intellect/2) &&
                    !pTarget.a.hasStatus("fear") &&
                    !pTarget.a.hasStatus("Disorientation"))
                    {
                        pTarget.a.has_attack_target = false;
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "oblivion", (int)Mathf.Clamp(20-intellect/2, 1, 20),0,5+intellect/5);
                        if  (pTarget.a.stats[S.intelligence]>0){
                            pTarget.a.data.intelligence -= 1;
                            pTarget.a.addTrait("non_existent_trait");
                            pTarget.a.removeTrait("non_existent_trait");
                            pSelf.a.data.intelligence += 1;
                            pSelf.a.addTrait("non_existent_trait");
                            pSelf.a.removeTrait("non_existent_trait");}
                    }
                    #endregion
                    #region паника
                    else if (Toolbox.randomChance(0.25f) && 
                    pSelf.a.data.hunger > 30-intellect/2 &&
                    !pTarget.a.hasStatus("oblivion") &&
                    !pTarget.a.hasStatus("Disorientation"))
                    {
                        pTarget.a.has_attack_target = false;
                        //pTarget.a.ai.task = pTarget.a.ai.task_library.get("ant_black_sand");
                        //pTarget.a.ai.setTask("panic_move", pCleanJob: true);
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "fear",(int)Mathf.Clamp(30-intellect/2, 1, 30) ,0,7+intellect);
                        //FunctionalAction.goToTarget(pTarget,pTarget.a.kingdom.id);
                    }
                    #endregion
                    #region дезориентация
                    else if (Toolbox.randomChance(0.33f) && 
                    pSelf.a.data.hunger > 30-intellect/2 &&
                    !pTarget.a.hasStatus("fear") &&
                    !pTarget.a.hasStatus("oblivion"))
                    {
                        pTarget.a.has_attack_target = false;
                        FunctionalAction.addStatusOnTarget(pSelf, pTarget, "Disorientation", (int)Mathf.Clamp(30-intellect/2, 1, 30),0,7+intellect);
                    }
                    #endregion
                    
                    #region рабство
                    #endregion
                    }
            }
            return true;
        }
#endregion
#region Шаманизм
        public static bool summonSpirit(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            float diplomacy= pSelf.a.stats[S.diplomacy];
            if (Toolbox.randomChance(0.6f) && pTarget != null && pSelf.a.data.hunger>20-diplomacy/2)
            {
                
                if(Toolbox.randomChance(0.25f))
                {
                    FunctionalAction.summon(pSelf, "water_spirit", (int)(20-diplomacy/2));
                }
                else if (Toolbox.randomChance(0.33f))
                {
                    FunctionalAction.summon(pSelf, "Fire_spirit", (int)(20-diplomacy/2));
                }
                else if (Toolbox.randomChance(0.5f))
                {
                    FunctionalAction.summon(pSelf, "earth_spirit", (int)(20-diplomacy/2));
                }
                else
                {
                    FunctionalAction.summon(pSelf, "air_spirit", (int)(20-diplomacy/2));
                }
        }
            

            return true;
        }
        
        public static bool SubjugationSpirit(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget!=null && pTarget.isActor() ){
                Actor b = pTarget.a;
                if (b.hasTrait("Spirit") && Toolbox.randomChance(0.7f) && pSelf.a.data.hunger>10)
                {
                    pSelf.a.data.hunger -= 10;
                    FunctionalAction.obey(pSelf,pTarget);
                }
                if (Toolbox.randomChance(0.3f))
                    summonSpirit(pSelf,  pTarget, pTile);
            }
            return true;
        }
        #endregion
#region Пространство
        public static bool SpellOfSpace (BaseSimObject pTarget, WorldTile pTile = null)
        {
            
            if (pTarget != null)
            {
                if (pTarget.isAlive())
                {
                    Actor pActor = pTarget.a;
                    if (pTarget.a.has_attack_target && 
                    pTarget.a.attackTarget.isActor() && 
                    pTarget.a.attackTarget.isAlive() &&
                    pTarget.a.attackTarget!=null)
                    {
                        Actor enemy = pTarget.a.attackTarget.a;
                        if (pTarget.a.s_attackType == WeaponType.Melee && 
                        Toolbox.DistTile(enemy.currentTile, pActor.currentTile) > pActor.stats[S.range]+1 &&
                        pTarget.a.data.hunger>10)
                        {
                            BaseSimObject pAttackTarget = pTarget.a.attackTarget;
                            FunctionalAction.teleportToTarget(pTarget, enemy.currentTile);
                            //setAttackTarget(pTarget, pAttackTarget);
                            FunctionalAction.spellCost(pTarget,10);
                        }
                        if (pTarget.a.s_attackType == WeaponType.Range && 
                        (Toolbox.DistTile(enemy.currentTile, pActor.currentTile)<=5 || 
                        Toolbox.DistTile(enemy.currentTile, pActor.currentTile) > pActor.stats[S.range]) &&
                        pTarget.a.data.hunger>10)
                        {
                            
                            BaseSimObject pAttackTarget = pTarget.a.attackTarget;
                            FunctionalAction.teleportToTarget(pTarget, enemy.currentTile.region.tiles.GetRandom<WorldTile>());
                            //setAttackTarget(pTarget, pAttackTarget);
                            FunctionalAction.spellCost(pTarget,10);
                        }
                    }
                    if (Toolbox.randomChance(0.3f) && pTarget.a.data.health<60 && pTarget.a.data.hunger>10)
                    {
                        WorldTile SafeTile = pTarget.currentTile.region.tiles.GetRandom<WorldTile>();
                        if (SafeTile.isSameIsland(pActor.currentTile))
                        {
                            if (FunctionalAction.teleportToTarget(pTarget, SafeTile))
                            {
                                pTarget.a.restoreHealth(30);
                                FunctionalAction.spellCost(pTarget,10);
                            }
                        }

                    }
                }
            }
            return true;
        }
        public static bool SpellOfSpace1 (BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
        
            if (pTarget != null)
            {
                if (pTarget.isAlive())
                {
                    if(Toolbox.randomChance(0.05f) && pSelf.a.data.hunger>90)
                    {
                        FunctionalAction.Protect(pSelf,2,20);
                        FunctionalAction.spellCost(pSelf, 90);
                    }
                    else if (pSelf.a.data.hunger > pSelf.kingdom.units.Count*10)
                    {
                        FunctionalAction.Evacuation(pSelf);
                    }
                }
                
            }
            
            return true;
        }
        #endregion
#region Вампиры
//Вампирские эффекты
        public static bool bloodRestore(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor())
            {
                if ((double) Toolbox.DistTile(pSelf.a.currentTile, pTarget.a.currentTile) < 2.0)
                {
                    int hungVal = pSelf.a.data.hunger + 5;
                    hungVal = Mathf.Clamp(hungVal, 1, 100);
                    pSelf.a.data.hunger = hungVal;
                    pSelf.a.restoreHealth(30);
                }
                
            }
            return true;
        }

        public static bool VampireAtackEffect1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null) 
      	{
         if (pTarget != null && pTarget.isActor() && 
         !pTarget.a.hasTrait("Vampirism") &&
         !pTarget.a.hasTrait("The Magic of Life") &&
         !pTarget.a.hasTrait("The Magic of Death") &&
         !pTarget.a.hasTrait("holy_magic"))
         {
            Actor b = pTarget.a;
            foreach (string race in Main.Races)
            {
                if (b.isRace(race))
                {
                    if (Toolbox.randomChance(0.2f) && 
                    !Main.listOfKingdoms.ContainsKey(b) &&
                    (double) Toolbox.DistTile(pSelf.a.currentTile, pTarget.a.currentTile) < 2.0)
                    { 
                        Main.listOfKingdoms.Add(b, pSelf.a.kingdom);
                        b.addTrait("Vampirism");
                        bloodRestore(pSelf,pTarget);
                        return true;
                    }
                    continue;
                }
                
            }
            bloodRestore(pSelf,pTarget);
         }
         return false;

        }

        public static bool VampireAtackEffect2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null) 
      	{
         if (pTarget != null && pTarget.isActor() && 
         !pTarget.a.hasTrait("Vampirism") &&
         !pTarget.a.hasTrait("The Magic of Life") &&
         !pTarget.a.hasTrait("The Magic of Death") &&
         !pTarget.a.hasTrait("holy_magic"))
         {
            Actor b = pTarget.a;//Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            foreach (string race in Main.Races)
            {
                if (b.isRace(race))
                {
                    if (Toolbox.randomChance(0.9f) && 
                    !Main.listOfKingdoms.ContainsKey(b)&&
                    (double) Toolbox.DistTile(pSelf.a.currentTile, pTarget.a.currentTile) < 2.0)
                    { 
                        Main.listOfKingdoms.Add(b, pSelf.a.kingdom);
                        
                        b.addTrait("Vampirism");
                        bloodRestore(pSelf,pTarget);
                        return true;
                    }
                    
                }

            }
            bloodRestore(pSelf,pTarget);
         }
         return false;

        }

        public static bool VampireDeathEffect1(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
            if (pTarget != null)
            {
                FunctionalAction.removeInfectTrait(pTarget);
                FunctionalAction.reborn(pTarget,"unit_vampire", pTile);
                
                
            }
            return true;
        }
        
        
        public static bool VampireDeathEffect2(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
         if (pTarget != null)
         {
            Actor a = pTarget.a;
            if (a.getAge()>Main.BloodAge){
                a.addTrait("Blood Magic");
            }
            if (a.getAge()>1000){
                FunctionalAction.removeInfectTrait(pTarget);
                a.addTrait("Elder Vampire");
                //a.addTrait("SSS");
                a.removeTrait("cursed");
                a.data.health += 10000;
            }
            /*if (a.kingdom.cities.count==0)
            {
                KingdomManager.destroyKingdom(a.kingdom);
            }*/

         }
         return true;
        }
        #endregion
#region Оборотни
//Оборотни
        public static bool LicanAtackEffect1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null) 
      	{
         if (pTarget != null && 
         pTarget.isActor() && 
         !pTarget.a.hasTrait("Lycanthropy") &&
         !pTarget.a.hasTrait("The Magic of Life") &&
         !pTarget.a.hasTrait("The Magic of Death"))
         {
            Actor b = Reflection.GetField(pTarget.GetType(), pTarget, "a") as Actor;
            foreach (string race in Main.Races)
            {
                if (b.isRace(race))
                {
                    if (Toolbox.randomChance(0.05f) && !Main.listOfKingdoms.ContainsKey(b))
                    { 
                        //b.kingdom = pSelf.kingdom;
                        Main.listOfKingdoms.Add(b, pSelf.a.kingdom);
                        b.addTrait("Lycanthropy");
                        return true;
                    }
                    continue;
                }
            }
         }
         return false;

        }
        public static bool LicanDeathEffect1(BaseSimObject pTarget, WorldTile pTile = null)
        {
        return true;
        }
        public static bool LicanAtackEffect2(BaseSimObject pTarget, WorldTile pTile = null)
        
        {
            Actor a = pTarget.a;
            //a.addTrait("Werewolf");
            FunctionalAction.removeInfectTrait(pTarget);
            FunctionalAction.reborn(pTarget,"unit_beastmen",pTile);
            return true;

        }
        public static bool LicanRegen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            FunctionalAction.Regen(pTarget,100);
            return true;
        }
        #endregion
#region Демоноборцы
//Ангелы
        public static bool pacification(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null)
            {
                if (pTarget.isActor())
                {
                    Actor a = pSelf.a;
                    //a.addTrait("strong_minded");
                    Actor b = pTarget.a;
                    if (b.isRace("demon") || b.isRace("demonit"))
                    {
                        FunctionalAction.spellCost(pTarget,0,650);
                    }
                    if (b.isRace("demonic") || b.hasTrait("Defiler"))
                    {
                        FunctionalAction.spellCost(pTarget,0,77);
                    }
                    if ((a.hasTrait("God") || a.hasTrait("Hero")) && (b.asset.id == "demonKing") && b.data.health == 1)
                    {
                        //spellCost(pTarget,0,1250);
                        b.killHimself();
                        a.data.kills++;
                        a.addTrait("kingslayer");
                        a.addTrait("mageslayer");
                    }
                        return true;
                    
                }
            }
            return false;
            //}
        }
        public static bool pacification1 (BaseSimObject pTarget, WorldTile pTile = null)
        {
            FunctionalAction.Regen(pTarget,100);
            Actor pActor = pTarget.a;
            if (pActor.hasTrait("Hero")||pActor.hasTrait("God"))
            {
                if (BehaviourActionBase<Actor>.world.worldLaws.world_law_peaceful_monsters.boolVal)
                    return false;
                Actor King = (Actor) null;
                float num1 = 0.0f;
                foreach (Actor Kings in Main.DemonKing)
                {
                if (Kings.currentTile.isSameIsland(pActor.currentTile))
                    {
                        float num2 = Toolbox.DistTile(Kings.currentTile, pActor.currentTile);
                        if ( King == null || (double) num2 < (double) num1)
                        {
                            King = Kings;
                            num1 = num2;
                        }
                    }
                }
                if ( King !=  null)
                {
                    pActor.goTo(King.currentTile);
                    return true;
                }
            }  
            if (Toolbox.randomChance(0.3f))
            {
                FunctionalAction.goToTarget(pTarget,"lighthouses",pTile);
            }
            
            return true;
        }
        #endregion
#region Осквернители
//Демоны
        public static bool desecration(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null) 
      	{
            
         if (pTarget != null)
         {
            if (pTarget.isActor())
            {
                Actor a = pTarget.a;
                    if (
                    (MagicTraits.good_defiler.ContainsKey(pSelf.a.asset.id)) )
                    { 
                        if (Toolbox.randomChance(MagicTraits.good_defiler[pSelf.a.asset.id]))
                        {
                            desecration1(pSelf,pTarget);
                        }
                    }
                    else if (Toolbox.randomChance(0.07f) && 
                    !a.hasTrait("Vampire") &&
                    !a.isRace("undead")&&
                    !a.isRace("chimera"))
                    {
                        desecration1(pSelf,pTarget);
                    }
                
                if (Toolbox.randomChance(0.09f))
                {
                    foreach (string race in Main.Races)
                    {
                        if ((a.isRace(race)))
                        {
                            a.addTrait("Demon Fighter");
                        }
                    }
                }
                
                return true;
            }
         }
         return false;

        }
        
        public static bool desecration1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(pTarget!=null && pTarget.isActor())
            {
                if (!(pTarget.a.hasTrait("Demon Fighter")) && 
                !(pTarget.a.hasTrait("Defiler")) &&
                !(pTarget.a.hasTrait("Hero")) && 
                !(pTarget.a.hasTrait("Lycanthropy")) && 
                !(pTarget.a.hasTrait("Spirit")) && 
                !(pTarget.a.hasTrait("holy_magic")) && 
                !(pTarget.a.hasTrait("God")) && 
                !pTarget.a.isRace(SK.crabzilla) && 
                !pTarget.a.hasTrait("boat"))
                {
                    Actor a = pTarget.a;
                    DeathBelive(pTarget, pTile);
                    if (((Component) a).gameObject == null ||  a == null || !a.inMapBorder())
                        return false;
                    a.removeTrait("cursed");
                    a.removeTrait("death_mark");
                    a.removeTrait("peaceful");
                    FunctionalAction.removeInfectTrait(pTarget);
                    Actor newUnit = (Actor) null;
                    if (a.asset.id == "dragon" || a.asset.id == "zombie_dragon")
                    {
                        newUnit = World.world.units.createNewUnit("fel_dragon", a.currentTile);
                    }
                    else if (a.asset.id == "wolf" || a.asset.id == "hyena" || a.asset.id == "dog" || a.asset.id == "chimera_wolf")
                    {
                        newUnit = World.world.units.createNewUnit("hellhound", a.currentTile);
                    }
                    else
                    {
                        newUnit = World.world.units.createNewUnit("lowest_defile_demon", a.currentTile);
                    }
                    ActorTool.copyUnitToOtherUnit(a, newUnit);
                    newUnit.kingdom = pSelf.kingdom;
                    Main.listOfTamedBeasts.Add(newUnit, pSelf.a);
                    if (Main.listOfOwners.ContainsKey(pSelf.a))
                    {
                        Main.listOfOwners[pSelf.a] +=1;
                    }
                    else {
                        Main.listOfOwners.Add(pSelf.a, 0);
                        Main.listOfOwners[pSelf.a] +=1;
                    }
                    pSelf.kingdom.addUnit(newUnit);
                    newUnit.addTrait("Subordinate");
                    EffectsLibrary.spawn("fx_spawn", newUnit.currentTile);
                    if (Main.listOfTamedBeasts.ContainsKey(pTarget.a))
                    {
                        if(Main.listOfOwners.ContainsKey(Main.listOfTamedBeasts[pTarget.a]))
                            Main.listOfOwners[Main.listOfTamedBeasts[pTarget.a]] -= 1;
                        Main.listOfTamedBeasts.Remove(pTarget.a);
                        
                    }
                    ActionLibrary.removeUnit(pTarget.a);
                    FunctionalAction.spellCost(pSelf,-100);
                    pSelf.a.restoreHealth(1000);
                }
            }
                
                
            return true;
        }
        public static bool desecration2 (BaseSimObject pTarget, WorldTile pTile = null)
        {
            #region реакция на святость
            if (GodMagic.insquad(pTarget.currentTile.x,pTarget.currentTile.y))
            {
                pTarget.a.getHit((int)(pTarget.a.getMaxHealth()/10),false, AttackType.Other, pTarget, false, false);
            }
            #endregion
            #region строительство
            if (pTarget.a.asset.id == "demonKing")
            {
                if(!Main.DemonKing.Contains(pTarget.a))
                {
                    Main.DemonKing.Add(pTarget.a);
                }
                if (!Main.listOfBuilding.ContainsKey(pTarget.a))
                {
                    FunctionalAction.spawn_building(pTarget.a, pTile, "DefilerGate");                  
                }
                else if (Main.listOfBuilding[pTarget.a]==null )
                {
                    Main.listOfBuilding.Remove(pTarget.a);
                    FunctionalAction.spawn_building(pTarget.a, pTile, "DefilerGate");   
                }
                else if (Main.listOfBuilding[pTarget.a].isRuin())
                {
                    Main.listOfBuilding.Remove(pTarget.a);
                    FunctionalAction.spawn_building(pTarget.a, pTile, "DefilerGate");   
                }
            }
            string building = "";
            if (Toolbox.randomChance(0.05f))
            {
                building = "Flame_Tower";
            }
            else if (Toolbox.randomChance(0.33f))
            {
                building = "HellKennel";
            }
            else if (Toolbox.randomChance(0.5f))
            {
                building = "Flame_tower";
            }
            else
            {
                building = "barracks_demons";
            }

            
            if (Toolbox.randomChance(0.1f) && pTarget.a.asset.id == "defile_demon")
            {
                if (!Main.listOfBuilding.ContainsKey(pTarget.a))
                {
                    FunctionalAction.spawn_building(pTarget.a, pTile, building);                  
                }
                else if (Main.listOfBuilding[pTarget.a]==null )
                {
                    Main.listOfBuilding.Remove(pTarget.a);
                    FunctionalAction.spawn_building(pTarget.a, pTile, building);   
                }
                else if (Main.listOfBuilding[pTarget.a].isRuin())
                {
                    Main.listOfBuilding.Remove(pTarget.a);
                    FunctionalAction.spawn_building(pTarget.a, pTile, building);   
                }
            }
            
            #endregion
            #region подчинение
            World.world.getObjectsInChunks(pTarget.currentTile, 5, MapObjectType.Actor);
            for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
            {
                Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                if (tempMapObject != pTarget.a &&
                tempMapObject.isRace("demon") &&
                !tempMapObject.hasTrait("Subordinate") )
                {
                    Actor b = tempMapObject;
                    if (pTarget.a.asset.id == "demonKing" && b.asset.id != "demonKing")
                    {
                        if (b!= null && b.isActor())
                        {
                            if (!Main.listOfTamedBeasts.ContainsKey(b))
                            {
                                b.kingdom.removeUnit(b);
                                b.kingdom = pTarget.kingdom;
                                pTarget.kingdom.addUnit(b);
                                Main.listOfTamedBeasts.Add(b, pTarget.a);
                                if (Main.listOfOwners.ContainsKey(pTarget.a))
                                {
                                    Main.listOfOwners[pTarget.a] +=1;
                                }
                                else {
                                    Main.listOfOwners.Add(pTarget.a, 0);
                                    Main.listOfOwners[pTarget.a] +=1;
                                }
                                b.addTrait("Subordinate");
                            }
                        }
                    }
                    else if(b.asset.id != "defile_demon" && b.asset.id != "demonKing" && pTarget.a.asset.id == "defile_demon")
                    {
                        if (b!= null && b.isActor())
                        {
                            if (!Main.listOfTamedBeasts.ContainsKey(b))
                            {
                                b.kingdom.removeUnit(b);
                                b.kingdom = pTarget.kingdom;
                                pTarget.kingdom.addUnit(b);
                                Main.listOfTamedBeasts.Add(b, pTarget.a);
                                if (Main.listOfOwners.ContainsKey(pTarget.a))
                                {
                                    Main.listOfOwners[pTarget.a] +=1;
                                }
                                else {
                                    Main.listOfOwners.Add(pTarget.a, 0);
                                    Main.listOfOwners[pTarget.a] +=1;
                                }
                                b.addTrait("Subordinate");
                            }
                        }
                    }
                    //obey(pTarget, tempMapObject);
                        
                    
                }
            }
            #endregion
            #region поиск
            if ((pTarget.a.asset.id == "defile_demon" && Main.InvasionDemons) || 
            (Toolbox.randomChance(0.1f) && pTarget.a.hasTrait("hiddenEvil")))
            {
                City Target = (City) null;
                float num1 = 0f;
                foreach (City listCity in World.world.cities.list)
                {
                    float num2 = Toolbox.DistVec3(listCity.cityCenter, pTarget.currentTile.posV);
                    if ( (Target == null || (double) num2 < (double) num1) && listCity.race.nameLocale != "Demonic")
                    {
                        Target = listCity;
                        num1 = num2;
                    }
                }
                if (Target == null)
                {
                    return false;
                }
                FunctionalAction.goToTarget(pTarget,Target.kingdom.id,pTile);
            }
            else if ((Toolbox.randomChance(0.1f) && pTarget.a.asset.id == "defile_demon" && !Main.InvasionDemons) || 
            (Toolbox.randomChance(0.1f) && pTarget.a.isRace("demon") && !pTarget.a.hasTrait("Subordinate")))
            {
                FunctionalAction.goToTarget(pTarget,"demons");
            }
            #endregion
            return true;
        }
        public static bool deathDesecration(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
         if (pTarget != null)
         {
            Actor a = pTarget.a;
            a.removeTrait("Defiler");
            a.removeTrait("madness");
            if (Toolbox.randomChance(0.001f) && 
            MagicTraits.good_defiler.ContainsKey(pTarget.a.asset.id) &&
            pTarget.a.asset.id != "defile_demon" &&
            pTarget.a.asset.id != "demonKing")
            {
                World.world.buildings.addBuilding("DefilerGate", pTile, false, false, BuildPlacingType.New);
            }
            if (pTarget.a.asset.id == "demonKing")
            {
                Main.DemonKing.Remove(pTarget.a);
                ActionLibrary.kingSlayer(pTarget, pTile);
            }
            return true;
            
         }
         return false;

        }
        public static bool hiddenEvils(BaseSimObject pTarget, WorldTile pTile = null) 
      	{
            World.world.getObjectsInChunks(pTarget.currentTile, 20, MapObjectType.Actor);
            if (pTarget.a.asset.id == "hidden_demon")
            {
                for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
                {
                    Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                    if (tempMapObject.hasTrait("Demon Fighter") && 
                    tempMapObject != pTarget.a && !tempMapObject.hasTrait("hiddenEvil"))
                    {
                        FunctionalAction.reborn(pTarget, tempMapObject.asset.id);
                        return false;
                    }
                }
            }
            else
            {
                int index1 = 0;
                for (int index = 0; index < World.world.temp_map_objects.Count; ++index)
                {
                    Actor tempMapObject = (Actor) World.world.temp_map_objects[index];
                    if (!tempMapObject.hasTrait("Demon Fighter") || tempMapObject.hasTrait("hiddenEvil"))
                    {
                        index1++;
                    }  
                }
                if (index1 == World.world.temp_map_objects.Count)
                {
                    FunctionalAction.reborn(pTarget, "hidden_demon");
                }
            }
            return true;
        }
        #endregion
#region Ящеры
        
//Ящеры
        public static bool LizardsRegen(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            //Regen(pSelf,777);
            FunctionalAction.spellCost(pSelf,0,-777);
            pSelf.a.removeTrait("eyepatch");
            pSelf.a.removeTrait("skin_burns");
            pSelf.a.removeTrait("crippled"); 
            return true;
        }
        public static bool LizardsRegen1(BaseSimObject pTarget, WorldTile pTile = null)
        {
            FunctionalAction.Regen(pTarget,100);
            
            return true;
        }
        #endregion
#endregion
    }
}