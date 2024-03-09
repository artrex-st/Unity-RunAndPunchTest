using System;
using TMPro;
using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
        private TextMeshProUGUI FpsCount => GetComponent<TextMeshProUGUI>();
        private int _frameCount;
        private float _time;
        private const float PollingTime = 1f;

        private void Update()
        {
            FeedFpsCounter();
        }

        private void FeedFpsCounter()
        {
            _time += Time.deltaTime;
            _frameCount++;

            if (_time >= PollingTime)
            {
                int frameRate = Mathf.RoundToInt(_frameCount / _time);
                FpsCount.text = $"FPS:{frameRate}";
                _time -= PollingTime;
                _frameCount = 0;
            }
        }
    }
}
