// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/25 12:37

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening
{
    /// <summary>
    /// Struct that stores two colors (used for LineRenderer tweens)
    /// </summary>
    public struct Color2
    {
        public Color ca, cb;

        public Color2(Color ca, Color cb)
        {
            this.ca = ca;
            this.cb = cb;
        }

        public static Color2 operator +(Color2 c1, Color2 c2)
        {
            return new Color2(c1.ca + c2.ca, c1.cb + c2.cb);
        }

        public static Color2 operator -(Color2 c1, Color2 c2)
        {
            return new Color2(c1.ca - c2.ca, c1.cb - c2.cb);
        }

        public static Color2 operator *(Color2 c1, float f)
        {
            return new Color2(c1.ca * f, c1.cb * f);
        }
    }
}