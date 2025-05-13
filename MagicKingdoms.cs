using System;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ReflectionUtility;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using HarmonyLib;

namespace Magic{
    class MagicKingdoms
    {
        public static void init(){
            
            KingdomAsset orc = AssetManager.kingdoms.get("orc");
            KingdomAsset nomads_orc = AssetManager.kingdoms.get("nomads_orc");
            KingdomAsset undead = AssetManager.kingdoms.get("undead");
            KingdomAsset demons = AssetManager.kingdoms.get("demons");
            KingdomAsset dwarf = AssetManager.kingdoms.get("dwarf");
            KingdomAsset nomads_dwarf = AssetManager.kingdoms.get("nomads_dwarf");
            KingdomAsset human = AssetManager.kingdoms.get("human");
            KingdomAsset nomads_human = AssetManager.kingdoms.get("nomads_human");
            KingdomAsset elf = AssetManager.kingdoms.get("elf");
            KingdomAsset nomads_elf = AssetManager.kingdoms.get("nomads_elf");
            KingdomAsset madness = AssetManager.kingdoms.get("mad");
            //KingdomAsset nature_creature = AssetManager.kingdoms.get("nature_creature");
            KingdomAsset evil = AssetManager.kingdoms.get("evil");
            KingdomAsset good = AssetManager.kingdoms.get("good");
            KingdomAsset ufo = AssetManager.kingdoms.get(SK.aliens);
            KingdomAsset crystals = AssetManager.kingdoms.get(SK.crystals);
            KingdomAsset demonic = AssetManager.kingdoms.get("demonic");
            KingdomAsset nomads_demonic = AssetManager.kingdoms.get("nomads_demonic");
            KingdomAsset angel = AssetManager.kingdoms.get("angel");
            KingdomAsset nomads_angel = AssetManager.kingdoms.get("nomads_angel");
            KingdomAsset darkelve = AssetManager.kingdoms.get("darkelve");
            KingdomAsset nomads_darkelve = AssetManager.kingdoms.get("nomads_darkelve");
            KingdomAsset goblin = AssetManager.kingdoms.get("goblin");
            KingdomAsset nomads_goblin = AssetManager.kingdoms.get("nomads_goblin");
            KingdomAsset beastmen = AssetManager.kingdoms.get("beastmen");
            KingdomAsset nomads_beastmen = AssetManager.kingdoms.get("nomads_beastmen");
            KingdomAsset lizard = AssetManager.kingdoms.get("lizard");
            KingdomAsset nomads_lizard = AssetManager.kingdoms.get("nomads_lizard");
            KingdomAsset gnome = AssetManager.kingdoms.get("gnome");
            KingdomAsset nomads_gnome = AssetManager.kingdoms.get("nomads_gnome");
            KingdomAsset android = AssetManager.kingdoms.get("android");
            KingdomAsset nomads_android = AssetManager.kingdoms.get("nomads_android");
            KingdomAsset japaneses = AssetManager.kingdoms.get("japaneses");
            KingdomAsset nomads_japaneses = AssetManager.kingdoms.get("nomads_japaneses");
            /*"unit_angel","Belobog"},
            {"unit_demonic","Chernobog"},
            {"unit_beastmen","Veles"},
            {"unit_gnome","Semargl"},
            {"unit_darkelve","Morana"},
            {"unit_goblin","Morok"},
            {"unit_lizard","Gorynya"},
            {"unit_android","AA-000"},
            {"unit_ancientchina","Shan di"},
            {"unit_japaneses","Amaterasu"*/
            
            evil.addFriendlyTag("demons");
            evil.addFriendlyTag("demonic");
            demons.addFriendlyTag("evil");
            demons.addFriendlyTag("lighthouses");
            demons.addFriendlyTag("demonic");
            if (demonic != null)
            {
                demonic.addFriendlyTag("demons");
                demonic.addFriendlyTag("evil");
                demonic.addTag("evil");
                demonic.addTag("demons");

                darkelve.addFriendlyTag("undead");
                darkelve.addFriendlyTag("vampire");

                gnome.addFriendlyTag(SK.crystals);
                nomads_gnome.addFriendlyTag(SK.crystals);

                angel.addFriendlyTag("good");
                angel.addTag("good");
                angel.addTag("neutral");
                angel.addTag(SK.nature_creature);
                angel.addFriendlyTag(SK.nature_creature);
                angel.addFriendlyTag(SK.neutral);
                angel.addFriendlyTag("chimera");
                angel.enemy_tags.Remove(SK.nature_creature);
                angel.enemy_tags.Remove(SK.good);
                angel.enemy_tags.Remove(SK.neutral);
                nomads_angel.enemy_tags.Remove(SK.nature_creature);
                nomads_angel.enemy_tags.Remove(SK.good);
                nomads_angel.enemy_tags.Remove(SK.neutral);
                nomads_angel.addFriendlyTag("good");
                nomads_angel.addTag("good");
                nomads_angel.addTag("neutral");
                nomads_angel.addTag(SK.nature_creature);
                nomads_angel.addFriendlyTag(SK.nature_creature);
                nomads_angel.addFriendlyTag(SK.neutral);
                nomads_angel.addFriendlyTag("chimera");

                beastmen.enemy_tags.Remove(SK.nature_creature);
                beastmen.enemy_tags.Remove(SK.good);
                beastmen.enemy_tags.Remove(SK.neutral);
                beastmen.addTag(SK.nature_creature);
                beastmen.addFriendlyTag("nature_creature");
                beastmen.addFriendlyTag("chimera");
                nomads_beastmen.enemy_tags.Remove(SK.nature_creature);
                nomads_beastmen.enemy_tags.Remove(SK.good);
                nomads_beastmen.enemy_tags.Remove(SK.neutral);
                nomads_beastmen.addTag(SK.nature_creature);
                nomads_beastmen.addFriendlyTag("nature_creature");
                nomads_beastmen.addFriendlyTag("chimera");
                
            }


            nomads_elf.addFriendlyTag("chimera");
            elf.addFriendlyTag("chimera");

           // nature_creature.addFriendlyTag("chimera");
            good.addFriendlyTag("angel");
            crystals.addFriendlyTag("gnome");
            crystals.addFriendlyTag("dwarf");
            undead.addFriendlyTag("vampire");
            undead.addFriendlyTag("goblin");
            undead.addFriendlyTag("darkelve");
            if (goblin != null)
            {
                goblin.addFriendlyTag("undead");
                goblin.addFriendlyTag("vampire");
                nomads_goblin.addFriendlyTag("undead");
                nomads_goblin.addFriendlyTag("vampire");
            }


            dwarf.addFriendlyTag(SK.crystals);
            nomads_dwarf.addFriendlyTag(SK.crystals);
            
            madness.addFriendlyTag("vampire");
            madness.addFriendlyTag("illithiiry");
            orc.addFriendlyTag("vampire");
            nomads_orc.addFriendlyTag("vampire");

            var chimera = new KingdomAsset();
            chimera.id = "chimera";
            chimera.mobs = true;
            chimera.addTag("chimera");
            chimera.addTag("nature_creature");
            chimera.addFriendlyTag("chimera");
            chimera.addFriendlyTag("angel");
            chimera.addFriendlyTag("elf");
            chimera.addFriendlyTag("beastmen");
            chimera.addFriendlyTag("nature_creature");
            //chimera.addEnemyTag("civ");
            chimera.default_kingdom_color = new ColorAsset("#113f0f", "#113f0f", "#113f0f");
            AssetManager.kingdoms.add(chimera);
            World.world.kingdoms.CallMethod("newHiddenKingdom", chimera);

            var Spirit = new KingdomAsset();
            Spirit.id = "spirit";
            Spirit.mobs = true;
            Spirit.addTag("spirit");
            Spirit.addFriendlyTag("spirit");
            //Spirit.addFriendlyTag("goblin");
            //Spirit.addFriendlyTag("orc");
            //Spirit.addFriendlyTag("nomads_orc");
            Spirit.addEnemyTag("civ");
            Spirit.default_kingdom_color = new ColorAsset("#38705C", "#38705C", "#38705C");
            AssetManager.kingdoms.add(Spirit);
            World.world.kingdoms.CallMethod("newHiddenKingdom", Spirit);

            var lighthouse = new KingdomAsset();
            lighthouse.id = "lighthouses";
            lighthouse.mobs = true;
            lighthouse.addTag("lighthouses");
            lighthouse.addFriendlyTag("lighthouses");
            lighthouse.addFriendlyTag("demons");
            //Spirit.addFriendlyTag("goblin");
            //Spirit.addFriendlyTag("orc");
            //Spirit.addFriendlyTag("nomads_orc");
            lighthouse.addEnemyTag("civ");
            lighthouse.default_kingdom_color = new ColorAsset("#38705C", "#38705C", "#38705C");
            AssetManager.kingdoms.add(lighthouse);
            World.world.kingdoms.CallMethod("newHiddenKingdom", lighthouse);

            var vampire = new KingdomAsset();
            vampire.id = "vampire";
            vampire.civ = true;
            vampire.addTag("vampire");
            vampire.addTag("undead");
            vampire.addTag(SK.civ);
            #region Дружба
            vampire.addFriendlyTag("vampire");
            vampire.addFriendlyTag("orc");
            vampire.addFriendlyTag("darkelve");
            vampire.addFriendlyTag("undead");
            vampire.addFriendlyTag("goblin");
            vampire.addFriendlyTag("mad");
            #endregion
            #region Злоба
            vampire.addEnemyTag("good");
            vampire.addEnemyTag("neutral");
            vampire.addEnemyTag("bandits");
            vampire.addEnemyTag("human");
            vampire.addEnemyTag("dwarf");
            vampire.addEnemyTag("elf");
            #endregion
            AssetManager.kingdoms.add(vampire);
            World.world.kingdoms.CallMethod("newHiddenKingdom", vampire);

            var nomadsvampire = new KingdomAsset();
            nomadsvampire.id = "nomads_vampire";
            nomadsvampire.nomads = true;
            nomadsvampire.mobs = true;
            nomadsvampire.addTag("vampire");
            nomadsvampire.addTag(SK.civ);
            #region Дружба
            nomadsvampire.addFriendlyTag("vampire");
            nomadsvampire.addFriendlyTag("goblin");
            nomadsvampire.addFriendlyTag("darkelve");
            nomadsvampire.addFriendlyTag("orc");
            nomadsvampire.addFriendlyTag("undead");
            nomadsvampire.addFriendlyTag("mad");
            #endregion
            #region Злоба
            nomadsvampire.addEnemyTag("good");
            nomadsvampire.addEnemyTag("neutral");
            nomadsvampire.addEnemyTag("human");
            nomadsvampire.addEnemyTag("dwarf");
            nomadsvampire.addEnemyTag("elf");
            #endregion
            nomadsvampire.default_kingdom_color = new ColorAsset("#38705C", "#38705C", "#38705C");
            AssetManager.kingdoms.add(nomadsvampire);
            World.world.kingdoms.CallMethod("newHiddenKingdom", nomadsvampire);

            var illithiiry = new KingdomAsset();
            illithiiry.id = "illithiiry";
            illithiiry.civ = true;
            illithiiry.addTag("illithiiry");
            //illithiiry.addTag("mad");
            illithiiry.addTag(SK.civ);
            #region Дружба
            illithiiry.addFriendlyTag("illithiiry");
            illithiiry.addFriendlyTag("mad");
            //illithiiry.addFriendlyTag("darkelve");
            //illithiiry.addFriendlyTag("undead");
            //illithiiry.addFriendlyTag("goblin");
            //illithiiry.addFriendlyTag("mad");
            #endregion
            #region Злоба
            illithiiry.addEnemyTag("good");
            illithiiry.addEnemyTag("neutral");
            //illithiiry.addEnemyTag("bandits");
            illithiiry.addEnemyTag("human");
            illithiiry.addEnemyTag("dwarf");
            illithiiry.addEnemyTag("elf");
            #endregion
            AssetManager.kingdoms.add(illithiiry);
            World.world.kingdoms.CallMethod("newHiddenKingdom", illithiiry);

            var nomadsillithiiry = new KingdomAsset();
            nomadsillithiiry.id = "nomads_illithiiry";
            nomadsillithiiry.nomads = true;
            nomadsillithiiry.mobs = true;
            nomadsillithiiry.addTag("illithiiry");
            //illithiiry.addTag("mad");
            nomadsillithiiry.addTag(SK.civ);
            #region Дружба
            nomadsillithiiry.addFriendlyTag("illithiiry");
            nomadsillithiiry.addFriendlyTag("mad");
            //illithiiry.addFriendlyTag("darkelve");
            //illithiiry.addFriendlyTag("undead");
            //illithiiry.addFriendlyTag("goblin");
            //illithiiry.addFriendlyTag("mad");
            #endregion
            #region Злоба
            nomadsillithiiry.addEnemyTag("good");
            nomadsillithiiry.addEnemyTag("neutral");
            //illithiiry.addEnemyTag("bandits");
            nomadsillithiiry.addEnemyTag("human");
            nomadsillithiiry.addEnemyTag("dwarf");
            nomadsillithiiry.addEnemyTag("elf");
            #endregion
            nomadsillithiiry.default_kingdom_color = new ColorAsset("#926ca7", "#926ca7", "#926ca7");
            AssetManager.kingdoms.add(nomadsillithiiry);
            World.world.kingdoms.CallMethod("newHiddenKingdom", nomadsillithiiry);
        }
    }
}