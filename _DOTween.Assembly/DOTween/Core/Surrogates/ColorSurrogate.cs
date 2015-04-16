using UnityEngine;

#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/15 12:10

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    public struct ColorSurrogate
    {
        public float r, g, b, a;

        public ColorSurrogate(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        #region Operations

        public static ColorSurrogate operator +(ColorSurrogate v1, ColorSurrogate v2)
        {
            return new ColorSurrogate(v1.r + v2.r, v1.g + v2.g, v1.b + v2.b, v1.a + v2.a);
        }

        public static ColorSurrogate operator -(ColorSurrogate v1, ColorSurrogate v2)
        {
            return new ColorSurrogate(v1.r - v2.r, v1.g - v2.g, v1.b - v2.b, v1.a - v2.a);
        }

        public static ColorSurrogate operator *(ColorSurrogate v1, float f)
        {
            return new ColorSurrogate(v1.r * f, v1.g * f, v1.b * f, v1.a * f);
        }

        #endregion

        #region Conversions

        public static implicit operator Color(ColorSurrogate v)
        {
            return new Color(v.r, v.g, v.b, v.a);
        }

        public static implicit operator ColorSurrogate(Color v)
        {
            return new ColorSurrogate(v.r, v.g, v.b, v.a);
        }

        public static implicit operator Color32(ColorSurrogate v)
        {
            return new Color32((byte)(Mathf.Clamp01(v.r) * 255f), (byte)(Mathf.Clamp01(v.g) * 255f), (byte)(Mathf.Clamp01(v.b) * 255f), (byte)(Mathf.Clamp01(v.a) * 255f));
        }

        public static implicit operator ColorSurrogate(Color32 v)
        {
            return new ColorSurrogate((float)v.r / 255f, (float)v.g / 255f, (float)v.b / 255f, (float)v.a / 255f);
        }

        #endregion
    }
}
#endif