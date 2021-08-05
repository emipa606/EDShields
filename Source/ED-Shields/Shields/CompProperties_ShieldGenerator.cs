using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields
{
    // Token: 0x02000003 RID: 3
    public class CompProperties_ShieldGenerator : CompProperties
    {
        // Token: 0x04000009 RID: 9
        public bool m_BlockDirect_Avalable;

        // Token: 0x0400000A RID: 10
        public bool m_BlockIndirect_Avalable;

        // Token: 0x04000003 RID: 3
        public int m_Field_Radius_Base;

        // Token: 0x04000001 RID: 1
        public int m_FieldIntegrity_Initial;

        // Token: 0x04000002 RID: 2
        public int m_FieldIntegrity_Max_Base;

        // Token: 0x0400000B RID: 11
        public bool m_InterceptDropPod_Avalable;

        // Token: 0x04000004 RID: 4
        public int m_PowerRequired_Charging;

        // Token: 0x04000005 RID: 5
        public int m_PowerRequired_Standby;

        // Token: 0x04000007 RID: 7
        public int m_RechargeAmmount_Base = 1;

        // Token: 0x04000006 RID: 6
        public int m_RechargeTickDelayInterval_Base;

        // Token: 0x04000008 RID: 8
        public int m_RecoverWarmupDelayTicks_Base;

        // Token: 0x0400000C RID: 12
        public bool m_StructuralIntegrityMode;

        // Token: 0x06000007 RID: 7 RVA: 0x000020A8 File Offset: 0x000002A8
        public CompProperties_ShieldGenerator()
        {
            compClass = typeof(Comp_ShieldGenerator);
        }
    }
}