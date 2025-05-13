using System;
using NCMS;
using NCMS.Utils;

namespace Magic
{
    class MagicNames
    {
        public static void init(){
          Names();
        }

        public static void Names()
        {
          NameGeneratorAsset phoenixName = new NameGeneratorAsset();
          phoenixName.id = "phoenix_name";
          phoenixName.part_groups.Add("Fyre,Plume,Ignite,Solaris,Spirit,Slag,Ryze,Solar,Brilliancy,Onyx,Nix,Soleil,Viva,Scorchey,Flametalon,Pharos,Brilliancy,Sun,Pire,Aura,Deja,Sunny");
          phoenixName.templates.Add("part_group");
          AssetManager.nameGenerator.add(phoenixName);
		}
	}
}