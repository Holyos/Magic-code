using System;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using NCMS.Utils;

namespace Magic
{
    class MagicEffects
    {
        public static void init()
        {

          //StatusEffect An02Slah = new StatusEffect();
		  //An02Slah.id = "an02";
		  //An02Slah.texture = "an02";
		  //An02Slah.animated = true;
		  //An02Slah.animation_speed = 0.1f;
		  //An02Slah.duration = 0f;
		  //AssetManager.status.add(An02Slah);

          //StatusEffect An02Effect = new StatusEffect();
		  //An02Effect.id = "fx_anprojetil_trail";
		  //An02Effect.texture = "anprojetil2";
		  //An02Effect.animated = true;
		  //An02Effect.animation_speed = 0.1f;
		  //An02Effect.duration = 0f;
		  //AssetManager.status.add(An02Effect);

          ProjectileAsset AnProjetil = new ProjectileAsset();
          AnProjetil.id = "AnProjetil";
          AnProjetil.texture = "pr_plasma_ball";
          AnProjetil.speed = 20f;
          AnProjetil.trailEffect_scale = 0.1f;
          AnProjetil.trailEffect_timer = 0.1f;
          AnProjetil.startScale = 0.035f;
          AnProjetil.targetScale = 0.2f;
          AnProjetil.trailEffect_enabled = true;
          AnProjetil.look_at_target = true;
          AssetManager.projectiles.add(AnProjetil);

          ProjectileAsset AngelProjetil = new ProjectileAsset();
          AngelProjetil.id = "AngelProjetil";
          AngelProjetil.texture = "pr_freeze_orb";
          AngelProjetil.speed = 12f;
          AngelProjetil.startScale = 0.035f;
		      AngelProjetil.targetScale = 0.2f;
          AngelProjetil.look_at_target = true;
          AssetManager.projectiles.add(AngelProjetil);

          ProjectileAsset JpProjetil = new ProjectileAsset();
          JpProjetil.id = "JpProjetil";
          JpProjetil.texture = "fireball";
          JpProjetil.texture_shadow = "shadow_ball";
          JpProjetil.speed = 20f;
          JpProjetil.startScale = 0.035f;
          JpProjetil.targetScale = 0.2f;
          JpProjetil.trailEffect_enabled = true;
          JpProjetil.look_at_target = true;
          AssetManager.projectiles.add(JpProjetil);
        }
    }
}