// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/11 11:34
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    // USING THIS PLUGIN WILL GENERATE GC ALLOCATIONS
    public class StringPlugin : ABSTweenPlugin<string, string, StringOptions>
    {
        static readonly StringBuilder _Buffer = new StringBuilder();
        static readonly List<Char> _OpenedTags = new List<char>(); // Opened tags that need to be closed at the end, stored by first character required in closing tag

        public override void SetFrom(TweenerCore<string, string, StringOptions> t, bool isRelative)
        {
            string prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = prevEndVal;
            t.setter(t.startValue);
        }

        public override void Reset(TweenerCore<string, string, StringOptions> t)
        {
            t.startValue = t.endValue = t.changeValue = null;
        }

        public override string ConvertToStartValue(TweenerCore<string, string, StringOptions> t, string value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<string, string, StringOptions> t)
        {
            // Do nothing (endValue stays the same)
        }

        public override void SetChangeValue(TweenerCore<string, string, StringOptions> t)
        {
            t.changeValue = t.endValue;

            // Store no-tags versions of values
            t.plugOptions.startValueStrippedLength = Regex.Replace(t.startValue, @"<[^>]*>", "").Length;
            t.plugOptions.changeValueStrippedLength = Regex.Replace(t.changeValue, @"<[^>]*>", "").Length;
        }

        public override float GetSpeedBasedDuration(StringOptions options, float unitsXSecond, string changeValue)
        {
//            float res = changeValue.Length / unitsXSecond;
            float res = (options.richTextEnabled ? options.changeValueStrippedLength : changeValue.Length) / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        // ChangeValue is the same as endValue in this plugin
        public override void EvaluateAndApply(StringOptions options, Tween t, bool isRelative, DOGetter<string> getter, DOSetter<string> setter, float elapsed, string startValue, string changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            _Buffer.Remove(0, _Buffer.Length);

            // Incremental works only with relative tweens (otherwise the tween makes no sense)
            // Sequence with Incremental loops have no effect here (why should they?)
            if (isRelative && t.loopType == LoopType.Incremental) {
                int iterations = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
                if (iterations > 0) {
                    _Buffer.Append(startValue);
                    for (int i = 0; i < iterations; ++i) _Buffer.Append(changeValue);
                    startValue = _Buffer.ToString();
                    _Buffer.Remove(0, _Buffer.Length);
                }
            }

            int startValueLen = options.richTextEnabled ? options.startValueStrippedLength : startValue.Length;
            int changeValueLen = options.richTextEnabled ? options.changeValueStrippedLength : changeValue.Length;
            int len = (int)Math.Round(changeValueLen * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod));
            if (len > changeValueLen) len = changeValueLen;
            else if (len < 0) len = 0;

            if (isRelative) {
                _Buffer.Append(startValue);
                if (options.scrambleMode != ScrambleMode.None) {
                    setter(Append(changeValue, 0, len, options.richTextEnabled).AppendScrambledChars(changeValueLen - len, ScrambledCharsToUse(options)).ToString());
                    return;
                }
                setter(Append(changeValue, 0, len, options.richTextEnabled).ToString());
                return;
            }

            if (options.scrambleMode != ScrambleMode.None) {
                setter(Append(changeValue, 0, len, options.richTextEnabled).AppendScrambledChars(changeValueLen - len, ScrambledCharsToUse(options)).ToString());
                return;
            }

            int diff = startValueLen - changeValueLen;
            int startValueMaxLen = startValueLen;
            if (diff > 0) {
                // String to be replaced is longer than endValue: remove parts of it while tweening
                float perc = (float)len / changeValueLen;
                startValueMaxLen -= (int)(startValueMaxLen * perc);
            } else startValueMaxLen -= len;
            Append(changeValue, 0, len, options.richTextEnabled);
            if (len < changeValueLen && len < startValueLen) Append(startValue, len, options.richTextEnabled ? len + startValueMaxLen : startValueMaxLen, options.richTextEnabled);
            setter(_Buffer.ToString());
        }

        // Manages eventual rich text, if enabled, readding tags to the given string and closing them when necessary
        StringBuilder Append(string value, int startIndex, int length, bool richTextEnabled)
        {
            if (!richTextEnabled) {
                _Buffer.Append(value, startIndex, length);
                return _Buffer;
            }

            _OpenedTags.Clear();
            const string tagMatch = @"<.*?(>)";
            const string closeTagMatch = @"(</).*?>";
            bool hasOpenTag = false;
            int fullLen = value.Length;
            int i;
            for (i = 0; i < length; ++i) {
                char c = value[i];
                if (c == '<') {
                    bool hadOpenTag = hasOpenTag;
                    char nextChar = value[i + 1];
                    hasOpenTag = !(i < fullLen - 1 && nextChar == '/');
                    if (hasOpenTag) _OpenedTags .Add(nextChar == '#' ? 'c' : nextChar);
                    else _OpenedTags.RemoveAt(_OpenedTags.Count - 1);
                    string s = value.Substring(i);
                    Match m = Regex.Match(s, tagMatch);
                    if (m.Success) {
                        if (!hasOpenTag && !hadOpenTag) {
                            // We have a closing tag without an opening tag, try to find previous correct opening tag and apply it
                            char closingTagFirstChar = value[i + 1];
                            char[] openingTagLookouts;
                            if (closingTagFirstChar == 'c') openingTagLookouts = new[] { '#', 'c' };
                            else openingTagLookouts = new[] { closingTagFirstChar };
                            int t = i - 1;
                            while (t > -1) {
                                if (value[t] == '<' && value[t + 1] != '/' && Array.IndexOf(openingTagLookouts, value[t + 2]) != -1) {
                                    _Buffer.Insert(0, value.Substring(t, value.IndexOf('>', t) + 1 - t));
                                    break;
                                }
                                t--;
                            }
                        }
                        // Append tag and increase loop length to match
                        _Buffer.Append(m.Value);
                        int add = m.Groups[1].Index + 1;
                        length += add;
                        startIndex += add;
                        i += add - 1;
                    }
                } else if (i >= startIndex) _Buffer.Append(c);
            }
            if (_OpenedTags.Count > 0 && i < fullLen - 1) {
                string next;
                while (_OpenedTags.Count > 0 && i < fullLen - 1) {
                    // Last open tag was not closed: find next close tag and apply it
                    next = value.Substring(i);
                    Match m = Regex.Match(next, closeTagMatch);
                    if (m.Success) {
                        // Append only if it's the correct closing tag
                        if (m.Value[2] == _OpenedTags[_OpenedTags.Count - 1]) {
                            _Buffer.Append(m.Value);
                            _OpenedTags.RemoveAt(_OpenedTags.Count - 1);
                        }
                        i += m.Value.Length;
                    } else break;
                }
            }
            return _Buffer;
        }

        char[] ScrambledCharsToUse(StringOptions options)
        {
            switch (options.scrambleMode) {
            case ScrambleMode.Uppercase:
                return StringPluginExtensions.ScrambledCharsUppercase;
            case ScrambleMode.Lowercase:
                return StringPluginExtensions.ScrambledCharsLowercase;
            case ScrambleMode.Numerals:
                return StringPluginExtensions.ScrambledCharsNumerals;
            case ScrambleMode.Custom:
                return options.scrambledChars;
            default:
                return StringPluginExtensions.ScrambledCharsAll;
            }
        }
    }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // ||| CLASS |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    internal static class StringPluginExtensions
    {
        public static readonly char[] ScrambledCharsAll = new[] {
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','x','y','z',
            '1','2','3','4','5','6','7','8','9','0'
        };
        public static readonly char[] ScrambledCharsUppercase = new[] {
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','X','Y','Z'
        };
        public static readonly char[] ScrambledCharsLowercase = new[] {
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','x','y','z'
        };
        public static readonly char[] ScrambledCharsNumerals = new[] {
            '1','2','3','4','5','6','7','8','9','0'
        };
        static int _lastRndSeed;

        static StringPluginExtensions()
        {
            ScrambledCharsAll.ScrambleChars();
            ScrambledCharsUppercase.ScrambleChars();
            ScrambledCharsLowercase.ScrambleChars();
            ScrambledCharsNumerals.ScrambleChars();
        }

        internal static void ScrambleChars(this char[] chars)
        {
            // Shuffle chars (uses Knuth shuggle algorithm)
            int len = chars.Length;
            for (int i = 0; i < len; i++) {
                char tmp = chars[i];
                int r = Random.Range(i, len);
                chars[i] = chars[r];
                chars[r] = tmp;
            }
        }

        internal static StringBuilder AppendScrambledChars(this StringBuilder buffer, int length, char[] chars)
        {
            if (length <= 0) return buffer;

            // Make sure random seed is different from previous one used
            int len = chars.Length;
            int rndSeed = _lastRndSeed;
            while (rndSeed == _lastRndSeed) {
                rndSeed = Random.Range(0, len);
            }
            _lastRndSeed = rndSeed;
            // Append
            for (int i = 0; i < length; ++i) {
                if (rndSeed >= len) rndSeed = 0;
                buffer.Append(chars[rndSeed]);
                rndSeed += 1;
            }
            return buffer;
        }
    }
}