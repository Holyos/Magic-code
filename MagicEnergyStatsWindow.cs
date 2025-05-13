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
    class MagicEnergyStatsWindow : MonoBehaviour
    {
        private static GameObject contents;
        private static GameObject scrollView;
        private static Vector2 originalSize;
        private static Dictionary<string, int> energyStats = new Dictionary<string, int>();

        public static void openWindow()
        {
            loadStats();
            Windows.ShowWindow("MagicEnergyStatsWindow");
        }


        public static void init()
        {
            contents = WindowManager.windowContents["MagicEnergyStatsWindow"];
            scrollView = GameObject.Find($"Canvas Container Main/Canvas - Windows/windows/MagicEnergyStatsWindow/Background/Scroll View");
            originalSize = contents.GetComponent<RectTransform>().sizeDelta;
            VerticalLayoutGroup layoutGroup = contents.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childControlHeight = false;
            layoutGroup.childControlWidth = false;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childScaleHeight = true;
            layoutGroup.childScaleWidth = true;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.spacing = -70;

            /*ScrollWindow kingdomWindow = Windows.GetWindow("kingdom");
            Button kingdomLevelsButton = NewUI.createBGWindowButton(
                kingdomWindow.gameObject,
                10,
                "iconLevelsButton",
                "kingdomLevelsButton",
                "View Current Levels Within The Kingdom!",
                "Look At An Overview Of What Levels Every Unit In The Kingdom Has!",
                () => openKingdomWindow(kingdomWindow.gameObject.GetComponent<KingdomWindow>().kingdom.units.getSimpleList())
            );*/
        }

        private static void loadStats()
        {
            energyStats.Clear();


            contents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (energyStats.Count/5)*150) + originalSize;

            foreach(Transform child in contents.transform)
            {
                Destroy(child.gameObject);
            }

            //Dictionary<string, int> Main.listOfEnergy = (from ele in energyStats orderby int.Parse(ele.Key) ascending select ele).ToDictionary(key => key.Key, value => value.Value);

            foreach(KeyValuePair<string, int> kv in Main.listOfEnergy)
            {
                NewUI.addText($"{kv.Key} : {kv.Value} Erg", contents, 10, new Vector3(0, 0, 0), new Vector2(120, 0));
            }
        }
    }
}