using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ranking
{
    public class KyuukonRateScore : BaseScore
    {
        private void Start()
        {
            Score = 0;
        }

        private void Update()
        {
            Score = (int)GameManager.KyuukonPerTime;
        }
    }
}