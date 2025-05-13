using System;
using System.Linq;
using System.Collections.Generic;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ReflectionUtility;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Beebyte.Obfuscator;
using HarmonyLib;

namespace Magic{
    class MagicRaces
    {
        public static void init(){

            var unit_demonic = AssetManager.actor_library.get("unit_demonic");
            var baby_demonic = AssetManager.actor_library.get("baby_demonic");
            var unit_angel = AssetManager.actor_library.get("unit_angel");
            if (unit_demonic != null)
            {
                unit_demonic.traits.Add("Defiler");
                unit_demonic.canBeKilledByDivineLight = true;
                baby_demonic.canBeKilledByDivineLight = true;
                //unit_angel.traits.Add("Demon Fighter");
            }

            
            var unit_goblin = AssetManager.actor_library.get("unit_goblin");
            var unit_lizard = AssetManager.actor_library.get("unit_lizard");
            if (unit_angel != null)
            {
                unit_angel.traits.Add("holy_magic");
            }

            var unit_orc = AssetManager.actor_library.get("unit_orc");
            unit_orc.base_stats[S.opinion] -= 20;
            unit_orc.base_stats[S.fertility] += 1;
            unit_orc.base_stats[S.max_children] +=4;
            unit_orc.base_stats[S.scale] += 0.05f;
            unit_orc.base_stats[S.loyalty_traits] -= 100;

            var unit_elf = AssetManager.actor_library.get("unit_elf");
            unit_elf.base_stats[S.max_age] = 700;
            unit_elf.base_stats[S.opinion] -= 5f;
            unit_elf.base_stats[S.fertility] -= 0.2f;
            unit_elf.base_stats[S.max_children] -= 2;
            unit_elf.base_stats[S.scale] += 0.02f;


            var unit_dwarf = AssetManager.actor_library.get("unit_dwarf");
            unit_dwarf.base_stats[S.max_age] = 200;
            unit_dwarf.base_stats[S.opinion] -= 3f;
            unit_dwarf.base_stats[S.fertility] -= 0.2f;
            unit_dwarf.base_stats[S.max_children] -= 2f;
            unit_dwarf.base_stats[S.scale] -= 0.03f;

            var unit_human = AssetManager.actor_library.get("unit_human");
            unit_human.base_stats[S.fertility] += 0.2f;
            unit_human.base_stats[S.diplomacy] += 5f;

            /*var vampire_slave = AssetManager.actor_library.clone("unit_vampire_slave", "unit_human");
            vampire_slave.base_stats[S.max_age] = 1000;
            vampire_slave.base_stats[S.max_children] = -200f;
            vampire_slave.base_stats[S.fertility] = -10000f;
            vampire_slave.base_stats[S.attack_speed] = 90f;
            vampire_slave.base_stats[S.knockback_reduction] += 10f;
            vampire_slave.setBaseStats(333, 30, 80, 5, 30, 92, 0);
            vampire_slave.nameLocale = "vampire_slave";
            vampire_slave.nameTemplate = "human_name";
            vampire_slave.race = "vampire";
            vampire_slave.icon = "vampire_slave";
            vampire_slave.effect_teleport = "fx_teleport_red";
		    vampire_slave.fmod_spawn = "event:/SFX/UNITS/Human/HumanSpawn";
		    vampire_slave.fmod_attack = "event:/SFX/UNITS/Human/HumanAttack";
		    vampire_slave.fmod_idle = "event:/SFX/UNITS/Human/HumanIdle";
		    vampire_slave.fmod_death = "event:/SFX/UNITS/Human/HumanDeath";
            vampire_slave.zombieID = "zombie";
            vampire_slave.canTurnIntoZombie = true;
            vampire_slave.canTurnIntoMush = true;
            vampire_slave.has_soul = true;
            vampire_slave.canBeKilledByDivineLight = false;
            vampire_slave.canTurnIntoTumorMonster = false;
            vampire_slave.can_turn_into_demon_in_age_of_chaos = false;
            vampire_slave.canTurnIntoIceOne = false;
            vampire_slave.disableJumpAnimation = true;
            vampire_slave.body_separate_part_head = false;
            vampire_slave.color = Toolbox.makeColor("#005E72");
            //AssetManager.actor_library.CallMethod("addTrait", "evil");
            //AssetManager.actor_library.CallMethod("addTrait", "bloodlust");
            //AssetManager.actor_library.CallMethod("addTrait", "Vampire");
            //AssetManager.actor_library.CallMethod("addTrait", "immortal");
            //.actor_library.CallMethod("addTrait", "venomous");
            //AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
            //AssetManager.actor_library.CallMethod("addTrait", "nightchild");
            AssetManager.actor_library.CallMethod("addTrait", "strong_minded");
            AssetManager.actor_library.CallMethod("addTrait", "vampire_slave");
            AssetManager.actor_library.CallMethod("loadShadow", vampire_slave);
            Localization.addLocalization(vampire_slave.nameLocale, vampire_slave.nameLocale);*/

            var vampire = AssetManager.actor_library.clone("unit_vampire", "unit_human");
            vampire.base_stats[S.max_age] = 1000;
            vampire.base_stats[S.max_children] = -200f;
            vampire.base_stats[S.fertility] = -10000f;
            vampire.base_stats[S.attack_speed] = 90f;
            vampire.base_stats[S.knockback_reduction] += 10f;
            vampire.setBaseStats(333, 30, 80, 5, 30, 92, 0);
            vampire.nameLocale = "Vampire";
            vampire.nameTemplate = "human_name";
            vampire.race = "vampire";
            vampire.icon = "Vampire";
            vampire.effect_teleport = "fx_teleport_red";
		    vampire.fmod_spawn = "event:/SFX/UNITS/Human/HumanSpawn";
		    vampire.fmod_attack = "event:/SFX/UNITS/Human/HumanAttack";
		    vampire.fmod_idle = "event:/SFX/UNITS/Human/HumanIdle";
		    vampire.fmod_death = "event:/SFX/UNITS/Human/HumanDeath";
            vampire.zombieID = "zombie";
            vampire.canTurnIntoZombie = false;
            vampire.canTurnIntoMush = false;
            vampire.has_soul = false;
            vampire.canBeKilledByDivineLight = true;
            vampire.canTurnIntoTumorMonster = false;
            vampire.immune_to_slowness = true;
            vampire.can_turn_into_demon_in_age_of_chaos = false;
            vampire.canTurnIntoIceOne = false;
            vampire.disableJumpAnimation = true;
            vampire.body_separate_part_head = false;
            vampire.color = Toolbox.makeColor("#005E72");
            AssetManager.actor_library.CallMethod("addTrait", "evil");
            AssetManager.actor_library.CallMethod("addTrait", "bloodlust");
            AssetManager.actor_library.CallMethod("addTrait", "Vampire");
            AssetManager.actor_library.CallMethod("addTrait", "immortal");
            AssetManager.actor_library.CallMethod("addTrait", "venomous");
            AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
            AssetManager.actor_library.CallMethod("addTrait", "nightchild");
            AssetManager.actor_library.CallMethod("addTrait", "strong_minded");
            AssetManager.actor_library.CallMethod("loadShadow", vampire);
            Localization.addLocalization(vampire.nameLocale, vampire.nameLocale);

            var babyvampire = AssetManager.actor_library.clone("baby_vampire", "unit_vampire");
            babyvampire.base_stats[S.speed] = 10f;
            babyvampire.animation_idle = "walk_0";
            babyvampire.growIntoID = "unit_vampire";
            babyvampire.canTurnIntoZombie = false;
            babyvampire.canTurnIntoMush = false;
            babyvampire.has_soul = false;
            babyvampire.immune_to_slowness = true;
            babyvampire.canTurnIntoTumorMonster = false;
            babyvampire.can_turn_into_demon_in_age_of_chaos = false;
            babyvampire.canTurnIntoIceOne = false;
            babyvampire.body_separate_part_head = false;
            babyvampire.body_separate_part_hands = false;
            babyvampire.take_items = false;
            babyvampire.baby = true;
            babyvampire.disableJumpAnimation = true;
            babyvampire.color_sets = vampire.color_sets;
            AssetManager.actor_library.CallMethod("addTrait", "peaceful");
            AssetManager.actor_library.CallMethod("addTrait", "evil");
            AssetManager.actor_library.CallMethod("addTrait", "bloodlust");
            AssetManager.actor_library.CallMethod("addTrait", "Vampire");
            AssetManager.actor_library.CallMethod("addTrait", "immortal");
            AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
            AssetManager.actor_library.CallMethod("addTrait", "nightchild");
            //AssetManager.actor_library.CallMethod("addTrait", "Кровопийца");
            AssetManager.actor_library.CallMethod("loadShadow", babyvampire);

            var illithiiry = AssetManager.actor_library.clone("unit_illithiiry", "unit_human");
            illithiiry.base_stats[S.max_age] = 500;
            illithiiry.base_stats[S.max_children] = 1f;
            illithiiry.base_stats[S.range] += 3f;
            //illithiiry.base_stats[S.fertility] = -10f;
            illithiiry.base_stats[S.attack_speed] = 50f;
            //illithiiry.base_stats[S.knockback_reduction] += 10f;
            illithiiry.setBaseStats(60, 10, 80, 0, 60, 98, 1);
            illithiiry.nameLocale = "illithiiry";
            illithiiry.nameTemplate = "human_name";
            illithiiry.race = "illithiiry";
            illithiiry.icon = "illithiiry";
            illithiiry.effect_teleport = "fx_teleport_red";
		    illithiiry.fmod_spawn = "event:/SFX/UNITS/Human/HumanSpawn";
		    illithiiry.fmod_attack = "event:/SFX/UNITS/Human/HumanAttack";
		    illithiiry.fmod_idle = "event:/SFX/UNITS/Human/HumanIdle";
		    illithiiry.fmod_death = "event:/SFX/UNITS/Human/HumanDeath";
            illithiiry.zombieID = "zombie";
            illithiiry.canTurnIntoZombie = true;
            illithiiry.canTurnIntoMush = false;
            illithiiry.has_soul = true;
            illithiiry.canBeKilledByDivineLight = false;
            illithiiry.canTurnIntoTumorMonster = true;
            illithiiry.can_turn_into_demon_in_age_of_chaos = true;
            illithiiry.canTurnIntoIceOne = false;
            illithiiry.disableJumpAnimation = true;
            illithiiry.body_separate_part_head = false;
            illithiiry.color = Toolbox.makeColor("#be00de");
            //AssetManager.actor_library.CallMethod("addTrait", "evil");
            //AssetManager.actor_library.CallMethod("addTrait", "genius");
            AssetManager.actor_library.CallMethod("addTrait", "MindMagic");
            AssetManager.actor_library.CallMethod("addTrait", "ugly");
            //AssetManager.actor_library.CallMethod("addTrait", "venomous");
            //AssetManager.actor_library.CallMethod("addTrait", "poison_immune");
            AssetManager.actor_library.CallMethod("addTrait", "nightchild");
            AssetManager.actor_library.CallMethod("addTrait", "strong_minded");
            AssetManager.actor_library.CallMethod("loadShadow", illithiiry);
            Localization.addLocalization(illithiiry.nameLocale, illithiiry.nameLocale);

            var babyillithiiry = AssetManager.actor_library.clone("baby_illithiiry", "unit_illithiiry");
            babyillithiiry.base_stats[S.speed] = 20f;
            babyillithiiry.animation_idle = "walk_0";
            babyillithiiry.growIntoID = "unit_illithiiry";
            babyillithiiry.canTurnIntoZombie = true;
            babyillithiiry.canTurnIntoMush = false;
            babyillithiiry.has_soul = true;
            babyillithiiry.canBeKilledByDivineLight = false;
            babyillithiiry.canTurnIntoTumorMonster = true;
            babyillithiiry.can_turn_into_demon_in_age_of_chaos = true;
            babyillithiiry.canTurnIntoIceOne = false;
            babyillithiiry.body_separate_part_head = false;
            babyillithiiry.body_separate_part_hands = false;
            babyillithiiry.take_items = false;
            babyillithiiry.baby = true;
            babyillithiiry.disableJumpAnimation = true;
            babyillithiiry.color_sets = illithiiry.color_sets;
            AssetManager.actor_library.CallMethod("addTrait", "peaceful");
            //AssetManager.actor_library.CallMethod("addTrait", "evil");
            AssetManager.actor_library.CallMethod("addTrait", "nightchild");
           // AssetManager.actor_library.CallMethod("addTrait", "genius");
            AssetManager.actor_library.CallMethod("addTrait", "MindMagic");
            AssetManager.actor_library.CallMethod("addTrait", "ugly");
            //AssetManager.actor_library.CallMethod("addTrait", "nightchild");
            AssetManager.actor_library.CallMethod("addTrait", "strong_minded");
            AssetManager.actor_library.CallMethod("loadShadow", babyillithiiry);
        }
    }
}