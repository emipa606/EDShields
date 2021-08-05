using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields
{
    // Token: 0x02000006 RID: 6
    internal class Comp_ShieldUpgrade : ThingComp
    {
        // Token: 0x04000038 RID: 56
        public CompProperties_ShieldUpgrade Properties;

        // Token: 0x0600003C RID: 60 RVA: 0x00002D82 File Offset: 0x00000F82
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Properties = (CompProperties_ShieldUpgrade) props;
            parent.Map.GetComponent<ShieldManagerMapComp>().RecalaculateAll();
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00002DB1 File Offset: 0x00000FB1
        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            map.GetComponent<ShieldManagerMapComp>().RecalaculateAll();
        }
    }
}