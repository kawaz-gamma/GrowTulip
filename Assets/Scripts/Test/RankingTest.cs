using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingTest : MonoBehaviour
{
    enum RankingType
    {
        TotalKyuukon,
        KyuukonRate,
    }

    [SerializeField]
    private RankingType type;
    [SerializeField]
    private int score = 100;

    // Start is called before the first frame update
    void Start()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score, (int)type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
