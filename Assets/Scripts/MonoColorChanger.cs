using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoColorChanger : MonoBehaviour
{
    private UnityEngine.UI.Image image;

    private void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void ToBlackColor()
    {
        image.color = Color.black;
    }
    public void ToWhiteColor()
    {
        image.color = Color.white;
    }
}
