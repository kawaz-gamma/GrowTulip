using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soujiki : MonoBehaviour
{
    public static float speed = 1f;
    public static float speedMag = 1.5f;
    [SerializeField]
    float interval;
    SoujikiState state = SoujikiState.Stop;
    Tulip target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case SoujikiState.Stop:
                if (GameManager.instance.tulipList.Count > 0)
                {
                    //target = GameManager.instance.tulipList[Random.Range(0, GameManager.instance.tulipList.Count)];
                    // 一番近いチューリップを探す
                    float distance = float.MaxValue;
                    foreach (Tulip tulip in GameManager.instance.tulipList)
                    {
                        // 乱数で抜ける
                        //if (Random.Range(0, 2) == 0) continue;
                        float tmp = Vector3.SqrMagnitude(tulip.transform.position - transform.position);
                        if (tmp < distance)
                        {
                            distance = tmp;
                            target = tulip;
                        }
                    }
                    if (target != null)
                    {
                        state = SoujikiState.Moving;
                        GameManager.instance.tulipList.Remove(target);
                    }
                }
                break;
            case SoujikiState.Moving:
                if (target == null)
                {
                    state = SoujikiState.Stop;
                    break;
                }
                Vector3 dir = target.transform.position - transform.position;
                if (dir.magnitude < speed * Time.deltaTime)
                {
                    transform.position = target.transform.position;
                }
                else
                {
                    dir.Normalize();
                    transform.position += dir * speed * Time.deltaTime;
                }
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Tulip")
        {
            var tulip = collision.GetComponent<Tulip>();
            GameManager.instance.GetTulip(tulip);
        }
    }
}

public enum SoujikiState
{
    Stop,
    Moving
}
