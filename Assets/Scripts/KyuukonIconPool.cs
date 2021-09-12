using System;
using System.Collections.Generic;
using UnityEngine;

public class KyuukonIconPool:MonoBehaviour
{
    [SerializeField]
    GameObject objectOrigin;

    private Queue<GameObject> queue = new Queue<GameObject>();

    public Transform InstantiateObject(Transform parent)
    {
        if (queue.Count > 0)
        {
            GameObject g = queue.Dequeue();
            g.SetActive(true);
            return g.transform;
        }
        else
        {
            GameObject g = Instantiate(objectOrigin, transform);
            g.transform.SetParent(parent);

            return g.transform;
        }
    }

    public void DestroyObject(GameObject obj)
    {
        obj.SetActive(false);
        queue.Enqueue(obj);
    }
}