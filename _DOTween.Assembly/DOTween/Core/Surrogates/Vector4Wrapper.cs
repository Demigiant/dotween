#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 17:51

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    public struct Vector4Wrapper
    {
        public Vector4 value;

        public Vector4Wrapper(Vector4 value)
        {
            this.value = value;
        }

        public static implicit operator Vector4(Vector4Wrapper v)
        {
            return v.value;
        }

        public static implicit operator Vector4Wrapper(Vector4 v)
        {
            return new Vector4Wrapper(v);
        }
    }
}
#endif