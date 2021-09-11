using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public static float speed = 1f;
    public static float interval = 2f;
    public static float speedMag = 1.5f;
    Vector3 dir = Vector3.right;
    float loopTime = 0;
    bool loopFlag = false;
    float width = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        dir = Quaternion.Euler(0, 0, Random.Range(10f, 80f)) * dir;
    }

    // Update is called once per frame
    void Update()
    {
        loopTime += Time.deltaTime;
        if (loopTime >= interval)
        {
            if (GameManager.instance.PlantKyuukon(transform.position))
            {
                loopFlag = false;
                loopTime = 0f;
                //­‚µ‰ñ“]‚³‚¹‚é
                //dir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * dir;
            }
        }
        transform.position += dir * speed * Time.deltaTime;
        /*
        float landWidth = Camera.main.orthographicSize * GameManager.landBaseWidth;
        float landHeight = Camera.main.orthographicSize * GameManager.landBaseHeight;
        */
        //if(transform.position.x)
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            // À•W‚©‚ç•Ç‚Ì”»’è
            if (collision.transform.position.x > 0)
            {
                dir.x = -Mathf.Abs(dir.x);
            }
            else if (collision.transform.position.x < 0)
            {
                dir.x = Mathf.Abs(dir.x);
            }
            else if (collision.transform.position.y > 0)
            {
                dir.y = -Mathf.Abs(dir.y);
            }
            else if (collision.transform.position.y < 0)
            {
                dir.y = Mathf.Abs(dir.y);
            }
            //­‚µ‰ñ“]‚³‚¹‚é
            //dir = Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) * dir;
        }
    }

}
