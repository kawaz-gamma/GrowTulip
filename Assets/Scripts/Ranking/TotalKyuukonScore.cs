using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ranking
{
    public class TotalKyuukonScore : BaseScore
    {
        private void Start()
        {
            Score = 0;
        }

        private void Update()
        {
            Score = GameManager.totalKyuukonCount;
        }
    }
}