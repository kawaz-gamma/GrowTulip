using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TulipList
{
    private List<Tulip> list;

    public int Count { get { return list.Count; } }

    public TulipList()
    {
        list = new List<Tulip>();
    }

    public void Add(Tulip tulip)
    {
        tulip.id = list.Count;
        list.Add(tulip);
    }

    public void Remove(Tulip tulip)
    {
        int id = tulip.id;

        // 存在しなければ打切り
        if (id < 0 || id >= list.Count)
        {
            return;
        }

        // O(1)で消す  
        int lastIndex = list.Count - 1;
        if (id == lastIndex)
        {
            list.RemoveAt(lastIndex);
        }
        else
        {
            Tulip last = list[lastIndex];
            list[id] = list[lastIndex];
            list.RemoveAt(lastIndex);
            list[id].id = id;
        }
        tulip.id = -1;
    }

    public Tulip this[int i]
    {
        set { list[i] = value; }
        get { return list[i]; }
    }
}