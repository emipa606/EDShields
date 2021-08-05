using System;
using System.Reflection;
using HarmonyLib;
using Jaxxa.EnhancedDevelopment.Shields.Shields;
using UnityEngine;
using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Patch.Patches
{
    // Token: 0x0200000F RID: 15
    internal class PatchProjectile : Patch
    {
        // Token: 0x06000061 RID: 97 RVA: 0x000036CB File Offset: 0x000018CB
        protected override void ApplyPatch(Harmony harmony = null)
        {
            ApplyLaunchPatch();
            ApplyTickPatch(harmony);
        }

        // Token: 0x06000062 RID: 98 RVA: 0x000036DB File Offset: 0x000018DB
        protected override string PatchDescription()
        {
            return "PatchProjectile";
        }

        // Token: 0x06000063 RID: 99 RVA: 0x0000359C File Offset: 0x0000179C
        protected override bool ShouldPatchApply()
        {
            return true;
        }

        // Token: 0x06000064 RID: 100 RVA: 0x000036E4 File Offset: 0x000018E4
        private void ApplyLaunchPatch()
        {
            Type[] types =
            {
                typeof(Thing),
                typeof(Vector3),
                typeof(LocalTargetInfo),
                typeof(LocalTargetInfo),
                typeof(ProjectileHitFlags),
                typeof(bool),
                typeof(Thing),
                typeof(ThingDef)
            };
            Patcher.LogNULL(typeof(Projectile).GetMethod("Launch", types), "_ProjectileLaunch");
            Patcher.LogNULL(
                typeof(PatchProjectile).GetMethod("ProjectileLaunchPrefix", BindingFlags.Static | BindingFlags.Public),
                "_ProjectileLaunchPrefix");
        }

        // Token: 0x06000065 RID: 101 RVA: 0x0000359C File Offset: 0x0000179C
        public static bool ProjectileLaunchPrefix()
        {
            return true;
        }

        // Token: 0x06000066 RID: 102 RVA: 0x00003794 File Offset: 0x00001994
        private void ApplyTickPatch(Harmony harmony)
        {
            var method = typeof(Projectile).GetMethod("Tick");
            Patcher.LogNULL(method, "_ProjectileTick");
            var method2 =
                typeof(PatchProjectile).GetMethod("ProjectileTickPrefix", BindingFlags.Static | BindingFlags.Public);
            Patcher.LogNULL(method2, "_ProjectileTickPrefix");
            harmony.Patch(method, new HarmonyMethod(method2));
        }

        // Token: 0x06000067 RID: 103 RVA: 0x000037F6 File Offset: 0x000019F6
        public static bool ProjectileTickPrefix(ref Projectile __instance)
        {
            return !__instance.Map.GetComponent<ShieldManagerMapComp>().WillProjectileBeBlocked(__instance);
        }
    }
}