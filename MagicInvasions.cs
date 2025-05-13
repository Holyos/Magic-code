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
    class MagicInvasions
    {
        public static void init()
        {
            DisasterAsset spirit_tornado = new DisasterAsset();
            spirit_tornado.id = "spirit_tornado";
            spirit_tornado.rate = 3;
            spirit_tornado.chance = 1f;
            spirit_tornado.min_world_cities = 3;
            spirit_tornado.world_log = "worldlog_disaster_tornado";
            spirit_tornado.world_log_icon = "iconTornado";
            spirit_tornado.min_world_population = 100;
            spirit_tornado.type = DisasterType.Nature;

            spirit_tornado.spawn_asset_unit = "spirit_tornado";
            spirit_tornado.units_min = 1;
            spirit_tornado.units_max = 1;
            //spirit_tornado.ages_forbid.Add(S.age_hope);
            spirit_tornado.ages_allow.Add(S.age_chaos);
            spirit_tornado.ages_allow.Add(S.age_tears);
            spirit_tornado.ages_allow.Add(S.age_ash);
            //spirit_tornado.ages_allow.Add(S.age_chaos);
            spirit_tornado.ages_allow.Add(S.age_wonders);
            spirit_tornado.ages_allow.Add(S.age_moon);
            spirit_tornado.action = new DisasterAction(AssetManager.disasters.spawnBiomass);
			AssetManager.disasters.add(spirit_tornado);
        }
    }
}