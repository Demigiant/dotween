// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/04/19 22:08
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Globalization;
using UnityEngine;

namespace DG.DOTweenEditor
{
    internal static class EditorVersion
    {
        /// <summary>Full major version + first minor version (ex: 2018.1f)</summary>
        public static readonly float Version;
        /// <summary>Major version</summary>
        public static readonly int MajorVersion;
        /// <summary>First minor version (ex: in 2018.1 it would be 1)</summary>
        public static readonly int MinorVersion;

        static EditorVersion()
        {
            string sVersion = Application.unityVersion;
            string sMajor, sMinor;
            int dotIndex = sVersion.IndexOf('.');
            if (dotIndex == -1) {
                MajorVersion = int.Parse(sVersion);
                Version = MajorVersion;
            } else {
                sMajor = sVersion.Substring(0, dotIndex);
                MajorVersion = int.Parse(sMajor);
                // Remove and ignore extra minor versions dots
                sMinor = sVersion.Substring(dotIndex + 1);
                dotIndex = sMinor.IndexOf('.');
                if (dotIndex != -1) sMinor = sMinor.Substring(0, dotIndex);
                MinorVersion = int.Parse(sMinor);
                if (!float.TryParse(sMajor + '.' + sMinor, NumberStyles.Float, CultureInfo.InvariantCulture, out Version)) {
                    // There was a bug with Unity 2018.3.6 where culture didn't allow parse and the above row should solve it,
                    // but Imma leave this try-catch just for safety
                    Debug.LogWarning(string.Format("DOTweenEditor.EditorVersion ► Error when detecting Unity Version from \"{0}.{1}\"", sMajor, sMinor));
                    Version = 2018.3f;
                }
            }
        }
    }
}