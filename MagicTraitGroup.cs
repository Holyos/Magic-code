using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;

namespace Magic
{
    class MagicTraitGroup
    {
 
        public static string magic = "magic";
        public static string fel = "fel";
 
        public static void init()
        {

            ActorTraitGroupAsset magic = new ActorTraitGroupAsset();
            magic.id = "magic";
            magic.name = "trait_group_magic";
            magic.color = Toolbox.makeColor("#800080", -1f);
            AssetManager.trait_groups.add(magic);
            addTraitGroupToLocalizedLibrary("en", magic.id, "Magic");
            addTraitGroupToLocalizedLibrary("ru", magic.id, "Магия");

            ActorTraitGroupAsset fel = new ActorTraitGroupAsset();
            fel.id = "fel";
            fel.name = "trait_group_fel";
            fel.color = Toolbox.makeColor("#af120f", -1f);
            AssetManager.trait_groups.add(fel);
            addTraitGroupToLocalizedLibrary("en", fel.id, "Fel");
            addTraitGroupToLocalizedLibrary("ru", fel.id, "Скверна");

        }
        public static void addTraitGroupToLocalizedLibrary(string planguage, string id, string name)
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
                localizedText.Add("trait_group_" + id, name);
            }
        }
    }
}
