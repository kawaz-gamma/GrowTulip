using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class KoitanFPSDisplayer : MonoBehaviour
{
    float fpsTime = 0;
    int frameSum = 0;
    int perFrameCount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fpsTime += Time.unscaledDeltaTime;
        frameSum++;
        if (fpsTime > 1f)
        {
            fpsTime -= 1f;
            perFrameCount = frameSum;
            frameSum = 0;
        }
        KoitanDebug.Display($"FPS = {perFrameCount}\n");
    }
}
