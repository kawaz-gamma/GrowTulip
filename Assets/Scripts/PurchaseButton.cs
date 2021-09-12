using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    private UnityEngine.UI.Button button;
    private TMPro.RubyTextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        text = transform.GetChild(0).GetComponent<TMPro.RubyTextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var color = Color.white;
        if (!button.interactable)
        {
            color = Color.grey;
        }

        text.color = color;
    }
}
