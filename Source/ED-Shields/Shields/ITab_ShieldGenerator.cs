using RimWorld;
using UnityEngine;
using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields
{
    // Token: 0x02000008 RID: 8
    internal class ITab_ShieldGenerator : ITab
    {
        // Token: 0x0400003F RID: 63
        private static readonly Vector2 WinSize = new Vector2(500f, 400f);

        // Token: 0x06000041 RID: 65 RVA: 0x00002E0F File Offset: 0x0000100F
        public ITab_ShieldGenerator()
        {
            size = WinSize;
            labelKey = "TabShield";
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600003F RID: 63 RVA: 0x00002DD0 File Offset: 0x00000FD0
        private Comp_ShieldGenerator SelectedCompShieldGenerator
        {
            get
            {
                var thing = Find.Selector.SingleSelectedThing;
                if (thing is MinifiedThing minifiedThing)
                {
                    thing = minifiedThing.InnerThing;
                }

                if (thing == null)
                {
                    return null;
                }

                return thing.TryGetComp<Comp_ShieldGenerator>();
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000040 RID: 64 RVA: 0x00002E04 File Offset: 0x00001004
        public override bool IsVisible => SelectedCompShieldGenerator != null;

        // Token: 0x06000042 RID: 66 RVA: 0x00002E30 File Offset: 0x00001030
        protected override void FillTab()
        {
            var x = WinSize.x;
            var winSize = WinSize;
            var rect = new Rect(0f, 0f, x, winSize.y).ContractedBy(10f);
            var listing_Standard = new Listing_Standard {ColumnWidth = 250f};
            listing_Standard.Begin(rect);
            listing_Standard.GapLine();
            listing_Standard.Label("Charge: " + SelectedCompShieldGenerator.FieldIntegrity_Current + " / " +
                                   SelectedCompShieldGenerator.m_FieldIntegrity_Max);
            listing_Standard.Gap();
            listing_Standard.Label("Radius: " + SelectedCompShieldGenerator.m_FieldRadius_Requested + " / " +
                                   SelectedCompShieldGenerator.m_FieldRadius_Avalable);
            listing_Standard.IntAdjuster(ref SelectedCompShieldGenerator.m_FieldRadius_Requested, 1, 1);
            if (SelectedCompShieldGenerator.m_FieldRadius_Requested >
                SelectedCompShieldGenerator.m_FieldRadius_Avalable)
            {
                SelectedCompShieldGenerator.m_FieldRadius_Requested =
                    SelectedCompShieldGenerator.m_FieldRadius_Avalable;
            }

            if (SelectedCompShieldGenerator.BlockDirect_Active())
            {
                if (listing_Standard.ButtonText("Toggle Direct: Active"))
                {
                    SelectedCompShieldGenerator.SwitchDirect();
                }
            }
            else if (listing_Standard.ButtonText("Toggle Direct: Inactive"))
            {
                SelectedCompShieldGenerator.SwitchDirect();
            }

            if (SelectedCompShieldGenerator.BlockIndirect_Active())
            {
                if (listing_Standard.ButtonText("Toggle Indirect: Active"))
                {
                    SelectedCompShieldGenerator.SwitchIndirect();
                }
            }
            else if (listing_Standard.ButtonText("Toggle Indirect: Inactive"))
            {
                SelectedCompShieldGenerator.SwitchIndirect();
            }

            if (SelectedCompShieldGenerator.IsInterceptDropPod_Avalable())
            {
                if (SelectedCompShieldGenerator.IntercepDropPod_Active())
                {
                    if (listing_Standard.ButtonText("Toggle DropPod Intercept: Active"))
                    {
                        SelectedCompShieldGenerator.SwitchInterceptDropPod();
                    }
                }
                else if (listing_Standard.ButtonText("Toggle DropPod Intercept: Inactive"))
                {
                    SelectedCompShieldGenerator.SwitchInterceptDropPod();
                }
            }
            else
            {
                listing_Standard.Label("DropPod Intercept Unavalable");
            }

            listing_Standard.Label(SelectedCompShieldGenerator.IdentifyFriendFoe_Active()
                ? "IFF Active"
                : "IFF Inactive");

            listing_Standard.End();
        }
    }
}