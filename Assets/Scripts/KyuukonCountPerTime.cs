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
    float updateIntervalSec;
    float prevTime;
    float currentCount;

    public KyuukonCountPerTime(float countIntervalSec, float updateIntervalSec)
    {
        this.countIntervalSec = countIntervalSec;
        this.updateIntervalSec = updateIntervalSec;
        list = new LinkedList<KyuukonCountUnit>();
        prevTime = Time.time;
        currentCount = 0;
    }

    public float GetCount(int kyuukonTotalCount)
    {
        float currentTime = Time.time;
        if (currentTime - prevTime < updateIntervalSec)
        {
            return currentCount;
        }

        while (list.Count >= MAX_COUNT || (list.Count > 0 && list.First().Time < currentTime - countIntervalSec))
        {
            list.RemoveFirst();
        }

        list.AddLast(new KyuukonCountUnit(kyuukonTotalCount, currentTime));

        prevTime = currentTime;
        currentCount = (kyuukonTotalCount - list.First().Count) / countIntervalSec;
        return currentCount;
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
