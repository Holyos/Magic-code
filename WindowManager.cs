using System;
using System.Collections;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;

namespace Magic
{
    class WindowManager
    {
        public static Dictionary<string, GameObject> windowContents = new Dictionary<string, GameObject>();
        public static Dictionary<string, ScrollWindow> createdWindows = new Dictionary<string, ScrollWindow>();

        public static void init()
        {
            string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
            if (language == "ru")
            {
                newWindow("MagicStatsWindow", "Насыщение мира магией");
            }
            else
                newWindow("MagicStatsWindow", "Saturation of Magic");
            MagicStatsWindow.init();
            if (language == "ru")
            {
                newWindow("MagicEnergyStatsWindow", "Количество энергии в мире");
            }
            else
                newWindow("MagicEnergyStatsWindow", "The amount of energy in the world");
            MagicEnergyStatsWindow.init();
            if (language == "ru")
            {
                newWindow("GodMagicWindow", "Размеры божественного заклинания");
            }
            else
                newWindow("GodMagicWindow", "The dimensions of the Divine Spell");
            GodMagicWindow.init();
        }

        private static void newWindow(string id, string title)
        {
            ScrollWindow window;
            GameObject content;
            window = Windows.CreateNewWindow(id, title);
            createdWindows.Add(id, window);

            GameObject scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View");
            scrollView.gameObject.SetActive(true);

            content = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View/Viewport/Content");
            if (content != null)
            {
                windowContents.Add(id, content);
            }
        }
    }
}