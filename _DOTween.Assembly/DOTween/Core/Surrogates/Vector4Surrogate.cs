#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/14 12:10

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    /// <summary>
    /// A surrogate for Vector2/3/4 values to work around Unity's bug when trying to cast to
    /// a Vector2/3/4 plugin on WP8.1
    /// </summary>
    public struct Vector4Surrogate
    {
        public float x, y, z, w;

        public float magnitude {
            get { return Mathf.Sqrt(x * x + y * y + z * z + w * w); }
        }

        public Vector4Surrogate(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #region Operations

        public static Vector4Surrogate operator +(Vector4Surrogate v1, Vector4Surrogate v2)
        {
            return new Vector4Surrogate(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        public static Vector4Surrogate operator -(Vector4Surrogate v1, Vector4Surrogate v2)
        {
            return new Vector4Surrogate(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        public static Vector4Surrogate operator *(Vector4Surrogate v1, float f)
        {
            return new Vector4Surrogate(v1.x * f, v1.y * f, v1.z * f, v1.w * f);
        }

//        public static Vector4Surrogate operator /(Vector4Surrogate v1, float f)
//        {
//            return new Vector4Surrogate(v1.x / f, v1.y / f, v1.z / f, v1.w / f);
//        }

        #endregion

        #region Comparisons

        //        public static bool operator <(Vector4Surrogate v1, Vector4Surrogate v2)
//        {
//            return v1.magnitude < v2.magnitude;
//        }
//
//        public static bool operator <=(Vector4Surrogate v1, Vector4Surrogate v2)
//        {
//            return v1.magnitude <= v2.magnitude;
//        }
//
//        public static bool operator >(Vector4Surrogate v1, Vector4Surrogate v2)
//        {
//            return v1.magnitude > v2.magnitude;
//        }
//
//        public static bool operator >=(Vector4Surrogate v1, Vector4Surrogate v2)
//        {
//            return v1.magnitude >= v2.magnitude;
//        }
//
//        public static bool operator ==(Vector4Surrogate v1, Vector4Surrogate v2)
//        {
//            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;
//        }
//
//        public static bool operator !=(Vector4Surrogate v1, Vector4Surrogate v2)
//        {
//            return !(v1 == v2);
        //        }

        #endregion

        #region Conversions

//        public static explicit operator Vector2(Vector4Surrogate v)
//        {
//            return new Vector2(v.x, v.y);
//        }
//
//        public static explicit operator Vector3(Vector4Surrogate v)
//        {
//            return new Vector3(v.x, v.y, v.z);
//        }

        public static explicit operator Vector4(Vector4Surrogate v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        #endregion
    }
}
#endif