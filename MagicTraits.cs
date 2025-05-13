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

namespace Magic
{
    public class bloodStats
    {
        public float health = 0f;
        public float damage = 0f;
        public float attack_speed = 0f;
        public float speed = 0f;
        public int max_age = 0;
        public float armor = 0f;
    } 
    class MagicTraits
    { 
        public static Dictionary<string,float> good_defiler = new Dictionary<string,float>()
            {
            {"unit_goblin",0.3f}, 
            {"unit_lizard",0.3f}, 
            {"unit_android",0.3f}, 
            {"unit_darkelve",0.3f}, 
            {"unit_beastmen",0.3f}, 
            {"unit_gnome",0.3f}, 
            {"unit_demonic",0.3f}, 
            {"unit_japaneses",0.3f}, 
            {"unit_ancientchina",0.3f},
            {"unit_vampire",0.3f},
            {"unit_human",0.3f},
            {"unit_orc",0.3f},
            {"unit_elf",0.3f},
            {"unit_dwarf",0.3f},
            {"defile_demon",0.3f},
            {"demonKing",0.45f}
            };
        
        public static Dictionary<Actor, bloodStats> bloodEnhancement = new Dictionary<Actor, bloodStats>();
       /* public static float magicBirth = 3f;
        public static float magicInherit = 30f;
        public static float BloodAge = 300f;
        public static float DefilerBirth = 0f;
        public static float DefilerInherit = 0f;Main.
        public static float DemonFighterBirth = 0f;
        public static float DemonFighterInherit = 10f;
        public static bool NewMagicOfDeath;*/
        public static void init()
        {
#region Черты  
        #region Скрытое Зло
            ActorTrait hiddenEvil = new ActorTrait();
            //hiddenEvil.action_attack_target = new AttackAction(hiddenEvil);
            //Pacification.action_get_hit = new GetHitAction(pacification);
            hiddenEvil.action_special_effect = new WorldAction(TraitEffect.hiddenEvils);
            hiddenEvil.id = "hiddenEvil";
            hiddenEvil.path_icon = "ui/Extras/hiddenEvil";
            //hiddenEvil.birth = Main.DefilerBirth;
            hiddenEvil.inherit = Main.DefilerInherit;
            //hiddenEvil.base_stats[S.mod_health] += 0.5f;
            //hiddenEvil.base_stats[S.health] += 50f;
            //hiddenEvil.opposite = "Defiler";
            hiddenEvil.group_id = MagicTraitGroup.fel;
            //hiddenEvil.oppositeTraitMod -= 1000;
            hiddenEvil.can_be_given = true;
            AssetManager.traits.add(hiddenEvil);
            addTraitToLocalizedLibrary("en",hiddenEvil.id, "Hidden Evil","Find him before it's too late");
            addTraitToLocalizedLibrary("ru",hiddenEvil.id, "Скрытое Зло","Найди его прежде чем станет поздно");
            PlayerConfig.unlockTrait("hiddenEvil");
            #endregion
        #region Ангелы
            ActorTrait Pacification = new ActorTrait();
            Pacification.action_attack_target = new AttackAction(TraitEffect.pacification);
            //Pacification.action_get_hit = new GetHitAction(pacification);
            Pacification.action_special_effect = new WorldAction(TraitEffect.pacification1);
            Pacification.id = "Demon Fighter";
            Pacification.path_icon = "ui/Extras/Pacification";
            Pacification.birth = Main.DemonFighterBirth;
            Pacification.inherit = Main.DemonFighterInherit;
            Pacification.base_stats[S.mod_health] += 0.5f;
            Pacification.base_stats[S.health] += 50f;
            Pacification.opposite = "Defiler";
            Pacification.group_id = MagicTraitGroup.fel;
            Pacification.special_effect_interval = 1.01f;
            Pacification.oppositeTraitMod -= 1000;
            Pacification.can_be_given = true;
            AssetManager.traits.add(Pacification);
            addTraitToLocalizedLibrary("en",Pacification.id, Pacification.id,"Demon fighters stop the spread of filth");
            addTraitToLocalizedLibrary("ru",Pacification.id, "Демоноборец","Демоноборцы останавливают распространение скверны");
            PlayerConfig.unlockTrait("Demon Fighter");
        #endregion 
        #region Герой    
            ActorTrait Hero = new ActorTrait();
            Hero.action_attack_target = new AttackAction(TraitEffect.pacification);
            //Pacification.action_get_hit = new GetHitAction(pacification);
            Hero.action_special_effect = new WorldAction(TraitEffect.pacification1);
            Hero.id = "Hero";
            Hero.path_icon = "ui/Extras/Hero";
            Hero.base_stats[S.mod_health] += 7.77f;
            Hero.base_stats[S.health] += 777f;
            Hero.base_stats[S.damage] += 777f;
            Hero.base_stats[S.armor] += 777f;
            Hero.base_stats[S.speed] += 77f;
            Hero.base_stats[S.attack_speed] += 77f;
            Hero.base_stats[S.warfare] += 77f;
            Hero.base_stats[S.intelligence] -= 777f;
            Hero.opposite = "Defiler";
            Hero.group_id = MagicTraitGroup.fel;
            Hero.oppositeTraitMod -= 1000;
            Hero.can_be_given = true;
            AssetManager.traits.add(Hero);
            addTraitToLocalizedLibrary("en",Hero.id, Hero.id, "Call upon him in the hour of need");
            addTraitToLocalizedLibrary("ru",Hero.id, "Герой", "Призовите его в час нужды");
            PlayerConfig.unlockTrait("Hero");
        #endregion
        #region демоны
            ActorTrait Desecration = new ActorTrait();
            Desecration.action_attack_target = new AttackAction(TraitEffect.desecration);
            Desecration.action_special_effect = new WorldAction(TraitEffect.desecration2);
            Desecration.action_get_hit = new GetHitAction(TraitEffect.desecration);
            Desecration.action_death = new WorldAction(TraitEffect.deathDesecration);
            Desecration.birth = Main.DefilerBirth;
            Desecration.inherit = Main.DefilerInherit;
            Desecration.id = "Defiler";
            Desecration.path_icon = "ui/Extras/desecration";
            Desecration.base_stats[S.damage] += 50f;
            Desecration.base_stats[S.attack_speed] += 50f;
            Desecration.group_id = MagicTraitGroup.fel;
            Desecration.can_be_given = true;
            Desecration.opposite = "Demon Fighter";
            //Desecration.oppositeTraitMod -= 100000;
            Desecration.can_be_removed = true;
            AssetManager.traits.add(Desecration);
            addTraitToLocalizedLibrary("en",Desecration.id, "Sinner","Demons destroy the universe and the minds of their victims");
            addTraitToLocalizedLibrary("ru",Desecration.id, "Грешник","Демоны разрушают мироздание и разумы своих жертв");
            PlayerConfig.unlockTrait("Defiler");
        #endregion 
        #region Вампиры
            ActorTrait vampirism = new ActorTrait();
            vampirism.action_death = new WorldAction(TraitEffect.VampireDeathEffect1);
            vampirism.id = "Vampirism";
            vampirism.path_icon = "ui/Extras/Vampirism";
            vampirism.group_id = TraitGroup.acquired;
            string[] oppositeArrVism = new string[] { "Lycanthropy", "Vampire", "Elder Vampire","Werewolf", "Phoenix","AndroidPower2", "AndroidPower1","Spirit" };
            vampirism.oppositeArr = oppositeArrVism;
            vampirism.can_be_given = true;
            AssetManager.traits.add(vampirism);
            addTraitToLocalizedLibrary("en",vampirism.id, vampirism.id,"He doesn't like walking in the sun");
            addTraitToLocalizedLibrary("ru",vampirism.id, "Вампиризм","Ему неприятно ходить под солнцем");
            PlayerConfig.unlockTrait("Vampirism");

            ActorTrait Vampire = new ActorTrait();
            Vampire.action_attack_target = new AttackAction(TraitEffect.VampireAtackEffect1);
            Vampire.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(TraitEffect.bloodRestore));
            Vampire.action_special_effect = new WorldAction(TraitEffect.VampireDeathEffect2);
            Vampire.id = "Vampire";
            Vampire.path_icon = "ui/Extras/Vampire";
            /*Vampire.base_stats[S.attack_speed] += 75f;
            Vampire.base_stats[S.dodge] += 50f;
            Vampire.base_stats[S.health] += 200f;
            Vampire.base_stats[S.armor] += 20f;
            Vampire.base_stats[S.knockback_reduction] += 10f;
            Vampire.base_stats[S.fertility] -= 5000f;
            Vampire.base_stats[S.max_children] -= 200f;*/
            Vampire.group_id = TraitGroup.special;
            string[] oppositeArrvamp = new string[] { "Vampirism", "AndroidPower2" };
            Vampire.oppositeArr = oppositeArrvamp;
            Vampire.can_be_given = false;
            AssetManager.traits.add(Vampire);
            addTraitToLocalizedLibrary("en",Vampire.id, Vampire.id,"Vampires infect their victims through a bite and drink blood with pleasure");
            addTraitToLocalizedLibrary("ru",Vampire.id, "Вампир","Вампиры заражают своих жертв через укус и с удовольствием пьют кровь");
            PlayerConfig.unlockTrait("Vampire");

            ActorTrait ElderVampire = new ActorTrait();
            ElderVampire.action_attack_target = new AttackAction(TraitEffect.VampireAtackEffect2);
            ElderVampire.action_special_effect = new WorldAction(TraitEffect.LicanRegen);
            //ElderVampire.action_get_hit = new GetHitAction(VampireAtackEffect3);
            ElderVampire.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(TraitEffect.bloodRestore));
            ElderVampire.id = "Elder Vampire";
            ElderVampire.path_icon = "ui/Extras/ElderVampire";
            ElderVampire.base_stats[S.attack_speed] += 300f;
            ElderVampire.base_stats[S.knockback_reduction] += 1000f;
            ElderVampire.base_stats[S.dodge] += 500f;
            ElderVampire.base_stats[S.health] += 10000f;
            ElderVampire.base_stats[S.armor] += 200f;
            ElderVampire.base_stats[S.fertility] -= 5000f;
            ElderVampire.base_stats[S.max_children] -= 200f;
            ElderVampire.group_id = TraitGroup.special;
            string[] oppositeArrElderVamp = new string[] { "Vampirism", "AndroidPower1" };
            ElderVampire.oppositeArr = oppositeArrElderVamp;
            ElderVampire.can_be_given = true;
            AssetManager.traits.add(ElderVampire);
            addTraitToLocalizedLibrary("en",ElderVampire.id, ElderVampire.id,"Ancient vampires are the millennial horror of the worlds");
            addTraitToLocalizedLibrary("ru",ElderVampire.id, "Древний Вампир","Древние вампиры это тысячелетний ужас миров");
            PlayerConfig.unlockTrait("Elder Vampire");

            ActorTrait bloodsucker = new ActorTrait();
            bloodsucker.action_attack_target = new AttackAction(TraitEffect.bloodRestore);
            bloodsucker.id = "Bloodsucker";
            bloodsucker.path_icon = "ui/Extras/bloodsucker";
            bloodsucker.group_id = TraitGroup.spirit;
            bloodsucker.can_be_given = true;
            AssetManager.traits.add(bloodsucker);
            addTraitToLocalizedLibrary("en",bloodsucker.id, bloodsucker.id,"He lives off blood");
            addTraitToLocalizedLibrary("ru",bloodsucker.id, "Кровопийца","Он живет за счет крови");
            PlayerConfig.unlockTrait("Bloodsucker");
        #endregion 
        #region Феникс
            ActorTrait pheonix = new ActorTrait();
            pheonix.id = "Phoenix";
            pheonix.path_icon = "ui/Extras/phoenix";
            pheonix.group_id = TraitGroup.spirit;
            pheonix.birth = 0f;
            pheonix.inherit = 0f;
            pheonix.can_be_given = true;
            pheonix.base_stats[S.mod_health] += 1f;
            pheonix.base_stats[S.knockback_reduction] += 1000f;
            pheonix.base_stats[S.attack_speed] += 50f;
            pheonix.base_stats[S.speed] += 50f;
            //pheonix.base_stats[S.intelligence] += 50f;
            //pheonix.action_special_effect = new WorldAction(removeBadTrait);
            pheonix.special_effect_interval = 1.01f;
            pheonix.action_special_effect = (WorldAction)Delegate.Combine(pheonix.action_special_effect, new WorldAction(TraitEffect.LizardsRegen1));
            pheonix.action_death =  new WorldAction(FunctionalAction.rebornANew);
            string[] oppositeArrPhoeenix = new string[] { "Vampirism", "Lycanthropy","Werewolf" };
            pheonix.oppositeArr = oppositeArrPhoeenix;
            AssetManager.traits.add(pheonix);
            addTraitToLocalizedLibrary("en",pheonix.id, pheonix.id,"From the ashes, I will be reborn");
            addTraitToLocalizedLibrary("ru",pheonix.id, "Феникс","Из пепла, я возрожусь");
            PlayerConfig.unlockTrait(pheonix.id);
        #endregion 
        #region Магический дар
            ActorTrait giftOfMagic = new ActorTrait();
            giftOfMagic.id = "Magical Gift";
            //giftOfMagic.action_attack_target = new AttackAction(SubjugationSpirit);
            //giftOfMagic.action_get_hit = new GetHitAction(summonSpirit);
            //giftOfMagic.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
            giftOfMagic.action_death = new WorldAction(ActionLibrary.mageSlayer);
            giftOfMagic.path_icon = "ui/Extras/giftOfMagic";
            giftOfMagic.group_id = MagicTraitGroup.magic;
            giftOfMagic.can_be_given = true;
            giftOfMagic.base_stats[S.mod_health] -= 0.1f;
            giftOfMagic.base_stats[S.speed] -= 10f;
            giftOfMagic.base_stats[S.intelligence] += 5f;
            giftOfMagic.action_special_effect =  new WorldAction(TraitEffect.MagicUpgrade);
            giftOfMagic.special_effect_interval = 5f;
            giftOfMagic.birth = Main.magicBirth;
            giftOfMagic.inherit = Main.magicInherit;
            AssetManager.traits.add(giftOfMagic);
            addTraitToLocalizedLibrary("en",giftOfMagic.id, giftOfMagic.id,"A potential magician");
            addTraitToLocalizedLibrary("ru",giftOfMagic.id, "Магический Дар","Потенциальный маг");
            PlayerConfig.unlockTrait(giftOfMagic.id);
        #endregion 
        #region Огонь
                ActorTrait magicOfFire = new ActorTrait();
                magicOfFire.id = "Fire Magic";
                magicOfFire.action_special_effect = new WorldAction(TraitEffect.spellOfFire2);
                magicOfFire.action_attack_target = new AttackAction(TraitEffect.spellOfFire1);
                //magicOfFire.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfFire.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                magicOfFire.action_death = new WorldAction(ActionLibrary.mageSlayer);
                magicOfFire.path_icon = "ui/Extras/magicOfFire";
                magicOfFire.group_id = MagicTraitGroup.magic;
                magicOfFire.can_be_given = true;
                magicOfFire.base_stats[S.mod_damage] += 0.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                magicOfFire.base_stats[S.intelligence] += 2f;
                magicOfFire.base_stats[S.warfare] += 3f;
                magicOfFire.special_effect_interval = 3f;
                AssetManager.traits.add(magicOfFire);
                addTraitToLocalizedLibrary("en",magicOfFire.id, magicOfFire.id,"Fire Magic");
                addTraitToLocalizedLibrary("ru",magicOfFire.id, "Магия Огня","Магия Огня");
                PlayerConfig.unlockTrait(magicOfFire.id);
                #endregion 
        #region Вода
                ActorTrait magicOfWater = new ActorTrait();
                magicOfWater.id = "Water Magic";
                magicOfWater.action_special_effect = new WorldAction(TraitEffect.spellOfWater1);
                magicOfWater.action_attack_target = new AttackAction(TraitEffect.spellOfWater2);
                //magicOfFire.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfFire.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                magicOfWater.action_death = new WorldAction(ActionLibrary.mageSlayer);
                magicOfWater.path_icon = "ui/Extras/magicOfWater";
                magicOfWater.group_id = MagicTraitGroup.magic;
                magicOfWater.can_be_given = true;
                magicOfWater.base_stats[S.mod_health] += 0.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                magicOfWater.base_stats[S.intelligence] += 2f;
                magicOfWater.base_stats[S.diplomacy] += 3f;
                magicOfWater.special_effect_interval = 1f;
                AssetManager.traits.add(magicOfWater);
                addTraitToLocalizedLibrary("en",magicOfWater.id, magicOfWater.id,"Water Magic");
                addTraitToLocalizedLibrary("ru",magicOfWater.id, "Магия Воды","Магия Воды");
                PlayerConfig.unlockTrait(magicOfWater.id);
                #endregion 
        #region Воздух
                ActorTrait magicOfAir = new ActorTrait();
                magicOfAir.id = "Air Magic";
                magicOfAir.action_special_effect =  new WorldAction(TraitEffect.spellOfAir1);
                magicOfAir.action_attack_target = new AttackAction(TraitEffect.spellOfAir2);
                //magicOfFire.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfFire.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                magicOfAir.action_death = new WorldAction(ActionLibrary.mageSlayer);
                magicOfAir.path_icon = "ui/Extras/magicOfAir";
                magicOfAir.group_id = MagicTraitGroup.magic;
                magicOfAir.can_be_given = true;
                magicOfAir.base_stats[S.mod_health] += 0.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                magicOfAir.base_stats[S.intelligence] += 3f;
                magicOfAir.base_stats[S.diplomacy] += 3f;
                magicOfAir.base_stats[S.stewardship] -= 1f;
                magicOfAir.special_effect_interval = 3f;
                AssetManager.traits.add(magicOfAir);
                addTraitToLocalizedLibrary("en",magicOfAir.id, magicOfAir.id,"Air Magic");
                addTraitToLocalizedLibrary("ru",magicOfAir.id, "Магия Воздуха","Магия Воздуха");
                PlayerConfig.unlockTrait(magicOfAir.id);
                #endregion
        #region Земля
                ActorTrait magicOfEarth = new ActorTrait();
                magicOfEarth.id = "Earth Magic";
                magicOfEarth.action_special_effect =  new WorldAction(TraitEffect.spellOfEarth1);
                magicOfEarth.action_attack_target = new AttackAction(TraitEffect.spellOfEarth2);
                //magicOfFire.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfFire.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                magicOfEarth.action_death = new WorldAction(ActionLibrary.mageSlayer);
                magicOfEarth.path_icon = "ui/Extras/magicOfEarth";
                magicOfEarth.group_id = MagicTraitGroup.magic;
                magicOfEarth.can_be_given = true;
                magicOfEarth.base_stats[S.mod_health] += 0.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                magicOfEarth.base_stats[S.intelligence] += 2f;
                magicOfEarth.base_stats[S.stewardship] += 3f;
                magicOfEarth.special_effect_interval = 3f;
                AssetManager.traits.add(magicOfEarth);
                addTraitToLocalizedLibrary("en",magicOfEarth.id, magicOfEarth.id,"Earth Magic");
                addTraitToLocalizedLibrary("ru",magicOfEarth.id, "Магия Земли","Магия Земли");
                PlayerConfig.unlockTrait(magicOfEarth.id);
                #endregion
        #region Жизнь
                ActorTrait magicOfLife = new ActorTrait();
                magicOfLife.id = "The Magic of Life";
                magicOfLife.action_special_effect = new WorldAction(TraitEffect.spellOfLife1);
                magicOfLife.action_attack_target = new AttackAction(TraitEffect.spellOfLife2);
                //magicOfLife.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfLife.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                magicOfLife.action_death = new WorldAction(ActionLibrary.mageSlayer);
                magicOfLife.path_icon = "ui/Extras/magicOfLife";
                magicOfLife.group_id = MagicTraitGroup.magic;
                magicOfLife.can_be_given = true;
                magicOfLife.base_stats[S.mod_health] += 1.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                magicOfLife.base_stats[S.intelligence] += 2f;
                magicOfLife.base_stats[S.max_age] += 100f;
                magicOfLife.base_stats[S.diplomacy] += 3f;
                magicOfLife.special_effect_interval = 1.5f;
                AssetManager.traits.add(magicOfLife);
                addTraitToLocalizedLibrary("en",magicOfLife.id, magicOfLife.id,"The Magic of Life");
                addTraitToLocalizedLibrary("ru",magicOfLife.id, "Магия Жизни","Магия Жизни");
                PlayerConfig.unlockTrait(magicOfLife.id);
        #endregion         
        #region Смерть
                ActorTrait magicOfDeath = new ActorTrait();
                magicOfDeath.id = "The Magic of Death";
                magicOfDeath.action_special_effect = new WorldAction(TraitEffect.spellOfDeath1);
                magicOfDeath.action_attack_target = new AttackAction(TraitEffect.spellOfDeath2);
                //magicOfDeath.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfDeath.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                magicOfDeath.action_death = new WorldAction(TraitEffect.spellOfDeath3);
                magicOfDeath.path_icon = "ui/Extras/magicOfDeath";
                magicOfDeath.group_id = MagicTraitGroup.magic;
                magicOfDeath.can_be_given = true;
                magicOfDeath.base_stats[S.mod_health] += 1.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                magicOfDeath.base_stats[S.intelligence] += 2f;
                magicOfDeath.base_stats[S.diplomacy] -= 3f;
                magicOfDeath.base_stats[S.max_age] -= 100f;
                magicOfDeath.base_stats[S.stewardship] += 3f;
                magicOfDeath.base_stats[S.warfare] += 3f;
                magicOfDeath.special_effect_interval = 1.5f;
                AssetManager.traits.add(magicOfDeath);
                addTraitToLocalizedLibrary("en",magicOfDeath.id, magicOfDeath.id,"The Magic of Death");
                addTraitToLocalizedLibrary("ru",magicOfDeath.id, "Магия Смерти","Магия Смерти");
                PlayerConfig.unlockTrait(magicOfDeath.id);
                #endregion  
        #region Святость
                ActorTrait holyMagic = new ActorTrait();
                holyMagic.id = "holy_magic";
                holyMagic.action_special_effect =  new WorldAction(TraitEffect.holySpell1);
                holyMagic.action_attack_target = new AttackAction(TraitEffect.holySpell2);
                //magicOfDeath.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfDeath.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                //holyMagic.action_death = new WorldAction(TraitEffect.holyDeath);
                holyMagic.path_icon = "ui/Extras/holyMagic";
                holyMagic.group_id = MagicTraitGroup.magic;
                holyMagic.can_be_given = true;
                holyMagic.base_stats[S.mod_health] += 0.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                holyMagic.base_stats[S.intelligence] += 5f;
                holyMagic.base_stats[S.diplomacy] += 1f;
                holyMagic.base_stats[S.stewardship] += 2f;
                holyMagic.base_stats[S.warfare] -= 1f;
                holyMagic.special_effect_interval = 1f;
                AssetManager.traits.add(holyMagic);
                addTraitToLocalizedLibrary("en",holyMagic.id, "Holy magic","Holy magic is a gift from the god");
                addTraitToLocalizedLibrary("ru",holyMagic.id, "Святая магия","Святая магия - дар бога");
                PlayerConfig.unlockTrait(holyMagic.id);
        #endregion
       /* #region Греховность
                ActorTrait sinfulMagic = new ActorTrait();
                sinfulMagic.id = "sinful_magic";
                sinfulMagic.action_special_effect =  new WorldAction(TraitEffect.holySpell1);
                sinfulMagic.action_attack_target = new AttackAction(TraitEffect.holySpell2);
                //magicOfDeath.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfDeath.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                //sinfulMagic.action_death = new WorldAction(TraitEffect.holyDeath);
                sinfulMagic.path_icon = "ui/Extras/holyMagic";
                sinfulMagic.group_id = MagicTraitGroup.magic;
                sinfulMagic.can_be_given = true;
                sinfulMagic.base_stats[S.mod_health] += 2.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                sinfulMagic.base_stats[S.intelligence] += 5f;
                sinfulMagic.base_stats[S.diplomacy] -= 10f;
                sinfulMagic.base_stats[S.stewardship] += 10f;
                sinfulMagic.base_stats[S.warfare] += 25f;
                sinfulMagic.special_effect_interval = 1f;
                AssetManager.traits.add(sinfulMagic);
                addTraitToLocalizedLibrary("en",sinfulMagic.id, "Sinful magic","Sinful magic is a gift from the dark god");
                addTraitToLocalizedLibrary("ru",sinfulMagic.id, "Греховная магия","Греховная магия - дар темного бога");
                PlayerConfig.unlockTrait(sinfulMagic.id);
        #endregion*/
        #region Кровь
                ActorTrait magicOfBlood = new ActorTrait();
                magicOfBlood.id = "Blood Magic";
                magicOfBlood.action_special_effect =  new WorldAction(TraitEffect.spellOfBlood1);
                magicOfBlood.action_attack_target = new AttackAction(TraitEffect.spellOfBlood2);
                //magicOfDeath.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfDeath.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                magicOfBlood.action_death = new WorldAction(TraitEffect.spellOfBlood3);
                magicOfBlood.path_icon = "ui/Extras/magicOfBlood";
                magicOfBlood.group_id = MagicTraitGroup.magic;
                magicOfBlood.can_be_given = true;
                magicOfBlood.base_stats[S.mod_health] += 2.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                magicOfBlood.base_stats[S.intelligence] += 2f;
                magicOfBlood.base_stats[S.diplomacy] -= 3f;
                magicOfBlood.base_stats[S.stewardship] += 3f;
                magicOfBlood.base_stats[S.warfare] += 3f;
                magicOfBlood.special_effect_interval = 1f;
                AssetManager.traits.add(magicOfBlood);
                addTraitToLocalizedLibrary("en",magicOfBlood.id, magicOfBlood.id,"Blood Magic");
                addTraitToLocalizedLibrary("ru",magicOfBlood.id, "Магия Крови","Магия Крови");
                PlayerConfig.unlockTrait(magicOfBlood.id);
                #endregion
        #region Разум
                ActorTrait mindMagic = new ActorTrait();
                mindMagic.id = "MindMagic";
                mindMagic.action_special_effect =  new WorldAction(TraitEffect.spellOfMind1);
                mindMagic.action_attack_target = new AttackAction(TraitEffect.spellOfMind2);
                //magicOfDeath.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfDeath.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                //mindMagic.action_death = new WorldAction(TraitEffect.spellOfBlood3);
                mindMagic.path_icon = "ui/Extras/MindMagic";
                mindMagic.group_id = MagicTraitGroup.magic;
                mindMagic.can_be_given = true;
                mindMagic.base_stats[S.mod_health] += 0.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                mindMagic.base_stats[S.intelligence] += 10f;
                //mindMagic.base_stats[S.diplomacy] -= 30f;
                mindMagic.base_stats[S.stewardship] += 3f;
                mindMagic.base_stats[S.warfare] += 3f;
                mindMagic.special_effect_interval = 2f;
                AssetManager.traits.add(mindMagic);
                addTraitToLocalizedLibrary("en",mindMagic.id, "The magic of the mind","The magic of the mind");
                addTraitToLocalizedLibrary("ru",mindMagic.id, "Магия Разума","Магия Разума");
                PlayerConfig.unlockTrait(mindMagic.id);
                #endregion
        /*#region Иллюзии
                ActorTrait illusionmagic = new ActorTrait();
                illusionmagic.id = "illusion_magic";
                //illusionmagic.action_special_effect =  new WorldAction(TraitEffect.holySpell1);
                illusionmagic.action_attack_target = new AttackAction(TraitEffect.spellOfIllusion1);
                //magicOfDeath.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfDeath.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                //holyMagic.action_death = new WorldAction(TraitEffect.holyDeath);
                illusionmagic.path_icon = "ui/Extras/holyMagic";
                illusionmagic.group_id = MagicTraitGroup.magic;
                illusionmagic.can_be_given = true;
                /*illusionmagic.base_stats[S.mod_health] += 0.5f;
                //magicOfFire.base_stats[S.speed] -= 10f;
                illusionmagic.base_stats[S.intelligence] += 5f;
                illusionmagic.base_stats[S.diplomacy] += 1f;
                illusionmagic.base_stats[S.stewardship] += 2f;
                illusionmagic.base_stats[S.warfare] -= 1f;
                illusionmagic.special_effect_interval = 1f;
                AssetManager.traits.add(illusionmagic);
                addTraitToLocalizedLibrary("en",illusionmagic.id, "Illusion magic","");
                addTraitToLocalizedLibrary("ru",illusionmagic.id, "Магия Иллюзий","");
                PlayerConfig.unlockTrait(illusionmagic.id);
        #endregion*/
        #region Пространство
                ActorTrait MagicOfSpace = new ActorTrait();
                MagicOfSpace.id = "MagicOfSpace";
                MagicOfSpace.action_special_effect = new WorldAction(TraitEffect.SpellOfSpace);
                MagicOfSpace.action_attack_target = new AttackAction(TraitEffect.SpellOfSpace1);
                //magicOfDeath.action_get_hit = new GetHitAction(summonSpirit);
                //magicOfDeath.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                //magicOfBlood.action_death = new WorldAction(spellOfBlood3);
                MagicOfSpace.path_icon = "ui/Extras/MagicOfSpace";
                MagicOfSpace.group_id = MagicTraitGroup.magic;
                MagicOfSpace.can_be_given = true;
                MagicOfSpace.base_stats[S.knockback_reduction] += 1f;
                MagicOfSpace.base_stats[S.intelligence] += 3f;
                MagicOfSpace.base_stats[S.warfare] += 3f;
                MagicOfSpace.special_effect_interval = 1f;
                AssetManager.traits.add(MagicOfSpace);
                addTraitToLocalizedLibrary("en",MagicOfSpace.id, "Magic Of Space","Magic Of Space");
                addTraitToLocalizedLibrary("ru",MagicOfSpace.id, "Магия Пространства","Магия пространства");
                PlayerConfig.unlockTrait(MagicOfSpace.id);
                #endregion        
        #region Шаман
                ActorTrait shaman = new ActorTrait();
                shaman.id = "Shaman";
                //shaman.inherit = 30f;
                shaman.action_attack_target = new AttackAction(TraitEffect.SubjugationSpirit);
                shaman.action_get_hit = new GetHitAction(TraitEffect.summonSpirit);
                //shaman.action_attack_target = (AttackAction)Delegate.Combine(Vampire.action_attack_target, new AttackAction(bloodRestore));
                shaman.action_death = new WorldAction(ActionLibrary.mageSlayer);
                shaman.path_icon = "ui/Extras/Shaman";
                shaman.group_id = MagicTraitGroup.magic;
                shaman.can_be_given = true;
                shaman.base_stats[S.mod_health] += 0.1f;
                shaman.base_stats[S.speed] -= 1f;
                shaman.base_stats[S.intelligence] += 2f;
                shaman.base_stats[S.diplomacy] += 10f;
                //shaman.action_special_effect = (WorldAction)Delegate.Combine(shaman.action_special_effect, new WorldAction(SubjugationSpirit));
                //shaman.special_effect_interval = 10f;
                AssetManager.traits.add(shaman);
                addTraitToLocalizedLibrary("en",shaman.id, shaman.id,"Talking to spirits");
                addTraitToLocalizedLibrary("ru",shaman.id, "Шаман","Говорящий с духами");
                PlayerConfig.unlockTrait(shaman.id);
        #endregion
        #region Духи
                ActorTrait spirit = new ActorTrait();
                spirit.id = "Spirit";
                spirit.path_icon = "ui/Extras/Spirit";
                spirit.group_id = TraitGroup.special;
                spirit.can_be_given = false;
                string[] oppositeArrSpir = new string[] { "Lycanthropy", "AndroidPower2", "Vampirism" };
                spirit.oppositeArr = oppositeArrSpir;
                //spirit.action_special_effect = new WorldAction(following);
                //spirit.special_effect_interval = 1f;
                //spirit.action_attack_target = new AttackAction(SpiritInitiation);
                AssetManager.traits.add(spirit);
                addTraitToLocalizedLibrary("en",spirit.id, spirit.id,"Out of this world");
                addTraitToLocalizedLibrary("ru",spirit.id, "Дух","Не от мира сего");
                PlayerConfig.unlockTrait(spirit.id);

                ActorTrait chained = new ActorTrait();
                chained.id = "Subordinate";
                chained.path_icon = "ui/Extras/chained";
                chained.group_id = TraitGroup.special;
                chained.can_be_given = false;
                string[] oppositeArrChain = new string[] { "Lycanthropy", "AndroidPower2", "Vampirism" };
                chained.oppositeArr = oppositeArrChain;
                chained.action_special_effect = new WorldAction(FunctionalAction.following);
                chained.action_death = new WorldAction(FunctionalAction.following1);
                chained.special_effect_interval = 2f;
                //spirit.action_attack_target = new AttackAction(SpiritInitiation);
                AssetManager.traits.add(chained);
                addTraitToLocalizedLibrary("en",chained.id, chained.id,"Called and obeying");
                addTraitToLocalizedLibrary("ru",chained.id, "Подчиненный","Призванный и подчиняющийся");
                PlayerConfig.unlockTrait(chained.id);
        #endregion
        #region регенерация Ящеров
         ActorTrait LizardRegen = new ActorTrait();
         //LizardRegen.action_attack_target = new AttackAction(LicanAtackEffect1);
         LizardRegen.id = "Regeneration of the Lizard";
         LizardRegen.path_icon = "ui/Extras/LizardRegen";
         LizardRegen.action_get_hit = new GetHitAction(TraitEffect.LizardsRegen);
         LizardRegen.action_special_effect = new WorldAction(TraitEffect.LizardsRegen1);
         LizardRegen.group_id = MagicTraitGroup.fel;
         LizardRegen.can_be_given = true;
         LizardRegen.special_effect_interval = 1.1f;
         AssetManager.traits.add(LizardRegen);
         addTraitToLocalizedLibrary("en",LizardRegen.id, LizardRegen.id,"Lizards must be killed with one blow");
         addTraitToLocalizedLibrary("ru",LizardRegen.id, "Регенерация Ящера","Ящеров надо убивать одним ударом");
         PlayerConfig.unlockTrait("Regeneration of the Lizard");
#endregion
        #region Бездонный
         ActorTrait BottomlessSource = new ActorTrait();
         //LizardRegen.action_attack_target = new AttackAction(LicanAtackEffect1);
         BottomlessSource.id = "BottomlessSource";
         BottomlessSource.path_icon = "ui/Extras/BottomlessSource";
         //LizardRegen.action_get_hit = new GetHitAction(LizardsRegen);
         BottomlessSource.action_special_effect = new WorldAction(TraitEffect.BottomlessSources);
         BottomlessSource.group_id = MagicTraitGroup.magic;
         BottomlessSource.can_be_given = true;
         AssetManager.traits.add(BottomlessSource);
         addTraitToLocalizedLibrary("en",BottomlessSource.id, "BottomlessSource" ,"A magical trait for endless dampness");
         addTraitToLocalizedLibrary("ru",BottomlessSource.id, "Бездонный источник","Магическая черта на бесконечную сытость");
         PlayerConfig.unlockTrait("BottomlessSource");
#endregion
        #region Бог
            ActorTrait God = new ActorTrait();
            /*God.action_attack_target = new AttackAction(TraitEffect.God);*/
            God.action_special_effect = new WorldAction(TraitEffect.God);
            God.action_get_hit = new GetHitAction(TraitEffect.GetHitGod);
            God.action_death = new WorldAction(TraitEffect.DeathGod);
            God.birth = 0;
            God.inherit = 0;
            God.id = "God";
            God.path_icon = "ui/Extras/God";
            //God.base_stats[S.damage] -= 50f;
            //God.base_stats[S.health] -= 2000f;
            God.group_id = TraitGroup.special;
            God.can_be_given = true;
            God.base_stats[S.knockback_reduction] += 1f;
            God.base_stats[S.armor] += 77f;
            //God.opposite = "Demon Fighter";
            //Desecration.oppositeTraitMod -= 100000;
            God.can_be_removed = true;
            AssetManager.traits.add(God);
            addTraitToLocalizedLibrary("en",God.id, God.id,"Hello, I am God");
            addTraitToLocalizedLibrary("ru",God.id, "Бог","Здравствуй, я - бог");
            PlayerConfig.unlockTrait("God");
        #endregion
        #region Верующий
            ActorTrait Believer = new ActorTrait();
            /*Believer.action_attack_target = new AttackAction(TraitEffect.God);*/
            Believer.action_special_effect = new WorldAction(TraitEffect.SpreadingTheFaith);
            //Believer.action_get_hit = new GetHitAction(TraitEffect.GetHitGod);
            Believer.action_death = new WorldAction(TraitEffect.DeathBelive);
            Believer.birth = 0;
            Believer.inherit = 0;
            Believer.id = "Believer";
            Believer.path_icon = "ui/Extras/Believer";
            //God.base_stats[S.damage] += 50f;
            //God.base_stats[S.attack_speed] += 50f;
            Believer.group_id = TraitGroup.special;
            Believer.can_be_given = false;
            //God.opposite = "Demon Fighter";
            //Desecration.oppositeTraitMod -= 100000;
            Believer.can_be_removed = true;
            AssetManager.traits.add(Believer);
            addTraitToLocalizedLibrary("en",Believer.id, Believer.id,"He has a god");
            addTraitToLocalizedLibrary("ru",Believer.id, "Верующий","У него есть бог");
            PlayerConfig.unlockTrait("Believer");
        #endregion
#endregion       
        }


     
        public static void addTraitToLocalizedLibrary(string planguage, string id, string name, string description)
        {
            string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
            string templanguage;
            templanguage = language;
            if (templanguage != "ru" && templanguage != "en")
            {
                templanguage = "en";
            }
            if (planguage == templanguage)
            {
                Dictionary<string, string> localizedText = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
                localizedText.Add("trait_" + id, name);
                localizedText.Add("trait_" + id + "_info", description);
            }
        }
    }
}