using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ReflectionUtility;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Magic{
    [ModEntry]
    class Main : MonoBehaviour{
        #region
        public static Main instance;
#region Данные
        //подчиненный-хозяин
        public static Dictionary<Actor, Actor> listOfTamedBeasts = new Dictionary<Actor, Actor>();
        // количество подчиненных
        public static Dictionary<Actor, int> listOfOwners = new Dictionary<Actor, int>();
        //количество мертвых тел
        public static Dictionary<Actor, int> deadBodies = new Dictionary<Actor, int>();
        //колличество духов
        public static Dictionary<Actor, HashSet<int>> countSpirit = new Dictionary<Actor, HashSet<int>>();
        //колличество великих дубов
        public static Dictionary<Kingdom, List<Building>> countGreatTree = new Dictionary<Kingdom, List<Building>>();

        // верующий-бог
        public static Dictionary<Actor, Actor> Faith = new Dictionary<Actor, Actor>();
        // число паствы
        public static Dictionary<Actor, int> Сongregation = new Dictionary<Actor, int>();
        //расы с богами
        public static Dictionary<string, Actor> godID = new Dictionary<string, Actor>();
        //святые маги
        public static Dictionary<Actor, int> Saint = new Dictionary<Actor, int>();

        // кровавое усиление
        public static Dictionary<BaseSimObject, BaseStats> listOfStats = new Dictionary<BaseSimObject, BaseStats>();
        //Королевство заразившего
        public static Dictionary<Actor, Kingdom> listOfKingdoms = new Dictionary<Actor, Kingdom>();

        // список архидемонов
        public static List<Actor> DemonKing = new List<Actor>();
        //создание здания высшими демонами 
        public static Dictionary<Actor, Building> listOfBuilding = new Dictionary<Actor, Building>();

        //список использованных на прошлом шаге торнадо
        public static List<Actor> bound_tornado = new List<Actor>();

        //настройки
        public static SavedSettings savedSettings = new SavedSettings();

        //юнит-животное в которое превращается
        public static Dictionary<Actor,string> totems = new Dictionary<Actor,string>();
        //животные для превращения
        public static List<string> Animals = new List<string>
        {SA.snake,
        SA.monkey,
        SA.bear,
        SA.fox,
        SA.hyena,
        SA.wolf,
        SA.acid_blob,
        SA.buffalo,
        SA.cat,
        SA.cow,
        SA.crocodile,
        SA.dog,
        SA.penguin,
        SA.rabbit,
        SA.ratKing,
        SA.rhino,
        SA.river_turtle,
        SA.turtle};
        //уникальные здания
        public MagicBuilds magicBuilds = new MagicBuilds();
        //магический фон
        public static List<string> Magic = new List<string>(){
            "Fire Magic", 
            "Water Magic", 
            "Air Magic", 
            "Earth Magic", 
            "The Magic of Life", 
            "The Magic of Death", 
            "MagicOfSpace", 
            "Shaman",
            "MindMagic"};
        public static Dictionary<string, int> listOfEnergy = new Dictionary<string, int>(){
            {"Fire energy",0},
            {"Water energy",0},
            {"Earth energy",0},
            {"Air energy",0},
            {"Life energy",0},
            {"Death energy",0}
        };
#endregion
        public MagicRaceLibrary MagicRaceLibrary = new MagicRaceLibrary();
        
        
        #region настройки
        public static float magicBirth = 3f;
        public static float magicInherit = 30f;
        public static float BloodAge = 300f;
        public static float DefilerBirth = 0f;
        public static float DefilerInherit = 0f;
        public static float DemonFighterBirth = 0f;
        public static float DemonFighterInherit = 10f;
        public static float spiritInitiations = 5f;
        public static int demon_gate = 5;
        public static bool NewMagicOfDeath;
        public static bool spawn_race;
        public static bool spawn_demon;
        public static bool InvasionDemons;
        public static bool spirit_spawn;
        #endregion
        #endregion
        internal const string id = "androlg.mods.worldbox.magic";
        internal static Harmony harmony;
        internal static Dictionary<string, UnityEngine.Object> modsResources;
        public List<string> addRaces = new List<string>(){
            "vampire","illithiiry"};
        public static List<string> Races = new List<string>(){
            "goblin", 
            "lizard", 
            "android", 
            "darkelve", 
            "beastmen", 
            "gnome", 
            "angel", 
            "demonic", 
            "japaneses", 
            "ancientchina",
            "vampire",
            "human",
            "orc",
            "elf",
            "dwarf",
            "illithiiry"};
            
        
        public BuildingLibrary buildingLibrary = new BuildingLibrary();
        public const string mainPath = "Mods/magic";
        private static string correctSettingsVersion = "0.0.4";

        void Awake(){
           /*loadSettings();
            Dictionary<string, ScrollWindow> allWindows = (Dictionary<string, ScrollWindow>)Reflection.GetField(typeof(ScrollWindow), null, "allWindows");
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
            allWindows["inspect_unit"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "village");
            allWindows["village"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "debug");
            allWindows["debug"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "kingdom");
            allWindows["kingdom"].gameObject.SetActive(false);*/
            modsResources = Reflection.GetField(typeof(ResourcesPatch), null, "modsResources") as Dictionary<string, UnityEngine.Object>;
            harmony = new Harmony(id);
            Patches.init();
            MagicNames.init();
            MagicKingdoms.init();
            MagicEffects.init();
            MagicEffect.init();
            MagicTraitGroup.init();
            //FelTile.init();
            //NewUI.init();
            //WindowManager.init();
            MagicGuns.init();
            MagicBuilds.init();
            //PatchKevin.init();
            buildingLibrary.init();
            MagicRaces.init();
            MagicRaceLibrary.init();
            MagicSpells.init();
            MagicUnitys.init();
            //MagicInvasions.init();
            MagicTab.init();
            MagicButtons.init();
            
            var dictItems = Reflection.GetField(typeof(ActorAnimationLoader), null, "dictItems") as Dictionary<string, Sprite>;
            ActorAnimationLoader.loadAnimationBoat($"boat_fishing");
            instance = this;
        }
        IEnumerator Start()
        {
            loadSettings();
            Dictionary<string, ScrollWindow> allWindows = (Dictionary<string, ScrollWindow>)Reflection.GetField(typeof(ScrollWindow), null, "allWindows");
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
            allWindows["inspect_unit"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "village");
            allWindows["village"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "debug");
            allWindows["debug"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "kingdom");
            allWindows["kingdom"].gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            /*modsResources = Reflection.GetField(typeof(ResourcesPatch), null, "modsResources") as Dictionary<string, UnityEngine.Object>;
            harmony = new Harmony(id);
            Patches.init();
            //FelTile.init();
            MagicNames.init();
            MagicKingdoms.init();
            MagicEffects.init();
            //PatchKevin.init();
            MagicEffect.init();
            MagicTraitGroup.init();*/
            //NewUI.init();
            
            WindowManager.init();
            MagicTraits.init();
            NaturalBirth.init();
            //MagicEnergy.init();
            GodMagic.init();
            /*MagicTraits.init();
            MagicGuns.init();
            MagicBuilds.init();
            buildingLibrary.init();
            MagicRaces.init();
            MagicRaceLibrary.init();
            MagicSpells.init();
            MagicUnitys.init();
            //MagicInvasions.init();
            MagicTab.init();
            MagicButtons.init();
            NaturalBirth.init();
            var dictItems = Reflection.GetField(typeof(ActorAnimationLoader), null, "dictItems") as Dictionary<string, Sprite>;
            ActorAnimationLoader.loadAnimationBoat($"boat_fishing");
            instance = this;*/
            Harmony.CreateAndPatchAll(typeof(MagicRaceLibrary));
        }
        public static void saveSettings(SavedSettings previousSettings = null)
        {
            if (previousSettings != null)
            {
                foreach(FieldInfo field in typeof(SavedSettings).GetFields())
                {
                    field.SetValue(savedSettings, field.GetValue(previousSettings));
                }
                savedSettings.settingVersion = correctSettingsVersion;
            }
            string json = JsonConvert.SerializeObject(savedSettings, Formatting.Indented);
            File.WriteAllText($"{Core.NCMSModsPath}/MagicStatsWindow.json", json);
        }
        public static bool loadSettings()
        {
            if (!File.Exists($"{Core.NCMSModsPath}/MagicStatsWindow.json"))
            {
                saveSettings();
                return false;
            }
            string data = File.ReadAllText($"{Core.NCMSModsPath}/MagicStatsWindow.json");
            SavedSettings loadedData = null;
            try{
                loadedData = JsonConvert.DeserializeObject<SavedSettings>(data);
            }catch{
                saveSettings();
                return false;
            }
            if (loadedData.settingVersion != correctSettingsVersion)
            {
                saveSettings(loadedData);
                return false;
            }
            savedSettings = loadedData;
            return true;
        }
        public static void modifyMagicOption(string key, string value, bool active, UnityAction call = null)
        {
            Main.savedSettings.magicOptions[key] = new InputOption{active = active, value = value};
            saveSettings();
            if (call != null)
            {
                call.Invoke();
            }
        }
        public static void modifyGodOption(string key, string value, bool active, UnityAction call = null)
        {
            Main.savedSettings.GodMagicOptions[key] = new InputOption{active = active, value = value};
            saveSettings();
            if (call != null)
            {
                call.Invoke();
            }
        }
        /*public static void modifyMultipleInputOption(string key, string valueKey, string value)
        {
            savedSettings.multipleInputOptions[key][valueKey].value = value;
            saveSettings();
        }*/
        public static void modifyBoolOption(string key, bool value, UnityAction call = null)
        {
            Main.savedSettings.boolOptions[key] = value;
            saveSettings();
            if (call != null)
            {
                call.Invoke();
            }
        }
        public static void updateDirtyStats()
        {
            foreach(Actor unit in World.world.units)
            {
                unit.setStatsDirty();
            }
        }
    }
}