using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Jaxxa.EnhancedDevelopment.Shields.Patch;
using Jaxxa.EnhancedDevelopment.Shields.Shields.Utilities;
using RimWorld;
using UnityEngine;
using Verse;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields
{
    // Token: 0x02000005 RID: 5
    [StaticConstructorOnStartup]
    public class Comp_ShieldGenerator : ThingComp
    {
        // Token: 0x04000016 RID: 22
        private static readonly Texture2D UI_DIRECT_ON;

        // Token: 0x04000017 RID: 23
        private static readonly Texture2D UI_DIRECT_OFF = ContentFinder<Texture2D>.Get("UI/DirectOff");

        // Token: 0x04000018 RID: 24
        private static readonly Texture2D UI_INDIRECT_ON;

        // Token: 0x04000019 RID: 25
        private static readonly Texture2D UI_INDIRECT_OFF;

        // Token: 0x0400001A RID: 26
        private static readonly Texture2D UI_INTERCEPT_DROPPOD_ON;

        // Token: 0x0400001B RID: 27
        private static readonly Texture2D UI_INTERCEPT_DROPPOD_OFF;

        // Token: 0x0400001C RID: 28
        private static readonly Texture2D UI_SHOW_ON;

        // Token: 0x0400001D RID: 29
        private static readonly Texture2D UI_SHOW_OFF;

        // Token: 0x0400001E RID: 30
        private static readonly Texture2D UI_LAUNCH_REPORT;

        // Token: 0x04000014 RID: 20
        private Material currentMatrialColour;

        // Token: 0x04000028 RID: 40
        private List<Building> m_AppliedUpgrades = new List<Building>();

        // Token: 0x0400002D RID: 45
        private bool m_BlockDirect_Avalable;

        // Token: 0x0400002E RID: 46
        private bool m_BlockDirect_Requested = true;

        // Token: 0x0400002F RID: 47
        private bool m_BlockIndirect_Avalable;

        // Token: 0x04000030 RID: 48
        private bool m_BlockIndirect_Requested = true;

        // Token: 0x04000022 RID: 34
        private float m_ColourBlue;

        // Token: 0x04000021 RID: 33
        private float m_ColourGreen;

        // Token: 0x04000020 RID: 32
        private float m_ColourRed;

        // Token: 0x04000036 RID: 54
        private EnumShieldStatus m_CurrentStatus = EnumShieldStatus.Offline;

        // Token: 0x04000037 RID: 55
        private int m_FieldIntegrity_Current;

        // Token: 0x04000024 RID: 36
        private int m_FieldIntegrity_Initial;

        // Token: 0x04000023 RID: 35
        public int m_FieldIntegrity_Max;

        // Token: 0x0400002B RID: 43
        public int m_FieldRadius_Avalable;

        // Token: 0x0400002C RID: 44
        public int m_FieldRadius_Requested = 999;

        // Token: 0x04000033 RID: 51
        private bool m_IdentifyFriendFoe_Avalable;

        // Token: 0x04000034 RID: 52
        private bool m_IdentifyFriendFoe_Requested = true;

        // Token: 0x04000031 RID: 49
        private bool m_InterceptDropPod_Avalable;

        // Token: 0x04000032 RID: 50
        private bool m_InterceptDropPod_Requested = true;

        // Token: 0x04000029 RID: 41
        private CompPowerTrader m_Power;

        // Token: 0x0400002A RID: 42
        private int m_PowerRequired;

        // Token: 0x04000025 RID: 37
        private int m_RechargeTickDelayInterval;

        // Token: 0x04000026 RID: 38
        private int m_RecoverWarmupDelayTicks;

        // Token: 0x0400001F RID: 31
        private bool m_ShowVisually_Active = true;

        // Token: 0x04000027 RID: 39
        private int m_WarmupTicksRemaining;

        // Token: 0x04000015 RID: 21
        public CompProperties_ShieldGenerator Properties;

        // Token: 0x04000035 RID: 53
        public bool SlowDischarge_Active;

        // Token: 0x0600000F RID: 15 RVA: 0x00002144 File Offset: 0x00000344
        static Comp_ShieldGenerator()
        {
            UI_DIRECT_ON = ContentFinder<Texture2D>.Get("UI/DirectOn");
            UI_INDIRECT_OFF = ContentFinder<Texture2D>.Get("UI/IndirectOff");
            UI_INDIRECT_ON = ContentFinder<Texture2D>.Get("UI/IndirectOn");
            UI_INTERCEPT_DROPPOD_OFF = ContentFinder<Texture2D>.Get("UI/FireOff");
            UI_INTERCEPT_DROPPOD_ON = ContentFinder<Texture2D>.Get("UI/FireOn");
            UI_SHOW_ON = ContentFinder<Texture2D>.Get("UI/ShieldShowOn");
            UI_SHOW_OFF = ContentFinder<Texture2D>.Get("UI/ShieldShowOff");
            UI_LAUNCH_REPORT = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport");
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x0600001B RID: 27 RVA: 0x00002716 File Offset: 0x00000916
        // (set) Token: 0x0600001C RID: 28 RVA: 0x0000271E File Offset: 0x0000091E
        public EnumShieldStatus CurrentStatus
        {
            get => m_CurrentStatus;
            set => m_CurrentStatus = value;
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x0600001D RID: 29 RVA: 0x00002727 File Offset: 0x00000927
        // (set) Token: 0x0600001E RID: 30 RVA: 0x0000272F File Offset: 0x0000092F
        public int FieldIntegrity_Current
        {
            get => m_FieldIntegrity_Current;
            set
            {
                if (value < 0)
                {
                    CurrentStatus = EnumShieldStatus.Offline;
                    m_FieldIntegrity_Current = 0;
                    return;
                }

                if (value > m_FieldIntegrity_Max)
                {
                    m_FieldIntegrity_Current = m_FieldIntegrity_Max;
                    return;
                }

                m_FieldIntegrity_Current = value;
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000020DF File Offset: 0x000002DF
        public int FieldRadius_Active()
        {
            return Math.Min(m_FieldRadius_Requested, m_FieldRadius_Avalable);
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000020F2 File Offset: 0x000002F2
        public bool BlockDirect_Active()
        {
            return m_BlockDirect_Avalable && m_BlockDirect_Requested;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002104 File Offset: 0x00000304
        public bool BlockIndirect_Active()
        {
            return m_BlockIndirect_Avalable && m_BlockIndirect_Requested;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002116 File Offset: 0x00000316
        public bool IntercepDropPod_Active()
        {
            return m_InterceptDropPod_Avalable && m_InterceptDropPod_Requested;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002128 File Offset: 0x00000328
        public bool IsInterceptDropPod_Avalable()
        {
            return m_InterceptDropPod_Avalable;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002130 File Offset: 0x00000330
        public bool IdentifyFriendFoe_Active()
        {
            return m_IdentifyFriendFoe_Avalable && m_IdentifyFriendFoe_Requested;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000021E1 File Offset: 0x000003E1
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Properties = (CompProperties_ShieldGenerator) props;
            m_Power = parent.GetComp<CompPowerTrader>();
            RecalculateStatistics();
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002214 File Offset: 0x00000414
        public void RecalculateStatistics()
        {
            m_ColourRed = 0.5f;
            m_ColourGreen = 0f;
            m_ColourBlue = 0.5f;
            m_FieldIntegrity_Max = Properties.m_FieldIntegrity_Max_Base;
            m_FieldIntegrity_Initial = Properties.m_FieldIntegrity_Initial;
            m_FieldRadius_Avalable = Properties.m_Field_Radius_Base;
            m_BlockIndirect_Avalable = Properties.m_BlockIndirect_Avalable;
            m_BlockDirect_Avalable = Properties.m_BlockDirect_Avalable;
            m_InterceptDropPod_Avalable = Properties.m_InterceptDropPod_Avalable;
            m_PowerRequired = Properties.m_PowerRequired_Charging;
            m_RechargeTickDelayInterval = Properties.m_RechargeTickDelayInterval_Base;
            m_RecoverWarmupDelayTicks = Properties.m_RecoverWarmupDelayTicks_Base;
            SlowDischarge_Active = false;
            m_IdentifyFriendFoe_Avalable = false;
            m_AppliedUpgrades.ForEach(delegate(Building b)
            {
                var comp = b.GetComp<Comp_ShieldUpgrade>();
                Patcher.LogNULL(b, "_Building");
                Patcher.LogNULL(comp, "_Comp");
                AddStatsFromUpgrade(comp);
            });
            m_Power.powerOutputInt = -(float) m_PowerRequired;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002314 File Offset: 0x00000514
        private void AddStatsFromUpgrade(Comp_ShieldUpgrade comp)
        {
            var compProperties_ShieldUpgrade = (CompProperties_ShieldUpgrade) comp.props;
            Patcher.LogNULL(compProperties_ShieldUpgrade, "_Properties");
            m_FieldIntegrity_Max += compProperties_ShieldUpgrade.FieldIntegrity_Increase;
            m_FieldRadius_Avalable += compProperties_ShieldUpgrade.Range_Increase;
            m_PowerRequired += compProperties_ShieldUpgrade.PowerUsage_Increase;
            if (compProperties_ShieldUpgrade.DropPodIntercept)
            {
                m_InterceptDropPod_Avalable = true;
            }

            if (compProperties_ShieldUpgrade.IdentifyFriendFoe)
            {
                m_IdentifyFriendFoe_Avalable = true;
            }

            if (compProperties_ShieldUpgrade.SlowDischarge)
            {
                SlowDischarge_Active = true;
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x0000239F File Offset: 0x0000059F
        public override void CompTick()
        {
            base.CompTick();
            UpdateShieldStatus();
            TickRecharge();
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000023B4 File Offset: 0x000005B4
        public void UpdateShieldStatus()
        {
            switch (CurrentStatus)
            {
                case EnumShieldStatus.ActiveCharging:
                    if (FieldIntegrity_Current < 0)
                    {
                        CurrentStatus = EnumShieldStatus.Offline;
                        return;
                    }

                    if (!CheckPowerOn())
                    {
                        CurrentStatus = EnumShieldStatus.ActiveDischarging;
                        return;
                    }

                    if (FieldIntegrity_Current >= m_FieldIntegrity_Max)
                    {
                        CurrentStatus = EnumShieldStatus.ActiveSustaining;
                    }

                    break;
                case EnumShieldStatus.ActiveSustaining:
                    if (!CheckPowerOn())
                    {
                        CurrentStatus = EnumShieldStatus.ActiveDischarging;
                        return;
                    }

                    if (FieldIntegrity_Current < m_FieldIntegrity_Max)
                    {
                        CurrentStatus = EnumShieldStatus.ActiveCharging;
                    }

                    break;
                case EnumShieldStatus.ActiveDischarging:
                    if (CheckPowerOn())
                    {
                        CurrentStatus = EnumShieldStatus.ActiveCharging;
                        return;
                    }

                    if (!SlowDischarge_Active)
                    {
                        m_FieldIntegrity_Current = 0;
                    }

                    if (FieldIntegrity_Current <= 0)
                    {
                        CurrentStatus = EnumShieldStatus.Offline;
                    }

                    break;
                case EnumShieldStatus.Initilising:
                    if (!CheckPowerOn())
                    {
                        CurrentStatus = EnumShieldStatus.Offline;
                        return;
                    }

                    if (m_WarmupTicksRemaining > 0)
                    {
                        m_WarmupTicksRemaining--;
                        return;
                    }

                    CurrentStatus = EnumShieldStatus.ActiveCharging;
                    FieldIntegrity_Current = m_FieldIntegrity_Initial;
                    return;
                case EnumShieldStatus.Offline:
                    if (CheckPowerOn())
                    {
                        CurrentStatus = EnumShieldStatus.Initilising;
                        m_WarmupTicksRemaining = m_RecoverWarmupDelayTicks;
                    }

                    break;
                default:
                    return;
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000024B8 File Offset: 0x000006B8
        public bool IsActive()
        {
            return CurrentStatus == EnumShieldStatus.ActiveCharging ||
                   CurrentStatus == EnumShieldStatus.ActiveDischarging ||
                   CurrentStatus == EnumShieldStatus.ActiveSustaining;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000024D6 File Offset: 0x000006D6
        public bool CheckPowerOn()
        {
            return m_Power is {PowerOn: true};
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000024F0 File Offset: 0x000006F0
        public void TickRecharge()
        {
            if (Find.TickManager.TicksGame % m_RechargeTickDelayInterval != 0)
            {
                return;
            }

            if (CurrentStatus == EnumShieldStatus.ActiveCharging)
            {
                var fieldIntegrity_Current = FieldIntegrity_Current;
                FieldIntegrity_Current = fieldIntegrity_Current + 1;
                return;
            }

            if (CurrentStatus != EnumShieldStatus.ActiveDischarging)
            {
                return;
            }

            {
                var fieldIntegrity_Current = FieldIntegrity_Current;
                FieldIntegrity_Current = fieldIntegrity_Current - 1;
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002544 File Offset: 0x00000744
        public bool WillInterceptDropPod(DropPodIncoming dropPodToCheck)
        {
            if (!IntercepDropPod_Active())
            {
                return false;
            }

            if (CurrentStatus == EnumShieldStatus.Offline || CurrentStatus == EnumShieldStatus.Initilising)
            {
                return false;
            }

            if (IdentifyFriendFoe_Active())
            {
                if (!dropPodToCheck.Contents.innerContainer.Any(x => x.Faction.HostileTo(Faction.OfPlayer)))
                {
                    return false;
                }
            }

            var num = Vector3.Distance(dropPodToCheck.Position.ToVector3(), parent.Position.ToVector3());
            float num2 = FieldRadius_Active();
            return num <= num2;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000025E0 File Offset: 0x000007E0
        public bool WillProjectileBeBlocked(Projectile projectile)
        {
            if (CurrentStatus == EnumShieldStatus.Offline || CurrentStatus == EnumShieldStatus.Initilising)
            {
                return false;
            }

            if (projectile.def.projectile.flyOverhead)
            {
                if (!BlockIndirect_Active())
                {
                    return false;
                }
            }
            else if (!BlockDirect_Active())
            {
                return false;
            }

            if (Vector3.Distance(projectile.Position.ToVector3(), parent.Position.ToVector3()) > FieldRadius_Active())
            {
                return false;
            }

            if (!CorrectAngleToIntercept(projectile, parent))
            {
                return false;
            }

            if (!IdentifyFriendFoe_Active())
            {
                return true;
            }

            var field = typeof(Projectile).GetField("launcher", BindingFlags.Instance | BindingFlags.NonPublic);
            Patcher.LogNULL(field, "_LauncherFieldInfo");
            if (field is null)
            {
                return true;
            }

            var thing = (Thing) field.GetValue(projectile);
            Patcher.LogNULL(thing, "_Launcher");
            if (thing.Faction.IsPlayer)
            {
                return false;
            }

            return true;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000026B8 File Offset: 0x000008B8
        public static bool CorrectAngleToIntercept(Projectile pr, Thing shieldBuilding)
        {
            var exactRotation = pr.ExactRotation;
            var exactPosition = pr.ExactPosition;
            exactPosition.y = 0f;
            var vector = shieldBuilding.Position.ToVector3();
            vector.y = 0f;
            var quaternion = Quaternion.LookRotation(exactPosition - vector);
            return Quaternion.Angle(exactRotation, quaternion) > 90f;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002761 File Offset: 0x00000961
        public override void PostDraw()
        {
            base.PostDraw();
            DrawShields();
        }

        // Token: 0x06000020 RID: 32 RVA: 0x0000276F File Offset: 0x0000096F
        public void DrawShields()
        {
            if (!IsActive() || !m_ShowVisually_Active)
            {
                return;
            }

            DrawField(VectorsUtils.IntVecToVec(parent.Position));
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002798 File Offset: 0x00000998
        public void DrawSubField(IntVec3 center, float radius)
        {
            DrawSubField(VectorsUtils.IntVecToVec(center), radius);
        }

        // Token: 0x06000022 RID: 34 RVA: 0x000027A7 File Offset: 0x000009A7
        public void DrawField(Vector3 center)
        {
            DrawSubField(center, FieldRadius_Active());
        }

        // Token: 0x06000023 RID: 35 RVA: 0x000027B8 File Offset: 0x000009B8
        public void DrawSubField(Vector3 position, float shieldShieldRadius)
        {
            position += new Vector3(0.5f, 0f, 0.5f);
            var vector = new Vector3(shieldShieldRadius, 1f, shieldShieldRadius);
            var matrix4x = default(Matrix4x4);
            matrix4x.SetTRS(position, Quaternion.identity, vector);
            if (currentMatrialColour == null)
            {
                currentMatrialColour = SolidColorMaterials.NewSolidColorMaterial(
                    new Color(m_ColourRed, m_ColourGreen, m_ColourBlue, 0.15f), ShaderDatabase.MetaOverlay);
            }

            Graphics.DrawMesh(JaxxaGraphics.CircleMesh, matrix4x, currentMatrialColour, 0);
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00002854 File Offset: 0x00000A54
        public override string CompInspectStringExtra()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(base.CompInspectStringExtra());
            if (IsActive())
            {
                stringBuilder.AppendLine("Shield: " + FieldIntegrity_Current + "/" + m_FieldIntegrity_Max);
            }
            else if (CurrentStatus == EnumShieldStatus.Initilising)
            {
                stringBuilder.AppendLine(
                    "Ready in " + Math.Round(m_WarmupTicksRemaining.TicksToSeconds()) + " seconds.");
            }
            else
            {
                stringBuilder.AppendLine("Shield disabled!");
            }

            if (m_Power != null)
            {
                var text = m_Power.CompInspectStringExtra();
                stringBuilder.Append(!text.NullOrEmpty() ? text : "Error, No Power Comp Text.");
            }
            else
            {
                stringBuilder.Append("Error, No Power Comp.");
            }

            return stringBuilder.ToString();
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002939 File Offset: 0x00000B39
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            if (m_BlockDirect_Avalable)
            {
                if (BlockDirect_Active())
                {
                    yield return new Command_Action
                    {
                        action = SwitchDirect,
                        icon = UI_DIRECT_ON,
                        defaultLabel = "Block Direct",
                        defaultDesc = "On",
                        activateSound = SoundDef.Named("Click")
                    };
                }
                else
                {
                    yield return new Command_Action
                    {
                        action = SwitchDirect,
                        icon = UI_DIRECT_OFF,
                        defaultLabel = "Block Direct",
                        defaultDesc = "Off",
                        activateSound = SoundDef.Named("Click")
                    };
                }
            }

            if (m_BlockIndirect_Avalable)
            {
                if (BlockIndirect_Active())
                {
                    yield return new Command_Action
                    {
                        action = SwitchIndirect,
                        icon = UI_INDIRECT_ON,
                        defaultLabel = "Block Indirect",
                        defaultDesc = "On",
                        activateSound = SoundDef.Named("Click")
                    };
                }
                else
                {
                    yield return new Command_Action
                    {
                        action = SwitchIndirect,
                        icon = UI_INDIRECT_OFF,
                        defaultLabel = "Block Indirect",
                        defaultDesc = "Off",
                        activateSound = SoundDef.Named("Click")
                    };
                }
            }

            if (m_InterceptDropPod_Avalable)
            {
                if (IntercepDropPod_Active())
                {
                    yield return new Command_Action
                    {
                        action = SwitchInterceptDropPod,
                        icon = UI_INTERCEPT_DROPPOD_ON,
                        defaultLabel = "Intercept DropPod",
                        defaultDesc = "On",
                        activateSound = SoundDef.Named("Click")
                    };
                }
                else
                {
                    yield return new Command_Action
                    {
                        action = SwitchInterceptDropPod,
                        icon = UI_INTERCEPT_DROPPOD_OFF,
                        defaultLabel = "Intercept DropPod",
                        defaultDesc = "Off",
                        activateSound = SoundDef.Named("Click")
                    };
                }
            }

            if (m_ShowVisually_Active)
            {
                yield return new Command_Action
                {
                    action = SwitchVisual,
                    icon = UI_SHOW_ON,
                    defaultLabel = "Show Visually",
                    defaultDesc = "Show",
                    activateSound = SoundDef.Named("Click")
                };
            }
            else
            {
                yield return new Command_Action
                {
                    action = SwitchVisual,
                    icon = UI_SHOW_OFF,
                    defaultLabel = "Show Visually",
                    defaultDesc = "Hide",
                    activateSound = SoundDef.Named("Click")
                };
            }

            yield return new Command_Action
            {
                action = ApplyUpgrades,
                icon = UI_LAUNCH_REPORT,
                defaultLabel = "Apply Upgrades",
                defaultDesc = "Apply Upgrades",
                activateSound = SoundDef.Named("Click")
            };
        }

        // Token: 0x06000026 RID: 38 RVA: 0x0000294C File Offset: 0x00000B4C
        public void ApplyUpgrades()
        {
            var source = from x in parent.Map.listerBuildings.allBuildingsColonist
                where x.Position.InHorDistOf(parent.Position, 1.6f)
                where x.TryGetComp<Comp_ShieldUpgrade>() != null
                select x;
            var building = source.FirstOrDefault(x => IsAvalableUpgrade(x));
            if (building != null)
            {
                m_AppliedUpgrades.Add(building);
                building.DeSpawn();
                Messages.Message("Applying Shield Upgrade: " + building.def.label, parent,
                    MessageTypeDefOf.PositiveEvent);
                return;
            }

            if ((from x in source
                where !IsAvalableUpgrade(x, true)
                select x).Any())
            {
                Messages.Message("No Valid Shield Upgrades Found.", parent, MessageTypeDefOf.RejectInput);
                return;
            }

            Messages.Message("No Shield Upgrades Found.", parent, MessageTypeDefOf.RejectInput);
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00002A4C File Offset: 0x00000C4C
        private bool IsAvalableUpgrade(Building buildingToCheck, bool ResultMessages = false)
        {
            var comp_ShieldUpgrade = buildingToCheck.TryGetComp<Comp_ShieldUpgrade>();
            if (comp_ShieldUpgrade == null)
            {
                if (ResultMessages)
                {
                    Messages.Message("Upgrade Comp Not Found, How did you even get here?.", buildingToCheck,
                        MessageTypeDefOf.RejectInput);
                }

                return false;
            }

            if (m_IdentifyFriendFoe_Avalable && comp_ShieldUpgrade.Properties.IdentifyFriendFoe)
            {
                if (ResultMessages)
                {
                    Messages.Message("Upgrade Contains IFF while shield already has it.", buildingToCheck,
                        MessageTypeDefOf.RejectInput);
                }

                return false;
            }

            if (SlowDischarge_Active && comp_ShieldUpgrade.Properties.SlowDischarge)
            {
                if (ResultMessages)
                {
                    Messages.Message("Upgrade for slow discharge while shield already has it.", buildingToCheck,
                        MessageTypeDefOf.RejectInput);
                }

                return false;
            }

            if (!m_InterceptDropPod_Avalable || !comp_ShieldUpgrade.Properties.DropPodIntercept)
            {
                return true;
            }

            if (ResultMessages)
            {
                Messages.Message("Upgrade for drop pod intercept while shield already has it.", buildingToCheck,
                    MessageTypeDefOf.RejectInput);
            }

            return false;
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002B0F File Offset: 0x00000D0F
        public void SwitchDirect()
        {
            m_BlockDirect_Requested = !m_BlockDirect_Requested;
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002B20 File Offset: 0x00000D20
        public void SwitchIndirect()
        {
            m_BlockIndirect_Requested = !m_BlockIndirect_Requested;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002B31 File Offset: 0x00000D31
        public void SwitchInterceptDropPod()
        {
            m_InterceptDropPod_Requested = !m_InterceptDropPod_Requested;
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002B42 File Offset: 0x00000D42
        private void SwitchVisual()
        {
            m_ShowVisually_Active = !m_ShowVisually_Active;
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00002B54 File Offset: 0x00000D54
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref m_FieldRadius_Requested, "m_FieldRadius_Requested");
            Scribe_Values.Look(ref m_BlockDirect_Requested, "m_BlockDirect_Requested");
            Scribe_Values.Look(ref m_BlockIndirect_Requested, "m_BlockIndirect_Requested");
            Scribe_Values.Look(ref m_InterceptDropPod_Requested, "m_InterceptDropPod_Requested");
            Scribe_Values.Look(ref m_IdentifyFriendFoe_Requested, "m_IdentifyFriendFoe_Requested");
            Scribe_Values.Look(ref m_RechargeTickDelayInterval, "m_shieldRechargeTickDelay");
            Scribe_Values.Look(ref m_RecoverWarmupDelayTicks, "m_shieldRecoverWarmup");
            Scribe_Values.Look(ref m_ShowVisually_Active, "m_ShowVisually_Active", true);
            Scribe_Values.Look(ref m_ColourRed, "m_colourRed");
            Scribe_Values.Look(ref m_ColourGreen, "m_colourGreen");
            Scribe_Values.Look(ref m_ColourBlue, "m_colourBlue");
            Scribe_Values.Look(ref m_WarmupTicksRemaining, "m_WarmupTicksRemaining");
            Scribe_Values.Look(ref m_CurrentStatus, "m_CurrentStatus");
            Scribe_Values.Look(ref m_FieldIntegrity_Current, "m_FieldIntegrity_Current");
            Scribe_Collections.Look(ref m_AppliedUpgrades, "m_AppliedUpgrades", LookMode.Deep, Array.Empty<object>());
        }
    }
}