using System;
using UnityEngine;
using Verse;
using Random = UnityEngine.Random;

namespace Jaxxa.EnhancedDevelopment.Shields.Shields.Utilities
{
    // Token: 0x0200000B RID: 11
    internal class VectorsUtils
    {
        // Token: 0x0600004B RID: 75 RVA: 0x00003338 File Offset: 0x00001538
        public static double EuclDist(IntVec3 a, IntVec3 b)
        {
            return Math.Sqrt(((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y)) + ((a.z - b.z) * (a.z - b.z)));
        }

        // Token: 0x0600004C RID: 76 RVA: 0x0000339E File Offset: 0x0000159E
        public static double VectorSize(IntVec3 a)
        {
            return Math.Sqrt((a.x * a.x) + (a.y * a.y) + (a.z * a.z));
        }

        // Token: 0x0600004D RID: 77 RVA: 0x000033CF File Offset: 0x000015CF
        public static IntVec3 vecFromAngle(float angle1, float angle2, float r)
        {
            return new IntVec3((int) (r * Math.Sin(angle1) * Math.Cos(angle2)),
                (int) (r * Math.Sin(angle1) * Math.Sin(angle2)), (int) (r * Math.Cos(angle1)));
        }

        // Token: 0x0600004E RID: 78 RVA: 0x00003408 File Offset: 0x00001608
        public static double vectorAngleA(IntVec3 a)
        {
            var num = VectorSize(a);
            return Math.Acos(a.z / num);
        }

        // Token: 0x0600004F RID: 79 RVA: 0x0000342A File Offset: 0x0000162A
        public static IntVec3 randomDirection(float r)
        {
            return vecFromAngle(Random.Range(0, 360), 0f, r);
        }

        // Token: 0x06000050 RID: 80 RVA: 0x00003443 File Offset: 0x00001643
        public static Vector3 IntVecToVec(IntVec3 from)
        {
            return new Vector3(from.x, from.y, from.z);
        }

        // Token: 0x06000051 RID: 81 RVA: 0x0000345F File Offset: 0x0000165F
        public static IntVec3 VecToIntVec(Vector3 from)
        {
            return new IntVec3((int) from.x, (int) from.y, (int) from.z);
        }
    }
}