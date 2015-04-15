#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/15 18:44

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    public struct QuaternionSurrogate
    {
        public float x, y, z, w;

        public QuaternionSurrogate(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #region Operations

        public static QuaternionSurrogate operator *(QuaternionSurrogate lhs, QuaternionSurrogate rhs)
        {
            return new QuaternionSurrogate(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        public static Vector3Surrogate operator *(QuaternionSurrogate rotation, Vector3Surrogate point)
        {
            float num = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num4 = rotation.x * num;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vector3Surrogate result;
            result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
            result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
            result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
            return result;
        }

        #endregion

        #region Conversions

        public static implicit operator Quaternion(QuaternionSurrogate v)
        {
            return new Quaternion(v.x, v.y, v.z, v.w);
        }

        public static implicit operator QuaternionSurrogate(Quaternion v)
        {
            return new QuaternionSurrogate(v.x, v.y, v.z, v.w);
        }

        #endregion
    }
}
#endif