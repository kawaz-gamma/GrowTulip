using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tulip : MonoBehaviour
{
    public TulipState state = TulipState.Kyuukon;
    [SerializeField]
    GameObject kyuukonSprite;
    [SerializeField]
    GameObject tulipFlowerObject;
    [SerializeField]
    GameObject tulipFlowerSprite;
    [SerializeField]
    Collider2D collider;
    [SerializeField]
    Animator animator;

    float lifeTime = 0;
    public const float INIT_TULIP_TIME = 5f;
    public static float tulipTime = INIT_TULIP_TIME;
    public const float TULIP_TIME_MAG = 1.5f;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        state = TulipState.Kyuukon;
        //collider.enabled = false;
        kyuukonSprite.SetActive(true);

        //animator = GetComponent<Animator>();

        tulipFlowerObject.SetActive(false);

        // 花の色を変更
        tulipFlowerSprite.GetComponent<SpriteRenderer>().material.SetFloat("_Hue", Random.Range(0, 360));

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case TulipState.Kyuukon:
                lifeTime += Time.deltaTime;
                animator.SetFloat("TimeLeft", tulipTime - lifeTime);

                if (lifeTime > tulipTime)
                {
                    state = TulipState.Tulip;
                    collider.enabled = true;
                    kyuukonSprite.SetActive(false);
                    tulipFlowerObject.SetActive(true);
                    // チューリップリストに追加
                    GameManager.instance.tulipList.Add(this);
                    GameManager.instance.PlayTulipSe();
                }
                break;
        }
    }
}

public enum TulipState
{
    Kyuukon,
    Tulip
}
