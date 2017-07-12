#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 17:50

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    public struct Vector2Wrapper
    {
        public Vector2 value;

        public Vector2Wrapper(Vector2 value)
        {
            this.value = value;
        }
        public Vector2Wrapper(float x, float y)
        {
            this.value = new Vector2(x, y);
        }

        public static implicit operator Vector2(Vector2Wrapper v)
        {
            return v.value;
        }

        public static implicit operator Vector2Wrapper(Vector2 v)
        {
            return new Vector2Wrapper(v);
        }
    }
}
#endif