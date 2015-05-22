// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/29 11:12
namespace DG.Tweening
{
    /// <summary>
    /// Type of scramble to apply to string tweens
    /// </summary>
    public enum ScrambleMode
    {
        /// <summary>
        /// No scrambling of characters
        /// </summary>
        None,
        /// <summary>
        /// A-Z + a-z + 0-9 characters
        /// </summary>
        All,
        /// <summary>
        /// A-Z characters
        /// </summary>
        Uppercase,
        /// <summary>
        /// a-z characters
        /// </summary>
        Lowercase,
        /// <summary>
        /// 0-9 characters
        /// </summary>
        Numerals,
        /// <summary>
        /// Custom characters
        /// </summary>
        Custom
    }
}