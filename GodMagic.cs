using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ReflectionUtility;
namespace Magic
{
    class GodMagic : MonoBehaviour
    {
        public static GodMagic instance;
        public static Coroutine currentCoroutine;
        public static bool coroutineIsRunning = false;
        public static void init()
        {
            //instance = new GameObject("GodMagicInstance").AddComponent<GodMagic>();
            //loadHeroicStats();
            //instance.startSpells();

            var bound_tornado = AssetManager.actor_library.clone("bound_tornado", SA.tornado);
            bound_tornado.ignoreJobs = true;
            bound_tornado.has_ai_system = false;
            bound_tornado.canBeMovedByPowers = false;
            bound_tornado.base_stats[S.scale] = 0.1f;
            bound_tornado.base_stats[S.health] = 1f;
            AssetManager.actor_library.add(bound_tornado);
            AssetManager.actor_library.CallMethod("loadShadow", bound_tornado);
        }

        public void startSpells()
        {
            Debug.Log("Hello");
            coroutineIsRunning = true;
            currentCoroutine = StartCoroutine(this.SpellGod());
        }

        IEnumerator SpellGod()
        {
            while(coroutineIsRunning)
            {
                //ActivatedSpell("fire");
                yield return new WaitForSeconds(5);
                //ActivatedSpell("fire");
            }
        }
        public static Dictionary <string, bounds> GodBounds = new Dictionary<string, bounds> {
            {"Rod", new bounds{}}
        };
        public static bool ActivatedSpell (string spell, string power, int cost, string type)
        {
            if (activeSpell(spell))
                    {
                        spellInSquad((int)(MapBox.width*int.Parse(Main.savedSettings.GodMagicOptions[$"{spell} Left (0-1000)"].value)/1000),
                        (int)(MapBox.width*int.Parse(Main.savedSettings.GodMagicOptions[$"{spell} Right (0-1000)"].value)/1000),
                        (int)(MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions[$"{spell} Lower (0-1000)"].value)/1000),
                        (int)(MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions[$"{spell} Upper (0-1000)"].value)/1000),
                        power);
                        Main.listOfEnergy[type + " energy"] -= cost;
                    }
            return true;
        }
        public static bool insquad (int x, int y)
        {
            if (x<GodBounds["Rod"].right_bound &&
            x>GodBounds["Rod"].left_bound &&
            y<GodBounds["Rod"].upper_bound &&
            y>GodBounds["Rod"].lower_bound)
                return true;
            else
                return false;
        }
        public static bool activeSpell (string spell)
        {
            if (Main.savedSettings.GodMagicOptions[$"{spell} Lower (0-1000)"].active &&
                Main.savedSettings.GodMagicOptions[$"{spell} Upper (0-1000)"].active &&
                Main.savedSettings.GodMagicOptions[$"{spell} Left (0-1000)"].active &&
                Main.savedSettings.GodMagicOptions[$"{spell} Right (0-1000)"].active)
                return true;
            else 
                return false;
        }
        public static void tornadoInBound (int x1, int x2, int y1, int y2)
        {
            for (int i = 0; i < Main.bound_tornado.Count; i++)
            {
                if (Main.bound_tornado[i].isAlive())
                    Main.bound_tornado[i].killHimself(true, AttackType.Other, false, false, false);
            }
            Main.bound_tornado.RemoveRange(0,Main.bound_tornado.Count);
            
            if (!activeSpell("Perun"))
                //Main.bound_tornado
                return;
            //bo

            for (int indexx = x1; indexx<(int)((x2)); ++indexx)
            {
                int y = y1;
                if (indexx >= 0 && indexx < MapBox.width && y >= 0 && y < MapBox.height)
                {
                    WorldTile tileSimple = MapBox.instance.GetTileSimple(indexx, y);
                    //MapBox.spawnLightningBig(tileSimple,0.01f);
                    
                    Actor tornado = World.world.units.createNewUnit("bound_tornado", tileSimple, 0f);
                    Main.bound_tornado.Add(tornado);

                }
                y = y2;
                if (indexx >= 0 && indexx < MapBox.width && y >= 0 && y < MapBox.height)
                {
                    WorldTile tileSimple = MapBox.instance.GetTileSimple(indexx, y);
                    //MapBox.spawnLightningBig(tileSimple,0.05f);
                    Actor tornado = World.world.units.createNewUnit("bound_tornado", tileSimple, 0f);
                    Main.bound_tornado.Add(tornado);
                }
            }

            for (int indexy = y1; indexy<(int)((y2)); ++indexy)
            {
                int indexx = x1;
                if (indexx >= 0 && indexx < MapBox.width && indexy >= 0 && indexy < MapBox.height)
                {
                    WorldTile tileSimple = MapBox.instance.GetTileSimple(indexx, indexy);
                    //MapBox.spawnLightningBig(tileSimple,0.2f);
                    Actor tornado = World.world.units.createNewUnit("bound_tornado", tileSimple, 0f);
                    Main.bound_tornado.Add(tornado);
                }
                indexx = x2;
                if (indexx >= 0 && indexx < MapBox.width && indexy >= 0 && indexy < MapBox.height)
                {
                    WorldTile tileSimple = MapBox.instance.GetTileSimple(indexx, indexy);
                    //MapBox.spawnLightningBig(tileSimple,0.1f);
                    Actor tornado = World.world.units.createNewUnit("bound_tornado", tileSimple, 0f);
                    Main.bound_tornado.Add(tornado);
                }
            }
        }
        public static void spellInSquad (int x1, int x2, int y1, int y2, string effect)
        {
            int x=x1;
            int y=y1;
            for (int indexx = x1; indexx<(int)((x2)); ++indexx)
            {
                
                for (int indexy = y1; indexy<(int)((y2)); ++indexy)
                {
                    /*System.Random rndx=new System.Random();
                    x = x1 + indexx * 20 + rndx.Next(20);
                    System.Random rndy=new System.Random();
                    y = y1 + indexy * 20 + rndy.Next(20);*/
                    if (x >= 0 && x < MapBox.width && y >= 0 && y < MapBox.height && Toolbox.randomChance(0.05f))
                    {
                        WorldTile tileSimple = MapBox.instance.GetTileSimple(indexx, indexy);
                        World.world.dropManager.spawn(tileSimple, effect, 15, -1f);
                        //AssetManager.powers.drawDivineLight(tileSimple,"divineLight");
                    }
                }
            }
        }
    }
    class bounds{
        public int upper_bound = 0;
        public int lower_bound = 0;
        public int left_bound = 0;
        public int right_bound = 0;
    }
}