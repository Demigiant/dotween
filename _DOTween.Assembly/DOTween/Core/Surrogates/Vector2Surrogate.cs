#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/14 12:42

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    /// <summary>
    /// A surrogate for Vector2/3/4 values to work around Unity's bug when trying to cast to
    /// a Vector2/3/4 plugin on WP8.1
    /// </summary>
    public struct Vector2Surrogate
    {
        public float x, y;

        public float magnitude {
            get { return Mathf.Sqrt(x * x + y * y); }
        }

        public Vector2Surrogate(float x, float y)
            : this()
        {
            this.x = x;
            this.y = y;
        }

        #region Operations

        public static Vector2Surrogate operator +(Vector2Surrogate v1, Vector2Surrogate v2)
        {
            return new Vector2Surrogate(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2Surrogate operator -(Vector2Surrogate v1, Vector2Surrogate v2)
        {
            return new Vector2Surrogate(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2Surrogate operator *(Vector2Surrogate v1, float f)
        {
            return new Vector2Surrogate(v1.x * f, v1.y * f);
        }

        #endregion

        #region Conversions

        public static explicit operator Vector3(Vector2Surrogate v)
        {
            return new Vector3(v.x, v.y);
        }

        #endregion
    }
}
#endif