using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間を計測するクラス
/// </summary>

namespace TadaLib
{
    public class Timer
    {
        private float startTimeSec_;
        private float limitTimeSec_; // 制限時間

        public Timer(float limitTimeSec)
        {
            limitTimeSec_ = limitTimeSec;
            startTimeSec_ = Time.time;
        }

        public void TimeReset()
        {
            startTimeSec_ = Time.time;
        }

        public bool IsTimeout()
        {
            return Time.time - startTimeSec_ >= limitTimeSec_;
        }

        // 時間を巻き戻す
        public void TimeReverse(float time)
        {
            startTimeSec_ = Mathf.Min(startTimeSec_ + time, Time.time);
        }

        public float GetTime()
        {
            return Time.time - startTimeSec_;
        }
    }
}