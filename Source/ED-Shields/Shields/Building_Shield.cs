using RimWorld;
using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields
{
    // Token: 0x02000002 RID: 2
    [StaticConstructorOnStartup]
    public class Building_Shield : Building
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override string GetInspectString()
        {
            return GetComp<Comp_ShieldGenerator>().CompInspectStringExtra();
        }

        // Token: 0x06000002 RID: 2 RVA: 0x0000205D File Offset: 0x0000025D
        public bool WillInterceptDropPod(DropPodIncoming dropPodToCheck)
        {
            return GetComp<Comp_ShieldGenerator>().WillInterceptDropPod(dropPodToCheck);
        }

        // Token: 0x06000003 RID: 3 RVA: 0x0000206B File Offset: 0x0000026B
        public bool WillProjectileBeBlocked(Projectile projectileToCheck)
        {
            return GetComp<Comp_ShieldGenerator>().WillProjectileBeBlocked(projectileToCheck);
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002079 File Offset: 0x00000279
        public void TakeDamageFromProjectile(Projectile projectile)
        {
            GetComp<Comp_ShieldGenerator>().FieldIntegrity_Current -= projectile.DamageAmount;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002093 File Offset: 0x00000293
        public void RecalculateStatistics()
        {
            GetComp<Comp_ShieldGenerator>().RecalculateStatistics();
        }
    }
}