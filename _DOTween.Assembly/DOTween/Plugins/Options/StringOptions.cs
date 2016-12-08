// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/13 16:37
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Options
{
    public struct StringOptions : IPlugOptions
    {
        public bool richTextEnabled;
        public ScrambleMode scrambleMode;
        public char[] scrambledChars; // If empty uses default scramble characters

        // Stored by StringPlugin
        internal int startValueStrippedLength, changeValueStrippedLength; // No-tag lengths of start and change value

        public void Reset()
        {
            richTextEnabled = false;
            scrambleMode = ScrambleMode.None;
            scrambledChars = null;
            startValueStrippedLength = changeValueStrippedLength = 0;
        }
    }
}