#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/14 12:37

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    /// <summary>
    /// A surrogate for Vector2/3/4 values to work around Unity's bug when trying to cast to
    /// a Vector2/3/4 plugin on WP8.1
    /// </summary>
    public struct Vector3Surrogate
    {
        public float x, y, z;

        public float magnitude {
            get { return Mathf.Sqrt(x * x + y * y + z * z); }
        }

        public Vector3Surrogate(float x, float y, float z)
            : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region Operations

        public static Vector3Surrogate operator +(Vector3Surrogate v1, Vector3Surrogate v2)
        {
            return new Vector3Surrogate(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3Surrogate operator -(Vector3Surrogate v1, Vector3Surrogate v2)
        {
            return new Vector3Surrogate(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3Surrogate operator *(Vector3Surrogate v1, float f)
        {
            return new Vector3Surrogate(v1.x * f, v1.y * f, v1.z * f);
        }

        #endregion

        #region Conversions

        public static explicit operator Vector3(Vector3Surrogate v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        #endregion
    }
}
#endif