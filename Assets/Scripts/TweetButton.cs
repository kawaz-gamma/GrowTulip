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
        //�{���{�n�b�V���^�O���Q�c�C�[�g�i�摜�Ȃ��j
        naichilab.UnityRoomTweet.Tweet("YOUR-GAMEID", $"�u�`���[���b�v��āv��{GameManager.totalKyuukonCount}�̋������l�������I", "unityroom", "unity1week");
    }
}
