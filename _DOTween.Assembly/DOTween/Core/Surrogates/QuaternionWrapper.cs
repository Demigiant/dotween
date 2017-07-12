#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 17:53

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Surrogates
{
    public struct QuaternionWrapper
    {
        public Quaternion value;

        public QuaternionWrapper(Quaternion value)
        {
            this.value = value;
        }

        public static implicit operator Quaternion(QuaternionWrapper v)
        {
            return v.value;
        }

        public static implicit operator QuaternionWrapper(Quaternion v)
        {
            return new QuaternionWrapper(v);
        }
    }
}
#endif