#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 17:52

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    public struct ColorWrapper
    {
        public Color value;

        public ColorWrapper(Color value)
        {
            this.value = value;
        }

        public static implicit operator Color(ColorWrapper v)
        {
            return v.value;
        }

        public static implicit operator Color32(ColorWrapper v)
        {
            return v.value;
        }

        public static implicit operator ColorWrapper(Color v)
        {
            return new ColorWrapper(v);
        }

        public static implicit operator ColorWrapper(Color32 v)
        {
            return new ColorWrapper(v);
        }
    }
}
#endif