using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields
{
    // Token: 0x02000004 RID: 4
    internal class CompProperties_ShieldUpgrade : CompProperties
    {
        // Token: 0x04000011 RID: 17
        public bool DropPodIntercept;

        // Token: 0x0400000E RID: 14
        public int FieldIntegrity_Increase;

        // Token: 0x04000010 RID: 16
        public bool IdentifyFriendFoe;

        // Token: 0x0400000D RID: 13
        public int PowerUsage_Increase;

        // Token: 0x0400000F RID: 15
        public int Range_Increase;

        // Token: 0x04000012 RID: 18
        public bool SIFMode;

        // Token: 0x04000013 RID: 19
        public bool SlowDischarge;

        // Token: 0x06000008 RID: 8 RVA: 0x000020C7 File Offset: 0x000002C7
        public CompProperties_ShieldUpgrade()
        {
            compClass = typeof(Comp_ShieldUpgrade);
        }
    }
}