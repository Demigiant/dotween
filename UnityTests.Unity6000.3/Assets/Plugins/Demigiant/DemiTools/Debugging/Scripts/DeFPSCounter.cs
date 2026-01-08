// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2025/12/17 - Based on my own HOFpsGadget from 2012/10/05

using System;
using System.Collections;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Demigiant.DemiTools.Debugging
{
    /// <summary>
    /// Shows framerate and eventual memory info during runtime.
    /// Can also force a specific framerate
    /// </summary>
    [AddComponentMenu("DemiTools/DeFPSCounter")]
    public class DeFPSCounter : MonoBehaviour
    {
        [Header("FPS/Memory")]
        [Range(0, 60f), Tooltip("Delay between this Component's startup and when it begins calculating data")]
        public int startupDelay;
        [Range(0.01f, 1f), Tooltip("Delay between each update of the FPS calculation")]
        public float updateDelay = 0.5f;
        [Tooltip("If TRUE also shows memory info")]
        public bool showMemory;
        [Tooltip("Alignment of the info gadget")]
        public TextAlignment alignment = TextAlignment.Right;
        [Range(0, 10), Tooltip("Distance from the borders of the screen")]
        public int margin = 2;
        [Range(5, 20), Tooltip("Font size")]
        public int fontSize = 11;
        
        [Header("FrameRate")]
        [Range(0, 500), Tooltip("If different from 0 forces the given framerate via Application.targetFrameRate")]
        public int limitFrameRate = -1;

        bool _paused = true;
        float _accum;
        int _frames;
        int _totFps;
        string _avrgFps;
        float _time;
        float _timeleft;
        string _fps = "";
        string _memory = "";
        readonly StringBuilder _msg = new StringBuilder(40);
        bool _stylesSet;
        Vector2 _boxSize;
        int _boxHalfWidth;
        GUIStyle _fpsStyle;

        #region Unity

        IEnumerator Start()
        {
            if (limitFrameRate > 0) Application.targetFrameRate = limitFrameRate;
            if (startupDelay > 0) yield return new WaitForSeconds(startupDelay);
            _paused = false;
            _timeleft = updateDelay;
        }

        void Update()
        {
            if (_paused) return;
            
            // Calculate FPS to show
            _timeleft -= Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            ++_frames;
            if (_timeleft <= 0) {
                _fps = Mathf.Round(_accum / _frames).ToString(CultureInfo.InvariantCulture);
                _timeleft = updateDelay;
                _accum = 0;
                _frames = 0;
                if (showMemory) _memory = string.Format("{0:#,0}", System.GC.GetTotalMemory(false));
            }
            // Calculate average
            if (Time.deltaTime > 0) {
                _time += Time.timeScale / Time.deltaTime;
                _totFps++;
                _avrgFps = Mathf.Round(_time / _totFps).ToString(CultureInfo.InvariantCulture);
                // Message
                _msg.Remove(0, _msg.Length);
                _msg.Append("FPS: ").Append(_fps).Append(" / ").Append(_avrgFps);
                if (showMemory) _msg.Append("\nMEM: ").Append(_memory);
            }
        }

        void OnGUI()
        {
            if (_paused) return;
            
            if (!_stylesSet) {
                _stylesSet = true;
                _fpsStyle = new GUIStyle(GUI.skin.box)
                {
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(4, 4, 2 ,2),
                    alignment = TextAnchor.UpperLeft,
                    normal = { textColor = Color.white },
                    fontSize = fontSize
                };
                _boxSize = _fpsStyle.CalcSize(new GUIContent("FPS: 9999/9999" + (showMemory ? "\nMEM: 9,999,999,999" : "")));
                _boxHalfWidth = (int)(_boxSize.x * 0.5f);
            }

            bool clicked;
            switch (alignment) {
            case TextAlignment.Left:
                clicked = GUI.Button(new Rect(margin, margin, _boxSize.x, _boxSize.y), _msg.ToString(), _fpsStyle);
                break;
            case TextAlignment.Center:
                clicked = GUI.Button(new Rect(Screen.width * 0.5f - _boxHalfWidth, margin, _boxSize.x, _boxSize.y), _msg.ToString(), _fpsStyle);
                break;
            default:
                clicked = GUI.Button(new Rect(Screen.width - _boxSize.x - margin, margin, _boxSize.x, _boxSize.y), _msg.ToString(), _fpsStyle);
                break;
            }
            if (clicked) ResetFps();
        }

        #endregion

        #region Public Methods

        public void ResetFps()
        {
            _time = _accum = _frames = _totFps = 0;
            _timeleft = updateDelay;
        }
        
        #endregion
    }
}