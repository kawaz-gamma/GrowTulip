using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class KyuukonCountPerTime
{
    const int MAX_COUNT = 1000;
    LinkedList<KyuukonCountUnit> list;
    float countIntervalSec;

    public float GetCount(int kyuukonTotalCount)
    {
        float currentTime = Time.time;
        while (list.Count >= MAX_COUNT || (list.Count > 0 && list.First().Time < currentTime - countIntervalSec))
        {
            list.RemoveFirst();
        }

        list.AddLast(new KyuukonCountUnit(kyuukonTotalCount, currentTime));
        return kyuukonTotalCount - list.First().Count;
    }

    public KyuukonCountPerTime(float countIntervalSec)
    {
        this.countIntervalSec = countIntervalSec;
    }
}

class KyuukonCountUnit
{
    public readonly int Count;
    public readonly float Time;

    public KyuukonCountUnit(int count,float time)
    {
        Count = count;
        Time = time;
    }
}
