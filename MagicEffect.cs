using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using ai;
using ai.behaviours;
using System.Reflection;
using Newtonsoft.Json;
namespace Magic
{
    class MagicEffect
    {
        public static void init()
        {
            BehaviourTaskActor panic_move = new BehaviourTaskActor();
            panic_move.id = "panic_move";
            panic_move.ignoreFightCheck = true;
            //BehaviourTaskActor panicmove = panic_move;
            //AssetManager.task_library = panic_move;
            AssetManager.tasks_actor.add(panic_move);
            //panic_move.addBeh((BehaviourActionActor) new BehEndJob());
            panic_move.addBeh((BehaviourActionActor) new BehFindRandomTile8Directions());
            panic_move.addBeh((BehaviourActionActor) new BehGoToTileTarget(){
                walkOnWater = false,
                walkOnBlocks = false
            });

            StatusEffect burning = AssetManager.status.get("burning");
            burning.opposite_traits.Add("Fire Magic");

            StatusEffect frozen = AssetManager.status.get("frozen");
            frozen.opposite_traits.Add("Water Magic");


            StatusEffect cough = AssetManager.status.get("cough");
            cough.opposite_traits.Add("The Magic of Life");
            cough.opposite_traits.Add("The Magic of Death");

            StatusEffect ash_fever = AssetManager.status.get("ash_fever");
            ash_fever.opposite_traits.Add("The Magic of Life");
           

            StatusEffect fireEnhancement = new StatusEffect();
            fireEnhancement.id = "fireEnhancement";
            fireEnhancement.duration = 20.0f;
            fireEnhancement.base_stats[S.armor] += 10f;
            //fireEnhancement.base_stats[S.attack_speed] += 80f;
            fireEnhancement.base_stats[S.mod_damage] += 0.5f;
            fireEnhancement.base_stats[S.damage] += 100f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            fireEnhancement.animated = false;
            fireEnhancement.path_icon = "ui/effect_icons/fireEnhancement";
            fireEnhancement.action_interval = 1f;
            fireEnhancement.action = new WorldAction(sparksOfFire);
            fireEnhancement.action_get_hit = new GetHitAction(burningAction);
            fireEnhancement.description = "status_description_fireEnhancement";
            fireEnhancement.name = "status_title_fireEnhancement";
            addTraitToLocalizedLibrary("ru", fireEnhancement.name, fireEnhancement.description, "Усиление Огня", "Огонь пожирает его врагов");
            addTraitToLocalizedLibrary("en", fireEnhancement.name, fireEnhancement.description, "Fire Enhancement", "The fire devours his enemies");
            AssetManager.status.add(fireEnhancement);

            StatusEffect fear = new StatusEffect();
            fear.id = "fear";
            fear.duration = 20.0f;
            fear.base_stats[S.speed] += 20f;
            //fireEnhancement.base_stats[S.attack_speed] += 80f;
            fear.base_stats[S.mod_damage] -= 0.5f;
            fear.base_stats[S.range] -= 100f;
            fear.base_stats[S.damage] -= 5f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            fear.animated = false;
            fear.path_icon = "ui/effect_icons/fear";
            fear.action_interval = 0.001f;
            fear.action = new WorldAction(Fear);
            //fear.action_get_hit = new GetHitAction(burningAction);
            fear.description = "status_description_fear";
            fear.name = "status_title_fear";
            fear.opposite_status.Add("Disorientation");
            fear.opposite_status.Add("oblivion");
            addTraitToLocalizedLibrary("ru", fear.name, fear.description, "Паника", "Он убегает со всех ног");
            addTraitToLocalizedLibrary("en", fear.name, fear.description, "Panic", "He's running away as fast as he can");
            AssetManager.status.add(fear);

            StatusEffect oblivion = new StatusEffect();
            oblivion.id = "oblivion";
            oblivion.duration = 5.0f;
            //oblivion.base_stats[S.speed] += 20f;
            //fireEnhancement.base_stats[S.attack_speed] += 80f;
            oblivion.base_stats[S.armor] -= 5f;
            oblivion.base_stats[S.intelligence] -= 5f;
            oblivion.base_stats[S.range] -= 100f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            oblivion.animated = false;
            oblivion.path_icon = "ui/effect_icons/oblivion";
            oblivion.action_interval = 0.002f;
            oblivion.action = new WorldAction(Oblivion);
            //fear.action_get_hit = new GetHitAction(burningAction);
            oblivion.description = "status_description_oblivion";
            oblivion.name = "status_title_oblivion";
            oblivion.opposite_status.Add("Disorientation");
            oblivion.opposite_status.Add("fear");
            addTraitToLocalizedLibrary("ru", oblivion.name, oblivion.description, "Забвение", "Что... что я здесь делаю?");
            addTraitToLocalizedLibrary("en", oblivion.name, oblivion.description, "Oblivion", "What... What am I doing here?");
            AssetManager.status.add(oblivion);

            StatusEffect Disorientation = new StatusEffect();
            Disorientation.id = "Disorientation";
            Disorientation.duration = 20.0f;
            Disorientation.base_stats[S.speed] += 5f;
            //fireEnhancement.base_stats[S.attack_speed] += 80f;
            Disorientation.base_stats[S.mod_damage] -= 0.5f;
            Disorientation.base_stats[S.range] -= 100f;
            Disorientation.base_stats[S.damage] -= 5f;
            //Disorientation.base_stats[S.critical] -= 100f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            Disorientation.animated = false;
            Disorientation.path_icon = "ui/effect_icons/Disorientation";
            Disorientation.action_interval = 0.02f;
            Disorientation.action = new WorldAction(Disorientations);
            //fear.action_get_hit = new GetHitAction(burningAction);
            Disorientation.description = "status_description_Disorientation";
            Disorientation.name = "status_title_Disorientation";
            Disorientation.opposite_status.Add("oblivion");
            Disorientation.opposite_status.Add("fear");
            addTraitToLocalizedLibrary("ru", Disorientation.name, Disorientation.description, "Дезориентация", "Он бежит навстречу опасности");
            addTraitToLocalizedLibrary("en", Disorientation.name, Disorientation.description, "Disorientation", "He is running towards danger");
            AssetManager.status.add(Disorientation);

            StatusEffect waterEnhancement = new StatusEffect();
            waterEnhancement.id = "waterEnhancement";
            waterEnhancement.duration = 60.0f;
            waterEnhancement.base_stats[S.health] += 100f;
            //waterEnhancement.base_stats[S.attack_speed] += 80f;
            waterEnhancement.base_stats[S.mod_health] += 0.5f;
            waterEnhancement.base_stats[S.armor] += 5f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            waterEnhancement.animated = false;
            waterEnhancement.path_icon = "ui/effect_icons/waterEnhancement";
            waterEnhancement.action_interval = 1f;
            waterEnhancement.action = new WorldAction(sparksOfWater);
            waterEnhancement.action = new WorldAction(waterEnhancementAction);
            //waterEnhancement.action_get_hit = new GetHitAction(burningAction);
            waterEnhancement.description = "status_description_waterEnhancement";
            waterEnhancement.name = "status_title_waterEnhancement";
            addTraitToLocalizedLibrary("ru", waterEnhancement.name, waterEnhancement.description, "Усиление Воды", "Вода питает его жизнь");
            addTraitToLocalizedLibrary("en", waterEnhancement.name, waterEnhancement.description, "Water Enhancement", "Water feeds his life");
            AssetManager.status.add(waterEnhancement);

            StatusEffect airEnhancement = new StatusEffect();
            airEnhancement.id = "airEnhancement";
            airEnhancement.duration = 30.0f;
            airEnhancement.base_stats[S.speed] += 100f;
            airEnhancement.base_stats[S.attack_speed] += 80f;
            airEnhancement.base_stats[S.mod_speed] += 0.5f;
            airEnhancement.base_stats[S.armor] += 2f;
            airEnhancement.base_stats[S.dodge] += 5f;
            airEnhancement.base_stats[S.range] += 10f;
            airEnhancement.base_stats[S.knockback_reduction] -= 0.15f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            airEnhancement.animated = false;
            airEnhancement.action_interval = 1f;
            airEnhancement.action = new WorldAction(sparksOfAir);
            airEnhancement.path_icon = "ui/effect_icons/airEnhancement";
            //airEnhancement.action = new WorldAction(airEnhancementAction);
            //waterEnhancement.action_get_hit = new GetHitAction(burningAction);
            airEnhancement.description = "status_description_airEnhancement";
            airEnhancement.name = "status_title_airEnhancement";
            addTraitToLocalizedLibrary("ru", airEnhancement.name, airEnhancement.description, "Усиление Воздуха", "Воздух делает жизнь легче");
            addTraitToLocalizedLibrary("en", airEnhancement.name, airEnhancement.description, "Air Enhancement", "Air makes life easier");
            AssetManager.status.add(airEnhancement);

            StatusEffect earthEnhancement = new StatusEffect();
            earthEnhancement.id = "earthEnhancement";
            earthEnhancement.duration = 60.0f;
            earthEnhancement.base_stats[S.speed] -= 20f;
            earthEnhancement.base_stats[S.attack_speed] -= 10f;
            //earthEnhancement.base_stats[S.mod_speed] += 0.5f;
            earthEnhancement.base_stats[S.armor] += 70f;
            earthEnhancement.base_stats[S.knockback_reduction] += 100f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            earthEnhancement.animated = false;
            earthEnhancement.action_interval = 1f;
            earthEnhancement.action = new WorldAction(sparksOfEarth);
            earthEnhancement.path_icon = "ui/effect_icons/earthEnhancement";
            //earthEnhancement.action = new WorldAction(earthEnhancementAction);
            //waterEnhancement.action_get_hit = new GetHitAction(burningAction);
            earthEnhancement.description = "status_description_earthEnhancement";
            earthEnhancement.name = "status_title_earthEnhancement";
            addTraitToLocalizedLibrary("ru", earthEnhancement.name, earthEnhancement.description, "Усиление Земли", "Земля защищает свое дитя");
            addTraitToLocalizedLibrary("en", earthEnhancement.name, earthEnhancement.description, "Earth Enhancement", "The earth protects its child");
            AssetManager.status.add(earthEnhancement);

            StatusEffect curse = new StatusEffect();
            curse.id = "curse";
            curse.duration = 60.0f;
            curse.base_stats[S.mod_speed] -= 10f;
            curse.base_stats[S.attack_speed] -= 60f;
            //earthEnhancement.base_stats[S.mod_speed] += 0.5f;
            curse.base_stats[S.armor] -= 50f;
            curse.base_stats[S.mod_health] -= 0.6f;
            curse.base_stats[S.mod_damage] -= 0.5f;
            curse.base_stats[S.knockback_reduction] += 100f;
            curse.base_stats[S.max_age] -= 30f;
            //fireEnhancement.base_stats[S.knockback_reduction] += 100.0f;
            curse.animated = false;
            curse.action_interval = 1f;
            curse.action = new WorldAction(sparksOfDeath);
            curse.opposite_traits.Add("The Magic of Death");
            curse.path_icon = "ui/effect_icons/curse";
            //earthEnhancement.action = new WorldAction(earthEnhancementAction);
            //waterEnhancement.action_get_hit = new GetHitAction(burningAction);
            curse.description = "status_description_curse";
            curse.name = "status_title_curse";
            addTraitToLocalizedLibrary("ru", curse.name, curse.description, "Проклятье Смерти", "Она тянет к нему свои руки");
            addTraitToLocalizedLibrary("en", curse.name, curse.description, "The Curse of Death", "She holds out her hands to him");
            AssetManager.status.add(curse);

            
            
            
            //panic_move.addBeh((BehaviourActionActor) new BehRandomWait(1f, 6f));

            loadCustomEffect();
        }
        private static bool waterEnhancementAction(BaseSimObject pTarget, WorldTile pTile)
        {
            FunctionalAction.Regen(pTarget, 30);
            //pTarget.a.restoreHealth(30);
            pTarget.a.finishStatusEffect("burning");
            return true;
        }

        private static bool Fear(BaseSimObject pTarget, WorldTile pTile)
        {
            //pTarget.a.giveInventoryResourcesToCity();
            //pTarget.a.ai.endJob();
            //pTarget.a.clearBeh();
            //pTarget.a.ai.task = pTarget.a.ai.get("panic_move");
            //pTarget.a.has_attack_target = false;
            if(pTarget.a.ai.job!=null){
                pTarget.a.endJob();
            }
            if (pTarget.a.ai.task==null || (pTarget.a.ai.task.id != "panic_move" && 
            pTarget.a.ai.task.id != "swim_to_island" &&
            pTarget.a.ai.task.id != "move_from_block")){
                //pTarget.a.endJob();
                //pTarget.a.cancelAllBeh();
                //pTarget.a.ai.task = AssetManager.tasks_actor.get("panic_move");
                //pTarget.a.ai.action=pTarget.a.ai.task.get(1);
                //pTarget.a.ai.task = pTarget.a.ai.task_library.get("panic_move");
                pTarget.a.ai.setTask("panic_move");
                //FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Ground, false, false);
                //pTarget.a.timer_action = 0.2f;
                }
            //else if (pTarget.a.ai.action_index==2)
            return true;
        }
        private static bool Oblivion(BaseSimObject pTarget, WorldTile pTile)
        {
            pTarget.a.cancelAllBeh();
            return true;
        }
        private static bool Disorientations(BaseSimObject pTarget, WorldTile pTile)
        {

            pTarget.a.clearAttackTarget();
                if (pTarget.a.asset.dieInLava)
                    if (!FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Lava))
                        if (pTarget.a.asset.dieOnBlocks)
                            if (!FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Block))
                                if (!FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Ocean))
                                    FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Ground);
                else if (pTarget.a.asset.dieOnBlocks)
                    if (!FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Block))
                        if (!FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Ocean))
                            FunctionalAction.find_water_lava(pTarget.a, TileLayerType.Ground);
            //FunctionalAction.Regen(pTarget, 30);
            //pTarget.a.restoreHealth(30);
            //pTarget.a.finishStatusEffect("burning");
            return true;
        }
        private static bool sparksOfDeath(BaseSimObject pTarget, WorldTile pTile)
        {
            int pDamage = (int) ((double) pTarget.getMaxHealth() * 0.099999997764825821) + 1;
            pTarget.getHit((float) pDamage, pType: AttackType.AshFever);
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#661b7a"));
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#2cc930"));
            return true;
        }
        private static bool sparksOfFire(BaseSimObject pTarget, WorldTile pTile)
        {
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#F27F3D"));
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#D95032"));
            return true;
        }
        private static bool sparksOfEarth(BaseSimObject pTarget, WorldTile pTile)
        {
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#582308"));
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#99380f"));
            return true;
        }
        private static bool sparksOfAir(BaseSimObject pTarget, WorldTile pTile)
        {
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#dffcea"));
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#d1d1d1"));
            return true;
        }
        private static bool sparksOfWater(BaseSimObject pTarget, WorldTile pTile)
        {
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#4154d5"));
            pTarget.a.spawnParticle((Color32)Toolbox.makeColor("#223199"));
            return true;
        }

        private static bool burningAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget != null && pTarget.isActor())
                pTarget.a.addStatusEffect("burning");
            return true;
        }
        public static void loadCustomEffect()
        {
            //please, this took me a whole day and the entire modder team to help me with this
            var effect = AssetManager.effects_library.add(new EffectAsset
            {
                id = "fx_YOYO_effect",
                use_basic_prefab = true,
                show_on_mini_map = true,
                sprite_path = "effects/antimatterEffect",
                sound_launch = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb",
                sorting_layer_id = "Objects"
            });
            World.world.stackEffects.CallMethod("add", effect);

            var effectCustomEffect = AssetManager.effects_library.add(new EffectAsset
            {
                id = "fx_CustomTeleport_effect",
                use_basic_prefab = true,
                show_on_mini_map = true,
                sprite_path = "effects/fx_tele_minato",
                sorting_layer_id = "Objects"
            });
            World.world.stackEffects.CallMethod("add", effectCustomEffect);

            Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHHHHHH WORKS PLEASE");
        }
        public static void addTraitToLocalizedLibrary(string planguage, string name, string desc, string id, string description)
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
                localizedText.Add(name, id);
                localizedText.Add(desc, description);
            }
           
        }
    }
}