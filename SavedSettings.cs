using System;
using System.Collections;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using ReflectionUtility;

namespace Magic
{
    [Serializable]
    public class SavedSettings
    {
        public string settingVersion = "0.0.4";
        public Dictionary<string, InputOption> magicOptions = new Dictionary<string, InputOption>
        {
            {"MagicBirth%", new InputOption{active = true, value = "3" }},
            {"MagicInherit%", new InputOption{active = true, value = "35" }},
            {"AgeOfBloodMagic", new InputOption{active = true, value = "300" }},
            {"SpiritInitiation%", new InputOption{active = true, value = "5" }},
            {"DefilerBirth%", new InputOption{active = true, value = "0" }},
            {"DefilerInherit%", new InputOption{active = true, value = "0" }},
            {"DemonFighterBirth%", new InputOption{active = true, value = "0" }},
            {"DemonFighterInherit%", new InputOption{active = true, value = "10" }},
        };
        public Dictionary<string, bool> boolOptions = new Dictionary<string, bool>
        {
            {"NaturalBirth", true},
            {"SpiritBirth", false},
            {"DemonicBuild", false},
            {"DeathMagic", true},
            {"InvasionDemons", false}
        };

        public Dictionary<string, InputOption> GodMagicOptions = new Dictionary<string, InputOption>
        {
                {"Rod Upper (0-1000)",  new InputOption{active = false, value = "0"}},
                {"Rod Lower (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Rod Left (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Rod Right (0-1000)",  new InputOption{active = true, value = "0"}},

                {"Svarog Upper (0-1000)",  new InputOption{active = false, value = "0"}},
                {"Svarog Lower (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Svarog Left (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Svarog Right (0-1000)",  new InputOption{active = true, value = "0"}},

                {"Perun Upper (0-1000)",  new InputOption{active = false, value = "0"}},
                {"Perun Lower (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Perun Left (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Perun Right (0-1000)",  new InputOption{active = true, value = "0"}},

                {"Koschey Upper (0-1000)",  new InputOption{active = false, value = "0"}},
                {"Koschey Lower (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Koschey Left (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Koschey Right (0-1000)",  new InputOption{active = true, value = "0"}},

                {"Zhiva Upper (0-1000)",  new InputOption{active = false, value = "0"}},
                {"Zhiva Lower (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Zhiva Left (0-1000)",  new InputOption{active = true, value = "0"}},
                {"Zhiva Right (0-1000)",  new InputOption{active = true, value = "0"}},
        };
    }
    public class InputOption
    {
        public bool active = true;
        public string value;
    }
    public class BoundOption
    {
        public string upper_bound = "0";
        public string lower_bound = "0";
        public string left_bound = "0";
        public string right_bound = "0";
        public bool active = false;
    }
}