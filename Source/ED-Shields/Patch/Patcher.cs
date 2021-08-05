using System.Collections.Generic;
using HarmonyLib;
using Jaxxa.EnhancedDevelopment.Shields.Patch.Patches;
using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Patch
{
    // Token: 0x0200000D RID: 13
    [StaticConstructorOnStartup]
    internal class Patcher
    {
        // Token: 0x06000058 RID: 88 RVA: 0x000034EC File Offset: 0x000016EC
        static Patcher()
        {
            var str = "Jaxxa.EnhancedDevelopment.Shields.Patch.Patcher(): ";
            Log.Message(str + "Starting.");
            var list = new List<Patch> {new PatchProjectile(), new PatchDropPod()};
            var _Harmony = new Harmony("Jaxxa.EnhancedDevelopment.Shields");
            list.ForEach(delegate(Patch p) { p.ApplyPatchIfRequired(_Harmony); });
            Log.Message(str + "Complete.");
        }

        // Token: 0x06000059 RID: 89 RVA: 0x00003561 File Offset: 0x00001761
        public static void LogNULL(object objectToTest, string name, bool logSucess = false)
        {
            if (objectToTest == null)
            {
                Log.Error(name + " Is NULL.");
                return;
            }

            if (logSucess)
            {
                Log.Message(name + " Is Not NULL.");
            }
        }
    }
}