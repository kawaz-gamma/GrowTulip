using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ranking
{
    public abstract class BaseScore : MonoBehaviour
    {
        [SerializeField]
        private ScoreType type;
        public ScoreType ScoreType => type;

        public int Score { set; get; }
    }
}