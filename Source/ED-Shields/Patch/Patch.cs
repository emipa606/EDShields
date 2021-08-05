using HarmonyLib;
using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Patch
{
    // Token: 0x0200000C RID: 12
    internal abstract class Patch
    {
        // Token: 0x06000053 RID: 83
        protected abstract bool ShouldPatchApply();

        // Token: 0x06000054 RID: 84
        protected abstract void ApplyPatch(Harmony harmony = null);

        // Token: 0x06000055 RID: 85
        protected abstract string PatchDescription();

        // Token: 0x06000056 RID: 86 RVA: 0x00003484 File Offset: 0x00001684
        public void ApplyPatchIfRequired(Harmony harmony = null)
        {
            var str = "Shields.Patch.ApplyPatchIfRequired: ";
            if (ShouldPatchApply())
            {
                Log.Message(str + "Applying Patch: " + PatchDescription());
                ApplyPatch(harmony);
                Log.Message(str + "Applied Patch: " + PatchDescription());
                return;
            }

            Log.Message(str + "Skipping Applying Patch: " + PatchDescription());
        }
    }
}