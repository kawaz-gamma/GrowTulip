using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ranking
{
    public class ReloadTimer
    {
        private float initIntervalSec_;
        private float accelIntervalSec_;
        private float maxIntervalSec_;

        private TadaLib.Timer timer_;
        private float intervalSec_;

        public ReloadTimer(float initIntervalSec, float accelIntervalSec, float maxIntervalSec)
        {
            initIntervalSec_ = initIntervalSec;
            accelIntervalSec_ = accelIntervalSec;
            maxIntervalSec_ = maxIntervalSec;

            timer_ = new TadaLib.Timer(initIntervalSec);

            intervalSec_ = initIntervalSec_;
        }

        public void Proc()
        {
            intervalSec_ = Mathf.Min(intervalSec_ + accelIntervalSec_ * Time.deltaTime, maxIntervalSec_);

            if (Input.GetMouseButtonDown(0))
            {
                intervalSec_ = initIntervalSec_;
            }
        }

        public bool IsTimeout()
        {
            return timer_.IsTimeout();
        }

        public void TimeReset()
        {
            timer_ = new TadaLib.Timer(intervalSec_);
        }
    }
}