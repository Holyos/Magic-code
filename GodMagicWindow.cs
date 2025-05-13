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
    class GodMagicWindow : MonoBehaviour
    {
        private static GameObject contents;
        private static GameObject scrollView;
        private static Vector2 originalSize;
        public static GodMagicWindow instance;

        public static void openWindow()
        {
            //loadStats();
            Windows.ShowWindow("GodMagicWindow");
        }
        public static void init()
        {
            
            contents = WindowManager.windowContents["GodMagicWindow"];
            instance = new GameObject("GodMagicWindowInstance").AddComponent<GodMagicWindow>();
            scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/GodMagicWindow/Background/Scroll View");
            originalSize = contents.GetComponent<RectTransform>().sizeDelta;
            VerticalLayoutGroup layoutGroup = contents.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childControlHeight = false;
            layoutGroup.childControlWidth = false;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childScaleHeight = true;
            layoutGroup.childScaleWidth = true;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.spacing = 10;
            loadGodOptions();
        }
        private static void loadGodOptions()
        {
            contents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, ((Main.savedSettings.GodMagicOptions.Count))*250);
            foreach(KeyValuePair<string, InputOption> kv in Main.savedSettings.GodMagicOptions)
            {

                UnityAction call = null;
                switch(kv.Key)
                {
                    case "Rod Upper (0-1000)":
                        call = delegate{

                            if (Main.savedSettings.GodMagicOptions["Rod Upper (0-1000)"].active)
                            {
                                GodMagic.GodBounds["Rod"].upper_bound = (int) MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions["Rod Upper (0-1000)"].value)/1000;
                            }
                            else
                            {
                                GodMagic.GodBounds["Rod"].upper_bound = -1;
                                Debug.Log("Inactive");
                            }
                        };
                        break;
                    case "Rod Lower (0-1000)":
                        call = delegate{
                            if (Main.savedSettings.GodMagicOptions["Rod Lower (0-1000)"].active)
                            {
                                GodMagic.GodBounds["Rod"].lower_bound = (int) MapBox.height*int.Parse(Main.savedSettings.GodMagicOptions["Rod Lower (0-1000)"].value)/1000;
                            }
                            else
                            {
                                GodMagic.GodBounds["Rod"].lower_bound = -1;
                                Debug.Log("Inactive");
                            }
                        };
                        break;
                    case "Rod Left (0-1000)":
                        call = delegate{
                            if (Main.savedSettings.GodMagicOptions["Rod Left (0-1000)"].active)
                            {
                                GodMagic.GodBounds["Rod"].left_bound = (int) MapBox.width*int.Parse(Main.savedSettings.GodMagicOptions["Rod Left (0-1000)"].value)/1000;
                            }
                            else
                            {
                                GodMagic.GodBounds["Rod"].left_bound = -1;
                                Debug.Log("Inactive");
                            }
                        };
                        break;
                    case "Rod Right (0-1000)":
                        call = delegate{
                            if (Main.savedSettings.GodMagicOptions["Rod Right (0-1000)"].active)
                            {
                                GodMagic.GodBounds["Rod"].right_bound = (int) MapBox.width*int.Parse(Main.savedSettings.GodMagicOptions["Rod Right (0-1000)"].value)/1000;
                            }
                            else
                            {
                                GodMagic.GodBounds["Rod"].right_bound = -1;
                                Debug.Log("Inactive");
                            }
                        };
                        break;

                }
                if (call != null)
                {
                    call.Invoke();
                }

                NameInput input = NewUI.createInputOption(
                    $"{kv.Key}_setting", 
                    kv.Key, 
                    "Modify The Value Of This Setting", 
                    0,
                    contents, 
                    kv.Value.value
                );
                input.inputField.characterValidation = InputField.CharacterValidation.Integer;
                input.inputField.onValueChanged.AddListener(delegate{
                    string pValue = NewUI.checkStatInput(input);
                    Main.modifyGodOption(kv.Key, pValue, PowerButtons.GetToggleValue($"{kv.Key}Button"), call);
                    input.setText(pValue);
                });

                PowerButton activeButton = PowerButtons.CreateButton(
                    $"{kv.Key}Button", 
                    Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.units.icon.png"),
                    "Activate Setting", 
                    "", 
                    new Vector2(200, 0), 
                    ButtonType.Toggle, 
                    input.transform.parent.transform, 
                    delegate{
                        string pValue = NewUI.checkStatInput(input);
                        Main.modifyGodOption(kv.Key, pValue, PowerButtons.GetToggleValue($"{kv.Key}Button"), call);
                        input.setText(pValue);
                    }
                );
                if (kv.Value.active)
                {
                    PowerButtons.ToggleButton($"{kv.Key}Button");
                }
                activeButton.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(64, 64);
            }
        }
    }
}