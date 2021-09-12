using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweetButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tweet()
    {
        //本文＋ハッシュタグ＊２ツイート（画像なし）
        naichilab.UnityRoomTweet.Tweet("YOUR-GAMEID", $"「チューリップ育て」で{GameManager.totalKyuukonCount}個の球根を獲得した！", "unityroom", "unity1week");
    }
}
