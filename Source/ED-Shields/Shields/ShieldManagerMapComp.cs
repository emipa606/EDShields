using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields
{
    // Token: 0x02000009 RID: 9
    [StaticConstructorOnStartup]
    internal class ShieldManagerMapComp : MapComponent
    {
        // Token: 0x04000040 RID: 64
        public static readonly SoundDef HitSoundDef = SoundDef.Named("Shields_HitShield");

        // Token: 0x04000041 RID: 65
        private List<Projectile> m_Projectiles = new List<Projectile>();

        // Token: 0x06000044 RID: 68 RVA: 0x0000308D File Offset: 0x0000128D
        public ShieldManagerMapComp(Map map) : base(map)
        {
            this.map = map;
        }

        // Token: 0x06000045 RID: 69 RVA: 0x000030A8 File Offset: 0x000012A8

        // Token: 0x06000046 RID: 70 RVA: 0x000030B0 File Offset: 0x000012B0
        public bool WillDropPodBeIntercepted(DropPodIncoming dropPodToTest)
        {
            return map.listerBuildings.AllBuildingsColonistOfClass<Building_Shield>()
                .Any(x => x.WillInterceptDropPod(dropPodToTest));
        }

        // Token: 0x06000047 RID: 71 RVA: 0x000030F0 File Offset: 0x000012F0
        public bool WillProjectileBeBlocked(Projectile projectile)
        {
            var building_Shield = map.listerBuildings.AllBuildingsColonistOfClass<Building_Shield>()
                .FirstOrFallback(x => x.WillProjectileBeBlocked(projectile));
            if (building_Shield == null)
            {
                return false;
            }

            building_Shield.TakeDamageFromProjectile(projectile);
            FleckMaker.ThrowLightningGlow(projectile.ExactPosition, map, 0.5f);
            HitSoundDef.PlayOneShot(new TargetInfo(projectile.Position, projectile.Map));
            projectile.Destroy();
            return true;
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00003191 File Offset: 0x00001391
        public void RecalaculateAll()
        {
            map.listerBuildings.AllBuildingsColonistOfClass<Building_Shield>().ToList()
                .ForEach(delegate(Building_Shield x) { x.RecalculateStatistics(); });
        }
    }
}