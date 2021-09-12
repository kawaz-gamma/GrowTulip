using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UpgradeButton
{
    public string name;
    public Button button;
    public TMP_Text textMesh;
    public bool kownFlag;
    public int price;
    public float priceMag;
}
