using System.Reflection;
using HarmonyLib;
using Jaxxa.EnhancedDevelopment.Shields.Shields;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Jaxxa.EnhancedDevelopment.Shields.Patch.Patches
{
    // Token: 0x0200000E RID: 14
    internal class PatchDropPod : Patch
    {
        // Token: 0x0600005B RID: 91 RVA: 0x0000358C File Offset: 0x0000178C
        protected override void ApplyPatch(Harmony harmony = null)
        {
            ApplyTickPatch(harmony);
        }

        // Token: 0x0600005C RID: 92 RVA: 0x00003595 File Offset: 0x00001795
        protected override string PatchDescription()
        {
            return "PatchDropPod";
        }

        // Token: 0x0600005D RID: 93 RVA: 0x0000359C File Offset: 0x0000179C
        protected override bool ShouldPatchApply()
        {
            return true;
        }

        // Token: 0x0600005E RID: 94 RVA: 0x000035A0 File Offset: 0x000017A0
        private void ApplyTickPatch(Harmony harmony)
        {
            var method = typeof(DropPodIncoming).GetMethod("Impact", BindingFlags.Instance | BindingFlags.NonPublic);
            Patcher.LogNULL(method, "_DropPodIncomingImpact");
            var method2 = typeof(PatchDropPod).GetMethod("DropPodIncomingImpactPrefix",
                BindingFlags.Static | BindingFlags.Public);
            Patcher.LogNULL(method2, "_DropPodIncomingImpactPrefix");
            harmony.Patch(method, new HarmonyMethod(method2));
        }

        // Token: 0x0600005F RID: 95 RVA: 0x00003604 File Offset: 0x00001804
        public static bool DropPodIncomingImpactPrefix(ref DropPodIncoming __instance)
        {
            Log.Message("Dp incoming called.");
            if (!__instance.Map.GetComponent<ShieldManagerMapComp>().WillDropPodBeIntercepted(__instance))
            {
                return true;
            }

            __instance.innerContainer.ClearAndDestroyContents();
            SkyfallerShrapnelUtility.MakeShrapnel(__instance.Position, __instance.Map, __instance.shrapnelDirection,
                __instance.def.skyfaller.shrapnelDistanceFactor, 3, 0, true);
            Find.CameraDriver.shaker.DoShake(__instance.def.skyfaller.cameraShake);
            DamageDefOf.Bomb.soundExplosion.PlayOneShot(
                SoundInfo.InMap(new TargetInfo(__instance.Position, __instance.Map)));
            __instance.Destroy();
            return false;
        }
    }
}